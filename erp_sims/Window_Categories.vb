Imports MySql.Data.MySqlClient

Public Class Window_Categories
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


    Private Sub Button_NewCategory_Click(sender As Object, e As EventArgs) 
        Dim Window_NewCategory As New Window_NewCategory()
        Window_NewCategory.Show()
        Me.Close()
    End Sub

    Private Sub Window_Categories_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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
        

        UpdateBrandsListTable()
        UpdateCategoriesListTable()
        Label_UserName.Text = GlobalShared.CurrentUser.Name
        Label_UserEmail.Text = GlobalShared.CurrentUser.Email
    End Sub



    Public ConnectionString = "server=localhost;user id=root;password=benjaminn1202;database=sims_db"
    Public Brands As Brand() = CreateBrandArray()
    Public Categories As Category() = CreateCategoryArray()
    Public Accounts As Account() = CreateAccountArray()
    

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
                            ' Create a new Employee object for each row
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



    Public BrandsListCurrentPage As Integer = 1
    Public BrandsListItemsPerPage As Integer = 10
    Public BrandsListTotalPages As Integer
    Public CategoriesListCurrentPage As Integer = 1
    Public CategoriesListItemsPerPage As Integer = 10
    Public CategoriesListTotalPages As Integer

    ' Update brands list
    Public Sub UpdateBrandsListTable()
        FlowLayoutPanel_BrandsListTable.Controls.Clear()

        BrandsListTotalPages = Math.Ceiling(Brands.Count / BrandsListItemsPerPage)
        Label_BrandsListTablePageControlPageNumber.Text = $"{BrandsListCurrentPage}/{BrandsListTotalPages}"

        Dim TableLayoutPanel_BrandsList As New TableLayoutPanel With {
            .AutoSize = True,
            .AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink,
            .BackColor = System.Drawing.Color.FromArgb(CType(255, Byte), CType(255, Byte), CType(255, Byte)),
            .CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single],
            .Margin = New System.Windows.Forms.Padding(0, 0, 0, 0),
            .MaximumSize = New System.Drawing.Size(1102, 0),
            .MinimumSize = New System.Drawing.Size(1102, 0)
        }

        TableLayoutPanel_BrandsList.ColumnCount = 4
        For i = 1 To 8
            TableLayoutPanel_BrandsList.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0! / 4))
        Next

        Dim StartIndex As Integer = (BrandsListCurrentPage - 1) * BrandsListItemsPerPage
        Dim EndIndex As Integer = Math.Min(StartIndex + BrandsListItemsPerPage, Brands.Count)

        For i = StartIndex To EndIndex - 1
            Dim brand As Brand = Brands(i)
            TableLayoutPanel_BrandsList.RowCount += 1

            Dim BrandAccount = Employees.FirstOrDefault(Function(acc) acc.EmployeeId = brand.CreatedBy)

            Dim labels As New List(Of Label)
            labels.Add(CreateLabel("Brand " & brand.BrandId.ToString(), 1))
            labels.Add(CreateLabel(brand.BrandName.ToString(), 2, True))
            labels.Add(CreateLabel(brand.CreatedBy & " - " &  BrandAccount.FirstName & " " & BrandAccount.MiddleName & " " & BrandAccount.LastName, 1))
            labels.Add(CreateLabel(brand.CreatedAt.ToString(), 1))
            
            For col As Integer = 0 To labels.Count - 1
                TableLayoutPanel_BrandsList.Controls.Add(labels(col), col, TableLayoutPanel_BrandsList.RowCount - 1)
            Next

            Dim maxLabelHeight As Integer = labels.Max(Function(lbl) lbl.Height)

            TableLayoutPanel_BrandsList.RowStyles.Add(New RowStyle(SizeType.Absolute, maxLabelHeight + 12))
        Next

        FlowLayoutPanel_BrandsListTable.Controls.Add(TableLayoutPanel_BrandsList)
    End Sub

    ' Update categories list
    Public Sub UpdateCategoriesListTable()
        FlowLayoutPanel_CategoriesListTable.Controls.Clear()

        CategoriesListTotalPages = Math.Ceiling(Categories.Count / CategoriesListItemsPerPage)
        Label_CategoriesListTablePageControlPageNumber.Text = $"{CategoriesListCurrentPage}/{CategoriesListTotalPages}"

        Dim TableLayoutPanel_CategoriesList As New TableLayoutPanel With {
            .AutoSize = True,
            .AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink,
            .BackColor = System.Drawing.Color.FromArgb(CType(255, Byte), CType(255, Byte), CType(255, Byte)),
            .CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single],
            .Margin = New System.Windows.Forms.Padding(0, 0, 0, 0),
            .MaximumSize = New System.Drawing.Size(1102, 0),
            .MinimumSize = New System.Drawing.Size(1102, 0)
        }

        TableLayoutPanel_CategoriesList.ColumnCount = 4
        For i = 1 To 8
            TableLayoutPanel_CategoriesList.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0! / 4))
        Next

        Dim StartIndex As Integer = (CategoriesListCurrentPage - 1) * CategoriesListItemsPerPage
        Dim EndIndex As Integer = Math.Min(StartIndex + CategoriesListItemsPerPage, Categories.Count)

        For i = StartIndex To EndIndex - 1
            Dim category As Category = Categories(i)
            TableLayoutPanel_CategoriesList.RowCount += 1

            Dim CategoryAccount = Employees.FirstOrDefault(Function(acc) acc.EmployeeId = category.CreatedBy)

            Dim labels As New List(Of Label)
            labels.Add(CreateLabel("Category " & category.CategoryId.ToString(), 1))
            labels.Add(CreateLabel(category.CategoryName.ToString(), 2, True))
            labels.Add(CreateLabel(category.CreatedBy & " - " &  CategoryAccount.FirstName & " " & CategoryAccount.MiddleName & " " & CategoryAccount.LastName, 1))
            labels.Add(CreateLabel(category.CreatedAt.ToString(), 1))
            
            For col As Integer = 0 To labels.Count - 1
                TableLayoutPanel_CategoriesList.Controls.Add(labels(col), col, TableLayoutPanel_CategoriesList.RowCount - 1)
            Next

            Dim maxLabelHeight As Integer = labels.Max(Function(lbl) lbl.Height)

            TableLayoutPanel_CategoriesList.RowStyles.Add(New RowStyle(SizeType.Absolute, maxLabelHeight + 12))
        Next

        FlowLayoutPanel_CategoriesListTable.Controls.Add(TableLayoutPanel_CategoriesList)
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



    ' INSERT Methods
    Private Sub Button_NewBrand_Click(sender As Object, e As EventArgs) Handles Button_NewBrand.Click
        Dim Prompt_NewBrand As New Form()
        Dim FlowLayoutPanel_BrandDetailsNewBrand = New System.Windows.Forms.FlowLayoutPanel()
        Dim FlowLayoutPanel_PageHeaderNewBrand = New System.Windows.Forms.FlowLayoutPanel()
        Dim Panel_PageHeaderIconNewBrand = New System.Windows.Forms.Panel()
        Dim Label_PageHeaderNewBrand = New System.Windows.Forms.Label()
        Dim Label_BrandDetailsNewBrand = New System.Windows.Forms.Label()
        Dim FlowLayoutPanel_BrandDetailBrandNameInputNewBrand = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label_BrandDetailBrandNameInputNewBrand = New System.Windows.Forms.Label()
        Dim Textbox_BrandDetailBrandNameInputNewBrand = New Guna.UI2.WinForms.Guna2TextBox()
        Dim FlowLayoutPanel_DateRangeInputsSales = New System.Windows.Forms.FlowLayoutPanel()
        Dim Button_MakeBrandNewSale = New Guna.UI2.WinForms.Guna2Button()
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
        '_Temporary_NewBrand
        '
        Prompt_NewBrand.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Prompt_NewBrand.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Prompt_NewBrand.ClientSize = New System.Drawing.Size(366, 350)
        Prompt_NewBrand.Controls.Add(FlowLayoutPanel_BrandDetailsNewBrand)
        Prompt_NewBrand.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Prompt_NewBrand.Text = "New Brand"
        FlowLayoutPanel_BrandDetailsNewBrand.ResumeLayout(false)
        FlowLayoutPanel_BrandDetailsNewBrand.PerformLayout
        FlowLayoutPanel_PageHeaderNewBrand.ResumeLayout(false)
        FlowLayoutPanel_PageHeaderNewBrand.PerformLayout
        FlowLayoutPanel_BrandDetailBrandNameInputNewBrand.ResumeLayout(false)
        FlowLayoutPanel_BrandDetailBrandNameInputNewBrand.PerformLayout
        ResumeLayout(false)

        AddHandler Button_MakeBrandNewSale.Click,
        Sub(sender1 As Object, e1 As EventArgs)
            If String.IsNullOrWhiteSpace(Textbox_BrandDetailBrandNameInputNewBrand.Text) Then
                MessageBox.Show("Brand name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
            
            Try
                Dim query As String = "INSERT INTO brands (brand_name, created_by) VALUES (@BrandName, @CreatedBy)"
        
                Using connection As New MySqlConnection(ConnectionString)
                    connection.Open()
            
                    Using command As New MySqlCommand(query, connection)
                        command.Parameters.AddWithValue("@BrandName", Textbox_BrandDetailBrandNameInputNewBrand.Text.Trim())
                        command.Parameters.AddWithValue("@CreatedBy", GlobalShared.CurrentUser.AccountId)
                
                        command.ExecuteNonQuery()
                    End Using
                End Using

                MessageBox.Show("Brand has been successfully added!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Brands = CreateBrandArray()
                UpdateBrandsListTable()
                Prompt_NewBrand.Close()
            Catch ex As Exception
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Prompt_NewBrand.ShowDialog(Me)
    End Sub

    Private Sub Button_NewCategory_Click_1(sender As Object, e As EventArgs) Handles Button_NewCategory.Click
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
        Prompt_NewCategory.Text = "New Category"
        FlowLayoutPanel_CategorDetailsNewCategor.ResumeLayout(false)
        FlowLayoutPanel_CategorDetailsNewCategor.PerformLayout
        FlowLayoutPanel_PageHeaderNewCategor.ResumeLayout(false)
        FlowLayoutPanel_PageHeaderNewCategor.PerformLayout
        FlowLayoutPanel_CategorDetailCategorNameInputNewCategor.ResumeLayout(false)
        FlowLayoutPanel_CategorDetailCategorNameInputNewCategor.PerformLayout
        ResumeLayout(false)

        AddHandler Button_MakeCategorNewSale.Click,
        Sub(sender1 As Object, e1 As EventArgs)
            Dim categoryName As String = Textbox_CategorDetailCategorNameInputNewCategor.Text.Trim()

            If String.IsNullOrEmpty(categoryName) Then
                MessageBox.Show("Category name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            Try
                Using connection As New MySqlConnection(ConnectionString)
                    connection.Open()
            
                    Dim query As String = "INSERT INTO categories (category_name, created_by) VALUES (@category_name, @created_by)"
            
                    Using command As New MySqlCommand(query, connection)
                        command.Parameters.AddWithValue("@category_name", categoryName)
                        command.Parameters.AddWithValue("@created_by", GlobalShared.CurrentUser.AccountId)

                        command.ExecuteNonQuery()
                    End Using
                End Using

                MessageBox.Show("Category added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Categories = CreateCategoryArray()
                UpdateCategoriesListTable()
                Prompt_NewCategory.Close()
            Catch ex As Exception
                MessageBox.Show($"An error occurred: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Prompt_NewCategory.ShowDialog(Me)
    End Sub

    Private Sub Button_Logout_Click(sender As Object, e As EventArgs) Handles Button_Logout.Click
        Dim result As DialogResult = MessageBox.Show("Are you sure you want to log out?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            Dim newForm As New Window_Login()
            newForm.Show()
            Me.Close()
        End If
    End Sub

    Private Sub Button_BrandsListTablePageControlPrevious_Click_1(sender As Object, e As EventArgs) Handles Button_BrandsListTablePageControlPrevious.Click
        If BrandsListCurrentPage > 1 Then
            BrandsListCurrentPage -= 1
            UpdateBrandsListTable()
        End If
    End Sub

    Private Sub Button_BrandsListTablePageControlNext_Click_1(sender As Object, e As EventArgs) Handles Button_BrandsListTablePageControlNext.Click
        If BrandsListCurrentPage < BrandsListTotalPages Then
            BrandsListCurrentPage += 1
            UpdateBrandsListTable()
        End If
    End Sub

    Private Sub Button_CategoriesListTablePageControlPrevious_Click_1(sender As Object, e As EventArgs) Handles Button_CategoriesListTablePageControlPrevious.Click
        If CategoriesListCurrentPage > 1 Then
            CategoriesListCurrentPage -= 1
            UpdateCategoriesListTable()
        End If
    End Sub

    Private Sub Button_CategoriesListTablePageControlNext_Click_1(sender As Object, e As EventArgs) Handles Button_CategoriesListTablePageControlNext.Click
        If CategoriesListCurrentPage < CategoriesListTotalPages Then
            CategoriesListCurrentPage += 1
            UpdateCategoriesListTable()
        End If
    End Sub

    Private Sub Button_SearchBrands_Click(sender As Object, e As EventArgs) Handles Button_SearchBrands.Click
        Dim searchText As String = Textbox_SearchBarBrands.Text.Trim().ToLower()

    ' Filter the brands based on the search text
    Dim filteredBrands = Brands.Where(Function(c) c.BrandName.ToLower().Contains(searchText) OrElse
    Employees.Any(Function(emp) emp.EmployeeId = c.CreatedBy AndAlso 
        $"{emp.FirstName} {emp.MiddleName} {emp.LastName} {emp.Suffix}".ToLower().Contains(searchText))
    ).ToList()

    FlowLayoutPanel_BrandsListTable.Controls.Clear()

    BrandsListTotalPages = Math.Ceiling(filteredBrands.Count / BrandsListItemsPerPage)
    Label_BrandsListTablePageControlPageNumber.Text = $"{BrandsListCurrentPage}/{BrandsListTotalPages}"

    Dim TableLayoutPanel_BrandsList As New TableLayoutPanel With {
        .AutoSize = True,
        .AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink,
        .BackColor = System.Drawing.Color.FromArgb(CType(255, Byte), CType(255, Byte), CType(255, Byte)),
        .CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single],
        .Margin = New System.Windows.Forms.Padding(0, 0, 0, 0),
        .MaximumSize = New System.Drawing.Size(1102, 0),
        .MinimumSize = New System.Drawing.Size(1102, 0)
    }

    TableLayoutPanel_BrandsList.ColumnCount = 4
    For i = 1 To 4
        TableLayoutPanel_BrandsList.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0! / 4))
    Next

    Dim StartIndex As Integer = (BrandsListCurrentPage - 1) * BrandsListItemsPerPage
    Dim EndIndex As Integer = Math.Min(StartIndex + BrandsListItemsPerPage, filteredBrands.Count)

    For i = StartIndex To EndIndex - 1
        Dim brand As Brand = filteredBrands(i)
        TableLayoutPanel_BrandsList.RowCount += 1

        Dim BrandAccount = Employees.FirstOrDefault(Function(acc) acc.EmployeeId = brand.CreatedBy)

        Dim labels As New List(Of Label)
        labels.Add(CreateLabel("Brand " & brand.BrandId.ToString(), 1))
        labels.Add(CreateLabel(brand.BrandName.ToString(), 2, True))
        labels.Add(CreateLabel(brand.CreatedBy & " - " &  BrandAccount.FirstName & " " & BrandAccount.MiddleName & " " & BrandAccount.LastName, 1))
        labels.Add(CreateLabel(brand.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"), 1))

        For col As Integer = 0 To labels.Count - 1
            TableLayoutPanel_BrandsList.Controls.Add(labels(col), col, TableLayoutPanel_BrandsList.RowCount - 1)
        Next

        Dim maxLabelHeight As Integer = labels.Max(Function(lbl) lbl.Height)

        TableLayoutPanel_BrandsList.RowStyles.Add(New RowStyle(SizeType.Absolute, maxLabelHeight + 12))
    Next

    FlowLayoutPanel_BrandsListTable.Controls.Add(TableLayoutPanel_BrandsList)

    End Sub

    Private Sub Button_SearchCategories_Click(sender As Object, e As EventArgs) Handles Button_SearchCategories.Click
        Dim searchText As String = Textbox_SearchBarCategories.Text.Trim().ToLower()

    ' Filter the categories based on the search text
    Dim filteredCategories = Categories.Where(Function(c) c.CategoryName.ToLower().Contains(searchText) OrElse
    Employees.Any(Function(emp) emp.EmployeeId = c.CreatedBy AndAlso 
        $"{emp.FirstName} {emp.MiddleName} {emp.LastName} {emp.Suffix}".ToLower().Contains(searchText))
    ).ToList()


    FlowLayoutPanel_CategoriesListTable.Controls.Clear()

    CategoriesListTotalPages = Math.Ceiling(filteredCategories.Count / CategoriesListItemsPerPage)
    Label_CategoriesListTablePageControlPageNumber.Text = $"{CategoriesListCurrentPage}/{CategoriesListTotalPages}"

    Dim TableLayoutPanel_CategoriesList As New TableLayoutPanel With {
        .AutoSize = True,
        .AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink,
        .BackColor = System.Drawing.Color.FromArgb(CType(255, Byte), CType(255, Byte), CType(255, Byte)),
        .CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single],
        .Margin = New System.Windows.Forms.Padding(0, 0, 0, 0),
        .MaximumSize = New System.Drawing.Size(1102, 0),
        .MinimumSize = New System.Drawing.Size(1102, 0)
    }

    TableLayoutPanel_CategoriesList.ColumnCount = 4
    For i = 1 To 4
        TableLayoutPanel_CategoriesList.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0! / 4))
    Next

    Dim StartIndex As Integer = (CategoriesListCurrentPage - 1) * CategoriesListItemsPerPage
    Dim EndIndex As Integer = Math.Min(StartIndex + CategoriesListItemsPerPage, filteredCategories.Count)

    For i = StartIndex To EndIndex - 1
        Dim category As Category = filteredCategories(i)
        TableLayoutPanel_CategoriesList.RowCount += 1

        Dim CategoryAccount = Employees.FirstOrDefault(Function(acc) acc.EmployeeId = category.CreatedBy)

        Dim labels As New List(Of Label)
        labels.Add(CreateLabel("Category " & category.CategoryId.ToString(), 1))
        labels.Add(CreateLabel(category.CategoryName, 2, True))
        labels.Add(CreateLabel(category.CreatedBy & " - " &  CategoryAccount.FirstName & " " & CategoryAccount.MiddleName & " " & CategoryAccount.LastName, 1))
        labels.Add(CreateLabel(category.CreatedAt.ToString(), 1))

        For col As Integer = 0 To labels.Count - 1
            TableLayoutPanel_CategoriesList.Controls.Add(labels(col), col, TableLayoutPanel_CategoriesList.RowCount - 1)
        Next

        Dim maxLabelHeight As Integer = labels.Max(Function(lbl) lbl.Height)

        TableLayoutPanel_CategoriesList.RowStyles.Add(New RowStyle(SizeType.Absolute, maxLabelHeight + 12))
    Next

    FlowLayoutPanel_CategoriesListTable.Controls.Add(TableLayoutPanel_CategoriesList)
    End Sub
End Class