Public Class frmSleepTimer

	'***********************************************************************
	' Sirius Playlist Editor Sleep Timer
	' PE_SLEEPTIMER.VB
	' Written: May 2025
	' Programmer: Aaron Scott
	' Copyright 2025 Sirius Software All Rights Reserved
	'***********************************************************************

	Private PlayForMinutes As Integer
	Private ElapsedTime As Integer


	'***********************************************************************

	' The form has loaded.

	'***********************************************************************
	Private Sub frmSleepTimer_Load(sender As Object, e As EventArgs) Handles Me.Load

		' Declare variables.


		' Fill in the previous values.

		cmbHours.SelectedIndex = Val(GetSetting("Sirius" & ProgramName.Replace(" ", ""), "Settings", "SleepHours", "1"))
		cmbMinutes.SelectedIndex = Val(GetSetting("Sirius" & ProgramName.Replace(" ", ""), "Settings", "SleepMinutes", "0"))

		' Select the previous "time's up" option.

		Select Case Val(GetSetting("Sirius" & ProgramName.Replace(" ", ""), "Settings", "TimeUp", "3"))
			Case 1
				rbStop.Checked = True
			Case 2
				rbClosePlayer.Checked = True
			Case 3
				rbCloseProgram.Checked = True
		End Select
		chkFadeOut.Checked = CBool(GetSetting("Sirius" & ProgramName.Replace(" ", ""), "Settings", "Fadeout", "True"))

		' Select the previous "after music stops" option.

		Select Case Val(GetSetting("Sirius" & ProgramName.Replace(" ", ""), "Settings", "EndAction", "2"))
			Case 1
				rbNothing.Checked = True
			Case 2
				rbHibernate.Checked = True
			Case 3
				rbShutdown.Checked = True
		End Select

	End Sub
	'***********************************************************************

	' The form is closed.

	'***********************************************************************
	Private Sub frmSleepTimer_Closed(sender As Object, e As EventArgs) Handles Me.Closed

		' Declare variables

		Dim ii As Integer

		' Save all the current options.
		SaveSetting("Sirius" & ProgramName.Replace(" ", ""), "Settings", "SleepHours", CStr(cmbHours.SelectedIndex))
		SaveSetting("Sirius" & ProgramName.Replace(" ", ""), "Settings", "SleepMinutes", CStr(cmbMinutes.SelectedIndex))
		If rbStop.Checked Then ii = 1
		If rbClosePlayer.Checked Then ii = 2
		If rbCloseProgram.Checked Then ii = 3
		SaveSetting("Sirius" & ProgramName.Replace(" ", ""), "Settings", "TimeUp", CStr(ii))
		SaveSetting("Sirius" & ProgramName.Replace(" ", ""), "Settings", "Fadeout", CStr(chkFadeOut.Checked))
		If rbNothing.Checked Then ii = 1
		If rbHibernate.Checked Then ii = 2
		If rbShutdown.Checked Then ii = 3
		SaveSetting("Sirius" & ProgramName.Replace(" ", ""), "Settings", "EndAction", CStr(ii))

	End Sub
	'***********************************************************************

	' The Start button is clicked.

	'***********************************************************************
	Private Sub btnStart_Click(sender As Object, e As EventArgs) Handles btnStart.Click

		' Initialize the amount of time for which the timer is set.

		PlayForMinutes = 0
		If cmbHours.SelectedIndex >= 0 Then PlayForMinutes += Val(cmbHours.SelectedItem) * 60
		If cmbMinutes.SelectedIndex >= 0 Then PlayForMinutes += Val(cmbMinutes.SelectedItem)

		' Start the timer and minimize the form.

		Timer1.Enabled = True
		ElapsedTime = 0
		Me.WindowState = FormWindowState.Minimized

	End Sub

	'***********************************************************************

	' The timer has ticked (once each minute).

	'***********************************************************************
	Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

		' Declare variables

		Dim FadeIncrement As Integer = 5

		' Increment the elapsed time and display it.

		ElapsedTime += 1
		Dim hours As Integer = ElapsedTime \ 60
		Dim minutes As Integer = ElapsedTime Mod 60
		lblElapsedTime.Text = hours.ToString("00") & ":" & minutes.ToString("00")

		' During the last 20 minutes, begin fading out the volume

		If chkFadeOut.Checked Then
			If PlayForMinutes - ElapsedTime <= 20 Then
				If frmMusicPlayer.MP.Volume > FadeIncrement Then
					frmMusicPlayer.MP.Volume -= FadeIncrement
				Else
					frmMusicPlayer.MP.Volume = 0
				End If
			End If
		End If

		' If the time is up, perform the final actions.

		If ElapsedTime = PlayForMinutes Then
			Timer1.Stop()

			' Stop the music, close the player form or the entire program.

			If rbStop.Checked Then
				frmMusicPlayer.StopPlaying()
				Me.Close()
			End If
			If rbClosePlayer.Checked Then
				frmMusicPlayer.Close()
				Me.Close()
			End If
			If rbCloseProgram.Checked Then
				frmMusicPlayer.Close()
				frmMain.Close()
			End If

			' Optionally hibernate or shut down the computer.

			If rbHibernate.Checked Then System.Diagnostics.Process.Start("shutdown", "/h")
			If rbShutdown.Checked Then System.Diagnostics.Process.Start("shutdown", "/s")

			frmMusicPlayer.MP.Volume = 100 ' Restore volume after fade-out

		End If
	End Sub

	'***********************************************************************

	' The Stop button is clicked.

	'***********************************************************************
	Private Sub btnStop_Click(sender As Object, e As EventArgs) Handles btnStop.Click
		Timer1.Stop()
	End Sub

	'***********************************************************************

	' The Reset button is clicked.

	'***********************************************************************
	Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
		ElapsedTime = 0
		lblElapsedTime.Text = "00:00"
		Timer1.Enabled = False
	End Sub


End Class