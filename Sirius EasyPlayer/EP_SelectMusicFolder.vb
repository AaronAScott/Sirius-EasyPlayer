Public Class frmSelectMusicFolder
	'***********************************************************************
	' Sirius Playlist Editor Select Music Folder form.
	' PE_SELECTMUSICFOLDER.VB
	' Written: May 2026
	' Programmer: Aaron Scott
	' Copyright 2026 Sirius Software All Rights Reserved
	'***********************************************************************

	' Set a flag which will let the program know if the user
	' cancelled this operation.

	Private UserCancel As Boolean = True
	'***********************************************************************

	' The form is loaded.

	'***********************************************************************
	Private Sub frmSelectMusicFolder_Load(sender As Object, e As EventArgs) Handles Me.Load

		' Declare variables

		Dim zx As String

		' Fill the list box with a list of any detectable music folders on any drive.

		PopulateDriveList(cmbMusicFolders)

		zx = GetSetting("Sirius" & ProgramName.Replace(" ", ""), "Settings", "MusicFolder", "")
		If zx <> "" Then
			For Each Folder In cmbMusicFolders.Items
				If Folder = zx Then
					cmbMusicFolders.SelectedItem = Folder
					Exit For
				End If
			Next
		End If
	End Sub
	'***********************************************************************

	' The form is closed.

	'***********************************************************************
	Private Sub frmSelectMusicFolder_FormClosed(sender As Object, e As EventArgs) Handles Me.FormClosed


		If UserCancel Then Me.DialogResult = DialogResult.Cancel Else Me.DialogResult = DialogResult.OK

	End Sub

	'***********************************************************************

	' Event handler for the folder combo box.

	'***********************************************************************
	Private Sub cmbMusicFolders_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbMusicFolders.SelectedIndexChanged

		' When an item is selected, enable the "Import" button.

		If cmbMusicFolders.SelectedIndex >= 0 Then
			If My.Computer.FileSystem.DirectoryExists(cmbMusicFolders.SelectedItem) Then
				btnImport.Enabled = True
			Else
				btnImport.Enabled = False
			End If
		End If
	End Sub

	'***********************************************************************

	' The import button is clicked.

	'***********************************************************************
	Private Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.Click

		' Save the selected music location and return a success code.

		Dim zx As String = cmbMusicFolders.SelectedItem
		SaveSetting("Sirius" & ProgramName.Replace(" ", ""), "Settings", "MusicFolder", zx.Trim)
		UserCancel = False
		Me.Close()

	End Sub
End Class