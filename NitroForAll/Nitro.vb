Imports System.Drawing
Imports GTA
Imports Metadata

Public Class Nitro
    Inherits Script

    Public Sub New()
        PP = Game.Player.Character
        LV = Game.Player.Character.LastVehicle

        Decor.Unlock()
        Decor.Register(modDecor, Decor.eDecorType.Int)
        Decor.Register(helpDecor, Decor.eDecorType.Bool)
        Decor.Register(stageDecor, Decor.eDecorType.Int)
        Decor.Lock()
    End Sub

    Private Sub Nitro_Tick(sender As Object, e As EventArgs) Handles Me.Tick
        If Not Decor.Registered(modDecor, Decor.eDecorType.Int) Then
            Decor.Unlock()
            Decor.Register(modDecor, Decor.eDecorType.Int)
            Decor.Lock()
        End If

        If Not Decor.Registered(stageDecor, Decor.eDecorType.Int) Then
            Decor.Unlock()
            Decor.Register(stageDecor, Decor.eDecorType.Int)
            Decor.Lock()
        End If

        If Not Decor.Registered(helpDecor, Decor.eDecorType.Bool) Then
            Decor.Unlock()
            Decor.Register(helpDecor, Decor.eDecorType.Bool)
            Decor.Lock()
        End If

        PP = Game.Player.Character
        LV = Game.Player.Character.LastVehicle

        If LV.GetInt(modDecor) >= 1 Then
            If Not LV.GetBool(helpDecor) AndAlso GetInteriorID(PP.Position) = 0 Then
                DisplayHelpTextThisFrame(Game.GetGXTEntry("AVMH_NITROS").Replace("~INPUT_VEH_BIKE_WINGS~", "~INPUT_VEH_DUCK~"))
                LV.SetBool(helpDecor, True)
            End If

            If Not PP.IsInVehicle(LV) Then
                nitroTimer = 0F
            End If

            If Game.IsControlJustPressed(0, Control.VehicleDuck) AndAlso PP.IsInVehicle(LV) AndAlso IsRadarEnabled() Then
                If ListOfVeh.Contains(LV) Then
                    nitroTimer = 0F
                Else
                    nitroTimer = refillTimer
                End If
            End If

            If Not nitroTimer <= 0F Then
                refillTimer = nitroTimer
                LV.SetBoostActiveSound(True)
                LV.SetBoostActiveSound(False)
                If IsIktSpeedoModInstalled() Then
                    Decor.SetInt(LV, "ikt_speedo_nos", 1)
                    Decor.SetFloat(LV, "ikt_speedo_nos_level", refillTimer / maxNitro)
                Else
                    SetAbilityBar(refillTimer, maxNitro)
                    NewFunc.SetNitroHudActive(True)
                End If
                If Not ListOfVeh.Contains(LV) AndAlso LV.CanInstallNitroMod Then
                    LV.SetNitroEnabled(True, 0F, 0F, 0F, True)
                    ListOfVeh.Add(LV)
                End If
                Select Case LV.GetInt(modDecor)
                    Case 1
                        LV.EngineTorqueMultiplier = 2.0F
                        LV.EnginePowerMultiplier = 2.0F
                        If Not LV.GetInt(stageDecor) = 1 Then LV.SetInt(stageDecor, 1)
                    Case 2
                        LV.EngineTorqueMultiplier = 5.0F
                        LV.EnginePowerMultiplier = 5.0F
                        If Not LV.GetInt(stageDecor) = 2 Then LV.SetInt(stageDecor, 2)
                    Case 3
                        LV.EngineTorqueMultiplier = 8.0F
                        LV.EnginePowerMultiplier = 8.0F
                        If Not LV.GetInt(stageDecor) = 3 Then LV.SetInt(stageDecor, 3)
                End Select
            Else
                If Not LV.IsDead Then
                    If Not refillTimer >= maxNitro Then refillTimer += 0.005F
                End If
                If PP.IsInVehicle(LV) Then
                    If IsIktSpeedoModInstalled() Then
                        Decor.SetInt(LV, "ikt_speedo_nos", 1)
                        Decor.SetFloat(LV, "ikt_speedo_nos_level", refillTimer / maxNitro)
                    Else
                        SetAbilityBar(refillTimer, maxNitro)
                        NewFunc.SetNitroHudActive(True)
                    End If
                End If
                If ListOfVeh.Contains(LV) Then
                    LV.SetNitroEnabled(False)
                    LV.EngineTorqueMultiplier = 1.0F
                    LV.EnginePowerMultiplier = 1.0F
                    ListOfVeh.Remove(LV)
                End If
            End If
        Else
            If ListOfVeh.Contains(LV) Then
                LV.SetNitroEnabled(False)
                LV.SetBool(helpDecor, False)
                ListOfVeh.Remove(LV)
                nitroTimer = 0F
                LV.EngineTorqueMultiplier = 1.0F
                LV.EnginePowerMultiplier = 1.0F
            End If

            If IsIktSpeedoModInstalled() Then
                Decor.SetInt(LV, "ikt_speedo_nos", 0)
                Decor.SetFloat(LV, "ikt_speedo_nos_level", 0F)
            End If
        End If

        If PP.IsInVehicle(LV) Then
            If ListOfVeh.Contains(LV) Then
                If Not nitroTimer <= 0F Then nitroTimer -= 0.01F
            End If
        Else
            NewFunc.SetNitroHudActive(False)
        End If

        If NewFunc.IsCheating("add nitro 1") Then
            LV.SetInt(modDecor, 1)
        ElseIf NewFunc.IsCheating("add nitro 2") Then
            LV.SetInt(modDecor, 2)
        ElseIf NewFunc.IsCheating("add nitro 3") Then
            LV.SetInt(modDecor, 3)
        End If

        If NewFunc.IsCheating("remove nitro") Then
            LV.SetInt(modDecor, 0)
            LV.SetBool(helpDecor, False)
        End If

        If NewFunc.IsCheating("unlimited nitro 1") Then
            maxNitro = Single.MaxValue
            refillTimer = maxNitro
        ElseIf NewFunc.IsCheating("unlimited nitro 0") Then
            maxNitro = 3.0F
            refillTimer = maxNitro
        End If
    End Sub

    Private Sub Nitro_Aborted(sender As Object, e As EventArgs) Handles Me.Aborted

    End Sub
End Class
