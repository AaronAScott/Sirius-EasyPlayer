Public Enum MusicItemType
	Artist
	Album
	Song
End Enum
Public Class DisplayLine
	Public ItemType As MusicItemType
	Public Selected As Boolean
	Public ArtistName As String = ""
	Public AlbumName As String = ""
	Public SongName As String = ""
	Public ImageFile As String = ""
	Public ImageBounds As Rectangle
	Public Bounds As Rectangle
	Public Index As Integer
End Class
Public Class DisplayLines

	Public DisplayLines As New List(Of DisplayLine)

	' Adds a DisplayLine object to the collection
	Public Sub Add(displayLine As DisplayLine)
		displayLine.Index = DisplayLines.Count
		DisplayLines.Add(displayLine)
	End Sub

	' Finds the DisplayLine that contains the given point
	Public Function Find(pt As Point) As DisplayLine
		Return DisplayLines.FirstOrDefault(Function(dl) dl.Bounds.Contains(pt) Or dl.ImageBounds.Contains(pt))
	End Function

	' Clears the collection when a new paint routine starts
	Public Sub Clear()
		displayLines.Clear()
	End Sub

	' Returns the number of DisplayLine objects
	Public ReadOnly Property Count As Integer
		Get
			Return displayLines.Count
		End Get
	End Property

	' Indexer to retrieve a specific DisplayLine
	Default Public ReadOnly Property Item(index As Integer) As DisplayLine
		Get
			Return displayLines(index)
		End Get
	End Property

	' Selects a specific DisplayLine and unselects all others
	Public Property SelectedLine As DisplayLine
		Get
			For Each dl In DisplayLines
				If dl.Selected Then
					Return dl
					Exit For
				End If
			Next dl
			Return Nothing
		End Get
		Set(value As DisplayLine)

			For Each dl In DisplayLines
				dl.Selected = False
			Next dl
			If Not value Is Nothing Then value.Selected = True
		End Set
	End Property

End Class
