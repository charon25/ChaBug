Imports System.IO

Public Class SoundsManager

    Private Declare Function mciSendString Lib "winmm.dll" Alias "mciSendStringA" _
(ByVal lpstrCommand As String, ByVal lpstrReturnString As String, ByVal uReturnLength As _
 Integer, ByVal hwndCallback As Integer) As Integer
    Private channelID As Integer
    Private channels As List(Of String)
    Private isSoundActivatedLocal As Boolean

    Sub New()
        channelID = 0
        channels = New List(Of String)
        generateSoundFiles()
        isSoundActivatedLocal = True
    End Sub
    Private Sub generateSoundFiles()
        For Each f As String In Directory.EnumerateDirectories(FOLDER)
            MsgBox(f)
            File.Delete(f)
        Next
        Dim tempMS As MemoryStream
        '---------------------------------------------------------Bullets
        tempMS = New MemoryStream()
        My.Resources.bullet_sound1.CopyTo(tempMS)
        File.WriteAllBytes(FIREBALL1, tempMS.ToArray())
        tempMS = New MemoryStream()
        My.Resources.bullet_sound2.CopyTo(tempMS)
        File.WriteAllBytes(FIREBALL2, tempMS.ToArray())
        tempMS = New MemoryStream()
        My.Resources.bullet_sound3.CopyTo(tempMS)
        File.WriteAllBytes(FIREBALL3, tempMS.ToArray())
        '---------------------------------------------------------Dégâts
        tempMS = New MemoryStream()
        My.Resources.hurt1.CopyTo(tempMS)
        File.WriteAllBytes(HURT1, tempMS.ToArray())
        tempMS = New MemoryStream()
        My.Resources.hurt2.CopyTo(tempMS)
        File.WriteAllBytes(HURT2, tempMS.ToArray())
        tempMS = New MemoryStream()
        My.Resources.hurt3.CopyTo(tempMS)
        File.WriteAllBytes(HURT3, tempMS.ToArray())
        tempMS = New MemoryStream()
        My.Resources.hurt4.CopyTo(tempMS)
        File.WriteAllBytes(HURT4, tempMS.ToArray())
        '---------------------------------------------------------Mort d'un ennemi
        tempMS = New MemoryStream()
        My.Resources.enemy_death1.CopyTo(tempMS)
        File.WriteAllBytes(ENEMY_DEATH1, tempMS.ToArray())
        tempMS = New MemoryStream()
        My.Resources.enemy_death2.CopyTo(tempMS)
        File.WriteAllBytes(ENEMY_DEATH2, tempMS.ToArray())
        '---------------------------------------------------------Fin d'un tableau
        tempMS = New MemoryStream()
        My.Resources.end_of_tableau.CopyTo(tempMS)
        File.WriteAllBytes(END_OF_TABLEAU, tempMS.ToArray())
        '---------------------------------------------------------Mort d'un fichier corrompu
        tempMS = New MemoryStream()
        My.Resources.cf_death1.CopyTo(tempMS)
        File.WriteAllBytes(CF_DEATH1, tempMS.ToArray())
        tempMS = New MemoryStream()
        My.Resources.cf_death2.CopyTo(tempMS)
        File.WriteAllBytes(CF_DEATH2, tempMS.ToArray())
        tempMS = New MemoryStream()
        My.Resources.cf_death3.CopyTo(tempMS)
        File.WriteAllBytes(CF_DEATH3, tempMS.ToArray())
        '---------------------------------------------------------Dégâts à un fichier corrompu
        tempMS = New MemoryStream()
        My.Resources.cf_hurt1.CopyTo(tempMS)
        File.WriteAllBytes(CF_HURT1, tempMS.ToArray())
        tempMS = New MemoryStream()
        My.Resources.cf_hurt2.CopyTo(tempMS)
        File.WriteAllBytes(CF_HURT2, tempMS.ToArray())
        tempMS = New MemoryStream()
        My.Resources.cf_hurt3.CopyTo(tempMS)
        File.WriteAllBytes(CF_HURT3, tempMS.ToArray())
        '---------------------------------------------------------Entrer dans un dossier
        tempMS = New MemoryStream()
        My.Resources.enter_dir.CopyTo(tempMS)
        File.WriteAllBytes(ENTER_DIR, tempMS.ToArray())
        '---------------------------------------------------------Saut
        tempMS = New MemoryStream()
        My.Resources.jump1.CopyTo(tempMS)
        File.WriteAllBytes(JUMP1, tempMS.ToArray())
        tempMS = New MemoryStream()
        My.Resources.jump2.CopyTo(tempMS)
        File.WriteAllBytes(JUMP2, tempMS.ToArray())
        '---------------------------------------------------------Marche normale
        tempMS = New MemoryStream()
        My.Resources.walk_normal.CopyTo(tempMS)
        File.WriteAllBytes(WALK_NORMAL, tempMS.ToArray())
        '---------------------------------------------------------Marche explorateur
        tempMS = New MemoryStream()
        My.Resources.walk_explorer.CopyTo(tempMS)
        File.WriteAllBytes(WALK_EXPLORER, tempMS.ToArray())
        '---------------------------------------------------------Fermer l'explorateur
        tempMS = New MemoryStream()
        My.Resources.close_explorer.CopyTo(tempMS)
        File.WriteAllBytes(CLOSE_EXPLORER, tempMS.ToArray())
        '---------------------------------------------------------Corriger un bug
        tempMS = New MemoryStream()
        My.Resources.solve_bug.CopyTo(tempMS)
        File.WriteAllBytes(SOLVE_BUG, tempMS.ToArray())
        '---------------------------------------------------------Mort
        tempMS = New MemoryStream()
        My.Resources.death.CopyTo(tempMS)
        File.WriteAllBytes(DEATH, tempMS.ToArray())
        '---------------------------------------------------------Tir d'un fichier corrompu
        tempMS = New MemoryStream()
        My.Resources.cf_bullet1.CopyTo(tempMS)
        File.WriteAllBytes(CF_BULLET1, tempMS.ToArray())
        tempMS = New MemoryStream()
        My.Resources.cf_bullet2.CopyTo(tempMS)
        File.WriteAllBytes(CF_BULLET2, tempMS.ToArray())
        tempMS = New MemoryStream()
        My.Resources.cf_bullet3.CopyTo(tempMS)
        File.WriteAllBytes(CF_BULLET3, tempMS.ToArray())
        '---------------------------------------------------------Musique
        tempMS = New MemoryStream()
        My.Resources.music.CopyTo(tempMS)
        File.WriteAllBytes(MUSIC, tempMS.ToArray())
    End Sub
    Public Sub closeAll(Optional evenMusic As Boolean = False)
        Dim channelsToDelete As New List(Of String)
        For Each channel As String In channels
            If evenMusic OrElse Not channel.Contains("music") Then
                stopSound(channel)
                channelsToDelete.Add(channel)
            End If
        Next
        For Each channel As String In channelsToDelete
            channels.Remove(channel)
        Next
        channelsToDelete.Clear()
    End Sub
    Public Function playSound(name As String) As String
        Dim soundName As String = CStr(Date.Now.Ticks)
        Dim nom As String = Path.GetFileNameWithoutExtension(name) & "_" & CStr(channelID)
        channels.Add(nom)
        mciSendString("open " & Chr(34) & name & Chr(34) & " alias " & nom, Nothing, 0, 0)
        mciSendString("play " & nom, Nothing, 0, 0)
        channelID += 1
        Return nom
    End Function
    Public Sub stopSound(name As String)
        If name <> "" Then
            mciSendString("stop " & name, Nothing, 0, 0)
            mciSendString("close " & name, Nothing, 0, 0)
        End If
    End Sub


    Private Const FOLDER As String = "files\sounds\"
    Private Const EXT As String = ".wav"

    'Fireball
    Private Const FIREBALL1 As String = FOLDER & "bullet1" & EXT
    Private Const FIREBALL2 As String = FOLDER & "bullet2" & EXT
    Private Const FIREBALL3 As String = FOLDER & "bullet3" & EXT
    Private Shared FIREBALLS() As String = {FIREBALL1, FIREBALL2, FIREBALL3}
    Public Shared Function FIREBALL(ByRef rand As Random) As String
        Return FIREBALLS(rand.Next(FIREBALLS.Length))
    End Function

    'Dégâts
    Private Const HURT1 As String = FOLDER & "hurt1" & EXT
    Private Const HURT2 As String = FOLDER & "hurt2" & EXT
    Private Const HURT3 As String = FOLDER & "hurt3" & EXT
    Private Const HURT4 As String = FOLDER & "hurt4" & EXT
    Private Shared HURTS() As String = {HURT1, HURT2, HURT3, HURT4}
    Public Shared Function HURT(ByRef rand As Random) As String
        Return HURTS(rand.Next(HURTS.Length))
    End Function

    'Mort d'un ennemi
    Private Const ENEMY_DEATH1 As String = FOLDER & "enemy_death1" & EXT
    Private Const ENEMY_DEATH2 As String = FOLDER & "enemy_death2" & EXT
    Private Shared ENEMY_DEATHS() As String = {ENEMY_DEATH1, ENEMY_DEATH2}
    Public Shared Function ENEMY_DEATH(ByRef rand As Random) As String
        Return ENEMY_DEATHS(rand.Next(ENEMY_DEATHS.Length))
    End Function

    'Fin d'un tableau
    Public Const END_OF_TABLEAU As String = FOLDER & "end_of_tableau" & EXT

    'Mort d'un fichier corrompu
    Private Const CF_DEATH1 As String = FOLDER & "cf_death1" & EXT
    Private Const CF_DEATH2 As String = FOLDER & "cf_death2" & EXT
    Private Const CF_DEATH3 As String = FOLDER & "cf_death3" & EXT
    Private Shared CF_DEATHS() As String = {CF_DEATH1, CF_DEATH2, CF_DEATH3}
    Public Shared Function CF_DEATH(ByRef rand As Random) As String
        Return CF_DEATHS(rand.Next(CF_DEATHS.Length))
    End Function

    'Dégâts à un fichier corrompu
    Private Const CF_HURT1 As String = FOLDER & "cf_hurt1" & EXT
    Private Const CF_HURT2 As String = FOLDER & "cf_hurt2" & EXT
    Private Const CF_HURT3 As String = FOLDER & "cf_hurt3" & EXT
    Private Shared CF_HURTS() As String = {CF_HURT1, CF_HURT2, CF_HURT3}
    Public Shared Function CF_HURT(ByRef rand As Random) As String
        Return CF_HURTS(rand.Next(CF_HURTS.Length))
    End Function

    'Entrer dans un dossier
    Public Const ENTER_DIR As String = FOLDER & "enter_dir" & EXT

    'Saut
    Private Const JUMP1 As String = FOLDER & "jump1" & EXT
    Private Const JUMP2 As String = FOLDER & "jump2" & EXT
    Private Shared JUMPS() As String = {JUMP1, JUMP2}
    Public Shared Function JUMP(ByRef rand As Random) As String
        Return JUMPS(rand.Next(JUMPS.Length))
    End Function

    'Marche normale
    Public Const WALK_NORMAL As String = FOLDER & "walk_normal" & EXT
    Public Const WALK_NORMAL_DURATION_MS As Integer = 161

    'Marche explorateur
    Public Const WALK_EXPLORER As String = FOLDER & "wolk_explorer" & EXT
    Public Const WALK_EXPLORER_DURATION_MS As Integer = 169

    'Fermer l'explorateur
    Public Const CLOSE_EXPLORER As String = FOLDER & "close_explorer" & EXT

    'Corriger un bug
    Public Const SOLVE_BUG As String = FOLDER & "solve_bug" & EXT

    'Mort
    Public Const DEATH As String = FOLDER & "death" & EXT

    'Tir d'un fichier corrompu
    Private Const CF_BULLET1 As String = FOLDER & "cf_bullet1" & EXT
    Private Const CF_BULLET2 As String = FOLDER & "cf_bullet2" & EXT
    Private Const CF_BULLET3 As String = FOLDER & "cf_bullet3" & EXT
    Private Shared CF_BULLETS() As String = {CF_BULLET1, CF_BULLET2, CF_BULLET3}
    Public Shared Function CF_BULLET(ByRef rand As Random) As String
        Return CF_BULLETS(rand.Next(CF_BULLETS.Length))
    End Function

    'Musique
    Public Const MUSIC As String = FOLDER & "music" & EXT
    Public Const MUSIC_DURATION_MS As Integer = 39419

End Class
