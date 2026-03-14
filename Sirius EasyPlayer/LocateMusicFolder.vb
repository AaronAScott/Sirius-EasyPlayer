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

	' Set a flag which will let the program know if the user
	' cancelled this operation.


	Public Property SelectedPath As String = ""

	Public Function LocateMusicFolder() As String
		Me.ShowDialog()   ' modal; execution pauses here
		Return SelectedPath
	End Function

	Private UserCancel As Boolean = True
	'***********************************************************************

	' The form is loaded.

	'***********************************************************************
	Private Sub frmSelectMusicFolder_Load(sender As Object, e As EventArgs) Handles Me.Load

		' Declare variables

		Dim DriveList As List(Of DriveInfo) = GetDrives()
		Dim dr As DriveInfo

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
	Private Sub TreeView1_DoubleClick(sender As Object, e As EventArgs) Handles TreeView1.DoubleClick

		Dim tv = DirectCast(sender, TreeView)
		Dim node = tv.SelectedNode
		If node Is Nothing Then Exit Sub

		' Mark step 1 as completed.

		PictureBox1.Visible = True

		' Validate the folder as a music folder.

		If ValidateSelectedMusicFolder(node.Tag) Then
			PictureBox2.Visible = True
			If ValidateMusicLibrary(node.Tag) Then
				PictureBox3.Visible = True
				ImportMusicList(node.Tag, frmMain.lblStatus)
				PictureBox4.Visible = True
				Me.Close()
			Else
				MsgBox("The selected folder does not contain a music library of the required format.", MsgBoxStyle.Information, "Locate Music Library")
				Stop
			End If
		End If

	End Sub
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
	Private Function ValidateMusicLibrary(MusicFolder) As Boolean

		' Declare variables.

		Dim ii As Integer
		Dim Errors As Integer
		Dim zx As String
		Dim ds As New DataSet
		Dim dt As DataTable

		Try
			LibraryDA.Fill(ds, "Table")
			dt = ds.Tables("Table")

			If dt.Rows.Count > 0 Then
				For ii = 0 To dt.Rows.Count - 1
					zx = $"{MusicFolder}\{dt.Rows(ii)("ArtistName")}\{dt.Rows(ii)("AlbumName")}\{dt.Rows(ii)("SongName")}"
					If Not My.Computer.FileSystem.FileExists(zx) Then Errors += 1
				Next ii
			End If
			If Errors = 0 Then
				PictureBox3.Visible = True
				SaveSetting("Sirius" & ProgramName.Replace(" ", ""), "Settings", "MusicFolder", MusicFolder)
				MsgBox("Your new music folder has been set.", MsgBoxStyle.Information, "New Music Folder Selected")
				Return True
			End If

			Dim r As MsgBoxResult = MsgBox("The current music library shows " & Errors & " between it and the new music folder. If you choose to proceed, your library must be recreated from the new music folder." & vbCrLf & "Do you want to proceed?", MsgBoxStyle.Question + MsgBoxStyle.YesNoCancel, "Music Library Errors Found")

			If r <> MsgBoxResult.Yes Then Return False

			ImportMusicList(MusicFolder, frmMain.lblStatus)
			PictureBox4.Visible = True

		Catch ex As Exception

		End Try

		Return False

	End Function
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
				Dim songs As String()
				Try
					songs = Directory.GetFiles(album, "*.mp3").
						   Concat(Directory.GetFiles(album, "*.flac")).
						   Concat(Directory.GetFiles(album, "*.wma")).
						   ToArray()
				Catch
					Return False
				End Try

				If songs.Length > 0 Then Return True
			Next album
		Next artist

		Return False
	End Function
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
End Class