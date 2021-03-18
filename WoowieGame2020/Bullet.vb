Public Class BulletType

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
    Private _origin As Par.bulletOrigin
    Public Property Origin As Par.bulletOrigin
        Get
            Return _origin
        End Get
        Set(value As Par.bulletOrigin)
            _origin = value
        End Set
    End Property
    Public isDead As Boolean
    Public IsInExplorer As Boolean
    Public CreatedInExplorer As Boolean
    Private hitPlatform As Boolean
    Private intersectionPoint As PointF
    Private intersectionDirection As Integer
    Private isFrozen As Boolean

    Sub New(_location As VectorType, _destination As VectorType, _origin As Par.bulletOrigin)
        Me._location = _location
        _velocity = (_destination - _location).toUnitVector() * Par.BULLET_VELOCITY
        Me._origin = _origin
        isDead = False
        IsInExplorer = Form1.isExplorerVisible AndAlso (_location.X >= Par.EXPLORER_CORNER_X AndAlso _location.X <= Par.EXPLORER_CORNER_X + Par.EXPLORER_WIDTH AndAlso _location.Y >= Par.EXPLORER_CORNER_Y AndAlso _location.Y <= Par.EXPLORER_CORNER_Y + Par.EXPLORER_HEIGHT)
        CreatedInExplorer = IsInExplorer
        computeIntersectionPoint()
    End Sub

    Private Sub computeIntersectionPoint()
        If Origin <> Par.bulletOrigin.CorruptedFile Then
            Dim t, u, denom, x1, x2, x3, x4, y1, y2, y3, y4, dist As Double
            Dim minDistance As Integer = Integer.MaxValue
            Dim intersect As PointF
            hitPlatform = False
            For Each platform As PlatformType In Form1.map.PlatformsList
                If Not (IsInExplorer Xor platform.IsInExplorer) Then
                    x1 = _location.X
                    x2 = _location.X + _velocity.X
                    x3 = platform.Boundaries.Location.X
                    x4 = platform.Boundaries.Location.X + platform.Boundaries.Width
                    y1 = _location.Y
                    y2 = _location.Y + _velocity.Y
                    y3 = platform.Boundaries.Location.Y
                    y4 = y3
                    denom = -(y1 - y2) * (x3 - x4)
                    If denom <> 0 Then
                        t = -(y1 - y3) * (x3 - x4) / denom
                        u = -((x1 - x2) * (y1 - y3) - (y1 - y2) * (x1 - x3)) / denom
                        If t > 0 AndAlso u > 0 AndAlso u < 1 Then
                            intersect = New PointF(x1 + t * (x2 - x1), y1 + t * (y2 - y1))
                            dist = (intersect.X - _location.X) * (intersect.X - _location.X) + (intersect.Y - _location.Y) * (intersect.Y - _location.Y)
                            If dist < minDistance Then
                                minDistance = dist
                                hitPlatform = True
                                intersectionPoint = intersect
                            End If
                        End If
                    End If
                End If
            Next
            For Each wall As WallType In Form1.map.WallsList
                If Not (IsInExplorer Xor wall.isInExplorer()) Then
                    x1 = _location.X
                    x2 = _location.X + _velocity.X
                    x3 = wall.Boundaries.Location.X
                    x4 = x3
                    y1 = _location.Y
                    y2 = _location.Y + _velocity.Y
                    y3 = wall.Boundaries.Location.Y
                    y4 = wall.Boundaries.Location.Y + wall.Boundaries.Height
                    denom = (x1 - x2) * (y3 - y4)
                    If denom <> 0 Then
                        t = (x1 - x3) * (y3 - y4) / denom
                        u = -((x1 - x2) * (y1 - y3) - (y1 - y2) * (x1 - x3)) / denom
                        If t > 0 AndAlso u > 0 AndAlso u < 1 Then
                            intersect = New PointF(x1 + t * (x2 - x1), y1 + t * (y2 - y1))
                            dist = (intersect.X - _location.X) * (intersect.X - _location.X) + (intersect.Y - _location.Y) * (intersect.Y - _location.Y)
                            If dist < minDistance Then
                                minDistance = dist
                                hitPlatform = True
                                intersectionPoint = intersect
                            End If
                        End If
                    End If
                    x3 = wall.Boundaries.Location.X + wall.Boundaries.Width
                    x4 = x3
                    denom = (x1 - x2) * (y3 - y4)
                    If denom <> 0 Then
                        t = (x1 - x3) * (y3 - y4) / denom
                        u = -((x1 - x2) * (y1 - y3) - (y1 - y2) * (x1 - x3)) / denom
                        If t > 0 AndAlso u > 0 AndAlso u < 1 Then
                            intersect = New PointF(x1 + t * (x2 - x1), y1 + t * (y2 - y1))
                            dist = (intersect.X - _location.X) * (intersect.X - _location.X) + (intersect.Y - _location.Y) * (intersect.Y - _location.Y)
                            If dist < minDistance Then
                                minDistance = dist
                                hitPlatform = True
                                intersectionPoint = intersect
                            End If
                        End If
                    End If
                End If
            Next
            If hitPlatform Then
                intersectionDirection = Math.Sign(intersectionPoint.X - _location.X)
            End If
        End If
    End Sub

    Sub move()
        If Not isFrozen Then
            _location += _velocity
            If (_location.X < -Par.BULLET_WIDTH / 2 OrElse _location.X > Par.FORM_WIDTH + Par.BULLET_WIDTH / 2 OrElse _location.Y < -Par.BULLET_HEIGHT / 2 OrElse _location.Y > Par.FORM_HEIGHT + Par.BULLET_HEIGHT / 2) Then
                isDead = True
            End If
            If intersectionDirection * (intersectionPoint.X - _location.X) < 0 Then
                isDead = True
            End If
            If Not isDead Then
                If _origin = Par.bulletOrigin.Player Then
                    For Each enemy As EnemyType In Form1.map.enemiesList
                        If Not (CreatedInExplorer Xor enemy.ShouldBeInExplorer) Then
                            If Par.collides(Me.getCorner(), Par.BULLET_WIDTH, Par.BULLET_HEIGHT, enemy.getCorner(), Par.ENEMY_WIDTH, Par.ENEMY_HEIGHT) Then
                                enemy.kill()
                                isDead = True
                                Exit For
                            End If
                        End If
                    Next
                ElseIf _origin = Par.bulletOrigin.Enemy OrElse _origin = Par.bulletOrigin.CorruptedFile Then
                    If Not (Form1.C.IsInExplorer Xor CreatedInExplorer) Then
                        If Par.collides(Me.getCorner(), Par.BULLET_WIDTH, Par.BULLET_HEIGHT, Form1.map.Character.getCorner(), Form1.map.Character.Width, Form1.map.Character.Height) Then
                            Form1.loseHealth()
                            isDead = True
                        End If
                    End If
                End If
            End If
            If CreatedInExplorer Then
                If _origin = Par.bulletOrigin.Player Then
                    For Each corruptedFile As CorruptedFileType In Form1.C.ContainingDirectory.CorruptedFilesList
                        Dim CF_Location As Point = corruptedFile.File.getCorner()
                        If _location.X + Par.BULLET_WIDTH / 2 >= CF_Location.X AndAlso _location.X - Par.BULLET_WIDTH / 2 <= CF_Location.X + Par.EXPLORER_ELEMENT_WIDTH AndAlso _location.Y + Par.BULLET_HEIGHT / 2 >= CF_Location.Y AndAlso _location.Y - Par.BULLET_HEIGHT / 2 <= CF_Location.Y + Par.EXPLORER_ELEMENT_HEIGHT Then
                            corruptedFile.loseHealth()
                            isDead = True
                        End If
                    Next
                End If
                If isOutOfExplorer() Then
                    isDead = True
                End If
            End If
        End If
        IsInExplorer = Form1.isExplorerVisible AndAlso (_location.X >= Par.EXPLORER_CORNER_X AndAlso _location.X <= Par.EXPLORER_CORNER_X + Par.EXPLORER_WIDTH AndAlso _location.Y >= Par.EXPLORER_CORNER_Y AndAlso _location.Y <= Par.EXPLORER_CORNER_Y + Par.EXPLORER_HEIGHT)
    End Sub

    Public Sub Freeze()
        isFrozen = True
    End Sub
    Public Sub Unfreeze()
        isFrozen = False
    End Sub
    Public Function isOutOfExplorer() As Boolean
        Return Not (_location.X >= Par.EXPLORER_CORNER_X AndAlso _location.X <= Par.EXPLORER_CORNER_X + Par.EXPLORER_WIDTH AndAlso _location.Y >= Par.EXPLORER_CORNER_Y AndAlso _location.Y <= Par.EXPLORER_CORNER_Y + Par.EXPLORER_HEIGHT)
    End Function
    Public Function getCorner() As Point
        Return New Point(_location.X - Par.BULLET_WIDTH / 2, _location.Y - Par.BULLET_HEIGHT / 2)
    End Function

    Public Shared Operator =(B1 As BulletType, B2 As BulletType)
        Return (B1.Location = B2.Location AndAlso B1.Velocity = B2.Velocity)
    End Operator
    Public Shared Operator <>(B1 As BulletType, B2 As BulletType)
        Return Not (B1 = B2)
    End Operator

End Class
