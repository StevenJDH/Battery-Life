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
Partial Class FrmPopUp
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.lblPercentage = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblDischargeCharge = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.lblTimeRemaining = New System.Windows.Forms.Label()
        Me.lblCapacity = New System.Windows.Forms.Label()
        Me.lblRate = New System.Windows.Forms.Label()
        Me.lblHealthStatus = New System.Windows.Forms.Label()
        Me.lblWear = New System.Windows.Forms.Label()
        Me.VerticalProgressbar1 = New Battery_Life.VerticalProgressbar()
        Me.lblCycles = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.lblElapsedTime = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(80, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(264, 32)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Battery Life v2.0.0 Beta"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.Battery_Life.My.Resources.Resources.mini_icon
        Me.PictureBox1.Location = New System.Drawing.Point(48, 16)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(32, 32)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 6
        Me.PictureBox1.TabStop = False
        '
        'lblPercentage
        '
        Me.lblPercentage.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPercentage.Location = New System.Drawing.Point(168, 56)
        Me.lblPercentage.Name = "lblPercentage"
        Me.lblPercentage.Size = New System.Drawing.Size(176, 16)
        Me.lblPercentage.TabIndex = 7
        Me.lblPercentage.Text = "0%"
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(48, 56)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(120, 16)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Percent:"
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(48, 72)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(120, 16)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "Capacity:"
        '
        'lblDischargeCharge
        '
        Me.lblDischargeCharge.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDischargeCharge.Location = New System.Drawing.Point(48, 88)
        Me.lblDischargeCharge.Name = "lblDischargeCharge"
        Me.lblDischargeCharge.Size = New System.Drawing.Size(120, 16)
        Me.lblDischargeCharge.TabIndex = 10
        Me.lblDischargeCharge.Text = "Discharge Rate:"
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(48, 104)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(120, 16)
        Me.Label5.TabIndex = 11
        Me.Label5.Text = "Time Remaining:"
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(48, 136)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(120, 16)
        Me.Label6.TabIndex = 12
        Me.Label6.Text = "Health Status:"
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(48, 168)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(120, 16)
        Me.Label7.TabIndex = 13
        Me.Label7.Text = "Battery Wear:"
        '
        'lblTimeRemaining
        '
        Me.lblTimeRemaining.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTimeRemaining.Location = New System.Drawing.Point(168, 104)
        Me.lblTimeRemaining.Name = "lblTimeRemaining"
        Me.lblTimeRemaining.Size = New System.Drawing.Size(176, 16)
        Me.lblTimeRemaining.TabIndex = 14
        Me.lblTimeRemaining.Text = "0"
        '
        'lblCapacity
        '
        Me.lblCapacity.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCapacity.Location = New System.Drawing.Point(168, 72)
        Me.lblCapacity.Name = "lblCapacity"
        Me.lblCapacity.Size = New System.Drawing.Size(176, 16)
        Me.lblCapacity.TabIndex = 15
        Me.lblCapacity.Text = "0"
        '
        'lblRate
        '
        Me.lblRate.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRate.Location = New System.Drawing.Point(168, 88)
        Me.lblRate.Name = "lblRate"
        Me.lblRate.Size = New System.Drawing.Size(176, 16)
        Me.lblRate.TabIndex = 16
        Me.lblRate.Text = "0"
        '
        'lblHealthStatus
        '
        Me.lblHealthStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHealthStatus.Location = New System.Drawing.Point(168, 136)
        Me.lblHealthStatus.Name = "lblHealthStatus"
        Me.lblHealthStatus.Size = New System.Drawing.Size(176, 16)
        Me.lblHealthStatus.TabIndex = 17
        Me.lblHealthStatus.Text = "N/A"
        '
        'lblWear
        '
        Me.lblWear.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWear.Location = New System.Drawing.Point(168, 168)
        Me.lblWear.Name = "lblWear"
        Me.lblWear.Size = New System.Drawing.Size(176, 16)
        Me.lblWear.TabIndex = 18
        Me.lblWear.Text = "0"
        '
        'VerticalProgressbar1
        '
        Me.VerticalProgressbar1.Location = New System.Drawing.Point(16, 16)
        Me.VerticalProgressbar1.Name = "VerticalProgressbar1"
        Me.VerticalProgressbar1.Size = New System.Drawing.Size(24, 168)
        Me.VerticalProgressbar1.TabIndex = 4
        '
        'lblCycles
        '
        Me.lblCycles.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCycles.Location = New System.Drawing.Point(168, 152)
        Me.lblCycles.Name = "lblCycles"
        Me.lblCycles.Size = New System.Drawing.Size(176, 16)
        Me.lblCycles.TabIndex = 20
        Me.lblCycles.Text = "0"
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(48, 152)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(120, 16)
        Me.Label8.TabIndex = 19
        Me.Label8.Text = "Cycle Count:"
        '
        'lblElapsedTime
        '
        Me.lblElapsedTime.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblElapsedTime.Location = New System.Drawing.Point(168, 120)
        Me.lblElapsedTime.Name = "lblElapsedTime"
        Me.lblElapsedTime.Size = New System.Drawing.Size(176, 16)
        Me.lblElapsedTime.TabIndex = 22
        Me.lblElapsedTime.Text = "0"
        '
        'Label9
        '
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(48, 120)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(120, 16)
        Me.Label9.TabIndex = 21
        Me.Label9.Text = "Elapsed Time:"
        '
        'FrmPopUp
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(353, 201)
        Me.Controls.Add(Me.lblElapsedTime)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.lblCycles)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.lblWear)
        Me.Controls.Add(Me.lblHealthStatus)
        Me.Controls.Add(Me.lblRate)
        Me.Controls.Add(Me.lblCapacity)
        Me.Controls.Add(Me.lblTimeRemaining)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.lblDischargeCharge)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.lblPercentage)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.VerticalProgressbar1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "FrmPopUp"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.TopMost = True
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents VerticalProgressbar1 As VerticalProgressbar
    Friend WithEvents Label1 As Label
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents lblPercentage As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents lblDischargeCharge As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents lblTimeRemaining As Label
    Friend WithEvents lblCapacity As Label
    Friend WithEvents lblRate As Label
    Friend WithEvents lblHealthStatus As Label
    Friend WithEvents lblWear As Label
    Friend WithEvents lblCycles As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents lblElapsedTime As Label
    Friend WithEvents Label9 As Label
End Class
