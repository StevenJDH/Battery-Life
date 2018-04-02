Option Explicit On
Option Infer On

Public Class BatteryInfo

    Public Property SystemPowerStatus As PowerStatus

    Public Sub New()
        SystemPowerStatus = SystemInformation.PowerStatus
    End Sub

    Public Function GetBatteryTimeRemaining() As String
        On Error Resume Next

        Dim nSeconds As Integer
        Dim iHours As Short = 0
        Dim iMinutes As Short = 0
        Dim iSeconds As Short = 0

        ' get the approximate amount of battery time remaining.
        If SystemPowerStatus.BatteryLifeRemaining > 0 Then
            nSeconds = SystemPowerStatus.BatteryLifeRemaining
            If nSeconds < 1 Then
                Return ""
            End If
            iSeconds = nSeconds - Int(nSeconds / 60) * 60
            iMinutes = Int((nSeconds - Int(nSeconds / 3600) * 3600) / 60)
            iHours = Int(nSeconds / 3600)
            If Int(iHours) > 24 Then
                Return "Over a day left "
            Else
                If iHours < 1 Then
                    If iMinutes < 1 Then
                        Return CStr(iSeconds) & "s "
                    Else
                        Return CStr(iMinutes) & "m " & CStr(iSeconds) & "s "
                    End If
                Else
                    Return CStr(iHours) & "h " & CStr(iMinutes) & "m " & CStr(iSeconds) & "s "
                End If
            End If
        ElseIf SystemPowerStatus.BatteryFullLifetime > 0 Then ' the full charge lifetime of the primary battery power source.
            nSeconds = SystemPowerStatus.BatteryFullLifetime
            If nSeconds < 1 Then
                Return ""
            End If
            iSeconds = nSeconds - Int(nSeconds / 60) * 60
            iMinutes = Int((nSeconds - Int(nSeconds / 3600) * 3600) / 60)
            iHours = Int(nSeconds / 3600)
            If Int(iHours) > 24 Then
                Return "Over a day left "
            Else
                If iHours < 1 Then
                    If iMinutes < 1 Then
                        Return CStr(iSeconds) & "s "
                    Else
                        Return CStr(iMinutes) & "m " & CStr(iSeconds) & "s "
                    End If
                Else
                    Return CStr(iHours) & "h " & CStr(iMinutes) & "m " & CStr(iSeconds) & "s "
                End If
            End If
        Else
            ' if the battery is fully charged (ie. 1), what happens is that BatteryLifeRemaining returns 0.
            ' I suspect it's a bug OR the battery needs to be discharged a little so the OS can estimate\
            ' power use, and therefore the time the battery will provide for that given power use.
            ' and for the second part, some systems (HP TC1100) will not provide the full charge lifetime of the battery.
            Return ""
        End If
    End Function

    Public Function GetBatteryPercentage() As String
        If SystemPowerStatus.BatteryLifePercent > 1.0F Then
            Return "Calculating percentage..."
        Else
            Return Math.Round(SystemPowerStatus.BatteryLifePercent * 100.0F, 0)
        End If
    End Function

End Class
