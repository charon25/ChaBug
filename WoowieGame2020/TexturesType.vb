Public Class TexturesType

    Private normalWallTextures() As Bitmap
    Private enemyTextures() As Bitmap

    Sub New()
        ReDim normalWallTextures(Par.NORMAL_WALL_TEXTURES_COUNT - 1)
        ReDim enemyTextures(Par.ENEMY_STATES - 1)
        initTextures()
    End Sub

    Private Sub initTextures()
        Dim img As Bitmap = My.Resources.wallTiles
        img.SetResolution(Par.RESOLUTION, Par.RESOLUTION)
        Dim tempBitmap As Bitmap
        Dim g As Graphics
            For j As Integer = 0 To Par.NORMAL_WALL_TEXTURES_COUNT - 1
                tempBitmap = New Bitmap(Par.WALL_TILE_SIZE, Par.WALL_TILE_SIZE)
                tempBitmap.SetResolution(Par.RESOLUTION, Par.RESOLUTION)
                g = Graphics.FromImage(tempBitmap)
            g.DrawImage(img, New Rectangle(0, 0, Par.WALL_TILE_SIZE, Par.WALL_TILE_SIZE), New Rectangle(j * Par.WALL_TILE_SIZE, 0, Par.WALL_TILE_SIZE, Par.WALL_TILE_SIZE), GraphicsUnit.Pixel)
            normalWallTextures(j) = tempBitmap.Clone()
            normalWallTextures(j).SetResolution(Par.RESOLUTION, Par.RESOLUTION)
            Next

        img = My.Resources.enemySheet
        img.SetResolution(Par.RESOLUTION, Par.RESOLUTION)
        For i As Integer = 0 To Par.ENEMY_STATES - 1
            tempBitmap = New Bitmap(Par.ENEMY_WIDTH, Par.ENEMY_HEIGHT)
            g = Graphics.FromImage(tempBitmap)
            g.DrawImage(img, New Rectangle(0, 0, Par.ENEMY_WIDTH, Par.ENEMY_HEIGHT), New Rectangle(i * Par.ENEMY_WIDTH, 0, Par.ENEMY_WIDTH, Par.ENEMY_HEIGHT), GraphicsUnit.Pixel)
            enemyTextures(i) = tempBitmap.Clone()
            enemyTextures(i).SetResolution(Par.RESOLUTION, Par.RESOLUTION)
        Next
    End Sub

    Public Function getNormalWallTexture(normalWallTextureType As Par.normalWallTexturesType) As Bitmap
        Return normalWallTextures(normalWallTextureType)
    End Function
    Public Function getEnemyTexture(enemyState As Par.enemyState)
        Return enemyTextures(enemyState)
    End Function

End Class
