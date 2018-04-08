'This class Exposes the properties for the Win32 BATTERY_INFORMATION and BATTERY_STATUS structures.
Public Class Win32BatteryInformation

    'BATTERY_INFORMATION
    Public Property DesignedCapacity As Integer
    Public Property FullChargeCapacity As Integer
    Public Property CycleCount As Integer

    'BATTERY_STATUS
    Friend Property PowerState As Win32.POWER_STATE
    Public Property CurrentCapacity As UInteger
    Public Property Voltage As UInteger
    Public Property Rate As Integer

End Class
