Imports System.Diagnostics.Eventing
Imports System.IO

Public Class Form1

	Private CurrentPL As String = ""
	Private sa As New SiriusAudio


	Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click


		If CurrentPL <> "" Then
			sa.Play()
		End If

	End Sub
	Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
		If CurrentPL <> "" Then
			sa.PreviousSong()
		End If

	End Sub
	Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
		If CurrentPL <> "" Then
			sa.NextSong()
		End If

	End Sub

	Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load

		Dim Playlists As IReadOnlyCollection(Of String)

		' Get all the playlists in the Playlists folder.


		Playlists = My.Computer.FileSystem.GetFiles("d:\music\Playlists\", FileIO.SearchOption.SearchTopLevelOnly, "*.wpl")
		For Each File In Playlists
			mnuSelectPL.DropDownItems.Add(New ToolStripMenuItem(Path.GetFileNameWithoutExtension(File), Nothing, AddressOf mnuSelectPL_Click))
		Next File

		AddHandler sa.SongChanged, AddressOf SongChanged
		AddHandler sa.PlaylistLoaded, AddressOf PlaylistLoaded
		AddHandler sa.PlayStateChanged, AddressOf PlaystateChanged
		AddHandler sa.MediaError, AddressOf PlayError

		Label2.Text = "Play state: " & Choose(sa.PlayState + 1, "Undefined", "Stopped", "Paused", "Playing", "ScanForward", "ScanReverse", "MediaEnded", "Ready")


	End Sub
	Private Sub Form1_Closed(sender As Object, e As EventArgs) Handles Me.Closed
		RemoveHandler sa.SongChanged, AddressOf SongChanged
		RemoveHandler sa.PlaylistLoaded, AddressOf PlaylistLoaded
		RemoveHandler sa.PlayStateChanged, AddressOf PlaystateChanged
		RemoveHandler sa.MediaError, AddressOf PlayError
	End Sub
	Private Sub SongChanged(idx As Integer, filename As String)
		Label1.Text = idx & vbCrLf & filename
		ListBox1.SelectedIndex = idx
		Dim t = TagLib.File.Create(filename)

		Dim s As TimeSpan = t.Properties.Duration
		Dim duration As Double = s.TotalMinutes
		Label3.Text = "Duration: " & Format(duration, "00.00")
	End Sub
	Private Sub PlaystateChanged(ps As Integer)
		Label2.Text = "Play state: " & Choose(ps, "Undefined", "Stopped", "Paused", "Playing", "ScanForward", "ScanReverse", "WMAFileEncountered", "Ready")
		Application.DoEvents()
	End Sub
	Private Sub PlaylistLoaded()
		Label1.Text = ""
	End Sub
	Private Sub PlayError(msg As String)
		'MsgBox(msg)
	End Sub

	Private Sub mnuSelectPL_Click(sender As Object, e As EventArgs)

		CurrentPL = "d:\music\playlists\" & sender.text & ".wpl"
		sa.Playlist = CurrentPL

		ListBox1.Items.Clear()
		ListBox1.BeginUpdate()

		For Each zx As String In sa.PlayListItems
			ListBox1.Items.Add(zx)
		Next zx
		ListBox1.EndUpdate()

	End Sub

	Private Sub ListBox1_DoubleClick(sender As Object, e As EventArgs) Handles ListBox1.DoubleClick
		If ListBox1.SelectedIndex >= 0 And ListBox1.SelectedIndex < sa.PlayListItems.Count Then sa.PlaySong(ListBox1.SelectedIndex)
	End Sub
End Class
