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

Imports System.IO
Imports System.Runtime.InteropServices

Public NotInheritable Class Win32
    'This method is used to get the extra battery details.
    <DllImport("powrprof.dll", SetLastError:=True)>
    Friend Shared Function CallNtPowerInformation(InformationLevel As Int32, lpInputBuffer As IntPtr,
                                                   nInputBufferSize As UInt32, lpOutputBuffer As IntPtr,
                                                   nOutputBufferSize As UInt32) As UInt32
    End Function
    'This is used with powrprof.dll to get additional battery information.
    <StructLayout(LayoutKind.Sequential)>
    Friend Structure SYSTEM_BATTERY_STATE
        <MarshalAs(UnmanagedType.I1)>
        Public AcOnLine As Boolean
        <MarshalAs(UnmanagedType.I1)>
        Public BatteryPresent As Boolean
        <MarshalAs(UnmanagedType.I1)>
        Public Charging As Boolean
        <MarshalAs(UnmanagedType.I1)>
        Public Discharging As Boolean
        Public spare1 As Byte
        Public spare2 As Byte
        Public spare3 As Byte
        Public spare4 As Byte
        Public MaxCapacity As UInteger
        Public RemainingCapacity As UInteger
        Public Rate As Integer
        Public EstimatedTime As UInteger
        Public DefaultAlert1 As UInteger
        Public DefaultAlert2 As UInteger
    End Structure



    '----------------------------------------------------------------------------------------------------------------
    ' The below code was originally written by Andrew Hawker (ahawker on GitHub) that I ported to VB.Net.
    ' I've also added additional functionality that I needed to get more battery information such as the cycle count.
    '----------------------------------------------------------------------------------------------------------------


    Friend Shared ReadOnly GUID_DEVCLASS_BATTERY As New Guid(&H72631E54, &H78A4, &H11D0, &HBC, &HF7, &H0,
        &HAA, &H0, &HB7, &HB3, &H2A)
    Friend Const IOCTL_BATTERY_QUERY_TAG As UInteger = (&H29 << 16) Or (CType(FileAccess.Read, Integer) << 14) Or (&H10 << 2) Or (0)
    Friend Const IOCTL_BATTERY_QUERY_INFORMATION As UInteger = (&H29 << 16) Or (CType(FileAccess.Read, Integer) << 14) Or (&H11 << 2) Or (0)
    Friend Const IOCTL_BATTERY_QUERY_STATUS As UInteger = (&H29 << 16) Or (CType(FileAccess.Read, Integer) << 14) Or (&H13 << 2) Or (0)

    Friend Const DEVICE_INTERFACE_BUFFER_SIZE As Integer = 120


    <DllImport("setupapi.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
    Friend Shared Function SetupDiGetClassDevs(ByRef guid As Guid, <MarshalAs(UnmanagedType.LPTStr)> enumerator As String,
                                               hwnd As IntPtr, flags As DEVICE_GET_CLASS_FLAGS) As IntPtr
    End Function

    <DllImport("setupapi.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
    Friend Shared Function SetupDiDestroyDeviceInfoList(deviceInfoSet As IntPtr) As Boolean
    End Function

    <DllImport("setupapi.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
    Friend Shared Function SetupDiEnumDeviceInterfaces(hdevInfo As IntPtr, devInfo As IntPtr, ByRef guid As Guid,
                                                       memberIndex As UInteger,
                                                       ByRef devInterfaceData As SP_DEVICE_INTERFACE_DATA) As Boolean
    End Function

    <DllImport("setupapi.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
    Friend Shared Function SetupDiGetDeviceInterfaceDetail(hdevInfo As IntPtr,
                                                           ByRef deviceInterfaceData As SP_DEVICE_INTERFACE_DATA,
                                                           ByRef deviceInterfaceDetailData As SP_DEVICE_INTERFACE_DETAIL_DATA, deviceInterfaceDetailDataSize As UInteger, ByRef requiredSize As UInteger, deviceInfoData As IntPtr) As Boolean
    End Function

    <DllImport("setupapi.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
    Friend Shared Function SetupDiGetDeviceInterfaceDetail(hdevInfo As IntPtr,
                                                           ByRef deviceInterfaceData As SP_DEVICE_INTERFACE_DATA,
                                                           deviceInterfaceDetailData As IntPtr,
                                                           deviceInterfaceDetailDataSize As UInteger,
                                                           ByRef requiredSize As UInteger, deviceInfoData As IntPtr) As Boolean
    End Function

    <DllImport("kernel32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
    Friend Shared Function CreateFile(filename As String, <MarshalAs(UnmanagedType.U4)> desiredAccess As FileAccess,
                                      <MarshalAs(UnmanagedType.U4)> shareMode As FileShare, securityAttributes As IntPtr,
                                      <MarshalAs(UnmanagedType.U4)> creationDisposition As FileMode,
                                      <MarshalAs(UnmanagedType.U4)> flags As FILE_ATTRIBUTES,
                                      template As IntPtr) As IntPtr
    End Function

    <DllImport("kernel32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
    Friend Shared Function DeviceIoControl(handle As IntPtr, controlCode As UInteger, <[In]> inBuffer As IntPtr,
                                           inBufferSize As UInteger, <Out> outBuffer As IntPtr, outBufferSize As UInteger,
                                           ByRef bytesReturned As UInteger, overlapped As IntPtr) As Boolean
    End Function

    <DllImport("kernel32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
    Friend Shared Function DeviceIoControl(handle As IntPtr, controlCode As UInteger, ByRef inBuffer As UInteger,
                                           inBufferSize As UInteger, ByRef outBuffer As UInteger, outBufferSize As UInteger,
                                           ByRef bytesReturned As UInteger, overlapped As IntPtr) As Boolean
    End Function

    <Flags>
    Friend Enum DEVICE_GET_CLASS_FLAGS As UInteger
        DIGCF_DEFAULT = &H1
        DIGCF_PRESENT = &H2
        DIGCF_ALLCLASSES = &H4
        DIGCF_PROFILE = &H8
        DIGCF_DEVICEINTERFACE = &H10
    End Enum

    <Flags>
    Friend Enum LOCAL_MEMORY_FLAGS
        LMEM_FIXED = &H0
        LMEM_MOVEABLE = &H2
        LMEM_NOCOMPACT = &H10
        LMEM_NODISCARD = &H20
        LMEM_ZEROINIT = &H40
        LMEM_MODIFY = &H80
        LMEM_DISCARDABLE = &HF00
        LMEM_VALID_FLAGS = &HF72
        LMEM_INVALID_HANDLE = &H8000
        LHND = (LMEM_MOVEABLE Or LMEM_ZEROINIT)
        LPTR = (LMEM_FIXED Or LMEM_ZEROINIT)
        NONZEROLHND = (LMEM_MOVEABLE)
        NONZEROLPTR = (LMEM_FIXED)
    End Enum

    <Flags>
    Friend Enum FILE_ATTRIBUTES As UInteger
        [Readonly] = &H1
        Hidden = &H2
        System = &H4
        Directory = &H10
        Archive = &H20
        Device = &H40
        Normal = &H80
        Temporary = &H100
        SparseFile = &H200
        ReparsePoint = &H400
        Compressed = &H800
        Offline = &H1000
        NotContentIndexed = &H2000
        Encrypted = &H4000
        Write_Through = &H80000000UI
        Overlapped = &H40000000
        NoBuffering = &H20000000
        RandomAccess = &H10000000
        SequentialScan = &H8000000
        DeleteOnClose = &H4000000
        BackupSemantics = &H2000000
        PosixSemantics = &H1000000
        OpenReparsePoint = &H200000
        OpenNoRecall = &H100000
        FirstPipeInstance = &H80000
    End Enum

    Friend Enum BATTERY_QUERY_INFORMATION_LEVEL
        BatteryInformation = 0
        BatteryGranularityInformation = 1
        BatteryTemperature = 2
        BatteryEstimatedTime = 3
        BatteryDeviceName = 4
        BatteryManufactureDate = 5
        BatteryManufactureName = 6
        BatteryUniqueID = 7
    End Enum

    <Flags>
    Friend Enum POWER_STATE As UInteger
        BATTERY_POWER_ONLINE = &H1
        BATTERY_DISCHARGING = &H2
        BATTERY_CHARGING = &H4
        BATTERY_CRITICAL = &H8
    End Enum

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)>
    Friend Structure BATTERY_INFORMATION
        Public Capabilities As Integer
        Public Technology As Byte

        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=3)>
        Public Reserved As Byte()
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=4)>
        Public Chemistry As Byte()

        Public DesignedCapacity As Integer
        Public FullChargedCapacity As Integer
        Public DefaultAlert1 As Integer
        Public DefaultAlert2 As Integer
        Public CriticalBias As Integer
        Public CycleCount As Integer
    End Structure

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)>
    Friend Structure SP_DEVICE_INTERFACE_DETAIL_DATA
        Public CbSize As Integer
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=256)>
        Public DevicePath As String
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Friend Structure SP_DEVICE_INTERFACE_DATA
        Public CbSize As Integer
        Public InterfaceClassGuid As Guid
        Public Flags As Integer
        Public Reserved As UIntPtr
    End Structure

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)>
    Friend Structure BATTERY_QUERY_INFORMATION
        Public BatteryTag As UInteger
        Public InformationLevel As BATTERY_QUERY_INFORMATION_LEVEL
        Public AtRate As Integer
    End Structure

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)>
    Friend Structure BATTERY_STATUS
        Public PowerState As POWER_STATE
        Public Capacity As UInteger
        Public Voltage As UInteger
        Public Rate As Integer
    End Structure

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)>
    Friend Structure BATTERY_WAIT_STATUS
        Public BatteryTag As UInteger
        Public Timeout As UInteger
        Public PowerState As POWER_STATE
        Public LowCapacity As UInteger
        Public HighCapacity As UInteger
    End Structure
End Class
