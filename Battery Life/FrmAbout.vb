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

Public Class FrmAbout

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        lblButton.BackColor = Color.Transparent
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        Me.Close()
    End Sub

    Private Sub lblButton_Click(sender As Object, e As EventArgs) Handles lblButton.Click
        On Error Resume Next
        ' Sends URL to the operating system for opening.
        Process.Start("https://www.paypal.me/stevenjdh")
    End Sub

    Private Sub pnlButtonImage_Click(sender As Object, e As EventArgs) Handles pnlButtonImage.Click
        lblButton_Click(Me, EventArgs.Empty)
    End Sub

    Private Sub FrmAbout_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim version As Version = Reflection.Assembly.GetExecutingAssembly().GetName().Version

        ' Automatically sets title and version information in label.
        lblTitleVer.Text = $"Battery Life v{version.Major}.{version.Minor}.{version.Build} Beta"

        ' We store the actual link this way in case we ever want to make changes to the link label.
        lnkGitHub.Links.Add(New LinkLabel.Link() With {.LinkData = "https://github.com/StevenJDH/Battery-Life"})
    End Sub

    Private Sub lnkGitHub_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnkGitHub.LinkClicked
        On Error Resume Next
        ' Casts URL back to string and sends it to the operating system for opening.
        Process.Start(e.Link.LinkData.ToString())
    End Sub

End Class