Option Explicit On
Option Infer On

Public Class FrmMain

    Dim windowEffect As New SimulateKeyPress
    Friend alertTriggerLevel As Integer = 10 'Percent at which to start alerts
    Dim myBattery As New BatteryInfo
    Dim batteryAlert As AlertType = AlertType.MsgBoxAndBeep
    Dim hasAlertFired As Boolean = False

    Enum AlertType
        MsgBoxAndBeep = 0
        MsgBox = 1
        Beep = 2
    End Enum

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If RadioButton3.Checked = True Then
            If Not batteryAlert = AlertType.MsgBoxAndBeep Then
                batteryAlert = AlertType.MsgBoxAndBeep
                My.Computer.Registry.SetValue("HKEY_CURRENT_USER\Software\A Steve Creation\Battery Life",
                   "Alert", batteryAlert)
            End If
        ElseIf RadioButton2.Checked = True Then
            If Not batteryAlert = AlertType.Beep Then
                batteryAlert = AlertType.Beep
                My.Computer.Registry.SetValue("HKEY_CURRENT_USER\Software\A Steve Creation\Battery Life",
                   "Alert", batteryAlert)
            End If
        ElseIf RadioButton1.Checked = True Then
            If Not batteryAlert = AlertType.MsgBox Then
                batteryAlert = AlertType.MsgBox
                My.Computer.Registry.SetValue("HKEY_CURRENT_USER\Software\A Steve Creation\Battery Life",
                   "Alert", batteryAlert)
            End If
        End If

        If Not alertTriggerLevel = NumericUpDown1.Value Then
            alertTriggerLevel = NumericUpDown1.Value
            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\Software\A Steve Creation\Battery Life",
                "Trigger", alertTriggerLevel)
        End If
        Me.Hide()
    End Sub

    Private Sub FrmMain_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        NotifyIcon1.Dispose()
    End Sub

    Private Sub FrmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If My.Computer.Registry.GetValue("HKEY_CURRENT_USER\Software\A Steve Creation\Battery Life", "1run", Nothing) Is Nothing Then
            My.Computer.Registry.CurrentUser.CreateSubKey("Software\A Steve Creation\Battery Life")
            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\Software\A Steve Creation\Battery Life",
                    "1run", 1)
            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\Software\A Steve Creation\Battery Life",
                    "Alert", 0)
            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\Software\A Steve Creation\Battery Life",
                    "Trigger", 10)
            MsgBox("Looks like this is your first time using this program. The default settings will be used.", MsgBoxStyle.Information, "Battery Life")
            NotifyIcon1.Icon = Me.Icon
            NotifyIcon1.BalloonTipText = "Battery Life activated."
            NotifyIcon1.ShowBalloonTip(3000)
        Else
            batteryAlert = My.Computer.Registry.GetValue _
                ("HKEY_CURRENT_USER\Software\A Steve Creation\Battery Life", "Alert", 0)
            Select Case batteryAlert
                Case AlertType.MsgBoxAndBeep
                    RadioButton3.Checked = True
                Case AlertType.MsgBox
                    RadioButton1.Checked = True
                Case AlertType.Beep
                    RadioButton2.Checked = True
            End Select
            alertTriggerLevel = My.Computer.Registry.GetValue _
                ("HKEY_CURRENT_USER\Software\A Steve Creation\Battery Life", "Trigger", 10)
            NumericUpDown1.Value = alertTriggerLevel
            NotifyIcon1.Icon = Me.Icon
        End If
        Me.Hide()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        NotifyIcon1.Text = myBattery.GetBatteryStatus

        If alertTriggerLevel >= myBattery.GetBatteryPercentage() Then
            Select Case batteryAlert
                Case AlertType.MsgBoxAndBeep
                    Console.Beep()
                    If hasAlertFired = False Then
                        hasAlertFired = True
                        windowEffect.MinimizeAll()
                        Dim Frm As New FrmAlert
                        Frm.ShowDialog()
                        Frm.Dispose()
                        Frm = Nothing
                    End If
                Case AlertType.MsgBox
                    If hasAlertFired = False Then
                        hasAlertFired = True
                        windowEffect.MinimizeAll()
                        Dim Frm As New FrmAlert
                        Frm.ShowDialog()
                        Frm.Dispose()
                        Frm = Nothing
                    End If
                Case AlertType.Beep
                    Console.Beep()
            End Select
        End If

        'Resets our alert to be triggered again after a charge.
        If myBattery.IsCharging = True Then
            hasAlertFired = False
        End If
    End Sub

    Private Sub NotifyIcon1_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles NotifyIcon1.MouseDoubleClick
        Me.Show()
        Me.WindowState = FormWindowState.Normal
        'This is to bring it to front if already open but hidden behind other windows.
        Me.TopMost = True
        Me.TopMost = False
    End Sub

    Private Sub TestToolStripMenuItem1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles TestToolStripMenuItem1.Click
        Select Case batteryAlert
            Case AlertType.MsgBoxAndBeep
                Console.Beep()
                windowEffect.MinimizeAll()
                Dim Frm As New FrmAlert
                Frm.ShowDialog()
                Frm.Dispose()
                Frm = Nothing
            Case AlertType.MsgBox
                windowEffect.MinimizeAll()
                Dim Frm As New FrmAlert
                Frm.ShowDialog()
                Frm.Dispose()
                Frm = Nothing
            Case AlertType.Beep
                Console.Beep()
        End Select
    End Sub

    Private Sub Donate5PaypalToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Donate5PaypalToolStripMenuItem.Click
        On Error Resume Next
        Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=8493677")
    End Sub

    Private Sub AboutToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        MsgBox("Battery Life 1.1 (04-Apr-2018)" & vbNewLine & vbNewLine & "Author: Steven Jenkins De Haro" &
        vbNewLine & "A Steve Creation/Convergence" & vbNewLine & vbNewLine &
        "Microsoft .NET Framework 4.6.1", MsgBoxStyle.OkOnly, "Battery Life")
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

End Class