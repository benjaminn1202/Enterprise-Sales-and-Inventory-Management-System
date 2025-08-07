<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Window_EditCategory
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
        Me.Background_EditCategory = New System.Windows.Forms.Panel()
        Me.Panel_MainContentContainerEditCategory = New System.Windows.Forms.Panel()
        Me.Panel_EditCategory = New System.Windows.Forms.Panel()
        Me.FlowLayoutPanel_ContentsEditCategory = New System.Windows.Forms.FlowLayoutPanel()
        Me.FlowLayoutPanel_CategoryDetailsEditCategory = New System.Windows.Forms.FlowLayoutPanel()
        Me.Label_CategoryDetailsEditCategory = New System.Windows.Forms.Label()
        Me.FlowLayoutPanel_CategoryDetailCategoryIDInputEditCategory = New System.Windows.Forms.FlowLayoutPanel()
        Me.Label_CategoryDetailCategoryIDInputEditCategory = New System.Windows.Forms.Label()
        Me.Textbox_CategoryDetailCategoryIDInputEditCategory = New Guna.UI2.WinForms.Guna2TextBox()
        Me.FlowLayoutPanel_CategoryDetailCategoryNameInputEditCategory = New System.Windows.Forms.FlowLayoutPanel()
        Me.Label_CategoryDetailCategoryNameInputEditCategory = New System.Windows.Forms.Label()
        Me.Textbox_CategoryDetailCategoryNameInputEditCategory = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Button_MakeCategoryEditCategory = New Guna.UI2.WinForms.Guna2Button()
        Me.Panel_ControlsEditCategory = New Guna.UI2.WinForms.Guna2ShadowPanel()
        Me.Button_BackEditCategory = New Guna.UI2.WinForms.Guna2Button()
        Me.FlowLayoutPanel_PageHeaderEditCategory = New System.Windows.Forms.FlowLayoutPanel()
        Me.Panel_PageHeaderIconEditCategory = New System.Windows.Forms.Panel()
        Me.Label_PageHeaderEditCategory = New System.Windows.Forms.Label()
        Me.Background_EditCategory.SuspendLayout
        Me.Panel_MainContentContainerEditCategory.SuspendLayout
        Me.Panel_EditCategory.SuspendLayout
        Me.FlowLayoutPanel_ContentsEditCategory.SuspendLayout
        Me.FlowLayoutPanel_CategoryDetailsEditCategory.SuspendLayout
        Me.FlowLayoutPanel_CategoryDetailCategoryIDInputEditCategory.SuspendLayout
        Me.FlowLayoutPanel_CategoryDetailCategoryNameInputEditCategory.SuspendLayout
        Me.Panel_ControlsEditCategory.SuspendLayout
        Me.FlowLayoutPanel_PageHeaderEditCategory.SuspendLayout
        Me.SuspendLayout
        '
        'Background_EditCategory
        '
        Me.Background_EditCategory.BackgroundImage = Global.ERP_SIMS.My.Resources.Resources.Image_Background
        Me.Background_EditCategory.Controls.Add(Me.Panel_MainContentContainerEditCategory)
        Me.Background_EditCategory.Location = New System.Drawing.Point(0, 0)
        Me.Background_EditCategory.Margin = New System.Windows.Forms.Padding(0)
        Me.Background_EditCategory.Name = "Background_EditCategory"
        Me.Background_EditCategory.Size = New System.Drawing.Size(1384, 800)
        Me.Background_EditCategory.TabIndex = 4
        '
        'Panel_MainContentContainerEditCategory
        '
        Me.Panel_MainContentContainerEditCategory.Controls.Add(Me.Panel_EditCategory)
        Me.Panel_MainContentContainerEditCategory.Controls.Add(Me.Panel_ControlsEditCategory)
        Me.Panel_MainContentContainerEditCategory.Location = New System.Drawing.Point(120, 0)
        Me.Panel_MainContentContainerEditCategory.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel_MainContentContainerEditCategory.Name = "Panel_MainContentContainerEditCategory"
        Me.Panel_MainContentContainerEditCategory.Size = New System.Drawing.Size(1144, 800)
        Me.Panel_MainContentContainerEditCategory.TabIndex = 7
        '
        'Panel_EditCategory
        '
        Me.Panel_EditCategory.AutoScroll = true
        Me.Panel_EditCategory.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Me.Panel_EditCategory.Controls.Add(Me.FlowLayoutPanel_ContentsEditCategory)
        Me.Panel_EditCategory.Location = New System.Drawing.Point(0, 64)
        Me.Panel_EditCategory.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel_EditCategory.Name = "Panel_EditCategory"
        Me.Panel_EditCategory.Size = New System.Drawing.Size(1144, 736)
        Me.Panel_EditCategory.TabIndex = 7
        '
        'FlowLayoutPanel_ContentsEditCategory
        '
        Me.FlowLayoutPanel_ContentsEditCategory.AutoSize = true
        Me.FlowLayoutPanel_ContentsEditCategory.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.FlowLayoutPanel_ContentsEditCategory.Controls.Add(Me.FlowLayoutPanel_CategoryDetailsEditCategory)
        Me.FlowLayoutPanel_ContentsEditCategory.Controls.Add(Me.Button_MakeCategoryEditCategory)
        Me.FlowLayoutPanel_ContentsEditCategory.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.FlowLayoutPanel_ContentsEditCategory.Location = New System.Drawing.Point(22, 28)
        Me.FlowLayoutPanel_ContentsEditCategory.Margin = New System.Windows.Forms.Padding(0)
        Me.FlowLayoutPanel_ContentsEditCategory.Name = "FlowLayoutPanel_ContentsEditCategory"
        Me.FlowLayoutPanel_ContentsEditCategory.Size = New System.Drawing.Size(550, 277)
        Me.FlowLayoutPanel_ContentsEditCategory.TabIndex = 0
        '
        'FlowLayoutPanel_CategoryDetailsEditCategory
        '
        Me.FlowLayoutPanel_CategoryDetailsEditCategory.AutoSize = true
        Me.FlowLayoutPanel_CategoryDetailsEditCategory.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.FlowLayoutPanel_CategoryDetailsEditCategory.Controls.Add(Me.Label_CategoryDetailsEditCategory)
        Me.FlowLayoutPanel_CategoryDetailsEditCategory.Controls.Add(Me.FlowLayoutPanel_CategoryDetailCategoryIDInputEditCategory)
        Me.FlowLayoutPanel_CategoryDetailsEditCategory.Controls.Add(Me.FlowLayoutPanel_CategoryDetailCategoryNameInputEditCategory)
        Me.FlowLayoutPanel_CategoryDetailsEditCategory.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.FlowLayoutPanel_CategoryDetailsEditCategory.Location = New System.Drawing.Point(0, 0)
        Me.FlowLayoutPanel_CategoryDetailsEditCategory.Margin = New System.Windows.Forms.Padding(0, 0, 0, 40)
        Me.FlowLayoutPanel_CategoryDetailsEditCategory.Name = "FlowLayoutPanel_CategoryDetailsEditCategory"
        Me.FlowLayoutPanel_CategoryDetailsEditCategory.Size = New System.Drawing.Size(550, 178)
        Me.FlowLayoutPanel_CategoryDetailsEditCategory.TabIndex = 8
        '
        'Label_CategoryDetailsEditCategory
        '
        Me.Label_CategoryDetailsEditCategory.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label_CategoryDetailsEditCategory.AutoSize = true
        Me.Label_CategoryDetailsEditCategory.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Me.Label_CategoryDetailsEditCategory.Font = New System.Drawing.Font("Microsoft JhengHei", 22!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Me.Label_CategoryDetailsEditCategory.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Me.Label_CategoryDetailsEditCategory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label_CategoryDetailsEditCategory.Location = New System.Drawing.Point(0, 0)
        Me.Label_CategoryDetailsEditCategory.Margin = New System.Windows.Forms.Padding(0, 0, 0, 19)
        Me.Label_CategoryDetailsEditCategory.Name = "Label_CategoryDetailsEditCategory"
        Me.Label_CategoryDetailsEditCategory.Size = New System.Drawing.Size(187, 28)
        Me.Label_CategoryDetailsEditCategory.TabIndex = 7
        Me.Label_CategoryDetailsEditCategory.Text = "Category Details"
        Me.Label_CategoryDetailsEditCategory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'FlowLayoutPanel_CategoryDetailCategoryIDInputEditCategory
        '
        Me.FlowLayoutPanel_CategoryDetailCategoryIDInputEditCategory.Controls.Add(Me.Label_CategoryDetailCategoryIDInputEditCategory)
        Me.FlowLayoutPanel_CategoryDetailCategoryIDInputEditCategory.Controls.Add(Me.Textbox_CategoryDetailCategoryIDInputEditCategory)
        Me.FlowLayoutPanel_CategoryDetailCategoryIDInputEditCategory.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.FlowLayoutPanel_CategoryDetailCategoryIDInputEditCategory.Location = New System.Drawing.Point(0, 47)
        Me.FlowLayoutPanel_CategoryDetailCategoryIDInputEditCategory.Margin = New System.Windows.Forms.Padding(0, 0, 0, 19)
        Me.FlowLayoutPanel_CategoryDetailCategoryIDInputEditCategory.Name = "FlowLayoutPanel_CategoryDetailCategoryIDInputEditCategory"
        Me.FlowLayoutPanel_CategoryDetailCategoryIDInputEditCategory.Size = New System.Drawing.Size(550, 56)
        Me.FlowLayoutPanel_CategoryDetailCategoryIDInputEditCategory.TabIndex = 10
        '
        'Label_CategoryDetailCategoryIDInputEditCategory
        '
        Me.Label_CategoryDetailCategoryIDInputEditCategory.AutoSize = true
        Me.Label_CategoryDetailCategoryIDInputEditCategory.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Me.Label_CategoryDetailCategoryIDInputEditCategory.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Me.Label_CategoryDetailCategoryIDInputEditCategory.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Me.Label_CategoryDetailCategoryIDInputEditCategory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label_CategoryDetailCategoryIDInputEditCategory.Location = New System.Drawing.Point(0, 0)
        Me.Label_CategoryDetailCategoryIDInputEditCategory.Margin = New System.Windows.Forms.Padding(0)
        Me.Label_CategoryDetailCategoryIDInputEditCategory.Name = "Label_CategoryDetailCategoryIDInputEditCategory"
        Me.Label_CategoryDetailCategoryIDInputEditCategory.Size = New System.Drawing.Size(74, 16)
        Me.Label_CategoryDetailCategoryIDInputEditCategory.TabIndex = 2
        Me.Label_CategoryDetailCategoryIDInputEditCategory.Text = "Category ID"
        Me.Label_CategoryDetailCategoryIDInputEditCategory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Textbox_CategoryDetailCategoryIDInputEditCategory
        '
        Me.Textbox_CategoryDetailCategoryIDInputEditCategory.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Me.Textbox_CategoryDetailCategoryIDInputEditCategory.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Me.Textbox_CategoryDetailCategoryIDInputEditCategory.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Textbox_CategoryDetailCategoryIDInputEditCategory.DefaultText = ""
        Me.Textbox_CategoryDetailCategoryIDInputEditCategory.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer))
        Me.Textbox_CategoryDetailCategoryIDInputEditCategory.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer))
        Me.Textbox_CategoryDetailCategoryIDInputEditCategory.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Me.Textbox_CategoryDetailCategoryIDInputEditCategory.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Me.Textbox_CategoryDetailCategoryIDInputEditCategory.Enabled = false
        Me.Textbox_CategoryDetailCategoryIDInputEditCategory.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Me.Textbox_CategoryDetailCategoryIDInputEditCategory.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Me.Textbox_CategoryDetailCategoryIDInputEditCategory.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
        Me.Textbox_CategoryDetailCategoryIDInputEditCategory.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Me.Textbox_CategoryDetailCategoryIDInputEditCategory.Location = New System.Drawing.Point(0, 16)
        Me.Textbox_CategoryDetailCategoryIDInputEditCategory.Margin = New System.Windows.Forms.Padding(0)
        Me.Textbox_CategoryDetailCategoryIDInputEditCategory.Name = "Textbox_CategoryDetailCategoryIDInputEditCategory"
        Me.Textbox_CategoryDetailCategoryIDInputEditCategory.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.Textbox_CategoryDetailCategoryIDInputEditCategory.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer))
        Me.Textbox_CategoryDetailCategoryIDInputEditCategory.PlaceholderText = "00000"
        Me.Textbox_CategoryDetailCategoryIDInputEditCategory.SelectedText = ""
        Me.Textbox_CategoryDetailCategoryIDInputEditCategory.Size = New System.Drawing.Size(550, 40)
        Me.Textbox_CategoryDetailCategoryIDInputEditCategory.TabIndex = 3
        '
        'FlowLayoutPanel_CategoryDetailCategoryNameInputEditCategory
        '
        Me.FlowLayoutPanel_CategoryDetailCategoryNameInputEditCategory.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.FlowLayoutPanel_CategoryDetailCategoryNameInputEditCategory.Controls.Add(Me.Label_CategoryDetailCategoryNameInputEditCategory)
        Me.FlowLayoutPanel_CategoryDetailCategoryNameInputEditCategory.Controls.Add(Me.Textbox_CategoryDetailCategoryNameInputEditCategory)
        Me.FlowLayoutPanel_CategoryDetailCategoryNameInputEditCategory.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.FlowLayoutPanel_CategoryDetailCategoryNameInputEditCategory.Location = New System.Drawing.Point(0, 122)
        Me.FlowLayoutPanel_CategoryDetailCategoryNameInputEditCategory.Margin = New System.Windows.Forms.Padding(0)
        Me.FlowLayoutPanel_CategoryDetailCategoryNameInputEditCategory.Name = "FlowLayoutPanel_CategoryDetailCategoryNameInputEditCategory"
        Me.FlowLayoutPanel_CategoryDetailCategoryNameInputEditCategory.Size = New System.Drawing.Size(550, 56)
        Me.FlowLayoutPanel_CategoryDetailCategoryNameInputEditCategory.TabIndex = 8
        '
        'Label_CategoryDetailCategoryNameInputEditCategory
        '
        Me.Label_CategoryDetailCategoryNameInputEditCategory.AutoSize = true
        Me.Label_CategoryDetailCategoryNameInputEditCategory.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Me.Label_CategoryDetailCategoryNameInputEditCategory.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Me.Label_CategoryDetailCategoryNameInputEditCategory.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Me.Label_CategoryDetailCategoryNameInputEditCategory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label_CategoryDetailCategoryNameInputEditCategory.Location = New System.Drawing.Point(0, 0)
        Me.Label_CategoryDetailCategoryNameInputEditCategory.Margin = New System.Windows.Forms.Padding(0)
        Me.Label_CategoryDetailCategoryNameInputEditCategory.Name = "Label_CategoryDetailCategoryNameInputEditCategory"
        Me.Label_CategoryDetailCategoryNameInputEditCategory.Size = New System.Drawing.Size(97, 16)
        Me.Label_CategoryDetailCategoryNameInputEditCategory.TabIndex = 2
        Me.Label_CategoryDetailCategoryNameInputEditCategory.Text = "Category Name"
        Me.Label_CategoryDetailCategoryNameInputEditCategory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Textbox_CategoryDetailCategoryNameInputEditCategory
        '
        Me.Textbox_CategoryDetailCategoryNameInputEditCategory.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Textbox_CategoryDetailCategoryNameInputEditCategory.AutoCompleteCustomSource.AddRange(New String() {"(001) Item 1", "(002) Item 2", "(003) Item 3", "(004) Item 4", "(005) Item 5", "(006) Item 6", "(007) Item 7", "(008) Item 8", "(009) Item 9", "(010) Item 10"})
        Me.Textbox_CategoryDetailCategoryNameInputEditCategory.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Me.Textbox_CategoryDetailCategoryNameInputEditCategory.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Me.Textbox_CategoryDetailCategoryNameInputEditCategory.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Textbox_CategoryDetailCategoryNameInputEditCategory.DefaultText = ""
        Me.Textbox_CategoryDetailCategoryNameInputEditCategory.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer))
        Me.Textbox_CategoryDetailCategoryNameInputEditCategory.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer))
        Me.Textbox_CategoryDetailCategoryNameInputEditCategory.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Me.Textbox_CategoryDetailCategoryNameInputEditCategory.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Me.Textbox_CategoryDetailCategoryNameInputEditCategory.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Me.Textbox_CategoryDetailCategoryNameInputEditCategory.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Me.Textbox_CategoryDetailCategoryNameInputEditCategory.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
        Me.Textbox_CategoryDetailCategoryNameInputEditCategory.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Me.Textbox_CategoryDetailCategoryNameInputEditCategory.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Me.Textbox_CategoryDetailCategoryNameInputEditCategory.Location = New System.Drawing.Point(0, 16)
        Me.Textbox_CategoryDetailCategoryNameInputEditCategory.Margin = New System.Windows.Forms.Padding(0)
        Me.Textbox_CategoryDetailCategoryNameInputEditCategory.Name = "Textbox_CategoryDetailCategoryNameInputEditCategory"
        Me.Textbox_CategoryDetailCategoryNameInputEditCategory.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.Textbox_CategoryDetailCategoryNameInputEditCategory.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer))
        Me.Textbox_CategoryDetailCategoryNameInputEditCategory.PlaceholderText = "Required"
        Me.Textbox_CategoryDetailCategoryNameInputEditCategory.SelectedText = ""
        Me.Textbox_CategoryDetailCategoryNameInputEditCategory.Size = New System.Drawing.Size(550, 40)
        Me.Textbox_CategoryDetailCategoryNameInputEditCategory.TabIndex = 39
        '
        'Button_MakeCategoryEditCategory
        '
        Me.Button_MakeCategoryEditCategory.BackgroundImage = Global.ERP_SIMS.My.Resources.Resources.Button_SaveChanges
        Me.Button_MakeCategoryEditCategory.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Me.Button_MakeCategoryEditCategory.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Me.Button_MakeCategoryEditCategory.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer))
        Me.Button_MakeCategoryEditCategory.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer))
        Me.Button_MakeCategoryEditCategory.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Me.Button_MakeCategoryEditCategory.Font = New System.Drawing.Font("Segoe UI", 9!)
        Me.Button_MakeCategoryEditCategory.ForeColor = System.Drawing.Color.White
        Me.Button_MakeCategoryEditCategory.Location = New System.Drawing.Point(0, 218)
        Me.Button_MakeCategoryEditCategory.Margin = New System.Windows.Forms.Padding(0, 0, 0, 19)
        Me.Button_MakeCategoryEditCategory.Name = "Button_MakeCategoryEditCategory"
        Me.Button_MakeCategoryEditCategory.Size = New System.Drawing.Size(156, 40)
        Me.Button_MakeCategoryEditCategory.TabIndex = 12
        '
        'Panel_ControlsEditCategory
        '
        Me.Panel_ControlsEditCategory.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Me.Panel_ControlsEditCategory.Controls.Add(Me.Button_BackEditCategory)
        Me.Panel_ControlsEditCategory.Controls.Add(Me.FlowLayoutPanel_PageHeaderEditCategory)
        Me.Panel_ControlsEditCategory.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Me.Panel_ControlsEditCategory.Location = New System.Drawing.Point(0, 0)
        Me.Panel_ControlsEditCategory.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel_ControlsEditCategory.Name = "Panel_ControlsEditCategory"
        Me.Panel_ControlsEditCategory.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Me.Panel_ControlsEditCategory.ShadowDepth = 16
        Me.Panel_ControlsEditCategory.ShadowStyle = Guna.UI2.WinForms.Guna2ShadowPanel.ShadowMode.Dropped
        Me.Panel_ControlsEditCategory.Size = New System.Drawing.Size(1144, 64)
        Me.Panel_ControlsEditCategory.TabIndex = 8
        '
        'Button_BackEditCategory
        '
        Me.Button_BackEditCategory.BackgroundImage = Global.ERP_SIMS.My.Resources.Resources.Button_Back
        Me.Button_BackEditCategory.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Me.Button_BackEditCategory.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Me.Button_BackEditCategory.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer))
        Me.Button_BackEditCategory.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer))
        Me.Button_BackEditCategory.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Me.Button_BackEditCategory.Font = New System.Drawing.Font("Segoe UI", 9!)
        Me.Button_BackEditCategory.ForeColor = System.Drawing.Color.White
        Me.Button_BackEditCategory.Location = New System.Drawing.Point(1099, 25)
        Me.Button_BackEditCategory.Margin = New System.Windows.Forms.Padding(0)
        Me.Button_BackEditCategory.Name = "Button_BackEditCategory"
        Me.Button_BackEditCategory.Size = New System.Drawing.Size(25, 15)
        Me.Button_BackEditCategory.TabIndex = 24
        '
        'FlowLayoutPanel_PageHeaderEditCategory
        '
        Me.FlowLayoutPanel_PageHeaderEditCategory.AutoSize = true
        Me.FlowLayoutPanel_PageHeaderEditCategory.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.FlowLayoutPanel_PageHeaderEditCategory.Controls.Add(Me.Panel_PageHeaderIconEditCategory)
        Me.FlowLayoutPanel_PageHeaderEditCategory.Controls.Add(Me.Label_PageHeaderEditCategory)
        Me.FlowLayoutPanel_PageHeaderEditCategory.Location = New System.Drawing.Point(22, 16)
        Me.FlowLayoutPanel_PageHeaderEditCategory.Margin = New System.Windows.Forms.Padding(0, 0, 0, 42)
        Me.FlowLayoutPanel_PageHeaderEditCategory.Name = "FlowLayoutPanel_PageHeaderEditCategory"
        Me.FlowLayoutPanel_PageHeaderEditCategory.Size = New System.Drawing.Size(332, 32)
        Me.FlowLayoutPanel_PageHeaderEditCategory.TabIndex = 0
        '
        'Panel_PageHeaderIconEditCategory
        '
        Me.Panel_PageHeaderIconEditCategory.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Panel_PageHeaderIconEditCategory.BackgroundImage = Global.ERP_SIMS.My.Resources.Resources.HeaderIcon_Edit
        Me.Panel_PageHeaderIconEditCategory.Location = New System.Drawing.Point(0, 0)
        Me.Panel_PageHeaderIconEditCategory.Margin = New System.Windows.Forms.Padding(0, 0, 16, 0)
        Me.Panel_PageHeaderIconEditCategory.Name = "Panel_PageHeaderIconEditCategory"
        Me.Panel_PageHeaderIconEditCategory.Size = New System.Drawing.Size(32, 32)
        Me.Panel_PageHeaderIconEditCategory.TabIndex = 3
        '
        'Label_PageHeaderEditCategory
        '
        Me.Label_PageHeaderEditCategory.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label_PageHeaderEditCategory.AutoSize = true
        Me.Label_PageHeaderEditCategory.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Me.Label_PageHeaderEditCategory.Font = New System.Drawing.Font("Microsoft JhengHei", 24!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Me.Label_PageHeaderEditCategory.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Me.Label_PageHeaderEditCategory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label_PageHeaderEditCategory.Location = New System.Drawing.Point(48, 0)
        Me.Label_PageHeaderEditCategory.Margin = New System.Windows.Forms.Padding(0)
        Me.Label_PageHeaderEditCategory.Name = "Label_PageHeaderEditCategory"
        Me.Label_PageHeaderEditCategory.Size = New System.Drawing.Size(284, 31)
        Me.Label_PageHeaderEditCategory.TabIndex = 2
        Me.Label_PageHeaderEditCategory.Text = "Editing Category 00000"
        Me.Label_PageHeaderEditCategory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Window_EditCategory
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1384, 800)
        Me.Controls.Add(Me.Background_EditCategory)
        Me.Name = "Window_EditCategory"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Window_EditCategory"
        Me.Background_EditCategory.ResumeLayout(false)
        Me.Panel_MainContentContainerEditCategory.ResumeLayout(false)
        Me.Panel_EditCategory.ResumeLayout(false)
        Me.Panel_EditCategory.PerformLayout
        Me.FlowLayoutPanel_ContentsEditCategory.ResumeLayout(false)
        Me.FlowLayoutPanel_ContentsEditCategory.PerformLayout
        Me.FlowLayoutPanel_CategoryDetailsEditCategory.ResumeLayout(false)
        Me.FlowLayoutPanel_CategoryDetailsEditCategory.PerformLayout
        Me.FlowLayoutPanel_CategoryDetailCategoryIDInputEditCategory.ResumeLayout(false)
        Me.FlowLayoutPanel_CategoryDetailCategoryIDInputEditCategory.PerformLayout
        Me.FlowLayoutPanel_CategoryDetailCategoryNameInputEditCategory.ResumeLayout(false)
        Me.FlowLayoutPanel_CategoryDetailCategoryNameInputEditCategory.PerformLayout
        Me.Panel_ControlsEditCategory.ResumeLayout(false)
        Me.Panel_ControlsEditCategory.PerformLayout
        Me.FlowLayoutPanel_PageHeaderEditCategory.ResumeLayout(false)
        Me.FlowLayoutPanel_PageHeaderEditCategory.PerformLayout
        Me.ResumeLayout(false)

End Sub

    Friend WithEvents Background_EditCategory As Panel
    Friend WithEvents Panel_MainContentContainerEditCategory As Panel
    Friend WithEvents Panel_EditCategory As Panel
    Friend WithEvents FlowLayoutPanel_ContentsEditCategory As FlowLayoutPanel
    Friend WithEvents FlowLayoutPanel_CategoryDetailsEditCategory As FlowLayoutPanel
    Friend WithEvents Label_CategoryDetailsEditCategory As Label
    Friend WithEvents FlowLayoutPanel_CategoryDetailCategoryIDInputEditCategory As FlowLayoutPanel
    Friend WithEvents Label_CategoryDetailCategoryIDInputEditCategory As Label
    Friend WithEvents Textbox_CategoryDetailCategoryIDInputEditCategory As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents FlowLayoutPanel_CategoryDetailCategoryNameInputEditCategory As FlowLayoutPanel
    Friend WithEvents Label_CategoryDetailCategoryNameInputEditCategory As Label
    Friend WithEvents Textbox_CategoryDetailCategoryNameInputEditCategory As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents Button_MakeCategoryEditCategory As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents Panel_ControlsEditCategory As Guna.UI2.WinForms.Guna2ShadowPanel
    Friend WithEvents Button_BackEditCategory As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents FlowLayoutPanel_PageHeaderEditCategory As FlowLayoutPanel
    Friend WithEvents Panel_PageHeaderIconEditCategory As Panel
    Friend WithEvents Label_PageHeaderEditCategory As Label
End Class
