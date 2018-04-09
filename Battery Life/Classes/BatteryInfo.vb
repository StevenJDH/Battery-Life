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

    ''' <summary>
    ''' Gets the remaining time on the battery before a charge will be needed.
    ''' </summary>
    ''' <returns>Remaining battery runtime</returns>
    Public Function GetBatteryTimeRemaining() As String
        If systemPowerStatus.BatteryLifeRemaining > 0 Then
            Return GetTimeFromSeconds(systemPowerStatus.BatteryLifeRemaining)
        Else
            Return ""
        End If
    End Function

    'Issue with this is that it is returning -1
    Public Function GetFullRuntime() As String 'TODO: Replace this method with newer version that is always supported.
        If systemPowerStatus.BatteryFullLifetime > 0 Then ' The full charge lifetime of the primary battery power source.
            Return GetTimeFromSeconds(systemPowerStatus.BatteryFullLifetime)
        Else
            Return "Unknown"
        End If
    End Function

    Private Function GetTimeFromSeconds(seconds As Integer) As String
        Dim ts As TimeSpan

        ts = New TimeSpan(0, 0, seconds)
        If Int(ts.Days) > 0 Then
            Return $"{ts.Days}d {ts.Hours}h {ts.Minutes}m {ts.Seconds}s "
        ElseIf ts.Hours > 0 Then
            Return $"{ts.Hours}h {ts.Minutes}m {ts.Seconds}s "
        ElseIf ts.Minutes > 0 Then
            Return $"{ts.Minutes}m {ts.Seconds}s "
        Else
            Return $"{ts.Seconds}s "
        End If
    End Function

    Public Function GetBatteryPercentage() As String
        If systemPowerStatus.BatteryLifePercent > 1.0F Then
            Return "Calculating percentage..."
        Else
            Return Math.Round(systemPowerStatus.BatteryLifePercent * 100.0F, 0)
        End If
    End Function

    ''' <summary>
    ''' Checks to see if the computers has a battery or not.
    ''' </summary>
    ''' <returns>Batter presence state</returns>
    Public Function HasBattery() As Boolean
        If systemPowerStatus.BatteryChargeStatus = BatteryChargeStatus.NoSystemBattery Then
            Return False
        Else
            Return True
        End If
    End Function

    ''' <summary>
    ''' Checks to see if the battery is charging or not.
    ''' </summary>
    ''' <returns>Charging status</returns>
    Public Function IsCharging() As Boolean
        If systemPowerStatus.PowerLineStatus = PowerLineStatus.Online Or
            systemPowerStatus.PowerLineStatus = PowerLineStatus.Unknown Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' The current rate of charging/discharge of the battery. A nonzero, positive rate indicates charging. A negative 
    ''' rate indicates discharging. Some batteries report only discharging rates.
    ''' </summary>
    ''' <returns>Charging/discharging rate in mW</returns>
    Public Function GetRate() As Integer
        Try
            Dim batteryStatus As Win32.SYSTEM_BATTERY_STATE = Win32Functions.GetSystemBatteryState()
            Return batteryStatus.Rate.ToString()
        Catch ex As Exception
            Return 0
        End Try
    End Function

    ''' <summary>
    ''' Gets a summary of the battery state such as percentage and time remaining if not still being calculated.
    ''' </summary>
    ''' <returns>Summary of battery state</returns>
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

    ''' <summary>
    ''' Provides the health status of the battery.
    ''' </summary>
    ''' <returns>health status</returns>
    Public Function GetHealthStatus() As String
        Try
            Return searcherWMIBatteryInfo.Get(0)("Status").ToString 'Ideally this will just return "OK".
        Catch ex As COMException
            Return "No access" 'Can trigger if the program is sandboxed by a firewall.
        Catch ex As Exception
            Return "Not supported"
        End Try
    End Function

    ''' <summary>
    ''' Gets the battery's wear level percentage and the designed capacity.
    ''' </summary>
    ''' <returns>Wear information</returns>
    Public Function GetWear() As String
        Dim design As Integer = GetDesignedCapacity()
        Dim current As Integer = GetCurrentMaxCapacity()

        Return $"{Math.Round(((design - current) / design) * 100, 0)}% of {design.ToString("N0")} mWh"
    End Function

    ''' <summary>
    ''' The estimated remaining capacity of the battery.
    ''' </summary>
    ''' <returns>Remaining capacity of the battery in mWh</returns>
    Public Function GetRemainingCapacity() As UInteger
        Try
            Dim batteryStatus As Win32.SYSTEM_BATTERY_STATE = Win32Functions.GetSystemBatteryState()
            Return batteryStatus.RemainingCapacity
        Catch ex As Exception
            Return 0
        End Try
    End Function

    ''' <summary>
    ''' The current theoretical max capacity of the battery. This is not the same as the designed capacity.
    ''' </summary>
    ''' <returns>Max capacity of battery in mWh</returns>
    Public Function GetCurrentMaxCapacity() As UInteger
        Try
            Dim batteryStatus As Win32.SYSTEM_BATTERY_STATE = Win32Functions.GetSystemBatteryState()
            Return batteryStatus.MaxCapacity
        Catch ex As Exception
            Return 0
        End Try
    End Function

    ''' <summary>
    ''' Gets the designed capacity of the battery.
    ''' </summary>
    ''' <returns>Designed capacity in mWh</returns>
    Public Function GetDesignedCapacity() As Integer
        Try
            Dim batteryStatus As Win32BatteryExtra = Win32Functions.GetBatteryInformation()

            Return batteryStatus.DesignedCapacity
        Catch ex As Exception
            Return 0
        End Try
    End Function

    ''' <summary>
    ''' Gets the number of charge/discharge cycles the battery has experienced. This provides a means to determine the 
    ''' battery's wear. If the battery does not support a cycle counter, this member is zero.
    ''' </summary>
    ''' <returns>Cycle count</returns>
    Public Function GetCycleCount() As Integer
        Try
            Dim batteryStatus As Win32BatteryExtra = Win32Functions.GetBatteryInformation()

            Return batteryStatus.CycleCount
        Catch ex As Exception
            Return 0
        End Try
    End Function

    'Remaining time to charge the battery fully in minutes at the current charging rate and usage. Return type uint32.
    'If the Property Is Not supported, return -1. 
    Public Function GetTimeToFullCharge() As Integer 'TODO: Replace this method with newer version that is always supported.
        Try
            Return searcherWMIBatteryInfo.Get(0)("TimeToFullCharge").ToString()
        Catch ex As Exception
            Return -1 'Can trigger if the program is sandboxed by a firewall.
        End Try
    End Function
End Class
