<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSelectMusicFolder
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSelectMusicFolder))
		Me.Label1 = New System.Windows.Forms.Label()
		Me.Label2 = New System.Windows.Forms.Label()
		Me.cmbMusicFolders = New System.Windows.Forms.ComboBox()
		Me.btnBrowse = New System.Windows.Forms.Button()
		Me.btnImport = New System.Windows.Forms.Button()
		Me.SuspendLayout()
		'
		'Label1
		'
		Me.Label1.Location = New System.Drawing.Point(29, 24)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(298, 53)
		Me.Label1.TabIndex = 0
		Me.Label1.Text = "No music folder has been specified.  Select one from the drop-down list, or brows" &
	"e for for one now."
		'
		'Label2
		'
		Me.Label2.Location = New System.Drawing.Point(26, 77)
		Me.Label2.Name = "Label2"
		Me.Label2.Size = New System.Drawing.Size(80, 20)
		Me.Label2.TabIndex = 1
		Me.Label2.Text = "Music Folder:"
		'
		'cmbMusicFolders
		'
		Me.cmbMusicFolders.FormattingEnabled = True
		Me.cmbMusicFolders.Location = New System.Drawing.Point(112, 74)
		Me.cmbMusicFolders.Name = "cmbMusicFolders"
		Me.cmbMusicFolders.Size = New System.Drawing.Size(121, 21)
		Me.cmbMusicFolders.TabIndex = 2
		'
		'btnBrowse
		'
		Me.btnBrowse.Location = New System.Drawing.Point(252, 72)
		Me.btnBrowse.Name = "btnBrowse"
		Me.btnBrowse.Size = New System.Drawing.Size(75, 23)
		Me.btnBrowse.TabIndex = 3
		Me.btnBrowse.Text = "&Browse"
		Me.btnBrowse.UseVisualStyleBackColor = True
		'
		'btnImport
		'
		Me.btnImport.Enabled = False
		Me.btnImport.Location = New System.Drawing.Point(140, 147)
		Me.btnImport.Name = "btnImport"
		Me.btnImport.Size = New System.Drawing.Size(75, 23)
		Me.btnImport.TabIndex = 4
		Me.btnImport.Text = "&Import"
		Me.btnImport.UseVisualStyleBackColor = True
		'
		'frmSelectMusicFolder
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(354, 222)
		Me.Controls.Add(Me.btnImport)
		Me.Controls.Add(Me.btnBrowse)
		Me.Controls.Add(Me.cmbMusicFolders)
		Me.Controls.Add(Me.Label2)
		Me.Controls.Add(Me.Label1)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmSelectMusicFolder"
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Select Music Folder"
		Me.ResumeLayout(False)

	End Sub

	Friend WithEvents Label1 As Label
	Friend WithEvents Label2 As Label
	Friend WithEvents cmbMusicFolders As ComboBox
	Friend WithEvents btnBrowse As Button
	Friend WithEvents btnImport As Button
End Class
