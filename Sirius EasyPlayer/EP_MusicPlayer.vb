Imports System.IO
Imports System.Drawing.Drawing2D
Imports System.Runtime.InteropServices

Public Class frmMusicPlayer
	Inherits System.Windows.Forms.Form

	'***********************************************************************
	' Sirius Playlist Music Player form
	' PE_MAIN.VB
	' Written: May 2025
	' Programmer: Aaron Scott
	' Copyright 2025 Sirius Software All Rights Reserved
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

	' Declare public variables.

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

		' Fill the playlist menu item with submenus showing all playlists.

		PopulatePlaylistMenu()

		' Restore the window state.

		zx = GetSetting("Sirius" & ProgramName.Replace(" ", ""), "MusicPlayer", "Size", "0,649, 550")
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
		MP.Location = New Point((Me.Width - MP.Width - ListBox1.Width) \ 2, Me.Height - MP.Height - MenuStrip1.Height - 20)
		'.MP.Player.settings.volume = 100
		MP.ListBox = ListBox1

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

		SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS)
		kbdHook.Uninstall()
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

		If ii >= 205 Then ListBox1.Width = ii
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

	' Event Handler for the music player PlayStateChanged event.

	'***********************************************************************

	Private Sub PlayStateChanged(NewState As Integer)

		' Check the player playstate
		'wmppsUndefined = 0,
		'wmppsStopped = 1,
		'wmppsPaused = 2,
		'wmppsPlaying = 3,
		'wmppsScanForward = 4,
		'wmppsScanReverse = 5,
		'wmppsBuffering = 6,
		'wmppsWaiting = 7,
		'wmppsMediaEnded = 8,
		'wmppsTransitioning = 9,
		'wmppsReady = 10,
		'wmppsReconnecting = 11,
		'wmppsLast = 12

		' Start/Stop the timer as needed, or reset
		' the elapsed time when the music is changing.

		Select Case NewState
			Case 1
				Timer1.Stop()
			Case 2
				Timer1.Stop()
			Case 3
				Timer1.Start()
			Case 4, 5, 9
				Timer1.Stop()
		End Select

		' Whatever has happened, reset the elapsed time.

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
		If MP.Player.PlayState = 3 Then Timer1.Enabled = True ' 3=Playing

		' Get the new album art and force it to be displayed.

		AlbumArt = MP.AlbumArt
		picAlbumArt.Invalidate()

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

	End Sub
	'***********************************************************************

	' The keep awake timer has ticked (5 minute intervals).

	'***********************************************************************
	Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick

		' Remind the system that this form is busy.  

		SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS Or EXECUTION_STATE.ES_DISPLAY_REQUIRED Or EXECUTION_STATE.ES_SYSTEM_REQUIRED)

	End Sub
	'***********************************************************************

	' The playlist is loading into the music player

	'***********************************************************************
	Private Sub StartLoad()

		Me.Cursor = Cursors.WaitCursor
		ListBox1.Enabled = False

	End Sub
	'***********************************************************************

	' The playlist has finished loading.

	'***********************************************************************

	Private Sub EndLoad()

		Me.Cursor = Cursors.Default
		ListBox1.Enabled = True

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

	' Event handler for the shortcut keys.

	'***********************************************************************
	Public Sub ShortcutKeyPressed(KeyCode As Keys)

		Select Case KeyCode
			Case Keys.MediaPreviousTrack
				MP.Player.PreviousSong()
			Case Keys.MediaNextTrack
				MP.Player.NextSong()
			Case Keys.MediaPlayPause
				MP.Player.Play()
		End Select

	End Sub

End Class