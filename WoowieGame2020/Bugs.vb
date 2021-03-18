Public Class Bugs

    Public CorruptedFilesCount() As Integer
    Private lIndex As List(Of Integer)

    Sub New()
        ReDim CorruptedFilesCount(Par.BUGS_COUNT - 1)
        lIndex = New List(Of Integer)
    End Sub

    Public Sub shuffledKeys()
        If CorruptedFilesCount(Par.SHUFFLE_BUG) = 0 Then
            CorruptedFileType.addCorruptedFileToDir(Form1.rootDir, New CorruptedFileType(Form1.configFile, 3, Par.SHUFFLE_BUG))
            CorruptedFilesCount(Par.SHUFFLE_BUG) = 1
            Dim index As Integer
            Dim lKeys As New List(Of Keys)
            lKeys.AddRange(Par.KEYS_ARRAY.Reverse())
            ReDim Par.KEYS_ARRAY(Par.KEY_NUMBER - 1)
            For i As Integer = 0 To Par.KEY_NUMBER - 1
                index = Form1.rand.Next(lKeys.Count - 1)
                Par.KEYS_ARRAY(i) = lKeys(index)
                lKeys.RemoveAt(index)
            Next
        End If
    End Sub
    Public Sub nonWorkingPlatforms()
        If CorruptedFilesCount(Par.NONWORKING_PLATFORMS_BUG) = 0 Then
            CorruptedFileType.addCorruptedFileToDir(Form1.dataDir, New CorruptedFileType(Form1.generalDataFile, 3, Par.NONWORKING_PLATFORMS_BUG))
            CorruptedFileType.addCorruptedFileToDir(Form1.terrainDir, New CorruptedFileType(Form1.platformDataDir, 3, Par.NONWORKING_PLATFORMS_BUG))
            CorruptedFilesCount(Par.NONWORKING_PLATFORMS_BUG) = 2
            For Each platform As PlatformType In Form1.map.PlatformsList
                If Not platform.IsEssential Then
                    platform.Enabled = False
                End If
            Next
            For Each wall As WallType In Form1.map.WallsList
                If Not wall.IsEssential Then
                    wall.Enabled = False
                End If
            Next
        End If
    End Sub
    Public Sub noLevel()
        If CorruptedFilesCount(Par.NOLEVEL_BUG) = 0 Then
            CorruptedFileType.addCorruptedFileToDir(Form1.levelsDir, New CorruptedFileType(Form1.levelLoaderFile, 2, Par.NOLEVEL_BUG))
            CorruptedFileType.addCorruptedFileToDir(Form1.levelsDir, New CorruptedFileType(Form1.world1File, 6, Par.NOLEVEL_BUG))
            CorruptedFilesCount(Par.NOLEVEL_BUG) = 2
            Form1.isThereLevel = False
            For Each platform As PlatformType In Form1.map.PlatformsList
                If Not platform.IsEssential Then
                    platform.Enabled = False
                End If
            Next
            For Each wall As WallType In Form1.map.WallsList
                If Not wall.IsEssential Then
                    wall.Enabled = False
                End If
            Next
        End If
    End Sub
    Public Sub playerNoTexture()
        If CorruptedFilesCount(Par.PLAYER_NO_TEXTURE_BUG) = 0 Then
            CorruptedFileType.addCorruptedFileToDir(Form1.playerTextureDir, New CorruptedFileType(Form1.playerSpritesFile, 8, Par.PLAYER_NO_TEXTURE_BUG))
            CorruptedFilesCount(Par.PLAYER_NO_TEXTURE_BUG) = 1
            Form1.C.IsBugged = True
        End If
    End Sub
    Public Sub bulletsNoTexture()
        If CorruptedFilesCount(Par.BULLETS_NO_TEXTURE_BUG) = 0 Then
            CorruptedFileType.addCorruptedFileToDir(Form1.fireballsTextureDir, New CorruptedFileType(Form1.fireballTexturefile, 2, Par.BULLETS_NO_TEXTURE_BUG))
            CorruptedFilesCount(Par.BULLETS_NO_TEXTURE_BUG) = 1
        End If
    End Sub
    Public Sub backgroundBug()
        If CorruptedFilesCount(Par.BACKGROUND_BUG) = 0 Then
            CorruptedFileType.addCorruptedFileToDir(Form1.terrainTextureDir, New CorruptedFileType(Form1.backgroundFile1, 4, Par.BACKGROUND_BUG))
            CorruptedFileType.addCorruptedFileToDir(Form1.terrainTextureDir, New CorruptedFileType(Form1.backgroundFile2, 3, Par.BACKGROUND_BUG))
            CorruptedFilesCount(Par.BACKGROUND_BUG) = 2
            Form1.isBackgroundBugged = True
            Form1.initRender(False)
            Form1.drawTerrain()
        End If
    End Sub
    Public Sub noHealth()
        If CorruptedFilesCount(Par.NOHEALTH_BUG) = 0 Then
            CorruptedFileType.addCorruptedFileToDir(Form1.HUDDir, New CorruptedFileType(Form1.HUDHealthDataDir, Form1.param.Health, Par.NOHEALTH_BUG))
            CorruptedFilesCount(Par.NOHEALTH_BUG) = 1
            Form1.map.HUD.IsHealthBugged = True
        End If
    End Sub
    Public Sub noMana()
        If CorruptedFilesCount(Par.NOMANA_BUG) = 0 Then
            CorruptedFileType.addCorruptedFileToDir(Form1.HUDDir, New CorruptedFileType(Form1.HUDManaDataDir, 4, Par.NOMANA_BUG))
            CorruptedFilesCount(Par.NOMANA_BUG) = 1
            Form1.map.HUD.IsManaBugged = True
        End If
    End Sub

    Public Sub oneCorruptedFileKilled(bugID As Integer)
        If CorruptedFilesCount(bugID) > 0 Then
            CorruptedFilesCount(bugID) -= 1
        End If
        If CorruptedFilesCount(bugID) = 0 Then
            resolveAllBugs(bugID)
        End If
    End Sub
    Private Sub resolveAllBugs(bugID As Integer)
        Form1.playSound(SoundsManager.SOLVE_BUG)
        If Form1.param.Health < 2 * Par.INITIAL_HEALTH Then
            Form1.param.Health += 1
        End If
        Select Case bugID
            Case Par.SHUFFLE_BUG
                resolveShuffledKeys()
            Case Par.NONWORKING_PLATFORMS_BUG
                resolveNonWorkingPlatforms()
            Case Par.NOLEVEL_BUG
                resolveNoLevel()
            Case Par.PLAYER_NO_TEXTURE_BUG
                resolvePlayerNoTexture()
            Case Par.BACKGROUND_BUG
                resolveBackgroundBug()
            Case Par.NOHEALTH_BUG
                resolveNoHealth()
            Case Par.NOMANA_BUG
                resolveNoMana()
        End Select
    End Sub
    Private Sub resolveShuffledKeys()
        For i As Integer = 0 To Par.KEY_NUMBER - 1
            Par.KEYS_ARRAY(i) = Par.PROPER_KEYS_ARRAY(Form1.keyboardType, i)
        Next
    End Sub
    Private Sub resolveNonWorkingPlatforms()
        For Each platform As PlatformType In Form1.map.PlatformsList
            If Not platform.IsEssential Then
                platform.Enabled = True
            End If
        Next
        For Each wall As WallType In Form1.map.WallsList
            If Not wall.IsEssential Then
                wall.Enabled = True
            End If
        Next
    End Sub
    Private Sub resolveNoLevel()
        For Each platform As PlatformType In Form1.map.PlatformsList
            If Not platform.IsEssential Then
                platform.Enabled = True
            End If
        Next
        For Each wall As WallType In Form1.map.WallsList
            If Not wall.IsEssential Then
                wall.Enabled = True
            End If
        Next
        Form1.isThereLevel = True
        Form1.initRender(False)
        Form1.drawTerrain()
    End Sub
    Private Sub resolvePlayerNoTexture()
        Form1.C.IsBugged = False
    End Sub
    Private Sub resolveBackgroundBug()
        Form1.isBackgroundBugged = False
        Form1.initRender(False)
        Form1.drawTerrain()
    End Sub
    Private Sub resolveNoHealth()
        Form1.map.HUD.IsHealthBugged = False
    End Sub
    Private Sub resolveNoMana()
        Form1.map.HUD.IsManaBugged = False
    End Sub

    Public Sub randomBug(count As Integer)
        For i As Integer = 1 To count
            If lIndex.Count = 0 Then
                For k As Integer = 0 To Par.BUGS_COUNT - 1
                    lIndex.Add(k)
                Next
            End If
            Dim index As Integer = Form1.rand.Next(lIndex.Count)
            Select Case lIndex(index)
                Case Par.SHUFFLE_BUG
                    shuffledKeys()
                Case Par.NONWORKING_PLATFORMS_BUG
                    nonWorkingPlatforms()
                Case Par.NOLEVEL_BUG
                    noLevel()
                Case Par.PLAYER_NO_TEXTURE_BUG
                    playerNoTexture()
                Case Par.BULLETS_NO_TEXTURE_BUG
                    bulletsNoTexture()
                Case Par.BACKGROUND_BUG
                    backgroundBug()
                Case Par.NOHEALTH_BUG
                    noHealth()
                Case Par.NOMANA_BUG
                    noMana()
            End Select
            lIndex.Remove(index)
        Next
    End Sub

End Class