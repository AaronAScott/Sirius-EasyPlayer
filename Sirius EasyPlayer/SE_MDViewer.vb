Public Class frmMDViewer
	Private Sub frmMDViewer_Load(sender As Object, e As EventArgs) Handles Me.Load

		MarkdownViewer1.LoadMarkdown(My.Application.Info.DirectoryPath & "\Readme.md")

	End Sub
End Class