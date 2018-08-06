Imports System.ComponentModel

Public Class FrmPopUp
    Private Sub FrmPopUp_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim x As Integer = Screen.PrimaryScreen.WorkingArea.Right - Me.Width - 5
        Dim y As Integer = Screen.PrimaryScreen.WorkingArea.Bottom - Me.Height - 2

        If Cursor.Position.X + Me.Width < Screen.PrimaryScreen.WorkingArea.Right - 5 Then
            x = Cursor.Position.X
        End If
        Me.Location = New Point(x, y)

        RefreshInfo()
    End Sub

    Private Sub RefreshInfo()
        'TODO: Fully test code.
        Dim myBattery As New BatteryInfo
        Dim percent As Single = myBattery.GetBatteryPercentage()

        VerticalProgressbar1.Increment(percent)
        lblPercentage.Text = $"{percent}%"

        lblCapacity.Text = $"{myBattery.GetRemainingCapacity.ToString("N0")} mWh of {myBattery.GetCurrentMaxCapacity.ToString("N0")} mWh"
        If myBattery.IsCharging = True Then
            lblDischargeCharge.Text = "Charge Rate:"
            lblTimeRemaining.Text = "Charging"
            VerticalProgressbar1.SetChargingColor(True)
        Else
            lblDischargeCharge.Text = "Discharge Rate:"
            Dim timeRemaning As String = myBattery.GetBatteryTimeRemaining
            If timeRemaning = "" Then
                lblTimeRemaining.Text = "Calculating"
            Else
                lblTimeRemaining.Text = timeRemaning
            End If
            VerticalProgressbar1.SetChargingColor(False)
        End If

        Dim rate As Integer = myBattery.GetRate
        lblRate.Text = If(Not rate = 0, $"{rate.ToString("N0")} mW", "Not supported")

        lblHealthStatus.Text = myBattery.GetHealthStatus

        Dim cycleCount As Integer = myBattery.GetCycleCount
        lblCycles.Text = If(Not cycleCount = 0, cycleCount, "Not supported")

        lblWear.Text = myBattery.GetWear
    End Sub

    Private Sub FrmPopUp_Deactivate(sender As Object, e As EventArgs) Handles Me.Deactivate
        Me.Close()
    End Sub
End Class