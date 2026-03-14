Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Xml
Imports Microsoft.VisualBasic.Devices
Imports WMPLib

Public Class SiriusAudio
	Implements IDisposable
	'****************************************************************
	' Wrapper class for SiriusAudio.dll
	' SIRIUSAUDIO.VB
	' Written: February 2026
	' Programmer: Aaron Scott with Microsoft Copilot
	' Copyright 2026 Sirius Software All Rights Reserved
	'****************************************************************

	' Declare the functions in the SiriusAudio.dll we will be calling.
	<DllImport("SiriusAudio.dll", CharSet:=CharSet.Unicode)>
	Private Shared Sub InitializeAudio()
	End Sub
	<DllImport("SiriusAudio.dll", CharSet:=CharSet.Unicode)>
	Private Shared Sub ShutdownAudio()
	End Sub
	<DllImport("SiriusAudio.dll", CharSet:=CharSet.Unicode)>
	Private Shared Function GetPlaylist() As <MarshalAs(UnmanagedType.BStr)> String
	End Function
	<DllImport("SiriusAudio.dll", CharSet:=CharSet.Unicode)>
	Private Shared Sub LoadPlaylist(p As String)
	End Sub
	Public Delegate Sub SiriusCallback(ev As SiriusEvent, payload As IntPtr)
	<DllImport("SiriusAudio.dll", CallingConvention:=CallingConvention.Cdecl)>
	Private Shared Function GetCurrentSong() As IntPtr
	End Function
	<DllImport("SiriusAudio.dll", CallingConvention:=CallingConvention.Cdecl)>
	Private Shared Sub PlayPause()
	End Sub
	<DllImport("SiriusAudio.dll", CallingConvention:=CallingConvention.Cdecl)>
	Private Shared Sub PlayNext()
	End Sub
	<DllImport("SiriusAudio.dll", CallingConvention:=CallingConvention.Cdecl)>
	Private Shared Sub PlayPrevious()
	End Sub
	<DllImport("SiriusAudio.dll", CallingConvention:=CallingConvention.Cdecl)>
	Private Shared Sub PlayStop()
	End Sub
	<DllImport("SiriusAudio.dll", CallingConvention:=CallingConvention.Cdecl)>
	Private Shared Sub PlaySongAtIndex(i As Integer)
	End Sub
	<DllImport("SiriusAudio.dll", CallingConvention:=CallingConvention.Cdecl)>
	Private Shared Function GetAutostart() As Integer
	End Function
	<DllImport("SiriusAudio.dll", CallingConvention:=CallingConvention.Cdecl)>
	Private Shared Sub SetAutostart(i As Integer)
	End Sub
	<DllImport("SiriusAudio.dll", CallingConvention:=CallingConvention.Cdecl)>
	Private Shared Function GetRepeat() As Integer
	End Function
	<DllImport("SiriusAudio.dll", CallingConvention:=CallingConvention.Cdecl)>
	Private Shared Sub SetRepeat(i As Integer)
	End Sub
	<DllImport("SiriusAudio.dll", CallingConvention:=CallingConvention.Cdecl)>
	Private Shared Function GetPlaystate() As Integer
	End Function
	<DllImport("SiriusAudio.dll", CallingConvention:=CallingConvention.Cdecl)>
	Private Shared Function PlaylistItemCount() As Integer
	End Function
	<DllImport("SiriusAudio.dll", CallingConvention:=CallingConvention.Cdecl)>
	Private Shared Function HasPendingEvents() As Integer
	End Function
	<DllImport("SiriusAudio.dll", CallingConvention:=CallingConvention.Cdecl)>
	Private Shared Function GetVolume() As Single
	End Function
	<DllImport("SiriusAudio.dll", CallingConvention:=CallingConvention.Cdecl)>
	Private Shared Sub SetVolume(v As Single)
	End Sub
	Private Declare Function DequeueEvent Lib "SiriusAudio.dll" (ByRef code As Integer, ByRef payload As IntPtr) As Integer  ' Enum for the types of events we will be raising upon notice from the .dll

	Public Enum SiriusEvent
		None
		PlaylistLoading
		PlaylistLoaded
		SongChanged
		PlayStateChanged
		UnreadableByMA
		_Error
		_Count  ' always last; Not an Event
	End Enum

	' Enum for the various playstates exposed by the .dll.
	Public Enum SEP_Playstate
		SEP_Undefined
		SEP_Stopped
		SEP_Paused
		SEP_Playing
		SEP_PlayingExternal
		SEP_ScanForward
		SEP_ScanReverse
		SEP_MediaEnded
		SEP_PlaylistEnded
		SEP_Ready
	End Enum

	' Declare the events to be raised by this class.

	Public Event PlaylistLoading()
	Public Event PlaylistLoaded()
	Public Event SongChanged(idx As Integer, Filename As String)
	Public Event PlayStateChanged(ps As Integer)
	Public Event MediaError(Msg As String)
	Public Event MediaUnplayable(idx As Integer)

	' Declare private variables.

	Private mPlaylist As String
	Private currentindex As Integer

	' Declare a structure to allow passing SongChanged information to the callback.

	<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
	Public Structure SongInfo
		Public index As Integer
		<MarshalAs(UnmanagedType.BStr)>
		Public filename As String
	End Structure


	' Create a timer that will dequeue event information from the DLL

	Private WithEvents _relayTimer As New Timer() With {.Interval = 100, .Enabled = True}

	Private wmPlayer As New WMPLib.WindowsMediaPlayer

	'****************************************************************

	' The class constructor and destructor.

	'****************************************************************
	Public Sub New()
		InitializeAudio()
		AddHandler wmPlayer.Error, AddressOf wmp_Error
		AddHandler wmPlayer.PlayStateChange, AddressOf wmpPlayStateChanged
	End Sub
	Public Sub Dispose() Implements IDisposable.Dispose
		ShutdownAudio()
		GC.SuppressFinalize(Me)
		RemoveHandler wmPlayer.Error, AddressOf wmp_Error
		RemoveHandler wmPlayer.PlayStateChange, AddressOf wmpPlayStateChanged
	End Sub

	Protected Overrides Sub Finalize()
		Dispose()
	End Sub
	'****************************************************************
	' The Play method.
	'****************************************************************
	Friend Sub Play()

		' Determine how to handle play/pause based on the audio engine
		' currently active.

		' wmPlayer is used for files miniaudio can't play.

		If wmPlayer.playState = SEP_Playstate.SEP_Playing Then
			wmPlayer.controls.pause()
			Exit Sub
		ElseIf wmPlayer.playState = SEP_Playstate.SEP_Paused Then
			wmPlayer.controls.play()
			Exit Sub
		End If

		' All other file types, played by SiriusAudio

		PlayPause()

	End Sub

	'****************************************************************
	' The Play Previous method
	'****************************************************************
	Public Sub PreviousSong()

		' Stop both engines before moving to the previous song.

		wmPlayer.controls.stop()
		PlayStop()
		PlayPrevious()
	End Sub
	'****************************************************************
	' The Play Next method
	'****************************************************************
	Public Sub NextSong()

		' Stop both engines before moving to the next song.

		wmPlayer.controls.stop()
		PlayStop()
		PlayNext()
	End Sub
	'****************************************************************
	' The PlaySong method
	'****************************************************************
	Public Sub PlaySong(idx As Integer)

		' Stop both engines before playing the selected song.

		wmPlayer.controls.stop()
		PlayStop()
		PlaySongAtIndex(idx)
	End Sub
	'****************************************************************

	' The playstop sub.

	'****************************************************************
	Public Sub StopAll()
		wmPlayer.controls.stop()
		PlayStop()
	End Sub
	'****************************************************************
	' Sub to play .wma or any other files miniaudio cannot play here,
	' in the wrapper using WMPLIB.  If it cannot play them, an
	' error is reported to the user.
	'****************************************************************
	Private Sub PlayByWMP(idx As Integer, filename As String)

		Dim media As WMPLib.IWMPMedia
		wmPlayer.controls.stop()
		PlayStop()
		media = wmPlayer.newMedia(filename)
		wmPlayer.currentMedia = media
		wmPlayer.controls.play()
		RaiseEvent PlayStateChanged(SEP_Playstate.SEP_PlayingExternal)

		' Remember the index of the current song.  In case
		' WMP cannot play it, we'll return that index with
		' the "Unplayable" event.

		currentindex = idx
	End Sub
	'**********************************************************

	' An error has occurred during playback.

	'**********************************************************
	Private Sub wmp_Error()
		If wmPlayer.Error.count > 0 Then
			RaiseEvent MediaUnplayable(currentindex)
		End If
	End Sub

	'****************************************************************
	' Sub to process error messages received.
	'****************************************************************
	Private Sub ProcessErrorMsg(Msg As String)
		RaiseEvent MediaError(Msg)
	End Sub
	'****************************************************************
	' Event handler for the WMPLIB engine's PlayStateChanged event.
	'****************************************************************
	Private Sub wmpPlayStateChanged(newState As Integer)

		' Event handler for the PlayStateChanged event.
		' We only handle a few of these.

		'wmppsUndefined = 0,
		'wmppsStopped = 1,
		'wmppsPaused = 2,
		'wmppsPlaying = 3,
		'wmppsScanForward = 4,
		'wmppsScanReverse = 5,
		'wmppsBuffering = 6,
		'wmppsMediaEnded = 8,
		'**********************************************************

		' Process the message.  We only handle media ended.

		' If the last song in a playlist was played by WMP, we'll get only a MediaEnded
		' event.  So we need to test to see if we are at the end of the playlist before
		' calling PlayNext.

		If newState = WMPPlayState.wmppsMediaEnded Then
			Dim c As Integer = PlaylistItemCount()
			Dim info As SongInfo = Marshal.PtrToStructure(Of SongInfo)(GetCurrentSong)
			If info.index < c - 1 Or Repeat Then
				PlayNext()
			Else
				RaiseEvent PlayStateChanged(SEP_Playstate.SEP_PlaylistEnded)
			End If

		End If

	End Sub

	'****************************************************************
	' The Songlist property.  This property accepts a prepared list
	' of song names separated by CrLfs.
	'****************************************************************
	Friend Property Songlist() As String
		Get
			Return Playlist ' This is the prepared list in any event.
		End Get
		Set(value As String)

			' Make sure the song list ends with a CrLf.

			If Not value.EndsWith(vbCrLf) Then
				RaiseEvent MediaError("Invalid Songlist Format")
				Exit Property
			End If

			' Pass it on to the engine.

			LoadPlaylist(value)

		End Set
	End Property

	'****************************************************************
	' The Playlist property.  This property accepts the name of
	' a Windows Playlist (.wpl) file, and returns a single string
	' of file names separated by CrLf
	'****************************************************************
	Friend Property Playlist As String
		Get

			Dim zx As String = GetPlaylist()
			Return zx

		End Get
		Set(value As String)

			' Declare variables.

			Dim plist As New System.Text.StringBuilder
			Dim songpath As String
			Dim MusicPath As String = GetSetting("SiriusSiriusEasyPlayer", "Settings", "MusicFolder", "") & "\"

			' Save the new playlist name.

			mPlaylist = value

			' Make sure the playlist exists.

			If Not System.IO.File.Exists(mPlaylist) Then
				RaiseEvent MediaError("Playlist Filename not found")
				Exit Property
			End If

			' Begin parsing the playlist, which is an XML document.

			Try
				Dim xmlDoc As New XmlDocument()
				xmlDoc.Load(mPlaylist)

				Dim mediaNodes As XmlNodeList = xmlDoc.SelectNodes("//smil/body/seq/media")
				For Each Node As XmlNode In mediaNodes
					songpath = Node.Attributes("src")?.Value.Replace("..\", MusicPath)
					plist.Append(UnescapeXml(songpath) & vbCrLf)
				Next Node

				' Pass the playlist to the .dll.

				LoadPlaylist(plist.ToString)

			Catch ex As Exception
				RaiseEvent MediaError($"Error parsing playlist: {ex.Message}")
			End Try

		End Set

	End Property
	'****************************************************************
	' The PlaylistItems property.  This returns an array of playlist file
	' names.
	'****************************************************************
	Public ReadOnly Property PlayListItems() As String()
		Get

			Dim zx As String = GetPlaylist()
			Dim playlist As String()
			playlist = zx.Split({vbCrLf}, StringSplitOptions.RemoveEmptyEntries)
			Return playlist
		End Get
	End Property
	'****************************************************************
	' The CurrentSong property.  This returns a structure of SongInfo
	' containing information as to the current song's index and 
	' filename.
	'****************************************************************
	Public ReadOnly Property CurrentSong() As SongInfo
		Get
			Dim ptr As IntPtr = GetCurrentSong()
			Return Marshal.PtrToStructure(Of SongInfo)(ptr)
		End Get
	End Property
	'****************************************************************
	' The Repeat property.  This returns and sets the flag that
	' determines if the audio engine replays a playlist after
	' reaching the end of the last song.
	'****************************************************************
	Public Property Repeat() As Boolean
		Get
			Return CBool(GetRepeat())
		End Get
		Set(value As Boolean)
			SetRepeat(Math.Abs(CInt(value)))
		End Set
	End Property
	'****************************************************************
	' The Autostart property.  This returns and sets the flag that
	' determines if the audio engine begins playing automatically
	' after a playlist is loaded.
	'****************************************************************
	Public Property Autostart() As Boolean
		Get
			Return CBool(GetAutostart())
		End Get
		Set(value As Boolean)
			SetAutostart(CInt(value))
		End Set
	End Property
	'****************************************************************
	' The Playstate property.
	'****************************************************************
	Public ReadOnly Property PlayState() As Integer
		Get
			Return GetPlaystate()
		End Get
	End Property
	Public Property Volume As Single
		Get
			Return GetVolume()
		End Get
		Set(value As Single)
			SetVolume(value)
		End Set
	End Property
	'****************************************************************
	' The event dispatcher.  Upon a tick event, which happens 10ms after the
	' callback has placed event information on the stack, and
	' enabled this timer, this dispatcher will retrieve the first
	' event info from the stack and trigger the event.
	'****************************************************************
	Private Sub _relayTimer_Tick(sender As Object, e As EventArgs) Handles _relayTimer.Tick

		Dim code As Integer
		Dim payload As IntPtr

		' See if there are any events pending in the DLL.
		If HasPendingEvents() = 0 Then Exit Sub

		' Drain all pending events from the DLL queue.
		Do While DequeueEvent(code, payload) <> 0


			' Check the event queue and raise the appropriate event.

			Select Case code
				Case SiriusEvent.PlaylistLoading
					RaiseEvent PlaylistLoading()

				Case SiriusEvent.PlaylistLoaded
					RaiseEvent PlaylistLoaded()

				Case SiriusEvent.SongChanged
					Dim info As SongInfo = Marshal.PtrToStructure(Of SongInfo)(payload)
					RaiseEvent SongChanged(info.index, info.filename)

				Case SiriusEvent.PlayStateChanged
					Dim ev As Integer = payload.ToInt32
					RaiseEvent PlayStateChanged(ev)
					If ev = SEP_Playstate.SEP_MediaEnded Then PlayNext()

				Case SiriusEvent.UnreadableByMA
					Dim info As SongInfo = Marshal.PtrToStructure(Of SongInfo)(payload)
					PlayByWMP(info.index, info.filename)

				Case SiriusEvent._Error
					ProcessErrorMsg(Marshal.PtrToStringUni(payload))

			End Select
		Loop
	End Sub
End Class
