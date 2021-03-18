Public Class PlatformType

    Private _boundaries As BoundaryType
    Public Property Boundaries As BoundaryType
        Get
            Return _boundaries
        End Get
        Set(value As BoundaryType)
            _boundaries = value
        End Set
    End Property
    Private _isDescendable As Boolean
    Public ReadOnly Property IsDescendable As Boolean
        Get
            Return _isDescendable
        End Get
    End Property
    Private _isInExplorer As Boolean
    Public Property IsInExplorer As Boolean
        Get
            Return _isInExplorer
        End Get
        Set(value As Boolean)
            _isInExplorer = value
        End Set
    End Property
    Public ExplorerElement As ExplorerElementType
    Public Name As String
    Public IsEssential As Boolean
    Public Enabled As Boolean

    Sub New(_X As Integer, _Y As Integer, _width As Integer, _isDescendable As Boolean, _isInExplorer As Boolean, _name As String, _isEssential As Boolean, Optional ByRef _explorerElement As ExplorerElementType = Nothing)
        Me._boundaries = New BoundaryType(New VectorType(_X, _Y), _width, 1)
        Me._isDescendable = _isDescendable
        Me._isInExplorer = _isInExplorer
        ExplorerElement = _explorerElement
        IsEssential = _isEssential
        Name = _name
        Enabled = True
    End Sub
    Sub New()
        Enabled = True
    End Sub

    Shared Operator =(P1 As PlatformType, P2 As PlatformType)
        Return (P1.Boundaries = P2.Boundaries)
    End Operator
    Shared Operator <>(P1 As PlatformType, P2 As PlatformType)
        Return (P1.Boundaries <> P2.Boundaries)
    End Operator

End Class
