Option Explicit On
Option Infer On

Imports System.Management
Imports System.Runtime.InteropServices

Public Class BatteryInfo
    Private systemPowerStatus As PowerStatus
    Private searcherWMIBatteryInfo As ManagementObjectSearcher

    Public Sub New()
        systemPowerStatus = SystemInformation.PowerStatus
        searcherWMIBatteryInfo = New ManagementObjectSearcher("Select * FROM Win32_Battery")
    End Sub

    Public Function GetBatteryTimeRemaining() As String
        On Error Resume Next

        Dim nSeconds As Integer
        Dim iHours As Short = 0
        Dim iMinutes As Short = 0
        Dim iSeconds As Short = 0

        ' get the approximate amount of battery time remaining.
        If systemPowerStatus.BatteryLifeRemaining > 0 Then
            nSeconds = systemPowerStatus.BatteryLifeRemaining
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
        ElseIf systemPowerStatus.BatteryFullLifetime > 0 Then ' the full charge lifetime of the primary battery power source.
            nSeconds = systemPowerStatus.BatteryFullLifetime
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
        If systemPowerStatus.BatteryLifePercent > 1.0F Then
            Return "Calculating percentage..."
        Else
            Return Math.Round(systemPowerStatus.BatteryLifePercent * 100.0F, 0)
        End If
    End Function

    Public Function HasBattery() As Boolean
        If systemPowerStatus.BatteryChargeStatus = BatteryChargeStatus.NoSystemBattery Then
            Return False
        Else
            Return True
        End If
    End Function

    Public Function IsCharging() As Boolean
        If systemPowerStatus.PowerLineStatus = PowerLineStatus.Online Or
            systemPowerStatus.PowerLineStatus = PowerLineStatus.Unknown Then
            Return False
        Else
            Return True
        End If
    End Function

    Public Function GetBatteryStatus() As String
        'If system does not have a battery there is no point in obtaining the battery values
        If HasBattery() = False Then
            Return "No battery detected"
        End If

        Select Case systemPowerStatus.PowerLineStatus
            Case PowerLineStatus.Offline
                If systemPowerStatus.BatteryLifePercent > 1.0F Then
                    Return "Still calculating..."
                ElseIf GetBatteryTimeRemaining() = "" Then
                    Return GetBatteryPercentage() & "% remaining"
                Else
                    Return GetBatteryTimeRemaining() & $"({GetBatteryPercentage()}%) remaining"
                End If
            Case PowerLineStatus.Online
                Return "Plugged in"
            Case PowerLineStatus.Unknown
                Return "Status unknown"
            Case Else
                Return "Status unknown" 'This is just here to cover all of our paths.
        End Select
    End Function

    'Provides the health status of the battery. Return type string.
    Public Function GetHealthStatus() As String
        Try
            Return searcherWMIBatteryInfo.Get(0)("Status").ToString 'Ideally this will just return "OK".
        Catch ex As COMException
            Return "No access" 'Can trigger if the program is sandboxed by a firewall.
        Catch ex As Exception
            Return "Not supported"
        End Try
    End Function

    'Gets the battery weak if supported
    Public Function GetBatteryWear() As String
        If Not GetDesignCapacity() = 0 Then
            Return $"{Math.Round(GetFullChargeCapacity() / GetDesignCapacity() * 100, 0)}% of {GetDesignCapacity()}"
        Else
            Return "Not supported"
        End If
    End Function

    'Design capacity of the battery in milliwatt-hours. If the property is not supported, enter 0 (zero). Return type uint32.
    Private Function GetDesignCapacity() As Integer
        Try
            Return searcherWMIBatteryInfo.Get(0)("DesignCapacity").ToString
        Catch ex As Exception
            Return 0 'Can trigger if the program is sandboxed by a firewall.
        End Try
    End Function

    'Full charge capacity of the battery in milliwatt-hours. Comparison of the value to the DesignCapacity property 
    'determines when the battery requires replacement. A battery's end of life is typically when the FullChargeCapacity 
    'Property falls below 80% Of the DesignCapacity Property. If the Property Is Not supported, enter 0 (zero). 
    'Return type uint32.
    Private Function GetFullChargeCapacity() As Integer
        Try
            Return searcherWMIBatteryInfo.Get(0)("FullChargeCapacity").ToString
        Catch ex As Exception
            Return 0 'Can trigger if the program is sandboxed by a firewall.
        End Try
    End Function

    'Remaining time to charge the battery fully in minutes at the current charging rate and usage. Return type uint32.
    'If the Property Is Not supported, return -1. 
    Public Function GetTimeToFullCharge() As Integer
        Try
            Return searcherWMIBatteryInfo.Get(0)("TimeToFullCharge").ToString()
        Catch ex As Exception
            Return -1 'Can trigger if the program is sandboxed by a firewall.
        End Try
    End Function
End Class
