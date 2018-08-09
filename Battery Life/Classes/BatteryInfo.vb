' This file is part of Battery Life <https://github.com/StevenJDH/Battery-Life>.
' Copyright (C) 2018 Steven Jenkins De Haro.
' 
' Battery Life is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' Battery Life is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU General Public License for more details.
'
' You should have received a copy of the GNU General Public License
' along with Battery Life.  If not, see <http://www.gnu.org/licenses/>.

Option Explicit On
Option Infer On

Imports System.Management
Imports System.Runtime.InteropServices
Imports Microsoft.Win32

Public Class BatteryInfo

    Private systemPowerStatus As PowerStatus
    Private searcherWMIBatteryInfo As ManagementObjectSearcher
    Private stopWatch As Stopwatch
    Private fullRuntime As TimeSpan
    Private sincePercent As Single
    Private Shared fInstance As BatteryInfo = Nothing

    Public Shared ReadOnly Property Instance()
        Get
            If (fInstance Is Nothing) Then
                fInstance = New BatteryInfo() 'Singleton pattern
            End If

            Return fInstance
        End Get
    End Property

    Private Sub New()
        systemPowerStatus = SystemInformation.PowerStatus
        searcherWMIBatteryInfo = New ManagementObjectSearcher("Select * FROM Win32_Battery")
        stopWatch = New Stopwatch()
        fullRuntime = New TimeSpan(0, 0, GetBatterySecondsRemaining())
        sincePercent = GetBatteryPercentage()
        stopWatch.Start()

        AddHandler SystemEvents.PowerModeChanged, New PowerModeChangedEventHandler(AddressOf SystemEvents_PowerModeChanged)
    End Sub

    ''' <summary>
    ''' Gets the formated remaining time on the battery before a charge will be needed.
    ''' </summary>
    ''' <returns>Remaining battery runtime</returns>
    Public Function GetBatteryTimeRemaining() As String
        Dim seconds As Integer = GetBatterySecondsRemaining()

        If Not seconds = -1 Then
            Return GetTimeFromSeconds(seconds)
        Else
            Return ""
        End If
    End Function

    ''' <summary>
    ''' Gets the remaining seconds on the battery before a charge will be needed.
    ''' </summary>
    ''' <returns>Remaining battery runtime in seconds</returns>
    Public Function GetBatterySecondsRemaining() As Integer
        If systemPowerStatus.BatteryLifeRemaining > 0 Then
            Return systemPowerStatus.BatteryLifeRemaining
        Else
            Return -1
        End If
    End Function

    'Issue with this is that it is returning -1
    'The system is only capable of estimating BatteryFullLifeTime based on calculations on BatteryLifeTime and BatteryLifePercent. 
    'Without smart battery subsystems, this value may Not be accurate enough To be useful.
    Public Function GetFullRuntime() As String 'TODO: Replace this method with newer version that is always supported.
        'Maybe just check to see if not charging and save the time remaining and the percent as a baseline for our full 
        'life. The Change power event would be good for this. Make sure to fire this once on load. Use a TimeSpane to get the
        'Elapsed Time where the save percent can also be shown.
        If systemPowerStatus.BatteryFullLifetime > 0 Then ' The full charge lifetime of the primary battery power source.
            Return GetTimeFromSeconds(systemPowerStatus.BatteryFullLifetime)
        Else
            Return "Unknown"
        End If
    End Function

    ''' <summary>
    ''' Gets the elapsed time since a particular percentage after switching between the charging or the 
    ''' discharging of the battery, or the loading of the application.
    ''' </summary>
    ''' <returns>Elapsed time from what percent</returns>
    Public Function GetElapsedTime() As String
        Dim ts As TimeSpan = stopWatch.Elapsed

        Return $"{ts.ToString("h\:mm")} (since {sincePercent}%)"
    End Function

    ''' <summary>
    ''' Indicates that a power status change has occurred, and therefore, will update the elapsed time and
    ''' from what percent that change occurred. For example, battery enters a critical state or when switching
    ''' between A/C power and battery power.
    ''' </summary>
    ''' <param name="sender">The sender of the event</param>
    ''' <param name="e">Power mode events reported by the operating system</param>
    Private Sub SystemEvents_PowerModeChanged(sender As Object, e As PowerModeChangedEventArgs)
        If e.Mode = PowerModes.StatusChange Then 'TODO: Event fires twice when raised. Build in a fix if this impacts code.
            sincePercent = GetBatteryPercentage()
            stopWatch.Restart()
        End If
    End Sub

    ''' <summary>
    ''' Provides a formated duration with estimated target time.
    ''' </summary>
    ''' <param name="seconds">Time in seconds</param>
    ''' <returns>Duration with target time estimate</returns>
    Private Function GetTimeFromSeconds(seconds As Integer) As String
        Dim ts As TimeSpan
        Dim currentTime As DateTime
        Dim targetTime As DateTime

        ts = New TimeSpan(0, 0, seconds)
        currentTime = DateTime.Now
        targetTime = currentTime.Add(ts)

        If Int(ts.Days) > 0 Then
            Return $"{ts.Days}d {ts.Hours}h {ts.Minutes}m {ts.Seconds}s @ {targetTime.ToString("h:mm tt")} "
        ElseIf ts.Hours > 0 Then
            Return $"{ts.Hours}h {ts.Minutes}m {ts.Seconds}s @ {targetTime.ToString("h:mm tt")} "
        ElseIf ts.Minutes > 0 Then
            Return $"{ts.Minutes}m {ts.Seconds}s @ {targetTime.ToString("h:mm tt")} "
        Else
            Return $"{ts.Seconds}s @ {targetTime.ToString("h:mm tt")} "
        End If
    End Function

    ''' <summary>
    ''' Gets the percentage of remaining battery life.
    ''' </summary>
    ''' <returns>Battery life percentage</returns>
    Public Function GetBatteryPercentage() As Single
        If systemPowerStatus.BatteryLifePercent > 1.0F Then
            Return -1
        Else
            Return Math.Round(systemPowerStatus.BatteryLifePercent * 100.0F, 1)
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
                    Return $"{GetBatteryPercentage()}% remaining"
                Else
                    Return $"{GetBatteryTimeRemaining()}({GetBatteryPercentage()}%) remaining"
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

End Class
