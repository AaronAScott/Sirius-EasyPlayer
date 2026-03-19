<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmSync
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSync))
		Me.Label1 = New System.Windows.Forms.Label()
		Me.lstSyncList = New System.Windows.Forms.ListBox()
		Me.TabPage1 = New System.Windows.Forms.TabPage()
		Me.Panel1 = New System.Windows.Forms.Panel()
		Me.TabControl1 = New System.Windows.Forms.TabControl()
		Me.btnCancel = New System.Windows.Forms.Button()
		Me.btnSync = New System.Windows.Forms.Button()
		Me.picLegend = New System.Windows.Forms.PictureBox()
		Me.PictureBox1 = New System.Windows.Forms.PictureBox()
		Me.PictureBox2 = New System.Windows.Forms.PictureBox()
		Me.TabPage1.SuspendLayout()
		Me.TabControl1.SuspendLayout()
		CType(Me.picLegend, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'Label1
		'
		Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label1.Location = New System.Drawing.Point(42, 43)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(286, 23)
		Me.Label1.TabIndex = 0
		Me.Label1.Text = "Music to be Synced"
		Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		'
		'lstSyncList
		'
		Me.lstSyncList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
		Me.lstSyncList.FormattingEnabled = True
		Me.lstSyncList.Location = New System.Drawing.Point(42, 70)
		Me.lstSyncList.Name = "lstSyncList"
		Me.lstSyncList.Size = New System.Drawing.Size(286, 368)
		Me.lstSyncList.TabIndex = 1
		'
		'TabPage1
		'
		Me.TabPage1.Controls.Add(Me.Panel1)
		Me.TabPage1.Location = New System.Drawing.Point(4, 22)
		Me.TabPage1.Name = "TabPage1"
		Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
		Me.TabPage1.Size = New System.Drawing.Size(246, 346)
		Me.TabPage1.TabIndex = 0
		Me.TabPage1.Text = "TabPage1"
		Me.TabPage1.UseVisualStyleBackColor = True
		'
		'Panel1
		'
		Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.Panel1.Location = New System.Drawing.Point(3, 3)
		Me.Panel1.Name = "Panel1"
		Me.Panel1.Size = New System.Drawing.Size(240, 340)
		Me.Panel1.TabIndex = 0
		'
		'TabControl1
		'
		Me.TabControl1.Controls.Add(Me.TabPage1)
		Me.TabControl1.Location = New System.Drawing.Point(497, 70)
		Me.TabControl1.Name = "TabControl1"
		Me.TabControl1.SelectedIndex = 0
		Me.TabControl1.Size = New System.Drawing.Size(254, 372)
		Me.TabControl1.TabIndex = 3
		'
		'btnCancel
		'
		Me.btnCancel.Enabled = False
		Me.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
		Me.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Red
		Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
		Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.btnCancel.Location = New System.Drawing.Point(351, 397)
		Me.btnCancel.Name = "btnCancel"
		Me.btnCancel.Size = New System.Drawing.Size(127, 41)
		Me.btnCancel.TabIndex = 13
		Me.btnCancel.Text = "S&top Sync"
		Me.btnCancel.UseVisualStyleBackColor = True
		'
		'btnSync
		'
		Me.btnSync.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Lime
		Me.btnSync.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
		Me.btnSync.FlatStyle = System.Windows.Forms.FlatStyle.Flat
		Me.btnSync.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.btnSync.Location = New System.Drawing.Point(351, 321)
		Me.btnSync.Name = "btnSync"
		Me.btnSync.Size = New System.Drawing.Size(127, 41)
		Me.btnSync.TabIndex = 12
		Me.btnSync.Text = "&Start Sync"
		Me.btnSync.UseVisualStyleBackColor = True
		'
		'picLegend
		'
		Me.picLegend.Location = New System.Drawing.Point(334, 70)
		Me.picLegend.Name = "picLegend"
		Me.picLegend.Size = New System.Drawing.Size(161, 50)
		Me.picLegend.TabIndex = 14
		Me.picLegend.TabStop = False
		'
		'PictureBox1
		'
		Me.PictureBox1.Location = New System.Drawing.Point(351, 169)
		Me.PictureBox1.Name = "PictureBox1"
		Me.PictureBox1.Size = New System.Drawing.Size(120, 80)
		Me.PictureBox1.TabIndex = 15
		Me.PictureBox1.TabStop = False
		'
		'PictureBox2
		'
		Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
		Me.PictureBox2.Location = New System.Drawing.Point(351, 235)
		Me.PictureBox2.Name = "PictureBox2"
		Me.PictureBox2.Size = New System.Drawing.Size(120, 80)
		Me.PictureBox2.TabIndex = 16
		Me.PictureBox2.TabStop = False
		Me.PictureBox2.Visible = False
		'
		'frmSync
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(800, 450)
		Me.Controls.Add(Me.PictureBox2)
		Me.Controls.Add(Me.PictureBox1)
		Me.Controls.Add(Me.picLegend)
		Me.Controls.Add(Me.btnCancel)
		Me.Controls.Add(Me.btnSync)
		Me.Controls.Add(Me.TabControl1)
		Me.Controls.Add(Me.lstSyncList)
		Me.Controls.Add(Me.Label1)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
		Me.Name = "frmSync"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Sync to Mobile Device"
		Me.TabPage1.ResumeLayout(False)
		Me.TabControl1.ResumeLayout(False)
		CType(Me.picLegend, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)

	End Sub

	Friend WithEvents Label1 As Label
	Friend WithEvents lstSyncList As ListBox
	Friend WithEvents TabPage1 As TabPage
	Friend WithEvents Panel1 As Panel
	Friend WithEvents TabControl1 As TabControl
	Friend WithEvents btnCancel As Button
	Friend WithEvents btnSync As Button
	Friend WithEvents picLegend As PictureBox
	Friend WithEvents PictureBox1 As PictureBox
	Friend WithEvents PictureBox2 As PictureBox
End Class
