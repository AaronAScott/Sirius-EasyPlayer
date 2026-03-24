Imports System.ComponentModel
Imports System.Drawing
Imports System.Text.RegularExpressions

<ToolboxItem(True)>
Public Class MarkdownViewer
     Inherits Panel

     '===========================
     ' Render structures
     '===========================
     Private Class InlineRun
          Public Property Text As String
          Public Property Style As FontStyle

          Public Sub New(text As String, style As FontStyle)
               Me.Text = text
               Me.Style = style
          End Sub
     End Class

     Private Class RenderLine
          Public Property Text As String
          Public Property Style As RenderStyle
          Public Property Runs As List(Of InlineRun)
          Public Property Height As Integer
          Public Property IsContinuation As Boolean   ' ← add this

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
     Private Enum RenderStyle
          Header1
          Header2
          Header3
          Body
          Rule
          Bullet
     End Enum

     Private _parsedLines As New List(Of RenderLine)   ' source (unwrapped)
     Private _lines As New List(Of RenderLine)         ' wrapped, rendered
     Private mRawText As String

     ' static fonts based on style
     Private _fontBody As Font
     Private _fontHeader1 As Font
     Private _fontHeader2 As Font
     Private _fontHeader3 As Font
     Private _fontBullet As Font

     '===========================
     ' Control setup
     '===========================
     Public Sub New()
          Me.DoubleBuffered = True
          Me.AutoScroll = True

          Dim baseFont = New Font("Times New Roman", 11)
          _fontBody = baseFont
          _fontBullet = baseFont
          _fontHeader1 = New Font(baseFont.FontFamily, baseFont.Size + 6, FontStyle.Bold)
          _fontHeader2 = New Font(baseFont.FontFamily, baseFont.Size + 3, FontStyle.Bold)
          _fontHeader3 = New Font(baseFont.FontFamily, baseFont.Size + 1, FontStyle.Bold)
     End Sub

     Private Function GetLineFont(style As RenderStyle) As Font
          Select Case style
               Case RenderStyle.Header1 : Return _fontHeader1
               Case RenderStyle.Header2 : Return _fontHeader2
               Case RenderStyle.Header3 : Return _fontHeader3
               Case RenderStyle.Bullet : Return _fontBullet
               Case Else : Return _fontBody
          End Select
     End Function

     Private Function GetRunFont(lineStyle As RenderStyle, runStyle As FontStyle) As Font
          Dim base = GetLineFont(lineStyle)
          Return New Font(base, runStyle)
     End Function

     Public Sub LoadFile(path As String)
          Dim text = IO.File.ReadAllText(path)
          mRawText = text
          ParseMarkdown(text)
          RecalculateLayout()
          Me.Invalidate()
     End Sub

     Public Property RawText As String
          Get
               Return mRawText
          End Get
          Set(value As String)
               mRawText = value
               If Not DesignMode AndAlso Not String.IsNullOrEmpty(value) Then
                    ParseMarkdown(value)
                    RecalculateLayout()
                    Me.Invalidate()
               End If
          End Set
     End Property

     '===========================
     ' Markdown parsing
     '===========================
     Private Sub ParseMarkdown(md As String)
          _parsedLines.Clear()

          Dim rawLines = md.Replace(vbCrLf, vbLf).Split(vbLf)

          For Each line In rawLines
               If line.StartsWith("# ") Then
                    _parsedLines.Add(New RenderLine(line.Substring(2).Trim(), RenderStyle.Header1))

               ElseIf line.StartsWith("## ") Then
                    _parsedLines.Add(New RenderLine(line.Substring(3).Trim(), RenderStyle.Header2))

               ElseIf line.StartsWith("### ") Then
                    _parsedLines.Add(New RenderLine(line.Substring(4).Trim(), RenderStyle.Header3))

               ElseIf line.Trim() = "---" Then
                    _parsedLines.Add(New RenderLine("", RenderStyle.Rule))

               ElseIf line.TrimStart().StartsWith("- ") OrElse
                      line.TrimStart().StartsWith("* ") OrElse
                      line.TrimStart().StartsWith("+ ") Then

                    Dim trimmed = line.TrimStart().Substring(2).Trim()
                    _parsedLines.Add(New RenderLine(ParseInline(trimmed), RenderStyle.Bullet))

               Else
                    _parsedLines.Add(New RenderLine(ParseInline(line), RenderStyle.Body))
               End If
          Next
     End Sub

     '===========================
     ' Inline parsing
     '===========================
     Private Function ParseInline(text As String) As List(Of InlineRun)
          Dim runs As New List(Of InlineRun)

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

     '===========================
     ' Layout calculation
     '===========================
     Private Sub RecalculateLayout()
          If _parsedLines Is Nothing OrElse _parsedLines.Count = 0 Then Exit Sub
          If ClientSize.Width <= 0 Then Exit Sub

          Dim newLines As New List(Of RenderLine)

          Using g As Graphics = Me.CreateGraphics()
               For Each line In _parsedLines
                    If line.Style = RenderStyle.Body OrElse line.Style = RenderStyle.Bullet Then
                         newLines.AddRange(WrapLine(line, g, Me.ClientSize.Width - 10))
                    Else
                         newLines.Add(line)
                    End If
               Next

               _lines = newLines

               Dim total As Integer = 0
               For Each line In _lines
                    Select Case line.Style
                         Case RenderStyle.Rule
                              line.Height = 12
                         Case Else
                              line.Height = CInt(GetLineFont(line.Style).GetHeight(g)) + 2
                    End Select
                    total += line.Height
               Next

               AutoScrollMinSize = New Size(ClientSize.Width, total)
          End Using
     End Sub

     Private Function WrapLine(line As RenderLine, g As Graphics, maxWidth As Integer) As List(Of RenderLine)
          Dim wrapped As New List(Of RenderLine)
          Dim currentRuns As New List(Of InlineRun)
          Dim currentWidth As Integer = 0

          ' Preserve blank lines
          If line.Text.Trim() = "" Then
               Return New List(Of RenderLine) From {
            New RenderLine(New List(Of InlineRun) From {
                New InlineRun("", FontStyle.Regular)
            }, line.Style)
        }
          End If

          For Each run In line.Runs
               Dim runFont = GetRunFont(line.Style, run.Style)

               ' Split into words + spaces
               Dim tokens = Regex.Matches(run.Text, "\S+|\s+").
                      Cast(Of Match)().
                      Select(Function(m) m.Value)

               For Each token In tokens
                    Dim tokenWidth = CInt(g.MeasureString(token, runFont).Width)

                    ' Wrap if needed
                    If currentWidth + tokenWidth > maxWidth AndAlso currentRuns.Count > 0 Then
                         wrapped.Add(New RenderLine(New List(Of InlineRun)(currentRuns), line.Style))
                         wrapped.Last().IsContinuation = (wrapped.Count > 1 AndAlso line.Style = RenderStyle.Bullet)
                         currentRuns.Clear()
                         currentWidth = 0
                    End If

                    currentRuns.Add(New InlineRun(token, run.Style))
                    currentWidth += tokenWidth
               Next
          Next

          ' Final line
          If currentRuns.Count > 0 Then
               wrapped.Add(New RenderLine(currentRuns, line.Style))
               wrapped.Last().IsContinuation = (wrapped.Count > 1 AndAlso line.Style = RenderStyle.Bullet)
          End If

          Return wrapped
     End Function
     Protected Overrides Sub OnResize(e As EventArgs)
          MyBase.OnResize(e)

          If Not DesignMode AndAlso _parsedLines IsNot Nothing AndAlso _parsedLines.Count > 0 Then
               BeginInvoke(Sub()
                                RecalculateLayout()
                                Invalidate()
                           End Sub)
          End If
     End Sub
     '===========================
     ' Rendering
     '===========================
     Protected Overrides Sub OnPaint(e As PaintEventArgs)
          MyBase.OnPaint(e)

          Dim g = e.Graphics
          Dim y As Integer = AutoScrollPosition.Y

          For Each line In _lines
               Dim lineFont = GetLineFont(line.Style)

               Select Case line.Style

                    Case RenderStyle.Rule
                         g.DrawLine(Pens.Gray, 0, y + 4, ClientSize.Width, y + 4)

                    Case RenderStyle.Header3, RenderStyle.Header2, RenderStyle.Header1
                         g.DrawString(line.Text, lineFont, Brushes.Black, 0, y)

                    Case RenderStyle.Bullet
                         Dim bullet = "• "
                         Dim indent = 20

                         If Not line.IsContinuation Then
                              ' First visual line of bullet
                              g.DrawString(bullet, lineFont, Brushes.Black, 0, y)
                         End If

                         Dim x As Integer = If(line.IsContinuation, 0, indent)

                         For Each run In line.Runs
                              Dim runFont = GetRunFont(line.Style, run.Style)
                              g.DrawString(run.Text, runFont, Brushes.Black, x, y)
                              x += CInt(g.MeasureString(run.Text, runFont).Width)
                         Next
                    Case RenderStyle.Body
                         Dim x As Integer = 0
                         For Each run In line.Runs
                              Dim runFont = GetRunFont(line.Style, run.Style)
                              g.DrawString(run.Text, runFont, Brushes.Black, x, y)
                              x += CInt(g.MeasureString(run.Text, runFont).Width)
                         Next

                    Case Else
                         g.DrawString(line.Text, lineFont, Brushes.Black, 0, y)

               End Select

               y += line.Height
          Next
     End Sub

End Class