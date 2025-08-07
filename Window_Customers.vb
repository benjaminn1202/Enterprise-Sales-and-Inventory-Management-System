Imports MySql.Data.MySqlClient
Imports Guna.UI2.WinForms
Imports System.Text


Public Class Window_Customers
    ' Navigation
    Private Sub NavigateToForm(targetForm As Form, currentForm As Form)
        targetForm.Show()
        currentForm.Close()
    End Sub

    Private Sub Button_NavigationItemSaleScreen_Click(sender As Object, e As EventArgs) Handles Button_NavigationItemSaleScreen.Click
        NavigateToForm(New Window_NewTransaction(), Me)
    End Sub

    Private Sub Button_NavigationItemSales_Click(sender As Object, e As EventArgs) Handles Button_NavigationItemSales.Click
        NavigateToForm(New Window_Sales(), Me)
    End Sub

    Private Sub Button_NavigationItemCustomer_Click(sender As Object, e As EventArgs) Handles Button_NavigationItemCustomers.Click
        NavigateToForm(New Window_Customers(), Me)
    End Sub

    Private Sub Button_NavigationItemReceiving_Click(sender As Object, e As EventArgs) Handles Button_NavigationItemReceiving.Click
        NavigateToForm(New Window_AddStock(), Me)
    End Sub

    Private Sub Button_NavigationItemCustomers_Click(sender As Object, e As EventArgs) Handles Button_NavigationItemCustomers.Click
        NavigateToForm(New Window_Customers(), Me)
    End Sub

    Private Sub Button_NavigationItemStockEntries_Click(sender As Object, e As EventArgs) Handles Button_NavigationItemStockEntries.Click
        NavigateToForm(New Window_StockEntries(), Me)
    End Sub

    Private Sub Button_NavigationItemBrandsAndCategories_Click(sender As Object, e As EventArgs) Handles Button_NavigationItemBrandsAndCategories.Click
        NavigateToForm(New Window_Categories(), Me)
    End Sub

    Private Sub Button_NavigationItemVendors_Click(sender As Object, e As EventArgs) Handles Button_NavigationItemVendors.Click
        NavigateToForm(New Window_Vendors(), Me)
    End Sub



    Public ConnectionString = "server=localhost;user id=root;password=benjaminn1202;database=sims_db"
    Public Customers As Customer() = CreateCustomerArray()
    Public PaymentTerms As PaymentTerm() = CreatePaymentTermArray()
    Public Accounts As Account() = CreateAccountArray()
    

    Public CustomersListCurrentPage As Integer = 1
    Public CustomersListItemsPerPage As Integer = 10
    Public CustomersListTotalPages As Integer

    Public Sub UpdateCustomersListTable()
        FlowLayoutPanel_CustomersListTable.Controls.Clear()

        CustomersListTotalPages = Math.Ceiling(Customers.Count / CustomersListItemsPerPage)
        Label_CustomersListTablePageControlPageNumber.Text = $"{CustomersListCurrentPage}/{CustomersListTotalPages}"

        Dim TableLayoutPanel_CustomersList As New TableLayoutPanel With {
            .AutoSize = True,
            .AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink,
            .BackColor = System.Drawing.Color.FromArgb(CType(255, Byte), CType(255, Byte), CType(255, Byte)),
            .CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single],
            .Margin = New System.Windows.Forms.Padding(0, 0, 0, 0),
            .MaximumSize = New System.Drawing.Size(1102, 0),
            .MinimumSize = New System.Drawing.Size(1102, 0)
        }

        TableLayoutPanel_CustomersList.ColumnCount = 6
        For i = 1 To 6
            TableLayoutPanel_CustomersList.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11!))
        Next

        Dim StartIndex As Integer = (CustomersListCurrentPage - 1) * CustomersListItemsPerPage
        Dim EndIndex As Integer = Math.Min(StartIndex + CustomersListItemsPerPage, Customers.Count)

        For i = StartIndex To EndIndex - 1
            Dim customer As Customer = Customers(i)
            TableLayoutPanel_CustomersList.RowCount += 1

            Dim TransactionCustomer As New Customer
            Dim TransactionAccount As New Account
            Dim TransactionPaymentTerm As New PaymentTerm

            ' Directly get the transaction data for this customer
            TransactionAccount = Accounts.FirstOrDefault(Function(a) a.AccountId = customer.CreatedBy)
            TransactionPaymentTerm = PaymentTerms.FirstOrDefault(Function(t) t.PaymentTermId = customer.PaymentTermId)

            ' Create the labels for the customer row
            Dim labels As New List(Of Label)
            labels.Add(CreateLabel(customer.CustomerId.ToString() & " - " & customer.Name, 2, True))
            labels.Add(CreateLabel(customer.PaymentTermId & " - " & If(TransactionPaymentTerm IsNot Nothing, TransactionPaymentTerm.TermName, ""), 2, True))
            labels.Add(CreateLabel(customer.Company, 1))
            labels.Add(CreateLabel(customer.Email, 1))
            labels.Add(CreateLabel(customer.Phone, 1))
            labels.Add(CreateLabel(customer.AddressLine & ", " & customer.City & ", " & customer.ZipCode & ", " & customer.Country, 1))

            For col As Integer = 0 To labels.Count - 1
                TableLayoutPanel_CustomersList.Controls.Add(labels(col), col, TableLayoutPanel_CustomersList.RowCount - 1)
            Next

            Dim maxLabelHeight As Integer = labels.Max(Function(lbl) lbl.Height)

            TableLayoutPanel_CustomersList.RowStyles.Add(New RowStyle(SizeType.Absolute, maxLabelHeight + 12))
        Next

        FlowLayoutPanel_CustomersListTable.Controls.Add(TableLayoutPanel_CustomersList)
    End Sub

    Private Function CreateLabel(text As String, style As Integer, Optional underlined As Boolean = False) As Label
        Dim fontStyle As FontStyle = If(underlined, FontStyle.Bold Or FontStyle.Underline, FontStyle.Bold)

        Return New Label With {
            .Anchor = System.Windows.Forms.AnchorStyles.Left,
            .AutoSize = True,
            .BackColor = System.Drawing.Color.FromArgb(0, 0, 0, 0),
            .Font = New System.Drawing.Font("Microsoft JhengHei", 12!, fontStyle, System.Drawing.GraphicsUnit.Pixel, CType(0, Byte)),
            .ForeColor = If(style = 2, System.Drawing.Color.FromArgb(128, 128, 255), System.Drawing.Color.FromArgb(2, 34, 34)),
            .ImageAlign = System.Drawing.ContentAlignment.MiddleLeft,
            .Margin = New System.Windows.Forms.Padding(12, 6, 12, 6),
            .Text = text
        }
    End Function

    

    Function CreateAccountArray() As Account()
        Dim query As String = "SELECT account_id, email, password, type, name, employee_id FROM accounts"
    
        Dim accountList As New List(Of Account)()

        Using connection As New MySqlConnection(ConnectionString)
            Try
                connection.Open()

                Using command As New MySqlCommand(query, connection)
                    Using reader As MySqlDataReader = command.ExecuteReader()
                        While reader.Read()
                            Dim account As New Account() With {
                                .AccountId = reader.GetInt32("account_id"),
                                .Email = reader.GetString("email"),
                                .Password = reader.GetString("password"),
                                .Type = reader.GetString("type"),
                                .Name = reader.GetString("name"),
                                .EmployeeId = reader.GetString("employee_id")
                            }
                            accountList.Add(account)
                        End While
                    End Using
                End Using
            Catch ex As Exception
                Console.WriteLine("Error: " & ex.Message)
            End Try
        End Using

        Return accountList.ToArray()
    End Function

    Function CreateCustomerArray() As Customer()
        Dim customers As New List(Of Customer)()

        Try
            Using connection As New MySqlConnection(ConnectionString)
                connection.Open()

                Dim query As String = "SELECT * FROM customers"
                Using command As New MySqlCommand(query, connection)
                    Using reader As MySqlDataReader = command.ExecuteReader()
                        While reader.Read()
                            Dim customerId As Integer = reader.GetInt32("customer_id")
                            Dim customerName As String = reader.GetString("name")
                            Dim paymentTermId As Integer = If(reader.IsDBNull(reader.GetOrdinal("payment_term_id")), 0, reader.GetInt32("payment_term_id"))
                            Dim company As String = If(reader.IsDBNull(reader.GetOrdinal("company")), Nothing, reader.GetString("company"))
                            Dim email As String = If(reader.IsDBNull(reader.GetOrdinal("email")), Nothing, reader.GetString("email"))
                            Dim phone As String = If(reader.IsDBNull(reader.GetOrdinal("phone")), Nothing, reader.GetString("phone"))
                            Dim addressLine As String = If(reader.IsDBNull(reader.GetOrdinal("address_line")), Nothing, reader.GetString("address_line"))
                            Dim city As String = If(reader.IsDBNull(reader.GetOrdinal("city")), Nothing, reader.GetString("city"))
                            Dim zipCode As String = If(reader.IsDBNull(reader.GetOrdinal("zip_code")), Nothing, reader.GetString("zip_code"))
                            Dim country As String = If(reader.IsDBNull(reader.GetOrdinal("country")), Nothing, reader.GetString("country"))
                            Dim createdBy As Integer = If(reader.IsDBNull(reader.GetOrdinal("created_by")), 0, reader.GetInt32("created_by"))
                            Dim createdAt As DateTime = If(reader.IsDBNull(reader.GetOrdinal("created_at")), DateTime.Now, reader.GetDateTime("created_at"))

                            Dim customer As New Customer(customerId, customerName, paymentTermId, company, email, phone, addressLine, city, zipCode, country, createdBy, createdAt)
                            customers.Add(customer)
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Console.WriteLine("Error: " & ex.Message)
        End Try

        Return customers.ToArray()
    End Function



    Private Sub ShowNewCustomerPrompt()
        Dim Prompt_NewCustomer As New Form()
        Dim FlowLayoutPanel_NewCustomer = New System.Windows.Forms.FlowLayoutPanel()
        Dim FlowLayoutPanel_PageHeaderNewCustomer = New System.Windows.Forms.FlowLayoutPanel()
        Dim Panel_PageHeaderIconNewCustomer = New System.Windows.Forms.Panel()
        Dim Label_PageHeaderNewCustomer = New System.Windows.Forms.Label()
        Dim Label_CustomerDetailsNewCustomer = New System.Windows.Forms.Label()
        Dim FlowLayoutPanel_CustomerDetailCustomerNameInputNewCustomer = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label_CustomerDetailCustomerNameInputNewCustomer = New System.Windows.Forms.Label()
        Dim Textbox_CustomerDetailCustomerNameInputNewCustomer = New Guna.UI2.WinForms.Guna2TextBox()
        Dim FlowLayoutPanel_CustomerDetailContactPersonNameInputNewCustomer = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label_CustomerDetailContactPersonNameInputNewCustomer = New System.Windows.Forms.Label()
        Dim Textbox_CustomerDetailContactPersonNameInputNewCustomer = New Guna.UI2.WinForms.Guna2TextBox()
        Dim FlowLayoutPanel_CustomerDetailPaymentTermInputNewCustomer = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label_PlaceOrderPaymentTerm = New System.Windows.Forms.Label()
        Dim Panel_CustomerDetailPaymentTermInputNewCustomer = New System.Windows.Forms.Panel()
        Dim Combobox_CustomerDetailPaymentTermInputNewCustomer = New Guna.UI2.WinForms.Guna2ComboBox()
        Dim Button_CustomerDetailPaymentTermInputNewCustomer = New Guna.UI2.WinForms.Guna2Button()
        Dim FlowLayoutPanel_CustomerDetailEmailInputNewCustomer = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label_CustomerDetailEmailInputNewCustomer = New System.Windows.Forms.Label()
        Dim Textbox_CustomerDetailEmailInputNewCustomer = New Guna.UI2.WinForms.Guna2TextBox()
        Dim FlowLayoutPanel_CustomerDetailPhoneInputNewCustomer = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label_CustomerDetailPhoneInputNewCustomer = New System.Windows.Forms.Label()
        Dim Textbox_CustomerDetailPhoneInputNewCustomer = New Guna.UI2.WinForms.Guna2TextBox()
        Dim Label_CustomerDetailAddressLineInputNewCustomer = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label_CustomerAddressLinePhoneInputNewCustomer = New System.Windows.Forms.Label()
        Dim Textbox_CustomerDetailAddressLineInputNewCustomer = New Guna.UI2.WinForms.Guna2TextBox()
        Dim FlowLayoutPanel_CustomerDetailCityInputNewCustomer = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label_CustomerDetailCityInputNewCustomer = New System.Windows.Forms.Label()
        Dim Textbox_CustomerDetailCityInputNewCustomer = New Guna.UI2.WinForms.Guna2TextBox()
        Dim FlowLayoutPanel_CustomerDetailZipCodeInputNewCustomer = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label_CustomerDetailZipCodeInputNewCustomer = New System.Windows.Forms.Label()
        Dim Textbox_CustomerDetailZipCodeInputNewCustomer = New Guna.UI2.WinForms.Guna2TextBox()
        Dim FlowLayoutPanel_CustomerDetailCountryInputNewCustomer = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label_CustomerDetailCountryInputNewCustomer = New System.Windows.Forms.Label()
        Dim Textbox_CustomerDetailCountryInputNewCustomer = New Guna.UI2.WinForms.Guna2TextBox()
        Dim FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Dim Button_ResetNewCustomer = New Guna.UI2.WinForms.Guna2Button()
        Dim Button_MakeCustomerNewCustomer = New Guna.UI2.WinForms.Guna2Button()
        Dim FlowLayoutPanel_DateRangeInputsSales = New System.Windows.Forms.FlowLayoutPanel()
        FlowLayoutPanel_NewCustomer.SuspendLayout
        FlowLayoutPanel_PageHeaderNewCustomer.SuspendLayout
        FlowLayoutPanel_CustomerDetailCustomerNameInputNewCustomer.SuspendLayout
        FlowLayoutPanel_CustomerDetailContactPersonNameInputNewCustomer.SuspendLayout
        FlowLayoutPanel_CustomerDetailPaymentTermInputNewCustomer.SuspendLayout
        Panel_CustomerDetailPaymentTermInputNewCustomer.SuspendLayout
        FlowLayoutPanel_CustomerDetailEmailInputNewCustomer.SuspendLayout
        FlowLayoutPanel_CustomerDetailPhoneInputNewCustomer.SuspendLayout
        Label_CustomerDetailAddressLineInputNewCustomer.SuspendLayout
        FlowLayoutPanel_CustomerDetailCityInputNewCustomer.SuspendLayout
        FlowLayoutPanel_CustomerDetailZipCodeInputNewCustomer.SuspendLayout
        FlowLayoutPanel_CustomerDetailCountryInputNewCustomer.SuspendLayout
        SuspendLayout
        '
        'FlowLayoutPanel_NewCustomer
        '
        FlowLayoutPanel_NewCustomer.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        FlowLayoutPanel_NewCustomer.Controls.Add(FlowLayoutPanel_PageHeaderNewCustomer)
        FlowLayoutPanel_NewCustomer.Controls.Add(Label_CustomerDetailsNewCustomer)
        FlowLayoutPanel_NewCustomer.Controls.Add(FlowLayoutPanel_CustomerDetailCustomerNameInputNewCustomer)
        FlowLayoutPanel_NewCustomer.Controls.Add(FlowLayoutPanel_CustomerDetailContactPersonNameInputNewCustomer)
        FlowLayoutPanel_NewCustomer.Controls.Add(FlowLayoutPanel_CustomerDetailPaymentTermInputNewCustomer)
        FlowLayoutPanel_NewCustomer.Controls.Add(FlowLayoutPanel_CustomerDetailEmailInputNewCustomer)
        FlowLayoutPanel_NewCustomer.Controls.Add(FlowLayoutPanel_CustomerDetailPhoneInputNewCustomer)
        FlowLayoutPanel_NewCustomer.Controls.Add(Label_CustomerDetailAddressLineInputNewCustomer)
        FlowLayoutPanel_NewCustomer.Controls.Add(FlowLayoutPanel_CustomerDetailCityInputNewCustomer)
        FlowLayoutPanel_NewCustomer.Controls.Add(FlowLayoutPanel_CustomerDetailZipCodeInputNewCustomer)
        FlowLayoutPanel_NewCustomer.Controls.Add(FlowLayoutPanel_CustomerDetailCountryInputNewCustomer)
        FlowLayoutPanel_NewCustomer.Controls.Add(FlowLayoutPanel1)
        FlowLayoutPanel_NewCustomer.Controls.Add(Button_ResetNewCustomer)
        FlowLayoutPanel_NewCustomer.Controls.Add(Button_MakeCustomerNewCustomer)
        FlowLayoutPanel_NewCustomer.Controls.Add(FlowLayoutPanel_DateRangeInputsSales)
        FlowLayoutPanel_NewCustomer.Location = New System.Drawing.Point(0, 0)
        FlowLayoutPanel_NewCustomer.Margin = New System.Windows.Forms.Padding(0)
        FlowLayoutPanel_NewCustomer.Name = "FlowLayoutPanel_NewCustomer"
        FlowLayoutPanel_NewCustomer.Size = New System.Drawing.Size(720, 720)
        FlowLayoutPanel_NewCustomer.TabIndex = 35
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
        Panel_PageHeaderIconNewCustomer.BackgroundImage = Global.ERP_SIMS.My.Resources.Resources.HeaderIcon_Customers
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
        Label_PageHeaderNewCustomer.Text = "New Customer"
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
        Label_CustomerDetailsNewCustomer.Text = "Customer Details"
        Label_CustomerDetailsNewCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'FlowLayoutPanel_CustomerDetailCustomerNameInputNewCustomer
        '
        FlowLayoutPanel_CustomerDetailCustomerNameInputNewCustomer.AutoSize = true
        FlowLayoutPanel_CustomerDetailCustomerNameInputNewCustomer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_CustomerDetailCustomerNameInputNewCustomer.Controls.Add(Label_CustomerDetailCustomerNameInputNewCustomer)
        FlowLayoutPanel_CustomerDetailCustomerNameInputNewCustomer.Controls.Add(Textbox_CustomerDetailCustomerNameInputNewCustomer)
        FlowLayoutPanel_CustomerDetailCustomerNameInputNewCustomer.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_CustomerDetailCustomerNameInputNewCustomer.Location = New System.Drawing.Point(12, 132)
        FlowLayoutPanel_CustomerDetailCustomerNameInputNewCustomer.Margin = New System.Windows.Forms.Padding(12, 0, 6, 12)
        FlowLayoutPanel_CustomerDetailCustomerNameInputNewCustomer.Name = "FlowLayoutPanel_CustomerDetailCustomerNameInputNewCustomer"
        FlowLayoutPanel_CustomerDetailCustomerNameInputNewCustomer.Size = New System.Drawing.Size(342, 56)
        FlowLayoutPanel_CustomerDetailCustomerNameInputNewCustomer.TabIndex = 19
        '
        'Label_CustomerDetailCustomerNameInputNewCustomer
        '
        Label_CustomerDetailCustomerNameInputNewCustomer.AutoSize = true
        Label_CustomerDetailCustomerNameInputNewCustomer.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_CustomerDetailCustomerNameInputNewCustomer.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_CustomerDetailCustomerNameInputNewCustomer.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_CustomerDetailCustomerNameInputNewCustomer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_CustomerDetailCustomerNameInputNewCustomer.Location = New System.Drawing.Point(0, 0)
        Label_CustomerDetailCustomerNameInputNewCustomer.Margin = New System.Windows.Forms.Padding(0)
        Label_CustomerDetailCustomerNameInputNewCustomer.Name = "Label_CustomerDetailCustomerNameInputNewCustomer"
        Label_CustomerDetailCustomerNameInputNewCustomer.Size = New System.Drawing.Size(87, 16)
        Label_CustomerDetailCustomerNameInputNewCustomer.TabIndex = 2
        Label_CustomerDetailCustomerNameInputNewCustomer.Text = "Customer Name"
        Label_CustomerDetailCustomerNameInputNewCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Textbox_CustomerDetailCustomerNameInputNewCustomer
        '
        Textbox_CustomerDetailCustomerNameInputNewCustomer.Anchor = System.Windows.Forms.AnchorStyles.Left
        Textbox_CustomerDetailCustomerNameInputNewCustomer.AutoCompleteCustomSource.AddRange(New String() {"(001) Item 1", "(002) Item 2", "(003) Item 3", "(004) Item 4", "(005) Item 5", "(006) Item 6", "(007) Item 7", "(008) Item 8", "(009) Item 9", "(010) Item 10"})
        Textbox_CustomerDetailCustomerNameInputNewCustomer.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Textbox_CustomerDetailCustomerNameInputNewCustomer.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Textbox_CustomerDetailCustomerNameInputNewCustomer.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailCustomerNameInputNewCustomer.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_CustomerDetailCustomerNameInputNewCustomer.Cursor = System.Windows.Forms.Cursors.IBeam
        Textbox_CustomerDetailCustomerNameInputNewCustomer.DefaultText = ""
        Textbox_CustomerDetailCustomerNameInputNewCustomer.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer))
        Textbox_CustomerDetailCustomerNameInputNewCustomer.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer))
        Textbox_CustomerDetailCustomerNameInputNewCustomer.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_CustomerDetailCustomerNameInputNewCustomer.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_CustomerDetailCustomerNameInputNewCustomer.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailCustomerNameInputNewCustomer.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailCustomerNameInputNewCustomer.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
        Textbox_CustomerDetailCustomerNameInputNewCustomer.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_CustomerDetailCustomerNameInputNewCustomer.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailCustomerNameInputNewCustomer.Location = New System.Drawing.Point(0, 16)
        Textbox_CustomerDetailCustomerNameInputNewCustomer.Margin = New System.Windows.Forms.Padding(0)
        Textbox_CustomerDetailCustomerNameInputNewCustomer.Name = "Textbox_CustomerDetailCustomerNameInputNewCustomer"
        Textbox_CustomerDetailCustomerNameInputNewCustomer.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Textbox_CustomerDetailCustomerNameInputNewCustomer.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer))
        Textbox_CustomerDetailCustomerNameInputNewCustomer.PlaceholderText = "Required"
        Textbox_CustomerDetailCustomerNameInputNewCustomer.SelectedText = ""
        Textbox_CustomerDetailCustomerNameInputNewCustomer.Size = New System.Drawing.Size(342, 40)
        Textbox_CustomerDetailCustomerNameInputNewCustomer.TabIndex = 39
        '
        'FlowLayoutPanel_CustomerDetailContactPersonNameInputNewCustomer
        '
        FlowLayoutPanel_CustomerDetailContactPersonNameInputNewCustomer.AutoSize = true
        FlowLayoutPanel_CustomerDetailContactPersonNameInputNewCustomer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_CustomerDetailContactPersonNameInputNewCustomer.Controls.Add(Label_CustomerDetailContactPersonNameInputNewCustomer)
        FlowLayoutPanel_CustomerDetailContactPersonNameInputNewCustomer.Controls.Add(Textbox_CustomerDetailContactPersonNameInputNewCustomer)
        FlowLayoutPanel_CustomerDetailContactPersonNameInputNewCustomer.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_CustomerDetailContactPersonNameInputNewCustomer.Location = New System.Drawing.Point(366, 132)
        FlowLayoutPanel_CustomerDetailContactPersonNameInputNewCustomer.Margin = New System.Windows.Forms.Padding(6, 0, 12, 12)
        FlowLayoutPanel_CustomerDetailContactPersonNameInputNewCustomer.Name = "FlowLayoutPanel_CustomerDetailContactPersonNameInputNewCustomer"
        FlowLayoutPanel_CustomerDetailContactPersonNameInputNewCustomer.Size = New System.Drawing.Size(342, 56)
        FlowLayoutPanel_CustomerDetailContactPersonNameInputNewCustomer.TabIndex = 20
        '
        'Label_CustomerDetailContactPersonNameInputNewCustomer
        '
        Label_CustomerDetailContactPersonNameInputNewCustomer.AutoSize = true
        Label_CustomerDetailContactPersonNameInputNewCustomer.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_CustomerDetailContactPersonNameInputNewCustomer.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_CustomerDetailContactPersonNameInputNewCustomer.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_CustomerDetailContactPersonNameInputNewCustomer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_CustomerDetailContactPersonNameInputNewCustomer.Location = New System.Drawing.Point(0, 0)
        Label_CustomerDetailContactPersonNameInputNewCustomer.Margin = New System.Windows.Forms.Padding(0)
        Label_CustomerDetailContactPersonNameInputNewCustomer.Name = "Label_CustomerDetailContactPersonNameInputNewCustomer"
        Label_CustomerDetailContactPersonNameInputNewCustomer.Size = New System.Drawing.Size(130, 16)
        Label_CustomerDetailContactPersonNameInputNewCustomer.TabIndex = 2
        Label_CustomerDetailContactPersonNameInputNewCustomer.Text = "Contact Person Name"
        Label_CustomerDetailContactPersonNameInputNewCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Textbox_CustomerDetailContactPersonNameInputNewCustomer
        '
        Textbox_CustomerDetailContactPersonNameInputNewCustomer.Anchor = System.Windows.Forms.AnchorStyles.Left
        Textbox_CustomerDetailContactPersonNameInputNewCustomer.AutoCompleteCustomSource.AddRange(New String() {"(001) Item 1", "(002) Item 2", "(003) Item 3", "(004) Item 4", "(005) Item 5", "(006) Item 6", "(007) Item 7", "(008) Item 8", "(009) Item 9", "(010) Item 10"})
        Textbox_CustomerDetailContactPersonNameInputNewCustomer.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Textbox_CustomerDetailContactPersonNameInputNewCustomer.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Textbox_CustomerDetailContactPersonNameInputNewCustomer.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailContactPersonNameInputNewCustomer.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_CustomerDetailContactPersonNameInputNewCustomer.Cursor = System.Windows.Forms.Cursors.IBeam
        Textbox_CustomerDetailContactPersonNameInputNewCustomer.DefaultText = ""
        Textbox_CustomerDetailContactPersonNameInputNewCustomer.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer))
        Textbox_CustomerDetailContactPersonNameInputNewCustomer.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer))
        Textbox_CustomerDetailContactPersonNameInputNewCustomer.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_CustomerDetailContactPersonNameInputNewCustomer.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_CustomerDetailContactPersonNameInputNewCustomer.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailContactPersonNameInputNewCustomer.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailContactPersonNameInputNewCustomer.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
        Textbox_CustomerDetailContactPersonNameInputNewCustomer.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_CustomerDetailContactPersonNameInputNewCustomer.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailContactPersonNameInputNewCustomer.Location = New System.Drawing.Point(0, 16)
        Textbox_CustomerDetailContactPersonNameInputNewCustomer.Margin = New System.Windows.Forms.Padding(0)
        Textbox_CustomerDetailContactPersonNameInputNewCustomer.Name = "Textbox_CustomerDetailContactPersonNameInputNewCustomer"
        Textbox_CustomerDetailContactPersonNameInputNewCustomer.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Textbox_CustomerDetailContactPersonNameInputNewCustomer.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer))
        Textbox_CustomerDetailContactPersonNameInputNewCustomer.PlaceholderText = "Required"
        Textbox_CustomerDetailContactPersonNameInputNewCustomer.SelectedText = ""
        Textbox_CustomerDetailContactPersonNameInputNewCustomer.Size = New System.Drawing.Size(342, 40)
        Textbox_CustomerDetailContactPersonNameInputNewCustomer.TabIndex = 39
        '
        'FlowLayoutPanel_CustomerDetailPaymentTermInputNewCustomer
        '
        FlowLayoutPanel_CustomerDetailPaymentTermInputNewCustomer.AutoSize = true
        FlowLayoutPanel_CustomerDetailPaymentTermInputNewCustomer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_CustomerDetailPaymentTermInputNewCustomer.Controls.Add(Label_PlaceOrderPaymentTerm)
        FlowLayoutPanel_CustomerDetailPaymentTermInputNewCustomer.Controls.Add(Panel_CustomerDetailPaymentTermInputNewCustomer)
        FlowLayoutPanel_CustomerDetailPaymentTermInputNewCustomer.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_CustomerDetailPaymentTermInputNewCustomer.Location = New System.Drawing.Point(12, 200)
        FlowLayoutPanel_CustomerDetailPaymentTermInputNewCustomer.Margin = New System.Windows.Forms.Padding(12, 0, 6, 12)
        FlowLayoutPanel_CustomerDetailPaymentTermInputNewCustomer.Name = "FlowLayoutPanel_CustomerDetailPaymentTermInputNewCustomer"
        FlowLayoutPanel_CustomerDetailPaymentTermInputNewCustomer.Size = New System.Drawing.Size(342, 56)
        FlowLayoutPanel_CustomerDetailPaymentTermInputNewCustomer.TabIndex = 36
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
        'Panel_CustomerDetailPaymentTermInputNewCustomer
        '
        Panel_CustomerDetailPaymentTermInputNewCustomer.Controls.Add(Combobox_CustomerDetailPaymentTermInputNewCustomer)
        Panel_CustomerDetailPaymentTermInputNewCustomer.Controls.Add(Button_CustomerDetailPaymentTermInputNewCustomer)
        Panel_CustomerDetailPaymentTermInputNewCustomer.Location = New System.Drawing.Point(0, 16)
        Panel_CustomerDetailPaymentTermInputNewCustomer.Margin = New System.Windows.Forms.Padding(0)
        Panel_CustomerDetailPaymentTermInputNewCustomer.Name = "Panel_CustomerDetailPaymentTermInputNewCustomer"
        Panel_CustomerDetailPaymentTermInputNewCustomer.Size = New System.Drawing.Size(342, 40)
        Panel_CustomerDetailPaymentTermInputNewCustomer.TabIndex = 31
        '
        'Combobox_CustomerDetailPaymentTermInputNewCustomer
        '
        Combobox_CustomerDetailPaymentTermInputNewCustomer.BackColor = System.Drawing.Color.Transparent
        Combobox_CustomerDetailPaymentTermInputNewCustomer.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Combobox_CustomerDetailPaymentTermInputNewCustomer.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Combobox_CustomerDetailPaymentTermInputNewCustomer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Combobox_CustomerDetailPaymentTermInputNewCustomer.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Combobox_CustomerDetailPaymentTermInputNewCustomer.FocusedColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Combobox_CustomerDetailPaymentTermInputNewCustomer.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Combobox_CustomerDetailPaymentTermInputNewCustomer.Font = New System.Drawing.Font("Segoe UI", 10!)
        Combobox_CustomerDetailPaymentTermInputNewCustomer.ForeColor = System.Drawing.Color.FromArgb(CType(CType(68,Byte),Integer), CType(CType(88,Byte),Integer), CType(CType(112,Byte),Integer))
        Combobox_CustomerDetailPaymentTermInputNewCustomer.ItemHeight = 34
        Combobox_CustomerDetailPaymentTermInputNewCustomer.Items.AddRange(New Object() {"Benjamin Rollan", "Benjamin Rollan", "Benjamin Rollan", "Benjamin Rollan"})
        Combobox_CustomerDetailPaymentTermInputNewCustomer.Location = New System.Drawing.Point(0, 0)
        Combobox_CustomerDetailPaymentTermInputNewCustomer.Margin = New System.Windows.Forms.Padding(0)
        Combobox_CustomerDetailPaymentTermInputNewCustomer.Name = "Combobox_CustomerDetailPaymentTermInputNewCustomer"
        Combobox_CustomerDetailPaymentTermInputNewCustomer.Size = New System.Drawing.Size(210, 40)
        Combobox_CustomerDetailPaymentTermInputNewCustomer.TabIndex = 31
        '
        'Button_CustomerDetailPaymentTermInputNewCustomer
        '
        Button_CustomerDetailPaymentTermInputNewCustomer.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(44,Byte),Integer), CType(CType(62,Byte),Integer))
        Button_CustomerDetailPaymentTermInputNewCustomer.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Button_CustomerDetailPaymentTermInputNewCustomer.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Button_CustomerDetailPaymentTermInputNewCustomer.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer))
        Button_CustomerDetailPaymentTermInputNewCustomer.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer))
        Button_CustomerDetailPaymentTermInputNewCustomer.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Button_CustomerDetailPaymentTermInputNewCustomer.Font = New System.Drawing.Font("Segoe UI", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel)
        Button_CustomerDetailPaymentTermInputNewCustomer.ForeColor = System.Drawing.Color.White
        Button_CustomerDetailPaymentTermInputNewCustomer.Location = New System.Drawing.Point(215, 0)
        Button_CustomerDetailPaymentTermInputNewCustomer.Margin = New System.Windows.Forms.Padding(12, 0, 0, 0)
        Button_CustomerDetailPaymentTermInputNewCustomer.Name = "Button_CustomerDetailPaymentTermInputNewCustomer"
        Button_CustomerDetailPaymentTermInputNewCustomer.Size = New System.Drawing.Size(127, 40)
        Button_CustomerDetailPaymentTermInputNewCustomer.TabIndex = 31
        Button_CustomerDetailPaymentTermInputNewCustomer.Text = "New Term"
        Button_CustomerDetailPaymentTermInputNewCustomer.TextOffset = New System.Drawing.Point(0, -2)
        '
        'FlowLayoutPanel_CustomerDetailEmailInputNewCustomer
        '
        FlowLayoutPanel_CustomerDetailEmailInputNewCustomer.AutoSize = true
        FlowLayoutPanel_CustomerDetailEmailInputNewCustomer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_CustomerDetailEmailInputNewCustomer.Controls.Add(Label_CustomerDetailEmailInputNewCustomer)
        FlowLayoutPanel_CustomerDetailEmailInputNewCustomer.Controls.Add(Textbox_CustomerDetailEmailInputNewCustomer)
        FlowLayoutPanel_CustomerDetailEmailInputNewCustomer.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_CustomerDetailEmailInputNewCustomer.Location = New System.Drawing.Point(366, 200)
        FlowLayoutPanel_CustomerDetailEmailInputNewCustomer.Margin = New System.Windows.Forms.Padding(6, 0, 12, 12)
        FlowLayoutPanel_CustomerDetailEmailInputNewCustomer.Name = "FlowLayoutPanel_CustomerDetailEmailInputNewCustomer"
        FlowLayoutPanel_CustomerDetailEmailInputNewCustomer.Size = New System.Drawing.Size(342, 56)
        FlowLayoutPanel_CustomerDetailEmailInputNewCustomer.TabIndex = 21
        '
        'Label_CustomerDetailEmailInputNewCustomer
        '
        Label_CustomerDetailEmailInputNewCustomer.AutoSize = true
        Label_CustomerDetailEmailInputNewCustomer.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_CustomerDetailEmailInputNewCustomer.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_CustomerDetailEmailInputNewCustomer.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_CustomerDetailEmailInputNewCustomer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_CustomerDetailEmailInputNewCustomer.Location = New System.Drawing.Point(0, 0)
        Label_CustomerDetailEmailInputNewCustomer.Margin = New System.Windows.Forms.Padding(0)
        Label_CustomerDetailEmailInputNewCustomer.Name = "Label_CustomerDetailEmailInputNewCustomer"
        Label_CustomerDetailEmailInputNewCustomer.Size = New System.Drawing.Size(38, 16)
        Label_CustomerDetailEmailInputNewCustomer.TabIndex = 2
        Label_CustomerDetailEmailInputNewCustomer.Text = "Email"
        Label_CustomerDetailEmailInputNewCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Textbox_CustomerDetailEmailInputNewCustomer
        '
        Textbox_CustomerDetailEmailInputNewCustomer.Anchor = System.Windows.Forms.AnchorStyles.Left
        Textbox_CustomerDetailEmailInputNewCustomer.AutoCompleteCustomSource.AddRange(New String() {"(001) Item 1", "(002) Item 2", "(003) Item 3", "(004) Item 4", "(005) Item 5", "(006) Item 6", "(007) Item 7", "(008) Item 8", "(009) Item 9", "(010) Item 10"})
        Textbox_CustomerDetailEmailInputNewCustomer.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Textbox_CustomerDetailEmailInputNewCustomer.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Textbox_CustomerDetailEmailInputNewCustomer.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailEmailInputNewCustomer.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_CustomerDetailEmailInputNewCustomer.Cursor = System.Windows.Forms.Cursors.IBeam
        Textbox_CustomerDetailEmailInputNewCustomer.DefaultText = ""
        Textbox_CustomerDetailEmailInputNewCustomer.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer))
        Textbox_CustomerDetailEmailInputNewCustomer.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer))
        Textbox_CustomerDetailEmailInputNewCustomer.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_CustomerDetailEmailInputNewCustomer.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_CustomerDetailEmailInputNewCustomer.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailEmailInputNewCustomer.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailEmailInputNewCustomer.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
        Textbox_CustomerDetailEmailInputNewCustomer.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_CustomerDetailEmailInputNewCustomer.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailEmailInputNewCustomer.Location = New System.Drawing.Point(0, 16)
        Textbox_CustomerDetailEmailInputNewCustomer.Margin = New System.Windows.Forms.Padding(0)
        Textbox_CustomerDetailEmailInputNewCustomer.Name = "Textbox_CustomerDetailEmailInputNewCustomer"
        Textbox_CustomerDetailEmailInputNewCustomer.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Textbox_CustomerDetailEmailInputNewCustomer.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer))
        Textbox_CustomerDetailEmailInputNewCustomer.PlaceholderText = "Required"
        Textbox_CustomerDetailEmailInputNewCustomer.SelectedText = ""
        Textbox_CustomerDetailEmailInputNewCustomer.Size = New System.Drawing.Size(342, 40)
        Textbox_CustomerDetailEmailInputNewCustomer.TabIndex = 39
        '
        'FlowLayoutPanel_CustomerDetailPhoneInputNewCustomer
        '
        FlowLayoutPanel_CustomerDetailPhoneInputNewCustomer.AutoSize = true
        FlowLayoutPanel_CustomerDetailPhoneInputNewCustomer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_CustomerDetailPhoneInputNewCustomer.Controls.Add(Label_CustomerDetailPhoneInputNewCustomer)
        FlowLayoutPanel_CustomerDetailPhoneInputNewCustomer.Controls.Add(Textbox_CustomerDetailPhoneInputNewCustomer)
        FlowLayoutPanel_CustomerDetailPhoneInputNewCustomer.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_CustomerDetailPhoneInputNewCustomer.Location = New System.Drawing.Point(12, 268)
        FlowLayoutPanel_CustomerDetailPhoneInputNewCustomer.Margin = New System.Windows.Forms.Padding(12, 0, 6, 12)
        FlowLayoutPanel_CustomerDetailPhoneInputNewCustomer.Name = "FlowLayoutPanel_CustomerDetailPhoneInputNewCustomer"
        FlowLayoutPanel_CustomerDetailPhoneInputNewCustomer.Size = New System.Drawing.Size(342, 56)
        FlowLayoutPanel_CustomerDetailPhoneInputNewCustomer.TabIndex = 22
        '
        'Label_CustomerDetailPhoneInputNewCustomer
        '
        Label_CustomerDetailPhoneInputNewCustomer.AutoSize = true
        Label_CustomerDetailPhoneInputNewCustomer.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_CustomerDetailPhoneInputNewCustomer.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_CustomerDetailPhoneInputNewCustomer.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_CustomerDetailPhoneInputNewCustomer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_CustomerDetailPhoneInputNewCustomer.Location = New System.Drawing.Point(0, 0)
        Label_CustomerDetailPhoneInputNewCustomer.Margin = New System.Windows.Forms.Padding(0)
        Label_CustomerDetailPhoneInputNewCustomer.Name = "Label_CustomerDetailPhoneInputNewCustomer"
        Label_CustomerDetailPhoneInputNewCustomer.Size = New System.Drawing.Size(43, 16)
        Label_CustomerDetailPhoneInputNewCustomer.TabIndex = 2
        Label_CustomerDetailPhoneInputNewCustomer.Text = "Phone"
        Label_CustomerDetailPhoneInputNewCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Textbox_CustomerDetailPhoneInputNewCustomer
        '
        Textbox_CustomerDetailPhoneInputNewCustomer.Anchor = System.Windows.Forms.AnchorStyles.Left
        Textbox_CustomerDetailPhoneInputNewCustomer.AutoCompleteCustomSource.AddRange(New String() {"(001) Item 1", "(002) Item 2", "(003) Item 3", "(004) Item 4", "(005) Item 5", "(006) Item 6", "(007) Item 7", "(008) Item 8", "(009) Item 9", "(010) Item 10"})
        Textbox_CustomerDetailPhoneInputNewCustomer.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Textbox_CustomerDetailPhoneInputNewCustomer.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Textbox_CustomerDetailPhoneInputNewCustomer.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailPhoneInputNewCustomer.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_CustomerDetailPhoneInputNewCustomer.Cursor = System.Windows.Forms.Cursors.IBeam
        Textbox_CustomerDetailPhoneInputNewCustomer.DefaultText = ""
        Textbox_CustomerDetailPhoneInputNewCustomer.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer))
        Textbox_CustomerDetailPhoneInputNewCustomer.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer))
        Textbox_CustomerDetailPhoneInputNewCustomer.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_CustomerDetailPhoneInputNewCustomer.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_CustomerDetailPhoneInputNewCustomer.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailPhoneInputNewCustomer.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailPhoneInputNewCustomer.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
        Textbox_CustomerDetailPhoneInputNewCustomer.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_CustomerDetailPhoneInputNewCustomer.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailPhoneInputNewCustomer.Location = New System.Drawing.Point(0, 16)
        Textbox_CustomerDetailPhoneInputNewCustomer.Margin = New System.Windows.Forms.Padding(0)
        Textbox_CustomerDetailPhoneInputNewCustomer.Name = "Textbox_CustomerDetailPhoneInputNewCustomer"
        Textbox_CustomerDetailPhoneInputNewCustomer.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Textbox_CustomerDetailPhoneInputNewCustomer.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer))
        Textbox_CustomerDetailPhoneInputNewCustomer.PlaceholderText = "Required"
        Textbox_CustomerDetailPhoneInputNewCustomer.SelectedText = ""
        Textbox_CustomerDetailPhoneInputNewCustomer.Size = New System.Drawing.Size(342, 40)
        Textbox_CustomerDetailPhoneInputNewCustomer.TabIndex = 39
        '
        'Label_CustomerDetailAddressLineInputNewCustomer
        '
        Label_CustomerDetailAddressLineInputNewCustomer.AutoSize = true
        Label_CustomerDetailAddressLineInputNewCustomer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Label_CustomerDetailAddressLineInputNewCustomer.Controls.Add(Label_CustomerAddressLinePhoneInputNewCustomer)
        Label_CustomerDetailAddressLineInputNewCustomer.Controls.Add(Textbox_CustomerDetailAddressLineInputNewCustomer)
        Label_CustomerDetailAddressLineInputNewCustomer.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Label_CustomerDetailAddressLineInputNewCustomer.Location = New System.Drawing.Point(366, 268)
        Label_CustomerDetailAddressLineInputNewCustomer.Margin = New System.Windows.Forms.Padding(6, 0, 12, 12)
        Label_CustomerDetailAddressLineInputNewCustomer.Name = "Label_CustomerDetailAddressLineInputNewCustomer"
        Label_CustomerDetailAddressLineInputNewCustomer.Size = New System.Drawing.Size(342, 56)
        Label_CustomerDetailAddressLineInputNewCustomer.TabIndex = 23
        '
        'Label_CustomerAddressLinePhoneInputNewCustomer
        '
        Label_CustomerAddressLinePhoneInputNewCustomer.AutoSize = true
        Label_CustomerAddressLinePhoneInputNewCustomer.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_CustomerAddressLinePhoneInputNewCustomer.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_CustomerAddressLinePhoneInputNewCustomer.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_CustomerAddressLinePhoneInputNewCustomer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_CustomerAddressLinePhoneInputNewCustomer.Location = New System.Drawing.Point(0, 0)
        Label_CustomerAddressLinePhoneInputNewCustomer.Margin = New System.Windows.Forms.Padding(0)
        Label_CustomerAddressLinePhoneInputNewCustomer.Name = "Label_CustomerAddressLinePhoneInputNewCustomer"
        Label_CustomerAddressLinePhoneInputNewCustomer.Size = New System.Drawing.Size(78, 16)
        Label_CustomerAddressLinePhoneInputNewCustomer.TabIndex = 2
        Label_CustomerAddressLinePhoneInputNewCustomer.Text = "Address Line"
        Label_CustomerAddressLinePhoneInputNewCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Textbox_CustomerDetailAddressLineInputNewCustomer
        '
        Textbox_CustomerDetailAddressLineInputNewCustomer.Anchor = System.Windows.Forms.AnchorStyles.Left
        Textbox_CustomerDetailAddressLineInputNewCustomer.AutoCompleteCustomSource.AddRange(New String() {"(001) Item 1", "(002) Item 2", "(003) Item 3", "(004) Item 4", "(005) Item 5", "(006) Item 6", "(007) Item 7", "(008) Item 8", "(009) Item 9", "(010) Item 10"})
        Textbox_CustomerDetailAddressLineInputNewCustomer.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Textbox_CustomerDetailAddressLineInputNewCustomer.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Textbox_CustomerDetailAddressLineInputNewCustomer.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailAddressLineInputNewCustomer.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_CustomerDetailAddressLineInputNewCustomer.Cursor = System.Windows.Forms.Cursors.IBeam
        Textbox_CustomerDetailAddressLineInputNewCustomer.DefaultText = ""
        Textbox_CustomerDetailAddressLineInputNewCustomer.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer))
        Textbox_CustomerDetailAddressLineInputNewCustomer.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer))
        Textbox_CustomerDetailAddressLineInputNewCustomer.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_CustomerDetailAddressLineInputNewCustomer.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_CustomerDetailAddressLineInputNewCustomer.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailAddressLineInputNewCustomer.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailAddressLineInputNewCustomer.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
        Textbox_CustomerDetailAddressLineInputNewCustomer.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_CustomerDetailAddressLineInputNewCustomer.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailAddressLineInputNewCustomer.Location = New System.Drawing.Point(0, 16)
        Textbox_CustomerDetailAddressLineInputNewCustomer.Margin = New System.Windows.Forms.Padding(0)
        Textbox_CustomerDetailAddressLineInputNewCustomer.Name = "Textbox_CustomerDetailAddressLineInputNewCustomer"
        Textbox_CustomerDetailAddressLineInputNewCustomer.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Textbox_CustomerDetailAddressLineInputNewCustomer.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer))
        Textbox_CustomerDetailAddressLineInputNewCustomer.PlaceholderText = "Required"
        Textbox_CustomerDetailAddressLineInputNewCustomer.SelectedText = ""
        Textbox_CustomerDetailAddressLineInputNewCustomer.Size = New System.Drawing.Size(342, 40)
        Textbox_CustomerDetailAddressLineInputNewCustomer.TabIndex = 39
        '
        'FlowLayoutPanel_CustomerDetailCityInputNewCustomer
        '
        FlowLayoutPanel_CustomerDetailCityInputNewCustomer.AutoSize = true
        FlowLayoutPanel_CustomerDetailCityInputNewCustomer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_CustomerDetailCityInputNewCustomer.Controls.Add(Label_CustomerDetailCityInputNewCustomer)
        FlowLayoutPanel_CustomerDetailCityInputNewCustomer.Controls.Add(Textbox_CustomerDetailCityInputNewCustomer)
        FlowLayoutPanel_CustomerDetailCityInputNewCustomer.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_CustomerDetailCityInputNewCustomer.Location = New System.Drawing.Point(12, 336)
        FlowLayoutPanel_CustomerDetailCityInputNewCustomer.Margin = New System.Windows.Forms.Padding(12, 0, 6, 12)
        FlowLayoutPanel_CustomerDetailCityInputNewCustomer.Name = "FlowLayoutPanel_CustomerDetailCityInputNewCustomer"
        FlowLayoutPanel_CustomerDetailCityInputNewCustomer.Size = New System.Drawing.Size(342, 56)
        FlowLayoutPanel_CustomerDetailCityInputNewCustomer.TabIndex = 24
        '
        'Label_CustomerDetailCityInputNewCustomer
        '
        Label_CustomerDetailCityInputNewCustomer.AutoSize = true
        Label_CustomerDetailCityInputNewCustomer.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_CustomerDetailCityInputNewCustomer.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_CustomerDetailCityInputNewCustomer.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_CustomerDetailCityInputNewCustomer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_CustomerDetailCityInputNewCustomer.Location = New System.Drawing.Point(0, 0)
        Label_CustomerDetailCityInputNewCustomer.Margin = New System.Windows.Forms.Padding(0)
        Label_CustomerDetailCityInputNewCustomer.Name = "Label_CustomerDetailCityInputNewCustomer"
        Label_CustomerDetailCityInputNewCustomer.Size = New System.Drawing.Size(28, 16)
        Label_CustomerDetailCityInputNewCustomer.TabIndex = 2
        Label_CustomerDetailCityInputNewCustomer.Text = "City"
        Label_CustomerDetailCityInputNewCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Textbox_CustomerDetailCityInputNewCustomer
        '
        Textbox_CustomerDetailCityInputNewCustomer.Anchor = System.Windows.Forms.AnchorStyles.Left
        Textbox_CustomerDetailCityInputNewCustomer.AutoCompleteCustomSource.AddRange(New String() {"(001) Item 1", "(002) Item 2", "(003) Item 3", "(004) Item 4", "(005) Item 5", "(006) Item 6", "(007) Item 7", "(008) Item 8", "(009) Item 9", "(010) Item 10"})
        Textbox_CustomerDetailCityInputNewCustomer.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Textbox_CustomerDetailCityInputNewCustomer.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Textbox_CustomerDetailCityInputNewCustomer.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailCityInputNewCustomer.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_CustomerDetailCityInputNewCustomer.Cursor = System.Windows.Forms.Cursors.IBeam
        Textbox_CustomerDetailCityInputNewCustomer.DefaultText = ""
        Textbox_CustomerDetailCityInputNewCustomer.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer))
        Textbox_CustomerDetailCityInputNewCustomer.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer))
        Textbox_CustomerDetailCityInputNewCustomer.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_CustomerDetailCityInputNewCustomer.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_CustomerDetailCityInputNewCustomer.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailCityInputNewCustomer.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailCityInputNewCustomer.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
        Textbox_CustomerDetailCityInputNewCustomer.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_CustomerDetailCityInputNewCustomer.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailCityInputNewCustomer.Location = New System.Drawing.Point(0, 16)
        Textbox_CustomerDetailCityInputNewCustomer.Margin = New System.Windows.Forms.Padding(0)
        Textbox_CustomerDetailCityInputNewCustomer.Name = "Textbox_CustomerDetailCityInputNewCustomer"
        Textbox_CustomerDetailCityInputNewCustomer.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Textbox_CustomerDetailCityInputNewCustomer.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer))
        Textbox_CustomerDetailCityInputNewCustomer.PlaceholderText = "Required"
        Textbox_CustomerDetailCityInputNewCustomer.SelectedText = ""
        Textbox_CustomerDetailCityInputNewCustomer.Size = New System.Drawing.Size(342, 40)
        Textbox_CustomerDetailCityInputNewCustomer.TabIndex = 39
        '
        'FlowLayoutPanel_CustomerDetailZipCodeInputNewCustomer
        '
        FlowLayoutPanel_CustomerDetailZipCodeInputNewCustomer.AutoSize = true
        FlowLayoutPanel_CustomerDetailZipCodeInputNewCustomer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_CustomerDetailZipCodeInputNewCustomer.Controls.Add(Label_CustomerDetailZipCodeInputNewCustomer)
        FlowLayoutPanel_CustomerDetailZipCodeInputNewCustomer.Controls.Add(Textbox_CustomerDetailZipCodeInputNewCustomer)
        FlowLayoutPanel_CustomerDetailZipCodeInputNewCustomer.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_CustomerDetailZipCodeInputNewCustomer.Location = New System.Drawing.Point(366, 336)
        FlowLayoutPanel_CustomerDetailZipCodeInputNewCustomer.Margin = New System.Windows.Forms.Padding(6, 0, 12, 12)
        FlowLayoutPanel_CustomerDetailZipCodeInputNewCustomer.Name = "FlowLayoutPanel_CustomerDetailZipCodeInputNewCustomer"
        FlowLayoutPanel_CustomerDetailZipCodeInputNewCustomer.Size = New System.Drawing.Size(342, 56)
        FlowLayoutPanel_CustomerDetailZipCodeInputNewCustomer.TabIndex = 25
        '
        'Label_CustomerDetailZipCodeInputNewCustomer
        '
        Label_CustomerDetailZipCodeInputNewCustomer.AutoSize = true
        Label_CustomerDetailZipCodeInputNewCustomer.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_CustomerDetailZipCodeInputNewCustomer.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_CustomerDetailZipCodeInputNewCustomer.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_CustomerDetailZipCodeInputNewCustomer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_CustomerDetailZipCodeInputNewCustomer.Location = New System.Drawing.Point(0, 0)
        Label_CustomerDetailZipCodeInputNewCustomer.Margin = New System.Windows.Forms.Padding(0)
        Label_CustomerDetailZipCodeInputNewCustomer.Name = "Label_CustomerDetailZipCodeInputNewCustomer"
        Label_CustomerDetailZipCodeInputNewCustomer.Size = New System.Drawing.Size(59, 16)
        Label_CustomerDetailZipCodeInputNewCustomer.TabIndex = 2
        Label_CustomerDetailZipCodeInputNewCustomer.Text = "Zip Code"
        Label_CustomerDetailZipCodeInputNewCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Textbox_CustomerDetailZipCodeInputNewCustomer
        '
        Textbox_CustomerDetailZipCodeInputNewCustomer.Anchor = System.Windows.Forms.AnchorStyles.Left
        Textbox_CustomerDetailZipCodeInputNewCustomer.AutoCompleteCustomSource.AddRange(New String() {"(001) Item 1", "(002) Item 2", "(003) Item 3", "(004) Item 4", "(005) Item 5", "(006) Item 6", "(007) Item 7", "(008) Item 8", "(009) Item 9", "(010) Item 10"})
        Textbox_CustomerDetailZipCodeInputNewCustomer.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Textbox_CustomerDetailZipCodeInputNewCustomer.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Textbox_CustomerDetailZipCodeInputNewCustomer.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailZipCodeInputNewCustomer.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_CustomerDetailZipCodeInputNewCustomer.Cursor = System.Windows.Forms.Cursors.IBeam
        Textbox_CustomerDetailZipCodeInputNewCustomer.DefaultText = ""
        Textbox_CustomerDetailZipCodeInputNewCustomer.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer))
        Textbox_CustomerDetailZipCodeInputNewCustomer.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer))
        Textbox_CustomerDetailZipCodeInputNewCustomer.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_CustomerDetailZipCodeInputNewCustomer.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_CustomerDetailZipCodeInputNewCustomer.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailZipCodeInputNewCustomer.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailZipCodeInputNewCustomer.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
        Textbox_CustomerDetailZipCodeInputNewCustomer.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_CustomerDetailZipCodeInputNewCustomer.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailZipCodeInputNewCustomer.Location = New System.Drawing.Point(0, 16)
        Textbox_CustomerDetailZipCodeInputNewCustomer.Margin = New System.Windows.Forms.Padding(0)
        Textbox_CustomerDetailZipCodeInputNewCustomer.Name = "Textbox_CustomerDetailZipCodeInputNewCustomer"
        Textbox_CustomerDetailZipCodeInputNewCustomer.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Textbox_CustomerDetailZipCodeInputNewCustomer.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer))
        Textbox_CustomerDetailZipCodeInputNewCustomer.PlaceholderText = "Required"
        Textbox_CustomerDetailZipCodeInputNewCustomer.SelectedText = ""
        Textbox_CustomerDetailZipCodeInputNewCustomer.Size = New System.Drawing.Size(342, 40)
        Textbox_CustomerDetailZipCodeInputNewCustomer.TabIndex = 39
        '
        'FlowLayoutPanel_CustomerDetailCountryInputNewCustomer
        '
        FlowLayoutPanel_CustomerDetailCountryInputNewCustomer.AutoSize = true
        FlowLayoutPanel_CustomerDetailCountryInputNewCustomer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_CustomerDetailCountryInputNewCustomer.Controls.Add(Label_CustomerDetailCountryInputNewCustomer)
        FlowLayoutPanel_CustomerDetailCountryInputNewCustomer.Controls.Add(Textbox_CustomerDetailCountryInputNewCustomer)
        FlowLayoutPanel_CustomerDetailCountryInputNewCustomer.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_CustomerDetailCountryInputNewCustomer.Location = New System.Drawing.Point(12, 404)
        FlowLayoutPanel_CustomerDetailCountryInputNewCustomer.Margin = New System.Windows.Forms.Padding(12, 0, 6, 12)
        FlowLayoutPanel_CustomerDetailCountryInputNewCustomer.Name = "FlowLayoutPanel_CustomerDetailCountryInputNewCustomer"
        FlowLayoutPanel_CustomerDetailCountryInputNewCustomer.Size = New System.Drawing.Size(342, 56)
        FlowLayoutPanel_CustomerDetailCountryInputNewCustomer.TabIndex = 26
        '
        'Label_CustomerDetailCountryInputNewCustomer
        '
        Label_CustomerDetailCountryInputNewCustomer.AutoSize = true
        Label_CustomerDetailCountryInputNewCustomer.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_CustomerDetailCountryInputNewCustomer.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_CustomerDetailCountryInputNewCustomer.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_CustomerDetailCountryInputNewCustomer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_CustomerDetailCountryInputNewCustomer.Location = New System.Drawing.Point(0, 0)
        Label_CustomerDetailCountryInputNewCustomer.Margin = New System.Windows.Forms.Padding(0)
        Label_CustomerDetailCountryInputNewCustomer.Name = "Label_CustomerDetailCountryInputNewCustomer"
        Label_CustomerDetailCountryInputNewCustomer.Size = New System.Drawing.Size(51, 16)
        Label_CustomerDetailCountryInputNewCustomer.TabIndex = 2
        Label_CustomerDetailCountryInputNewCustomer.Text = "Country"
        Label_CustomerDetailCountryInputNewCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Textbox_CustomerDetailCountryInputNewCustomer
        '
        Textbox_CustomerDetailCountryInputNewCustomer.Anchor = System.Windows.Forms.AnchorStyles.Left
        Textbox_CustomerDetailCountryInputNewCustomer.AutoCompleteCustomSource.AddRange(New String() {"(001) Item 1", "(002) Item 2", "(003) Item 3", "(004) Item 4", "(005) Item 5", "(006) Item 6", "(007) Item 7", "(008) Item 8", "(009) Item 9", "(010) Item 10"})
        Textbox_CustomerDetailCountryInputNewCustomer.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Textbox_CustomerDetailCountryInputNewCustomer.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Textbox_CustomerDetailCountryInputNewCustomer.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailCountryInputNewCustomer.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_CustomerDetailCountryInputNewCustomer.Cursor = System.Windows.Forms.Cursors.IBeam
        Textbox_CustomerDetailCountryInputNewCustomer.DefaultText = ""
        Textbox_CustomerDetailCountryInputNewCustomer.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer))
        Textbox_CustomerDetailCountryInputNewCustomer.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer))
        Textbox_CustomerDetailCountryInputNewCustomer.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_CustomerDetailCountryInputNewCustomer.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_CustomerDetailCountryInputNewCustomer.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailCountryInputNewCustomer.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailCountryInputNewCustomer.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
        Textbox_CustomerDetailCountryInputNewCustomer.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_CustomerDetailCountryInputNewCustomer.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailCountryInputNewCustomer.Location = New System.Drawing.Point(0, 16)
        Textbox_CustomerDetailCountryInputNewCustomer.Margin = New System.Windows.Forms.Padding(0)
        Textbox_CustomerDetailCountryInputNewCustomer.Name = "Textbox_CustomerDetailCountryInputNewCustomer"
        Textbox_CustomerDetailCountryInputNewCustomer.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Textbox_CustomerDetailCountryInputNewCustomer.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer))
        Textbox_CustomerDetailCountryInputNewCustomer.PlaceholderText = "Required"
        Textbox_CustomerDetailCountryInputNewCustomer.SelectedText = ""
        Textbox_CustomerDetailCountryInputNewCustomer.Size = New System.Drawing.Size(342, 40)
        Textbox_CustomerDetailCountryInputNewCustomer.TabIndex = 39
        '
        'FlowLayoutPanel1
        '
        FlowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel1.Location = New System.Drawing.Point(366, 404)
        FlowLayoutPanel1.Margin = New System.Windows.Forms.Padding(6, 0, 12, 12)
        FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        FlowLayoutPanel1.Size = New System.Drawing.Size(342, 56)
        FlowLayoutPanel1.TabIndex = 37
        '
        'Button_ResetNewCustomer
        '
        Button_ResetNewCustomer.BackColor = System.Drawing.Color.FromArgb(CType(CType(220,Byte),Integer), CType(CType(76,Byte),Integer), CType(CType(100,Byte),Integer))
        Button_ResetNewCustomer.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Button_ResetNewCustomer.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Button_ResetNewCustomer.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer))
        Button_ResetNewCustomer.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer))
        Button_ResetNewCustomer.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Button_ResetNewCustomer.Font = New System.Drawing.Font("Segoe UI", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel)
        Button_ResetNewCustomer.ForeColor = System.Drawing.Color.White
        Button_ResetNewCustomer.Location = New System.Drawing.Point(12, 472)
        Button_ResetNewCustomer.Margin = New System.Windows.Forms.Padding(12, 0, 6, 12)
        Button_ResetNewCustomer.Name = "Button_ResetNewCustomer"
        Button_ResetNewCustomer.Size = New System.Drawing.Size(342, 40)
        Button_ResetNewCustomer.TabIndex = 34
        Button_ResetNewCustomer.Text = "Reset"
        Button_ResetNewCustomer.TextOffset = New System.Drawing.Point(0, -2)
        '
        'Button_MakeCustomerNewCustomer
        '
        Button_MakeCustomerNewCustomer.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(44,Byte),Integer), CType(CType(62,Byte),Integer))
        Button_MakeCustomerNewCustomer.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Button_MakeCustomerNewCustomer.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Button_MakeCustomerNewCustomer.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer))
        Button_MakeCustomerNewCustomer.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer))
        Button_MakeCustomerNewCustomer.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Button_MakeCustomerNewCustomer.Font = New System.Drawing.Font("Segoe UI", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel)
        Button_MakeCustomerNewCustomer.ForeColor = System.Drawing.Color.White
        Button_MakeCustomerNewCustomer.Location = New System.Drawing.Point(366, 472)
        Button_MakeCustomerNewCustomer.Margin = New System.Windows.Forms.Padding(6, 0, 12, 12)
        Button_MakeCustomerNewCustomer.Name = "Button_MakeCustomerNewCustomer"
        Button_MakeCustomerNewCustomer.Size = New System.Drawing.Size(342, 40)
        Button_MakeCustomerNewCustomer.TabIndex = 33
        Button_MakeCustomerNewCustomer.Text = "Create Customer"
        Button_MakeCustomerNewCustomer.TextOffset = New System.Drawing.Point(0, -2)
        '
        'FlowLayoutPanel_DateRangeInputsSales
        '
        FlowLayoutPanel_DateRangeInputsSales.AutoSize = true
        FlowLayoutPanel_DateRangeInputsSales.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_DateRangeInputsSales.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_DateRangeInputsSales.Location = New System.Drawing.Point(720, 472)
        FlowLayoutPanel_DateRangeInputsSales.Margin = New System.Windows.Forms.Padding(0, 0, 0, 42)
        FlowLayoutPanel_DateRangeInputsSales.Name = "FlowLayoutPanel_DateRangeInputsSales"
        FlowLayoutPanel_DateRangeInputsSales.Size = New System.Drawing.Size(0, 0)
        FlowLayoutPanel_DateRangeInputsSales.TabIndex = 35
        '
        '_Temporary_NewCustomer
        '
        Prompt_NewCustomer.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Prompt_NewCustomer.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Prompt_NewCustomer.ClientSize = New System.Drawing.Size(720, 720)
        Prompt_NewCustomer.Controls.Add(FlowLayoutPanel_NewCustomer)
        Prompt_NewCustomer.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Prompt_NewCustomer.Text = "New Customer"
        FlowLayoutPanel_NewCustomer.ResumeLayout(false)
        FlowLayoutPanel_NewCustomer.PerformLayout
        FlowLayoutPanel_PageHeaderNewCustomer.ResumeLayout(false)
        FlowLayoutPanel_PageHeaderNewCustomer.PerformLayout
        FlowLayoutPanel_CustomerDetailCustomerNameInputNewCustomer.ResumeLayout(false)
        FlowLayoutPanel_CustomerDetailCustomerNameInputNewCustomer.PerformLayout
        FlowLayoutPanel_CustomerDetailContactPersonNameInputNewCustomer.ResumeLayout(false)
        FlowLayoutPanel_CustomerDetailContactPersonNameInputNewCustomer.PerformLayout
        FlowLayoutPanel_CustomerDetailPaymentTermInputNewCustomer.ResumeLayout(false)
        FlowLayoutPanel_CustomerDetailPaymentTermInputNewCustomer.PerformLayout
        Panel_CustomerDetailPaymentTermInputNewCustomer.ResumeLayout(false)
        FlowLayoutPanel_CustomerDetailEmailInputNewCustomer.ResumeLayout(false)
        FlowLayoutPanel_CustomerDetailEmailInputNewCustomer.PerformLayout
        FlowLayoutPanel_CustomerDetailPhoneInputNewCustomer.ResumeLayout(false)
        FlowLayoutPanel_CustomerDetailPhoneInputNewCustomer.PerformLayout
        Label_CustomerDetailAddressLineInputNewCustomer.ResumeLayout(false)
        Label_CustomerDetailAddressLineInputNewCustomer.PerformLayout
        FlowLayoutPanel_CustomerDetailCityInputNewCustomer.ResumeLayout(false)
        FlowLayoutPanel_CustomerDetailCityInputNewCustomer.PerformLayout
        FlowLayoutPanel_CustomerDetailZipCodeInputNewCustomer.ResumeLayout(false)
        FlowLayoutPanel_CustomerDetailZipCodeInputNewCustomer.PerformLayout
        FlowLayoutPanel_CustomerDetailCountryInputNewCustomer.ResumeLayout(false)
        FlowLayoutPanel_CustomerDetailCountryInputNewCustomer.PerformLayout
        ResumeLayout(false)

        PaymentTerms = CreatePaymentTermArray()
        PopulatePaymentTermComboBox(Combobox_CustomerDetailPaymentTermInputNewCustomer)

        AddHandler Button_CustomerDetailPaymentTermInputNewCustomer.Click,
        Sub(senderrr As Object, eee As EventArgs)
            Dim Prompt_NewPaymentTerm As New Form()
            Dim FlowLayoutPanel_TermDetailsNewTerm = New System.Windows.Forms.FlowLayoutPanel()
            Dim FlowLayoutPanel_PageHeaderNewTerm = New System.Windows.Forms.FlowLayoutPanel()
            Dim Label_PageHeaderNewTerm = New System.Windows.Forms.Label()
            Dim Label_TermDetailsNewTerm = New System.Windows.Forms.Label()
            Dim FlowLayoutPanel_TermDetailTermNameInputNewTerm = New System.Windows.Forms.FlowLayoutPanel()
            Dim Label_TermDetailTermNameInputNewTerm = New System.Windows.Forms.Label()
            Dim Textbox_TermDetailTermNameInputNewTerm = New Guna.UI2.WinForms.Guna2TextBox()
            Dim idk1 = New System.Windows.Forms.FlowLayoutPanel()
            Dim Button_MakeTermNewSale = New Guna.UI2.WinForms.Guna2Button()
            Dim Panel_PageHeaderIconNewTerm = New System.Windows.Forms.Panel()
            Dim idk2 = New System.Windows.Forms.FlowLayoutPanel()
            Dim Label1 = New System.Windows.Forms.Label()
            Dim Textbox_TermDetailsTermDaysInputNewTerm = New Guna.UI2.WinForms.Guna2TextBox()
            FlowLayoutPanel_TermDetailsNewTerm.SuspendLayout
            FlowLayoutPanel_PageHeaderNewTerm.SuspendLayout
            FlowLayoutPanel_TermDetailTermNameInputNewTerm.SuspendLayout
            idk2.SuspendLayout
            SuspendLayout
    '
            'FlowLayoutPanel_TermDetailsNewTerm
            '
            FlowLayoutPanel_TermDetailsNewTerm.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
            FlowLayoutPanel_TermDetailsNewTerm.Controls.Add(FlowLayoutPanel_PageHeaderNewTerm)
            FlowLayoutPanel_TermDetailsNewTerm.Controls.Add(Label_TermDetailsNewTerm)
            FlowLayoutPanel_TermDetailsNewTerm.Controls.Add(FlowLayoutPanel_TermDetailTermNameInputNewTerm)
            FlowLayoutPanel_TermDetailsNewTerm.Controls.Add(idk1)
            FlowLayoutPanel_TermDetailsNewTerm.Controls.Add(idk2)
            FlowLayoutPanel_TermDetailsNewTerm.Controls.Add(Button_MakeTermNewSale)
            FlowLayoutPanel_TermDetailsNewTerm.Location = New System.Drawing.Point(0, 0)
            FlowLayoutPanel_TermDetailsNewTerm.Margin = New System.Windows.Forms.Padding(0)
            FlowLayoutPanel_TermDetailsNewTerm.Name = "FlowLayoutPanel_TermDetailsNewTerm"
            FlowLayoutPanel_TermDetailsNewTerm.Size = New System.Drawing.Size(366, 350)
            FlowLayoutPanel_TermDetailsNewTerm.TabIndex = 35
            '
            'FlowLayoutPanel_PageHeaderNewTerm
            '
            FlowLayoutPanel_PageHeaderNewTerm.AutoSize = true
            FlowLayoutPanel_PageHeaderNewTerm.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            FlowLayoutPanel_PageHeaderNewTerm.Controls.Add(Panel_PageHeaderIconNewTerm)
            FlowLayoutPanel_PageHeaderNewTerm.Controls.Add(Label_PageHeaderNewTerm)
            FlowLayoutPanel_PageHeaderNewTerm.Location = New System.Drawing.Point(12, 24)
            FlowLayoutPanel_PageHeaderNewTerm.Margin = New System.Windows.Forms.Padding(12, 24, 0, 24)
            FlowLayoutPanel_PageHeaderNewTerm.Name = "FlowLayoutPanel_PageHeaderNewTerm"
            FlowLayoutPanel_PageHeaderNewTerm.Size = New System.Drawing.Size(289, 32)
            FlowLayoutPanel_PageHeaderNewTerm.TabIndex = 0
            '
            'Label_PageHeaderNewTerm
            '
            Label_PageHeaderNewTerm.Anchor = System.Windows.Forms.AnchorStyles.Left
            Label_PageHeaderNewTerm.AutoSize = true
            Label_PageHeaderNewTerm.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
            Label_PageHeaderNewTerm.Font = New System.Drawing.Font("Microsoft JhengHei", 24!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
            Label_PageHeaderNewTerm.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
            Label_PageHeaderNewTerm.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Label_PageHeaderNewTerm.Location = New System.Drawing.Point(48, 0)
            Label_PageHeaderNewTerm.Margin = New System.Windows.Forms.Padding(0)
            Label_PageHeaderNewTerm.Name = "Label_PageHeaderNewTerm"
            Label_PageHeaderNewTerm.Size = New System.Drawing.Size(241, 31)
            Label_PageHeaderNewTerm.TabIndex = 2
            Label_PageHeaderNewTerm.Text = "New Payment Term"
            Label_PageHeaderNewTerm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'Label_TermDetailsNewTerm
            '
            Label_TermDetailsNewTerm.Anchor = System.Windows.Forms.AnchorStyles.Left
            Label_TermDetailsNewTerm.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
            Label_TermDetailsNewTerm.Font = New System.Drawing.Font("Microsoft JhengHei", 22!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
            Label_TermDetailsNewTerm.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
            Label_TermDetailsNewTerm.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Label_TermDetailsNewTerm.Location = New System.Drawing.Point(12, 80)
            Label_TermDetailsNewTerm.Margin = New System.Windows.Forms.Padding(12, 0, 12, 24)
            Label_TermDetailsNewTerm.Name = "Label_TermDetailsNewTerm"
            Label_TermDetailsNewTerm.Size = New System.Drawing.Size(342, 28)
            Label_TermDetailsNewTerm.TabIndex = 18
            Label_TermDetailsNewTerm.Text = "Payment Term Details"
            Label_TermDetailsNewTerm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'FlowLayoutPanel_TermDetailTermNameInputNewTerm
            '
            FlowLayoutPanel_TermDetailTermNameInputNewTerm.AutoSize = true
            FlowLayoutPanel_TermDetailTermNameInputNewTerm.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            FlowLayoutPanel_TermDetailTermNameInputNewTerm.Controls.Add(Label_TermDetailTermNameInputNewTerm)
            FlowLayoutPanel_TermDetailTermNameInputNewTerm.Controls.Add(Textbox_TermDetailTermNameInputNewTerm)
            FlowLayoutPanel_TermDetailTermNameInputNewTerm.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
            FlowLayoutPanel_TermDetailTermNameInputNewTerm.Location = New System.Drawing.Point(12, 132)
            FlowLayoutPanel_TermDetailTermNameInputNewTerm.Margin = New System.Windows.Forms.Padding(12, 0, 6, 12)
            FlowLayoutPanel_TermDetailTermNameInputNewTerm.Name = "FlowLayoutPanel_TermDetailTermNameInputNewTerm"
            FlowLayoutPanel_TermDetailTermNameInputNewTerm.Size = New System.Drawing.Size(342, 56)
            FlowLayoutPanel_TermDetailTermNameInputNewTerm.TabIndex = 19
            '
            'Label_TermDetailTermNameInputNewTerm
            '
            Label_TermDetailTermNameInputNewTerm.AutoSize = true
            Label_TermDetailTermNameInputNewTerm.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
            Label_TermDetailTermNameInputNewTerm.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
            Label_TermDetailTermNameInputNewTerm.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
            Label_TermDetailTermNameInputNewTerm.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Label_TermDetailTermNameInputNewTerm.Location = New System.Drawing.Point(0, 0)
            Label_TermDetailTermNameInputNewTerm.Margin = New System.Windows.Forms.Padding(0)
            Label_TermDetailTermNameInputNewTerm.Name = "Label_TermDetailTermNameInputNewTerm"
            Label_TermDetailTermNameInputNewTerm.Size = New System.Drawing.Size(74, 16)
            Label_TermDetailTermNameInputNewTerm.TabIndex = 2
            Label_TermDetailTermNameInputNewTerm.Text = "Term Name"
            Label_TermDetailTermNameInputNewTerm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'Textbox_TermDetailTermNameInputNewTerm
            '
            Textbox_TermDetailTermNameInputNewTerm.Anchor = System.Windows.Forms.AnchorStyles.Left
            Textbox_TermDetailTermNameInputNewTerm.AutoCompleteCustomSource.AddRange(New String() {"(001) Item 1", "(002) Item 2", "(003) Item 3", "(004) Item 4", "(005) Item 5", "(006) Item 6", "(007) Item 7", "(008) Item 8", "(009) Item 9", "(010) Item 10"})
            Textbox_TermDetailTermNameInputNewTerm.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
            Textbox_TermDetailTermNameInputNewTerm.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
            Textbox_TermDetailTermNameInputNewTerm.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
            Textbox_TermDetailTermNameInputNewTerm.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
            Textbox_TermDetailTermNameInputNewTerm.Cursor = System.Windows.Forms.Cursors.IBeam
            Textbox_TermDetailTermNameInputNewTerm.DefaultText = ""
            Textbox_TermDetailTermNameInputNewTerm.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer))
            Textbox_TermDetailTermNameInputNewTerm.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer))
            Textbox_TermDetailTermNameInputNewTerm.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
            Textbox_TermDetailTermNameInputNewTerm.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
            Textbox_TermDetailTermNameInputNewTerm.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
            Textbox_TermDetailTermNameInputNewTerm.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
            Textbox_TermDetailTermNameInputNewTerm.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
            Textbox_TermDetailTermNameInputNewTerm.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
            Textbox_TermDetailTermNameInputNewTerm.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
            Textbox_TermDetailTermNameInputNewTerm.Location = New System.Drawing.Point(0, 16)
            Textbox_TermDetailTermNameInputNewTerm.Margin = New System.Windows.Forms.Padding(0)
            Textbox_TermDetailTermNameInputNewTerm.Name = "Textbox_TermDetailTermNameInputNewTerm"
            Textbox_TermDetailTermNameInputNewTerm.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
            Textbox_TermDetailTermNameInputNewTerm.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer))
            Textbox_TermDetailTermNameInputNewTerm.PlaceholderText = "Required"
            Textbox_TermDetailTermNameInputNewTerm.SelectedText = ""
            Textbox_TermDetailTermNameInputNewTerm.Size = New System.Drawing.Size(342, 40)
            Textbox_TermDetailTermNameInputNewTerm.TabIndex = 39
            '
            'idk1
            '
            idk1.AutoSize = true
            idk1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            idk1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
            idk1.Location = New System.Drawing.Point(360, 132)
            idk1.Margin = New System.Windows.Forms.Padding(0, 0, 0, 42)
            idk1.Name = "idk1"
            idk1.Size = New System.Drawing.Size(0, 0)
            idk1.TabIndex = 35
            '
            'Button_MakeTermNewSale
            '
            Button_MakeTermNewSale.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(44,Byte),Integer), CType(CType(62,Byte),Integer))
            Button_MakeTermNewSale.DisabledState.BorderColor = System.Drawing.Color.DarkGray
            Button_MakeTermNewSale.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
            Button_MakeTermNewSale.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer))
            Button_MakeTermNewSale.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer))
            Button_MakeTermNewSale.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
            Button_MakeTermNewSale.Font = New System.Drawing.Font("Segoe UI", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel)
            Button_MakeTermNewSale.ForeColor = System.Drawing.Color.White
            Button_MakeTermNewSale.Location = New System.Drawing.Point(12, 280)
            Button_MakeTermNewSale.Margin = New System.Windows.Forms.Padding(12, 24, 12, 12)
            Button_MakeTermNewSale.Name = "Button_MakeTermNewSale"
            Button_MakeTermNewSale.Size = New System.Drawing.Size(342, 40)
            Button_MakeTermNewSale.TabIndex = 33
            Button_MakeTermNewSale.Text = "Create Term"
            Button_MakeTermNewSale.TextOffset = New System.Drawing.Point(0, -2)
            '
            'Panel_PageHeaderIconNewTerm
            '
            Panel_PageHeaderIconNewTerm.Anchor = System.Windows.Forms.AnchorStyles.Left
            Panel_PageHeaderIconNewTerm.BackgroundImage = Global.ERP_SIMS.My.Resources.Resources.HeaderIcon_Brands
            Panel_PageHeaderIconNewTerm.Location = New System.Drawing.Point(0, 0)
            Panel_PageHeaderIconNewTerm.Margin = New System.Windows.Forms.Padding(0, 0, 16, 0)
            Panel_PageHeaderIconNewTerm.Name = "Panel_PageHeaderIconNewTerm"
            Panel_PageHeaderIconNewTerm.Size = New System.Drawing.Size(32, 32)
            Panel_PageHeaderIconNewTerm.TabIndex = 3
            '
            'idk2
            '
            idk2.AutoSize = true
            idk2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            idk2.Controls.Add(Label1)
            idk2.Controls.Add(Textbox_TermDetailsTermDaysInputNewTerm)
            idk2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
            idk2.Location = New System.Drawing.Point(12, 200)
            idk2.Margin = New System.Windows.Forms.Padding(12, 0, 6, 0)
            idk2.Name = "idk2"
            idk2.Size = New System.Drawing.Size(342, 56)
            idk2.TabIndex = 36
            '
            'Label1
            '
            Label1.AutoSize = true
            Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
            Label1.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
            Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
            Label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Label1.Location = New System.Drawing.Point(0, 0)
            Label1.Margin = New System.Windows.Forms.Padding(0)
            Label1.Name = "Label1"
            Label1.Size = New System.Drawing.Size(66, 16)
            Label1.TabIndex = 2
            Label1.Text = "Term Days"
            Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'Textbox_TermDetailsTermDaysInputNewTerm
            '
            Textbox_TermDetailsTermDaysInputNewTerm.Anchor = System.Windows.Forms.AnchorStyles.Left
            Textbox_TermDetailsTermDaysInputNewTerm.AutoCompleteCustomSource.AddRange(New String() {"(001) Item 1", "(002) Item 2", "(003) Item 3", "(004) Item 4", "(005) Item 5", "(006) Item 6", "(007) Item 7", "(008) Item 8", "(009) Item 9", "(010) Item 10"})
            Textbox_TermDetailsTermDaysInputNewTerm.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
            Textbox_TermDetailsTermDaysInputNewTerm.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
            Textbox_TermDetailsTermDaysInputNewTerm.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
            Textbox_TermDetailsTermDaysInputNewTerm.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
            Textbox_TermDetailsTermDaysInputNewTerm.Cursor = System.Windows.Forms.Cursors.IBeam
            Textbox_TermDetailsTermDaysInputNewTerm.DefaultText = ""
            Textbox_TermDetailsTermDaysInputNewTerm.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer))
            Textbox_TermDetailsTermDaysInputNewTerm.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer))
            Textbox_TermDetailsTermDaysInputNewTerm.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
            Textbox_TermDetailsTermDaysInputNewTerm.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
            Textbox_TermDetailsTermDaysInputNewTerm.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
            Textbox_TermDetailsTermDaysInputNewTerm.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
            Textbox_TermDetailsTermDaysInputNewTerm.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
            Textbox_TermDetailsTermDaysInputNewTerm.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
            Textbox_TermDetailsTermDaysInputNewTerm.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
            Textbox_TermDetailsTermDaysInputNewTerm.Location = New System.Drawing.Point(0, 16)
            Textbox_TermDetailsTermDaysInputNewTerm.Margin = New System.Windows.Forms.Padding(0)
            Textbox_TermDetailsTermDaysInputNewTerm.Name = "Textbox_TermDetailsTermDaysInputNewTerm"
            Textbox_TermDetailsTermDaysInputNewTerm.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
            Textbox_TermDetailsTermDaysInputNewTerm.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer))
            Textbox_TermDetailsTermDaysInputNewTerm.PlaceholderText = "Required"
            Textbox_TermDetailsTermDaysInputNewTerm.SelectedText = ""
            Textbox_TermDetailsTermDaysInputNewTerm.Size = New System.Drawing.Size(342, 40)
            Textbox_TermDetailsTermDaysInputNewTerm.TabIndex = 39
            '
            '_Temporary_PaymentTerm
            '
            Prompt_NewPaymentTerm.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
            Prompt_NewPaymentTerm.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Prompt_NewPaymentTerm.ClientSize = New System.Drawing.Size(366, 350)
            Prompt_NewPaymentTerm.Controls.Add(FlowLayoutPanel_TermDetailsNewTerm)
            Prompt_NewPaymentTerm.Text = "New Payment Term"
            Prompt_NewPaymentTerm.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            FlowLayoutPanel_TermDetailsNewTerm.ResumeLayout(false)
            FlowLayoutPanel_TermDetailsNewTerm.PerformLayout
            FlowLayoutPanel_PageHeaderNewTerm.ResumeLayout(false)
            FlowLayoutPanel_PageHeaderNewTerm.PerformLayout
            FlowLayoutPanel_TermDetailTermNameInputNewTerm.ResumeLayout(false)
            FlowLayoutPanel_TermDetailTermNameInputNewTerm.PerformLayout
            idk2.ResumeLayout(false)
            idk2.PerformLayout
            ResumeLayout(false)

            PaymentTerms = CreatePaymentTermArray()
            PopulatePaymentTermComboBox(Combobox_CustomerDetailPaymentTermInputNewCustomer)

            AddHandler Button_MakeTermNewSale.Click,
            Sub(sender1 As Object, e1 As EventArgs)
                If String.IsNullOrWhiteSpace(Textbox_TermDetailTermNameInputNewTerm.Text) OrElse String.IsNullOrWhiteSpace(Textbox_TermDetailsTermDaysInputNewTerm.Text) Then
                    MessageBox.Show("All fields are required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If

                Dim termDays As Integer
                If Not Integer.TryParse(Textbox_TermDetailsTermDaysInputNewTerm.Text, termDays) OrElse termDays <= 0 Then
                    MessageBox.Show("Term Days must be a positive number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If

                Dim query As String = "INSERT INTO payment_terms (term_name, term_days, created_by) VALUES (@term_name, @term_days, @created_by)"
                Using connection As New MySqlConnection(ConnectionString)
                    Using command As New MySqlCommand(query, connection)
                        command.Parameters.AddWithValue("@term_name", Textbox_TermDetailTermNameInputNewTerm.Text.Trim())
                        command.Parameters.AddWithValue("@term_days", termDays)
                        command.Parameters.AddWithValue("@created_by", GlobalShared.CurrentUser.AccountId)
                        Try
                            connection.Open()
                            command.ExecuteNonQuery()
                            MessageBox.Show("Payment Term successfully added!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            PaymentTerms = CreatePaymentTermArray()
                            PopulatePaymentTermComboBox(Combobox_CustomerDetailPaymentTermInputNewCustomer)
                            Prompt_NewPaymentTerm.Close()
                        Catch ex As Exception
                            MessageBox.Show($"Error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Try
                    End Using
                End Using
            End Sub

            Prompt_NewPaymentTerm.ShowDialog(Me)
        End Sub

        AddHandler Button_ResetNewCustomer.Click,
        Sub(sender1 As Object, e1 As EventArgs)
            Textbox_CustomerDetailCustomerNameInputNewCustomer.Text = ""
            Textbox_CustomerDetailContactPersonNameInputNewCustomer.Text = ""
            Combobox_CustomerDetailPaymentTermInputNewCustomer.SelectedIndex = -1
            Textbox_CustomerDetailEmailInputNewCustomer.Text = ""
            Textbox_CustomerDetailPhoneInputNewCustomer.Text = ""
            Textbox_CustomerDetailAddressLineInputNewCustomer.Text = ""
            Textbox_CustomerDetailCityInputNewCustomer.Text = ""
            Textbox_CustomerDetailZipCodeInputNewCustomer.Text = ""
            Textbox_CustomerDetailCountryInputNewCustomer.Text = ""
        End Sub

        AddHandler Button_MakeCustomerNewCustomer.Click,
        Sub(sender1 As Object, e1 As EventArgs)
            Dim customerName As String = Textbox_CustomerDetailCustomerNameInputNewCustomer.Text.Trim()
            Dim company As String = Textbox_CustomerDetailContactPersonNameInputNewCustomer.Text.Trim()
            Dim paymentTermName As String = Combobox_CustomerDetailPaymentTermInputNewCustomer.Text.Trim()
            Dim email As String = Textbox_CustomerDetailEmailInputNewCustomer.Text.Trim()
            Dim phone As String = Textbox_CustomerDetailPhoneInputNewCustomer.Text.Trim()
            Dim addressLine As String = Textbox_CustomerDetailAddressLineInputNewCustomer.Text.Trim()
            Dim city As String = Textbox_CustomerDetailCityInputNewCustomer.Text.Trim()
            Dim zipCode As String = Textbox_CustomerDetailZipCodeInputNewCustomer.Text.Trim()
            Dim country As String = Textbox_CustomerDetailCountryInputNewCustomer.Text.Trim()

            Dim paymentTerm As PaymentTerm = PaymentTerms.FirstOrDefault(Function(pt) pt.TermName = paymentTermName)
            If String.IsNullOrEmpty(customerName) OrElse String.IsNullOrEmpty(paymentTermName) OrElse paymentTerm Is Nothing OrElse String.IsNullOrEmpty(email) OrElse String.IsNullOrEmpty(phone) OrElse String.IsNullOrEmpty(addressLine) OrElse String.IsNullOrEmpty(city) OrElse String.IsNullOrEmpty(zipCode) OrElse String.IsNullOrEmpty(country) Then
                MessageBox.Show("Please enter all the required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            Try
                Using connection As New MySqlConnection(ConnectionString)
                    connection.Open()
                    Dim query As String = "INSERT INTO customers (name, company, payment_term_id, email, phone, address_line, city, zip_code, country, created_by) VALUES (@name, @company, @payment_term_id, @email, @phone, @address_line, @city, @zip_code, @country, @created_by)"
                    Using command As New MySqlCommand(query, connection)
                        command.Parameters.AddWithValue("@name", customerName)
                        command.Parameters.AddWithValue("@company", If(String.IsNullOrEmpty(company), DBNull.Value, company))
                        command.Parameters.AddWithValue("@payment_term_id", paymentTerm.PaymentTermId)
                        command.Parameters.AddWithValue("@email", email)
                        command.Parameters.AddWithValue("@phone", phone)
                        command.Parameters.AddWithValue("@address_line", addressLine)
                        command.Parameters.AddWithValue("@city", city)
                        command.Parameters.AddWithValue("@zip_code", zipCode)
                        command.Parameters.AddWithValue("@country", country)
                        command.Parameters.AddWithValue("@created_by", GlobalShared.CurrentUser.AccountId)
                        command.ExecuteNonQuery()
                    End Using
                End Using
                MessageBox.Show("Customer added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Customers = CreateCustomerArray()
                UpdateCustomersListTable()
            Catch ex As Exception
                MessageBox.Show($"An error occurred: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Prompt_NewCustomer.ShowDialog(Me)
    End Sub
    
    Public Sub PopulatePaymentTermComboBox(comboBox As Guna.UI2.WinForms.Guna2ComboBox)
        comboBox.Items.Clear()

        For Each paymentTerm As PaymentTerm In PaymentTerms
            comboBox.Items.Add(paymentTerm.TermName)
        Next
    End Sub

    Public Function CreatePaymentTermArray() As PaymentTerm()
        Dim paymentTerms As New List(Of PaymentTerm)()

        Using connection As New MySqlConnection(ConnectionString)
            connection.Open()
            Dim query As String = "SELECT payment_term_id, term_name, term_days, created_by, created_at FROM payment_terms"
            Using command As New MySqlCommand(query, connection)
                Using reader As MySqlDataReader = command.ExecuteReader()
                    While reader.Read()
                        Dim paymentTerm As New PaymentTerm(
                            reader.GetInt32("payment_term_id"),
                            reader.GetString("term_name"),
                            reader.GetInt32("term_days"),
                            If(reader.IsDBNull(reader.GetOrdinal("created_by")), Nothing, reader.GetInt32("created_by")),
                            reader.GetDateTime("created_at")
                        )
                        paymentTerms.Add(paymentTerm)
                    End While
                End Using
            End Using
        End Using

        Return paymentTerms.ToArray()
    End Function

    Private Sub Button_NewCustomer_Click(sender As Object, e As EventArgs) Handles Button_NewCustomer.Click
        ShowNewCustomerPrompt()
    End Sub

    Private Sub Window_Customers_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If GlobalShared.CurrentUser.Type = "sales_representative" Then
            ' Hide navigation buttons for sales representative
            Button_NavigationItemSales.Visible = False
            Button_NavigationItemCustomers.Visible = False
            Button_NavigationItemStocks.Visible = False
            Button_NavigationItemBrandsAndCategories.Visible = False
            Button_NavigationItemVendors.Visible = False
        Else
            ' Show navigation buttons for admin or other types
            Button_NavigationItemSales.Visible = True
            Button_NavigationItemCustomers.Visible = True
            Button_NavigationItemStocks.Visible = True
            Button_NavigationItemBrandsAndCategories.Visible = True
            Button_NavigationItemVendors.Visible = True
        End If
        
        UpdateCustomersListTable()
        Customers = CreateCustomerArray()

        Label_UserName.Text = GlobalShared.CurrentUser.Name
        Label_UserEmail.Text = GlobalShared.CurrentUser.Email
    End Sub

    Private Sub Label_CustomersListTablePageControlItemNumber_Click(sender As Object, e As EventArgs) 
    End Sub

    Private Sub Button_Logout_Click(sender As Object, e As EventArgs) Handles Button_Logout.Click
        Dim result As DialogResult = MessageBox.Show("Are you sure you want to log out?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            Dim newForm As New Window_Login()
            newForm.Show()
            Me.Close()
        End If
    End Sub

    Private Sub Button_CustomersListTablePageControlPrevious_Click_1(sender As Object, e As EventArgs) Handles Button_CustomersListTablePageControlPrevious.Click
        If CustomersListCurrentPage > 1 Then
            CustomersListCurrentPage -= 1
            UpdateCustomersListTable()
        End If
    End Sub

    Private Sub Button_CustomersListTablePageControlNext_Click_1(sender As Object, e As EventArgs) Handles Button_CustomersListTablePageControlNext.Click
        If CustomersListCurrentPage < CustomersListTotalPages Then
            CustomersListCurrentPage += 1
            UpdateCustomersListTable()
        End If
    End Sub

    Private Sub Button_SearchCustomers_Click(sender As Object, e As EventArgs) Handles Button_SearchCustomers.Click
        Dim searchText As String = Textbox_SearchBarCustomers.Text.Trim().ToLower()

    ' Filter the customers based on the search text
    Dim filteredCustomers = Customers.Where(Function(c) c.Name.ToLower().Contains(searchText) OrElse
        Accounts.FirstOrDefault(Function(a) a.AccountId = c.CreatedBy)?.Name.ToLower().Contains(searchText)
    ).ToList()

    FlowLayoutPanel_CustomersListTable.Controls.Clear()

    CustomersListTotalPages = Math.Ceiling(filteredCustomers.Count / CustomersListItemsPerPage)
    Label_CustomersListTablePageControlPageNumber.Text = $"{CustomersListCurrentPage}/{CustomersListTotalPages}"

    Dim TableLayoutPanel_CustomersList As New TableLayoutPanel With {
        .AutoSize = True,
        .AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink,
        .BackColor = System.Drawing.Color.FromArgb(CType(255, Byte), CType(255, Byte), CType(255, Byte)),
        .CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single],
        .Margin = New System.Windows.Forms.Padding(0, 0, 0, 0),
        .MaximumSize = New System.Drawing.Size(1102, 0),
        .MinimumSize = New System.Drawing.Size(1102, 0)
    }

    TableLayoutPanel_CustomersList.ColumnCount = 6
    For i = 1 To 6
        TableLayoutPanel_CustomersList.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11!))
    Next

    Dim StartIndex As Integer = (CustomersListCurrentPage - 1) * CustomersListItemsPerPage
    Dim EndIndex As Integer = Math.Min(StartIndex + CustomersListItemsPerPage, filteredCustomers.Count)

    For i = StartIndex To EndIndex - 1
        Dim customer As Customer = filteredCustomers(i)
        TableLayoutPanel_CustomersList.RowCount += 1

        Dim TransactionAccount As Account = Accounts.FirstOrDefault(Function(a) a.AccountId = customer.CreatedBy)

        ' Create the labels for the customer row
        Dim labels As New List(Of Label)
        labels.Add(CreateLabel(customer.CustomerId.ToString() & " - " & customer.Name, 2, True))
        labels.Add(CreateLabel(customer.Company, 1))
        labels.Add(CreateLabel(customer.Email, 1))
        labels.Add(CreateLabel(customer.Phone, 1))
        labels.Add(CreateLabel(customer.AddressLine & ", " & customer.City & ", " & customer.ZipCode & ", " & customer.Country, 1))
        labels.Add(CreateLabel(If(TransactionAccount IsNot Nothing, TransactionAccount.Name, "Unknown"), 1))

        For col As Integer = 0 To labels.Count - 1
            TableLayoutPanel_CustomersList.Controls.Add(labels(col), col, TableLayoutPanel_CustomersList.RowCount - 1)
        Next

        Dim maxLabelHeight As Integer = labels.Max(Function(lbl) lbl.Height)

        TableLayoutPanel_CustomersList.RowStyles.Add(New RowStyle(SizeType.Absolute, maxLabelHeight + 12))
    Next

    FlowLayoutPanel_CustomersListTable.Controls.Add(TableLayoutPanel_CustomersList)

    End Sub
End Class

