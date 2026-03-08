Public Class ColorPicker
    '**********************************************************
    ' Color Picker module for custom theme colors
    ' COLORPICKER.VB
    ' Written: November 2018
    ' Programmer: Aaron Scott
    ' Contains open source code
    ' Copyright (C) 1993-2018 Sirius Software All Rights Reserved
    '**********************************************************

    ' Declare properties of this module

    Public Color As Color

    ' Declare variables local to this module

    Private mColors As List(Of Color)

    '**********************************************************

    ' The form is loaded.

    '**********************************************************
    Private Sub ColorPicker_Load(sender As Object, e As EventArgs) Handles Me.Load

        ' Declare variables

        Dim ii As Integer
        Dim xx As Integer
        Dim reflect As System.Array = [Enum].GetValues(GetType(KnownColor))
        Dim known(reflect.Length) As KnownColor
        Dim clr As Color

        ' Clear the list box and get the named colors into a list.

        ListBox1.Items.Clear()
        mColors = New List(Of Color)
        Array.Copy(reflect, known, reflect.Length)

        ' Fill the listbox with the list of named colors.

        For ii = 0 To known.Length - 2
            clr = System.Drawing.Color.FromKnownColor(known(ii))
            If Not clr.IsEmpty And Not clr.IsSystemColor And Not clr.ToKnownColor = KnownColor.Transparent Then
                xx = ListBox1.Items.Add(clr)
                mColors.Add(clr)

                ' If the color to be added is the current user-defined color, select it.

                If clr.ToArgb = UserDefinedColor.ToArgb Then ListBox1.SelectedIndex = xx
            End If
        Next
    End Sub
    '**********************************************************

    ' Event handler for the owner-draw list box.

    '**********************************************************
    Private Sub ListBox_DrawItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawItemEventArgs) Handles ListBox1.DrawItem

        ' Declare variables

        Dim gr As Graphics = e.Graphics
        Dim w As Integer = e.Bounds.Bottom - e.Bounds.Top
        Dim bbr As New SolidBrush(Me.BackColor)
        Dim rbr As SolidBrush
        Dim tbr As SolidBrush
        Dim txt As String
        Dim ix As Integer

        ' Draw the background.

        e.DrawBackground()

        ' Make sure the index is valid.

        If e.Index >= 0 And e.Index < mColors.Count Then

            ' Fill the rectangle with the color and write the name of the color.

            rbr = New SolidBrush(mColors(e.Index))
            gr.FillRectangle(rbr, e.Bounds.Left + 2, e.Bounds.Top + 2, w - 4, w - 4)

            ' If the item is selected, highlight it.

            If e.State = DrawItemState.Selected Then gr.FillRectangle(SystemBrushes.Highlight, e.Bounds.Left + w, e.Bounds.Top, e.Bounds.Width - w, e.Bounds.Height)

            ' Get the brush for the text.

            If e.State = DrawItemState.Focus Then
                tbr = CType(SystemBrushes.HighlightText, SolidBrush)
            Else
                tbr = New SolidBrush(e.ForeColor)
            End If

            ' Extract the name of the color.

            txt = mColors(e.Index).ToString
            ix = txt.IndexOf("[")
            If ix <> 0 Then
                Dim jx As Integer = txt.IndexOf("]")
                If jx > ix Then txt = txt.Substring(ix + 1, jx - ix - 1)
            End If

            ' Write the name of the color.

            If e.Index >= 0 Then gr.DrawString(txt, Me.Font, tbr, e.Bounds.Left + w, e.Bounds.Top)
        End If

    End Sub

    '**********************************************************

    ' The okay button is clicked.

    '**********************************************************
    Private Sub cmdOkay_Click(sender As Object, e As EventArgs) Handles btnOkay.Click
        Color = mColors(ListBox1.SelectedIndex)
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub
    '**********************************************************

    ' The cancel button is clicked.

    '**********************************************************

    Private Sub cmdCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub

    '**********************************************************

    ' An item in the list box is double-clicked.

    '**********************************************************
    Private Sub ListBox1_DoubleClick(sender As Object, e As EventArgs) Handles ListBox1.DoubleClick
        cmdOkay_Click(btnOkay, New EventArgs)
    End Sub

    '**********************************************************

    ' The custom color button is clicked.

    '**********************************************************
    Private Sub btnCustom_Click(sender As Object, e As EventArgs) Handles btnCustom.Click

        ' Show the color dialog

        ColorDialog1.Color = Color
        If ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            Color = ColorDialog1.Color
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub
End Class