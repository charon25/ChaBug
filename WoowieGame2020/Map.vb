Imports System.Web.Script.Serialization

<Serializable()>
Public Class MapType

    Private _platformsList As List(Of PlatformType)
    Public Property PlatformsList As List(Of PlatformType)
        Get
            Return _platformsList
        End Get
        Set(value As List(Of PlatformType))
            _platformsList = value
        End Set
    End Property
    Private _wallsList As List(Of WallType)
    Public Property WallsList As List(Of WallType)
        Get
            Return _wallsList
        End Get
        Set(value As List(Of WallType))
            _wallsList = value
        End Set
    End Property
    Private _c As CharacterType
    Public Property Character As CharacterType
        Get
            Return _c
        End Get
        Set(value As CharacterType)
            _c = value
        End Set
    End Property
    Private _enemiesList As List(Of EnemyType)
    Public Property enemiesList As List(Of EnemyType)
        Get
            Return _enemiesList
        End Get
        Set(value As List(Of EnemyType))
            _enemiesList = value
        End Set
    End Property
    Private _groundHeight As Integer
    Public Property GroundHeight As Integer
        Get
            Return _groundHeight
        End Get
        Set(value As Integer)
            _groundHeight = value
        End Set
    End Property
    Public bulletsList As List(Of BulletType)
    Private _textureObjectsList As List(Of TextureObjectType)
    Public Property TextureObjectsList As List(Of TextureObjectType)
        Get
            Return _textureObjectsList
        End Get
        Set(value As List(Of TextureObjectType))
            _textureObjectsList = value
        End Set
    End Property
    <NonSerialized()> Public HUD As HUDType

    Sub New(a As Integer)
        bulletsList = New List(Of BulletType)
        _platformsList = New List(Of PlatformType)
        _enemiesList = New List(Of EnemyType)
        _wallsList = New List(Of WallType)
        _textureObjectsList = New List(Of TextureObjectType)
    End Sub
    Sub New()
        bulletsList = New List(Of BulletType)
        HUD = New HUDType()
    End Sub

    Public Shared Function createMapFromText(text As String, Location As VectorType, first As Boolean, Optional ByVal HUD As HUDType = Nothing) As MapType
        Dim Serialiseur As New JavaScriptSerializer
        Serialiseur.MaxJsonLength = Integer.MaxValue
        Dim map As MapType = Serialiseur.Deserialize(Of MapType)(text)
        If Not first Then
            map.Character.Location.Y = Location.Y
        End If
        If IsNothing(map.PlatformsList) Then
            map.PlatformsList = New List(Of PlatformType)
        End If
        For Each wall As WallType In map.WallsList
            wall.Enabled = True
            map.PlatformsList.Add(wall.getAssociatedPlatform())
            wall.generateTexture()
        Next
        For Each textureObject As TextureObjectType In map.TextureObjectsList
            textureObject.generateTexture()
        Next
        If first OrElse IsNothing(HUD) Then
            map.HUD = New HUDType()
        Else
            map.HUD = HUD
        End If
        map.Character.IsVisible = True
        Return map
    End Function

End Class