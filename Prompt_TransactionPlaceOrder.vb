Public Class Prompt_TransactionPlaceOrder

    Private Sub ShowTransactionPlaceOrderPrompt()
        Dim Prompt_TransactionPlaceOrder As New Form()
        Dim FlowLayoutPanel_WindowHeaderStockPlaceOrder = New System.Windows.Forms.FlowLayoutPanel()
        Dim Panel_WindowHeaderStockPlaceOrder = New System.Windows.Forms.Panel()
        Dim Label_WindowHeaderStockPlaceOrder = New System.Windows.Forms.Label()
        Dim FlowLayoutPanel_PlaceOrder = New System.Windows.Forms.FlowLayoutPanel()
        Dim FlowLayoutPanel1_PlaceOrder = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label_SubHeader1PlaceOrder = New System.Windows.Forms.Label()
        Dim FlowLayoutPanel_PlaceOrderCustomer = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label_PlaceOrderCustomer = New System.Windows.Forms.Label()
        Dim Panel_PlaceOrderCustomer = New System.Windows.Forms.Panel()
        Dim Combobox_PlaceOrderCustomer = New Guna.UI2.WinForms.Guna2ComboBox()
        Dim Button_PlaceOrderNewCustomer = New Guna.UI2.WinForms.Guna2Button()
        Dim FlowLayoutPanel_PlaceOrderCustomerDetails = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label_PlaceOrderCustomerDetails = New System.Windows.Forms.Label()
        Dim Labell_PlaceOrderCustomerDetailsText = New System.Windows.Forms.Label()
        Dim FlowLayoutPanel_PlaceOrderPaymentTerm = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label_PlaceOrderPaymentTerm = New System.Windows.Forms.Label()
        Dim Panel_PlaceOrderPaymentTerm = New System.Windows.Forms.Panel()
        Dim Combobox_PlaceOrderPaymentTerm = New Guna.UI2.WinForms.Guna2ComboBox()
        Dim Button_PlaceOrderNewPaymentTerm = New Guna.UI2.WinForms.Guna2Button()
        Dim FlowLayoutPanel_PlaceOrderPaymentTermDetails = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label_PlaceOrderPaymentTermDetails = New System.Windows.Forms.Label()
        Dim Label_PlaceOrderPaymentTermDetailsText = New System.Windows.Forms.Label()
        Dim FlowLayoutPanel5 = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label_PlaceOrderTax = New System.Windows.Forms.Label()
        Dim Panel_PlaceOrderTax = New System.Windows.Forms.Panel()
        Dim Combobox_PlaceOrderTax = New Guna.UI2.WinForms.Guna2ComboBox()
        Dim Button_PlaceOrderNewTax = New Guna.UI2.WinForms.Guna2Button()
        Dim FlowLayoutPanel_PlaceOrderTaxDetails = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label_PlaceOrderTaxDetails = New System.Windows.Forms.Label()
        Dim Label_PlaceOrderTaxDetailstext = New System.Windows.Forms.Label()
        Dim FlowLayoutPanel_PlaceOrderReceivedAmount = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label_PlaceOrderReceivedAmount = New System.Windows.Forms.Label()
        Dim Textbox_PlaceOrderReceivedAmount = New Guna.UI2.WinForms.Guna2TextBox()
        Dim FlowLayoutPanel2_PlaceOrder = New System.Windows.Forms.FlowLayoutPanel()
        Dim S = New System.Windows.Forms.Label()
        Dim Label_PlaceOrderSubtotal = New System.Windows.Forms.Label()
        Dim Label_PlaceOrderSubtotalValue = New System.Windows.Forms.Label()
        Dim Label_PlaceOrderTaxPercentage = New System.Windows.Forms.Label()
        Dim Label_PlaceOrderTaxPercentageValue = New System.Windows.Forms.Label()
        Dim Label_PlaceOrdertaxAmount = New System.Windows.Forms.Label()
        Dim Label_PlaceOrdertaxAmountValue = New System.Windows.Forms.Label()
        Dim Panel4 = New System.Windows.Forms.Panel()
        Dim Label_PlaceOrderNetTotal = New System.Windows.Forms.Label()
        Dim Label_PlaceOrderNetTotalValue = New System.Windows.Forms.Label()
        Dim Label_PlaceOrderReceived = New System.Windows.Forms.Label()
        Dim Label_PlaceOrderReceivedValue = New System.Windows.Forms.Label()
        Dim Label_PlaceOrderChange = New System.Windows.Forms.Label()
        Dim Label_PlaceOrderChangeValue = New System.Windows.Forms.Label()
        Dim Button_PlaceOrderCompleteTransaction = New Guna.UI2.WinForms.Guna2Button()
        Dim Label_PlaceOrderDue = New System.Windows.Forms.Label()
        Dim Label_PlaceOrderDueValue = New System.Windows.Forms.Label()
        FlowLayoutPanel_WindowHeaderStockPlaceOrder.SuspendLayout
        FlowLayoutPanel_PlaceOrder.SuspendLayout
        FlowLayoutPanel1_PlaceOrder.SuspendLayout
        FlowLayoutPanel_PlaceOrderCustomer.SuspendLayout
        Panel_PlaceOrderCustomer.SuspendLayout
        FlowLayoutPanel_PlaceOrderCustomerDetails.SuspendLayout
        FlowLayoutPanel_PlaceOrderPaymentTerm.SuspendLayout
        Panel_PlaceOrderPaymentTerm.SuspendLayout
        FlowLayoutPanel_PlaceOrderPaymentTermDetails.SuspendLayout
        FlowLayoutPanel5.SuspendLayout
        Panel_PlaceOrderTax.SuspendLayout
        FlowLayoutPanel_PlaceOrderTaxDetails.SuspendLayout
        FlowLayoutPanel_PlaceOrderReceivedAmount.SuspendLayout
        FlowLayoutPanel2_PlaceOrder.SuspendLayout
        SuspendLayout
        '
        'FlowLayoutPanel_WindowHeaderStockPlaceOrder
        '
        FlowLayoutPanel_WindowHeaderStockPlaceOrder.AutoSize = true
        FlowLayoutPanel_WindowHeaderStockPlaceOrder.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_WindowHeaderStockPlaceOrder.Controls.Add(Panel_WindowHeaderStockPlaceOrder)
        FlowLayoutPanel_WindowHeaderStockPlaceOrder.Controls.Add(Label_WindowHeaderStockPlaceOrder)
        FlowLayoutPanel_WindowHeaderStockPlaceOrder.Location = New System.Drawing.Point(12, 24)
        FlowLayoutPanel_WindowHeaderStockPlaceOrder.Margin = New System.Windows.Forms.Padding(12, 24, 0, 24)
        FlowLayoutPanel_WindowHeaderStockPlaceOrder.Name = "FlowLayoutPanel_WindowHeaderStockPlaceOrder"
        FlowLayoutPanel_WindowHeaderStockPlaceOrder.Size = New System.Drawing.Size(197, 32)
        FlowLayoutPanel_WindowHeaderStockPlaceOrder.TabIndex = 31
        '
        'Panel_WindowHeaderStockPlaceOrder
        '
        Panel_WindowHeaderStockPlaceOrder.Anchor = System.Windows.Forms.AnchorStyles.Left
        Panel_WindowHeaderStockPlaceOrder.BackgroundImage = Global.ERP_SIMS.My.Resources.Resources.HeaderIcon_Sale
        Panel_WindowHeaderStockPlaceOrder.Location = New System.Drawing.Point(0, 0)
        Panel_WindowHeaderStockPlaceOrder.Margin = New System.Windows.Forms.Padding(0, 0, 16, 0)
        Panel_WindowHeaderStockPlaceOrder.Name = "Panel_WindowHeaderStockPlaceOrder"
        Panel_WindowHeaderStockPlaceOrder.Size = New System.Drawing.Size(32, 32)
        Panel_WindowHeaderStockPlaceOrder.TabIndex = 3
        '
        'Label_WindowHeaderStockPlaceOrder
        '
        Label_WindowHeaderStockPlaceOrder.Anchor = System.Windows.Forms.AnchorStyles.Left
        Label_WindowHeaderStockPlaceOrder.AutoSize = true
        Label_WindowHeaderStockPlaceOrder.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_WindowHeaderStockPlaceOrder.Font = New System.Drawing.Font("Microsoft JhengHei", 24!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_WindowHeaderStockPlaceOrder.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_WindowHeaderStockPlaceOrder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_WindowHeaderStockPlaceOrder.Location = New System.Drawing.Point(48, 0)
        Label_WindowHeaderStockPlaceOrder.Margin = New System.Windows.Forms.Padding(0)
        Label_WindowHeaderStockPlaceOrder.Name = "Label_WindowHeaderStockPlaceOrder"
        Label_WindowHeaderStockPlaceOrder.Size = New System.Drawing.Size(149, 31)
        Label_WindowHeaderStockPlaceOrder.TabIndex = 2
        Label_WindowHeaderStockPlaceOrder.Text = "Place Order"
        Label_WindowHeaderStockPlaceOrder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'FlowLayoutPanel_PlaceOrder
        '
        FlowLayoutPanel_PlaceOrder.AutoSize = true
        FlowLayoutPanel_PlaceOrder.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_PlaceOrder.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        FlowLayoutPanel_PlaceOrder.Controls.Add(FlowLayoutPanel_WindowHeaderStockPlaceOrder)
        FlowLayoutPanel_PlaceOrder.Location = New System.Drawing.Point(0, 0)
        FlowLayoutPanel_PlaceOrder.Margin = New System.Windows.Forms.Padding(0)
        FlowLayoutPanel_PlaceOrder.MinimumSize = New System.Drawing.Size(720, 0)
        FlowLayoutPanel_PlaceOrder.Name = "FlowLayoutPanel_PlaceOrder"
        FlowLayoutPanel_PlaceOrder.Size = New System.Drawing.Size(720, 80)
        FlowLayoutPanel_PlaceOrder.TabIndex = 36
        '
        'FlowLayoutPanel1_PlaceOrder
        '
        FlowLayoutPanel1_PlaceOrder.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel1_PlaceOrder.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        FlowLayoutPanel1_PlaceOrder.Controls.Add(Label_SubHeader1PlaceOrder)
        FlowLayoutPanel1_PlaceOrder.Controls.Add(FlowLayoutPanel_PlaceOrderCustomer)
        FlowLayoutPanel1_PlaceOrder.Controls.Add(FlowLayoutPanel_PlaceOrderCustomerDetails)
        FlowLayoutPanel1_PlaceOrder.Controls.Add(FlowLayoutPanel_PlaceOrderPaymentTerm)
        FlowLayoutPanel1_PlaceOrder.Controls.Add(FlowLayoutPanel_PlaceOrderPaymentTermDetails)
        FlowLayoutPanel1_PlaceOrder.Controls.Add(FlowLayoutPanel5)
        FlowLayoutPanel1_PlaceOrder.Controls.Add(FlowLayoutPanel_PlaceOrderTaxDetails)
        FlowLayoutPanel1_PlaceOrder.Controls.Add(FlowLayoutPanel_PlaceOrderReceivedAmount)
        FlowLayoutPanel1_PlaceOrder.Location = New System.Drawing.Point(0, 80)
        FlowLayoutPanel1_PlaceOrder.Margin = New System.Windows.Forms.Padding(0)
        FlowLayoutPanel1_PlaceOrder.Name = "FlowLayoutPanel1_PlaceOrder"
        FlowLayoutPanel1_PlaceOrder.Size = New System.Drawing.Size(360, 640)
        FlowLayoutPanel1_PlaceOrder.TabIndex = 37
        '
        'Label_SubHeader1PlaceOrder
        '
        Label_SubHeader1PlaceOrder.Anchor = System.Windows.Forms.AnchorStyles.Left
        Label_SubHeader1PlaceOrder.AutoSize = true
        Label_SubHeader1PlaceOrder.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_SubHeader1PlaceOrder.Font = New System.Drawing.Font("Microsoft JhengHei", 22!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_SubHeader1PlaceOrder.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Label_SubHeader1PlaceOrder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_SubHeader1PlaceOrder.Location = New System.Drawing.Point(12, 0)
        Label_SubHeader1PlaceOrder.Margin = New System.Windows.Forms.Padding(12, 0, 12, 24)
        Label_SubHeader1PlaceOrder.Name = "Label_SubHeader1PlaceOrder"
        Label_SubHeader1PlaceOrder.Size = New System.Drawing.Size(84, 28)
        Label_SubHeader1PlaceOrder.TabIndex = 19
        Label_SubHeader1PlaceOrder.Text = "Details"
        Label_SubHeader1PlaceOrder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'FlowLayoutPanel_PlaceOrderCustomer
        '
        FlowLayoutPanel_PlaceOrderCustomer.AutoSize = true
        FlowLayoutPanel_PlaceOrderCustomer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_PlaceOrderCustomer.Controls.Add(Label_PlaceOrderCustomer)
        FlowLayoutPanel_PlaceOrderCustomer.Controls.Add(Panel_PlaceOrderCustomer)
        FlowLayoutPanel_PlaceOrderCustomer.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_PlaceOrderCustomer.Location = New System.Drawing.Point(12, 52)
        FlowLayoutPanel_PlaceOrderCustomer.Margin = New System.Windows.Forms.Padding(12, 0, 6, 12)
        FlowLayoutPanel_PlaceOrderCustomer.Name = "FlowLayoutPanel_PlaceOrderCustomer"
        FlowLayoutPanel_PlaceOrderCustomer.Size = New System.Drawing.Size(336, 56)
        FlowLayoutPanel_PlaceOrderCustomer.TabIndex = 34
        '
        'Label_PlaceOrderCustomer
        '
        Label_PlaceOrderCustomer.AutoSize = true
        Label_PlaceOrderCustomer.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_PlaceOrderCustomer.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_PlaceOrderCustomer.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_PlaceOrderCustomer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_PlaceOrderCustomer.Location = New System.Drawing.Point(0, 0)
        Label_PlaceOrderCustomer.Margin = New System.Windows.Forms.Padding(0)
        Label_PlaceOrderCustomer.Name = "Label_PlaceOrderCustomer"
        Label_PlaceOrderCustomer.Size = New System.Drawing.Size(61, 16)
        Label_PlaceOrderCustomer.TabIndex = 2
        Label_PlaceOrderCustomer.Text = "Customer"
        Label_PlaceOrderCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Panel_PlaceOrderCustomer
        '
        Panel_PlaceOrderCustomer.Controls.Add(Combobox_PlaceOrderCustomer)
        Panel_PlaceOrderCustomer.Controls.Add(Button_PlaceOrderNewCustomer)
        Panel_PlaceOrderCustomer.Location = New System.Drawing.Point(0, 16)
        Panel_PlaceOrderCustomer.Margin = New System.Windows.Forms.Padding(0)
        Panel_PlaceOrderCustomer.Name = "Panel_PlaceOrderCustomer"
        Panel_PlaceOrderCustomer.Size = New System.Drawing.Size(336, 40)
        Panel_PlaceOrderCustomer.TabIndex = 31
        '
        'Combobox_PlaceOrderCustomer
        '
        Combobox_PlaceOrderCustomer.BackColor = System.Drawing.Color.Transparent
        Combobox_PlaceOrderCustomer.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Combobox_PlaceOrderCustomer.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Combobox_PlaceOrderCustomer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Combobox_PlaceOrderCustomer.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Combobox_PlaceOrderCustomer.FocusedColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Combobox_PlaceOrderCustomer.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Combobox_PlaceOrderCustomer.Font = New System.Drawing.Font("Segoe UI", 10!)
        Combobox_PlaceOrderCustomer.ForeColor = System.Drawing.Color.FromArgb(CType(CType(68,Byte),Integer), CType(CType(88,Byte),Integer), CType(CType(112,Byte),Integer))
        Combobox_PlaceOrderCustomer.ItemHeight = 34
        Combobox_PlaceOrderCustomer.Items.AddRange(New Object() {"Benjamin Rollan", "Benjamin Rollan", "Benjamin Rollan", "Benjamin Rollan"})
        Combobox_PlaceOrderCustomer.Location = New System.Drawing.Point(0, 0)
        Combobox_PlaceOrderCustomer.Margin = New System.Windows.Forms.Padding(0)
        Combobox_PlaceOrderCustomer.Name = "Combobox_PlaceOrderCustomer"
        Combobox_PlaceOrderCustomer.Size = New System.Drawing.Size(197, 40)
        Combobox_PlaceOrderCustomer.TabIndex = 31
        '
        'Button_PlaceOrderNewCustomer
        '
        Button_PlaceOrderNewCustomer.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(44,Byte),Integer), CType(CType(62,Byte),Integer))
        Button_PlaceOrderNewCustomer.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Button_PlaceOrderNewCustomer.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Button_PlaceOrderNewCustomer.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer))
        Button_PlaceOrderNewCustomer.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer))
        Button_PlaceOrderNewCustomer.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Button_PlaceOrderNewCustomer.Font = New System.Drawing.Font("Segoe UI", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel)
        Button_PlaceOrderNewCustomer.ForeColor = System.Drawing.Color.White
        Button_PlaceOrderNewCustomer.Location = New System.Drawing.Point(209, 0)
        Button_PlaceOrderNewCustomer.Margin = New System.Windows.Forms.Padding(12, 0, 0, 0)
        Button_PlaceOrderNewCustomer.Name = "Button_PlaceOrderNewCustomer"
        Button_PlaceOrderNewCustomer.Size = New System.Drawing.Size(127, 40)
        Button_PlaceOrderNewCustomer.TabIndex = 31
        Button_PlaceOrderNewCustomer.Text = "New Customer"
        Button_PlaceOrderNewCustomer.TextOffset = New System.Drawing.Point(0, -2)
        '
        'FlowLayoutPanel_PlaceOrderCustomerDetails
        '
        FlowLayoutPanel_PlaceOrderCustomerDetails.AutoSize = true
        FlowLayoutPanel_PlaceOrderCustomerDetails.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_PlaceOrderCustomerDetails.Controls.Add(Label_PlaceOrderCustomerDetails)
        FlowLayoutPanel_PlaceOrderCustomerDetails.Controls.Add(Labell_PlaceOrderCustomerDetailsText)
        FlowLayoutPanel_PlaceOrderCustomerDetails.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_PlaceOrderCustomerDetails.Location = New System.Drawing.Point(12, 120)
        FlowLayoutPanel_PlaceOrderCustomerDetails.Margin = New System.Windows.Forms.Padding(12, 0, 0, 12)
        FlowLayoutPanel_PlaceOrderCustomerDetails.Name = "FlowLayoutPanel_PlaceOrderCustomerDetails"
        FlowLayoutPanel_PlaceOrderCustomerDetails.Size = New System.Drawing.Size(147, 122)
        FlowLayoutPanel_PlaceOrderCustomerDetails.TabIndex = 38
        '
        'Label_PlaceOrderCustomerDetails
        '
        Label_PlaceOrderCustomerDetails.Anchor = System.Windows.Forms.AnchorStyles.Left
        Label_PlaceOrderCustomerDetails.AutoSize = true
        Label_PlaceOrderCustomerDetails.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_PlaceOrderCustomerDetails.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_PlaceOrderCustomerDetails.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_PlaceOrderCustomerDetails.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_PlaceOrderCustomerDetails.Location = New System.Drawing.Point(0, 0)
        Label_PlaceOrderCustomerDetails.Margin = New System.Windows.Forms.Padding(0, 0, 0, 8)
        Label_PlaceOrderCustomerDetails.Name = "Label_PlaceOrderCustomerDetails"
        Label_PlaceOrderCustomerDetails.Size = New System.Drawing.Size(125, 18)
        Label_PlaceOrderCustomerDetails.TabIndex = 3
        Label_PlaceOrderCustomerDetails.Text = "Customer Details"
        Label_PlaceOrderCustomerDetails.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Labell_PlaceOrderCustomerDetailsText
        '
        Labell_PlaceOrderCustomerDetailsText.Anchor = System.Windows.Forms.AnchorStyles.Left
        Labell_PlaceOrderCustomerDetailsText.AutoSize = true
        Labell_PlaceOrderCustomerDetailsText.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Labell_PlaceOrderCustomerDetailsText.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Labell_PlaceOrderCustomerDetailsText.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Labell_PlaceOrderCustomerDetailsText.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Labell_PlaceOrderCustomerDetailsText.Location = New System.Drawing.Point(0, 26)
        Labell_PlaceOrderCustomerDetailsText.Margin = New System.Windows.Forms.Padding(0)
        Labell_PlaceOrderCustomerDetailsText.Name = "Labell_PlaceOrderCustomerDetailsText"
        Labell_PlaceOrderCustomerDetailsText.Size = New System.Drawing.Size(147, 96)
        Labell_PlaceOrderCustomerDetailsText.TabIndex = 4
        Labell_PlaceOrderCustomerDetailsText.Text = "Customer ID: No value"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Company: No value"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Email: No value"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Phone: No value"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Addre"& _ 
    "ss: No value"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Payment Term: No Value"
        Labell_PlaceOrderCustomerDetailsText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'FlowLayoutPanel_PlaceOrderPaymentTerm
        '
        FlowLayoutPanel_PlaceOrderPaymentTerm.AutoSize = true
        FlowLayoutPanel_PlaceOrderPaymentTerm.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_PlaceOrderPaymentTerm.Controls.Add(Label_PlaceOrderPaymentTerm)
        FlowLayoutPanel_PlaceOrderPaymentTerm.Controls.Add(Panel_PlaceOrderPaymentTerm)
        FlowLayoutPanel_PlaceOrderPaymentTerm.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_PlaceOrderPaymentTerm.Location = New System.Drawing.Point(12, 254)
        FlowLayoutPanel_PlaceOrderPaymentTerm.Margin = New System.Windows.Forms.Padding(12, 0, 6, 12)
        FlowLayoutPanel_PlaceOrderPaymentTerm.Name = "FlowLayoutPanel_PlaceOrderPaymentTerm"
        FlowLayoutPanel_PlaceOrderPaymentTerm.Size = New System.Drawing.Size(336, 56)
        FlowLayoutPanel_PlaceOrderPaymentTerm.TabIndex = 35
        '
        'Label_PlaceOrderPaymentTerm
        '
        Label_PlaceOrderPaymentTerm.AutoSize = true
        Label_PlaceOrderPaymentTerm.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_PlaceOrderPaymentTerm.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_PlaceOrderPaymentTerm.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_PlaceOrderPaymentTerm.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_PlaceOrderPaymentTerm.Location = New System.Drawing.Point(0, 0)
        Label_PlaceOrderPaymentTerm.Margin = New System.Windows.Forms.Padding(0)
        Label_PlaceOrderPaymentTerm.Name = "Label_PlaceOrderPaymentTerm"
        Label_PlaceOrderPaymentTerm.Size = New System.Drawing.Size(88, 16)
        Label_PlaceOrderPaymentTerm.TabIndex = 2
        Label_PlaceOrderPaymentTerm.Text = "Payment Term"
        Label_PlaceOrderPaymentTerm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Panel_PlaceOrderPaymentTerm
        '
        Panel_PlaceOrderPaymentTerm.Controls.Add(Combobox_PlaceOrderPaymentTerm)
        Panel_PlaceOrderPaymentTerm.Controls.Add(Button_PlaceOrderNewPaymentTerm)
        Panel_PlaceOrderPaymentTerm.Location = New System.Drawing.Point(0, 16)
        Panel_PlaceOrderPaymentTerm.Margin = New System.Windows.Forms.Padding(0)
        Panel_PlaceOrderPaymentTerm.Name = "Panel_PlaceOrderPaymentTerm"
        Panel_PlaceOrderPaymentTerm.Size = New System.Drawing.Size(336, 40)
        Panel_PlaceOrderPaymentTerm.TabIndex = 31
        '
        'Combobox_PlaceOrderPaymentTerm
        '
        Combobox_PlaceOrderPaymentTerm.BackColor = System.Drawing.Color.Transparent
        Combobox_PlaceOrderPaymentTerm.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Combobox_PlaceOrderPaymentTerm.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Combobox_PlaceOrderPaymentTerm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Combobox_PlaceOrderPaymentTerm.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Combobox_PlaceOrderPaymentTerm.FocusedColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Combobox_PlaceOrderPaymentTerm.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Combobox_PlaceOrderPaymentTerm.Font = New System.Drawing.Font("Segoe UI", 10!)
        Combobox_PlaceOrderPaymentTerm.ForeColor = System.Drawing.Color.FromArgb(CType(CType(68,Byte),Integer), CType(CType(88,Byte),Integer), CType(CType(112,Byte),Integer))
        Combobox_PlaceOrderPaymentTerm.ItemHeight = 34
        Combobox_PlaceOrderPaymentTerm.Items.AddRange(New Object() {"Benjamin Rollan", "Benjamin Rollan", "Benjamin Rollan", "Benjamin Rollan"})
        Combobox_PlaceOrderPaymentTerm.Location = New System.Drawing.Point(0, 0)
        Combobox_PlaceOrderPaymentTerm.Margin = New System.Windows.Forms.Padding(0)
        Combobox_PlaceOrderPaymentTerm.Name = "Combobox_PlaceOrderPaymentTerm"
        Combobox_PlaceOrderPaymentTerm.Size = New System.Drawing.Size(197, 40)
        Combobox_PlaceOrderPaymentTerm.TabIndex = 31
        '
        'Button_PlaceOrderNewPaymentTerm
        '
        Button_PlaceOrderNewPaymentTerm.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(44,Byte),Integer), CType(CType(62,Byte),Integer))
        Button_PlaceOrderNewPaymentTerm.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Button_PlaceOrderNewPaymentTerm.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Button_PlaceOrderNewPaymentTerm.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer))
        Button_PlaceOrderNewPaymentTerm.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer))
        Button_PlaceOrderNewPaymentTerm.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Button_PlaceOrderNewPaymentTerm.Font = New System.Drawing.Font("Segoe UI", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel)
        Button_PlaceOrderNewPaymentTerm.ForeColor = System.Drawing.Color.White
        Button_PlaceOrderNewPaymentTerm.Location = New System.Drawing.Point(209, 0)
        Button_PlaceOrderNewPaymentTerm.Margin = New System.Windows.Forms.Padding(12, 0, 0, 0)
        Button_PlaceOrderNewPaymentTerm.Name = "Button_PlaceOrderNewPaymentTerm"
        Button_PlaceOrderNewPaymentTerm.Size = New System.Drawing.Size(127, 40)
        Button_PlaceOrderNewPaymentTerm.TabIndex = 31
        Button_PlaceOrderNewPaymentTerm.Text = "New Term"
        Button_PlaceOrderNewPaymentTerm.TextOffset = New System.Drawing.Point(0, -2)
        '
        'FlowLayoutPanel_PlaceOrderPaymentTermDetails
        '
        FlowLayoutPanel_PlaceOrderPaymentTermDetails.AutoSize = true
        FlowLayoutPanel_PlaceOrderPaymentTermDetails.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_PlaceOrderPaymentTermDetails.Controls.Add(Label_PlaceOrderPaymentTermDetails)
        FlowLayoutPanel_PlaceOrderPaymentTermDetails.Controls.Add(Label_PlaceOrderPaymentTermDetailsText)
        FlowLayoutPanel_PlaceOrderPaymentTermDetails.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_PlaceOrderPaymentTermDetails.Location = New System.Drawing.Point(12, 322)
        FlowLayoutPanel_PlaceOrderPaymentTermDetails.Margin = New System.Windows.Forms.Padding(12, 0, 0, 12)
        FlowLayoutPanel_PlaceOrderPaymentTermDetails.Name = "FlowLayoutPanel_PlaceOrderPaymentTermDetails"
        FlowLayoutPanel_PlaceOrderPaymentTermDetails.Size = New System.Drawing.Size(169, 74)
        FlowLayoutPanel_PlaceOrderPaymentTermDetails.TabIndex = 39
        '
        'Label_PlaceOrderPaymentTermDetails
        '
        Label_PlaceOrderPaymentTermDetails.Anchor = System.Windows.Forms.AnchorStyles.Left
        Label_PlaceOrderPaymentTermDetails.AutoSize = true
        Label_PlaceOrderPaymentTermDetails.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_PlaceOrderPaymentTermDetails.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_PlaceOrderPaymentTermDetails.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_PlaceOrderPaymentTermDetails.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_PlaceOrderPaymentTermDetails.Location = New System.Drawing.Point(0, 0)
        Label_PlaceOrderPaymentTermDetails.Margin = New System.Windows.Forms.Padding(0, 0, 0, 8)
        Label_PlaceOrderPaymentTermDetails.Name = "Label_PlaceOrderPaymentTermDetails"
        Label_PlaceOrderPaymentTermDetails.Size = New System.Drawing.Size(158, 18)
        Label_PlaceOrderPaymentTermDetails.TabIndex = 3
        Label_PlaceOrderPaymentTermDetails.Text = "Payment Term Details"
        Label_PlaceOrderPaymentTermDetails.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label_PlaceOrderPaymentTermDetailsText
        '
        Label_PlaceOrderPaymentTermDetailsText.Anchor = System.Windows.Forms.AnchorStyles.Left
        Label_PlaceOrderPaymentTermDetailsText.AutoSize = true
        Label_PlaceOrderPaymentTermDetailsText.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_PlaceOrderPaymentTermDetailsText.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_PlaceOrderPaymentTermDetailsText.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_PlaceOrderPaymentTermDetailsText.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_PlaceOrderPaymentTermDetailsText.Location = New System.Drawing.Point(0, 26)
        Label_PlaceOrderPaymentTermDetailsText.Margin = New System.Windows.Forms.Padding(0)
        Label_PlaceOrderPaymentTermDetailsText.Name = "Label_PlaceOrderPaymentTermDetailsText"
        Label_PlaceOrderPaymentTermDetailsText.Size = New System.Drawing.Size(169, 48)
        Label_PlaceOrderPaymentTermDetailsText.TabIndex = 4
        Label_PlaceOrderPaymentTermDetailsText.Text = "Payement Term ID: No Value"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Term Days: No Value"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Due Date: No Value"
        Label_PlaceOrderPaymentTermDetailsText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'FlowLayoutPanel5
        '
        FlowLayoutPanel5.AutoSize = true
        FlowLayoutPanel5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel5.Controls.Add(Label_PlaceOrderTax)
        FlowLayoutPanel5.Controls.Add(Panel_PlaceOrderTax)
        FlowLayoutPanel5.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel5.Location = New System.Drawing.Point(12, 408)
        FlowLayoutPanel5.Margin = New System.Windows.Forms.Padding(12, 0, 6, 12)
        FlowLayoutPanel5.Name = "FlowLayoutPanel5"
        FlowLayoutPanel5.Size = New System.Drawing.Size(336, 56)
        FlowLayoutPanel5.TabIndex = 36
        '
        'Label_PlaceOrderTax
        '
        Label_PlaceOrderTax.AutoSize = true
        Label_PlaceOrderTax.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_PlaceOrderTax.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_PlaceOrderTax.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_PlaceOrderTax.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_PlaceOrderTax.Location = New System.Drawing.Point(0, 0)
        Label_PlaceOrderTax.Margin = New System.Windows.Forms.Padding(0)
        Label_PlaceOrderTax.Name = "Label_PlaceOrderTax"
        Label_PlaceOrderTax.Size = New System.Drawing.Size(27, 16)
        Label_PlaceOrderTax.TabIndex = 2
        Label_PlaceOrderTax.Text = "Tax"
        Label_PlaceOrderTax.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Panel_PlaceOrderTax
        '
        Panel_PlaceOrderTax.Controls.Add(Combobox_PlaceOrderTax)
        Panel_PlaceOrderTax.Controls.Add(Button_PlaceOrderNewTax)
        Panel_PlaceOrderTax.Location = New System.Drawing.Point(0, 16)
        Panel_PlaceOrderTax.Margin = New System.Windows.Forms.Padding(0)
        Panel_PlaceOrderTax.Name = "Panel_PlaceOrderTax"
        Panel_PlaceOrderTax.Size = New System.Drawing.Size(336, 40)
        Panel_PlaceOrderTax.TabIndex = 31
        '
        'Combobox_PlaceOrderTax
        '
        Combobox_PlaceOrderTax.BackColor = System.Drawing.Color.Transparent
        Combobox_PlaceOrderTax.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Combobox_PlaceOrderTax.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Combobox_PlaceOrderTax.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Combobox_PlaceOrderTax.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Combobox_PlaceOrderTax.FocusedColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Combobox_PlaceOrderTax.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Combobox_PlaceOrderTax.Font = New System.Drawing.Font("Segoe UI", 10!)
        Combobox_PlaceOrderTax.ForeColor = System.Drawing.Color.FromArgb(CType(CType(68,Byte),Integer), CType(CType(88,Byte),Integer), CType(CType(112,Byte),Integer))
        Combobox_PlaceOrderTax.ItemHeight = 34
        Combobox_PlaceOrderTax.Items.AddRange(New Object() {"Benjamin Rollan", "Benjamin Rollan", "Benjamin Rollan", "Benjamin Rollan"})
        Combobox_PlaceOrderTax.Location = New System.Drawing.Point(0, 0)
        Combobox_PlaceOrderTax.Margin = New System.Windows.Forms.Padding(0)
        Combobox_PlaceOrderTax.Name = "Combobox_PlaceOrderTax"
        Combobox_PlaceOrderTax.Size = New System.Drawing.Size(197, 40)
        Combobox_PlaceOrderTax.TabIndex = 31
        '
        'Button_PlaceOrderNewTax
        '
        Button_PlaceOrderNewTax.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(44,Byte),Integer), CType(CType(62,Byte),Integer))
        Button_PlaceOrderNewTax.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Button_PlaceOrderNewTax.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Button_PlaceOrderNewTax.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer))
        Button_PlaceOrderNewTax.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer))
        Button_PlaceOrderNewTax.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Button_PlaceOrderNewTax.Font = New System.Drawing.Font("Segoe UI", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel)
        Button_PlaceOrderNewTax.ForeColor = System.Drawing.Color.White
        Button_PlaceOrderNewTax.Location = New System.Drawing.Point(209, 0)
        Button_PlaceOrderNewTax.Margin = New System.Windows.Forms.Padding(12, 0, 0, 0)
        Button_PlaceOrderNewTax.Name = "Button_PlaceOrderNewTax"
        Button_PlaceOrderNewTax.Size = New System.Drawing.Size(127, 40)
        Button_PlaceOrderNewTax.TabIndex = 31
        Button_PlaceOrderNewTax.Text = "New Tax"
        Button_PlaceOrderNewTax.TextOffset = New System.Drawing.Point(0, -2)
        '
        'FlowLayoutPanel_PlaceOrderTaxDetails
        '
        FlowLayoutPanel_PlaceOrderTaxDetails.AutoSize = true
        FlowLayoutPanel_PlaceOrderTaxDetails.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_PlaceOrderTaxDetails.Controls.Add(Label_PlaceOrderTaxDetails)
        FlowLayoutPanel_PlaceOrderTaxDetails.Controls.Add(Label_PlaceOrderTaxDetailstext)
        FlowLayoutPanel_PlaceOrderTaxDetails.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_PlaceOrderTaxDetails.Location = New System.Drawing.Point(12, 476)
        FlowLayoutPanel_PlaceOrderTaxDetails.Margin = New System.Windows.Forms.Padding(12, 0, 0, 12)
        FlowLayoutPanel_PlaceOrderTaxDetails.Name = "FlowLayoutPanel_PlaceOrderTaxDetails"
        FlowLayoutPanel_PlaceOrderTaxDetails.Size = New System.Drawing.Size(156, 58)
        FlowLayoutPanel_PlaceOrderTaxDetails.TabIndex = 40
        '
        'Label_PlaceOrderTaxDetails
        '
        Label_PlaceOrderTaxDetails.Anchor = System.Windows.Forms.AnchorStyles.Left
        Label_PlaceOrderTaxDetails.AutoSize = true
        Label_PlaceOrderTaxDetails.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_PlaceOrderTaxDetails.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_PlaceOrderTaxDetails.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_PlaceOrderTaxDetails.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_PlaceOrderTaxDetails.Location = New System.Drawing.Point(0, 0)
        Label_PlaceOrderTaxDetails.Margin = New System.Windows.Forms.Padding(0, 0, 0, 8)
        Label_PlaceOrderTaxDetails.Name = "Label_PlaceOrderTaxDetails"
        Label_PlaceOrderTaxDetails.Size = New System.Drawing.Size(82, 18)
        Label_PlaceOrderTaxDetails.TabIndex = 3
        Label_PlaceOrderTaxDetails.Text = "Tax Details"
        Label_PlaceOrderTaxDetails.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label_PlaceOrderTaxDetailstext
        '
        Label_PlaceOrderTaxDetailstext.Anchor = System.Windows.Forms.AnchorStyles.Left
        Label_PlaceOrderTaxDetailstext.AutoSize = true
        Label_PlaceOrderTaxDetailstext.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_PlaceOrderTaxDetailstext.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_PlaceOrderTaxDetailstext.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_PlaceOrderTaxDetailstext.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_PlaceOrderTaxDetailstext.Location = New System.Drawing.Point(0, 26)
        Label_PlaceOrderTaxDetailstext.Margin = New System.Windows.Forms.Padding(0)
        Label_PlaceOrderTaxDetailstext.Name = "Label_PlaceOrderTaxDetailstext"
        Label_PlaceOrderTaxDetailstext.Size = New System.Drawing.Size(156, 32)
        Label_PlaceOrderTaxDetailstext.TabIndex = 4
        Label_PlaceOrderTaxDetailstext.Text = "Tax ID: No Value"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Tax: Percentage: No Value"
        Label_PlaceOrderTaxDetailstext.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'FlowLayoutPanel_PlaceOrderReceivedAmount
        '
        FlowLayoutPanel_PlaceOrderReceivedAmount.AutoSize = true
        FlowLayoutPanel_PlaceOrderReceivedAmount.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_PlaceOrderReceivedAmount.Controls.Add(Label_PlaceOrderReceivedAmount)
        FlowLayoutPanel_PlaceOrderReceivedAmount.Controls.Add(Textbox_PlaceOrderReceivedAmount)
        FlowLayoutPanel_PlaceOrderReceivedAmount.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_PlaceOrderReceivedAmount.Location = New System.Drawing.Point(12, 546)
        FlowLayoutPanel_PlaceOrderReceivedAmount.Margin = New System.Windows.Forms.Padding(12, 0, 12, 0)
        FlowLayoutPanel_PlaceOrderReceivedAmount.Name = "FlowLayoutPanel_PlaceOrderReceivedAmount"
        FlowLayoutPanel_PlaceOrderReceivedAmount.Size = New System.Drawing.Size(336, 56)
        FlowLayoutPanel_PlaceOrderReceivedAmount.TabIndex = 41
        '
        'Label_PlaceOrderReceivedAmount
        '
        Label_PlaceOrderReceivedAmount.AutoSize = true
        Label_PlaceOrderReceivedAmount.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_PlaceOrderReceivedAmount.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_PlaceOrderReceivedAmount.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_PlaceOrderReceivedAmount.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_PlaceOrderReceivedAmount.Location = New System.Drawing.Point(0, 0)
        Label_PlaceOrderReceivedAmount.Margin = New System.Windows.Forms.Padding(0)
        Label_PlaceOrderReceivedAmount.Name = "Label_PlaceOrderReceivedAmount"
        Label_PlaceOrderReceivedAmount.Size = New System.Drawing.Size(107, 16)
        Label_PlaceOrderReceivedAmount.TabIndex = 2
        Label_PlaceOrderReceivedAmount.Text = "Received Amount"
        Label_PlaceOrderReceivedAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Textbox_PlaceOrderReceivedAmount
        '
        Textbox_PlaceOrderReceivedAmount.Anchor = System.Windows.Forms.AnchorStyles.Left
        Textbox_PlaceOrderReceivedAmount.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_PlaceOrderReceivedAmount.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_PlaceOrderReceivedAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Textbox_PlaceOrderReceivedAmount.DefaultText = ""
        Textbox_PlaceOrderReceivedAmount.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer))
        Textbox_PlaceOrderReceivedAmount.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer))
        Textbox_PlaceOrderReceivedAmount.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_PlaceOrderReceivedAmount.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_PlaceOrderReceivedAmount.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_PlaceOrderReceivedAmount.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_PlaceOrderReceivedAmount.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
        Textbox_PlaceOrderReceivedAmount.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_PlaceOrderReceivedAmount.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_PlaceOrderReceivedAmount.Location = New System.Drawing.Point(0, 16)
        Textbox_PlaceOrderReceivedAmount.Margin = New System.Windows.Forms.Padding(0)
        Textbox_PlaceOrderReceivedAmount.Name = "Textbox_PlaceOrderReceivedAmount"
        Textbox_PlaceOrderReceivedAmount.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Textbox_PlaceOrderReceivedAmount.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer))
        Textbox_PlaceOrderReceivedAmount.PlaceholderText = "Enter Received Amount"
        Textbox_PlaceOrderReceivedAmount.SelectedText = ""
        Textbox_PlaceOrderReceivedAmount.Size = New System.Drawing.Size(336, 40)
        Textbox_PlaceOrderReceivedAmount.TabIndex = 39
        '
        'FlowLayoutPanel2_PlaceOrder
        '
        FlowLayoutPanel2_PlaceOrder.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel2_PlaceOrder.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        FlowLayoutPanel2_PlaceOrder.Controls.Add(S)
        FlowLayoutPanel2_PlaceOrder.Controls.Add(Label_PlaceOrderSubtotal)
        FlowLayoutPanel2_PlaceOrder.Controls.Add(Label_PlaceOrderSubtotalValue)
        FlowLayoutPanel2_PlaceOrder.Controls.Add(Label_PlaceOrderTaxPercentage)
        FlowLayoutPanel2_PlaceOrder.Controls.Add(Label_PlaceOrderTaxPercentageValue)
        FlowLayoutPanel2_PlaceOrder.Controls.Add(Label_PlaceOrdertaxAmount)
        FlowLayoutPanel2_PlaceOrder.Controls.Add(Label_PlaceOrdertaxAmountValue)
        FlowLayoutPanel2_PlaceOrder.Controls.Add(Panel4)
        FlowLayoutPanel2_PlaceOrder.Controls.Add(Label_PlaceOrderNetTotal)
        FlowLayoutPanel2_PlaceOrder.Controls.Add(Label_PlaceOrderNetTotalValue)
        FlowLayoutPanel2_PlaceOrder.Controls.Add(Label_PlaceOrderReceived)
        FlowLayoutPanel2_PlaceOrder.Controls.Add(Label_PlaceOrderReceivedValue)
        FlowLayoutPanel2_PlaceOrder.Controls.Add(Label_PlaceOrderChange)
        FlowLayoutPanel2_PlaceOrder.Controls.Add(Label_PlaceOrderChangeValue)
        FlowLayoutPanel2_PlaceOrder.Controls.Add(Label_PlaceOrderDue)
        FlowLayoutPanel2_PlaceOrder.Controls.Add(Label_PlaceOrderDueValue)
        FlowLayoutPanel2_PlaceOrder.Controls.Add(Button_PlaceOrderCompleteTransaction)
        FlowLayoutPanel2_PlaceOrder.Location = New System.Drawing.Point(360, 80)
        FlowLayoutPanel2_PlaceOrder.Margin = New System.Windows.Forms.Padding(0)
        FlowLayoutPanel2_PlaceOrder.Name = "FlowLayoutPanel2_PlaceOrder"
        FlowLayoutPanel2_PlaceOrder.Size = New System.Drawing.Size(360, 640)
        FlowLayoutPanel2_PlaceOrder.TabIndex = 38
        '
        'S
        '
        S.Anchor = System.Windows.Forms.AnchorStyles.Left
        S.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        S.Font = New System.Drawing.Font("Microsoft JhengHei", 22!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        S.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        S.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        S.Location = New System.Drawing.Point(12, 0)
        S.Margin = New System.Windows.Forms.Padding(12, 0, 12, 24)
        S.Name = "S"
        S.Size = New System.Drawing.Size(336, 28)
        S.TabIndex = 20
        S.Text = "Summary"
        S.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label_PlaceOrderSubtotal
        '
        Label_PlaceOrderSubtotal.Anchor = System.Windows.Forms.AnchorStyles.Left
        Label_PlaceOrderSubtotal.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_PlaceOrderSubtotal.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_PlaceOrderSubtotal.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Label_PlaceOrderSubtotal.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_PlaceOrderSubtotal.Location = New System.Drawing.Point(12, 52)
        Label_PlaceOrderSubtotal.Margin = New System.Windows.Forms.Padding(12, 0, 12, 12)
        Label_PlaceOrderSubtotal.Name = "Label_PlaceOrderSubtotal"
        Label_PlaceOrderSubtotal.Size = New System.Drawing.Size(108, 18)
        Label_PlaceOrderSubtotal.TabIndex = 37
        Label_PlaceOrderSubtotal.Text = "Subtotal"
        Label_PlaceOrderSubtotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label_PlaceOrderSubtotalValue
        '
        Label_PlaceOrderSubtotalValue.Anchor = System.Windows.Forms.AnchorStyles.Left
        Label_PlaceOrderSubtotalValue.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_PlaceOrderSubtotalValue.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_PlaceOrderSubtotalValue.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Label_PlaceOrderSubtotalValue.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_PlaceOrderSubtotalValue.Location = New System.Drawing.Point(144, 52)
        Label_PlaceOrderSubtotalValue.Margin = New System.Windows.Forms.Padding(12, 0, 12, 12)
        Label_PlaceOrderSubtotalValue.Name = "Label_PlaceOrderSubtotalValue"
        Label_PlaceOrderSubtotalValue.Size = New System.Drawing.Size(204, 18)
        Label_PlaceOrderSubtotalValue.TabIndex = 38
        Label_PlaceOrderSubtotalValue.Text = "100,000,000"
        Label_PlaceOrderSubtotalValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label_PlaceOrderTaxPercentage
        '
        Label_PlaceOrderTaxPercentage.Anchor = System.Windows.Forms.AnchorStyles.Left
        Label_PlaceOrderTaxPercentage.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_PlaceOrderTaxPercentage.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_PlaceOrderTaxPercentage.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Label_PlaceOrderTaxPercentage.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_PlaceOrderTaxPercentage.Location = New System.Drawing.Point(12, 82)
        Label_PlaceOrderTaxPercentage.Margin = New System.Windows.Forms.Padding(12, 0, 12, 12)
        Label_PlaceOrderTaxPercentage.Name = "Label_PlaceOrderTaxPercentage"
        Label_PlaceOrderTaxPercentage.Size = New System.Drawing.Size(123, 18)
        Label_PlaceOrderTaxPercentage.TabIndex = 39
        Label_PlaceOrderTaxPercentage.Text = "Tax Percentage"
        Label_PlaceOrderTaxPercentage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label_PlaceOrderTaxPercentageValue
        '
        Label_PlaceOrderTaxPercentageValue.Anchor = System.Windows.Forms.AnchorStyles.Left
        Label_PlaceOrderTaxPercentageValue.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_PlaceOrderTaxPercentageValue.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_PlaceOrderTaxPercentageValue.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Label_PlaceOrderTaxPercentageValue.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_PlaceOrderTaxPercentageValue.Location = New System.Drawing.Point(159, 82)
        Label_PlaceOrderTaxPercentageValue.Margin = New System.Windows.Forms.Padding(12, 0, 12, 12)
        Label_PlaceOrderTaxPercentageValue.Name = "Label_PlaceOrderTaxPercentageValue"
        Label_PlaceOrderTaxPercentageValue.Size = New System.Drawing.Size(188, 18)
        Label_PlaceOrderTaxPercentageValue.TabIndex = 40
        Label_PlaceOrderTaxPercentageValue.Text = "100%"
        Label_PlaceOrderTaxPercentageValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label_PlaceOrdertaxAmount
        '
        Label_PlaceOrdertaxAmount.Anchor = System.Windows.Forms.AnchorStyles.Left
        Label_PlaceOrdertaxAmount.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_PlaceOrdertaxAmount.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_PlaceOrdertaxAmount.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Label_PlaceOrdertaxAmount.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_PlaceOrdertaxAmount.Location = New System.Drawing.Point(12, 112)
        Label_PlaceOrdertaxAmount.Margin = New System.Windows.Forms.Padding(12, 0, 12, 12)
        Label_PlaceOrdertaxAmount.Name = "Label_PlaceOrdertaxAmount"
        Label_PlaceOrdertaxAmount.Size = New System.Drawing.Size(108, 18)
        Label_PlaceOrdertaxAmount.TabIndex = 41
        Label_PlaceOrdertaxAmount.Text = "Tax Amount"
        Label_PlaceOrdertaxAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label_PlaceOrdertaxAmountValue
        '
        Label_PlaceOrdertaxAmountValue.Anchor = System.Windows.Forms.AnchorStyles.Left
        Label_PlaceOrdertaxAmountValue.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_PlaceOrdertaxAmountValue.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_PlaceOrdertaxAmountValue.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Label_PlaceOrdertaxAmountValue.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_PlaceOrdertaxAmountValue.Location = New System.Drawing.Point(144, 112)
        Label_PlaceOrdertaxAmountValue.Margin = New System.Windows.Forms.Padding(12, 0, 12, 12)
        Label_PlaceOrdertaxAmountValue.Name = "Label_PlaceOrdertaxAmountValue"
        Label_PlaceOrdertaxAmountValue.Size = New System.Drawing.Size(204, 18)
        Label_PlaceOrdertaxAmountValue.TabIndex = 42
        Label_PlaceOrdertaxAmountValue.Text = "100,000,000"
        Label_PlaceOrdertaxAmountValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Panel4
        '
        Panel4.BackColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Panel4.Location = New System.Drawing.Point(12, 154)
        Panel4.Margin = New System.Windows.Forms.Padding(12)
        Panel4.Name = "Panel4"
        Panel4.Size = New System.Drawing.Size(336, 3)
        Panel4.TabIndex = 43
        '
        'Label_PlaceOrderNetTotal
        '
        Label_PlaceOrderNetTotal.Anchor = System.Windows.Forms.AnchorStyles.Left
        Label_PlaceOrderNetTotal.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_PlaceOrderNetTotal.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_PlaceOrderNetTotal.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Label_PlaceOrderNetTotal.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_PlaceOrderNetTotal.Location = New System.Drawing.Point(12, 169)
        Label_PlaceOrderNetTotal.Margin = New System.Windows.Forms.Padding(12, 0, 12, 12)
        Label_PlaceOrderNetTotal.Name = "Label_PlaceOrderNetTotal"
        Label_PlaceOrderNetTotal.Size = New System.Drawing.Size(108, 18)
        Label_PlaceOrderNetTotal.TabIndex = 44
        Label_PlaceOrderNetTotal.Text = "Net Total"
        Label_PlaceOrderNetTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label_PlaceOrderNetTotalValue
        '
        Label_PlaceOrderNetTotalValue.Anchor = System.Windows.Forms.AnchorStyles.Left
        Label_PlaceOrderNetTotalValue.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_PlaceOrderNetTotalValue.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_PlaceOrderNetTotalValue.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Label_PlaceOrderNetTotalValue.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_PlaceOrderNetTotalValue.Location = New System.Drawing.Point(144, 169)
        Label_PlaceOrderNetTotalValue.Margin = New System.Windows.Forms.Padding(12, 0, 12, 12)
        Label_PlaceOrderNetTotalValue.Name = "Label_PlaceOrderNetTotalValue"
        Label_PlaceOrderNetTotalValue.Size = New System.Drawing.Size(204, 18)
        Label_PlaceOrderNetTotalValue.TabIndex = 45
        Label_PlaceOrderNetTotalValue.Text = "100,000,000"
        Label_PlaceOrderNetTotalValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label_PlaceOrderReceived
        '
        Label_PlaceOrderReceived.Anchor = System.Windows.Forms.AnchorStyles.Left
        Label_PlaceOrderReceived.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_PlaceOrderReceived.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_PlaceOrderReceived.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Label_PlaceOrderReceived.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_PlaceOrderReceived.Location = New System.Drawing.Point(12, 199)
        Label_PlaceOrderReceived.Margin = New System.Windows.Forms.Padding(12, 0, 12, 12)
        Label_PlaceOrderReceived.Name = "Label_PlaceOrderReceived"
        Label_PlaceOrderReceived.Size = New System.Drawing.Size(108, 18)
        Label_PlaceOrderReceived.TabIndex = 46
        Label_PlaceOrderReceived.Text = "Received"
        Label_PlaceOrderReceived.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label_PlaceOrderReceivedValue
        '
        Label_PlaceOrderReceivedValue.Anchor = System.Windows.Forms.AnchorStyles.Left
        Label_PlaceOrderReceivedValue.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_PlaceOrderReceivedValue.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_PlaceOrderReceivedValue.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Label_PlaceOrderReceivedValue.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_PlaceOrderReceivedValue.Location = New System.Drawing.Point(144, 199)
        Label_PlaceOrderReceivedValue.Margin = New System.Windows.Forms.Padding(12, 0, 12, 12)
        Label_PlaceOrderReceivedValue.Name = "Label_PlaceOrderReceivedValue"
        Label_PlaceOrderReceivedValue.Size = New System.Drawing.Size(204, 18)
        Label_PlaceOrderReceivedValue.TabIndex = 47
        Label_PlaceOrderReceivedValue.Text = "100,000,000"
        Label_PlaceOrderReceivedValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label_PlaceOrderChange
        '
        Label_PlaceOrderChange.Anchor = System.Windows.Forms.AnchorStyles.Left
        Label_PlaceOrderChange.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_PlaceOrderChange.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_PlaceOrderChange.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Label_PlaceOrderChange.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_PlaceOrderChange.Location = New System.Drawing.Point(12, 229)
        Label_PlaceOrderChange.Margin = New System.Windows.Forms.Padding(12, 0, 12, 12)
        Label_PlaceOrderChange.Name = "Label_PlaceOrderChange"
        Label_PlaceOrderChange.Size = New System.Drawing.Size(108, 18)
        Label_PlaceOrderChange.TabIndex = 48
        Label_PlaceOrderChange.Text = "Change"
        Label_PlaceOrderChange.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label_PlaceOrderChangeValue
        '
        Label_PlaceOrderChangeValue.Anchor = System.Windows.Forms.AnchorStyles.Left
        Label_PlaceOrderChangeValue.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_PlaceOrderChangeValue.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_PlaceOrderChangeValue.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Label_PlaceOrderChangeValue.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_PlaceOrderChangeValue.Location = New System.Drawing.Point(144, 229)
        Label_PlaceOrderChangeValue.Margin = New System.Windows.Forms.Padding(12, 0, 12, 12)
        Label_PlaceOrderChangeValue.Name = "Label_PlaceOrderChangeValue"
        Label_PlaceOrderChangeValue.Size = New System.Drawing.Size(204, 18)
        Label_PlaceOrderChangeValue.TabIndex = 49
        Label_PlaceOrderChangeValue.Text = "100,000,000"
        Label_PlaceOrderChangeValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Button_PlaceOrderCompleteTransaction
        '
        Button_PlaceOrderCompleteTransaction.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(44,Byte),Integer), CType(CType(62,Byte),Integer))
        Button_PlaceOrderCompleteTransaction.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Button_PlaceOrderCompleteTransaction.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Button_PlaceOrderCompleteTransaction.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer))
        Button_PlaceOrderCompleteTransaction.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer))
        Button_PlaceOrderCompleteTransaction.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Button_PlaceOrderCompleteTransaction.Font = New System.Drawing.Font("Segoe UI", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel)
        Button_PlaceOrderCompleteTransaction.ForeColor = System.Drawing.Color.White
        Button_PlaceOrderCompleteTransaction.Location = New System.Drawing.Point(12, 561)
        Button_PlaceOrderCompleteTransaction.Margin = New System.Windows.Forms.Padding(12, 272, 0, 0)
        Button_PlaceOrderCompleteTransaction.Name = "Button_PlaceOrderCompleteTransaction"
        Button_PlaceOrderCompleteTransaction.Size = New System.Drawing.Size(336, 40)
        Button_PlaceOrderCompleteTransaction.TabIndex = 50
        Button_PlaceOrderCompleteTransaction.Text = "Complete Transaction"
        Button_PlaceOrderCompleteTransaction.TextOffset = New System.Drawing.Point(0, -2)
        '
        'Label_PlaceOrderDue
        '
        Label_PlaceOrderDue.Anchor = System.Windows.Forms.AnchorStyles.Left
        Label_PlaceOrderDue.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_PlaceOrderDue.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_PlaceOrderDue.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Label_PlaceOrderDue.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_PlaceOrderDue.Location = New System.Drawing.Point(12, 259)
        Label_PlaceOrderDue.Margin = New System.Windows.Forms.Padding(12, 0, 12, 12)
        Label_PlaceOrderDue.Name = "Label_PlaceOrderDue"
        Label_PlaceOrderDue.Size = New System.Drawing.Size(108, 18)
        Label_PlaceOrderDue.TabIndex = 51
        Label_PlaceOrderDue.Text = "Due"
        Label_PlaceOrderDue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label_PlaceOrderDueValue
        '
        Label_PlaceOrderDueValue.Anchor = System.Windows.Forms.AnchorStyles.Left
        Label_PlaceOrderDueValue.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_PlaceOrderDueValue.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_PlaceOrderDueValue.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Label_PlaceOrderDueValue.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_PlaceOrderDueValue.Location = New System.Drawing.Point(144, 259)
        Label_PlaceOrderDueValue.Margin = New System.Windows.Forms.Padding(12, 0, 12, 12)
        Label_PlaceOrderDueValue.Name = "Label_PlaceOrderDueValue"
        Label_PlaceOrderDueValue.Size = New System.Drawing.Size(204, 18)
        Label_PlaceOrderDueValue.TabIndex = 52
        Label_PlaceOrderDueValue.Text = "100,000,000"
        Label_PlaceOrderDueValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Prompt_TransactionPlaceOrder
        '
        Prompt_TransactionPlaceOrder.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Prompt_TransactionPlaceOrder.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Prompt_TransactionPlaceOrder.ClientSize = New System.Drawing.Size(720, 720)
        Prompt_TransactionPlaceOrder.Controls.Add(FlowLayoutPanel2_PlaceOrder)
        Prompt_TransactionPlaceOrder.Controls.Add(FlowLayoutPanel1_PlaceOrder)
        Prompt_TransactionPlaceOrder.Controls.Add(FlowLayoutPanel_PlaceOrder)
        Prompt_TransactionPlaceOrder.Name = "Prompt_TransactionPlaceOrder"
        Prompt_TransactionPlaceOrder.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Prompt_TransactionPlaceOrder.Text = "Prompt_TransactionPlaceOrder"
        FlowLayoutPanel_WindowHeaderStockPlaceOrder.ResumeLayout(false)
        FlowLayoutPanel_WindowHeaderStockPlaceOrder.PerformLayout
        FlowLayoutPanel_PlaceOrder.ResumeLayout(false)
        FlowLayoutPanel_PlaceOrder.PerformLayout
        FlowLayoutPanel1_PlaceOrder.ResumeLayout(false)
        FlowLayoutPanel1_PlaceOrder.PerformLayout
        FlowLayoutPanel_PlaceOrderCustomer.ResumeLayout(false)
        FlowLayoutPanel_PlaceOrderCustomer.PerformLayout
        Panel_PlaceOrderCustomer.ResumeLayout(false)
        FlowLayoutPanel_PlaceOrderCustomerDetails.ResumeLayout(false)
        FlowLayoutPanel_PlaceOrderCustomerDetails.PerformLayout
        FlowLayoutPanel_PlaceOrderPaymentTerm.ResumeLayout(false)
        FlowLayoutPanel_PlaceOrderPaymentTerm.PerformLayout
        Panel_PlaceOrderPaymentTerm.ResumeLayout(false)
        FlowLayoutPanel_PlaceOrderPaymentTermDetails.ResumeLayout(false)
        FlowLayoutPanel_PlaceOrderPaymentTermDetails.PerformLayout
        FlowLayoutPanel5.ResumeLayout(false)
        FlowLayoutPanel5.PerformLayout
        Panel_PlaceOrderTax.ResumeLayout(false)
        FlowLayoutPanel_PlaceOrderTaxDetails.ResumeLayout(false)
        FlowLayoutPanel_PlaceOrderTaxDetails.PerformLayout
        FlowLayoutPanel_PlaceOrderReceivedAmount.ResumeLayout(false)
        FlowLayoutPanel_PlaceOrderReceivedAmount.PerformLayout
        FlowLayoutPanel2_PlaceOrder.ResumeLayout(false)
        ResumeLayout(false)
        PerformLayout

        Prompt_TransactionPlaceOrder.ShowDialog(Me)
    End Sub

    Private Sub Prompt_TransactionPlaceOrder_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Label_PlaceOrderSubtotalValue_Click(sender As Object, e As EventArgs) Handles Label_PlaceOrderSubtotalValue.Click

    End Sub

    
End Class