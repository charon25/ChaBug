<Serializable()>
Public Class VectorType

    Public X As Double
    Public Y As Double

    'Constructeurs
    Sub New(ByVal _X As Double, ByVal _Y As Double)
        X = _X
        Y = _Y
    End Sub
    Sub New(ByVal P As Point)
        X = P.X
        Y = P.Y
    End Sub
    Sub New(P As PointF)
        X = P.X
        Y = P.Y
    End Sub
    Sub New()

    End Sub

    'Opérations
    Public Shared Operator +(ByVal V1 As VectorType, ByVal V2 As VectorType) As VectorType
        Return New VectorType(V1.X + V2.X, V1.Y + V2.Y)
    End Operator
    Public Shared Operator -(ByVal V1 As VectorType, ByVal V2 As VectorType) As VectorType
        Return V1 + (-1) * V2
    End Operator
    Public Shared Operator *(ByVal V As VectorType, ByVal k As Double) As VectorType
        Return New VectorType(k * V.X, k * V.Y)
    End Operator
    Public Shared Operator *(ByVal V As VectorType, ByVal k As Integer) As VectorType
        Return V * CDbl(k)
    End Operator
    Public Shared Operator *(ByVal k As Double, ByVal V As VectorType) As VectorType
        Return V * k
    End Operator
    Public Shared Operator *(ByVal k As Integer, ByVal V As VectorType) As VectorType
        Return V * CDbl(k)
    End Operator
    Public Shared Operator /(ByVal V As VectorType, ByVal k As Double) As VectorType
        Return New VectorType(V.X / k, V.Y / k)
    End Operator
    Public Shared Operator /(ByVal V As VectorType, ByVal k As Integer) As VectorType
        Return V / CDbl(k)
    End Operator
    Public Shared Operator =(ByVal V1 As VectorType, ByVal V2 As VectorType) As Boolean
        Return V1.X = V2.X And V1.Y = V2.Y
    End Operator
    Public Shared Operator <>(ByVal V1 As VectorType, ByVal v2 As VectorType) As Boolean
        Return Not V1 = v2
    End Operator

    'Méthodes d'instance
    Public Overrides Function toString() As String
        Return "(" + CStr(X) + " ; " + CStr(Y) + ")"
    End Function
    Public Function norm() As Double
        Return Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2))
    End Function
    Public Function toUnitVector() As VectorType
        If X = 0 AndAlso Y = 0 Then
            Return New VectorType(0, 0)
        Else
            Return Me / norm()
        End If
    End Function
    Public Function toPoint() As Point
        Return New Point(X, Y)
    End Function
    Public Sub nullify(d As Double)
        If Math.Abs(X) < d Then
            X = 0
        End If
        If Math.Abs(Y) < d Then
            Y = 0
        End If
    End Sub

    'Méthode de classe
    Public Shared Function nullVector() As VectorType
        Return New VectorType(0, 0)
    End Function


End Class

<Serializable()>
Public Class BoundaryType

    Private _location As VectorType
    Public Property Location As VectorType
        Get
            Return _location
        End Get
        Set(value As VectorType)
            _location = value
        End Set
    End Property
    Private _width As Integer
    Public Property Width As Integer
        Get
            Return _width
        End Get
        Set(value As Integer)
            _width = value
        End Set
    End Property
    Private _height As Integer
    Public Property Height As Integer
        Get
            Return _height
        End Get
        Set(value As Integer)
            _height = value
        End Set
    End Property

    Sub New()

    End Sub
    Sub New(_location As VectorType, _width As Integer, _height As Integer)
        Me._location = _location
        Me._width = _width
        Me._height = _height
    End Sub

    Shared Operator =(B1 As BoundaryType, B2 As BoundaryType)
        Return (B1.Location = B2.Location AndAlso B1.Width = B2.Width)
    End Operator
    Shared Operator <>(B1 As BoundaryType, B2 As BoundaryType)
        Return Not (B1 = B2)
    End Operator

End Class