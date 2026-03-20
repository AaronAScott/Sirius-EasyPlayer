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
		Me.btnCancel = New System.Windows.Forms.Button()
		Me.btnSync = New System.Windows.Forms.Button()
		Me.picLegend = New System.Windows.Forms.PictureBox()
		Me.lstSyncList = New Sirius_EasyPlayer.DoubleBufferedListBox()
		Me.TextBox1 = New System.Windows.Forms.TextBox()
		Me.lblDeviceInfo = New System.Windows.Forms.Label()
		CType(Me.picLegend, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'Label1
		'
		Me.Label1.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label1.Location = New System.Drawing.Point(31, 44)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(297, 23)
		Me.Label1.TabIndex = 0
		Me.Label1.Text = "Music to be Synced"
		Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
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
		Me.btnSync.Enabled = False
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
		Me.picLegend.Size = New System.Drawing.Size(161, 59)
		Me.picLegend.TabIndex = 14
		Me.picLegend.TabStop = False
		'
		'lstSyncList
		'
		Me.lstSyncList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
		Me.lstSyncList.FormattingEnabled = True
		Me.lstSyncList.Location = New System.Drawing.Point(31, 70)
		Me.lstSyncList.Name = "lstSyncList"
		Me.lstSyncList.Size = New System.Drawing.Size(297, 368)
		Me.lstSyncList.TabIndex = 15
		'
		'TextBox1
		'
		Me.TextBox1.Location = New System.Drawing.Point(511, 70)
		Me.TextBox1.Multiline = True
		Me.TextBox1.Name = "TextBox1"
		Me.TextBox1.ReadOnly = True
		Me.TextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
		Me.TextBox1.Size = New System.Drawing.Size(261, 368)
		Me.TextBox1.TabIndex = 16
		Me.TextBox1.WordWrap = False
		'
		'lblDeviceInfo
		'
		Me.lblDeviceInfo.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblDeviceInfo.Location = New System.Drawing.Point(511, 44)
		Me.lblDeviceInfo.Name = "lblDeviceInfo"
		Me.lblDeviceInfo.Size = New System.Drawing.Size(261, 23)
		Me.lblDeviceInfo.TabIndex = 17
		'
		'frmSync
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(800, 450)
		Me.Controls.Add(Me.lblDeviceInfo)
		Me.Controls.Add(Me.TextBox1)
		Me.Controls.Add(Me.lstSyncList)
		Me.Controls.Add(Me.picLegend)
		Me.Controls.Add(Me.btnCancel)
		Me.Controls.Add(Me.btnSync)
		Me.Controls.Add(Me.Label1)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
		Me.Name = "frmSync"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Sync to Mobile Device"
		CType(Me.picLegend, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub

	Friend WithEvents Label1 As Label
	Friend WithEvents btnCancel As Button
	Friend WithEvents btnSync As Button
	Friend WithEvents picLegend As PictureBox
	Friend WithEvents lstSyncList As DoubleBufferedListBox
	Friend WithEvents TextBox1 As TextBox
	Friend WithEvents lblDeviceInfo As Label
End Class
