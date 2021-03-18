Public Class HUDType

    Private _backgroundImage As Bitmap
    Public Property backgroundImage As Bitmap
        Get
            Return _backgroundImage
        End Get
        Set(value As Bitmap)
            _backgroundImage = value
        End Set
    End Property
    Public IsHealthBugged As Boolean
    Public IsManaBugged As Boolean

    Sub New()
        _backgroundImage = New Bitmap(Par.HUD_WIDTH, Par.HUD_HEIGHT)
        IsHealthBugged = False
        IsManaBugged = False
    End Sub

    Public Sub update()
        _backgroundImage = New Bitmap(Par.HUD_WIDTH, Par.HUD_HEIGHT)
        _backgroundImage.SetResolution(Par.RESOLUTION, Par.RESOLUTION)
        Dim g As Graphics = Graphics.FromImage(_backgroundImage)
        'Coeurs
        If Not IsHealthBugged Then
            For i As Integer = 0 To Form1.param.Health - 1
                g.DrawImage(My.Resources.health, New Point(Par.HEALTH_OFFSET_X + i * (Par.HEALTH_SIZE + Par.HEALTH_OFFSET_X), Par.HEALTH_OFFSET_Y))
            Next
        End If
        'Cooldown
        If Not IsManaBugged Then
            If Form1.C.Cooldown = 0 Then
                g.DrawImage(My.Resources.cooldownFrameFull, Par.MANA_OFFSET_X, Par.MANA_OFFSET_Y)
            Else
                g.DrawImage(My.Resources.cooldownFrame, Par.MANA_OFFSET_X, Par.MANA_OFFSET_Y)
            End If
            Dim cooldownWidth As Integer
            If Form1.C.InitialCooldown = 0 Then
                cooldownWidth = Par.MANA_BAR_WIDTH
            Else
                cooldownWidth = Math.Round(Par.MANA_BAR_WIDTH * (1 - Form1.C.Cooldown / Form1.C.InitialCooldown))
            End If
            g.DrawImage(My.Resources.cooldownBar, New Rectangle(Par.MANA_OFFSET_X + Par.MANA_BAR_OFFSET, Par.MANA_OFFSET_Y + Par.MANA_BAR_OFFSET, cooldownWidth, Par.MANA_BAR_HEIGHT), New Rectangle(0, 0, cooldownWidth, Par.MANA_BAR_HEIGHT), GraphicsUnit.Pixel)
        End If
    End Sub

End Class
