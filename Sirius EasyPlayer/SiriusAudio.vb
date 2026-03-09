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
	Private Shared Function GetPlaystate() As Integer
	End Function
	<DllImport("SiriusAudio.dll", CallingConvention:=CallingConvention.Cdecl)>
	Private Shared Function HasPendingEvents() As Integer
	End Function
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

	Public Enum maPlayStates
		Undefined
		Stopped
		Paused
		Playing
		ScanForward
		ScanReverse
		MediaEnded
		PlayingExternal
		Ready
	End Enum

	' Declare the events to be raised by this class.

	Public Event PlaylistLoading()
	Public Event PlaylistLoaded()
	Public Event SongChanged(idx As Integer, Filename As String)
	Public Event PlayStateChanged(ps As Integer)
	Public Event MediaError(Msg As String)

	' Declare private variables.

	Private mPlaylist As String

	' Declare a structure to allow passing SongChanged information to the callback.

	<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
	Public Structure SongInfo
		Public index As Integer
		<MarshalAs(UnmanagedType.BStr)>
		Public filename As String
	End Structure


	' Create a timer that will dequeue event information from the DLL

	Private WithEvents _relayTimer As New Timer() With {.Interval = 100, .Enabled = True}

	Private mPlayer As New WMPLib.WindowsMediaPlayer

	'****************************************************************

	' The class constructor and destructor.

	'****************************************************************
	Public Sub New()
		InitializeAudio()
	End Sub
	Public Sub Dispose() Implements IDisposable.Dispose
		ShutdownAudio()
		GC.SuppressFinalize(Me)
	End Sub

	Protected Overrides Sub Finalize()
		Dispose()
	End Sub
	'****************************************************************
	' The Play method.
	'****************************************************************
	Public Sub Play()

		' Determine how to handle play/pause based on the audio engine
		' currently active.

		' WMP - used for files miniaudio can't play.

		If mPlayer.playState = maPlayStates.Playing Then
			mPlayer.controls.pause()
		Else
			mPlayer.controls.play()
		End If

		' All other file types, played by SiriusAudio

		PlayPause()

	End Sub

	'****************************************************************
	' The Play Previous method
	'****************************************************************
	Public Sub PreviousSong()

		' Stop both engines before moving to the previous song.

		mPlayer.controls.stop()
		PlayStop()
		PlayPrevious()
	End Sub
	'****************************************************************
	' The Play Next method
	'****************************************************************
	Public Sub NextSong()

		' Stop both engines before moving to the next song.

		mPlayer.controls.stop()
		PlayStop()
		PlayNext()
	End Sub
	'****************************************************************
	' The PlaySong method
	'****************************************************************
	Public Sub PlaySong(idx As Integer)

		' Stop both engines before playing the selected song.

		mPlayer.controls.stop()
		PlayStop()
		PlaySongAtIndex(idx)
	End Sub
	'****************************************************************

	' The playstop sub.

	'****************************************************************
	Public Sub StopAll()
		mPlayer.controls.stop()
		PlayStop()
	End Sub
	'****************************************************************
	' Sub to play .wma or any other files miniaudio cannot play here,
	' in the wrapper using WMPLIB.  If it cannot play them, an
	' error is reported to the user.
	'****************************************************************
	Private Sub PlayByWMP(idx As Integer, filename As String)

		Dim media As WMPLib.IWMPMedia
		mPlayer.controls.stop()
		PlayStop()
		media = mPlayer.newMedia(filename)
		mPlayer.currentMedia = media
		mPlayer.controls.play()

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

		' Send the event to the form containing this control.

		Select Case newState

			Case WMPPlayState.wmppsStopped

			Case WMPPlayState.wmppsPlaying

			Case WMPPlayState.wmppsMediaEnded
				PlayNext()
		End Select

		' Pass the event on to the next layer.

		RaiseEvent PlayStateChanged(newState)

	End Sub

	'****************************************************************
	' The Playlist property.  This property accepts the name of
	' a Windows Playlist (.wpl) file, and returns a single string
	' of file names separated by CrLf
	'****************************************************************

	Public Property Playlist As String
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
	' The Autostart property.  This returns and sets the flag that
	' determines if the audio engine begins playing automatically
	' after a playlist is loaded.
	'****************************************************************
	Public Property Autostart(state As Integer) As Integer
		Get
			Return GetAutostart()
		End Get
		Set(value As Integer)
			If value = 0 Or value = 1 Then
				SetAutostart(value)
			Else
				RaiseEvent MediaError("Autostart must be '0' or '1'.")
			End If
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
					If ev = maPlayStates.MediaEnded Then PlayNext()

				Case SiriusEvent.UnreadableByMA
					Dim info As SongInfo = Marshal.PtrToStructure(Of SongInfo)(payload)
					RaiseEvent SongChanged(info.index, info.filename)
					PlayByWMP(info.index, info.filename)

				Case SiriusEvent._Error
					RaiseEvent MediaError(Marshal.PtrToStringUni(payload))

			End Select
		Loop
	End Sub
End Class
