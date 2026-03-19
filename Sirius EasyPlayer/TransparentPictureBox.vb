Public Class TransparentPictureBox
     Inherits PictureBox

     Protected Overrides Sub OnPaintBackground(pevent As PaintEventArgs)
          ' Do nothing — prevents white/gray background
     End Sub

End Class