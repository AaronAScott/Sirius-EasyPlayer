Option Strict Off
Option Explicit On
Imports System.Drawing.Imaging
Imports System.Drawing.Text
Imports VB = Microsoft.VisualBasic
Module CommonLibrary
	'**********************************************************
	' Library Functions for Visual Basic Programs
	' LIB.BAS
	' Written: April 2007
	' Updated: November 2013
	' Updated: November 2023
	' Updated: April 2026
	' Programmer: Aaron Scott
	' Copyright (C) 1993-2026 Sirius Software All Rights Reserved
	'**********************************************************

	'**************************************************
	'
	' Function to return a log to base 10
	'
	'**************************************************
	Public Function Log10(ByRef N As Double) As Double
		If N > 0 Then Log10 = System.Math.Log(N) / System.Math.Log(10.0#) Else Log10 = 0
	End Function

	'************************************************************
	'
	' Sub to get the long path name of a path or file
	'
	' Input: Path = The short path to the file we want
	'               the long path to.
	' Output: The long path of the full file.
	'
	'************************************************************
	Public Function GetOSLongPathName(ByRef Path As String) As String

		' Declare variables

		Dim xx As Integer
		Dim zx As String

		zx = Space(1024)
		xx = GetLongPathName(Path, zx, zx.Length)

		' Return the long file name

		xx = zx.IndexOf(Chr(0))
		If xx = -1 Then xx = zx.Length + 1
		GetOSLongPathName = zx.Substring(xx - 1)

	End Function

	'**********************************************************
	'
	' Function to indicate if a string contains any of a number
	' of characters.
	'
	'**********************************************************
	Public Function StringContains(Str_Renamed As String, SrchStr As String) As Boolean

		' Declare variables

		Dim jj As Integer

		' Look for each search string in the supplied string

		If Str_Renamed.Length >= 1 Then
			For jj = 0 To SrchStr.Length - 1
				If Str_Renamed.IndexOf(SrchStr.Substring(jj, 1)) >= 0 Then
					Return False
				End If
			Next jj
		End If
		Return True

	End Function

	' *********************************************************
	'
	' Sub to sort a string array using a shell-metzner sort
	'
	' *********************************************************
	Sub Sort(ByRef Text() As String)

		' Declare variables

		Dim Swapped As Boolean
		Dim Size As Integer
		Dim Midl As Integer
		Dim ii As Integer
		Dim jj As Integer
		Dim kk As Integer
		Dim X As String

		' Begin sorting the array

		Size = UBound(Text, 1)
		Midl = Size
		While Midl > 0
			Midl = Int(Midl / 2)
			For ii = 1 To Midl
				Do
					jj = ii
					kk = ii + Midl
					Swapped = False
					Do
						System.Windows.Forms.Application.DoEvents()
						If Text(jj) > Text(kk) Then
							X = Text(kk) : Text(kk) = Text(jj) : Text(jj) = X
							Swapped = True
						End If
						jj = kk
						kk = kk + Midl
					Loop While kk <= Size
				Loop While Swapped = True
			Next ii
		End While

	End Sub
	' *********************************************************
	'
	' Function to perform a binary search on a text array for
	' a specific entry.  The array must be 1-based, and sorted.
	'
	' *********************************************************
	Public Function Search(ByRef Entry As String, ByRef Text() As String) As Integer

		' Declare variables

		Dim Bottom As Single
		Dim Top As Single
		Dim Midl As Single
		Dim ii As Integer
		Dim xx As String
		Dim zz As String

		' Lowercase the entry string for comparison

		zz = Entry.ToLower

		' Begin a binary search of the combobox

		Search = 0
		Bottom = 1
		Top = UBound(Text, 1)
		Midl = Int(Bottom + (Top - Bottom + 0.1) / 2)
		Do
			xx = Text(Midl).ToLower
			If zz = xx Then
				Search = Midl
				Exit Function
			ElseIf zz > xx Then
				If Bottom = Midl Then Exit Function
				Bottom = Midl
				If Top - Midl > 1 Then ii = Int((Top - Midl) / 2) Else ii = 1
				Midl = Midl + ii
			ElseIf zz < xx Then
				If Top = Midl Then Exit Function
				Top = Midl
				If Midl - Bottom > 1 Then ii = Int((Midl - Bottom) / 2) Else ii = 1
				Midl = Midl - ii
			End If
		Loop While ii > 0 And Midl >= LBound(Text, 1) And Midl <= UBound(Text, 1)

	End Function
	'************************************************
	'
	' Function to convert a hex string to the string
	' it was made of
	'
	'************************************************
	Public Function FromHex(ByRef Text As String) As String

		' Declare variables

		Dim zx0 As String
		Dim jj As Integer

		' Begin taking the string apart, two bytes at a
		' time, and converting those hex values back to
		' what they represent.

		zx0 = ""
		For jj = 0 To Text.Length - 1 Step 2
			zx0 &= Chr(CInt("&H" & Text.Substring(jj, 2)))
		Next jj

		' return the value

		FromHex = zx0

	End Function
	'**************************************************
	'
	' Function to scramble a string of text to make
	' it indecipherable
	'
	'**************************************************
	Public Function Scramble(ByRef Text As String) As String

		' Declare variables

		Dim zx As String

		' Get the string into a local copy

		zx = Text

		' To scramble, we first encrypt it, which is a simple
		' reversible process.

		zx = Encrypt(zx)

		' Now convert that encrypted text to hex

		zx = ToHex(zx)

		' Now encrypt the hex

		zx = Encrypt(zx)

		' Return the scrambled result.  It should be
		' secure against anybody except a cryptographer!

		Scramble = zx

	End Function
	'***********************************************
	'
	' Function to return the hex representation of
	' the characters of a string.
	'
	'************************************************
	Public Function ToHex(value As String) As String

		' Declare variables

		Dim ii As Integer
		Dim zx As String = ""

		' Get the hex representation of each character in the string.

		For ii = 0 To value.Length - 1
			zx &= ToHex(Asc(value.Substring(ii, 1)))
		Next ii

		' Return the result.

		Return zx

	End Function

	'***********************************************
	'
	' Function to return the hex representation of
	' an integer.
	'
	'************************************************
	Public Function ToHex(value As Integer) As String

		' Declare variables.

		Dim zx As String

		' Begin going through the passed string, convert
		' each byte to its Hex representation

		zx = VB.Format(value, "x").ToUpper
		If zx.Length / 2 <> Int(zx.Length / 2) Then zx = "0" & zx

		Return zx

	End Function
	'**************************************************
	'
	' Function to un-scramble a string of text to make
	' it clear text.
	'
	'**************************************************
	Public Function UnScramble(ByRef Text As String) As String

		' Declare variables

		Dim zx As String

		' Get the string into a local copy

		zx = Text

		' To unscramble, we first encrypt it, which is a simple
		' reversible process.

		zx = Encrypt(zx)

		' Now convert that encrypted text from hex

		zx = FromHex(zx)

		' Now encrypt the un-hexed string

		zx = Encrypt(zx)

		' Return the un-scrambled result.

		UnScramble = zx

	End Function
	'***************************************************************************

	' Function to return maximum of an arbitrary group of integer.
	' Input : W1, W2...
	' Output: the maximum of the arguments

	'***************************************************************************
	Public Function Max(ByVal ParamArray IntList() As Integer) As Integer

		' Declare variables

		Dim ii As Integer = 0
		Dim MaxValue As Integer

		' Make sure we have a list.

		If IntList.Count > 0 Then
			MaxValue = IntList(ii)
			For ii = 1 To IntList.Count - 1
				If IntList(ii) > MaxValue Then MaxValue = IntList(ii)
			Next ii
		End If
		Return MaxValue

	End Function
	'***************************************************************************

	' Function to return minimum of an arbitrary group of integer.
	' Input : W1, W2...
	' Output: the minimum of the arguments

	'***************************************************************************
	Public Function Min(ByVal ParamArray IntList() As Integer) As Integer

		' Declare variables

		Dim ii As Integer = 0
		Dim MinValue As Integer

		' Make sure we have a list.

		If IntList.Count > 0 Then
			MinValue = IntList(ii)
			For ii = 1 To IntList.Count - 1
				If IntList(ii) < MinValue Then MinValue = IntList(ii)
			Next ii
		End If
		Return MinValue

	End Function
	'**********************************************************
	'
	' Function to trim text to a specified width.  This works
	' much like the WordWrap function, but breaks anywhere, and
	' not at word boundries.
	'
	'**********************************************************
	Public Function TextTrim(ByRef g As Graphics, Text As String, Width As Integer, F As Font) As String

		' Declare variables

		Dim jj As Integer


		' Begin trimming down the text until it is less than or
		' equal to the specified width.

		For jj = Text.Length To 0 Step -1
			If g.MeasureString(Text.Substring(0, jj), F).Width <= Width Then
				Return Text.Substring(0, jj)
			End If
		Next jj
		Return ""

	End Function

	'**********************************************************

	' Overloads to the TextTrim function

	'**********************************************************
	Public Function TextTrim(ByRef Destination As Object, Text As String, Width As Single, F As Font) As String
		Return TextTrim(Destination, Text, CInt(Width), F)

	End Function
	Public Function TextTrim(ByRef Destination As Object, Text As String, Width As Double, F As Font) As String
		Return TextTrim(Destination, Text, CInt(Width), F)
	End Function

	'*****************************************************
	'
	' Function to return the current date in the format
	' yyyymmdd
	'
	'*****************************************************
	Public Function DBCurrentDate() As Integer

		' Get the current date in the form yyyymmdd

		DBCurrentDate = Val(DateString.Substring(6, 4) & DateString.Substring(0, 2) & DateString.Substring(3, 2))

	End Function

	'**********************************************************
	'
	' Function to encrypt or decrypt a string by swapping all
	' bits to their oppsite values.
	'
	'**********************************************************
	Public Function Encrypt(ByRef Text As String) As String

		' Declare variables

		Dim jj As Integer
		Dim ww As String

		' Swap the bits of each byte in the string, which creates
		' a reversible garbling of the text

		ww = Text
		For jj = 1 To ww.Length
			Mid(ww, jj, 1) = Chr(Asc(Mid(ww, jj, 1)) Xor &HFFS)
		Next jj
		Encrypt = ww

	End Function
	'**********************************************************
	'
	' Function to capitalize the first letter of each word.
	' If the word is a name such as "McDonald" or "O'Brien",
	' the letter after the Mc or O' will also be capitalized, as
	' will any letter after any non-alpha character.
	' All other letters are converted to lower case
	'
	'**********************************************************
	Public Function Capitalize(ByRef Text As String, Optional CapitalizeAll As Boolean = True) As String

		' Declare variables

		Dim ii As Integer
		Dim jj As Integer
		Dim X As String
		Dim Y As String
		Dim z As String

		' Get both an uppercase and a lowercase version of the
		' text.

		Y = Text.ToUpper
		z = Text.ToLower

		' Begin parsing the string.  Save an uppercase version
		' of the current character, in case we decide to use it.

		For jj = 0 To z.Length - 1
			X = z.Substring(jj, 1).ToUpper

			' The first letter is always capitalized

			If jj = 0 Then
				z = X & z.Substring(jj + 1)
				If Not CapitalizeAll Then Return z

				' If the previous character is not a letter, and not an
				' apostrophe, and not a number, capitalize the current
				' letter.

			Else
				ii = jj - 1
				If (Y.Substring(ii, 1) < "A" Or Y.Substring(ii, 1) > "Z") And Val(Y.Substring(ii, 1)) = 0 And Y.Substring(ii, 1) <> "0" And Y.Substring(ii, 1) <> "'" Then z = z.Substring(0, jj) & X & z.Substring(jj + 1)


				' Capitalize letters after "Mc" or "O'"

				If jj > 1 Then
					If z.Substring(jj - 2, 2) = "Mc" Then z = z.Substring(0, jj) & X & z.Substring(jj + 1)
					If z.Substring(jj - 2, 2) = "O'" Then z = z.Substring(0, jj) & X & z.Substring(jj + 1)
				End If
			End If
		Next jj
		Capitalize = z
	End Function

	'*********************************************************
	'
	' function to format a date of the form yyyymmdd to the
	' string mm/dd/yyyy
	'
	'*********************************************************
	Public Function ChDate(ByVal d As Integer, Optional ByVal FullYear As Boolean = True) As String

		' Declare variables

		Dim yr As Integer
		Dim mo As Integer
		Dim dy As Integer
		Dim zx0 As String = ""

		If d > 19000101 Then
			zx0 = Str(d).TrimStart
			If FullYear Then yr = Val(zx0.Substring(0, 4)) Else yr = Val(zx0.Substring(2, 2))
			mo = Val(zx0.Substring(4, 2))
			dy = Val(zx0.Substring(6, 2))
			zx0 = Format(mo, "00") & "-" & Format(dy, "00") & "-"
			If FullYear Then zx0 &= Format(yr, "0000") Else zx0 &= Format(yr, "00")
		End If
		ChDate = zx0
	End Function
	'************************************************************
	'
	' Sub to ensure that a path has a terminating
	' backslash
	'
	' Input: Path = a directory path
	' Output: the path with a guaranteed final backslash
	' Call this as a sub or a function, as suits the code
	'************************************************************
	Public Function AddDirSeparator(Path As String) As String
		If Path.Substring(Path.Length - 1, 1) <> "\" Then Path = Path & "\"
		AddDirSeparator = Path
	End Function

	'****************************************************************************
	'
	'  function to convert a date stored as a LONG in the format yyyymmdd to
	'  a sequential integer which represents the day from the start of the
	'  the year 1901.
	'
	'****************************************************************************/
	Public Function DateToSeq(ByRef d As Integer) As Integer

		' Declare variables

		Dim yy As Integer
		Dim mm As Integer
		Dim dd As Integer
		Dim f As Integer

		Static nodays(11) As Integer
		nodays(0) = 0
		nodays(1) = 31
		nodays(2) = 59
		nodays(3) = 90
		nodays(4) = 120
		nodays(5) = 151
		nodays(6) = 181
		nodays(7) = 212
		nodays(8) = 243
		nodays(9) = 273
		nodays(10) = 304
		nodays(11) = 334
		Static days_in_month(11) As Integer
		days_in_month(0) = 31
		days_in_month(1) = 28
		days_in_month(2) = 31
		days_in_month(3) = 30
		days_in_month(4) = 31
		days_in_month(5) = 30
		days_in_month(6) = 31
		days_in_month(7) = 31
		days_in_month(8) = 30
		days_in_month(9) = 31
		days_in_month(10) = 30
		days_in_month(11) = 31

		' a zero date is valid. allow it to return zero

		If d = 0 Then
			DateToSeq = 0
			Exit Function
		End If

		' otherwise calculate sequential date. any other errors return -1.

		yy = d \ 10000
		mm = d \ 100 - yy * 100
		dd = d - yy * 10000 - mm * 100
		If yy < 1901 Or yy > 2099 Or dd < 1 Or mm < 1 Or mm > 12 Or dd > 31 Then
			DateToSeq = -1
			Exit Function
		ElseIf mm <> 2 And dd > days_in_month(mm - 1) Or (yy \ 4 * 4 <> yy And mm = 2 And dd > 28) Then
			DateToSeq = -1
			Exit Function
		Else

			' the date is valid, convert it to its sequential value where the value 1
			'       represents the date 01/01/1901

			f = (nodays(mm - 1) + 365 * (yy - 1901) + dd + ((yy - 1901) \ 4))
			If yy \ 4 * 4 = yy And mm > 2 Then f = f + 1
			DateToSeq = f
		End If
	End Function

	'**********************************************************
	'
	' Function to return a date of the form yyyymmdd from just
	' about any character representation
	'
	'**********************************************************
	Public Function lDate(ByRef TextDate As String) As Integer

		' Declare variables

		Dim hKey As Integer
		Dim z As Integer
		Dim xx As Integer
		Dim SplitYear As Integer
		Dim mm As Integer
		Dim dd As Integer
		Dim yy As Integer
		Dim Bad As Boolean
		Dim X As String
		Dim zx0 As String
		Dim zx1 As String

		' If "date" is null or "00/00/00" then return zero.
		' Otherwise, we will validate the date.

		Bad = False
		If TextDate = "" Or TextDate.Substring(0, 8) = "00/00/00" Or TextDate.Substring(0, 8) = "00-00-00" Then
			lDate = 0

			' Convert mm-dd-yy to mm/dd/yy

		Else
			zx0 = TextDate.Replace("-", "/")

			' Convert m/d/yy to mm/dd/yy for standard length date.

			xx = zx0.IndexOf("/", 0)
			If xx = 1 Then zx0 = SRep(LPad(Left(zx0, 1), 2), 1, " ", "0") & zx0.Substring(1, zx0.Length - 1)
			xx = zx0.IndexOf("/", 3)
			If xx = 5 Then zx0 = zx0.Substring(0, 3) & SRep(LPad(Mid(zx0, 4, 1), 2), 1, " ", "0") & zx0.Substring(4, zx0.Length - 1)

			' Now get rid of slashes, leaving mmddyy or mmddyyyy

			zx0 = zx0.Replace("/", "")

			' If length is 8, assume we have mmddyyyy or yyyymmdd

			If zx0.Length = 8 Then

				' If the first two digits are >= 19 (19xx), must be a full
				' year, of the form yyyymmdd

				If Val(zx0.Substring(0, 2)) >= 19 Then
					z = Val(zx0)

					' Otherwise convert to yyyymmdd

				Else
					z = Val(zx0.Substring(4, 4) & zx0.Substring(0, 4))
				End If

				' If length is six, can only be mmddyy or yymmdd.
				' If yymmdd, is only unambiguous if year is past
				' 1931.  Then the number can ONLY be a year.

			ElseIf zx0.length = 6 Then

				' Get the "split year", used by Windows to convert a 2-digit
				' year to a 4-digit year.

				xx = RegOpenKeyEx(HKEY_USERS, ".Default\Control Panel\International\Calendars\TwoDigitYearMax", 0, KEY_ALL_ACCESS, hKey)
				zx1 = Space(255)
				xx = RegQueryValueEx(hKey, "1", 0, REG_SZ, zx1, Len(zx1))
				xx = RegCloseKey(hKey)
				SplitYear = Val(Right(Trim(SRep(zx1, 1, Chr(0), "")), 2))
				If SplitYear = 0 Then SplitYear = 29

				' Get the parts of the supposed date

				mm = Val(zx0.Substring(0, 2))
				dd = Val(zx0.Substring(2, 2))
				yy = Val(zx0.Substring(4, 2))

				' See if the value of "mm" is 32 or greater.  If so,
				' can only be a date of form yymmdd, if other parts
				' make sense.

				' Year is 1932 or better, form is yymmdd. Tack on
				' Century to get yyyymmdd

				If mm > 31 Then
					If dd >= 1 And dd <= 12 And yy >= 1 And yy <= 31 Then
						If mm > SplitYear Then
							z = 19000000.0# + (mm * 10000.0#) + (dd * 100.0#) + yy
						Else
							z = 20000000.0# + (mm * 10000.0#) + (dd * 100.0#) + yy
						End If
					Else
						Bad = True
					End If

					' Otherwise, convert mmddyy to yyyymmdd

				ElseIf mm >= 1 And mm <= 12 And dd >= 1 And dd <= 31 Then
					If yy > SplitYear Then
						z = 19000000.0# + (yy * 10000.0#) + (mm * 100.0#) + dd
					Else
						z = 20000000.0# + (yy * 10000.0#) + (mm * 100.0#) + dd
					End If

					' If false possibilities of interpreting the 6-digit number as
					' a date, return -1.

				Else
					Bad = True
				End If
			End If

			' Validate the assemblage before returning

			If Not Bad Then
				X = CStr(z)
				yy = Val(X.Substring(0, 4))
				mm = Val(X.Substring(4, 2))
				dd = Val(X.Substring(6, 2))
				If yy < 1901 Or yy > 2099 Then Bad = True
				If mm < 1 Or mm > 12 Then Bad = True
				If dd < 1 Or dd > Choose(Val(X.Substring(4, 2)), 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31) Then Bad = True
				If (yy / 4 <> Int(yy / 4) Or (yy / 100 = Int(yy / 100) And yy / 400 <> Int(yy / 400))) And mm = 2 And dd > 28 Then Bad = True
			End If

			' Make a final check that the date is valid.

			If Not IsDate(ChDate(z)) Then Bad = True

			' If the date is valid, return it, otherwise return -1

			If Bad = True Then lDate = -1 Else lDate = z
		End If

	End Function
	'**********************************************************

	' Function to perform a safe "get" from a recordset.
	' Input: T = the recordset to get from
	'        S = the name of the string field to get

	'**********************************************************
	Public Function GetR(ByRef t As DataRow, ByRef s As String) As Object

		' Declare variables

		Dim o As Object

		' Get the object from the datarow.

		Try
			o = t.Item(s)
			If TypeOf (o) Is String Then o = CType(o, String).TrimEnd
			If IsDBNull(o) Then
				If Type.GetTypeCode(t.Table.Columns(s).DataType) = TypeCode.Int32 Then
					Return 0
				ElseIf Type.GetTypeCode(t.Table.Columns(s).DataType) = TypeCode.DateTime Then
					Return #12:00:00 PM#
				ElseIf Type.GetTypeCode(t.Table.Columns(s).DataType) = TypeCode.String Then
					Return ""
				End If
			Else
				Return o
			End If
		Catch ex As Exception
			Return "#" & ex.Message & "#"
		End Try

		Return Nothing

	End Function

	'*********************************************************
	'
	' Function to pad a string with blanks to the left
	'
	'**********************************************************
	Public Function LPad(ByRef Text As String, ByRef wd As Integer) As String
		If wd >= Len(Text) Then LPad = Right(Space(wd) & Text, wd) Else LPad = Text
	End Function

	'*********************************************************
	'
	' Function to pad a string with blanks to the right
	'
	'*********************************************************
	Public Function RPad(ByRef Text As String, ByRef wd As Integer) As String
		If wd > Len(Text) Then RPad = Left(Text & Space(wd), wd) Else RPad = Text
	End Function

	'*********************************************************
	'
	' Function to parse a string of items separated by commas,
	' or any specified delimiter.
	'
	'*********************************************************
	Public Function ParseString(ByRef ArgList As String, Optional ByRef Delimiter As String = ",") As String

		' Declare variables

		Dim xx As Integer
		Dim yy As Integer

		' Get the text to first delimiter or, if no more delimiters, to the end
		' of the string

		xx = ArgList.IndexOf(Delimiter)
		If xx < 0 Then xx = ArgList.Length

		' See if the delimiter is within quotes and if so, move on to the
		' next delimiter.

		yy = ArgList.IndexOf(Chr(34))
		If yy > 0 And yy < xx Then
			Do
				yy = ArgList.IndexOf(Chr(34), yy + 1)
				xx = ArgList.IndexOf(Delimiter, yy + Delimiter.Length)
				If xx < 0 Then xx = ArgList.Length
			Loop While xx < yy
		End If

		' Return the parsed string

		If xx <= ArgList.Length Then ParseString = ArgList.Substring(0, xx) Else ParseString = ArgList

		' Strip off the returned argument so supplied string
		' is changed

		If xx + Delimiter.Length < ArgList.Length Then ArgList = ArgList.Substring(xx + Delimiter.Length) Else ArgList = ""

	End Function

	'**********************************************************
	'
	' Function to return a date of the form mm/dd/yy from
	' a number stored as yyyymmdd
	'
	'**********************************************************
	Public Function SDate(ByRef d As Integer) As String

		' Declare variables

		Dim zx0 As String
		Dim yr As Integer
		Dim mo As Integer
		Dim dy As Integer

		' Assemble the date and return it.

		zx0 = Str(d)
		yr = Val(zx0.Substring(3, 2))
		mo = Val(zx0.Substring(5, 2))
		dy = Val(zx0.Substring(7, 2))
		zx0 = Format(mo, "00") & "/" & Format(dy, "00") & "/" & Format(yr, "00")
		SDate = zx0
	End Function



	'***************************************************************************
	'
	' Function to convert a sequential number to a date of the form yyyymmdd.
	' Sequence number 1 is 1/1/1901, with the range extending to 12/31/2099
	'
	'***************************************************************************
	Public Function SeqToDate(ByRef d As Integer) As Integer

		' Declare variables

		Dim mm As Integer
		Dim yy As Integer
		Dim dd As Integer

		Static nodays(12) As Integer
		nodays(1) = 31
		nodays(2) = 28
		nodays(3) = 31
		nodays(4) = 30
		nodays(5) = 31
		nodays(6) = 30
		nodays(7) = 31
		nodays(8) = 31
		nodays(9) = 30
		nodays(10) = 31
		nodays(11) = 30
		nodays(12) = 31

		SeqToDate = 0
		yy = Int(d / 365.25 - 0.0006)
		dd = d - yy * 365 - Int(yy / 4)
		If (yy + 1) / 4 = Int((yy + 1) / 4) Then nodays(2) = 29 Else nodays(2) = 28
		For mm = 1 To 12
			If dd <= nodays(mm) Then
				SeqToDate = ((yy + 1901) * 10000 + mm * 100 + dd)
				Exit Function
			End If
			dd = dd - nodays(mm)
		Next mm

	End Function

	'***************************************************************************
	'
	' Search and replace function
	' Input  Target   = The string to be changed
	'        StartPos = The starting point in Target where changes may be made
	'        SrchStr  = The substring to be searched for
	'        ReplStr  = The substring to be replaced into Target
	'        CompareMethod = 0 = Case Sensitive compare
	'                        1 = Non-Case Sensitive compare
	'***************************************************************************
	Public Function SRep(ByRef Target As String, ByRef StartPos As Integer, ByRef SrchStr As String, ByRef ReplStr As String, Optional ByRef CompareMethod As Microsoft.VisualBasic.CompareMethod = CompareMethod.Binary) As String

		' Declare variables

		Dim xx As Integer
		Dim zx0 As String

		' Begin searching through the string and replacing
		' occurences of the search string with the
		' replace string.

		zx0 = Target
		xx = InStr(StartPos, zx0, SrchStr, CompareMethod)
		While xx > 0
			zx0 = Left(zx0, xx - 1) & ReplStr & Right(zx0, Len(zx0) - (xx + Len(SrchStr) - 1))
			xx = InStr(xx + Len(ReplStr), zx0, SrchStr)
		End While
		SRep = zx0

	End Function

	'**********************************************************
	'
	' Function to convert a string to a double
	'
	'**********************************************************
	Public Function StrToDbl(ByRef t As String, Optional ByRef Mult As Double = 0) As Double
		If Mult = 0 Then
			StrToDbl = Fix(System.Math.Abs(Val(t)) * 100 + 0.5) * System.Math.Sign(Val(t))
		Else
			StrToDbl = Fix(System.Math.Abs(Val(t)) * Mult + 0.5) * System.Math.Sign(Val(t))
		End If
	End Function


	'**********************************************************

	' Function to return the longest string which will print
	' in the specified width, and trim that portion off the
	' supplied string.

	'**********************************************************
	Public Function WordWrap(g As Graphics, F As Font, ByRef Text As String, wd As Integer) As String

		' Declare variables

		Dim jj As Integer
		Dim wWord As Integer
		Dim wText As Integer
		Dim xx As String
		Dim zx As String

		' Replace any CrLf characters with just Lf

		Text = Text.Replace(vbCrLf, vbLf)

		' Begin accumulating the return text, word by word, until we would
		' exceed the width of the space.

		zx = ""
		wText = 0
		Do
			xx = GetNextWord(Text) ' Get next word in text.
			wWord = g.MeasureString(xx, F).Width

			' Adding the word would make the text too long.

			If wText + wWord > wd Then
				If wText > 0 Then ' Boundary test, in case final word is still too long.
					Return zx.TrimStart
				Else
					' In case the remaining text contains a single, too-long word, break it wherever
					' we need to to fit.

					For jj = xx.Length - 1 To 0 Step -1
						If g.MeasureString(xx.Substring(0, jj), F).Width <= wd Then
							If jj < Text.Length - 1 Then Text = Text.Substring(jj + 1) Else Text = ""
							Return xx.Substring(0, jj + 1)
						End If
					Next jj
				End If

				' Accumulate the word and remeasure the text.
			Else
				If xx.Length <= Text.Length Then Text = Text.Substring(xx.Length) Else Text = ""
				zx &= xx
				wText = g.MeasureString(zx, F).Width

				' If the text now ends with vbLf, return it.
				If zx.EndsWith(vbLf) Then Return zx.TrimStart
			End If
		Loop While Text <> ""

		Return zx
	End Function
	'**********************************************************

	' Function to parse a line of text and return the next word
	' followed by any of a list of characters that make good
	' breakpoints in wrapped text.

	'**********************************************************
	Private Function GetNextWord(Text As String) As String

		' Declare variables.

		Dim jj As Integer
		Dim BreakChars As String = " ,: ;.\/-_" & vbLf

		' Loop through the supplied text looking for a breaking
		' character, and return the text up to and including
		' that character.

		For jj = 0 To Text.Length - 1
			If BreakChars.Contains(Text.Chars(jj)) Then
				Return Text.Substring(0, jj + 1)
			End If
		Next jj
		Return Text

	End Function
	'***************************************************************************

	' Overloads for the WordWrap function.

	'***************************************************************************
	Public Function WordWrap(ByRef g As Graphics, F As Font, ByRef Text As String, wd As Single) As String
		Return WordWrap(g, F, Text, CInt(wd))
	End Function
	Public Function WordWrap(ByRef g As Graphics, F As Font, ByRef Text As String, wd As Double) As String
		Return WordWrap(g, F, Text, CInt(wd))
	End Function

	'***************************************************************************
	'
	' Function to return the workstation ID, used for networking applications.
	' Input : None
	' Output: workstation ID 01-99.  If none is defined, "01" is the default
	'
	'***************************************************************************
	Public Function WSID() As String

		' Declare variables

		Dim zx0 As String

		zx0 = System.Environment.GetEnvironmentVariable("WSID", EnvironmentVariableTarget.User)
		If zx0 <> "" Then
			If Val(zx0) > 0 Then
				zx0 = Right("000" & zx0, 3)
			Else
				zx0 = Left(Trim(zx0), 3)
			End If
			WSID = zx0
		Else
			WSID = "001"
		End If

	End Function

	'***********************************************************
	'
	' Function to return the text width of a string in a picture
	' box or form.
	'
	'***********************************************************
	Public Function TextWidth(ByVal Target As Object, ByVal T As String, Optional ByRef F As Font = Nothing) As Single

		' Declare variables

		Dim g As Graphics = Nothing
		Dim fN As Font

		' Create the graphics for the target.

		If TypeOf (Target) Is PictureBox Or TypeOf (Target) Is Panel Then g = Target.CreateGraphics Else Return 0
		If F Is Nothing Then fN = Target.font Else fN = F

		' Return the value of the width

		Return g.MeasureString(T, fN).Width

	End Function
	'***********************************************************
	'
	' Function to return the text height of a string in a picture
	' box or form.
	'
	'***********************************************************
	Public Function TextHeight(ByVal Target As Object, ByVal T As String, Optional ByRef F As Font = Nothing) As Single

		' Declare variables

		Dim g As Graphics = Nothing
		Dim fN As Font

		' Create the graphics for the target.

		If TypeOf (Target) Is PictureBox Or TypeOf (Target) Is Panel Then g = Target.CreateGraphics Else Return 0
		If F Is Nothing Then fN = Target.font Else fN = F

		' Return the value of the text height

		Return g.MeasureString(T, fN).Height


	End Function
	'***********************************************************
	'
	' Function to locate a text box by a pseudo-index.
	'
	'***********************************************************
	Public Function FindTextBox(ByVal c As System.ComponentModel.Container, ByVal Name As String, ByVal Index As Integer) As TextBox

		' Declare variables

		Dim t As TextBox

		For Each t In c.Components
			If t.Name = Name & Index Then Return t
		Next t

		Return Nothing

	End Function

	'***********************************************************
	'
	' Function to locate a text box by a pseudo-index.
	'
	'***********************************************************
	Public Function FindLabel(ByVal c As System.ComponentModel.Container, ByVal Name As String, ByVal Index As Integer) As Label

		' Declare variables

		Dim l As Label

		For Each l In c.Components
			If l.Name = Name & "_" & Index Then Return l
		Next l

		Return Nothing

	End Function
	'***********************************************************
	'
	' Function to format numbers or dates
	'
	'***********************************************************
	Public Function Format(ByVal Value As Object, Optional ByVal Template As String = "") As String

		' Declare variables

		Dim ii As Integer
		Dim jj As Integer
		Dim m As Integer
		Dim d As Integer
		Dim y As Integer
		Dim s1 As String = ""
		Dim s2 As String = ""
		Dim s3 As String = ""
		Dim div As String = ""
		Dim BeforeTemplate As String = ""
		Dim AfterTemplate As String = ""
		Dim RawNum As String = ""
		Dim FormattedNum As String = ""
		Dim FormattedDate As String = ""
		Dim zx0 As String = ""
		Dim zx1 As String = ""
		Dim zx2 As String = ""
		Dim zx3 As String = ""
		Dim dt As Date

		' If no format is supplied, just return a string representation of the number

		Format = "" ' Default return value
		If Template = "" Then
			Return CStr(Value)

			' If a format is specified, determine the type of object

		ElseIf TypeOf (Value) Is Date Then

			' Get the supplied value as a date (saves using CType on every call to its members)

			dt = Value

			' get the portions of the date

			y = dt.Year
			m = dt.Month
			d = dt.Day

			' Get the sections of the format

			zx0 = Template
			If InStr(1, Template, "/") > 0 Then
				div = "/"
				s1 = ParseString(zx0, "/")
				s2 = ParseString(zx0, "/")
				s3 = zx0
			ElseIf InStr(1, Template, "-") > 0 Then
				div = "-"
				s1 = ParseString(zx0, "-")
				s2 = ParseString(zx0, "-")
				s3 = zx0
			Else
				s1 = zx0
				s2 = ""
				s3 = ""
			End If


			' Now begin formatting the date

			For jj = 1 To 3
				zx1 = Choose(jj, s1, s2, s3).tolower
				Select Case zx1
					Case "m"
						FormattedDate = FormattedDate & m
					Case "mm"
						FormattedDate = FormattedDate & Right("0" & m, 2)
					Case "d"
						FormattedDate = FormattedDate & d
					Case "dd"
						FormattedDate = FormattedDate & Right("0" & d, 2)
					Case "yy"
						FormattedDate = FormattedDate & Right(y, 2)
					Case "yyyy"
						FormattedDate = FormattedDate & y
				End Select
				If jj = 1 And (s2 <> "" Or s3 <> "") Then FormattedDate = FormattedDate & div
				If jj = 2 And s3 <> "" Then FormattedDate = FormattedDate & div
			Next
			Return FormattedDate
		ElseIf TypeOf (Value) Is Double Or TypeOf (Value) Is Single Or TypeOf (Value) Is Integer Then

			' Get the raw number, ignorning negative signs

			RawNum = CStr(Math.Abs(Value))

			' Get both the format and the raw number halves: before and after any decimal place

			zx0 = ValidateFormat(Template)
			BeforeTemplate = ParseString(zx0, ".")
			AfterTemplate = zx0

			zx0 = RawNum
			zx1 = ParseString(zx0, ".")
			zx2 = zx0

			' Determine how many places, if any, in the before format are occupied by
			' commas.

			ii = 0
			For jj = 0 To BeforeTemplate.Length - 1
				If BeforeTemplate.Substring(jj, 1) = "," Then ii = ii + 1
			Next jj

			' Make sure the before and after portions of both the format and the number are of equal length

			zx3 = BeforeTemplate.Replace(",", "")
			If zx1.Length < zx3.Length Then
				zx1 = LPad(zx1, BeforeTemplate.Length)
			ElseIf zx3.length < zx1.length Then
				BeforeTemplate = LPad(BeforeTemplate, zx1.Length + ii)
			End If
			zx3 = AfterTemplate.Replace(",", "")
			If zx2.Length < zx3.Length Then
				zx2 = RPad(zx2, AfterTemplate.Length)
			ElseIf zx3.length < zx2.length Then
				AfterTemplate = RPad(AfterTemplate, zx2.Length)
			End If

			' Begin matching up the number and the format character by character and using the 
			' format to assemble the number.

			ii = 0
			For jj = 0 To BeforeTemplate.Length - 1
				s1 = BeforeTemplate.Substring(jj, 1)
				s2 = zx1.Substring(ii, 1)
				Select Case s1
					Case "#", " "
						If s2 <> " " Then FormattedNum = FormattedNum & s2
						ii = ii + 1
					Case "0"
						If s2 = " " Then
							FormattedNum = FormattedNum & "0"
						Else
							FormattedNum = FormattedNum & s2
						End If
						ii = ii + 1
					Case ","
						If Val(FormattedNum) > 0 Then FormattedNum = FormattedNum & ","
				End Select
			Next jj
			If Template.IndexOf(".") >= 0 Then FormattedNum = FormattedNum & "." ' Add decimal according to format.
			For jj = 0 To AfterTemplate.Length - 1
				s1 = AfterTemplate.Substring(jj, 1)
				s2 = zx2.Substring(jj, 1)
				Select Case s1
					Case "#"
						If s2 <> " " Then FormattedNum = FormattedNum & s2
					Case "0"
						If s2 = " " Then
							FormattedNum = FormattedNum & "0"
						Else
							FormattedNum &= s2
						End If
				End Select
			Next jj
			If Value < 0 Then FormattedNum = "-" & FormattedNum
			Return FormattedNum.TrimStart
		End If

	End Function
	'***********************************************************

	' Function to remove any invalid character from the format template.

	'***********************************************************
	Private Function ValidateFormat(ByVal Template As String) As String

		' Declare variables

		Dim jj As Integer
		Dim ii As Integer
		Dim zx0 As String

		' Look for date specifiers, to decide how to validate the format

		zx0 = ""
		ii = 0
		If Template.IndexOf("-") >= 0 Or Template.IndexOf("/") >= 0 Or Template.ToLower.IndexOf("m") >= 0 Or Template.ToLower.IndexOf("d") >= 0 Or Template.ToLower.IndexOf("y") >= 0 Then
			For jj = 1 To Len(Template)
				If Template.Substring(jj, 1) = "-" Or Template.Substring(jj, 1) = "/" Or Template.Substring(jj, 1).ToLower = "m" Or Template.Substring(jj, 1).ToLower = "d" Or Template.Substring(jj, 1).ToLower = "y" Then
					ii += 1
					zx0 &= Template.Substring(jj, 1)
				End If
			Next jj
		Else
			If Template.Substring(0, 1) = "0" Or Template.Substring(0, 1) = "#" Or Template.Substring(0, 1) = "." Then
				For jj = 0 To Template.Length - 1
					If Template.Substring(jj, 1) = "0" Or Template.Substring(jj, 1) = "#" Or Template.Substring(jj, 1) = "," Or Template.Substring(jj, 1) = "." Then
						ii += 1
						zx0 &= Template.Substring(jj, 1)
					End If
				Next jj
			End If
		End If
		ValidateFormat = zx0 ' Return validated template
	End Function

	'**********************************************************

	' Class definition for RotatedText class.

	'**********************************************************
	Public Class RotatedText
		Public Property Text As String
		Public Property Font As Font
		Public Property Angle As Single
		Public Property Origin As Point
		Public Property TextColor As Color = Color.Black
		Public Sub New(_text As String, _font As Font, _angle As Single, _origin As Point)
			Text = _text
			Font = _font
			Angle = _angle
			Origin = _origin
		End Sub
		Public Sub Print(g As Graphics)
			g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
			g.TranslateTransform(Origin.X, Origin.Y)
			g.RotateTransform(Angle)
			g.DrawString(Text, Font, New SolidBrush(TextColor), 0, 0)
			g.ResetTransform()
		End Sub
	End Class
End Module