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

Imports System.Runtime.InteropServices

Public Class SimulateKeyPress

    'This method is useful to simulate Key presses to the window with focus.
    <DllImport("user32.dll", CallingConvention:=CallingConvention.StdCall, CharSet:=CharSet.Unicode, EntryPoint:="keybd_event",
           ExactSpelling:=True, SetLastError:=True)>
    Private Shared Sub keybd_event(ByVal bVk As Byte, ByVal bScan As Byte, ByVal dwFlags As Int32, ByVal dwExtraInfo As Int32)
    End Sub

    Private Const VK_STARTKEY = &H5B 'This is the hex character code for the start menu button.
    Private Const VK_M = &H4D 'This is the hex character code for the letter 'M' (ASCII 77).
    Private Const KEYEVENTF_KEYUP = &H2 'This is used to release a key, in this case, the start menu button.

    ''' <summary>
    ''' Minimizes all the windows on the desktop using the Windows key + M combination press.
    ''' </summary>
    Public Sub MinimizeAll()
        keybd_event(VK_STARTKEY, 0, 0, 0)
        keybd_event(VK_M, 0, 0, 0)
        keybd_event(VK_STARTKEY, 0, KEYEVENTF_KEYUP, 0)
    End Sub

End Class
