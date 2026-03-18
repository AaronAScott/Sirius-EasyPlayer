Imports System.IO
Imports System.Windows.Forms

Public Class frmLocateMusicFolder

	'***********************************************************************
	' Sirius Playlist Editor Locate Music Folder form.
	' PE_LOCATEMUSICFOLDER.VB
	' Written: March 2026
	' Programmer: Aaron Scott
	' Copyright 2026 Sirius Software All Rights Reserved
	'***********************************************************************

	'***********************************************************************

	' The form is loaded.

	'***********************************************************************
	Private Sub frmSelectMusicFolder_Load(sender As Object, e As EventArgs) Handles Me.Load

		' Declare variables

		Dim DriveList As List(Of DriveInfo) = GetDrives()
		Dim dr As DriveInfo

		' Clear out the treeview.

		TreeView1.Nodes.Clear()

		' Populate the treeview control with a list of available drives.

		For Each dr In DriveList
			If dr.IsReady Then
				Dim node As TreeNode = TreeView1.Nodes.Add(dr.Name, dr.Name)
				node.Tag = dr.RootDirectory.FullName
				node.Nodes.Add("...")   ' placeholder
			End If
		Next dr

	End Sub
	'***********************************************************************

	' The form is closed.

	'***********************************************************************
	Private Sub frmSelectMusicFolder_FormClosed(sender As Object, e As EventArgs) Handles Me.FormClosed


		'If UserCancel Then Me.DialogResult = DialogResult.Cancel Else Me.DialogResult = DialogResult.OK

	End Sub
	'***********************************************************************

	' A Node is clicked.  Determine what to do.

	'***********************************************************************
	Private Sub TreeView1_BeforeExpand(sender As Object, e As TreeViewCancelEventArgs) Handles TreeView1.BeforeExpand

		Dim node As TreeNode = e.Node

		' Only expand if this node still has the placeholder
		If node.Nodes.Count = 1 AndAlso node.Nodes(0).Text = "..." Then
			node.Nodes.Clear()

			Dim path As String = CStr(node.Tag)

			Try
				' Enumerate only directories
				For Each dir As String In Directory.GetDirectories(path)
					Dim di As New DirectoryInfo(dir)

					' Skip hidden/system folders if you want (optional)
					' If (di.Attributes And (FileAttributes.Hidden Or FileAttributes.System)) <> 0 Then Continue For

					Dim child As TreeNode = node.Nodes.Add(di.Name, di.Name)
					child.Tag = di.FullName

					' Add placeholder if this folder has subfolders
					Try
						If di.GetDirectories().Length > 0 Then
							child.Nodes.Add("...")
						End If
					Catch
						' Ignore folders we can't enumerate
					End Try
				Next

			Catch
				' Ignore drives or folders that throw exceptions
			End Try
		End If
	End Sub
	'***********************************************************************

	' A node is selected.  Fill its tree.

	'***********************************************************************
	Private Sub TreeView1_DoubleClick(sender As Object, e As EventArgs) Handles TreeView1.DoubleClick

		Dim tv = DirectCast(sender, TreeView)
		Dim node = tv.SelectedNode
		Dim MusicFolder As String
		If node Is Nothing Then Exit Sub

		' Mark step 1 as completed.

		picStep1Success.Visible = True
		picStep1Failure.Visible = False
		picStep2Failure.Visible = False

		' Validate the folder as a music folder.

		If ValidateSelectedMusicFolder(node.Tag) Then
			picStep2Success.Visible = True

			' Validate the selected folder against the current library.

			Select Case ValidateMusicLibrary(node.Tag)
				Case -1
					If MsgBox("Your new music folder does not match your existing library. Do you want to proceed with it anyway, or select a different music folder?" & vbCrLf & "Click ""Yes"" to proceed with the selected folder, or ""No"" to select a new folder.", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Library Mismatch Found") = MsgBoxResult.Yes Then
						SaveSetting("Sirius" & ProgramName.Replace(" ", ""), "Settings", "MusicFolder", node.Tag)
						MusicFolder = node.Tag
						picStep3Success.Visible = True
						RecreateMusicLibrary(MusicFolder)
						picStep4Success.Visible = True
					Else
						picStep1Success.Visible = False
						picStep2Success.Visible = False
						Exit Sub
					End If
				Case 0
					SaveSetting("Sirius" & ProgramName.Replace(" ", ""), "Settings", "MusicFolder", node.Tag)
					picStep3Success.Visible = True
					ImportMusicList(node.Tag, frmMain.lblStatus)
					picStep4Success.Visible = True
					btnDone.Enabled = True
				Case 1
					SaveSetting("Sirius" & ProgramName.Replace(" ", ""), "Settings", "MusicFolder", node.Tag)
					MsgBox("Your new music folder matches your existing library.  No import is nedded.", MsgBoxStyle.Information, "New Music Folder Set")
					btnDone.Enabled = True
					picStep4Success.Visible = True
			End Select

			' If the selected folder was not a valid music folder, mark this step as failed.

		Else
			picStep1Success.Visible = False
			picStep2Success.Visible = False
			picStep2Failure.Visible = True
			MsgBox("The selected folder does not contain a music library of the required format.", MsgBoxStyle.Information, "Locate Music Library")
		End If

	End Sub
	'***********************************************************************

	' Function to fill the treeview with a list of available drives, to start with.

	'***********************************************************************
	Public Function GetDrives() As List(Of DriveInfo)
		Dim drives As New List(Of DriveInfo)

		For Each d As DriveInfo In DriveInfo.GetDrives()
			Try
				If d.IsReady Then
					drives.Add(d)
				End If
			Catch
				' Ignore drives that throw (card readers, etc.)
			End Try
		Next

		Return drives
	End Function
	'***********************************************************************

	' Function to check the existing library against the new music folder.
	' Return:   0 = Library is empty (no music imported yet)
	'          -1 = Library exists but doesn't match folder.
	'           1 = Library exists and matches new folder.
	'***********************************************************************
	Private Function ValidateMusicLibrary(MusicFolder) As Integer


		' Declare variables.

		Dim ii As Integer
		Dim Errors As Integer
		Dim zx As String
		Dim ds As New DataSet
		Dim dt As DataTable

		Try
			LibraryDA.Fill(ds, "Table")
			dt = ds.Tables("Table")

			' If the library is empty, return 0 to indicate it's empty.

			If dt.Rows.Count = 0 Then
				Return 0

				' Otherwise, compare the library with the music folder.

				For ii = 0 To dt.Rows.Count - 1
					zx = $"{MusicFolder}\{dt.Rows(ii)("ArtistName")}\{dt.Rows(ii)("AlbumName")}\{dt.Rows(ii)("SongName")}"
					If Not My.Computer.FileSystem.FileExists(zx) Then Errors += 1
				Next ii
			End If
		Catch ex As exception

		End Try

		' Return the status of the check.

		If Errors = 0 Then Return 1 Else Return -1

	End Function
	'***********************************************************************

	' Function to make sure the selected folder contains music files, at least
	' one.
	'***********************************************************************
	Private Function ValidateSelectedMusicFolder(rootPath As String) As Boolean
		' Root must exist
		If Not Directory.Exists(rootPath) Then Return False

		' --- LEVEL 1: Artist folders ---
		Dim artistDirs As String()
		Try
			artistDirs = Directory.GetDirectories(rootPath)
		Catch
			Return False
		End Try

		If artistDirs.Length = 0 Then Return False

		For Each artist In artistDirs
			Dim artistInfo As New DirectoryInfo(artist)

			' Skip hidden/system folders if desired
			If (artistInfo.Attributes And (FileAttributes.Hidden Or FileAttributes.System)) <> 0 Then
				Continue For
			End If

			' --- LEVEL 2: Album folders ---
			Dim albumDirs As String()
			Try
				albumDirs = Directory.GetDirectories(artist)
			Catch
				Return False
			End Try

			If albumDirs.Length = 0 Then Return False

			For Each album In albumDirs
				Dim albumInfo As New DirectoryInfo(album)

				If (albumInfo.Attributes And (FileAttributes.Hidden Or FileAttributes.System)) <> 0 Then
					Continue For
				End If

				' --- LEVEL 3: Songs ---
				Dim songs As IReadOnlyCollection(Of String)
				Try
					songs = My.Computer.FileSystem.GetFiles(album, FileIO.SearchOption.SearchTopLevelOnly, ExtensionPrecedenceWildcards)
				Catch
					Return False
				End Try

				If songs.Count > 0 Then Return True
			Next album
		Next artist

		Return False
	End Function
	'***********************************************************************

	' Sub to re-create the music library, if necessary.

	'***********************************************************************
	Private Sub RecreateMusicLibrary(MusicFolder As String)

		' Declare variables

		Dim ii As Integer

		If DbOpen Then CloseDatabase()

		' Clear out the display lines collection, and erase the picturebox.

		frmMain.DisplayLines.Clear()
		Using g As Graphics = frmMain.picLibraryDisplay.CreateGraphics
			g.FillRectangle(Brushes.White, frmMain.picLibraryDisplay.Bounds)
		End Using

		' Recreate the new database.  The SQL commands will drop the existing one first, then
		' create the new one.

		CreateNewDatabase()

		ii = ImportMusicList(MusicFolder, frmMain.lblStatus)

		' Rebuld the libary dataset.

		LibraryDS.Clear()
		LibraryDA.Fill(LibraryDS, "Table")
		LibraryTable = LibraryDS.Tables("Table")
		frmMain.VScrollBar1.Maximum = LibraryTable.Rows.Count - 1

		' Force the newly-recreated library to display.

		frmMain.picLibraryDisplay.Invalidate()

	End Sub
	'***********************************************************************

	' The select music library process is completed.  Exit and set the ok result

	'***********************************************************************
	Private Sub btnDone_Click(sender As Object, e As EventArgs) Handles btnDone.Click

		Me.DialogResult = DialogResult.OK
		Me.Close()

	End Sub

	'***********************************************************************

	' The select music library process is cancelled.  Exit and set the cancel result.

	'***********************************************************************
	Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click

		Me.DialogResult = DialogResult.Cancel
		Me.Close()

	End Sub
End Class