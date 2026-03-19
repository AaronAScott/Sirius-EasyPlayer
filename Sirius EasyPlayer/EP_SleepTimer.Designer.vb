<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSleepTimer
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
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSleepTimer))
		Me.Label1 = New System.Windows.Forms.Label()
		Me.Label2 = New System.Windows.Forms.Label()
		Me.Label3 = New System.Windows.Forms.Label()
		Me.GroupBox1 = New System.Windows.Forms.GroupBox()
		Me.rbCloseProgram = New System.Windows.Forms.RadioButton()
		Me.rbClosePlayer = New System.Windows.Forms.RadioButton()
		Me.RadioButton2 = New System.Windows.Forms.RadioButton()
		Me.rbStop = New System.Windows.Forms.RadioButton()
		Me.RadioButton1 = New System.Windows.Forms.RadioButton()
		Me.GroupBox2 = New System.Windows.Forms.GroupBox()
		Me.rbShutdown = New System.Windows.Forms.RadioButton()
		Me.rbHibernate = New System.Windows.Forms.RadioButton()
		Me.RadioButton8 = New System.Windows.Forms.RadioButton()
		Me.rbNothing = New System.Windows.Forms.RadioButton()
		Me.Label4 = New System.Windows.Forms.Label()
		Me.lblElapsedTime = New System.Windows.Forms.Label()
		Me.cmbHours = New System.Windows.Forms.ComboBox()
		Me.cmbMinutes = New System.Windows.Forms.ComboBox()
		Me.Label5 = New System.Windows.Forms.Label()
		Me.Label6 = New System.Windows.Forms.Label()
		Me.btnStart = New System.Windows.Forms.Button()
		Me.btnStop = New System.Windows.Forms.Button()
		Me.btnReset = New System.Windows.Forms.Button()
		Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
		Me.Label7 = New System.Windows.Forms.Label()
		Me.Label8 = New System.Windows.Forms.Label()
		Me.chkFadeOut = New System.Windows.Forms.CheckBox()
		Me.GroupBox1.SuspendLayout()
		Me.GroupBox2.SuspendLayout()
		Me.SuspendLayout()
		'
		'Label1
		'
		Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label1.Location = New System.Drawing.Point(30, 29)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(156, 23)
		Me.Label1.TabIndex = 0
		Me.Label1.Text = "Play &Music For:"
		'
		'Label2
		'
		Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label2.Location = New System.Drawing.Point(30, 109)
		Me.Label2.Name = "Label2"
		Me.Label2.Size = New System.Drawing.Size(169, 23)
		Me.Label2.TabIndex = 5
		Me.Label2.Text = "&When Time is Up:"
		'
		'Label3
		'
		Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label3.Location = New System.Drawing.Point(30, 226)
		Me.Label3.Name = "Label3"
		Me.Label3.Size = New System.Drawing.Size(169, 23)
		Me.Label3.TabIndex = 10
		Me.Label3.Text = "&Then:"
		'
		'GroupBox1
		'
		Me.GroupBox1.Controls.Add(Me.chkFadeOut)
		Me.GroupBox1.Controls.Add(Me.rbCloseProgram)
		Me.GroupBox1.Controls.Add(Me.rbClosePlayer)
		Me.GroupBox1.Controls.Add(Me.RadioButton2)
		Me.GroupBox1.Controls.Add(Me.rbStop)
		Me.GroupBox1.Controls.Add(Me.RadioButton1)
		Me.GroupBox1.Location = New System.Drawing.Point(34, 136)
		Me.GroupBox1.Name = "GroupBox1"
		Me.GroupBox1.Size = New System.Drawing.Size(369, 87)
		Me.GroupBox1.TabIndex = 6
		Me.GroupBox1.TabStop = False
		'
		'rbCloseProgram
		'
		Me.rbCloseProgram.AutoSize = True
		Me.rbCloseProgram.Location = New System.Drawing.Point(20, 54)
		Me.rbCloseProgram.Name = "rbCloseProgram"
		Me.rbCloseProgram.Size = New System.Drawing.Size(93, 17)
		Me.rbCloseProgram.TabIndex = 9
		Me.rbCloseProgram.TabStop = True
		Me.rbCloseProgram.Text = "Close &Program"
		Me.rbCloseProgram.UseVisualStyleBackColor = True
		'
		'rbClosePlayer
		'
		Me.rbClosePlayer.AutoSize = True
		Me.rbClosePlayer.Location = New System.Drawing.Point(20, 34)
		Me.rbClosePlayer.Name = "rbClosePlayer"
		Me.rbClosePlayer.Size = New System.Drawing.Size(83, 17)
		Me.rbClosePlayer.TabIndex = 8
		Me.rbClosePlayer.TabStop = True
		Me.rbClosePlayer.Text = "&Close Player"
		Me.rbClosePlayer.UseVisualStyleBackColor = True
		'
		'RadioButton2
		'
		Me.RadioButton2.AutoSize = True
		Me.RadioButton2.Location = New System.Drawing.Point(20, 35)
		Me.RadioButton2.Name = "RadioButton2"
		Me.RadioButton2.Size = New System.Drawing.Size(90, 17)
		Me.RadioButton2.TabIndex = 1
		Me.RadioButton2.TabStop = True
		Me.RadioButton2.Text = "RadioButton2"
		Me.RadioButton2.UseVisualStyleBackColor = True
		'
		'rbStop
		'
		Me.rbStop.AutoSize = True
		Me.rbStop.Location = New System.Drawing.Point(20, 14)
		Me.rbStop.Name = "rbStop"
		Me.rbStop.Size = New System.Drawing.Size(84, 17)
		Me.rbStop.TabIndex = 7
		Me.rbStop.TabStop = True
		Me.rbStop.Text = "Stop &Playing"
		Me.rbStop.UseVisualStyleBackColor = True
		'
		'RadioButton1
		'
		Me.RadioButton1.AutoSize = True
		Me.RadioButton1.Location = New System.Drawing.Point(20, 14)
		Me.RadioButton1.Name = "RadioButton1"
		Me.RadioButton1.Size = New System.Drawing.Size(90, 17)
		Me.RadioButton1.TabIndex = 0
		Me.RadioButton1.TabStop = True
		Me.RadioButton1.Text = "RadioButton1"
		Me.RadioButton1.UseVisualStyleBackColor = True
		'
		'GroupBox2
		'
		Me.GroupBox2.Controls.Add(Me.rbShutdown)
		Me.GroupBox2.Controls.Add(Me.rbHibernate)
		Me.GroupBox2.Controls.Add(Me.RadioButton8)
		Me.GroupBox2.Controls.Add(Me.rbNothing)
		Me.GroupBox2.Location = New System.Drawing.Point(34, 262)
		Me.GroupBox2.Name = "GroupBox2"
		Me.GroupBox2.Size = New System.Drawing.Size(208, 87)
		Me.GroupBox2.TabIndex = 4
		Me.GroupBox2.TabStop = False
		'
		'rbShutdown
		'
		Me.rbShutdown.AutoSize = True
		Me.rbShutdown.Location = New System.Drawing.Point(20, 54)
		Me.rbShutdown.Name = "rbShutdown"
		Me.rbShutdown.Size = New System.Drawing.Size(121, 17)
		Me.rbShutdown.TabIndex = 14
		Me.rbShutdown.TabStop = True
		Me.rbShutdown.Text = "&Shutdown Computer"
		Me.rbShutdown.UseVisualStyleBackColor = True
		'
		'rbHibernate
		'
		Me.rbHibernate.AutoSize = True
		Me.rbHibernate.Location = New System.Drawing.Point(20, 34)
		Me.rbHibernate.Name = "rbHibernate"
		Me.rbHibernate.Size = New System.Drawing.Size(119, 17)
		Me.rbHibernate.TabIndex = 13
		Me.rbHibernate.TabStop = True
		Me.rbHibernate.Text = "&Hibernate Computer"
		Me.rbHibernate.UseVisualStyleBackColor = True
		'
		'RadioButton8
		'
		Me.RadioButton8.AutoSize = True
		Me.RadioButton8.Location = New System.Drawing.Point(20, 35)
		Me.RadioButton8.Name = "RadioButton8"
		Me.RadioButton8.Size = New System.Drawing.Size(90, 17)
		Me.RadioButton8.TabIndex = 1
		Me.RadioButton8.TabStop = True
		Me.RadioButton8.Text = "RadioButton8"
		Me.RadioButton8.UseVisualStyleBackColor = True
		'
		'rbNothing
		'
		Me.rbNothing.AutoSize = True
		Me.rbNothing.Location = New System.Drawing.Point(20, 14)
		Me.rbNothing.Name = "rbNothing"
		Me.rbNothing.Size = New System.Drawing.Size(79, 17)
		Me.rbNothing.TabIndex = 11
		Me.rbNothing.TabStop = True
		Me.rbNothing.Text = "Do &Nothing"
		Me.rbNothing.UseVisualStyleBackColor = True
		'
		'Label4
		'
		Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label4.Location = New System.Drawing.Point(309, 29)
		Me.Label4.Name = "Label4"
		Me.Label4.Size = New System.Drawing.Size(132, 23)
		Me.Label4.TabIndex = 15
		Me.Label4.Text = "Elapsed Time"
		'
		'lblElapsedTime
		'
		Me.lblElapsedTime.BackColor = System.Drawing.Color.DimGray
		Me.lblElapsedTime.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblElapsedTime.ForeColor = System.Drawing.Color.LightSkyBlue
		Me.lblElapsedTime.Location = New System.Drawing.Point(348, 65)
		Me.lblElapsedTime.Name = "lblElapsedTime"
		Me.lblElapsedTime.Size = New System.Drawing.Size(55, 23)
		Me.lblElapsedTime.TabIndex = 16
		Me.lblElapsedTime.Text = "00:00"
		'
		'cmbHours
		'
		Me.cmbHours.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cmbHours.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmbHours.FormattingEnabled = True
		Me.cmbHours.Items.AddRange(New Object() {"0", "1", "2", "3", "4", "5", "6", "7", "8"})
		Me.cmbHours.Location = New System.Drawing.Point(34, 65)
		Me.cmbHours.Name = "cmbHours"
		Me.cmbHours.Size = New System.Drawing.Size(68, 32)
		Me.cmbHours.TabIndex = 1
		'
		'cmbMinutes
		'
		Me.cmbMinutes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cmbMinutes.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmbMinutes.FormattingEnabled = True
		Me.cmbMinutes.Items.AddRange(New Object() {"0", "15", "30", "45"})
		Me.cmbMinutes.Location = New System.Drawing.Point(174, 65)
		Me.cmbMinutes.Name = "cmbMinutes"
		Me.cmbMinutes.Size = New System.Drawing.Size(68, 32)
		Me.cmbMinutes.TabIndex = 3
		'
		'Label5
		'
		Me.Label5.AutoSize = True
		Me.Label5.Location = New System.Drawing.Point(109, 84)
		Me.Label5.Name = "Label5"
		Me.Label5.Size = New System.Drawing.Size(35, 13)
		Me.Label5.TabIndex = 2
		Me.Label5.Text = "Hours"
		'
		'Label6
		'
		Me.Label6.AutoSize = True
		Me.Label6.Location = New System.Drawing.Point(248, 84)
		Me.Label6.Name = "Label6"
		Me.Label6.Size = New System.Drawing.Size(44, 13)
		Me.Label6.TabIndex = 4
		Me.Label6.Text = "Minutes"
		'
		'btnStart
		'
		Me.btnStart.Location = New System.Drawing.Point(343, 241)
		Me.btnStart.Name = "btnStart"
		Me.btnStart.Size = New System.Drawing.Size(75, 23)
		Me.btnStart.TabIndex = 17
		Me.btnStart.Text = "St&art"
		Me.btnStart.UseVisualStyleBackColor = True
		'
		'btnStop
		'
		Me.btnStop.Location = New System.Drawing.Point(343, 278)
		Me.btnStop.Name = "btnStop"
		Me.btnStop.Size = New System.Drawing.Size(75, 23)
		Me.btnStop.TabIndex = 18
		Me.btnStop.Text = "St&op"
		Me.btnStop.UseVisualStyleBackColor = True
		'
		'btnReset
		'
		Me.btnReset.Location = New System.Drawing.Point(343, 313)
		Me.btnReset.Name = "btnReset"
		Me.btnReset.Size = New System.Drawing.Size(75, 23)
		Me.btnReset.TabIndex = 19
		Me.btnReset.Text = "&Reset"
		Me.btnReset.UseVisualStyleBackColor = True
		'
		'Timer1
		'
		Me.Timer1.Interval = 60000
		'
		'Label7
		'
		Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label7.Location = New System.Drawing.Point(30, 29)
		Me.Label7.Name = "Label7"
		Me.Label7.Size = New System.Drawing.Size(156, 23)
		Me.Label7.TabIndex = 0
		Me.Label7.Text = "Play &Music For:"
		'
		'Label8
		'
		Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label8.Location = New System.Drawing.Point(309, 29)
		Me.Label8.Name = "Label8"
		Me.Label8.Size = New System.Drawing.Size(132, 23)
		Me.Label8.TabIndex = 15
		Me.Label8.Text = "Elapsed Time"
		'
		'chkFadeOut
		'
		Me.chkFadeOut.AutoSize = True
		Me.chkFadeOut.Location = New System.Drawing.Point(169, 15)
		Me.chkFadeOut.Name = "chkFadeOut"
		Me.chkFadeOut.Size = New System.Drawing.Size(194, 17)
		Me.chkFadeOut.TabIndex = 10
		Me.chkFadeOut.Text = "&Fade-out during the last 20 minutes "
		Me.chkFadeOut.UseVisualStyleBackColor = True
		'
		'frmSleepTimer
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(469, 393)
		Me.Controls.Add(Me.btnReset)
		Me.Controls.Add(Me.btnStop)
		Me.Controls.Add(Me.btnStart)
		Me.Controls.Add(Me.Label6)
		Me.Controls.Add(Me.Label5)
		Me.Controls.Add(Me.cmbMinutes)
		Me.Controls.Add(Me.cmbHours)
		Me.Controls.Add(Me.lblElapsedTime)
		Me.Controls.Add(Me.Label8)
		Me.Controls.Add(Me.Label4)
		Me.Controls.Add(Me.GroupBox2)
		Me.Controls.Add(Me.GroupBox1)
		Me.Controls.Add(Me.Label3)
		Me.Controls.Add(Me.Label2)
		Me.Controls.Add(Me.Label7)
		Me.Controls.Add(Me.Label1)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
		Me.Name = "frmSleepTimer"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Play Me to Sleep"
		Me.GroupBox1.ResumeLayout(False)
		Me.GroupBox1.PerformLayout()
		Me.GroupBox2.ResumeLayout(False)
		Me.GroupBox2.PerformLayout()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub

	Friend WithEvents Label1 As Label
	Friend WithEvents Label2 As Label
	Friend WithEvents Label3 As Label
	Friend WithEvents GroupBox1 As GroupBox
	Friend WithEvents rbCloseProgram As RadioButton
	Friend WithEvents rbClosePlayer As RadioButton
	Friend WithEvents RadioButton2 As RadioButton
	Friend WithEvents rbStop As RadioButton
	Friend WithEvents RadioButton1 As RadioButton
	Friend WithEvents GroupBox2 As GroupBox
	Friend WithEvents rbShutdown As RadioButton
	Friend WithEvents rbHibernate As RadioButton
	Friend WithEvents RadioButton8 As RadioButton
	Friend WithEvents rbNothing As RadioButton
	Friend WithEvents Label4 As Label
	Friend WithEvents lblElapsedTime As Label
	Friend WithEvents cmbHours As ComboBox
	Friend WithEvents cmbMinutes As ComboBox
	Friend WithEvents Label5 As Label
	Friend WithEvents Label6 As Label
	Friend WithEvents btnStart As Button
	Friend WithEvents btnStop As Button
	Friend WithEvents btnReset As Button
	Friend WithEvents Timer1 As Timer
	Friend WithEvents Label7 As Label
	Friend WithEvents Label8 As Label
	Friend WithEvents chkFadeOut As CheckBox
End Class
