Imports System.Web.Script.Serialization, System.IO, System.Drawing.Imaging, System.Threading

Public Class Form1

    'Jeu
    Public gameState As Par.gameState
    Public rand As Random
    Public Bugs As Bugs
    Dim audioPlayer As SoundsManager
    Dim audioPlayerThread As Thread
    Dim musicStart As Long
    Public keyboardType As Integer
    Public changeKeyboard As Boolean
    Public oldChangeKeyboard As Boolean
    Public isDeadOnce As Boolean

    'Rendu
    Dim chunk As PictureBox
    Dim chunkMS As MemoryStream
    Public Textures As TexturesType
    Public isThereLevel As Boolean
    Public isBackgroundBugged As Boolean

    'Personnage
    Public C As CharacterType

    'Map
    Public map As MapType
    Public param As Parameters
    Dim tableauIndex As Integer
    Public bulletsToRemove As List(Of BulletType)
    Public enemiesToRemove As List(Of EnemyType)
    Public CFToRemove As List(Of CorruptedFileType)
    ''POLICE
    Const FONT_PATH As String = "files\font\Timeless.ttf"
    Public PFC As System.Drawing.Text.PrivateFontCollection

    'FPS
    Dim t1 As Long
    Dim delta, fps As Double

    'Touches
    Dim keyState() As Boolean
    Dim keyOldState() As Boolean

    'Explorer
    Dim explorerTextureObject As TextureObjectType
    Public isExplorerVisible As Boolean
    Dim explorerBottomPlatform As PlatformType
    Dim explorerTopPlatform As PlatformType
    Dim explorerTopPlatform2 As PlatformType
    Dim explorerLeftWall As WallType
    Dim explorerRightWall As WallType
    ''Éléments de l'explorateur
    Public rootDir As DirectoryType
    '---
    Public assetsDir As DirectoryType
    Public dataDir As DirectoryType
    Public levelsDir As DirectoryType
    Public configFile As FileType
    Public readmeFile As FileType
    '---
    Public texturesDir As DirectoryType
    Public soundsDir As DirectoryType
    '------
    Public terrainDir As DirectoryType
    Public playerDir As DirectoryType
    Public enemiesDir As DirectoryType
    Public HUDDir As DirectoryType
    Public generalDataFile As FileType
    Public textureRendererFile As FileType
    '------
    Public levelLoaderFile As FileType
    Public world1File As FileType
    '------
    '---
    Public terrainTextureDir As DirectoryType
    Public playerTextureDir As DirectoryType
    Public enemiesTextureDir As DirectoryType
    Public fireballsTextureDir As DirectoryType
    '------
    Public platformDataDir As FileType
    Public HUDHealthDataDir As FileType
    Public HUDManaDataDir As FileType
    '---
    Public playerSpritesFile As FileType
    Public fireballTexturefile As FileType
    Public backgroundFile1 As FileType
    Public backgroundFile2 As FileType

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        generateFolders()
        audioPlayer = New SoundsManager()
        keyboardType = 1

        'Touches
        ReDim Par.KEYS_ARRAY(Par.KEY_NUMBER - 1)
        ReDim keyState(Par.KEY_NUMBER - 1)
        ReDim keyOldState(Par.KEY_NUMBER - 1)
        For i As Integer = 0 To Par.KEY_NUMBER - 1
            Par.KEYS_ARRAY(i) = Par.PROPER_KEYS_ARRAY(keyboardType, i)
            keyState(i) = False
            keyOldState(i) = False
        Next

        'Rendu
        Textures = New TexturesType()

        'Jeu
        gameState = Par.gameState.Level
        rand = New Random()
        Bugs = New Bugs()
        isThereLevel = True
        isBackgroundBugged = False
        isDeadOnce = False

        'Explorer
        isExplorerVisible = False
        explorerTextureObject = New TextureObjectType(New Point(Par.EXPLORER_CORNER_X, Par.EXPLORER_CORNER_Y), New Size(Par.EXPLORER_WIDTH, Par.EXPLORER_HEIGHT), "explorerBackground")
        explorerTextureObject.generateTexture()
        explorerBottomPlatform = New PlatformType(Par.EXPLORER_CORNER_X, Par.EXPLORER_PLATFORM_Y, Par.EXPLORER_WIDTH, False, True, "explorerBottomPlatform", True)
        explorerTopPlatform = New PlatformType(Par.EXPLORER_CORNER_X, Par.EXPLORER_CORNER_Y + 1, Par.EXPLORER_WIDTH, True, True, "explorerTopPlatform", True)
        explorerTopPlatform2 = New PlatformType(Par.EXPLORER_CORNER_X, Par.EXPLORER_CORNER_Y + 1, Par.EXPLORER_WIDTH, True, False, "explorerTopPlatform2", True)
        generateExplorerTree()

        'Map
        tableauIndex = Par.FIRST_LEVEL
        param = New Parameters()
        initMap()
        '###########

        '###########
        initRendering()

        'Police
        If Not File.Exists(FONT_PATH) Then
            File.WriteAllBytes(FONT_PATH, My.Resources.Timeless)
        Else
            PFC = New System.Drawing.Text.PrivateFontCollection()
            PFC.AddFontFile(FONT_PATH)
        End If

        'Game Timer
        gameTimer.Start()

    End Sub
    Sub generateFolders()
        If Not Directory.Exists("files") Then
            Directory.CreateDirectory("files")
        End If
        If Not Directory.Exists("files\font") Then
            Directory.CreateDirectory("files\font")
        End If
        If Not Directory.Exists("files\sounds") Then
            Directory.CreateDirectory("files\sounds")
        End If
    End Sub
    Sub generateExplorerTree()
        rootDir = New DirectoryType(New Point(0, 0), "root", Nothing, True)
        '---
        assetsDir = New DirectoryType(ExplorerElementType.getCoordinates(0), "Assets", rootDir)
        rootDir.Childs.Add(assetsDir)
        dataDir = New DirectoryType(ExplorerElementType.getCoordinates(1), "Data", rootDir)
        rootDir.Childs.Add(dataDir)
        levelsDir = New DirectoryType(ExplorerElementType.getCoordinates(2), "Levels", rootDir)
        rootDir.Childs.Add(levelsDir)
        configFile = New FileType(ExplorerElementType.getCoordinates(3), "config.txt", "configtxt")
        rootDir.Childs.Add(configFile)
        readmeFile = New FileType(ExplorerElementType.getCoordinates(4), "readme.txt", "configtxt")
        rootDir.Childs.Add(readmeFile)
        '---
        soundsDir = New DirectoryType(ExplorerElementType.getCoordinates(1), "Sounds", assetsDir)
        assetsDir.Childs.Add(soundsDir)
        texturesDir = New DirectoryType(ExplorerElementType.getCoordinates(2), "Textures", assetsDir)
        assetsDir.Childs.Add(texturesDir)
        '------
        terrainDir = New DirectoryType(ExplorerElementType.getCoordinates(1), "Terrain", dataDir)
        dataDir.Childs.Add(terrainDir)
        playerDir = New DirectoryType(ExplorerElementType.getCoordinates(2), "Player", dataDir)
        dataDir.Childs.Add(playerDir)
        enemiesDir = New DirectoryType(ExplorerElementType.getCoordinates(3), "Enemies", dataDir)
        dataDir.Childs.Add(enemiesDir)
        HUDDir = New DirectoryType(ExplorerElementType.getCoordinates(4), "HUD", dataDir)
        dataDir.Childs.Add(HUDDir)
        generalDataFile = New FileType(ExplorerElementType.getCoordinates(5), "general.dat", "void")
        dataDir.Childs.Add(generalDataFile)
        '------
        levelLoaderFile = New FileType(ExplorerElementType.getCoordinates(1), "level_loader.bat", "void")
        levelsDir.Childs.Add(levelLoaderFile)
        world1File = New FileType(ExplorerElementType.getCoordinates(2), "world_1.map", "configtxt")
        levelsDir.Childs.Add(world1File)
        '---
        terrainTextureDir = New DirectoryType(ExplorerElementType.getCoordinates(1), "Terrain", texturesDir)
        texturesDir.Childs.Add(terrainTextureDir)
        playerTextureDir = New DirectoryType(ExplorerElementType.getCoordinates(2), "Player", texturesDir)
        texturesDir.Childs.Add(playerTextureDir)
        enemiesTextureDir = New DirectoryType(ExplorerElementType.getCoordinates(3), "Enemies", texturesDir)
        texturesDir.Childs.Add(enemiesTextureDir)
        fireballsTextureDir = New DirectoryType(ExplorerElementType.getCoordinates(4), "Fireballs", texturesDir)
        texturesDir.Childs.Add(fireballsTextureDir)
        '------
        platformDataDir = New FileType(ExplorerElementType.getCoordinates(1), "platform.dat", "void")
        terrainDir.Childs.Add(platformDataDir)
        HUDHealthDataDir = New FileType(ExplorerElementType.getCoordinates(1), "health.dat", "void")
        HUDDir.Childs.Add(HUDHealthDataDir)
        HUDManaDataDir = New FileType(ExplorerElementType.getCoordinates(2), "mana.dat", "void")
        HUDDir.Childs.Add(HUDManaDataDir)
        '---
        playerSpritesFile = New FileType(ExplorerElementType.getCoordinates(1), "sprite.png", "image")
        playerTextureDir.Childs.Add(playerSpritesFile)
        fireballTexturefile = New FileType(ExplorerElementType.getCoordinates(1), "fireball.png", "image")
        fireballsTextureDir.Childs.Add(fireballTexturefile)
        backgroundFile1 = New FileType(ExplorerElementType.getCoordinates(1), "bg_1.png", "image")
        terrainTextureDir.Childs.Add(backgroundFile1)
        backgroundFile2 = New FileType(ExplorerElementType.getCoordinates(2), "bg_2.png", "image")
        terrainTextureDir.Childs.Add(backgroundFile2)
    End Sub

    '#######################################MAP
    Sub initMap()
        gameState = Par.gameState.LoadingLevel
        If Not isDeadOnce AndAlso tableauIndex = Par.FIRST_LEVEL Then
            map = MapType.createMapFromText(My.Resources.levels.Split("§")(tableauIndex), VectorType.nullVector(), True)
        Else
            map = MapType.createMapFromText(My.Resources.levels.Split("§")(tableauIndex), C.Location, False, map.HUD)
        End If
        map.PlatformsList.Add(explorerBottomPlatform)
        map.PlatformsList.Add(explorerTopPlatform)
        If map.GroundHeight < Par.EXPLORER_PLATFORM_Y Then
            explorerLeftWall = New WallType(New VectorType(Par.EXPLORER_CORNER_X, map.GroundHeight), Par.EXPLORER_WALLS_WIDTH, (Par.EXPLORER_PLATFORM_Y - map.GroundHeight), 1, False, False, "explorerLeftWall", True)

            explorerRightWall = New WallType(New VectorType(Par.EXPLORER_CORNER_X + Par.EXPLORER_WIDTH - Par.EXPLORER_WALLS_WIDTH, map.GroundHeight), Par.EXPLORER_WALLS_WIDTH, Par.EXPLORER_PLATFORM_Y - map.GroundHeight, Par.wallState.Explorer, False, False, "explorerRightWall", True)

            map.WallsList.Add(explorerLeftWall)
            map.PlatformsList.Add(explorerLeftWall.getAssociatedPlatform())
            map.WallsList.Add(explorerRightWall)
            map.PlatformsList.Add(explorerRightWall.getAssociatedPlatform())
        End If
        C = map.Character
        C.ContainingDirectory = rootDir
        If tableauIndex = Par.FIRST_LEVEL_WITH_BUG Then
            Bugs.noHealth()
        ElseIf tableauIndex > Par.FIRST_LEVEL_WITH_BUG Then
            Bugs.randomBug(Par.BUGS_BY_LEVEL(tableauIndex))
        End If
        initRender(Not isDeadOnce AndAlso tableauIndex = Par.FIRST_LEVEL)
    End Sub
    Sub initRender(first As Boolean)
        chunkMS = New MemoryStream()
        If first Then
            chunk = New PictureBox()
            With chunk
                .Size = New Size(Par.FORM_WIDTH, Par.FORM_HEIGHT)
                .Location = New Point(0, 0)
                .BackColor = Color.Transparent
                .Enabled = False
                .SendToBack()
            End With
            Me.gamePanel.Controls.Add(chunk)
        End If
    End Sub
    Sub initRendering()
        drawTerrain()
        initVariables()
        If isExplorerVisible Then
            openExplorer()
        End If
        gameState = Par.gameState.Level
    End Sub
    Sub initVariables()
        bulletsToRemove = New List(Of BulletType)
        enemiesToRemove = New List(Of EnemyType)
        CFToRemove = New List(Of CorruptedFileType)
    End Sub
    Sub nextTableau()
        If param.Health < Par.INITIAL_HEALTH - 2 Then
            param.Health += 1
        End If
        If tableauIndex = Par.LAST_LEVEL Then
            win()
        Else
            tableauIndex += 1
            playSound(SoundsManager.END_OF_TABLEAU)
            initMap()
            initRendering()
        End If
    End Sub
    Sub win()
        MsgBox("bravo")
        gameState = Par.gameState.Win
    End Sub

    '#######################################Touches
    Private Sub Form1_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        For i As Integer = 0 To Par.KEYS_ARRAY.Length - 1
            If e.KeyCode = Par.KEYS_ARRAY(i) Then
                keyState(i) = True
            End If
        Next
        If e.KeyCode = Par.CHANGE_KEYBOARD_KEY Then
            changeKeyboard = True
        End If
    End Sub
    Private Sub Form1_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        For i As Integer = 0 To Par.KEYS_ARRAY.Length - 1
            If e.KeyCode = Par.KEYS_ARRAY(i) Then
                keyState(i) = False
            End If
        Next
        If e.KeyCode = Par.CHANGE_KEYBOARD_KEY Then
            changeKeyboard = False
        End If
    End Sub
    Private Sub gamePanel_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles gamePanel.MouseDown
        createBullet(e.Location)
    End Sub

    '#######################################SON
    Sub playSound(name As String)
        audioPlayerThread = New Thread(Sub() audioPlayer.playSound(name))
        audioPlayerThread.Start()
    End Sub


    '#######################################DESSIN
    Sub drawTerrain()
        Dim img As Bitmap = getBackgroundImage()
        img.SetResolution(Par.RESOLUTION, Par.RESOLUTION)
        Dim g As Graphics = Graphics.FromImage(img)

        If isThereLevel Then
            'Murs
            For Each wall As WallType In map.WallsList
                If wall.WallState = Par.wallState.Normal AndAlso wall.HasTextures Then
                    g.DrawImage(wall.getTexture(), wall.Boundaries.Location.toPoint())
                End If
            Next
        Else
            g.FillRectangle(Brushes.Black, 0, 0, Par.FORM_WIDTH, Par.FORM_HEIGHT)
        End If

        chunk.Image = img
        chunk.Refresh()
        img.Save(chunkMS, ImageFormat.Png)
    End Sub
    Sub drawMovingObjects()
        Dim img As Bitmap = Image.FromStream(chunkMS)
        img.SetResolution(Par.RESOLUTION, Par.RESOLUTION)
        Dim g As Graphics = Graphics.FromImage(img)

        'Textures
        For Each textureObject As TextureObjectType In map.TextureObjectsList
            g.DrawImage(textureObject.BackgroundImage, textureObject.Location)
        Next

        If isThereLevel Then
            'Ennemis 
            For Each enemy As EnemyType In map.enemiesList
                If Not (enemy.ShouldBeInExplorer Xor enemy.IsInExplorer) Then
                    g.DrawImage(enemy.getTexture(), enemy.getCorner())
                End If
            Next
        End If
        If isExplorerVisible Then
            For Each corruptedfile As CorruptedFileType In C.ContainingDirectory.CorruptedFilesList
                g.DrawImage(My.Resources.corruptedFile, corruptedfile.File.Location + New Point(Par.EXPLORER_CORNER_X + Par.EXPLORER_INT_OFFSET_X, Par.EXPLORER_CORNER_Y + Par.EXPLORER_INT_OFFSET_Y - Par.CF_RETRAIT))
                g.DrawImage(corruptedfile.generateHealthBarTexture(), corruptedfile.File.Location.X + Par.EXPLORER_CORNER_X + Par.EXPLORER_INT_OFFSET_X + Par.FILE_Y_RETRAIT, corruptedfile.File.Location.Y + Par.EXPLORER_CORNER_Y + Par.EXPLORER_INT_OFFSET_Y + Par.EXPLORER_ELEMENT_HEIGHT + 4)
            Next
        End If

        'Joueur 
        g.DrawImage(C.getTexture(), C.getCorner())

        'Balles  
        For Each bullet As BulletType In map.bulletsList
            If Not (bullet.IsInExplorer Xor bullet.CreatedInExplorer) Then
                If Bugs.CorruptedFilesCount(Par.BULLETS_NO_TEXTURE_BUG) = 0 Then
                    If bullet.Origin = Par.bulletOrigin.CorruptedFile Then
                        g.DrawImage(My.Resources.corruptedFileBullet, bullet.getCorner())
                    Else
                        g.DrawImage(My.Resources.bullet, bullet.getCorner())
                    End If
                Else
                    g.DrawImage(My.Resources.bulletBugged, bullet.getCorner())
                End If
            End If
        Next

        'HUD 
        g.DrawImage(map.HUD.backgroundImage, 0, 0)

        img.SetResolution(Par.RESOLUTION, Par.RESOLUTION)
        chunk.Image = img
        chunk.Refresh()
    End Sub
    Function getBackgroundImage() As Bitmap
        Dim img As Bitmap = My.Resources.background
        If isBackgroundBugged Then
            Dim g As Graphics = Graphics.FromImage(img)
            For i As Integer = 1 To rand.Next(Par.BG_BUG_MIN, Par.BG_BUG_MAX + 1)
                g.DrawImage(My.Resources.backgroundBugged, Par.BG_BUG_SIZE * rand.Next(Par.BG_BUG_COUNT_MAX_X + 1), Par.BG_BUG_SIZE * rand.Next(Par.BG_BUG_COUNT_MAX_Y + 1))
            Next
        End If
        Return img.Clone()
    End Function

    '#######################################Balles
    Sub createBullet(mouseLocation As Point)
        If C.Cooldown = 0 Then
            playSound(SoundsManager.FIREBALL(rand))
            Dim start As VectorType = C.Location
            If C.isLookingLeft Then
                start += New VectorType(-9, -7)
            Else
                start += New VectorType(9, -7)
            End If
            map.bulletsList.Add(New BulletType(start, New VectorType(mouseLocation), Par.bulletOrigin.Player))
            C.InitialCooldown = Par.SHOOT_MIN_COOLDOWN + Math.Round(param.Health * Par.SHOOT_COOLDOWN_COEFFICIENT)
            C.Cooldown = C.InitialCooldown
        End If
    End Sub

    '#######################################Défaite
    Sub loseHealth()
        If C.InvincibilityCooldown = 0 Then
            param.Health -= 1
            C.InvincibilityCooldown = Par.PLAYER_INV_COOLDOWN
            If param.Health = 0 Then
                lose()
            Else
                playSound(SoundsManager.HURT(rand))
            End If
        End If
    End Sub
    Sub lose()
        playSound(SoundsManager.DEATH)
        gameState = Par.gameState.Death
    End Sub

    '#######################################Timer de Jeu
    Sub openExplorer()
        playSound(SoundsManager.ENTER_DIR)
        map.PlatformsList.Add(explorerTopPlatform2)
        If map.TextureObjectsList.Contains(explorerTextureObject) Then
            map.TextureObjectsList.Remove(explorerTextureObject)
        End If
        Dim img As Bitmap = My.Resources.textureObjectType_explorerBackground
        Dim g As Graphics = Graphics.FromImage(img)
        g.DrawImage(ExplorerElementType.generateBackground(C.ContainingDirectory), New Point(Par.EXPLORER_INT_OFFSET_X, Par.EXPLORER_INT_OFFSET_Y))
        explorerTextureObject.BackgroundImage = img.Clone()
        map.TextureObjectsList.Add(explorerTextureObject)
        '
        Dim wallsToRemove As New List(Of WallType)
        For Each wall As WallType In map.WallsList
            If wall.Name = "explorerElement" Then
                wallsToRemove.Add(wall)
            End If
        Next
        For Each wall As WallType In wallsToRemove
            map.WallsList.Remove(wall)
        Next
        Dim platformsToRemove As New List(Of PlatformType)
        For Each platform As PlatformType In map.PlatformsList
            If platform.Name = "explorerElement" Then
                platformsToRemove.Add(platform)
            End If
        Next
        For Each platform As PlatformType In platformsToRemove
            map.PlatformsList.Remove(platform)
        Next
        '
        For Each explorerElement As ExplorerElementType In C.ContainingDirectory.Childs
            map.WallsList.Add(explorerElement.getAssociatedWall())
            map.PlatformsList.Add(map.WallsList.Last().getAssociatedPlatform)
        Next
        '
        For Each bullet As BulletType In map.bulletsList
            If bullet.CreatedInExplorer Then
                bullet.Unfreeze()
            End If
        Next
    End Sub
    Sub closeExplorer()
        playSound(SoundsManager.CLOSE_EXPLORER)
        Dim wallsToRemove As New List(Of WallType)
        For Each wall As WallType In map.WallsList
            If wall.Name = "explorerElement" Then
                wallsToRemove.Add(wall)
            End If
        Next
        For Each wall As WallType In wallsToRemove
            map.WallsList.Remove(wall)
        Next
        Dim platformsToRemove As New List(Of PlatformType)
        For Each platform As PlatformType In map.PlatformsList
            If platform.Name = "explorerElement" OrElse platform.Name = "explorerTopPlatform2" Then
                platformsToRemove.Add(platform)
            End If
        Next
        For Each platform As PlatformType In platformsToRemove
            map.PlatformsList.Remove(platform)
        Next
        '
        map.TextureObjectsList.Remove(explorerTextureObject)
        For Each bullet As BulletType In map.bulletsList
            If bullet.CreatedInExplorer Then
                bullet.Freeze()
            End If
        Next
        If C.Location.Y > map.GroundHeight Then
            C.Location.Y = map.GroundHeight - 5
        End If
    End Sub

    '#######################################Timer de Jeu
    Private Sub gameTimer_Tick(sender As System.Object, e As System.EventArgs) Handles gameTimer.Tick
        t1 = Date.Now.Ticks
        If gameState = Par.gameState.Level Or gameState = Par.gameState.Explorer Then
            'Touches
            If keyState(Par.RIGHT) Then
                C.goRight()
            Else
                C.stopRight()
            End If
            If keyState(Par.LEFT) Then
                C.goLeft()
            Else
                C.stopLeft()
            End If
            If Not C.IsAirborne Then
                If keyState(Par.JUMP) Then
                    C.jump()
                End If
                If keyState(Par.DESCEND) Then
                    C.stepDown()
                End If
            End If
            If changeKeyboard And Not oldChangeKeyboard Then
                If Bugs.CorruptedFilesCount(Par.SHUFFLE_BUG) = 0 Then
                    keyboardType = 1 - keyboardType
                    For i As Integer = 0 To Par.KEY_NUMBER - 1
                        Par.KEYS_ARRAY(i) = Par.PROPER_KEYS_ARRAY(keyboardType, i)
                    Next
                End If
            End If
            C.Input = keyState(Par.RIGHT) OrElse keyState(Par.LEFT) OrElse keyState(Par.JUMP) OrElse keyState(Par.DESCEND)
            C.move()
            If keyState(Par.OPEN_EXPLORER) AndAlso Not keyOldState(Par.OPEN_EXPLORER) Then
                isExplorerVisible = Not isExplorerVisible
                If isExplorerVisible Then
                    openExplorer()
                Else
                    closeExplorer()
                End If
            End If
            If keyState(Par.ENTER_ELEMENT) AndAlso Not keyOldState(Par.ENTER_ELEMENT) AndAlso Not C.IsAirborne Then
                If TypeOf C.steppingElement Is DirectoryType Then
                    If CType(C.steppingElement, DirectoryType).IsReturn Then
                        C.ContainingDirectory = C.ContainingDirectory.Parent
                    Else
                        C.ContainingDirectory = C.steppingElement
                    End If
                    openExplorer()
                End If
            End If

            'Ennemis
            For Each enemy As EnemyType In map.enemiesList
                If isThereLevel Then enemy.age()
            Next
            For Each enemy As EnemyType In enemiesToRemove
                map.enemiesList.Remove(enemy)
            Next
            enemiesToRemove.Clear()
            'Corrupted Files
            If isExplorerVisible Then
                For Each corruptedfile As CorruptedFileType In C.ContainingDirectory.CorruptedFilesList
                    corruptedfile.age()
                    If corruptedfile.IsDead Then
                        Bugs.oneCorruptedFileKilled(corruptedfile.BugID)
                        CFToRemove.Add(corruptedfile)
                    End If
                Next
                For Each corruptedfile As CorruptedFileType In CFToRemove
                    C.ContainingDirectory.CorruptedFilesList.Remove(corruptedfile)
                Next
                CFToRemove.Clear()
            End If

            'Balles
            For Each bullet As BulletType In map.bulletsList
                bullet.move()
                If bullet.isDead Then
                    bulletsToRemove.Add(bullet)
                End If
            Next
            For Each bullet As BulletType In bulletsToRemove
                map.bulletsList.Remove(bullet)
            Next
            bulletsToRemove.Clear()

            'HUD
            map.HUD.update()

            'Dessin
            drawMovingObjects()

            For i As Integer = 0 To Par.KEY_NUMBER - 1
                keyOldState(i) = keyState(i)
            Next
            oldChangeKeyboard = changeKeyboard

        ElseIf gameState = Par.gameState.Death Then
            If keyState(0) OrElse keyState(1) OrElse keyState(2) OrElse keyState(3) OrElse keyState(4) OrElse keyState(5) Then
                tableauIndex = Math.Max(0, tableauIndex - 3)
                param.Health = Par.INITIAL_HEALTH
                isDeadOnce = True
                initMap()
                initRendering()
                If isExplorerVisible Then
                    closeExplorer()
                End If
            End If
        End If

        'Musique
        If Date.Now.Ticks - musicStart > SoundsManager.MUSIC_DURATION_MS * 10000 Then
            musicStart = Date.Now.Ticks
            playSound(SoundsManager.MUSIC)
        End If

        'FPS
        delta = (Date.Now.Ticks - t1) / 10000000
        If delta < Par.T Then
            Thread.Sleep(Int(1000 * (Par.T - delta)))
        End If
    End Sub
End Class