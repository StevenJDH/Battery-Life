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

'This class Exposes the properties for the Win32 BATTERY_INFORMATION and BATTERY_STATUS structures.
Public Class Win32BatteryExtra

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
