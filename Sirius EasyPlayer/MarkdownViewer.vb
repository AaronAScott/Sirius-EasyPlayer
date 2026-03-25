Imports System.ComponentModel
Imports System.Drawing
Imports System.Text.RegularExpressions

<ToolboxItem(True)>
Public Class MarkdownViewer
	Inherits Panel
	'*******************************************************************
	' Markdown viewer for Visual Basic programs.
	' MARKDOWNVIEWER.VB
	' Written: March 2026
	' Programmer: Aaron Scott
	' Copyright 2026 Sirius Software All Rights Reserved
	'*******************************************************************


	'*******************************************************************

	' Rendering structures

	'*******************************************************************

	' This class represents a portion of a line to be represented in
	' a fashion incidated by the MD tags.

	Private Class InlineRun
		Public Property Text As String
		Public Property Style As FontStyle

		Public Sub New(text As String, style As FontStyle)
			Me.Text = text
			Me.Style = style
		End Sub
	End Class

	' This class represents one line of the MD file.

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

	' Enum for the types of RenderLines a file may contain.
	Private Enum RenderStyle
		Header1
		Header2
		Header3
		Body
		Rule
		Bullet
	End Enum

	' Declare variables local to this class.

	Private _parsedLines As New List(Of RenderLine)   ' source (unwrapped)
	Private _lines As New List(Of RenderLine)         ' wrapped, rendered
	Private mRawText As String

	' static fonts based on style
	Private _fontBody As Font
	Private _fontHeader1 As Font
	Private _fontHeader2 As Font
	Private _fontHeader3 As Font
	Private _fontBullet As Font

	'*******************************************************************

	' Constructor.

	'*******************************************************************
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
	'*******************************************************************

	' Function to return the font for displaying a line.

	'*******************************************************************
	Private Function GetLineFont(style As RenderStyle) As Font
		Select Case style
			Case RenderStyle.Header1 : Return _fontHeader1
			Case RenderStyle.Header2 : Return _fontHeader2
			Case RenderStyle.Header3 : Return _fontHeader3
			Case RenderStyle.Bullet : Return _fontBullet
			Case Else : Return _fontBody
		End Select
	End Function

	'*******************************************************************

	' Function to return the font for a run, for example italic or bold.

	'*******************************************************************
	Private Function GetRunFont(lineStyle As RenderStyle, runStyle As FontStyle) As Font
		Dim base = GetLineFont(lineStyle)
		Return New Font(base, runStyle)
	End Function

	'*******************************************************************

	' Sub to load an .md file and display it.

	'*******************************************************************
	Public Sub LoadFile(path As String)
		Try
			Dim text = IO.File.ReadAllText(path)
			mRawText = text
			ParseMarkdown(text)
			RecalculateLayout()
			Me.Invalidate()
		Catch ex As Exception
		End Try

	End Sub
	'*******************************************************************

	' Property to set or retrieve the actual .md file text.

	'*******************************************************************

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

	'*******************************************************************

	' Markdown parsing

	'*******************************************************************
	Private Sub ParseMarkdown(md As String)

		If String.IsNullOrEmpty(md) Then
			_parsedLines.Clear()
			_lines.Clear()
			Return
		End If

		Dim rawLines = md.Replace(vbCrLf, vbLf).Split(vbLf)

		For Each line In rawLines

			' Look for heading codes.

			If line.StartsWith("# ") Then ' Heading 1
				_parsedLines.Add(New RenderLine(line.Substring(2).Trim(), RenderStyle.Header1))

			ElseIf line.StartsWith("## ") Then ' Heading 2
				_parsedLines.Add(New RenderLine(line.Substring(3).Trim(), RenderStyle.Header2))

			ElseIf line.StartsWith("### ") Then ' Heading 3
				_parsedLines.Add(New RenderLine(line.Substring(4).Trim(), RenderStyle.Header3))

			ElseIf line.Trim() = "---" Then ' Rule
				_parsedLines.Add(New RenderLine("", RenderStyle.Rule))

				' Look for bullet codes.

			ElseIf line.TrimStart().StartsWith("- ") OrElse
				  line.TrimStart().StartsWith("* ") OrElse
				  line.TrimStart().StartsWith("+ ") Then

				Dim trimmed = line.TrimStart().Substring(2).Trim()
				_parsedLines.Add(New RenderLine(ParseInline(trimmed), RenderStyle.Bullet))

				' Otherwise, just plain text.

			Else
				_parsedLines.Add(New RenderLine(ParseInline(line), RenderStyle.Body))
			End If
		Next
	End Sub

	'*******************************************************************

	' Inline parsing

	'*******************************************************************
	Private Function ParseInline(text As String) As List(Of InlineRun)

		' Declare variables.

		Dim runs As New List(Of InlineRun)
		Dim pattern = "(\*\*.*?\*\*|\*.*?\*)"
		Dim parts = Regex.Split(text, pattern)

		' Look for parts of a line to be represented with bold or italics.

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

	'*******************************************************************

	' Layout calculation.  This will prepare lines for rendering,
	' including calculating the line height, lines that must be
	' wrapped around and so on.

	'*******************************************************************
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
	'*******************************************************************

	' Function to return a portion of a line that must be wrapped around.

	'*******************************************************************
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
	'*******************************************************************

	' In case the control is resized, we must recalculate the
	' layout and re-draw the display.

	'*******************************************************************
	Protected Overrides Sub OnResize(e As EventArgs)
		MyBase.OnResize(e)

		If Not DesignMode AndAlso _parsedLines IsNot Nothing AndAlso _parsedLines.Count > 0 Then
			BeginInvoke(Sub()
						  RecalculateLayout()
						  Invalidate()
					  End Sub)
		End If
	End Sub
	'*******************************************************************

	' Paint the display: the actual rendering of the .md text takes
	' place here.

	'*******************************************************************
	Protected Overrides Sub OnPaint(e As PaintEventArgs)

		MyBase.OnPaint(e)

		If DesignMode OrElse _lines Is Nothing OrElse _lines.Count = 0 Then Return

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