Imports System.Data.SqlClient
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Globalization
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Xml

Public Class frmMain
	Inherits System.Windows.Forms.Form

	'***********************************************************************
	' Sirius Sirius EasyPlayer Main Form
	' EP_MAIN.VB
	' Written: March 2026
	' Programmer: Aaron Scott
	' Copyright 2026 Sirius Software All Rights Reserved
	'***********************************************************************

	' Declare variables local to this module.

	Private PlaylistChangesSaved As Boolean = True
	Private PlaylistName As String = "(Untitled)"
	Private SelectItem As Integer
	Private INDENT As Single
	Private TopRowIndex As Integer
	Private BottomRowIndex As Integer
	Private CurrentRow As DataRow
	Private CurrentRowIndex As Integer
	Private ArtistLineHeight As Integer
	Private AlbumLineHeight As Integer
	Private SongLineHeight As Integer
	Private dropIndex As Integer = -1
	Private dragToolTip As ToolTip()
	Private MP As MediaPlayer
	Private ElapsedTime As Integer
	Private kbdhook As New KeyboardHook
	Private mAlbumArt As Image

	' Declare the fonts we'll use

	Private fArtist As Font
	Private fAlbum As Font
	Private fSong As Font

	' Declare the DisplayLines collection as public,
	' as the routines to re-locate a library might need
	' to clear them.

	Public DisplayLines As New DisplayLines

	' Unified undo stack structure
	Class UndoAction
		Public Property ActionType As String
		Public Property Items As List(Of String)
		Public Property OriginalIndex As Integer
		Public Property InsertedIndices As List(Of Integer) ' Track paste positions
	End Class
	' Stores actual playlist data (tracks) - the core list
	Private playlist As New List(Of String)

	' Clipboard storage for cut/copied items - temporary holding area
	Private clipboardList As New List(Of String)

	' Stack for undo operations - tracks all editing actions
	Private undoStack As New Stack(Of UndoAction)

	'***********************************************************************

	' The form is loaded.

	'***********************************************************************
	Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles Me.Load

		' Declare variables.

		Dim ii As Integer
		Dim zx As String
		Dim g As Graphics = picLibraryDisplay.CreateGraphics
		Dim rect As Rectangle

		ProgramName = "Sirius EasyPlayer"
		Version = CStr(My.Application.Info.Version.Major) & "." & CStr(My.Application.Info.Version.Minor) & CStr(My.Application.Info.Version.Build) & CStr(My.Application.Info.Version.MinorRevision)
		DBVersion = "1.00"

		' Add program dependencies. Declaring the DLLs as "file" will cause them
		' to be merely copied over if they don't exist.

		Dependencies.Add(New Dependency("SiriusEasyPlayer", "file", "Taglib-sharp.dll"))
		Dependencies.Add(New Dependency("SiriusEasyPlayer", "file", "Newtonsoft.Json.dll"))
		Dependencies.Add(New Dependency("SiriusEasyPlayer", "file", "SiriusAudio.dll"))
		Dependencies.Add(New Dependency("SiriusEasyPlayer", "file", "README.md"))
		Dependencies.Add(New Dependency("SiriusEasyPlayer", "file", "LICENSE.md"))

		' Check for updates.

		CheckForUpdates()

		' Create the fonts for displaying album information.

		fArtist = New Font("Arial", 12, FontStyle.Bold)
		fAlbum = New Font("Arial", 10)
		fSong = New Font("Arial", 9)

		' Set the height of the different types of lines,

		ArtistLineHeight = g.MeasureString("X", fArtist).Height
		AlbumLineHeight = 36 ' Allows for an image and space above and below.
		SongLineHeight = g.MeasureString("X", fSong).Height
		g.Dispose()

		ProgramColorTheme = "0x5112"
		MsgBoxTheme.FontSize = 12

		' Make the form immediately visible.

		Me.Show()

		' See if the library database exists.  If it does, open it.  If not, create it.

		If My.Computer.FileSystem.FileExists(MusicLibraryDatabase) Then

			' Check the availablility of the music library.

			Do
				If WaitForMusicFolder(MusicFolder, 30) Then
					Exit Do
				Else
					If MsgBox("Your music folder, located at """ & MusicFolder & """ is unavailble." & vbCrLf & "If it's a removable drive, re-connect it, and click ""Okay"" to try again.  If you click ""Cancel"", you'll be prompted to select the new location of your music.", MsgBoxStyle.Information + MsgBoxStyle.OkCancel, "Music Folder Not Online") = MsgBoxResult.Ok Then
						Continue Do
					Else
						frmLocateMusicFolder.ShowDialog()
					End If
				End If

			Loop

			' Open the database.

			DbOpen = OpenADatabase(MusicLibraryDatabase)

			' If no library exists, create one.

		Else
			If MsgBox("No music library has been created.  Click ""Okay"" to create a music library now.", MsgBoxStyle.OkCancel + MsgBoxStyle.Information, "First Time Setup") = MsgBoxResult.Cancel Then End
			CreateNewDatabase()
		End If

		' Restore the window state.

		zx = GetSetting("Sirius" & ProgramName.Replace(" ", ""), "Main", "Size", "0,39,143,1038,683,400")
		Me.WindowState = Val(ParseString(zx))
		If Me.WindowState = System.Windows.Forms.FormWindowState.Normal Then

			' Before using the positions, make sure they are valid

			rect = My.Computer.Screen.WorkingArea
			If Val(zx) >= 0 And Val(zx) <= rect.Width - Me.Width Then
				Me.Top = Val(ParseString(zx))
				Me.Left = Val(ParseString(zx))
				Me.Width = Val(ParseString(zx))
				Me.Height = Val(ParseString(zx))
				SplitContainer1.SplitterDistance = Val(zx)
			End If
		End If

		' Check if the library opened successfully.

		If DbOpen Then

			' Populate the music library

			LibraryDS.Clear()
			LibraryDA.Fill(LibraryDS, "Table")
			LibraryTable = SelectByView(LibraryDS.Tables("Table"))

			' if the the library is empty, import songs now.

			If LibraryTable.Rows.Count = 0 Then
				If frmLocateMusicFolder.ShowDialog = DialogResult.OK Then
					ii = ImportMusicList(MusicFolder, lblStatus)
					LibraryDS.Clear()
					LibraryDA.Fill(LibraryDS, "Table")
					LibraryTable = SelectByView(LibraryDS.Tables("Table"))
				End If
			End If

			' Set the amount of change for the scroll bars

			TopRowIndex = 0
			VScrollBar1.SmallChange = 1
			VScrollBar1.LargeChange = 25
			VScrollBar1.Maximum = LibraryTable.Rows.Count - 1

			' Set the library view mode: only the best-quality versions, or all versions.

			mnuViewBest.Checked = CBool(GetSetting("Sirius" & ProgramName.Replace(" ", ""), "Settings", "ViewBestOnly", "True"))

			' Cause the list to display.

			Show()
			picLibraryDisplay.Invalidate()

			' If we were passed the name of a playlist, open it now.

			If My.Application.CommandLineArgs.Count > 0 Then OpenPlaylist(My.Application.CommandLineArgs(0))
		End If
	End Sub

	'***********************************************************************

	' The form is closed.

	'***********************************************************************
	Private Sub frmMain_Closed(sender As Object, e As EventArgs) Handles Me.Closed

		Dim zx As String

		' Remember the window state

		If Me.Top > 0 And Me.Left > 0 Then
			zx = Format(Me.WindowState & "," & Me.Top & "," & Me.Left & "," & Me.Width & "," & Me.Height & "," & SplitContainer1.SplitterDistance)
			SaveSetting("Sirius" & ProgramName.Replace(" ", ""), "Main", "Size", zx)
		End If

		fArtist.Dispose()
		fAlbum.Dispose()
		fSong.Dispose()

	End Sub

	'***********************************************************************

	' The form is resized.

	'***********************************************************************
	Private Sub frmMain_Resize(sender As Object, e As EventArgs) Handles Me.Resize

		If Me.WindowState = FormWindowState.Normal Then
			picLibraryDisplay.Height = SplitContainer1.Panel1.Height - 50
			Panel1.Height = picLibraryDisplay.Height
			Panel1.Left = picLibraryDisplay.Width - Panel1.Width
			VScrollBar1.Height = Panel1.Height
			VScrollBar1.Location = New Point(5, 0)
			lstPlayList.Location = New Point(0, lblHeader_0.Top + lblHeader_0.Height)
			If pnlMusicPlayer.Visible Then
				lstPlayList.Size = New Size(SplitContainer1.Panel2.Width, SplitContainer1.Panel2.Height - lblHeader_0.Height - pnlMusicPlayer.Height)
			Else
				lstPlayList.Size = New Size(SplitContainer1.Panel2.Width, SplitContainer1.Panel2.Height - lblHeader_0.Height)
			End If
			SplitContainer1.Width = Me.Width - 18
			lblHeader_0.Width = lstPlayList.Width * 0.5
			lblHeader_1.Width = lstPlayList.Width - lblHeader_0.Width
			lblHeader_1.Left = lblHeader_0.Width
			If MP IsNot Nothing Then MP.Left = (SplitContainer1.Panel2.Width - MP.Width) \ 2

			' Rearrange the music display panel.

			If pnlDisplay.Visible Then
				pnlDisplay.Height = SplitContainer1.Panel2.Height - pnlMusicPlayer.Height
				pnlDisplay.Width = SplitContainer1.Panel2.Width
				picAlbumArt.Left = (pnlDisplay.Width - picAlbumArt.Width) / 2
				lblAlbum.Left = picAlbumArt.Left
				lblArtist.Left = picAlbumArt.Left
				lblElapsedTime.Left = picAlbumArt.Left
				ProgressBar1.Left = lblElapsedTime.Left + lblElapsedTime.Width + 2
				lblDuration.Left = ProgressBar1.Left + ProgressBar1.Width + 2
				lblAlbum.Width = pnlDisplay.Width - 20
				lblAlbum.Left = 10
				lblArtist.Width = lblAlbum.Width
				lblArtist.Left = lblAlbum.Left
			End If
		End If
	End Sub
	'***********************************************************************

	' The slippter has moved on the split container.

	'***********************************************************************
	Private Sub SplitContainer1_SplitterMoved(sender As Object, e As SplitterEventArgs) Handles SplitContainer1.SplitterMoved
		frmMain_Resize(Me, EventArgs.Empty) ' Resize everything
	End Sub
	'***********************************************************************

	' The Recreate Library menu option was clicked.

	'***********************************************************************
	Private Sub mnuRecreate_Click(sender As Object, e As EventArgs) Handles mnuRecreate.Click

		' Make sure the user wants to do this.

		If MsgBox("This function will destroy any previous library.  Use it only if the existing library has become corrupted, lost or unreadable.", MsgBoxStyle.Exclamation + MsgBoxStyle.OkCancel, "Recreate Music Library") = MsgBoxResult.Ok Then

			If DbOpen Then CloseDatabase()

			' Clear out the display lines collection, and erase the picturebox.

			DisplayLines.Clear()
			Using g As Graphics = picLibraryDisplay.CreateGraphics
				g.FillRectangle(Brushes.White, picLibraryDisplay.Bounds)
			End Using

			' Recreate the new database.  The SQL commands will drop the existing one first, then
			' create the new one.

			CreateNewDatabase()

			' Ask the user to select a music folder from which to populate the library.

			If frmLocateMusicFolder.ShowDialog = DialogResult.OK Then

				' Rebuld the libary dataset.i
				LibraryDS.Clear()
				LibraryDA.Fill(LibraryDS, "Table")
				LibraryTable = SelectByView(LibraryDS.Tables("Table"))
				VScrollBar1.Maximum = LibraryTable.Rows.Count - 1

				' Force the newly-recreated library to display.

				picLibraryDisplay.Invalidate()
			End If
		End If
	End Sub
	'***********************************************************************

	' The repair metadata option is selected.

	'***********************************************************************
	Private Sub mnuRepairMetadata_Click(sender As Object, e As EventArgs) Handles mnuRepairMetadata.Click
		RepairMetadata()
	End Sub
	'***********************************************************************

	' The Change Music File Version Precendence menu option is selected.

	'***********************************************************************
	Private Sub mnuChangePrecedence_Click(sender As Object, e As EventArgs) Handles mnuChangePrecedence.Click
		frmChangeFilePrecedence.ShowDialog()
	End Sub
	'***********************************************************************

	' The Check for New Music menu option is clicked.

	'***********************************************************************
	Private Sub mnuCheckForNew_Click(sender As Object, e As EventArgs) Handles mnuCheckForNew.Click

		' Declare variables.

		Dim ii As Integer
		Dim zx As String

		' Call the import routine.

		ii = UpdateMusicList(MusicFolder, lblStatus)
		If ii > 0 Then
			zx = "Update completed. " & ii & " new music file(s) were added."
		Else
			zx = "Update completed.  No new music found."
		End If
		MsgBox(zx, MsgBoxStyle.Information, "Check for New Music")

	End Sub
	'*********************************************************

	' The View Best/View all menu options have changed.

	'*********************************************************
	Private Sub mnuViewMode_CheckedChanged(sender As Object, e As EventArgs) Handles mnuViewBest.Click, mnuViewAll.Click

		' Declare variables

		Dim ViewBest As Boolean = CBool(GetSetting("Sirius" & ProgramName.Replace(" ", ""), "Settings", "ViewBestOnly", CStr(mnuViewBest.Checked)))

		' Toggle the View Best state.

		ViewBest = Not ViewBest
		If ViewBest Then
			mnuViewBest.Checked = True
			mnuViewAll.Checked = False
		Else
			mnuViewBest.Checked = False
			mnuViewAll.Checked = True
		End If

		' Save the option.

		SaveSetting("Sirius" & ProgramName.Replace(" ", ""), "Settings", "ViewBestOnly", CStr(ViewBest))

		' Rebuild the dataset

		LibraryDS.Clear()
		LibraryDA.Fill(LibraryDS, "Table")
		LibraryTable = SelectByView(LibraryDS.Tables("Table"))

		' Repaint the display.

		picLibraryDisplay.Invalidate()

	End Sub
	'*********************************************************

	' The "About Sirius EasyPlayer" menu option is clicked.

	'*********************************************************
	Private Sub mnuAbout_Click(sender As Object, e As EventArgs) Handles mnuAbout.Click
		About.ShowDialog()
	End Sub
	'*********************************************************
	'
	' A paint event has ocurred.  Display the song records.
	'
	'*********************************************************
	Private Sub picLibraryDisplay_Paint(ByVal sender As Object, e As PaintEventArgs) Handles picLibraryDisplay.Paint

		' Declare variables

		Dim jj As Integer
		Dim y As Integer
		Dim ItemType As MusicItemType
		Dim Artist As String
		Dim Album As String
		Dim Song As String
		Dim ImageFile As String
		Dim rect As Rectangle
		Dim dl As DisplayLine
		Dim g As Graphics = e.Graphics

		' If there is no library yet, just exit.

		If LibraryTable Is Nothing OrElse LibraryTable.Rows.Count = 0 Then Exit Sub

		' Create a new set of DisplayLines.  The collection of display lines
		' holds the information for each song currently visible in the list,
		' including the artist, the album, the song and the rectangle in which to
		' draw each item, which differs depending upon the item being drawn, the artist
		' being a larger font, the album being larger yet, as there is an image beside it,
		' and the song, which is plain text.

		DisplayLines.Clear()

		' DisplayLines Artist/Album/Song records.  If a line was previously selected,
		' redraw it with a selected bar.  Otherwise, redraw it as clear.

		' TopRowIndex determines which row in the library dataset begins the
		' display.

		For jj = TopRowIndex To LibraryTable.Rows.Count - 1

			Try
				' Make sure we have enough room to draw another row.

				If y + SongLineHeight > picLibraryDisplay.Height Then Exit For

				Artist = GetR(LibraryTable.Rows(jj), "ArtistName")
				Album = GetR(LibraryTable.Rows(jj), "AlbumName")
				Song = GetR(LibraryTable.Rows(jj), "SongName")
				ImageFile = GetR(LibraryTable.Rows(jj), "AlbumImage")

				' Determine the type of item we have.

				If Artist <> "" And Album = "" Then
					ItemType = MusicItemType.Artist
				ElseIf Artist <> "" And Album <> "" And Song = "" Then
					ItemType = MusicItemType.Album
				Else
					ItemType = MusicItemType.Song
				End If

				' Create a new displayline object.

				dl = New DisplayLine
				dl.ItemType = ItemType

				' Draw the rest of the line based on the type of line

				Select Case ItemType
					Case MusicItemType.Artist
						rect = New Rectangle(INDENT, y, picLibraryDisplay.Width, ArtistLineHeight)
						dl.Bounds = rect
						dl.ArtistName = Artist
						y = DrawOneLine(dl, e.Graphics)

					Case MusicItemType.Album
						dl.ImageBounds = New Rectangle(INDENT, y, 32, 32)
						rect = New Rectangle(INDENT + 34, y + (34 - g.MeasureString("X", fAlbum).Height) / 2, picLibraryDisplay.Width - 34, 34)
						dl.Bounds = rect
						dl.ArtistName = Artist
						dl.AlbumName = Album
						dl.ImageFile = ImageFile
						y = DrawOneLine(dl, e.Graphics)

					Case MusicItemType.Song
						rect = New Rectangle(INDENT + 34, y, picLibraryDisplay.Width, SongLineHeight)
						dl.Bounds = rect
						dl.ArtistName = Artist
						dl.AlbumName = Album
						dl.SongName = Song
						y = DrawOneLine(dl, e.Graphics)
				End Select

				' Add the item to the displaylines collection.

				DisplayLines.Add(dl)

				' Error trapping.

			Catch ex As Exception
				MsgBox(ex.Message)
			End Try
		Next jj

		' Remember the index of the last row of the library datatable displayed.

		BottomRowIndex = jj

	End Sub
	'**********************************************************

	' Paint event handler for the panel that contains the
	' music player.

	'**********************************************************
	Private Sub pnlMusicPlayer_Paint(sender As Object, e As PaintEventArgs) Handles pnlMusicPlayer.Paint

		Using BackgroundBrush As New LinearGradientBrush(pnlMusicPlayer.ClientRectangle, Color.DarkGray, Color.White, LinearGradientMode.ForwardDiagonal)
			e.Graphics.FillRectangle(BackgroundBrush, e.ClipRectangle)
		End Using
	End Sub
	'**********************************************************

	' The mouse is pressed over an item.  Highlight it.

	'**********************************************************
	Private Sub picLibraryDisplay_MouseUp(sender As Object, e As MouseEventArgs) Handles picLibraryDisplay.MouseUp

		' Get the previously-selected line, if any, and the
		' newly selected line.

		Dim dl1 As DisplayLine = DisplayLines.SelectedLine
		Dim dl2 As DisplayLine = DisplayLines.Find(e.Location)

		' If the right mouse button is clicked over the album image, bring up the context menu.

		If e.Button = MouseButtons.Right AndAlso dl2 IsNot Nothing AndAlso dl2.ImageBounds.Contains(e.Location) Then
			ContextMenuStrip2.Show(picLibraryDisplay, e.Location)
		End If

		' If the right mouse button is clicked over an artist name, an album name or a song name,
		' bring up the context menu.

		If e.Button = MouseButtons.Right AndAlso dl2 IsNot Nothing AndAlso Not dl2.ImageBounds.Contains(e.Location) Then
			If frmMusicPlayer.IsOpen Or Not IsNothing(MP) Then mnuCMPlayItem.Enabled = False Else mnuCMPlayItem.Enabled = True
																									    _
			' Determine whether we're playing an album
			Select Case dl2.ItemType
				Case MusicItemType.Artist
					mnuCMPlayItem.Text = "&Play Artist"
					mnuCMAddToPlaylist.Text = "Add &Artist to Playlist"
					mnuCMPlayItem.Tag = MusicItemType.Artist & ":Artist:" & dl2.ArtistName
					mnuCMCompatibility.Tag = dl2.ArtistName
					RemoveHandler mnuCMCompatibility.CheckedChanged, AddressOf mnuCMCompatibility_CheckedChanged
					mnuCMCompatibility.Checked = IsCompatibilitySet(mnuCMCompatibility.Tag)
					AddHandler mnuCMCompatibility.CheckedChanged, AddressOf mnuCMCompatibility_CheckedChanged
				Case MusicItemType.Album
					mnuCMPlayItem.Text = "&Play Album"
					mnuCMAddToPlaylist.Text = "Add &Album to Playlist"
					mnuCMPlayItem.Tag = MusicItemType.Album & ":Artist:" & dl2.ArtistName & ":Album:" & dl2.AlbumName
					mnuCMCompatibility.Tag = dl2.ArtistName & ":" & dl2.AlbumName
					RemoveHandler mnuCMCompatibility.CheckedChanged, AddressOf mnuCMCompatibility_CheckedChanged
					mnuCMCompatibility.Checked = IsCompatibilitySet(mnuCMCompatibility.Tag)
					AddHandler mnuCMCompatibility.CheckedChanged, AddressOf mnuCMCompatibility_CheckedChanged
				Case MusicItemType.Song
					mnuCMPlayItem.Text = "&Play Song"
					mnuCMAddToPlaylist.Text = "Add &Song to Playlist"
					mnuCMPlayItem.Tag = MusicItemType.Song & ":Artist:" & dl2.ArtistName & ":Album:" & dl2.AlbumName & ":Song:" & dl2.SongName
					mnuCMCompatibility.Tag = dl2.ArtistName & ":" & dl2.AlbumName & ":" & dl2.SongName
					RemoveHandler mnuCMCompatibility.CheckedChanged, AddressOf mnuCMCompatibility_CheckedChanged
					mnuCMCompatibility.Checked = IsCompatibilitySet(mnuCMCompatibility.Tag)
					AddHandler mnuCMCompatibility.CheckedChanged, AddressOf mnuCMCompatibility_CheckedChanged
			End Select
			ContextMenuStrip3.Show(picLibraryDisplay, e.Location)
		End If

		' If we have a valid line selected, then make it the 
		' selected line of the collection.

		If Not dl2 Is Nothing Then DisplayLines.SelectedLine = dl2

		' Draw any previously-selected, or newly-selected line.
		' The DrawOneLine routine will highlight a selected line
		' and un-highlight any other.

		Using g As Graphics = picLibraryDisplay.CreateGraphics
			If dl1 IsNot Nothing Then DrawOneLine(dl1, g)
			If dl2 IsNot Nothing Then DrawOneLine(dl2, g)
		End Using

	End Sub
	'***********************************************************************

	' An item is double-clicked in the library display.

	'***********************************************************************
	Private Sub picLibraryDisplay_DoubleClick(sender As Object, e As EventArgs) Handles picLibraryDisplay.DoubleClick

		' Declare variables.

		Dim xx As Integer = 0
		Dim zx As String
		Dim dl As DisplayLine = DisplayLines.SelectedLine
		Dim AlbumDir As String
		Dim song As String
		Dim SongFiles As List(Of String)

		' If we haven't created a song list, do so now.

		lstPlayList.BeginUpdate()

		Select Case dl.ItemType
			Case MusicItemType.Artist

				' Loop through the album folders beneath the artist folder.

				xx = lstPlayList.SelectedIndex
				For Each AlbumDir In Directory.GetDirectories(MusicFolder & dl.ArtistName)
					' Process songs within album
					Dim songs As IReadOnlyCollection(Of String) = My.Computer.FileSystem.GetFiles(AlbumDir, FileIO.SearchOption.SearchTopLevelOnly, ExtensionPrecedenceWildcards())
					If mnuViewBest.Checked Then
						SongFiles = FilterPreferredCopies(songs)
					Else
						SongFiles = songs.ToList
					End If
					For Each song In SongFiles
						If lstPlayList.SelectedIndex >= 0 Then
							lstPlayList.Items.Insert(xx, song)
							xx += 1
						Else
							xx = lstPlayList.Items.Add(song)
						End If
					Next Song
				Next AlbumDir

			Case MusicItemType.Album
				' Process songs within album

				AlbumDir = $"{MusicFolder}{dl.ArtistName}\{dl.AlbumName}\"
				xx = lstPlayList.SelectedIndex
				Dim songs As IReadOnlyCollection(Of String) = My.Computer.FileSystem.GetFiles(AlbumDir, FileIO.SearchOption.SearchTopLevelOnly, ExtensionPrecedenceWildcards())
				If mnuViewBest.Checked Then
					SongFiles = FilterPreferredCopies(songs)
				Else
					SongFiles = songs.ToList
				End If
				For Each song In SongFiles
					If lstPlayList.SelectedIndex >= 0 Then
						lstPlayList.Items.Insert(xx, song)
						xx += 1
					Else
						xx = lstPlayList.Items.Add(song)
					End If
				Next song


			Case MusicItemType.Song

				xx = lstPlayList.SelectedIndex
				zx = MusicFolder & AddDirSeparator(dl.ArtistName) & AddDirSeparator(dl.AlbumName)
				If lstPlayList.SelectedIndex >= 0 Then
					lstPlayList.Items.Insert(xx, zx & dl.SongName)
					xx += 1
				Else
					xx = lstPlayList.Items.Add(zx & dl.SongName)
				End If

		End Select
		lstPlayList.EndUpdate()

		Application.DoEvents()
	End Sub
	'**********************************************************

	' The Control Table Editor menu option is clicked.

	'**********************************************************
	Private Sub mnuControlTableEdit_Click(sender As Object, e As EventArgs) Handles mnuControlTableEditor.Click
		frmControlTableEditor.ShowDialog()
	End Sub
	'**********************************************************

	' The Registry Editor menu option is clicked.

	'**********************************************************
	Private Sub mnuRegistryEditor_Click(sender As Object, e As EventArgs) Handles mnuRegistryEditor.Click
		frmRegistryEditor.ShowDialog()
	End Sub
	'**********************************************************

	' The Album Art picture box needs to be redrawn.

	'**********************************************************
	Private Sub picAlbumArt_Paint(sender As Object, e As PaintEventArgs) Handles picAlbumArt.Paint

		Dim g As Graphics = e.Graphics
		If mAlbumArt Is Nothing Then
			g.DrawImage(GetNoAlbumArtImage(New Drawing.Size(picAlbumArt.Size)), picAlbumArt.ClientRectangle)
		Else
			g.DrawImage(mAlbumArt, picAlbumArt.ClientRectangle)
		End If

	End Sub
	'**********************************************************
	'
	' The scroll bar has been moved.
	'
	'**********************************************************
	Private Sub VScroll1_Scroll(ByVal sender As Object, ByVal e As ScrollEventArgs) Handles VScrollBar1.Scroll

		' Declare variables

		Dim xx As Integer

		' Clear the selected item

		SelectItem = -1

		Select Case e.Type
			Case ScrollEventType.EndScroll

				' Move the number or records difference between the old
				' position and the new position.

				xx = System.Math.Abs(TopRowIndex - e.NewValue)
				If e.NewValue > TopRowIndex Then
					MoveDown(xx)
				ElseIf e.NewValue < TopRowIndex Then
					MoveUp(xx)
				End If

				' Remember the new position of the first record in the
				' DisplayLines

				TopRowIndex = e.NewValue
		End Select

	End Sub

	'**********************************************************

	' Event Handler for the "Draw Item" event of the playlist
	' list box.

	'**********************************************************
	Private Sub lstPlayList_DrawItem(sender As Object, e As DrawItemEventArgs) Handles lstPlayList.DrawItem

		' Declare variables

		Dim ContainsError As Boolean
		Dim jj As Integer
		Dim xx As String
		Dim zx As String
		Dim Artist As String
		Dim Album As String
		Dim Song As String
		Dim Rect As Rectangle
		Dim g As Graphics = e.Graphics
		Dim f As Font = e.Font
		Dim l As Label

		Dim sf As New StringFormat()
		sf.FormatFlags = StringFormatFlags.NoWrap
		sf.Trimming = StringTrimming.EllipsisCharacter

		' If the music player control has taken control of drawing this listbox, it will set the listbox
		' tag property to "true" during this event.  If this is seen, exit and do nothing: the draw event
		' has been handled higher up the chain of event handlers.

		If sender.tag = "Handled" Then
			sender.tag = ""
			Exit Sub
		End If

		' Get the song name and album name from the listbox item.

		If e.Index >= 0 Then
			zx = lstPlayList.Items(e.Index)

			' Lines that indicate song data that is in anyway invalid are flagged with
			' an asterisk as the first character of the song file name.

			If zx.StartsWith("*") Then
				ContainsError = True
				zx = zx.Substring(1)
			Else
				ContainsError = False

			End If

			' Each item is the fully-qualified name of a song file.  Strip off the name
			' of the artist (the first level folder under the music folder), the album,
			' (the next level) and the name of the song file.

			Dim wx As String() = zx.Split("\")
			If wx(0) = ".." Then Artist = wx(1) Else Artist = wx(2)
			Album = Path.GetFileName(Path.GetDirectoryName(zx))

			' First draw the background.

			e.DrawBackground()

			' Unless there is an item to draw, do nothing.

			If e.Index >= 0 Then

				' Get the line to be displayed.

				xx = sender.items(e.Index)

				' Calculate the rectangles for song name and album name.

				For jj = 1 To 2
					l = Choose(jj, lblHeader_0, lblHeader_1)
					Rect = l.Bounds
					Rect.X -= sender.left
					Rect.Y = e.Bounds.Y

					' Display the data in the column.

					If jj = 1 Then
						Song = TextTrim(g, SanitizeSongName(Path.GetFileNameWithoutExtension(zx)), l.Width, f)
						If ContainsError Then
							g.DrawString(Song, f, Brushes.Red, Rect, sf)
						Else
							g.DrawString(Song, f, Brushes.Black, Rect, sf)
						End If
					Else
						zx = Artist & "-" & Album
						If ContainsError Then
							zx = TextTrim(g, zx, Rect.Width, f)
							g.DrawString(zx, f, Brushes.Red, Rect, sf)
						Else
							g.DrawString(zx, f, Brushes.Blue, Rect, sf)
						End If
					End If
				Next jj
			End If
		End If

	End Sub
	'***********************************************************************

	' The New Playlist menu option is clicked.

	'***********************************************************************
	Private Sub mnuNew_Click(sender As Object, e As EventArgs) Handles mnuNew.Click

		lstPlayList.Items.Clear()
		PlaylistName = "(untitled)"

	End Sub
	'***********************************************************************

	' The Open Playlist menu option is clicked.

	'***********************************************************************
	Private Sub mnuOpenPL_Click(sender As Object, e As EventArgs) Handles mnuOpenPL.Click

		' Declare variables

		Dim res As DialogResult

		' Have the user select a playlist from the playlists folder, in the music folder.

		OpenFileDialog1.Title = "Open Windows Playlist"
		OpenFileDialog1.Filter = "Windows Playlists (*.wpl)|*.wpl"
		OpenFileDialog1.InitialDirectory = MusicFolder & "Playlists"
		OpenFileDialog1.FileName = ""

		' If the user selected a playlist to open, proceed.

		If OpenFileDialog1.ShowDialog() = DialogResult.OK Then

			' If any changes have been made to the current playlist, see if the user
			' wants to save them.

			If Not PlaylistChangesSaved Then
				res = MsgBox("Your changes to playlist " & PlaylistName & " have not been saved.  Would you like to save them now?", MsgBoxStyle.Question + MsgBoxStyle.YesNoCancel)
				If res = DialogResult.Cancel Then Exit Sub
				If res = DialogResult.OK Then mnuSavePL_Click(mnuSavePL, EventArgs.Empty)
			End If

			' Open the playlist 

			OpenPlaylist(OpenFileDialog1.FileName)
		End If
		AlbumSongList.Clear()

	End Sub
	'***********************************************************************

	' The Save Playlist menu option is clicked.

	'***********************************************************************
	Private Sub mnuSavePL_Click(sender As Object, e As EventArgs) Handles mnuSavePL.Click

		' Declare variables

		Dim wx As String

		' Get the name and location to which the playlist will be saved.

		SaveFileDialog1.Title = "Save Windows Playlist"
		SaveFileDialog1.Filter = "Windows Playlists (*.wpl)|*.wpl"
		SaveFileDialog1.InitialDirectory = MusicFolder & "Playlists"
		SaveFileDialog1.AddExtension = True
		SaveFileDialog1.OverwritePrompt = True
		SaveFileDialog1.FileName = PlaylistName & ".wpl"

		' If the user said to save it, proceed.

		If SaveFileDialog1.ShowDialog() = DialogResult.OK Then

			' The playlist name will be the file name, with no extension.

			PlaylistName = Path.GetFileNameWithoutExtension(SaveFileDialog1.FileName)

			' Pass the playlist listbox to the routine which will create the contents
			' as an XML document, returned to variable wx.

			wx = CreateWindowsPlaylist(lstPlayList, PlaylistName)

			' Make sure the encoding matches a proper windows playlist, and write it out.

			Dim utf8WithoutBOM As New UTF8Encoding(False)
			My.Computer.FileSystem.WriteAllText(SaveFileDialog1.FileName, wx, False, utf8WithoutBOM)

		End If

	End Sub
	'***********************************************************************

	' The Play Music menu option is selected.

	'***********************************************************************
	Private Sub mnuOpenPlayer_Click(sender As Object, e As EventArgs) Handles mnuOpenPlayer.Click
		frmMusicPlayer.Show()
		frmMain_Resize(Me, EventArgs.Empty)
	End Sub
	'***********************************************************************

	' The Repair Playlist menu option is clicked.

	'***********************************************************************
	Private Sub mnuRepair_Click(sender As Object, e As EventArgs) Handles mnuRepair.Click

		' Declare variables

		Dim ii As Integer
		Dim jj As Integer
		Dim ItemsRepaired As Integer
		Dim ItemsNotRepaired As Integer
		Dim wx As String
		Dim xx As IReadOnlyCollection(Of String)
		Dim zx As String
		Dim parts As String()
		Dim Artist As String
		Dim Album As String
		Dim Song As String

		' Begin going through each item in the playlist, looking for ones marked as containing errors.

		If lstPlayList.Items.Count > 0 Then
			My.Computer.FileSystem.CurrentDirectory = MusicFolder & "Playlists"

			For ii = 0 To lstPlayList.Items.Count - 1

				' When we find an item flagged as containing errors, begin the repair.

				If lstPlayList.Items(ii).startswith("*") Then

					' Parse the line into artist, album and song name.

					zx = lstPlayList.Items(ii)
					parts = zx.Substring(1).Split("\")
					Artist = parts(parts.Count - 3)
					Album = parts(parts.Count - 2)
					Song = parts(parts.Count - 1)

					' Assemble a fully-qualified file name of the song, using
					' the current music folder.
					' Strip out any HTML codes used in the entry, if any
					' failed to get removed earlier (shouldn't happen).

					wx = $"..\{Artist}\{Album}\{Song}"
					wx = UnescapeXml(wx)

					' See if just changing the music folder makes the song locatable.

					If My.Computer.FileSystem.FileExists(wx) Then
						lstPlayList.Items(ii) = wx
						ItemsRepaired += 1

						' If we still did not find the song, we need to do more work.

					Else

						' A song may have an absolute path to its location in the music folder.
						' Change these to relative paths.

						If zx.IndexOf("Music\") > 0 Then zx = "..\" & zx.Substring(zx.IndexOf("Music\") + 6)

						' Songs may have incorrect track numbers prepended to them.  Strip off anything
						' before the first alphabetic character.

						For jj = 0 To Song.Length - 1
							If Song.Chars(jj) >= "A" Then
								Song = Song.Substring(jj, Song.Length - 4 - jj)

								' Now look for ANY song which matches the bare song name.
								' If we find more than one, we'll take the first only.

								Try
									xx = Directory.GetFiles(MusicFolder & Artist & "\" & Album, "*" & Song & ".*")
									If xx.Count > 0 Then wx = xx(0)

									' Any error means the song is unrepairable.

								Catch ex As Exception
									ItemsNotRepaired += 1
								End Try
								Exit For
							End If
						Next jj

						' Now do a final check to see if the song has been successfully located.

						If My.Computer.FileSystem.FileExists(wx) Then
							lstPlayList.Items(ii) = wx
							ItemsRepaired += 1
						Else
							ItemsNotRepaired += 1
						End If
					End If
				End If
			Next ii
		End If

		' Report the results.

		wx = "Playlist """ & PlaylistName & """ has been repaired." & vbCrLf & ItemsRepaired & " Item(s) were repaired."
		If ItemsNotRepaired > 0 Then wx &= vbCrLf & ItemsNotRepaired & "Item(s) were NOT repairable.  These will need to be removed and manually located and re-added."
		MsgBox(wx, MsgBoxStyle.Information, "Repair Playlist")

	End Sub
	'***********************************************************************

	' Event handler for the MouseUp event of the playlist list box.  If the 
	' right mouse button is clicked, select the current item manually, so that
	' there will be a selected item for the context list box.

	'***********************************************************************
	Private Sub lstPlayList_MouseUp(sender As Object, e As MouseEventArgs) Handles lstPlayList.MouseUp

		If e.Button = MouseButtons.Right Then
			lstPlayList.SelectedIndex = -1
			Dim index As Integer = lstPlayList.IndexFromPoint(e.Location)
			If index <> ListBox.NoMatches Then
				lstPlayList.SelectedIndex = index ' Select item before context menu appears
			End If

			' If the list box is empty, disable the play and sync menu items.

			If lstPlayList.Items.Count = 0 Then
				mnuCMPlay.Enabled = False
				mnuCMSync.Enabled = False
			Else
				mnuCMPlay.Enabled = True
				mnuCMSync.Enabled = True
			End If

		End If
	End Sub
	'***********************************************************************

	' Event handler for the playlist list box context menu "edit" menu.

	'***********************************************************************
	Private Sub mnuCMEdit_Click(sender As Object, e As EventArgs) Handles mnuCMEdit.Click

		Dim zx As String = InputBox("Enter corrected information for this song.", "Edit Playlist Item", lstPlayList.Items(lstPlayList.SelectedIndex))
		lstPlayList.Items(lstPlayList.SelectedIndex) = zx.Replace("*", "")

	End Sub
	'***********************************************************************

	' Event handlers for the cut/copy/paste/undo menu items.

	'***********************************************************************
	Private Sub mnuCut_Click(sender As Object, e As EventArgs) Handles mnuCut.Click, mnuCMCut.Click
		CutItems(lstPlayList, playlist)
	End Sub
	Private Sub mnuCopy_Click(sender As Object, e As EventArgs) Handles mnuCopy.Click, mnuCMCopy.Click
		CopyItems(lstPlayList)
	End Sub
	Private Sub mnuPaste_Click(sender As Object, e As EventArgs) Handles mnuPaste.Click, mnuCMPaste.Click
		PasteItems(lstPlayList)
	End Sub
	Private Sub mnuDelete_Click(sender As Object, e As EventArgs) Handles mnuDelete.Click, mnuCMDelete.Click
		DeleteItems(lstPlayList)
	End Sub
	Private Sub mnuUndo_Click(sender As Object, e As EventArgs) Handles mnuUndo.Click
		UndoLastOperation(lstPlayList)
	End Sub
	'**************************************************

	' The Change Music Folder Location menu is clicked.

	'**************************************************
	Private Sub mnuChangeLocation_Click(sender As Object, e As EventArgs) Handles mnuChangeLocation.Click
		frmLocateMusicFolder.ShowDialog()
	End Sub

	'**************************************************

	' The Add menu option is selected.

	'**************************************************
	Private Sub mnuCMAdd_Click(sender As Object, e As EventArgs) Handles mnuCMAddToPlaylist.Click

		picLibraryDisplay_DoubleClick(picLibraryDisplay, New EventArgs)

	End Sub

	'**************************************************

	' The Play context menu option is selected.

	'**************************************************
	Private Sub Play_Click(sender As Object, e As EventArgs) Handles mnuCMPlay.Click

		' Declare variables

		Dim ii As Integer
		Dim zx As String
		Dim sb As New StringBuilder

		' Create a songlist for the music player.

		For ii = 0 To lstPlayList.Items.Count - 1
			zx = lstPlayList.Items(ii).replace("..\", MusicFolder)
			sb.Append(zx & vbCrLf)
		Next ii

		'   Create a music player in the panel below the playlist list box.'

		MP = New MediaPlayer
		pnlMusicPlayer.Controls.Add(MP)
		MP.ListBox = lstPlayList

		' Wire up the handler.

		AddHandler MP.PlayerStop, AddressOf MP_PlayerStop

		' Add the keyboard handler

		kbdhook.Install()

		' Position the music player over the playlist list box.

		MP.Location = New Point((SplitContainer1.Panel2.Width - MP.Width) / 2, 0)
		lstPlayList.Height = SplitContainer1.Panel2.Height - MP.Height - lblHeader_0.Height
		pnlMusicPlayer.Visible = True

		' Add the songlist to the music player. It will play automatically since autostart defaults to true.

		MP.Songlist = sb.ToString

		' Disable the listbox and the Open Music Player menu options.

		lstPlayList.Enabled = False
		mnuOpenPlayer.Enabled = False
		lstPlayList.ContextMenu = Nothing
		mnuCMPlayItem.Enabled = False

	End Sub
	'***********************************************************************

	' Sub to filter the contents of a playlist under construction to
	' remove all but the best available quality files.

	'***********************************************************************
	Private Sub mnuCMChooseBest_Click(sender As Object, e As EventArgs) Handles mnuCMChooseBest.Click

		' Declare variables.

		Dim zx As String
		Dim songs(lstPlayList.Items.Count - 1) As String
		Dim filtered As List(Of String)
		lstPlayList.Items.CopyTo(songs, 0)

		filtered = FilterPreferredCopies(songs)

		lstPlayList.Items.Clear()
		lstPlayList.BeginUpdate()
		For Each zx In filtered
			lstPlayList.Items.Add(zx)
		Next zx
		lstPlayList.EndUpdate()

	End Sub
	'***********************************************************************

	' Sub to sort or shuffle a playlist.

	'***********************************************************************
	Private Sub mnuOrderBy_Click(sender As Object, e As EventArgs) Handles mnuCMSort.Click, mnuCMShuffle.Click

		' Declare variables

		Dim ii As Integer
		Dim zx As String
		Dim LBItems() As String

		' Get the items from the list box.

		If lstPlayList.Items.Count > 0 Then
			ReDim LBItems(lstPlayList.Items.Count - 1)
			For ii = 0 To lstPlayList.Items.Count - 1
				zx = lstPlayList.Items(ii).replace("*", "").replace("..\", MusicFolder)
				LBItems(ii) = zx
			Next ii

			' Either sort or shuffle them depending on the menu selected.

			If sender Is mnuCMSort Then LBItems = OrderBy(LBItems, 0) Else LBItems = OrderBy(LBItems, 1)

			' Reset the playlist.

			For ii = 0 To lstPlayList.Items.Count - 1
				lstPlayList.Items(ii) = LBItems(ii)
			Next ii
		End If

	End Sub
	'***********************************************************************

	' The View Readme menu option is selected.

	'***********************************************************************
	Private Sub mnuViewReadme_Click(sender As Object, e As EventArgs) Handles mnuViewReadme.Click

		' Create a viwer.  If the user sets the "EditMDFiles" value, using the Control Table Editor,
		' to "True", they will be able to both view and edit .md files.

		Dim f As New frmMDViewer
		Dim zx As String = My.Application.Info.DirectoryPath & "\Readme.md"
		f.LoadFile(zx)
		Me.Text = "Viewing " & Path.GetFileName(zx)

		f.EnableEditing = CBool(GetControlItem("EditMDFiles", "False"))
		f.ShowDialog()

	End Sub
	'***********************************************************************

	' The View Readme menu option is selected.

	'***********************************************************************
	Private Sub mnuViewLicense_Click(sender As Object, e As EventArgs) Handles mnuViewLicense.Click

		' Create a viwer.  If the user sets the "EditMDFiles" value, using the Control Table Editor,
		' to "True", they will be able to both view and edit .md files.

		Dim f As New frmMDViewer
		Dim zx As String = My.Application.Info.DirectoryPath & "\license.md"
		f.LoadFile(zx)
		Me.Text = "Viewing " & Path.GetFileName(zx)

		f.EnableEditing = CBool(GetControlItem("EditMDFiles", "False"))
		f.ShowDialog()

	End Sub

	'***********************************************************************

	' The find album art context menu item is clicked.

	'***********************************************************************
	Private Sub mnuCMFindArt_Click(sender As Object, e As EventArgs) Handles mnuCMFindArt.Click

		' Declare variables.

		Dim ArtistName As String = DisplayLines.SelectedLine.ArtistName
		Dim AlbumName As String = DisplayLines.SelectedLine.AlbumName
		Dim AlbumArt As Image
		Dim Command As SqlCommand
		Dim dl As DisplayLine
		Dim zx As String

		' See if the album art already exists in the folder.

		zx = GetExistingAlbumArt(DisplayLines.SelectedLine.ArtistName, DisplayLines.SelectedLine.AlbumName)
		If zx <> "" Then

			' Update the album entry in the library to refer to the new art.

			Command = New SqlCommand("UPDATE Library Set AlbumImage='" & zx.Replace("'", "''") & "' WHERE ArtistName='" & DisplayLines.SelectedLine.ArtistName.Replace("'", "''") & "' AND AlbumName='" & DisplayLines.SelectedLine.AlbumName.Replace("'", "''") & "'", DB)
			Command.ExecuteNonQuery()

			' Rebuild the dataset

			LibraryDS.Clear()
			LibraryDA.Fill(LibraryDS, "Table")
			LibraryTable = SelectByView(LibraryDS.Tables("Table"))

			' Update the current display line with the location of the small album art.

			dl = DisplayLines.SelectedLine
			dl.ImageFile = zx
			DisplayLines.SelectedLine = dl

			AlbumArt = Image.FromFile(zx)
			Using g As Graphics = picLibraryDisplay.CreateGraphics
				g.DrawImage(AlbumArt, DisplayLines.SelectedLine.ImageBounds)
			End Using
		Else
			' Get the album art from the internet.

			Try
				lblStatus.Text = "Searching for album art."
				Application.DoEvents()
				AlbumArt = GetAlbumArt(DisplayLines.SelectedLine.ArtistName, DisplayLines.SelectedLine.AlbumName)
				lblStatus.Text = ""
				Application.DoEvents()

				' If we successfully retrieved the album art, then save it to the album folder.

				If AlbumArt IsNot Nothing Then

					' Save the large image.

					Dim imageGuid As String = AlbumArt.FrameDimensionsList(0).ToString
					' Save the original image as AlbumArtLarge.jpg
					Using LargeArt As New Bitmap(AlbumArt, New Size(250, 250))
						LargeArt.Save(MusicFolder & DisplayLines.SelectedLine.ArtistName & "\" & DisplayLines.SelectedLine.AlbumName & "\AlbumArt_{" & imageGuid & "}_Large.jpg", ImageFormat.Jpeg)
					End Using

					' Resize to 48x48 and save the small image.

					Using smallArt As New Bitmap(AlbumArt, New Size(48, 48))
						smallArt.Save(MusicFolder & DisplayLines.SelectedLine.ArtistName & "\" & DisplayLines.SelectedLine.AlbumName & "\AlbumArt_{" & imageGuid & "}_Small.jpg", ImageFormat.Jpeg)

						' Update the current display line with the location of the small album art.

						DisplayLines.SelectedLine.ImageFile = MusicFolder & DisplayLines.SelectedLine.ArtistName & "\" & DisplayLines.SelectedLine.AlbumName & "\AlbumArt_{" & imageGuid & "}_Small.jpg"

						' Redraw the album image to show the new art.

						Using g As Graphics = picLibraryDisplay.CreateGraphics
							g.DrawImage(smallArt, DisplayLines.SelectedLine.ImageBounds)
						End Using
					End Using

					' Update the album entry in the library to refer to the new art.

					Command = New SqlCommand("UPDATE Library Set AlbumImage='" & DisplayLines.SelectedLine.ImageFile & "' WHERE ArtistName='" & DisplayLines.SelectedLine.ArtistName & "' AND AlbumName='" & DisplayLines.SelectedLine.AlbumName & "' AND (SongName='' OR SongName IS NULL", DB)
					Command.ExecuteNonQuery()

					' Rebuild the dataset

					LibraryDS.Clear()
					LibraryDA.Fill(LibraryDS, "Table")
					LibraryTable = SelectByView(LibraryDS.Tables("Table"))
				End If
			Catch ex As Exception
			End Try
		End If
	End Sub
	'**********************************************************

	' Event handler for the context menu Play (artist, album, song)
	' menu click event.

	'**********************************************************
	Private Sub mnuCMPlayItem_Click(sender As Object, e As EventArgs) Handles mnuCMPlayItem.Click

		' Declare variables

		Dim mnu As ToolStripMenuItem = DirectCast(sender, ToolStripMenuItem)
		Dim zx As String = mnu.Tag
		Dim parts = zx.Split(":")
		Dim Songs As IReadOnlyCollection(Of String) = Nothing
		Dim Filtered As List(Of String)
		Dim song As String
		Dim sb As New StringBuilder

		' Determine whether we are going to play an artist, an album or a song.

		Select Case parts(0)
			Case 0 ' "Artist"
				Songs = My.Computer.FileSystem.GetFiles(MusicFolder & parts(2), FileIO.SearchOption.SearchAllSubDirectories, ExtensionPrecedenceWildcards)
			Case 1 ' "Album"
				Songs = My.Computer.FileSystem.GetFiles(MusicFolder & parts(2) & "\" & parts(4), FileIO.SearchOption.SearchAllSubDirectories, ExtensionPrecedenceWildcards)
			Case 2 ' "Song"
				Songs = Directory.GetFiles(MusicFolder & parts(2) & "\" & parts(4), parts(6))
		End Select

		' Disable the "Play" option in the context menu.

		mnuCMPlayItem.Enabled = False

		' Create a list of the songs, filtered to remove duplicates in multiple
		' musical formats.

		Filtered = FilterPreferredCopies(Songs)

		' Assemble a song list of the selected songs.

		If Not Songs Is Nothing Then
			For Each song In Filtered
				sb.Append(song & vbCrLf)
			Next song

			'   Create a music player in the panel below the playlist list box.'

			MP = New MediaPlayer
			MP.Repeat = False
			pnlMusicPlayer.Controls.Add(MP)

			' Make the listbox invisible and the display and music control panels
			' visible.

			lstPlayList.Visible = False
			lblHeader_0.Visible = False
			lblHeader_1.Visible = False
			pnlMusicPlayer.Visible = True
			pnlDisplay.Visible = True

			' Cause the music player to resize.

			frmMain_Resize(Me, New EventArgs)

			' Wire up the handler.

			AddHandler MP.PlayerStop, AddressOf MP_PlayerStop
			AddHandler MP.SongChanged, AddressOf MP_SongChanged
			AddHandler MP.PlayStateChanged, AddressOf MP_PlayStateChanged

			' Set the location of the music control to the center of its owning panel.

			MP.Location = New Point((SplitContainer1.Panel2.Width - MP.Width) / 2, 0)

			' Set the songlist to be played.

			MP.Songlist = sb.ToString ' This will start the player automatically since autostart defaults to true

			' Disable the open player menu option.

			mnuOpenPlayer.Enabled = False

			' Install a keyboard hook to capture media action keys.

			kbdhook.Install()

			' Start the elapsed time timer.

			timElapsedTime.Enabled = True

		End If
	End Sub
	'**********************************************************

	' Event handler for the context menu Use Compatibility
	' artist, album, song menu click event.

	'**********************************************************
	Private Sub mnuCMCompatibility_CheckedChanged(sender As Object, e As EventArgs) Handles mnuCMCompatibility.CheckedChanged

		Dim mnu As ToolStripMenuItem = DirectCast(sender, ToolStripMenuItem)
		Dim zx As String = mnu.Tag
		Dim parts = zx.Split(":")
		Dim Songs As IReadOnlyCollection(Of String) = Nothing
		Dim song As String
		Dim sb As New StringBuilder
		Dim Command As SqlCommand

		' Determine whether we are going to set compatibility for an artist, an album or a song.

		Select Case parts.Count
			Case 1 ' "Artist"
				Songs = My.Computer.FileSystem.GetFiles(MusicFolder & parts(0), FileIO.SearchOption.SearchAllSubDirectories, ExtensionPrecedenceWildcards)
				Try
					Command = New SqlCommand("UPDATE [Library] SET Fallback =" & Math.Abs(CInt(mnu.Checked)) & " WHERE ArtistName='" & parts(0) & "'", DB)
					Command.ExecuteNonQuery()
				Catch ex As Exception
					MsgBox("Failed to set/unset compatibility mode.", MsgBoxStyle.Information, "Change Compatibility Mode")
				End Try
			Case 2 ' "Album"
				Songs = My.Computer.FileSystem.GetFiles(MusicFolder & parts(0) & "\" & parts(1), FileIO.SearchOption.SearchAllSubDirectories, ExtensionPrecedenceWildcards)
				Try
					Command = New SqlCommand("UPDATE [Library] SET Fallback =" & Math.Abs(CInt(mnu.Checked)) & " WHERE ArtistName='" & parts(0) & "' AND AlbumName='" & parts(1) & "'", DB)
					Command.ExecuteNonQuery()
				Catch ex As Exception
					MsgBox("Failed to set/unset compatibility mode.", MsgBoxStyle.Information, "Change Compatibility Mode")
				End Try
			Case 3 ' "Song"
				Songs = Directory.GetFiles(MusicFolder & parts(0) & "\" & parts(1), parts(2))
				Try
					Command = New SqlCommand("UPDATE [Library] SET Fallback =" & Math.Abs(CInt(mnu.Checked)) & " WHERE ArtistName='" & parts(0) & "' AND AlbumName='" & parts(1) & "' AND SongName='" & parts(2) & "'", DB)
					Command.ExecuteNonQuery()
				Catch ex As Exception
					MsgBox("Failed to set/unset compatibility mode.", MsgBoxStyle.Information, "Change Compatibility Mode")
				End Try
		End Select


		' Get the current FallbackList

		zx = GetControlItem("FallbackList", "")

		' Add the selected songs to the list.

		If Not Songs Is Nothing Then
			If zx <> "" Then sb.Append(zx)
			For Each song In Songs
				sb.Append(song & vbCrLf)
			Next song

			' Save back the new list

			PutControlItem("FallbackList", sb.ToString)
		End If


	End Sub
	'**********************************************************

	' Event handler for the context menu Paste Album Art menu
	' click event.

	'**********************************************************
	Private Sub mnuCMPasteAlbumArt_Click(sender As Object, e As EventArgs) Handles mnuCMPasteAlbumArt.Click


		' Declare variables.

		Dim ArtistName As String = DisplayLines.SelectedLine.ArtistName
		Dim AlbumName As String = DisplayLines.SelectedLine.AlbumName
		Dim AlbumArt As Image

		' If the clipboard does not contain an image, do nothing

		If Not Clipboard.ContainsImage Then Exit Sub

		' Get the artist and album from the selected line.

		Dim files = Directory.GetFiles($"{MusicFolder}{ArtistName}\{AlbumName}\", "*large.jpg")

		' Warn the user before overwriting existing art.

		If files.Length > 0 Then
			If MsgBox("This album already contains album art.  Do you want to override it with the pasted image?  This operation CANNOT be undone.", MsgBoxStyle.YesNo, "Replace Existing Album Art") = MsgBoxResult.No Then Exit Sub
		End If

		' Get the album art from the clipboard and save it.

		AlbumArt = Clipboard.GetImage
		AlbumArt.Save($"{MusicFolder}{ArtistName}\{AlbumName}\AlbumArt_Large.jpg")
		Using smallArt As New Bitmap(AlbumArt, New Size(48, 48))
			smallArt.Save($"{MusicFolder}{ArtistName}\{AlbumName}\AlbumArt_Small.jpg", ImageFormat.Jpeg)

			' Update the current display line with the location of the small album art.

			DisplayLines.SelectedLine.ImageFile = $"{MusicFolder}{ArtistName}\{AlbumName}\AlbumArt_Small.jpg"

			' Redraw the album image to show the new art.

			Using g As Graphics = picLibraryDisplay.CreateGraphics
				g.DrawImage(smallArt, DisplayLines.SelectedLine.ImageBounds)
			End Using
		End Using

	End Sub
	'**********************************************************

	' The Sync to Mobile Device menu option is clicked.

	'**********************************************************
	Private Sub mnuCMSync_Click(sender As Object, e As EventArgs) Handles mnuCMSync.Click
		frmSync.Show()
	End Sub

	'**********************************************************

	' Event handler for the status label Text Changed event.

	'**********************************************************
	Private Sub lblStatus_TextChanged(sender As Object, e As EventArgs) Handles lblStatus.TextChanged
		If lblStatus.Text <> "" Then timClearMessage.Enabled = True
	End Sub
	'**********************************************************

	' Event handler for the ClearMessage time tick event.

	'**********************************************************
	Private Sub timClearMessage_Tick(sender As Object, e As EventArgs) Handles timClearMessage.Tick
		timClearMessage.Enabled = False
		lblStatus.Text = ""
	End Sub
	'**********************************************************

	' Event handler for the elapsed time tick event.

	'**********************************************************
	Private Sub timElapsedTime_Tick(sender As Object, e As EventArgs) Handles timElapsedTime.Tick

		' Declare variables

		Dim ii As Integer

		ElapsedTime += 1
		Dim minutes As Integer = ElapsedTime \ 60
		Dim seconds As Integer = ElapsedTime Mod 60
		lblElapsedTime.Text = minutes.ToString("00") & ": " & seconds.ToString("00")

		If MP.Duration > 0 Then ii = ElapsedTime / MP.Duration / 60 * 100 Else ii = 0
		If ii > 100 Then ii = 100
		ProgressBar1.Value = ii
		Application.DoEvents()

		' Reset the selected song it it's been changed in the listbox.

		If Not lstPlayList Is Nothing AndAlso lstPlayList.Items.Count > 0 Then
			If MP.CurrentSong.Index <> lstPlayList.SelectedIndex Then lstPlayList.SelectedIndex = MP.CurrentSong.Index
		End If


	End Sub
	'**********************************************************

	' Event handler for the player stop event.

	'**********************************************************
	Private Sub MP_PlayerStop()

		' Make the panel holding the music player invisible and dispose of the player

		pnlDisplay.Visible = False
		pnlMusicPlayer.Visible = False
		lstPlayList.Enabled = True
		lblHeader_0.Visible = True
		lblHeader_1.Visible = True

		' Call the resize event to reshape everything.

		frmMain_Resize(Me, EventArgs.Empty)

		' Execute a full stop.

		MP.PlayStop()

		' Remove the handler.

		RemoveHandler MP.PlayerStop, AddressOf MP_PlayerStop
		RemoveHandler MP.SongChanged, AddressOf MP_SongChanged
		RemoveHandler MP.PlayStateChanged, AddressOf MP_PlayStateChanged

		' Uninstall the keyboard hook.

		kbdhook.Uninstall()

		' Enable the Open Music Player menu option again.

		mnuOpenPlayer.Enabled = True

		' Enable the "Play" option in the context menu.

		mnuCMPlay.Enabled = False
		mnuCMPlayItem.Enabled = True
		lstPlayList.ContextMenuStrip = ContextMenuStrip2

		' Disable the elapsed time timer

		timElapsedTime.Enabled = False

		' Dispose of the music player.

		MP.Dispose()
		MP = Nothing

	End Sub
	'***********************************************************************

	' Event Handler for the player SongChanged event.

	'***********************************************************************
	Private Sub MP_SongChanged()

		' Get the duration of the new song and display it below the album art.

		Dim minutes As Integer = Fix(MP.Duration)
		Dim seconds As Integer = (MP.Duration - minutes) * 60
		lblDuration.Text = minutes.ToString("00") & ":" & seconds.ToString("00")

		' Reset the elapsed time and start the timer.

		ElapsedTime = 0
		If MP.Playstate = SiriusAudio.SEP_Playstate.SEP_Playing Or MP.Playstate = SiriusAudio.SEP_Playstate.SEP_PlayingExternal Then timElapsedTime.Enabled = True

		' Get the new album art and force it to be displayed.

		mAlbumArt = MP.AlbumArt
		picAlbumArt.Invalidate()
		lblAlbum.Text = MP.Album
		lblArtist.Text = MP.Artist

	End Sub
	'***********************************************************************

	' Event handler for the PlayStateChanged event.

	'***********************************************************************
	Private Sub MP_PlayStateChanged(NewState As Integer)

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
				timElapsedTime.Enabled = False
			Case SiriusAudio.SEP_Playstate.SEP_Playing, SiriusAudio.SEP_Playstate.SEP_PlayingExternal
				timElapsedTime.Enabled = True
			Case SiriusAudio.SEP_Playstate.SEP_PlaylistEnded
				MP_PlayerStop()
		End Select


	End Sub
	'**********************************************************

	' Sub to draw one line and return the ending y position.

	'**********************************************************
	Private Function DrawOneLine(dl As DisplayLine, g As Graphics) As Integer

		' Declare variables

		Dim y As Integer
		Dim AlbumImage As Bitmap = Nothing
		Dim bH As New SolidBrush(Color.FromArgb(128, Color.DarkGoldenrod))
		Dim bB1 As New SolidBrush(picLibraryDisplay.BackColor)

		' Begin drawing the specified DisplayLine object to the picturebox.

		Try

			' Clear the background.

			g.FillRectangle(Brushes.White, dl.Bounds)

			' Draw the background.

			If dl.Selected Then
				g.FillRectangle(bH, dl.Bounds) ' Highlighted if selected
			Else
				g.FillRectangle(bB1, dl.Bounds) ' Normal background if not selected.
			End If

			' Draw the rest of the line based on the type of line

			Select Case dl.ItemType
				Case MusicItemType.Artist ' The artist name is drawn in a larger font.
					g.DrawString(dl.ArtistName, fArtist, Brushes.DeepSkyBlue, dl.Bounds)
					y += dl.Bounds.Y + dl.Bounds.Height

				Case MusicItemType.Album

					' If we have an album image, draw it.
					If dl.ImageFile <> "" Then
						Try
							Using tempImage As Image = Image.FromFile(dl.ImageFile)
								AlbumImage = New Bitmap(tempImage)
								' tempImage is automatically disposed when exiting the Using block
								g.DrawImage(AlbumImage, dl.ImageBounds)
							End Using

							' If there is no album image, or if we get an error while trying to
							' draw it, display a default image.

						Catch ex As Exception
							g.DrawImage(GetNoAlbumArtImage, dl.ImageBounds)
						End Try
					Else
						g.DrawImage(GetNoAlbumArtImage, dl.ImageBounds)
					End If

					' Draw the name of the album to the right of the image and
					' centered in the height of it.

					g.DrawString(dl.AlbumName, fAlbum, Brushes.Blue, dl.Bounds)
					y += dl.Bounds.Y + dl.Bounds.Height

					' For a song, simply draw the song name in normal font.

				Case MusicItemType.Song
					g.DrawString(SanitizeSongName(dl.SongName), fSong, Brushes.Black, dl.Bounds)
					y += dl.Bounds.Y + dl.Bounds.Height
			End Select
		Catch ex As Exception
			MsgBox("Error in DrawOneLine: " & ex.Message, MsgBoxStyle.Information, "Draw One Line")
		End Try

		' Dispose of objects we created

		bH.Dispose()
		bB1.Dispose()
		If AlbumImage IsNot Nothing Then AlbumImage.Dispose()

		Return y

	End Function
	'**********************************************************
	'
	' Sub to move down a certain number of lines.
	'
	'**********************************************************
	Private Sub MoveDown(ByRef LinesDown As Integer)

		' Declare variables

		Dim xx As Integer

		Try
			' Position to the last record row number using the bookmark.

			xx = BottomRowIndex

			' Add the number of lines we are to move to the row number.

			xx += LinesDown

			' If the new row number is larger than the largest row number, 
			' set it to the largest row number.

			If xx >= LibraryTable.Rows.Count Then xx = LibraryTable.Rows.Count - 1

			' Now set the current top of the list to the row number minus MAXLINES.

			TopRowIndex = xx

			' Now redraw the list.

			picLibraryDisplay.Invalidate()
		Catch e As Exception
			MsgBox("MoveDown failed." & vbCrLf & e.Message, MsgBoxStyle.Exclamation, "Scroll Down")
		End Try

	End Sub

	'**********************************************************
	'
	' Sub to move up a specified number of lines.
	'
	'**********************************************************
	Private Sub MoveUp(ByRef LinesUp As Integer)

		' Declare variables

		Dim xx As Integer

		Try
			' Position to the first record row number using the bookmark.

			xx = TopRowIndex

			' Clear the display data

			' Subtract the number of lines we are to move to the row number.

			xx -= LinesUp

			' If the new row number is less than zero, set it to zero.

			If xx < 0 Then xx = 0

			' Now set the current top of the list to the row number.

			TopRowIndex = xx

			' Now redraw the list.

			picLibraryDisplay.Invalidate()


		Catch e As Exception
			MsgBox("MoveUp failed." & vbCrLf & e.Message, MsgBoxStyle.Exclamation, "Scroll Up")
		End Try
	End Sub

	'***********************************************************************

	' Sub to open, parse and fill the list box with the
	' contents of a playlist.

	'***********************************************************************

	Private Sub OpenPlaylist(Playlist As String)

		' Declare variables

		Dim wx As String
		Dim zx As String

		' Determine the local drive for the playlist, as it may have been opened on a remote machine.

		If Playlist.StartsWith("\\") Then zx = Path.GetPathRoot(Playlist) & "\" Else zx = MusicFolder

		' Begin reading in the playlist and adding the items to the list box.

		lstPlayList.Items.Clear()
		AlbumSongList.Clear()
		AlbumSongList = ParseWindowsPlaylist(Playlist)
		PlaylistName = Path.GetFileNameWithoutExtension(Playlist)
		lstPlayList.BeginUpdate()
		My.Computer.FileSystem.CurrentDirectory = zx
		For Each Song In AlbumSongList
			wx = Song.AlbumFolder

			' Before adding a song to the playlist list box, see if it can be found.
			' Any song not found will be flagged with a leading asterisk, which will
			' cause the list box to display the line in red.

			If Not My.Computer.FileSystem.FileExists(wx.Replace("..\", zx)) Then wx = "*" Else wx = ""
			lstPlayList.Items.Add(wx & Song.AlbumFolder)
		Next Song
		lstPlayList.EndUpdate()

	End Sub
	'***********************************************************************

	' Sub to refresh the DisplayLines

	'***********************************************************************
	Public Sub RefreshDisplay()

		Using g As Graphics = picLibraryDisplay.CreateGraphics
			For Each dl In DisplayLines.DisplayLines
				DrawOneLine(dl, g)
			Next dl
		End Using


	End Sub

	'***********************************************************************

	' Function to create a windows playlist.  The list is created as an
	' XML document, from the contents of the playlist listbox, which
	' contains a list of fully-qualified song names.  It returns the
	' text of the new file as a string.

	'***********************************************************************
	Public Function CreateWindowsPlaylist(ListBox1 As ListBox, playlistTitle As String) As String

		' Declare variables

		Dim jj As Integer
		Dim zx As String
		Dim sb As New StringBuilder()

		' Header
		sb.AppendLine("<?wpl version=""1.0""?>")
		sb.AppendLine("<smil>")
		sb.AppendLine("  <head>")
		'sb.AppendLine("      <meta name = ""Generator"" content=""Microsoft Windows Media Player -- 12.0.26100.3624""/>")
		sb.AppendLine("      <meta name = ""Generator"" content=""Sirius EasyPlayer -- 1.0.0.0""/>")
		sb.AppendLine("      <meta name = ""ItemCount"" content=""" & ListBox1.Items.Count & """/>")
		sb.AppendLine("    <title>" & EscapeXml(playlistTitle) & "</title>")
		sb.AppendLine("  </head>")
		sb.AppendLine("  <body>")
		sb.AppendLine("    <seq>")

		' Iterate through songs
		If ListBox1.Items.Count > 0 Then

			For jj = 0 To ListBox1.Items.Count - 1
				zx = ListBox1.Items(jj)
				Dim fixed As String = Regex.Replace(zx, "^[A-Za-z]:\\Music\\", "..\")
				zx = EscapeXml(fixed)
				sb.AppendLine($"      <media src=""{zx}""/>")
			Next jj
		End If

		' Footer
		sb.AppendLine("    </seq>")
		sb.AppendLine("  </body>")
		sb.AppendLine("</smil>")

		Return sb.ToString()
	End Function

	'***********************************************************************

	' Function to tell if an artist, album or individual song has compatibility
	' mode set.

	'***********************************************************************
	Private Function IsCompatibilitySet(info As String) As Boolean

		' Declare variables

		Dim parts() As String = info.Split(":")
		Dim Cmd As SqlCommand
		Dim ds As New DataSet
		Dim result As Boolean
		' Determine if we're looking for an artist, an album or an individual song.

		Select Case parts.Count
			Case 1
				Try
					Cmd = New SqlCommand("SELECT * From [Library] WHERE ArtistName='" & parts(0) & "' AND (AlbumName='' OR AlbumName IS NULL) AND Fallback=1", DB)
					LibraryDA.SelectCommand = Cmd
					LibraryDA.Fill(ds, "Table")
					If ds.Tables("Table").Rows.Count > 0 Then result = True Else result = False
				Catch ex As Exception
				End Try

			Case 2
				Try
					Cmd = New SqlCommand("SELECT * From [Library] WHERE ArtistName='" & parts(0) & "' AND AlbumName='" & parts(1) & "' AND (SongName='' OR SongName IS NULL) AND Fallback=1", DB)
					LibraryDA.SelectCommand = Cmd
					LibraryDA.Fill(ds, "Table")
					If ds.Tables("Table").Rows.Count > 0 Then result = True Else result = False
				Catch ex As Exception
				End Try
			Case 3
				Try
					Cmd = New SqlCommand("SELECT * From [Library] WHERE ArtistName='" & parts(0) & "' AND AlbumName='" & parts(1) & "' AND SongName='" & parts(2) & "' AND Fallback=1", DB)
					LibraryDA.SelectCommand = Cmd
					LibraryDA.Fill(ds, "Table")
					If ds.Tables("Table").Rows.Count > 0 Then result = True Else result = False
				Catch ex As Exception
				End Try

		End Select


		' Restore the proper select statement to the data adapter

		LibraryDA.SelectCommand = LibrarySelectCommand()

		Return result

	End Function
	'***********************************************************************

	' Function to optionally filter the library table to show only the
	' best versions of songs of which multiple versions exist.

	'***********************************************************************
	Private Function SelectByView(fullTable As DataTable) As DataTable

		' If the current view is "View All", just return the full table.

		If mnuViewAll.Checked Then Return fullTable

		Dim LastArtist As String = ""
		Dim LastAlbum As String = ""
		Dim SongName As String
		Dim ext As String
		Dim dr As DataRow

		Dim bestTable As DataTable = fullTable.Clone()

		For Each row As DataRow In fullTable.Rows
			SongName = row("SongName")
			ext = Path.GetExtension(SongName)

			' clone the current row.
			dr = bestTable.NewRow()
			dr.ItemArray = row.ItemArray.Clone()

			' Be sure to include artist and album entries.  These have
			' no song name.


			If row("SongName").ToString = "" Then
				bestTable.Rows.Add(dr)

				' See if the last row added contains the name without extension of the 
				' current row's song name.

			ElseIf Not (bestTable.Rows(bestTable.Rows.Count - 1)("SongName")).ToString.ToLower.Contains(Path.GetFileNameWithoutExtension(SongName.tolower)) Then
				' First time seeing this song.
				bestTable.Rows.Add(dr)

				' If we find another song with the same name but different extenstion, see if it's 
				' a better version.
			Else
				' Compare extensions and keep the better one.
				If ExtensionRank(ext) < ExtensionRank(Path.GetExtension(bestTable.Rows(bestTable.Rows.Count - 1)("SongName").ToString)) Then
					bestTable.Rows(bestTable.Rows.Count - 1).Delete()
					bestTable.Rows.Add(dr)
				End If
			End If
		Next row


		Return bestTable
	End Function
	'***********************************************************************

	' Function to return the precendence order for a file extension.

	'***********************************************************************
	Private Function ExtensionRank(ext As String) As Integer

		Dim ii As Integer
		Dim ExtList() As String = ExtensionPrecedence
		For ii = 0 To ExtList.Count - 1
			If ext.ToLower = ExtList(ii) Then Return ii + 1
		Next ii

		Return 99
	End Function


	'***********************************************************************

	' Sub to perform a "cut" of a selected song or songs from the playlist
	' listbox.  This routine was created by Microfsoft Copilot.

	'***********************************************************************
	Private Sub CutItems(listbox As ListBox, playlist As List(Of String))

		' Declare variables.

		Dim i As Integer
		Dim cutItems As New List(Of String)
		Dim cutIndices As New List(Of Integer)

		' Store items and their original indices BEFORE removing them
		If listbox.SelectedIndices.Count > 0 Then
			For Each Index In listbox.SelectedIndices.Cast(Of Integer)
				cutItems.Add(listbox.Items(Index).ToString()) ' Store item
				cutIndices.Add(Index) ' Store index
			Next Index
		ElseIf listbox.SelectedIndex >= 0 Then
			Dim index As Integer = listbox.SelectedIndex
			cutItems.Add(listbox.Items(index).ToString())
			cutIndices.Add(index)
		End If

		' Remove items AFTER storing them
		For i = cutIndices.Count - 1 To 0 Step -1 ' Remove safely
			listbox.Items.RemoveAt(cutIndices(i))
		Next i

		clipboardList = cutItems ' Store cut items in clipboard

		' Push the cut action to the undo stack, saving both items and indices
		undoStack.Push(New UndoAction With {.ActionType = "Cut", .Items = cutItems, .InsertedIndices = cutIndices})
	End Sub
	'***********************************************************************

	' Sub to perform a "copy" of a selected song or songs from the playlist
	' listbox.  This routine was created by Microfsoft Copilot.

	'***********************************************************************
	Private Sub CopyItems(listbox As ListBox)

		clipboardList.Clear()

		' Ensure selections exist before copying
		If listbox.SelectedIndices.Count > 0 Then
			clipboardList = listbox.SelectedIndices.Cast(Of Integer).Select(Function(i) listbox.Items(i).ToString()).ToList()
		ElseIf listbox.SelectedIndex >= 0 Then
			clipboardList.Add(listbox.Items(listbox.SelectedIndex).ToString())
		End If

	End Sub
	'***********************************************************************

	' Sub to paste a selected song or songs from the editing clipboard.
	' This routine was created by Microfsoft Copilot.

	'***********************************************************************
	Private Sub PasteItems(listbox As ListBox)

		If clipboardList.Count = 0 Then Exit Sub ' No items to paste

		' Declare variables.

		Dim i As Integer
		Dim insertedItems As New List(Of String)
		Dim insertedIndices As New List(Of Integer)
		Dim insertIndex As Integer = If(listbox.SelectedIndex >= 0, listbox.SelectedIndex, listbox.Items.Count)

		' Paste items **in reverse order** to maintain proper sequence
		For i = clipboardList.Count - 1 To 0 Step -1
			listbox.Items.Insert(insertIndex, clipboardList(i))
			insertedItems.Add(clipboardList(i))
			insertedIndices.Add(insertIndex)
		Next i

		' Push paste operation to undo stack
		undoStack.Push(New UndoAction With {.ActionType = "Paste", .Items = insertedItems, .InsertedIndices = insertedIndices})
	End Sub
	'***********************************************************************

	' Sub to delete a selected song or songs from the playlist listbox.
	' This routine maintains undo functionality for restoration.

	'***********************************************************************
	Private Sub DeleteItems(listbox As ListBox)

		' Declare variables.
		Dim deletedItems As New List(Of String)
		Dim deletedIndices As New List(Of Integer)

		' Store items and their indices BEFORE deleting them.
		If listbox.SelectedIndices.Count > 0 Then
			For Each index In listbox.SelectedIndices.Cast(Of Integer)
				deletedItems.Add(listbox.Items(index).ToString()) ' Store item
				deletedIndices.Add(index) ' Store index
			Next index
		ElseIf listbox.SelectedIndex >= 0 Then
			Dim index As Integer = listbox.SelectedIndex
			deletedItems.Add(listbox.Items(index).ToString())
			deletedIndices.Add(index)
		End If

		' Remove items AFTER storing them.
		For i As Integer = deletedIndices.Count - 1 To 0 Step -1 ' Remove safely
			listbox.Items.RemoveAt(deletedIndices(i))
		Next i

		' Push the delete action to the undo stack for recovery.
		undoStack.Push(New UndoAction With {.ActionType = "Delete", .Items = deletedItems, .InsertedIndices = deletedIndices})

	End Sub
	'***********************************************************************

	' Sub to undo recent cut or paste operations.
	' This routine was created by Microfsoft Copilot.

	'***********************************************************************
	Private Sub UndoLastOperation(listbox As ListBox)

		If undoStack.Count = 0 Then Exit Sub

		' Declare variables.

		Dim i As Integer
		Dim lastOperation = undoStack.Pop()

		' Determine what the last action was, that we need to undo.

		Select Case lastOperation.ActionType
			Case "Cut"
				' Restore cut items to their original indices
				If lastOperation.InsertedIndices IsNot Nothing Then
					For i = 0 To lastOperation.InsertedIndices.Count - 1
						listbox.Items.Insert(lastOperation.InsertedIndices(i), lastOperation.Items(i))
					Next i
				End If

			Case "Paste"
				' Remove pasted items at recorded positions
				If lastOperation.InsertedIndices IsNot Nothing Then
					For i = lastOperation.InsertedIndices.Count - 1 To 0 Step -1
						If lastOperation.InsertedIndices(i) < listbox.Items.Count Then
							listbox.Items.RemoveAt(lastOperation.InsertedIndices(i))
						End If
					Next
				End If

				' **Restore previous cut operation to keep it valid**
				If undoStack.Count > 0 AndAlso undoStack.Peek().ActionType = "Cut" Then
					Dim cutOperation = undoStack.Pop()
					undoStack.Push(cutOperation) ' Re-add cut operation
				End If

			Case "Delete"
				' Restore deleted items to their original indices.
				If lastOperation.InsertedIndices IsNot Nothing Then
					For i = 0 To lastOperation.InsertedIndices.Count - 1
						listbox.Items.Insert(lastOperation.InsertedIndices(i), lastOperation.Items(i))
					Next i
				End If

			Case "Copy"
				' Ignore copy—it doesn't alter the list
		End Select
	End Sub
	'***********************************************************************
	' Function to take a fully-qualified song name and check to see if that
	' song exists in a better version (.flac over .mp3, for example).  If one
	' does, this function returns that name.  Otherwise it returns the original
	' version unchanged.
	'***********************************************************************
	Public Function GetBestVersionOfSong(fullPath As String) As String

		If String.IsNullOrWhiteSpace(fullPath) OrElse Not File.Exists(fullPath) Then
			Return fullPath
		End If

		Dim folder = Path.GetDirectoryName(fullPath)
		Dim baseName = Path.GetFileNameWithoutExtension(fullPath)

		' Find all files in the folder with the same base name
		Dim candidates = Directory.EnumerateFiles(folder).
	   Where(Function(f) String.Equals(
			   Path.GetFileNameWithoutExtension(f),
			   baseName,
			   StringComparison.OrdinalIgnoreCase)).
	   ToList()

		If candidates.Count = 0 Then
			' Should never happen, but fall back safely
			Return fullPath
		End If

		' Pick the best version based on extension precedence
		Dim best = candidates.
	   OrderBy(Function(f)
				 Dim ext = Path.GetExtension(f).ToLowerInvariant()
				 Dim idx = Array.IndexOf(ExtensionPrecedence(), ext)
				 If idx = -1 Then idx = Integer.MaxValue
				 Return idx
			 End Function).
	   First()

		Return best

	End Function
	'***********************************************************************

	' Function to take a list of songs returned from a search of an album
	' and filter out duplicates, in order of precedence.

	'***********************************************************************
	Public Function FilterPreferredCopies(files As IReadOnlyCollection(Of String)) As List(Of String)

		' Group by base filename (no extension)
		Dim groups = files.
	   GroupBy(Function(f) Path.GetFileNameWithoutExtension(f),
			 StringComparer.OrdinalIgnoreCase)

		Dim result As New List(Of String)

		For Each g In groups
			' For each group, pick the file with the highest-precedence extension
			Dim chosen = g.
		  OrderBy(Function(f)
					Dim ext = Path.GetExtension(f).ToLowerInvariant()
					Dim idx = Array.IndexOf(ExtensionPrecedence, ext)
					If idx = -1 Then idx = Integer.MaxValue ' unknown extension = lowest priority
					Return idx
				End Function).
		  First()

			result.Add(chosen)
		Next g

		' Sort final list alphabetically for deterministic playback
		result.Sort(StringComparer.OrdinalIgnoreCase)

		Return result
	End Function
	'***********************************************************************

	' Overload to the FilterPreferredCopies function, which will accept
	' a simple string array.

	'***********************************************************************
	Public Function FilterPreferredCopies(files As String()) As List(Of String)
		Return FilterPreferredCopies(CType(files, IReadOnlyCollection(Of String)))
	End Function
	'***********************************************************************

	' Sub to sort (by artist/album/song) or shuffle a playlist.

	'***********************************************************************
	Public Function OrderBy(items As String(), mode As Integer) As String()

		If items Is Nothing OrElse items.Length <= 1 Then Return items

		Select Case mode
			Case 0
				Array.Sort(items, StringComparer.OrdinalIgnoreCase)

			Case 1
				Dim rng As New Random()
				For i As Integer = items.Length - 1 To 1 Step -1
					Dim j As Integer = rng.Next(i + 1)
					Dim temp As String = items(i)
					items(i) = items(j)
					items(j) = temp
				Next

			Case Else
				' Unknown mode → do nothing
		End Select

		Return items

	End Function
	'***********************************************************************

	' Function to verify the playlist folder is online and available.

	'***********************************************************************
	Public Function WaitForMusicFolder(MusicFolder As String, timeoutSeconds As Integer) As Boolean

		' Declare variables.

		Dim sw As New System.Diagnostics.Stopwatch()

		' Get the drive type.

		Dim di As New DriveInfo(Path.GetPathRoot(MusicFolder))

		' If the drive is ready, just exit.

		If di.IsReady Then Return True

		' Start the stopwatch.  We'll wait just a bit in case it needs "waking up".

		sw.Start()

		' Begin a loop of checking the drive's availablity during the allowed wait period.

		Do While sw.Elapsed.TotalSeconds < timeoutSeconds
			Try
				If Directory.Exists(MusicFolder) Then
					' Try a harmless operation that forces the drive to wake
					Dim test = Directory.EnumerateFiles(MusicFolder).FirstOrDefault()
					Return True
				End If
			Catch
				' Drive not ready yet — wait and retry
			End Try

			Threading.Thread.Sleep(500) ' half-second pause
		Loop

		Return False
	End Function
	'***********************************************************************

	' This routine is called by the keyboard hook, when it intercepts a
	' media control button.  This ensures that the player always responds
	' to a media control button even when this program does not have the
	' focus.

	'***********************************************************************
	Public Sub OnShortcutKeyPressed(KeyCode As Keys)
		Select Case KeyCode
			Case Keys.MediaNextTrack
				MP.PlayNext()
			Case Keys.MediaPreviousTrack
				MP.PlayPrevious()
			Case Keys.MediaPlayPause
				MP.PlayPause()
		End Select
	End Sub

End Class
