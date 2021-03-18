Public Class FileType
    Inherits ExplorerElementType

    Private _texture As Bitmap

    Sub New(_location As Point, _name As String, _textureName As String)
        Location = _location
        Name = _name
        Dim ResourceSet As Resources.ResourceSet = My.Resources.ResourceManager.GetResourceSet(Globalization.CultureInfo.CurrentCulture, True, True)
        For Each resource As DictionaryEntry In ResourceSet.OfType(Of Object)()
            If resource.Key.ToString = "fileType_" & _textureName Then
                _texture = resource.Value
                Exit Sub
            End If
        Next
    End Sub

    Public Overrides Function getTexture() As System.Drawing.Bitmap
        Return _texture
    End Function

    Public Overrides Function getAssociatedWall() As WallType
        Return New WallType(New VectorType(Par.EXPLORER_CORNER_X + Par.EXPLORER_INT_OFFSET_X, Par.EXPLORER_CORNER_Y + Par.EXPLORER_INT_OFFSET_Y) + New VectorType(Location.X + Par.FILE_Y_RETRAIT, Location.Y), Par.EXPLORER_ELEMENT_WIDTH - 2 * Par.FILE_Y_RETRAIT, Par.EXPLORER_ELEMENT_HEIGHT, Par.wallState.Explorer, True, False, "explorerElement", True, Me)
    End Function

End Class
