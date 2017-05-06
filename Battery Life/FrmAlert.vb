Public Class FrmAlert

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Timer1.Enabled = False
        Me.Close()
    End Sub

    Private Sub FrmAlert_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Icon = FrmMain.Icon
        Timer1.Enabled = True
        Console.Beep(1000, 800)
        Label1.Text = "Your battery has " & FrmMain.nTrigger & "% left. Please plug it in."
    End Sub

    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If Label2.ForeColor = Color.Red Then
            Label2.ForeColor = Color.DarkOrange
        Else
            Label2.ForeColor = Color.Red
        End If
    End Sub

End Class