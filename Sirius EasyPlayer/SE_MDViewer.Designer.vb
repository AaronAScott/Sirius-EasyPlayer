<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMDViewer
	Inherits System.Windows.Forms.Form

	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> _
	Protected Overrides Sub Dispose(ByVal disposing As Boolean)
		Try
			If disposing AndAlso components IsNot Nothing Then
				components.Dispose()
			End If
		Finally
			MyBase.Dispose(disposing)
		End Try
	End Sub

	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer

	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.  
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	Private Sub InitializeComponent()
		Me.MarkdownViewer1 = New Sirius_EasyPlayer.MarkdownViewer()
		Me.SuspendLayout()
		'
		'MarkdownViewer1
		'
		Me.MarkdownViewer1.AutoScroll = True
		Me.MarkdownViewer1.AutoScrollMinSize = New System.Drawing.Size(576, 0)
		Me.MarkdownViewer1.Location = New System.Drawing.Point(67, 12)
		Me.MarkdownViewer1.Name = "MarkdownViewer1"
		Me.MarkdownViewer1.Size = New System.Drawing.Size(576, 317)
		Me.MarkdownViewer1.TabIndex = 0
		'
		'frmMDViewer
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(800, 450)
		Me.Controls.Add(Me.MarkdownViewer1)
		Me.Name = "frmMDViewer"
		Me.Text = "SE_MDViewer"
		Me.ResumeLayout(False)

	End Sub

	Friend WithEvents MarkdownViewer1 As MarkdownViewer
End Class
