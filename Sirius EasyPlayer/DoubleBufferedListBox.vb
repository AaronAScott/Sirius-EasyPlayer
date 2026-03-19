Public Class DoubleBufferedListBox
     Inherits ListBox

     Public Sub New()
          MyBase.New()
          Me.DrawMode = DrawMode.OwnerDrawFixed

          Me.SetStyle(ControlStyles.OptimizedDoubleBuffer Or
                      ControlStyles.AllPaintingInWmPaint, True)
          Me.UpdateStyles()
     End Sub

End Class

