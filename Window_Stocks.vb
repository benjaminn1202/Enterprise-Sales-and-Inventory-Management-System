Imports Guna.UI2.WinForms
Imports MySql.Data.MySqlClient
Imports System.Text
Imports System.Text.RegularExpressions

Public Class Window_Stocks
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

    Private Sub Button_NavigationItemCustomers_Click(sender As Object, e As EventArgs) Handles Button_NavigationItemCustomers.Click
        NavigateToForm(New Window_Customers(), Me)
    End Sub

    Private Sub Button_NavigationItemReceiving_Click(sender As Object, e As EventArgs) Handles Button_NavigationItemReceiving.Click
        NavigateToForm(New Window_AddStock(), Me)
    End Sub

    Private Sub Button_NavigationItemStocks_Click(sender As Object, e As EventArgs) Handles Button_NavigationItemStocks.Click
        NavigateToForm(New Window_Stocks(), Me)
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

    Public Stocks As Stock() = CreateStockArray()
    Public Brands As Brand() = CreateBrandArray()
    Public Categories As Category() = CreateCategoryArray()
    Public StockEntries As StockEntry() = CreateStockEntryArray()
    Public Vendors As Vendor() = CreateVendorArray()
    
    
    Public Items As Item() = CreateItemArray()
    Public Accounts As Account() = CreateAccountArray()

    Public StocksListCurrentPage As Integer = 1
    Public StocksListItemsPerPage As Integer = 10
    Public StocksListTotalPages As Integer



    ' Get data from database
    Public Employees As Employee() = CreateEmployeeArray()

    Public Function CreateEmployeeArray() As Employee()
        Dim query As String = "SELECT * FROM employee_tbl"
        Dim employees As New List(Of Employee)

        Using connection As New MySqlConnection("server=localhost;user id=root;password=benjaminn1202;database=hrm_db")
            Try
                connection.Open()

                Using command As New MySqlCommand(query, connection)
                    Using reader As MySqlDataReader = command.ExecuteReader()
                        While reader.Read()
                            Dim employee As New Employee(
                                CInt(reader("employee_id")),
                                reader("last_name").ToString(),
                                reader("first_name").ToString(),
                                If(reader.IsDBNull(reader.GetOrdinal("middle_name")), Nothing, reader("middle_name").ToString()),
                                If(reader.IsDBNull(reader.GetOrdinal("suffix")), Nothing, reader("suffix").ToString()),
                                reader("gender").ToString(),
                                CDate(reader("date_of_birth")),
                                reader("phone_number").ToString(),
                                reader("email").ToString(),
                                If(reader.IsDBNull(reader.GetOrdinal("password")), Nothing, reader("password").ToString()),
                                If(reader.IsDBNull(reader.GetOrdinal("employee_address")), Nothing, reader("employee_address").ToString()),
                                If(reader.IsDBNull(reader.GetOrdinal("department_id")), Nothing, CInt(reader("department_id"))),
                                If(reader.IsDBNull(reader.GetOrdinal("position_id")), Nothing, CInt(reader("position_id"))),
                                CDate(reader("hire_date")),
                                If(reader.IsDBNull(reader.GetOrdinal("exp_lvl")), Nothing, reader("exp_lvl").ToString()),
                                CDec(reader("salary_advance")),
                                CDec(reader("deductions")),
                                CDec(reader("commissions")),
                                reader("status").ToString(),
                                CDate(reader("created_at")),
                                CDate(reader("updated_at"))
                            )

                            ' Add the employee to the list
                            employees.Add(employee)
                        End While
                    End Using
                End Using
            Catch ex As Exception
                ' Handle errors (e.g., connection issues)
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using

        ' Return the list as an array
        Return employees.ToArray()
    End Function


    Function CreateStockArray() As Stock()
        Dim stocks As New List(Of Stock)()

        Try
            Using connection As New MySqlConnection(ConnectionString)
                connection.Open()

                Dim query As String = "SELECT stock_id, stock_entry_id, stock_quantity, item_buying_price, total_buying_price, item_id, expiry, status FROM stocks"
                Using command As New MySqlCommand(query, connection)
                    Using reader As MySqlDataReader = command.ExecuteReader()
                        While reader.Read()
                            Dim stockId As Integer = reader.GetInt32("stock_id")
                            Dim stockEntryId As Integer = reader.GetInt32("stock_entry_id")
                            Dim stockQuantity As Integer = reader.GetInt32("stock_quantity")
                            Dim itemBuyingPrice As Decimal = reader.GetDecimal("item_buying_price")
                            Dim totalBuyingPrice As Decimal = reader.GetDecimal("total_buying_price")
                            Dim itemId As Integer = reader.GetInt32("item_id")
                            Dim expiry As DateTime? = If(Not reader.IsDBNull(reader.GetOrdinal("expiry")), reader.GetDateTime("expiry"), CType(Nothing, DateTime?))
                            Dim status As String = reader.GetString("status")

                            Dim stock As New Stock(stockId, stockEntryId, stockQuantity, itemBuyingPrice, totalBuyingPrice, itemId, expiry, status)
                            stocks.Add(stock)
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Console.WriteLine("Error: " & ex.Message)
        End Try

        Return stocks.ToArray()
    End Function

    Public Function CreateStockEntryArray() As StockEntry()
        Dim stockEntries As New List(Of StockEntry)()
        Dim query As String = "SELECT * FROM stock_entries"

        Using connection As New MySqlConnection(ConnectionString)
            Using command As New MySqlCommand(query, connection)
                Try
                    connection.Open()
                    Using reader As MySqlDataReader = command.ExecuteReader()
                        While reader.Read()
                            Dim stockEntry As New StockEntry(
                                reader.GetInt32("stock_entry_id"),
                                If(IsDBNull(reader("vendor_id")), 0, reader.GetInt32("vendor_id")),
                                reader.GetString("status"),
                                If(IsDBNull(reader("net_total")), 0.0, reader.GetDouble("net_total")),
                                reader.GetDateTime("created_at"),
                                If(IsDBNull(reader("created_by")), 0, reader.GetInt32("created_by"))
                            )
                            stockEntries.Add(stockEntry)
                        End While
                    End Using
                Catch ex As Exception
                    MessageBox.Show("An error occurred while fetching stock entries: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End Using
        End Using

        Return stockEntries.ToArray()
    End Function


    Function CreateBrandArray() As Brand()
        Dim brands As New List(Of Brand)()

        Try
            Using connection As New MySqlConnection(ConnectionString)
                connection.Open()

                Dim query As String = "SELECT brand_id, brand_name, created_at, created_by FROM brands"

                Using command As New MySqlCommand(query, connection)
                    Using reader As MySqlDataReader = command.ExecuteReader()
                        While reader.Read()
                            Dim brandId As Integer = reader.GetInt32("brand_id")
                            Dim brandName As String = reader.GetString("brand_name")
                            Dim createdAt As DateTime = reader.GetDateTime("created_at")
                            Dim createdBy As Integer = reader.GetInt32("created_by")

                            Dim brand As New Brand(brandId, brandName, createdAt, createdBy)
                            brands.Add(brand)
                        End While
                    End Using
                End Using
            End Using

        Catch ex As Exception
            Console.WriteLine("An error occurred: " & ex.Message)
        End Try

        Return brands.ToArray()
    End Function

    Function CreateCategoryArray() As Category()
        Dim categories As New List(Of Category)()

        Try
            Using connection As New MySqlConnection(ConnectionString)
                connection.Open()

                Dim query As String = "SELECT category_id, category_name, created_at, created_by FROM categories"

                Using command As New MySqlCommand(query, connection)
                    Using reader As MySqlDataReader = command.ExecuteReader()
                        While reader.Read()
                            Dim categoryId As Integer = reader.GetInt32("category_id")
                            Dim categoryName As String = reader.GetString("category_name")
                            Dim createdAt As DateTime = reader.GetDateTime("created_at")
                            Dim createdBy As Integer = reader.GetInt32("created_by")

                            Dim category As New Category(categoryId, categoryName, createdAt, createdBy)
                            categories.Add(category)
                        End While
                    End Using
                End Using
            End Using

        Catch ex As Exception
            Console.WriteLine("An error occurred: " & ex.Message)
        End Try

        Return categories.ToArray()
    End Function

    Function CreateVendorArray() As Vendor()
        Dim vendors As New List(Of Vendor)()

        Try
            Using connection As New MySqlConnection(ConnectionString)
                connection.Open()

                Dim query As String = "SELECT * FROM vendors"
                Using command As New MySqlCommand(query, connection)
                    Using reader As MySqlDataReader = command.ExecuteReader()
                        While reader.Read()
                            Dim vendorId As Integer = reader.GetInt32("vendor_id")
                            Dim vendorName As String = reader.GetString("vendor_name")
                            Dim contactPerson As String = reader.GetString("contact_person")
                            Dim email As String = reader.GetString("email")
                            Dim phone As String = reader.GetString("phone")
                            Dim addressLine As String = reader.GetString("address_line")
                            Dim city As String = reader.GetString("city")
                            Dim zipCode As String = reader.GetString("zip_code")
                            Dim country As String = reader.GetString("country")
                            Dim createdBy As Integer = reader.GetInt32("created_by")
                            Dim createdAt As DateTime = reader.GetDateTime("created_at")

                            Dim vendor As New Vendor(vendorId, vendorName, contactPerson, email, phone, addressLine, city, zipCode, country, createdBy, createdAt)
                            vendors.Add(vendor)
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Console.WriteLine("Error: " & ex.Message)
        End Try

        Return vendors.ToArray()
    End Function

    Function CreateItemArray() As Item()
        Dim items As New List(Of Item)()

        Try
            Using connection As New MySqlConnection(ConnectionString)
                connection.Open()

                Dim query As String = "SELECT item_id, item_name, available_quantity, unavailable_quantity, selling_price, brand_id, category_id FROM items"
                Using command As New MySqlCommand(query, connection)
                    Using reader As MySqlDataReader = command.ExecuteReader()
                        While reader.Read()
                            Dim itemId As Integer = reader.GetInt32("item_id")
                            Dim itemName As String = reader.GetString("item_name")
                            Dim availableQuantity As Integer = reader.GetInt32("available_quantity")
                            Dim unavailableQuantity As Integer = reader.GetInt32("unavailable_quantity")
                            Dim sellingPrice As Decimal = reader.GetDecimal("selling_price")
                            Dim brandId As Integer = reader.GetInt32("brand_id")
                            Dim categoryId As Integer = reader.GetInt32("category_id")

                            Dim item As New Item(itemId, itemName, availableQuantity, unavailableQuantity, sellingPrice, brandId, categoryId)
                            items.Add(item)
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Console.WriteLine("Error: " & ex.Message)
        End Try

        Return items.ToArray()
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
    
    ' Draw forms
    Private Sub ShowNewBrandPrompt()
        Dim Prompt_NewBrand As New Form()
        Dim FlowLayoutPanel_BrandDetailsNewBrand = New System.Windows.Forms.FlowLayoutPanel()
        Dim FlowLayoutPanel_PageHeaderNewBrand = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label_PageHeaderNewBrand = New System.Windows.Forms.Label()
        Dim Label_BrandDetailsNewBrand = New System.Windows.Forms.Label()
        Dim FlowLayoutPanel_BrandDetailBrandNameInputNewBrand = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label_BrandDetailBrandNameInputNewBrand = New System.Windows.Forms.Label()
        Dim Textbox_BrandDetailBrandNameInputNewBrand = New Guna.UI2.WinForms.Guna2TextBox()
        Dim FlowLayoutPanel_DateRangeInputsSales = New System.Windows.Forms.FlowLayoutPanel()
        Dim Button_MakeBrandNewSale = New Guna.UI2.WinForms.Guna2Button()
        Dim Panel_PageHeaderIconNewBrand = New System.Windows.Forms.Panel()
        FlowLayoutPanel_BrandDetailsNewBrand.SuspendLayout
        FlowLayoutPanel_PageHeaderNewBrand.SuspendLayout
        FlowLayoutPanel_BrandDetailBrandNameInputNewBrand.SuspendLayout
        SuspendLayout
        '
        'FlowLayoutPanel_BrandDetailsNewBrand
        '
        FlowLayoutPanel_BrandDetailsNewBrand.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        FlowLayoutPanel_BrandDetailsNewBrand.Controls.Add(FlowLayoutPanel_PageHeaderNewBrand)
        FlowLayoutPanel_BrandDetailsNewBrand.Controls.Add(Label_BrandDetailsNewBrand)
        FlowLayoutPanel_BrandDetailsNewBrand.Controls.Add(FlowLayoutPanel_BrandDetailBrandNameInputNewBrand)
        FlowLayoutPanel_BrandDetailsNewBrand.Controls.Add(FlowLayoutPanel_DateRangeInputsSales)
        FlowLayoutPanel_BrandDetailsNewBrand.Controls.Add(Button_MakeBrandNewSale)
        FlowLayoutPanel_BrandDetailsNewBrand.Location = New System.Drawing.Point(0, 0)
        FlowLayoutPanel_BrandDetailsNewBrand.Margin = New System.Windows.Forms.Padding(0)
        FlowLayoutPanel_BrandDetailsNewBrand.Name = "FlowLayoutPanel_BrandDetailsNewBrand"
        FlowLayoutPanel_BrandDetailsNewBrand.Size = New System.Drawing.Size(366, 350)
        FlowLayoutPanel_BrandDetailsNewBrand.TabIndex = 34
        '
        'FlowLayoutPanel_PageHeaderNewBrand
        '
        FlowLayoutPanel_PageHeaderNewBrand.AutoSize = true
        FlowLayoutPanel_PageHeaderNewBrand.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_PageHeaderNewBrand.Controls.Add(Panel_PageHeaderIconNewBrand)
        FlowLayoutPanel_PageHeaderNewBrand.Controls.Add(Label_PageHeaderNewBrand)
        FlowLayoutPanel_PageHeaderNewBrand.Location = New System.Drawing.Point(12, 24)
        FlowLayoutPanel_PageHeaderNewBrand.Margin = New System.Windows.Forms.Padding(12, 24, 0, 24)
        FlowLayoutPanel_PageHeaderNewBrand.Name = "FlowLayoutPanel_PageHeaderNewBrand"
        FlowLayoutPanel_PageHeaderNewBrand.Size = New System.Drawing.Size(189, 32)
        FlowLayoutPanel_PageHeaderNewBrand.TabIndex = 0
        '
        'Label_PageHeaderNewBrand
        '
        Label_PageHeaderNewBrand.Anchor = System.Windows.Forms.AnchorStyles.Left
        Label_PageHeaderNewBrand.AutoSize = true
        Label_PageHeaderNewBrand.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_PageHeaderNewBrand.Font = New System.Drawing.Font("Microsoft JhengHei", 24!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_PageHeaderNewBrand.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_PageHeaderNewBrand.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_PageHeaderNewBrand.Location = New System.Drawing.Point(48, 0)
        Label_PageHeaderNewBrand.Margin = New System.Windows.Forms.Padding(0)
        Label_PageHeaderNewBrand.Name = "Label_PageHeaderNewBrand"
        Label_PageHeaderNewBrand.Size = New System.Drawing.Size(141, 31)
        Label_PageHeaderNewBrand.TabIndex = 2
        Label_PageHeaderNewBrand.Text = "New Brand"
        Label_PageHeaderNewBrand.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label_BrandDetailsNewBrand
        '
        Label_BrandDetailsNewBrand.Anchor = System.Windows.Forms.AnchorStyles.Left
        Label_BrandDetailsNewBrand.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_BrandDetailsNewBrand.Font = New System.Drawing.Font("Microsoft JhengHei", 22!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_BrandDetailsNewBrand.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Label_BrandDetailsNewBrand.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_BrandDetailsNewBrand.Location = New System.Drawing.Point(12, 80)
        Label_BrandDetailsNewBrand.Margin = New System.Windows.Forms.Padding(12, 0, 12, 24)
        Label_BrandDetailsNewBrand.Name = "Label_BrandDetailsNewBrand"
        Label_BrandDetailsNewBrand.Size = New System.Drawing.Size(342, 28)
        Label_BrandDetailsNewBrand.TabIndex = 18
        Label_BrandDetailsNewBrand.Text = "Brand Details"
        Label_BrandDetailsNewBrand.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'FlowLayoutPanel_BrandDetailBrandNameInputNewBrand
        '
        FlowLayoutPanel_BrandDetailBrandNameInputNewBrand.AutoSize = true
        FlowLayoutPanel_BrandDetailBrandNameInputNewBrand.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_BrandDetailBrandNameInputNewBrand.Controls.Add(Label_BrandDetailBrandNameInputNewBrand)
        FlowLayoutPanel_BrandDetailBrandNameInputNewBrand.Controls.Add(Textbox_BrandDetailBrandNameInputNewBrand)
        FlowLayoutPanel_BrandDetailBrandNameInputNewBrand.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_BrandDetailBrandNameInputNewBrand.Location = New System.Drawing.Point(12, 132)
        FlowLayoutPanel_BrandDetailBrandNameInputNewBrand.Margin = New System.Windows.Forms.Padding(12, 0, 6, 0)
        FlowLayoutPanel_BrandDetailBrandNameInputNewBrand.Name = "FlowLayoutPanel_BrandDetailBrandNameInputNewBrand"
        FlowLayoutPanel_BrandDetailBrandNameInputNewBrand.Size = New System.Drawing.Size(342, 56)
        FlowLayoutPanel_BrandDetailBrandNameInputNewBrand.TabIndex = 19
        '
        'Label_BrandDetailBrandNameInputNewBrand
        '
        Label_BrandDetailBrandNameInputNewBrand.AutoSize = true
        Label_BrandDetailBrandNameInputNewBrand.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_BrandDetailBrandNameInputNewBrand.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_BrandDetailBrandNameInputNewBrand.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_BrandDetailBrandNameInputNewBrand.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_BrandDetailBrandNameInputNewBrand.Location = New System.Drawing.Point(0, 0)
        Label_BrandDetailBrandNameInputNewBrand.Margin = New System.Windows.Forms.Padding(0)
        Label_BrandDetailBrandNameInputNewBrand.Name = "Label_BrandDetailBrandNameInputNewBrand"
        Label_BrandDetailBrandNameInputNewBrand.Size = New System.Drawing.Size(78, 16)
        Label_BrandDetailBrandNameInputNewBrand.TabIndex = 2
        Label_BrandDetailBrandNameInputNewBrand.Text = "Brand Name"
        Label_BrandDetailBrandNameInputNewBrand.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Textbox_BrandDetailBrandNameInputNewBrand
        '
        Textbox_BrandDetailBrandNameInputNewBrand.Anchor = System.Windows.Forms.AnchorStyles.Left
        Textbox_BrandDetailBrandNameInputNewBrand.AutoCompleteCustomSource.AddRange(New String() {"(001) Item 1", "(002) Item 2", "(003) Item 3", "(004) Item 4", "(005) Item 5", "(006) Item 6", "(007) Item 7", "(008) Item 8", "(009) Item 9", "(010) Item 10"})
        Textbox_BrandDetailBrandNameInputNewBrand.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Textbox_BrandDetailBrandNameInputNewBrand.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Textbox_BrandDetailBrandNameInputNewBrand.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_BrandDetailBrandNameInputNewBrand.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_BrandDetailBrandNameInputNewBrand.Cursor = System.Windows.Forms.Cursors.IBeam
        Textbox_BrandDetailBrandNameInputNewBrand.DefaultText = ""
        Textbox_BrandDetailBrandNameInputNewBrand.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer))
        Textbox_BrandDetailBrandNameInputNewBrand.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer))
        Textbox_BrandDetailBrandNameInputNewBrand.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_BrandDetailBrandNameInputNewBrand.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_BrandDetailBrandNameInputNewBrand.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_BrandDetailBrandNameInputNewBrand.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_BrandDetailBrandNameInputNewBrand.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
        Textbox_BrandDetailBrandNameInputNewBrand.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_BrandDetailBrandNameInputNewBrand.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_BrandDetailBrandNameInputNewBrand.Location = New System.Drawing.Point(0, 16)
        Textbox_BrandDetailBrandNameInputNewBrand.Margin = New System.Windows.Forms.Padding(0)
        Textbox_BrandDetailBrandNameInputNewBrand.Name = "Textbox_BrandDetailBrandNameInputNewBrand"
        Textbox_BrandDetailBrandNameInputNewBrand.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Textbox_BrandDetailBrandNameInputNewBrand.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer))
        Textbox_BrandDetailBrandNameInputNewBrand.PlaceholderText = "Required"
        Textbox_BrandDetailBrandNameInputNewBrand.SelectedText = ""
        Textbox_BrandDetailBrandNameInputNewBrand.Size = New System.Drawing.Size(342, 40)
        Textbox_BrandDetailBrandNameInputNewBrand.TabIndex = 39
        '
        'FlowLayoutPanel_DateRangeInputsSales
        '
        FlowLayoutPanel_DateRangeInputsSales.AutoSize = true
        FlowLayoutPanel_DateRangeInputsSales.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_DateRangeInputsSales.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_DateRangeInputsSales.Location = New System.Drawing.Point(360, 132)
        FlowLayoutPanel_DateRangeInputsSales.Margin = New System.Windows.Forms.Padding(0, 0, 0, 42)
        FlowLayoutPanel_DateRangeInputsSales.Name = "FlowLayoutPanel_DateRangeInputsSales"
        FlowLayoutPanel_DateRangeInputsSales.Size = New System.Drawing.Size(0, 0)
        FlowLayoutPanel_DateRangeInputsSales.TabIndex = 35
        '
        'Button_MakeBrandNewSale
        '
        Button_MakeBrandNewSale.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(44,Byte),Integer), CType(CType(62,Byte),Integer))
        Button_MakeBrandNewSale.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Button_MakeBrandNewSale.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Button_MakeBrandNewSale.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer))
        Button_MakeBrandNewSale.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer))
        Button_MakeBrandNewSale.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Button_MakeBrandNewSale.Font = New System.Drawing.Font("Segoe UI", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel)
        Button_MakeBrandNewSale.ForeColor = System.Drawing.Color.White
        Button_MakeBrandNewSale.Location = New System.Drawing.Point(12, 212)
        Button_MakeBrandNewSale.Margin = New System.Windows.Forms.Padding(12, 24, 12, 12)
        Button_MakeBrandNewSale.Name = "Button_MakeBrandNewSale"
        Button_MakeBrandNewSale.Size = New System.Drawing.Size(342, 40)
        Button_MakeBrandNewSale.TabIndex = 33
        Button_MakeBrandNewSale.Text = "Create Brand"
        Button_MakeBrandNewSale.TextOffset = New System.Drawing.Point(0, -2)
        '
        'Panel_PageHeaderIconNewBrand
        '
        Panel_PageHeaderIconNewBrand.Anchor = System.Windows.Forms.AnchorStyles.Left
        Panel_PageHeaderIconNewBrand.BackgroundImage = Global.ERP_SIMS.My.Resources.Resources.HeaderIcon_Brands
        Panel_PageHeaderIconNewBrand.Location = New System.Drawing.Point(0, 0)
        Panel_PageHeaderIconNewBrand.Margin = New System.Windows.Forms.Padding(0, 0, 16, 0)
        Panel_PageHeaderIconNewBrand.Name = "Panel_PageHeaderIconNewBrand"
        Panel_PageHeaderIconNewBrand.Size = New System.Drawing.Size(32, 32)
        Panel_PageHeaderIconNewBrand.TabIndex = 3
        '
        '_Temporary_NewBrand
        '
        Prompt_NewBrand.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Prompt_NewBrand.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Prompt_NewBrand.ClientSize = New System.Drawing.Size(366, 350)
        Prompt_NewBrand.Controls.Add(FlowLayoutPanel_BrandDetailsNewBrand)
        Prompt_NewBrand.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        FlowLayoutPanel_BrandDetailsNewBrand.ResumeLayout(false)
        FlowLayoutPanel_BrandDetailsNewBrand.PerformLayout
        FlowLayoutPanel_PageHeaderNewBrand.ResumeLayout(false)
        FlowLayoutPanel_PageHeaderNewBrand.PerformLayout
        FlowLayoutPanel_BrandDetailBrandNameInputNewBrand.ResumeLayout(false)
        FlowLayoutPanel_BrandDetailBrandNameInputNewBrand.PerformLayout
        ResumeLayout(false)

        AddHandler Button_MakeBrandNewSale.Click, Sub(sender As Object, e As EventArgs)
            Dim brandName As String = Textbox_BrandDetailBrandNameInputNewBrand.Text.Trim()
            Dim isValid As Boolean = True

            If String.IsNullOrEmpty(brandName) Then
                Textbox_BrandDetailBrandNameInputNewBrand.BorderColor = Color.Red
                isValid = False
            Else
                Textbox_BrandDetailBrandNameInputNewBrand.BorderColor = Color.Black
            End If

            If Not isValid Then
                MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Try
                Using conn As New MySqlConnection(ConnectionString)
                    conn.Open()
                    Dim query As String = "INSERT INTO brands (brand_name, created_by) VALUES (@brand_name, @created_by)"
                    Using cmd As New MySqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@brand_name", brandName)
                        cmd.Parameters.AddWithValue("@created_by", GlobalShared.CurrentUser.AccountId)
                        cmd.ExecuteNonQuery()
                    End Using
                End Using
                MessageBox.Show("Brand successfully added!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Prompt_NewBrand.Close()
            Catch ex As Exception
                MessageBox.Show($"Error adding brand: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Prompt_NewBrand.ShowDialog(Me)
    End Sub
    
    Private Sub ShowNewCategoryPrompt()
        Dim Prompt_NewCategory As New Form()
        Dim FlowLayoutPanel_CategorDetailsNewCategor = New System.Windows.Forms.FlowLayoutPanel()
        Dim FlowLayoutPanel_PageHeaderNewCategor = New System.Windows.Forms.FlowLayoutPanel()
        Dim Panel_PageHeaderIconNewCategor = New System.Windows.Forms.Panel()
        Dim Label_PageHeaderNewCategor = New System.Windows.Forms.Label()
        Dim Label_CategorDetailsNewCategor = New System.Windows.Forms.Label()
        Dim FlowLayoutPanel_CategorDetailCategorNameInputNewCategor = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label_CategorDetailCategorNameInputNewCategor = New System.Windows.Forms.Label()
        Dim Textbox_CategorDetailCategorNameInputNewCategor = New Guna.UI2.WinForms.Guna2TextBox()
        Dim Button_MakeCategorNewSale = New Guna.UI2.WinForms.Guna2Button()
        FlowLayoutPanel_CategorDetailsNewCategor.SuspendLayout
        FlowLayoutPanel_PageHeaderNewCategor.SuspendLayout
        FlowLayoutPanel_CategorDetailCategorNameInputNewCategor.SuspendLayout
        SuspendLayout
        '
        'FlowLayoutPanel_CategorDetailsNewCategor
        '
        FlowLayoutPanel_CategorDetailsNewCategor.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        FlowLayoutPanel_CategorDetailsNewCategor.Controls.Add(FlowLayoutPanel_PageHeaderNewCategor)
        FlowLayoutPanel_CategorDetailsNewCategor.Controls.Add(Label_CategorDetailsNewCategor)
        FlowLayoutPanel_CategorDetailsNewCategor.Controls.Add(FlowLayoutPanel_CategorDetailCategorNameInputNewCategor)
        FlowLayoutPanel_CategorDetailsNewCategor.Controls.Add(Button_MakeCategorNewSale)
        FlowLayoutPanel_CategorDetailsNewCategor.Location = New System.Drawing.Point(0, 0)
        FlowLayoutPanel_CategorDetailsNewCategor.Margin = New System.Windows.Forms.Padding(0)
        FlowLayoutPanel_CategorDetailsNewCategor.Name = "FlowLayoutPanel_CategorDetailsNewCategor"
        FlowLayoutPanel_CategorDetailsNewCategor.Size = New System.Drawing.Size(366, 350)
        FlowLayoutPanel_CategorDetailsNewCategor.TabIndex = 35
        '
        'FlowLayoutPanel_PageHeaderNewCategor
        '
        FlowLayoutPanel_PageHeaderNewCategor.AutoSize = true
        FlowLayoutPanel_PageHeaderNewCategor.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_PageHeaderNewCategor.Controls.Add(Panel_PageHeaderIconNewCategor)
        FlowLayoutPanel_PageHeaderNewCategor.Controls.Add(Label_PageHeaderNewCategor)
        FlowLayoutPanel_PageHeaderNewCategor.Location = New System.Drawing.Point(12, 24)
        FlowLayoutPanel_PageHeaderNewCategor.Margin = New System.Windows.Forms.Padding(12, 24, 0, 24)
        FlowLayoutPanel_PageHeaderNewCategor.Name = "FlowLayoutPanel_PageHeaderNewCategor"
        FlowLayoutPanel_PageHeaderNewCategor.Size = New System.Drawing.Size(226, 32)
        FlowLayoutPanel_PageHeaderNewCategor.TabIndex = 0
        '
        'Panel_PageHeaderIconNewCategor
        '
        Panel_PageHeaderIconNewCategor.Anchor = System.Windows.Forms.AnchorStyles.Left
        Panel_PageHeaderIconNewCategor.BackgroundImage = Global.ERP_SIMS.My.Resources.Resources.HeaderIcon_Categories
        Panel_PageHeaderIconNewCategor.Location = New System.Drawing.Point(0, 0)
        Panel_PageHeaderIconNewCategor.Margin = New System.Windows.Forms.Padding(0, 0, 16, 0)
        Panel_PageHeaderIconNewCategor.Name = "Panel_PageHeaderIconNewCategor"
        Panel_PageHeaderIconNewCategor.Size = New System.Drawing.Size(32, 32)
        Panel_PageHeaderIconNewCategor.TabIndex = 3
        '
        'Label_PageHeaderNewCategor
        '
        Label_PageHeaderNewCategor.Anchor = System.Windows.Forms.AnchorStyles.Left
        Label_PageHeaderNewCategor.AutoSize = true
        Label_PageHeaderNewCategor.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_PageHeaderNewCategor.Font = New System.Drawing.Font("Microsoft JhengHei", 24!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_PageHeaderNewCategor.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_PageHeaderNewCategor.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_PageHeaderNewCategor.Location = New System.Drawing.Point(48, 0)
        Label_PageHeaderNewCategor.Margin = New System.Windows.Forms.Padding(0)
        Label_PageHeaderNewCategor.Name = "Label_PageHeaderNewCategor"
        Label_PageHeaderNewCategor.Size = New System.Drawing.Size(178, 31)
        Label_PageHeaderNewCategor.TabIndex = 2
        Label_PageHeaderNewCategor.Text = "New Category"
        Label_PageHeaderNewCategor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label_CategorDetailsNewCategor
        '
        Label_CategorDetailsNewCategor.Anchor = System.Windows.Forms.AnchorStyles.Left
        Label_CategorDetailsNewCategor.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_CategorDetailsNewCategor.Font = New System.Drawing.Font("Microsoft JhengHei", 22!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_CategorDetailsNewCategor.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Label_CategorDetailsNewCategor.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_CategorDetailsNewCategor.Location = New System.Drawing.Point(12, 80)
        Label_CategorDetailsNewCategor.Margin = New System.Windows.Forms.Padding(12, 0, 12, 24)
        Label_CategorDetailsNewCategor.Name = "Label_CategorDetailsNewCategor"
        Label_CategorDetailsNewCategor.Size = New System.Drawing.Size(342, 28)
        Label_CategorDetailsNewCategor.TabIndex = 18
        Label_CategorDetailsNewCategor.Text = "Category Details"
        Label_CategorDetailsNewCategor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'FlowLayoutPanel_CategorDetailCategorNameInputNewCategor
        '
        FlowLayoutPanel_CategorDetailCategorNameInputNewCategor.AutoSize = true
        FlowLayoutPanel_CategorDetailCategorNameInputNewCategor.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_CategorDetailCategorNameInputNewCategor.Controls.Add(Label_CategorDetailCategorNameInputNewCategor)
        FlowLayoutPanel_CategorDetailCategorNameInputNewCategor.Controls.Add(Textbox_CategorDetailCategorNameInputNewCategor)
        FlowLayoutPanel_CategorDetailCategorNameInputNewCategor.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_CategorDetailCategorNameInputNewCategor.Location = New System.Drawing.Point(12, 132)
        FlowLayoutPanel_CategorDetailCategorNameInputNewCategor.Margin = New System.Windows.Forms.Padding(12, 0, 6, 0)
        FlowLayoutPanel_CategorDetailCategorNameInputNewCategor.Name = "FlowLayoutPanel_CategorDetailCategorNameInputNewCategor"
        FlowLayoutPanel_CategorDetailCategorNameInputNewCategor.Size = New System.Drawing.Size(342, 56)
        FlowLayoutPanel_CategorDetailCategorNameInputNewCategor.TabIndex = 19
        '
        'Label_CategorDetailCategorNameInputNewCategor
        '
        Label_CategorDetailCategorNameInputNewCategor.AutoSize = true
        Label_CategorDetailCategorNameInputNewCategor.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_CategorDetailCategorNameInputNewCategor.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_CategorDetailCategorNameInputNewCategor.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_CategorDetailCategorNameInputNewCategor.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_CategorDetailCategorNameInputNewCategor.Location = New System.Drawing.Point(0, 0)
        Label_CategorDetailCategorNameInputNewCategor.Margin = New System.Windows.Forms.Padding(0)
        Label_CategorDetailCategorNameInputNewCategor.Name = "Label_CategorDetailCategorNameInputNewCategor"
        Label_CategorDetailCategorNameInputNewCategor.Size = New System.Drawing.Size(97, 16)
        Label_CategorDetailCategorNameInputNewCategor.TabIndex = 2
        Label_CategorDetailCategorNameInputNewCategor.Text = "Category Name"
        Label_CategorDetailCategorNameInputNewCategor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Textbox_CategorDetailCategorNameInputNewCategor
        '
        Textbox_CategorDetailCategorNameInputNewCategor.Anchor = System.Windows.Forms.AnchorStyles.Left
        Textbox_CategorDetailCategorNameInputNewCategor.AutoCompleteCustomSource.AddRange(New String() {"(001) Item 1", "(002) Item 2", "(003) Item 3", "(004) Item 4", "(005) Item 5", "(006) Item 6", "(007) Item 7", "(008) Item 8", "(009) Item 9", "(010) Item 10"})
        Textbox_CategorDetailCategorNameInputNewCategor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Textbox_CategorDetailCategorNameInputNewCategor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Textbox_CategorDetailCategorNameInputNewCategor.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CategorDetailCategorNameInputNewCategor.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_CategorDetailCategorNameInputNewCategor.Cursor = System.Windows.Forms.Cursors.IBeam
        Textbox_CategorDetailCategorNameInputNewCategor.DefaultText = ""
        Textbox_CategorDetailCategorNameInputNewCategor.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer))
        Textbox_CategorDetailCategorNameInputNewCategor.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer))
        Textbox_CategorDetailCategorNameInputNewCategor.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_CategorDetailCategorNameInputNewCategor.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_CategorDetailCategorNameInputNewCategor.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CategorDetailCategorNameInputNewCategor.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CategorDetailCategorNameInputNewCategor.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
        Textbox_CategorDetailCategorNameInputNewCategor.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_CategorDetailCategorNameInputNewCategor.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CategorDetailCategorNameInputNewCategor.Location = New System.Drawing.Point(0, 16)
        Textbox_CategorDetailCategorNameInputNewCategor.Margin = New System.Windows.Forms.Padding(0)
        Textbox_CategorDetailCategorNameInputNewCategor.Name = "Textbox_CategorDetailCategorNameInputNewCategor"
        Textbox_CategorDetailCategorNameInputNewCategor.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Textbox_CategorDetailCategorNameInputNewCategor.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer))
        Textbox_CategorDetailCategorNameInputNewCategor.PlaceholderText = "Required"
        Textbox_CategorDetailCategorNameInputNewCategor.SelectedText = ""
        Textbox_CategorDetailCategorNameInputNewCategor.Size = New System.Drawing.Size(342, 40)
        Textbox_CategorDetailCategorNameInputNewCategor.TabIndex = 39
        '
        'Button_MakeCategorNewSale
        '
        Button_MakeCategorNewSale.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(44,Byte),Integer), CType(CType(62,Byte),Integer))
        Button_MakeCategorNewSale.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Button_MakeCategorNewSale.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Button_MakeCategorNewSale.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer))
        Button_MakeCategorNewSale.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer))
        Button_MakeCategorNewSale.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Button_MakeCategorNewSale.Font = New System.Drawing.Font("Segoe UI", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel)
        Button_MakeCategorNewSale.ForeColor = System.Drawing.Color.White
        Button_MakeCategorNewSale.Location = New System.Drawing.Point(12, 212)
        Button_MakeCategorNewSale.Margin = New System.Windows.Forms.Padding(12, 24, 12, 12)
        Button_MakeCategorNewSale.Name = "Button_MakeCategorNewSale"
        Button_MakeCategorNewSale.Size = New System.Drawing.Size(342, 40)
        Button_MakeCategorNewSale.TabIndex = 33
        Button_MakeCategorNewSale.Text = "Create Category"
        Button_MakeCategorNewSale.TextOffset = New System.Drawing.Point(0, -2)
        '
        '_Temporary_NewCategory
        '
        Prompt_NewCategory.ClientSize = New System.Drawing.Size(366, 350)
        Prompt_NewCategory.Controls.Add(FlowLayoutPanel_CategorDetailsNewCategor)
        Prompt_NewCategory.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        FlowLayoutPanel_CategorDetailsNewCategor.ResumeLayout(false)
        FlowLayoutPanel_CategorDetailsNewCategor.PerformLayout
        FlowLayoutPanel_PageHeaderNewCategor.ResumeLayout(false)
        FlowLayoutPanel_PageHeaderNewCategor.PerformLayout
        FlowLayoutPanel_CategorDetailCategorNameInputNewCategor.ResumeLayout(false)
        FlowLayoutPanel_CategorDetailCategorNameInputNewCategor.PerformLayout
        ResumeLayout(false)

        AddHandler Button_MakeCategorNewSale.Click, Sub(sender As Object, e As EventArgs)
            Dim categoryName As String = Textbox_CategorDetailCategorNameInputNewCategor.Text.Trim()
            Dim isValid As Boolean = True

            If String.IsNullOrEmpty(categoryName) Then
                Textbox_CategorDetailCategorNameInputNewCategor.BorderColor = Color.Red
                isValid = False
            Else
                Textbox_CategorDetailCategorNameInputNewCategor.BorderColor = Color.Black
            End If

            If Not isValid Then
                MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Try
                Using conn As New MySqlConnection(ConnectionString)
                    conn.Open()
                    Dim query As String = "INSERT INTO categories (category_name, created_by) VALUES (@category_name, @created_by)"
                    Using cmd As New MySqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@category_name", categoryName)
                        cmd.Parameters.AddWithValue("@created_by", GlobalShared.CurrentUser.AccountId)
                        cmd.ExecuteNonQuery()
                    End Using
                End Using
                MessageBox.Show("Category successfully added!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Prompt_NewCategory.Close()
            Catch ex As Exception
                MessageBox.Show($"Error adding category: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Prompt_NewCategory.ShowDialog(Me)
    End Sub

    Private Sub ShowNewItemPrompt()
        Dim Prompt_NewItem As New Form()
        Dim FlowLayoutPanel_NewItem = New System.Windows.Forms.FlowLayoutPanel()
        Dim FlowLayoutPanel_PageHeaderNewItem = New System.Windows.Forms.FlowLayoutPanel()
        Dim Panel_PageHeaderIconNewItem = New System.Windows.Forms.Panel()
        Dim Label_PageHeaderNewItem = New System.Windows.Forms.Label()
        Dim Label_ItemDetailsNewItem = New System.Windows.Forms.Label()
        Dim FlowLayoutPanel_ItemDetailItemNameInputNewItem = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label_ItemDetailItemNameInputNewItem = New System.Windows.Forms.Label()
        Dim Textbox_ItemDetailItemNameInputNewItem = New Guna.UI2.WinForms.Guna2TextBox()
        Dim FlowLayoutPanel_ItemDetailSellingPriceInputNewItem = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label_ItemDetailSellingPriceInputNewItem = New System.Windows.Forms.Label()
        Dim Textbox_ItemDetailSellingPriceInputNewItem = New Guna.UI2.WinForms.Guna2TextBox()
        Dim FlowLayoutPanel_ItemDetailBrandInputNewItem = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label_ItemDetailBrandInputNewItem = New System.Windows.Forms.Label()
        Dim Panel_CustomersPlaceOrder = New System.Windows.Forms.Panel()
        Dim ComboBox_ItemDetailBrandInputNewItem = New Guna.UI2.WinForms.Guna2ComboBox()
        Dim Button_NewBrandNewItem = New Guna.UI2.WinForms.Guna2Button()
        Dim FlowLayoutPanel_DateRangeInputsSales = New System.Windows.Forms.FlowLayoutPanel()
        Dim FlowLayoutPanel_ItemDetailCategoryInputNewItem = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label_ItemDetailCategoryInputNewItem = New System.Windows.Forms.Label()
        Dim Panel1 = New System.Windows.Forms.Panel()
        Dim Combobox_ItemDetailCategoryInputNewItem = New Guna.UI2.WinForms.Guna2ComboBox()
        Dim Button_NewCategoryNewItem = New Guna.UI2.WinForms.Guna2Button()
        Dim Button_ResetNewItem = New Guna.UI2.WinForms.Guna2Button()
        Dim Button_MakeItemNewItem = New Guna.UI2.WinForms.Guna2Button()
        FlowLayoutPanel_NewItem.SuspendLayout
        FlowLayoutPanel_PageHeaderNewItem.SuspendLayout
        FlowLayoutPanel_ItemDetailItemNameInputNewItem.SuspendLayout
        FlowLayoutPanel_ItemDetailSellingPriceInputNewItem.SuspendLayout
        FlowLayoutPanel_ItemDetailBrandInputNewItem.SuspendLayout
        Panel_CustomersPlaceOrder.SuspendLayout
        FlowLayoutPanel_ItemDetailCategoryInputNewItem.SuspendLayout
        Panel1.SuspendLayout
        SuspendLayout
        '
        'FlowLayoutPanel_NewItem
        '
        FlowLayoutPanel_NewItem.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        FlowLayoutPanel_NewItem.Controls.Add(FlowLayoutPanel_PageHeaderNewItem)
        FlowLayoutPanel_NewItem.Controls.Add(Label_ItemDetailsNewItem)
        FlowLayoutPanel_NewItem.Controls.Add(FlowLayoutPanel_ItemDetailItemNameInputNewItem)
        FlowLayoutPanel_NewItem.Controls.Add(FlowLayoutPanel_ItemDetailSellingPriceInputNewItem)
        FlowLayoutPanel_NewItem.Controls.Add(FlowLayoutPanel_ItemDetailBrandInputNewItem)
        FlowLayoutPanel_NewItem.Controls.Add(FlowLayoutPanel_DateRangeInputsSales)
        FlowLayoutPanel_NewItem.Controls.Add(FlowLayoutPanel_ItemDetailCategoryInputNewItem)
        FlowLayoutPanel_NewItem.Controls.Add(Button_ResetNewItem)
        FlowLayoutPanel_NewItem.Controls.Add(Button_MakeItemNewItem)
        FlowLayoutPanel_NewItem.Location = New System.Drawing.Point(0, 0)
        FlowLayoutPanel_NewItem.Margin = New System.Windows.Forms.Padding(0)
        FlowLayoutPanel_NewItem.Name = "FlowLayoutPanel_NewItem"
        FlowLayoutPanel_NewItem.Size = New System.Drawing.Size(720, 720)
        FlowLayoutPanel_NewItem.TabIndex = 34
        '
        'FlowLayoutPanel_PageHeaderNewItem
        '
        FlowLayoutPanel_PageHeaderNewItem.AutoSize = true
        FlowLayoutPanel_PageHeaderNewItem.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_PageHeaderNewItem.Controls.Add(Panel_PageHeaderIconNewItem)
        FlowLayoutPanel_PageHeaderNewItem.Controls.Add(Label_PageHeaderNewItem)
        FlowLayoutPanel_PageHeaderNewItem.Location = New System.Drawing.Point(12, 24)
        FlowLayoutPanel_PageHeaderNewItem.Margin = New System.Windows.Forms.Padding(12, 24, 0, 24)
        FlowLayoutPanel_PageHeaderNewItem.Name = "FlowLayoutPanel_PageHeaderNewItem"
        FlowLayoutPanel_PageHeaderNewItem.Size = New System.Drawing.Size(174, 32)
        FlowLayoutPanel_PageHeaderNewItem.TabIndex = 0
        '
        'Panel_PageHeaderIconNewItem
        '
        Panel_PageHeaderIconNewItem.Anchor = System.Windows.Forms.AnchorStyles.Left
        Panel_PageHeaderIconNewItem.BackgroundImage = Global.ERP_SIMS.My.Resources.Resources.HeaderIcon_Stocks
        Panel_PageHeaderIconNewItem.Location = New System.Drawing.Point(0, 0)
        Panel_PageHeaderIconNewItem.Margin = New System.Windows.Forms.Padding(0, 0, 16, 0)
        Panel_PageHeaderIconNewItem.Name = "Panel_PageHeaderIconNewItem"
        Panel_PageHeaderIconNewItem.Size = New System.Drawing.Size(32, 32)
        Panel_PageHeaderIconNewItem.TabIndex = 3
        '
        'Label_PageHeaderNewItem
        '
        Label_PageHeaderNewItem.Anchor = System.Windows.Forms.AnchorStyles.Left
        Label_PageHeaderNewItem.AutoSize = true
        Label_PageHeaderNewItem.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_PageHeaderNewItem.Font = New System.Drawing.Font("Microsoft JhengHei", 24!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_PageHeaderNewItem.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_PageHeaderNewItem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_PageHeaderNewItem.Location = New System.Drawing.Point(48, 0)
        Label_PageHeaderNewItem.Margin = New System.Windows.Forms.Padding(0)
        Label_PageHeaderNewItem.Name = "Label_PageHeaderNewItem"
        Label_PageHeaderNewItem.Size = New System.Drawing.Size(126, 31)
        Label_PageHeaderNewItem.TabIndex = 2
        Label_PageHeaderNewItem.Text = "New Item"
        Label_PageHeaderNewItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label_ItemDetailsNewItem
        '
        Label_ItemDetailsNewItem.Anchor = System.Windows.Forms.AnchorStyles.Left
        Label_ItemDetailsNewItem.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_ItemDetailsNewItem.Font = New System.Drawing.Font("Microsoft JhengHei", 22!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_ItemDetailsNewItem.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Label_ItemDetailsNewItem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_ItemDetailsNewItem.Location = New System.Drawing.Point(12, 80)
        Label_ItemDetailsNewItem.Margin = New System.Windows.Forms.Padding(12, 0, 12, 24)
        Label_ItemDetailsNewItem.Name = "Label_ItemDetailsNewItem"
        Label_ItemDetailsNewItem.Size = New System.Drawing.Size(696, 28)
        Label_ItemDetailsNewItem.TabIndex = 18
        Label_ItemDetailsNewItem.Text = "Item Details"
        Label_ItemDetailsNewItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'FlowLayoutPanel_ItemDetailItemNameInputNewItem
        '
        FlowLayoutPanel_ItemDetailItemNameInputNewItem.AutoSize = true
        FlowLayoutPanel_ItemDetailItemNameInputNewItem.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_ItemDetailItemNameInputNewItem.Controls.Add(Label_ItemDetailItemNameInputNewItem)
        FlowLayoutPanel_ItemDetailItemNameInputNewItem.Controls.Add(Textbox_ItemDetailItemNameInputNewItem)
        FlowLayoutPanel_ItemDetailItemNameInputNewItem.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_ItemDetailItemNameInputNewItem.Location = New System.Drawing.Point(12, 132)
        FlowLayoutPanel_ItemDetailItemNameInputNewItem.Margin = New System.Windows.Forms.Padding(12, 0, 6, 12)
        FlowLayoutPanel_ItemDetailItemNameInputNewItem.Name = "FlowLayoutPanel_ItemDetailItemNameInputNewItem"
        FlowLayoutPanel_ItemDetailItemNameInputNewItem.Size = New System.Drawing.Size(342, 56)
        FlowLayoutPanel_ItemDetailItemNameInputNewItem.TabIndex = 19
        '
        'Label_ItemDetailItemNameInputNewItem
        '
        Label_ItemDetailItemNameInputNewItem.AutoSize = true
        Label_ItemDetailItemNameInputNewItem.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_ItemDetailItemNameInputNewItem.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_ItemDetailItemNameInputNewItem.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_ItemDetailItemNameInputNewItem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_ItemDetailItemNameInputNewItem.Location = New System.Drawing.Point(0, 0)
        Label_ItemDetailItemNameInputNewItem.Margin = New System.Windows.Forms.Padding(0)
        Label_ItemDetailItemNameInputNewItem.Name = "Label_ItemDetailItemNameInputNewItem"
        Label_ItemDetailItemNameInputNewItem.Size = New System.Drawing.Size(70, 16)
        Label_ItemDetailItemNameInputNewItem.TabIndex = 2
        Label_ItemDetailItemNameInputNewItem.Text = "Item Name"
        Label_ItemDetailItemNameInputNewItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Textbox_ItemDetailItemNameInputNewItem
        '
        Textbox_ItemDetailItemNameInputNewItem.Anchor = System.Windows.Forms.AnchorStyles.Left
        Textbox_ItemDetailItemNameInputNewItem.AutoCompleteCustomSource.AddRange(New String() {"(001) Item 1", "(002) Item 2", "(003) Item 3", "(004) Item 4", "(005) Item 5", "(006) Item 6", "(007) Item 7", "(008) Item 8", "(009) Item 9", "(010) Item 10"})
        Textbox_ItemDetailItemNameInputNewItem.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Textbox_ItemDetailItemNameInputNewItem.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Textbox_ItemDetailItemNameInputNewItem.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_ItemDetailItemNameInputNewItem.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_ItemDetailItemNameInputNewItem.Cursor = System.Windows.Forms.Cursors.IBeam
        Textbox_ItemDetailItemNameInputNewItem.DefaultText = ""
        Textbox_ItemDetailItemNameInputNewItem.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer))
        Textbox_ItemDetailItemNameInputNewItem.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer))
        Textbox_ItemDetailItemNameInputNewItem.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_ItemDetailItemNameInputNewItem.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_ItemDetailItemNameInputNewItem.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_ItemDetailItemNameInputNewItem.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_ItemDetailItemNameInputNewItem.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
        Textbox_ItemDetailItemNameInputNewItem.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_ItemDetailItemNameInputNewItem.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_ItemDetailItemNameInputNewItem.Location = New System.Drawing.Point(0, 16)
        Textbox_ItemDetailItemNameInputNewItem.Margin = New System.Windows.Forms.Padding(0)
        Textbox_ItemDetailItemNameInputNewItem.Name = "Textbox_ItemDetailItemNameInputNewItem"
        Textbox_ItemDetailItemNameInputNewItem.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Textbox_ItemDetailItemNameInputNewItem.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer))
        Textbox_ItemDetailItemNameInputNewItem.PlaceholderText = "Required"
        Textbox_ItemDetailItemNameInputNewItem.SelectedText = ""
        Textbox_ItemDetailItemNameInputNewItem.Size = New System.Drawing.Size(342, 40)
        Textbox_ItemDetailItemNameInputNewItem.TabIndex = 39
        '
        'FlowLayoutPanel_ItemDetailSellingPriceInputNewItem
        '
        FlowLayoutPanel_ItemDetailSellingPriceInputNewItem.AutoSize = true
        FlowLayoutPanel_ItemDetailSellingPriceInputNewItem.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_ItemDetailSellingPriceInputNewItem.Controls.Add(Label_ItemDetailSellingPriceInputNewItem)
        FlowLayoutPanel_ItemDetailSellingPriceInputNewItem.Controls.Add(Textbox_ItemDetailSellingPriceInputNewItem)
        FlowLayoutPanel_ItemDetailSellingPriceInputNewItem.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_ItemDetailSellingPriceInputNewItem.Location = New System.Drawing.Point(366, 132)
        FlowLayoutPanel_ItemDetailSellingPriceInputNewItem.Margin = New System.Windows.Forms.Padding(6, 0, 12, 12)
        FlowLayoutPanel_ItemDetailSellingPriceInputNewItem.Name = "FlowLayoutPanel_ItemDetailSellingPriceInputNewItem"
        FlowLayoutPanel_ItemDetailSellingPriceInputNewItem.Size = New System.Drawing.Size(342, 56)
        FlowLayoutPanel_ItemDetailSellingPriceInputNewItem.TabIndex = 20
        '
        'Label_ItemDetailSellingPriceInputNewItem
        '
        Label_ItemDetailSellingPriceInputNewItem.AutoSize = true
        Label_ItemDetailSellingPriceInputNewItem.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_ItemDetailSellingPriceInputNewItem.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_ItemDetailSellingPriceInputNewItem.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_ItemDetailSellingPriceInputNewItem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_ItemDetailSellingPriceInputNewItem.Location = New System.Drawing.Point(0, 0)
        Label_ItemDetailSellingPriceInputNewItem.Margin = New System.Windows.Forms.Padding(0)
        Label_ItemDetailSellingPriceInputNewItem.Name = "Label_ItemDetailSellingPriceInputNewItem"
        Label_ItemDetailSellingPriceInputNewItem.Size = New System.Drawing.Size(75, 16)
        Label_ItemDetailSellingPriceInputNewItem.TabIndex = 2
        Label_ItemDetailSellingPriceInputNewItem.Text = "Selling Price"
        Label_ItemDetailSellingPriceInputNewItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Textbox_ItemDetailSellingPriceInputNewItem
        '
        Textbox_ItemDetailSellingPriceInputNewItem.Anchor = System.Windows.Forms.AnchorStyles.Left
        Textbox_ItemDetailSellingPriceInputNewItem.AutoCompleteCustomSource.AddRange(New String() {"(001) Item 1", "(002) Item 2", "(003) Item 3", "(004) Item 4", "(005) Item 5", "(006) Item 6", "(007) Item 7", "(008) Item 8", "(009) Item 9", "(010) Item 10"})
        Textbox_ItemDetailSellingPriceInputNewItem.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Textbox_ItemDetailSellingPriceInputNewItem.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Textbox_ItemDetailSellingPriceInputNewItem.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_ItemDetailSellingPriceInputNewItem.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_ItemDetailSellingPriceInputNewItem.Cursor = System.Windows.Forms.Cursors.IBeam
        Textbox_ItemDetailSellingPriceInputNewItem.DefaultText = ""
        Textbox_ItemDetailSellingPriceInputNewItem.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer))
        Textbox_ItemDetailSellingPriceInputNewItem.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer))
        Textbox_ItemDetailSellingPriceInputNewItem.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_ItemDetailSellingPriceInputNewItem.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_ItemDetailSellingPriceInputNewItem.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_ItemDetailSellingPriceInputNewItem.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_ItemDetailSellingPriceInputNewItem.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
        Textbox_ItemDetailSellingPriceInputNewItem.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_ItemDetailSellingPriceInputNewItem.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_ItemDetailSellingPriceInputNewItem.Location = New System.Drawing.Point(0, 16)
        Textbox_ItemDetailSellingPriceInputNewItem.Margin = New System.Windows.Forms.Padding(0)
        Textbox_ItemDetailSellingPriceInputNewItem.Name = "Textbox_ItemDetailSellingPriceInputNewItem"
        Textbox_ItemDetailSellingPriceInputNewItem.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Textbox_ItemDetailSellingPriceInputNewItem.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer))
        Textbox_ItemDetailSellingPriceInputNewItem.PlaceholderText = "Required"
        Textbox_ItemDetailSellingPriceInputNewItem.SelectedText = ""
        Textbox_ItemDetailSellingPriceInputNewItem.Size = New System.Drawing.Size(342, 40)
        Textbox_ItemDetailSellingPriceInputNewItem.TabIndex = 39
        '
        'FlowLayoutPanel_ItemDetailBrandInputNewItem
        '
        FlowLayoutPanel_ItemDetailBrandInputNewItem.AutoSize = true
        FlowLayoutPanel_ItemDetailBrandInputNewItem.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_ItemDetailBrandInputNewItem.Controls.Add(Label_ItemDetailBrandInputNewItem)
        FlowLayoutPanel_ItemDetailBrandInputNewItem.Controls.Add(Panel_CustomersPlaceOrder)
        FlowLayoutPanel_ItemDetailBrandInputNewItem.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_ItemDetailBrandInputNewItem.Location = New System.Drawing.Point(12, 200)
        FlowLayoutPanel_ItemDetailBrandInputNewItem.Margin = New System.Windows.Forms.Padding(12, 0, 0, 12)
        FlowLayoutPanel_ItemDetailBrandInputNewItem.Name = "FlowLayoutPanel_ItemDetailBrandInputNewItem"
        FlowLayoutPanel_ItemDetailBrandInputNewItem.Size = New System.Drawing.Size(693, 56)
        FlowLayoutPanel_ItemDetailBrandInputNewItem.TabIndex = 37
        '
        'Label_ItemDetailBrandInputNewItem
        '
        Label_ItemDetailBrandInputNewItem.AutoSize = true
        Label_ItemDetailBrandInputNewItem.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_ItemDetailBrandInputNewItem.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_ItemDetailBrandInputNewItem.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_ItemDetailBrandInputNewItem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_ItemDetailBrandInputNewItem.Location = New System.Drawing.Point(0, 0)
        Label_ItemDetailBrandInputNewItem.Margin = New System.Windows.Forms.Padding(0)
        Label_ItemDetailBrandInputNewItem.Name = "Label_ItemDetailBrandInputNewItem"
        Label_ItemDetailBrandInputNewItem.Size = New System.Drawing.Size(40, 16)
        Label_ItemDetailBrandInputNewItem.TabIndex = 2
        Label_ItemDetailBrandInputNewItem.Text = "Brand"
        Label_ItemDetailBrandInputNewItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Panel_CustomersPlaceOrder
        '
        Panel_CustomersPlaceOrder.Controls.Add(ComboBox_ItemDetailBrandInputNewItem)
        Panel_CustomersPlaceOrder.Controls.Add(Button_NewBrandNewItem)
        Panel_CustomersPlaceOrder.Location = New System.Drawing.Point(0, 16)
        Panel_CustomersPlaceOrder.Margin = New System.Windows.Forms.Padding(0)
        Panel_CustomersPlaceOrder.Name = "Panel_CustomersPlaceOrder"
        Panel_CustomersPlaceOrder.Size = New System.Drawing.Size(693, 40)
        Panel_CustomersPlaceOrder.TabIndex = 31
        '
        'ComboBox_ItemDetailBrandInputNewItem
        '
        ComboBox_ItemDetailBrandInputNewItem.BackColor = System.Drawing.Color.Transparent
        ComboBox_ItemDetailBrandInputNewItem.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        ComboBox_ItemDetailBrandInputNewItem.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        ComboBox_ItemDetailBrandInputNewItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        ComboBox_ItemDetailBrandInputNewItem.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        ComboBox_ItemDetailBrandInputNewItem.FocusedColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        ComboBox_ItemDetailBrandInputNewItem.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        ComboBox_ItemDetailBrandInputNewItem.Font = New System.Drawing.Font("Segoe UI", 10!)
        ComboBox_ItemDetailBrandInputNewItem.ForeColor = System.Drawing.Color.FromArgb(CType(CType(68,Byte),Integer), CType(CType(88,Byte),Integer), CType(CType(112,Byte),Integer))
        ComboBox_ItemDetailBrandInputNewItem.ItemHeight = 34
        ComboBox_ItemDetailBrandInputNewItem.Items.AddRange(New Object() {"Benjamin Rollan", "Benjamin Rollan", "Benjamin Rollan", "Benjamin Rollan"})
        ComboBox_ItemDetailBrandInputNewItem.Location = New System.Drawing.Point(0, 0)
        ComboBox_ItemDetailBrandInputNewItem.Margin = New System.Windows.Forms.Padding(0)
        ComboBox_ItemDetailBrandInputNewItem.Name = "ComboBox_ItemDetailBrandInputNewItem"
        ComboBox_ItemDetailBrandInputNewItem.Size = New System.Drawing.Size(549, 40)
        ComboBox_ItemDetailBrandInputNewItem.TabIndex = 31
        '
        'Button_NewBrandNewItem
        '
        Button_NewBrandNewItem.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(44,Byte),Integer), CType(CType(62,Byte),Integer))
        Button_NewBrandNewItem.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Button_NewBrandNewItem.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Button_NewBrandNewItem.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer))
        Button_NewBrandNewItem.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer))
        Button_NewBrandNewItem.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Button_NewBrandNewItem.Font = New System.Drawing.Font("Segoe UI", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel)
        Button_NewBrandNewItem.ForeColor = System.Drawing.Color.White
        Button_NewBrandNewItem.Location = New System.Drawing.Point(561, 0)
        Button_NewBrandNewItem.Margin = New System.Windows.Forms.Padding(12, 0, 0, 0)
        Button_NewBrandNewItem.Name = "Button_NewBrandNewItem"
        Button_NewBrandNewItem.Size = New System.Drawing.Size(132, 40)
        Button_NewBrandNewItem.TabIndex = 31
        Button_NewBrandNewItem.Text = "New Brand"
        Button_NewBrandNewItem.TextOffset = New System.Drawing.Point(0, -2)
        '
        'FlowLayoutPanel_DateRangeInputsSales
        '
        FlowLayoutPanel_DateRangeInputsSales.AutoSize = true
        FlowLayoutPanel_DateRangeInputsSales.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_DateRangeInputsSales.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_DateRangeInputsSales.Location = New System.Drawing.Point(705, 200)
        FlowLayoutPanel_DateRangeInputsSales.Margin = New System.Windows.Forms.Padding(0, 0, 0, 42)
        FlowLayoutPanel_DateRangeInputsSales.Name = "FlowLayoutPanel_DateRangeInputsSales"
        FlowLayoutPanel_DateRangeInputsSales.Size = New System.Drawing.Size(0, 0)
        FlowLayoutPanel_DateRangeInputsSales.TabIndex = 35
        '
        'FlowLayoutPanel_ItemDetailCategoryInputNewItem
        '
        FlowLayoutPanel_ItemDetailCategoryInputNewItem.AutoSize = true
        FlowLayoutPanel_ItemDetailCategoryInputNewItem.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_ItemDetailCategoryInputNewItem.Controls.Add(Label_ItemDetailCategoryInputNewItem)
        FlowLayoutPanel_ItemDetailCategoryInputNewItem.Controls.Add(Panel1)
        FlowLayoutPanel_ItemDetailCategoryInputNewItem.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_ItemDetailCategoryInputNewItem.Location = New System.Drawing.Point(12, 268)
        FlowLayoutPanel_ItemDetailCategoryInputNewItem.Margin = New System.Windows.Forms.Padding(12, 0, 0, 0)
        FlowLayoutPanel_ItemDetailCategoryInputNewItem.Name = "FlowLayoutPanel_ItemDetailCategoryInputNewItem"
        FlowLayoutPanel_ItemDetailCategoryInputNewItem.Size = New System.Drawing.Size(693, 56)
        FlowLayoutPanel_ItemDetailCategoryInputNewItem.TabIndex = 38
        '
        'Label_ItemDetailCategoryInputNewItem
        '
        Label_ItemDetailCategoryInputNewItem.AutoSize = true
        Label_ItemDetailCategoryInputNewItem.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_ItemDetailCategoryInputNewItem.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_ItemDetailCategoryInputNewItem.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_ItemDetailCategoryInputNewItem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_ItemDetailCategoryInputNewItem.Location = New System.Drawing.Point(0, 0)
        Label_ItemDetailCategoryInputNewItem.Margin = New System.Windows.Forms.Padding(0)
        Label_ItemDetailCategoryInputNewItem.Name = "Label_ItemDetailCategoryInputNewItem"
        Label_ItemDetailCategoryInputNewItem.Size = New System.Drawing.Size(59, 16)
        Label_ItemDetailCategoryInputNewItem.TabIndex = 2
        Label_ItemDetailCategoryInputNewItem.Text = "Category"
        Label_ItemDetailCategoryInputNewItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Panel1
        '
        Panel1.Controls.Add(Combobox_ItemDetailCategoryInputNewItem)
        Panel1.Controls.Add(Button_NewCategoryNewItem)
        Panel1.Location = New System.Drawing.Point(0, 16)
        Panel1.Margin = New System.Windows.Forms.Padding(0)
        Panel1.Name = "Panel1"
        Panel1.Size = New System.Drawing.Size(693, 40)
        Panel1.TabIndex = 31
        '
        'Combobox_ItemDetailCategoryInputNewItem
        '
        Combobox_ItemDetailCategoryInputNewItem.BackColor = System.Drawing.Color.Transparent
        Combobox_ItemDetailCategoryInputNewItem.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Combobox_ItemDetailCategoryInputNewItem.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Combobox_ItemDetailCategoryInputNewItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Combobox_ItemDetailCategoryInputNewItem.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Combobox_ItemDetailCategoryInputNewItem.FocusedColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Combobox_ItemDetailCategoryInputNewItem.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Combobox_ItemDetailCategoryInputNewItem.Font = New System.Drawing.Font("Segoe UI", 10!)
        Combobox_ItemDetailCategoryInputNewItem.ForeColor = System.Drawing.Color.FromArgb(CType(CType(68,Byte),Integer), CType(CType(88,Byte),Integer), CType(CType(112,Byte),Integer))
        Combobox_ItemDetailCategoryInputNewItem.ItemHeight = 34
        Combobox_ItemDetailCategoryInputNewItem.Items.AddRange(New Object() {"Benjamin Rollan", "Benjamin Rollan", "Benjamin Rollan", "Benjamin Rollan"})
        Combobox_ItemDetailCategoryInputNewItem.Location = New System.Drawing.Point(0, 0)
        Combobox_ItemDetailCategoryInputNewItem.Margin = New System.Windows.Forms.Padding(0)
        Combobox_ItemDetailCategoryInputNewItem.Name = "Combobox_ItemDetailCategoryInputNewItem"
        Combobox_ItemDetailCategoryInputNewItem.Size = New System.Drawing.Size(549, 40)
        Combobox_ItemDetailCategoryInputNewItem.TabIndex = 31
        '
        'Button_NewCategoryNewItem
        '
        Button_NewCategoryNewItem.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(44,Byte),Integer), CType(CType(62,Byte),Integer))
        Button_NewCategoryNewItem.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Button_NewCategoryNewItem.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Button_NewCategoryNewItem.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer))
        Button_NewCategoryNewItem.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer))
        Button_NewCategoryNewItem.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Button_NewCategoryNewItem.Font = New System.Drawing.Font("Segoe UI", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel)
        Button_NewCategoryNewItem.ForeColor = System.Drawing.Color.White
        Button_NewCategoryNewItem.Location = New System.Drawing.Point(561, 0)
        Button_NewCategoryNewItem.Margin = New System.Windows.Forms.Padding(12, 0, 0, 0)
        Button_NewCategoryNewItem.Name = "Button_NewCategoryNewItem"
        Button_NewCategoryNewItem.Size = New System.Drawing.Size(132, 40)
        Button_NewCategoryNewItem.TabIndex = 31
        Button_NewCategoryNewItem.Text = "New Category"
        Button_NewCategoryNewItem.TextOffset = New System.Drawing.Point(0, -2)
        '
        'Button_ResetNewItem
        '
        Button_ResetNewItem.BackColor = System.Drawing.Color.FromArgb(CType(CType(220,Byte),Integer), CType(CType(76,Byte),Integer), CType(CType(100,Byte),Integer))
        Button_ResetNewItem.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Button_ResetNewItem.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Button_ResetNewItem.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer))
        Button_ResetNewItem.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer))
        Button_ResetNewItem.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Button_ResetNewItem.Font = New System.Drawing.Font("Segoe UI", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel)
        Button_ResetNewItem.ForeColor = System.Drawing.Color.White
        Button_ResetNewItem.Location = New System.Drawing.Point(12, 348)
        Button_ResetNewItem.Margin = New System.Windows.Forms.Padding(12, 24, 6, 12)
        Button_ResetNewItem.Name = "Button_ResetNewItem"
        Button_ResetNewItem.Size = New System.Drawing.Size(342, 40)
        Button_ResetNewItem.TabIndex = 34
        Button_ResetNewItem.Text = "Reset"
        Button_ResetNewItem.TextOffset = New System.Drawing.Point(0, -2)
        '
        'Button_MakeItemNewItem
        '
        Button_MakeItemNewItem.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(44,Byte),Integer), CType(CType(62,Byte),Integer))
        Button_MakeItemNewItem.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Button_MakeItemNewItem.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Button_MakeItemNewItem.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer))
        Button_MakeItemNewItem.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer))
        Button_MakeItemNewItem.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Button_MakeItemNewItem.Font = New System.Drawing.Font("Segoe UI", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel)
        Button_MakeItemNewItem.ForeColor = System.Drawing.Color.White
        Button_MakeItemNewItem.Location = New System.Drawing.Point(366, 348)
        Button_MakeItemNewItem.Margin = New System.Windows.Forms.Padding(6, 24, 12, 12)
        Button_MakeItemNewItem.Name = "Button_MakeItemNewItem"
        Button_MakeItemNewItem.Size = New System.Drawing.Size(342, 40)
        Button_MakeItemNewItem.TabIndex = 33
        Button_MakeItemNewItem.Text = "Create Item"
        Button_MakeItemNewItem.TextOffset = New System.Drawing.Point(0, -2)
        '
        '_Temporary_NewItem
        '
        Prompt_NewItem.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Prompt_NewItem.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Prompt_NewItem.ClientSize = New System.Drawing.Size(720, 720)
        Prompt_NewItem.Controls.Add(FlowLayoutPanel_NewItem)
        Prompt_NewItem.Name = "_Temporary_NewItem"
        Prompt_NewItem.Text = "_Temporary_NewItem"
        Prompt_NewItem.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        FlowLayoutPanel_NewItem.ResumeLayout(false)
        FlowLayoutPanel_NewItem.PerformLayout
        FlowLayoutPanel_PageHeaderNewItem.ResumeLayout(false)
        FlowLayoutPanel_PageHeaderNewItem.PerformLayout
        FlowLayoutPanel_ItemDetailItemNameInputNewItem.ResumeLayout(false)
        FlowLayoutPanel_ItemDetailItemNameInputNewItem.PerformLayout
        FlowLayoutPanel_ItemDetailSellingPriceInputNewItem.ResumeLayout(false)
        FlowLayoutPanel_ItemDetailSellingPriceInputNewItem.PerformLayout
        FlowLayoutPanel_ItemDetailBrandInputNewItem.ResumeLayout(false)
        FlowLayoutPanel_ItemDetailBrandInputNewItem.PerformLayout
        Panel_CustomersPlaceOrder.ResumeLayout(false)
        FlowLayoutPanel_ItemDetailCategoryInputNewItem.ResumeLayout(false)
        FlowLayoutPanel_ItemDetailCategoryInputNewItem.PerformLayout
        Panel1.ResumeLayout(false)
        ResumeLayout(false)

        AddHandler Button_NewBrandNewItem.Click, Sub(sender As Object, e As EventArgs)
            Dim Prompt_NewBrand As New Form()
            Dim FlowLayoutPanel_BrandDetailsNewBrand = New System.Windows.Forms.FlowLayoutPanel()
            Dim FlowLayoutPanel_PageHeaderNewBrand = New System.Windows.Forms.FlowLayoutPanel()
            Dim Label_PageHeaderNewBrand = New System.Windows.Forms.Label()
            Dim Label_BrandDetailsNewBrand = New System.Windows.Forms.Label()
            Dim FlowLayoutPanel_BrandDetailBrandNameInputNewBrand = New System.Windows.Forms.FlowLayoutPanel()
            Dim Label_BrandDetailBrandNameInputNewBrand = New System.Windows.Forms.Label()
            Dim Textbox_BrandDetailBrandNameInputNewBrand = New Guna.UI2.WinForms.Guna2TextBox()
            Dim FlowLayoutPanel_BrandNameNewBrand = New System.Windows.Forms.FlowLayoutPanel()
            Dim Button_MakeBrandNewSale = New Guna.UI2.WinForms.Guna2Button()
            Dim Panel_PageHeaderIconNewBrand = New System.Windows.Forms.Panel()
            FlowLayoutPanel_BrandDetailsNewBrand.SuspendLayout
            FlowLayoutPanel_PageHeaderNewBrand.SuspendLayout
            FlowLayoutPanel_BrandDetailBrandNameInputNewBrand.SuspendLayout
            SuspendLayout
            '
            'FlowLayoutPanel_BrandDetailsNewBrand
            '
            FlowLayoutPanel_BrandDetailsNewBrand.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
            FlowLayoutPanel_BrandDetailsNewBrand.Controls.Add(FlowLayoutPanel_PageHeaderNewBrand)
            FlowLayoutPanel_BrandDetailsNewBrand.Controls.Add(Label_BrandDetailsNewBrand)
            FlowLayoutPanel_BrandDetailsNewBrand.Controls.Add(FlowLayoutPanel_BrandDetailBrandNameInputNewBrand)
            FlowLayoutPanel_BrandDetailsNewBrand.Controls.Add(FlowLayoutPanel_BrandNameNewBrand)
            FlowLayoutPanel_BrandDetailsNewBrand.Controls.Add(Button_MakeBrandNewSale)
            FlowLayoutPanel_BrandDetailsNewBrand.Location = New System.Drawing.Point(0, 0)
            FlowLayoutPanel_BrandDetailsNewBrand.Margin = New System.Windows.Forms.Padding(0)
            FlowLayoutPanel_BrandDetailsNewBrand.Name = "FlowLayoutPanel_BrandDetailsNewBrand"
            FlowLayoutPanel_BrandDetailsNewBrand.Size = New System.Drawing.Size(366, 350)
            FlowLayoutPanel_BrandDetailsNewBrand.TabIndex = 34
            '
            'FlowLayoutPanel_PageHeaderNewBrand
            '
            FlowLayoutPanel_PageHeaderNewBrand.AutoSize = true
            FlowLayoutPanel_PageHeaderNewBrand.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            FlowLayoutPanel_PageHeaderNewBrand.Controls.Add(Panel_PageHeaderIconNewBrand)
            FlowLayoutPanel_PageHeaderNewBrand.Controls.Add(Label_PageHeaderNewBrand)
            FlowLayoutPanel_PageHeaderNewBrand.Location = New System.Drawing.Point(12, 24)
            FlowLayoutPanel_PageHeaderNewBrand.Margin = New System.Windows.Forms.Padding(12, 24, 0, 24)
            FlowLayoutPanel_PageHeaderNewBrand.Name = "FlowLayoutPanel_PageHeaderNewBrand"
            FlowLayoutPanel_PageHeaderNewBrand.Size = New System.Drawing.Size(189, 32)
            FlowLayoutPanel_PageHeaderNewBrand.TabIndex = 0
            '
            'Label_PageHeaderNewBrand
            '
            Label_PageHeaderNewBrand.Anchor = System.Windows.Forms.AnchorStyles.Left
            Label_PageHeaderNewBrand.AutoSize = true
            Label_PageHeaderNewBrand.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
            Label_PageHeaderNewBrand.Font = New System.Drawing.Font("Microsoft JhengHei", 24!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
            Label_PageHeaderNewBrand.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
            Label_PageHeaderNewBrand.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Label_PageHeaderNewBrand.Location = New System.Drawing.Point(48, 0)
            Label_PageHeaderNewBrand.Margin = New System.Windows.Forms.Padding(0)
            Label_PageHeaderNewBrand.Name = "Label_PageHeaderNewBrand"
            Label_PageHeaderNewBrand.Size = New System.Drawing.Size(141, 31)
            Label_PageHeaderNewBrand.TabIndex = 2
            Label_PageHeaderNewBrand.Text = "New Brand"
            Label_PageHeaderNewBrand.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'Label_BrandDetailsNewBrand
            '
            Label_BrandDetailsNewBrand.Anchor = System.Windows.Forms.AnchorStyles.Left
            Label_BrandDetailsNewBrand.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
            Label_BrandDetailsNewBrand.Font = New System.Drawing.Font("Microsoft JhengHei", 22!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
            Label_BrandDetailsNewBrand.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
            Label_BrandDetailsNewBrand.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Label_BrandDetailsNewBrand.Location = New System.Drawing.Point(12, 80)
            Label_BrandDetailsNewBrand.Margin = New System.Windows.Forms.Padding(12, 0, 12, 24)
            Label_BrandDetailsNewBrand.Name = "Label_BrandDetailsNewBrand"
            Label_BrandDetailsNewBrand.Size = New System.Drawing.Size(342, 28)
            Label_BrandDetailsNewBrand.TabIndex = 18
            Label_BrandDetailsNewBrand.Text = "Brand Details"
            Label_BrandDetailsNewBrand.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'FlowLayoutPanel_BrandDetailBrandNameInputNewBrand
            '
            FlowLayoutPanel_BrandDetailBrandNameInputNewBrand.AutoSize = true
            FlowLayoutPanel_BrandDetailBrandNameInputNewBrand.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            FlowLayoutPanel_BrandDetailBrandNameInputNewBrand.Controls.Add(Label_BrandDetailBrandNameInputNewBrand)
            FlowLayoutPanel_BrandDetailBrandNameInputNewBrand.Controls.Add(Textbox_BrandDetailBrandNameInputNewBrand)
            FlowLayoutPanel_BrandDetailBrandNameInputNewBrand.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
            FlowLayoutPanel_BrandDetailBrandNameInputNewBrand.Location = New System.Drawing.Point(12, 132)
            FlowLayoutPanel_BrandDetailBrandNameInputNewBrand.Margin = New System.Windows.Forms.Padding(12, 0, 6, 0)
            FlowLayoutPanel_BrandDetailBrandNameInputNewBrand.Name = "FlowLayoutPanel_BrandDetailBrandNameInputNewBrand"
            FlowLayoutPanel_BrandDetailBrandNameInputNewBrand.Size = New System.Drawing.Size(342, 56)
            FlowLayoutPanel_BrandDetailBrandNameInputNewBrand.TabIndex = 19
            '
            'Label_BrandDetailBrandNameInputNewBrand
            '
            Label_BrandDetailBrandNameInputNewBrand.AutoSize = true
            Label_BrandDetailBrandNameInputNewBrand.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
            Label_BrandDetailBrandNameInputNewBrand.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
            Label_BrandDetailBrandNameInputNewBrand.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
            Label_BrandDetailBrandNameInputNewBrand.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Label_BrandDetailBrandNameInputNewBrand.Location = New System.Drawing.Point(0, 0)
            Label_BrandDetailBrandNameInputNewBrand.Margin = New System.Windows.Forms.Padding(0)
            Label_BrandDetailBrandNameInputNewBrand.Name = "Label_BrandDetailBrandNameInputNewBrand"
            Label_BrandDetailBrandNameInputNewBrand.Size = New System.Drawing.Size(78, 16)
            Label_BrandDetailBrandNameInputNewBrand.TabIndex = 2
            Label_BrandDetailBrandNameInputNewBrand.Text = "Brand Name"
            Label_BrandDetailBrandNameInputNewBrand.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'Textbox_BrandDetailBrandNameInputNewBrand
            '
            Textbox_BrandDetailBrandNameInputNewBrand.Anchor = System.Windows.Forms.AnchorStyles.Left
            Textbox_BrandDetailBrandNameInputNewBrand.AutoCompleteCustomSource.AddRange(New String() {"(001) Item 1", "(002) Item 2", "(003) Item 3", "(004) Item 4", "(005) Item 5", "(006) Item 6", "(007) Item 7", "(008) Item 8", "(009) Item 9", "(010) Item 10"})
            Textbox_BrandDetailBrandNameInputNewBrand.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
            Textbox_BrandDetailBrandNameInputNewBrand.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
            Textbox_BrandDetailBrandNameInputNewBrand.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
            Textbox_BrandDetailBrandNameInputNewBrand.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
            Textbox_BrandDetailBrandNameInputNewBrand.Cursor = System.Windows.Forms.Cursors.IBeam
            Textbox_BrandDetailBrandNameInputNewBrand.DefaultText = ""
            Textbox_BrandDetailBrandNameInputNewBrand.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer))
            Textbox_BrandDetailBrandNameInputNewBrand.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer))
            Textbox_BrandDetailBrandNameInputNewBrand.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
            Textbox_BrandDetailBrandNameInputNewBrand.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
            Textbox_BrandDetailBrandNameInputNewBrand.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
            Textbox_BrandDetailBrandNameInputNewBrand.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
            Textbox_BrandDetailBrandNameInputNewBrand.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
            Textbox_BrandDetailBrandNameInputNewBrand.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
            Textbox_BrandDetailBrandNameInputNewBrand.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
            Textbox_BrandDetailBrandNameInputNewBrand.Location = New System.Drawing.Point(0, 16)
            Textbox_BrandDetailBrandNameInputNewBrand.Margin = New System.Windows.Forms.Padding(0)
            Textbox_BrandDetailBrandNameInputNewBrand.Name = "Textbox_BrandDetailBrandNameInputNewBrand"
            Textbox_BrandDetailBrandNameInputNewBrand.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
            Textbox_BrandDetailBrandNameInputNewBrand.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer))
            Textbox_BrandDetailBrandNameInputNewBrand.PlaceholderText = "Required"
            Textbox_BrandDetailBrandNameInputNewBrand.SelectedText = ""
            Textbox_BrandDetailBrandNameInputNewBrand.Size = New System.Drawing.Size(342, 40)
            Textbox_BrandDetailBrandNameInputNewBrand.TabIndex = 39
            '
            'FlowLayoutPanel_BrandNameNewBrand
            '
            FlowLayoutPanel_BrandNameNewBrand.AutoSize = true
            FlowLayoutPanel_BrandNameNewBrand.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            FlowLayoutPanel_BrandNameNewBrand.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
            FlowLayoutPanel_BrandNameNewBrand.Location = New System.Drawing.Point(360, 132)
            FlowLayoutPanel_BrandNameNewBrand.Margin = New System.Windows.Forms.Padding(0, 0, 0, 42)
            FlowLayoutPanel_BrandNameNewBrand.Name = "FlowLayoutPanel_BrandNameNewBrand"
            FlowLayoutPanel_BrandNameNewBrand.Size = New System.Drawing.Size(0, 0)
            FlowLayoutPanel_BrandNameNewBrand.TabIndex = 35
            '
            'Button_MakeBrandNewSale
            '
            Button_MakeBrandNewSale.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(44,Byte),Integer), CType(CType(62,Byte),Integer))
            Button_MakeBrandNewSale.DisabledState.BorderColor = System.Drawing.Color.DarkGray
            Button_MakeBrandNewSale.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
            Button_MakeBrandNewSale.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer))
            Button_MakeBrandNewSale.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer))
            Button_MakeBrandNewSale.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
            Button_MakeBrandNewSale.Font = New System.Drawing.Font("Segoe UI", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel)
            Button_MakeBrandNewSale.ForeColor = System.Drawing.Color.White
            Button_MakeBrandNewSale.Location = New System.Drawing.Point(12, 212)
            Button_MakeBrandNewSale.Margin = New System.Windows.Forms.Padding(12, 24, 12, 12)
            Button_MakeBrandNewSale.Name = "Button_MakeBrandNewSale"
            Button_MakeBrandNewSale.Size = New System.Drawing.Size(342, 40)
            Button_MakeBrandNewSale.TabIndex = 33
            Button_MakeBrandNewSale.Text = "Create Brand"
            Button_MakeBrandNewSale.TextOffset = New System.Drawing.Point(0, -2)
            '
            'Panel_PageHeaderIconNewBrand
            '
            Panel_PageHeaderIconNewBrand.Anchor = System.Windows.Forms.AnchorStyles.Left
            Panel_PageHeaderIconNewBrand.BackgroundImage = Global.ERP_SIMS.My.Resources.Resources.HeaderIcon_Brands
            Panel_PageHeaderIconNewBrand.Location = New System.Drawing.Point(0, 0)
            Panel_PageHeaderIconNewBrand.Margin = New System.Windows.Forms.Padding(0, 0, 16, 0)
            Panel_PageHeaderIconNewBrand.Name = "Panel_PageHeaderIconNewBrand"
            Panel_PageHeaderIconNewBrand.Size = New System.Drawing.Size(32, 32)
            Panel_PageHeaderIconNewBrand.TabIndex = 3
            '
            '_Temporary_NewBrand
            '
            Prompt_NewBrand.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
            Prompt_NewBrand.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Prompt_NewBrand.ClientSize = New System.Drawing.Size(366, 350)
            Prompt_NewBrand.Controls.Add(FlowLayoutPanel_BrandDetailsNewBrand)
            Prompt_NewBrand.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            FlowLayoutPanel_BrandDetailsNewBrand.ResumeLayout(false)
            FlowLayoutPanel_BrandDetailsNewBrand.PerformLayout
            FlowLayoutPanel_PageHeaderNewBrand.ResumeLayout(false)
            FlowLayoutPanel_PageHeaderNewBrand.PerformLayout
            FlowLayoutPanel_BrandDetailBrandNameInputNewBrand.ResumeLayout(false)
            FlowLayoutPanel_BrandDetailBrandNameInputNewBrand.PerformLayout
            ResumeLayout(false)

            AddHandler Button_MakeBrandNewSale.Click, Sub(senderr As Object, ee As EventArgs)
                Dim brandName As String = Textbox_BrandDetailBrandNameInputNewBrand.Text.Trim()
                Dim isValid As Boolean = True

                If String.IsNullOrEmpty(brandName) Then
                    Textbox_BrandDetailBrandNameInputNewBrand.BorderColor = Color.Red
                    isValid = False
                Else
                    Textbox_BrandDetailBrandNameInputNewBrand.BorderColor = Color.Black
                End If

                If Not isValid Then
                    MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If

                Try
                    Using conn As New MySqlConnection(ConnectionString)
                        conn.Open()
                        Dim query As String = "INSERT INTO brands (brand_name, created_by) VALUES (@brand_name, @created_by)"
                        Using cmd As New MySqlCommand(query, conn)
                            cmd.Parameters.AddWithValue("@brand_name", brandName)
                            cmd.Parameters.AddWithValue("@created_by", GlobalShared.CurrentUser.AccountId)
                            cmd.ExecuteNonQuery()
                        End Using
                    End Using
                    MessageBox.Show("Brand successfully added!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Brands = CreateBrandArray()
                    PopulateBrandComboBox(ComboBox_ItemDetailBrandInputNewItem)
                    Prompt_NewBrand.Close()
                Catch ex As Exception
                    MessageBox.Show($"Error adding brand: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End Sub

            Prompt_NewBrand.ShowDialog(Me)
        End Sub

        AddHandler Button_NewCategoryNewItem.Click, Sub(sender As Object, e As EventArgs)
            Dim Prompt_NewCategory As New Form()
            Dim FlowLayoutPanel_CategorDetailsNewCategor = New System.Windows.Forms.FlowLayoutPanel()
            Dim FlowLayoutPanel_PageHeaderNewCategor = New System.Windows.Forms.FlowLayoutPanel()
            Dim Panel_PageHeaderIconNewCategor = New System.Windows.Forms.Panel()
            Dim Label_PageHeaderNewCategor = New System.Windows.Forms.Label()
            Dim Label_CategorDetailsNewCategor = New System.Windows.Forms.Label()
            Dim FlowLayoutPanel_CategorDetailCategorNameInputNewCategor = New System.Windows.Forms.FlowLayoutPanel()
            Dim Label_CategorDetailCategorNameInputNewCategor = New System.Windows.Forms.Label()
            Dim Textbox_CategorDetailCategorNameInputNewCategor = New Guna.UI2.WinForms.Guna2TextBox()
            Dim Button_MakeCategorNewSale = New Guna.UI2.WinForms.Guna2Button()
            FlowLayoutPanel_CategorDetailsNewCategor.SuspendLayout
            FlowLayoutPanel_PageHeaderNewCategor.SuspendLayout
            FlowLayoutPanel_CategorDetailCategorNameInputNewCategor.SuspendLayout
            SuspendLayout
            '
            'FlowLayoutPanel_CategorDetailsNewCategor
            '
            FlowLayoutPanel_CategorDetailsNewCategor.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
            FlowLayoutPanel_CategorDetailsNewCategor.Controls.Add(FlowLayoutPanel_PageHeaderNewCategor)
            FlowLayoutPanel_CategorDetailsNewCategor.Controls.Add(Label_CategorDetailsNewCategor)
            FlowLayoutPanel_CategorDetailsNewCategor.Controls.Add(FlowLayoutPanel_CategorDetailCategorNameInputNewCategor)
            FlowLayoutPanel_CategorDetailsNewCategor.Controls.Add(Button_MakeCategorNewSale)
            FlowLayoutPanel_CategorDetailsNewCategor.Location = New System.Drawing.Point(0, 0)
            FlowLayoutPanel_CategorDetailsNewCategor.Margin = New System.Windows.Forms.Padding(0)
            FlowLayoutPanel_CategorDetailsNewCategor.Name = "FlowLayoutPanel_CategorDetailsNewCategor"
            FlowLayoutPanel_CategorDetailsNewCategor.Size = New System.Drawing.Size(366, 350)
            FlowLayoutPanel_CategorDetailsNewCategor.TabIndex = 35
            '
            'FlowLayoutPanel_PageHeaderNewCategor
            '
            FlowLayoutPanel_PageHeaderNewCategor.AutoSize = true
            FlowLayoutPanel_PageHeaderNewCategor.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            FlowLayoutPanel_PageHeaderNewCategor.Controls.Add(Panel_PageHeaderIconNewCategor)
            FlowLayoutPanel_PageHeaderNewCategor.Controls.Add(Label_PageHeaderNewCategor)
            FlowLayoutPanel_PageHeaderNewCategor.Location = New System.Drawing.Point(12, 24)
            FlowLayoutPanel_PageHeaderNewCategor.Margin = New System.Windows.Forms.Padding(12, 24, 0, 24)
            FlowLayoutPanel_PageHeaderNewCategor.Name = "FlowLayoutPanel_PageHeaderNewCategor"
            FlowLayoutPanel_PageHeaderNewCategor.Size = New System.Drawing.Size(226, 32)
            FlowLayoutPanel_PageHeaderNewCategor.TabIndex = 0
            '
            'Panel_PageHeaderIconNewCategor
            '
            Panel_PageHeaderIconNewCategor.Anchor = System.Windows.Forms.AnchorStyles.Left
            Panel_PageHeaderIconNewCategor.BackgroundImage = Global.ERP_SIMS.My.Resources.Resources.HeaderIcon_Categories
            Panel_PageHeaderIconNewCategor.Location = New System.Drawing.Point(0, 0)
            Panel_PageHeaderIconNewCategor.Margin = New System.Windows.Forms.Padding(0, 0, 16, 0)
            Panel_PageHeaderIconNewCategor.Name = "Panel_PageHeaderIconNewCategor"
            Panel_PageHeaderIconNewCategor.Size = New System.Drawing.Size(32, 32)
            Panel_PageHeaderIconNewCategor.TabIndex = 3
            '
            'Label_PageHeaderNewCategor
            '
            Label_PageHeaderNewCategor.Anchor = System.Windows.Forms.AnchorStyles.Left
            Label_PageHeaderNewCategor.AutoSize = true
            Label_PageHeaderNewCategor.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
            Label_PageHeaderNewCategor.Font = New System.Drawing.Font("Microsoft JhengHei", 24!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
            Label_PageHeaderNewCategor.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
            Label_PageHeaderNewCategor.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Label_PageHeaderNewCategor.Location = New System.Drawing.Point(48, 0)
            Label_PageHeaderNewCategor.Margin = New System.Windows.Forms.Padding(0)
            Label_PageHeaderNewCategor.Name = "Label_PageHeaderNewCategor"
            Label_PageHeaderNewCategor.Size = New System.Drawing.Size(178, 31)
            Label_PageHeaderNewCategor.TabIndex = 2
            Label_PageHeaderNewCategor.Text = "New Category"
            Label_PageHeaderNewCategor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'Label_CategorDetailsNewCategor
            '
            Label_CategorDetailsNewCategor.Anchor = System.Windows.Forms.AnchorStyles.Left
            Label_CategorDetailsNewCategor.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
            Label_CategorDetailsNewCategor.Font = New System.Drawing.Font("Microsoft JhengHei", 22!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
            Label_CategorDetailsNewCategor.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
            Label_CategorDetailsNewCategor.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Label_CategorDetailsNewCategor.Location = New System.Drawing.Point(12, 80)
            Label_CategorDetailsNewCategor.Margin = New System.Windows.Forms.Padding(12, 0, 12, 24)
            Label_CategorDetailsNewCategor.Name = "Label_CategorDetailsNewCategor"
            Label_CategorDetailsNewCategor.Size = New System.Drawing.Size(342, 28)
            Label_CategorDetailsNewCategor.TabIndex = 18
            Label_CategorDetailsNewCategor.Text = "Category Details"
            Label_CategorDetailsNewCategor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'FlowLayoutPanel_CategorDetailCategorNameInputNewCategor
            '
            FlowLayoutPanel_CategorDetailCategorNameInputNewCategor.AutoSize = true
            FlowLayoutPanel_CategorDetailCategorNameInputNewCategor.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            FlowLayoutPanel_CategorDetailCategorNameInputNewCategor.Controls.Add(Label_CategorDetailCategorNameInputNewCategor)
            FlowLayoutPanel_CategorDetailCategorNameInputNewCategor.Controls.Add(Textbox_CategorDetailCategorNameInputNewCategor)
            FlowLayoutPanel_CategorDetailCategorNameInputNewCategor.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
            FlowLayoutPanel_CategorDetailCategorNameInputNewCategor.Location = New System.Drawing.Point(12, 132)
            FlowLayoutPanel_CategorDetailCategorNameInputNewCategor.Margin = New System.Windows.Forms.Padding(12, 0, 6, 0)
            FlowLayoutPanel_CategorDetailCategorNameInputNewCategor.Name = "FlowLayoutPanel_CategorDetailCategorNameInputNewCategor"
            FlowLayoutPanel_CategorDetailCategorNameInputNewCategor.Size = New System.Drawing.Size(342, 56)
            FlowLayoutPanel_CategorDetailCategorNameInputNewCategor.TabIndex = 19
            '
            'Label_CategorDetailCategorNameInputNewCategor
            '
            Label_CategorDetailCategorNameInputNewCategor.AutoSize = true
            Label_CategorDetailCategorNameInputNewCategor.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
            Label_CategorDetailCategorNameInputNewCategor.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
            Label_CategorDetailCategorNameInputNewCategor.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
            Label_CategorDetailCategorNameInputNewCategor.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Label_CategorDetailCategorNameInputNewCategor.Location = New System.Drawing.Point(0, 0)
            Label_CategorDetailCategorNameInputNewCategor.Margin = New System.Windows.Forms.Padding(0)
            Label_CategorDetailCategorNameInputNewCategor.Name = "Label_CategorDetailCategorNameInputNewCategor"
            Label_CategorDetailCategorNameInputNewCategor.Size = New System.Drawing.Size(97, 16)
            Label_CategorDetailCategorNameInputNewCategor.TabIndex = 2
            Label_CategorDetailCategorNameInputNewCategor.Text = "Category Name"
            Label_CategorDetailCategorNameInputNewCategor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'Textbox_CategorDetailCategorNameInputNewCategor
            '
            Textbox_CategorDetailCategorNameInputNewCategor.Anchor = System.Windows.Forms.AnchorStyles.Left
            Textbox_CategorDetailCategorNameInputNewCategor.AutoCompleteCustomSource.AddRange(New String() {"(001) Item 1", "(002) Item 2", "(003) Item 3", "(004) Item 4", "(005) Item 5", "(006) Item 6", "(007) Item 7", "(008) Item 8", "(009) Item 9", "(010) Item 10"})
            Textbox_CategorDetailCategorNameInputNewCategor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
            Textbox_CategorDetailCategorNameInputNewCategor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
            Textbox_CategorDetailCategorNameInputNewCategor.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
            Textbox_CategorDetailCategorNameInputNewCategor.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
            Textbox_CategorDetailCategorNameInputNewCategor.Cursor = System.Windows.Forms.Cursors.IBeam
            Textbox_CategorDetailCategorNameInputNewCategor.DefaultText = ""
            Textbox_CategorDetailCategorNameInputNewCategor.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer))
            Textbox_CategorDetailCategorNameInputNewCategor.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer))
            Textbox_CategorDetailCategorNameInputNewCategor.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
            Textbox_CategorDetailCategorNameInputNewCategor.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
            Textbox_CategorDetailCategorNameInputNewCategor.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
            Textbox_CategorDetailCategorNameInputNewCategor.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
            Textbox_CategorDetailCategorNameInputNewCategor.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
            Textbox_CategorDetailCategorNameInputNewCategor.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
            Textbox_CategorDetailCategorNameInputNewCategor.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
            Textbox_CategorDetailCategorNameInputNewCategor.Location = New System.Drawing.Point(0, 16)
            Textbox_CategorDetailCategorNameInputNewCategor.Margin = New System.Windows.Forms.Padding(0)
            Textbox_CategorDetailCategorNameInputNewCategor.Name = "Textbox_CategorDetailCategorNameInputNewCategor"
            Textbox_CategorDetailCategorNameInputNewCategor.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
            Textbox_CategorDetailCategorNameInputNewCategor.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer))
            Textbox_CategorDetailCategorNameInputNewCategor.PlaceholderText = "Required"
            Textbox_CategorDetailCategorNameInputNewCategor.SelectedText = ""
            Textbox_CategorDetailCategorNameInputNewCategor.Size = New System.Drawing.Size(342, 40)
            Textbox_CategorDetailCategorNameInputNewCategor.TabIndex = 39
            '
            'Button_MakeCategorNewSale
            '
            Button_MakeCategorNewSale.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(44,Byte),Integer), CType(CType(62,Byte),Integer))
            Button_MakeCategorNewSale.DisabledState.BorderColor = System.Drawing.Color.DarkGray
            Button_MakeCategorNewSale.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
            Button_MakeCategorNewSale.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer))
            Button_MakeCategorNewSale.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer))
            Button_MakeCategorNewSale.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
            Button_MakeCategorNewSale.Font = New System.Drawing.Font("Segoe UI", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel)
            Button_MakeCategorNewSale.ForeColor = System.Drawing.Color.White
            Button_MakeCategorNewSale.Location = New System.Drawing.Point(12, 212)
            Button_MakeCategorNewSale.Margin = New System.Windows.Forms.Padding(12, 24, 12, 12)
            Button_MakeCategorNewSale.Name = "Button_MakeCategorNewSale"
            Button_MakeCategorNewSale.Size = New System.Drawing.Size(342, 40)
            Button_MakeCategorNewSale.TabIndex = 33
            Button_MakeCategorNewSale.Text = "Create Category"
            Button_MakeCategorNewSale.TextOffset = New System.Drawing.Point(0, -2)
            '
            '_Temporary_NewCategory
            '
            Prompt_NewCategory.ClientSize = New System.Drawing.Size(366, 350)
            Prompt_NewCategory.Controls.Add(FlowLayoutPanel_CategorDetailsNewCategor)
            Prompt_NewCategory.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            FlowLayoutPanel_CategorDetailsNewCategor.ResumeLayout(false)
            FlowLayoutPanel_CategorDetailsNewCategor.PerformLayout
            FlowLayoutPanel_PageHeaderNewCategor.ResumeLayout(false)
            FlowLayoutPanel_PageHeaderNewCategor.PerformLayout
            FlowLayoutPanel_CategorDetailCategorNameInputNewCategor.ResumeLayout(false)
            FlowLayoutPanel_CategorDetailCategorNameInputNewCategor.PerformLayout
            ResumeLayout(false)

            AddHandler Button_MakeCategorNewSale.Click, Sub(senderr As Object, ee As EventArgs)
                Dim categoryName As String = Textbox_CategorDetailCategorNameInputNewCategor.Text.Trim()
                Dim isValid As Boolean = True

                If String.IsNullOrEmpty(categoryName) Then
                    Textbox_CategorDetailCategorNameInputNewCategor.BorderColor = Color.Red
                    isValid = False
                Else
                    Textbox_CategorDetailCategorNameInputNewCategor.BorderColor = Color.Black
                End If

                If Not isValid Then
                    MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If

                Try
                    Using conn As New MySqlConnection(ConnectionString)
                        conn.Open()
                        Dim query As String = "INSERT INTO categories (category_name, created_by) VALUES (@category_name, @created_by)"
                        Using cmd As New MySqlCommand(query, conn)
                            cmd.Parameters.AddWithValue("@category_name", categoryName)
                            cmd.Parameters.AddWithValue("@created_by", GlobalShared.CurrentUser.AccountId)
                            cmd.ExecuteNonQuery()
                        End Using
                    End Using
                    MessageBox.Show("Category successfully added!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Categories = CreateCategoryArray()
                    PopulateCategoryComboBox(Combobox_ItemDetailCategoryInputNewItem)
                    Prompt_NewCategory.Close()
                Catch ex As Exception
                    MessageBox.Show($"Error adding category: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End Sub
            
            Prompt_NewCategory.ShowDialog(Me)
        End Sub

        AddHandler Button_MakeItemNewItem.Click, Sub(sender As Object, e As EventArgs)
            Dim isValid As Boolean = True
            Dim itemName As String = Textbox_ItemDetailItemNameInputNewItem.Text.Trim()
            Dim sellingPrice As Decimal
            Dim brandId As Integer = -1
            Dim categoryId As Integer = -1

            If String.IsNullOrEmpty(itemName) Then
                Textbox_ItemDetailItemNameInputNewItem.BorderColor = Color.Red
                isValid = False
            End If

            If Not Decimal.TryParse(Textbox_ItemDetailSellingPriceInputNewItem.Text.Trim(), sellingPrice) OrElse sellingPrice <= 0 Then
                Textbox_ItemDetailSellingPriceInputNewItem.BorderColor = Color.Red
                isValid = False
            End If

            If ComboBox_ItemDetailBrandInputNewItem.SelectedItem IsNot Nothing Then
                Dim selectedBrandName As String = ComboBox_ItemDetailBrandInputNewItem.SelectedItem.ToString()
                Dim selectedBrand As Brand = Brands.FirstOrDefault(Function(b) b.BrandName = selectedBrandName)
                If selectedBrand IsNot Nothing Then
                    brandId = selectedBrand.BrandId
                Else
                    ComboBox_ItemDetailBrandInputNewItem.BorderColor = Color.Red
                    isValid = False
                End If
            Else
                ComboBox_ItemDetailBrandInputNewItem.BorderColor = Color.Red
                isValid = False
            End If

            If ComboBox_ItemDetailCategoryInputNewItem.SelectedItem IsNot Nothing Then
                Dim selectedCategoryName As String = ComboBox_ItemDetailCategoryInputNewItem.SelectedItem.ToString()
                Dim selectedCategory As Category = Categories.FirstOrDefault(Function(c) c.CategoryName = selectedCategoryName)
                If selectedCategory IsNot Nothing Then
                    categoryId = selectedCategory.CategoryId
                Else
                    ComboBox_ItemDetailCategoryInputNewItem.BorderColor = Color.Red
                    isValid = False
                End If
            Else
                ComboBox_ItemDetailCategoryInputNewItem.BorderColor = Color.Red
                isValid = False
            End If

            If isValid Then
                Try
                    Using conn As New MySqlConnection(ConnectionString)
                        conn.Open()
                        Dim query As String = "INSERT INTO items (item_name, available_quantity, unavailable_quantity, selling_price, brand_id, category_id) " &
                                              "VALUES (@item_name, @available_quantity, @unavailable_quantity, @selling_price, @brand_id, @category_id)"
                        Using cmd As New MySqlCommand(query, conn)
                            cmd.Parameters.AddWithValue("@item_name", itemName)
                            cmd.Parameters.AddWithValue("@available_quantity", 0)
                            cmd.Parameters.AddWithValue("@unavailable_quantity", 0)
                            cmd.Parameters.AddWithValue("@selling_price", sellingPrice)
                            cmd.Parameters.AddWithValue("@brand_id", If(brandId = -1, DBNull.Value, brandId))
                            cmd.Parameters.AddWithValue("@category_id", If(categoryId = -1, DBNull.Value, categoryId))
                            cmd.ExecuteNonQuery()
                        End Using
                    End Using
                    MessageBox.Show("Item successfully added!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Catch ex As Exception
                    MessageBox.Show($"Error adding item: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            Else
                MessageBox.Show("Please correct the highlighted fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        End Sub

        AddHandler Button_ResetNewItem.Click, Sub(sender As Object, e As EventArgs)
            Textbox_ItemDetailItemNameInputNewItem.Text = ""
            Textbox_ItemDetailSellingPriceInputNewItem.Text = ""
            ComboBox_ItemDetailBrandInputNewItem.SelectedIndex = -1
            ComboBox_ItemDetailBrandInputNewItem.Text = ""
            Combobox_ItemDetailCategoryInputNewItem.SelectedIndex = -1
            Combobox_ItemDetailCategoryInputNewItem.Text = ""

            Textbox_ItemDetailItemNameInputNewItem.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
            Textbox_ItemDetailSellingPriceInputNewItem.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
            ComboBox_ItemDetailBrandInputNewItem.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
            Combobox_ItemDetailCategoryInputNewItem.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        End Sub

        PopulateBrandComboBox(ComboBox_ItemDetailBrandInputNewItem)
        PopulateCategoryComboBox(ComboBox_ItemDetailCategoryInputNewItem)

        Prompt_NewItem.ShowDialog(Me)
    End Sub



    ' Populate combo box
    Public Sub PopulateBrandComboBox(comboBox As Guna.UI2.WinForms.Guna2ComboBox)
        comboBox.Items.Clear()

        For Each brand In Brands
            comboBox.Items.Add(brand.BrandName)
        Next

        If comboBox.Items.Count > 0 Then
            comboBox.SelectedIndex = 0
        End If
    End Sub

    Public Sub PopulateCategoryComboBox(comboBox As Guna.UI2.WinForms.Guna2ComboBox)
        comboBox.Items.Clear()

        For Each category In Categories
            comboBox.Items.Add(category.CategoryName)
        Next
        
        If comboBox.Items.Count > 0 Then
            comboBox.SelectedIndex = 0
        End If
    End Sub



    ' Update stocks table
    Public Sub UpdateStocksListTable()
        FlowLayoutPanel_StocksListTable.Controls.Clear()

        StocksListTotalPages = Math.Ceiling(Stocks.Count / StocksListItemsPerPage)
        Label_StocksListTablePageControlPageNumber.Text = $"{StocksListCurrentPage}/{StocksListTotalPages}"

        Dim TableLayoutPanel_StocksList As New TableLayoutPanel With {
            .AutoSize = True,
            .AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink,
            .BackColor = System.Drawing.Color.FromArgb(CType(255, Byte), CType(255, Byte), CType(255, Byte)),
            .CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single],
            .Margin = New System.Windows.Forms.Padding(0, 0, 0, 0),
            .MaximumSize = New System.Drawing.Size(1102, 0),
            .MinimumSize = New System.Drawing.Size(1102, 0)
        }

        TableLayoutPanel_StocksList.ColumnCount = 8
        For i = 1 To 8
            TableLayoutPanel_StocksList.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0! / 8))
        Next

        Dim StartIndex As Integer = (StocksListCurrentPage - 1) * StocksListItemsPerPage
        Dim EndIndex As Integer = Math.Min(StartIndex + StocksListItemsPerPage, Stocks.Count)

        For i = StartIndex To EndIndex - 1
            Dim stock As Stock = Stocks(i)
            TableLayoutPanel_StocksList.RowCount += 1

            Dim StockItem = Items.FirstOrDefault(Function(item) item.ItemId = stock.ItemId)

            Dim labels As New List(Of Label)
            labels.Add(CreateLabel("Stock " & stock.StockId.ToString(), 1))
            labels.Add(CreateLabel("Outvoice " & stock.StockEntryId, 2, True))
            labels.Add(CreateLabel("x" & stock.StockQuantity, 1))
            labels.Add(CreateLabel("P" & stock.ItemBuyingPrice, 1))
            labels.Add(CreateLabel("P" & stock.TotalBuyingPrice, 1))
            labels.Add(CreateLabel(stock.ItemId & " - " & StockItem.ItemName, 2, True))
            labels.Add(CreateLabel(If(stock.Expiry Is Nothing, "No Expiry", stock.Expiry.Value.ToShortDateString()), 1))
            labels.Add(CreateLabel(stock.Status, 1))

            For col As Integer = 0 To labels.Count - 1
                TableLayoutPanel_StocksList.Controls.Add(labels(col), col, TableLayoutPanel_StocksList.RowCount - 1)
            Next

            Dim maxLabelHeight As Integer = labels.Max(Function(lbl) lbl.Height)

            TableLayoutPanel_StocksList.RowStyles.Add(New RowStyle(SizeType.Absolute, maxLabelHeight + 12))
        Next

        FlowLayoutPanel_StocksListTable.Controls.Add(TableLayoutPanel_StocksList)
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


    ' Stock entries table
    Public StockEntriesListCurrentPage As Integer = 1
    Public StockEntriesListItemsPerPage As Integer = 10
    Public StockEntriesListTotalPages As Integer

   
    Public Sub UpdateStockEntriesListTable()
        FlowLayoutPanel_StockEntriesListTable.Controls.Clear()

        StockEntriesListTotalPages = Math.Ceiling(Stocks.Count / StockEntriesListItemsPerPage)
        Label_StockEntriesListTablePageControlPageNumber.Text = $"{StockEntriesListCurrentPage}/{StockEntriesListTotalPages}"

        Dim TableLayoutPanel_StocksList As New TableLayoutPanel With {
            .AutoSize = True,
            .AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink,
            .BackColor = System.Drawing.Color.FromArgb(CType(255, Byte), CType(255, Byte), CType(255, Byte)),
            .CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single],
            .Margin = New System.Windows.Forms.Padding(0, 0, 0, 0),
            .MaximumSize = New System.Drawing.Size(1102, 0),
            .MinimumSize = New System.Drawing.Size(1102, 0)
        }

        TableLayoutPanel_StocksList.ColumnCount = 6
        For i = 1 To 6
            TableLayoutPanel_StocksList.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0! / 6))
        Next

        Dim StartIndex As Integer = (StockEntriesListCurrentPage - 1) * StockEntriesListItemsPerPage
        Dim EndIndex As Integer = Math.Min(StartIndex + StockEntriesListItemsPerPage, Stocks.Count)

        For i = StartIndex To EndIndex - 1
            Dim stockEntry As StockEntry = StockEntries(i)
            TableLayoutPanel_StocksList.RowCount += 1

            Dim StockItem = StockEntries.FirstOrDefault(Function(item) item.StockEntryId = stockEntry.StockEntryId)

            Dim labels As New List(Of Label)
            labels.Add(CreateLabel("Outvoice " & stockEntry.StockEntryId.ToString(), 1))
            labels.Add(CreateLabel("Vendor " & stockEntry.VendorId, 2, True))
            labels.Add(CreateLabel("P" & stockEntry.NetTotal, 2, True))
            labels.Add(CreateLabel(stockEntry.Status, 1))
            labels.Add(CreateLabel(stockEntry.CreatedBy, 1))
            labels.Add(CreateLabel(stockEntry.CreatedAt, 1))
            

            For col As Integer = 0 To labels.Count - 1
                TableLayoutPanel_StocksList.Controls.Add(labels(col), col, TableLayoutPanel_StocksList.RowCount - 1)
            Next

            Dim maxLabelHeight As Integer = labels.Max(Function(lbl) lbl.Height)

            TableLayoutPanel_StocksList.RowStyles.Add(New RowStyle(SizeType.Absolute, maxLabelHeight + 12))
        Next

        FlowLayoutPanel_StockEntriesListTable.Controls.Add(TableLayoutPanel_StocksList)
    End Sub



    Private Sub Button_NewStock_Click(sender As Object, e As EventArgs) Handles Button_NewStock.Click
        ShowNewItemPrompt()
    End Sub

    Private Sub Window_Stocks_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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
        
        Label_UserName.Text = GlobalShared.CurrentUser.Name
        Label_UserEmail.Text = GlobalShared.CurrentUser.Email
        UpdateStocksListTable()
        UpdateStockEntriesListTable()
    End Sub

    Private Sub Button_Restock_Click(sender As Object, e As EventArgs) Handles Button_Restock.Click
        Dim Receiving As New Window_AddStock()
        Receiving.Show()
        Me.Close()
    End Sub

    Private Sub Button_Logout_Click(sender As Object, e As EventArgs) Handles Button_Logout.Click
        Dim result As DialogResult = MessageBox.Show("Are you sure you want to log out?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            Dim newForm As New Window_Login()
            newForm.Show()
            Me.Close()
        End If
    End Sub

    Private Sub Button_StocksListTablePageControlPrevious_Click_1(sender As Object, e As EventArgs) Handles Button_StocksListTablePageControlPrevious.Click
        If StocksListCurrentPage > 1 Then
            StocksListCurrentPage -= 1
            UpdateStocksListTable()
        End If
    End Sub

    Private Sub Button_StocksListTablePageControlNext_Click_1(sender As Object, e As EventArgs) Handles Button_StocksListTablePageControlNext.Click
        If StocksListCurrentPage < StocksListTotalPages Then
            StocksListCurrentPage += 1
            UpdateStocksListTable()
        End If
    End Sub

    Private Sub Button_SearchStocks_Click(sender As Object, e As EventArgs) Handles Button_SearchStocks.Click
        Dim searchText As String = Textbox_SearchBarStocks.Text.ToLower().Trim()

        Dim filteredStocks As IEnumerable(Of Stock) = Stocks.Where(Function(stock)
        Dim item As Item = Items.FirstOrDefault(Function(i) i.ItemId = stock.ItemId)

        ' Check if the item's name or the creator's name matches the search text
        Return (item IsNot Nothing AndAlso item.ItemName.ToLower().Contains(searchText))
    End Function)

    FlowLayoutPanel_StocksListTable.Controls.Clear()

    Dim totalFilteredStocks As Integer = filteredStocks.Count()
    StocksListTotalPages = Math.Ceiling(totalFilteredStocks / StocksListItemsPerPage)
    Label_StocksListTablePageControlPageNumber.Text = $"{StocksListCurrentPage}/{StocksListTotalPages}"

    Dim TableLayoutPanel_StocksList As New TableLayoutPanel With {
        .AutoSize = True,
        .AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink,
        .BackColor = System.Drawing.Color.FromArgb(CType(255, Byte), CType(255, Byte), CType(255, Byte)),
        .CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single],
        .Margin = New System.Windows.Forms.Padding(0, 0, 0, 0),
        .MaximumSize = New System.Drawing.Size(1102, 0),
        .MinimumSize = New System.Drawing.Size(1102, 0)
    }

    TableLayoutPanel_StocksList.ColumnCount = 8
    For i = 1 To 8
        TableLayoutPanel_StocksList.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0! / 8))
    Next

    Dim StartIndex As Integer = (StocksListCurrentPage - 1) * StocksListItemsPerPage
    Dim EndIndex As Integer = Math.Min(StartIndex + StocksListItemsPerPage, totalFilteredStocks)

    Dim paginatedStocks As IEnumerable(Of Stock) = filteredStocks.Skip(StartIndex).Take(StocksListItemsPerPage)

    For Each stock As Stock In paginatedStocks
        TableLayoutPanel_StocksList.RowCount += 1

        Dim item As Item = Items.FirstOrDefault(Function(i) i.ItemId = stock.ItemId)
        Dim createdByAccount As Account = Accounts.FirstOrDefault(Function(a) a.AccountId = stock.StockEntryId)

        Dim labels As New List(Of Label)
        labels.Add(CreateLabel("Stock " & stock.StockId.ToString(), 1))
        labels.Add(CreateLabel("Outvoice " & stock.StockEntryId, 2, True))
        labels.Add(CreateLabel("x" & stock.StockQuantity, 1))
        labels.Add(CreateLabel("P" & stock.ItemBuyingPrice, 1))
        labels.Add(CreateLabel("P" & stock.TotalBuyingPrice, 1))
        labels.Add(CreateLabel(stock.ItemId & " - " & If(item IsNot Nothing, item.ItemName, "Unknown Item"), 2, True))
        labels.Add(CreateLabel(If(stock.Expiry Is Nothing, "No Expiry", stock.Expiry.Value.ToShortDateString()), 1))
        labels.Add(CreateLabel(stock.Status, 1))

        For col As Integer = 0 To labels.Count - 1
            TableLayoutPanel_StocksList.Controls.Add(labels(col), col, TableLayoutPanel_StocksList.RowCount - 1)
        Next

        Dim maxLabelHeight As Integer = labels.Max(Function(lbl) lbl.Height)

        TableLayoutPanel_StocksList.RowStyles.Add(New RowStyle(SizeType.Absolute, maxLabelHeight + 12))
    Next

    FlowLayoutPanel_StocksListTable.Controls.Add(TableLayoutPanel_StocksList)

    End Sub



    Private Sub ShowOutvoiceReceiptPrompt(stockEntryId As Integer)
    Dim Prompt_OutvoiceReceipt As New Form()

    Dim FlowLayoutPanel_OutvoiceReceipt = New System.Windows.Forms.FlowLayoutPanel()
    Dim FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
    Dim Label1 = New System.Windows.Forms.Label()
    Dim Label2 = New System.Windows.Forms.Label()
    Dim Label3 = New System.Windows.Forms.Label()
    Dim Label4 = New System.Windows.Forms.Label()
    Dim Label5 = New System.Windows.Forms.Label()
    Dim Label7 = New System.Windows.Forms.Label()
    Dim Label8 = New System.Windows.Forms.Label()
    FlowLayoutPanel_OutvoiceReceipt.SuspendLayout
    FlowLayoutPanel1.SuspendLayout
    SuspendLayout

    FlowLayoutPanel_OutvoiceReceipt.Controls.Add(FlowLayoutPanel1)
    FlowLayoutPanel_OutvoiceReceipt.Location = New System.Drawing.Point(0, 0)
    FlowLayoutPanel_OutvoiceReceipt.Margin = New System.Windows.Forms.Padding(0)
    FlowLayoutPanel_OutvoiceReceipt.Name = "FlowLayoutPanel_OutvoiceReceipt"
    FlowLayoutPanel_OutvoiceReceipt.Size = New System.Drawing.Size(556, 720)
    FlowLayoutPanel_OutvoiceReceipt.TabIndex = 40
    
    FlowLayoutPanel1.Controls.Add(Label1)
    FlowLayoutPanel1.Controls.Add(Label2)
    FlowLayoutPanel1.Controls.Add(Label3)
    FlowLayoutPanel1.Controls.Add(Label4)
    FlowLayoutPanel1.Controls.Add(Label5)
    FlowLayoutPanel1.Controls.Add(Label7)
    FlowLayoutPanel1.Controls.Add(Label8)
    FlowLayoutPanel1.Location = New System.Drawing.Point(0, 0)
    FlowLayoutPanel1.Margin = New System.Windows.Forms.Padding(0)
    FlowLayoutPanel1.Name = "FlowLayoutPanel1"
    FlowLayoutPanel1.Size = New System.Drawing.Size(556, 720)
    FlowLayoutPanel1.TabIndex = 42
    
    Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
    Label1.AutoSize = true
    Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
    Label1.Font = New System.Drawing.Font("Cascadia Mono", 16!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
    Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
    Label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
    Label1.Location = New System.Drawing.Point(12, 24)
    Label1.Margin = New System.Windows.Forms.Padding(12, 24, 6, 12)
    Label1.MinimumSize = New System.Drawing.Size(532, 0)
    Label1.Name = "Label1"
    Label1.Size = New System.Drawing.Size(532, 42)
    Label1.TabIndex = 42
    Label1.Text = "Ideal Marketing and Manufacturing Corporation"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"123 Business Blvd, Metro City, Cou"& _ 
"ntry X"
    Label1.TextAlign = System.Drawing.ContentAlignment.TopCenter
    
    Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
    Label2.AutoSize = true
    Label2.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
    Label2.Font = New System.Drawing.Font("Cascadia Mono", 16!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
    Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
    Label2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
    Label2.Location = New System.Drawing.Point(12, 78)
    Label2.Margin = New System.Windows.Forms.Padding(12, 0, 6, 12)
    Label2.MinimumSize = New System.Drawing.Size(532, 0)
    Label2.Name = "Label2"
    Label2.Size = New System.Drawing.Size(532, 21)
    Label2.TabIndex = 43
    Label2.Text = "=========================================================="
    Label2.TextAlign = System.Drawing.ContentAlignment.TopCenter
    
    Label3.Anchor = System.Windows.Forms.AnchorStyles.Left
    Label3.AutoSize = true
    Label3.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
    Label3.Font = New System.Drawing.Font("Cascadia Mono", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
    Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
    Label3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
    Label3.Location = New System.Drawing.Point(12, 123)
    Label3.Margin = New System.Windows.Forms.Padding(12, 12, 6, 12)
    Label3.MinimumSize = New System.Drawing.Size(532, 0)
    Label3.Name = "Label3"
    Label3.Size = New System.Drawing.Size(532, 72)
    Label3.TabIndex = 44
    
    Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
    Label4.AutoSize = true
    Label4.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
    Label4.Font = New System.Drawing.Font("Cascadia Mono", 16!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
    Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
    Label4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
    Label4.Location = New System.Drawing.Point(12, 207)
    Label4.Margin = New System.Windows.Forms.Padding(12, 0, 6, 12)
    Label4.MinimumSize = New System.Drawing.Size(532, 0)
    Label4.Name = "Label4"
    Label4.Size = New System.Drawing.Size(532, 21)
    Label4.TabIndex = 45
    Label4.Text = "=========================================================="
    Label4.TextAlign = System.Drawing.ContentAlignment.TopCenter
    
    Label5.Anchor = System.Windows.Forms.AnchorStyles.Left
    Label5.AutoSize = true
    Label5.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
    Label5.Font = New System.Drawing.Font("Cascadia Mono", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
    Label5.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
    Label5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
    Label5.Location = New System.Drawing.Point(12, 240)
    Label5.Margin = New System.Windows.Forms.Padding(12, 0, 6, 12)
    Label5.MinimumSize = New System.Drawing.Size(532, 0)
    Label5.Name = "Label5"
    Label5.Size = New System.Drawing.Size(532, 90)
    Label5.TabIndex = 41
    
    Label7.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
    Label7.AutoSize = true
    Label7.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
    Label7.Font = New System.Drawing.Font("Cascadia Mono", 16!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
    Label7.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
    Label7.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
    Label7.Location = New System.Drawing.Point(12, 342)
    Label7.Margin = New System.Windows.Forms.Padding(12, 0, 6, 12)
    Label7.MinimumSize = New System.Drawing.Size(532, 0)
    Label7.Name = "Label7"
    Label7.Size = New System.Drawing.Size(532, 21)
    Label7.TabIndex = 48
    Label7.Text = "=========================================================="
    Label7.TextAlign = System.Drawing.ContentAlignment.TopCenter
    
    Label8.Anchor = System.Windows.Forms.AnchorStyles.Left
    Label8.AutoSize = true
    Label8.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
    Label8.Font = New System.Drawing.Font("Cascadia Mono", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
    Label8.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
    Label8.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
    Label8.Location = New System.Drawing.Point(12, 375)
    Label8.Margin = New System.Windows.Forms.Padding(12, 0, 6, 12)
    Label8.MinimumSize = New System.Drawing.Size(532, 0)
    Label8.Name = "Label8"
    Label8.Size = New System.Drawing.Size(532, 18)
    Label8.TabIndex = 49
    
    Prompt_OutvoiceReceipt.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
    Prompt_OutvoiceReceipt.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Prompt_OutvoiceReceipt.ClientSize = New System.Drawing.Size(556, 720)
    Prompt_OutvoiceReceipt.Controls.Add(FlowLayoutPanel_OutvoiceReceipt)
    Prompt_OutvoiceReceipt.Name = "Prompt_OutvoiceReceipt"
    Prompt_OutvoiceReceipt.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
    Prompt_OutvoiceReceipt.Text = "Prompt_OutvoiceReceipt"
    FlowLayoutPanel_OutvoiceReceipt.ResumeLayout(false)
    FlowLayoutPanel1.ResumeLayout(false)
    FlowLayoutPanel1.PerformLayout
    ResumeLayout(false)

    Dim output As New StringBuilder()
    output.AppendLine("Qty                   Item Name                       Amount")
    output.AppendLine("-----------------------------------------------------------------")

    Dim Transaction_Entry As StockEntry = Nothing

    Dim query = "SELECT * FROM stock_entries WHERE stock_entry_id = @StockEntryId"

    Using connection As New MySqlConnection(ConnectionString)
        Using command As New MySqlCommand(query, connection)
            command.Parameters.AddWithValue("@StockEntryId", stockEntryId)

            Try
                connection.Open()
                Using reader As MySqlDataReader = command.ExecuteReader()
                    If reader.HasRows Then
                        While reader.Read()
                            Transaction_Entry = New StockEntry(
                                reader.GetInt32("stock_entry_id"),
                                If(IsDBNull(reader("vendor_id")), 0, reader.GetInt32("vendor_id")),
                                reader.GetString("status"),
                                If(IsDBNull(reader("net_total")), 0D, reader.GetDouble("net_total")),
                                reader.GetDateTime("created_at"),
                                If(IsDBNull(reader("created_by")), 0, reader.GetInt32("created_by"))
                            )
                        End While
                    End If
                End Using
            Catch ex As Exception
                MessageBox.Show("An error occurred while fetching the stock entry: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Using

    Dim CurrentTransactionItems As New List(Of Stock)()
    query = "SELECT * FROM stocks WHERE stock_id IN (SELECT stock_id FROM transaction_items WHERE transaction_id = @TransactionId)"

    Using connection As New MySqlConnection(ConnectionString)
        Using command As New MySqlCommand(query, connection)
            command.Parameters.AddWithValue("@TransactionId", stockEntryId)

            Try
                connection.Open()
                Using reader As MySqlDataReader = command.ExecuteReader()
                    While reader.Read()
                        Dim stock As New Stock(
                            reader.GetInt32("stock_id"),
                            If(IsDBNull(reader("stock_entry_id")), 0, reader.GetInt32("stock_entry_id")),
                            reader.GetInt32("stock_quantity"),
                            reader.GetDecimal("item_buying_price"),
                            reader.GetDecimal("total_buying_price"),
                            If(IsDBNull(reader("item_id")), 0, reader.GetInt32("item_id")),
                            If(IsDBNull(reader("expiry")), CType(Nothing, DateTime?), reader.GetDateTime("expiry")),
                            reader.GetString("status")
                        )
                        CurrentTransactionItems.Add(stock)
                    End While
                End Using
            Catch ex As Exception
                MessageBox.Show("An error occurred while fetching stocks: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Using


    Dim TransactionVendor As New Vendor
    Dim TransactionAccount As New Account

    For Each vendor In Vendors
        If vendor.VendorId = Transaction_Entry.VendorId Then
            TransactionVendor = vendor
        End If
    Next

    For Each stock In CurrentTransactionItems
        Dim item = Items.FirstOrDefault(Function(i) i.ItemId = stock.ItemId)
        Dim itemName As String = If(item IsNot Nothing, item.ItemName, "Unknown Item")

        Dim qty As String = stock.StockQuantity.ToString().PadRight(20)
        Dim name As String = itemName.PadRight(30)
        Dim amount As String = stock.TotalBuyingPrice.ToString("C").PadLeft(10)

        output.AppendLine($"{qty}{name}{amount}")
    Next
    
    For Each account In Accounts
        If account.AccountId = TransactionVendor.CreatedBy Then
            TransactionAccount = account
        End If
    Next

    Dim SelectedVendor = Vendors.FirstOrDefault(Function(v) v.VendorId = Transaction_Entry.VendorId)
    Dim CUrrentTransactionEntry = Transaction_Entry

    Label3.Text = "Outvoice ID: " & CurrentTransactionEntry.StockEntryId & Environment.NewLine & _
        "Vendor: " & CurrentTransactionEntry.VendorId & " - "  & SelectedVendor.VendorName &  Environment.NewLine & _
        "Created By: " & CurrentTransactionEntry.CreatedBy & Environment.NewLine & _
        "Created At: " & CurrentTransactionEntry.CreatedAt.ToString("MM/dd/yyyy hh:mm tt") & Environment.NewLine
        Label5.Text = output.ToString()
        Label8.Text = "Net Total: " & CurrentTransactionEntry.NetTotal

    Prompt_OutvoiceReceipt.ShowDialog(Me)
End Sub

    
End Class

