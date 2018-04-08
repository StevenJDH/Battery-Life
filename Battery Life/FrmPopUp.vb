Imports System.ComponentModel

Public Class FrmPopUp
    Private Sub FrmPopUp_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim x As Integer = Screen.PrimaryScreen.WorkingArea.Right - Me.Width - 5
        Dim y As Integer = Screen.PrimaryScreen.WorkingArea.Bottom - Me.Height - 2

        If Cursor.Position.X + Me.Width < Screen.PrimaryScreen.WorkingArea.Right - 5 Then
            x = Cursor.Position.X
        End If
        Me.Location = New Point(x, y)

        'TODO: Test code. 
        Dim myBattery As New BatteryInfo
        Dim percent As Integer
        'TODO: Improve return value logic.
        If Integer.TryParse(myBattery.GetBatteryPercentage, percent) = True Then
            VerticalProgressbar1.Increment(percent)
            lblPercentage.Text = $"{percent}%"
        End If
        lblCapacity.Text = $"{myBattery.GetRemainingCapacity.ToString("N0")} mWh of {myBattery.GetCurrentMaxCapacity.ToString("N0")} mWh"
        If myBattery.IsCharging = True Then
            lblDischargeCharge.Text = "Charge Rate:"
            lblTimeRemaining.Text = "Charging"
            VerticalProgressbar1.SetChargingColor(True)
        Else
            lblDischargeCharge.Text = "Discharge Rate:"
            lblTimeRemaining.Text = myBattery.GetBatteryTimeRemaining
            VerticalProgressbar1.SetChargingColor(False)
        End If
        lblRate.Text = $"{myBattery.GetRate.ToString("N0")} mW"
        lblHealthStatus.Text = myBattery.GetHealthStatus
        lblCycles.Text = myBattery.GetCycleCount
        lblWear.Text = myBattery.GetWear
    End Sub

    Private Sub FrmPopUp_Deactivate(sender As Object, e As EventArgs) Handles Me.Deactivate
        Me.Close()
    End Sub
End Class