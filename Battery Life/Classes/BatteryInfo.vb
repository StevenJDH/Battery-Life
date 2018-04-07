Option Explicit On
Option Infer On

Imports System.Management
Imports System.Runtime.InteropServices

Public Class BatteryInfo
    'This method is used to get the extra battery details.
    <DllImport("powrprof.dll", SetLastError:=True)>
    Private Shared Function CallNtPowerInformation(InformationLevel As Int32, lpInputBuffer As IntPtr,
                                                   nInputBufferSize As UInt32, lpOutputBuffer As IntPtr,
                                                   nOutputBufferSize As UInt32) As UInt32
    End Function
    'This is used with powrprof.dll to get additional battery information.
    Structure SYSTEM_BATTERY_STATE
        Public AcOnLine As Byte
        Public BatteryPresent As Byte
        Public Charging As Byte
        Public Discharging As Byte
        Public spare1 As Byte
        Public spare2 As Byte
        Public spare3 As Byte
        Public spare4 As Byte
        Public MaxCapacity As UInt32
        Public RemainingCapacity As UInt32
        Public Rate As Int32
        Public EstimatedTime As UInt32
        Public DefaultAlert1 As UInt32
        Public DefaultAlert2 As UInt32
    End Structure

    Private systemPowerStatus As PowerStatus
    Private searcherWMIBatteryInfo As ManagementObjectSearcher

    Public Sub New()
        systemPowerStatus = SystemInformation.PowerStatus
        searcherWMIBatteryInfo = New ManagementObjectSearcher("Select * FROM Win32_Battery")
    End Sub

    Public Function GetBatteryTimeRemaining() As String
        Dim ts As TimeSpan

        If systemPowerStatus.BatteryLifeRemaining > 0 Then
            ts = New TimeSpan(0, 0, systemPowerStatus.BatteryLifeRemaining)
        ElseIf systemPowerStatus.BatteryFullLifetime > 0 Then ' The full charge lifetime of the primary battery power source.
            ts = New TimeSpan(0, 0, systemPowerStatus.BatteryFullLifetime)
        Else
            ' If the battery is fully charged (ie. 1), what happens is that BatteryLifeRemaining returns 0.
            ' I suspect it's a bug OR the battery needs to be discharged a little so the OS can estimate\
            ' power use, and therefore the time the battery will provide for that given power use.
            ' And for the second part, some systems (HP TC1100) will not provide the full charge lifetime of the battery.
            Return ""
        End If

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

    'The current rate of charging/discharge of the battery, in mW. A nonzero, positive rate indicates charging; a negative rate 
    'indicates discharging. Some batteries report only discharging rates. This value should be treated As a Long As it can 
    'contain negative values (With the high bit Set).
    Public Function Rate() As Long 'TODO: Finish writing this method.
        Dim status As IntPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(GetType(SYSTEM_BATTERY_STATE)))
        'The 5 is for POWER_INFORMATION_LEVEL code 0x5 (which should be the index for system battery state).
        Dim retval As UInteger = CallNtPowerInformation(5, CType(Nothing, IntPtr), 0, status, CType(Marshal.SizeOf(GetType(SYSTEM_BATTERY_STATE)), UInt32))
        Dim batteryStatus As SYSTEM_BATTERY_STATE = CType(Marshal.PtrToStructure(status, GetType(SYSTEM_BATTERY_STATE)), SYSTEM_BATTERY_STATE)


        'Alternative way just to get the discharge rate:
        'Dim powerCounter As New PerformanceCounter("Power Meter", "Power", "Power Meter (0)", True)
        'Return powerCounter.NextValue().ToString()

        Return batteryStatus.Rate.ToString()
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

    'Gets the battery wear if supported
    Public Function GetBatteryWear() As String 'TODO: Replace this method with newer version that is always supported.
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
