Imports System.ComponentModel
Imports System.Drawing.Drawing2D
Imports System
Imports System.Collections
Imports System.ComponentModel.Design
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
Imports System.Windows.Forms.Design
Imports System.Security.Cryptography
Imports System.Xml
Imports TagLib
Imports WMPLib




'*******************************************************

' Media Player Control
' MEDIAPLAYER.VB
' Written: May 2025
' Programmer: Aaron Scott
' Copyright 2025 Sirius Software All Rights Reserved

'*******************************************************
Public Class MediaPlayer
	Implements IDisposable


	' Declare variables local to this class.

	Private MediaPlaying As Boolean = False
	Private IgnoreMediaChangeEvent As Boolean
	Private PlayingFromListbox As Boolean
	Private Shared cbInstanceCount As Integer = 0
	Private mDuration As Double
	Private MusicFolder As String
	Private mPlaylist As String = ""
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
	Private Button1 As New Rectangle(ButtonX, ButtonY, 32, 32)
	Private Button2 As New Rectangle(ButtonX + 36, ButtonY, 32, 32)
	Private Button3 As New Rectangle(ButtonX + 72, ButtonY, 32, 32)
	Private Button4 As New Rectangle(ButtonX + 108, ButtonY, 32, 32)
	Private VolumeRect As New Rectangle(60, ButtonY, 32, 32)
	Private mPlayer As New SiriusAudio
	Private mAlbumArt As Image

	Private AlbumImage As Image = Nothing

	Private Enum MediaPlayerAction
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
	Private Event ButtonPressed(Action As MediaPlayerAction)
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

		MusicFolder = GetSetting("Sirius" & ProgramName.Replace(" ", ""), "Settings", "MusicFolder", "")

		Me.SetStyle(ControlStyles.UserMouse, True) ' This makes sure mouse events work.

	End Sub
	'*******************************************************

	' The class is destroyed.

	'*******************************************************
	Public Overloads Sub Dispose() Implements IDisposable.Dispose
		RemoveHandler Me.MouseDown, AddressOf picControl_MouseDown
		RemoveHandler Me.MouseUp, AddressOf picControl_MouseUp
		RemoveHandler Me.ButtonPressed, AddressOf MP_ButtonPressed
		RemoveHandler mPlayer.SongChanged, AddressOf saSongChanged
		If lstPlayList IsNot Nothing Then
			RemoveHandler lstPlayList.DrawItem, AddressOf lstPlaylist_DrawItem
			RemoveHandler lstPlayList.DoubleClick, AddressOf lstPlaylist_DoubleClick
		End If
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
			If Button1.Contains(adjustedPoint) Then
				DrawButton(g, Button1, Color.Gray, Color.LightGray, "Previous", "Pressed")
				RaiseEvent ButtonPressed(CInt(MediaPlayerAction.ActionPrevious))
			ElseIf Button2.Contains(adjustedPoint) Then
				If MediaPlaying Then
					DrawButton(g, Button2, Color.Gray, Color.LightGray, "Pause", "Pressed")
					RaiseEvent ButtonPressed(CInt(MediaPlayerAction.ActionPause))
				Else
					DrawButton(g, Button2, Color.Gray, Color.LightGray, "Play", "Pressed")
					RaiseEvent ButtonPressed(CInt(MediaPlayerAction.ActionPlay))
				End If
				MediaPlaying = Not MediaPlaying
			ElseIf Button3.Contains(adjustedPoint) Then
				DrawButton(g, Button3, Color.Gray, Color.LightGray, "Next", "Pressed")
				RaiseEvent ButtonPressed(CInt(MediaPlayerAction.ActionNext))
			ElseIf Button4.Contains(adjustedPoint) Then
				DrawButton(g, Button4, Color.Gray, Color.LightGray, "Stop", "Pressed")
				RaiseEvent ButtonPressed(CInt(MediaPlayerAction.ActionStop))
			ElseIf Math.Abs(distance - radius) <= 8 Then ' Only recognize clicks close to the outer edge
				' Convert clicked position to an angle relative to center
				Dim rawAngle As Single = 360 - (Math.Atan2(center.Y - adjustedPointF.Y, adjustedPointF.X - center.X) * (180 / Math.PI))
				rawAngle = rawAngle Mod 360

				Dim MappedVolume As Integer
				If rawAngle >= 120 And rawAngle < 360 Then MappedVolume = (rawAngle - 120) / 240 * 100 Else MappedVolume = (67 + CInt(rawAngle / 60 * 34)) Mod 100

				' Convert angle to volume (150° → 0%, 30° → 100%)
				'Dim mappedVolume As Integer = CInt(Math.Max(0, Math.Min(100, (150 - rawAngle) / (150 - 30) * 100)))
				' Apply volume change
				'mPlayer.settings.volume = MappedVolume
				DrawVolumeControl(g)
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
			If Button1.Contains(adjustedPoint) Then
				DrawButton(g, Button1, Color.Gray, Color.LightGray, "Previous")
			ElseIf Button2.Contains(adjustedPoint) Then
				If MediaPlaying Then
					DrawButton(g, Button2, Color.Gray, Color.LightGray, "Pause")
				Else
					DrawButton(g, Button2, Color.Gray, Color.LightGray, "Play")
				End If
			ElseIf Button3.Contains(adjustedPoint) Then
				DrawButton(g, Button3, Color.Gray, Color.LightGray, "Next")
			Else
				DrawButton(g, Button4, Color.Gray, Color.LightGray, "Stop")
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
		DrawButton(g, Button1, Color.Gray, Color.LightGray, "Previous")

		' Draw Playing button or pause button
		If Not MediaPlaying Then
			DrawButton(g, Button2, Color.Gray, Color.LightGray, "Play")
		Else
			' Draw Pause button
			DrawButton(g, Button2, Color.Gray, Color.LightGray, "Pause")
		End If
		' Draw Next button
		DrawButton(g, Button3, Color.Gray, Color.LightGray, "Next")

		' Draw Stop button
		DrawButton(g, Button4, Color.Gray, Color.LightGray, "Stop")

		' Draw the album image, or a default image.

		If AlbumImage IsNot Nothing Then
			g.DrawImage(AlbumImage, AlbumImageRect)
		Else
			DrawDefaultAlbumImage(g)
		End If

		' Draw the volume control.

		DrawVolumeControl(g)

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
	Sub DrawVolumeControl(ByVal g As Graphics)

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

		' Draw volume indicator reaching full radius
		Using indicatorPen As New Pen(Color.White, 2)
			'If mPlayer.settings.volume <= 67 Then indicatorAngle = 150 + (210 * mPlayer.settings.volume / 100) Else indicatorAngle = mPlayer.settings.volume / 100 * 30
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
				g.DrawString(Song, f, Brushes.Goldenrod, rect)
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

		MediaPlaying = True
		Using g As Graphics = Me.CreateGraphics
			DrawButton(g, Button2, Color.Gray, Color.LightGray, "Pause")
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

			Case MediaPlayerAction.ActionPause
				mPlayer.Play()

			Case MediaPlayerAction.ActionStop
				mPlayer.StopAll()
				RaiseEvent PlayerStop()

			Case MediaPlayerAction.ActionPrevious
				mPlayer.PreviousSong()

			Case MediaPlayerAction.ActionNext
				mPlayer.NextSong()

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

	' The current song has changed.

	'**********************************************************
	Private Sub saSongChanged(idx As Integer, filename As String)

		' Declare variables.

		Dim wx As String = ""
		Dim parts() As String

		' Make sure the song can be found.  If not, mark the listbox entry as containing
		' an error.

		If Not My.Computer.FileSystem.FileExists(filename) Then
			wx = "*" & lstPlayList.Items(lstPlayList.SelectedIndex)
			lstPlayList.Items(lstPlayList.SelectedIndex) = wx
		End If

		' Get the song information from the metadata.  Do not use the Item
		' for the information, as it sometimes returns incorrect artist
		' information, which prevents finding the album art.

		parts = filename.Split("\")
		ArtistName = parts(2)
		AlbumName = parts(3)
		SongTitle = SanitizeSongName(Path.GetFileNameWithoutExtension(filename))

		' Get the duration from metadata.

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

			AlbumImage = GetCoverArt($"{MusicFolder}\{ArtistName}\{AlbumName}", CoverArtSize.Small)

			' Redraw the new album image.

			If AlbumImage Is Nothing Then
				DrawDefaultAlbumImage(g)
			Else
				g.DrawImage(AlbumImage, AlbumImageRect)
			End If
		End Using


		' Get the large album image save it.

		mAlbumArt = GetCoverArt($"{MusicFolder}\{ArtistName}\{AlbumName}", CoverArtSize.Large)

		' Set the current song as the selected item in the list box.

		lstPlayList.SelectedIndex = idx

		' Make sure the selected song is visible in the list box.

		lstPlayList.TopIndex = Math.Max(0, lstPlayList.SelectedIndex - 10)

		' Cleaup
		panelbrush.Dispose()

		' Trigger the event in the main form

		RaiseEvent SongChanged()
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

	' The player property.

	'**********************************************************
	Public ReadOnly Property Player As SiriusAudio
		Get
			Player = mPlayer
		End Get
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

	' The Volume property.

	'**********************************************************
	Public Property Volume As Integer
		Get
			Volume = 100
		End Get
		Set(value As Integer)
			Using g As Graphics = Me.CreateGraphics
				DrawVolumeControl(g)
			End Using
		End Set
	End Property
	'**********************************************************

	' The Playlist property.

	'**********************************************************
	Public Property Playlist As String
		Get
			Playlist = mPlaylist
		End Get
		Set(value As String)

			' Declare variables

			Dim i As Integer

			' If the media player has not been set, raise an error.

			If mPlayer Is Nothing Or lstPlayList Is Nothing Then
				Err.Raise(380, , "Invalid property value")
			End If

			' Save the new playlist name.

			mPlaylist = value

			' Fill the playlist list box with the playlist contents.

			mPlayer.Playlist = mPlaylist

			' Populate the listbox with the parsed songs.

			lstPlayList.BeginUpdate()

			For i = 0 To mPlayer.PlayListItems.Count - 1
				lstPlayList.Items.Add(mPlayer.PlayListItems(i))
			Next i
			lstPlayList.EndUpdate()
		End Set


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
