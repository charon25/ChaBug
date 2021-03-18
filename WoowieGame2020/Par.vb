Public Class Par

    'Général
    Public Const FORM_WIDTH As Integer = 1280
    Public Const FORM_HEIGHT As Integer = 720
    Public Const RESOLUTION As Double = 72.009
    Public Const FPS As Integer = 60
    Public Const T As Double = 1 / FPS
    Public Const FIRST_LEVEL As Integer = 0
    Public Const LAST_LEVEL As Integer = 6

    'Touches
    Public Const KEY_NUMBER As Integer = 6
    Public Shared KEYS_ARRAY As Keys() = {}
    Public Shared PROPER_KEYS_ARRAY As Keys(,) = {{Keys.D, Keys.Q, Keys.Z, Keys.S, Keys.E, Keys.A}, {Keys.D, Keys.A, Keys.W, Keys.S, Keys.E, Keys.Q}}
    Public Const CHANGE_KEYBOARD_KEY As Integer = Keys.K
    Public Const RIGHT As Integer = 0
    Public Const LEFT As Integer = 1
    Public Const JUMP As Integer = 2
    Public Const DESCEND As Integer = 3
    Public Const OPEN_EXPLORER As Integer = 4
    Public Const ENTER_ELEMENT As Integer = 5

    'Joueur
    Public Const GRAVITY As Double = 5.5
    Public Const PLAYER_VELOCITY As Double = 15
    Public Const PLAYER_JUMP_VELOCITY As Double = 46
    '
    Public Const SHOOT_MAX_COOLDOWN_HEALTH As Integer = 7
    Public Const SHOOT_MAX_COOLDOWN As Integer = 35
    Public Const SHOOT_MIN_COOLDOWN_HEALTH As Integer = 1
    Public Const SHOOT_MIN_COOLDOWN As Integer = 15
    Public Const SHOOT_COOLDOWN_COEFFICIENT As Double = (SHOOT_MAX_COOLDOWN - SHOOT_MIN_COOLDOWN) / (SHOOT_MAX_COOLDOWN_HEALTH - SHOOT_MIN_COOLDOWN_HEALTH)
    '
    Public Const SHOOT_RECOVERY As Integer = 8
    Public Const PLAYER_WIDTH As Integer = 24 '20
    Public Const PLAYER_HEIGHT As Integer = 24 '40
    Public Const CHARACTER_STATES As Integer = 10
    Public Const CHARACTER_ANIMATION_FRAMES As Integer = 4
    Public Enum characterAnimatorState
        IdleRight = 0
        IdleLeft = 1
        MovingRight = 2
        MovingLeft = 3
        AirBorneUpRight = 4
        AirBorneUpLeft = 5
        AirBorneDownRight = 6
        AirBorneDownLeft = 7
        ShootingRight = 8
        ShootingLeft = 9
    End Enum
    Public Shared CHARACTER_ANIMATION_SPEED As Integer() = {-1, -1, 1, 1, -1, -1, -1, -1, -1, -1}
    Public Const PLAYER_INV_COOLDOWN As Integer = 40
    Public Const PLAYER_BLINK_COOLDOWN As Integer = 5

    'Balles
    Public Const BULLET_VELOCITY As Double = 30
    Public Const BULLET_WIDTH As Integer = 10
    Public Const BULLET_HEIGHT As Integer = 10
    Public Enum bulletOrigin
        Player = 0
        Enemy = 1
        CorruptedFile = 2
    End Enum

    'Ennemis
    Public Const ENEMY_WIDTH As Integer = 24
    Public Const ENEMY_HEIGHT As Integer = 22
    Public Const ENEMY_RECOVERY As Integer = 18
    Public Const ENEMY_STATES As Integer = 4
    Public Enum enemyState
        IdleRight = 0
        IdleLeft = 1
        ShootingRight = 2
        ShootingLeft = 3
    End Enum

    'Murs
    Public Enum wallState
        Normal = 0
        Explorer = 1
    End Enum
    Public Enum normalWallTexturesType
        ULCorner = 0
        URCorner = 1
        LRCorner = 2
        LLCorner = 3
        TopBorder = 4
        RightBorder = 5
        BottomBorder = 6
        LeftBorder = 7
        Center = 8
    End Enum
    Public Const NORMAL_WALL_TEXTURES_COUNT As Integer = 9
    Public Const WALL_TILE_SIZE As Integer = 25

    'Jeu
    Public Enum gameState
        Level = 0
        LoadingLevel = 1
        Explorer = 2
        Death = 3
        Win = 4
    End Enum

    'HUD
    Public Const HUD_WIDTH As Integer = 300
    Public Const HUD_HEIGHT As Integer = 100
    Public Const HEALTH_SIZE As Integer = 32
    Public Const HEALTH_OFFSET_X As Integer = 10
    Public Const HEALTH_OFFSET_Y As Integer = HEALTH_OFFSET_X
    Public Const MANA_OFFSET_X As Integer = HEALTH_OFFSET_X
    Public Const MANA_OFFSET_Y As Integer = HUD_HEIGHT - HEALTH_SIZE - HEALTH_OFFSET_X
    Public Const MANA_BAR_WIDTH As Integer = 194
    Public Const MANA_BAR_HEIGHT As Integer = 26
    Public Const MANA_BAR_OFFSET As Integer = 3

    'Explorer
    Public Const EXPLORER_WIDTH As Integer = 896
    Public Const EXPLORER_HEIGHT As Integer = 504
    Public Const EXPLORER_CORNER_X As Integer = (FORM_WIDTH - EXPLORER_WIDTH) / 2
    Public Const EXPLORER_CORNER_Y As Integer = 70 + (FORM_HEIGHT - EXPLORER_HEIGHT) / 2
    Public Const EXPLORER_PLATFORM_Y As Integer = EXPLORER_CORNER_Y + 475
    Public Const EXPLORER_WALLS_WIDTH As Integer = 6
    'Interieur
    Public Const EXPLORER_INT_WIDTH As Integer = 886
    Public Const EXPLORER_INT_HEIGHT As Integer = 429
    Public Const EXPLORER_INT_OFFSET_X As Integer = 5
    Public Const EXPLORER_INT_OFFSET_Y As Integer = 46
    ''Arbre 
    Public Const EXPLORER_ELEMENT_WIDTH As Integer = 140
    Public Const EXPLORER_ELEMENT_HEIGHT As Integer = 120
    Public Const EXPLORER_ELEMENTS_PER_LINE As Integer = 4
    Public Const EXPLORER_ELEMENTS_SPACE_X As Integer = 60
    Public Const EXPLORER_ELEMENTS_SPACE_Y As Integer = 63
    Public Const EXPLORER_ELEMENT_OFFSET_X As Integer = (EXPLORER_WIDTH - EXPLORER_ELEMENTS_PER_LINE * (EXPLORER_ELEMENT_WIDTH + EXPLORER_ELEMENTS_SPACE_X)) / 2
    Public Const EXPLORER_ELEMENT_OFFSET_Y As Integer = 40
    ''Murs de l'arbre
    Public Const DIRECTORY_Y_RETRAIT As Integer = 20
    Public Const FILE_Y_RETRAIT As Integer = 20
    ''Fichiers corrompus
    Public Const CF_HEALTH_WIDTH As Integer = 98
    Public Const CF_HEALTH_HEIGHT As Integer = 8
    Public Const CF_HEALTH_OFFSET_X As Integer = 1
    Public Const CF_HEALTH_OFFSET_Y As Integer = 1
    Public Const CF_RETRAIT As Integer = 20
    Public Const CF_COOLDOWN = 13

    'Bugs
    Public Const FIRST_LEVEL_WITH_BUG As Integer = 4
    Public Shared BUGS_BY_LEVEL() As Integer = {0, 0, 0, 0, 1, 1, 2}
    Public Const BUGS_COUNT As Integer = 8
    Public Const SHUFFLE_BUG As Integer = 0
    Public Const NONWORKING_PLATFORMS_BUG As Integer = 1
    Public Const NOLEVEL_BUG As Integer = 2
    Public Const PLAYER_NO_TEXTURE_BUG As Integer = 3
    Public Const BULLETS_NO_TEXTURE_BUG As Integer = 4
    Public Const BACKGROUND_BUG As Integer = 5
    Public Const NOHEALTH_BUG As Integer = 6
    Public Const NOMANA_BUG As Integer = 7

    ''Bug background
    Public Const BG_BUG_MIN As Integer = 500
    Public Const BG_BUG_MAX As Integer = 500
    Public Const BG_BUG_SIZE As Integer = 30
    Public Const BG_BUG_COUNT_MAX_X As Integer = FORM_WIDTH / BG_BUG_SIZE
    Public Const BG_BUG_COUNT_MAX_Y As Integer = FORM_HEIGHT / BG_BUG_SIZE

    'Paramètres
    Public Const INITIAL_HEALTH As Integer = 5

    'Fonctions
    Public Shared Function collides(X1 As Double, Y1 As Double, W1 As Double, H1 As Double, X2 As Double, Y2 As Double, W2 As Double, H2 As Double) As Boolean
        Return Not (X1 >= X2 + W2 OrElse X1 + W1 <= X2 OrElse Y1 >= Y2 + H2 OrElse Y1 + H1 <= Y2)
    End Function
    Public Shared Function collides(P1 As PointF, W1 As Double, H1 As Double, P2 As PointF, W2 As Double, H2 As Double) As Boolean
        Return Not (P1.X >= P2.X + W2 OrElse P1.X + W1 <= P2.X OrElse P1.Y >= P2.Y + H2 OrElse P1.Y + H1 <= P2.Y)
    End Function

End Class
