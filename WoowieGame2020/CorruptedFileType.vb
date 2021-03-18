Public Class CorruptedFileType

    Public File As FileType
    Private health As Integer
    Private initialHealth As Integer
    Public IsDead As Boolean
    Public Cooldown As Integer
    Public BugID As Integer

    Sub New(_file As FileType, _health As Integer, _bugID As Integer)
        File = _file
        health = _health
        initialHealth = _health
        IsDead = False
        Cooldown = Form1.rand.Next(Par.CF_COOLDOWN)
        BugID = _bugID
    End Sub

    Public Function generateHealthBarTexture() As Bitmap
        Dim tempImg As Bitmap = My.Resources.HealthBarFrameCorruptedFile
        tempImg.SetResolution(Par.RESOLUTION, Par.RESOLUTION)
        Dim g As Graphics = Graphics.FromImage(tempImg)
        g.FillRectangle(Brushes.Red, New Rectangle(Par.CF_HEALTH_OFFSET_X, Par.CF_HEALTH_OFFSET_Y, Math.Round(Par.CF_HEALTH_WIDTH * Health / initialHealth), Par.CF_HEALTH_HEIGHT))
        Return tempImg.Clone()
    End Function

    Public Sub age()
        Cooldown -= 1
        If Cooldown <= 0 Then
            Cooldown = Par.CF_COOLDOWN + Form1.rand.Next(-5, 5)
            Form1.map.bulletsList.Add(New BulletType(New VectorType(File.getCorner()) + New VectorType(Par.EXPLORER_ELEMENT_WIDTH / 2, Par.EXPLORER_ELEMENT_HEIGHT / 2), Form1.C.Location + New VectorType(0, -Form1.C.Height / 2), Par.bulletOrigin.CorruptedFile))
            Form1.playSound(SoundsManager.CF_BULLET(Form1.rand))
        End If
    End Sub

    Public Shared Sub addCorruptedFileToDir(ByRef dir As DirectoryType, ByRef CF As CorruptedFileType)
        dir.CorruptedFilesList.Add(CF)
    End Sub


    Public Sub loseHealth()
        health -= 1
        If health = 0 Then
            Form1.playSound(SoundsManager.CF_DEATH(Form1.rand))
            IsDead = True
        Else
            Form1.playSound(SoundsManager.CF_HURT(Form1.rand))
        End If
    End Sub

End Class
