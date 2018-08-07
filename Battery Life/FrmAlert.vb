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

Public Class FrmAlert

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Timer1.Enabled = False
        Me.Close()
    End Sub

    Private Sub FrmAlert_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Icon = FrmMain.Icon
        Timer1.Enabled = True
        Console.Beep(1000, 800)
        Label1.Text = "Your battery has " & FrmMain.alertTriggerLevel & "% left. Please plug it in."
    End Sub

    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If Label2.ForeColor = Color.Red Then
            Label2.ForeColor = Color.DarkOrange
        Else
            Label2.ForeColor = Color.Red
        End If
    End Sub

End Class