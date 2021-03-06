﻿' This file is part of Battery Life <https://github.com/StevenJDH/Battery-Life>.
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
Partial Class VerticalProgressbar
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.pbRisingBase = New System.Windows.Forms.PictureBox()
        Me.pbProgressColor = New System.Windows.Forms.PictureBox()
        CType(Me.pbRisingBase, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbProgressColor, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pbRisingBase
        '
        Me.pbRisingBase.BackColor = System.Drawing.SystemColors.ControlLight
        Me.pbRisingBase.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pbRisingBase.Location = New System.Drawing.Point(0, 0)
        Me.pbRisingBase.Name = "pbRisingBase"
        Me.pbRisingBase.Size = New System.Drawing.Size(20, 150)
        Me.pbRisingBase.TabIndex = 3
        Me.pbRisingBase.TabStop = False
        '
        'pbProgressColor
        '
        Me.pbProgressColor.BackColor = System.Drawing.Color.DodgerBlue
        Me.pbProgressColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pbProgressColor.Location = New System.Drawing.Point(0, 0)
        Me.pbProgressColor.Name = "pbProgressColor"
        Me.pbProgressColor.Size = New System.Drawing.Size(20, 150)
        Me.pbProgressColor.TabIndex = 4
        Me.pbProgressColor.TabStop = False
        '
        'VerticalProgressbar
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.pbRisingBase)
        Me.Controls.Add(Me.pbProgressColor)
        Me.Name = "VerticalProgressbar"
        Me.Size = New System.Drawing.Size(20, 150)
        CType(Me.pbRisingBase, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbProgressColor, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pbRisingBase As PictureBox
    Friend WithEvents pbProgressColor As PictureBox
End Class
