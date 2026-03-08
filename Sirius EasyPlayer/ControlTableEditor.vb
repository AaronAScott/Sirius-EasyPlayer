Option Strict Off
Option Explicit On
Imports System.Data.SqlClient
Friend Class frmControlTableEditor
     Inherits System.Windows.Forms.Form
     '**************************************************
     ' Control Table Editor for Visual Basic SQL programs
     ' CONTROLTABLEEDITOR.VB
     ' Written: May 2007
     ' Updated: January 2017
     ' Programmer: Aaron Scott
     ' Copyright (C) 2003-2017 Sirius Software All Rights Reserved
     '**************************************************

     ' Declare variables that are public properties.

     Private ControlDS As New DataSet
     Private ControlTable As New DataTable

     ' Declare private variables

     Private TableOpen As Boolean
     Private CurrentRow As DataRow

     '**************************************************
     '
     ' Sub to enable or disable the entry fields
     '
     '**************************************************
     Private Sub EnableFields(ByRef State As Boolean)

          Label2.Enabled = State
          Label3.Enabled = State
          Text1.Enabled = State
          Text2.Enabled = State

     End Sub

     '**************************************************
     '
     ' The close button is clicked.
     '
     '**************************************************
     Private Sub cmdClose_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdClose.Click
          Me.Close()
     End Sub

     '**************************************************
     '
     ' The delete button is clicked.
     '
     '**************************************************
     Private Sub cmdDelete_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDelete.Click

          ' Make sure we have an item name

          If Text1.Text = "" Then
               MsgBox("You must supply an item name .", MsgBoxStyle.Information, "Save Control Item")
               On Error Resume Next
               Text1.Focus()
               On Error GoTo 0
               Exit Sub
          End If

          ' Delete the control item

          PutControlItem(Text1.Text, "")

          ' Delete the item from the list

          If List1.SelectedIndex > 0 Then List1.Items.RemoveAt(List1.SelectedIndex)

          ' Reset the selection

          List1.SelectedIndex = -1
          List1_SelectedIndexChanged(List1, New System.EventArgs())

     End Sub

     '**************************************************
     '
     ' The save button is clicked.
     '
     '**************************************************
     Private Sub cmdSave_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSave.Click

          ' Make sure we have an item name and value

          If Text1.Text = "" Or Text2.Text = "" Then
               MsgBox("You must supply both an item name and a value.", MsgBoxStyle.Information, "Save Control Item")
               Text1.Focus()
               Exit Sub
          End If

          ' Save the item. If the item name changes, delete
          ' the old item first.

          If List1.SelectedIndex > 0 And LCase(Text1.Text) <> LCase(List1.Items(List1.SelectedIndex)) Then PutControlItem(List1.Items(List1.SelectedIndex), "")
          PutControlItem(Text1.Text, Text2.Text)

          ' If we added a new item, add it to the list box.

          If List1.SelectedIndex = 0 Then
               List1.Items.Add(Text1.Text)
          ElseIf List1.SelectedIndex > 0 Then
               List1.Items(List1.SelectedIndex) = Text1.Text
          End If

          ' Reset the selection

          List1.SelectedIndex = -1
          List1_SelectedIndexChanged(List1, New System.EventArgs())

     End Sub

     '**************************************************

     ' The form is closed.

     '**************************************************
     Private Sub frmControlTableEditor_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
          ControlDS.Dispose()
          ControlTable.Dispose()
     End Sub
     '**************************************************
     '
     ' The form is loaded.
     '
     '**************************************************
     Private Sub frmControlTableEditor_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

          ' Declare variables

          Dim jj As Integer

          ' Open the control table

          ControlDS.Clear()
          ControlDA.Fill(ControlDS, "Table")
          ControlTable = ControlDS.Tables("Table")

          ' Fill the list box with control items.

          List1.Items.Clear()
          List1.Items.Add("(New Item)")
          If ControlTable.Rows.Count > 0 Then
               jj = 0
               Do
                    List1.Items.Add(GetR(ControlTable.Rows(jj), "ItemName"))
                    jj += 1
               Loop While jj < ControlTable.Rows.Count
          End If
          Text1.Text = ""
          Text2.Text = ""

     End Sub

     '**************************************************
     '
     ' An item is selected
     '
     '**************************************************
     Private Sub List1_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles List1.SelectedIndexChanged

          ' Declare variables

          Dim xx As Integer
          Dim zx As String

          ' If the "New Item" is selected, clear the
          ' entry fields.  Otherwise, fill them with
          ' the selected item.

          If List1.SelectedIndex >= 0 Then

               ' Display a warning first, if an existing item
               ' is selected (but not if "new item" is selected).

               If List1.SelectedIndex > 0 Then Label4.Text = "Warning!  Changing or deleting existing control table items may damage your database or render it unusable by the program." & vbCrLf & vbCrLf & "You should alter or delete control table items only under direction of Support Personnel." Else Label4.Text = ""

               ' Enable the entry fields

               EnableFields(True)

               ' See if the "New Item" option was clicked.

               If List1.SelectedIndex = 0 Then
                    Text1.Text = ""
                    Text2.Text = ""
                    cmdDelete.Enabled = False

                    ' Otherwise, get an existing item.

               Else
                    Text1.Text = List1.Items(List1.SelectedIndex)
                    zx = List1.Items(List1.SelectedIndex)
                    xx = Find(ControlTable, "ItemName='" & zx & "'")
                    If xx >= 0 Then
                         CurrentRow = ControlTable.Rows(xx)
                         Text2.Text = GetR(CurrentRow, "Value")
                         cmdDelete.Enabled = True
                    End If
               End If

               ' Enable the save and delete buttons

               cmdSave.Enabled = True

               ' Set the focus to the item name field

               Text1.Focus()

               ' If there is nothing selected, clear fields and disable
               ' the functions.

          Else
               EnableFields(False)
               Label4.Text = ""
               Text1.Text = ""
               Text2.Text = ""
               cmdSave.Enabled = False
               cmdDelete.Enabled = False
          End If

     End Sub

     '**************************************************
     '
     ' One of the entry fields has got the focus
     '
     '**************************************************
     Private Sub Text_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Text1.Enter, Text2.Enter
          eventSender.SelectionStart = 0
          eventSender.SelectionLength = Len(eventSender.Text)
     End Sub

End Class