<Serializable()>
Public Class TextureObjectType

    Private _location As Point
    Public Property Location As Point
        Get
            Return _location
        End Get
        Set(value As Point)
            _location = value
        End Set
    End Property
    Private _size As Size
    Public Property Size As Size
        Get
            Return _size
        End Get
        Set(value As Size)
            _size = value
        End Set
    End Property
    Private _backgroundImageName As String
    Public Property BackgroundImageName As String
        Get
            Return _backgroundImageName
        End Get
        Set(value As String)
            _backgroundImageName = value
        End Set
    End Property
    Public BackgroundImage As Bitmap

    Sub New(_location As Point, _size As Size, _name As String)
        Me._location = _location
        Me._size = _size
        Me._backgroundImageName = _name
    End Sub
    Sub New()

    End Sub
    Public Sub generateTexture()
        Dim ResourceSet As Resources.ResourceSet = My.Resources.ResourceManager.GetResourceSet(Globalization.CultureInfo.CurrentCulture, True, True)
        For Each resource As DictionaryEntry In ResourceSet.OfType(Of Object)()
            If resource.Key.ToString = "textureObjectType_" & _backgroundImageName Then
                BackgroundImage = resource.Value
                Exit Sub
            End If
        Next
    End Sub

    Public Shared Operator =(T1 As TextureObjectType, T2 As TextureObjectType)
        Return (T1.BackgroundImageName = T2.BackgroundImageName)
    End Operator
    Public Shared Operator <>(T1 As TextureObjectType, T2 As TextureObjectType)
        Return (T1.BackgroundImageName <> T2.BackgroundImageName)
    End Operator

End Class
