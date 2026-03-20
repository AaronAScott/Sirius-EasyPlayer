Imports System.ComponentModel
Imports System.Drawing
Imports System.Text.RegularExpressions

<ToolboxItem(True)>
Public Class MarkdownViewer
     Inherits Panel
     Private Class RenderLine
          Public Property Text As String
          Public Property Style As RenderStyle
          Public Property Runs As List(Of InlineRun)

          Public Sub New(text As String, style As RenderStyle)
               Me.Text = text
               Me.Style = style
               Me.Runs = New List(Of InlineRun) From {
            New InlineRun(text, FontStyle.Regular)
        }
          End Sub

          Public Sub New(runs As List(Of InlineRun), style As RenderStyle)
               Me.Runs = runs
               Me.Style = style
               Me.Text = String.Join("", runs.Select(Function(r) r.Text))
          End Sub
     End Class
     Private Class InlineRun
          Public Property Text As String
          Public Property Style As FontStyle

          Public Sub New(text As String, style As FontStyle)
               Me.Text = text
               Me.Style = style
          End Sub
     End Class

     Private Enum RenderStyle
          Header1
          Header2
          Body
          Rule
          Bullet
     End Enum
     Private _lines As New List(Of RenderLine)

     Public Sub New()
          Me.DoubleBuffered = True
          Me.AutoScroll = True
     End Sub
     Public Sub LoadMarkdown(path As String)
          Dim text = IO.File.ReadAllText(path)
          ParseMarkdown(text)
          Me.Invalidate()
     End Sub

     Private Sub ParseMarkdown(md As String)
          _lines.Clear()

          Dim rawLines = md.Replace(vbCrLf, vbLf).Split(vbLf)

          For Each line In rawLines
               If line.StartsWith("# ") Then
                    _lines.Add(New RenderLine(line.Substring(2).Trim(), RenderStyle.Header1))
               ElseIf line.StartsWith("## ") Then
                    _lines.Add(New RenderLine(line.Substring(3).Trim(), RenderStyle.Header2))
               ElseIf line.Trim() = "---" Then
                    _lines.Add(New RenderLine("", RenderStyle.Rule))
               ElseIf line.TrimStart().StartsWith("- ") OrElse line.TrimStart().StartsWith("* ") OrElse line.TrimStart().StartsWith("+ ") Then
                    Dim trimmed = line.TrimStart().Substring(2).Trim()
                    _lines.Add(New RenderLine(ParseInline(trimmed), RenderStyle.Bullet))
               Else
                    _lines.Add(New RenderLine(ParseInline(line), RenderStyle.Body))
               End If
          Next
     End Sub

     Private Function ParseInline(text As String) As List(Of InlineRun)
          Dim runs As New List(Of InlineRun)

          ' Bold: **text**
          ' Italic: *text*
          Dim pattern = "(\*\*.*?\*\*|\*.*?\*)"
          Dim parts = Regex.Split(text, pattern)

          For Each part In parts
               If part.StartsWith("**") AndAlso part.EndsWith("**") Then
                    runs.Add(New InlineRun(part.Substring(2, part.Length - 4), FontStyle.Bold))
               ElseIf part.StartsWith("*") AndAlso part.EndsWith("*") Then
                    runs.Add(New InlineRun(part.Substring(1, part.Length - 2), FontStyle.Italic))
               Else
                    runs.Add(New InlineRun(part, FontStyle.Regular))
               End If
          Next

          Return runs
     End Function

     Protected Overrides Sub OnPaint(e As PaintEventArgs)
          MyBase.OnPaint(e)

          Dim y As Integer = 0
          Dim g = e.Graphics
          g.Clear(Me.BackColor)

          For Each line In _lines
               Select Case line.Style
                    Case RenderStyle.Header1
                         Using f As New Font(Me.Font.FontFamily, Me.Font.Size + 6, FontStyle.Bold)
                              g.DrawString(line.Text, f, Brushes.Black, 0, y)
                              y += CInt(f.GetHeight(g)) + 6
                         End Using

                    Case RenderStyle.Header2
                         Using f As New Font(Me.Font.FontFamily, Me.Font.Size + 3, FontStyle.Bold)
                              g.DrawString(line.Text, f, Brushes.Black, 0, y)
                              y += CInt(f.GetHeight(g)) + 4
                         End Using

                    Case RenderStyle.Rule
                         g.DrawLine(Pens.Gray, 0, y + 4, Me.Width, y + 4)
                         y += 12

                    Case RenderStyle.Body
                         Dim x As Integer = 0
                         For Each run In line.Runs
                              Using f As New Font(Me.Font, run.Style)
                                   g.DrawString(run.Text, f, Brushes.Black, x, y)
                                   x += CInt(g.MeasureString(run.Text, f).Width)
                              End Using
                         Next
                         y += CInt(Me.Font.GetHeight(g)) + 2
                    Case RenderStyle.Bullet
                         Dim bullet As String = "• "
                         Dim indent As Integer = 20

                         Using f As New Font(Me.Font, FontStyle.Regular)
                              g.DrawString(bullet, f, Brushes.Black, 0, y)

                              Dim x As Integer = indent
                              For Each run In line.Runs
                                   Using rf As New Font(Me.Font, run.Style)
                                        g.DrawString(run.Text, rf, Brushes.Black, x, y)
                                        x += CInt(g.MeasureString(run.Text, rf).Width)
                                   End Using
                              Next
                              y += CInt(Me.Font.GetHeight(g)) + 2
                         End Using
               End Select
          Next

          Me.AutoScrollMinSize = New Size(Me.Width, y)
     End Sub


End Class