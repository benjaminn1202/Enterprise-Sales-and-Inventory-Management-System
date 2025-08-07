<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Window_Login
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
        Me.components = New System.ComponentModel.Container()
        Me.Panel_LoginForm = New Guna.UI2.WinForms.Guna2Panel()
        Me.Button_Login = New Guna.UI2.WinForms.Guna2Button()
        Me.FlowLayoutPanel_LoginShowPassword = New System.Windows.Forms.FlowLayoutPanel()
        Me.Checkbox_LoginShowPassword = New Guna.UI2.WinForms.Guna2CustomCheckBox()
        Me.Label_LoginShowPassword = New System.Windows.Forms.Label()
        Me.FlowLayoutPanel_LoginInputPassword = New System.Windows.Forms.FlowLayoutPanel()
        Me.Label_LoginInputPassword = New System.Windows.Forms.Label()
        Me.Textbox_LoginInputPassword = New Guna.UI2.WinForms.Guna2TextBox()
        Me.FlowLayoutPanel_LoginInputEmail = New System.Windows.Forms.FlowLayoutPanel()
        Me.Label_LoginInputEmail = New System.Windows.Forms.Label()
        Me.Textbox_LoginInputEmail = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Label_LoginFormSubHeader = New System.Windows.Forms.Label()
        Me.Label_LoginForHeader = New System.Windows.Forms.Label()
        Me.Guna2BorderlessForm1 = New Guna.UI2.WinForms.Guna2BorderlessForm(Me.components)
        Me.ProgressBar_Loading = New System.Windows.Forms.ProgressBar()
        Me.Panel_LoginForm.SuspendLayout
        Me.FlowLayoutPanel_LoginShowPassword.SuspendLayout
        Me.FlowLayoutPanel_LoginInputPassword.SuspendLayout
        Me.FlowLayoutPanel_LoginInputEmail.SuspendLayout
        Me.SuspendLayout
        '
        'Panel_LoginForm
        '
        Me.Panel_LoginForm.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Me.Panel_LoginForm.Controls.Add(Me.Button_Login)
        Me.Panel_LoginForm.Controls.Add(Me.FlowLayoutPanel_LoginShowPassword)
        Me.Panel_LoginForm.Controls.Add(Me.FlowLayoutPanel_LoginInputPassword)
        Me.Panel_LoginForm.Controls.Add(Me.FlowLayoutPanel_LoginInputEmail)
        Me.Panel_LoginForm.Controls.Add(Me.Label_LoginFormSubHeader)
        Me.Panel_LoginForm.Controls.Add(Me.Label_LoginForHeader)
        Me.Panel_LoginForm.CustomizableEdges.BottomLeft = false
        Me.Panel_LoginForm.CustomizableEdges.BottomRight = false
        Me.Panel_LoginForm.CustomizableEdges.TopLeft = false
        Me.Panel_LoginForm.CustomizableEdges.TopRight = false
        Me.Panel_LoginForm.Location = New System.Drawing.Point(64, 147)
        Me.Panel_LoginForm.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel_LoginForm.Name = "Panel_LoginForm"
        Me.Panel_LoginForm.Size = New System.Drawing.Size(484, 506)
        Me.Panel_LoginForm.TabIndex = 0
        '
        'Button_Login
        '
        Me.Button_Login.BackgroundImage = Global.ERP_SIMS.My.Resources.Resources.Button_Login
        Me.Button_Login.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Me.Button_Login.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Me.Button_Login.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer))
        Me.Button_Login.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer))
        Me.Button_Login.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Me.Button_Login.Font = New System.Drawing.Font("Segoe UI", 9!)
        Me.Button_Login.ForeColor = System.Drawing.Color.White
        Me.Button_Login.Location = New System.Drawing.Point(32, 404)
        Me.Button_Login.Margin = New System.Windows.Forms.Padding(0)
        Me.Button_Login.Name = "Button_Login"
        Me.Button_Login.Size = New System.Drawing.Size(420, 40)
        Me.Button_Login.TabIndex = 1
        '
        'FlowLayoutPanel_LoginShowPassword
        '
        Me.FlowLayoutPanel_LoginShowPassword.Controls.Add(Me.Checkbox_LoginShowPassword)
        Me.FlowLayoutPanel_LoginShowPassword.Controls.Add(Me.Label_LoginShowPassword)
        Me.FlowLayoutPanel_LoginShowPassword.Location = New System.Drawing.Point(32, 346)
        Me.FlowLayoutPanel_LoginShowPassword.Margin = New System.Windows.Forms.Padding(0)
        Me.FlowLayoutPanel_LoginShowPassword.Name = "FlowLayoutPanel_LoginShowPassword"
        Me.FlowLayoutPanel_LoginShowPassword.Size = New System.Drawing.Size(420, 18)
        Me.FlowLayoutPanel_LoginShowPassword.TabIndex = 6
        '
        'Checkbox_LoginShowPassword
        '
        Me.Checkbox_LoginShowPassword.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Checkbox_LoginShowPassword.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Me.Checkbox_LoginShowPassword.CheckedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Me.Checkbox_LoginShowPassword.CheckedState.BorderRadius = 0
        Me.Checkbox_LoginShowPassword.CheckedState.BorderThickness = 1
        Me.Checkbox_LoginShowPassword.CheckedState.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Me.Checkbox_LoginShowPassword.CheckMarkColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Me.Checkbox_LoginShowPassword.Location = New System.Drawing.Point(0, 0)
        Me.Checkbox_LoginShowPassword.Margin = New System.Windows.Forms.Padding(0)
        Me.Checkbox_LoginShowPassword.Name = "Checkbox_LoginShowPassword"
        Me.Checkbox_LoginShowPassword.Size = New System.Drawing.Size(18, 18)
        Me.Checkbox_LoginShowPassword.TabIndex = 4
        Me.Checkbox_LoginShowPassword.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Me.Checkbox_LoginShowPassword.UncheckedState.BorderRadius = 0
        Me.Checkbox_LoginShowPassword.UncheckedState.BorderThickness = 1
        Me.Checkbox_LoginShowPassword.UncheckedState.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        '
        'Label_LoginShowPassword
        '
        Me.Label_LoginShowPassword.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label_LoginShowPassword.AutoSize = true
        Me.Label_LoginShowPassword.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Me.Label_LoginShowPassword.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Me.Label_LoginShowPassword.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Me.Label_LoginShowPassword.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label_LoginShowPassword.Location = New System.Drawing.Point(18, 1)
        Me.Label_LoginShowPassword.Margin = New System.Windows.Forms.Padding(0)
        Me.Label_LoginShowPassword.Name = "Label_LoginShowPassword"
        Me.Label_LoginShowPassword.Size = New System.Drawing.Size(94, 16)
        Me.Label_LoginShowPassword.TabIndex = 3
        Me.Label_LoginShowPassword.Text = "Show Password"
        Me.Label_LoginShowPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'FlowLayoutPanel_LoginInputPassword
        '
        Me.FlowLayoutPanel_LoginInputPassword.AutoSize = true
        Me.FlowLayoutPanel_LoginInputPassword.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.FlowLayoutPanel_LoginInputPassword.Controls.Add(Me.Label_LoginInputPassword)
        Me.FlowLayoutPanel_LoginInputPassword.Controls.Add(Me.Textbox_LoginInputPassword)
        Me.FlowLayoutPanel_LoginInputPassword.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.FlowLayoutPanel_LoginInputPassword.Location = New System.Drawing.Point(32, 257)
        Me.FlowLayoutPanel_LoginInputPassword.Margin = New System.Windows.Forms.Padding(0)
        Me.FlowLayoutPanel_LoginInputPassword.Name = "FlowLayoutPanel_LoginInputPassword"
        Me.FlowLayoutPanel_LoginInputPassword.Size = New System.Drawing.Size(420, 56)
        Me.FlowLayoutPanel_LoginInputPassword.TabIndex = 5
        '
        'Label_LoginInputPassword
        '
        Me.Label_LoginInputPassword.AutoSize = true
        Me.Label_LoginInputPassword.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Me.Label_LoginInputPassword.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Me.Label_LoginInputPassword.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Me.Label_LoginInputPassword.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label_LoginInputPassword.Location = New System.Drawing.Point(0, 0)
        Me.Label_LoginInputPassword.Margin = New System.Windows.Forms.Padding(0)
        Me.Label_LoginInputPassword.Name = "Label_LoginInputPassword"
        Me.Label_LoginInputPassword.Size = New System.Drawing.Size(60, 16)
        Me.Label_LoginInputPassword.TabIndex = 2
        Me.Label_LoginInputPassword.Text = "Password"
        Me.Label_LoginInputPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Textbox_LoginInputPassword
        '
        Me.Textbox_LoginInputPassword.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Me.Textbox_LoginInputPassword.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Me.Textbox_LoginInputPassword.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Textbox_LoginInputPassword.DefaultText = ""
        Me.Textbox_LoginInputPassword.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer))
        Me.Textbox_LoginInputPassword.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer))
        Me.Textbox_LoginInputPassword.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Me.Textbox_LoginInputPassword.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Me.Textbox_LoginInputPassword.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Me.Textbox_LoginInputPassword.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Me.Textbox_LoginInputPassword.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
        Me.Textbox_LoginInputPassword.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Me.Textbox_LoginInputPassword.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Me.Textbox_LoginInputPassword.Location = New System.Drawing.Point(0, 16)
        Me.Textbox_LoginInputPassword.Margin = New System.Windows.Forms.Padding(0)
        Me.Textbox_LoginInputPassword.Name = "Textbox_LoginInputPassword"
        Me.Textbox_LoginInputPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.Textbox_LoginInputPassword.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer))
        Me.Textbox_LoginInputPassword.PlaceholderText = "example_password"
        Me.Textbox_LoginInputPassword.SelectedText = ""
        Me.Textbox_LoginInputPassword.Size = New System.Drawing.Size(420, 40)
        Me.Textbox_LoginInputPassword.TabIndex = 4
        '
        'FlowLayoutPanel_LoginInputEmail
        '
        Me.FlowLayoutPanel_LoginInputEmail.AutoSize = true
        Me.FlowLayoutPanel_LoginInputEmail.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.FlowLayoutPanel_LoginInputEmail.Controls.Add(Me.Label_LoginInputEmail)
        Me.FlowLayoutPanel_LoginInputEmail.Controls.Add(Me.Textbox_LoginInputEmail)
        Me.FlowLayoutPanel_LoginInputEmail.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.FlowLayoutPanel_LoginInputEmail.Location = New System.Drawing.Point(32, 180)
        Me.FlowLayoutPanel_LoginInputEmail.Margin = New System.Windows.Forms.Padding(0)
        Me.FlowLayoutPanel_LoginInputEmail.Name = "FlowLayoutPanel_LoginInputEmail"
        Me.FlowLayoutPanel_LoginInputEmail.Size = New System.Drawing.Size(420, 56)
        Me.FlowLayoutPanel_LoginInputEmail.TabIndex = 4
        '
        'Label_LoginInputEmail
        '
        Me.Label_LoginInputEmail.AutoSize = true
        Me.Label_LoginInputEmail.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Me.Label_LoginInputEmail.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Me.Label_LoginInputEmail.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Me.Label_LoginInputEmail.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label_LoginInputEmail.Location = New System.Drawing.Point(0, 0)
        Me.Label_LoginInputEmail.Margin = New System.Windows.Forms.Padding(0)
        Me.Label_LoginInputEmail.Name = "Label_LoginInputEmail"
        Me.Label_LoginInputEmail.Size = New System.Drawing.Size(38, 16)
        Me.Label_LoginInputEmail.TabIndex = 2
        Me.Label_LoginInputEmail.Text = "Email"
        Me.Label_LoginInputEmail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Textbox_LoginInputEmail
        '
        Me.Textbox_LoginInputEmail.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Me.Textbox_LoginInputEmail.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Me.Textbox_LoginInputEmail.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Textbox_LoginInputEmail.DefaultText = ""
        Me.Textbox_LoginInputEmail.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer))
        Me.Textbox_LoginInputEmail.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer))
        Me.Textbox_LoginInputEmail.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Me.Textbox_LoginInputEmail.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Me.Textbox_LoginInputEmail.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Me.Textbox_LoginInputEmail.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Me.Textbox_LoginInputEmail.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
        Me.Textbox_LoginInputEmail.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Me.Textbox_LoginInputEmail.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Me.Textbox_LoginInputEmail.Location = New System.Drawing.Point(0, 16)
        Me.Textbox_LoginInputEmail.Margin = New System.Windows.Forms.Padding(0)
        Me.Textbox_LoginInputEmail.Name = "Textbox_LoginInputEmail"
        Me.Textbox_LoginInputEmail.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.Textbox_LoginInputEmail.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer))
        Me.Textbox_LoginInputEmail.PlaceholderText = "example@email.com"
        Me.Textbox_LoginInputEmail.SelectedText = ""
        Me.Textbox_LoginInputEmail.ShadowDecoration.BorderRadius = 0
        Me.Textbox_LoginInputEmail.ShadowDecoration.Color = System.Drawing.Color.Red
        Me.Textbox_LoginInputEmail.Size = New System.Drawing.Size(420, 40)
        Me.Textbox_LoginInputEmail.TabIndex = 3
        '
        'Label_LoginFormSubHeader
        '
        Me.Label_LoginFormSubHeader.AutoSize = true
        Me.Label_LoginFormSubHeader.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Me.Label_LoginFormSubHeader.Font = New System.Drawing.Font("Microsoft JhengHei", 22!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Me.Label_LoginFormSubHeader.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Me.Label_LoginFormSubHeader.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label_LoginFormSubHeader.Location = New System.Drawing.Point(30, 77)
        Me.Label_LoginFormSubHeader.Margin = New System.Windows.Forms.Padding(0)
        Me.Label_LoginFormSubHeader.Name = "Label_LoginFormSubHeader"
        Me.Label_LoginFormSubHeader.Size = New System.Drawing.Size(72, 28)
        Me.Label_LoginFormSubHeader.TabIndex = 1
        Me.Label_LoginFormSubHeader.Text = "Login"
        Me.Label_LoginFormSubHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label_LoginForHeader
        '
        Me.Label_LoginForHeader.AutoSize = true
        Me.Label_LoginForHeader.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Me.Label_LoginForHeader.Font = New System.Drawing.Font("Microsoft JhengHei", 32!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Me.Label_LoginForHeader.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Me.Label_LoginForHeader.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label_LoginForHeader.Location = New System.Drawing.Point(30, 37)
        Me.Label_LoginForHeader.Margin = New System.Windows.Forms.Padding(0)
        Me.Label_LoginForHeader.Name = "Label_LoginForHeader"
        Me.Label_LoginForHeader.Size = New System.Drawing.Size(319, 40)
        Me.Label_LoginForHeader.TabIndex = 0
        Me.Label_LoginForHeader.Text = "Sales and Inventory"
        Me.Label_LoginForHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Guna2BorderlessForm1
        '
        Me.Guna2BorderlessForm1.ContainerControl = Me
        Me.Guna2BorderlessForm1.DockIndicatorTransparencyValue = 0.6R
        Me.Guna2BorderlessForm1.TransparentWhileDrag = true
        '
        'ProgressBar_Loading
        '
        Me.ProgressBar_Loading.BackColor = System.Drawing.Color.FromArgb(CType(CType(221,Byte),Integer), CType(CType(221,Byte),Integer), CType(CType(221,Byte),Integer))
        Me.ProgressBar_Loading.Location = New System.Drawing.Point(1175, 771)
        Me.ProgressBar_Loading.Margin = New System.Windows.Forms.Padding(0)
        Me.ProgressBar_Loading.Name = "ProgressBar_Loading"
        Me.ProgressBar_Loading.Size = New System.Drawing.Size(200, 20)
        Me.ProgressBar_Loading.TabIndex = 1
        '
        'Window_Login
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = Global.ERP_SIMS.My.Resources.Resources.Background_Login
        Me.ClientSize = New System.Drawing.Size(1384, 800)
        Me.Controls.Add(Me.ProgressBar_Loading)
        Me.Controls.Add(Me.Panel_LoginForm)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = false
        Me.Name = "Window_Login"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Login"
        Me.Panel_LoginForm.ResumeLayout(false)
        Me.Panel_LoginForm.PerformLayout
        Me.FlowLayoutPanel_LoginShowPassword.ResumeLayout(false)
        Me.FlowLayoutPanel_LoginShowPassword.PerformLayout
        Me.FlowLayoutPanel_LoginInputPassword.ResumeLayout(false)
        Me.FlowLayoutPanel_LoginInputPassword.PerformLayout
        Me.FlowLayoutPanel_LoginInputEmail.ResumeLayout(false)
        Me.FlowLayoutPanel_LoginInputEmail.PerformLayout
        Me.ResumeLayout(false)

End Sub

    Friend WithEvents Panel_LoginForm As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents Label_LoginForHeader As Label
    Friend WithEvents Label_LoginFormSubHeader As Label
    Friend WithEvents Label_LoginInputEmail As Label
    Friend WithEvents Textbox_LoginInputEmail As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents FlowLayoutPanel_LoginInputEmail As FlowLayoutPanel
    Friend WithEvents FlowLayoutPanel_LoginInputPassword As FlowLayoutPanel
    Friend WithEvents Label_LoginInputPassword As Label
    Friend WithEvents FlowLayoutPanel_LoginShowPassword As FlowLayoutPanel
    Friend WithEvents Label_LoginShowPassword As Label
    Friend WithEvents Checkbox_LoginShowPassword As Guna.UI2.WinForms.Guna2CustomCheckBox
    Friend WithEvents Textbox_LoginInputPassword As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents Button_Login As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents Guna2BorderlessForm1 As Guna.UI2.WinForms.Guna2BorderlessForm
    Friend WithEvents ProgressBar_Loading As ProgressBar
End Class
