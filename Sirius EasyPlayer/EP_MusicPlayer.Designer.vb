<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMusicPlayer
	Inherits System.Windows.Forms.Form

	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()>
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
	<System.Diagnostics.DebuggerStepThrough()>
	Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMusicPlayer))
		Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
		Me.mnuPlaylists = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuSelectPlaylist = New System.Windows.Forms.ToolStripMenuItem()
		Me.FeaturesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnPMTS = New System.Windows.Forms.ToolStripMenuItem()
		Me.picAlbumArt = New System.Windows.Forms.PictureBox()
		Me.lblDuration = New System.Windows.Forms.Label()
		Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
		Me.lblElapsedTime = New System.Windows.Forms.Label()
		Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
		Me.ListBox1 = New System.Windows.Forms.ListBox()
		Me.Timer2 = New System.Windows.Forms.Timer(Me.components)
		Me.MenuStrip1.SuspendLayout()
		CType(Me.picAlbumArt, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'MenuStrip1
		'
		Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuPlaylists, Me.FeaturesToolStripMenuItem})
		Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
		Me.MenuStrip1.Name = "MenuStrip1"
		Me.MenuStrip1.Size = New System.Drawing.Size(633, 24)
		Me.MenuStrip1.TabIndex = 1
		Me.MenuStrip1.Text = "MenuStrip1"
		'
		'mnuPlaylists
		'
		Me.mnuPlaylists.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuSelectPlaylist})
		Me.mnuPlaylists.Name = "mnuPlaylists"
		Me.mnuPlaylists.Size = New System.Drawing.Size(61, 20)
		Me.mnuPlaylists.Text = "&Playlists"
		'
		'mnuSelectPlaylist
		'
		Me.mnuSelectPlaylist.Name = "mnuSelectPlaylist"
		Me.mnuSelectPlaylist.Size = New System.Drawing.Size(145, 22)
		Me.mnuSelectPlaylist.Text = "&Select Playlist"
		'
		'FeaturesToolStripMenuItem
		'
		Me.FeaturesToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnPMTS})
		Me.FeaturesToolStripMenuItem.Name = "FeaturesToolStripMenuItem"
		Me.FeaturesToolStripMenuItem.Size = New System.Drawing.Size(63, 20)
		Me.FeaturesToolStripMenuItem.Text = "&Features"
		'
		'mnPMTS
		'
		Me.mnPMTS.Name = "mnPMTS"
		Me.mnPMTS.Size = New System.Drawing.Size(161, 22)
		Me.mnPMTS.Text = "&Play Me to Sleep"
		'
		'picAlbumArt
		'
		Me.picAlbumArt.Location = New System.Drawing.Point(89, 48)
		Me.picAlbumArt.Name = "picAlbumArt"
		Me.picAlbumArt.Size = New System.Drawing.Size(250, 250)
		Me.picAlbumArt.TabIndex = 2
		Me.picAlbumArt.TabStop = False
		'
		'lblDuration
		'
		Me.lblDuration.AutoSize = True
		Me.lblDuration.BackColor = System.Drawing.Color.Transparent
		Me.lblDuration.Location = New System.Drawing.Point(309, 301)
		Me.lblDuration.Name = "lblDuration"
		Me.lblDuration.Size = New System.Drawing.Size(34, 13)
		Me.lblDuration.TabIndex = 3
		Me.lblDuration.Text = "00:00"
		'
		'Timer1
		'
		Me.Timer1.Interval = 1000
		'
		'lblElapsedTime
		'
		Me.lblElapsedTime.AutoSize = True
		Me.lblElapsedTime.BackColor = System.Drawing.Color.Transparent
		Me.lblElapsedTime.Location = New System.Drawing.Point(86, 301)
		Me.lblElapsedTime.Name = "lblElapsedTime"
		Me.lblElapsedTime.Size = New System.Drawing.Size(34, 13)
		Me.lblElapsedTime.TabIndex = 4
		Me.lblElapsedTime.Text = "00:00"
		'
		'ProgressBar1
		'
		Me.ProgressBar1.ForeColor = System.Drawing.Color.Chartreuse
		Me.ProgressBar1.Location = New System.Drawing.Point(119, 304)
		Me.ProgressBar1.Name = "ProgressBar1"
		Me.ProgressBar1.Size = New System.Drawing.Size(190, 10)
		Me.ProgressBar1.TabIndex = 5
		'
		'ListBox1
		'
		Me.ListBox1.Dock = System.Windows.Forms.DockStyle.Right
		Me.ListBox1.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.ListBox1.FormattingEnabled = True
		Me.ListBox1.ItemHeight = 16
		Me.ListBox1.Location = New System.Drawing.Point(428, 24)
		Me.ListBox1.Name = "ListBox1"
		Me.ListBox1.Size = New System.Drawing.Size(205, 487)
		Me.ListBox1.TabIndex = 6
		'
		'Timer2
		'
		Me.Timer2.Enabled = True
		Me.Timer2.Interval = 300000
		'
		'frmMusicPlayer
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(633, 511)
		Me.Controls.Add(Me.ListBox1)
		Me.Controls.Add(Me.ProgressBar1)
		Me.Controls.Add(Me.lblElapsedTime)
		Me.Controls.Add(Me.lblDuration)
		Me.Controls.Add(Me.picAlbumArt)
		Me.Controls.Add(Me.MenuStrip1)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.KeyPreview = True
		Me.Name = "frmMusicPlayer"
		Me.MenuStrip1.ResumeLayout(False)
		Me.MenuStrip1.PerformLayout()
		CType(Me.picAlbumArt, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents MenuStrip1 As MenuStrip
	Friend WithEvents mnuPlaylists As ToolStripMenuItem
	Friend WithEvents mnuSelectPlaylist As ToolStripMenuItem
	Friend WithEvents FeaturesToolStripMenuItem As ToolStripMenuItem
	Friend WithEvents mnPMTS As ToolStripMenuItem
	Friend WithEvents picAlbumArt As PictureBox
	Friend WithEvents lblDuration As Label
	Friend WithEvents Timer1 As Timer
	Friend WithEvents lblElapsedTime As Label
	Friend WithEvents ProgressBar1 As ProgressBar
	Friend WithEvents ListBox1 As ListBox
	Friend WithEvents Timer2 As Timer
End Class
