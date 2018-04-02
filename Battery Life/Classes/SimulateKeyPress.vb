Option Explicit On
Option Infer On

Imports System.Runtime.InteropServices

Public Class SimulateKeyPress

    'This method is useful to simulate Key presses to the window with focus.
    <DllImport("user32.dll", CallingConvention:=CallingConvention.StdCall, CharSet:=CharSet.Unicode, EntryPoint:="keybd_event",
           ExactSpelling:=True, SetLastError:=True)>
    Public Shared Sub keybd_event(ByVal bVk As Byte, ByVal bScan As Byte, ByVal dwFlags As Int32, ByVal dwExtraInfo As Int32)
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
