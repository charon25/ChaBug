Public Class CharacterAnimatorType

    Public State As Par.characterAnimatorState
    Private textures(,) As Bitmap
    Private buggedTextures(,) As Bitmap
    Private age As Integer
    Private index As Integer

    Sub New()
        ReDim textures(Par.CHARACTER_STATES - 1, Par.CHARACTER_ANIMATION_FRAMES - 1)
        ReDim buggedTextures(Par.CHARACTER_STATES - 1, Par.CHARACTER_ANIMATION_FRAMES - 1)
        initTextures()
    End Sub

    Private Sub initTextures()
        Dim img As Bitmap = My.Resources.characterAnimationSheet
        img.SetResolution(Par.RESOLUTION, Par.RESOLUTION)
        Dim tempBitmap As Bitmap
        Dim g As Graphics
        For i As Integer = 0 To Par.CHARACTER_ANIMATION_FRAMES - 1
            For j As Integer = 0 To Par.CHARACTER_STATES - 1
                tempBitmap = New Bitmap(Par.PLAYER_WIDTH, Par.PLAYER_HEIGHT)
                tempBitmap.SetResolution(Par.RESOLUTION, Par.RESOLUTION)
                g = Graphics.FromImage(tempBitmap)
                g.DrawImage(img, New Rectangle(0, 0, Par.PLAYER_WIDTH, Par.PLAYER_HEIGHT), New Rectangle(i * Par.PLAYER_WIDTH, j * Par.PLAYER_HEIGHT, Par.PLAYER_WIDTH, Par.PLAYER_HEIGHT), GraphicsUnit.Pixel)
                textures(j, i) = tempBitmap.Clone()
                textures(j, i).SetResolution(Par.RESOLUTION, Par.RESOLUTION)

                buggedTextures(j, i) = My.Resources.characterAnimationSheetBugged
                buggedTextures(j, i).SetResolution(Par.RESOLUTION, Par.RESOLUTION)
            Next
        Next
    End Sub

    Public Sub Animate()
        If age <= 0 Then
            index = (index + 1) Mod Par.CHARACTER_ANIMATION_FRAMES
            age = Par.CHARACTER_ANIMATION_SPEED(index)
        Else
            age -= 1
        End If
    End Sub

    Public Function getTexture(isBugged As Boolean) As Bitmap
        If isBugged Then
            Return buggedTextures(State, index)
        Else
            Return textures(State, index)
        End If
    End Function

End Class
Public Class EnemyAnimatorType

End Class