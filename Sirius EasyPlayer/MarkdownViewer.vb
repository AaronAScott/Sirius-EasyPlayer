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
		Public Property Font As Font
		Public Property Width As Integer

		Public Sub New(text As String, style As FontStyle)
			Me.Text = text
			Me.Style = style
		End Sub
	End Class

	Private Class RenderLine
		Public Property Text As String
		Public Property Style As RenderStyle
		Public Property Runs As List(Of InlineRun)
		Public Property Font As Font
		Public Property Height As Integer

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

	Private _lines As New List(Of RenderLine)
	Private mRawText As String

	'===========================
	' Control setup
	'===========================
	Public Sub New()
		Me.DoubleBuffered = True
		Me.AutoScroll = True
	End Sub

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
		_lines.Clear()

		Dim rawLines = md.Replace(vbCrLf, vbLf).Split(vbLf)

		For Each line In rawLines
			If line.StartsWith("# ") Then
				_lines.Add(New RenderLine(line.Substring(2).Trim(), RenderStyle.Header1))

			ElseIf line.StartsWith("## ") Then
				_lines.Add(New RenderLine(line.Substring(3).Trim(), RenderStyle.Header2))

			ElseIf line.StartsWith("### ") Then
				_lines.Add(New RenderLine(line.Substring(4).Trim(), RenderStyle.Header3))

			ElseIf line.Trim() = "---" Then
				_lines.Add(New RenderLine("", RenderStyle.Rule))

			ElseIf line.TrimStart().StartsWith("- ") OrElse
				  line.TrimStart().StartsWith("* ") OrElse
				  line.TrimStart().StartsWith("+ ") Then

				Dim trimmed = line.TrimStart().Substring(2).Trim()
				_lines.Add(New RenderLine(ParseInline(trimmed), RenderStyle.Bullet))

			Else
				_lines.Add(New RenderLine(ParseInline(line), RenderStyle.Body))
			End If
		Next

		AssignFonts()
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
	' Font assignment (ideal)
	'===========================
	Private Sub AssignFonts()
		Dim baseFont = Me.Font

		For Each line In _lines
			Select Case line.Style

				Case RenderStyle.Header1
					line.Font = New Font(baseFont.FontFamily, baseFont.Size + 6, FontStyle.Bold)

				Case RenderStyle.Header2
					line.Font = New Font(baseFont.FontFamily, baseFont.Size + 3, FontStyle.Bold)

				Case RenderStyle.Header3
					line.Font = New Font(baseFont.FontFamily, baseFont.Size + 1, FontStyle.Bold)

				Case RenderStyle.Body, RenderStyle.Bullet
					line.Font = baseFont

				Case RenderStyle.Rule
					line.Font = baseFont
			End Select

			' Assign fonts to inline runs
			For Each run In line.Runs
				run.Font = New Font(line.Font, run.Style)
			Next
		Next
	End Sub

	'===========================
	' Layout calculation
	'===========================
	Private Sub RecalculateLayout()
		Dim total As Integer = 0

		Using g As Graphics = Me.CreateGraphics()
			For Each line In _lines

				Select Case line.Style

					Case RenderStyle.Rule
						line.Height = 12

					Case RenderStyle.Bullet, RenderStyle.Body
						line.Height = CInt(line.Font.GetHeight(g)) + 2

					Case RenderStyle.Header3
						line.Height = CInt(line.Font.GetHeight(g)) + 2

					Case Else
						line.Height = CInt(line.Font.GetHeight(g)) + 2

				End Select

				total += line.Height
			Next
		End Using

		AutoScrollMinSize = New Size(ClientSize.Width, total)
	End Sub

	'===========================
	' Rendering
	'===========================
	Protected Overrides Sub OnPaint(e As PaintEventArgs)
		MyBase.OnPaint(e)

		Dim g = e.Graphics
		Dim y As Integer = AutoScrollPosition.Y

		For Each line In _lines

			Select Case line.Style

				Case RenderStyle.Rule
					g.DrawLine(Pens.Gray, 0, y + 4, ClientSize.Width, y + 4)

				Case RenderStyle.Header3
					g.DrawString(line.Text, line.Font, Brushes.Black, 0, y)

				Case RenderStyle.Bullet
					Dim bullet = "• "
					Dim indent = 20

					g.DrawString(bullet, line.Font, Brushes.Black, 0, y)

					Dim x As Integer = indent
					For Each run In line.Runs
						g.DrawString(run.Text, run.Font, Brushes.Black, x, y)
						x += CInt(g.MeasureString(run.Text, run.Font).Width)
					Next run

				Case RenderStyle.Body
					Dim x As Integer = 0
					For Each run In line.Runs
						g.DrawString(run.Text, run.Font, Brushes.Black, x, y)
						x += CInt(g.MeasureString(run.Text, run.Font).Width)
					Next

				Case Else
					g.DrawString(line.Text, line.Font, Brushes.Black, 0, y)

			End Select

			y += line.Height
		Next
	End Sub

End Class