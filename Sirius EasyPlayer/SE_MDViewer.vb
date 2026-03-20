Imports System.IO
Public Class frmMDViewer

	Public Sub LoadFile(path As String)
		MarkdownViewer1.LoadFile(path)
	End Sub
	Private mEditing As Boolean

	Private Sub frmMDViewer_Load(sender As Object, e As EventArgs) Handles Me.Load

		If mEditing Then TextBox1.Text = MarkdownViewer1.RawText
	End Sub
	Public Property EnableEditing As Boolean
		Get
			Return mEditing
		End Get
		Set(value As Boolean)
			mEditing = value

			If value Then
				MenuStrip1.Visible = True
			Else
				MenuStrip1.Visible = False
			End If
		End Set
	End Property

	Private Sub mnuViewRender_Click(sender As Object, e As EventArgs) Handles mnuViewRender.Click
		MarkdownViewer1.Visible = True
		TextBox1.Visible = False
	End Sub

	Private Sub mnuViewEditor_Click(sender As Object, e As EventArgs) Handles mnuViewEditor.Click
		MarkdownViewer1.Visible = False
		TextBox1.Visible = True

	End Sub
End Class