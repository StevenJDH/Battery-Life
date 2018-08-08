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

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmAbout
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmAbout))
        Me.groupBox1 = New System.Windows.Forms.GroupBox()
        Me.rtxtLicense = New System.Windows.Forms.RichTextBox()
        Me.lblTitleVer = New System.Windows.Forms.Label()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.pnlButtonImage = New System.Windows.Forms.Panel()
        Me.lblButton = New System.Windows.Forms.Label()
        Me.label2 = New System.Windows.Forms.Label()
        Me.label3 = New System.Windows.Forms.Label()
        Me.lnkGitHub = New System.Windows.Forms.LinkLabel()
        Me.label4 = New System.Windows.Forms.Label()
        Me.groupBox1.SuspendLayout()
        Me.pnlButtonImage.SuspendLayout()
        Me.SuspendLayout()
        '
        'groupBox1
        '
        Me.groupBox1.Controls.Add(Me.rtxtLicense)
        Me.groupBox1.Location = New System.Drawing.Point(8, 120)
        Me.groupBox1.Name = "groupBox1"
        Me.groupBox1.Size = New System.Drawing.Size(592, 168)
        Me.groupBox1.TabIndex = 11
        Me.groupBox1.TabStop = False
        Me.groupBox1.Text = "GNU General Public License"
        '
        'rtxtLicense
        '
        Me.rtxtLicense.BackColor = System.Drawing.Color.White
        Me.rtxtLicense.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.rtxtLicense.Font = New System.Drawing.Font("Courier New", 8.25!)
        Me.rtxtLicense.Location = New System.Drawing.Point(8, 16)
        Me.rtxtLicense.Name = "rtxtLicense"
        Me.rtxtLicense.ReadOnly = True
        Me.rtxtLicense.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical
        Me.rtxtLicense.Size = New System.Drawing.Size(576, 144)
        Me.rtxtLicense.TabIndex = 5
        Me.rtxtLicense.Text = resources.GetString("rtxtLicense.Text")
        '
        'lblTitleVer
        '
        Me.lblTitleVer.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold)
        Me.lblTitleVer.Location = New System.Drawing.Point(8, 8)
        Me.lblTitleVer.Name = "lblTitleVer"
        Me.lblTitleVer.Size = New System.Drawing.Size(280, 16)
        Me.lblTitleVer.TabIndex = 10
        Me.lblTitleVer.Text = "Battery Life v0.0.0 Beta"
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(224, 296)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(160, 32)
        Me.btnOK.TabIndex = 9
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'pnlButtonImage
        '
        Me.pnlButtonImage.BackgroundImage = Global.Battery_Life.My.Resources.Resources.donation_button
        Me.pnlButtonImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.pnlButtonImage.Controls.Add(Me.lblButton)
        Me.pnlButtonImage.Cursor = System.Windows.Forms.Cursors.Hand
        Me.pnlButtonImage.Location = New System.Drawing.Point(8, 295)
        Me.pnlButtonImage.Name = "pnlButtonImage"
        Me.pnlButtonImage.Size = New System.Drawing.Size(112, 32)
        Me.pnlButtonImage.TabIndex = 16
        '
        'lblButton
        '
        Me.lblButton.BackColor = System.Drawing.Color.Orange
        Me.lblButton.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lblButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold)
        Me.lblButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblButton.Location = New System.Drawing.Point(8, 8)
        Me.lblButton.Name = "lblButton"
        Me.lblButton.Size = New System.Drawing.Size(96, 16)
        Me.lblButton.TabIndex = 12
        Me.lblButton.Text = "Donate..."
        Me.lblButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'label2
        '
        Me.label2.Location = New System.Drawing.Point(8, 24)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(280, 16)
        Me.label2.TabIndex = 15
        Me.label2.Text = "Copyright (C) 2018 Steven Jenkins De Haro"
        '
        'label3
        '
        Me.label3.Location = New System.Drawing.Point(8, 48)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(592, 32)
        Me.label3.TabIndex = 14
        Me.label3.Text = resources.GetString("label3.Text")
        '
        'lnkGitHub
        '
        Me.lnkGitHub.Location = New System.Drawing.Point(224, 88)
        Me.lnkGitHub.Name = "lnkGitHub"
        Me.lnkGitHub.Size = New System.Drawing.Size(232, 16)
        Me.lnkGitHub.TabIndex = 13
        Me.lnkGitHub.TabStop = True
        Me.lnkGitHub.Text = "https://github.com/StevenJDH/Battery-Life"
        '
        'label4
        '
        Me.label4.Location = New System.Drawing.Point(8, 88)
        Me.label4.Name = "label4"
        Me.label4.Size = New System.Drawing.Size(224, 20)
        Me.label4.TabIndex = 12
        Me.label4.Text = "VB.NET source code is available on GitHub: "
        '
        'FrmAbout
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(608, 337)
        Me.ControlBox = False
        Me.Controls.Add(Me.groupBox1)
        Me.Controls.Add(Me.lblTitleVer)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.pnlButtonImage)
        Me.Controls.Add(Me.label2)
        Me.Controls.Add(Me.label3)
        Me.Controls.Add(Me.lnkGitHub)
        Me.Controls.Add(Me.label4)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FrmAbout"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "About Battery Life"
        Me.groupBox1.ResumeLayout(False)
        Me.pnlButtonImage.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents groupBox1 As GroupBox
    Friend WithEvents rtxtLicense As RichTextBox
    Friend WithEvents lblTitleVer As Label
    Friend WithEvents btnOK As Button
    Friend WithEvents pnlButtonImage As Panel
    Friend WithEvents lblButton As Label
    Friend WithEvents label2 As Label
    Friend WithEvents label3 As Label
    Friend WithEvents lnkGitHub As LinkLabel
    Friend WithEvents label4 As Label
End Class
