<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ColorPicker
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ColorPicker))
        Me.ListBox1 = New System.Windows.Forms.ListBox()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOkay = New System.Windows.Forms.Button()
        Me.ColorDialog1 = New System.Windows.Forms.ColorDialog()
        Me.btnCustom = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'ListBox1
        '
        Me.ListBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.ListBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.ItemHeight = 18
        Me.ListBox1.Location = New System.Drawing.Point(0, 2)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(195, 238)
        Me.ListBox1.TabIndex = 0
        '
        'btnCancel
        '
        Me.btnCancel.BackColor = System.Drawing.SystemColors.Control
        Me.btnCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnCancel.Image = CType(resources.GetObject("btnCancel.Image"), System.Drawing.Image)
        Me.btnCancel.Location = New System.Drawing.Point(212, 176)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnCancel.Size = New System.Drawing.Size(63, 63)
        Me.btnCancel.TabIndex = 3
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'btnOkay
        '
        Me.btnOkay.BackColor = System.Drawing.SystemColors.Control
        Me.btnOkay.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnOkay.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOkay.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnOkay.Image = CType(resources.GetObject("btnOkay.Image"), System.Drawing.Image)
        Me.btnOkay.Location = New System.Drawing.Point(212, 2)
        Me.btnOkay.Name = "btnOkay"
        Me.btnOkay.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnOkay.Size = New System.Drawing.Size(63, 63)
        Me.btnOkay.TabIndex = 1
        Me.btnOkay.Text = "&Okay"
        Me.btnOkay.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnOkay.UseVisualStyleBackColor = False
        '
        'ColorDialog1
        '
        Me.ColorDialog1.AnyColor = True
        Me.ColorDialog1.FullOpen = True
        '
        'btnCustom
        '
        Me.btnCustom.BackColor = System.Drawing.SystemColors.Control
        Me.btnCustom.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnCustom.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnCustom.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCustom.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCustom.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnCustom.Image = CType(resources.GetObject("btnCustom.Image"), System.Drawing.Image)
        Me.btnCustom.Location = New System.Drawing.Point(212, 89)
        Me.btnCustom.Name = "btnCustom"
        Me.btnCustom.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnCustom.Size = New System.Drawing.Size(63, 63)
        Me.btnCustom.TabIndex = 2
        Me.btnCustom.Text = "C&ustom"
        Me.btnCustom.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnCustom.UseVisualStyleBackColor = False
        '
        'ColorPicker
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(291, 254)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnCustom)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOkay)
        Me.Controls.Add(Me.ListBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ColorPicker"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "ColorPicker"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Public WithEvents btnCancel As System.Windows.Forms.Button
    Public WithEvents btnOkay As System.Windows.Forms.Button
    Friend WithEvents ColorDialog1 As System.Windows.Forms.ColorDialog
    Public WithEvents btnCustom As System.Windows.Forms.Button
End Class
