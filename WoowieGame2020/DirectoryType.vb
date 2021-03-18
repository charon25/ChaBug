Public Class DirectoryType
    Inherits ExplorerElementType
    Private _childs As List(Of ExplorerElementType)
    Public Property Childs As List(Of ExplorerElementType)
        Get
            Return _childs
        End Get
        Set(value As List(Of ExplorerElementType))
            _childs = value
        End Set
    End Property
    Public IsReturn As Boolean
    Public CorruptedFilesList As List(Of CorruptedFileType)
    Public Parent As DirectoryType

    Sub New(_location As Point, _name As String, ByRef _parent As DirectoryType, Optional _isReturn As Boolean = False)
        Location = _location
        Name = _name
        _childs = New List(Of ExplorerElementType)
        If Not _isReturn Then _childs.Add(New DirectoryType(getCoordinates(0), "<--", Me, True))
        IsReturn = _isReturn
        Parent = _parent
        CorruptedFilesList = New List(Of CorruptedFileType)
    End Sub

    Public Overrides Function getTexture() As System.Drawing.Bitmap
        Return My.Resources.directory
    End Function

    Public Overrides Function getAssociatedWall() As WallType
        Return New WallType(New VectorType(Par.EXPLORER_CORNER_X + Par.EXPLORER_INT_OFFSET_X, Par.EXPLORER_CORNER_Y + Par.EXPLORER_INT_OFFSET_Y) + New VectorType(Location.X, Location.Y + Par.DIRECTORY_Y_RETRAIT), Par.EXPLORER_ELEMENT_WIDTH, Par.EXPLORER_ELEMENT_HEIGHT - Par.DIRECTORY_Y_RETRAIT, Par.wallState.Explorer, True, False, "explorerElement", True, Me)
    End Function


End Class
