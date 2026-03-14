// ************************************************************
// Core implementation of the SiriusAudio DLL
// SiriusAudio.c
// Written: February 2026
// Programmer: Aaron Scott with help from Microsoft Copilot
// Copyright 2026 Sirius Software All Rights Reserved
// ************************************************************


// ************************************************************
//  Miniaudio implementation
// ************************************************************
#define MINIAUDIO_IMPLEMENTATION
#include "miniaudio.h"
#include <windows.h>
#include <oleauto.h>
#include "SiriusEvents.h"

// ************************************************************
//  DLL export handling
// ************************************************************
#ifdef __cplusplus
extern "C" {
#endif

#ifdef _WIN32
#define DLL_EXPORT __declspec(dllexport)
#else
#define DLL_EXPORT
#endif


	// Process priority classes for SetPriorityClass()
#define IDLE_PRIORITY_CLASS           0x00000040
#define BELOW_NORMAL_PRIORITY_CLASS   0x00004000
#define NORMAL_PRIORITY_CLASS         0x00000020
#define ABOVE_NORMAL_PRIORITY_CLASS   0x00008000
#define HIGH_PRIORITY_CLASS           0x00000080
#define REALTIME_PRIORITY_CLASS       0x00000100

	// The structure which the callback will pass back with the
	// song's index and file name for the SongChanged event.

	struct SongInfo {
		int index;
		BSTR filename;
	};

	// Global buffer pointer and index to song,
	// a structure to pass song information, the name
	// of the current file being played and other
	// variables that must maintain static values.
	
	static wchar_t* plptr;
	static wchar_t* currentptr;
	static int plidx;
	static size_t pllen;
	static int count = 0;
	static struct SongInfo info;
	static wchar_t* currentfilename = NULL;
	static int autostart = 1; // 0 = false; 1 = true
	static int repeat = 1; // 0 = false; 1 = true
	static int playstate = SEP_Undefined;

	// ************************************************************
	//  Global audio device 
	// ************************************************************

	static ma_engine g_engine;
	static int g_engineInitialized = 0;
	static ma_sound g_sound;
	static int g_soundInitialized = 0;

	// Callback function declaration to receive event information
	// from the audio device.

	//typedef void (*ma_sound_end_proc)(ma_sound* pSound, void* pUserData);
	void MediaEndCallback(ma_sound* pSound, void* pUserData);

	// ************************************************************
	// Function prototypes for imported and exported functions.
	// ************************************************************


	DLL_EXPORT void PlayPause(void);
	DLL_EXPORT int GetAutostart(void);
	DLL_EXPORT void SetAutostart(int i);
	DLL_EXPORT int GetRepeat(void);
	DLL_EXPORT void SetRepeat(int i);
	DLL_EXPORT int GetPlaystate(void);
	DLL_EXPORT int DequeueEvent(int* code, void** payload);


	// ************************************************************
	// Function prototypes for internal functions.
	// ************************************************************

	void InitEventSystem(void);
	void TerminateEventSystem(void);
	void QueueEvent(int code, void* payload);
	ma_result LoadAndPlaySong(const wchar_t* songpath);
	wchar_t* GetPreviousSongName(void);
	wchar_t* GetNextSongName(void);
	int IsWMA(const char* path);

	// Declarations for the event queue.

	#define MAX_EVENTS 128

	typedef struct PendingEvent {
		int   code;
		void* payload;
	} PendingEvent;

	static PendingEvent    g_eventQueue[MAX_EVENTS];
	static int             g_eventHead = 0;
	static int             g_eventTail = 0;
	static CRITICAL_SECTION g_eventLock;

	// ************************************************************
	//  Initialization
	// ************************************************************
	DLL_EXPORT int InitializeAudio(void)
	{
		if (g_engineInitialized)
			return 1;

		ma_engine_config config = ma_engine_config_init();

		ma_result result = ma_engine_init(&config, &g_engine);
		if (result != MA_SUCCESS) {
			g_engineInitialized = 0;
			return 0;
		}

		InitEventSystem();
		playstate = SEP_Ready;
		QueueEvent(SEP_PlayStateChanged, NULL);
		g_engineInitialized = 1;
		return 1;
	}

	// ************************************************************
	//  Shutdown
	// ************************************************************
	DLL_EXPORT void ShutdownAudio(void)
	{
		if (!g_engineInitialized)
			return; // Nothing to do

		TerminateEventSystem();
		ma_engine_uninit(&g_engine);
		g_engineInitialized = 0;

		return;
	}
	// ************************************************************
	// Function remove one event's information from the queue. This
	// is done in the wrapper program's timer_tick event handler.
	// ************************************************************
	DLL_EXPORT int DequeueEvent(int* code, void** payload)
	{
		int hasEvent = 0;

		EnterCriticalSection(&g_eventLock);

		if (g_eventHead != g_eventTail) {
			*code = g_eventQueue[g_eventHead].code;
			*payload = g_eventQueue[g_eventHead].payload;

			g_eventHead = (g_eventHead + 1) % MAX_EVENTS;
			hasEvent = 1;
		}

		LeaveCriticalSection(&g_eventLock);

		return hasEvent;
	}
	// ************************************************************
	//  Function to determine if there are any events in the queue.
	// ************************************************************
	DLL_EXPORT int HasPendingEvents(void)
	{
		int result;

		EnterCriticalSection(&g_eventLock);
		result = (g_eventHead != g_eventTail);
		LeaveCriticalSection(&g_eventLock);

		return result;
	}
	// ************************************************************
	// GetAutostart function.
	// ************************************************************
	DLL_EXPORT int GetAutostart(void)
	{
		return autostart;
	}
	// ************************************************************
	// SetAutostart function
	// ************************************************************
	DLL_EXPORT void SetAutostart(int i)
	{
		autostart = i;
	}
	// ************************************************************
	// GetRepeat function.
	// ************************************************************
	DLL_EXPORT int GetRepeat(void)
	{
		return repeat;
	}
	// ************************************************************
	// SetRepeat function
	// ************************************************************
	DLL_EXPORT void SetRepeat(int i)
	{
		repeat = i;
	}
	// ************************************************************
	// Playstate function
	// ************************************************************
	DLL_EXPORT int GetPlaystate(void)
	{
		return playstate;

	}
	// ************************************************************
	//  LoadPlaylist
	//  Receives a CRLF-separated UTF-16 playlist buffer from VB,
	//  and saves it in a local buffer.
	// ************************************************************
	DLL_EXPORT void LoadPlaylist(const wchar_t* text)
	{
		if (!text)
			return;

		// Free the former buffer, if any.

		if (plptr) {
			free(plptr);
			plptr = NULL;
		}

		// Stop any music playing.
		ma_sound_stop(&g_sound);

		// Signal the the playlist is loading.

		QueueEvent(SEP_PlaylistLoading, NULL);

		// Calculate required memory size to store the playlist and allocate memory.	

		size_t len = wcslen(text) + 1;        // number of wchar_t units INCLUDING null
		plptr = malloc(len * sizeof(wchar_t));  // number of bytes

		// Ensure we received a valid butter.

		if (!plptr) {
			// Allocation failed — reset state
			pllen = 0;
			plidx = -1;
			return;
		}

		// Copy the playlist into our local buffer and initialize variables.

		wcscpy_s(plptr, len, text);
		pllen = len;
		plidx = -1;
		currentptr = plptr;

		// Initialize our SongInfo structure.

		info.index = plidx;
		if (info.filename)
			SysFreeString(info.filename);
		info.filename = SysAllocStringLen(L"", 0);   // empty BSTR

		// Signal that the playlist has loaded.

		QueueEvent(SEP_PlaylistLoaded, NULL);

		// If autostart is set, play automatically.

		if (autostart) {
			playstate = SEP_Ready; // This will cause play routine to fetch first song from playlist.
			PlayPause();
		}

	}

	// ************************************************************
	// GetPlaylist
	// Creates a buffer and copies our playlist memory block
	// into it and returns it as BSTR, which Visual Basic can
	// capture.
	// ************************************************************
	DLL_EXPORT BSTR GetPlaylist(void)
	{
		if (!plptr || pllen == 0)
			return SysAllocStringLen(L"", 0);

		size_t chars = wcslen(plptr);  // number of characters, excluding null

		BSTR b = SysAllocStringLen(NULL, (UINT)chars);
		if (!b) return NULL;

		memcpy(b, plptr, chars * sizeof(wchar_t));
		b[chars] = L'\0';

		return b;
	}
	// ************************************************************
	// Function to return the count of songs in the playlist.
	// ************************************************************
	DLL_EXPORT int PlaylistItemCount(void)
	{

		// No playlist loaded
		if (!plptr || pllen == 0)
			return 0;

		count = 0;
		wchar_t* p = plptr;

		while (*p != L'\0')
		{
			// We are at the start of a line → count it
			count++;

			// Advance to end of this line
			while (*p != L'\r' && *p != L'\n' && *p != L'\0')
				p++;

			// Skip CRLF or LF
			if (*p == L'\r') {
				p++;
				if (*p == L'\n')
					p++;
			}
			else if (*p == L'\n') {
				p++;
			}
		}

		return count;

	}
	// ************************************************************
	// Function to set the engine volume
	// ************************************************************
	DLL_EXPORT void SetVolume(float vol)
	{
		ma_engine_set_volume(&g_engine, vol);

	}
	// ************************************************************
	// Function to retrieve the engine volume.
	// ************************************************************
	DLL_EXPORT float GetVolume(void)
	{
		return  ma_engine_get_volume(&g_engine);
	}
	// ************************************************************
	// Function to return the index and filename of the current song.
	// ************************************************************
	DLL_EXPORT struct SongInfo* GetCurrentSong(void)
	{

		if (info.index == -1) {
			if (info.filename)
				SysFreeString(info.filename);

			info.filename = SysAllocStringLen(L"", 0);  // empty string
		}

		return &info;

	}
	// ************************************************************
	// Function to play the song at a current index into the
	// playlist.
	// ************************************************************
	DLL_EXPORT void PlaySongAtIndex(int idx)
	{

		int count = PlaylistItemCount();
		if (idx < 0 || idx >= count) {
			QueueEvent(SEP_Error, L"Invalid Index");
			return;
		}

		// Start at beginning of playlist text
		wchar_t* p = plptr;
		int current = 0;

		// Walk to the idx-th line
		while (current < idx) {
			while (*p && *p != L'\n')
				p++;
			if (*p == L'\n')
				p++;
			current++;
		}

		// p now points to start of desired line
		wchar_t* start = p;

		// Find end of line
		while (*p && *p != L'\n')
			p++;

		// Set size of filename.
		size_t len = p - start;

		// Trim trailing '\r' because playlist always uses CRLF
		if (len > 0 && start[len - 1] == L'\r')
			len--;

		// Allocate separate buffer for currentfilename
		wchar_t* cf = malloc((len + 1) * sizeof(wchar_t));
		if (!cf) {
			QueueEvent(SEP_Error, L"Out of memory allocating filename");
			return;
		}
		// Copy the filename from the playlist and add a termination "\0".
		wmemcpy(cf, start, len);
		cf[len] = L'\0';

		// Free any current filename.

		if (currentfilename)
			free(currentfilename);

		// Save the selected filename.

		currentfilename = cf;

		// Update info for VB: BSTR + index
		if (info.filename)
			SysFreeString(info.filename);

		info.filename = SysAllocStringLen(start, (UINT)len);
		plidx = idx;
		info.index = plidx;

		// Start the song playing.
		LoadAndPlaySong(currentfilename);

	}
	// ************************************************************
	//  Play function, which toggles the play state.
	// ************************************************************
	DLL_EXPORT void PlayPause(void)
	{

		// What we do when PlayPause is called depends on the current playstate.

		switch (playstate) {

			// If the playstate is "ready", meaning the engine has been
			// initialized, but nothing playing yet, fetch the first song.
		case SEP_Ready:
			currentfilename = GetNextSongName();

			// GetNextSongName will return NULL if the playlist has reached the end and
			// Repeat is not turned on.  In this case, stop the sound, queue the event and
			// just exit.

			if (!currentfilename) {
				ma_sound_stop(&g_sound);
				QueueEvent(SEP_MediaEnded, NULL);
				break;
			}

			LoadAndPlaySong(currentfilename);
			playstate = SEP_Playing;
			break;

			// If the playstate is playing, pause the music.
		case SEP_Playing:
			ma_sound_stop(&g_sound);
			playstate = SEP_Paused;
			break;

			// If the music has been stopped or paused, resume playing.
		case SEP_Paused:
		case SEP_Stopped:
			ma_sound_start(&g_sound);
			playstate = SEP_Playing;
			break;

		default:
			break;
		}

		// Queue the event.

		QueueEvent(SEP_PlayStateChanged, (void*)(intptr_t)playstate);

	}
	// ************************************************************
	//  PlayNext function.
	// ************************************************************
	DLL_EXPORT void PlayNext(void)
	{

		// Raise the event.

		playstate = SEP_ScanForward;
		QueueEvent(SEP_PlayStateChanged, (void*)(intptr_t)playstate);

		// Get the next song.
		if (currentfilename) {
			free(currentfilename);
			currentfilename = NULL;
		}
		currentfilename = GetNextSongName();

		// Make sure we got a next name.  If repeat is turned off,
		// it could be NULL.

		if (currentfilename) {
			// Stop any song currently playing
			ma_sound_stop(&g_sound);

			// Pass the name to the LoadAndPlaySong function
			LoadAndPlaySong(currentfilename);
		}
	}
	// ************************************************************
	//  PlayPrevious function.
	// ************************************************************
	DLL_EXPORT void PlayPrevious(void)
	{

		// Raise the event. 
		playstate = SEP_ScanReverse;
		QueueEvent(SEP_PlayStateChanged, (void*)(intptr_t)playstate);

		// Get the name of the previous song.
		if (currentfilename) {
			free(currentfilename);
			currentfilename = NULL;
		}
		currentfilename = GetPreviousSongName();

		// Stop any song currently playing
		ma_sound_stop(&g_sound);

		// Pass the song name to the LoadAndPlay function.

		LoadAndPlaySong(currentfilename);

	}
	// ************************************************************
	//  PlayStop function.
	// ************************************************************
	DLL_EXPORT void PlayStop(void)
	{
		ma_sound_stop(&g_sound);
		ma_sound_seek_to_pcm_frame(&g_sound, 0);

		playstate = SEP_Stopped;
		QueueEvent(SEP_PlayStateChanged, (void*)(intptr_t)playstate);

	}
	// ************************************************************
	// Function to take a song name, feed it to the engine and
	// set the callback function for it.
	// ************************************************************

	ma_result LoadAndPlaySong(const wchar_t* songpath)
	{
		ma_result result;

		// Convert UTF-16 filename to UTF-8 for miniaudio.
		char utf8path[1024];
		size_t converted = 0;
		wcstombs_s(&converted, utf8path, sizeof(utf8path), songpath, _TRUNCATE);

		// NOTE: LoadAndPlaySong takes ownership of currentfilename and frees it.
		if (currentfilename) {
			free(currentfilename);
			currentfilename = NULL;
		}
		// Set the playstate

		playstate = SEP_Playing;
		QueueEvent(SEP_PlayStateChanged, (void*)(intptr_t)playstate);

		// Send SongChanged event.
		QueueEvent(SEP_SongChanged, &info);

		// Check if the file is a .wma file, which cannot be played here.
		if (IsWMA(utf8path)) {
			QueueEvent(SEP_UnreadableByMA, &info);
			return 0;
		}

		// If a sound has been initialized, unitialize it first.
		if (g_soundInitialized) {
			ma_sound_uninit(&g_sound);
		}
		g_soundInitialized = 1;

		// Initialize a sound from the music file, using the utf8 path.
		result = ma_sound_init_from_file(&g_engine, utf8path, 0, NULL, NULL, &g_sound);

		// If we can't play the file, raise an event.  The wrapper will
		// switch to the WMPLIB engine and try to play it.
		if (result != MA_SUCCESS) {
			QueueEvent(SEP_UnreadableByMA, &info);
			return result;
		}

		// Set the callback function address, which will receive
		// the signal when a song has ended.

		ma_sound_set_end_callback(&g_sound, MediaEndCallback, NULL);

		// Start the sound playing.
		ma_sound_start(&g_sound);

		return MA_SUCCESS;
	}
	// ************************************************************
	// Function to initialize the event queue.
	// ************************************************************
	void InitEventSystem(void)
	{
		InitializeCriticalSection(&g_eventLock);
		g_eventHead = g_eventTail = 0;
	}

	// ************************************************************
	// Function to terminate the event queue.
	// ************************************************************
	void TerminateEventSystem(void)
	{
		DeleteCriticalSection(&g_eventLock);
	}
	// ************************************************************
	// Function add event information (event code, optional data)
	// to the event queue.
	// ************************************************************

	void QueueEvent(int code, void* payload)
	{
		EnterCriticalSection(&g_eventLock);

		int nextTail = (g_eventTail + 1) % MAX_EVENTS;

		// Drop event if queue is full.
		if (nextTail != g_eventHead) {
			g_eventQueue[g_eventTail].code = code;
			g_eventQueue[g_eventTail].payload = payload;
			g_eventTail = nextTail;
		}

		LeaveCriticalSection(&g_eventLock);

	}
	// ************************************************************
	// Function to return the next song in the playlist, and to 
	// update the info structure, so it's always current.
	// ************************************************************
	wchar_t* GetNextSongName(void)
	{
		// No playlist loaded
		if (!plptr || pllen == 0)
			return SysAllocStringLen(L"", 0);

		int count = PlaylistItemCount();
		if (count == 0)
			return SysAllocStringLen(L"", 0);

		// If repeat is turned on, and we've just played the last song,
		// and repeat is on, then restart, by resetting the index.
		
		if (plidx == count - 1 && repeat != 0 )
			plidx = -1;

		else if (plidx == count - 1)
			return NULL;

		// Initialize plidx if needed
		// -1 means "no current song yet" → first Next should go to 0
		if (plidx < -1 || plidx >= count)
			plidx = -1;

		// Compute new index
		if (plidx == -1)
		{
			// First call after load: go to first song
			plidx = 0;
		}
		else
		{
			// Normal increment with wrap
			plidx++;
			if (plidx >= count)
				plidx = 0;
		}

		// Walk forward from plptr to the start of plidx
		wchar_t* p = plptr;

		for (int i = 0; i < plidx; i++)
		{
			// Skip to end of line
			while (*p != L'\r' && *p != L'\n' && *p != L'\0')
				p++;

			// Skip CRLF or LF
			if (*p == L'\r') {
				p++;
				if (*p == L'\n')
					p++;
			}
			else if (*p == L'\n') {
				p++;
			}
		}

		wchar_t* start = p;

		// Find end of this line
		while (*p != L'\r' && *p != L'\n' && *p != L'\0')
			p++;

		size_t len = (size_t)(p - start);

		// Allocate BSTR for VB
		BSTR result = SysAllocStringLen(start, (UINT)len);

		// Update currentptr to the start of this line
		currentptr = start;

		// Update SongInfo (free old BSTR first)
		if (info.filename)
			SysFreeString(info.filename);

		info.filename = result;
		info.index = plidx;

		// Allocate native UTF-16 filename for miniaudio
		if (currentfilename) {
			free(currentfilename);
			currentfilename = NULL;
		}

		currentfilename = malloc((len + 1) * sizeof(wchar_t));
		if (!currentfilename)
			return NULL;

		wcsncpy_s(currentfilename, len + 1, start, len);
		currentfilename[len] = L'\0';

		return currentfilename;
	}
	// ************************************************************
	// Function to return the name of the previous song in the
	// playlist and update the SongInfo structure so it's always current.
	// ************************************************************
	wchar_t* GetPreviousSongName(void)
	{
		// No playlist loaded
		if (!plptr || pllen == 0)
			return SysAllocStringLen(L"", 0);

		int count = PlaylistItemCount();
		if (count == 0)
			return SysAllocStringLen(L"", 0);

		// Initialize plidx if needed
		if (plidx < 0 || plidx >= count)
			plidx = 0;

		// Compute new index: wrap or decrement
		if (plidx == 0)
			plidx = count - 1;   // wrap to last
		else
			plidx--;             // normal previous

		// Walk forward from plptr to the start of plidx
		wchar_t* p = plptr;

		for (int i = 0; i < plidx; i++)
		{
			// Skip to end of line
			while (*p != L'\r' && *p != L'\n' && *p != L'\0')
				p++;

			// Skip CRLF or LF
			if (*p == L'\r') {
				p++;
				if (*p == L'\n')
					p++;
			}
			else if (*p == L'\n') {
				p++;
			}
		}

		wchar_t* start = p;

		// Find end of this line
		while (*p != L'\r' && *p != L'\n' && *p != L'\0')
			p++;

		size_t len = (size_t)(p - start);

		// Allocate BSTR for VB
		BSTR result = SysAllocStringLen(start, (UINT)len);

		// Update currentptr to the start of this line
		currentptr = start;

		// Update SongInfo (free old BSTR first)
		if (info.filename)
			SysFreeString(info.filename);

		info.filename = result;
		info.index = plidx;

		// Allocate native UTF-16 filename for miniaudio
		if (currentfilename) {
			free(currentfilename);
			currentfilename = NULL;
		}

		currentfilename = malloc((len + 1) * sizeof(wchar_t));
		if (!currentfilename)
			return NULL;

		wcsncpy_s(currentfilename, len + 1, start, len);
		currentfilename[len] = L'\0';

		return currentfilename;
	}
	// ************************************************************
	// Function to test for .wma files, which miniaudio cannot play.
	// ************************************************************
	int IsWMA(const char* path)
	{
		if (path == NULL) return 0;

		const char* ext = strrchr(path, '.');
		if (ext == NULL) return 0;

		// Move past the dot
		ext++;

		// Case-insensitive compare
		if (_stricmp(ext, "wma") == 0)
			return 1;

		return 0;
	}
	// ************************************************************
	// Callback function for the media ended event.
	// ************************************************************
	void MediaEndCallback(ma_sound* pSound, void* pUserData)
	{

		// Determine if we've reached the end of a playlist, which
		// only happens if repeat is 0, as otherwise, it just
		// starts over at the beginning.

		if (!repeat && plidx == PlaylistItemCount() - 1) {
			playstate = SEP_PlaylistEnded;
			QueueEvent(SEP_PlayStateChanged, (void*)(intptr_t)playstate);
			plidx = -1; // reset to start of playlist.
			playstate = SEP_Ready;
		}
		else {
			playstate = SEP_MediaEnded;
			QueueEvent(SEP_PlayStateChanged, (void*)(intptr_t)playstate);
		}
	

	}

	// ************************************************************
	// The Main routine.
	// ************************************************************
	BOOL APIENTRY DllMain(HMODULE hModule,
		DWORD  ul_reason_for_call,
		LPVOID lpReserved)
	{
		switch (ul_reason_for_call)
		{
		case DLL_PROCESS_ATTACH:
			InitializeCriticalSection(&g_eventLock);
			SetPriorityClass(GetCurrentProcess(), ABOVE_NORMAL_PRIORITY_CLASS); //l Make sure audio processing gets enough time
			g_eventHead = 0;
			g_eventTail = 0;

				
		case DLL_PROCESS_DETACH:
			if (plptr) {
				free(plptr);
				plptr = NULL;
			}
			pllen = 0;
			plidx = -1;
			if (currentfilename)
				free(currentfilename);
				break;
		}
		return TRUE;
	}
#ifdef __cplusplus
}
#endif