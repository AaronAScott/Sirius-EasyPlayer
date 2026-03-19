Imports System.Text


Public Class frmChangeFilePrecedence
	'***********************************************************************
	' Sirius Sirius EasyPlayer Change File Precedence form
	' EP_CHANGEFILEPRCEDENCE.VB
	' Written: March 2026
	' Programmer: Aaron Scott
	' Copyright 2026 Sirius Software All Rights Reserved
	'***********************************************************************

	'***********************************************************************

	' The form is loaded.

	'***********************************************************************
	Private Sub frmChangeFilePrecedence_Load(sender As Object, e As EventArgs) Handles Me.Load

		' Declare variables

		Dim ii As Integer
		Dim Ext() As String = ExtensionPrecedence

		' Clear list boxes' previous contents.

		ComboBox1.Items.Clear()
		ComboBox2.Items.Clear()
		ComboBox3.Items.Clear()
		ComboBox4.Items.Clear()
		ComboBox5.Items.Clear()

		' Set the list of each of the 5 drop-down list boxes.

		For ii = 0 To Ext.Count - 1
			ComboBox1.Items.Add(Ext(ii))
			ComboBox2.Items.Add(Ext(ii))
			ComboBox3.Items.Add(Ext(ii))
			ComboBox4.Items.Add(Ext(ii))
			ComboBox5.Items.Add(Ext(ii))
		Next ii

		' Make sure they are selected in order.

		ComboBox1.SelectedIndex = 0
		ComboBox2.SelectedIndex = 1
		ComboBox3.SelectedIndex = 2
		ComboBox4.SelectedIndex = 3
		ComboBox5.SelectedIndex = 4
	End Sub
	'***********************************************************************

	' The save button is clicked.

	'***********************************************************************
	Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

		' Declare variables.

		Dim sb As New StringBuilder

		' Make sure no two boxes have the same selection.

		If Not ValidateNoDuplicates() Then
			MsgBox("One or more boxes are set to the same file extension.", MsgBoxStyle.Information, "Duplicate Extension Detected")
			Exit Sub
		End If

		' Build the new extension order list.

		sb.Append(ComboBox1.SelectedItem & ",")
		sb.Append(ComboBox2.SelectedItem & ",")
		sb.Append(ComboBox3.SelectedItem & ",")
		sb.Append(ComboBox4.SelectedItem & ",")
		sb.Append(ComboBox5.SelectedItem)

		' Save the setting.  This is the ONLY place this can be changed in the program.

		SaveSetting("Sirius" & SRep(ProgramName, 1, " ", ""), "Settings", "FilePrecedence", sb.ToString)

		Me.Close()

	End Sub
	'***********************************************************************

	' The Cancel button is clicked.

	'***********************************************************************
	Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
		Me.Close()
	End Sub
	'***********************************************************************

	' Function to verify that no two combo boxes have the same selection.

	'***********************************************************************
	Private Function ValidateNoDuplicates() As Boolean
		Dim indices = {
		    ComboBox1.SelectedIndex,
		    ComboBox2.SelectedIndex,
		    ComboBox3.SelectedIndex,
		    ComboBox4.SelectedIndex,
		    ComboBox5.SelectedIndex
		}

		' Filter out any unexpected -1 values
		Dim chosen = indices.Where(Function(i) i >= 0)

		Dim seen As New HashSet(Of Integer)

		For Each i In chosen
			If Not seen.Add(i) Then
				Return False   ' Duplicate detected
			End If
		Next

		Return True
	End Function
End Class