Public Class AlbumSong

	' Define a structure to hold the album, album image and song info.
	Public Artist As String
	Public Album As String
	Public Song As String
	Public AlbumFolder As String

	Public Sub New(mArtist As String, mAlbum As String, mSong As String, mAlbumFolder As String)
		Artist = mArtist.Replace("&apos;", "'").Replace("&amp;", "&")
		Album = mAlbum.Replace("&apos;", "'").Replace("&amp;", "&")
		Song = mSong.Replace("&apos;", "'").Replace("&amp;", "&")
		AlbumFolder = mAlbumFolder.Replace("&apos;", "'").Replace("&amp;", "&")
	End Sub

End Class

