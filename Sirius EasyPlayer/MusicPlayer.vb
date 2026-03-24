Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.IO
Imports System.Security.Cryptography
Imports System.Windows.Forms
Imports System.Windows.Forms.Design
Imports System.Xml
Imports Microsoft.Win32
Imports TagLib
Imports WMPLib
'*******************************************************

' Media Player Control
' MEDIAPLAYER.VB
' Written: March 2026
' Programmer: Aaron Scott
' Copyright 2026 Sirius Software All Rights Reserved

'*******************************************************
Public Class MediaPlayer
	Implements IDisposable


	' Declare variables local to this class.

	Private IsPlaying As Boolean = False
	Private IgnoreMediaChangeEvent As Boolean
	Private Shared cbInstanceCount As Integer = 0
	Private mPlaystate As SiriusAudio.SEP_Playstate
	Private mPlaylist As String = ""
	Private SongIndex As Integer
	Private SongTitle As String
	Private AlbumName As String
	Private ArtistName As String
	Private picControl As PictureBox ' This holds all the drawn parts of the control.
	Private lstPlayList As ListBox = Nothing
	Private ControlSize As New Size(320, 100)
	Private AlbumImageRect As New Rectangle(0, 0, 48, 48) ' The album wimage will display here.
	Private StatusRect As New Rectangle(49, 0, ControlSize.Width - AlbumImageRect.Width, ControlSize.Height / 2) ' The status window will display here.
	Private ArtistNameRect As New Rectangle(StatusRect.X + 5, StatusRect.Y + 2, StatusRect.Width - 5, StatusRect.Height / 4) ' The name of the current artist will display here.
	Private AlbumNameRect As New Rectangle(StatusRect.X + 5, StatusRect.Y + 16, StatusRect.Width - 5, StatusRect.Height / 4) ' The name of the current album will display here.
	Private SongNameRect As New Rectangle(StatusRect.X + 5, StatusRect.Y + 29, StatusRect.Width - 5, StatusRect.Height / 4) ' The name of the current song will display here.
	Private ButtonX As Integer = AlbumImageRect.Width + (StatusRect.Width - 144) / 2 ' Center buttons beneath status display. 144 is the width of 4 buttons plus space between. 
	Private ButtonY As Integer = ControlSize.Height \ 2 + 7 ' Center buttons in bottom half of control's height.
	Private btnPrevious As New Rectangle(ButtonX, ButtonY, 32, 32)
	Private btnPlayPause As New Rectangle(ButtonX + 36, ButtonY, 32, 32)
	Private btnNext As New Rectangle(ButtonX + 72, ButtonY, 32, 32)
	Private btnStop As New Rectangle(ButtonX + 108, ButtonY, 32, 32)
	Private VolumeRect As New Rectangle(60, ButtonY, 32, 32)
	Private mPlayer As New SiriusAudio
	Private mAlbumArt As Image
	Private mDuration As Double
	Private mAlbum As String
	Private mArtist As String
	Private AlbumImage As Image = Nothing

	Public Enum MediaPlayerAction
		ActionPrevious
		ActionPlay
		ActionPause
		ActionNext
		ActionStop
	End Enum

	Public Shadows Event SongChanged()
	Public Event PlayStateChanged(NewState As Integer)
	Public Event PlayListStartLoad()
	Public Event PlayListEndLoad()
	Public Event PlayerStop()
	Public Event MediaError(idx As Integer)
	Private Event ButtonPressed(Action As MediaPlayerAction)

	Public Structure SongInfo
		Dim Index As Integer
		Dim SongName As String
	End Structure
	'*******************************************************

	' The class is created.

	'*******************************************************
	Public Sub New()

		' This call is required by the designer.
		InitializeComponent()

		' Add any initialization after the InitializeComponent() call.

		Me.SetStyle(ControlStyles.OptimizedDoubleBuffer Or
				ControlStyles.UserPaint Or
				ControlStyles.AllPaintingInWmPaint, True)
		Me.UpdateStyles()

		' Set the default values of the new control.

		cbInstanceCount += 1

		MyBase.Size = ControlSize
		Me.Name = "MediaPlayer" & cbInstanceCount
		Me.BackColor = Color.Transparent
		Me.BorderStyle = BorderStyle.FixedSingle
		picControl = New PictureBox
		picControl.Location = New Point(0, 0)
		picControl.Size = Me.Size
		picControl.BackColor = Color.Transparent
		picControl.BorderStyle = BorderStyle.FixedSingle

		AddHandler Me.MouseDown, AddressOf picControl_MouseDown
		AddHandler Me.MouseUp, AddressOf picControl_MouseUp
		AddHandler Me.ButtonPressed, AddressOf MP_ButtonPressed
		AddHandler mPlayer.SongChanged, AddressOf saSongChanged
		AddHandler mPlayer.PlaylistLoading, AddressOf saPlaylistLoading
		AddHandler mPlayer.PlaylistLoaded, AddressOf saPlaylistLoaded
		AddHandler mPlayer.PlayStateChanged, AddressOf saPlayStateChange
		AddHandler mPlayer.MediaError, AddressOf saMediaError
		If lstPlayList IsNot Nothing Then
			AddHandler lstPlayList.DrawItem, AddressOf lstPlaylist_DrawItem
			AddHandler lstPlayList.DoubleClick, AddressOf lstPlaylist_DoubleClick
		End If
		AddHandler Microsoft.Win32.SystemEvents.PowerModeChanged, AddressOf SystemEvents_PowerModeChanged
		AddHandler Microsoft.Win32.SystemEvents.SessionEnding, AddressOf SystemEvents_SessionEnding

		Me.SetStyle(ControlStyles.UserMouse, True) ' This makes sure mouse events work.

		' Be sure the control starts in a state of "Not Playing"

		IsPlaying = False

	End Sub
	'*******************************************************

	' The class is destroyed.

	'*******************************************************
	Public Overloads Sub Dispose() Implements IDisposable.Dispose
		RemoveHandler Me.MouseDown, AddressOf picControl_MouseDown
		RemoveHandler Me.MouseUp, AddressOf picControl_MouseUp
		RemoveHandler Me.ButtonPressed, AddressOf MP_ButtonPressed
		RemoveHandler mPlayer.SongChanged, AddressOf saSongChanged
		RemoveHandler mPlayer.PlaylistLoading, AddressOf saPlaylistLoading
		RemoveHandler mPlayer.PlaylistLoaded, AddressOf saPlaylistLoaded
		RemoveHandler mPlayer.PlayStateChanged, AddressOf saPlayStateChange
		RemoveHandler mPlayer.MediaUnplayable, AddressOf saMediaError
		If lstPlayList IsNot Nothing Then
			RemoveHandler lstPlayList.DrawItem, AddressOf lstPlaylist_DrawItem
			RemoveHandler lstPlayList.DoubleClick, AddressOf lstPlaylist_DoubleClick
		End If
		RemoveHandler Microsoft.Win32.SystemEvents.PowerModeChanged, AddressOf SystemEvents_PowerModeChanged
		RemoveHandler Microsoft.Win32.SystemEvents.SessionEnding, AddressOf SystemEvents_SessionEnding

		mPlayer.StopAll()
		MyBase.Dispose()
	End Sub

	'*******************************************************

	' Protect the control's size from changing.  Also, the
	' Size property has been made read-only.

	'*******************************************************
	Protected Overrides Sub SetBoundsCore(x As Integer, y As Integer, width As Integer, height As Integer, specified As BoundsSpecified)
		' Force fixed size
		MyBase.SetBoundsCore(x, y, ControlSize.Width, ControlSize.Height, specified)
	End Sub
	'*******************************************************

	' The MouseDown event.

	'*******************************************************
	Private Shadows Sub picControl_MouseDown(sender As Object, ByVal e As MouseEventArgs)

		' Calculate the proper point in the picturebox, relative to the control area.

		Dim adjustedPoint As Point = New Point(e.Location.X - picControl.Left, e.Location.Y - picControl.Top)  ' Determine which button was pressed, and redraw it with a white icon (pressed mode).

		' Calculate values necessary to detect where the volume control was clicked, if it was.

		Dim center As PointF = New PointF(VolumeRect.Left + VolumeRect.Width \ 2, VolumeRect.Top + VolumeRect.Height \ 2)
		Dim radius As Single = Math.Min(VolumeRect.Width, VolumeRect.Height) / 2 - 5

		' Convert adjustedPoint to floating-point for accuracy
		Dim adjustedPointF As PointF = New PointF(adjustedPoint.X, adjustedPoint.Y)

		' Calculate the distance from center to clicked point
		Dim distance As Single = Math.Sqrt((adjustedPointF.X - center.X) ^ 2 + (adjustedPointF.Y - center.Y) ^ 2)
		' Draw the "down" position of each button.

		' Check to see what button was pressed, or if the volume control was pressed.

		Using g As Graphics = Me.CreateGraphics
			If btnPrevious.Contains(adjustedPoint) Then
				DrawButton(g, btnPrevious, Color.Gray, Color.LightGray, "Previous", "Pressed")
				RaiseEvent ButtonPressed(CInt(MediaPlayerAction.ActionPrevious))
			ElseIf btnPlayPause.Contains(adjustedPoint) Then
				If IsPlaying Then
					DrawButton(g, btnPlayPause, Color.Gray, Color.LightGray, "Pause", "Pressed")
					RaiseEvent ButtonPressed(CInt(MediaPlayerAction.ActionPause))
				Else
					DrawButton(g, btnPlayPause, Color.Gray, Color.LightGray, "Play", "Pressed")
					RaiseEvent ButtonPressed(CInt(MediaPlayerAction.ActionPlay))
				End If
			ElseIf btnNext.Contains(adjustedPoint) Then
				DrawButton(g, btnNext, Color.Gray, Color.LightGray, "Next", "Pressed")
				RaiseEvent ButtonPressed(CInt(MediaPlayerAction.ActionNext))
			ElseIf btnStop.Contains(adjustedPoint) Then
				DrawButton(g, btnStop, Color.Gray, Color.LightGray, "Stop", "Pressed")
				RaiseEvent ButtonPressed(CInt(MediaPlayerAction.ActionStop))
			ElseIf Math.Abs(distance - radius) <= 8 Then ' Only recognize clicks close to the outer edge

				' Normalize rawAngle into a continuous 150–300 sweep

				Dim angle As Single = CSng(Math.Atan2(adjustedPointF.Y - center.Y, adjustedPointF.X - center.X) * (180 / Math.PI))

				If angle < 0 Then angle += 360                    ' Rotate coordinate system so 150° becomes 0°


				Const minAngle As Single = 150
				Const maxAngle As Single = 300
				Const sweep As Single = maxAngle - minAngle   ' = 150	

				' If the click is outside the usable arc, do nothing
				If angle < minAngle And angle > maxAngle Then
					Return
				End If

				' Normalize angle into the 150–300 band
				If angle < minAngle Then angle += 360

				' Convert angle to volume

				Dim vol As Single = (angle - 150) / 150   ' 0.0 → 1.0
				Dim MappedVolume As Int16 = CInt(vol * 100)

				' Apply Volume change
				mPlayer.Volume = MappedVolume / 100
				DrawVolumeControl(g, minAngle, sweep)
			End If
		End Using
		picControl.Refresh()

	End Sub
	'*******************************************************

	' The MouseUp event

	'*******************************************************
	Private Shadows Sub picControl_MouseUp(sender As Object, ByVal e As MouseEventArgs)

		' Calculate the proper point in the picturebox, relative to the control area.

		Dim adjustedPoint As Point = New Point(e.Location.X - picControl.Left, e.Location.Y - picControl.Top)  ' Determine which button was pressed, and redraw it with a white icon (pressed mode).

		' Determine which button was released and redraw it in gray, with a black icon (normal).

		Using g As Graphics = Me.CreateGraphics
			If btnPrevious.Contains(adjustedPoint) Then
				DrawButton(g, btnPrevious, Color.Gray, Color.LightGray, "Previous")
			ElseIf btnPlayPause.Contains(adjustedPoint) Then
				If IsPlaying Then
					DrawButton(g, btnPlayPause, Color.Gray, Color.LightGray, "Pause")
				Else
					DrawButton(g, btnPlayPause, Color.Gray, Color.LightGray, "Play")
				End If
			ElseIf btnNext.Contains(adjustedPoint) Then
				DrawButton(g, btnNext, Color.Gray, Color.LightGray, "Next")
			Else
				DrawButton(g, btnStop, Color.Gray, Color.LightGray, "Stop")
			End If
		End Using

	End Sub
	'*******************************************************

	' Event handler for the control's paint paint.

	'*******************************************************
	Private Sub MediaPlayer_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint

		' Declare variables

		Dim g As Graphics = e.Graphics
		g.SmoothingMode = SmoothingMode.HighQuality
		Dim rect As Rectangle = New Rectangle(0, Height \ 2, Me.Width, Me.Height \ 2)
		Dim innerCircle As New Rectangle(10, Me.Height - 20, 10, 10)
		Dim pen As New Pen(Color.Black, 2)
		Dim brush As New SolidBrush(Color.DarkGray)

		' Fill in the control background.

		Using ShadedBrush As New LinearGradientBrush(rect, DarkenOrLightenColor(Color.Gray, 10), DarkenOrLightenColor(Color.LightGray, 10), LinearGradientMode.Vertical)
			g.FillRectangle(ShadedBrush, rect)
		End Using

		' Draw the "headphone jack".

		' Outer Circle (Jack Port)
		Dim outerCircle As New Rectangle(5, Me.Height - 25, 20, 20)
		g.DrawEllipse(pen, outerCircle)

		' Inner Filled Circle (Jack Opening with Soft Shading)
		Dim innerBrush As New Drawing2D.LinearGradientBrush(innerCircle, Color.DarkGray, Color.Black, 45.0F)
		g.FillEllipse(innerBrush, innerCircle)

		' Outer Thin Highlight (Creates a subtle "metallic" edge)
		Dim highlightPen As New Pen(Color.LightGray, 1)
		g.DrawEllipse(highlightPen, outerCircle)

		' Draw the status display.

		Using panelbrush As New LinearGradientBrush(StatusRect, Color.Black, Color.DarkGray, LinearGradientMode.Vertical)
			g.FillRectangle(panelbrush, StatusRect)
		End Using
		If SongTitle IsNot Nothing Then
			Using textbrush As New SolidBrush(Color.LightSkyBlue)
				g.DrawString(ArtistName, picControl.Font, textbrush, ArtistNameRect)
				g.DrawString(AlbumName, picControl.Font, textbrush, AlbumNameRect)
				g.DrawString(SongTitle, picControl.Font, textbrush, SongNameRect)
			End Using
		End If

		' Draw Previous button
		DrawButton(g, btnPrevious, Color.Gray, Color.LightGray, "Previous")

		' Draw Playing button or pause button
		If Not IsPlaying Then
			DrawButton(g, btnPlayPause, Color.Gray, Color.LightGray, "Play")
		Else
			' Draw Pause button
			DrawButton(g, btnPlayPause, Color.Gray, Color.LightGray, "Pause")
		End If
		' Draw Next button
		DrawButton(g, btnNext, Color.Gray, Color.LightGray, "Next")

		' Draw Stop button
		DrawButton(g, btnStop, Color.Gray, Color.LightGray, "Stop")

		' Draw the album image, or a default image.

		If AlbumImage IsNot Nothing Then
			g.DrawImage(AlbumImage, AlbumImageRect)
		Else
			DrawDefaultAlbumImage(g)
		End If

		' Draw the volume control.

		DrawVolumeControl(g, 150, 270)

		' Cleanup
		pen.Dispose()
		brush.Dispose()
		innerBrush.Dispose()
		highlightPen.Dispose()

	End Sub
	'*******************************************************

	' Sub to draw a specified button of the control.

	'*******************************************************
	Private Sub DrawButton(g As Graphics, rect As Rectangle, color1 As Color, color2 As Color, shapeType As String, Optional ButtonState As String = "")

		' Declare variables

		Dim IconColor As Color

		' The icon color will be black normally, but white when pressed.

		If ButtonState = "Pressed" Then IconColor = Color.White Else IconColor = Color.Black

		' Gradient background
		Using brush As New LinearGradientBrush(rect, color1, color2, LinearGradientMode.Vertical)
			g.FillRectangle(brush, rect)
		End Using

		' Define icon shape
		Using shapeBrush As New SolidBrush(IconColor)
			Dim middleY As Integer = rect.Top + (rect.Height \ 2) ' Correct midpoint calculation

			Select Case shapeType
				Case "Previous"
					Dim triangleLeft As Point() = {New Point(rect.Right - 10, rect.Top + 6), New Point(rect.Right - 10, rect.Bottom - 6), New Point(rect.Left + 6, middleY)}
					Dim triangleLeft2 As Point() = {New Point(rect.Right - 6, rect.Top + 6), New Point(rect.Right - 6, rect.Bottom - 6), New Point(rect.Left + 10, middleY)}
					Dim verticalBar As Rectangle = New Rectangle(rect.Left + 3, rect.Top + 6, 2, rect.Height - 12)
					g.FillPolygon(shapeBrush, triangleLeft)
					g.FillPolygon(shapeBrush, triangleLeft2)
					g.FillRectangle(shapeBrush, verticalBar)

				Case "Play"
					Dim trianglePlay As Point() = {New Point(rect.Left + 6, rect.Top + 6), New Point(rect.Left + 6, rect.Bottom - 6), New Point(rect.Right - 10, middleY)}
					g.FillPolygon(shapeBrush, trianglePlay)

				Case "Pause"
					Dim barWidth As Integer = rect.Width \ 4 ' Each bar is 1/4 the button width
					Dim spacing As Integer = rect.Width \ 8  ' Space between the bars

					Dim leftBar As New Rectangle(rect.Left + spacing, rect.Top + 6, barWidth, rect.Height - 12)
					Dim rightBar As New Rectangle(rect.Left + (barWidth * 2) + spacing, rect.Top + 6, barWidth, rect.Height - 12)

					g.FillRectangle(shapeBrush, leftBar)
					g.FillRectangle(shapeBrush, rightBar)

				Case "Stop"
					'Dim square As Rectangle = New Rectangle(rect.Left + 8, rect.Top + 6, rect.Width - 16, rect.Height - 12)
					'g.FillRectangle(Brushes.Red, square)
					Dim circle As Rectangle = New Rectangle(rect.Left + 8, rect.Top + 8, rect.Width - 16, rect.Height - 17)
					If ButtonState = "Pressed" Then
						g.FillEllipse(Brushes.White, circle)
					Else
						g.FillEllipse(Brushes.Red, circle)
					End If

				Case "Next"
					Dim triangleRight As Point() = {New Point(rect.Left + 10, rect.Top + 6), New Point(rect.Left + 10, rect.Bottom - 6), New Point(rect.Right - 6, middleY)}
					Dim triangleRight2 As Point() = {New Point(rect.Left + 6, rect.Top + 6), New Point(rect.Left + 6, rect.Bottom - 6), New Point(rect.Right - 10, middleY)}
					Dim verticalBar As Rectangle = New Rectangle(rect.Right - 5, rect.Top + 6, 2, rect.Height - 12)
					g.FillPolygon(shapeBrush, triangleRight)
					g.FillPolygon(shapeBrush, triangleRight2)
					g.FillRectangle(shapeBrush, verticalBar)
			End Select
		End Using
		g.DrawLine(Pens.Black, New Point(rect.Left, rect.Top), New Point(rect.Left + rect.Width, rect.Top))
		g.DrawLine(Pens.Black, New Point(rect.Left, rect.Top), New Point(rect.Left, rect.Top + rect.Height))
		g.DrawLine(Pens.DarkGray, New Point(rect.Left + rect.Width, rect.Top), New Point(rect.Left + rect.Width, rect.Top + rect.Height))
		g.DrawLine(Pens.DarkGray, New Point(rect.Left, rect.Top + rect.Height), New Point(rect.Left + rect.Width, rect.Top + rect.Height))
	End Sub
	'**********************************************************

	' Sub to draw the volume control.

	'**********************************************************
	Sub DrawVolumeControl(ByVal g As Graphics, minAngle As Single, sweep As Single)

		' Declare variables.

		Dim rect As Rectangle = VolumeRect
		Dim indicatorAngle As Single
		Dim insetRect As New Rectangle(rect.X + 3, rect.Y + 3, rect.Width - 6, rect.Height - 6)
		Dim center As PointF = New PointF(insetRect.Left + insetRect.Width \ 2, insetRect.Top + insetRect.Height \ 2)
		Dim radius As Integer = Math.Min(insetRect.Width, insetRect.Height) \ 2 - 5

		' Enable high-quality rendering
		g.SmoothingMode = SmoothingMode.AntiAlias

		' Create radial gradient for realistic depth
		Using path As New GraphicsPath()
			path.AddEllipse(insetRect)
			Using radialBrush As New PathGradientBrush(path)
				radialBrush.CenterColor = Color.Gray
				radialBrush.SurroundColors = {Color.Black}
				radialBrush.FocusScales = New PointF(0.2F, 0.2F)
				g.FillEllipse(radialBrush, insetRect)
				g.DrawEllipse(Pens.LightGray, insetRect) 'Remove rough edges from re-drawing circle.
				g.DrawEllipse(Pens.Black, insetRect) 'Adds smooth edge to circle.
			End Using
		End Using
		indicatorAngle = minAngle + (sweep * mPlayer.Volume)
		' Draw volume indicator reaching full radius
		Using indicatorPen As New Pen(Color.White, 2)
			indicatorAngle = 150 + (150 * mPlayer.Volume)
			Dim indicatorStart As PointF = PolarToCartesian(center, radius * 0.2, indicatorAngle)
			Dim indicatorEnd As PointF = PolarToCartesian(center, radius, indicatorAngle)
			g.DrawLine(indicatorPen, indicatorStart, indicatorEnd)
		End Using

		' Draw min/max radial markers outside the circle
		Using markerPen As New Pen(Color.White, 2)
			Dim minStart As PointF = PolarToCartesian(center, radius + 7, 150) ' Proper 8 o’clock angle
			Dim minEnd As PointF = PolarToCartesian(center, radius + 10, 150)
			g.DrawLine(markerPen, minStart, minEnd)

			Dim maxStart As PointF = PolarToCartesian(center, radius + 7, 30) ' Proper 4 o’clock angle
			Dim maxEnd As PointF = PolarToCartesian(center, radius + 10, 30)
			g.DrawLine(markerPen, maxStart, maxEnd)
		End Using
	End Sub
	'**********************************************************

	' Function to calculate x,y coordinates from polar coordinates.

	'**********************************************************
	Function PolarToCartesian(center As PointF, radius As Single, angleInDegrees As Single) As PointF
		Dim angleInRadians As Double = Math.PI * angleInDegrees / 180.0
		Return New PointF(center.X + radius * Math.Cos(angleInRadians), center.Y + radius * Math.Sin(angleInRadians))
	End Function
	'**********************************************************

	' Event Handler for the "Draw Item" event of the playlist
	' list box.

	'**********************************************************
	Private Sub lstPlaylist_DrawItem(sender As Object, e As DrawItemEventArgs)

		' Declare variables

		Dim ContainsError As Boolean = False
		Dim INDENT As Single
		Dim zx As String
		Dim g As Graphics = e.Graphics
		Dim f As Font = e.Font
		Dim rect As Rectangle
		Dim isSelected As Boolean = (e.State And DrawItemState.Selected) = DrawItemState.Selected

		' Set the value of e.State to -1, which will, when passed down the chain of event
		' handlers, tell them this handler has done the drawing, and for themto do nothing.

		sender.tag = "Handled"

		' Get the song name and album name from the listbox item.

		If e.Index >= 0 Then
			zx = lstPlayList.Items(e.Index)

			' Check if this entry is marked as being missing.

			If zx.StartsWith("*") Then
				ContainsError = True
				zx = zx.Substring(1)
			End If

			' Parse the entry to get artist, album and song.

			Dim wx As String() = zx.Split("\")
			Dim Artist As String = wx(1)
			Dim Album As String = Path.GetFileName(Path.GetDirectoryName(zx))
			Dim Song As String = SanitizeSongName(Path.GetFileNameWithoutExtension(zx))

			' Erase the current item, so the text doesn't get drawn over and over,
			' as that darkens it over time.  And we can't use "drawbackground", because
			' we don't want the highlight.

			g.FillRectangle(New SolidBrush(lstPlayList.BackColor), e.Bounds)

			' Calculate the size of an indent and create a rectangle that omits it.

			INDENT = g.MeasureString("X", f).Width
			rect = New Rectangle(e.Bounds.X + INDENT, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height)

			' If this item contains an error, highlight that.  The "V" is a
			' circle enclosing an "x" in Wingdings 2.

			If ContainsError Then g.DrawString("V", New Font("Wingdings 2", e.Font.SizeInPoints), Brushes.Red, e.Bounds.Location)

			' See if the item is selected.

			If isSelected Then

				' Set the smoothingmode for drawing an oval highlight for the current item.

				g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

				' Draw oval highlight for current item.

				Dim ovalRect As New Rectangle(
			    e.Bounds.Left + INDENT,
			    e.Bounds.Top + INDENT,
			    e.Bounds.Width - INDENT * 2,
			    e.Bounds.Height - INDENT * 2
			)
				g.FillEllipse(New SolidBrush(Color.LightGreen), ovalRect)

				' Draw the text of the line, bolded.

				g.DrawString(Song, New Font(f, FontStyle.Bold), Brushes.Black, rect)

				' If not selected, draw the text normally.

			Else
				g.DrawString(Song, f, Brushes.Black, rect)
			End If
		End If
	End Sub
	'**********************************************************

	' Event handler for the listbox Double-click event

	'**********************************************************
	Private Sub lstPlaylist_DoubleClick(sender As Object, e As EventArgs)

		If lstPlayList.SelectedIndex = -1 Then Exit Sub ' Ensure an item is selected

		' Play the selected song by index.

		mPlayer.PlaySong(lstPlayList.SelectedIndex)

		' Draw the play/press button as "pause"

		Using g As Graphics = Me.CreateGraphics
			DrawButton(g, btnPlayPause, Color.Gray, Color.LightGray, "Pause")
		End Using
	End Sub
	'**********************************************************

	' Event handler for the Button Pressed event.

	'**********************************************************
	Private Sub MP_ButtonPressed(Action As MediaPlayerAction)

		'  Check the button pressed and perform the appropriate action.

		Select Case Action
			Case MediaPlayerAction.ActionPlay
				mPlayer.Play()
				RaiseEvent PlayStateChanged(SiriusAudio.SEP_Playstate.SEP_Playing)

			Case MediaPlayerAction.ActionPause
				mPlayer.Play()
				RaiseEvent PlayStateChanged(SiriusAudio.SEP_Playstate.SEP_Paused)

			Case MediaPlayerAction.ActionStop
				mPlayer.StopAll()
				RaiseEvent PlayerStop()
				RaiseEvent PlayStateChanged(SiriusAudio.SEP_Playstate.SEP_Stopped)

			Case MediaPlayerAction.ActionPrevious
				mPlayer.PreviousSong()
				RaiseEvent PlayStateChanged(SiriusAudio.SEP_Playstate.SEP_ScanReverse)
				RaiseEvent PlayStateChanged(SiriusAudio.SEP_Playstate.SEP_Playing)

			Case MediaPlayerAction.ActionNext
				mPlayer.NextSong()
				RaiseEvent PlayStateChanged(SiriusAudio.SEP_Playstate.SEP_ScanForward)
				RaiseEvent PlayStateChanged(SiriusAudio.SEP_Playstate.SEP_Playing)

		End Select

	End Sub

	'**********************************************************

	' Sub to draw an image of a CD when no album art is available.

	'**********************************************************
	Private Sub DrawDefaultAlbumImage(g As Graphics)

		' Declare variables.

		Dim rect1 As Rectangle
		Dim rect2 As Rectangle

		' Create an image of a CD.

		g.FillRectangle(Brushes.Wheat, AlbumImageRect)
		Using cdBrush As New LinearGradientBrush(AlbumImageRect, Color.Gray, Color.White, LinearGradientMode.ForwardDiagonal)
			g.FillEllipse(cdBrush, AlbumImageRect)
		End Using
		g.DrawEllipse(Pens.DarkGray, AlbumImageRect)
		rect1 = New Rectangle(20, 20, 8, 8)
		rect2 = New Rectangle(15, 15, 18, 18)
		Using cdbrush As New LinearGradientBrush(rect2, Color.Blue, Color.LightBlue, LinearGradientMode.ForwardDiagonal)
			g.FillEllipse(cdbrush, rect2)
		End Using
		g.FillEllipse(Brushes.White, rect1)
		g.DrawArc(Pens.White, AlbumImageRect, 135, 180)
		g.DrawArc(Pens.DarkGray, AlbumImageRect, 225, 180)

	End Sub
	'**********************************************************

	' The next FIVE handlers receive messages from the music engine,
	' SiriusAudio, and pass on messages to the music control


	'**********************************************************

	'**********************************************************

	' Handler for the Playlist Loading event.  Just pass on
	' the event.

	'**********************************************************
	Private Sub saPlaylistLoading()
		RaiseEvent PlayListStartLoad()
	End Sub
	'**********************************************************

	' Handler for the playlist endload event. Just pass on the 
	' event.

	'**********************************************************
	Private Sub saPlaylistLoaded()
		RaiseEvent PlayListEndLoad()
	End Sub
	'**********************************************************

	' Handler for the PlayStateChanged event.

	'**********************************************************
	Private Sub saPlayStateChange(NewState As Integer)

		' Save the current playstate, for the control's own
		' PlayState property.

		mPlaystate = NewState

		' Redraw the play or pause button depending on the new
		' playstate of the DLL.  The internal IsPlaying flag will
		' be reset, depending upon the new play state. Except for
		' the constructor, this is the ONLY place this flag must
		' be set.

		Select Case NewState
			Case SiriusAudio.SEP_Playstate.SEP_Paused, SiriusAudio.SEP_Playstate.SEP_Stopped
				Using g = Me.CreateGraphics
					DrawButton(g, btnPlayPause, Color.Gray, Color.LightGray, "Play")
				End Using
				IsPlaying = False

			Case SiriusAudio.SEP_Playstate.SEP_Playing
				Using g = Me.CreateGraphics
					DrawButton(g, btnPlayPause, Color.Gray, Color.LightGray, "Pause")
				End Using
				IsPlaying = True

		End Select

		' If we received a PlaylistEnded event,close the player.  This event
		' is ONLY sent if the Repeat flag is set to False.

		If NewState = SiriusAudio.SEP_Playstate.SEP_PlaylistEnded Then RaiseEvent PlayerStop()

	End Sub

	'**********************************************************

	' The current song has changed.

	'**********************************************************
	Private Sub saSongChanged(idx As Integer, filename As String)

		' Declare variables.

		Dim wx As String = ""
		Dim parts() As String

		' Get the song information from the metadata.  Do not use the Item
		' for the information, as it sometimes returns incorrect artist
		' information, which prevents finding the album art.

		parts = filename.Split("\")
		ArtistName = parts(2)
		AlbumName = parts(3)
		SongTitle = SanitizeSongName(Path.GetFileNameWithoutExtension(filename))
		SongIndex = idx

		' Save the artist and album names for the properties

		mArtist = ArtistName
		mAlbum = AlbumName

		' If the song cannot be found, call the unplayable event handler.  This must be
		' done as only the DLL will refuse to trigger an error condition on
		' a non-existent file.  It hands off the non-existent file to WMP as 
		' a fallback, which simply does nothing and generates no error.  So
		' saMediaError never gets called for a non-existent file, and only here
		' can that error be discovered and reported.

		If Not My.Computer.FileSystem.FileExists(filename) Then
			saMediaError(idx)
			Exit Sub
		End If

		' Get the duration from metadata and save it for the duration property.

		Dim f = TagLib.File.Create(filename)
		mDuration = f.Properties.Duration.TotalSeconds / 60

		' Redraw the status area of the control.

		Dim panelbrush As New LinearGradientBrush(StatusRect, Color.Black, Color.DarkGray, LinearGradientMode.Vertical)
		Using g As Graphics = Me.CreateGraphics
			g.FillRectangle(panelbrush, StatusRect)
			If SongTitle IsNot Nothing Then
				Using textbrush As New SolidBrush(Color.LightSkyBlue)
					g.DrawString(ArtistName, picControl.Font, textbrush, ArtistNameRect)
					g.DrawString(AlbumName, picControl.Font, textbrush, AlbumNameRect)
					g.DrawString(SongTitle, picControl.Font, textbrush, SongNameRect)
				End Using
			End If

			' Get the small album art.

			AlbumImage = GetCoverArt($"{MusicFolder}{ArtistName}\{AlbumName}", CoverArtSize.Small)

			' Redraw the new album image.

			If AlbumImage Is Nothing Then
				DrawDefaultAlbumImage(g)
			Else
				g.DrawImage(AlbumImage, AlbumImageRect)
			End If
		End Using


		' Get the large album image and save it for the properties.

		mAlbumArt = GetCoverArt($"{MusicFolder}{ArtistName}\{AlbumName}", CoverArtSize.Large)

		' Set the current song as the selected item in the list box.

		If Not lstPlayList Is Nothing Then
			lstPlayList.SelectedIndex = idx

			' Make sure the selected song is visible in the list box, not too far
			' down the list but not at the top.

			lstPlayList.TopIndex = Math.Max(0, lstPlayList.SelectedIndex - 10)
		End If

		' Cleaup
		panelbrush.Dispose()

		' Trigger the event in the main form

		RaiseEvent SongChanged()
	End Sub
	'**********************************************************

	' Event handler media error event.  This event only gets
	' triggered after the DLL triggers an "UnplayableByMA"
	' event, which causes the wrapper to attempt to play the
	' song by WMPLIB.  If *it* fails to play the song, it raises
	' the MediaError event, which is handled here.

	' This sub also gets *called* not as an event, from 
	' saSongChanged, if a song file cannot be found.

	'**********************************************************
	Private Sub saMediaError(idx As Integer)

		' Declare variables.

		Dim zx As String

		' Get the listbox entry for the failed song.

		If Not lstPlayList Is Nothing Then
			If idx >= 0 And idx < lstPlayList.Items.Count Then
				zx = lstPlayList.Items(idx)
				zx = "*" & zx
				lstPlayList.Items(idx) = zx
			End If
		End If

		' Play the next song .

		mPlayer.NextSong()

	End Sub
	'**********************************************************

	' Event Handler for the powermodechange event.  Trapping
	' this event will allow the user to just shut the lid while
	' music is playing (or put the desktop to sleep or hibernate)
	' without interrupting the current point in the playback.
	' Hopefully.  There are no guarantees with this event.

	'**********************************************************
	Private Sub SystemEvents_PowerModeChanged(sender As Object, e As PowerModeChangedEventArgs)
		If e.Mode = PowerModes.Suspend And IsPlaying Then
			mPlayer.StopAll()
			' Set this explicitly, since the power might go off before the playstatechange event arrives.
			IsPlaying = False

		End If
	End Sub

	'**********************************************************

	' Event handler for the session ending event.

	'**********************************************************
	Private Sub SystemEvents_SessionEnding(sender As Object, e As SessionEndingEventArgs)
		If IsPlaying Then
			mPlayer.StopAll()
			' Set this explicitly, since the power might go off before the playstatechange event arrives.
			IsPlaying = False
		End If
	End Sub
	'**********************************************************

	' Sub to put the player into pause mode.

	'**********************************************************
	Private Sub PausePlayback()

		If IsPlaying Then
			mPlayer.Play() ' If media is playing, this will pause it.
			Using g As Graphics = Me.CreateGraphics
				DrawButton(g, btnPlayPause, Color.Gray, Color.LightGray, "Play")
			End Using
		End If

	End Sub
	'**********************************************************

	' The play/pause method.

	'**********************************************************
	Public Sub PlayPause()

		mPlayer.Play()


	End Sub
	'**********************************************************

	' The Previous Song method.

	'**********************************************************
	Public Sub PlayPrevious()

		mPlayer.PreviousSong()

	End Sub
	'**********************************************************

	' The Next Song method.

	'**********************************************************
	Public Sub PlayNext()

		mPlayer.NextSong()

	End Sub
	'**********************************************************

	' The Stop method

	'**********************************************************
	Public Sub PlayStop()

		mPlayer.StopAll()

	End Sub

	'**********************************************************

	' The listbox property.

	'**********************************************************
	Public Property ListBox As ListBox
		Get
			Return lstPlayList
		End Get
		Set(value As ListBox)
			lstPlayList = value
			lstPlayList.DrawMode = DrawMode.OwnerDrawFixed
			lstPlayList.BackColor = DarkenOrLightenColor(Color.Gray, 80)
			lstPlayList.SelectionMode = SelectionMode.One
			AddHandler lstPlayList.DrawItem, AddressOf lstPlaylist_DrawItem
			AddHandler lstPlayList.DoubleClick, AddressOf lstPlaylist_DoubleClick
		End Set
	End Property
	'**********************************************************

	' The Repeat property.

	'**********************************************************
	Public Property Repeat As Boolean
		Get
			Return mPlayer.Repeat

		End Get
		Set(value As Boolean)

			mPlayer.Repeat = value

		End Set
	End Property
	'**********************************************************

	' The Autostart property.

	'**********************************************************
	Public Property AutoStart As Boolean
		Get
			Return mPlayer.Autostart

		End Get
		Set(value As Boolean)
			mPlayer.Autostart = value

		End Set
	End Property
	'**********************************************************

	' The duration property.

	'**********************************************************
	Public ReadOnly Property Duration As Double
		Get
			Duration = mDuration
		End Get
	End Property
	'**********************************************************

	' The album art property.

	'**********************************************************
	Public ReadOnly Property AlbumArt As Image
		Get
			AlbumArt = mAlbumArt
		End Get
	End Property
	'**********************************************************

	' The album property.

	'**********************************************************
	Public ReadOnly Property Album As String
		Get
			Album = mAlbum
		End Get
	End Property
	'**********************************************************

	' The album artist property.

	'**********************************************************
	Public ReadOnly Property Artist As String
		Get
			Artist = mArtist
		End Get
	End Property
	'**********************************************************

	' The Volume property.  This is a value from 0.0-1.0

	'**********************************************************
	Public Property Volume As Single
		Get
			Volume = mPlayer.Volume  ' The player requires 0.0-1.0
		End Get
		Set(value As Single)
			If value >= 0.0 And value <= 1.0 Then
				mPlayer.Volume = value

				' Draw the volume control
				Using g As Graphics = Me.CreateGraphics
					DrawVolumeControl(g, 150, 270)
				End Using
			End If
		End Set
	End Property
	'**********************************************************

	' The Songlist property.  This takes a list of songs
	' separated by CrLf.

	'**********************************************************
	Public Property Songlist As String
		Get
			Return mPlayer.Songlist
		End Get
		Set(value As String)
			mPlayer.Songlist = value
		End Set
	End Property
	'**********************************************************

	' The Playlist property. This takes the name of a Windows
	' Playlist (.wmp) file.

	'**********************************************************
	Public Property Playlist As String
		Get
			Playlist = mPlaylist
		End Get
		Set(value As String)

			' If the media player has not been set, raise an error.

			If mPlayer Is Nothing Or lstPlayList Is Nothing Then
				Err.Raise(380, , "Invalid property value")
			End If

			' Save the new playlist name.

			mPlaylist = value

			' Set the playlist property to the name of the playlist file.  It will play automatically
			' as the player defaults to autostart.

			mPlayer.Playlist = mPlaylist

		End Set


	End Property
	'*******************************************************

	' The PlaylistItems property

	'*******************************************************
	Public ReadOnly Property PlaylistItems As String()
		Get
			Return mPlayer.PlayListItems

		End Get
	End Property
	'*******************************************************

	' The CurrentSong property.

	'*******************************************************
	Public ReadOnly Property CurrentSong As SongInfo
		Get
			Dim si As New SongInfo
			si.Index = SongIndex
			si.SongName = SongTitle
			Return si
		End Get
	End Property
	'*******************************************************

	' The Playstate property

	'*******************************************************
	Public ReadOnly Property Playstate As SiriusAudio.SEP_Playstate
		Get
			Return mPlaystate
		End Get
	End Property
	'*******************************************************

	' Override the Size property to make it read-only.

	'*******************************************************
	Public Shadows ReadOnly Property Size As Size
		Get
			Size = MyBase.Size
		End Get
	End Property
End Class
