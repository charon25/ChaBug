Public MustInherit Class ExplorerElementType

    Private _location As Point
    Public Property Location As Point
        Get
            Return _location
        End Get
        Set(value As Point)
            _location = value
        End Set
    End Property
    Private _name As String
    Public Property Name As String
        Get
            Return _name
        End Get
        Set(value As String)
            _name = value
        End Set
    End Property

    Public MustOverride Function getTexture() As Bitmap
    Public MustOverride Function getAssociatedWall() As WallType

    Public Function getCorner() As Point
        Return New Point(Location.X + Par.EXPLORER_CORNER_X + Par.EXPLORER_INT_OFFSET_X, Location.Y + Par.EXPLORER_CORNER_Y + Par.EXPLORER_INT_OFFSET_Y)
    End Function

    Public Shared Function generateBackground(dir As DirectoryType) As Bitmap
        Dim img As New Bitmap(Par.EXPLORER_INT_WIDTH, Par.EXPLORER_INT_HEIGHT)
        img.SetResolution(Par.RESOLUTION, Par.RESOLUTION)
        Dim customFont As Font = New Font("Calibri", 30, FontStyle.Regular) 'Form1.PFC.Families(0), 30)
        Dim g As Graphics = Graphics.FromImage(img)
        For Each element As ExplorerElementType In dir.Childs
            g.DrawImage(element.getTexture(), element.Location)
            Dim S As SizeF = g.MeasureString(element.Name, customFont)
            g.DrawString(element.Name, customFont, Brushes.Black, New Point(element.Location.X + (Par.EXPLORER_ELEMENT_WIDTH - S.Width) / 2, element.Location.Y + Par.EXPLORER_ELEMENT_HEIGHT + 10))
        Next
        Return img
    End Function
    Public Shared Function getCoordinates(index As Integer)
        Dim x As Integer = index Mod Par.EXPLORER_ELEMENTS_PER_LINE
        Dim y As Integer = index \ Par.EXPLORER_ELEMENTS_PER_LINE
        Return New Point(Par.EXPLORER_ELEMENT_OFFSET_X + x * (Par.EXPLORER_ELEMENT_WIDTH + Par.EXPLORER_ELEMENTS_SPACE_X), Par.EXPLORER_ELEMENT_OFFSET_Y + y * (Par.EXPLORER_ELEMENT_HEIGHT + Par.EXPLORER_ELEMENTS_SPACE_Y))
    End Function

End Class
