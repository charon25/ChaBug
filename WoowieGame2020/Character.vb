<Serializable()>
Public Class CharacterType

    Private _location As VectorType
    Public Property Location As VectorType
        Get
            Return _location
        End Get
        Set(value As VectorType)
            _location = value
        End Set
    End Property
    Private _velocity As VectorType
    Public Property Velocity As VectorType
        Get
            Return _velocity
        End Get
        Set(value As VectorType)
            _velocity = value
        End Set
    End Property
    <NonSerialized()> Private _acceleration As VectorType
    Public Property Acceleration As VectorType
        Get
            Return _acceleration
        End Get
        Set(value As VectorType)
            _acceleration = value
        End Set
    End Property
    Private _gravityAcceleration As VectorType
    Public Property gravityAcceleration As VectorType
        Get
            Return _gravityAcceleration
        End Get
        Set(value As VectorType)
            _gravityAcceleration = value
        End Set
    End Property
    Private _inherentAcceleration As VectorType
    Public Property inherentAcceleration As VectorType
        Get
            Return _inherentAcceleration
        End Get
        Set(value As VectorType)
            _inherentAcceleration = value
        End Set
    End Property
    Private _isAirborne As Boolean
    Public Property IsAirborne As Boolean
        Get
            Return _isAirborne
        End Get
        Set(value As Boolean)
            _isAirborne = value
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
    Private Property isJumping As Boolean
    Private _steppingPlatform As PlatformType
    Public Property steppingPlatform As PlatformType
        Get
            Return _steppingPlatform
        End Get
        Set(value As PlatformType)
            _steppingPlatform = value
        End Set
    End Property
    Private _touchingWall As WallType
    Public Property touchingWall As WallType
        Get
            Return _touchingWall
        End Get
        Set(value As WallType)
            _touchingWall = value
        End Set
    End Property
    Private _isLookingLeft As Boolean
    Public Property isLookingLeft As Boolean
        Get
            Return _isLookingLeft
        End Get
        Set(value As Boolean)
            _isLookingLeft = value
        End Set
    End Property
    Private directionOfTouchingWall As Integer
    Private settingNewMap As Boolean
    Public Cooldown As Integer
    Private shootRecovery As Integer
    Private Animator As CharacterAnimatorType
    Public InitialCooldown As Integer
    Public Input As Boolean
    Public IsInExplorer As Boolean
    Public ContainingDirectory As DirectoryType
    Public steppingElement As ExplorerElementType
    Public IsBugged As Boolean
    Public InvincibilityCooldown As Integer
    Public BlinkCooldown As Integer
    Public IsVisible As Boolean
    Private lastStep As Long

    Sub New(position As PointF, _width As Integer, _height As Integer)
        _location = New VectorType(position)
        _velocity = VectorType.nullVector()
        _acceleration = VectorType.nullVector()
        gravityAcceleration = New VectorType(0, Par.GRAVITY)
        inherentAcceleration = VectorType.nullVector()
        Me._width = _width
        Me._height = _height
        _isAirborne = False
        isJumping = False
        settingNewMap = False
        shootRecovery = 0
    End Sub
    Sub New()
        isJumping = False
        settingNewMap = False
        Cooldown = 0
        shootRecovery = 0
        Animator = New CharacterAnimatorType()
        Input = False
    End Sub

    Public Sub move()
        If _isAirborne Then
            _velocity.X *= 1.1
        End If
        _acceleration = gravityAcceleration + inherentAcceleration
        _velocity += _acceleration
        _isAirborne = Not isOnPlatform()
        If Not _isAirborne Then
            inherentAcceleration = New VectorType(0, -Par.GRAVITY)
            If _velocity.Y >= 0 Then _location.Y = steppingPlatform.Boundaries.Location.Y
            steppingElement = steppingPlatform.ExplorerElement
            If _velocity.X <> 0 Then
                If steppingPlatform.IsInExplorer Then
                    If (Date.Now.Ticks - lastStep) >= SoundsManager.WALK_EXPLORER_DURATION_MS * 10000 Then
                        lastStep = Date.Now.Ticks
                        Form1.playSound(SoundsManager.WALK_EXPLORER)
                    End If
                Else
                    If (Date.Now.Ticks - lastStep) >= SoundsManager.WALK_NORMAL_DURATION_MS * 10000 Then
                        lastStep = Date.Now.Ticks
                        Form1.playSound(SoundsManager.WALK_NORMAL)
                    End If
                End If
            End If
        Else
            inherentAcceleration = VectorType.nullVector()
        End If
        If isTouchingWall() Then
            _velocity.X = 0
            If directionOfTouchingWall = 1 Then
                _location.X = touchingWall.Boundaries.Location.X - _width / 2
                _isLookingLeft = False
            ElseIf directionOfTouchingWall = -1 Then
                _location.X = touchingWall.Boundaries.Location.X + touchingWall.Boundaries.Width + _width / 2
                _isLookingLeft = True
            End If
        Else
            directionOfTouchingWall = 0
        End If
        _location += _velocity
        IsInExplorer = Form1.isExplorerVisible AndAlso (_location.X >= Par.EXPLORER_CORNER_X AndAlso _location.X <= Par.EXPLORER_CORNER_X + Par.EXPLORER_WIDTH AndAlso _location.Y >= Par.EXPLORER_CORNER_Y AndAlso _location.Y <= Par.EXPLORER_CORNER_Y + Par.EXPLORER_HEIGHT)
        isJumping = False
        If _location.X < _width / 2 Then
            _location.X = _width / 2
        End If
        If _location.X > Par.FORM_WIDTH + _width / 2 AndAlso Not settingNewMap AndAlso Form1.gameState = Par.gameState.Level Then
            settingNewMap = True
            Form1.nextTableau()
        End If
        If Cooldown > 0 Then
            If Cooldown = InitialCooldown Then
                shootRecovery = Par.SHOOT_RECOVERY + 1
            End If
            Cooldown -= 1
        End If
        If _velocity.X <> 0 Then
            _isLookingLeft = (_velocity.X < 0)
        End If
        If IsAirborne Then
            If _velocity.Y > 0 Then
                If Not _isLookingLeft Then
                    Animator.State = Par.characterAnimatorState.AirBorneDownRight
                Else
                    Animator.State = Par.characterAnimatorState.AirBorneDownLeft
                End If
            Else
                If Not _isLookingLeft Then
                    Animator.State = Par.characterAnimatorState.AirBorneUpRight
                Else
                    Animator.State = Par.characterAnimatorState.AirBorneUpLeft
                End If
            End If
        Else
            If _velocity.X > 0 Then
                Animator.State = Par.characterAnimatorState.MovingRight
            ElseIf _velocity.X < 0 Then
                Animator.State = Par.characterAnimatorState.MovingLeft
            Else
                If _isLookingLeft Then
                    If Not Input OrElse directionOfTouchingWall = 0 Then
                        Animator.State = Par.characterAnimatorState.IdleLeft
                    Else
                        Animator.State = Par.characterAnimatorState.MovingLeft
                    End If
                Else
                    If Not Input OrElse directionOfTouchingWall = 0 Then
                        Animator.State = Par.characterAnimatorState.IdleRight
                    Else
                        Animator.State = Par.characterAnimatorState.MovingRight
                    End If
                End If
            End If
        End If
        If shootRecovery > 0 Then
            shootRecovery -= 1
            If _isLookingLeft Then
                Animator.State = Par.characterAnimatorState.ShootingLeft
            Else
                Animator.State = Par.characterAnimatorState.ShootingRight
            End If
        End If
        If InvincibilityCooldown > 0 Then
            InvincibilityCooldown -= 1
            If BlinkCooldown > 0 Then
                BlinkCooldown -= 1
            Else
                BlinkCooldown = Par.PLAYER_BLINK_COOLDOWN
                IsVisible = Not IsVisible
            End If
        Else
            IsVisible = True
        End If
        Animator.Animate()
        For Each enemy As EnemyType In Form1.map.enemiesList
            If Not (IsInExplorer Xor enemy.ShouldBeInExplorer) Then
                If _location.X + _width / 2 >= enemy.Location.X AndAlso _location.X - _width / 2 <= enemy.Location.X + Par.ENEMY_WIDTH AndAlso _location.Y >= enemy.Location.Y AndAlso _location.Y - _height <= enemy.Location.Y + Par.ENEMY_HEIGHT Then
                    Form1.loseHealth()
                End If
            End If
        Next
        If _location.Y - _height > Par.FORM_HEIGHT Then
            Form1.lose()
        End If
    End Sub

    Public Sub goRight()
        _velocity.X = Par.PLAYER_VELOCITY
    End Sub
    Public Sub stopRight()
        If _velocity.X > 0 Then
            _velocity.X = 0
        End If
    End Sub

    Public Sub goLeft()
        _velocity.X = -Par.PLAYER_VELOCITY
    End Sub
    Public Sub stopLeft()
        If _velocity.X < 0 Then
            _velocity.X = 0
        End If
    End Sub

    Public Sub jump()
        _isAirborne = True
        isJumping = True
        _velocity.Y = -Par.PLAYER_JUMP_VELOCITY
        Form1.playSound(SoundsManager.JUMP(Form1.rand))
    End Sub
    Public Sub stepDown()
        If steppingPlatform.IsDescendable Then
            _location.Y += 1
        End If
    End Sub

    Private Function isOnPlatform() As Boolean
        Dim a = Form1.map.PlatformsList
        For Each platform As PlatformType In Form1.map.PlatformsList
            If platform.Enabled Then
                If Not (IsInExplorer Xor platform.IsInExplorer) Then
                    If _location.X + _width / 2 > platform.Boundaries.Location.X AndAlso _location.X - _width / 2 < platform.Boundaries.Location.X + platform.Boundaries.Width Then
                        If _location.Y <= platform.Boundaries.Location.Y AndAlso _location.Y + _velocity.Y >= platform.Boundaries.Location.Y Then
                            If Not isJumping Then _velocity.Y = 0
                            steppingPlatform = platform
                            Return True
                        End If
                    End If
                End If
            End If
        Next
        Return False
    End Function
    Private Function isTouchingWall() As Boolean
        For Each wall As WallType In Form1.map.WallsList
            If wall.Enabled Then
                If Not (IsInExplorer Xor wall.isInExplorer()) Then
                    If _location.Y > wall.Boundaries.Location.Y AndAlso _location.Y - _height < wall.Boundaries.Location.Y + wall.Boundaries.Height Then
                        If _location.X + _width / 2 <= wall.Boundaries.Location.X AndAlso _location.X + _width / 2 + _velocity.X >= wall.Boundaries.Location.X Then
                            directionOfTouchingWall = 1
                            touchingWall = wall
                            Return True
                        End If
                        If _location.X - _width / 2 >= wall.Boundaries.Location.X + wall.Boundaries.Width AndAlso _location.X - _width / 2 + _velocity.X <= wall.Boundaries.Location.X + wall.Boundaries.Width Then
                            directionOfTouchingWall = -1
                            touchingWall = wall
                            Return True
                        End If
                    End If
                End If
            End If
        Next
        Return False
    End Function

    Public Function getRectangle() As Rectangle
        Return New Rectangle(_location.X - _width / 2, _location.Y - _height, _width, _height)
    End Function
    Public Function getMiddlePoint() As VectorType
        Return _location + New VectorType(0, -_height / 2)
    End Function
    Public Function getCorner() As PointF
        Return New PointF(_location.X - _width / 2, _location.Y - _height)
    End Function

    Public Function getTexture() As Bitmap
        If IsVisible Then
            Dim img As Bitmap = Animator.getTexture(IsBugged)
            img.SetResolution(Par.RESOLUTION, Par.RESOLUTION)
            Return img
        Else
            Return New Bitmap(Par.PLAYER_WIDTH, Par.PLAYER_HEIGHT)
        End If
    End Function


End Class