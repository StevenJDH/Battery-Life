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
