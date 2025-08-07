<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Window_EditBrand
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
        Me.Background_EditBrand = New System.Windows.Forms.Panel()
        Me.Panel_MainContentContainerEditBrand = New System.Windows.Forms.Panel()
        Me.Panel_EditBrand = New System.Windows.Forms.Panel()
        Me.FlowLayoutPanel_ContentsEditBrand = New System.Windows.Forms.FlowLayoutPanel()
        Me.FlowLayoutPanel_BrandDetailsEditBrand = New System.Windows.Forms.FlowLayoutPanel()
        Me.Label_BrandDetailsEditBrand = New System.Windows.Forms.Label()
        Me.FlowLayoutPanel_BrandDetailBrandIDInputEditBrand = New System.Windows.Forms.FlowLayoutPanel()
        Me.Label_BrandDetailBrandIDInputEditBrand = New System.Windows.Forms.Label()
        Me.Textbox_BrandDetailBrandIDInputEditBrand = New Guna.UI2.WinForms.Guna2TextBox()
        Me.FlowLayoutPanel_BrandDetailBrandNameInputEditBrand = New System.Windows.Forms.FlowLayoutPanel()
        Me.Label_BrandDetailBrandNameInputEditBrand = New System.Windows.Forms.Label()
        Me.Textbox_BrandDetailBrandNameInputEditBrand = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Button_MakeBrandEditBrand = New Guna.UI2.WinForms.Guna2Button()
        Me.Panel_ControlsEditBrand = New Guna.UI2.WinForms.Guna2ShadowPanel()
        Me.Button_BackEditBrand = New Guna.UI2.WinForms.Guna2Button()
        Me.FlowLayoutPanel_PageHeaderEditBrand = New System.Windows.Forms.FlowLayoutPanel()
        Me.Panel_PageHeaderIconEditBrand = New System.Windows.Forms.Panel()
        Me.Label_PageHeaderEditBrand = New System.Windows.Forms.Label()
        Me.Background_EditBrand.SuspendLayout
        Me.Panel_MainContentContainerEditBrand.SuspendLayout
        Me.Panel_EditBrand.SuspendLayout
        Me.FlowLayoutPanel_ContentsEditBrand.SuspendLayout
        Me.FlowLayoutPanel_BrandDetailsEditBrand.SuspendLayout
        Me.FlowLayoutPanel_BrandDetailBrandIDInputEditBrand.SuspendLayout
        Me.FlowLayoutPanel_BrandDetailBrandNameInputEditBrand.SuspendLayout
        Me.Panel_ControlsEditBrand.SuspendLayout
        Me.FlowLayoutPanel_PageHeaderEditBrand.SuspendLayout
        Me.SuspendLayout
        '
        'Background_EditBrand
        '
        Me.Background_EditBrand.BackgroundImage = Global.ERP_SIMS.My.Resources.Resources.Image_Background
        Me.Background_EditBrand.Controls.Add(Me.Panel_MainContentContainerEditBrand)
        Me.Background_EditBrand.Location = New System.Drawing.Point(0, 0)
        Me.Background_EditBrand.Margin = New System.Windows.Forms.Padding(0)
        Me.Background_EditBrand.Name = "Background_EditBrand"
        Me.Background_EditBrand.Size = New System.Drawing.Size(1384, 800)
        Me.Background_EditBrand.TabIndex = 3
        '
        'Panel_MainContentContainerEditBrand
        '
        Me.Panel_MainContentContainerEditBrand.Controls.Add(Me.Panel_EditBrand)
        Me.Panel_MainContentContainerEditBrand.Controls.Add(Me.Panel_ControlsEditBrand)
        Me.Panel_MainContentContainerEditBrand.Location = New System.Drawing.Point(120, 0)
        Me.Panel_MainContentContainerEditBrand.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel_MainContentContainerEditBrand.Name = "Panel_MainContentContainerEditBrand"
        Me.Panel_MainContentContainerEditBrand.Size = New System.Drawing.Size(1144, 800)
        Me.Panel_MainContentContainerEditBrand.TabIndex = 7
        '
        'Panel_EditBrand
        '
        Me.Panel_EditBrand.AutoScroll = true
        Me.Panel_EditBrand.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Me.Panel_EditBrand.Controls.Add(Me.FlowLayoutPanel_ContentsEditBrand)
        Me.Panel_EditBrand.Location = New System.Drawing.Point(0, 64)
        Me.Panel_EditBrand.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel_EditBrand.Name = "Panel_EditBrand"
        Me.Panel_EditBrand.Size = New System.Drawing.Size(1144, 736)
        Me.Panel_EditBrand.TabIndex = 7
        '
        'FlowLayoutPanel_ContentsEditBrand
        '
        Me.FlowLayoutPanel_ContentsEditBrand.AutoSize = true
        Me.FlowLayoutPanel_ContentsEditBrand.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.FlowLayoutPanel_ContentsEditBrand.Controls.Add(Me.FlowLayoutPanel_BrandDetailsEditBrand)
        Me.FlowLayoutPanel_ContentsEditBrand.Controls.Add(Me.Button_MakeBrandEditBrand)
        Me.FlowLayoutPanel_ContentsEditBrand.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.FlowLayoutPanel_ContentsEditBrand.Location = New System.Drawing.Point(22, 28)
        Me.FlowLayoutPanel_ContentsEditBrand.Margin = New System.Windows.Forms.Padding(0)
        Me.FlowLayoutPanel_ContentsEditBrand.Name = "FlowLayoutPanel_ContentsEditBrand"
        Me.FlowLayoutPanel_ContentsEditBrand.Size = New System.Drawing.Size(550, 277)
        Me.FlowLayoutPanel_ContentsEditBrand.TabIndex = 0
        '
        'FlowLayoutPanel_BrandDetailsEditBrand
        '
        Me.FlowLayoutPanel_BrandDetailsEditBrand.AutoSize = true
        Me.FlowLayoutPanel_BrandDetailsEditBrand.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.FlowLayoutPanel_BrandDetailsEditBrand.Controls.Add(Me.Label_BrandDetailsEditBrand)
        Me.FlowLayoutPanel_BrandDetailsEditBrand.Controls.Add(Me.FlowLayoutPanel_BrandDetailBrandIDInputEditBrand)
        Me.FlowLayoutPanel_BrandDetailsEditBrand.Controls.Add(Me.FlowLayoutPanel_BrandDetailBrandNameInputEditBrand)
        Me.FlowLayoutPanel_BrandDetailsEditBrand.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.FlowLayoutPanel_BrandDetailsEditBrand.Location = New System.Drawing.Point(0, 0)
        Me.FlowLayoutPanel_BrandDetailsEditBrand.Margin = New System.Windows.Forms.Padding(0, 0, 0, 40)
        Me.FlowLayoutPanel_BrandDetailsEditBrand.Name = "FlowLayoutPanel_BrandDetailsEditBrand"
        Me.FlowLayoutPanel_BrandDetailsEditBrand.Size = New System.Drawing.Size(550, 178)
        Me.FlowLayoutPanel_BrandDetailsEditBrand.TabIndex = 8
        '
        'Label_BrandDetailsEditBrand
        '
        Me.Label_BrandDetailsEditBrand.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label_BrandDetailsEditBrand.AutoSize = true
        Me.Label_BrandDetailsEditBrand.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Me.Label_BrandDetailsEditBrand.Font = New System.Drawing.Font("Microsoft JhengHei", 22!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Me.Label_BrandDetailsEditBrand.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Me.Label_BrandDetailsEditBrand.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label_BrandDetailsEditBrand.Location = New System.Drawing.Point(0, 0)
        Me.Label_BrandDetailsEditBrand.Margin = New System.Windows.Forms.Padding(0, 0, 0, 19)
        Me.Label_BrandDetailsEditBrand.Name = "Label_BrandDetailsEditBrand"
        Me.Label_BrandDetailsEditBrand.Size = New System.Drawing.Size(153, 28)
        Me.Label_BrandDetailsEditBrand.TabIndex = 7
        Me.Label_BrandDetailsEditBrand.Text = "Brand Details"
        Me.Label_BrandDetailsEditBrand.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'FlowLayoutPanel_BrandDetailBrandIDInputEditBrand
        '
        Me.FlowLayoutPanel_BrandDetailBrandIDInputEditBrand.Controls.Add(Me.Label_BrandDetailBrandIDInputEditBrand)
        Me.FlowLayoutPanel_BrandDetailBrandIDInputEditBrand.Controls.Add(Me.Textbox_BrandDetailBrandIDInputEditBrand)
        Me.FlowLayoutPanel_BrandDetailBrandIDInputEditBrand.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.FlowLayoutPanel_BrandDetailBrandIDInputEditBrand.Location = New System.Drawing.Point(0, 47)
        Me.FlowLayoutPanel_BrandDetailBrandIDInputEditBrand.Margin = New System.Windows.Forms.Padding(0, 0, 0, 19)
        Me.FlowLayoutPanel_BrandDetailBrandIDInputEditBrand.Name = "FlowLayoutPanel_BrandDetailBrandIDInputEditBrand"
        Me.FlowLayoutPanel_BrandDetailBrandIDInputEditBrand.Size = New System.Drawing.Size(550, 56)
        Me.FlowLayoutPanel_BrandDetailBrandIDInputEditBrand.TabIndex = 10
        '
        'Label_BrandDetailBrandIDInputEditBrand
        '
        Me.Label_BrandDetailBrandIDInputEditBrand.AutoSize = true
        Me.Label_BrandDetailBrandIDInputEditBrand.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Me.Label_BrandDetailBrandIDInputEditBrand.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Me.Label_BrandDetailBrandIDInputEditBrand.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Me.Label_BrandDetailBrandIDInputEditBrand.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label_BrandDetailBrandIDInputEditBrand.Location = New System.Drawing.Point(0, 0)
        Me.Label_BrandDetailBrandIDInputEditBrand.Margin = New System.Windows.Forms.Padding(0)
        Me.Label_BrandDetailBrandIDInputEditBrand.Name = "Label_BrandDetailBrandIDInputEditBrand"
        Me.Label_BrandDetailBrandIDInputEditBrand.Size = New System.Drawing.Size(55, 16)
        Me.Label_BrandDetailBrandIDInputEditBrand.TabIndex = 2
        Me.Label_BrandDetailBrandIDInputEditBrand.Text = "Brand ID"
        Me.Label_BrandDetailBrandIDInputEditBrand.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Textbox_BrandDetailBrandIDInputEditBrand
        '
        Me.Textbox_BrandDetailBrandIDInputEditBrand.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Me.Textbox_BrandDetailBrandIDInputEditBrand.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Me.Textbox_BrandDetailBrandIDInputEditBrand.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Textbox_BrandDetailBrandIDInputEditBrand.DefaultText = ""
        Me.Textbox_BrandDetailBrandIDInputEditBrand.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer))
        Me.Textbox_BrandDetailBrandIDInputEditBrand.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer))
        Me.Textbox_BrandDetailBrandIDInputEditBrand.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Me.Textbox_BrandDetailBrandIDInputEditBrand.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Me.Textbox_BrandDetailBrandIDInputEditBrand.Enabled = false
        Me.Textbox_BrandDetailBrandIDInputEditBrand.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Me.Textbox_BrandDetailBrandIDInputEditBrand.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Me.Textbox_BrandDetailBrandIDInputEditBrand.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
        Me.Textbox_BrandDetailBrandIDInputEditBrand.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Me.Textbox_BrandDetailBrandIDInputEditBrand.Location = New System.Drawing.Point(0, 16)
        Me.Textbox_BrandDetailBrandIDInputEditBrand.Margin = New System.Windows.Forms.Padding(0)
        Me.Textbox_BrandDetailBrandIDInputEditBrand.Name = "Textbox_BrandDetailBrandIDInputEditBrand"
        Me.Textbox_BrandDetailBrandIDInputEditBrand.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.Textbox_BrandDetailBrandIDInputEditBrand.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer))
        Me.Textbox_BrandDetailBrandIDInputEditBrand.PlaceholderText = "00000"
        Me.Textbox_BrandDetailBrandIDInputEditBrand.SelectedText = ""
        Me.Textbox_BrandDetailBrandIDInputEditBrand.Size = New System.Drawing.Size(550, 40)
        Me.Textbox_BrandDetailBrandIDInputEditBrand.TabIndex = 3
        '
        'FlowLayoutPanel_BrandDetailBrandNameInputEditBrand
        '
        Me.FlowLayoutPanel_BrandDetailBrandNameInputEditBrand.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.FlowLayoutPanel_BrandDetailBrandNameInputEditBrand.Controls.Add(Me.Label_BrandDetailBrandNameInputEditBrand)
        Me.FlowLayoutPanel_BrandDetailBrandNameInputEditBrand.Controls.Add(Me.Textbox_BrandDetailBrandNameInputEditBrand)
        Me.FlowLayoutPanel_BrandDetailBrandNameInputEditBrand.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.FlowLayoutPanel_BrandDetailBrandNameInputEditBrand.Location = New System.Drawing.Point(0, 122)
        Me.FlowLayoutPanel_BrandDetailBrandNameInputEditBrand.Margin = New System.Windows.Forms.Padding(0)
        Me.FlowLayoutPanel_BrandDetailBrandNameInputEditBrand.Name = "FlowLayoutPanel_BrandDetailBrandNameInputEditBrand"
        Me.FlowLayoutPanel_BrandDetailBrandNameInputEditBrand.Size = New System.Drawing.Size(550, 56)
        Me.FlowLayoutPanel_BrandDetailBrandNameInputEditBrand.TabIndex = 8
        '
        'Label_BrandDetailBrandNameInputEditBrand
        '
        Me.Label_BrandDetailBrandNameInputEditBrand.AutoSize = true
        Me.Label_BrandDetailBrandNameInputEditBrand.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Me.Label_BrandDetailBrandNameInputEditBrand.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Me.Label_BrandDetailBrandNameInputEditBrand.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Me.Label_BrandDetailBrandNameInputEditBrand.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label_BrandDetailBrandNameInputEditBrand.Location = New System.Drawing.Point(0, 0)
        Me.Label_BrandDetailBrandNameInputEditBrand.Margin = New System.Windows.Forms.Padding(0)
        Me.Label_BrandDetailBrandNameInputEditBrand.Name = "Label_BrandDetailBrandNameInputEditBrand"
        Me.Label_BrandDetailBrandNameInputEditBrand.Size = New System.Drawing.Size(78, 16)
        Me.Label_BrandDetailBrandNameInputEditBrand.TabIndex = 2
        Me.Label_BrandDetailBrandNameInputEditBrand.Text = "Brand Name"
        Me.Label_BrandDetailBrandNameInputEditBrand.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Textbox_BrandDetailBrandNameInputEditBrand
        '
        Me.Textbox_BrandDetailBrandNameInputEditBrand.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Textbox_BrandDetailBrandNameInputEditBrand.AutoCompleteCustomSource.AddRange(New String() {"(001) Item 1", "(002) Item 2", "(003) Item 3", "(004) Item 4", "(005) Item 5", "(006) Item 6", "(007) Item 7", "(008) Item 8", "(009) Item 9", "(010) Item 10"})
        Me.Textbox_BrandDetailBrandNameInputEditBrand.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Me.Textbox_BrandDetailBrandNameInputEditBrand.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Me.Textbox_BrandDetailBrandNameInputEditBrand.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Textbox_BrandDetailBrandNameInputEditBrand.DefaultText = ""
        Me.Textbox_BrandDetailBrandNameInputEditBrand.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer))
        Me.Textbox_BrandDetailBrandNameInputEditBrand.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer))
        Me.Textbox_BrandDetailBrandNameInputEditBrand.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Me.Textbox_BrandDetailBrandNameInputEditBrand.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Me.Textbox_BrandDetailBrandNameInputEditBrand.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Me.Textbox_BrandDetailBrandNameInputEditBrand.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Me.Textbox_BrandDetailBrandNameInputEditBrand.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
        Me.Textbox_BrandDetailBrandNameInputEditBrand.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Me.Textbox_BrandDetailBrandNameInputEditBrand.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Me.Textbox_BrandDetailBrandNameInputEditBrand.Location = New System.Drawing.Point(0, 16)
        Me.Textbox_BrandDetailBrandNameInputEditBrand.Margin = New System.Windows.Forms.Padding(0)
        Me.Textbox_BrandDetailBrandNameInputEditBrand.Name = "Textbox_BrandDetailBrandNameInputEditBrand"
        Me.Textbox_BrandDetailBrandNameInputEditBrand.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.Textbox_BrandDetailBrandNameInputEditBrand.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer))
        Me.Textbox_BrandDetailBrandNameInputEditBrand.PlaceholderText = "Required"
        Me.Textbox_BrandDetailBrandNameInputEditBrand.SelectedText = ""
        Me.Textbox_BrandDetailBrandNameInputEditBrand.Size = New System.Drawing.Size(550, 40)
        Me.Textbox_BrandDetailBrandNameInputEditBrand.TabIndex = 39
        '
        'Button_MakeBrandEditBrand
        '
        Me.Button_MakeBrandEditBrand.BackgroundImage = Global.ERP_SIMS.My.Resources.Resources.Button_SaveChanges
        Me.Button_MakeBrandEditBrand.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Me.Button_MakeBrandEditBrand.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Me.Button_MakeBrandEditBrand.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer))
        Me.Button_MakeBrandEditBrand.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer))
        Me.Button_MakeBrandEditBrand.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Me.Button_MakeBrandEditBrand.Font = New System.Drawing.Font("Segoe UI", 9!)
        Me.Button_MakeBrandEditBrand.ForeColor = System.Drawing.Color.White
        Me.Button_MakeBrandEditBrand.Location = New System.Drawing.Point(0, 218)
        Me.Button_MakeBrandEditBrand.Margin = New System.Windows.Forms.Padding(0, 0, 0, 19)
        Me.Button_MakeBrandEditBrand.Name = "Button_MakeBrandEditBrand"
        Me.Button_MakeBrandEditBrand.Size = New System.Drawing.Size(156, 40)
        Me.Button_MakeBrandEditBrand.TabIndex = 12
        '
        'Panel_ControlsEditBrand
        '
        Me.Panel_ControlsEditBrand.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Me.Panel_ControlsEditBrand.Controls.Add(Me.Button_BackEditBrand)
        Me.Panel_ControlsEditBrand.Controls.Add(Me.FlowLayoutPanel_PageHeaderEditBrand)
        Me.Panel_ControlsEditBrand.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Me.Panel_ControlsEditBrand.Location = New System.Drawing.Point(0, 0)
        Me.Panel_ControlsEditBrand.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel_ControlsEditBrand.Name = "Panel_ControlsEditBrand"
        Me.Panel_ControlsEditBrand.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Me.Panel_ControlsEditBrand.ShadowDepth = 16
        Me.Panel_ControlsEditBrand.ShadowStyle = Guna.UI2.WinForms.Guna2ShadowPanel.ShadowMode.Dropped
        Me.Panel_ControlsEditBrand.Size = New System.Drawing.Size(1144, 64)
        Me.Panel_ControlsEditBrand.TabIndex = 8
        '
        'Button_BackEditBrand
        '
        Me.Button_BackEditBrand.BackgroundImage = Global.ERP_SIMS.My.Resources.Resources.Button_Back
        Me.Button_BackEditBrand.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Me.Button_BackEditBrand.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Me.Button_BackEditBrand.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer))
        Me.Button_BackEditBrand.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer))
        Me.Button_BackEditBrand.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Me.Button_BackEditBrand.Font = New System.Drawing.Font("Segoe UI", 9!)
        Me.Button_BackEditBrand.ForeColor = System.Drawing.Color.White
        Me.Button_BackEditBrand.Location = New System.Drawing.Point(1099, 25)
        Me.Button_BackEditBrand.Margin = New System.Windows.Forms.Padding(0)
        Me.Button_BackEditBrand.Name = "Button_BackEditBrand"
        Me.Button_BackEditBrand.Size = New System.Drawing.Size(25, 15)
        Me.Button_BackEditBrand.TabIndex = 24
        '
        'FlowLayoutPanel_PageHeaderEditBrand
        '
        Me.FlowLayoutPanel_PageHeaderEditBrand.AutoSize = true
        Me.FlowLayoutPanel_PageHeaderEditBrand.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.FlowLayoutPanel_PageHeaderEditBrand.Controls.Add(Me.Panel_PageHeaderIconEditBrand)
        Me.FlowLayoutPanel_PageHeaderEditBrand.Controls.Add(Me.Label_PageHeaderEditBrand)
        Me.FlowLayoutPanel_PageHeaderEditBrand.Location = New System.Drawing.Point(22, 16)
        Me.FlowLayoutPanel_PageHeaderEditBrand.Margin = New System.Windows.Forms.Padding(0, 0, 0, 42)
        Me.FlowLayoutPanel_PageHeaderEditBrand.Name = "FlowLayoutPanel_PageHeaderEditBrand"
        Me.FlowLayoutPanel_PageHeaderEditBrand.Size = New System.Drawing.Size(295, 32)
        Me.FlowLayoutPanel_PageHeaderEditBrand.TabIndex = 0
        '
        'Panel_PageHeaderIconEditBrand
        '
        Me.Panel_PageHeaderIconEditBrand.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Panel_PageHeaderIconEditBrand.BackgroundImage = Global.ERP_SIMS.My.Resources.Resources.HeaderIcon_Edit
        Me.Panel_PageHeaderIconEditBrand.Location = New System.Drawing.Point(0, 0)
        Me.Panel_PageHeaderIconEditBrand.Margin = New System.Windows.Forms.Padding(0, 0, 16, 0)
        Me.Panel_PageHeaderIconEditBrand.Name = "Panel_PageHeaderIconEditBrand"
        Me.Panel_PageHeaderIconEditBrand.Size = New System.Drawing.Size(32, 32)
        Me.Panel_PageHeaderIconEditBrand.TabIndex = 3
        '
        'Label_PageHeaderEditBrand
        '
        Me.Label_PageHeaderEditBrand.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label_PageHeaderEditBrand.AutoSize = true
        Me.Label_PageHeaderEditBrand.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Me.Label_PageHeaderEditBrand.Font = New System.Drawing.Font("Microsoft JhengHei", 24!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Me.Label_PageHeaderEditBrand.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Me.Label_PageHeaderEditBrand.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label_PageHeaderEditBrand.Location = New System.Drawing.Point(48, 0)
        Me.Label_PageHeaderEditBrand.Margin = New System.Windows.Forms.Padding(0)
        Me.Label_PageHeaderEditBrand.Name = "Label_PageHeaderEditBrand"
        Me.Label_PageHeaderEditBrand.Size = New System.Drawing.Size(247, 31)
        Me.Label_PageHeaderEditBrand.TabIndex = 2
        Me.Label_PageHeaderEditBrand.Text = "Editing Brand 00000"
        Me.Label_PageHeaderEditBrand.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Window_EditBrand
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1384, 800)
        Me.Controls.Add(Me.Background_EditBrand)
        Me.Name = "Window_EditBrand"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Edit Brand"
        Me.Background_EditBrand.ResumeLayout(false)
        Me.Panel_MainContentContainerEditBrand.ResumeLayout(false)
        Me.Panel_EditBrand.ResumeLayout(false)
        Me.Panel_EditBrand.PerformLayout
        Me.FlowLayoutPanel_ContentsEditBrand.ResumeLayout(false)
        Me.FlowLayoutPanel_ContentsEditBrand.PerformLayout
        Me.FlowLayoutPanel_BrandDetailsEditBrand.ResumeLayout(false)
        Me.FlowLayoutPanel_BrandDetailsEditBrand.PerformLayout
        Me.FlowLayoutPanel_BrandDetailBrandIDInputEditBrand.ResumeLayout(false)
        Me.FlowLayoutPanel_BrandDetailBrandIDInputEditBrand.PerformLayout
        Me.FlowLayoutPanel_BrandDetailBrandNameInputEditBrand.ResumeLayout(false)
        Me.FlowLayoutPanel_BrandDetailBrandNameInputEditBrand.PerformLayout
        Me.Panel_ControlsEditBrand.ResumeLayout(false)
        Me.Panel_ControlsEditBrand.PerformLayout
        Me.FlowLayoutPanel_PageHeaderEditBrand.ResumeLayout(false)
        Me.FlowLayoutPanel_PageHeaderEditBrand.PerformLayout
        Me.ResumeLayout(false)

End Sub

    Friend WithEvents Background_EditBrand As Panel
    Friend WithEvents Panel_MainContentContainerEditBrand As Panel
    Friend WithEvents Panel_EditBrand As Panel
    Friend WithEvents FlowLayoutPanel_ContentsEditBrand As FlowLayoutPanel
    Friend WithEvents FlowLayoutPanel_BrandDetailsEditBrand As FlowLayoutPanel
    Friend WithEvents Label_BrandDetailsEditBrand As Label
    Friend WithEvents FlowLayoutPanel_BrandDetailBrandIDInputEditBrand As FlowLayoutPanel
    Friend WithEvents Label_BrandDetailBrandIDInputEditBrand As Label
    Friend WithEvents Textbox_BrandDetailBrandIDInputEditBrand As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents FlowLayoutPanel_BrandDetailBrandNameInputEditBrand As FlowLayoutPanel
    Friend WithEvents Label_BrandDetailBrandNameInputEditBrand As Label
    Friend WithEvents Textbox_BrandDetailBrandNameInputEditBrand As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents Button_MakeBrandEditBrand As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents Panel_ControlsEditBrand As Guna.UI2.WinForms.Guna2ShadowPanel
    Friend WithEvents Button_BackEditBrand As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents FlowLayoutPanel_PageHeaderEditBrand As FlowLayoutPanel
    Friend WithEvents Panel_PageHeaderIconEditBrand As Panel
    Friend WithEvents Label_PageHeaderEditBrand As Label
End Class
