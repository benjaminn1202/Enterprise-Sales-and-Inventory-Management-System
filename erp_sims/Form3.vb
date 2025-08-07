Imports MySql.Data.MySqlClient

Public Class Form3
    Public ConnectionString = "server=localhost;user id=root;password=benjaminn1202;database=sims_db"
    

    Private Sub ShowNewVendorPrompt()
        Dim Prompt_NewVendor As New Form()

        Dim FlowLayoutPanel_NewVendor = New System.Windows.Forms.FlowLayoutPanel()
        Dim FlowLayoutPanel_PageHeaderNewCustomer = New System.Windows.Forms.FlowLayoutPanel()
        Dim Panel_PageHeaderIconNewCustomer = New System.Windows.Forms.Panel()
        Dim Label_PageHeaderNewCustomer = New System.Windows.Forms.Label()
        Dim Label_CustomerDetailsNewCustomer = New System.Windows.Forms.Label()
        Dim FlowLayoutPanel_VendorDetailVendorNameInputNewVendor = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label_VendorDetailVendorNameInputNewVendor = New System.Windows.Forms.Label()
        Dim Textbox_VendorDetailVendorNameInputNewVendor = New Guna.UI2.WinForms.Guna2TextBox()
        Dim FlowLayoutPanel_VendorDetailContactPersonNameInputNewVendor = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label_VendorDetailContactPersonNameInputNewVendor = New System.Windows.Forms.Label()
        Dim Textbox_VendorDetailContactPersonNameInputNewVendor = New Guna.UI2.WinForms.Guna2TextBox()
        Dim FlowLayoutPanel_VendorDetailEmailInputNewVendor = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label_VendorDetailEmailInputNewVendor = New System.Windows.Forms.Label()
        Dim Textbox_VendorDetailEmailInputNewVendor = New Guna.UI2.WinForms.Guna2TextBox()
        Dim FlowLayoutPanel_VendorDetailPhoneInputNewVendor = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label_VendorDetailPhoneInputNewVendor = New System.Windows.Forms.Label()
        Dim Textbox_VendorDetailPhoneInputNewVendor = New Guna.UI2.WinForms.Guna2TextBox()
        Dim Label_VendorDetailAddressLineInputNewVendor = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label_VendorAddressLinePhoneInputNewVendor = New System.Windows.Forms.Label()
        Dim Textbox_VendorDetailAddressLineInputNewVendor = New Guna.UI2.WinForms.Guna2TextBox()
        Dim FlowLayoutPanel_VendorDetailCityInputNewVendor = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label_VendorDetailCityInputNewVendor = New System.Windows.Forms.Label()
        Dim Textbox_VendorDetailCityInputNewVendor = New Guna.UI2.WinForms.Guna2TextBox()
        Dim FlowLayoutPanel_VendorDetailZipCodeInputNewVendor = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label_VendorDetailZipCodeInputNewVendor = New System.Windows.Forms.Label()
        Dim Textbox_VendorDetailZipCodeInputNewVendor = New Guna.UI2.WinForms.Guna2TextBox()
        Dim FlowLayoutPanel_VendorDetailCountryInputNewVendor = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label_VendorDetailCountryInputNewVendor = New System.Windows.Forms.Label()
        Dim Textbox_VendorDetailCountryInputNewVendor = New Guna.UI2.WinForms.Guna2TextBox()
        Dim Button_ResetNewVendor = New Guna.UI2.WinForms.Guna2Button()
        Dim Button_MakeVendorNewVendor = New Guna.UI2.WinForms.Guna2Button()
        Dim FlowLayoutPanel_DateRangeInputsSales = New System.Windows.Forms.FlowLayoutPanel()
        FlowLayoutPanel_NewVendor.SuspendLayout
        FlowLayoutPanel_PageHeaderNewCustomer.SuspendLayout
        FlowLayoutPanel_VendorDetailVendorNameInputNewVendor.SuspendLayout
        FlowLayoutPanel_VendorDetailContactPersonNameInputNewVendor.SuspendLayout
        FlowLayoutPanel_VendorDetailEmailInputNewVendor.SuspendLayout
        FlowLayoutPanel_VendorDetailPhoneInputNewVendor.SuspendLayout
        Label_VendorDetailAddressLineInputNewVendor.SuspendLayout
        FlowLayoutPanel_VendorDetailCityInputNewVendor.SuspendLayout
        FlowLayoutPanel_VendorDetailZipCodeInputNewVendor.SuspendLayout
        FlowLayoutPanel_VendorDetailCountryInputNewVendor.SuspendLayout
        SuspendLayout
        '
        'FlowLayoutPanel_NewVendor
        '
        FlowLayoutPanel_NewVendor.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        FlowLayoutPanel_NewVendor.Controls.Add(FlowLayoutPanel_PageHeaderNewCustomer)
        FlowLayoutPanel_NewVendor.Controls.Add(Label_CustomerDetailsNewCustomer)
        FlowLayoutPanel_NewVendor.Controls.Add(FlowLayoutPanel_VendorDetailVendorNameInputNewVendor)
        FlowLayoutPanel_NewVendor.Controls.Add(FlowLayoutPanel_VendorDetailContactPersonNameInputNewVendor)
        FlowLayoutPanel_NewVendor.Controls.Add(FlowLayoutPanel_VendorDetailEmailInputNewVendor)
        FlowLayoutPanel_NewVendor.Controls.Add(FlowLayoutPanel_VendorDetailPhoneInputNewVendor)
        FlowLayoutPanel_NewVendor.Controls.Add(Label_VendorDetailAddressLineInputNewVendor)
        FlowLayoutPanel_NewVendor.Controls.Add(FlowLayoutPanel_VendorDetailCityInputNewVendor)
        FlowLayoutPanel_NewVendor.Controls.Add(FlowLayoutPanel_VendorDetailZipCodeInputNewVendor)
        FlowLayoutPanel_NewVendor.Controls.Add(FlowLayoutPanel_VendorDetailCountryInputNewVendor)
        FlowLayoutPanel_NewVendor.Controls.Add(Button_ResetNewVendor)
        FlowLayoutPanel_NewVendor.Controls.Add(Button_MakeVendorNewVendor)
        FlowLayoutPanel_NewVendor.Controls.Add(FlowLayoutPanel_DateRangeInputsSales)
        FlowLayoutPanel_NewVendor.Location = New System.Drawing.Point(0, 0)
        FlowLayoutPanel_NewVendor.Margin = New System.Windows.Forms.Padding(0)
        FlowLayoutPanel_NewVendor.Name = "FlowLayoutPanel_NewVendor"
        FlowLayoutPanel_NewVendor.Size = New System.Drawing.Size(720, 720)
        FlowLayoutPanel_NewVendor.TabIndex = 34
        '
        'FlowLayoutPanel_PageHeaderNewCustomer
        '
        FlowLayoutPanel_PageHeaderNewCustomer.AutoSize = true
        FlowLayoutPanel_PageHeaderNewCustomer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_PageHeaderNewCustomer.Controls.Add(Panel_PageHeaderIconNewCustomer)
        FlowLayoutPanel_PageHeaderNewCustomer.Controls.Add(Label_PageHeaderNewCustomer)
        FlowLayoutPanel_PageHeaderNewCustomer.Location = New System.Drawing.Point(12, 24)
        FlowLayoutPanel_PageHeaderNewCustomer.Margin = New System.Windows.Forms.Padding(12, 24, 0, 24)
        FlowLayoutPanel_PageHeaderNewCustomer.Name = "FlowLayoutPanel_PageHeaderNewCustomer"
        FlowLayoutPanel_PageHeaderNewCustomer.Size = New System.Drawing.Size(207, 32)
        FlowLayoutPanel_PageHeaderNewCustomer.TabIndex = 0
        '
        'Panel_PageHeaderIconNewCustomer
        '
        Panel_PageHeaderIconNewCustomer.Anchor = System.Windows.Forms.AnchorStyles.Left
        Panel_PageHeaderIconNewCustomer.BackgroundImage = Global.ERP_SIMS.My.Resources.Resources.HeaderIcon_Vendors
        Panel_PageHeaderIconNewCustomer.Location = New System.Drawing.Point(0, 0)
        Panel_PageHeaderIconNewCustomer.Margin = New System.Windows.Forms.Padding(0, 0, 16, 0)
        Panel_PageHeaderIconNewCustomer.Name = "Panel_PageHeaderIconNewCustomer"
        Panel_PageHeaderIconNewCustomer.Size = New System.Drawing.Size(32, 32)
        Panel_PageHeaderIconNewCustomer.TabIndex = 3
        '
        'Label_PageHeaderNewCustomer
        '
        Label_PageHeaderNewCustomer.Anchor = System.Windows.Forms.AnchorStyles.Left
        Label_PageHeaderNewCustomer.AutoSize = true
        Label_PageHeaderNewCustomer.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_PageHeaderNewCustomer.Font = New System.Drawing.Font("Microsoft JhengHei", 24!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_PageHeaderNewCustomer.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_PageHeaderNewCustomer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_PageHeaderNewCustomer.Location = New System.Drawing.Point(48, 0)
        Label_PageHeaderNewCustomer.Margin = New System.Windows.Forms.Padding(0)
        Label_PageHeaderNewCustomer.Name = "Label_PageHeaderNewCustomer"
        Label_PageHeaderNewCustomer.Size = New System.Drawing.Size(159, 31)
        Label_PageHeaderNewCustomer.TabIndex = 2
        Label_PageHeaderNewCustomer.Text = "New Vendor"
        Label_PageHeaderNewCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label_CustomerDetailsNewCustomer
        '
        Label_CustomerDetailsNewCustomer.Anchor = System.Windows.Forms.AnchorStyles.Left
        Label_CustomerDetailsNewCustomer.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_CustomerDetailsNewCustomer.Font = New System.Drawing.Font("Microsoft JhengHei", 22!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_CustomerDetailsNewCustomer.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Label_CustomerDetailsNewCustomer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_CustomerDetailsNewCustomer.Location = New System.Drawing.Point(12, 80)
        Label_CustomerDetailsNewCustomer.Margin = New System.Windows.Forms.Padding(12, 0, 12, 24)
        Label_CustomerDetailsNewCustomer.Name = "Label_CustomerDetailsNewCustomer"
        Label_CustomerDetailsNewCustomer.Size = New System.Drawing.Size(696, 28)
        Label_CustomerDetailsNewCustomer.TabIndex = 18
        Label_CustomerDetailsNewCustomer.Text = "Vendor Details"
        Label_CustomerDetailsNewCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'FlowLayoutPanel_VendorDetailVendorNameInputNewVendor
        '
        FlowLayoutPanel_VendorDetailVendorNameInputNewVendor.AutoSize = true
        FlowLayoutPanel_VendorDetailVendorNameInputNewVendor.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_VendorDetailVendorNameInputNewVendor.Controls.Add(Label_VendorDetailVendorNameInputNewVendor)
        FlowLayoutPanel_VendorDetailVendorNameInputNewVendor.Controls.Add(Textbox_VendorDetailVendorNameInputNewVendor)
        FlowLayoutPanel_VendorDetailVendorNameInputNewVendor.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_VendorDetailVendorNameInputNewVendor.Location = New System.Drawing.Point(12, 132)
        FlowLayoutPanel_VendorDetailVendorNameInputNewVendor.Margin = New System.Windows.Forms.Padding(12, 0, 6, 12)
        FlowLayoutPanel_VendorDetailVendorNameInputNewVendor.Name = "FlowLayoutPanel_VendorDetailVendorNameInputNewVendor"
        FlowLayoutPanel_VendorDetailVendorNameInputNewVendor.Size = New System.Drawing.Size(342, 56)
        FlowLayoutPanel_VendorDetailVendorNameInputNewVendor.TabIndex = 19
        '
        'Label_VendorDetailVendorNameInputNewVendor
        '
        Label_VendorDetailVendorNameInputNewVendor.AutoSize = true
        Label_VendorDetailVendorNameInputNewVendor.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_VendorDetailVendorNameInputNewVendor.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_VendorDetailVendorNameInputNewVendor.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_VendorDetailVendorNameInputNewVendor.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_VendorDetailVendorNameInputNewVendor.Location = New System.Drawing.Point(0, 0)
        Label_VendorDetailVendorNameInputNewVendor.Margin = New System.Windows.Forms.Padding(0)
        Label_VendorDetailVendorNameInputNewVendor.Name = "Label_VendorDetailVendorNameInputNewVendor"
        Label_VendorDetailVendorNameInputNewVendor.Size = New System.Drawing.Size(87, 16)
        Label_VendorDetailVendorNameInputNewVendor.TabIndex = 2
        Label_VendorDetailVendorNameInputNewVendor.Text = "Vendor Name"
        Label_VendorDetailVendorNameInputNewVendor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Textbox_VendorDetailVendorNameInputNewVendor
        '
        Textbox_VendorDetailVendorNameInputNewVendor.Anchor = System.Windows.Forms.AnchorStyles.Left
        Textbox_VendorDetailVendorNameInputNewVendor.AutoCompleteCustomSource.AddRange(New String() {"(001) Item 1", "(002) Item 2", "(003) Item 3", "(004) Item 4", "(005) Item 5", "(006) Item 6", "(007) Item 7", "(008) Item 8", "(009) Item 9", "(010) Item 10"})
        Textbox_VendorDetailVendorNameInputNewVendor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Textbox_VendorDetailVendorNameInputNewVendor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Textbox_VendorDetailVendorNameInputNewVendor.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_VendorDetailVendorNameInputNewVendor.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_VendorDetailVendorNameInputNewVendor.Cursor = System.Windows.Forms.Cursors.IBeam
        Textbox_VendorDetailVendorNameInputNewVendor.DefaultText = ""
        Textbox_VendorDetailVendorNameInputNewVendor.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer))
        Textbox_VendorDetailVendorNameInputNewVendor.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer))
        Textbox_VendorDetailVendorNameInputNewVendor.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_VendorDetailVendorNameInputNewVendor.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_VendorDetailVendorNameInputNewVendor.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_VendorDetailVendorNameInputNewVendor.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_VendorDetailVendorNameInputNewVendor.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
        Textbox_VendorDetailVendorNameInputNewVendor.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_VendorDetailVendorNameInputNewVendor.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_VendorDetailVendorNameInputNewVendor.Location = New System.Drawing.Point(0, 16)
        Textbox_VendorDetailVendorNameInputNewVendor.Margin = New System.Windows.Forms.Padding(0)
        Textbox_VendorDetailVendorNameInputNewVendor.Name = "Textbox_VendorDetailVendorNameInputNewVendor"
        Textbox_VendorDetailVendorNameInputNewVendor.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Textbox_VendorDetailVendorNameInputNewVendor.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer))
        Textbox_VendorDetailVendorNameInputNewVendor.PlaceholderText = "Required"
        Textbox_VendorDetailVendorNameInputNewVendor.SelectedText = ""
        Textbox_VendorDetailVendorNameInputNewVendor.Size = New System.Drawing.Size(342, 40)
        Textbox_VendorDetailVendorNameInputNewVendor.TabIndex = 39
        '
        'FlowLayoutPanel_VendorDetailContactPersonNameInputNewVendor
        '
        FlowLayoutPanel_VendorDetailContactPersonNameInputNewVendor.AutoSize = true
        FlowLayoutPanel_VendorDetailContactPersonNameInputNewVendor.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_VendorDetailContactPersonNameInputNewVendor.Controls.Add(Label_VendorDetailContactPersonNameInputNewVendor)
        FlowLayoutPanel_VendorDetailContactPersonNameInputNewVendor.Controls.Add(Textbox_VendorDetailContactPersonNameInputNewVendor)
        FlowLayoutPanel_VendorDetailContactPersonNameInputNewVendor.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_VendorDetailContactPersonNameInputNewVendor.Location = New System.Drawing.Point(366, 132)
        FlowLayoutPanel_VendorDetailContactPersonNameInputNewVendor.Margin = New System.Windows.Forms.Padding(6, 0, 12, 12)
        FlowLayoutPanel_VendorDetailContactPersonNameInputNewVendor.Name = "FlowLayoutPanel_VendorDetailContactPersonNameInputNewVendor"
        FlowLayoutPanel_VendorDetailContactPersonNameInputNewVendor.Size = New System.Drawing.Size(342, 56)
        FlowLayoutPanel_VendorDetailContactPersonNameInputNewVendor.TabIndex = 20
        '
        'Label_VendorDetailContactPersonNameInputNewVendor
        '
        Label_VendorDetailContactPersonNameInputNewVendor.AutoSize = true
        Label_VendorDetailContactPersonNameInputNewVendor.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_VendorDetailContactPersonNameInputNewVendor.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_VendorDetailContactPersonNameInputNewVendor.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_VendorDetailContactPersonNameInputNewVendor.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_VendorDetailContactPersonNameInputNewVendor.Location = New System.Drawing.Point(0, 0)
        Label_VendorDetailContactPersonNameInputNewVendor.Margin = New System.Windows.Forms.Padding(0)
        Label_VendorDetailContactPersonNameInputNewVendor.Name = "Label_VendorDetailContactPersonNameInputNewVendor"
        Label_VendorDetailContactPersonNameInputNewVendor.Size = New System.Drawing.Size(130, 16)
        Label_VendorDetailContactPersonNameInputNewVendor.TabIndex = 2
        Label_VendorDetailContactPersonNameInputNewVendor.Text = "Contact Person Name"
        Label_VendorDetailContactPersonNameInputNewVendor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Textbox_VendorDetailContactPersonNameInputNewVendor
        '
        Textbox_VendorDetailContactPersonNameInputNewVendor.Anchor = System.Windows.Forms.AnchorStyles.Left
        Textbox_VendorDetailContactPersonNameInputNewVendor.AutoCompleteCustomSource.AddRange(New String() {"(001) Item 1", "(002) Item 2", "(003) Item 3", "(004) Item 4", "(005) Item 5", "(006) Item 6", "(007) Item 7", "(008) Item 8", "(009) Item 9", "(010) Item 10"})
        Textbox_VendorDetailContactPersonNameInputNewVendor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Textbox_VendorDetailContactPersonNameInputNewVendor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Textbox_VendorDetailContactPersonNameInputNewVendor.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_VendorDetailContactPersonNameInputNewVendor.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_VendorDetailContactPersonNameInputNewVendor.Cursor = System.Windows.Forms.Cursors.IBeam
        Textbox_VendorDetailContactPersonNameInputNewVendor.DefaultText = ""
        Textbox_VendorDetailContactPersonNameInputNewVendor.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer))
        Textbox_VendorDetailContactPersonNameInputNewVendor.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer))
        Textbox_VendorDetailContactPersonNameInputNewVendor.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_VendorDetailContactPersonNameInputNewVendor.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_VendorDetailContactPersonNameInputNewVendor.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_VendorDetailContactPersonNameInputNewVendor.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_VendorDetailContactPersonNameInputNewVendor.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
        Textbox_VendorDetailContactPersonNameInputNewVendor.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_VendorDetailContactPersonNameInputNewVendor.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_VendorDetailContactPersonNameInputNewVendor.Location = New System.Drawing.Point(0, 16)
        Textbox_VendorDetailContactPersonNameInputNewVendor.Margin = New System.Windows.Forms.Padding(0)
        Textbox_VendorDetailContactPersonNameInputNewVendor.Name = "Textbox_VendorDetailContactPersonNameInputNewVendor"
        Textbox_VendorDetailContactPersonNameInputNewVendor.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Textbox_VendorDetailContactPersonNameInputNewVendor.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer))
        Textbox_VendorDetailContactPersonNameInputNewVendor.PlaceholderText = "Required"
        Textbox_VendorDetailContactPersonNameInputNewVendor.SelectedText = ""
        Textbox_VendorDetailContactPersonNameInputNewVendor.Size = New System.Drawing.Size(342, 40)
        Textbox_VendorDetailContactPersonNameInputNewVendor.TabIndex = 39
        '
        'FlowLayoutPanel_VendorDetailEmailInputNewVendor
        '
        FlowLayoutPanel_VendorDetailEmailInputNewVendor.AutoSize = true
        FlowLayoutPanel_VendorDetailEmailInputNewVendor.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_VendorDetailEmailInputNewVendor.Controls.Add(Label_VendorDetailEmailInputNewVendor)
        FlowLayoutPanel_VendorDetailEmailInputNewVendor.Controls.Add(Textbox_VendorDetailEmailInputNewVendor)
        FlowLayoutPanel_VendorDetailEmailInputNewVendor.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_VendorDetailEmailInputNewVendor.Location = New System.Drawing.Point(12, 200)
        FlowLayoutPanel_VendorDetailEmailInputNewVendor.Margin = New System.Windows.Forms.Padding(12, 0, 6, 12)
        FlowLayoutPanel_VendorDetailEmailInputNewVendor.Name = "FlowLayoutPanel_VendorDetailEmailInputNewVendor"
        FlowLayoutPanel_VendorDetailEmailInputNewVendor.Size = New System.Drawing.Size(342, 56)
        FlowLayoutPanel_VendorDetailEmailInputNewVendor.TabIndex = 21
        '
        'Label_VendorDetailEmailInputNewVendor
        '
        Label_VendorDetailEmailInputNewVendor.AutoSize = true
        Label_VendorDetailEmailInputNewVendor.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_VendorDetailEmailInputNewVendor.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_VendorDetailEmailInputNewVendor.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_VendorDetailEmailInputNewVendor.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_VendorDetailEmailInputNewVendor.Location = New System.Drawing.Point(0, 0)
        Label_VendorDetailEmailInputNewVendor.Margin = New System.Windows.Forms.Padding(0)
        Label_VendorDetailEmailInputNewVendor.Name = "Label_VendorDetailEmailInputNewVendor"
        Label_VendorDetailEmailInputNewVendor.Size = New System.Drawing.Size(38, 16)
        Label_VendorDetailEmailInputNewVendor.TabIndex = 2
        Label_VendorDetailEmailInputNewVendor.Text = "Email"
        Label_VendorDetailEmailInputNewVendor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Textbox_VendorDetailEmailInputNewVendor
        '
        Textbox_VendorDetailEmailInputNewVendor.Anchor = System.Windows.Forms.AnchorStyles.Left
        Textbox_VendorDetailEmailInputNewVendor.AutoCompleteCustomSource.AddRange(New String() {"(001) Item 1", "(002) Item 2", "(003) Item 3", "(004) Item 4", "(005) Item 5", "(006) Item 6", "(007) Item 7", "(008) Item 8", "(009) Item 9", "(010) Item 10"})
        Textbox_VendorDetailEmailInputNewVendor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Textbox_VendorDetailEmailInputNewVendor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Textbox_VendorDetailEmailInputNewVendor.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_VendorDetailEmailInputNewVendor.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_VendorDetailEmailInputNewVendor.Cursor = System.Windows.Forms.Cursors.IBeam
        Textbox_VendorDetailEmailInputNewVendor.DefaultText = ""
        Textbox_VendorDetailEmailInputNewVendor.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer))
        Textbox_VendorDetailEmailInputNewVendor.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer))
        Textbox_VendorDetailEmailInputNewVendor.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_VendorDetailEmailInputNewVendor.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_VendorDetailEmailInputNewVendor.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_VendorDetailEmailInputNewVendor.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_VendorDetailEmailInputNewVendor.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
        Textbox_VendorDetailEmailInputNewVendor.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_VendorDetailEmailInputNewVendor.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_VendorDetailEmailInputNewVendor.Location = New System.Drawing.Point(0, 16)
        Textbox_VendorDetailEmailInputNewVendor.Margin = New System.Windows.Forms.Padding(0)
        Textbox_VendorDetailEmailInputNewVendor.Name = "Textbox_VendorDetailEmailInputNewVendor"
        Textbox_VendorDetailEmailInputNewVendor.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Textbox_VendorDetailEmailInputNewVendor.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer))
        Textbox_VendorDetailEmailInputNewVendor.PlaceholderText = "Required"
        Textbox_VendorDetailEmailInputNewVendor.SelectedText = ""
        Textbox_VendorDetailEmailInputNewVendor.Size = New System.Drawing.Size(342, 40)
        Textbox_VendorDetailEmailInputNewVendor.TabIndex = 39
        '
        'FlowLayoutPanel_VendorDetailPhoneInputNewVendor
        '
        FlowLayoutPanel_VendorDetailPhoneInputNewVendor.AutoSize = true
        FlowLayoutPanel_VendorDetailPhoneInputNewVendor.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_VendorDetailPhoneInputNewVendor.Controls.Add(Label_VendorDetailPhoneInputNewVendor)
        FlowLayoutPanel_VendorDetailPhoneInputNewVendor.Controls.Add(Textbox_VendorDetailPhoneInputNewVendor)
        FlowLayoutPanel_VendorDetailPhoneInputNewVendor.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_VendorDetailPhoneInputNewVendor.Location = New System.Drawing.Point(366, 200)
        FlowLayoutPanel_VendorDetailPhoneInputNewVendor.Margin = New System.Windows.Forms.Padding(6, 0, 12, 12)
        FlowLayoutPanel_VendorDetailPhoneInputNewVendor.Name = "FlowLayoutPanel_VendorDetailPhoneInputNewVendor"
        FlowLayoutPanel_VendorDetailPhoneInputNewVendor.Size = New System.Drawing.Size(342, 56)
        FlowLayoutPanel_VendorDetailPhoneInputNewVendor.TabIndex = 22
        '
        'Label_VendorDetailPhoneInputNewVendor
        '
        Label_VendorDetailPhoneInputNewVendor.AutoSize = true
        Label_VendorDetailPhoneInputNewVendor.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_VendorDetailPhoneInputNewVendor.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_VendorDetailPhoneInputNewVendor.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_VendorDetailPhoneInputNewVendor.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_VendorDetailPhoneInputNewVendor.Location = New System.Drawing.Point(0, 0)
        Label_VendorDetailPhoneInputNewVendor.Margin = New System.Windows.Forms.Padding(0)
        Label_VendorDetailPhoneInputNewVendor.Name = "Label_VendorDetailPhoneInputNewVendor"
        Label_VendorDetailPhoneInputNewVendor.Size = New System.Drawing.Size(43, 16)
        Label_VendorDetailPhoneInputNewVendor.TabIndex = 2
        Label_VendorDetailPhoneInputNewVendor.Text = "Phone"
        Label_VendorDetailPhoneInputNewVendor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Textbox_VendorDetailPhoneInputNewVendor
        '
        Textbox_VendorDetailPhoneInputNewVendor.Anchor = System.Windows.Forms.AnchorStyles.Left
        Textbox_VendorDetailPhoneInputNewVendor.AutoCompleteCustomSource.AddRange(New String() {"(001) Item 1", "(002) Item 2", "(003) Item 3", "(004) Item 4", "(005) Item 5", "(006) Item 6", "(007) Item 7", "(008) Item 8", "(009) Item 9", "(010) Item 10"})
        Textbox_VendorDetailPhoneInputNewVendor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Textbox_VendorDetailPhoneInputNewVendor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Textbox_VendorDetailPhoneInputNewVendor.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_VendorDetailPhoneInputNewVendor.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_VendorDetailPhoneInputNewVendor.Cursor = System.Windows.Forms.Cursors.IBeam
        Textbox_VendorDetailPhoneInputNewVendor.DefaultText = ""
        Textbox_VendorDetailPhoneInputNewVendor.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer))
        Textbox_VendorDetailPhoneInputNewVendor.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer))
        Textbox_VendorDetailPhoneInputNewVendor.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_VendorDetailPhoneInputNewVendor.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_VendorDetailPhoneInputNewVendor.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_VendorDetailPhoneInputNewVendor.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_VendorDetailPhoneInputNewVendor.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
        Textbox_VendorDetailPhoneInputNewVendor.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_VendorDetailPhoneInputNewVendor.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_VendorDetailPhoneInputNewVendor.Location = New System.Drawing.Point(0, 16)
        Textbox_VendorDetailPhoneInputNewVendor.Margin = New System.Windows.Forms.Padding(0)
        Textbox_VendorDetailPhoneInputNewVendor.Name = "Textbox_VendorDetailPhoneInputNewVendor"
        Textbox_VendorDetailPhoneInputNewVendor.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Textbox_VendorDetailPhoneInputNewVendor.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer))
        Textbox_VendorDetailPhoneInputNewVendor.PlaceholderText = "Required"
        Textbox_VendorDetailPhoneInputNewVendor.SelectedText = ""
        Textbox_VendorDetailPhoneInputNewVendor.Size = New System.Drawing.Size(342, 40)
        Textbox_VendorDetailPhoneInputNewVendor.TabIndex = 39
        '
        'Label_VendorDetailAddressLineInputNewVendor
        '
        Label_VendorDetailAddressLineInputNewVendor.AutoSize = true
        Label_VendorDetailAddressLineInputNewVendor.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Label_VendorDetailAddressLineInputNewVendor.Controls.Add(Label_VendorAddressLinePhoneInputNewVendor)
        Label_VendorDetailAddressLineInputNewVendor.Controls.Add(Textbox_VendorDetailAddressLineInputNewVendor)
        Label_VendorDetailAddressLineInputNewVendor.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Label_VendorDetailAddressLineInputNewVendor.Location = New System.Drawing.Point(12, 268)
        Label_VendorDetailAddressLineInputNewVendor.Margin = New System.Windows.Forms.Padding(12, 0, 6, 12)
        Label_VendorDetailAddressLineInputNewVendor.Name = "Label_VendorDetailAddressLineInputNewVendor"
        Label_VendorDetailAddressLineInputNewVendor.Size = New System.Drawing.Size(342, 56)
        Label_VendorDetailAddressLineInputNewVendor.TabIndex = 23
        '
        'Label_VendorAddressLinePhoneInputNewVendor
        '
        Label_VendorAddressLinePhoneInputNewVendor.AutoSize = true
        Label_VendorAddressLinePhoneInputNewVendor.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_VendorAddressLinePhoneInputNewVendor.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_VendorAddressLinePhoneInputNewVendor.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_VendorAddressLinePhoneInputNewVendor.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_VendorAddressLinePhoneInputNewVendor.Location = New System.Drawing.Point(0, 0)
        Label_VendorAddressLinePhoneInputNewVendor.Margin = New System.Windows.Forms.Padding(0)
        Label_VendorAddressLinePhoneInputNewVendor.Name = "Label_VendorAddressLinePhoneInputNewVendor"
        Label_VendorAddressLinePhoneInputNewVendor.Size = New System.Drawing.Size(78, 16)
        Label_VendorAddressLinePhoneInputNewVendor.TabIndex = 2
        Label_VendorAddressLinePhoneInputNewVendor.Text = "Address Line"
        Label_VendorAddressLinePhoneInputNewVendor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Textbox_VendorDetailAddressLineInputNewVendor
        '
        Textbox_VendorDetailAddressLineInputNewVendor.Anchor = System.Windows.Forms.AnchorStyles.Left
        Textbox_VendorDetailAddressLineInputNewVendor.AutoCompleteCustomSource.AddRange(New String() {"(001) Item 1", "(002) Item 2", "(003) Item 3", "(004) Item 4", "(005) Item 5", "(006) Item 6", "(007) Item 7", "(008) Item 8", "(009) Item 9", "(010) Item 10"})
        Textbox_VendorDetailAddressLineInputNewVendor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Textbox_VendorDetailAddressLineInputNewVendor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Textbox_VendorDetailAddressLineInputNewVendor.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_VendorDetailAddressLineInputNewVendor.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_VendorDetailAddressLineInputNewVendor.Cursor = System.Windows.Forms.Cursors.IBeam
        Textbox_VendorDetailAddressLineInputNewVendor.DefaultText = ""
        Textbox_VendorDetailAddressLineInputNewVendor.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer))
        Textbox_VendorDetailAddressLineInputNewVendor.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer))
        Textbox_VendorDetailAddressLineInputNewVendor.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_VendorDetailAddressLineInputNewVendor.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_VendorDetailAddressLineInputNewVendor.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_VendorDetailAddressLineInputNewVendor.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_VendorDetailAddressLineInputNewVendor.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
        Textbox_VendorDetailAddressLineInputNewVendor.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_VendorDetailAddressLineInputNewVendor.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_VendorDetailAddressLineInputNewVendor.Location = New System.Drawing.Point(0, 16)
        Textbox_VendorDetailAddressLineInputNewVendor.Margin = New System.Windows.Forms.Padding(0)
        Textbox_VendorDetailAddressLineInputNewVendor.Name = "Textbox_VendorDetailAddressLineInputNewVendor"
        Textbox_VendorDetailAddressLineInputNewVendor.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Textbox_VendorDetailAddressLineInputNewVendor.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer))
        Textbox_VendorDetailAddressLineInputNewVendor.PlaceholderText = "Required"
        Textbox_VendorDetailAddressLineInputNewVendor.SelectedText = ""
        Textbox_VendorDetailAddressLineInputNewVendor.Size = New System.Drawing.Size(342, 40)
        Textbox_VendorDetailAddressLineInputNewVendor.TabIndex = 39
        '
        'FlowLayoutPanel_VendorDetailCityInputNewVendor
        '
        FlowLayoutPanel_VendorDetailCityInputNewVendor.AutoSize = true
        FlowLayoutPanel_VendorDetailCityInputNewVendor.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_VendorDetailCityInputNewVendor.Controls.Add(Label_VendorDetailCityInputNewVendor)
        FlowLayoutPanel_VendorDetailCityInputNewVendor.Controls.Add(Textbox_VendorDetailCityInputNewVendor)
        FlowLayoutPanel_VendorDetailCityInputNewVendor.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_VendorDetailCityInputNewVendor.Location = New System.Drawing.Point(366, 268)
        FlowLayoutPanel_VendorDetailCityInputNewVendor.Margin = New System.Windows.Forms.Padding(6, 0, 12, 12)
        FlowLayoutPanel_VendorDetailCityInputNewVendor.Name = "FlowLayoutPanel_VendorDetailCityInputNewVendor"
        FlowLayoutPanel_VendorDetailCityInputNewVendor.Size = New System.Drawing.Size(342, 56)
        FlowLayoutPanel_VendorDetailCityInputNewVendor.TabIndex = 24
        '
        'Label_VendorDetailCityInputNewVendor
        '
        Label_VendorDetailCityInputNewVendor.AutoSize = true
        Label_VendorDetailCityInputNewVendor.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_VendorDetailCityInputNewVendor.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_VendorDetailCityInputNewVendor.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_VendorDetailCityInputNewVendor.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_VendorDetailCityInputNewVendor.Location = New System.Drawing.Point(0, 0)
        Label_VendorDetailCityInputNewVendor.Margin = New System.Windows.Forms.Padding(0)
        Label_VendorDetailCityInputNewVendor.Name = "Label_VendorDetailCityInputNewVendor"
        Label_VendorDetailCityInputNewVendor.Size = New System.Drawing.Size(28, 16)
        Label_VendorDetailCityInputNewVendor.TabIndex = 2
        Label_VendorDetailCityInputNewVendor.Text = "City"
        Label_VendorDetailCityInputNewVendor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Textbox_VendorDetailCityInputNewVendor
        '
        Textbox_VendorDetailCityInputNewVendor.Anchor = System.Windows.Forms.AnchorStyles.Left
        Textbox_VendorDetailCityInputNewVendor.AutoCompleteCustomSource.AddRange(New String() {"(001) Item 1", "(002) Item 2", "(003) Item 3", "(004) Item 4", "(005) Item 5", "(006) Item 6", "(007) Item 7", "(008) Item 8", "(009) Item 9", "(010) Item 10"})
        Textbox_VendorDetailCityInputNewVendor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Textbox_VendorDetailCityInputNewVendor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Textbox_VendorDetailCityInputNewVendor.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_VendorDetailCityInputNewVendor.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_VendorDetailCityInputNewVendor.Cursor = System.Windows.Forms.Cursors.IBeam
        Textbox_VendorDetailCityInputNewVendor.DefaultText = ""
        Textbox_VendorDetailCityInputNewVendor.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer))
        Textbox_VendorDetailCityInputNewVendor.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer))
        Textbox_VendorDetailCityInputNewVendor.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_VendorDetailCityInputNewVendor.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_VendorDetailCityInputNewVendor.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_VendorDetailCityInputNewVendor.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_VendorDetailCityInputNewVendor.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
        Textbox_VendorDetailCityInputNewVendor.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_VendorDetailCityInputNewVendor.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_VendorDetailCityInputNewVendor.Location = New System.Drawing.Point(0, 16)
        Textbox_VendorDetailCityInputNewVendor.Margin = New System.Windows.Forms.Padding(0)
        Textbox_VendorDetailCityInputNewVendor.Name = "Textbox_VendorDetailCityInputNewVendor"
        Textbox_VendorDetailCityInputNewVendor.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Textbox_VendorDetailCityInputNewVendor.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer))
        Textbox_VendorDetailCityInputNewVendor.PlaceholderText = "Required"
        Textbox_VendorDetailCityInputNewVendor.SelectedText = ""
        Textbox_VendorDetailCityInputNewVendor.Size = New System.Drawing.Size(342, 40)
        Textbox_VendorDetailCityInputNewVendor.TabIndex = 39
        '
        'FlowLayoutPanel_VendorDetailZipCodeInputNewVendor
        '
        FlowLayoutPanel_VendorDetailZipCodeInputNewVendor.AutoSize = true
        FlowLayoutPanel_VendorDetailZipCodeInputNewVendor.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_VendorDetailZipCodeInputNewVendor.Controls.Add(Label_VendorDetailZipCodeInputNewVendor)
        FlowLayoutPanel_VendorDetailZipCodeInputNewVendor.Controls.Add(Textbox_VendorDetailZipCodeInputNewVendor)
        FlowLayoutPanel_VendorDetailZipCodeInputNewVendor.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_VendorDetailZipCodeInputNewVendor.Location = New System.Drawing.Point(12, 336)
        FlowLayoutPanel_VendorDetailZipCodeInputNewVendor.Margin = New System.Windows.Forms.Padding(12, 0, 6, 12)
        FlowLayoutPanel_VendorDetailZipCodeInputNewVendor.Name = "FlowLayoutPanel_VendorDetailZipCodeInputNewVendor"
        FlowLayoutPanel_VendorDetailZipCodeInputNewVendor.Size = New System.Drawing.Size(342, 56)
        FlowLayoutPanel_VendorDetailZipCodeInputNewVendor.TabIndex = 25
        '
        'Label_VendorDetailZipCodeInputNewVendor
        '
        Label_VendorDetailZipCodeInputNewVendor.AutoSize = true
        Label_VendorDetailZipCodeInputNewVendor.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_VendorDetailZipCodeInputNewVendor.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_VendorDetailZipCodeInputNewVendor.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_VendorDetailZipCodeInputNewVendor.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_VendorDetailZipCodeInputNewVendor.Location = New System.Drawing.Point(0, 0)
        Label_VendorDetailZipCodeInputNewVendor.Margin = New System.Windows.Forms.Padding(0)
        Label_VendorDetailZipCodeInputNewVendor.Name = "Label_VendorDetailZipCodeInputNewVendor"
        Label_VendorDetailZipCodeInputNewVendor.Size = New System.Drawing.Size(59, 16)
        Label_VendorDetailZipCodeInputNewVendor.TabIndex = 2
        Label_VendorDetailZipCodeInputNewVendor.Text = "Zip Code"
        Label_VendorDetailZipCodeInputNewVendor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Textbox_VendorDetailZipCodeInputNewVendor
        '
        Textbox_VendorDetailZipCodeInputNewVendor.Anchor = System.Windows.Forms.AnchorStyles.Left
        Textbox_VendorDetailZipCodeInputNewVendor.AutoCompleteCustomSource.AddRange(New String() {"(001) Item 1", "(002) Item 2", "(003) Item 3", "(004) Item 4", "(005) Item 5", "(006) Item 6", "(007) Item 7", "(008) Item 8", "(009) Item 9", "(010) Item 10"})
        Textbox_VendorDetailZipCodeInputNewVendor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Textbox_VendorDetailZipCodeInputNewVendor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Textbox_VendorDetailZipCodeInputNewVendor.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_VendorDetailZipCodeInputNewVendor.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_VendorDetailZipCodeInputNewVendor.Cursor = System.Windows.Forms.Cursors.IBeam
        Textbox_VendorDetailZipCodeInputNewVendor.DefaultText = ""
        Textbox_VendorDetailZipCodeInputNewVendor.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer))
        Textbox_VendorDetailZipCodeInputNewVendor.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer))
        Textbox_VendorDetailZipCodeInputNewVendor.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_VendorDetailZipCodeInputNewVendor.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_VendorDetailZipCodeInputNewVendor.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_VendorDetailZipCodeInputNewVendor.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_VendorDetailZipCodeInputNewVendor.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
        Textbox_VendorDetailZipCodeInputNewVendor.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_VendorDetailZipCodeInputNewVendor.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_VendorDetailZipCodeInputNewVendor.Location = New System.Drawing.Point(0, 16)
        Textbox_VendorDetailZipCodeInputNewVendor.Margin = New System.Windows.Forms.Padding(0)
        Textbox_VendorDetailZipCodeInputNewVendor.Name = "Textbox_VendorDetailZipCodeInputNewVendor"
        Textbox_VendorDetailZipCodeInputNewVendor.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Textbox_VendorDetailZipCodeInputNewVendor.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer))
        Textbox_VendorDetailZipCodeInputNewVendor.PlaceholderText = "Required"
        Textbox_VendorDetailZipCodeInputNewVendor.SelectedText = ""
        Textbox_VendorDetailZipCodeInputNewVendor.Size = New System.Drawing.Size(342, 40)
        Textbox_VendorDetailZipCodeInputNewVendor.TabIndex = 39
        '
        'FlowLayoutPanel_VendorDetailCountryInputNewVendor
        '
        FlowLayoutPanel_VendorDetailCountryInputNewVendor.AutoSize = true
        FlowLayoutPanel_VendorDetailCountryInputNewVendor.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_VendorDetailCountryInputNewVendor.Controls.Add(Label_VendorDetailCountryInputNewVendor)
        FlowLayoutPanel_VendorDetailCountryInputNewVendor.Controls.Add(Textbox_VendorDetailCountryInputNewVendor)
        FlowLayoutPanel_VendorDetailCountryInputNewVendor.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_VendorDetailCountryInputNewVendor.Location = New System.Drawing.Point(366, 336)
        FlowLayoutPanel_VendorDetailCountryInputNewVendor.Margin = New System.Windows.Forms.Padding(6, 0, 12, 12)
        FlowLayoutPanel_VendorDetailCountryInputNewVendor.Name = "FlowLayoutPanel_VendorDetailCountryInputNewVendor"
        FlowLayoutPanel_VendorDetailCountryInputNewVendor.Size = New System.Drawing.Size(342, 56)
        FlowLayoutPanel_VendorDetailCountryInputNewVendor.TabIndex = 26
        '
        'Label_VendorDetailCountryInputNewVendor
        '
        Label_VendorDetailCountryInputNewVendor.AutoSize = true
        Label_VendorDetailCountryInputNewVendor.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_VendorDetailCountryInputNewVendor.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_VendorDetailCountryInputNewVendor.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_VendorDetailCountryInputNewVendor.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_VendorDetailCountryInputNewVendor.Location = New System.Drawing.Point(0, 0)
        Label_VendorDetailCountryInputNewVendor.Margin = New System.Windows.Forms.Padding(0)
        Label_VendorDetailCountryInputNewVendor.Name = "Label_VendorDetailCountryInputNewVendor"
        Label_VendorDetailCountryInputNewVendor.Size = New System.Drawing.Size(51, 16)
        Label_VendorDetailCountryInputNewVendor.TabIndex = 2
        Label_VendorDetailCountryInputNewVendor.Text = "Country"
        Label_VendorDetailCountryInputNewVendor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Textbox_VendorDetailCountryInputNewVendor
        '
        Textbox_VendorDetailCountryInputNewVendor.Anchor = System.Windows.Forms.AnchorStyles.Left
        Textbox_VendorDetailCountryInputNewVendor.AutoCompleteCustomSource.AddRange(New String() {"(001) Item 1", "(002) Item 2", "(003) Item 3", "(004) Item 4", "(005) Item 5", "(006) Item 6", "(007) Item 7", "(008) Item 8", "(009) Item 9", "(010) Item 10"})
        Textbox_VendorDetailCountryInputNewVendor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Textbox_VendorDetailCountryInputNewVendor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Textbox_VendorDetailCountryInputNewVendor.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_VendorDetailCountryInputNewVendor.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_VendorDetailCountryInputNewVendor.Cursor = System.Windows.Forms.Cursors.IBeam
        Textbox_VendorDetailCountryInputNewVendor.DefaultText = ""
        Textbox_VendorDetailCountryInputNewVendor.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer))
        Textbox_VendorDetailCountryInputNewVendor.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer))
        Textbox_VendorDetailCountryInputNewVendor.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_VendorDetailCountryInputNewVendor.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_VendorDetailCountryInputNewVendor.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_VendorDetailCountryInputNewVendor.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_VendorDetailCountryInputNewVendor.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
        Textbox_VendorDetailCountryInputNewVendor.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_VendorDetailCountryInputNewVendor.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_VendorDetailCountryInputNewVendor.Location = New System.Drawing.Point(0, 16)
        Textbox_VendorDetailCountryInputNewVendor.Margin = New System.Windows.Forms.Padding(0)
        Textbox_VendorDetailCountryInputNewVendor.Name = "Textbox_VendorDetailCountryInputNewVendor"
        Textbox_VendorDetailCountryInputNewVendor.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Textbox_VendorDetailCountryInputNewVendor.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer))
        Textbox_VendorDetailCountryInputNewVendor.PlaceholderText = "Required"
        Textbox_VendorDetailCountryInputNewVendor.SelectedText = ""
        Textbox_VendorDetailCountryInputNewVendor.Size = New System.Drawing.Size(342, 40)
        Textbox_VendorDetailCountryInputNewVendor.TabIndex = 39
        '
        'Button_ResetNewVendor
        '
        Button_ResetNewVendor.BackColor = System.Drawing.Color.FromArgb(CType(CType(220,Byte),Integer), CType(CType(76,Byte),Integer), CType(CType(100,Byte),Integer))
        Button_ResetNewVendor.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Button_ResetNewVendor.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Button_ResetNewVendor.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer))
        Button_ResetNewVendor.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer))
        Button_ResetNewVendor.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Button_ResetNewVendor.Font = New System.Drawing.Font("Segoe UI", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel)
        Button_ResetNewVendor.ForeColor = System.Drawing.Color.White
        Button_ResetNewVendor.Location = New System.Drawing.Point(12, 404)
        Button_ResetNewVendor.Margin = New System.Windows.Forms.Padding(12, 0, 6, 12)
        Button_ResetNewVendor.Name = "Button_ResetNewVendor"
        Button_ResetNewVendor.Size = New System.Drawing.Size(342, 40)
        Button_ResetNewVendor.TabIndex = 34
        Button_ResetNewVendor.Text = "Reset"
        Button_ResetNewVendor.TextOffset = New System.Drawing.Point(0, -2)
        '
        'Button_MakeVendorNewVendor
        '
        Button_MakeVendorNewVendor.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(44,Byte),Integer), CType(CType(62,Byte),Integer))
        Button_MakeVendorNewVendor.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Button_MakeVendorNewVendor.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Button_MakeVendorNewVendor.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer))
        Button_MakeVendorNewVendor.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer))
        Button_MakeVendorNewVendor.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Button_MakeVendorNewVendor.Font = New System.Drawing.Font("Segoe UI", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel)
        Button_MakeVendorNewVendor.ForeColor = System.Drawing.Color.White
        Button_MakeVendorNewVendor.Location = New System.Drawing.Point(366, 404)
        Button_MakeVendorNewVendor.Margin = New System.Windows.Forms.Padding(6, 0, 12, 12)
        Button_MakeVendorNewVendor.Name = "Button_MakeVendorNewVendor"
        Button_MakeVendorNewVendor.Size = New System.Drawing.Size(342, 40)
        Button_MakeVendorNewVendor.TabIndex = 33
        Button_MakeVendorNewVendor.Text = "Make Vendor"
        Button_MakeVendorNewVendor.TextOffset = New System.Drawing.Point(0, -2)
        '
        'FlowLayoutPanel_DateRangeInputsSales
        '
        FlowLayoutPanel_DateRangeInputsSales.AutoSize = true
        FlowLayoutPanel_DateRangeInputsSales.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_DateRangeInputsSales.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_DateRangeInputsSales.Location = New System.Drawing.Point(720, 404)
        FlowLayoutPanel_DateRangeInputsSales.Margin = New System.Windows.Forms.Padding(0, 0, 0, 42)
        FlowLayoutPanel_DateRangeInputsSales.Name = "FlowLayoutPanel_DateRangeInputsSales"
        FlowLayoutPanel_DateRangeInputsSales.Size = New System.Drawing.Size(0, 0)
        FlowLayoutPanel_DateRangeInputsSales.TabIndex = 35
        '
        '_Temporary_NewVendorPrompt
        '
        Prompt_NewVendor.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Prompt_NewVendor.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Prompt_NewVendor.ClientSize = New System.Drawing.Size(720, 720)
        Prompt_NewVendor.Controls.Add(FlowLayoutPanel_NewVendor)
        Prompt_NewVendor.Name = "_Temporary_NewVendorPrompt"
        Prompt_NewVendor.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Prompt_NewVendor.Text = "New Vendor"
        FlowLayoutPanel_NewVendor.ResumeLayout(false)
        FlowLayoutPanel_NewVendor.PerformLayout
        FlowLayoutPanel_PageHeaderNewCustomer.ResumeLayout(false)
        FlowLayoutPanel_PageHeaderNewCustomer.PerformLayout
        FlowLayoutPanel_VendorDetailVendorNameInputNewVendor.ResumeLayout(false)
        FlowLayoutPanel_VendorDetailVendorNameInputNewVendor.PerformLayout
        FlowLayoutPanel_VendorDetailContactPersonNameInputNewVendor.ResumeLayout(false)
        FlowLayoutPanel_VendorDetailContactPersonNameInputNewVendor.PerformLayout
        FlowLayoutPanel_VendorDetailEmailInputNewVendor.ResumeLayout(false)
        FlowLayoutPanel_VendorDetailEmailInputNewVendor.PerformLayout
        FlowLayoutPanel_VendorDetailPhoneInputNewVendor.ResumeLayout(false)
        FlowLayoutPanel_VendorDetailPhoneInputNewVendor.PerformLayout
        Label_VendorDetailAddressLineInputNewVendor.ResumeLayout(false)
        Label_VendorDetailAddressLineInputNewVendor.PerformLayout
        FlowLayoutPanel_VendorDetailCityInputNewVendor.ResumeLayout(false)
        FlowLayoutPanel_VendorDetailCityInputNewVendor.PerformLayout
        FlowLayoutPanel_VendorDetailZipCodeInputNewVendor.ResumeLayout(false)
        FlowLayoutPanel_VendorDetailZipCodeInputNewVendor.PerformLayout
        FlowLayoutPanel_VendorDetailCountryInputNewVendor.ResumeLayout(false)
        FlowLayoutPanel_VendorDetailCountryInputNewVendor.PerformLayout
        ResumeLayout(false)

        AddHandler Button_MakeVendorNewVendor.Click,
        Sub(sender As Object, e As EventArgs)
            If String.IsNullOrWhiteSpace(Textbox_VendorDetailVendorNameInputNewVendor.Text) OrElse
               String.IsNullOrWhiteSpace(Textbox_VendorDetailContactPersonNameInputNewVendor.Text) OrElse
               String.IsNullOrWhiteSpace(Textbox_VendorDetailEmailInputNewVendor.Text) OrElse
               String.IsNullOrWhiteSpace(Textbox_VendorDetailPhoneInputNewVendor.Text) OrElse
               String.IsNullOrWhiteSpace(Textbox_VendorDetailAddressLineInputNewVendor.Text) OrElse
               String.IsNullOrWhiteSpace(Textbox_VendorDetailCityInputNewVendor.Text) OrElse
               String.IsNullOrWhiteSpace(Textbox_VendorDetailZipCodeInputNewVendor.Text) OrElse
               String.IsNullOrWhiteSpace(Textbox_VendorDetailCountryInputNewVendor.Text) Then

                MessageBox.Show("All fields are required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            ' Insert data into the database
            Dim query As String = "INSERT INTO vendors (vendor_name, contact_person, email, phone, address_line, city, zip_code, country, created_by) " &
                                  "VALUES (@vendor_name, @contact_person, @email, @phone, @address_line, @city, @zip_code, @country, @created_by)"
            Try
                Using conn As New MySqlConnection(ConnectionString)
                    conn.Open()
                    Using cmd As New MySqlCommand(query, conn)
                        ' Bind parameters
                        cmd.Parameters.AddWithValue("@vendor_name", Textbox_VendorDetailVendorNameInputNewVendor.Text)
                        cmd.Parameters.AddWithValue("@contact_person", Textbox_VendorDetailContactPersonNameInputNewVendor.Text)
                        cmd.Parameters.AddWithValue("@email", Textbox_VendorDetailEmailInputNewVendor.Text)
                        cmd.Parameters.AddWithValue("@phone", Textbox_VendorDetailPhoneInputNewVendor.Text)
                        cmd.Parameters.AddWithValue("@address_line", Textbox_VendorDetailAddressLineInputNewVendor.Text)
                        cmd.Parameters.AddWithValue("@city", Textbox_VendorDetailCityInputNewVendor.Text)
                        cmd.Parameters.AddWithValue("@zip_code", Textbox_VendorDetailZipCodeInputNewVendor.Text)
                        cmd.Parameters.AddWithValue("@country", Textbox_VendorDetailCountryInputNewVendor.Text)
                        cmd.Parameters.AddWithValue("@created_by", GlobalShared.CurrentUser.AccountId)

                        ' Execute the command
                        cmd.ExecuteNonQuery()
                    End Using
                End Using

                MessageBox.Show("Vendor added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        AddHandler Button_ResetNewVendor.Click,
        Sub()
            Textbox_VendorDetailVendorNameInputNewVendor.Clear()
            Textbox_VendorDetailContactPersonNameInputNewVendor.Clear()
            Textbox_VendorDetailEmailInputNewVendor.Clear()
            Textbox_VendorDetailPhoneInputNewVendor.Clear()
            Textbox_VendorDetailAddressLineInputNewVendor.Clear()
            Textbox_VendorDetailCityInputNewVendor.Clear()
            Textbox_VendorDetailZipCodeInputNewVendor.Clear()
            Textbox_VendorDetailCountryInputNewVendor.Clear()
        End Sub

        Prompt_NewVendor.ShowDialog(Me)
    End Sub

    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ShowNewVendorPrompt()
    End Sub
End Class