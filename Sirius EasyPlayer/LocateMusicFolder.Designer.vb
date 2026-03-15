<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLocateMusicFolder
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmLocateMusicFolder))
		Me.TreeView1 = New System.Windows.Forms.TreeView()
		Me.Label1 = New System.Windows.Forms.Label()
		Me.Label2 = New System.Windows.Forms.Label()
		Me.Label3 = New System.Windows.Forms.Label()
		Me.Label4 = New System.Windows.Forms.Label()
		Me.picStep1Success = New System.Windows.Forms.PictureBox()
		Me.picStep2Success = New System.Windows.Forms.PictureBox()
		Me.picStep3Success = New System.Windows.Forms.PictureBox()
		Me.picStep4Success = New System.Windows.Forms.PictureBox()
		Me.Label5 = New System.Windows.Forms.Label()
		Me.btnDone = New System.Windows.Forms.Button()
		Me.btnCancel = New System.Windows.Forms.Button()
		Me.picStep1Failure = New System.Windows.Forms.PictureBox()
		Me.picStep2Failure = New System.Windows.Forms.PictureBox()
		Me.picStep3Failure = New System.Windows.Forms.PictureBox()
		Me.picStep4Failure = New System.Windows.Forms.PictureBox()
		CType(Me.picStep1Success, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.picStep2Success, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.picStep3Success, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.picStep4Success, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.picStep1Failure, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.picStep2Failure, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.picStep3Failure, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.picStep4Failure, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'TreeView1
		'
		Me.TreeView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.TreeView1.Location = New System.Drawing.Point(21, 49)
		Me.TreeView1.Name = "TreeView1"
		Me.TreeView1.Size = New System.Drawing.Size(412, 372)
		Me.TreeView1.TabIndex = 0
		'
		'Label1
		'
		Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label1.Location = New System.Drawing.Point(494, 49)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(268, 23)
		Me.Label1.TabIndex = 1
		Me.Label1.Text = "Locate Music Folder"
		'
		'Label2
		'
		Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label2.Location = New System.Drawing.Point(494, 93)
		Me.Label2.Name = "Label2"
		Me.Label2.Size = New System.Drawing.Size(268, 23)
		Me.Label2.TabIndex = 2
		Me.Label2.Text = "Validate Music Folder"
		'
		'Label3
		'
		Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label3.Location = New System.Drawing.Point(494, 137)
		Me.Label3.Name = "Label3"
		Me.Label3.Size = New System.Drawing.Size(268, 23)
		Me.Label3.TabIndex = 3
		Me.Label3.Text = "Verify Library"
		'
		'Label4
		'
		Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label4.Location = New System.Drawing.Point(494, 181)
		Me.Label4.Name = "Label4"
		Me.Label4.Size = New System.Drawing.Size(268, 23)
		Me.Label4.TabIndex = 4
		Me.Label4.Text = "Import Library"
		'
		'picStep1Success
		'
		Me.picStep1Success.Image = CType(resources.GetObject("picStep1Success.Image"), System.Drawing.Image)
		Me.picStep1Success.Location = New System.Drawing.Point(456, 40)
		Me.picStep1Success.Name = "picStep1Success"
		Me.picStep1Success.Size = New System.Drawing.Size(32, 32)
		Me.picStep1Success.TabIndex = 5
		Me.picStep1Success.TabStop = False
		Me.picStep1Success.Visible = False
		'
		'picStep2Success
		'
		Me.picStep2Success.Image = CType(resources.GetObject("picStep2Success.Image"), System.Drawing.Image)
		Me.picStep2Success.Location = New System.Drawing.Point(456, 84)
		Me.picStep2Success.Name = "picStep2Success"
		Me.picStep2Success.Size = New System.Drawing.Size(32, 32)
		Me.picStep2Success.TabIndex = 6
		Me.picStep2Success.TabStop = False
		Me.picStep2Success.Visible = False
		'
		'picStep3Success
		'
		Me.picStep3Success.Image = CType(resources.GetObject("picStep3Success.Image"), System.Drawing.Image)
		Me.picStep3Success.Location = New System.Drawing.Point(456, 128)
		Me.picStep3Success.Name = "picStep3Success"
		Me.picStep3Success.Size = New System.Drawing.Size(32, 32)
		Me.picStep3Success.TabIndex = 7
		Me.picStep3Success.TabStop = False
		Me.picStep3Success.Visible = False
		'
		'picStep4Success
		'
		Me.picStep4Success.Image = CType(resources.GetObject("picStep4Success.Image"), System.Drawing.Image)
		Me.picStep4Success.Location = New System.Drawing.Point(456, 172)
		Me.picStep4Success.Name = "picStep4Success"
		Me.picStep4Success.Size = New System.Drawing.Size(32, 32)
		Me.picStep4Success.TabIndex = 8
		Me.picStep4Success.TabStop = False
		Me.picStep4Success.Visible = False
		'
		'Label5
		'
		Me.Label5.Location = New System.Drawing.Point(21, 13)
		Me.Label5.Name = "Label5"
		Me.Label5.Size = New System.Drawing.Size(412, 33)
		Me.Label5.TabIndex = 9
		Me.Label5.Text = "Select the drive and folder in which your music is located.  You may double-click" &
	" on the selected folder to set it as your new music folder."
		'
		'btnDone
		'
		Me.btnDone.Enabled = False
		Me.btnDone.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Lime
		Me.btnDone.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
		Me.btnDone.FlatStyle = System.Windows.Forms.FlatStyle.Flat
		Me.btnDone.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.btnDone.Location = New System.Drawing.Point(456, 380)
		Me.btnDone.Name = "btnDone"
		Me.btnDone.Size = New System.Drawing.Size(127, 41)
		Me.btnDone.TabIndex = 10
		Me.btnDone.Text = "Done"
		Me.btnDone.UseVisualStyleBackColor = True
		'
		'btnCancel
		'
		Me.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightBlue
		Me.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
		Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
		Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.btnCancel.Location = New System.Drawing.Point(635, 380)
		Me.btnCancel.Name = "btnCancel"
		Me.btnCancel.Size = New System.Drawing.Size(127, 41)
		Me.btnCancel.TabIndex = 11
		Me.btnCancel.Text = "Cancel"
		Me.btnCancel.UseVisualStyleBackColor = True
		'
		'picStep1Failure
		'
		Me.picStep1Failure.Image = CType(resources.GetObject("picStep1Failure.Image"), System.Drawing.Image)
		Me.picStep1Failure.Location = New System.Drawing.Point(456, 40)
		Me.picStep1Failure.Name = "picStep1Failure"
		Me.picStep1Failure.Size = New System.Drawing.Size(32, 32)
		Me.picStep1Failure.TabIndex = 12
		Me.picStep1Failure.TabStop = False
		Me.picStep1Failure.Visible = False
		'
		'picStep2Failure
		'
		Me.picStep2Failure.Image = CType(resources.GetObject("picStep2Failure.Image"), System.Drawing.Image)
		Me.picStep2Failure.Location = New System.Drawing.Point(456, 84)
		Me.picStep2Failure.Name = "picStep2Failure"
		Me.picStep2Failure.Size = New System.Drawing.Size(32, 32)
		Me.picStep2Failure.TabIndex = 13
		Me.picStep2Failure.TabStop = False
		Me.picStep2Failure.Visible = False
		'
		'picStep3Failure
		'
		Me.picStep3Failure.Image = CType(resources.GetObject("picStep3Failure.Image"), System.Drawing.Image)
		Me.picStep3Failure.Location = New System.Drawing.Point(456, 128)
		Me.picStep3Failure.Name = "picStep3Failure"
		Me.picStep3Failure.Size = New System.Drawing.Size(32, 32)
		Me.picStep3Failure.TabIndex = 14
		Me.picStep3Failure.TabStop = False
		Me.picStep3Failure.Visible = False
		'
		'picStep4Failure
		'
		Me.picStep4Failure.Image = CType(resources.GetObject("picStep4Failure.Image"), System.Drawing.Image)
		Me.picStep4Failure.Location = New System.Drawing.Point(456, 172)
		Me.picStep4Failure.Name = "picStep4Failure"
		Me.picStep4Failure.Size = New System.Drawing.Size(32, 32)
		Me.picStep4Failure.TabIndex = 15
		Me.picStep4Failure.TabStop = False
		Me.picStep4Failure.Visible = False
		'
		'frmLocateMusicFolder
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(800, 450)
		Me.Controls.Add(Me.picStep4Failure)
		Me.Controls.Add(Me.picStep3Failure)
		Me.Controls.Add(Me.picStep2Failure)
		Me.Controls.Add(Me.picStep1Failure)
		Me.Controls.Add(Me.btnCancel)
		Me.Controls.Add(Me.btnDone)
		Me.Controls.Add(Me.Label5)
		Me.Controls.Add(Me.picStep4Success)
		Me.Controls.Add(Me.picStep3Success)
		Me.Controls.Add(Me.picStep2Success)
		Me.Controls.Add(Me.picStep1Success)
		Me.Controls.Add(Me.Label4)
		Me.Controls.Add(Me.Label3)
		Me.Controls.Add(Me.Label2)
		Me.Controls.Add(Me.Label1)
		Me.Controls.Add(Me.TreeView1)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmLocateMusicFolder"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Locate Music Folder"
		CType(Me.picStep1Success, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.picStep2Success, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.picStep3Success, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.picStep4Success, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.picStep1Failure, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.picStep2Failure, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.picStep3Failure, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.picStep4Failure, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)

	End Sub

	Friend WithEvents TreeView1 As TreeView
	Friend WithEvents Label1 As Label
	Friend WithEvents Label2 As Label
	Friend WithEvents Label3 As Label
	Friend WithEvents Label4 As Label
	Friend WithEvents picStep1Success As PictureBox
	Friend WithEvents picStep2Success As PictureBox
	Friend WithEvents picStep3Success As PictureBox
	Friend WithEvents picStep4Success As PictureBox
	Friend WithEvents Label5 As Label
	Friend WithEvents btnDone As Button
	Friend WithEvents btnCancel As Button
	Friend WithEvents picStep1Failure As PictureBox
	Friend WithEvents picStep2Failure As PictureBox
	Friend WithEvents picStep3Failure As PictureBox
	Friend WithEvents picStep4Failure As PictureBox
End Class
