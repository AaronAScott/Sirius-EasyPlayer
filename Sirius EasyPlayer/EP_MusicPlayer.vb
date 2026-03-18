Imports System.Drawing.Drawing2D
Imports System.IO
Imports System.Runtime.CompilerServices.RuntimeHelpers
Imports System.Runtime.InteropServices
Imports Sirius_EasyPlayer.MediaPlayer

Public Class frmMusicPlayer
	Inherits System.Windows.Forms.Form

	'***********************************************************************
	' Sirius Playlist Music Player form
	' PE_MAIN.VB
	' Written: March 2026
	' Programmer: Aaron Scott
	' Copyright 2026 Sirius Software All Rights Reserved
	'***********************************************************************

	' Declare function that will let this form block screensaver and shutdown.

	<DllImport("kernel32.dll", SetLastError:=True)>
	Private Shared Function SetThreadExecutionState(ByVal esFlags As EXECUTION_STATE) As EXECUTION_STATE
	End Function

	<Flags()>
	Private Enum EXECUTION_STATE As Integer
		ES_CONTINUOUS = &H80000000
		ES_DISPLAY_REQUIRED = &H2
		ES_SYSTEM_REQUIRED = &H1
		' Add ES_AWAYMODE_REQUIRED if using media playback scenarios
	End Enum

	' Declare public properties.

	Public Shared Property IsOpen As Boolean = False
	Public MP As MediaPlayer

	' Declare variables local to this module.

	Private ListBoxLeft As Integer = 428 ' Set at design time
	Private ElapsedTime As Integer
	Private AlbumArt As Image = Nothing
	Private kbdHook As New KeyboardHook
	'***********************************************************************

	' The form is loaded.

	'***********************************************************************
	Private Sub frmMusicPlayer_Load(sender As Object, e As EventArgs) Handles Me.Load

		' Declare variables.

		Dim zx As String
		Dim rect As Rectangle

		' Indicate this form is open.

		IsOpen = True

		' Fill the playlist menu item with submenus showing all playlists.

		PopulatePlaylistMenu()

		' Restore the window state.

		zx = GetSetting("Sirius" & ProgramName.Replace(" ", ""), "MusicPlayer", "Size", "0,100,100,685, 550")
		Me.WindowState = Val(ParseString(zx))
		If Me.WindowState = System.Windows.Forms.FormWindowState.Normal Then

			' Before using the positions, make sure they are valid

			rect = My.Computer.Screen.WorkingArea
			If Val(zx) >= 0 And Val(zx) <= rect.Width - Me.Width Then
				Me.Top = Val(ParseString(zx))
				Me.Left = Val(ParseString(zx))
				Me.Width = Val(ParseString(zx))
				Me.Height = Val(zx)
			End If
		End If

		' Create the media player control and add it to the form.

		MP = New MediaPlayer
		MP.Parent = Me
		Me.Controls.Add(MP)
		MP.Location = New Point((Me.Width - MP.Width - lstPlaylist.Width) \ 2, Me.Height - MP.Height - MenuStrip1.Height - 20)
		MP.Player.Volume = 0.67
		MP.Player.Repeat = True
		MP.ListBox = lstPlaylist

		' Add the handler for the media player events

		AddHandler MP.SongChanged, AddressOf MP_SongChanged
		AddHandler MP.PlayStateChanged, AddressOf PlayStateChanged
		AddHandler MP.PlayListStartLoad, AddressOf StartLoad
		AddHandler MP.PlayListEndLoad, AddressOf EndLoad

		' Turn off screen timeout and install a keyboard hook.

		SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS Or EXECUTION_STATE.ES_DISPLAY_REQUIRED Or EXECUTION_STATE.ES_SYSTEM_REQUIRED)
		kbdHook.Install()


	End Sub

	'***********************************************************************

	' The form is closed.

	'***********************************************************************
	Private Sub frmMusicPlayer_Closed(sender As Object, e As EventArgs) Handles Me.Closed

		' Declare variables.

		Dim zx As String

		' Indicate this form is closed.

		IsOpen = False

		' Remember the window state

		If Me.Top > 0 And Me.Left > 0 Then
			zx = $"{Me.WindowState},{Me.Top},{Me.Left},{Me.Width},{Me.Height}"
			SaveSetting("Sirius" & ProgramName.Replace(" ", ""), "MusicPlayer", "Size", zx)
		End If

		' Remove the event handlers and dispose of the music player.

		RemoveHandler MP.SongChanged, AddressOf MP_SongChanged
		RemoveHandler MP.PlayStateChanged, AddressOf PlayStateChanged
		RemoveHandler MP.PlayListStartLoad, AddressOf StartLoad
		RemoveHandler MP.PlayListEndLoad, AddressOf EndLoad
		MP.Dispose()

		' Restore the ability for the screen to timeout and remove the keyboard hook.

		kbdHook.Uninstall()
		SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS)

	End Sub
	'***********************************************************************

	' The form is resized.

	'***********************************************************************
	Private Sub frmMusicPlayer_Resize(sender As Object, e As EventArgs) Handles Me.Resize

		' Declare variables

		Dim ii As Integer = Me.Width - ListBoxLeft

		' Keep the form height the same.

		Me.Height = 550

		' Make the list box fit the increased or descreased space.

		If ii >= 205 Then lstPlaylist.Width = ii - 15
	End Sub
	'***********************************************************************

	' Paint event handler for the form.

	'***********************************************************************
	Private Sub frmMusicPlayer_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint

		Dim g As Graphics = e.Graphics

		Using BackgroundBrush As New LinearGradientBrush(Me.ClientRectangle, DarkenOrLightenColor(Color.LightBlue, -17), Color.White, LinearGradientMode.ForwardDiagonal)
			g.FillRectangle(BackgroundBrush, Me.ClientRectangle)
		End Using
	End Sub
	'***********************************************************************

	' A playlist is selected.

	'***********************************************************************
	Private Sub mnuSelectPlaylist_Click(sender As Object, e As EventArgs)

		' Declare variables.

		Dim zx As String

		' Determine the name of the playlist item selected.

		zx = sender.text

		' Determine the name of the playlist item selected.

		MP.Playlist = GetSetting("Sirius" & ProgramName.Replace(" ", ""), "Settings", "MusicFolder", "") & "\Playlists\" & zx & ".wpl"

		' Set the name of the playlist in form's title bar.

		Me.Text = "Playing " & zx
	End Sub
	'***********************************************************************

	' The Play Me to Sleep menu option is selected.

	'***********************************************************************
	Private Sub mnPMTS_Click(sender As Object, e As EventArgs) Handles mnPMTS.Click

		frmSleepTimer.Show()

	End Sub
	'***********************************************************************

	' The Album Art picture box needs to be redrawn.

	'***********************************************************************
	Private Sub picAlbumArt_Paint(sender As Object, e As PaintEventArgs) Handles picAlbumArt.Paint

		Dim g As Graphics = e.Graphics
		If AlbumArt Is Nothing Then
			g.DrawImage(GetNoAlbumArtImage(New Drawing.Size(picAlbumArt.Size)), picAlbumArt.ClientRectangle)
		Else
			g.DrawImage(AlbumArt, picAlbumArt.ClientRectangle)
		End If

	End Sub
	'***********************************************************************

	' The timer has ticked (1 second intervals).

	'***********************************************************************
	Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

		' Declare variables

		Dim ii As Integer

		ElapsedTime += 1
		Dim minutes As Integer = ElapsedTime \ 60
		Dim seconds As Integer = ElapsedTime Mod 60
		lblElapsedTime.Text = minutes.ToString("00") & ":" & seconds.ToString("00")

		If MP.Duration > 0 Then ii = ElapsedTime / MP.Duration / 60 * 100 Else ii = 0
		If ii > 100 Then ii = 100
		ProgressBar1.Value = ii
		Application.DoEvents()

		' Reset the selected song it it's been changed in the listbox.

		If Not lstPlaylist Is Nothing Then
			If MP.Player.CurrentSong.index <> lstPlaylist.SelectedIndex Then lstPlaylist.SelectedIndex = MP.Player.CurrentSong.index
		End If

	End Sub
	'***********************************************************************

	' The keep awake timer has ticked (5 minute intervals).

	'***********************************************************************
	Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick

		' Remind the system that this form is busy.  

		SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS Or EXECUTION_STATE.ES_DISPLAY_REQUIRED Or EXECUTION_STATE.ES_SYSTEM_REQUIRED)

	End Sub
	'***********************************************************************

	' Event Handler for the music player PlayStateChanged event.

	'***********************************************************************

	Private Sub PlayStateChanged(NewState As Integer)

		' Check the player playstate
		' Undefined = 0,
		' Stopped = 1,
		' Paused = 2,
		' Playing = 3,
		' ScanForward = 4,
		' ScanReverse = 5,
		' MediaEnded = 6
		' PlayingExternal = 7,
		' Ready = 8,

		' Start/Stop the timer as needed, or reset
		' the elapsed time when the music is changing.

		Select Case NewState
			Case SiriusAudio.SEP_Playstate.SEP_Stopped, SiriusAudio.SEP_Playstate.SEP_Paused
				Timer1.Enabled = False
			Case SiriusAudio.SEP_Playstate.SEP_Playing, SiriusAudio.SEP_Playstate.SEP_PlayingExternal
				Timer1.Enabled = True
			Case SiriusAudio.SEP_Playstate.SEP_PlaylistEnded
				MP.Player.StopAll()
				Timer1.Enabled = False
		End Select

	End Sub
	'***********************************************************************

	' Event Handler for the SongChanged event.

	'***********************************************************************
	Private Sub MP_SongChanged()

		' Get the duration of the new song and display it below the album art.

		Dim minutes As Integer = Fix(MP.Duration)
		Dim seconds As Integer = (MP.Duration - minutes) * 60
		lblDuration.Text = minutes.ToString("00") & ":" & seconds.ToString("00")

		' Reset the elapsed time and start the timer.

		ElapsedTime = 0
		ProgressBar1.Value = ElapsedTime
		Timer1.Enabled = True

		' Get the new album art and force it to be displayed.

		AlbumArt = MP.AlbumArt
		picAlbumArt.Invalidate()

	End Sub
	'***********************************************************************

	' The playlist is loading into the music player

	'***********************************************************************
	Private Sub StartLoad()

		Me.Cursor = Cursors.WaitCursor
		lstPlaylist.Enabled = False
		lstPlaylist.Items.Clear()

	End Sub
	'***********************************************************************

	' The playlist has finished loading.

	'***********************************************************************

	Private Sub EndLoad()

		' Declare variables

		Dim i As Integer

		' Refill the list box with the new playlist.

		lstPlaylist.BeginUpdate()
		For i = 0 To MP.Player.PlayListItems.Count - 1
			lstPlaylist.Items.Add(MP.Player.PlayListItems(i))
		Next i
		lstPlaylist.EndUpdate()

		Me.Cursor = Cursors.Default
		lstPlaylist.Enabled = True
		lstPlaylist.Refresh()

	End Sub
	'***********************************************************************

	' Sub to add the submenu items to the playlist menu item.

	'***********************************************************************
	Private Sub PopulatePlaylistMenu()

		' Declare variables

		Dim zx As String = GetSetting("Sirius" & ProgramName.Replace(" ", ""), "Settings", "MusicFolder", "")
		Dim Playlists As IReadOnlyCollection(Of String)

		' Get all the playlists in the Playlists folder.


		Playlists = My.Computer.FileSystem.GetFiles(zx & "\Playlists\", FileIO.SearchOption.SearchTopLevelOnly, "*.wpl")
		For Each File In Playlists
			mnuSelectPlaylist.DropDownItems.Add(New ToolStripMenuItem(Path.GetFileNameWithoutExtension(File), Nothing, AddressOf mnuSelectPlaylist_Click))
		Next File

	End Sub
	'***********************************************************************

	' Sub to stop the music playing.  This is called from the sleep timer,
	' if used.

	'***********************************************************************
	Public Sub StopPlaying()

		MP.Player.StopAll()

	End Sub

	'***********************************************************************

	' This routine is called by the keyboard hook, when it intercepts a
	' media control button.  This ensures that the player always responds
	' to a media control button even when this program does not have the
	' focus.

	'***********************************************************************
	Public Sub OnShortcutKeyPressed(KeyCode As Keys)
		Select Case KeyCode
			Case Keys.MediaNextTrack
				MP.Player.NextSong()
			Case Keys.MediaPreviousTrack
				MP.Player.PreviousSong()
			Case Keys.MediaPlayPause
				MP.Player.Play()
		End Select
	End Sub

End Class
