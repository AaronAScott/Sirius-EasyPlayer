Imports System.Drawing.Drawing2D
Imports System.IO
Imports System.Reflection
Imports System.Text
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
	Private AvailableDevices As List(Of SyncDevice)
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

		' Look for sync-able devices.

		AvailableDevices = DetectSyncDevices()

		' If there is an available device, enable the sync button.

		If AvailableDevices.Count > 0 Then btnSync.Enabled = True

		' Fill list box with contents of playlist editor's list box.

		lstSyncList.BeginUpdate()
		For Each zx In frmMain.lstPlayList.Items
			lstSyncList.Items.Add(zx)
		Next zx
		lstSyncList.EndUpdate()

		' Display the name of the connected device, if any.

		ShowDeviceName()

	End Sub
	'***********************************************************************

	' The form is closed.

	'***********************************************************************
	Private Sub frmSync_FormClosed(sender As Object, e As EventArgs) Handles Me.FormClosed

	End Sub
	'***********************************************************************

	' Sub to show the status of a connected device

	'***********************************************************************
	Private Sub ShowDeviceName()

		If AvailableDevices.Count = 0 Then
			lblDeviceInfo.Text = "No sync device(s) found."

		Else
			lblDeviceInfo.Text = AvailableDevices(0).Name

		End If
	End Sub
	'***********************************************************************

	' Override the WndProc procedure to catch events about devices changing.

	'***********************************************************************
	Protected Overrides Sub WndProc(ByRef m As Message)
		Const WM_DEVICECHANGE As Integer = &H219
		Const DBT_DEVICEARRIVAL As Integer = &H8000
		Const DBT_DEVICEREMOVECOMPLETE As Integer = &H8004

		If m.Msg = WM_DEVICECHANGE Then
			If m.WParam.ToInt32() = DBT_DEVICEARRIVAL OrElse m.WParam.ToInt32() = DBT_DEVICEREMOVECOMPLETE Then
				If m.Msg = WM_DEVICECHANGE Then
					If m.WParam.ToInt32() = DBT_DEVICEARRIVAL Then
						AvailableDevices = DetectSyncDevices(True) ' True says to wait
						ShowDeviceName()
						If AvailableDevices.Count > 0 Then btnSync.Enabled = True Else btnSync.Enabled = False
					End If
				End If
			End If
		End If
		MyBase.WndProc(m)
	End Sub
	'***********************************************************************

	' The sync button is clicked.

	'***********************************************************************
	Private Sub btnSync_Click(sender As Object, e As EventArgs) Handles btnSync.Click

		' Declare variables

		Dim sb As New StringBuilder

		' Disable the sync button.

		btnSync.Enabled = False

		' Enable transfer, enable the cancel button and draw the transfer arrow.

		TransferEnabled = True
		btnCancel.Enabled = True
		Using g = Me.CreateGraphics
			DrawTransferArrow(g, 351, 169, 120, 80, TransferEnabled)
		End Using

		' Run the copy routine on its own thread, so the cancel button
		' doesn't get blocked.

		Task.Run(Sub()
				    CopySongs(AvailableDevices(0))
			    End Sub)
	End Sub
	'***********************************************************************

	' Sub to copy songs from the list to the connected device.

	'***********************************************************************
	Private Sub CopySongs(SD As SyncDevice)

		' Declare variables.

		Dim ii As Integer
		Dim finalBatchCount As Integer = 0
		Dim ext As String
		Dim zx As String
		Dim parts() As String
		Dim sb As New StringBuilder

		' Iterate through the list of songs to copy.

		For ii = 0 To lstSyncList.Items.Count - 1

			' Get the item to be copied.

			zx = lstSyncList.Items(ii)
			ext = Path.GetExtension(zx).ToLower

			' Get the components of the file name.

			parts = zx.Split("\")

			' If the file is a valid mobile format, proceed to copy it.

			If ext = ".wma" Or ext = ".mp3" Then
				Try

					' Copy the song.

					My.Computer.FileSystem.CopyFile(zx, $"{SD.MusicFolder}\{parts(2)}\{parts(3)}\{parts(4)}", False)

				Catch ex As Exception
				End Try
			End If

			' Update the list box to show the song was copied FROM.

			lstSyncList.Invoke(Sub()
							    finalBatchCount = UpdateListBox(ii, zx)
						    End Sub)

			' Update the text box to show the song was copied TO.

			TextBox1.Invoke(Sub()
							 AddSong(sb, Path.GetFileNameWithoutExtension(zx) & vbCrLf)
						 End Sub)

			' If the cancel button has disabled transfer, exit.

			If Not TransferEnabled Then Exit For

		Next ii

		' Re-enable sync button and gray out transfer arrow.

		btnSync.Invoke(Sub()
						btnSync.Enabled = True
						btnCancel.Enabled = False
						TransferEnabled = False
						Using g = Me.CreateGraphics
							DrawTransferArrow(g, 351, 169, 120, 80, TransferEnabled)
						End Using
					End Sub)

		' Ensure final EndUpdate runs if needed

		lstSyncList.Invoke(Sub()
						    If finalBatchCount > 0 Then
							    lstSyncList.EndUpdate()
						    End If
					    End Sub)

	End Sub
	'***********************************************************************

	' Sub to add the song just copied to the list of synced songs.

	'***********************************************************************
	Private Sub AddSong(sb As StringBuilder, t As String)

		' Add the line to the string builder and set the text
		' property in one move, to prevent string-thrashing.

		sb.Append(t)
		TextBox1.Text = sb.ToString

		' Make sure the list shows the stuff just added.

		TextBox1.SelectionStart = TextBox1.TextLength
		TextBox1.ScrollToCaret()

	End Sub
	'***********************************************************************

	' Function to update the item just copied, in the list box, so it
	' will be drawn as green, indicating "copied".

	'***********************************************************************
	Private Function UpdateListBox(ByVal idx As Integer, t As String) As Integer

		' Declare variables

		Static jj As Integer = 0
		Dim visibleCount As Integer = lstSyncList.ClientSize.Height \ lstSyncList.Font.Height

		' Begin an update when we are just starting or have just finished a batch.

		If jj = 0 Then
			lstSyncList.BeginUpdate()
		End If

		' Count the items in this batch, and set the asterisk, which tells the
		' list box to display the line as "copied".

		jj += 1
		lstSyncList.Items(idx) = "*" & t

		' If we've updated a full window's worth of songs, end the update
		' and reset the counter.

		If jj >= visibleCount Then
			lstSyncList.EndUpdate()
			lstSyncList.TopIndex = Math.Max(0, idx - 5)
			jj = 0
		End If

		' Return the current counter value, so we'll know if we need
		' a final EndUpdate.

		Return jj

	End Function
	'***********************************************************************

	' The cancel button is clicked.

	'***********************************************************************
	Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click

		TransferEnabled = False
		btnCancel.Enabled = False
		Using g = Me.CreateGraphics
			DrawTransferArrow(g, 351, 169, 120, 80, TransferEnabled)
		End Using

	End Sub
	'***********************************************************************

	' Sub to draw the color legend, indicating what the colors of music
	' items indicates.

	'***********************************************************************
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

		' Third line: File synced (green square)
		Dim y3 As Integer = y2 + lineHeight
		g.FillRectangle(New SolidBrush(Color.FromArgb(255, 32, 255, 32)), leftMargin, y3, boxSize, boxSize)
		g.DrawString("Synced",
			  picLegend.Font,
			  Brushes.Black,
			  leftMargin + boxSize + textOffset,
			  y3 - 2)
	End Sub
	'***********************************************************************

	' Function to search for and create a list of all attached devices
	' to which syncing is possible.

	'***********************************************************************
	Public Function DetectSyncDevices(Optional Wait As Boolean = False) As List(Of SyncDevice)

		' Declare variables.

		Dim AccumulatedTime As Single = 0.0
		Dim results As New List(Of SyncDevice)

		Do
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

			If Not Wait Then Exit Do
			System.Threading.Thread.Sleep(500)
			AccumulatedTime += 0.5
		Loop While AccumulatedTime < 15.0

		' Return the list, if any.

		Return results
	End Function
	'***********************************************************************

	' Event handler for the music item listbox.

	'***********************************************************************

	Private Sub lstSyncList_DrawItem(sender As Object, e As DrawItemEventArgs) Handles lstSyncList.DrawItem

		' Declare variables.

		Dim Transferred As Boolean
		Dim wx As String
		Dim zx As String
		Dim parts() As String
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

			' If the first character is "*", the song has been transferred.
			' Strip off the character and set the transferred flag.

			If zx.StartsWith("*") Then
				Transferred = True
				zx = zx.Substring(1)
			Else
				Transferred = False
			End If

			' Get the parts of the full filepath.

			parts = zx.Split("\")
			wx = parts(3) & " - " & Path.GetFileName(zx)

			' See if the file ends with .wma or .mp3.  Those
			' are the only valid formats for syncing.

			zx = Path.GetExtension(zx).ToLower

			' Highlight songs that cannot be transferred.

			If zx <> ".mp3" And zx <> ".wma" Then
				g.DrawString(wx, e.Font, Brushes.Red, e.Bounds, sf)
			ElseIf Transferred Then
				g.DrawString(wx, e.Font, New SolidBrush(Color.FromArgb(255, 32, 255, 32)), e.Bounds, sf)
			Else
				g.DrawString(wx, e.Font, Brushes.Black, e.Bounds, sf)
			End If

		End If
	End Sub
	'***********************************************************************

	' Event handler for painting the form.

	'***********************************************************************
	Private Sub frmSync_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint

		DrawTransferArrow(e.Graphics, 351, 169, 120, 80, TransferEnabled)

	End Sub
	'***********************************************************************

	' Sub to draw the file transfer arrow.

	'***********************************************************************
	Public Sub DrawTransferArrow(g As Graphics, x As Integer, y As Integer, w As Integer, h As Integer, enabled As Boolean)

		' Set smoothing mode for drawing path.

		g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

		' Vertical proportions
		Dim shaftTop As Integer = CInt(h * 0.25)
		Dim shaftBottom As Integer = CInt(h * 0.75)
		Dim midY As Integer = CInt(h * 0.5)

		' Horizontal proportions
		Dim shaftW As Integer = CInt(w * 0.55)
		Dim tipX As Integer = w - 1

		' Arrow polygon in local coordinates.
		Dim ArrowPts As Point() = {
		    New Point(0, shaftTop),
		    New Point(shaftW, shaftTop),
		    New Point(shaftW - 12, 0),
		    New Point(w, midY),
		    New Point(shaftW - 12, h),
		    New Point(shaftW, shaftBottom),
		    New Point(0, shaftBottom)
		}

		' Build path and translate to (x,y)
		Dim path As New Drawing2D.GraphicsPath()
		path.AddPolygon(ArrowPts)

		Dim m As New Drawing2D.Matrix()
		m.Translate(x, y)
		path.Transform(m)

		' If enabled, draw the arrow in color.
		If enabled Then

			' Create a linear gradient brush using the arrow path
			Dim bounds = path.GetBounds()

			Using pgb As New LinearGradientBrush(
				   New Point(bounds.Left, midY),
				   New Point(bounds.Right, midY),
				   Color.DarkBlue,
				   Color.White)
				g.FillPath(pgb, path)
			End Using
		Else

			'If disabled, draw the arrow in shades of black and white.

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

						' Local path for grayscale rendering
						Dim localPath As New Drawing2D.GraphicsPath()
						localPath.AddPolygon(ArrowPts)

						' Base fill (unchanged)
						Using lg2 As New Drawing2D.LinearGradientBrush(
						    New PointF(0, 0),
						    New PointF(w, h),
						    Color.White,
						    Color.FromArgb(0, 0, 160)
						)
							g2.FillPath(lg2, localPath)
						End Using

						' Highlight (unchanged)
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

					g.DrawImage(bmp, New Rectangle(x, y, w, h), 0, 0, w, h, GraphicsUnit.Pixel, ia)
				End Using
			End Using

		End If

		' Outline the arrow
		Using pen As New Pen(Color.Black, 1)
			g.DrawPath(pen, path)
		End Using

	End Sub

End Class