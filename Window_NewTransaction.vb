Imports Guna.UI2.WinForms
Imports MySql.Data.MySqlClient
Imports System.Text
Imports System.Text.RegularExpressions

' unavailable pa yung status kaya di gumagana pag nagiinsert ng iabng items sa sale entry.

Public Class Window_NewTransaction
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
    Public Customers As Customer() = CreateCustomerArray()
    Public PaymentTerms As PaymentTerm() = CreatePaymentTermArray()
    Public Taxes As Tax() = CreateTaxArray()
    

    Private CategoriesPerPage As Integer = 4
    Private CurrentCategoryPage As Integer = 1
    Private CurrentCategoryID As Integer = -1

    Private TransactionItemsTableCurrentPage As Integer = 1
    Private TransactionItemsTableItemsPerPage As Integer = 4
    Private TransactionItemsTableTotalPagess As Integer = 1
    Private ItemsPerPage As Integer = 15
    Private CurrentPage As Integer = 1

    Public CurrentTransactionItems As New List(Of TransactionItem)()
    Public CurrentSelectedTransactionItemIDs As New List(Of Integer)()
    Public CurrentTransactionEntry As New TransactionEntry()



    ' Populate combo box items
    Public Sub PopulateCustomerComboBox(comboBox As Guna.UI2.WinForms.Guna2ComboBox)
        comboBox.Items.Clear()

        For Each customer As Customer In Customers
            comboBox.Items.Add(customer.Name)
        Next
    End Sub

    Public Sub PopulatePaymentTermComboBox(comboBox As Guna.UI2.WinForms.Guna2ComboBox)
        comboBox.Items.Clear()

        For Each paymentTerm As PaymentTerm In PaymentTerms
            comboBox.Items.Add(paymentTerm.TermName)
        Next
    End Sub

    Public Sub PopulateTaxComboBox(comboBox As Guna.UI2.WinForms.Guna2ComboBox)
        comboBox.Items.Clear()

        For Each tax As Tax In Taxes
            comboBox.Items.Add(tax.TaxName)
        Next
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

    Public Function CreateTaxArray() As Tax()
        Dim taxes As New List(Of Tax)()

        Using connection As New MySqlConnection(ConnectionString)
            Dim query As String = "SELECT tax_id, tax_name, tax_percentage, created_by, created_at FROM taxes"
            Dim command As New MySqlCommand(query, connection)

            connection.Open()
            Using reader As MySqlDataReader = command.ExecuteReader()
                While reader.Read()
                    Dim taxId As Integer = reader.GetInt32("tax_id")
                    Dim taxName As String = reader.GetString("tax_name")
                    Dim taxPercentage As Decimal = reader.GetDecimal("tax_percentage")
                    Dim createdBy As Integer? = If(reader.IsDBNull(reader.GetOrdinal("created_by")), CType(Nothing, Integer?), reader.GetInt32("created_by"))
                    Dim createdAt As DateTime = reader.GetDateTime("created_at")

                    Dim tax As New Tax(taxId, taxName, taxPercentage, createdBy, createdAt)
                    taxes.Add(tax)
                End While
            End Using
        End Using

        Return taxes.ToArray()
    End Function









    ' Populate categories section
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



    ' Populate item grid
    Private Sub PopulateItemsGrid(CategoryID As Integer, Optional searchItemName As String = Nothing)
        Dim filteredItems As List(Of Item)

        If String.IsNullOrEmpty(searchItemName) Then
            If CategoryID = -1 Then
                filteredItems = Items.ToList()
            Else
                filteredItems = Items.Where(Function(item) item.CategoryId = CategoryID).ToList()
            End If
        Else
            If CategoryID = -1 Then
                filteredItems = Items.Where(Function(item) item.ItemName.IndexOf(searchItemName, StringComparison.OrdinalIgnoreCase) >= 0).ToList()
            Else
                filteredItems = Items.Where(Function(item) item.CategoryId = CategoryID AndAlso 
                                                      item.ItemName.IndexOf(searchItemName, StringComparison.OrdinalIgnoreCase) >= 0).ToList()
            End If
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
                .TextAlign = ContentAlignment.MiddleLeft,
                .Text = ButtonText,
                .Size = New Size(118, 180),
                .Margin = New Padding(0),
                .FillColor = Color.FromArgb(0, 72, 101)
            }

            If AvailableStocksTotal = 0 Then
                AddHandler button.Click, 
                Sub(sender, e)
                    MessageBox.Show("There are no available stocks left for this item.")
                End Sub
            Else
                AddHandler button.Click, 
                Sub(sender, e)
                    Dim ItemID = item.ItemId
                    UpdateCurrentTransactionData(ItemID, item)
                    UpdateTransactionItemsTable()
                End Sub
            End If

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



    ' Transaction items
    Public Sub UpdateCurrentTransactionData(ItemID As Integer, SelectedItem As Item)
        Dim SearchedTransactionItem = CurrentTransactionItems.FirstOrDefault(Function(s) s.ItemId = ItemID)

        If SearchedTransactionItem IsNot Nothing Then
            SearchedTransactionItem.TransactionItemQuantity += 1
            SearchedTransactionItem.TransactionItemAmount = SearchedTransactionItem.TransactionItemQuantity * SearchedTransactionItem.TransactionItemSellingPrice
        Else
            Dim transactionItem As New TransactionItem() With {
                .ItemId = SelectedItem.ItemId,
                .TransactionItemQuantity = 1,
                .TransactionItemSellingPrice = SelectedItem.SellingPrice,
                .TransactionItemAmount = SelectedItem.SellingPrice * 1
            }

            CurrentTransactionItems.Add(transactionItem)
        End If

        ' Get subtotal of the transaction
        Dim subtotal As Decimal = 0D
        For Each i In CurrentTransactionItems
            subTotal += i.TransactionItemAmount
        Next
        CurrentTransactionEntry.Subtotal = subtotal
        Label_SubTotalSaleScreen.Text = CurrentTransactionEntry.Subtotal
    End Sub

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
            TableLayoutPanel_TransactionItemsTableNewSale.RowStyles.Add(New RowStyle(SizeType.Absolute, 74))
        Next

        Dim StartIndex As Integer = (TransactionItemsTableCurrentPage - 1) * TransactionItemsTableItemsPerPage
        Dim EndIndex As Integer = Math.Min(StartIndex + TransactionItemsTableItemsPerPage - 1, CurrentTransactionItems.Count - 1)

        For i As Integer = StartIndex To EndIndex
            Dim transactionItem = CurrentTransactionItems(i)
            Dim item = Items.FirstOrDefault(Function(s) s.ItemId = transactionItem.ItemID)

            Dim editButton As New Guna.UI2.WinForms.Guna2Button With {
                .Anchor = AnchorStyles.Left,
                .BackgroundImage = Global.ERP_SIMS.My.Resources.Resources.Button_TableEdit,
                .BackColor = Color.Transparent,
                .FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer)),
                .Margin = New System.Windows.Forms.Padding(4, 4, 0, 4),
                .Size = New System.Drawing.Size(34, 32)
            }

            AddHandler editButton.Click, Sub(sender As Object, e As EventArgs)
                ShowEditTransactionItemPrompt(transactionItem)
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
                .Text = transactionItem.TransactionItemQuantity,
                .TextAlign = ContentAlignment.MiddleLeft
            }
            Dim labelBuyingPrice As New Label With {
                .Anchor = AnchorStyles.Left,
                .AutoSize = True,
                .BackColor = Color.White,
                .Font = New Font("Microsoft JhengHei", 12, FontStyle.Bold, GraphicsUnit.Pixel),
                .ForeColor = Color.FromArgb(2, 34, 34),
                .Margin = New Padding(12, 6, 12, 6),
                .Text = transactionItem.TransactionItemSellingPrice,
                .TextAlign = ContentAlignment.MiddleLeft
            }
            Dim labelAmount As New Label With {
                .Anchor = AnchorStyles.Left,
                .AutoSize = True,
                .BackColor = Color.White,
                .Font = New Font("Microsoft JhengHei", 12, FontStyle.Bold, GraphicsUnit.Pixel),
                .ForeColor = Color.FromArgb(2, 34, 34),
                .Margin = New Padding(12, 6, 12, 6),
                .Text = transactionItem.TransactionItemAmount,
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



    ' Show prompts


    Private Sub ShowEditTransactionItemPrompt(SelectedTransactionItem As TransactionItem)
        Dim item = Items.FirstOrDefault(Function(ti) ti.ItemId = SelectedTransactionItem.ItemID)

        Dim Prompt_EditTransactionItem As New Form()
        Dim FlowLayoutPanel_EditTransactionItem = New System.Windows.Forms.FlowLayoutPanel()
        Dim FlowLayoutPanel_PageHeaderEditTransactionItem = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label_PageHeaderNewBrand = New System.Windows.Forms.Label()
        Dim Label_ItemNameEditTransactionItem = New System.Windows.Forms.Label()
        Dim FlowLayoutPanel_DateRangeInputsSales = New System.Windows.Forms.FlowLayoutPanel()
        Dim FlowLayoutPanel_QuantityInputEditTransactionItem = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label_QuantityInputEditTransactionItem = New System.Windows.Forms.Label()
        Dim Textbox_QuantityInputEditTransactionItem = New Guna.UI2.WinForms.Guna2TextBox()
        Dim Button_SaveChangesEditReceivedItem = New Guna.UI2.WinForms.Guna2Button()
        Dim Button_RemoveItemEditReceivedItem = New Guna.UI2.WinForms.Guna2Button()
        Dim Panel_PageHeaderEditTransactionItem = New System.Windows.Forms.Panel()
        FlowLayoutPanel_EditTransactionItem.SuspendLayout
        FlowLayoutPanel_PageHeaderEditTransactionItem.SuspendLayout
        FlowLayoutPanel_QuantityInputEditTransactionItem.SuspendLayout
        SuspendLayout
        '
        'FlowLayoutPanel_EditTransactionItem
        '
        FlowLayoutPanel_EditTransactionItem.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        FlowLayoutPanel_EditTransactionItem.Controls.Add(FlowLayoutPanel_PageHeaderEditTransactionItem)
        FlowLayoutPanel_EditTransactionItem.Controls.Add(Label_ItemNameEditTransactionItem)
        FlowLayoutPanel_EditTransactionItem.Controls.Add(FlowLayoutPanel_DateRangeInputsSales)
        FlowLayoutPanel_EditTransactionItem.Controls.Add(FlowLayoutPanel_QuantityInputEditTransactionItem)
        FlowLayoutPanel_EditTransactionItem.Controls.Add(Button_SaveChangesEditReceivedItem)
        FlowLayoutPanel_EditTransactionItem.Controls.Add(Button_RemoveItemEditReceivedItem)
        FlowLayoutPanel_EditTransactionItem.Location = New System.Drawing.Point(0, -2)
        FlowLayoutPanel_EditTransactionItem.Margin = New System.Windows.Forms.Padding(0)
        FlowLayoutPanel_EditTransactionItem.Name = "FlowLayoutPanel_EditTransactionItem"
        FlowLayoutPanel_EditTransactionItem.Size = New System.Drawing.Size(366, 520)
        FlowLayoutPanel_EditTransactionItem.TabIndex = 36
        '
        'FlowLayoutPanel_PageHeaderEditTransactionItem
        '
        FlowLayoutPanel_PageHeaderEditTransactionItem.AutoSize = true
        FlowLayoutPanel_PageHeaderEditTransactionItem.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_PageHeaderEditTransactionItem.Controls.Add(Panel_PageHeaderEditTransactionItem)
        FlowLayoutPanel_PageHeaderEditTransactionItem.Controls.Add(Label_PageHeaderNewBrand)
        FlowLayoutPanel_PageHeaderEditTransactionItem.Location = New System.Drawing.Point(12, 24)
        FlowLayoutPanel_PageHeaderEditTransactionItem.Margin = New System.Windows.Forms.Padding(12, 24, 0, 24)
        FlowLayoutPanel_PageHeaderEditTransactionItem.Name = "FlowLayoutPanel_PageHeaderEditTransactionItem"
        FlowLayoutPanel_PageHeaderEditTransactionItem.Size = New System.Drawing.Size(340, 32)
        FlowLayoutPanel_PageHeaderEditTransactionItem.TabIndex = 0
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
        Label_PageHeaderNewBrand.Size = New System.Drawing.Size(292, 31)
        Label_PageHeaderNewBrand.TabIndex = 2
        Label_PageHeaderNewBrand.Text = "Transaction Item Details"
        Label_PageHeaderNewBrand.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label_ItemNameEditTransactionItem
        '
        Label_ItemNameEditTransactionItem.Anchor = System.Windows.Forms.AnchorStyles.Left
        Label_ItemNameEditTransactionItem.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_ItemNameEditTransactionItem.Font = New System.Drawing.Font("Microsoft JhengHei", 22!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_ItemNameEditTransactionItem.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Label_ItemNameEditTransactionItem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_ItemNameEditTransactionItem.Location = New System.Drawing.Point(12, 80)
        Label_ItemNameEditTransactionItem.Margin = New System.Windows.Forms.Padding(12, 0, 12, 24)
        Label_ItemNameEditTransactionItem.Name = "Label_ItemNameEditTransactionItem"
        Label_ItemNameEditTransactionItem.Size = New System.Drawing.Size(342, 28)
        Label_ItemNameEditTransactionItem.TabIndex = 18
        Label_ItemNameEditTransactionItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
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
        'FlowLayoutPanel_QuantityInputEditTransactionItem
        '
        FlowLayoutPanel_QuantityInputEditTransactionItem.AutoSize = true
        FlowLayoutPanel_QuantityInputEditTransactionItem.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_QuantityInputEditTransactionItem.Controls.Add(Label_QuantityInputEditTransactionItem)
        FlowLayoutPanel_QuantityInputEditTransactionItem.Controls.Add(Textbox_QuantityInputEditTransactionItem)
        FlowLayoutPanel_QuantityInputEditTransactionItem.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_QuantityInputEditTransactionItem.Location = New System.Drawing.Point(12, 132)
        FlowLayoutPanel_QuantityInputEditTransactionItem.Margin = New System.Windows.Forms.Padding(12, 0, 6, 12)
        FlowLayoutPanel_QuantityInputEditTransactionItem.Name = "FlowLayoutPanel_QuantityInputEditTransactionItem"
        FlowLayoutPanel_QuantityInputEditTransactionItem.Size = New System.Drawing.Size(342, 56)
        FlowLayoutPanel_QuantityInputEditTransactionItem.TabIndex = 40
        '
        'Label_QuantityInputEditTransactionItem
        '
        Label_QuantityInputEditTransactionItem.AutoSize = true
        Label_QuantityInputEditTransactionItem.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_QuantityInputEditTransactionItem.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_QuantityInputEditTransactionItem.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_QuantityInputEditTransactionItem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_QuantityInputEditTransactionItem.Location = New System.Drawing.Point(0, 0)
        Label_QuantityInputEditTransactionItem.Margin = New System.Windows.Forms.Padding(0)
        Label_QuantityInputEditTransactionItem.Name = "Label_QuantityInputEditTransactionItem"
        Label_QuantityInputEditTransactionItem.Size = New System.Drawing.Size(55, 16)
        Label_QuantityInputEditTransactionItem.TabIndex = 2
        Label_QuantityInputEditTransactionItem.Text = "Quantity"
        Label_QuantityInputEditTransactionItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Textbox_QuantityInputEditTransactionItem
        '
        Textbox_QuantityInputEditTransactionItem.Anchor = System.Windows.Forms.AnchorStyles.Left
        Textbox_QuantityInputEditTransactionItem.AutoCompleteCustomSource.AddRange(New String() {"(001) Item 1", "(002) Item 2", "(003) Item 3", "(004) Item 4", "(005) Item 5", "(006) Item 6", "(007) Item 7", "(008) Item 8", "(009) Item 9", "(010) Item 10"})
        Textbox_QuantityInputEditTransactionItem.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Textbox_QuantityInputEditTransactionItem.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Textbox_QuantityInputEditTransactionItem.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_QuantityInputEditTransactionItem.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_QuantityInputEditTransactionItem.Cursor = System.Windows.Forms.Cursors.IBeam
        Textbox_QuantityInputEditTransactionItem.DefaultText = ""
        Textbox_QuantityInputEditTransactionItem.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer))
        Textbox_QuantityInputEditTransactionItem.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer))
        Textbox_QuantityInputEditTransactionItem.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_QuantityInputEditTransactionItem.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_QuantityInputEditTransactionItem.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_QuantityInputEditTransactionItem.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_QuantityInputEditTransactionItem.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
        Textbox_QuantityInputEditTransactionItem.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_QuantityInputEditTransactionItem.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_QuantityInputEditTransactionItem.Location = New System.Drawing.Point(0, 16)
        Textbox_QuantityInputEditTransactionItem.Margin = New System.Windows.Forms.Padding(0)
        Textbox_QuantityInputEditTransactionItem.Name = "Textbox_QuantityInputEditTransactionItem"
        Textbox_QuantityInputEditTransactionItem.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Textbox_QuantityInputEditTransactionItem.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer))
        Textbox_QuantityInputEditTransactionItem.PlaceholderText = "Required"
        Textbox_QuantityInputEditTransactionItem.SelectedText = ""
        Textbox_QuantityInputEditTransactionItem.Size = New System.Drawing.Size(342, 40)
        Textbox_QuantityInputEditTransactionItem.TabIndex = 39
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
        Button_SaveChangesEditReceivedItem.Location = New System.Drawing.Point(12, 224)
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
        Button_RemoveItemEditReceivedItem.Location = New System.Drawing.Point(12, 276)
        Button_RemoveItemEditReceivedItem.Margin = New System.Windows.Forms.Padding(12, 0, 12, 12)
        Button_RemoveItemEditReceivedItem.Name = "Button_RemoveItemEditReceivedItem"
        Button_RemoveItemEditReceivedItem.Size = New System.Drawing.Size(342, 40)
        Button_RemoveItemEditReceivedItem.TabIndex = 43
        Button_RemoveItemEditReceivedItem.Text = "Remove Item"
        Button_RemoveItemEditReceivedItem.TextOffset = New System.Drawing.Point(0, -2)
        '
        'Panel_PageHeaderEditTransactionItem
        '
        Panel_PageHeaderEditTransactionItem.Anchor = System.Windows.Forms.AnchorStyles.Left
        Panel_PageHeaderEditTransactionItem.BackgroundImage = Global.ERP_SIMS.My.Resources.Resources.HeaderIcon_Stocks
        Panel_PageHeaderEditTransactionItem.Location = New System.Drawing.Point(0, 0)
        Panel_PageHeaderEditTransactionItem.Margin = New System.Windows.Forms.Padding(0, 0, 16, 0)
        Panel_PageHeaderEditTransactionItem.Name = "Panel_PageHeaderEditTransactionItem"
        Panel_PageHeaderEditTransactionItem.Size = New System.Drawing.Size(32, 32)
        Panel_PageHeaderEditTransactionItem.TabIndex = 3
        '
        '_Temporary_EditTransactionItem
        '
        Prompt_EditTransactionItem.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Prompt_EditTransactionItem.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Prompt_EditTransactionItem.ClientSize = New System.Drawing.Size(366, 517)
        Prompt_EditTransactionItem.Controls.Add(FlowLayoutPanel_EditTransactionItem)
        Prompt_EditTransactionItem.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        FlowLayoutPanel_EditTransactionItem.ResumeLayout(false)
        FlowLayoutPanel_EditTransactionItem.PerformLayout
        FlowLayoutPanel_PageHeaderEditTransactionItem.ResumeLayout(false)
        FlowLayoutPanel_PageHeaderEditTransactionItem.PerformLayout
        FlowLayoutPanel_QuantityInputEditTransactionItem.ResumeLayout(false)
        FlowLayoutPanel_QuantityInputEditTransactionItem.PerformLayout
        ResumeLayout(false)

        Label_ItemNameEditTransactionItem.Text = item.ItemId & " - " & item.ItemName
        Textbox_QuantityInputEditTransactionItem.Text = SelectedTransactionItem.TransactionItemQuantity

        AddHandler Button_SaveChangesEditReceivedItem.Click, Sub(sender As Object, e As EventArgs)
            SelectedTransactionItem.TransactionItemQuantity = Textbox_QuantityInputEditTransactionItem.Text
            SelectedTransactionItem.TransactionItemAmount = SelectedTransactionItem.TransactionItemQuantity * SelectedTransactionItem.TransactionItemSellingPrice
        
            ' Get subtotal of the transaction
            Dim subtotal As Decimal = 0D
            For Each i In CurrentTransactionItems
                subTotal += i.TransactionItemAmount
            Next
            CurrentTransactionEntry.Subtotal = subtotal
            Label_SubTotalSaleScreen.Text = CurrentTransactionEntry.Subtotal
            UpdateTransactionItemsTable()
            Prompt_EditTransactionItem.Close()
        End Sub

        AddHandler Button_RemoveItemEditReceivedItem.Click, Sub(sender As Object, e As EventArgs)
            CurrentTransactionItems.Remove(SelectedTransactionItem)
            
            ' Get subtotal of the transaction
            Dim subtotal As Decimal = 0D
            For Each i In CurrentTransactionItems
                subTotal += i.TransactionItemAmount
            Next
            CurrentTransactionEntry.Subtotal = subtotal
            Label_SubTotalSaleScreen.Text = CurrentTransactionEntry.Subtotal
            UpdateTransactionItemsTable()
            Prompt_EditTransactionItem.Close()
        End Sub

        Prompt_EditTransactionItem.ShowDialog(Me)
    End Sub



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

        PopulateCustomerComboBox(Combobox_PlaceOrderCustomer)
        PopulatePaymentTermComboBox(Combobox_PlaceOrderPaymentTerm)
        PopulateTaxComboBox(Combobox_PlaceOrderTax)

        Combobox_PlaceOrderPaymentTerm.SelectedItem  = 3
        CurrentTransactionEntry.ReceivedAmount = CurrentTransactionEntry.Subtotal
        Textbox_PlaceOrderReceivedAmount.Text = CurrentTransactionEntry.ReceivedAmount
        Dim selectedTax1 As Tax = Taxes.FirstOrDefault(Function(t) t.TaxId = CurrentTransactionEntry.TaxId)

        Dim taxPercentage As Decimal = If(selectedTax1 IsNot Nothing, selectedTax1.TaxPercentage, 0)

        CurrentTransactionEntry.TaxAmount = CurrentTransactionEntry.Subtotal * (taxPercentage / 100)
        CurrentTransactionEntry.NetTotal = CurrentTransactionEntry.Subtotal + CurrentTransactionEntry.TaxAmount
        CurrentTransactionEntry.DueAmount = CurrentTransactionEntry.NetTotal - CurrentTransactionEntry.ReceivedAmount
        CurrentTransactionEntry.ChangeAmount = CurrentTransactionEntry.ReceivedAmount - CurrentTransactionEntry.NetTotal

        Label_PlaceOrderSubtotalValue.Text = CurrentTransactionEntry.Subtotal.ToString()
        Label_PlaceOrderTaxPercentageValue.Text = taxPercentage.ToString()
        Label_PlaceOrdertaxAmountValue.Text = CurrentTransactionEntry.TaxAmount.ToString()
        Label_PlaceOrderNetTotalValue.Text = CurrentTransactionEntry.NetTotal.ToString()
        Label_PlaceOrderReceivedValue.Text = CurrentTransactionEntry.ReceivedAmount.ToString()
        Label_PlaceOrderChangeValue.Text = CurrentTransactionEntry.ChangeAmount.ToString()
        Label_PlaceOrderDueValue.Text = CurrentTransactionEntry.DueAmount.ToString()

        AddHandler Button_PlaceOrderNewPaymentTerm.Click, 
        Sub(sender As Object, e As EventArgs)
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
            PopulatePaymentTermComboBox(Combobox_PlaceOrderPaymentTerm)

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
                            PopulatePaymentTermComboBox(Combobox_PlaceOrderPaymentTerm)
                            Prompt_NewPaymentTerm.Close()
                        Catch ex As Exception
                            MessageBox.Show($"Error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Try
                    End Using
                End Using
            End Sub

            Prompt_NewPaymentTerm.ShowDialog(Me)
        End Sub

        AddHandler Button_PlaceOrderNewTax.Click, 
        Sub()
            Dim Prompt_NewTax As New Form()
            Dim FlowLayoutPanel_TaxDetailsNewTax = New System.Windows.Forms.FlowLayoutPanel()
            Dim FlowLayoutPanel_PageHeaderNewTax = New System.Windows.Forms.FlowLayoutPanel()
            Dim Panel_PageHeaderIconNewTax = New System.Windows.Forms.Panel()
            Dim Label_PageHeaderNewTax = New System.Windows.Forms.Label()
            Dim Label_TaxDetailsNewTax = New System.Windows.Forms.Label()
            Dim FlowLayoutPanel_TaxDetailTaxNameInputNewTax = New System.Windows.Forms.FlowLayoutPanel()
            Dim Label_TaxDetailTaxNameInputNewTax = New System.Windows.Forms.Label()
            Dim Textbox_TaxDetailTaxNameInputNewTax = New Guna.UI2.WinForms.Guna2TextBox()
            Dim FlowLayoutPanel_DateRangeInputsSales = New System.Windows.Forms.FlowLayoutPanel()
            Dim FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
            Dim Label1 = New System.Windows.Forms.Label()
            Dim Textbox_TaxDetailsTaxDaysInputNewTax = New Guna.UI2.WinForms.Guna2TextBox()
            Dim Button_MakeTaxNewSale = New Guna.UI2.WinForms.Guna2Button()
            FlowLayoutPanel_TaxDetailsNewTax.SuspendLayout
            FlowLayoutPanel_PageHeaderNewTax.SuspendLayout
            FlowLayoutPanel_TaxDetailTaxNameInputNewTax.SuspendLayout
            FlowLayoutPanel1.SuspendLayout
            SuspendLayout
            '
            'FlowLayoutPanel_TaxDetailsNewTax
            '
            FlowLayoutPanel_TaxDetailsNewTax.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
            FlowLayoutPanel_TaxDetailsNewTax.Controls.Add(FlowLayoutPanel_PageHeaderNewTax)
            FlowLayoutPanel_TaxDetailsNewTax.Controls.Add(Label_TaxDetailsNewTax)
            FlowLayoutPanel_TaxDetailsNewTax.Controls.Add(FlowLayoutPanel_TaxDetailTaxNameInputNewTax)
            FlowLayoutPanel_TaxDetailsNewTax.Controls.Add(FlowLayoutPanel_DateRangeInputsSales)
            FlowLayoutPanel_TaxDetailsNewTax.Controls.Add(FlowLayoutPanel1)
            FlowLayoutPanel_TaxDetailsNewTax.Controls.Add(Button_MakeTaxNewSale)
            FlowLayoutPanel_TaxDetailsNewTax.Location = New System.Drawing.Point(0, 0)
            FlowLayoutPanel_TaxDetailsNewTax.Margin = New System.Windows.Forms.Padding(0)
            FlowLayoutPanel_TaxDetailsNewTax.Name = "FlowLayoutPanel_TaxDetailsNewTax"
            FlowLayoutPanel_TaxDetailsNewTax.Size = New System.Drawing.Size(366, 350)
            FlowLayoutPanel_TaxDetailsNewTax.TabIndex = 36
            '
            'FlowLayoutPanel_PageHeaderNewTax
            '
            FlowLayoutPanel_PageHeaderNewTax.AutoSize = true
            FlowLayoutPanel_PageHeaderNewTax.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            FlowLayoutPanel_PageHeaderNewTax.Controls.Add(Panel_PageHeaderIconNewTax)
            FlowLayoutPanel_PageHeaderNewTax.Controls.Add(Label_PageHeaderNewTax)
            FlowLayoutPanel_PageHeaderNewTax.Location = New System.Drawing.Point(12, 24)
            FlowLayoutPanel_PageHeaderNewTax.Margin = New System.Windows.Forms.Padding(12, 24, 0, 24)
            FlowLayoutPanel_PageHeaderNewTax.Name = "FlowLayoutPanel_PageHeaderNewTax"
            FlowLayoutPanel_PageHeaderNewTax.Size = New System.Drawing.Size(161, 32)
            FlowLayoutPanel_PageHeaderNewTax.TabIndex = 0
            '
            'Panel_PageHeaderIconNewTax
            '
            Panel_PageHeaderIconNewTax.Anchor = System.Windows.Forms.AnchorStyles.Left
            Panel_PageHeaderIconNewTax.BackgroundImage = Global.ERP_SIMS.My.Resources.Resources.HeaderIcon_Brands
            Panel_PageHeaderIconNewTax.Location = New System.Drawing.Point(0, 0)
            Panel_PageHeaderIconNewTax.Margin = New System.Windows.Forms.Padding(0, 0, 16, 0)
            Panel_PageHeaderIconNewTax.Name = "Panel_PageHeaderIconNewTax"
            Panel_PageHeaderIconNewTax.Size = New System.Drawing.Size(32, 32)
            Panel_PageHeaderIconNewTax.TabIndex = 3
            '
            'Label_PageHeaderNewTax
            '
            Label_PageHeaderNewTax.Anchor = System.Windows.Forms.AnchorStyles.Left
            Label_PageHeaderNewTax.AutoSize = true
            Label_PageHeaderNewTax.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
            Label_PageHeaderNewTax.Font = New System.Drawing.Font("Microsoft JhengHei", 24!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
            Label_PageHeaderNewTax.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
            Label_PageHeaderNewTax.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Label_PageHeaderNewTax.Location = New System.Drawing.Point(48, 0)
            Label_PageHeaderNewTax.Margin = New System.Windows.Forms.Padding(0)
            Label_PageHeaderNewTax.Name = "Label_PageHeaderNewTax"
            Label_PageHeaderNewTax.Size = New System.Drawing.Size(113, 31)
            Label_PageHeaderNewTax.TabIndex = 2
            Label_PageHeaderNewTax.Text = "New Tax"
            Label_PageHeaderNewTax.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'Label_TaxDetailsNewTax
            '
            Label_TaxDetailsNewTax.Anchor = System.Windows.Forms.AnchorStyles.Left
            Label_TaxDetailsNewTax.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
            Label_TaxDetailsNewTax.Font = New System.Drawing.Font("Microsoft JhengHei", 22!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
            Label_TaxDetailsNewTax.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
            Label_TaxDetailsNewTax.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Label_TaxDetailsNewTax.Location = New System.Drawing.Point(12, 80)
            Label_TaxDetailsNewTax.Margin = New System.Windows.Forms.Padding(12, 0, 12, 24)
            Label_TaxDetailsNewTax.Name = "Label_TaxDetailsNewTax"
            Label_TaxDetailsNewTax.Size = New System.Drawing.Size(342, 28)
            Label_TaxDetailsNewTax.TabIndex = 18
            Label_TaxDetailsNewTax.Text = "Tax  Details"
            Label_TaxDetailsNewTax.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'FlowLayoutPanel_TaxDetailTaxNameInputNewTax
            '
            FlowLayoutPanel_TaxDetailTaxNameInputNewTax.AutoSize = true
            FlowLayoutPanel_TaxDetailTaxNameInputNewTax.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            FlowLayoutPanel_TaxDetailTaxNameInputNewTax.Controls.Add(Label_TaxDetailTaxNameInputNewTax)
            FlowLayoutPanel_TaxDetailTaxNameInputNewTax.Controls.Add(Textbox_TaxDetailTaxNameInputNewTax)
            FlowLayoutPanel_TaxDetailTaxNameInputNewTax.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
            FlowLayoutPanel_TaxDetailTaxNameInputNewTax.Location = New System.Drawing.Point(12, 132)
            FlowLayoutPanel_TaxDetailTaxNameInputNewTax.Margin = New System.Windows.Forms.Padding(12, 0, 6, 12)
            FlowLayoutPanel_TaxDetailTaxNameInputNewTax.Name = "FlowLayoutPanel_TaxDetailTaxNameInputNewTax"
            FlowLayoutPanel_TaxDetailTaxNameInputNewTax.Size = New System.Drawing.Size(342, 56)
            FlowLayoutPanel_TaxDetailTaxNameInputNewTax.TabIndex = 19
            '
            'Label_TaxDetailTaxNameInputNewTax
            '
            Label_TaxDetailTaxNameInputNewTax.AutoSize = true
            Label_TaxDetailTaxNameInputNewTax.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
            Label_TaxDetailTaxNameInputNewTax.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
            Label_TaxDetailTaxNameInputNewTax.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
            Label_TaxDetailTaxNameInputNewTax.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Label_TaxDetailTaxNameInputNewTax.Location = New System.Drawing.Point(0, 0)
            Label_TaxDetailTaxNameInputNewTax.Margin = New System.Windows.Forms.Padding(0)
            Label_TaxDetailTaxNameInputNewTax.Name = "Label_TaxDetailTaxNameInputNewTax"
            Label_TaxDetailTaxNameInputNewTax.Size = New System.Drawing.Size(74, 16)
            Label_TaxDetailTaxNameInputNewTax.TabIndex = 2
            Label_TaxDetailTaxNameInputNewTax.Text = "Tax Name"
            Label_TaxDetailTaxNameInputNewTax.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'Textbox_TaxDetailTaxNameInputNewTax
            '
            Textbox_TaxDetailTaxNameInputNewTax.Anchor = System.Windows.Forms.AnchorStyles.Left
            Textbox_TaxDetailTaxNameInputNewTax.AutoCompleteCustomSource.AddRange(New String() {"(001) Item 1", "(002) Item 2", "(003) Item 3", "(004) Item 4", "(005) Item 5", "(006) Item 6", "(007) Item 7", "(008) Item 8", "(009) Item 9", "(010) Item 10"})
            Textbox_TaxDetailTaxNameInputNewTax.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
            Textbox_TaxDetailTaxNameInputNewTax.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
            Textbox_TaxDetailTaxNameInputNewTax.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
            Textbox_TaxDetailTaxNameInputNewTax.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
            Textbox_TaxDetailTaxNameInputNewTax.Cursor = System.Windows.Forms.Cursors.IBeam
            Textbox_TaxDetailTaxNameInputNewTax.DefaultText = ""
            Textbox_TaxDetailTaxNameInputNewTax.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer))
            Textbox_TaxDetailTaxNameInputNewTax.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer))
            Textbox_TaxDetailTaxNameInputNewTax.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
            Textbox_TaxDetailTaxNameInputNewTax.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
            Textbox_TaxDetailTaxNameInputNewTax.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
            Textbox_TaxDetailTaxNameInputNewTax.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
            Textbox_TaxDetailTaxNameInputNewTax.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
            Textbox_TaxDetailTaxNameInputNewTax.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
            Textbox_TaxDetailTaxNameInputNewTax.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
            Textbox_TaxDetailTaxNameInputNewTax.Location = New System.Drawing.Point(0, 16)
            Textbox_TaxDetailTaxNameInputNewTax.Margin = New System.Windows.Forms.Padding(0)
            Textbox_TaxDetailTaxNameInputNewTax.Name = "Textbox_TaxDetailTaxNameInputNewTax"
            Textbox_TaxDetailTaxNameInputNewTax.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
            Textbox_TaxDetailTaxNameInputNewTax.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer))
            Textbox_TaxDetailTaxNameInputNewTax.PlaceholderText = "Required"
            Textbox_TaxDetailTaxNameInputNewTax.SelectedText = ""
            Textbox_TaxDetailTaxNameInputNewTax.Size = New System.Drawing.Size(342, 40)
            Textbox_TaxDetailTaxNameInputNewTax.TabIndex = 39
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
            'FlowLayoutPanel1
            '
            FlowLayoutPanel1.AutoSize = true
            FlowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            FlowLayoutPanel1.Controls.Add(Label1)
            FlowLayoutPanel1.Controls.Add(Textbox_TaxDetailsTaxDaysInputNewTax)
            FlowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
            FlowLayoutPanel1.Location = New System.Drawing.Point(12, 200)
            FlowLayoutPanel1.Margin = New System.Windows.Forms.Padding(12, 0, 6, 0)
            FlowLayoutPanel1.Name = "FlowLayoutPanel1"
            FlowLayoutPanel1.Size = New System.Drawing.Size(342, 56)
            FlowLayoutPanel1.TabIndex = 36
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
            Label1.Text = "Tax Days"
            Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'Textbox_TaxDetailsTaxDaysInputNewTax
            '
            Textbox_TaxDetailsTaxDaysInputNewTax.Anchor = System.Windows.Forms.AnchorStyles.Left
            Textbox_TaxDetailsTaxDaysInputNewTax.AutoCompleteCustomSource.AddRange(New String() {"(001) Item 1", "(002) Item 2", "(003) Item 3", "(004) Item 4", "(005) Item 5", "(006) Item 6", "(007) Item 7", "(008) Item 8", "(009) Item 9", "(010) Item 10"})
            Textbox_TaxDetailsTaxDaysInputNewTax.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
            Textbox_TaxDetailsTaxDaysInputNewTax.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
            Textbox_TaxDetailsTaxDaysInputNewTax.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
            Textbox_TaxDetailsTaxDaysInputNewTax.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
            Textbox_TaxDetailsTaxDaysInputNewTax.Cursor = System.Windows.Forms.Cursors.IBeam
            Textbox_TaxDetailsTaxDaysInputNewTax.DefaultText = ""
            Textbox_TaxDetailsTaxDaysInputNewTax.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer))
            Textbox_TaxDetailsTaxDaysInputNewTax.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer))
            Textbox_TaxDetailsTaxDaysInputNewTax.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
            Textbox_TaxDetailsTaxDaysInputNewTax.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
            Textbox_TaxDetailsTaxDaysInputNewTax.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
            Textbox_TaxDetailsTaxDaysInputNewTax.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
            Textbox_TaxDetailsTaxDaysInputNewTax.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
            Textbox_TaxDetailsTaxDaysInputNewTax.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
            Textbox_TaxDetailsTaxDaysInputNewTax.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
            Textbox_TaxDetailsTaxDaysInputNewTax.Location = New System.Drawing.Point(0, 16)
            Textbox_TaxDetailsTaxDaysInputNewTax.Margin = New System.Windows.Forms.Padding(0)
            Textbox_TaxDetailsTaxDaysInputNewTax.Name = "Textbox_TaxDetailsTaxDaysInputNewTax"
            Textbox_TaxDetailsTaxDaysInputNewTax.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
            Textbox_TaxDetailsTaxDaysInputNewTax.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer))
            Textbox_TaxDetailsTaxDaysInputNewTax.PlaceholderText = "Required"
            Textbox_TaxDetailsTaxDaysInputNewTax.SelectedText = ""
            Textbox_TaxDetailsTaxDaysInputNewTax.Size = New System.Drawing.Size(342, 40)
            Textbox_TaxDetailsTaxDaysInputNewTax.TabIndex = 39
            '
            'Button_MakeTaxNewSale
            '
            Button_MakeTaxNewSale.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(44,Byte),Integer), CType(CType(62,Byte),Integer))
            Button_MakeTaxNewSale.DisabledState.BorderColor = System.Drawing.Color.DarkGray
            Button_MakeTaxNewSale.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
            Button_MakeTaxNewSale.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer))
            Button_MakeTaxNewSale.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer))
            Button_MakeTaxNewSale.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
            Button_MakeTaxNewSale.Font = New System.Drawing.Font("Segoe UI", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel)
            Button_MakeTaxNewSale.ForeColor = System.Drawing.Color.White
            Button_MakeTaxNewSale.Location = New System.Drawing.Point(12, 280)
            Button_MakeTaxNewSale.Margin = New System.Windows.Forms.Padding(12, 24, 12, 12)
            Button_MakeTaxNewSale.Name = "Button_MakeTaxNewSale"
            Button_MakeTaxNewSale.Size = New System.Drawing.Size(342, 40)
            Button_MakeTaxNewSale.TabIndex = 33
            Button_MakeTaxNewSale.Text = "Create Tax"
            Button_MakeTaxNewSale.TextOffset = New System.Drawing.Point(0, -2)
            '
            '_Temporary_NewTax
            '
            Prompt_NewTax.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
            Prompt_NewTax.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Prompt_NewTax.ClientSize = New System.Drawing.Size(366, 350)
            Prompt_NewTax.Controls.Add(FlowLayoutPanel_TaxDetailsNewTax)
            Prompt_NewTax.Name = "_Temporary_NewTax"
            Prompt_NewTax.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Prompt_NewTax.Text = "New Tax"
            FlowLayoutPanel_TaxDetailsNewTax.ResumeLayout(false)
            FlowLayoutPanel_TaxDetailsNewTax.PerformLayout
            FlowLayoutPanel_PageHeaderNewTax.ResumeLayout(false)
            FlowLayoutPanel_PageHeaderNewTax.PerformLayout
            FlowLayoutPanel_TaxDetailTaxNameInputNewTax.ResumeLayout(false)
            FlowLayoutPanel_TaxDetailTaxNameInputNewTax.PerformLayout
            FlowLayoutPanel1.ResumeLayout(false)
            FlowLayoutPanel1.PerformLayout
            ResumeLayout(false)

            AddHandler Button_MakeTaxNewSale.Click,
            Sub(sender As Object, e As EventArgs)
                Dim taxName As String = Textbox_TaxDetailTaxNameInputNewTax.Text.Trim()
                Dim taxPercentageText As String = Textbox_TaxDetailsTaxDaysInputNewTax.Text.Trim()
    
                Dim taxPercentagee As Decimal
                If String.IsNullOrEmpty(taxName) OrElse String.IsNullOrEmpty(taxPercentageText) OrElse 
                   Not Decimal.TryParse(taxPercentageText, taxPercentage) OrElse taxPercentagee < 0 Then
                    MessageBox.Show("All fields are required, and tax percentage must be a valid non-negative number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If

                Try
                    Using connection As New MySqlConnection(ConnectionString)
                        connection.Open()
                        Dim query As String = "INSERT INTO taxes (tax_name, tax_percentage, created_by) VALUES (@tax_name, @tax_percentage, @created_by)"
                        Using command As New MySqlCommand(query, connection)
                            command.Parameters.AddWithValue("@tax_name", taxName)
                            command.Parameters.AddWithValue("@tax_percentage", taxPercentage)
                            command.Parameters.AddWithValue("@created_by", GlobalShared.CurrentUser.AccountId)
                            command.ExecuteNonQuery()
                        End Using
                    End Using
                    MessageBox.Show("Tax added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Catch ex As Exception
                    MessageBox.Show($"An error occurred: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try

                Prompt_NewTax.Close()
            End Sub

            Prompt_NewTax.ShowDialog(Me)

            PopulateTaxComboBox(Combobox_PlaceOrderTax)
        End Sub

        AddHandler Combobox_PlaceOrderCustomer.SelectedIndexChanged, 
        Sub(sender As Object, e As EventArgs)
            Dim selectedCustomerName As String = Combobox_PlaceOrderCustomer.SelectedItem.ToString()
            Dim customerDetails = Customers.FirstOrDefault(Function(c) c.Name = selectedCustomerName)
            Dim selectedPaymentTerm = PaymentTerms.FirstOrDefault(Function(pt) pt.PaymentTermId = customerDetails.PaymentTermId)

            selectedCustomer = customerDetails

            If customerDetails IsNot Nothing Then
        
                If selectedPaymentTerm IsNot Nothing Then
                    CurrentTransactionEntry.CustomerId = customerDetails.CustomerId
                    CurrentTransactionEntry.PaymentTermId = selectedPaymentTerm.PaymentTermId
                    CurrentTransactionEntry.DueDate = DateTime.Now.AddDays(selectedPaymentTerm.TermDays)
                    Combobox_PlaceOrderPaymentTerm.Text = selectedPaymentTerm.TermName

                    Label_PlaceOrderPaymentTermDetailsText.Text = 
                    $"Payment Term ID: {selectedPaymentTerm.PaymentTermId}" & Environment.NewLine & 
                    $"Term Days: {selectedPaymentTerm.TermDays}" & Environment.NewLine & 
                    $"Due Date: {DateTime.Now.AddDays(selectedPaymentTerm.TermDays):yyyy-MM-dd}"
                Else
                    Label_PlaceOrderPaymentTermDetailsText.Text = 
                    "Payment Term ID: No Value" & Environment.NewLine & 
                    "Term Days: No Value" & Environment.NewLine & 
                    "Due Date: No Value"
                End If
            Else
                Label_PlaceOrderPaymentTermDetailsText.Text = 
                "Customer Details: No Value"
            End If

            If customerDetails IsNot Nothing Then
                Labell_PlaceOrderCustomerDetailsText.Text =
                    $"Customer ID: {customerDetails.CustomerId}" & Environment.NewLine &
                    $"Company: {If(String.IsNullOrEmpty(customerDetails.Company), "No value", customerDetails.Company)}" & Environment.NewLine &
                    $"Email: {If(String.IsNullOrEmpty(customerDetails.Email), "No value", customerDetails.Email)}" & Environment.NewLine &
                    $"Phone: {If(String.IsNullOrEmpty(customerDetails.Phone), "No value", customerDetails.Phone)}" & Environment.NewLine &
                    $"Address: {If(String.IsNullOrEmpty(customerDetails.AddressLine), "No value", customerDetails.AddressLine)}" & Environment.NewLine &
                    $"Payment Term: {If(customerDetails.PaymentTermId = 0, "No value", selectedPaymentTerm.TermName)}"
            Else
                Labell_PlaceOrderCustomerDetailsText.Text =
                    "Customer ID: No value" & Environment.NewLine &
                    "Company: No value" & Environment.NewLine &
                    "Email: No value" & Environment.NewLine &
                    "Phone: No value" & Environment.NewLine &
                    "Address: No value" & Environment.NewLine &
                    "Payment Term: No value"
            End If
        End Sub

        AddHandler Combobox_PlaceOrderPaymentTerm.SelectedIndexChanged,
        Sub(sender As Object, e As EventArgs)
            Dim selectedTermName As String = Combobox_PlaceOrderPaymentTerm.SelectedItem?.ToString()
            Dim selectedPaymentTerm = PaymentTerms.FirstOrDefault(Function(pt) pt.TermName = selectedTermName)

            If selectedPaymentTerm IsNot Nothing Then
                CurrentTransactionEntry.PaymentTermId = selectedPaymentTerm.PaymentTermId
                CurrentTransactionEntry.DueDate = DateTime.Now.AddDays(selectedPaymentTerm.TermDays)

                Label_PlaceOrderPaymentTermDetailsText.Text = 
                $"Payment Term ID: {selectedPaymentTerm.PaymentTermId}" & Environment.NewLine &
                $"Term Days: {selectedPaymentTerm.TermDays}" & Environment.NewLine &
                $"Due Date: {DateTime.Now.AddDays(selectedPaymentTerm.TermDays):yyyy-MM-dd}"
            Else
                Label_PlaceOrderPaymentTermDetailsText.Text = 
                "Payment Term ID: No Value" & Environment.NewLine & 
                "Term Days: No Value" & Environment.NewLine & 
                "Due Date: No Value"
            End If
        End Sub

        AddHandler Combobox_PlaceOrderTax.SelectedIndexChanged,
        Sub(sender As Object, e As EventArgs)
            Dim selectedTaxName As String = Combobox_PlaceOrderTax.SelectedItem?.ToString()
            Dim selectedTax As Tax = Taxes.FirstOrDefault(Function(t) t.TaxName = selectedTaxName)

            If selectedTax IsNot Nothing Then
                CurrentTransactionEntry.TaxId = selectedTax.TaxId
                CurrentTransactionEntry.TaxAmount = CurrentTransactionEntry.Subtotal * (selectedTax.TaxPercentage / 100)
                CurrentTransactionEntry.NetTotal = CurrentTransactionEntry.Subtotal + CurrentTransactionEntry.TaxAmount
                CurrentTransactionEntry.DueAmount = CurrentTransactionEntry.NetTotal - CurrentTransactionEntry.ReceivedAmount
                CurrentTransactionEntry.ChangeAmount = CurrentTransactionEntry.ReceivedAmount - CurrentTransactionEntry.NetTotal

                Label_PlaceOrderTaxDetailstext.Text = 
                $"Tax ID: {selectedTax.TaxId}{Environment.NewLine}Tax Percentage: {selectedTax.TaxPercentage}%"
            Else
                Label_PlaceOrderTaxDetailstext.Text = 
                "Tax ID: No Value" & Environment.NewLine & 
                "Tax Percentage: No Value"
            End If

            Label_PlaceOrderSubtotalValue.Text = CurrentTransactionEntry.Subtotal.ToString()
            Label_PlaceOrderTaxPercentageValue.Text = If(selectedTax IsNot Nothing, selectedTax.TaxPercentage.ToString(), "No Value")
            Label_PlaceOrdertaxAmountValue.Text = CurrentTransactionEntry.TaxAmount.ToString()
            Label_PlaceOrderNetTotalValue.Text = CurrentTransactionEntry.NetTotal.ToString()
            Label_PlaceOrderReceivedValue.Text = CurrentTransactionEntry.ReceivedAmount.ToString()
            Label_PlaceOrderChangeValue.Text = CurrentTransactionEntry.ChangeAmount.ToString()
            Label_PlaceOrderDueValue.Text = CurrentTransactionEntry.DueAmount.ToString()
        End Sub

        AddHandler Textbox_PlaceOrderReceivedAmount.KeyUp, 
        Sub(sender As Object, e As KeyEventArgs)
            Dim receivedAmount As Decimal
            If Decimal.TryParse(Textbox_PlaceOrderReceivedAmount.Text, receivedAmount) Then
                CurrentTransactionEntry.ReceivedAmount = receivedAmount
            Else
                CurrentTransactionEntry.ReceivedAmount = 0
            End If

            Label_PlaceOrderReceivedValue.Text = CurrentTransactionEntry.ReceivedAmount.ToString()

            Dim selectedTax2 As Tax = Taxes.FirstOrDefault(Function(t) t.TaxId = CurrentTransactionEntry.TaxId)
            Dim taxPercentage2 As Decimal = If(selectedTax2 IsNot Nothing, selectedTax2.TaxPercentage, 0)

            CurrentTransactionEntry.TaxAmount = CurrentTransactionEntry.Subtotal * (taxPercentage2 / 100)
            CurrentTransactionEntry.NetTotal = CurrentTransactionEntry.Subtotal + CurrentTransactionEntry.TaxAmount
            CurrentTransactionEntry.DueAmount = CurrentTransactionEntry.NetTotal - CurrentTransactionEntry.ReceivedAmount
            CurrentTransactionEntry.ChangeAmount = CurrentTransactionEntry.ReceivedAmount - CurrentTransactionEntry.NetTotal

            Label_PlaceOrderSubtotalValue.Text = CurrentTransactionEntry.Subtotal.ToString()
            Label_PlaceOrderTaxPercentageValue.Text = taxPercentage2.ToString()
            Label_PlaceOrdertaxAmountValue.Text = CurrentTransactionEntry.TaxAmount.ToString()
            Label_PlaceOrderNetTotalValue.Text = CurrentTransactionEntry.NetTotal.ToString()
            Label_PlaceOrderReceivedValue.Text = CurrentTransactionEntry.ReceivedAmount.ToString()
            Label_PlaceOrderChangeValue.Text = CurrentTransactionEntry.ChangeAmount.ToString()
            Label_PlaceOrderDueValue.Text = CurrentTransactionEntry.DueAmount.ToString()
        End Sub

        AddHandler Button_PlaceOrderCompleteTransaction.Click,
        Sub(sender As Object, e As EventArgs)
            Dim nullFields = CurrentTransactionEntry.CheckNullProperties()

            If nullFields.Count > 0 Then
                MessageBox.Show("All fields must be not empty!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

                For Each field In nullFields
                    Console.WriteLine(field)
                Next
            Else
                CurrentTransactionEntry.Status = "Pending Approval"
                InsertTransaction(CurrentTransactionEntry, CurrentTransactionItems)
            End If
            
        End Sub

        
        Prompt_TransactionPlaceOrder.ShowDialog(Me)
    End Sub

    Public Sub InsertTransaction(transactionEntry As TransactionEntry, transactionItems As List(Of TransactionItem))
        Dim validStatuses As String() = {"Completed", "Pending Approval", "Void", "Open", "Overdue"}

        If Not validStatuses.Contains(transactionEntry.Status) Then
            Throw New Exception($"Invalid status value: {transactionEntry.Status}. Allowed values are: {String.Join(", ", validStatuses)}")
        End If

        Dim simsConnectionString As String = "server=localhost;user id=root;password=benjaminn1202;database=sims_db"
        Dim hrmConnectionString As String = "server=localhost;user id=root;password=benjaminn1202;database=hrm_db"

        Using simsConnection As New MySqlConnection(simsConnectionString)
            simsConnection.Open()

            Using simsTransaction As MySqlTransaction = simsConnection.BeginTransaction()
                Try
                    ' Insert into transaction_entries
                    Dim insertTransactionQuery As String = "
                        INSERT INTO transaction_entries (
                            customer_id, subtotal, net_total, received_amount, change_amount, tax_id, 
                            tax_amount, payment_term_id, due_amount, due_date, status, created_at, created_by
                        ) VALUES (
                            @CustomerId, @Subtotal, @NetTotal, @ReceivedAmount, @ChangeAmount, @TaxId, 
                            @TaxAmount, @PaymentTermId, @DueAmount, @DueDate, @Status, @CreatedAt, @CreatedBy
                        );
                        SELECT LAST_INSERT_ID();"

                    Dim command As New MySqlCommand(insertTransactionQuery, simsConnection, simsTransaction)
                    With command.Parameters
                        .AddWithValue("@CustomerId", transactionEntry.CustomerId)
                        .AddWithValue("@Subtotal", transactionEntry.Subtotal)
                        .AddWithValue("@NetTotal", transactionEntry.NetTotal)
                        .AddWithValue("@ReceivedAmount", transactionEntry.ReceivedAmount)
                        .AddWithValue("@ChangeAmount", transactionEntry.ChangeAmount)
                        .AddWithValue("@TaxId", If(transactionEntry.TaxId = 0, DBNull.Value, transactionEntry.TaxId))
                        .AddWithValue("@TaxAmount", If(transactionEntry.TaxAmount = 0, DBNull.Value, transactionEntry.TaxAmount))
                        .AddWithValue("@PaymentTermId", If(transactionEntry.PaymentTermId = 0, DBNull.Value, transactionEntry.PaymentTermId))
                        .AddWithValue("@DueAmount", If(transactionEntry.DueAmount = 0, DBNull.Value, transactionEntry.DueAmount))
                        .AddWithValue("@DueDate", transactionEntry.DueDate)
                        .AddWithValue("@Status", transactionEntry.Status)
                        .AddWithValue("@CreatedAt", DateTime.Now)
                        .AddWithValue("@CreatedBy", GlobalShared.CurrentUser.AccountId)
                    End With

                    Dim newTransactionId As Integer = Convert.ToInt32(command.ExecuteScalar())

                    ' Insert into transaction_items
                    Dim insertItemQuery As String = "
                        INSERT INTO transaction_items (
                            transaction_id, item_id, transaction_item_quantity, transaction_item_selling_price, 
                            transaction_item_amount, stock_id
                        ) VALUES (
                            @TransactionId, @ItemId, @TransactionItemQuantity, @TransactionItemSellingPrice, 
                            @TransactionItemAmount, @StockId
                        );"

                    For Each item As TransactionItem In transactionItems
                        ' Retrieve stock details
                        Dim selectStockQuery As String = "
                            SELECT stock_id, stock_quantity
                            FROM stocks
                            WHERE item_id = @ItemId AND status = 'Available'
                            ORDER BY
                                CASE WHEN expiry IS NOT NULL THEN expiry ELSE '9999-12-31' END ASC,
                                stock_id ASC
                            LIMIT 1;"

                        Dim stockCommand As New MySqlCommand(selectStockQuery, simsConnection, simsTransaction)
                        stockCommand.Parameters.AddWithValue("@ItemId", item.ItemId)

                        Dim stockReader As MySqlDataReader = stockCommand.ExecuteReader()

                        If Not stockReader.Read() Then
                            stockReader.Close()
                            Throw New Exception($"No available stock found for ItemId {item.ItemId}.")
                        End If

                        Dim stockId As Integer = stockReader("stock_id")
                        Dim availableQuantity As Integer = stockReader("stock_quantity")
                        stockReader.Close()

                        If availableQuantity < item.TransactionItemQuantity Then
                            Throw New Exception($"Insufficient stock for ItemId {item.ItemId}.")
                        End If

                        ' Update stock quantity
                        Dim updateStockQuery As String = "
                            UPDATE stocks
                            SET stock_quantity = stock_quantity - @Quantity
                            WHERE stock_id = @StockId;"

                        Dim updateStockCommand As New MySqlCommand(updateStockQuery, simsConnection, simsTransaction)
                        updateStockCommand.Parameters.AddWithValue("@Quantity", item.TransactionItemQuantity)
                        updateStockCommand.Parameters.AddWithValue("@StockId", stockId)
                        updateStockCommand.ExecuteNonQuery()

                        ' Insert into transaction_items
                        Dim itemCommand As New MySqlCommand(insertItemQuery, simsConnection, simsTransaction)
                        With itemCommand.Parameters
                            .AddWithValue("@TransactionId", newTransactionId)
                            .AddWithValue("@ItemId", item.ItemId)
                            .AddWithValue("@TransactionItemQuantity", item.TransactionItemQuantity)
                            .AddWithValue("@TransactionItemSellingPrice", item.TransactionItemSellingPrice)
                            .AddWithValue("@TransactionItemAmount", item.TransactionItemAmount)
                            .AddWithValue("@StockId", stockId)
                        End With

                        itemCommand.ExecuteNonQuery()
                    Next

                    ' Calculate and update commission in hrm_db
                    Dim commissionRate As Decimal = 0.005D ' 0.5%
                    Dim commissionAmount As Decimal = transactionEntry.NetTotal * commissionRate

                    Using hrmConnection As New MySqlConnection(hrmConnectionString)
                        hrmConnection.Open()

                        Dim updateCommissionQuery As String = "
                            UPDATE employee_tbl
                            SET commissions = commissions + @CommissionAmount
                            WHERE employee_id = @EmployeeId;"

                        Dim commissionCommand As New MySqlCommand(updateCommissionQuery, hrmConnection)
                        With commissionCommand.Parameters
                            .AddWithValue("@CommissionAmount", commissionAmount)
                            .AddWithValue("@EmployeeId", GlobalShared.CurrentUser.AccountId)
                        End With

                        commissionCommand.ExecuteNonQuery()
                    End Using

                    simsTransaction.Commit()

                    MessageBox.Show("Transaction successfully added!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    ShowOutvoiceReceiptPrompt()

                    Dim newForm As New Window_NewTransaction()
                    newForm.Show()
                    Me.Hide()
                    Prompt_TransactionPlaceOrder.Close()
                    Me.Close()
                Catch ex As Exception
                    simsTransaction.Rollback()
                    Throw New Exception("Transaction failed: " & ex.Message)
                End Try
            End Using
        End Using
    End Sub


    Dim SelectedCustomer As Customer
    
    Private Sub Window_NewTransaction_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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
        
        PopulateCategoriesSelection()
        PopulateItemsGrid(CurrentCategoryId)

        Label_UserName.Text = GlobalShared.CurrentUser.Name
        Label_UserEmail.Text = GlobalShared.CurrentUser.Email
    End Sub

    Private Sub Button_ToPayNewSale_Click(sender As Object, e As EventArgs) Handles Button_ToPayNewSale.Click
        If Not CurrentTransactionItems.Any() Then
            MessageBox.Show("No items in the transaction. Please add items before proceeding.")
            Return
        End If

        CurrentTransactionEntry.TaxId = 3
        ShowTransactionPlaceOrderPrompt()
    End Sub

    Private Sub Button_ResetSaleScreeen_Click(sender As Object, e As EventArgs) Handles Button_ResetSaleScreeen.Click
        DIm newForm As New Window_NewTransaction()
        newForm.Show()
        Me.Close()
    End Sub

    Private Sub Textbox_SearchBarBrands_TextChanged(sender As Object, e As EventArgs) Handles Textbox_SearchBarBrands.TextChanged
        PopulateItemsGrid(CurrentCategoryID, Textbox_SearchBarBrands.Text)
    End Sub

    Private Sub Button_Logout_Click(sender As Object, e As EventArgs) Handles Button_Logout.Click
        Dim result As DialogResult = MessageBox.Show("Are you sure you want to log out?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            Dim newForm As New Window_Login()
            newForm.Show()
            Me.Close()
        End If
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

            Dim qty As String = stock.TransactionItemQuantity.ToString().PadRight(20)
            Dim name As String = itemName.PadRight(30)
            Dim amount As String = stock.TransactionItemAmount.ToString("C").PadLeft(10)

            output.AppendLine($"{qty}{name}{amount}")
        Next

        CurrentTransactionEntry.CustomerId = SelectedCustomer.CustomerId

        Label3.Text = "Invoice ID: " & CurrentTransactionEntry.TransactionId & Environment.NewLine & _
        "Customer: " & CurrentTransactionEntry.CustomerId & " - "  & SelectedCustomer.Name &  Environment.NewLine & _
        "Due Date: " & CurrentTransactionEntry.DueDate &  Environment.NewLine & _
        "Created By: " & CurrentTransactionEntry.CreatedBy & Environment.NewLine & _
        "Created At: " & DateTime.Now & Environment.NewLine
        Label5.Text = output.ToString()
        Label8.Text = "Net Total: " & CurrentTransactionEntry.NetTotal & Environment.NewLine & _
        "Tax Amount: " & CurrentTransactionEntry.TaxAmount & Environment.NewLine & _
        "Grand Total: " & CurrentTransactionEntry.NetTotal & Environment.NewLine & _
        "Due Amount: " & CurrentTransactionEntry.DueAmount & Environment.NewLine & _
        "Change Amount: " & CurrentTransactionEntry.ChangeAmount & Environment.NewLine

        Prompt_OutvoiceReceipt.ShowDialog(Me)
    End Sub


End Class