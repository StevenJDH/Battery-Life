Public Class VerticalProgressbar

    Private pValue As Single = 0
    Public ReadOnly Property Max As Integer = 100

    Public Sub Increment(value As Single)
        'TODO: create exception handler.
        Dim progress As Single

        progress = value + pValue

        If progress > Max Then
            pValue = Max
            pbRisingBase.Height = pbProgressColor.Height - (pValue * (pbProgressColor.Height / 100)) 'Proportional change, i.e. 200 would be 2.
        ElseIf Not value < 0 Then
            pValue = progress
            pbRisingBase.Height = pbProgressColor.Height - (pValue * (pbProgressColor.Height / 100)) 'Proportional change, i.e. 200 would be 2.
        End If
    End Sub

    Public Sub SetChargingColor(isCharing As Boolean)
        If isCharing = True Then
            pbProgressColor.BackColor = Color.DarkOrange
        Else
            pbProgressColor.BackColor = Color.DodgerBlue
        End If
    End Sub

    Private Sub VerticalProgressbar_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        pbRisingBase.Size = Me.Size
        pbProgressColor.Size = Me.Size
    End Sub

End Class
