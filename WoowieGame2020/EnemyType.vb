<Serializable()>
Public Class EnemyType

    Private _location As VectorType
    Public Property Location As VectorType
        Get
            Return _location
        End Get
        Set(value As VectorType)
            _location = value
        End Set
    End Property
    Private _cooldown As Integer
    Public Property Cooldown As Integer
        Get
            Return _cooldown
        End Get
        Set(value As Integer)
            _cooldown = value
        End Set
    End Property
    Private _initialCooldown As Integer
    Public Property initialCooldown As Integer
        Get
            Return _initialCooldown
        End Get
        Set(value As Integer)
            _initialCooldown = value
        End Set
    End Property
    Private shootRecovery As Integer
    Private state As Par.enemyState
    Private isLookingLeft As Boolean
    Private _shouldBeInExplorer As Boolean
    Public Property ShouldBeInExplorer As Boolean
        Get
            Return _shouldBeInExplorer
        End Get
        Set(value As Boolean)
            _shouldBeInExplorer = value
        End Set
    End Property
    Public IsInExplorer As Boolean

    Sub New(_location As VectorType, _initialCooldown As Integer, _shouldBeInExplorer As Boolean)
        Me._location = _location
        Me._initialCooldown = _initialCooldown
        Me._shouldBeInExplorer = _shouldBeInExplorer
    End Sub
    Sub New()
        shootRecovery = 0
    End Sub

    Public Sub age()
        If Not (IsInExplorer Xor _shouldBeInExplorer) Then
            isLookingLeft = (Form1.map.Character.Location.X <= _location.X)
            If _cooldown > 0 Then
                _cooldown -= 1
                If _cooldown <= 5 Then
                    shootRecovery = Par.ENEMY_RECOVERY + 1
                End If
            Else
                shoot()
                _cooldown = _initialCooldown
            End If
            If isLookingLeft Then
                state = Par.enemyState.IdleLeft
            Else
                state = Par.enemyState.IdleRight
            End If
            If shootRecovery > 0 Then
                shootRecovery -= 1
                If isLookingLeft Then
                    state = Par.enemyState.ShootingLeft
                Else
                    state = Par.enemyState.ShootingRight
                End If
            End If
        End If
        IsInExplorer = Form1.isExplorerVisible AndAlso (_location.X >= Par.EXPLORER_CORNER_X AndAlso _location.X <= Par.EXPLORER_CORNER_X + Par.EXPLORER_WIDTH AndAlso _location.Y >= Par.EXPLORER_CORNER_Y AndAlso _location.Y <= Par.EXPLORER_CORNER_Y + Par.EXPLORER_HEIGHT)
    End Sub

    Private Sub shoot()
        Form1.playSound(SoundsManager.FIREBALL(Form1.rand))
        Dim start As VectorType = _location
        If isLookingLeft Then
            start += New VectorType(-7, 4)
        Else
            start += New VectorType(7, 4)
        End If
        Form1.map.bulletsList.Add(New BulletType(start, Form1.map.Character.getMiddlePoint(), Par.bulletOrigin.Enemy))
    End Sub

    Public Sub kill()
        Form1.playSound(SoundsManager.ENEMY_DEATH(Form1.rand))
        Form1.enemiesToRemove.Add(Me)
    End Sub
    Public Function getTexture() As Bitmap
        Return Form1.Textures.getEnemyTexture(state)
    End Function

    Public Function getCorner() As Point
        Return New Point(_location.X - Par.ENEMY_WIDTH / 2, _location.Y - Par.ENEMY_HEIGHT / 2)
    End Function
End Class