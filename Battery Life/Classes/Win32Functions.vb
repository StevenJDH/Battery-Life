Imports System.IO
Imports System.Runtime.InteropServices

Public NotInheritable Class Win32Functions
    'Battery information access status used with above.
    Private Const STATUS_ACCESS_DENIED As UInteger = 3221225506
    'The 5 is for POWER_INFORMATION_LEVEL code 0x5 (which should be the index for system battery state).
    Private Const SYSTEM_BATTERYINFO As Integer = 5

    ''' <summary>
    ''' Provides additional battery information using unmanaged code.
    ''' </summary>
    ''' <returns>Object with access to battery information</returns>
    Friend Shared Function GetSystemBatteryState() As Win32.SYSTEM_BATTERY_STATE
        Dim status As IntPtr = IntPtr.Zero
        Dim batteryStatus As Win32.SYSTEM_BATTERY_STATE

        Try
            status = Marshal.AllocCoTaskMem(Marshal.SizeOf(GetType(Win32.SYSTEM_BATTERY_STATE)))
            Dim returnValue As UInteger = Win32.CallNtPowerInformation(SYSTEM_BATTERYINFO, Nothing, 0, status, Marshal.SizeOf(GetType(Win32.SYSTEM_BATTERY_STATE)))
            If returnValue = STATUS_ACCESS_DENIED Then
                Throw New UnauthorizedAccessException("Access to battery information is denied.")
            End If
            batteryStatus = Marshal.PtrToStructure(status, GetType(Win32.SYSTEM_BATTERY_STATE))
        Catch ex As Exception
            Throw
        Finally
            Marshal.FreeCoTaskMem(status)
        End Try

        Return batteryStatus
    End Function


    '----------------------------------------------------------------------------------------------------------------
    ' The below code was originally written by Andrew Hawker (ahawker on GitHub) that I ported to VB.Net.
    ' I've also added additional functionality that I needed to get more battery information such as the cycle count.
    '----------------------------------------------------------------------------------------------------------------


    Public Shared Function GetBatteryInformation() As Win32BatteryExtra
        Dim deviceDataPointer As IntPtr = IntPtr.Zero
        Dim queryInfoPointer As IntPtr = IntPtr.Zero
        Dim batteryInfoPointer As IntPtr = IntPtr.Zero
        Dim batteryWaitStatusPointer As IntPtr = IntPtr.Zero
        Dim batteryStatusPointer As IntPtr = IntPtr.Zero

        Try
            Dim deviceHandle As IntPtr = SetupDiGetClassDevs(Win32.GUID_DEVCLASS_BATTERY, Win32.DEVICE_GET_CLASS_FLAGS.DIGCF_PRESENT Or Win32.DEVICE_GET_CLASS_FLAGS.DIGCF_DEVICEINTERFACE)

            Dim deviceInterfaceData As New Win32.SP_DEVICE_INTERFACE_DATA()
            deviceInterfaceData.CbSize = Marshal.SizeOf(deviceInterfaceData)

            SetupDiEnumDeviceInterfaces(deviceHandle, Win32.GUID_DEVCLASS_BATTERY, 0, deviceInterfaceData)

            deviceDataPointer = Marshal.AllocHGlobal(Win32.DEVICE_INTERFACE_BUFFER_SIZE)
            'Win32.SP_DEVICE_INTERFACE_DETAIL_DATA deviceDetailData =
            '    (Win32.SP_DEVICE_INTERFACE_DETAIL_DATA)Marshal.PtrToStructure(deviceDataPointer, typeof(Win32.SP_DEVICE_INTERFACE_DETAIL_DATA));

            'toggle these two and see if anything changes... ^^^^^^^^^^^^
            Dim deviceDetailData As New Win32.SP_DEVICE_INTERFACE_DETAIL_DATA()
            deviceDetailData.CbSize = If((IntPtr.Size = 8), 8, 4 + Marshal.SystemDefaultCharSize)

            SetupDiGetDeviceInterfaceDetail(deviceHandle, deviceInterfaceData, deviceDetailData, Win32.DEVICE_INTERFACE_BUFFER_SIZE)

            Dim batteryHandle As IntPtr = CreateFile(deviceDetailData.DevicePath, FileAccess.ReadWrite, FileShare.ReadWrite, FileMode.Open, Win32.FILE_ATTRIBUTES.Normal)

            Dim queryInformation As New Win32.BATTERY_QUERY_INFORMATION()

            DeviceIoControl(batteryHandle, Win32.IOCTL_BATTERY_QUERY_TAG, queryInformation.BatteryTag)

            Dim batteryInformation As New Win32.BATTERY_INFORMATION()
            queryInformation.InformationLevel = Win32.BATTERY_QUERY_INFORMATION_LEVEL.BatteryInformation

            Dim queryInfoSize As Integer = Marshal.SizeOf(queryInformation)
            Dim batteryInfoSize As Integer = Marshal.SizeOf(batteryInformation)

            queryInfoPointer = Marshal.AllocHGlobal(queryInfoSize)
            Marshal.StructureToPtr(queryInformation, queryInfoPointer, False)

            batteryInfoPointer = Marshal.AllocHGlobal(batteryInfoSize)
            Marshal.StructureToPtr(batteryInformation, batteryInfoPointer, False)

            DeviceIoControl(batteryHandle, Win32.IOCTL_BATTERY_QUERY_INFORMATION, queryInfoPointer, queryInfoSize, batteryInfoPointer, batteryInfoSize)

            Dim updatedBatteryInformation As Win32.BATTERY_INFORMATION = CType(Marshal.PtrToStructure(batteryInfoPointer, GetType(Win32.BATTERY_INFORMATION)), Win32.BATTERY_INFORMATION)

            Dim batteryWaitStatus As New Win32.BATTERY_WAIT_STATUS()
            batteryWaitStatus.BatteryTag = queryInformation.BatteryTag

            Dim batteryStatus As New Win32.BATTERY_STATUS()

            Dim waitStatusSize As Integer = Marshal.SizeOf(batteryWaitStatus)
            Dim batteryStatusSize As Integer = Marshal.SizeOf(batteryStatus)

            batteryWaitStatusPointer = Marshal.AllocHGlobal(waitStatusSize)
            Marshal.StructureToPtr(batteryWaitStatus, batteryWaitStatusPointer, False)

            batteryStatusPointer = Marshal.AllocHGlobal(batteryStatusSize)
            Marshal.StructureToPtr(batteryStatus, batteryStatusPointer, False)

            DeviceIoControl(batteryHandle, Win32.IOCTL_BATTERY_QUERY_STATUS, batteryWaitStatusPointer, waitStatusSize, batteryStatusPointer, batteryStatusSize)

            Dim updatedStatus As Win32.BATTERY_STATUS = CType(Marshal.PtrToStructure(batteryStatusPointer, GetType(Win32.BATTERY_STATUS)), Win32.BATTERY_STATUS)

            Win32.SetupDiDestroyDeviceInfoList(deviceHandle)


            Return New Win32BatteryExtra() With {
                .DesignedCapacity = updatedBatteryInformation.DesignedCapacity,
                .FullChargeCapacity = updatedBatteryInformation.FullChargedCapacity,
                .CycleCount = updatedBatteryInformation.CycleCount,
                .PowerState = updatedStatus.PowerState,
                .CurrentCapacity = updatedStatus.Capacity,
                .Voltage = updatedStatus.Voltage,
                .Rate = updatedStatus.Rate
            }
        Finally
            Marshal.FreeHGlobal(deviceDataPointer)
            Marshal.FreeHGlobal(queryInfoPointer)
            Marshal.FreeHGlobal(batteryInfoPointer)
            Marshal.FreeHGlobal(batteryStatusPointer)
            Marshal.FreeHGlobal(batteryWaitStatusPointer)
        End Try
    End Function

    Private Shared Function DeviceIoControl(deviceHandle As IntPtr, controlCode As UInteger, ByRef output As UInteger) As Boolean
        Dim bytesReturned As UInteger
        Dim junkInput As UInteger = 0
        Dim retval As Boolean = Win32.DeviceIoControl(deviceHandle, controlCode, junkInput, 0, output, CType(Marshal.SizeOf(output), UInteger),
            bytesReturned, IntPtr.Zero)

        If Not retval Then
            Dim errorCode As Integer = Marshal.GetLastWin32Error()
            If errorCode <> 0 Then
                Throw Marshal.GetExceptionForHR(errorCode)
            Else
                Throw New Exception("DeviceIoControl call failed but Win32 didn't catch an error.")
            End If
        End If

        Return retval
    End Function

    Private Shared Function DeviceIoControl(deviceHandle As IntPtr, controlCode As UInteger, input As IntPtr, inputSize As Integer, output As IntPtr, outputSize As Integer) As Boolean
        Dim bytesReturned As UInteger
        Dim retval As Boolean = Win32.DeviceIoControl(deviceHandle, controlCode, input, CType(inputSize, UInteger), output, CType(outputSize, UInteger),
            bytesReturned, IntPtr.Zero)

        If Not retval Then
            Dim errorCode As Integer = Marshal.GetLastWin32Error()
            If errorCode <> 0 Then
                Throw Marshal.GetExceptionForHR(errorCode)
            Else
                Throw New Exception("DeviceIoControl call failed but Win32 didn't catch an error.")
            End If
        End If

        Return retval
    End Function

    Private Shared Function SetupDiGetClassDevs(guid As Guid, flags As Win32.DEVICE_GET_CLASS_FLAGS) As IntPtr
        Dim handle As IntPtr = Win32.SetupDiGetClassDevs(guid, Nothing, IntPtr.Zero, flags)

        If handle = IntPtr.Zero OrElse handle.ToInt64() = -1 Then
            Dim errorCode As Integer = Marshal.GetLastWin32Error()
            If errorCode <> 0 Then
                Throw Marshal.GetExceptionForHR(errorCode)
            Else
                Throw New Exception("SetupDiGetClassDev call returned a bad handle.")
            End If
        End If
        Return handle
    End Function

    Private Shared Function SetupDiEnumDeviceInterfaces(deviceInfoSet As IntPtr, guid As Guid, memberIndex As Integer, ByRef deviceInterfaceData As Win32.SP_DEVICE_INTERFACE_DATA) As Boolean
        Dim retval As Boolean = Win32.SetupDiEnumDeviceInterfaces(deviceInfoSet, IntPtr.Zero, guid, CType(memberIndex, UInteger), deviceInterfaceData)

        If Not retval Then
            Dim errorCode As Integer = Marshal.GetLastWin32Error()
            If errorCode <> 0 Then
                If errorCode = 259 Then
                    Throw New Exception("SetupDeviceInfoEnumerateDeviceInterfaces ran out of batteries to enumerate.")
                End If

                Throw Marshal.GetExceptionForHR(errorCode)
            Else
                Throw New Exception("SetupDeviceInfoEnumerateDeviceInterfaces call failed but Win32 didn't catch an error.")
            End If
        End If
        Return retval
    End Function

    Private Shared Function SetupDiDestroyDeviceInfoList(deviceInfoSet As IntPtr) As Boolean
        Dim retval As Boolean = Win32.SetupDiDestroyDeviceInfoList(deviceInfoSet)

        If Not retval Then
            Dim errorCode As Integer = Marshal.GetLastWin32Error()
            If errorCode <> 0 Then
                Throw Marshal.GetExceptionForHR(errorCode)
            Else
                Throw New Exception("SetupDiDestroyDeviceInfoList call failed but Win32 didn't catch an error.")
            End If
        End If
        Return retval
    End Function

    Private Shared Function SetupDiGetDeviceInterfaceDetail(deviceInfoSet As IntPtr, ByRef deviceInterfaceData As Win32.SP_DEVICE_INTERFACE_DATA, ByRef deviceInterfaceDetailData As Win32.SP_DEVICE_INTERFACE_DETAIL_DATA, deviceInterfaceDetailSize As Integer) As Boolean
        'int tmpSize = Marshal.SizeOf(deviceInterfaceDetailData);
        Dim reqSize As UInteger
        Dim retval As Boolean = Win32.SetupDiGetDeviceInterfaceDetail(deviceInfoSet, deviceInterfaceData, deviceInterfaceDetailData, CType(deviceInterfaceDetailSize, UInteger), reqSize, IntPtr.Zero)

        If Not retval Then
            Dim errorCode As Integer = Marshal.GetLastWin32Error()
            If errorCode <> 0 Then
                Throw Marshal.GetExceptionForHR(errorCode)
            Else
                Throw New Exception("SetupDiGetDeviceInterfaceDetail call failed but Win32 didn't catch an error.")
            End If
        End If
        Return retval
    End Function

    Private Shared Function CreateFile(filename As String, access As FileAccess, shareMode As FileShare, creation As FileMode, flags As Win32.FILE_ATTRIBUTES) As IntPtr
        Dim handle As IntPtr = Win32.CreateFile(filename, access, shareMode, IntPtr.Zero, creation, flags,
            IntPtr.Zero)

        If handle = IntPtr.Zero OrElse handle.ToInt32() = -1 Then
            Dim errorCode As Integer = Marshal.GetLastWin32Error()
            If errorCode <> 0 Then
                Marshal.ThrowExceptionForHR(errorCode)
            Else
                Throw New Exception("SetupDiGetDeviceInterfaceDetail call failed but Win32 didn't catch an error.")
            End If
        End If
        Return handle
    End Function

End Class