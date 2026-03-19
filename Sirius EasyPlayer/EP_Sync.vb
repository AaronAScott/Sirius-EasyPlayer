Imports System.Drawing.Drawing2D
Imports System.IO
Public Class frmSync
	Inherits System.Windows.Forms.Form
	'***********************************************************************
	' Sirius Sirius EasyPlayer Main Form
	' EP_MAIN.VB
	' Written: March 2026
	' Programmer: Aaron Scott
	' Copyright 2026 Sirius Software All Rights Reserved
	'***********************************************************************

	' Declare module-level variables.

	Private TransferEnabled As Boolean = False

	' Define a structure for holding the essential information for a sync-able
	' device.

	Public Structure SyncDevice
		Public Name As String          ' Volume label
		Public Root As String          ' e.g. "E:\"
		Public MusicFolder As String   ' e.g. "E:\Music\"
		Public PlaylistFolder As String ' e.g. "E:\Playlists\"
	End Structure
	'***********************************************************************

	' The form is loaded.

	'***********************************************************************
	Private Sub frmSync_Load(sender As Object, e As EventArgs) Handles Me.Load

		' Declare variables

		Dim zx As String
		Dim AvailableDevices As List(Of SyncDevice) = DetectSyncDevices()


		' Fill list box with contents of playlist editor's list box.

		lstSyncList.BeginUpdate()
		For Each zx In frmMain.lstPlayList.Items
			lstSyncList.Items.Add(zx)
		Next zx
		lstSyncList.EndUpdate()

		If AvailableDevices.Count > 0 Then
			Stop
		Else
			Using g = Panel1.CreateGraphics
				g.DrawString("No sync device(s) found.", New Font("Arial", 12), Brushes.Black, New Point(0, 0))
			End Using

		End If
	End Sub
	'***********************************************************************

	' The form is closed.

	'***********************************************************************
	Private Sub frmSync_FormClosed(sender As Object, e As EventArgs) Handles Me.FormClosed

	End Sub
	Private Sub btnSync_Click(sender As Object, e As EventArgs) Handles btnSync.Click

		TransferEnabled = True
		btnCancel.Enabled = True
		PictureBox1.Invalidate()

	End Sub
	Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click

		TransferEnabled = False
		btnCancel.Enabled = False
		PictureBox1.Invalidate()
	End Sub
	'************************************************************

	' Event handler for the picture box where we display the
	' graphic background of the main form.

	'************************************************************
	Private Sub PictureBox1_Paint(sender As Object, e As PaintEventArgs) Handles PictureBox1.Paint

		Dim g As Graphics = e.Graphics
		Dim r As New Rectangle(0, 0, PictureBox1.Width, PictureBox1.Height)
		g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
		g.PixelOffsetMode = PixelOffsetMode.HighQuality
		' Create ImageAttributes for transparency and (optionally) grayscale
		Dim ia As New System.Drawing.Imaging.ImageAttributes()

		' Make pure black transparent
		ia.SetColorKey(Color.Black, Color.FromArgb(255, 14, 14, 14))

		If Not TransferEnabled Then
			' Add grayscale matrix
			Dim cm As New Imaging.ColorMatrix(
		  {
			 New Single() {0.3F, 0.3F, 0.3F, 0, 0},
			 New Single() {0.3F, 0.3F, 0.3F, 0, 0},
			 New Single() {0.3F, 0.3F, 0.3F, 0, 0},
			 New Single() {0, 0, 0, 1, 0},
			 New Single() {0, 0, 0, 0, 1}
		  })
			ia.SetColorMatrix(cm)
		End If

		' Draw the image with transparency (and grayscale if disabled)
		g.DrawImage(PictureBox2.Image,
			 r,
			 0, 0,
			 PictureBox2.Image.Width,
			 PictureBox2.Image.Height,
			 GraphicsUnit.Pixel,
			 ia)

	End Sub

	Private Sub picLegend_Paint(sender As Object, e As PaintEventArgs) Handles picLegend.Paint

		Dim g = e.Graphics
		g.Clear(picLegend.BackColor)

		' Square size and spacing
		Dim boxSize As Integer = 10
		Dim leftMargin As Integer = 5
		Dim textOffset As Integer = 5
		Dim lineHeight As Integer = 18

		' First line: Mobile Format (black square)
		Dim y1 As Integer = 5
		g.FillRectangle(Brushes.Black, leftMargin, y1, boxSize, boxSize)
		g.DrawString("Mobile Format",
			  picLegend.Font,
			  Brushes.Black,
			  leftMargin + boxSize + textOffset,
			  y1 - 2)

		' Second line: Invalid Mobile Format (red square)
		Dim y2 As Integer = y1 + lineHeight
		g.FillRectangle(Brushes.Red, leftMargin, y2, boxSize, boxSize)
		g.DrawString("Invalid Mobile Format",
			  picLegend.Font,
			  Brushes.Black,
			  leftMargin + boxSize + textOffset,
			  y2 - 2)

	End Sub
	Private TransparentKey As Color = Color.FromArgb(0, 255, 0)

	'***********************************************************************

	' Function to search for and create a list of all attached devices
	' to which syncing is possible.

	'***********************************************************************
	Public Function DetectSyncDevices() As List(Of SyncDevice)

		' Declare variables.

		Dim results As New List(Of SyncDevice)

		' Check every connected drive.
		For Each d As DriveInfo In DriveInfo.GetDrives()
			If Not d.IsReady Then Continue For
			If d.DriveType <> DriveType.Removable Then Continue For

			' A removable drive must have a FAT32 or EXFAT format to be eligible.
			Dim fmt As String = d.DriveFormat.ToUpperInvariant()
			If fmt <> "FAT32" AndAlso fmt <> "EXFAT" Then Continue For

			' Verify write access
			Try
				Dim testPath = Path.Combine(d.RootDirectory.FullName, "sync_test.tmp")
				File.WriteAllText(testPath, "x")
				File.Delete(testPath)
			Catch
				Continue For
			End Try

			' Check for Standard layout
			Dim musicPath = Path.Combine(d.RootDirectory.FullName, "Music")
			Dim playlistPath = Path.Combine(d.RootDirectory.FullName, "Playlists")

			' Make sure the standard music and playlist paths exist.
			If Not Directory.Exists(musicPath) Then Directory.CreateDirectory(musicPath)
			If Not Directory.Exists(playlistPath) Then Directory.CreateDirectory(playlistPath)

			' Create a new SyncDevice object.
			Dim sd As New SyncDevice With {
			    .Name = d.VolumeLabel,
			    .Root = d.RootDirectory.FullName,
			    .MusicFolder = musicPath & Path.DirectorySeparatorChar,
			    .PlaylistFolder = playlistPath & Path.DirectorySeparatorChar
			}

			' Add to the list.

			results.Add(sd)
		Next d

		' Return the list, if any.

		Return results
	End Function

	Private Sub lstSyncList_DrawItem(sender As Object, e As DrawItemEventArgs) Handles lstSyncList.DrawItem

		' Declare variables.

		Dim zx As String
		Dim SongFile As String
		Dim g As Graphics = e.Graphics
		Dim lb As ListBox = DirectCast(sender, ListBox)
		Dim sf As New StringFormat()
		sf.FormatFlags = StringFormatFlags.NoWrap
		sf.Trimming = StringTrimming.EllipsisCharacter


		' Draw background.

		If e.Index >= 0 Then
			e.DrawBackground()

			' Get the item to be drawn.

			zx = lb.Items(e.Index)

			' Strip off just the song name.

			SongFile = ParseString(zx, vbTab)

			' See if the file ends with .wma or .mp3.  Those
			' are the only valid formats for syncing.

			zx = Path.GetExtension(SongFile).ToLower

			' Highlight songs that cannot be transferred.

			If zx <> ".mp3" And zx <> ".wma" Then
				g.DrawString(SongFile, e.Font, Brushes.Red, e.Bounds, sf)
			Else
				g.DrawString(SongFile, e.Font, Brushes.Black, e.Bounds, sf)
			End If

		End If
	End Sub

	Private Sub frmSync_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint

		'DrawTransferArrow(e.Graphics, 363, 200, 80, 60, TransferEnabled)
	End Sub
	Public Sub DrawTransferArrow(g As Graphics, x As Integer, y As Integer, w As Integer, h As Integer, enabled As Boolean)

		g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

		' Vertical proportions
		Dim shaftTop As Integer = CInt(h * 0.25)
		Dim shaftBottom As Integer = CInt(h * 0.75)
		Dim midY As Integer = CInt(h * 0.5)

		' Horizontal proportions
		Dim shaftW As Integer = CInt(w * 0.55)
		Dim tipX As Integer = w - 1

		' Your 7‑point arrow polygon (unchanged)
		Dim pts As Point() = {
	   New Point(0, shaftTop),
	   New Point(shaftW, shaftTop),
	   New Point(shaftW, 0),
	   New Point(w, midY),
	   New Point(shaftW, h),
	   New Point(shaftW, shaftBottom),
	   New Point(0, shaftBottom)
    }

		' Build path and translate to (x,y)
		Dim path As New Drawing2D.GraphicsPath()
		path.AddPolygon(pts)

		Dim m As New Drawing2D.Matrix()
		m.Translate(x, y)
		path.Transform(m)

		' ---------------------------------------------------------
		' ENABLED: linear gradient fill + improved circular highlight
		' ---------------------------------------------------------
		If enabled Then

			' Base fill: diagonal linear gradient
			Using lg As New Drawing2D.LinearGradientBrush(
		  New PointF(x, y),
		  New PointF(x + w, y + h),
		  Color.White,
		  Color.FromArgb(0, 0, 160)
	   )
				g.FillPath(lg, path)
			End Using

			' -----------------------------------------------------
			' Improved circular highlight
			' -----------------------------------------------------
			' Make the highlight ellipse wider so it spills into the shaft
			Dim hlWidth As Integer = CInt(h * 1.6)   ' wider than tall
			Dim hlRect As New Rectangle(x + w - hlWidth, y - CInt(h * 0.3), hlWidth, CInt(h * 1.6))

			Dim gpHL As New Drawing2D.GraphicsPath()
			gpHL.AddEllipse(hlRect)

			Using pgbHL As New Drawing2D.PathGradientBrush(gpHL)
				' Pull the center slightly inside the arrow, not at the extreme right
				pgbHL.CenterPoint = New PointF(x + w - CInt(h * 0.25), y + midY)

				' Stronger but still soft highlight
				pgbHL.CenterColor = Color.FromArgb(180, Color.White)
				pgbHL.SurroundColors = {Color.FromArgb(0, Color.White)}

				g.FillEllipse(pgbHL, hlRect)
			End Using

		Else

			' ---------------------------------------------------------
			' DISABLED: your grayscale logic (unchanged)
			' ---------------------------------------------------------
			Using ia As New Imaging.ImageAttributes()
				Dim cm As New Imaging.ColorMatrix(
			 {
				New Single() {0.3F, 0.3F, 0.3F, 0, 0},
				New Single() {0.3F, 0.3F, 0.3F, 0, 0},
				New Single() {0.3F, 0.3F, 0.3F, 0, 0},
				New Single() {0, 0, 0, 1, 0},
				New Single() {0, 0, 0, 0, 1}
			 })
				ia.SetColorMatrix(cm)

				Using bmp As New Bitmap(w, h)
					Using g2 = Graphics.FromImage(bmp)
						g2.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

						' Local polygon
						Dim localPath As New Drawing2D.GraphicsPath()
						localPath.AddPolygon({
				    New Point(0, shaftTop),
				    New Point(shaftW, shaftTop),
				    New Point(shaftW, 0),
				    New Point(w, midY),
				    New Point(shaftW, h),
				    New Point(shaftW, shaftBottom),
				    New Point(0, shaftBottom)
				})

						' Base fill
						Using lg2 As New Drawing2D.LinearGradientBrush(
				    New PointF(0, 0),
				    New PointF(w, h),
				    Color.White,
				    Color.FromArgb(0, 0, 160)
				)
							g2.FillPath(lg2, localPath)
						End Using

						' Highlight (local coords)
						Dim hlWidth2 As Integer = CInt(h * 1.6)
						Dim hlRect2 As New Rectangle(w - hlWidth2, -CInt(h * 0.3), hlWidth2, CInt(h * 1.6))

						Dim gpHL2 As New Drawing2D.GraphicsPath()
						gpHL2.AddEllipse(hlRect2)

						Using pgbHL2 As New Drawing2D.PathGradientBrush(gpHL2)
							pgbHL2.CenterPoint = New PointF(w - CInt(h * 0.25), midY)
							pgbHL2.CenterColor = Color.FromArgb(180, Color.White)
							pgbHL2.SurroundColors = {Color.FromArgb(0, Color.White)}
							g2.FillEllipse(pgbHL2, hlRect2)
						End Using

					End Using

					g.DrawImage(bmp, New Rectangle(x, y, w, h),
					   0, 0, w, h, GraphicsUnit.Pixel, ia)
				End Using
			End Using

		End If

		' Outline (unchanged)
		Using pen As New Pen(Color.Black, 1)
			g.DrawPath(pen, path)
		End Using

	End Sub
End Class