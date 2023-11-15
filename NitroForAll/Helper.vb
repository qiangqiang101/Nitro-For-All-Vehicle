Imports System.Drawing
Imports System.IO
Imports System.Runtime.CompilerServices
Imports GTA
Imports GTA.Math
Imports GTA.Native
Imports Metadata

Module Helper

    'Memory
    Public ListOfVeh As New List(Of Vehicle)

    'Decor
    Public modDecor As String = "inm_nitro_active"
    Public helpDecor As String = "inm_nitro_help"
    Public stageDecor As String = "nfsnitro_stage"

    Public maxNitro As Single = 3.0F
    Public nitroTimer As Single = 0F
    Public refillTimer As Single = maxNitro

    Public PP As Ped
    Public LV As Vehicle

    <Extension>
    Public Function CanInstallNitroMod(v As Vehicle) As Boolean
        Dim result As Boolean = True
        If v.HasRocketBoost Then result = False
        If v.HasRam Then result = False
        If v.HasScoop Then result = False
        If v.HasSpike Then result = False
        Return result
    End Function

    Public Sub DisplayHelpTextThisFrame(helpText As String, Optional Shape As Integer = -1)
        Native.Function.Call(Hash._SET_TEXT_COMPONENT_FORMAT, "CELL_EMAIL_BCON")
        Const maxStringLength As Integer = 99

        Dim i As Integer = 0
        While i < helpText.Length
            Native.Function.Call(Hash._0x6C188BE134E074AA, helpText.Substring(i, System.Math.Min(maxStringLength, helpText.Length - i)))
            i += maxStringLength
        End While
        Native.Function.Call(Hash._DISPLAY_HELP_TEXT_FROM_STRING_LABEL, 0, 0, 1, Shape)
    End Sub

    Public Function GetInteriorID(interior As Vector3) As Integer
        Return Native.Function.Call(Of Integer)(Hash.GET_INTERIOR_AT_COORDS, interior.X, interior.Y, interior.Z)
    End Function

    Public Function IsAnimPostFxRunning(Optional crossline As String = "CrossLine") As Boolean
        Return Native.Function.Call(Of Boolean)(Hash._0x36AD3E690DA5ACEB, crossline)
    End Function

    Public Sub PlayAnimPostFX(Optional crossline As String = "CrossLine", Optional duration As Integer = 0, Optional [loop] As Boolean = True)
        Native.Function.Call(Hash._0x2206BF9A37B7F724, crossline, duration, [loop])
    End Sub

    Public Sub StopAnimPostFX(Optional crossline As String = "CrossLine")
        Native.Function.Call(Hash._0x068E835A1D0DC0E3, crossline)
    End Sub

    Public Sub SetAbilityBar(value As Single, max As Single)
        Native.Function.Call(Hash.SET_ABILITY_BAR_VALUE, value, max)
    End Sub

    Public Function IsRadarEnabled() As Boolean
        Return Native.Function.Call(Of Boolean)(Hash._0xAF754F20EB5CD51A)
    End Function

#Region "Ikt Speedo"

    Public Function IsIktSpeedoModInstalled() As Boolean
        Return Decor.Registered("ikt_speedo_active", Decor.eDecorType.Bool)
    End Function

#End Region

End Module
