Imports Guna.UI2.WinForms
Imports MySql.Data.MySqlClient
Imports System.Text
Imports System.Text.RegularExpressions

'' Gagawin: New vendor and search bar

Public Class Window_AddStock
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
    
    Public Brands As Brand() = CreateBrandArray()
    Public Categories As Category() = CreateCategoryArray()
    Public Stocks As Stock() = CreateStockArray()
    Public Items As Item() = CreateItemArray()
    Public Vendors As Vendor() = CreateVendorArray()
    

    Private CurrentStockEntry As New StockEntry()

    ' Populate categories selection
    Private CategoriesPerPage As Integer = 4
    Private CurrentCategoryPage As Integer = 1
    Private CurrentCategoryID As Integer = -1

    Public Sub PopulateCategoriesSelection()
        Dim totalCategories As Integer = Categories.Length
        Dim totalPages As Integer = Math.Ceiling(totalCategories / CategoriesPerPage)

        If CurrentCategoryPage > totalPages Then CurrentCategoryPage = totalPages
        If CurrentCategoryPage < 1 Then CurrentCategoryPage = 1

        Dim startIndex As Integer = (CurrentCategoryPage - 1) * CategoriesPerPage
        Dim pagedCategories = Categories.Skip(startIndex).Take(CategoriesPerPage-1).ToArray()

        Dim TableLayoutPanel_CategoriesSelection As New TableLayoutPanel With {
            .AutoSize = True,
            .AutoSizeMode = AutoSizeMode.GrowAndShrink,
            .BackColor = Color.FromArgb(235, 225, 255),
            .CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
            .Location = New Point(52, 64),
            .Margin = New Padding(0),
            .MaximumSize = New Size(518, 40),
            .MinimumSize = New Size(518, 40),
            .Name = "TableLayoutPanel_CategoriesSelection",
            .Padding = New Padding(12, 0, 12, 0),
            .Size = New Size(518, 40),
            .ColumnCount = 4,
            .RowCount = 1
        }

        For i = 1 To TableLayoutPanel_CategoriesSelection.ColumnCount
            TableLayoutPanel_CategoriesSelection.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 122))
        Next

        Dim AllCategoriesButton As New Guna2Button With {
            .Text = "All Items",
            .Size = New Size(122, 38),
            .Margin = New Padding(0),
            .FillColor = Color.FromArgb(235, 225, 255),
            .ForeColor = Color.FromArgb(34, 34, 34)
        }

        AddHandler AllCategoriesButton.Click, 
            Sub(sender, e)
                CurrentCategoryID = -1
                PopulateItemsGrid(CurrentCategoryID)
            End Sub

        TableLayoutPanel_CategoriesSelection.Controls.Add(AllCategoriesButton)

        For Each category In pagedCategories
            Dim button As New Guna2Button With {
                .Text = category.CategoryName,
                .Size = New Size(122, 38),
                .Margin = New Padding(0),
                .FillColor = Color.FromArgb(235, 225, 255),
                .ForeColor = Color.FromArgb(34, 34, 34)
            }

            AddHandler button.Click, 
                Sub(sender, e)
                    CurrentCategoryID = category.CategoryId
                    PopulateItemsGrid(CurrentCategoryID)
                End Sub

            TableLayoutPanel_CategoriesSelection.Controls.Add(button)
        Next

        FlowLayoutPanel_CategoriesSelection.Controls.Clear()
        FlowLayoutPanel_CategoriesSelection.Controls.Add(TableLayoutPanel_CategoriesSelection)

        Button_CategoriesSelectionControlPrevious.Enabled = CurrentCategoryPage > 1
        Button_CategoriesSelectionControlNext.Enabled = CurrentCategoryPage < totalPages
    End Sub
    
    Private Sub Button_CategoriesSelectionControlPrevious_Click(sender As Object, e As EventArgs) Handles Button_CategoriesSelectionControlPrevious.Click
        If CurrentCategoryPage > 1 Then
            CurrentCategoryPage -= 1
            PopulateCategoriesSelection()
        End If
    End Sub
    
    Private Sub Button_CategoriesSelectionControlNext_Click(sender As Object, e As EventArgs) Handles Button_CategoriesSelectionControlNext.Click
        Dim totalPages As Integer = Math.Ceiling(If(CurrentCategoryID = -1, Categories.Count, Categories.Where(Function(item) item.CategoryId = CurrentCategoryID).Count) / CategoriesPerPage)
        If CurrentCategoryPage < totalPages Then
            CurrentCategoryPage += 1
            PopulateCategoriesSelection()
        End If
    End Sub

    Private Sub SearchItemsGrid(CategoryID As Integer, searchBrandText As String)
    Dim filteredItems As List(Of Item)

    ' Filter items by category
    If CategoryID = -1 Then
        filteredItems = Items.ToList()
    Else
        filteredItems = Items.Where(Function(item) item.CategoryId = CategoryID).ToList()
    End If

    ' Filter items by searchBrandText
    searchBrandText = searchBrandText.Trim().ToLower()
    If Not String.IsNullOrEmpty(searchBrandText) Then
        filteredItems = filteredItems.Where(Function(item) item.ItemName.ToLower().Contains(searchBrandText)).ToList()
    End If

    Dim totalPages As Integer = Math.Ceiling(filteredItems.Count / ItemsPerPage)

    If CurrentPage > totalPages Then CurrentPage = totalPages
    If CurrentPage < 1 Then CurrentPage = 1

    Dim startIndex As Integer = (CurrentPage - 1) * ItemsPerPage
    Dim pagedItems = filteredItems.Skip(startIndex).Take(ItemsPerPage).ToList()

    Dim TableLayoutPanel_ItemsGrid As New TableLayoutPanel With {
        .AutoSize = True,
        .AutoSizeMode = AutoSizeMode.GrowAndShrink,
        .BackColor = Color.FromArgb(235, 225, 255),
        .CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
        .Location = New Point(12, 114),
        .Margin = New Padding(0, 0, 0, 0),
        .MaximumSize = New Size(598, 0),
        .MinimumSize = New Size(598, 0),
        .Name = "TableLayoutPanel_ItemsGrid",
        .ColumnCount = 5,
        .RowCount = 3
    }

    For i = 1 To TableLayoutPanel_ItemsGrid.ColumnCount
        TableLayoutPanel_ItemsGrid.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 118))
    Next
    For i = 1 To TableLayoutPanel_ItemsGrid.RowCount
        TableLayoutPanel_ItemsGrid.RowStyles.Add(New RowStyle(SizeType.Absolute, 180))
    Next

    For Each item In pagedItems
        Dim AvailableStocksTotal = 0

        ' Calculate available stocks
        For Each stock In Stocks
            If stock.ItemId = item.ItemId Then
                If stock.Status = "Available" Then
                    AvailableStocksTotal += stock.StockQuantity
                End If
            End If
        Next

        Dim ButtonText = 
            "Item: " & item.ItemName & vbCrLf &
            "Price: " & item.SellingPrice & vbCrLf &
            "Stocks: " & AvailableStocksTotal

        Dim button As New Guna2Button With {
            .Text = ButtonText,
            .Size = New Size(118, 180),
            .Margin = New Padding(0),
            .FillColor = Color.FromArgb(0, 72, 101)
        }

        AddHandler button.Click, 
        Sub(sender, e)
            Dim ItemID = item.ItemId
            UpdateCurrentTransactionData(ItemID, item)
            UpdateTransactionItemsTable()
        End Sub

        TableLayoutPanel_ItemsGrid.Controls.Add(button)
    Next

    FlowLayoutPanel_TableLayoutPanel_ItemsGrid.Controls.Clear()
    FlowLayoutPanel_TableLayoutPanel_ItemsGrid.Controls.Add(TableLayoutPanel_ItemsGrid)

    Label_ItemsGridControlPageNumber.Text = $"{CurrentPage}/{totalPages}"
    Button_ItemsGridControlPrevious.Enabled = CurrentPage > 1
    Button_ItemsGridControlNext.Enabled = CurrentPage < totalPages
End Sub



    ' Update Grids
    Private Sub PopulateItemsGrid(CategoryID As Integer)
        Dim filteredItems As List(Of Item)

        If CategoryID = -1 Then
            filteredItems = Items.ToList()
        Else
            filteredItems = Items.Where(Function(item) item.CategoryId = CategoryID).ToList()
        End If

        Dim totalPages As Integer = Math.Ceiling(filteredItems.Count / ItemsPerPage)

        If CurrentPage > totalPages Then CurrentPage = totalPages
        If CurrentPage < 1 Then CurrentPage = 1

        Dim startIndex As Integer = (CurrentPage - 1) * ItemsPerPage
        Dim pagedItems = filteredItems.Skip(startIndex).Take(ItemsPerPage).ToList()

        Dim TableLayoutPanel_ItemsGrid As New TableLayoutPanel With {
            .AutoSize = True,
            .AutoSizeMode = AutoSizeMode.GrowAndShrink,
            .BackColor = Color.FromArgb(235, 225, 255),
            .CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
            .Location = New Point(12, 114),
            .Margin = New Padding(0, 0, 0, 0),
            .MaximumSize = New Size(598, 0),
            .MinimumSize = New Size(598, 0),
            .Name = "TableLayoutPanel_ItemsGrid",
            .ColumnCount = 5,
            .RowCount = 3
        }

        For i = 1 To TableLayoutPanel_ItemsGrid.ColumnCount
            TableLayoutPanel_ItemsGrid.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 118))
        Next
        For i = 1 To TableLayoutPanel_ItemsGrid.RowCount
            TableLayoutPanel_ItemsGrid.RowStyles.Add(New RowStyle(SizeType.Absolute, 180))
        Next

        For Each item In pagedItems
            Dim AvailableStocksTotal = 0

            For Each item1 In Stocks
                If item1.ItemId = item.ItemId Then
                    If item1.Status = "Available" Then
                        AvailableStocksTotal += item1.StockQuantity
                    End If
                End If
            Next

            Dim ButtonText = 
                "Item: " & item.ItemName & vbCrLf &
                "Price: " & item.SellingPrice & vbCrLf &
                "Stocks: " & AvailableStocksTotal

            Dim button As New Guna2Button With {
                .Text = ButtonText,
                .Size = New Size(118, 180),
                .Margin = New Padding(0),
                .FillColor = Color.FromArgb(0, 72, 101)
            }

            AddHandler button.Click, 
            Sub(sender, e)
                Dim ItemID = item.ItemId
                UpdateCurrentTransactionData(ItemID, item)
                UpdateTransactionItemsTable()
            End Sub

            TableLayoutPanel_ItemsGrid.Controls.Add(button)
        Next

        FlowLayoutPanel_TableLayoutPanel_ItemsGrid.Controls.Clear()
        FlowLayoutPanel_TableLayoutPanel_ItemsGrid.Controls.Add(TableLayoutPanel_ItemsGrid)

        Label_ItemsGridControlPageNumber.Text = $"{CurrentPage}/{totalPages}"
        Button_ItemsGridControlPrevious.Enabled = CurrentPage > 1
        Button_ItemsGridControlNext.Enabled = CurrentPage < totalPages
    End Sub

    Private Sub Button_ItemsGridControlPrevious_Click(sender As Object, e As EventArgs) Handles Button_ItemsGridControlPrevious.Click
        If CurrentPage > 1 Then
            CurrentPage -= 1
            PopulateItemsGrid(CurrentCategoryID)
        End If
    End Sub

    Private Sub Button_ItemsGridControlNext_Click(sender As Object, e As EventArgs) Handles Button_ItemsGridControlNext.Click
        Dim totalPages As Integer = Math.Ceiling(If(CurrentCategoryID = -1, Items.Count, Items.Where(Function(item) item.CategoryId = CurrentCategoryID).Count) / ItemsPerPage)
        If CurrentPage < totalPages Then
            CurrentPage += 1
            PopulateItemsGrid(CurrentCategoryID)
        End If
    End Sub

    Public Sub PopulateVendorComboBox()
        Dim query As String = "SELECT vendor_name FROM vendors"

        Using connection As New MySqlConnection(ConnectionString)
            Try
                connection.Open()

                Dim command As New MySqlCommand(query, connection)

                Using reader As MySqlDataReader = command.ExecuteReader()
                    ComboBox_VendorStockOutvoicePlaceOrder.Items.Clear()

                    While reader.Read()
                        ComboBox_VendorStockOutvoicePlaceOrder.Items.Add(reader("vendor_name").ToString())
                    End While
                End Using
            Catch ex As Exception
                MessageBox.Show("Error fetching vendors: " & ex.Message)
            End Try
        End Using
    End Sub



    ' Update database
    Sub UpdateAvailableQuantities()
        Try
            Using connection As New MySqlConnection(ConnectionString)
                connection.Open()

                Dim selectQuery As String = "
                    SELECT 
                        item_id, 
                        SUM(stock_quantity) AS total_available 
                    FROM stocks 
                    WHERE status = 'Available' 
                    GROUP BY item_id"

                Dim availableQuantities As New Dictionary(Of Integer, Integer)()

                Using selectCommand As New MySqlCommand(selectQuery, connection)
                    Using reader As MySqlDataReader = selectCommand.ExecuteReader()
                        While reader.Read()
                            Dim itemId As Integer = reader.GetInt32("item_id")
                            Dim totalAvailable As Integer = reader.GetInt32("total_available")
                            availableQuantities(itemId) = totalAvailable
                        End While
                    End Using
                End Using

                Dim updateQuery As String = "UPDATE items SET available_quantity = @AvailableQuantity WHERE item_id = @ItemId"

                Using updateCommand As New MySqlCommand(updateQuery, connection)
                    updateCommand.Parameters.Add("@AvailableQuantity", MySqlDbType.Int32)
                    updateCommand.Parameters.Add("@ItemId", MySqlDbType.Int32)

                    For Each kvp In availableQuantities
                        updateCommand.Parameters("@AvailableQuantity").Value = kvp.Value
                        updateCommand.Parameters("@ItemId").Value = kvp.Key
                        updateCommand.ExecuteNonQuery()
                    Next
                End Using
            End Using

            Console.WriteLine("Available quantities updated successfully.")
        Catch ex As Exception
            Console.WriteLine("An error occurred: " & ex.Message)
        End Try
    End Sub



    ' Get data from database
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


    ' Update receive items table
    Private TransactionItemsTableCurrentPage As Integer = 1
    Private TransactionItemsTableItemsPerPage As Integer = 8
    Private TransactionItemsTableTotalPagess As Integer = 1
    Private ItemsPerPage As Integer = 15
    Private CurrentPage As Integer = 1
    Public CurrentTransactionItems As New List(Of Stock)()
    Public CurrentSelectedTransactionItemIDs As New List(Of Integer)()
    Public CurrentTransactionEntry As New StockEntry()

    Public Sub UpdateTransactionItemsTable()
        FlowLayoutPanel_SaleItemsTable.Controls.Clear()
        FlowLayoutPanel_SaleItemsTable.Size = New Size(516, 0)

        TransactionItemsTableTotalPagess = Math.Ceiling(CurrentTransactionItems.Count / TransactionItemsTableItemsPerPage)

        Label_SaleItemsTablePageControlPageNumber.Text = $"{TransactionItemsTableCurrentPage}/{TransactionItemsTableTotalPagess}"

        Dim TableLayoutPanel_TransactionItemsTableNewSale As New TableLayoutPanel With {
            .AutoSize = True,
            .AutoSizeMode = AutoSizeMode.GrowAndShrink,
            .BackColor = System.Drawing.Color.White,
            .CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
            .Margin = New Padding(0, 0, 0, 0),
            .MaximumSize = New Size(516, 0),
            .MinimumSize = New Size(516, 0),
            .Size = New Size(516, 0)
        }

        TableLayoutPanel_TransactionItemsTableNewSale.ColumnCount = 5
        TableLayoutPanel_TransactionItemsTableNewSale.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 42))
        TableLayoutPanel_TransactionItemsTableNewSale.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.33333!))
        TableLayoutPanel_TransactionItemsTableNewSale.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 84))
        TableLayoutPanel_TransactionItemsTableNewSale.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.33333!))
        TableLayoutPanel_TransactionItemsTableNewSale.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.33333!))

        TableLayoutPanel_TransactionItemsTableNewSale.RowCount = Math.Min(TransactionItemsTableItemsPerPage, CurrentTransactionItems.Count - (TransactionItemsTableCurrentPage - 1) * TransactionItemsTableItemsPerPage)
        For i As Integer = 0 To TableLayoutPanel_TransactionItemsTableNewSale.RowCount - 1
            TableLayoutPanel_TransactionItemsTableNewSale.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        Next

        Dim StartIndex As Integer = (TransactionItemsTableCurrentPage - 1) * TransactionItemsTableItemsPerPage
        Dim EndIndex As Integer = Math.Min(StartIndex + TransactionItemsTableItemsPerPage - 1, CurrentTransactionItems.Count - 1)

        For i As Integer = StartIndex To EndIndex
            Dim stock = CurrentTransactionItems(i)
            Dim item = Items.FirstOrDefault(Function(s) s.ItemId = stock.ItemID)

            Dim editButton As New Guna.UI2.WinForms.Guna2Button With {
                .Anchor = AnchorStyles.Left,
                .BackgroundImage = Global.ERP_SIMS.My.Resources.Resources.Button_TableEdit,
                .BackColor = Color.Transparent,
                .FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer)),
                .Margin = New System.Windows.Forms.Padding(4, 4, 0, 4),
                .Size = New System.Drawing.Size(34, 32)
            }

            AddHandler editButton.Click, Sub(sender As Object, e As EventArgs)
                ShowEditReceiveItemPrompt(stock)
            End Sub

            Dim labelItemName As New Label With {
                .Anchor = AnchorStyles.Left,
                .AutoSize = True,
                .BackColor = Color.White,
                .Font = New Font("Microsoft JhengHei", 12, FontStyle.Bold, GraphicsUnit.Pixel),
                .ForeColor = Color.FromArgb(2, 34, 34),
                .Margin = New Padding(12, 6, 12, 6),
                .Text = item.ItemName,
                .TextAlign = ContentAlignment.MiddleLeft
            }
            Dim labelQuantity As New Label With {
                .Anchor = AnchorStyles.Left,
                .AutoSize = True,
                .BackColor = Color.White,
                .Font = New Font("Microsoft JhengHei", 12, FontStyle.Bold, GraphicsUnit.Pixel),
                .ForeColor = Color.FromArgb(2, 34, 34),
                .Margin = New Padding(12, 6, 12, 6),
                .Text = stock.StockQuantity,
                .TextAlign = ContentAlignment.MiddleLeft
            }
            Dim labelBuyingPrice As New Label With {
                .Anchor = AnchorStyles.Left,
                .AutoSize = True,
                .BackColor = Color.White,
                .Font = New Font("Microsoft JhengHei", 12, FontStyle.Bold, GraphicsUnit.Pixel),
                .ForeColor = Color.FromArgb(2, 34, 34),
                .Margin = New Padding(12, 6, 12, 6),
                .Text = stock.ItemBuyingPrice,
                .TextAlign = ContentAlignment.MiddleLeft
            }
            Dim labelAmount As New Label With {
                .Anchor = AnchorStyles.Left,
                .AutoSize = True,
                .BackColor = Color.White,
                .Font = New Font("Microsoft JhengHei", 12, FontStyle.Bold, GraphicsUnit.Pixel),
                .ForeColor = Color.FromArgb(2, 34, 34),
                .Margin = New Padding(12, 6, 12, 6),
                .Text = stock.TotalBuyingPrice,
                .TextAlign = ContentAlignment.MiddleLeft
            }

            TableLayoutPanel_TransactionItemsTableNewSale.Controls.Add(editButton, 0, i - StartIndex)
            TableLayoutPanel_TransactionItemsTableNewSale.Controls.Add(labelItemName, 1, i - StartIndex)
            TableLayoutPanel_TransactionItemsTableNewSale.Controls.Add(labelQuantity, 2, i - StartIndex)
            TableLayoutPanel_TransactionItemsTableNewSale.Controls.Add(labelBuyingPrice, 3, i - StartIndex)
            TableLayoutPanel_TransactionItemsTableNewSale.Controls.Add(labelAmount, 4, i - StartIndex)
        Next

        FlowLayoutPanel_SaleItemsTable.Controls.Add(TableLayoutPanel_TransactionItemsTableNewSale)
    End Sub

    Public Sub UpdateCurrentTransactionData(ItemID As Integer, SelectedItem As Item)
        Dim SearchedTransactionItem = CurrentTransactionItems.FirstOrDefault(Function(s) s.ItemId = ItemID)

        If SearchedTransactionItem IsNot Nothing Then
            SearchedTransactionItem.StockQuantity += 1
            SearchedTransactionItem.TotalBuyingPrice = SearchedTransactionItem.StockQuantity * SearchedTransactionItem.ItemBuyingPrice
        Else
            Dim TransactionItem As New Stock() With {
                .ItemId = SelectedItem.ItemId,
                .StockQuantity = 1,
                .ItemBuyingPrice = 0,
                .TotalBuyingPrice = 0 * 1
            }

            CurrentTransactionItems.Add(TransactionItem)
        End If

        Dim netTotal As Decimal = 0D

        For Each Item In CurrentTransactionItems
            netTotal += Item.TotalBuyingPrice
        Next

        CurrentTransactionEntry.NetTotal = netTotal

        Label_NetTotalSaleScreen.Text = CurrentTransactionEntry.NetTotal
    End Sub

    Private Sub Button_SaleItemsTablePageControlPrevious_Click(sender As Object, e As EventArgs) Handles Button_SaleItemsTablePageControlPrevious.Click
        If TransactionItemsTableCurrentPage > 1 Then
            TransactionItemsTableCurrentPage -= 1
            UpdateTransactionItemsTable()
        End If
    End Sub

    Private Sub Button_SaleItemsTablePageControlNext_Click(sender As Object, e As EventArgs) Handles Button_SaleItemsTablePageControlNext.Click
        If TransactionItemsTableCurrentPage < TransactionItemsTableTotalPagess Then
            TransactionItemsTableCurrentPage += 1
            UpdateTransactionItemsTable()
        End If
    End Sub



    ' Show forms
    Private Sub ShowEditReceiveItemPrompt(SelectedStock As Stock)
        Dim item = Items.FirstOrDefault(Function(s) s.ItemId = SelectedStock.ItemID)

        Dim Prompt_EditReceiveItem As New Form()
        Dim FlowLayoutPanel_EditReceiveItem = New System.Windows.Forms.FlowLayoutPanel()
        Dim FlowLayoutPanel_PageHeaderEditReceiveItem = New System.Windows.Forms.FlowLayoutPanel()
        Dim Panel_PageHeaderEditReceiveItem = New System.Windows.Forms.Panel()
        Dim Label_PageHeaderNewBrand = New System.Windows.Forms.Label()
        Dim Label_ItemNameEditReceiveItem = New System.Windows.Forms.Label()
        Dim FlowLayoutPanel_DateRangeInputsSales = New System.Windows.Forms.FlowLayoutPanel()
        Dim FlowLayoutPanel_BuyingPriceInputEditReceiveItem = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label_BuyingPriceInputEditReceiveItem = New System.Windows.Forms.Label()
        Dim Textbox_BuyingPriceInputEditReceiveItem = New Guna.UI2.WinForms.Guna2TextBox()
        Dim FlowLayoutPanel_QuantityInputEditReceiveItem = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label_QuantityInputEditReceiveItem = New System.Windows.Forms.Label()
        Dim Textbox_QuantityInputEditReceiveItem = New Guna.UI2.WinForms.Guna2TextBox()
        Dim FlowLayoutPanel_HasExpiryInputEditReceiveItem = New System.Windows.Forms.FlowLayoutPanel()
        Dim Checkbox_HasExpiryInputEditReceiveItem = New Guna.UI2.WinForms.Guna2CustomCheckBox()
        Dim Label_HasExpiryInputEditReceiveItem = New System.Windows.Forms.Label()
        Dim FlowLayoutPanel_ExpirationDateInputEditReceiveItem = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label_ExpirationDateInputEditReceiveItem = New System.Windows.Forms.Label()
        Dim DateTimePicker_ExpirationDateInputEditReceiveItem = New Guna.UI2.WinForms.Guna2DateTimePicker()
        Dim Button_SaveChangesEditReceivedItem = New Guna.UI2.WinForms.Guna2Button()
        Dim Button_RemoveItemEditReceivedItem = New Guna.UI2.WinForms.Guna2Button()
        FlowLayoutPanel_EditReceiveItem.SuspendLayout
        FlowLayoutPanel_PageHeaderEditReceiveItem.SuspendLayout
        FlowLayoutPanel_BuyingPriceInputEditReceiveItem.SuspendLayout
        FlowLayoutPanel_QuantityInputEditReceiveItem.SuspendLayout
        FlowLayoutPanel_HasExpiryInputEditReceiveItem.SuspendLayout
        FlowLayoutPanel_ExpirationDateInputEditReceiveItem.SuspendLayout
        SuspendLayout
        '
        'FlowLayoutPanel_EditReceiveItem
        '
        FlowLayoutPanel_EditReceiveItem.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        FlowLayoutPanel_EditReceiveItem.Controls.Add(FlowLayoutPanel_PageHeaderEditReceiveItem)
        FlowLayoutPanel_EditReceiveItem.Controls.Add(Label_ItemNameEditReceiveItem)
        FlowLayoutPanel_EditReceiveItem.Controls.Add(FlowLayoutPanel_DateRangeInputsSales)
        FlowLayoutPanel_EditReceiveItem.Controls.Add(FlowLayoutPanel_QuantityInputEditReceiveItem)
        FlowLayoutPanel_EditReceiveItem.Controls.Add(FlowLayoutPanel_BuyingPriceInputEditReceiveItem)
        FlowLayoutPanel_EditReceiveItem.Controls.Add(FlowLayoutPanel_HasExpiryInputEditReceiveItem)
        FlowLayoutPanel_EditReceiveItem.Controls.Add(FlowLayoutPanel_ExpirationDateInputEditReceiveItem)
        FlowLayoutPanel_EditReceiveItem.Controls.Add(Button_SaveChangesEditReceivedItem)
        FlowLayoutPanel_EditReceiveItem.Controls.Add(Button_RemoveItemEditReceivedItem)
        FlowLayoutPanel_EditReceiveItem.Location = New System.Drawing.Point(0, 0)
        FlowLayoutPanel_EditReceiveItem.Margin = New System.Windows.Forms.Padding(0)
        FlowLayoutPanel_EditReceiveItem.Name = "FlowLayoutPanel_EditReceiveItem"
        FlowLayoutPanel_EditReceiveItem.Size = New System.Drawing.Size(366, 520)
        FlowLayoutPanel_EditReceiveItem.TabIndex = 35
        '
        'FlowLayoutPanel_PageHeaderEditReceiveItem
        '
        FlowLayoutPanel_PageHeaderEditReceiveItem.AutoSize = true
        FlowLayoutPanel_PageHeaderEditReceiveItem.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_PageHeaderEditReceiveItem.Controls.Add(Panel_PageHeaderEditReceiveItem)
        FlowLayoutPanel_PageHeaderEditReceiveItem.Controls.Add(Label_PageHeaderNewBrand)
        FlowLayoutPanel_PageHeaderEditReceiveItem.Location = New System.Drawing.Point(12, 24)
        FlowLayoutPanel_PageHeaderEditReceiveItem.Margin = New System.Windows.Forms.Padding(12, 24, 0, 24)
        FlowLayoutPanel_PageHeaderEditReceiveItem.Name = "FlowLayoutPanel_PageHeaderEditReceiveItem"
        FlowLayoutPanel_PageHeaderEditReceiveItem.Size = New System.Drawing.Size(297, 32)
        FlowLayoutPanel_PageHeaderEditReceiveItem.TabIndex = 0
        '
        'Panel_PageHeaderEditReceiveItem
        '
        Panel_PageHeaderEditReceiveItem.Anchor = System.Windows.Forms.AnchorStyles.Left
        Panel_PageHeaderEditReceiveItem.BackgroundImage = Global.ERP_SIMS.My.Resources.Resources.HeaderIcon_Stocks
        Panel_PageHeaderEditReceiveItem.Location = New System.Drawing.Point(0, 0)
        Panel_PageHeaderEditReceiveItem.Margin = New System.Windows.Forms.Padding(0, 0, 16, 0)
        Panel_PageHeaderEditReceiveItem.Name = "Panel_PageHeaderEditReceiveItem"
        Panel_PageHeaderEditReceiveItem.Size = New System.Drawing.Size(32, 32)
        Panel_PageHeaderEditReceiveItem.TabIndex = 3
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
        Label_PageHeaderNewBrand.Size = New System.Drawing.Size(249, 31)
        Label_PageHeaderNewBrand.TabIndex = 2
        Label_PageHeaderNewBrand.Text = "Receive Item Details"
        Label_PageHeaderNewBrand.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label_ItemNameEditReceiveItem
        '
        Label_ItemNameEditReceiveItem.Anchor = System.Windows.Forms.AnchorStyles.Left
        Label_ItemNameEditReceiveItem.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_ItemNameEditReceiveItem.Font = New System.Drawing.Font("Microsoft JhengHei", 22!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_ItemNameEditReceiveItem.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Label_ItemNameEditReceiveItem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_ItemNameEditReceiveItem.Location = New System.Drawing.Point(12, 80)
        Label_ItemNameEditReceiveItem.Margin = New System.Windows.Forms.Padding(12, 0, 12, 24)
        Label_ItemNameEditReceiveItem.Name = "Label_ItemNameEditReceiveItem"
        Label_ItemNameEditReceiveItem.Size = New System.Drawing.Size(342, 28)
        Label_ItemNameEditReceiveItem.TabIndex = 18
        Label_ItemNameEditReceiveItem.Text = item.ItemName
        Label_ItemNameEditReceiveItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'FlowLayoutPanel_DateRangeInputsSales
        '
        FlowLayoutPanel_DateRangeInputsSales.AutoSize = true
        FlowLayoutPanel_DateRangeInputsSales.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_DateRangeInputsSales.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_DateRangeInputsSales.Location = New System.Drawing.Point(366, 80)
        FlowLayoutPanel_DateRangeInputsSales.Margin = New System.Windows.Forms.Padding(0, 0, 0, 42)
        FlowLayoutPanel_DateRangeInputsSales.Name = "FlowLayoutPanel_DateRangeInputsSales"
        FlowLayoutPanel_DateRangeInputsSales.Size = New System.Drawing.Size(0, 0)
        FlowLayoutPanel_DateRangeInputsSales.TabIndex = 35
        '
        'FlowLayoutPanel_BuyingPriceInputEditReceiveItem
        '
        FlowLayoutPanel_BuyingPriceInputEditReceiveItem.AutoSize = true
        FlowLayoutPanel_BuyingPriceInputEditReceiveItem.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_BuyingPriceInputEditReceiveItem.Controls.Add(Label_BuyingPriceInputEditReceiveItem)
        FlowLayoutPanel_BuyingPriceInputEditReceiveItem.Controls.Add(Textbox_BuyingPriceInputEditReceiveItem)
        FlowLayoutPanel_BuyingPriceInputEditReceiveItem.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_BuyingPriceInputEditReceiveItem.Location = New System.Drawing.Point(12, 200)
        FlowLayoutPanel_BuyingPriceInputEditReceiveItem.Margin = New System.Windows.Forms.Padding(12, 0, 6, 12)
        FlowLayoutPanel_BuyingPriceInputEditReceiveItem.Name = "FlowLayoutPanel_BuyingPriceInputEditReceiveItem"
        FlowLayoutPanel_BuyingPriceInputEditReceiveItem.Size = New System.Drawing.Size(342, 56)
        FlowLayoutPanel_BuyingPriceInputEditReceiveItem.TabIndex = 38
        '
        'Label_BuyingPriceInputEditReceiveItem
        '
        Label_BuyingPriceInputEditReceiveItem.AutoSize = true
        Label_BuyingPriceInputEditReceiveItem.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_BuyingPriceInputEditReceiveItem.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_BuyingPriceInputEditReceiveItem.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_BuyingPriceInputEditReceiveItem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_BuyingPriceInputEditReceiveItem.Location = New System.Drawing.Point(0, 0)
        Label_BuyingPriceInputEditReceiveItem.Margin = New System.Windows.Forms.Padding(0)
        Label_BuyingPriceInputEditReceiveItem.Name = "Label_BuyingPriceInputEditReceiveItem"
        Label_BuyingPriceInputEditReceiveItem.Size = New System.Drawing.Size(75, 16)
        Label_BuyingPriceInputEditReceiveItem.TabIndex = 2
        Label_BuyingPriceInputEditReceiveItem.Text = "Buying Price"
        Label_BuyingPriceInputEditReceiveItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Textbox_BuyingPriceInputEditReceiveItem
        '
        Textbox_BuyingPriceInputEditReceiveItem.Anchor = System.Windows.Forms.AnchorStyles.Left
        Textbox_BuyingPriceInputEditReceiveItem.AutoCompleteCustomSource.AddRange(New String() {"(001) Item 1", "(002) Item 2", "(003) Item 3", "(004) Item 4", "(005) Item 5", "(006) Item 6", "(007) Item 7", "(008) Item 8", "(009) Item 9", "(010) Item 10"})
        Textbox_BuyingPriceInputEditReceiveItem.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Textbox_BuyingPriceInputEditReceiveItem.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Textbox_BuyingPriceInputEditReceiveItem.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_BuyingPriceInputEditReceiveItem.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_BuyingPriceInputEditReceiveItem.Cursor = System.Windows.Forms.Cursors.IBeam
        Textbox_BuyingPriceInputEditReceiveItem.DefaultText = ""
        Textbox_BuyingPriceInputEditReceiveItem.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer))
        Textbox_BuyingPriceInputEditReceiveItem.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer))
        Textbox_BuyingPriceInputEditReceiveItem.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_BuyingPriceInputEditReceiveItem.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_BuyingPriceInputEditReceiveItem.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_BuyingPriceInputEditReceiveItem.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_BuyingPriceInputEditReceiveItem.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
        Textbox_BuyingPriceInputEditReceiveItem.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_BuyingPriceInputEditReceiveItem.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_BuyingPriceInputEditReceiveItem.Location = New System.Drawing.Point(0, 16)
        Textbox_BuyingPriceInputEditReceiveItem.Margin = New System.Windows.Forms.Padding(0)
        Textbox_BuyingPriceInputEditReceiveItem.Name = "Textbox_BuyingPriceInputEditReceiveItem"
        Textbox_BuyingPriceInputEditReceiveItem.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Textbox_BuyingPriceInputEditReceiveItem.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer))
        Textbox_BuyingPriceInputEditReceiveItem.PlaceholderText = "Required"
        Textbox_BuyingPriceInputEditReceiveItem.SelectedText = ""
        Textbox_BuyingPriceInputEditReceiveItem.Size = New System.Drawing.Size(342, 40)
        Textbox_BuyingPriceInputEditReceiveItem.Text = SelectedStock.ItemBuyingPrice
        Textbox_BuyingPriceInputEditReceiveItem.TabIndex = 39
        '
        'FlowLayoutPanel_QuantityInputEditReceiveItem
        '
        FlowLayoutPanel_QuantityInputEditReceiveItem.AutoSize = true
        FlowLayoutPanel_QuantityInputEditReceiveItem.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_QuantityInputEditReceiveItem.Controls.Add(Label_QuantityInputEditReceiveItem)
        FlowLayoutPanel_QuantityInputEditReceiveItem.Controls.Add(Textbox_QuantityInputEditReceiveItem)
        FlowLayoutPanel_QuantityInputEditReceiveItem.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_QuantityInputEditReceiveItem.Location = New System.Drawing.Point(12, 132)
        FlowLayoutPanel_QuantityInputEditReceiveItem.Margin = New System.Windows.Forms.Padding(12, 0, 6, 12)
        FlowLayoutPanel_QuantityInputEditReceiveItem.Name = "FlowLayoutPanel_QuantityInputEditReceiveItem"
        FlowLayoutPanel_QuantityInputEditReceiveItem.Size = New System.Drawing.Size(342, 56)
        Textbox_QuantityInputEditReceiveItem.Text = SelectedStock.StockQuantity
        FlowLayoutPanel_QuantityInputEditReceiveItem.TabIndex = 40
        '
        'Label_QuantityInputEditReceiveItem
        '
        Label_QuantityInputEditReceiveItem.AutoSize = true
        Label_QuantityInputEditReceiveItem.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_QuantityInputEditReceiveItem.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_QuantityInputEditReceiveItem.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_QuantityInputEditReceiveItem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_QuantityInputEditReceiveItem.Location = New System.Drawing.Point(0, 0)
        Label_QuantityInputEditReceiveItem.Margin = New System.Windows.Forms.Padding(0)
        Label_QuantityInputEditReceiveItem.Name = "Label_QuantityInputEditReceiveItem"
        Label_QuantityInputEditReceiveItem.Size = New System.Drawing.Size(55, 16)
        Label_QuantityInputEditReceiveItem.TabIndex = 2
        Label_QuantityInputEditReceiveItem.Text = "Quantity"
        Label_QuantityInputEditReceiveItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Textbox_QuantityInputEditReceiveItem
        '
        Textbox_QuantityInputEditReceiveItem.Anchor = System.Windows.Forms.AnchorStyles.Left
        Textbox_QuantityInputEditReceiveItem.AutoCompleteCustomSource.AddRange(New String() {"(001) Item 1", "(002) Item 2", "(003) Item 3", "(004) Item 4", "(005) Item 5", "(006) Item 6", "(007) Item 7", "(008) Item 8", "(009) Item 9", "(010) Item 10"})
        Textbox_QuantityInputEditReceiveItem.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Textbox_QuantityInputEditReceiveItem.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Textbox_QuantityInputEditReceiveItem.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_QuantityInputEditReceiveItem.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_QuantityInputEditReceiveItem.Cursor = System.Windows.Forms.Cursors.IBeam
        Textbox_QuantityInputEditReceiveItem.DefaultText = ""
        Textbox_QuantityInputEditReceiveItem.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer))
        Textbox_QuantityInputEditReceiveItem.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer))
        Textbox_QuantityInputEditReceiveItem.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_QuantityInputEditReceiveItem.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_QuantityInputEditReceiveItem.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_QuantityInputEditReceiveItem.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_QuantityInputEditReceiveItem.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
        Textbox_QuantityInputEditReceiveItem.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_QuantityInputEditReceiveItem.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_QuantityInputEditReceiveItem.Location = New System.Drawing.Point(0, 16)
        Textbox_QuantityInputEditReceiveItem.Margin = New System.Windows.Forms.Padding(0)
        Textbox_QuantityInputEditReceiveItem.Name = "Textbox_QuantityInputEditReceiveItem"
        Textbox_QuantityInputEditReceiveItem.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Textbox_QuantityInputEditReceiveItem.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer))
        Textbox_QuantityInputEditReceiveItem.PlaceholderText = "Required"
        Textbox_QuantityInputEditReceiveItem.SelectedText = ""
        Textbox_QuantityInputEditReceiveItem.Size = New System.Drawing.Size(342, 40)
        Textbox_QuantityInputEditReceiveItem.Text = SelectedStock.StockQuantity
        Textbox_QuantityInputEditReceiveItem.TabIndex = 39
        '
        'FlowLayoutPanel_HasExpiryInputEditReceiveItem
        '
        FlowLayoutPanel_HasExpiryInputEditReceiveItem.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_HasExpiryInputEditReceiveItem.Controls.Add(Checkbox_HasExpiryInputEditReceiveItem)
        FlowLayoutPanel_HasExpiryInputEditReceiveItem.Controls.Add(Label_HasExpiryInputEditReceiveItem)
        FlowLayoutPanel_HasExpiryInputEditReceiveItem.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_HasExpiryInputEditReceiveItem.Location = New System.Drawing.Point(12, 268)
        FlowLayoutPanel_HasExpiryInputEditReceiveItem.Margin = New System.Windows.Forms.Padding(12, 0, 6, 12)
        FlowLayoutPanel_HasExpiryInputEditReceiveItem.Name = "FlowLayoutPanel_HasExpiryInputEditReceiveItem"
        FlowLayoutPanel_HasExpiryInputEditReceiveItem.Size = New System.Drawing.Size(342, 43)
        FlowLayoutPanel_HasExpiryInputEditReceiveItem.TabIndex = 42
        '
        'Checkbox_HasExpiryInputEditReceiveItem
        '
        Checkbox_HasExpiryInputEditReceiveItem.Anchor = System.Windows.Forms.AnchorStyles.Left
        Checkbox_HasExpiryInputEditReceiveItem.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Checkbox_HasExpiryInputEditReceiveItem.CheckedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Checkbox_HasExpiryInputEditReceiveItem.CheckedState.BorderRadius = 0
        Checkbox_HasExpiryInputEditReceiveItem.CheckedState.BorderThickness = 1
        Checkbox_HasExpiryInputEditReceiveItem.CheckedState.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Checkbox_HasExpiryInputEditReceiveItem.CheckMarkColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Checkbox_HasExpiryInputEditReceiveItem.Location = New System.Drawing.Point(12, 12)
        Checkbox_HasExpiryInputEditReceiveItem.Margin = New System.Windows.Forms.Padding(12)
        Checkbox_HasExpiryInputEditReceiveItem.Name = "Checkbox_HasExpiryInputEditReceiveItem"
        Checkbox_HasExpiryInputEditReceiveItem.Size = New System.Drawing.Size(18, 18)
        Checkbox_HasExpiryInputEditReceiveItem.TabIndex = 41
        Checkbox_HasExpiryInputEditReceiveItem.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Checkbox_HasExpiryInputEditReceiveItem.UncheckedState.BorderRadius = 0
        Checkbox_HasExpiryInputEditReceiveItem.UncheckedState.BorderThickness = 1
        Checkbox_HasExpiryInputEditReceiveItem.UncheckedState.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        '
        'Label_HasExpiryInputEditReceiveItem
        '
        Label_HasExpiryInputEditReceiveItem.Anchor = System.Windows.Forms.AnchorStyles.Left
        Label_HasExpiryInputEditReceiveItem.AutoSize = true
        Label_HasExpiryInputEditReceiveItem.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_HasExpiryInputEditReceiveItem.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_HasExpiryInputEditReceiveItem.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_HasExpiryInputEditReceiveItem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_HasExpiryInputEditReceiveItem.Location = New System.Drawing.Point(42, 13)
        Label_HasExpiryInputEditReceiveItem.Margin = New System.Windows.Forms.Padding(0, 13, 0, 0)
        Label_HasExpiryInputEditReceiveItem.Name = "Label_HasExpiryInputEditReceiveItem"
        Label_HasExpiryInputEditReceiveItem.Size = New System.Drawing.Size(71, 16)
        Label_HasExpiryInputEditReceiveItem.TabIndex = 2
        Label_HasExpiryInputEditReceiveItem.Text = "Has expiry?"
        Label_HasExpiryInputEditReceiveItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'FlowLayoutPanel_ExpirationDateInputEditReceiveItem
        '
        FlowLayoutPanel_ExpirationDateInputEditReceiveItem.AutoSize = true
        FlowLayoutPanel_ExpirationDateInputEditReceiveItem.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_ExpirationDateInputEditReceiveItem.Controls.Add(Label_ExpirationDateInputEditReceiveItem)
        FlowLayoutPanel_ExpirationDateInputEditReceiveItem.Controls.Add(DateTimePicker_ExpirationDateInputEditReceiveItem)
        FlowLayoutPanel_ExpirationDateInputEditReceiveItem.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_ExpirationDateInputEditReceiveItem.Location = New System.Drawing.Point(12, 323)
        FlowLayoutPanel_ExpirationDateInputEditReceiveItem.Margin = New System.Windows.Forms.Padding(12, 0, 6, 0)
        FlowLayoutPanel_ExpirationDateInputEditReceiveItem.Name = "FlowLayoutPanel_ExpirationDateInputEditReceiveItem"
        FlowLayoutPanel_ExpirationDateInputEditReceiveItem.Size = New System.Drawing.Size(342, 56)
        FlowLayoutPanel_ExpirationDateInputEditReceiveItem.TabIndex = 39
        FlowLayoutPanel_ExpirationDateInputEditReceiveItem.Visible = false
        '
        'Label_ExpirationDateInputEditReceiveItem
        '
        Label_ExpirationDateInputEditReceiveItem.AutoSize = true
        Label_ExpirationDateInputEditReceiveItem.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_ExpirationDateInputEditReceiveItem.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_ExpirationDateInputEditReceiveItem.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_ExpirationDateInputEditReceiveItem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_ExpirationDateInputEditReceiveItem.Location = New System.Drawing.Point(0, 0)
        Label_ExpirationDateInputEditReceiveItem.Margin = New System.Windows.Forms.Padding(0)
        Label_ExpirationDateInputEditReceiveItem.Name = "Label_ExpirationDateInputEditReceiveItem"
        Label_ExpirationDateInputEditReceiveItem.Size = New System.Drawing.Size(94, 16)
        Label_ExpirationDateInputEditReceiveItem.TabIndex = 2
        Label_ExpirationDateInputEditReceiveItem.Text = "Expiration Date"
        Label_ExpirationDateInputEditReceiveItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'DateTimePicker_ExpirationDateInputEditReceiveItem
        '
        DateTimePicker_ExpirationDateInputEditReceiveItem.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        DateTimePicker_ExpirationDateInputEditReceiveItem.BorderThickness = 1
        DateTimePicker_ExpirationDateInputEditReceiveItem.Checked = false
        DateTimePicker_ExpirationDateInputEditReceiveItem.CheckedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        DateTimePicker_ExpirationDateInputEditReceiveItem.CheckedState.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        DateTimePicker_ExpirationDateInputEditReceiveItem.CheckedState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        DateTimePicker_ExpirationDateInputEditReceiveItem.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        DateTimePicker_ExpirationDateInputEditReceiveItem.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
        DateTimePicker_ExpirationDateInputEditReceiveItem.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        DateTimePicker_ExpirationDateInputEditReceiveItem.Format = System.Windows.Forms.DateTimePickerFormat.[Long]
        DateTimePicker_ExpirationDateInputEditReceiveItem.Location = New System.Drawing.Point(0, 16)
        DateTimePicker_ExpirationDateInputEditReceiveItem.Margin = New System.Windows.Forms.Padding(0)
        DateTimePicker_ExpirationDateInputEditReceiveItem.MaxDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        DateTimePicker_ExpirationDateInputEditReceiveItem.MinDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        DateTimePicker_ExpirationDateInputEditReceiveItem.Name = "DateTimePicker_ExpirationDateInputEditReceiveItem"
        DateTimePicker_ExpirationDateInputEditReceiveItem.Size = New System.Drawing.Size(342, 40)
        DateTimePicker_ExpirationDateInputEditReceiveItem.TabIndex = 40
        '
        'Button_SaveChangesEditReceivedItem
        '
        Button_SaveChangesEditReceivedItem.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(44,Byte),Integer), CType(CType(62,Byte),Integer))
        Button_SaveChangesEditReceivedItem.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Button_SaveChangesEditReceivedItem.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Button_SaveChangesEditReceivedItem.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer))
        Button_SaveChangesEditReceivedItem.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer))
        Button_SaveChangesEditReceivedItem.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Button_SaveChangesEditReceivedItem.Font = New System.Drawing.Font("Segoe UI", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel)
        Button_SaveChangesEditReceivedItem.ForeColor = System.Drawing.Color.White
        Button_SaveChangesEditReceivedItem.Location = New System.Drawing.Point(12, 403)
        Button_SaveChangesEditReceivedItem.Margin = New System.Windows.Forms.Padding(12, 24, 12, 12)
        Button_SaveChangesEditReceivedItem.Name = "Button_SaveChangesEditReceivedItem"
        Button_SaveChangesEditReceivedItem.Size = New System.Drawing.Size(342, 40)
        Button_SaveChangesEditReceivedItem.TabIndex = 33
        Button_SaveChangesEditReceivedItem.Text = "Save Changes"
        Button_SaveChangesEditReceivedItem.TextOffset = New System.Drawing.Point(0, -2)
        '
        'Button_RemoveItemEditReceivedItem
        '
        Button_RemoveItemEditReceivedItem.BackColor = System.Drawing.Color.FromArgb(CType(CType(220,Byte),Integer), CType(CType(76,Byte),Integer), CType(CType(100,Byte),Integer))
        Button_RemoveItemEditReceivedItem.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Button_RemoveItemEditReceivedItem.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Button_RemoveItemEditReceivedItem.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer))
        Button_RemoveItemEditReceivedItem.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer))
        Button_RemoveItemEditReceivedItem.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Button_RemoveItemEditReceivedItem.Font = New System.Drawing.Font("Segoe UI", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel)
        Button_RemoveItemEditReceivedItem.ForeColor = System.Drawing.Color.White
        Button_RemoveItemEditReceivedItem.Location = New System.Drawing.Point(12, 455)
        Button_RemoveItemEditReceivedItem.Margin = New System.Windows.Forms.Padding(12, 0, 12, 12)
        Button_RemoveItemEditReceivedItem.Name = "Button_RemoveItemEditReceivedItem"
        Button_RemoveItemEditReceivedItem.Size = New System.Drawing.Size(342, 40)
        Button_RemoveItemEditReceivedItem.TabIndex = 43
        Button_RemoveItemEditReceivedItem.Text = "Remove Item"
        Button_RemoveItemEditReceivedItem.TextOffset = New System.Drawing.Point(0, -2)
        Prompt_EditReceiveItem.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        '
        '_Temporary_EditReceiveItems
        '
        Prompt_EditReceiveItem.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Prompt_EditReceiveItem.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Prompt_EditReceiveItem.ClientSize = New System.Drawing.Size(366, 517)
        Prompt_EditReceiveItem.Controls.Add(FlowLayoutPanel_EditReceiveItem)
        Prompt_EditReceiveItem.Name = "_Temporary_EditReceiveItems"
        Prompt_EditReceiveItem.Text = "_Temporary_EditReceiveItems"
        FlowLayoutPanel_EditReceiveItem.ResumeLayout(false)
        FlowLayoutPanel_EditReceiveItem.PerformLayout
        FlowLayoutPanel_PageHeaderEditReceiveItem.ResumeLayout(false)
        FlowLayoutPanel_PageHeaderEditReceiveItem.PerformLayout
        FlowLayoutPanel_BuyingPriceInputEditReceiveItem.ResumeLayout(false)
        FlowLayoutPanel_BuyingPriceInputEditReceiveItem.PerformLayout
        FlowLayoutPanel_QuantityInputEditReceiveItem.ResumeLayout(false)
        FlowLayoutPanel_QuantityInputEditReceiveItem.PerformLayout
        FlowLayoutPanel_HasExpiryInputEditReceiveItem.ResumeLayout(false)
        FlowLayoutPanel_HasExpiryInputEditReceiveItem.PerformLayout
        FlowLayoutPanel_ExpirationDateInputEditReceiveItem.ResumeLayout(false)
        FlowLayoutPanel_ExpirationDateInputEditReceiveItem.PerformLayout
        Prompt_EditReceiveItem.ResumeLayout(false)
        
        If SelectedStock.Expiry.HasValue Then
            DateTimePicker_ExpirationDateInputEditReceiveItem.Value = SelectedStock.Expiry
            Checkbox_HasExpiryInputEditReceiveItem.Checked = True
            FlowLayoutPanel_ExpirationDateInputEditReceiveItem.Visible = True
        Else
            DateTimePicker_ExpirationDateInputEditReceiveItem.Format = DateTimePickerFormat.Long
            Checkbox_HasExpiryInputEditReceiveItem.Checked = False
            FlowLayoutPanel_ExpirationDateInputEditReceiveItem.Visible = False
        End If
        

        AddHandler Checkbox_HasExpiryInputEditReceiveItem.Click, Sub(sender As Object, e As EventArgs)
            If Checkbox_HasExpiryInputEditReceiveItem.Checked Then
                FlowLayoutPanel_ExpirationDateInputEditReceiveItem.Visible = True
            Else
                FlowLayoutPanel_ExpirationDateInputEditReceiveItem.Visible = false
            End If
        End Sub

        AddHandler Button_SaveChangesEditReceivedItem.Click, Sub(sender As Object, e As EventArgs)
            SelectedStock.StockQuantity = Textbox_QuantityInputEditReceiveItem.Text
            SelectedStock.ItemBuyingPrice = Textbox_BuyingPriceInputEditReceiveItem.Text
            SelectedStock.TotalBuyingPrice = SelectedStock.StockQuantity * SelectedStock.ItemBuyingPrice

            If Checkbox_HasExpiryInputEditReceiveItem.Checked Then
                SelectedStock.Expiry = DateTimePicker_ExpirationDateInputEditReceiveItem.Value
            End If

            UpdateTransactionItemsTable()
            Prompt_EditReceiveItem.Close()
        End Sub

        AddHandler Button_RemoveItemEditReceivedItem.Click, Sub(sender As Object, e As EventArgs)
            CurrentTransactionItems.Remove(SelectedStock)
            Prompt_EditReceiveItem.Close()

            Dim subtotal As Decimal = 0D
            For Each i In CurrentTransactionItems
                subTotal += i.TotalBuyingPrice
            Next
            CurrentTransactionEntry.NetTotal = subtotal
            Label_NetTotalSaleScreen.Text = CurrentTransactionEntry.NetTotal
            UpdateTransactionItemsTable()
            Prompt_EditReceiveItem.Close()
        End Sub

        Prompt_EditReceiveItem.ShowDialog(Me)
    End Sub

    Private Sub ShowOutvoiceReceiptPrompt()
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

        For Each stock In CurrentTransactionItems
            Dim item = Items.FirstOrDefault(Function(i) i.ItemId = stock.ItemId)
            Dim itemName As String = If(item IsNot Nothing, item.ItemName, "Unknown Item")

            Dim qty As String = stock.StockQuantity.ToString().PadRight(20)
            Dim name As String = itemName.PadRight(30)
            Dim amount As String = stock.TotalBuyingPrice.ToString("C").PadLeft(10)

            output.AppendLine($"{qty}{name}{amount}")
        Next

        Dim SelectedVendor = Vendors.FirstOrDefault(Function(v) v.VendorName = ComboBox_VendorStockOutvoicePlaceOrder.SelectedItem.ToString())
        CurrentTransactionEntry.VendorId = SelectedVendor.VendorId

        Label3.Text = "Outvoice ID: " & CurrentTransactionEntry.StockEntryId & Environment.NewLine & _
        "Vendor: " & CurrentTransactionEntry.VendorId & " - "  & SelectedVendor.VendorName &  Environment.NewLine & _
        "Created By: " & CurrentTransactionEntry.CreatedBy & Environment.NewLine & _
        "Created At: " & CurrentTransactionEntry.CreatedAt.ToString("MM/dd/yyyy hh:mm tt") & Environment.NewLine
        Label5.Text = output.ToString()
        Label8.Text = "Net Total: " & CurrentTransactionEntry.NetTotal

        Prompt_OutvoiceReceipt.ShowDialog(Me)
    End Sub

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
        FlowLayoutPanel_VendorDetailZipCodeInputNewVendor.Margin = New System.Windows.Forms.Padding(12, 0, 6, 24)
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
        FlowLayoutPanel_VendorDetailCountryInputNewVendor.Margin = New System.Windows.Forms.Padding(6, 0, 12, 24)
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
        Button_ResetNewVendor.Location = New System.Drawing.Point(12, 416)
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
        Button_MakeVendorNewVendor.Location = New System.Drawing.Point(366, 416)
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
        FlowLayoutPanel_DateRangeInputsSales.Location = New System.Drawing.Point(720, 416)
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
        Prompt_NewVendor.Text = "New Vendor"
        Prompt_NewVendor.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen

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

        AddHandler Button_ResetNewVendor.Click, Sub(sender As Object, e As EventArgs)
            Textbox_VendorDetailVendorNameInputNewVendor.Text = ""
            Textbox_VendorDetailContactPersonNameInputNewVendor.Text = ""
            Textbox_VendorDetailContactPersonNameInputNewVendor.Text = ""
            Textbox_VendorDetailEmailInputNewVendor.Text = ""
            Textbox_VendorDetailPhoneInputNewVendor.Text = ""
            Textbox_VendorDetailAddressLineInputNewVendor.Text = ""
            Textbox_VendorDetailCityInputNewVendor.Text = ""
            Textbox_VendorDetailZipCodeInputNewVendor.Text = ""
            Textbox_VendorDetailCountryInputNewVendor.Text = ""

            Textbox_VendorDetailVendorNameInputNewVendor.BorderColor = Color.Black
            Textbox_VendorDetailContactPersonNameInputNewVendor.BorderColor = Color.Black
            Textbox_VendorDetailContactPersonNameInputNewVendor.BorderColor = Color.Black
            Textbox_VendorDetailEmailInputNewVendor.BorderColor = Color.Black
            Textbox_VendorDetailPhoneInputNewVendor.BorderColor = Color.Black
            Textbox_VendorDetailAddressLineInputNewVendor.BorderColor = Color.Black
            Textbox_VendorDetailCityInputNewVendor.BorderColor = Color.Black
            Textbox_VendorDetailZipCodeInputNewVendor.BorderColor = Color.Black
            Textbox_VendorDetailCountryInputNewVendor.BorderColor = Color.Black
        End Sub

        AddHandler Button_MakeVendorNewVendor.Click, Sub(sender As Object, e As EventArgs)
            Dim vendorName As String = Textbox_VendorDetailVendorNameInputNewVendor.Text.Trim()
            Dim contactPerson As String = Textbox_VendorDetailContactPersonNameInputNewVendor.Text.Trim()
            Dim email As String = Textbox_VendorDetailEmailInputNewVendor.Text.Trim()
            Dim phone As String = Textbox_VendorDetailPhoneInputNewVendor.Text.Trim()
            Dim addressLine As String = Textbox_VendorDetailAddressLineInputNewVendor.Text.Trim()
            Dim city As String = Textbox_VendorDetailCityInputNewVendor.Text.Trim()
            Dim zipCode As String = Textbox_VendorDetailZipCodeInputNewVendor.Text.Trim()
            Dim country As String = Textbox_VendorDetailCountryInputNewVendor.Text.Trim()

            Dim isValid As Boolean = True

            If String.IsNullOrWhiteSpace(vendorName) Then
                Textbox_VendorDetailVendorNameInputNewVendor.BorderColor = Color.Red
                isValid = False
            Else
                Textbox_VendorDetailVendorNameInputNewVendor.BorderColor = Color.Gray
            End If

            If Not String.IsNullOrEmpty(email) AndAlso Not Regex.IsMatch(email, "^\S+@\S+\.\S+$") Then
                Textbox_VendorDetailEmailInputNewVendor.BorderColor = Color.Red
                isValid = False
            Else
                Textbox_VendorDetailEmailInputNewVendor.BorderColor = Color.Gray
            End If

            Textbox_VendorDetailContactPersonNameInputNewVendor.BorderColor = If(String.IsNullOrWhiteSpace(contactPerson), Color.Red, Color.Black)
            Textbox_VendorDetailPhoneInputNewVendor.BorderColor = If(String.IsNullOrWhiteSpace(phone), Color.Red, Color.Black)
            Textbox_VendorDetailAddressLineInputNewVendor.BorderColor = If(String.IsNullOrWhiteSpace(addressLine), Color.Red, Color.Black)
            Textbox_VendorDetailCityInputNewVendor.BorderColor = If(String.IsNullOrWhiteSpace(city), Color.Red, Color.Black)
            Textbox_VendorDetailZipCodeInputNewVendor.BorderColor = If(String.IsNullOrWhiteSpace(zipCode), Color.Red, Color.Black)
            Textbox_VendorDetailCountryInputNewVendor.BorderColor = If(String.IsNullOrWhiteSpace(country), Color.Red, Color.Black)

            If Not isValid Then
                MessageBox.Show("Please fill out all required fields correctly.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Using conn As New MySqlConnection(connectionString)
                Try
                    conn.Open()
                    Dim query As String = "INSERT INTO vendors (vendor_name, contact_person, email, phone, address_line, city, zip_code, country, created_by) " &
                                          "VALUES (@vendor_name, @contact_person, @email, @phone, @address_line, @city, @zip_code, @country, @created_by)"

                    Using cmd As New MySqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@vendor_name", vendorName)
                        cmd.Parameters.AddWithValue("@contact_person", If(String.IsNullOrWhiteSpace(contactPerson), DBNull.Value, contactPerson))
                        cmd.Parameters.AddWithValue("@email", If(String.IsNullOrWhiteSpace(email), DBNull.Value, email))
                        cmd.Parameters.AddWithValue("@phone", If(String.IsNullOrWhiteSpace(phone), DBNull.Value, phone))
                        cmd.Parameters.AddWithValue("@address_line", If(String.IsNullOrWhiteSpace(addressLine), DBNull.Value, addressLine))
                        cmd.Parameters.AddWithValue("@city", If(String.IsNullOrWhiteSpace(city), DBNull.Value, city))
                        cmd.Parameters.AddWithValue("@zip_code", If(String.IsNullOrWhiteSpace(zipCode), DBNull.Value, zipCode))
                        cmd.Parameters.AddWithValue("@country", If(String.IsNullOrWhiteSpace(country), DBNull.Value, country))
                        cmd.Parameters.AddWithValue("@created_by", GlobalShared.CurrentUser.AccountId)

                        cmd.ExecuteNonQuery()
                    End Using

                    MessageBox.Show("Vendor added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Prompt_NewVendor.Close()
                    PopulateVendorComboBox()
                Catch ex As MySqlException
                    MessageBox.Show($"An error occurred while inserting the vendor: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Finally
                    conn.Close()
                End Try
            End Using
        End Sub

        Prompt_NewVendor.ShowDialog(Me)
    End Sub



    Private Sub Window_AddStock_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If GlobalShared.CurrentUser.Type = "sales_representative" Then
            Button_NavigationItemSales.Visible = False
            Button_NavigationItemCustomers.Visible = False
            Button_NavigationItemStocks.Visible = False
            Button_NavigationItemBrandsAndCategories.Visible = False
            Button_NavigationItemVendors.Visible = False
        Else
            Button_NavigationItemSales.Visible = True
            Button_NavigationItemCustomers.Visible = True
            Button_NavigationItemStocks.Visible = True
            Button_NavigationItemBrandsAndCategories.Visible = True
            Button_NavigationItemVendors.Visible = True
        End If
        
        UpdateAvailableQuantities()
        PopulateCategoriesSelection()
        PopulateItemsGrid(CurrentCategoryID)
        PopulateVendorComboBox()

        Label_UserName.Text = GlobalShared.CurrentUser.Name
        Label_UserEmail.Text = GlobalShared.CurrentUser.Email
    End Sub



    Private Sub Button_ToPayNewSale_Click(sender As Object, e As EventArgs) Handles Button_ToPayNewSale.Click
        If ComboBox_VendorStockOutvoicePlaceOrder.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a vendor first.", "Vendor Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim SelectedVendor = Vendors.FirstOrDefault(Function(v) v.VendorName = ComboBox_VendorStockOutvoicePlaceOrder.SelectedItem.ToString())
        If SelectedVendor IsNot Nothing Then
            CurrentTransactionEntry.VendorId = SelectedVendor.VendorId
        Else
            MessageBox.Show("Selected vendor is not valid.", "Invalid Vendor", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If
        Dim nextStockEntryId As Integer = 0
        Dim query As String = "SELECT MAX(stock_entry_id) FROM stock_entries"

        Using connection As New MySqlConnection(ConnectionString)
            Using command As New MySqlCommand(query, connection)
                connection.Open()

                Dim result = command.ExecuteScalar()
                If result IsNot Nothing Then
                    If Integer.TryParse(result.ToString(), nextStockEntryId) Then
                        nextStockEntryId += 1
                    Else
                        nextStockEntryId = 1
                    End If
                Else
                    nextStockEntryId = 1
                End If
            End Using
        End Using

        For Each item In CurrentTransactionItems
            item.StockEntryId = CurrentTransactionEntry.StockEntryId
            item.Status = "Unavailable"
            item.DisplayDetails()
            Console.WriteLine("------------------------")
        Next

        CurrentTransactionEntry.StockEntryId = nextStockEntryId
        CurrentTransactionEntry.Status = "Disapproved"
        CurrentTransactionEntry.CreatedBy = GlobalShared.CurrentUser.AccountId
        CurrentTransactionEntry.CreatedAt = DateTime.Now

        Using connection As New MySqlConnection(ConnectionString)
            connection.Open()

            Using transaction = connection.BeginTransaction()
                Try
                    Dim stockEntryQuery As String = "INSERT INTO stock_entries (stock_entry_id, vendor_id, status, net_total, created_at, created_by) 
                                                     VALUES (@StockEntryId, @VendorId, @Status, @NetTotal, @CreatedAt, @CreatedBy)"
                    Using stockEntryCommand As New MySqlCommand(stockEntryQuery, connection, transaction)
                        stockEntryCommand.Parameters.AddWithValue("@StockEntryId", CurrentTransactionEntry.StockEntryId)
                        stockEntryCommand.Parameters.AddWithValue("@VendorId", CurrentTransactionEntry.VendorId)
                        stockEntryCommand.Parameters.AddWithValue("@Status", CurrentTransactionEntry.Status)
                        stockEntryCommand.Parameters.AddWithValue("@NetTotal", CurrentTransactionEntry.NetTotal)
                        stockEntryCommand.Parameters.AddWithValue("@CreatedAt", CurrentTransactionEntry.CreatedAt)
                        stockEntryCommand.Parameters.AddWithValue("@CreatedBy", CurrentTransactionEntry.CreatedBy)
                        stockEntryCommand.ExecuteNonQuery()
                    End Using

                    ' Insert into stocks
                    Dim stockQuery As String = "INSERT INTO stocks (stock_entry_id, stock_quantity, item_buying_price, total_buying_price, item_id, expiry, status) 
                                                VALUES (@StockEntryId, @StockQuantity, @ItemBuyingPrice, @TotalBuyingPrice, @ItemId, @Expiry, @Status)"
                    For Each item In CurrentTransactionItems
                        Using stockCommand As New MySqlCommand(stockQuery, connection, transaction)
                            stockCommand.Parameters.AddWithValue("@StockEntryId", CurrentTransactionEntry.StockEntryId)
                            stockCommand.Parameters.AddWithValue("@StockQuantity", item.StockQuantity)
                            stockCommand.Parameters.AddWithValue("@ItemBuyingPrice", item.ItemBuyingPrice)
                            stockCommand.Parameters.AddWithValue("@TotalBuyingPrice", item.TotalBuyingPrice)
                            stockCommand.Parameters.AddWithValue("@ItemId", item.ItemId)
                            stockCommand.Parameters.AddWithValue("@Expiry", If(item.Expiry.HasValue, CType(item.Expiry, Object), DBNull.Value))
                            stockCommand.Parameters.AddWithValue("@Status", item.Status)
                            stockCommand.ExecuteNonQuery()
                        End Using
                    Next

                    transaction.Commit()
                Catch ex As Exception
                    transaction.Rollback()
                    Throw
                End Try
            End Using
        End Using

        MessageBox.Show("Transaction successfully added!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

        ShowOutvoiceReceiptPrompt()
    End Sub

    Private Sub Button_NewCustomerStockPlaceOrder_Click(sender As Object, e As EventArgs) 
        ShowNewVendorPrompt()
    End Sub

    Private Sub Button_NewCustomerStockPlaceOrder_Click_1(sender As Object, e As EventArgs) Handles Button_NewCustomerStockPlaceOrder.Click
        ShowNewVendorPrompt()
    End Sub

    Private Sub Button_Logout_Click(sender As Object, e As EventArgs) Handles Button_Logout.Click
        Dim result As DialogResult = MessageBox.Show("Are you sure you want to log out?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            Dim newForm As New Window_Login()
            newForm.Show()
            Me.Close()
        End If
    End Sub

    Private Sub Textbox_SearchBarBrands_TextChanged(sender As Object, e As EventArgs) Handles Textbox_SearchBarBrands.TextChanged
        SearchItemsGrid(CurrentCategoryID, Textbox_SearchBarBrands.Text.ToString())
    End Sub

    Private Sub Button_ResetSaleScreeen_Click(sender As Object, e As EventArgs) Handles Button_ResetSaleScreeen.Click
        Dim newForm As New Window_AddStock()
        newForm.Show()
        Me.Close()
    End Sub
End Class