Option Explicit On
Option Infer On

Public Class FrmMain
    Public Declare Sub keybd_event Lib "user32.dll" (ByVal bVk As Byte, ByVal bScan As Byte, ByVal dwFlags As Int32, ByVal dwExtraInfo As Int32)

    Dim MySystemPowerStatus As System.Windows.Forms.PowerStatus = SystemInformation.PowerStatus
    Dim nAlert As Integer = 0 '0=Both 1=MsgBox 2=Beep
    Friend nTrigger As Integer = 10 'Percent at which to start alerts

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If RadioButton3.Checked = True Then
            If Not nAlert = 0 Then
                nAlert = 0
                My.Computer.Registry.SetValue("HKEY_CURRENT_USER\Software\A Steve Creation\Battery Life", _
                   "Alert", nAlert)
            End If
        ElseIf RadioButton2.Checked = True Then
            If Not nAlert = 2 Then
                nAlert = 2
                My.Computer.Registry.SetValue("HKEY_CURRENT_USER\Software\A Steve Creation\Battery Life", _
                   "Alert", nAlert)
            End If
        ElseIf RadioButton1.Checked = True Then
            If Not nAlert = 1 Then
                nAlert = 1
                My.Computer.Registry.SetValue("HKEY_CURRENT_USER\Software\A Steve Creation\Battery Life", _
                   "Alert", nAlert)
            End If
        End If

        If Not nTrigger = NumericUpDown1.Value Then
            nTrigger = NumericUpDown1.Value
            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\Software\A Steve Creation\Battery Life", _
                "Trigger", nTrigger)
        End If
        Me.Hide()
    End Sub

    Private Sub FrmMain_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        NotifyIcon1.Dispose()
    End Sub

    Private Sub FrmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If My.Computer.Registry.GetValue("HKEY_CURRENT_USER\Software\A Steve Creation\Battery Life", "1run", Nothing) Is Nothing Then
            My.Computer.Registry.CurrentUser.CreateSubKey("Software\A Steve Creation\Battery Life")
            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\Software\A Steve Creation\Battery Life", _
                    "1run", 1)
            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\Software\A Steve Creation\Battery Life", _
                    "Alert", 0)
            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\Software\A Steve Creation\Battery Life", _
                    "Trigger", 10)
            MsgBox("Looks like this is your first time using this program. The default settings will be used.", MsgBoxStyle.Information, "Battery Life")
            NotifyIcon1.Icon = Me.Icon
            NotifyIcon1.BalloonTipText = "Battery Life activated."
            NotifyIcon1.ShowBalloonTip(3000)
        Else
            nAlert = My.Computer.Registry.GetValue _
                ("HKEY_CURRENT_USER\Software\A Steve Creation\Battery Life", "Alert", 0)
            Select Case nAlert
                Case 0
                    RadioButton3.Checked = True
                Case 1
                    RadioButton1.Checked = True
                Case 2
                    RadioButton2.Checked = True
            End Select
            nTrigger = My.Computer.Registry.GetValue _
                ("HKEY_CURRENT_USER\Software\A Steve Creation\Battery Life", "Trigger", 10)
            NumericUpDown1.Value = nTrigger
            NotifyIcon1.Icon = Me.Icon
        End If
        Me.Hide()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        ' if system does not have a battery there is no point in obtaining the battery values
        If MySystemPowerStatus.BatteryChargeStatus = BatteryChargeStatus.NoSystemBattery Then
            NotifyIcon1.Text = "No battery detected"
        Else
            Select Case MySystemPowerStatus.PowerLineStatus
                Case PowerLineStatus.Offline
                    If MySystemPowerStatus.BatteryLifePercent > 100 Then
                        NotifyIcon1.Text = "Still calculating..."
                    Else
                        If GetBatTime() = "" Then
                            NotifyIcon1.Text = GetBatPercent() & "% remaining"
                        Else
                            NotifyIcon1.Text = GetBatTime() & "(" & GetBatPercent() & "%) remaining"
                        End If
                        If nTrigger >= GetBatPercent() Then
                            Select Case nAlert
                                Case 0
                                    Console.Beep()
                                    If Timer1.Tag = "" Then
                                        Timer1.Tag = "1"
                                        MinimizeAll()
                                        Dim Frm As New FrmAlert
                                        Frm.ShowDialog()
                                        Frm.Dispose()
                                        Frm = Nothing
                                    End If
                                Case 1
                                    If Timer1.Tag = "" Then
                                        Timer1.Tag = "1"
                                        MinimizeAll()
                                        Dim Frm As New FrmAlert
                                        Frm.ShowDialog()
                                        Frm.Dispose()
                                        Frm = Nothing
                                    End If
                                Case 2
                                    Console.Beep()
                            End Select
                        End If
                    End If
                Case PowerLineStatus.Online
                    Timer1.Tag = ""
                    NotifyIcon1.Text = "Plugged In"
                Case PowerLineStatus.Unknown
                    Timer1.Tag = ""
                    NotifyIcon1.Text = "Status unknown"
            End Select
        End If
    End Sub

    Private Function GetBatTime() As String
        On Error Resume Next
        Dim nSeconds As Integer
        Dim iHours As Short = 0
        Dim iMinutes As Short = 0
        Dim iSeconds As Short = 0
        ' get the approximate amount of battery time remaining.
        If MySystemPowerStatus.BatteryLifeRemaining > 0 Then
            nSeconds = MySystemPowerStatus.BatteryLifeRemaining
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
        ElseIf MySystemPowerStatus.BatteryFullLifetime > 0 Then ' the full charge lifetime of the primary battery power source.
            nSeconds = MySystemPowerStatus.BatteryFullLifetime
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

    Private Function GetBatPercent() As String
        If MySystemPowerStatus.BatteryLifePercent > 100 Then
            Return "Calculating percentage..."
        Else
            Return Math.Round(CDec(Replace(MySystemPowerStatus.BatteryLifePercent.ToString("p"), " %", "")), 0)
        End If
    End Function

    Private Sub NotifyIcon1_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles NotifyIcon1.MouseDoubleClick
        Me.Show()
        Me.WindowState = FormWindowState.Normal
        'This is to bring it to front if already open but hidden behind other windows.
        Me.TopMost = True
        Me.TopMost = False
    End Sub

    Private Sub TestToolStripMenuItem1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles TestToolStripMenuItem1.Click
        Select Case nAlert
            Case 0
                Console.Beep()
                MinimizeAll()
                Dim Frm As New FrmAlert
                Frm.ShowDialog()
                Frm.Dispose()
                Frm = Nothing
            Case 1
                MinimizeAll()
                Dim Frm As New FrmAlert
                Frm.ShowDialog()
                Frm.Dispose()
                Frm = Nothing
            Case 2
                Console.Beep()
        End Select
    End Sub

    Private Sub Donate5PaypalToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Donate5PaypalToolStripMenuItem.Click
        On Error Resume Next
        Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=8493677")
    End Sub

    Private Sub AboutToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        MsgBox("Battery Life 1.0 (12/02/2010)" & vbNewLine & vbNewLine & "Author: Steven Jenkins De Haro" & _
        vbNewLine & "A Steve Creation/Convergence" & vbNewLine & vbNewLine & _
        "Microsoft .NET Framework 3.5", MsgBoxStyle.OkOnly, "Battery Life")
    End Sub

    Private Sub SettingsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SettingsToolStripMenuItem.Click
        Me.Show()
        Me.WindowState = FormWindowState.Normal
        'This is to bring it to front if already open but hidden behind other windows.
        Me.TopMost = True
        Me.TopMost = False
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Close()
    End Sub

    Public Sub MinimizeAll()
        keybd_event(&H5B, 0, 0, 0)
        keybd_event(&H4D, 0, 0, 0)
        keybd_event(&H5B, 0, &H2, 0)
    End Sub

End Class