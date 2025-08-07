<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form3
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.FlowLayoutPanel_SubNavigation = New System.Windows.Forms.FlowLayoutPanel()
        Me.Guna2Panel1 = New Guna.UI2.WinForms.Guna2Panel()
        Me.Label_UserEmail = New System.Windows.Forms.Label()
        Me.Label_UserName = New System.Windows.Forms.Label()
        Me.Button_Logout = New Guna.UI2.WinForms.Guna2Button()
        Me.FlowLayoutPanel_SubNavigation.SuspendLayout
        Me.Guna2Panel1.SuspendLayout
        Me.SuspendLayout
        '
        'FlowLayoutPanel_SubNavigation
        '
        Me.FlowLayoutPanel_SubNavigation.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(72,Byte),Integer), CType(CType(101,Byte),Integer))
        Me.FlowLayoutPanel_SubNavigation.Controls.Add(Me.Guna2Panel1)
        Me.FlowLayoutPanel_SubNavigation.Controls.Add(Me.Button_Logout)
        Me.FlowLayoutPanel_SubNavigation.Location = New System.Drawing.Point(0, 0)
        Me.FlowLayoutPanel_SubNavigation.Margin = New System.Windows.Forms.Padding(0)
        Me.FlowLayoutPanel_SubNavigation.Name = "FlowLayoutPanel_SubNavigation"
        Me.FlowLayoutPanel_SubNavigation.Size = New System.Drawing.Size(1162, 63)
        Me.FlowLayoutPanel_SubNavigation.TabIndex = 10
        '
        'Guna2Panel1
        '
        Me.Guna2Panel1.Controls.Add(Me.Label_UserEmail)
        Me.Guna2Panel1.Controls.Add(Me.Label_UserName)
        Me.Guna2Panel1.Location = New System.Drawing.Point(12, 0)
        Me.Guna2Panel1.Margin = New System.Windows.Forms.Padding(12, 0, 0, 0)
        Me.Guna2Panel1.Name = "Guna2Panel1"
        Me.Guna2Panel1.Size = New System.Drawing.Size(1083, 63)
        Me.Guna2Panel1.TabIndex = 20
        '
        'Label_UserEmail
        '
        Me.Label_UserEmail.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.Label_UserEmail.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Me.Label_UserEmail.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Me.Label_UserEmail.ForeColor = System.Drawing.Color.WhiteSmoke
        Me.Label_UserEmail.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label_UserEmail.Location = New System.Drawing.Point(226, 35)
        Me.Label_UserEmail.Margin = New System.Windows.Forms.Padding(12, 0, 12, 12)
        Me.Label_UserEmail.Name = "Label_UserEmail"
        Me.Label_UserEmail.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Label_UserEmail.Size = New System.Drawing.Size(857, 16)
        Me.Label_UserEmail.TabIndex = 19
        Me.Label_UserEmail.Text = "email"
        Me.Label_UserEmail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label_UserName
        '
        Me.Label_UserName.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Me.Label_UserName.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Me.Label_UserName.ForeColor = System.Drawing.Color.WhiteSmoke
        Me.Label_UserName.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label_UserName.Location = New System.Drawing.Point(226, 12)
        Me.Label_UserName.Margin = New System.Windows.Forms.Padding(12, 12, 12, 6)
        Me.Label_UserName.Name = "Label_UserName"
        Me.Label_UserName.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Label_UserName.Size = New System.Drawing.Size(857, 18)
        Me.Label_UserName.TabIndex = 18
        Me.Label_UserName.Text = "Fullname"
        Me.Label_UserName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Button_Logout
        '
        Me.Button_Logout.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.Button_Logout.BackgroundImage = Global.ERP_SIMS.My.Resources.Resources.Button_Logout
        Me.Button_Logout.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Me.Button_Logout.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Me.Button_Logout.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer))
        Me.Button_Logout.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer))
        Me.Button_Logout.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Me.Button_Logout.Font = New System.Drawing.Font("Segoe UI", 9!)
        Me.Button_Logout.ForeColor = System.Drawing.Color.White
        Me.Button_Logout.Location = New System.Drawing.Point(1111, 19)
        Me.Button_Logout.Margin = New System.Windows.Forms.Padding(16, 0, 0, 0)
        Me.Button_Logout.Name = "Button_Logout"
        Me.Button_Logout.Size = New System.Drawing.Size(24, 24)
        Me.Button_Logout.TabIndex = 24
        '
        'Form3
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1384, 800)
        Me.Controls.Add(Me.FlowLayoutPanel_SubNavigation)
        Me.Name = "Form3"
        Me.Text = "Form3"
        Me.FlowLayoutPanel_SubNavigation.ResumeLayout(false)
        Me.Guna2Panel1.ResumeLayout(false)
        Me.ResumeLayout(false)

End Sub

    Friend WithEvents FlowLayoutPanel_SubNavigation As FlowLayoutPanel
    Friend WithEvents Guna2Panel1 As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents Label_UserEmail As Label
    Friend WithEvents Label_UserName As Label
    Friend WithEvents Button_Logout As Guna.UI2.WinForms.Guna2Button
End Class
