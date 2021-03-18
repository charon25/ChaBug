Public Class WallType

    Private _boundaries As BoundaryType
    Public Property Boundaries As BoundaryType
        Get
            Return _boundaries
        End Get
        Set(value As BoundaryType)
            _boundaries = value
        End Set
    End Property
    Private _wallState As Par.wallState
    Public Property WallState As Par.wallState
        Get
            Return _wallState
        End Get
        Set(value As Par.wallState)
            _wallState = value
        End Set
    End Property
    Private _isDescendable As Boolean
    Public Property IsDescendable As Boolean
        Get
            Return _isDescendable
        End Get
        Set(value As Boolean)
            _isDescendable = value
        End Set
    End Property
    Private _hasTextures As Boolean
    Public Property HasTextures As Boolean
        Get
            Return _hasTextures
        End Get
        Set(value As Boolean)
            _hasTextures = value
        End Set
    End Property
    Private _isEssential As Boolean
    Public Property IsEssential As Boolean
        Get
            Return _isEssential
        End Get
        Set(value As Boolean)
            _isEssential = value
        End Set
    End Property
    Private texture As Bitmap
    Public ExplorerElement As ExplorerElementType
    Public Name As String
    Public Enabled As Boolean

    Sub New(_location As VectorType, _width As Integer, _height As Integer, _wallState As Par.wallState, _isDescendable As Boolean, _hasTextures As Boolean, _name As String, _isEssential As Boolean, Optional ByRef _explorerElement As ExplorerElementType = Nothing)
        _boundaries = New BoundaryType(_location, _width, _height)
        Me._wallState = _wallState
        Me._isDescendable = _isDescendable
        Me._hasTextures = _hasTextures
        ExplorerElement = _explorerElement
        Me._isEssential = _isEssential
        Name = _name
        Enabled = True
    End Sub
    Sub New()
        Enabled = True
    End Sub
    Public Function getAssociatedPlatform() As PlatformType
        Return New PlatformType(_boundaries.Location.X, _boundaries.Location.Y, _boundaries.Width, _isDescendable, _wallState = Par.wallState.Explorer, Name, _isEssential, ExplorerElement)
    End Function

    Public Sub generateTexture()
        If _wallState = Par.wallState.Normal Then
            Dim tempBitmap As New Bitmap(_boundaries.Width, _boundaries.Height)
            tempBitmap.SetResolution(Par.RESOLUTION, Par.RESOLUTION)
            Dim g As Graphics = Graphics.FromImage(tempBitmap)
            Dim maxX As Integer = _boundaries.Width - Par.WALL_TILE_SIZE - 1
            Dim maxY As Integer = _boundaries.Height - Par.WALL_TILE_SIZE - 1

            For i As Integer = Par.WALL_TILE_SIZE - 1 To maxX Step Par.WALL_TILE_SIZE
                For j As Integer = Par.WALL_TILE_SIZE - 1 To maxY Step Par.WALL_TILE_SIZE
                    g.DrawImage(Form1.Textures.getNormalWallTexture(Par.normalWallTexturesType.Center), i, j)
                Next
            Next
            For i As Integer = Par.WALL_TILE_SIZE - 1 To maxX Step Par.WALL_TILE_SIZE
                g.DrawImage(Form1.Textures.getNormalWallTexture(Par.normalWallTexturesType.TopBorder), i, 0)
                g.DrawImage(Form1.Textures.getNormalWallTexture(Par.normalWallTexturesType.BottomBorder), i, maxY)
            Next
            For j As Integer = Par.WALL_TILE_SIZE - 1 To maxY Step Par.WALL_TILE_SIZE
                g.DrawImage(Form1.Textures.getNormalWallTexture(Par.normalWallTexturesType.LeftBorder), 0, j)
                g.DrawImage(Form1.Textures.getNormalWallTexture(Par.normalWallTexturesType.RightBorder), maxX, j)
            Next
            g.DrawImage(Form1.Textures.getNormalWallTexture(Par.normalWallTexturesType.ULCorner), 0, 0)
            g.DrawImage(Form1.Textures.getNormalWallTexture(Par.normalWallTexturesType.URCorner), maxX, 0)
            g.DrawImage(Form1.Textures.getNormalWallTexture(Par.normalWallTexturesType.LLCorner), maxX, maxY)
            g.DrawImage(Form1.Textures.getNormalWallTexture(Par.normalWallTexturesType.LRCorner), 0, maxY)

            texture = tempBitmap.Clone()
        End If
    End Sub
    Public Function getTexture() As Bitmap
        texture.SetResolution(Par.RESOLUTION, Par.RESOLUTION)
        Return texture
    End Function
    Public Function isInExplorer() As Boolean
        Return _wallState = Par.wallState.Explorer
    End Function


End Class
