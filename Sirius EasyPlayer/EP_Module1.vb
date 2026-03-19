Imports System.Data.SqlClient
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Net
Imports System.Security.Cryptography
Imports System.Text
Imports System.Xml
Imports TagLib.Audible
Imports vb = Microsoft.VisualBasic
Imports Newtonsoft.Json.Linq
Imports System.Drawing
Imports System.Net.Http
Imports System.Runtime.InteropServices

Public Module EP_Module1

	'***********************************************************************
	' Sirius Sirius EasyPlayer Module 1
	' EP_MODULE1.VB
	' Written: March 2026
	' Programmer: Aaron Scott
	' Copyright 2026 Sirius Software All Rights Reserved
	'***********************************************************************

	' Define the location for the music library. It will reside on the first-level directory
	' of the program's location.

	Public MusicLibraryDatabase As String = My.Application.Info.DirectoryPath.Substring(0, 3) & "Sirius EasyPlayer\MusicLibrary.mdf"

	' Declare public variables.

	Public DB As New SqlConnection
	Public LibraryDS As New DataSet
	Public LibraryTable As DataTable
	Public ControlDS As New DataSet
	Public ControlTable As DataTable
	Public AlbumSongList As New List(Of AlbumSong)

	' Create an Enum for the 4 components of a color theme.

	Public Enum ThemeItem
		All
		MainWindow
		BackgroundColorNumber
		TextColorNumber
		FontNumber
	End Enum
	Public Enum CoverArtSize
		Small
		Large
	End Enum

	'***********************************************************************

	' Function to import a list of music into the music library, and return
	' the result.

	' Return values: -1 :  Cannot find root path
	'                 0 :  Successful import of all music
	'                >0 :  Count of files not imported.

	'***********************************************************************

	Public Function ImportMusicList(RootPath As String, ByRef lblStatus As ToolStripLabel) As Integer

		' Declare variables

		Dim MissedCount As Integer = 0
		Dim AddedCount As Integer = 0
		Dim zx As String
		Dim filelist As IReadOnlyCollection(Of String)
		Dim ArtistName As String
		Dim AlbumName As String
		Dim SongName As String
		Dim ImageFile As String = ""
		Dim ExcludedFolders As String() = {"album art", "playlists", "my playlists", "license backup"}
		Dim Command As SqlCommand = Nothing

		' If no music folder can be found on the specified drive, exit and do nothing.

		If Not Directory.Exists(RootPath) Then Return -1

		' Begin navigating the music folder structure
		For Each ArtistDir In Directory.GetDirectories(RootPath)
			Try
				ArtistName = Path.GetFileName(ArtistDir).Replace("'", "''")
				AlbumName = ""
				SongName = ""
				ImageFile = ""

				' Skip excluded folders
				If ExcludedFolders.Contains(ArtistName.ToLower) Then Continue For

				' Add an entry for the artist.

				zx = GenerateMusicHash(ArtistName, "", "")
				Command = New SqlCommand("INSERT INTO [Library] (HashCode, ArtistName, AlbumName, SongName, AlbumImage) VALUES ('" & zx & "','" & ArtistName & "','','','')", DB)
				Command.ExecuteNonQuery()

				' Loop through the album folders beneath the artist folder.
				For Each AlbumDir In Directory.GetDirectories(ArtistDir)

					' Create a new music item for the album and set the album name and image key.
					AlbumName = Path.GetFileName(AlbumDir).Replace("'", "''")

					' Look for the first (there are usually several) of the small album images
					filelist = Directory.GetFiles(AlbumDir, "*.*", SearchOption.TopDirectoryOnly)
					ImageFile = ""
					For Each ImageFile In filelist
						If ImageFile.ToLower.EndsWith("_small.jpg") Then Exit For
					Next ImageFile

					' Add an entry for the album.

					zx = GenerateMusicHash(ArtistName, AlbumName, "")
					Command = New SqlCommand("INSERT INTO [Library] (HashCode, ArtistName, AlbumName, SongName, AlbumImage) VALUES ('" & zx & "','" & ArtistName & "','" & AlbumName & "','','" & ImageFile.Replace("'", "''") & "')", DB)
					Command.ExecuteNonQuery()

					' Process songs within album
					Dim SongFiles As IReadOnlyCollection(Of String) = My.Computer.FileSystem.GetFiles(AlbumDir, FileIO.SearchOption.SearchTopLevelOnly, ExtensionPrecedenceWildcards)
					For Each SongFile In SongFiles

						SongName = Path.GetFileName(SongFile).Replace("'", "''")
						lblStatus.Text = "Adding " & AlbumName & SongName
						Application.DoEvents()

						' Build a unique hash code of artist, album and song.

						zx = GenerateMusicHash(ArtistName, AlbumName, SongName)
						Command = New SqlCommand("INSERT INTO [Library] (HashCode, ArtistName, AlbumName, SongName, AlbumImage) VALUES ('" & zx & "','" & ArtistName & "','" & AlbumName & "','" & SongName & "','')", DB)
						Command.ExecuteNonQuery()
						AddedCount += 1
					Next SongFile

				Next AlbumDir
			Catch ex As Exception
				MsgBox("Error adding music item." & vbCrLf & ex.Message, MsgBoxStyle.Information, "Build Music Tree")
				MissedCount += 1
			End Try
		Next ArtistDir

		' Report the results.

		If MissedCount > 0 Then
			MsgBox("Music successfully imported, with errors: " & MissedCount & " file(s) were skipped.", MsgBoxStyle.Information, "Import Music List")
		Else
			MsgBox("Music successfully imported: " & AddedCount & " file(s) were added to the music library.", MsgBoxStyle.Information, "Import Music List")
		End If
		lblStatus.Text = ""
		Application.DoEvents()

		' Dispose of the command object, if it's been used.

		If Command IsNot Nothing Then Command.Dispose()

		' Return the result.

		Return MissedCount

	End Function
	'***********************************************************************

	' Function to update a list of music into the music library, and return
	' the result.

	' Return values: -1 :  Cannot find root path
	'                >0 :  Count of new files added.

	'***********************************************************************

	Public Function UpdateMusicList(RootPath As String, ByRef lblStatus As ToolStripLabel) As Integer

		' Declare variables

		Dim MissedCount As Integer = 0
		Dim AddedCount As Integer = 0
		Dim zx As String
		Dim filelist As IReadOnlyCollection(Of String)
		Dim ArtistName As String
		Dim AlbumName As String
		Dim SongName As String
		Dim ImageFile As String = ""
		Dim ExcludedFolders As String() = {"album art", "playlists", "my playlists", "license backup"}
		Dim AlbumArt As Image
		Dim Command As SqlCommand = Nothing
		Dim Matches As DataRow()

		' If no music folder can be found on the specified drive, exit and do nothing.

		If Not Directory.Exists(RootPath) Then Return -1

		' Begin navigating the music folder structure

		For Each ArtistDir In Directory.GetDirectories(RootPath)
			Try
				ArtistName = Path.GetFileName(ArtistDir).Replace("'", "''")
				AlbumName = ""
				SongName = ""

				' Skip excluded folders
				If ExcludedFolders.Contains(ArtistName.ToLower) Then Continue For

				' Add an entry for the artist.

				zx = GenerateMusicHash(ArtistName, "", "")
				Matches = LibraryTable.Select("HashCode='" & zx & "'")
				If Matches.Count = 0 Then
					Command = New SqlCommand("INSERT INTO [Library] (HashCode, ArtistName, AlbumName, SongName, AlbumImage) VALUES ('" & zx & "','" & ArtistName.Replace("'", "''") & "','','','')", DB)
					Command.ExecuteNonQuery()
				End If

				' Loop through the album folders beneath the artist folder.
				For Each AlbumDir In Directory.GetDirectories(ArtistDir)

					' Create a new music item for the album and set the album name and image key.
					AlbumName = Path.GetFileName(AlbumDir)
					' Look for the first (there are usually several) of the large album images
					filelist = Directory.GetFiles(AlbumDir, "*large.jpg", SearchOption.TopDirectoryOnly)

					' If we found album art, look for the small version to save for the library
					' display.

					If filelist.Count > 0 Then
						filelist = Directory.GetFiles(AlbumDir, "*small.jpg", SearchOption.TopDirectoryOnly)
						If filelist.Count > 0 Then ImageFile = filelist(0) ' Save first small image found
					Else

						' If we found no album art, try to retrieve it from the intenet.

						AlbumArt = GetAlbumArt(ArtistName, AlbumName)
						If AlbumArt IsNot Nothing Then
							Dim imageGuid As String = AlbumArt.FrameDimensionsList(0).ToString
							' Save the original image as AlbumArtLarge.jpg
							Using LargeArt As New Bitmap(AlbumArt, New Size(250, 250))
								LargeArt.Save($"{AlbumDir}\AlbumArt_{imageGuid}_Large.jpg", ImageFormat.Jpeg)
							End Using

							' Resize to 48x48
							Using smallArt As New Bitmap(AlbumArt, New Size(48, 48))
								smallArt.Save($"{AlbumDir}\AlbumArt_{imageGuid}_Small.jpg", ImageFormat.Jpeg)
							End Using
							ImageFile = AlbumDir & "\AlbumArtSmall.jpg"
						Else
							ImageFile = ""
						End If
					End If


					' Add an entry for the album.

					zx = GenerateMusicHash(ArtistName, AlbumName, "")
					Matches = LibraryTable.Select("HashCode='" & zx & "'")
					If Matches.Count = 0 Then
						Command = New SqlCommand("INSERT INTO [Library] (HashCode, ArtistName, AlbumName, SongName, AlbumImage) VALUES ('" & zx & "','" & ArtistName.Replace("'", "''") & "','" & AlbumName.Replace("'", "''") & "','','" & ImageFile.Replace("'", "''") & "')", DB)
						Command.ExecuteNonQuery()
					End If

					' If there is no album art specified, and we have a file name, update the library.  This ensures that any
					' album art added after an album gets associated with the album.

					If ImageFile <> "" AndAlso Matches.Count > 0 AndAlso GetR(Matches(0), "AlbumImage") = "" Then
						Command = New SqlCommand("UPDATE [Library] SET AlbumImage='" & ImageFile.Replace("'", "''") & "' WHERE HashCode='" & zx & "'", DB)
						Command.ExecuteNonQuery()
					End If

					' Process songs within album
					Dim SongFiles As IReadOnlyCollection(Of String) = My.Computer.FileSystem.GetFiles(AlbumDir, FileIO.SearchOption.SearchTopLevelOnly, ExtensionPrecedenceWildcards)
					For Each SongFile In SongFiles

						SongName = Path.GetFileName(SongFile)
						lblStatus.Text = $"Checking {AlbumName} {SongName}"
						Application.DoEvents()

						' Build a unique hash code of artist, album and song.

						zx = GenerateMusicHash(ArtistName, AlbumName, SongName)
						Matches = LibraryTable.Select("HashCode='" & zx & "'")
						If Matches.Count = 0 Then
							Command = New SqlCommand("INSERT INTO [Library] (HashCode, ArtistName, AlbumName, SongName, AlbumImage) VALUES ('" & zx & "','" & ArtistName.Replace("'", "''") & "','" & AlbumName.Replace("'", "''") & "','" & SongName.Replace("'", "''") & "','')", DB)
							Command.ExecuteNonQuery()
							AddedCount += 1
						End If
					Next SongFile

				Next AlbumDir
			Catch ex As Exception
				'MsgBox("Error adding music item." & vbCrLf & ex.Message, MsgBoxStyle.Information, "Build Music Tree")
				MissedCount += 1
			End Try
		Next ArtistDir

		lblStatus.Text = ""
		Application.DoEvents()

		' Dispose of the command object, if it's been used.

		If Command IsNot Nothing Then Command.Dispose()

		' Return the result.

		Return AddedCount

	End Function
	'*******************************************************************

	' Sub to create a new music Library.

	'*******************************************************************
	Public Sub CreateNewDatabase()

		' Declare variables

		Dim Command As SqlCommand

		Try
			'Get the server name.  The default is the old 2012 version of SQL Server Express.

			ServerName = "(LocalDB)\" & GetSetting("SiriusSoftwareGlobal", "SQLServer", "InstanceName", "v11.0")

			' Open a temporary connection.

			Dim TempConnection As New SqlConnection("Data Source=" & ServerName & ";database='';integrated security=true")
			TempConnection.Open()

			' Remove any failed attempts to create the database.

			Try
				Command = New SqlCommand("ALTER DATABASE [MusicLibrary] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;", TempConnection)
				Command.ExecuteNonQuery()
			Catch
			End Try
			Command = New SqlCommand("DROP DATABASE IF EXISTS [MusicLibrary];", TempConnection)
			Command.ExecuteNonQuery()
			Command = New SqlCommand("DROP TABLE IF EXISTS [Library];", TempConnection)
			Command.ExecuteNonQuery()
			Command = New SqlCommand("DROP TABLE IF EXISTS [Control];", TempConnection)
			Command.ExecuteNonQuery()

			' Set up the command to create the database, and execute it.

			Command = New SqlCommand("CREATE DATABASE [MusicLibrary] ON (NAME='MusicLibrary', FILENAME='" & MusicLibraryDatabase & "')", TempConnection)
			Command.ExecuteNonQuery()

			Command = New SqlCommand("USE [MusicLibrary];CREATE TABLE [Library]([ID] Int Not NULL IDENTITY (1, 1),[HashCode] CHAR(64) Not NULL UNIQUE, [ArtistName] NVARCHAR(255) Not NULL,[AlbumName] NVARCHAR(255) Not NULL,[SongName] NVARCHAR(255) Not NULL,[AlbumImage] NVARCHAR(255) NULL,[Fallback] BIT NOT NULL DEFAULT ((0)),PRIMARY KEY CLUSTERED ([ID] ASC)); CREATE NONCLUSTERED INDEX ArtistName ON [Library] (ArtistName ASC);", TempConnection)
			Command.ExecuteNonQuery()

			Command = New SqlCommand("USE [MusicLibrary];CREATE TABLE [Control]([ID]  INT  NOT NULL IDENTITY (1,1), [ItemName]  NCHAR (50) NOT NULL UNIQUE, [Value]  NVARCHAR(MAX)  NULL, PRIMARY KEY CLUSTERED ([ID] ASC))", TempConnection)
			Command.ExecuteNonQuery()

			Command = New SqlCommand("USE [MusicLibrary];INSERT INTO [Control] (	[ItemName], [Value]) VALUES('DBVersion', '" & ProgramName & " Ver. " & DBVersion & "')", TempConnection)
			Command.ExecuteNonQuery()

			Command.Dispose()

			TempConnection.Close()
			TempConnection.Dispose()

			' Inform the user how to log on to the new database

			MsgBox("New music library has been successfully created.", MsgBoxStyle.Information, "Create New Music Library")

			' Open the new database.

			DbOpen = OpenADatabase(MusicLibraryDatabase)
		Catch e As Exception
			MsgBox("Failed To create New music library." & vbCrLf & e.Message, MsgBoxStyle.Exclamation, ProgramName)

		End Try

	End Sub
	'*******************************************************************

	' Function to open a database and return the open status.
	' In fact, we only open a connection to the database for the sole
	' purpose of determining it can be accessed.  All of the datasets
	' access their tables regardless of this connection.

	'*******************************************************************
	Public Function OpenADatabase(Optional OpenDatabaseName As String = "", Optional ReOpen As Boolean = False) As Boolean

		' Declare variables

		Dim o As Boolean
		Dim LibraryDA As New SqlDataAdapter
		Dim Command As New SqlCommand

		' Remember the datapath and the database name.

		Datapath = GetPath(OpenDatabaseName)
		Databasename = OpenDatabaseName

		' Now try to open a connection.  We'll return True or False depending on whether
		' we succeed.

		o = True
		Try

			' If a database is open, close it first.

			If DB.State = ConnectionState.Open Then CloseDatabase()


			DB.ConnectionString = MyConnectionString()

			' Now open the database.

			DB.Open()

			' Initialize all the data adapters.

			InitializeDataAdapters()

			' Attempt to open the library table.  If we cannot open it, this is not a Sirius EasyPlayer
			' database. 


			LibraryDA.SelectCommand = LibrarySelectCommand()
			LibraryDA.Fill(LibraryDS, "Table")
			LibraryTable = LibraryDS.Tables("Table")

			' Fill the control table dataset.

			ControlDA.SelectCommand = ControlSelectCommand()
			ControlDA.Fill(ControlDS, "Table")
			ControlTable = ControlDS.Tables("Table")


			If VersionChecking(GetControlItem("DBVersion")) <> 0 Then
				DB.Close()
				o = False
			End If
		Catch ex As Exception
			o = False
			MsgBox(Databasename & " cannot be opened Or Is Not an Music Library." & vbCrLf & ex.Message, MsgBoxStyle.Information, ProgramName)
		End Try

		Return o

	End Function
	'*******************************************************************

	' Sub to close the database.

	'*******************************************************************
	Public Sub CloseDatabase()

		' Check if the database is open. Do not rely on the DBOpen flag, which will
		' only reflect "open" if the OpenADatabase function finished. If it did not,
		' or if this routine is called from it (as when upgrading a database), the
		' database may be open before the DBOpen flag has been set.

		If DB.State = ConnectionState.Open And Not DbOpen Then DB.Close()

		' Make sure the database is open.

		If DbOpen Then


			' Detach the data adapters, which will remove the connection to the database.

			DetachDataAdapters()

			' Close the Database connection and dispose of those datasets
			' which are always open.

			LibraryTable.Dispose()
			LibraryTable = Nothing
			ControlTable.Dispose()
			ControlTable = Nothing
			DB.Close()

			' Indicate the database is now closed.

			DbOpen = False

			' Display the program name on the form caption

			frmMain.Text = ProgramName
		End If

	End Sub
	'**********************************************************
	'
	' Sub to get the text of an item from the item text table,
	' and return an optional default value if the
	' item does not exist.
	'
	'**********************************************************
	Public Function GetControlItem(ByRef ItemName As String, Optional ByRef DefaultValue As String = "") As String

		' Declare variables

		Dim xx As Integer
		Dim zx0 As String

		' Attempt to find the item name in the control table.
		' If there is no database open, use the default value

		xx = Find(ControlTable, "ItemName='" & ItemName & "'")
		If xx = NOMATCH Then
			zx0 = DefaultValue
		Else
			zx0 = GetR(ControlTable.Rows(xx), "Value")
		End If

		' Return the item value.

		GetControlItem = zx0

	End Function
	'**********************************************************
	'
	' Sub to save a control file item to the control file.
	' Input:  Key   =  The item to be saved
	'         Value = The value of the item saved
	'
	'**********************************************************
	Public Sub PutControlItem(ByRef Key As String, ByRef Value As String)

		' Declare variables

		Dim xx As Integer
		Dim dr As DataRow

		' Look for the existing key in the control file

		Try

			xx = Find(ControlTable, "ItemName='" & Key & "'")

			' If the key was not found, add a new entry.

			If xx = NOMATCH Then
				dr = ControlTable.NewRow
				dr("ItemName") = Left(Key, 50)
				dr("Value") = Value
				ControlTable.Rows.Add(dr)

				'If the key was found, edit the record to add the new
				'value, if one is supplied.

			ElseIf Value <> "" Then
				dr = ControlTable.Rows(xx)
				dr("Value") = Value

				'If the key is found, but there is no value supplied,
				'delete the key

			ElseIf Value = "" Then
				dr = ControlTable.Rows(xx)
				dr.Delete()

			End If

			' Update the record

			ControlDA.Update(ControlTable)

			' Trap for errors on an update

		Catch ex As Exception
			MsgBox("PutControlItem Failed." & vbCrLf & ex.Message, MsgBoxStyle.Critical, ProgramName)
			ControlTable.RejectChanges()
		End Try


	End Sub
	'***********************************************************************

	' Function to find a specific record in a datatable.

	'***********************************************************************
	Public Function Find(Table As DataTable, Criteria As String)

		' Declare variables

		Dim jj As Integer
		Dim FoundID As Integer
		Dim FoundRows() As DataRow

		' Get all the records that match the specified criteria.

		Try
			FoundRows = Table.Select(Criteria)

			' If no rows were found, return NOMATCH.

			If FoundRows.Count = 0 Then
				Return NOMATCH
			Else

				' Get the ID of the first found record.

				FoundID = FoundRows(0)("ID")

				' Now loop through the table to find the row number of the record with that ID.
				' Return that row number.

				For jj = 0 To Table.Rows.Count - 1
					If Table.Rows(jj)("ID") = FoundID Then
						Return jj
					End If
				Next jj

			End If

		Catch ex As Exception
		End Try

		' We should, ideally, never reach this.

		Return NOMATCH



	End Function

	'*******************************************************************

	' Function to return the name of the a file without the path.

	'*******************************************************************
	Public Function FileNameNoPath(d As String) As String

		' Declare variables

		Dim jj As Integer
		Dim zx As String = ""

		' Peel off the path information and return just the database name.

		For jj = d.Length - 1 To 0 Step -1
			If d.Substring(jj, 1) = "\" Then
				zx = d.Substring(jj + 1)
				Return zx
			End If
		Next jj

		Return d

	End Function
	'*******************************************************************

	' Function to return the path of a file name.

	'*******************************************************************
	Public Function GetPath(ByVal FileName As String) As String

		' Declare variables

		Dim jj As Integer
		Dim Datapath As String = ""

		If InStr(1, FileName, "\") > 0 Then
			For jj = Len(FileName) To 1 Step -1
				If Mid(FileName, jj, 1) = "\" Then
					Datapath = Left(FileName, jj)
					Exit For
				End If
			Next jj
		End If

		Return Datapath


	End Function
	'**********************************************************
	'
	' Sub to perform database version-checking.
	' Returns: 0 = Database version okay
	'          1 = Database version NOT okay
	'
	'**********************************************************
	Public Function VersionChecking(ByRef VersionText As String) As Integer

		' Declare variables

		Dim UpdateSuccessful As Boolean
		Dim DBName As String = ""
		Dim v1 As String
		Dim p1 As String
		Dim Cmd As SqlCommand = Nothing
		Dim Transaction As SqlTransaction = Nothing

		' Watch this spot for future version information!

		v1 = ""
		p1 = ""

		VersionChecking = 0

		' If the version stamp is blank, then we update it to a
		' 1.0 level (since that is where version stamping is
		' added).  Note that this is a DATABASE version
		' and not a program version.

		If VersionText = "" Then
			PutControlItem("DBVersion", ProgramName & " Ver. " & "1.0")
			VersionText = GetControlItem("DBVersion")
		End If

		' Get just the version number

		v1 = Mid(VersionText, InStr(1, VersionText, " Ver. ") + 6, Len(VersionText))

		' Get the program name

		p1 = Trim(Left(VersionText, InStr(1, VersionText, " Ver. ") - 1))

		' If the program name does not match, alert the user.

		If Left(ProgramName, Len(p1)) <> p1 Then
			MsgBox(Databasename & " Is Not a " & ProgramName & " database.", MsgBoxStyle.Critical, ProgramName)
			VersionChecking = 1
			Exit Function
		End If

		' If the database version is less than that defined in
		' frmMain, then we need to check for database upgrades

		UpdateSuccessful = True
		If Val(v1) < Val(DBVersion) Then

			' Get the name of the database minus the path.

			DBName = Path.GetFileName(Databasename)

			' Advise user what's happening

			Try
				frmMain.lblStatus.Text = "Upgrading Database"
				My.Application.DoEvents()

				' Inform the user an update must occur.

				MsgBox("This database requires upgrading.", MsgBoxStyle.Information, "Database Version Upgrade")


				' Begin update procedures.

				' See if the database version is less than 1.01

				If Val(v1) < 1.01 Then

				End If

			Catch ex As Exception
				Transaction.Rollback()
				UpdateSuccessful = False
				MsgBox("Upgrade database failed." & vbCrLf & ex.Message, MsgBoxStyle.Exclamation, "Database Version Upgrade")
			End Try


			' Update the database version if the update succeeded.


			If UpdateSuccessful Then
				PutControlItem("DBVersion", ProgramName & " Ver. " & DBVersion)
				'DatabaseChanged = True
			End If
		End If

		' Clear upgrade message

		frmMain.lblStatus.Text = ""
		My.Application.DoEvents()

	End Function
	'*******************************************************

	' Function to return a darker or brighter shade of a color.

	'*******************************************************
	Public Function DarkenOrLightenColor(C As Color, Change As Integer) As Color

		' Declare variables.

		Dim r As Integer
		Dim g As Integer
		Dim b As Integer

		' Get the RGB values of the supplied color.

		r = C.R
		g = C.G
		b = C.B

		' Increase or decrease the value of each color component. If the changed
		' value would be less than zero, make it zero; if greater than 255,
		' make it 255.

		If (r + r * Change / 100 >= 0) And (r + r * Change / 100 <= 255) Then
			r = r + r * Change / 100
		Else
			If Change > 0 Then r = 255 Else If Change < 0 Then r = 0
		End If
		If (g + g * Change / 100 >= 0) And (g + g * Change / 100 <= 255) Then
			g = g + g * Change / 100
		Else
			If Change > 0 Then g = 255 Else If Change < 0 Then g = 0
		End If
		If (b + b * Change / 100 >= 0) And (b + b * Change / 100 <= 255) Then
			b = b + b * Change / 100
		Else
			If Change > 0 Then b = 255 Else If Change < 0 Then b = 0
		End If

		' Return the assembled new color.  The alpha doesn't change.

		Return Color.FromArgb(C.A, r, g, b)

	End Function
	'***********************************************************************

	' Function to get the cover art image for an album.

	'***********************************************************************
	Public Function GetCoverArt(AlbumPath As String, ImageSize As CoverArtSize) As Image

		Dim img As Image = Nothing
		Dim xx As IReadOnlyCollection(Of String)

		Try

			xx = My.Computer.FileSystem.GetFiles(AlbumPath, FileIO.SearchOption.SearchTopLevelOnly, "*.*")
			If xx.Count > 0 Then
				For Each File In xx
					If (File.ToLower.EndsWith("large.jpg") And ImageSize = CoverArtSize.Large) OrElse (File.ToLower.EndsWith("small.jpg") And ImageSize = CoverArtSize.Small) Then
						img = Image.FromFile(File)
						Exit For
					End If
				Next File
			End If
		Catch ex As Exception
		End Try

		Return img

	End Function
	'***********************************************************************

	' Function to generate a unique hash code from an artist/album/song name.

	' This function was generated by Microsoft Copilot.

	'***********************************************************************
	Public Function GenerateMusicHash(artist As String, album As String, song As String) As String

		Dim inputString As String = $"{artist.ToLower}|{album.ToLower}|{song.ToLower}" ' Concatenating with a separator
		Dim sha256 As SHA256 = SHA256.Create()
		Dim hashBytes As Byte() = sha256.ComputeHash(Encoding.UTF8.GetBytes(inputString))
		Return BitConverter.ToString(hashBytes).Replace("-", "") ' Convert to hex string

	End Function
	'***********************************************************************

	' Function to parse a windows playlist and return

	' This function was generated by Microsoft Copilot.

	'***********************************************************************
	Public Function ParseWindowsPlaylist(playlistFile As String) As List(Of AlbumSong)

		' Declare variables.

		Dim albumSongList As New List(Of AlbumSong)()

		' Make sure the playlist exists.

		If Not System.IO.File.Exists(playlistFile) Then
			Debug.WriteLine($"Playlist not found: {playlistFile}")
			Return albumSongList
		End If

		' Begin parsing the playlist, which is an XML document.

		Try
			Dim xmlDoc As New XmlDocument()
			xmlDoc.Load(playlistFile)

			Dim mediaNodes As XmlNodeList = xmlDoc.SelectNodes("//smil/body/seq/media")

			For Each Node As XmlNode In mediaNodes
				Dim songPath As String = Node.Attributes("src")?.Value

				If Not String.IsNullOrEmpty(songPath) Then
					Dim albumFolder As String = Path.GetDirectoryName(songPath)
					Dim trackTitle As String = Path.GetFileNameWithoutExtension(songPath)

					' Extract album and artist from folder structure
					Dim pathParts As String() = albumFolder.Split(Path.DirectorySeparatorChar)
					Dim albumTitle As String = If(pathParts.Length >= 2, pathParts.Last(), "Unknown Album")
					Dim albumArtist As String = If(pathParts.Length >= 3, pathParts(pathParts.Length - 2), "Unknown Artist")

					albumSongList.Add(New AlbumSong(albumArtist, albumTitle, trackTitle, songPath))
				End If
			Next Node

		Catch ex As Exception
			Debug.WriteLine($"Error parsing playlist: {ex.Message}")
		End Try

		Return albumSongList
	End Function
	'***********************************************************************

	' Sub to create a default image of a musical note to replace
	' any album image we cannot locate.

	'***********************************************************************
	Public Function GetNoAlbumArtImage(Optional ImgSize As Size? = Nothing) As Bitmap

		' Declare variables.

		Dim ImageSize As Size

		If ImgSize Is Nothing Then ImageSize = New Size(32, 32) Else ImageSize = ImgSize

		Dim bmp As New Bitmap(ImageSize.Width, ImageSize.Height) ' Size of a display album in the library.

		' Create an image for the default album imge.

		Using g As Graphics = Graphics.FromImage(bmp)
			g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
			g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit
			Using b As New LinearGradientBrush(New Rectangle(0, 0, ImageSize.Width, ImageSize.Height), Color.Blue, DarkenOrLightenColor(Color.LightBlue, +40), LinearGradientMode.ForwardDiagonal)
				g.FillRectangle(b, New Rectangle(0, 0, ImageSize.Width, ImageSize.Height))
				' Draw the musical note character
				Using font As New Font("Arial", CInt(ImageSize.Height * 0.44), FontStyle.Bold)
					Using fontBrush As New SolidBrush(Color.DarkBlue)
						g.DrawString("♫", font, fontBrush, ImageSize.Width / 5, ImageSize.Height / 8) ' Centered symbol
					End Using
				End Using
			End Using
		End Using

		Return bmp
	End Function
	'***********************************************************************

	' Function to make a song name from a windows playlist correct for
	' display.

	'***********************************************************************
	Public Function SanitizeSongName(SongName As String) As String

		Dim wx As String = SongName.Replace(" -", "")

		Dim zx As String
		If Val(wx) > 0 Or wx.IndexOf("_") = 4 Then zx = ParseString(wx, " ") 'remove leading numbers and index

		wx.Replace("&apos;", "'").Replace("&amp;", "&")

		Return wx

	End Function
	'***********************************************************************

	' Sub to remove the system atrribute from all album art in the music
	' folder.

	'***********************************************************************
	Public Sub RemoveSystemAttribute(MusicFolderPath As String)
		Try
			' Get all image files in the current folder
			For Each ImageFile As String In Directory.GetFiles(MusicFolderPath, "*.*", SearchOption.TopDirectoryOnly)
				If Path.GetExtension(ImageFile).Contains(".jpg") Then
					Dim fileInfo As New FileInfo(ImageFile)
					If fileInfo.Attributes And FileAttributes.System Then
						fileInfo.Attributes = fileInfo.Attributes And Not FileAttributes.System
						Debug.Print($"Updated: {ImageFile}")
					End If
				End If
			Next ImageFile

			' Recursively process subfolders
			For Each SubFolder As String In Directory.GetDirectories(MusicFolderPath)
				RemoveSystemAttribute(SubFolder)
			Next SubFolder
		Catch ex As Exception
			Debug.Print($"Error processing folder {MusicFolderPath}: {ex.Message}")
		End Try
	End Sub
	'***********************************************************************

	' Function to find an existing album art file.

	'***********************************************************************
	Public Function GetExistingAlbumArt(artist As String, album As String) As String

		' Declare variables

		Dim ii As Integer

		Dim ImageFiles As IReadOnlyCollection(Of String)
		ImageFiles = Directory.GetFiles(MusicFolder & artist & "\" & album, "*.jpg")
		If ImageFiles.Count > 0 Then
			For ii = 0 To ImageFiles.Count - 1
				If ImageFiles(ii).ToLower.Contains("}_small") Then Return ImageFiles(ii)
			Next ii
			Return ImageFiles(0)
		Else
			Return String.Empty
		End If
	End Function
	'***********************************************************************

	' Function to download the album art for a given artist/album.

	'***********************************************************************
	Public Function GetAlbumArt(artist As String, album As String) As Bitmap

		' Assemble the URL to fetch the album art.

		Dim jj As Integer
		Dim releaseGroupId As String
		Dim releaseId As String
		Dim imageUrl As String

		' Try the earliest release id first, to try to obtain the album art.

		For jj = 1 To 2
			If jj = 1 Then
				releaseId = GetEarliestReleaseId(artist, album) ' Fetch correct release ID
				imageUrl = $"https://coverartarchive.org/release/{releaseId}/front"

				' If we could find no alburm art with the earliest release id, try the releasegroupid
			Else
				releaseGroupId = GetReleaseGroupId(artist, album) ' ReleaseGroupID is required to specify album for the art.
				imageUrl = $"https://coverartarchive.org/release-group/{releaseGroupId}/front"
			End If

			' Download the image data, and convert to an image to be returned.

			Using client As New HttpClient()
				Try
					Dim imageData As Byte() = client.GetByteArrayAsync(imageUrl).Result
					Using ms As New MemoryStream(imageData)
						Return New Bitmap(ms)
					End Using
				Catch ex As Exception
				End Try
			End Using
		Next jj

		Return Nothing
	End Function
	'***********************************************************************

	' Function to obtain the ReleaseID for the earliest release of
	' an artist/album combination.

	'***********************************************************************
	Public Function GetEarliestReleaseId(artist As String, album As String) As String

		' Assemble the query string for retrieving the artist/album information.

		Dim queryUrl As String = $"https://musicbrainz.org/ws/2/release/?query=artist:{Uri.EscapeDataString(artist)} release:{Uri.EscapeDataString(album)}&fmt=json&limit=100"

		' Retrieve the album information.  This is a large mess of information.

		Using client As New HttpClient()
			client.DefaultRequestHeaders.UserAgent.ParseAdd("MyMusicApp/1.0")

			Try
				Dim jsonResponse As String = client.GetStringAsync(queryUrl).Result
				Dim jsonData As JObject = JObject.Parse(jsonResponse)

				' Get just the releases information.
				Dim releases As JArray = jsonData("releases")

				If releases IsNot Nothing AndAlso releases.Count > 0 Then
					' Sort releases by date and get the earliest one
					Dim earliestRelease = releases.OrderBy(Function(r) r("date")?.ToString()).FirstOrDefault()
					If earliestRelease IsNot Nothing Then
						Dim releaseId As String = earliestRelease("id").ToString()
						Return releaseId
					End If
				End If
			Catch ex As Exception
				Debug.WriteLine("Error fetching earliest release: " & ex.Message)
			End Try
		End Using

		Return Nothing
	End Function
	'***********************************************************************

	' Function to obtain the ReleaseGroupID for an artist/album combination.
	' This is a fallback, in case the earliest release id search fails.

	'***********************************************************************
	Public Function GetReleaseGroupId(artist As String, album As String) As String

		' Declare variables.

		Dim queryUrl As String = $"https://musicbrainz.org/ws/2/release-group/?query=artist:{Uri.EscapeDataString(artist)} release:{Uri.EscapeDataString(album)}&fmt=json"

		' Get the JSON data for the artist/album.

		Using client As New HttpClient()
			client.DefaultRequestHeaders.UserAgent.ParseAdd("MyMusicApp/1.0") ' User-Agent is required!

			Try
				Dim jsonResponse As String = client.GetStringAsync(queryUrl).Result

				' Parse the JSON data to get the releasegroupid.

				Dim jsonData As JObject = JObject.Parse(jsonResponse)
				Dim firstResult As JObject = jsonData("release-groups")?.First

				If firstResult IsNot Nothing Then
					Dim releaseGroupId As String = firstResult("id").ToString()
					Return releaseGroupId
				End If
			Catch ex As Exception
				Debug.WriteLine("Error fetching release group ID: " & ex.Message)
			End Try
		End Using

		Return Nothing
	End Function

	'***********************************************************************

	' Sub to repair all music files' metadata.

	'***********************************************************************
	Public Sub RepairMetadata()

		' Declare variables

		Dim filename As String
		Dim xx As String
		Dim zx As String
		Dim musiclist As IReadOnlyCollection(Of String)
		Dim parts() As String

		' Get all songs

		musiclist = My.Computer.FileSystem.GetFiles(MusicFolder, FileIO.SearchOption.SearchAllSubDirectories, ExtensionPrecedenceWildcards)

		' Iterate through the songs

		For Each filename In musiclist

			' Display the current song

			frmMain.lblStatus.Text = "Checking " & filename
			Application.DoEvents()

			' Get the tags for the song.

			Try
				Dim f = TagLib.File.Create(filename)

				' Get the title of each song without leading track number or extension.

				zx = Path.GetFileNameWithoutExtension(filename)
				If Val(zx) > 0 Then xx = ParseString(zx, " ") ' This removes leading track number

				' Extract the album and artist name from the path to the file.

				parts = filename.Split("\")

				' Don't process our playlists folder.

				If parts(1) <> "SPLE" Then

					' Check for missing metadata.

					If f.Tag.Title = "" Or f.Tag.FirstPerformer = "" Or f.Tag.Album = "" Then
						If f.Tag.FirstPerformer = "" Then f.Tag.Performers = {parts(2)}
						If f.Tag.Album = "" Then f.Tag.Album = parts(3)
						If f.Tag.Title = "" Then f.Tag.Title = zx
						f.Save()

						' Check for deformed title metadata: leading numbers, or paths in the title.

					ElseIf Val(f.Tag.Title) > 0 Or f.Tag.Title.Contains("\") Then
						f.Tag.Title = zx
						f.Save()

						' Check for a mismatch between the folder name and FirstPerformer

					ElseIf f.Tag.FirstPerformer <> parts(2) Then
						f.Tag.Performers = {parts(2)}
						f.Save()
					End If
				End If
			Catch ex As Exception
			End Try

		Next filename

		' Clear status message.

		frmMain.lblStatus.Text = ""
		Application.DoEvents()

	End Sub
	'**************************************************

	' Function to un-escape a special character in
	' an XML file.

	'**************************************************
	Public Function EscapeXml(value As String) As String
		If value Is Nothing Then Return ""

		Dim s As String = value
		s = s.Replace("&", "&amp;")
		s = s.Replace("<", "&lt;")
		s = s.Replace(">", "&gt;")
		s = s.Replace("""", "&quot;")
		s = s.Replace("'", "&apos;")

		Return s
	End Function
	'**************************************************

	' Function to un-escape a line in an XML file.

	'**************************************************
	Public Function UnescapeXml(value As String) As String
		If value Is Nothing Then Return ""

		Dim s As String = value
		s = s.Replace("&apos;", "'")
		s = s.Replace("&quot;", """")
		s = s.Replace("&gt;", ">")
		s = s.Replace("&lt;", "<")
		s = s.Replace("&amp;", "&")

		Return s
	End Function
	Public ReadOnly Property MusicFolder
		Get
			Return GetSetting("Sirius" & ProgramName.Replace(" ", ""), "Settings", "MusicFolder", "") & "\"
		End Get
	End Property
	'**************************************************
	'
	' Property to get or set the message box theme
	'
	'**************************************************
	Public Property ProgramColorTheme(Optional Component As ThemeItem = ThemeItem.All) As String
		Get

			' Declare variables

			Dim zx As String

			' Get the theme string.

			zx = GetSetting("Sirius" & SRep(ProgramName, 1, " ", ""), "ProgramColorTheme", "ThemeStyle", "0x1111")

			' Determine what portion of the theme to return.

			Select Case Component
				Case ThemeItem.All
					ProgramColorTheme = zx
				Case ThemeItem.MainWindow
					ProgramColorTheme = Mid(zx, 3, 1)
				Case ThemeItem.BackgroundColorNumber
					ProgramColorTheme = Mid(zx, 4, 1)
				Case ThemeItem.TextColorNumber
					ProgramColorTheme = Mid(zx, 5, 1)
				Case ThemeItem.FontNumber
					ProgramColorTheme = Mid(zx, 6, 1)
				Case Else
					ProgramColorTheme = zx
			End Select
		End Get
		Set(value As String)

			' Declare variables.

			Dim x1 As String
			Dim x2 As String
			Dim x3 As String
			Dim x4 As String

			' Extract the portions of the theme to test them.

			x1 = Mid(value, 3, 1)
			x2 = Mid(value, 4, 1)
			x3 = Mid(value, 5, 1)
			x4 = Mid(value, 6, 1)

			' Now validate each value.

			If (x1 < "1" Or x1 > "5") Or (x2 < "1" Or x2 > "3") Or (x3 < "1" Or x3 > "4") Or (x4 < "1" Or x4 > "2") Then

				' We will use the windows message box to display errors, to prevent reentrancy 
				' issues with the routine calling itself.

				vb.MsgBox("Invalid theme value: """ & value & """.  Theme not saved.", MsgBoxStyle.Exclamation, "Save Theme Property")
			Else

				' Save the new theme. 

				SaveSetting("Sirius" & SRep(ProgramName, 1, " ", ""), "ProgramColorTheme", "ThemeStyle", value)
			End If
		End Set
	End Property
	'**************************************************

	' Property to set or retrieve the file extension
	' precedence string set.

	'**************************************************
	Public ReadOnly Property ExtensionPrecedence As String()
		Get
			Dim zx As String = GetSetting("Sirius" & SRep(ProgramName, 1, " ", ""), "Settings", "FilePrecedence", ".flac,.ogg,.mp3,.wma,.wav")
			Dim Ext() As String = zx.Split(",")
			Return Ext
		End Get
	End Property
	'**************************************************

	' Functions to return wildcards of the extension
	' precedence array as a string array or a single string.

	'**************************************************
	Public Function ExtensionPrecedenceWildcards() As String()
		Return ExtensionPrecedence.Select(Function(ext) "*" & ext).ToArray()
	End Function

	Public Function ExtensionPrecedenceWildcardsString() As String
		Return String.Join(",", ExtensionPrecedence.Select(Function(ext) "*." & ext))
	End Function
End Module
