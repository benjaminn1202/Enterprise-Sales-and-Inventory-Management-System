<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Window_NewCategory
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
        Me.Background_NewCategory = New System.Windows.Forms.Panel()
        Me.Panel_MainContentContainerNewCategory = New System.Windows.Forms.Panel()
        Me.Panel_NewCategory = New System.Windows.Forms.Panel()
        Me.FlowLayoutPanel_ContentsNewCategory = New System.Windows.Forms.FlowLayoutPanel()
        Me.FlowLayoutPanel_CategoryDetailsNewCategory = New System.Windows.Forms.FlowLayoutPanel()
        Me.Label_CategoryDetailsNewCategory = New System.Windows.Forms.Label()
        Me.FlowLayoutPanel_CategoryDetailCategoryNameInputNewCategory = New System.Windows.Forms.FlowLayoutPanel()
        Me.Label_CategoryDetailCategoryNameInputNewCategory = New System.Windows.Forms.Label()
        Me.Textbox_CategoryDetailCategoryNameInputNewCategory = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Button_MakeCategoryNewCategory = New Guna.UI2.WinForms.Guna2Button()
        Me.Panel_ControlsNewCategory = New Guna.UI2.WinForms.Guna2ShadowPanel()
        Me.Button_BackNewCategory = New Guna.UI2.WinForms.Guna2Button()
        Me.FlowLayoutPanel_PageHeaderNewCategory = New System.Windows.Forms.FlowLayoutPanel()
        Me.Panel_PageHeaderIconNewCategory = New System.Windows.Forms.Panel()
        Me.Label_PageHeaderNewCategory = New System.Windows.Forms.Label()
        Me.Background_NewCategory.SuspendLayout
        Me.Panel_MainContentContainerNewCategory.SuspendLayout
        Me.Panel_NewCategory.SuspendLayout
        Me.FlowLayoutPanel_ContentsNewCategory.SuspendLayout
        Me.FlowLayoutPanel_CategoryDetailsNewCategory.SuspendLayout
        Me.FlowLayoutPanel_CategoryDetailCategoryNameInputNewCategory.SuspendLayout
        Me.Panel_ControlsNewCategory.SuspendLayout
        Me.FlowLayoutPanel_PageHeaderNewCategory.SuspendLayout
        Me.SuspendLayout
        '
        'Background_NewCategory
        '
        Me.Background_NewCategory.BackgroundImage = Global.ERP_SIMS.My.Resources.Resources.Image_Background
        Me.Background_NewCategory.Controls.Add(Me.Panel_MainContentContainerNewCategory)
        Me.Background_NewCategory.Location = New System.Drawing.Point(0, 0)
        Me.Background_NewCategory.Margin = New System.Windows.Forms.Padding(0)
        Me.Background_NewCategory.Name = "Background_NewCategory"
        Me.Background_NewCategory.Size = New System.Drawing.Size(1384, 800)
        Me.Background_NewCategory.TabIndex = 3
        '
        'Panel_MainContentContainerNewCategory
        '
        Me.Panel_MainContentContainerNewCategory.Controls.Add(Me.Panel_NewCategory)
        Me.Panel_MainContentContainerNewCategory.Controls.Add(Me.Panel_ControlsNewCategory)
        Me.Panel_MainContentContainerNewCategory.Location = New System.Drawing.Point(120, 0)
        Me.Panel_MainContentContainerNewCategory.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel_MainContentContainerNewCategory.Name = "Panel_MainContentContainerNewCategory"
        Me.Panel_MainContentContainerNewCategory.Size = New System.Drawing.Size(1144, 800)
        Me.Panel_MainContentContainerNewCategory.TabIndex = 7
        '
        'Panel_NewCategory
        '
        Me.Panel_NewCategory.AutoScroll = true
        Me.Panel_NewCategory.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Me.Panel_NewCategory.Controls.Add(Me.FlowLayoutPanel_ContentsNewCategory)
        Me.Panel_NewCategory.Location = New System.Drawing.Point(0, 64)
        Me.Panel_NewCategory.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel_NewCategory.Name = "Panel_NewCategory"
        Me.Panel_NewCategory.Size = New System.Drawing.Size(1144, 736)
        Me.Panel_NewCategory.TabIndex = 7
        '
        'FlowLayoutPanel_ContentsNewCategory
        '
        Me.FlowLayoutPanel_ContentsNewCategory.AutoSize = true
        Me.FlowLayoutPanel_ContentsNewCategory.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.FlowLayoutPanel_ContentsNewCategory.Controls.Add(Me.FlowLayoutPanel_CategoryDetailsNewCategory)
        Me.FlowLayoutPanel_ContentsNewCategory.Controls.Add(Me.Button_MakeCategoryNewCategory)
        Me.FlowLayoutPanel_ContentsNewCategory.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.FlowLayoutPanel_ContentsNewCategory.Location = New System.Drawing.Point(22, 28)
        Me.FlowLayoutPanel_ContentsNewCategory.Margin = New System.Windows.Forms.Padding(0)
        Me.FlowLayoutPanel_ContentsNewCategory.Name = "FlowLayoutPanel_ContentsNewCategory"
        Me.FlowLayoutPanel_ContentsNewCategory.Size = New System.Drawing.Size(550, 202)
        Me.FlowLayoutPanel_ContentsNewCategory.TabIndex = 0
        '
        'FlowLayoutPanel_CategoryDetailsNewCategory
        '
        Me.FlowLayoutPanel_CategoryDetailsNewCategory.AutoSize = true
        Me.FlowLayoutPanel_CategoryDetailsNewCategory.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.FlowLayoutPanel_CategoryDetailsNewCategory.Controls.Add(Me.Label_CategoryDetailsNewCategory)
        Me.FlowLayoutPanel_CategoryDetailsNewCategory.Controls.Add(Me.FlowLayoutPanel_CategoryDetailCategoryNameInputNewCategory)
        Me.FlowLayoutPanel_CategoryDetailsNewCategory.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.FlowLayoutPanel_CategoryDetailsNewCategory.Location = New System.Drawing.Point(0, 0)
        Me.FlowLayoutPanel_CategoryDetailsNewCategory.Margin = New System.Windows.Forms.Padding(0, 0, 0, 40)
        Me.FlowLayoutPanel_CategoryDetailsNewCategory.Name = "FlowLayoutPanel_CategoryDetailsNewCategory"
        Me.FlowLayoutPanel_CategoryDetailsNewCategory.Size = New System.Drawing.Size(550, 103)
        Me.FlowLayoutPanel_CategoryDetailsNewCategory.TabIndex = 8
        '
        'Label_CategoryDetailsNewCategory
        '
        Me.Label_CategoryDetailsNewCategory.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label_CategoryDetailsNewCategory.AutoSize = true
        Me.Label_CategoryDetailsNewCategory.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Me.Label_CategoryDetailsNewCategory.Font = New System.Drawing.Font("Microsoft JhengHei", 22!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Me.Label_CategoryDetailsNewCategory.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Me.Label_CategoryDetailsNewCategory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label_CategoryDetailsNewCategory.Location = New System.Drawing.Point(0, 0)
        Me.Label_CategoryDetailsNewCategory.Margin = New System.Windows.Forms.Padding(0, 0, 0, 19)
        Me.Label_CategoryDetailsNewCategory.Name = "Label_CategoryDetailsNewCategory"
        Me.Label_CategoryDetailsNewCategory.Size = New System.Drawing.Size(187, 28)
        Me.Label_CategoryDetailsNewCategory.TabIndex = 7
        Me.Label_CategoryDetailsNewCategory.Text = "Category Details"
        Me.Label_CategoryDetailsNewCategory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'FlowLayoutPanel_CategoryDetailCategoryNameInputNewCategory
        '
        Me.FlowLayoutPanel_CategoryDetailCategoryNameInputNewCategory.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.FlowLayoutPanel_CategoryDetailCategoryNameInputNewCategory.Controls.Add(Me.Label_CategoryDetailCategoryNameInputNewCategory)
        Me.FlowLayoutPanel_CategoryDetailCategoryNameInputNewCategory.Controls.Add(Me.Textbox_CategoryDetailCategoryNameInputNewCategory)
        Me.FlowLayoutPanel_CategoryDetailCategoryNameInputNewCategory.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.FlowLayoutPanel_CategoryDetailCategoryNameInputNewCategory.Location = New System.Drawing.Point(0, 47)
        Me.FlowLayoutPanel_CategoryDetailCategoryNameInputNewCategory.Margin = New System.Windows.Forms.Padding(0)
        Me.FlowLayoutPanel_CategoryDetailCategoryNameInputNewCategory.Name = "FlowLayoutPanel_CategoryDetailCategoryNameInputNewCategory"
        Me.FlowLayoutPanel_CategoryDetailCategoryNameInputNewCategory.Size = New System.Drawing.Size(550, 56)
        Me.FlowLayoutPanel_CategoryDetailCategoryNameInputNewCategory.TabIndex = 8
        '
        'Label_CategoryDetailCategoryNameInputNewCategory
        '
        Me.Label_CategoryDetailCategoryNameInputNewCategory.AutoSize = true
        Me.Label_CategoryDetailCategoryNameInputNewCategory.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Me.Label_CategoryDetailCategoryNameInputNewCategory.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Me.Label_CategoryDetailCategoryNameInputNewCategory.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Me.Label_CategoryDetailCategoryNameInputNewCategory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label_CategoryDetailCategoryNameInputNewCategory.Location = New System.Drawing.Point(0, 0)
        Me.Label_CategoryDetailCategoryNameInputNewCategory.Margin = New System.Windows.Forms.Padding(0)
        Me.Label_CategoryDetailCategoryNameInputNewCategory.Name = "Label_CategoryDetailCategoryNameInputNewCategory"
        Me.Label_CategoryDetailCategoryNameInputNewCategory.Size = New System.Drawing.Size(97, 16)
        Me.Label_CategoryDetailCategoryNameInputNewCategory.TabIndex = 2
        Me.Label_CategoryDetailCategoryNameInputNewCategory.Text = "Category Name"
        Me.Label_CategoryDetailCategoryNameInputNewCategory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Textbox_CategoryDetailCategoryNameInputNewCategory
        '
        Me.Textbox_CategoryDetailCategoryNameInputNewCategory.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Textbox_CategoryDetailCategoryNameInputNewCategory.AutoCompleteCustomSource.AddRange(New String() {"(001) Item 1", "(002) Item 2", "(003) Item 3", "(004) Item 4", "(005) Item 5", "(006) Item 6", "(007) Item 7", "(008) Item 8", "(009) Item 9", "(010) Item 10"})
        Me.Textbox_CategoryDetailCategoryNameInputNewCategory.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Me.Textbox_CategoryDetailCategoryNameInputNewCategory.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Me.Textbox_CategoryDetailCategoryNameInputNewCategory.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Textbox_CategoryDetailCategoryNameInputNewCategory.DefaultText = ""
        Me.Textbox_CategoryDetailCategoryNameInputNewCategory.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer))
        Me.Textbox_CategoryDetailCategoryNameInputNewCategory.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer))
        Me.Textbox_CategoryDetailCategoryNameInputNewCategory.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Me.Textbox_CategoryDetailCategoryNameInputNewCategory.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Me.Textbox_CategoryDetailCategoryNameInputNewCategory.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Me.Textbox_CategoryDetailCategoryNameInputNewCategory.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Me.Textbox_CategoryDetailCategoryNameInputNewCategory.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
        Me.Textbox_CategoryDetailCategoryNameInputNewCategory.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Me.Textbox_CategoryDetailCategoryNameInputNewCategory.Location = New System.Drawing.Point(0, 16)
        Me.Textbox_CategoryDetailCategoryNameInputNewCategory.Margin = New System.Windows.Forms.Padding(0)
        Me.Textbox_CategoryDetailCategoryNameInputNewCategory.Name = "Textbox_CategoryDetailCategoryNameInputNewCategory"
        Me.Textbox_CategoryDetailCategoryNameInputNewCategory.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.Textbox_CategoryDetailCategoryNameInputNewCategory.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer))
        Me.Textbox_CategoryDetailCategoryNameInputNewCategory.PlaceholderText = "Required"
        Me.Textbox_CategoryDetailCategoryNameInputNewCategory.SelectedText = ""
        Me.Textbox_CategoryDetailCategoryNameInputNewCategory.Size = New System.Drawing.Size(550, 40)
        Me.Textbox_CategoryDetailCategoryNameInputNewCategory.TabIndex = 39
        '
        'Button_MakeCategoryNewCategory
        '
        Me.Button_MakeCategoryNewCategory.BackgroundImage = Global.ERP_SIMS.My.Resources.Resources.Button_MakeCategory
        Me.Button_MakeCategoryNewCategory.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Me.Button_MakeCategoryNewCategory.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Me.Button_MakeCategoryNewCategory.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer))
        Me.Button_MakeCategoryNewCategory.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer))
        Me.Button_MakeCategoryNewCategory.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Me.Button_MakeCategoryNewCategory.Font = New System.Drawing.Font("Segoe UI", 9!)
        Me.Button_MakeCategoryNewCategory.ForeColor = System.Drawing.Color.White
        Me.Button_MakeCategoryNewCategory.Location = New System.Drawing.Point(0, 143)
        Me.Button_MakeCategoryNewCategory.Margin = New System.Windows.Forms.Padding(0, 0, 0, 19)
        Me.Button_MakeCategoryNewCategory.Name = "Button_MakeCategoryNewCategory"
        Me.Button_MakeCategoryNewCategory.Size = New System.Drawing.Size(162, 40)
        Me.Button_MakeCategoryNewCategory.TabIndex = 12
        '
        'Panel_ControlsNewCategory
        '
        Me.Panel_ControlsNewCategory.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Me.Panel_ControlsNewCategory.Controls.Add(Me.Button_BackNewCategory)
        Me.Panel_ControlsNewCategory.Controls.Add(Me.FlowLayoutPanel_PageHeaderNewCategory)
        Me.Panel_ControlsNewCategory.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Me.Panel_ControlsNewCategory.Location = New System.Drawing.Point(0, 0)
        Me.Panel_ControlsNewCategory.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel_ControlsNewCategory.Name = "Panel_ControlsNewCategory"
        Me.Panel_ControlsNewCategory.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Me.Panel_ControlsNewCategory.ShadowDepth = 16
        Me.Panel_ControlsNewCategory.ShadowStyle = Guna.UI2.WinForms.Guna2ShadowPanel.ShadowMode.Dropped
        Me.Panel_ControlsNewCategory.Size = New System.Drawing.Size(1144, 64)
        Me.Panel_ControlsNewCategory.TabIndex = 8
        '
        'Button_BackNewCategory
        '
        Me.Button_BackNewCategory.BackgroundImage = Global.ERP_SIMS.My.Resources.Resources.Button_Back
        Me.Button_BackNewCategory.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Me.Button_BackNewCategory.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Me.Button_BackNewCategory.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer))
        Me.Button_BackNewCategory.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer))
        Me.Button_BackNewCategory.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Me.Button_BackNewCategory.Font = New System.Drawing.Font("Segoe UI", 9!)
        Me.Button_BackNewCategory.ForeColor = System.Drawing.Color.White
        Me.Button_BackNewCategory.Location = New System.Drawing.Point(1099, 25)
        Me.Button_BackNewCategory.Margin = New System.Windows.Forms.Padding(0)
        Me.Button_BackNewCategory.Name = "Button_BackNewCategory"
        Me.Button_BackNewCategory.Size = New System.Drawing.Size(25, 15)
        Me.Button_BackNewCategory.TabIndex = 24
        '
        'FlowLayoutPanel_PageHeaderNewCategory
        '
        Me.FlowLayoutPanel_PageHeaderNewCategory.AutoSize = true
        Me.FlowLayoutPanel_PageHeaderNewCategory.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.FlowLayoutPanel_PageHeaderNewCategory.Controls.Add(Me.Panel_PageHeaderIconNewCategory)
        Me.FlowLayoutPanel_PageHeaderNewCategory.Controls.Add(Me.Label_PageHeaderNewCategory)
        Me.FlowLayoutPanel_PageHeaderNewCategory.Location = New System.Drawing.Point(22, 14)
        Me.FlowLayoutPanel_PageHeaderNewCategory.Margin = New System.Windows.Forms.Padding(0, 0, 0, 42)
        Me.FlowLayoutPanel_PageHeaderNewCategory.Name = "FlowLayoutPanel_PageHeaderNewCategory"
        Me.FlowLayoutPanel_PageHeaderNewCategory.Size = New System.Drawing.Size(256, 36)
        Me.FlowLayoutPanel_PageHeaderNewCategory.TabIndex = 0
        '
        'Panel_PageHeaderIconNewCategory
        '
        Me.Panel_PageHeaderIconNewCategory.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Panel_PageHeaderIconNewCategory.BackgroundImage = Global.ERP_SIMS.My.Resources.Resources.HeaderIcon_Entry
        Me.Panel_PageHeaderIconNewCategory.Location = New System.Drawing.Point(0, 2)
        Me.Panel_PageHeaderIconNewCategory.Margin = New System.Windows.Forms.Padding(0, 0, 16, 0)
        Me.Panel_PageHeaderIconNewCategory.Name = "Panel_PageHeaderIconNewCategory"
        Me.Panel_PageHeaderIconNewCategory.Size = New System.Drawing.Size(32, 32)
        Me.Panel_PageHeaderIconNewCategory.TabIndex = 3
        '
        'Label_PageHeaderNewCategory
        '
        Me.Label_PageHeaderNewCategory.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label_PageHeaderNewCategory.AutoSize = true
        Me.Label_PageHeaderNewCategory.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Me.Label_PageHeaderNewCategory.Font = New System.Drawing.Font("Microsoft JhengHei", 28!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Me.Label_PageHeaderNewCategory.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Me.Label_PageHeaderNewCategory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label_PageHeaderNewCategory.Location = New System.Drawing.Point(48, 0)
        Me.Label_PageHeaderNewCategory.Margin = New System.Windows.Forms.Padding(0)
        Me.Label_PageHeaderNewCategory.Name = "Label_PageHeaderNewCategory"
        Me.Label_PageHeaderNewCategory.Size = New System.Drawing.Size(208, 36)
        Me.Label_PageHeaderNewCategory.TabIndex = 2
        Me.Label_PageHeaderNewCategory.Text = "New Category"
        Me.Label_PageHeaderNewCategory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Window_NewCategory
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1384, 800)
        Me.Controls.Add(Me.Background_NewCategory)
        Me.Name = "Window_NewCategory"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "New Category"
        Me.Background_NewCategory.ResumeLayout(false)
        Me.Panel_MainContentContainerNewCategory.ResumeLayout(false)
        Me.Panel_NewCategory.ResumeLayout(false)
        Me.Panel_NewCategory.PerformLayout
        Me.FlowLayoutPanel_ContentsNewCategory.ResumeLayout(false)
        Me.FlowLayoutPanel_ContentsNewCategory.PerformLayout
        Me.FlowLayoutPanel_CategoryDetailsNewCategory.ResumeLayout(false)
        Me.FlowLayoutPanel_CategoryDetailsNewCategory.PerformLayout
        Me.FlowLayoutPanel_CategoryDetailCategoryNameInputNewCategory.ResumeLayout(false)
        Me.FlowLayoutPanel_CategoryDetailCategoryNameInputNewCategory.PerformLayout
        Me.Panel_ControlsNewCategory.ResumeLayout(false)
        Me.Panel_ControlsNewCategory.PerformLayout
        Me.FlowLayoutPanel_PageHeaderNewCategory.ResumeLayout(false)
        Me.FlowLayoutPanel_PageHeaderNewCategory.PerformLayout
        Me.ResumeLayout(false)

End Sub

    Friend WithEvents Background_NewCategory As Panel
    Friend WithEvents Panel_MainContentContainerNewCategory As Panel
    Friend WithEvents Panel_NewCategory As Panel
    Friend WithEvents FlowLayoutPanel_ContentsNewCategory As FlowLayoutPanel
    Friend WithEvents FlowLayoutPanel_CategoryDetailsNewCategory As FlowLayoutPanel
    Friend WithEvents Label_CategoryDetailsNewCategory As Label
    Friend WithEvents FlowLayoutPanel_CategoryDetailCategoryNameInputNewCategory As FlowLayoutPanel
    Friend WithEvents Label_CategoryDetailCategoryNameInputNewCategory As Label
    Friend WithEvents Textbox_CategoryDetailCategoryNameInputNewCategory As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents Button_MakeCategoryNewCategory As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents Panel_ControlsNewCategory As Guna.UI2.WinForms.Guna2ShadowPanel
    Friend WithEvents Button_BackNewCategory As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents FlowLayoutPanel_PageHeaderNewCategory As FlowLayoutPanel
    Friend WithEvents Panel_PageHeaderIconNewCategory As Panel
    Friend WithEvents Label_PageHeaderNewCategory As Label
End Class
