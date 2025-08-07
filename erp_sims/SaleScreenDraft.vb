Imports Guna.UI2.WinForms
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports iText.Kernel.Pdf
Imports iText.Layout
Imports iText.Layout.Element
Imports iText.IO.Image
Imports MySql.Data.MySqlClient

Public Class SaleScreenDraft
    Public ConnectionString = "server=localhost;user id=root;password=benjaminn1202;database=erp_sims_db"
    
    '' Data
    Public Customers As _Customer() = CreateCustomersArray()
    Public Brands As _Brand() = CreateBrandsArray()
    Public Categories As _Category() = CreateCategoriesArray()
    Public Items As _Item() = CreateItemsArray()

    Public CurrentTransactionItems As New List(Of _TransactionItem)()
    Public CurrentSelectedTransactionItemIDs As New List(Of Integer)()
    Public CurrentTransactionEntry As New _TransactionEntry()

    '' Pagination
    Private ItemsPerPage As Integer = 15
    Private CurrentPage As Integer = 1

    Private CategoriesPerPage As Integer = 4
    Private CurrentCategoryPage As Integer = 1
    Private CurrentCategoryID As Integer = -1

    Private TransactionItemsTableCurrentPage As Integer = 1
    Private TransactionItemsTableItemsPerPage As Integer = 4
    Private TransactionItemsTableTotalPagess As Integer = 1


    '' Methods to populate and control the items grid
    Private Sub PopulateItemsGrid(CategoryID As Integer)
        Dim filteredItems As List(Of _Item)

        If CategoryID = -1 Then
            filteredItems = Items.ToList()
        Else
            filteredItems = Items.Where(Function(item) item.category_id = CategoryID).ToList()
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
            Dim button As New Guna2Button With {
                .Text = $"{item.item_name} ({item.item_selling_price:C})",
                .Size = New Size(118, 180),
                .Margin = New Padding(0),
                .FillColor = Color.FromArgb(0, 72, 101)
            }

            AddHandler button.Click, 
                Sub(sender, e)
                    Dim ItemID = item.item_id
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

    Private Sub SearchAndPopulateItemsGrid(CategoryID As Integer, Optional searchText As String = "")
        Dim filteredItems As List(Of _Item)
        
        If CategoryID = -1 Then
            filteredItems = Items.ToList()
        Else
            filteredItems = Items.Where(Function(item) item.category_id = CategoryID).ToList()
        End If

        ' Apply search filter if searchText is provided
        If Not String.IsNullOrWhiteSpace(searchText) Then
            filteredItems = filteredItems.Where(Function(item) item.item_name.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0).ToList()
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
            Dim button As New Guna2Button With {
                .Text = $"{item.item_name} ({item.item_selling_price:C})",
                .Size = New Size(118, 180),
                .Margin = New Padding(0),
                .FillColor = Color.FromArgb(0, 72, 101)
            }

            AddHandler button.Click,
                Sub(sender, e)
                    Dim ItemID = item.item_id
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
        Dim totalPages As Integer = Math.Ceiling(If(CurrentCategoryID = -1, Items.Count, Items.Where(Function(item) item.category_id = CurrentCategoryID).Count) / ItemsPerPage)
        If CurrentPage < totalPages Then
            CurrentPage += 1
            PopulateItemsGrid(CurrentCategoryID)
        End If
    End Sub


    '' Methods to populate and control the categories selection
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
                .Text = category.category_name,
                .Size = New Size(122, 38),
                .Margin = New Padding(0),
                .FillColor = Color.FromArgb(235, 225, 255),
                .ForeColor = Color.FromArgb(34, 34, 34)
            }

            AddHandler button.Click, 
                Sub(sender, e)
                    CurrentCategoryID = category.category_id
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
        Dim totalPages As Integer = Math.Ceiling(If(CurrentCategoryID = -1, Categories.Count, Categories.Where(Function(item) item.category_id = CurrentCategoryID).Count) / CategoriesPerPage)
        If CurrentCategoryPage < totalPages Then
            CurrentCategoryPage += 1
            PopulateCategoriesSelection()
        End If
    End Sub


    '' Methods to control the transaction items table
    Public Sub UpdateCurrentTransactionData(ItemID As Integer, SelectedItem As _Item)
        Dim SearchedTransactionItem = CurrentTransactionItems.FirstOrDefault(Function(t) t.transaction_item_id = ItemID)

        If SearchedTransactionItem IsNot Nothing Then
            SearchedTransactionItem.transaction_item_quantity += 1
            SearchedTransactionItem.transaction_item_amount = SearchedTransactionItem.transaction_item_quantity * SearchedTransactionItem.transaction_item_selling_price
        Else
            Dim TransactionItem As New _TransactionItem() With {
                .transaction_item_id = SelectedItem.item_id,
                .transaction_item_name = SelectedItem.item_name,
                .transaction_item_quantity = 1,
                .transaction_item_selling_price = SelectedItem.item_selling_price,
                .transaction_item_amount = SelectedItem.item_selling_price
            }

            CurrentTransactionItems.Add(TransactionItem)
        End If

        Dim subtotal As Decimal = 0D

        For Each Item In CurrentTransactionItems
            subtotal += Item.transaction_item_amount
        Next

        CurrentTransactionEntry.subtotal = subtotal
        Label_SubtotalSaleScreen.Text = subtotal

        Dim vat_amount As Decimal = 0D
        Dim vat_percentage As Decimal = Convert.ToDecimal(Textbox_VATPercentageSaleScreen.Text) / 100D


        For Each Item In CurrentTransactionItems
            vat_amount += (Item.transaction_item_selling_price * Item.transaction_item_quantity) * vat_percentage
        Next

        CurrentTransactionEntry.net_total = subtotal + vat_amount
        CurrentTransactionEntry.tax_percentage = Textbox_VATPercentageSaleScreen.Text
        CurrentTransactionEntry.tax_amount = vat_amount

        Label_VATAmountSaleScreen.Text = vat_amount
        Label_NetTotalSaleScreen.Text = CurrentTransactionEntry.net_total
    End Sub

    Public Sub UpdateTransactionItemsTable()
        FlowLayoutPanel_SaleItemsTable.Controls.Clear()

        TransactionItemsTableTotalPagess = Math.Ceiling(CurrentTransactionItems.Count / TransactionItemsTableItemsPerPage)

        Label_SaleItemsTablePageControlPageNumber.Text = $"{TransactionItemsTableCurrentPage}/{TransactionItemsTableTotalPagess}"

        Dim TableLayoutPanel_TransactionItemsTableNewSale As New TableLayoutPanel With {
            .AutoSize = True,
            .AutoSizeMode = AutoSizeMode.GrowAndShrink,
            .BackColor = System.Drawing.Color.White,
            .CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
            .Margin = New Padding(0, 0, 0, 0),
            .MaximumSize = New Size(516, 0),
            .MinimumSize = New Size(516, 0)
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
            Dim item = CurrentTransactionItems(i)

            Dim checkbox As New Guna.UI2.WinForms.Guna2CheckBox With {
                .Anchor = AnchorStyles.Left,
                .BackColor = Color.Transparent,
                .CheckMarkColor = Color.FromArgb(34, 34, 34),
                .Margin = New Padding(12),
                .Size = New Size(18, 18)
            }

            AddHandler checkbox.CheckedChanged, Sub(sender As Object, e As EventArgs)
                If checkbox.Checked Then
                    CurrentSelectedTransactionItemIDs.Add(item.transaction_item_id)
                Else
                    CurrentSelectedTransactionItemIDs.Remove(item.transaction_item_id)
                End If
            End Sub

            Dim labelName As New Label With {
                .Anchor = AnchorStyles.Left,
                .AutoSize = True,
                .BackColor = Color.Transparent,
                .Font = New Font("Microsoft JhengHei", 12, FontStyle.Bold, GraphicsUnit.Pixel),
                .ForeColor = Color.FromArgb(2, 34, 34),
                .Margin = New Padding(12),
                .Text = item.transaction_item_name,
                .TextAlign = ContentAlignment.MiddleLeft
            }

            Dim textBoxQuantity As New Guna.UI2.WinForms.Guna2TextBox With {
                .Anchor = AnchorStyles.Left,
                .BackColor = Color.Transparent,
                .BorderColor = Color.FromArgb(34, 34, 34),
                .DefaultText = item.transaction_item_quantity.ToString(),
                .Font = New Font("Microsoft JhengHei", 14, FontStyle.Regular, GraphicsUnit.Pixel),
                .ForeColor = Color.FromArgb(34, 34, 34),
                .Margin = New Padding(12),
                .MaximumSize = New Size(60, 40),
                .MinimumSize = New Size(60, 32),
                .Size = New Size(60, 32),
                .TextOffset = New Point(-6, 0)
            }

            Dim labelPrice As New Label With {
                .Anchor = AnchorStyles.Left,
                .AutoSize = True,
                .BackColor = Color.Transparent,
                .Font = New Font("Microsoft JhengHei", 12, FontStyle.Bold, GraphicsUnit.Pixel),
                .ForeColor = Color.FromArgb(2, 34, 34),
                .Margin = New Padding(12),
                .Text = $"{item.transaction_item_selling_price:C2}",
                .TextAlign = ContentAlignment.MiddleLeft
            }

            Dim labelAmount As New Label With {
                .Anchor = AnchorStyles.Left,
                .AutoSize = True,
                .BackColor = Color.Transparent,
                .Font = New Font("Microsoft JhengHei", 12, FontStyle.Bold, GraphicsUnit.Pixel),
                .ForeColor = Color.FromArgb(2, 34, 34),
                .Margin = New Padding(12),
                .Text = $"{item.transaction_item_amount:C2}",
                .TextAlign = ContentAlignment.MiddleLeft
            }

            TableLayoutPanel_TransactionItemsTableNewSale.Controls.Add(checkbox, 0, i - StartIndex)
            TableLayoutPanel_TransactionItemsTableNewSale.Controls.Add(labelName, 1, i - StartIndex)
            TableLayoutPanel_TransactionItemsTableNewSale.Controls.Add(textBoxQuantity, 2, i - StartIndex)
            TableLayoutPanel_TransactionItemsTableNewSale.Controls.Add(labelPrice, 3, i - StartIndex)
            TableLayoutPanel_TransactionItemsTableNewSale.Controls.Add(labelAmount, 4, i - StartIndex)
        Next

        FlowLayoutPanel_SaleItemsTable.Controls.Add(TableLayoutPanel_TransactionItemsTableNewSale)
    End Sub

    Private Sub Button_SaleItemsTableDeleteNewSale_Click(sender As Object, e As EventArgs) Handles Button_SaleItemsTableDeleteNewSale.Click
        CurrentTransactionItems = CurrentTransactionItems.Where(Function(item) Not CurrentSelectedTransactionItemIDs.Contains(item.transaction_item_id)).ToList()
        CurrentSelectedTransactionItemIDs.Clear()
        UpdateTransactionItemsTable()

        Dim subtotal As Decimal = 0D

        For Each Item In CurrentTransactionItems
            subtotal += Item.transaction_item_amount
        Next

        CurrentTransactionEntry.subtotal = subtotal
        Label_SubtotalSaleScreen.Text = subtotal

        Dim vat_amount As Decimal = 0D
        Dim vat_percentage As Decimal = Convert.ToDecimal(Textbox_VATPercentageSaleScreen.Text) / 100D


        For Each Item In CurrentTransactionItems
            vat_amount += (Item.transaction_item_selling_price * Item.transaction_item_quantity) * vat_percentage
        Next

        CurrentTransactionEntry.net_total = subtotal + vat_amount
        CurrentTransactionEntry.tax_percentage = Textbox_VATPercentageSaleScreen.Text
        CurrentTransactionEntry.tax_amount = vat_amount

        Label_VATAmountSaleScreen.Text = vat_amount
        Label_NetTotalSaleScreen.Text = CurrentTransactionEntry.net_total
    End Sub

    Private Sub Button_SaleItemsTablePageControlNext_Click(sender As Object, e As EventArgs) Handles Button_SaleItemsTablePageControlNext.Click
        If TransactionItemsTableCurrentPage < TransactionItemsTableTotalPagess Then
            TransactionItemsTableCurrentPage += 1
            UpdateTransactionItemsTable()
        End If
    End Sub

    Private Sub Button_SaleItemsTablePageControlPrevious_Click(sender As Object, e As EventArgs) Handles Button_SaleItemsTablePageControlPrevious.Click
        If TransactionItemsTableCurrentPage > 1 Then
            TransactionItemsTableCurrentPage -= 1
            UpdateTransactionItemsTable()
        End If
    End Sub
    

    '' Methods to populate the data to be used
    Public Function CreateItemsArray() As _Item()
        Dim Items As _Item() = {
            New _Item With {.item_id = 0, .item_name = "Mixers", .item_quantity = 50, .item_selling_price = 1200.0D, .brand_id = 1, .category_id = 1, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 1, .item_name = "Roller Mills", .item_quantity = 40, .item_selling_price = 1500.0D, .brand_id = 1, .category_id = 1, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 2, .item_name = "Extruders", .item_quantity = 30, .item_selling_price = 2500.0D, .brand_id = 1, .category_id = 1, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 3, .item_name = "Winding Machines", .item_quantity = 25, .item_selling_price = 3000.0D, .brand_id = 1, .category_id = 1, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 4, .item_name = "Vulcanization Equipment", .item_quantity = 10, .item_selling_price = 3500.0D, .brand_id = 1, .category_id = 1, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 5, .item_name = "Grinders", .item_quantity = 60, .item_selling_price = 1000.0D, .brand_id = 1, .category_id = 1, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 6, .item_name = "Lathe Machines", .item_quantity = 15, .item_selling_price = 5000.0D, .brand_id = 1, .category_id = 1, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 7, .item_name = "Core Shafts", .item_quantity = 50, .item_selling_price = 1200.0D, .brand_id = 1, .category_id = 1, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 8, .item_name = "Pre-heaters", .item_quantity = 40, .item_selling_price = 2500.0D, .brand_id = 1, .category_id = 1, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 9, .item_name = "Cooling Systems", .item_quantity = 30, .item_selling_price = 2000.0D, .brand_id = 1, .category_id = 1, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 10, .item_name = "CNC Machines", .item_quantity = 20, .item_selling_price = 8000.0D, .brand_id = 1, .category_id = 1, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 11, .item_name = "Coating Machines", .item_quantity = 45, .item_selling_price = 1800.0D, .brand_id = 1, .category_id = 1, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 12, .item_name = "Polishing Machines", .item_quantity = 35, .item_selling_price = 2200.0D, .brand_id = 1, .category_id = 1, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 13, .item_name = "Offset Printing Plates", .item_quantity = 50, .item_selling_price = 700.0D, .brand_id = 2, .category_id = 2, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 14, .item_name = "Flexographic Plates", .item_quantity = 40, .item_selling_price = 750.0D, .brand_id = 2, .category_id = 2, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 15, .item_name = "Gravure Cylinders", .item_quantity = 30, .item_selling_price = 850.0D, .brand_id = 2, .category_id = 2, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 16, .item_name = "Letterpress Plates", .item_quantity = 20, .item_selling_price = 600.0D, .brand_id = 2, .category_id = 2, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 17, .item_name = "Thermographic Plates", .item_quantity = 15, .item_selling_price = 950.0D, .brand_id = 2, .category_id = 2, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 18, .item_name = "Rubber Compounds (natural rubber)", .item_quantity = 100, .item_selling_price = 100.0D, .brand_id = 3, .category_id = 3, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 19, .item_name = "Rubber Compounds (nitrile)", .item_quantity = 100, .item_selling_price = 120.0D, .brand_id = 3, .category_id = 3, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 20, .item_name = "Rubber Compounds (EPDM)", .item_quantity = 100, .item_selling_price = 130.0D, .brand_id = 3, .category_id = 3, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 21, .item_name = "Rubber Compounds (silicone)", .item_quantity = 100, .item_selling_price = 150.0D, .brand_id = 3, .category_id = 3, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 22, .item_name = "Rubber Compounds (polyurethane)", .item_quantity = 100, .item_selling_price = 180.0D, .brand_id = 3, .category_id = 3, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 23, .item_name = "Curing Agents (sulfur)", .item_quantity = 200, .item_selling_price = 50.0D, .brand_id = 3, .category_id = 3, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 24, .item_name = "Curing Agents (peroxides)", .item_quantity = 200, .item_selling_price = 60.0D, .brand_id = 3, .category_id = 3, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 25, .item_name = "Adhesives", .item_quantity = 150, .item_selling_price = 90.0D, .brand_id = 3, .category_id = 3, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 26, .item_name = "Solvents and Cleaners", .item_quantity = 180, .item_selling_price = 70.0D, .brand_id = 3, .category_id = 3, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 27, .item_name = "Anti-static Agents", .item_quantity = 100, .item_selling_price = 40.0D, .brand_id = 3, .category_id = 3, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 28, .item_name = "Release Agents", .item_quantity = 120, .item_selling_price = 85.0D, .brand_id = 3, .category_id = 3, .created_by = 1, .created_at = DateTime.Now}
            }

        Return Items
    End Function

    Function CreateCategoriesArray() As _Category()
        Dim Categories As _Category() = {
            New _Category With {.category_id = 1, .category_name = "Equipment for Rubber Roller", .created_by = 1, .created_at = DateTime.Now},
            New _Category With {.category_id = 2, .category_name = "Printing Plates", .created_by = 1, .created_at = DateTime.Now},
            New _Category With {.category_id = 3, .category_name = "Chemistry for Rubber Rollers and Printing", .created_by = 1, .created_at = DateTime.Now},
            New _Category With {.category_id = 4, .category_name = "Measuring Devices and Instrumentation", .created_by = 1, .created_at = DateTime.Now},
            New _Category With {.category_id = 5, .category_name = "Rubber Roller Applications in Printing", .created_by = 1, .created_at = DateTime.Now},
            New _Category With {.category_id = 6, .category_name = "Other Products", .created_by = 1, .created_at = DateTime.Now}
        }

        Return Categories
    End Function

    Public Function CreateBrandsArray() As _Brand()
        Dim Brands As _Brand() = {
            New _Brand With {.brand_id = 1, .brand_name = "RubberTech", .created_by = 1, .created_at = DateTime.Now},
            New _Brand With {.brand_id = 2, .brand_name = "PrintTech", .created_by = 1, .created_at = DateTime.Now},
            New _Brand With {.brand_id = 3, .brand_name = "ChemRubber", .created_by = 1, .created_at = DateTime.Now}
        }

        Return Brands
    End Function

    Public Function CreatePaymentermsArray() As _PaymentTerm()
        Dim items As _PaymentTerm() = {
            New _PaymentTerm With {
                .payment_term_id = 1,
                .payment_term_name = "Due on Receipt",
                .payment_term_value = 0,
                .created_by = 0,
                .created_at = DateTime.Now
            },
            New _PaymentTerm With {
                .payment_term_id = 2,
                .payment_term_name = "Net 15",
                .payment_term_value = 15,
                .created_by = 0,
                .created_at = DateTime.Now
            },
            New _PaymentTerm With {
                .payment_term_id = 3,
                .payment_term_name = "Net 30",
                .payment_term_value = 30,
                .created_by = 0,
                .created_at = DateTime.Now
            },
            New _PaymentTerm With {
                .payment_term_id = 4,
                .payment_term_name = "Net 45",
                .payment_term_value = 45,
                .created_by = 0,
                .created_at = DateTime.Now
            },
            New _PaymentTerm With {
                .payment_term_id = 5,
                .payment_term_name = "Net 60",
                .payment_term_value = 60,
                .created_by = 0,
                .created_at = DateTime.Now
            }
        }

        Return items
    End Function

    Public Function CreateTaxesArray() As _Tax()
        Dim items As _Tax() = {
            New _Tax With {
                .tax_id = 1,
                .tax_name = "Value Added Tax (VAT)",
                .tax_value = 0.12D,
                .created_by = 0,
                .created_at = DateTime.Now
            },
            New _Tax With {
                .tax_id = 2,
                .tax_name = "Sales Tax",
                .tax_value = 0.10D,
                .created_by = 0,
                .created_at = DateTime.Now
            },
            New _Tax With {
                .tax_id = 3,
                .tax_name = "Service Tax",
                .tax_value = 0.08D,
                .created_by = 0,
                .created_at = DateTime.Now
            },
            New _Tax With {
                .tax_id = 4,
                .tax_name = "Excise Tax",
                .tax_value = 0.06D,
                .created_by = 0,
                .created_at = DateTime.Now
            },
            New _Tax With {
                .tax_id = 5,
                .tax_name = "Environmental Tax",
                .tax_value = 0.02D,
                .created_by = 0,
                .created_at = DateTime.Now
            }
        }

        Return items
    End Function

    Public Function CreateCustomersArray() As _Customer()
        Dim items As _Customer() = New _Customer() {
            New _Customer With {
                .customer_id = 0,
                .customer_name = "Argente, Marcell R.",
                .company = "Argente Corp.",
                .email = "marcell@argente.com",
                .phone = "09123456789",
                .address_line = "123 Rubber St.",
                .city = "Metro City",
                .zip_code = "1234",
                .country = "CountryX",
                .created_by = 1,
                .created_at = DateTime.Now
            },
            New _Customer With {
                .customer_id = 1,
                .customer_name = "Bellen, Maricris B.",
                .company = "Bellen Industries",
                .email = "maricris@bellen.com",
                .phone = "09123456788",
                .address_line = "456 Print Ave.",
                .city = "Metro City",
                .zip_code = "5678",
                .country = "CountryX",
                .created_by = 1,
                .created_at = DateTime.Now
            },
            New _Customer With {
                .customer_id = 2,
                .customer_name = "Bucio, Joshua",
                .company = "Bucio Ltd.",
                .email = "joshua@bucio.com",
                .phone = "09123456787",
                .address_line = "789 Rubber Rd.",
                .city = "Metro City",
                .zip_code = "9012",
                .country = "CountryX",
                .created_by = 1,
                .created_at = DateTime.Now
            },
            New _Customer With {
                .customer_id = 3,
                .customer_name = "Flores, Shaira Erika D.",
                .company = "Flores Rubber",
                .email = "shaira@flores.com",
                .phone = "09123456786",
                .address_line = "321 Coating Blvd.",
                .city = "Metro City",
                .zip_code = "3456",
                .country = "CountryX",
                .created_by = 1,
                .created_at = DateTime.Now
            },
            New _Customer With {
                .customer_id = 4,
                .customer_name = "Naul, Lea C.",
                .company = "Naul Printing",
                .email = "lea@naul.com",
                .phone = "09123456785",
                .address_line = "654 Printing St.",
                .city = "Metro City",
                .zip_code = "7890",
                .country = "CountryX",
                .created_by = 1,
                .created_at = DateTime.Now
            },
            New _Customer With {
                .customer_id = 5,
                .customer_name = "Pederio, Michael Jhon R.",
                .company = "Pederio LLC",
                .email = "michael@pederio.com",
                .phone = "09123456784",
                .address_line = "987 Tension Ave.",
                .city = "Metro City",
                .zip_code = "2345",
                .country = "CountryX",
                .created_by = 1,
                .created_at = DateTime.Now
            },
            New _Customer With {
                .customer_id = 6,
                .customer_name = "Rollan, Benjamin B.",
                .company = "Rollan Goods",
                .email = "benjamin@rollan.com",
                .phone = "09123456783",
                .address_line = "654 Balancing St.",
                .city = "Metro City",
                .zip_code = "6789",
                .country = "CountryX",
                .created_by = 1,
                .created_at = DateTime.Now
            },
            New _Customer With {
                .customer_id = 7,
                .customer_name = "Trinidad, Eunice A.",
                .company = "Trinidad Supplies",
                .email = "eunice@trinidad.com",
                .phone = "09123456782",
                .address_line = "123 Testing Blvd.",
                .city = "Metro City",
                .zip_code = "1234",
                .country = "CountryX",
                .created_by = 1,
                .created_at = DateTime.Now
            },
            New _Customer With {
                .customer_id = 8,
                .customer_name = "Villanueva, John Carlos",
                .company = "Villanueva Enterprises",
                .email = "john@villanueva.com",
                .phone = "09123456781",
                .address_line = "456 Roller Rd.",
                .city = "Metro City",
                .zip_code = "5678",
                .country = "CountryX",
                .created_by = 1,
                .created_at = DateTime.Now
            }
        }


        Return items
    End Function


    ' Methods to call when  completing a transaction
    Private Sub CompleteTransaction(Prompt_PlaceOrder As Form)
        InsertTransactionInDB()
        ShowReceiptPrompt(CurrentTransactionEntry, CurrentTransactionItems, Prompt_PlaceOrder)
    End Sub

    Public Function GetNextTransactionId() As Integer
        Dim nextTransactionId As Integer = 0
    
        Dim query As String = "SELECT MAX(transaction_id) FROM transaction_entries"
    
        Using connection As New MySqlConnection(ConnectionString)
            Using command As New MySqlCommand(query, connection)
                connection.Open()

                Dim result = command.ExecuteScalar()
            
                If result IsNot Nothing Then
                    If Integer.TryParse(result.ToString(), nextTransactionId) Then
                        nextTransactionId += 1
                    End If
                End If
            End Using
        End Using
    
        Return nextTransactionId
    End Function

    Private Sub InsertTransactionInDB()
        CurrentTransactionEntry.created_by = 0
        CurrentTransactionEntry.created_at = DateTime.Now
        CurrentTransactionEntry.transaction_id = GetNextTransactionId()

        For Each item As _TransactionItem In CurrentTransactionItems
            item.transaction_entry_id = CurrentTransactionEntry.transaction_id
        Next

        Dim transactionEntryQuery As String = "INSERT INTO transaction_entries (customer_id, subtotal, net_total, received_amount, _change, tax_percentage, tax_amount, due_amount, due_date, status, created_by, created_at) " &
                                              "VALUES (@customer_id, @subtotal, @net_total, @received_amount, @_change, @tax_percentage, @tax_amount, @due_amount, @due_date, @status, @created_by, @created_at)"

        Dim transactionItemQuery As String = "INSERT INTO transaction_items (transaction_entry_id, transaction_item_name, transaction_item_quantity, transaction_item_selling_price, transaction_item_amount) " &
                                             "VALUES (@transaction_entry_id, @transaction_item_name, @transaction_item_quantity, @transaction_item_selling_price, @transaction_item_amount)"

        Using connection As New MySqlConnection(connectionString)
            Try
                connection.Open()

                Using transaction As MySqlTransaction = connection.BeginTransaction()
                    Try
                        Using command As New MySqlCommand(transactionEntryQuery, connection, transaction)
                            command.Parameters.AddWithValue("@customer_id", CurrentTransactionEntry.customer_id)
                            command.Parameters.AddWithValue("@subtotal", CurrentTransactionEntry.subtotal)
                            command.Parameters.AddWithValue("@net_total", CurrentTransactionEntry.net_total)
                            command.Parameters.AddWithValue("@received_amount", CurrentTransactionEntry.received_amount)
                            command.Parameters.AddWithValue("@_change", CurrentTransactionEntry.change)
                            command.Parameters.AddWithValue("@tax_percentage", CurrentTransactionEntry.tax_percentage)
                            command.Parameters.AddWithValue("@tax_amount", CurrentTransactionEntry.tax_amount)
                            command.Parameters.AddWithValue("@due_amount", CurrentTransactionEntry.due_amount)
                            command.Parameters.AddWithValue("@due_date", CurrentTransactionEntry.due_date)
                            command.Parameters.AddWithValue("@status", CurrentTransactionEntry.status)
                            command.Parameters.AddWithValue("@created_by", CurrentTransactionEntry.created_by)
                            command.Parameters.AddWithValue("@created_at", CurrentTransactionEntry.created_at)

                            command.ExecuteNonQuery()
                        End Using

                        For Each item As _TransactionItem In CurrentTransactionItems
                            Using command As New MySqlCommand(transactionItemQuery, connection, transaction)
                                command.Parameters.AddWithValue("@transaction_entry_id", item.transaction_entry_id)
                                command.Parameters.AddWithValue("@transaction_item_name", item.transaction_item_name)
                                command.Parameters.AddWithValue("@transaction_item_quantity", item.transaction_item_quantity)
                                command.Parameters.AddWithValue("@transaction_item_selling_price", item.transaction_item_selling_price)
                                command.Parameters.AddWithValue("@transaction_item_amount", item.transaction_item_amount)

                                command.ExecuteNonQuery()
                            End Using
                        Next

                        transaction.Commit()
                        Console.WriteLine("Transaction and items successfully inserted.")
                    Catch ex As Exception
                        transaction.Rollback()
                        Console.WriteLine($"Error during transaction: {ex.Message}")
                    End Try
                End Using
            Catch ex As Exception
                Console.WriteLine($"Database connection error: {ex.Message}")
            End Try
        End Using
    End Sub

    Private Sub ShowReceiptPrompt(transaction As _TransactionEntry, items As List(Of _TransactionItem),Prompt_PlaceOrder As Form)
        Dim companyName As String = "Ideal Marketing and Manufacturing Corporation"
        Dim companyAddress As String = "123 Business Blvd, Metro City, CountryX"

        Dim receipt As New System.Text.StringBuilder()

        receipt.AppendLine(companyName)
        receipt.AppendLine(companyAddress)
        receipt.AppendLine(New String("="c, 45))
        receipt.AppendLine("Transaction Receipt - " + CurrentTransactionEntry.status)
        receipt.AppendLine(New String("="c, 45))

        receipt.AppendLine($"Transaction ID: {transaction.transaction_id}")
        Dim customer = Customers.FirstOrDefault(Function(c) c.customer_id = transaction.customer_id)
        If customer IsNot Nothing Then
            receipt.AppendLine($"Customer: #{transaction.customer_id} - {customer.customer_name}")
        Else
            receipt.AppendLine("Customer: Unknown")
        End If
        receipt.AppendLine($"Subtotal: {transaction.subtotal:C2}")
        receipt.AppendLine($"Tax Percentage: {transaction.tax_percentage}%")
        receipt.AppendLine($"Tax Amount: {transaction.tax_amount:C2}")
        receipt.AppendLine($"Net Total: {transaction.net_total:C2}")
        receipt.AppendLine($"Received Amount: {transaction.received_amount:C2}")
        receipt.AppendLine($"Change: {transaction.change:C2}")
        receipt.AppendLine($"Due Amount: {transaction.due_amount:C2}")
        If CurrentTransactionEntry.due_date.Date = DateTime.Now.Date Then
            receipt.AppendLine("Due Date: Current Date: Due on Receipt")
        Else
            receipt.AppendLine($"Due Date: {CurrentTransactionEntry.due_date}")
        End If
        receipt.AppendLine($"Status: {transaction.status}")
        receipt.AppendLine($"Created By: {transaction.created_by}")
        receipt.AppendLine($"Created At: {transaction.created_at:g}")
        receipt.AppendLine(New String("="c, 45))

        receipt.AppendLine(String.Format("{0,-10}{1,-20}{2,10}", "Qty", "Item Name", "Amount"))
        receipt.AppendLine(New String("-"c, 45))
        For Each item In items
            receipt.AppendLine(String.Format("{0,-10}{1,-20}{2,10:C2}", item.transaction_item_quantity, item.transaction_item_name, item.transaction_item_amount))
        Next
        receipt.AppendLine(New String("="c, 45))

        Dim totalAmount As Decimal = items.Sum(Function(i) i.transaction_item_amount)
        receipt.AppendLine($"Total Amount: {totalAmount:C2}")
    
        Dim _Temporary_Receipt As New Form()

        Dim Label_ContentReceipt = New System.Windows.Forms.Label()
        Dim FlowLayoutPanel_Receipt = New System.Windows.Forms.FlowLayoutPanel()
    
        Label_ContentReceipt.Anchor = System.Windows.Forms.AnchorStyles.Left
        Label_ContentReceipt.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_ContentReceipt.Font = New System.Drawing.Font("Cascadia Mono", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_ContentReceipt.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Label_ContentReceipt.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_ContentReceipt.Location = New System.Drawing.Point(12, 12)
        Label_ContentReceipt.Margin = New System.Windows.Forms.Padding(12, 12, 6, 12)
        Label_ContentReceipt.Name = "Label_ContentReceipt"
        Label_ContentReceipt.Size = New System.Drawing.Size(696, 696)
        Label_ContentReceipt.TabIndex = 41
        Label_ContentReceipt.Text = receipt.ToString()
        '
        'FlowLayoutPanel_Receipt
        '
        FlowLayoutPanel_Receipt.Controls.Add(Label_ContentReceipt)
        FlowLayoutPanel_Receipt.Location = New System.Drawing.Point(0, 0)
        FlowLayoutPanel_Receipt.Margin = New System.Windows.Forms.Padding(0)
        FlowLayoutPanel_Receipt.Name = "FlowLayoutPanel_Receipt"
        FlowLayoutPanel_Receipt.Size = New System.Drawing.Size(720, 720)
        FlowLayoutPanel_Receipt.TabIndex = 40

        _Temporary_Receipt.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        _Temporary_Receipt.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        _Temporary_Receipt.ClientSize = New System.Drawing.Size(720, 720)
        _Temporary_Receipt.Controls.Add(FlowLayoutPanel_Receipt)
        _Temporary_Receipt.Name = "_Temporary_Receipt"
        _Temporary_Receipt.Text = "_Temporary_Receipt"
        _Temporary_Receipt.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        _Temporary_Receipt.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        _Temporary_Receipt.Location = New Point(Me.Location.X, Me.Location.Y)
        _Temporary_Receipt.ClientSize = New System.Drawing.Size(396, 720)

        FlowLayoutPanel_Receipt.ResumeLayout(false)
        ResumeLayout(false)

        AddHandler _Temporary_Receipt.FormClosed, Sub(sender As Object, e As EventArgs)
            CurrentTransactionItems.Clear()
            CurrentSelectedTransactionItemIDs.Clear()
            CurrentTransactionEntry = New _TransactionEntry()

            CurrentPage = 1
            CurrentCategoryPage = 1
            CurrentCategoryID = -1
            TransactionItemsTableCurrentPage = 1
            TransactionItemsTableTotalPagess = 1

            Dim subtotal As Decimal = 0D

            For Each Item In CurrentTransactionItems
                subtotal += Item.transaction_item_amount
            Next

            CurrentTransactionEntry.subtotal = subtotal
            Label_SubtotalSaleScreen.Text = subtotal

            Dim vat_amount As Decimal = 0D
            Dim vat_percentage As Decimal = Convert.ToDecimal(Textbox_VATPercentageSaleScreen.Text) / 100D


            For Each Item In CurrentTransactionItems
                vat_amount += (Item.transaction_item_selling_price * Item.transaction_item_quantity) * vat_percentage
            Next

            CurrentTransactionEntry.net_total = subtotal + vat_amount
            CurrentTransactionEntry.tax_percentage = Textbox_VATPercentageSaleScreen.Text
            CurrentTransactionEntry.tax_amount = vat_amount

            Label_VATAmountSaleScreen.Text = vat_amount
            Label_NetTotalSaleScreen.Text = CurrentTransactionEntry.net_total

            PopulateItemsGrid(CurrentCategoryID)
            PopulateCategoriesSelection()
            UpdateTransactionItemsTable()
            Prompt_PlaceOrder.Close()
        End Sub

        _Temporary_Receipt.ShowDialog(Me)

        ExportReceiptToPDF(FlowLayoutPanel_Receipt, "C:\ExportedPanel.pdf")
    End Sub

    Public Sub ExportReceiptToPDF(panel As Panel, outputPath As String)
        
    End Sub


    '' Show forms
    Private Sub ShowPlaceOrderPrompt()
        Dim Prompt_PlaceOrder As New Form()

        Dim FlowLayoutPanel_PlaceOrder = New FlowLayoutPanel
        Dim Label_SubHeader1PlaceOrder = New System.Windows.Forms.Label()
        Dim FlowLayoutPanel7 = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label_CustomersPlaceOrder = New System.Windows.Forms.Label()
        Dim LabelSubHeader2PlaceOrder = New System.Windows.Forms.Label()
        Dim FlowLayoutPanel_DueDatePlaceOrder = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label_DueDatePlaceOrder = New System.Windows.Forms.Label()
        Dim FlowLayoutPanel_ReceivedAmountPlaceOrder = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label_ReceivedAmountPlaceOrder = New System.Windows.Forms.Label()
        Dim Textbox_ReceivedAmountPlaceOrder = New Guna.UI2.WinForms.Guna2TextBox()
        Dim FlowLayoutPanel_ChangePlaceOrder = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label_ChangePlaceOrder = New System.Windows.Forms.Label()
        Dim Textbox_ChangePlaceOrder = New Guna.UI2.WinForms.Guna2TextBox()
        Dim FlowLayoutPanel_DueAmountPlaceOrder = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label_DueAmountPlaceOrder = New System.Windows.Forms.Label()
        Dim Textbox_DueAmountPlaceOrder = New Guna.UI2.WinForms.Guna2TextBox()
        Dim FlowLayoutPanel6 = New System.Windows.Forms.FlowLayoutPanel()
        Dim ComboBox_CustomersPlaceOrder = New Guna.UI2.WinForms.Guna2ComboBox()
        Dim FlowLayoutPanel_SaleDetailsCustomerDetailsPlaceOrder = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label_SaleDetailsCustomerDetailsHeadingPlaceOrder = New System.Windows.Forms.Label()
        Dim Labell_SaleDetailsCustomerDetailsPlaceOrder = New System.Windows.Forms.Label()
        Dim Button_NewCustomerPlaceOrder = New Guna.UI2.WinForms.Guna2Button()
        Dim Panel_CustomersPlaceOrder = New System.Windows.Forms.Panel()
        Dim FlowLayoutPanel_WindowHeaderPlaceOrder = New System.Windows.Forms.FlowLayoutPanel()
        Dim Panel_WindowHeaderPlaceOrder = New System.Windows.Forms.Panel()
        Dim Label_WindowHeaderPlaceOrder = New System.Windows.Forms.Label()
        Dim DateTimePicker_DueDatePlaceOrder = New Guna.UI2.WinForms.Guna2DateTimePicker()
        Dim Button_MaketransactionPlaceOrder = New Guna.UI2.WinForms.Guna2Button()
        

        FlowLayoutPanel_PlaceOrder.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        FlowLayoutPanel_PlaceOrder.Controls.Add(FlowLayoutPanel_WindowHeaderPlaceOrder)
        FlowLayoutPanel_PlaceOrder.Controls.Add(Label_SubHeader1PlaceOrder)
        FlowLayoutPanel_PlaceOrder.Controls.Add(FlowLayoutPanel7)
        FlowLayoutPanel_PlaceOrder.Controls.Add(FlowLayoutPanel_SaleDetailsCustomerDetailsPlaceOrder)
        FlowLayoutPanel_PlaceOrder.Controls.Add(LabelSubHeader2PlaceOrder)
        FlowLayoutPanel_PlaceOrder.Controls.Add(FlowLayoutPanel_DueDatePlaceOrder)
        FlowLayoutPanel_PlaceOrder.Controls.Add(FlowLayoutPanel_ReceivedAmountPlaceOrder)
        FlowLayoutPanel_PlaceOrder.Controls.Add(FlowLayoutPanel_ChangePlaceOrder)
        FlowLayoutPanel_PlaceOrder.Controls.Add(FlowLayoutPanel_DueAmountPlaceOrder)
        FlowLayoutPanel_PlaceOrder.Controls.Add(Button_MaketransactionPlaceOrder)
        FlowLayoutPanel_PlaceOrder.Location = New System.Drawing.Point(0, 0)
        FlowLayoutPanel_PlaceOrder.Margin = New System.Windows.Forms.Padding(0)
        FlowLayoutPanel_PlaceOrder.Name = "FlowLayoutPanel_PlaceOrder"
        FlowLayoutPanel_PlaceOrder.Size = New System.Drawing.Size(720, 720)

        Label_SubHeader1PlaceOrder.Anchor = System.Windows.Forms.AnchorStyles.Left
        Label_SubHeader1PlaceOrder.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_SubHeader1PlaceOrder.Font = New System.Drawing.Font("Microsoft JhengHei", 22!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_SubHeader1PlaceOrder.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Label_SubHeader1PlaceOrder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_SubHeader1PlaceOrder.Location = New System.Drawing.Point(12, 80)
        Label_SubHeader1PlaceOrder.Margin = New System.Windows.Forms.Padding(12, 0, 12, 24)
        Label_SubHeader1PlaceOrder.Name = "Label_SubHeader1PlaceOrder"
        Label_SubHeader1PlaceOrder.Size = New System.Drawing.Size(696, 28)
        Label_SubHeader1PlaceOrder.TabIndex = 18
        Label_SubHeader1PlaceOrder.Text = "Customer"
        Label_SubHeader1PlaceOrder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft

        FlowLayoutPanel7.AutoSize = true
        FlowLayoutPanel7.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel7.Controls.Add(Label_CustomersPlaceOrder)
        FlowLayoutPanel7.Controls.Add(Panel_CustomersPlaceOrder)
        FlowLayoutPanel7.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel7.Location = New System.Drawing.Point(12, 132)
        FlowLayoutPanel7.Margin = New System.Windows.Forms.Padding(12, 0, 6, 0)
        FlowLayoutPanel7.Name = "FlowLayoutPanel7"
        FlowLayoutPanel7.Size = New System.Drawing.Size(693, 56)
        FlowLayoutPanel7.TabIndex = 24

        Label_CustomersPlaceOrder.AutoSize = true
        Label_CustomersPlaceOrder.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_CustomersPlaceOrder.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_CustomersPlaceOrder.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_CustomersPlaceOrder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_CustomersPlaceOrder.Location = New System.Drawing.Point(0, 0)
        Label_CustomersPlaceOrder.Margin = New System.Windows.Forms.Padding(0)
        Label_CustomersPlaceOrder.Name = "Label_CustomersPlaceOrder"
        Label_CustomersPlaceOrder.Size = New System.Drawing.Size(61, 16)
        Label_CustomersPlaceOrder.TabIndex = 2
        Label_CustomersPlaceOrder.Text = "Customer"
        Label_CustomersPlaceOrder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft

        LabelSubHeader2PlaceOrder.Anchor = System.Windows.Forms.AnchorStyles.Left
        LabelSubHeader2PlaceOrder.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        LabelSubHeader2PlaceOrder.Font = New System.Drawing.Font("Microsoft JhengHei", 22!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        LabelSubHeader2PlaceOrder.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        LabelSubHeader2PlaceOrder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        LabelSubHeader2PlaceOrder.Location = New System.Drawing.Point(12, 349)
        LabelSubHeader2PlaceOrder.Margin = New System.Windows.Forms.Padding(12, 24, 12, 24)
        LabelSubHeader2PlaceOrder.Name = "LabelSubHeader2PlaceOrder"
        LabelSubHeader2PlaceOrder.Size = New System.Drawing.Size(696, 28)
        LabelSubHeader2PlaceOrder.TabIndex = 23
        LabelSubHeader2PlaceOrder.Text = "Payment"
        LabelSubHeader2PlaceOrder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft

        FlowLayoutPanel_DueDatePlaceOrder.AutoSize = true
        FlowLayoutPanel_DueDatePlaceOrder.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_DueDatePlaceOrder.Controls.Add(Label_DueDatePlaceOrder)
        FlowLayoutPanel_DueDatePlaceOrder.Controls.Add(DateTimePicker_DueDatePlaceOrder)
        FlowLayoutPanel_DueDatePlaceOrder.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_DueDatePlaceOrder.Location = New System.Drawing.Point(12, 401)
        FlowLayoutPanel_DueDatePlaceOrder.Margin = New System.Windows.Forms.Padding(12, 0, 6, 12)
        FlowLayoutPanel_DueDatePlaceOrder.Name = "FlowLayoutPanel_DueDatePlaceOrder"
        FlowLayoutPanel_DueDatePlaceOrder.Size = New System.Drawing.Size(342, 56)
        FlowLayoutPanel_DueDatePlaceOrder.TabIndex = 19

        Label_DueDatePlaceOrder.AutoSize = true
        Label_DueDatePlaceOrder.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_DueDatePlaceOrder.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_DueDatePlaceOrder.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_DueDatePlaceOrder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_DueDatePlaceOrder.Location = New System.Drawing.Point(0, 0)
        Label_DueDatePlaceOrder.Margin = New System.Windows.Forms.Padding(0)
        Label_DueDatePlaceOrder.Name = "Label_DueDatePlaceOrder"
        Label_DueDatePlaceOrder.Size = New System.Drawing.Size(88, 16)
        Label_DueDatePlaceOrder.TabIndex = 2
        Label_DueDatePlaceOrder.Text = "Due Date"
        Label_DueDatePlaceOrder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft

        FlowLayoutPanel_ReceivedAmountPlaceOrder.AutoSize = true
        FlowLayoutPanel_ReceivedAmountPlaceOrder.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_ReceivedAmountPlaceOrder.Controls.Add(Label_ReceivedAmountPlaceOrder)
        FlowLayoutPanel_ReceivedAmountPlaceOrder.Controls.Add(Textbox_ReceivedAmountPlaceOrder)
        FlowLayoutPanel_ReceivedAmountPlaceOrder.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_ReceivedAmountPlaceOrder.Location = New System.Drawing.Point(366, 401)
        FlowLayoutPanel_ReceivedAmountPlaceOrder.Margin = New System.Windows.Forms.Padding(6, 0, 12, 12)
        FlowLayoutPanel_ReceivedAmountPlaceOrder.Name = "FlowLayoutPanel_ReceivedAmountPlaceOrder"
        FlowLayoutPanel_ReceivedAmountPlaceOrder.Size = New System.Drawing.Size(342, 56)
        FlowLayoutPanel_ReceivedAmountPlaceOrder.TabIndex = 20

        Label_ReceivedAmountPlaceOrder.AutoSize = true
        Label_ReceivedAmountPlaceOrder.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_ReceivedAmountPlaceOrder.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_ReceivedAmountPlaceOrder.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_ReceivedAmountPlaceOrder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_ReceivedAmountPlaceOrder.Location = New System.Drawing.Point(0, 0)
        Label_ReceivedAmountPlaceOrder.Margin = New System.Windows.Forms.Padding(0)
        Label_ReceivedAmountPlaceOrder.Name = "Label_ReceivedAmountPlaceOrder"
        Label_ReceivedAmountPlaceOrder.Size = New System.Drawing.Size(107, 16)
        Label_ReceivedAmountPlaceOrder.TabIndex = 2
        Label_ReceivedAmountPlaceOrder.Text = "Received Amount"
        Label_ReceivedAmountPlaceOrder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft

        Textbox_ReceivedAmountPlaceOrder.Anchor = System.Windows.Forms.AnchorStyles.Left
        Textbox_ReceivedAmountPlaceOrder.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_ReceivedAmountPlaceOrder.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_ReceivedAmountPlaceOrder.Cursor = System.Windows.Forms.Cursors.IBeam
        Textbox_ReceivedAmountPlaceOrder.DefaultText = ""
        Textbox_ReceivedAmountPlaceOrder.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer))
        Textbox_ReceivedAmountPlaceOrder.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer))
        Textbox_ReceivedAmountPlaceOrder.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_ReceivedAmountPlaceOrder.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_ReceivedAmountPlaceOrder.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_ReceivedAmountPlaceOrder.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_ReceivedAmountPlaceOrder.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
        Textbox_ReceivedAmountPlaceOrder.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_ReceivedAmountPlaceOrder.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_ReceivedAmountPlaceOrder.Location = New System.Drawing.Point(0, 16)
        Textbox_ReceivedAmountPlaceOrder.Margin = New System.Windows.Forms.Padding(0)
        Textbox_ReceivedAmountPlaceOrder.Name = "Textbox_ReceivedAmountPlaceOrder"
        Textbox_ReceivedAmountPlaceOrder.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Textbox_ReceivedAmountPlaceOrder.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer))
        Textbox_ReceivedAmountPlaceOrder.PlaceholderText = "Enter Received Amount"
        Textbox_ReceivedAmountPlaceOrder.SelectedText = ""
        Textbox_ReceivedAmountPlaceOrder.Size = New System.Drawing.Size(342, 40)
        Textbox_ReceivedAmountPlaceOrder.TabIndex = 39

        FlowLayoutPanel_ChangePlaceOrder.AutoSize = true
        FlowLayoutPanel_ChangePlaceOrder.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_ChangePlaceOrder.Controls.Add(Label_ChangePlaceOrder)
        FlowLayoutPanel_ChangePlaceOrder.Controls.Add(Textbox_ChangePlaceOrder)
        FlowLayoutPanel_ChangePlaceOrder.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_ChangePlaceOrder.Location = New System.Drawing.Point(12, 469)
        FlowLayoutPanel_ChangePlaceOrder.Margin = New System.Windows.Forms.Padding(12, 0, 6, 12)
        FlowLayoutPanel_ChangePlaceOrder.Name = "FlowLayoutPanel_ChangePlaceOrder"
        FlowLayoutPanel_ChangePlaceOrder.Size = New System.Drawing.Size(342, 56)
        FlowLayoutPanel_ChangePlaceOrder.TabIndex = 21

        Label_ChangePlaceOrder.AutoSize = true
        Label_ChangePlaceOrder.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_ChangePlaceOrder.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_ChangePlaceOrder.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_ChangePlaceOrder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_ChangePlaceOrder.Location = New System.Drawing.Point(0, 0)
        Label_ChangePlaceOrder.Margin = New System.Windows.Forms.Padding(0)
        Label_ChangePlaceOrder.Name = "Label_ChangePlaceOrder"
        Label_ChangePlaceOrder.Size = New System.Drawing.Size(51, 16)
        Label_ChangePlaceOrder.TabIndex = 2
        Label_ChangePlaceOrder.Text = "Change"
        Label_ChangePlaceOrder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft

        Textbox_ChangePlaceOrder.Anchor = System.Windows.Forms.AnchorStyles.Left
        Textbox_ChangePlaceOrder.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_ChangePlaceOrder.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_ChangePlaceOrder.Cursor = System.Windows.Forms.Cursors.IBeam
        Textbox_ChangePlaceOrder.DefaultText = ""
        Textbox_ChangePlaceOrder.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer))
        Textbox_ChangePlaceOrder.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer))
        Textbox_ChangePlaceOrder.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_ChangePlaceOrder.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_ChangePlaceOrder.Enabled = false
        Textbox_ChangePlaceOrder.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_ChangePlaceOrder.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_ChangePlaceOrder.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
        Textbox_ChangePlaceOrder.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_ChangePlaceOrder.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_ChangePlaceOrder.Location = New System.Drawing.Point(0, 16)
        Textbox_ChangePlaceOrder.Margin = New System.Windows.Forms.Padding(0)
        Textbox_ChangePlaceOrder.Name = "Textbox_ChangePlaceOrder"
        Textbox_ChangePlaceOrder.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Textbox_ChangePlaceOrder.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer))
        Textbox_ChangePlaceOrder.PlaceholderText = "Change"
        Textbox_ChangePlaceOrder.SelectedText = ""
        Textbox_ChangePlaceOrder.Size = New System.Drawing.Size(342, 40)
        Textbox_ChangePlaceOrder.TabIndex = 39

        FlowLayoutPanel6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel6.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel6.Location = New System.Drawing.Point(366, 469)
        FlowLayoutPanel6.Margin = New System.Windows.Forms.Padding(6, 0, 12, 12)
        FlowLayoutPanel6.Name = "FlowLayoutPanel6"
        FlowLayoutPanel6.Size = New System.Drawing.Size(342, 56)
        FlowLayoutPanel6.TabIndex = 22

        ComboBox_CustomersPlaceOrder.BackColor = System.Drawing.Color.Transparent
        ComboBox_CustomersPlaceOrder.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        ComboBox_CustomersPlaceOrder.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        ComboBox_CustomersPlaceOrder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        ComboBox_CustomersPlaceOrder.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        ComboBox_CustomersPlaceOrder.FocusedColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        ComboBox_CustomersPlaceOrder.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        ComboBox_CustomersPlaceOrder.Font = New System.Drawing.Font("Segoe UI", 10!)
        ComboBox_CustomersPlaceOrder.ForeColor = System.Drawing.Color.FromArgb(CType(CType(68,Byte),Integer), CType(CType(88,Byte),Integer), CType(CType(112,Byte),Integer))
        ComboBox_CustomersPlaceOrder.ItemHeight = 34
        ComboBox_CustomersPlaceOrder.Items.AddRange(New Object() {"Benjamin Rollan", "Benjamin Rollan", "Benjamin Rollan", "Benjamin Rollan"})
        ComboBox_CustomersPlaceOrder.Location = New System.Drawing.Point(0, 0)
        ComboBox_CustomersPlaceOrder.Margin = New System.Windows.Forms.Padding(0)
        ComboBox_CustomersPlaceOrder.Name = "ComboBox_CustomersPlaceOrder"
        ComboBox_CustomersPlaceOrder.Size = New System.Drawing.Size(549, 40)
        ComboBox_CustomersPlaceOrder.TabIndex = 31

        FlowLayoutPanel_SaleDetailsCustomerDetailsPlaceOrder.AutoSize = true
        FlowLayoutPanel_SaleDetailsCustomerDetailsPlaceOrder.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_SaleDetailsCustomerDetailsPlaceOrder.Controls.Add(Label_SaleDetailsCustomerDetailsHeadingPlaceOrder)
        FlowLayoutPanel_SaleDetailsCustomerDetailsPlaceOrder.Controls.Add(Labell_SaleDetailsCustomerDetailsPlaceOrder)
        FlowLayoutPanel_SaleDetailsCustomerDetailsPlaceOrder.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_SaleDetailsCustomerDetailsPlaceOrder.Location = New System.Drawing.Point(12, 200)
        FlowLayoutPanel_SaleDetailsCustomerDetailsPlaceOrder.Margin = New System.Windows.Forms.Padding(12, 12, 0, 19)
        FlowLayoutPanel_SaleDetailsCustomerDetailsPlaceOrder.Name = "FlowLayoutPanel_SaleDetailsCustomerDetailsPlaceOrder"
        FlowLayoutPanel_SaleDetailsCustomerDetailsPlaceOrder.Size = New System.Drawing.Size(133, 106)
        FlowLayoutPanel_SaleDetailsCustomerDetailsPlaceOrder.TabIndex = 30

        Label_SaleDetailsCustomerDetailsHeadingPlaceOrder.Anchor = System.Windows.Forms.AnchorStyles.Left
        Label_SaleDetailsCustomerDetailsHeadingPlaceOrder.AutoSize = true
        Label_SaleDetailsCustomerDetailsHeadingPlaceOrder.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_SaleDetailsCustomerDetailsHeadingPlaceOrder.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_SaleDetailsCustomerDetailsHeadingPlaceOrder.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_SaleDetailsCustomerDetailsHeadingPlaceOrder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_SaleDetailsCustomerDetailsHeadingPlaceOrder.Location = New System.Drawing.Point(0, 0)
        Label_SaleDetailsCustomerDetailsHeadingPlaceOrder.Margin = New System.Windows.Forms.Padding(0, 0, 0, 8)
        Label_SaleDetailsCustomerDetailsHeadingPlaceOrder.Name = "Label_SaleDetailsCustomerDetailsHeadingPlaceOrder"
        Label_SaleDetailsCustomerDetailsHeadingPlaceOrder.Size = New System.Drawing.Size(125, 18)
        Label_SaleDetailsCustomerDetailsHeadingPlaceOrder.TabIndex = 3
        Label_SaleDetailsCustomerDetailsHeadingPlaceOrder.Text = "Customer Details"
        Label_SaleDetailsCustomerDetailsHeadingPlaceOrder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft

        Labell_SaleDetailsCustomerDetailsPlaceOrder.Anchor = System.Windows.Forms.AnchorStyles.Left
        Labell_SaleDetailsCustomerDetailsPlaceOrder.AutoSize = true
        Labell_SaleDetailsCustomerDetailsPlaceOrder.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Labell_SaleDetailsCustomerDetailsPlaceOrder.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Labell_SaleDetailsCustomerDetailsPlaceOrder.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Labell_SaleDetailsCustomerDetailsPlaceOrder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Labell_SaleDetailsCustomerDetailsPlaceOrder.Location = New System.Drawing.Point(0, 26)
        Labell_SaleDetailsCustomerDetailsPlaceOrder.Margin = New System.Windows.Forms.Padding(0)
        Labell_SaleDetailsCustomerDetailsPlaceOrder.Name = "Labell_SaleDetailsCustomerDetailsPlaceOrder"
        Labell_SaleDetailsCustomerDetailsPlaceOrder.Size = New System.Drawing.Size(133, 80)
        Labell_SaleDetailsCustomerDetailsPlaceOrder.TabIndex = 4
        Labell_SaleDetailsCustomerDetailsPlaceOrder.Text = "Customer ID: No value"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Company: No value"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Email: No value"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Phone: No value"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Addre"& _ 
    "ss: No value"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)
        Labell_SaleDetailsCustomerDetailsPlaceOrder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft

        Button_NewCustomerPlaceOrder.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(44,Byte),Integer), CType(CType(62,Byte),Integer))
        Button_NewCustomerPlaceOrder.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Button_NewCustomerPlaceOrder.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Button_NewCustomerPlaceOrder.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer))
        Button_NewCustomerPlaceOrder.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer))
        Button_NewCustomerPlaceOrder.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Button_NewCustomerPlaceOrder.Font = New System.Drawing.Font("Segoe UI", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel)
        Button_NewCustomerPlaceOrder.ForeColor = System.Drawing.Color.White
        Button_NewCustomerPlaceOrder.Location = New System.Drawing.Point(561, 0)
        Button_NewCustomerPlaceOrder.Margin = New System.Windows.Forms.Padding(12, 0, 0, 0)
        Button_NewCustomerPlaceOrder.Name = "Button_NewCustomerPlaceOrder"
        Button_NewCustomerPlaceOrder.Size = New System.Drawing.Size(132, 40)
        Button_NewCustomerPlaceOrder.TabIndex = 31
        Button_NewCustomerPlaceOrder.Text = "New Customer"
        Button_NewCustomerPlaceOrder.TextOffset = New System.Drawing.Point(0, -2)

        Panel_CustomersPlaceOrder.Controls.Add(ComboBox_CustomersPlaceOrder)
        Panel_CustomersPlaceOrder.Controls.Add(Button_NewCustomerPlaceOrder)
        Panel_CustomersPlaceOrder.Location = New System.Drawing.Point(0, 16)
        Panel_CustomersPlaceOrder.Margin = New System.Windows.Forms.Padding(0)
        Panel_CustomersPlaceOrder.Name = "Panel_CustomersPlaceOrder"
        Panel_CustomersPlaceOrder.Size = New System.Drawing.Size(693, 40)
        Panel_CustomersPlaceOrder.TabIndex = 31

        FlowLayoutPanel_WindowHeaderPlaceOrder.AutoSize = true
        FlowLayoutPanel_WindowHeaderPlaceOrder.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_WindowHeaderPlaceOrder.Controls.Add(Panel_WindowHeaderPlaceOrder)
        FlowLayoutPanel_WindowHeaderPlaceOrder.Controls.Add(Label_WindowHeaderPlaceOrder)
        FlowLayoutPanel_WindowHeaderPlaceOrder.Location = New System.Drawing.Point(12, 24)
        FlowLayoutPanel_WindowHeaderPlaceOrder.Margin = New System.Windows.Forms.Padding(12, 24, 0, 24)
        FlowLayoutPanel_WindowHeaderPlaceOrder.Name = "FlowLayoutPanel_WindowHeaderPlaceOrder"
        FlowLayoutPanel_WindowHeaderPlaceOrder.Size = New System.Drawing.Size(197, 32)
        FlowLayoutPanel_WindowHeaderPlaceOrder.TabIndex = 31
        
        Panel_WindowHeaderPlaceOrder.Anchor = System.Windows.Forms.AnchorStyles.Left
        Panel_WindowHeaderPlaceOrder.BackgroundImage = Global.ERP_SIMS.My.Resources.Resources.HeaderIcon_Sale
        Panel_WindowHeaderPlaceOrder.Location = New System.Drawing.Point(0, 0)
        Panel_WindowHeaderPlaceOrder.Margin = New System.Windows.Forms.Padding(0, 0, 16, 0)
        Panel_WindowHeaderPlaceOrder.Name = "Panel_WindowHeaderPlaceOrder"
        Panel_WindowHeaderPlaceOrder.Size = New System.Drawing.Size(32, 32)
        Panel_WindowHeaderPlaceOrder.TabIndex = 3

        Label_WindowHeaderPlaceOrder.Anchor = System.Windows.Forms.AnchorStyles.Left
        Label_WindowHeaderPlaceOrder.AutoSize = true
        Label_WindowHeaderPlaceOrder.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_WindowHeaderPlaceOrder.Font = New System.Drawing.Font("Microsoft JhengHei", 24!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_WindowHeaderPlaceOrder.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_WindowHeaderPlaceOrder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_WindowHeaderPlaceOrder.Location = New System.Drawing.Point(48, 0)
        Label_WindowHeaderPlaceOrder.Margin = New System.Windows.Forms.Padding(0)
        Label_WindowHeaderPlaceOrder.Name = "Label_WindowHeaderPlaceOrder"
        Label_WindowHeaderPlaceOrder.Size = New System.Drawing.Size(149, 31)
        Label_WindowHeaderPlaceOrder.TabIndex = 2
        Label_WindowHeaderPlaceOrder.Text = "Place Order"
        Label_WindowHeaderPlaceOrder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft

        DateTimePicker_DueDatePlaceOrder.BackColor = System.Drawing.Color.Transparent
        DateTimePicker_DueDatePlaceOrder.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        DateTimePicker_DueDatePlaceOrder.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        DateTimePicker_DueDatePlaceOrder.FocusedColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        DateTimePicker_DueDatePlaceOrder.Font = New System.Drawing.Font("Segoe UI", 10!)
        DateTimePicker_DueDatePlaceOrder.ForeColor = System.Drawing.Color.FromArgb(CType(CType(68,Byte),Integer), CType(CType(88,Byte),Integer), CType(CType(112,Byte),Integer))
        DateTimePicker_DueDatePlaceOrder.Location = New System.Drawing.Point(0, 16)
        DateTimePicker_DueDatePlaceOrder.Margin = New System.Windows.Forms.Padding(0)
        DateTimePicker_DueDatePlaceOrder.Name = "DateTimePicker_DueDatePlaceOrder"
        DateTimePicker_DueDatePlaceOrder.Size = New System.Drawing.Size(342, 40)
        DateTimePicker_DueDatePlaceOrder.TabIndex = 32

        Button_MaketransactionPlaceOrder.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(44,Byte),Integer), CType(CType(62,Byte),Integer))
        Button_MaketransactionPlaceOrder.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Button_MaketransactionPlaceOrder.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Button_MaketransactionPlaceOrder.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer))
        Button_MaketransactionPlaceOrder.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer))
        Button_MaketransactionPlaceOrder.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Button_MaketransactionPlaceOrder.Font = New System.Drawing.Font("Segoe UI", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel)
        Button_MaketransactionPlaceOrder.ForeColor = System.Drawing.Color.White
        Button_MaketransactionPlaceOrder.Location = New System.Drawing.Point(12, 549)
        Button_MaketransactionPlaceOrder.Margin = New System.Windows.Forms.Padding(12, 12, 6, 12)
        Button_MaketransactionPlaceOrder.Name = "Button_MaketransactionPlaceOrder"
        Button_MaketransactionPlaceOrder.Size = New System.Drawing.Size(693, 40)
        Button_MaketransactionPlaceOrder.TabIndex = 32
        Button_MaketransactionPlaceOrder.Text = "Make Transaction"
        Button_MaketransactionPlaceOrder.TextOffset = New System.Drawing.Point(0, -2)
        
        FlowLayoutPanel_DueAmountPlaceOrder.AutoSize = true
        FlowLayoutPanel_DueAmountPlaceOrder.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_DueAmountPlaceOrder.Controls.Add( Label_DueAmountPlaceOrder)
        FlowLayoutPanel_DueAmountPlaceOrder.Controls.Add( Textbox_DueAmountPlaceOrder)
        FlowLayoutPanel_DueAmountPlaceOrder.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_DueAmountPlaceOrder.Location = New System.Drawing.Point(366, 469)
        FlowLayoutPanel_DueAmountPlaceOrder.Margin = New System.Windows.Forms.Padding(6, 0, 12, 12)
        FlowLayoutPanel_DueAmountPlaceOrder.Name = "FlowLayoutPanel_DueAmountPlaceOrder"
        FlowLayoutPanel_DueAmountPlaceOrder.Size = New System.Drawing.Size(342, 56)
        FlowLayoutPanel_DueAmountPlaceOrder.TabIndex = 22

        Label_DueAmountPlaceOrder.AutoSize = true
        Label_DueAmountPlaceOrder.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_DueAmountPlaceOrder.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_DueAmountPlaceOrder.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_DueAmountPlaceOrder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_DueAmountPlaceOrder.Location = New System.Drawing.Point(0, 0)
        Label_DueAmountPlaceOrder.Margin = New System.Windows.Forms.Padding(0)
        Label_DueAmountPlaceOrder.Name = "Label_DueAmountPlaceOrder"
        Label_DueAmountPlaceOrder.Size = New System.Drawing.Size(51, 16)
        Label_DueAmountPlaceOrder.TabIndex = 3
        Label_DueAmountPlaceOrder.Text = "Due Amount"
        Label_DueAmountPlaceOrder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
    
        Textbox_DueAmountPlaceOrder.Anchor = System.Windows.Forms.AnchorStyles.Left
        Textbox_DueAmountPlaceOrder.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_DueAmountPlaceOrder.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_DueAmountPlaceOrder.Cursor = System.Windows.Forms.Cursors.IBeam
        Textbox_DueAmountPlaceOrder.DefaultText = "0.00"
        Textbox_DueAmountPlaceOrder.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer))
        Textbox_DueAmountPlaceOrder.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer))
        Textbox_DueAmountPlaceOrder.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_DueAmountPlaceOrder.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_DueAmountPlaceOrder.Enabled = false
        Textbox_DueAmountPlaceOrder.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_DueAmountPlaceOrder.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_DueAmountPlaceOrder.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
        Textbox_DueAmountPlaceOrder.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_DueAmountPlaceOrder.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_DueAmountPlaceOrder.Location = New System.Drawing.Point(0, 16)
        Textbox_DueAmountPlaceOrder.Margin = New System.Windows.Forms.Padding(0)
        Textbox_DueAmountPlaceOrder.Name = "Textbox_DueAmountPlaceOrder"
        Textbox_DueAmountPlaceOrder.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Textbox_DueAmountPlaceOrder.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer))
        Textbox_DueAmountPlaceOrder.PlaceholderText = "Change"
        Textbox_DueAmountPlaceOrder.SelectedText = ""
        Textbox_DueAmountPlaceOrder.Size = New System.Drawing.Size(342, 40)
        Textbox_DueAmountPlaceOrder.TabIndex = 40

        Prompt_PlaceOrder.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Prompt_PlaceOrder.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Prompt_PlaceOrder.ClientSize = New System.Drawing.Size(720, 720)
        Prompt_PlaceOrder.Controls.Add(FlowLayoutPanel_PlaceOrder)
        Prompt_PlaceOrder.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Prompt_PlaceOrder.Name = "Prompt_PlaceOrder"
        Prompt_PlaceOrder.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Prompt_PlaceOrder.Location = New Point(Me.Location.X, Me.Location.Y)

        FlowLayoutPanel_PlaceOrder.ResumeLayout(false)
        FlowLayoutPanel_PlaceOrder.PerformLayout
        FlowLayoutPanel7.ResumeLayout(false)
        FlowLayoutPanel7.PerformLayout
        FlowLayoutPanel_DueDatePlaceOrder.ResumeLayout(false)
        FlowLayoutPanel_DueDatePlaceOrder.PerformLayout
        FlowLayoutPanel_ReceivedAmountPlaceOrder.ResumeLayout(false)
        FlowLayoutPanel_ReceivedAmountPlaceOrder.PerformLayout
        FlowLayoutPanel_ChangePlaceOrder.ResumeLayout(false)
        FlowLayoutPanel_ChangePlaceOrder.PerformLayout
        FlowLayoutPanel_SaleDetailsCustomerDetailsPlaceOrder.ResumeLayout(false)
        FlowLayoutPanel_SaleDetailsCustomerDetailsPlaceOrder.PerformLayout
        Panel_CustomersPlaceOrder.ResumeLayout(false)
        FlowLayoutPanel_WindowHeaderPlaceOrder.ResumeLayout(false)
        FlowLayoutPanel_WindowHeaderPlaceOrder.PerformLayout
        ResumeLayout(false)

        ComboBox_CustomersPlaceOrder.Items.Clear()
        CurrentTransactionEntry.due_date = DateTime.Now

        AddHandler DateTimePicker_DueDatePlaceOrder.ValueChanged, Sub(sender As Object, e As EventArgs)
            Dim dueDate As String = If(DateTimePicker_DueDatePlaceOrder.Value.Date = DateTime.Now.Date,
            "Due on Receipt",
            DateTimePicker_DueDatePlaceOrder.Value.ToString("MM/dd/yyyy hh:mm tt"))
            CurrentTransactionEntry.due_date = DateTime.Parse(dueDate)
        End Sub

        AddHandler Button_NewCustomerPlaceOrder.Click, Sub(sender As Object, e As EventArgs)
            ShowNewCustomerPrompt()
        End Sub

        AddHandler Button_MaketransactionPlaceOrder.Click, Sub(sender As Object, e As EventArgs)
            If CurrentTransactionEntry.due_date.Date = DateTime.Now.Date Then
                CurrentTransactionEntry.status = "Completed"
            
            ElseIf CurrentTransactionEntry.due_date.Date <> DateTime.Now.Date AndAlso CurrentTransactionEntry.customer_id <> 0 Then
                CurrentTransactionEntry.status = "Pending"
            End If

            CompleteTransaction(Prompt_PlaceOrder)
        End Sub


        AddHandler Textbox_ReceivedAmountPlaceOrder.TextChanged, Sub(sender As Object, e As EventArgs)
            If Not String.IsNullOrEmpty(Textbox_ReceivedAmountPlaceOrder.Text) Then
                Dim received As Decimal
                If Decimal.TryParse(Textbox_ReceivedAmountPlaceOrder.Text, received) Then
                    CurrentTransactionEntry.received_amount = received

                    Dim due_amount As Decimal = CurrentTransactionEntry.net_total - received
                    Dim change As Decimal = 0

                    If due_amount > 0 Then
                        CurrentTransactionEntry.due_amount = due_amount
                        CurrentTransactionEntry.change = 0
                    Else
                        CurrentTransactionEntry.due_amount = 0
                        change = Math.Abs(due_amount)
                        CurrentTransactionEntry.change = change
                    End If

                    Textbox_ChangePlaceOrder.Text = change.ToString("F2")
                    Textbox_DueAmountPlaceOrder.Text = CurrentTransactionEntry.due_amount.ToString("F2")
                Else
                    MessageBox.Show("Please enter a valid numeric value.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If
            Else
                CurrentTransactionEntry.due_amount = CurrentTransactionEntry.net_total
                CurrentTransactionEntry.change = 0
                Textbox_ChangePlaceOrder.Text = "0.00"
                Textbox_DueAmountPlaceOrder.Text = CurrentTransactionEntry.net_total.ToString("F2")
            End If
        End Sub

        AddHandler ComboBox_CustomersPlaceOrder.SelectedIndexChanged, Sub(sender As Object, e As EventArgs)
            Dim selectedItem = ComboBox_CustomersPlaceOrder.Text

            If selectedItem IsNot Nothing Then
                Dim selectedCustomerName As String = selectedItem
                Dim selectedCustomer As _Customer = Customers.FirstOrDefault(Function(c) c.customer_name = selectedCustomerName)

                If selectedCustomer IsNot Nothing Then
                    CurrenttransactionEntry.customer_id = selectedCustomer.customer_id
                    Labell_SaleDetailsCustomerDetailsPlaceOrder.Text = $"Customer ID: {selectedCustomer.customer_id}" & vbCrLf &
                                                                    $"Company: {selectedCustomer.company}" & vbCrLf &
                                                                    $"Email: {selectedCustomer.email}" & vbCrLf &
                                                                    $"Phone: {selectedCustomer.phone}" & vbCrLf &
                                                                    $"Address: {selectedCustomer.address_line}, {selectedCustomer.city}, {selectedCustomer.zip_code}, {selectedCustomer.country}"
                Else
                    Labell_SaleDetailsCustomerDetailsPlaceOrder.Text = "Customer ID: No value" & vbCrLf &
                                                                    "Company: No value" & vbCrLf &
                                                                    "Email: No value" & vbCrLf &
                                                                    "Phone: No value" & vbCrLf &
                                                                    "Address: No value"
                End If
            End If
        End Sub

        For Each customer In Customers
            Dim customer_name As String = customer.customer_name
            ComboBox_CustomersPlaceOrder.Items.Add(customer_name)
        Next


        Prompt_PlaceOrder.ShowDialog(Me)
    End Sub

    Private Sub ShowNewCustomerPrompt()
        Dim Prompt_NewCustomer As New Form()
        
        Dim FlowLayoutPanel_NewCustomer = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label_CustomerDetailsNewCustomer = New System.Windows.Forms.Label()
        Dim FlowLayoutPanel_CustomerDetailCustomerNameInputNewCustomer = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label_CustomerDetailCustomerNameInputNewCustomer = New System.Windows.Forms.Label()
        Dim Textbox_CustomerDetailCustomerNameInputNewCustomer = New Guna.UI2.WinForms.Guna2TextBox()
        Dim FlowLayoutPanel_CustomerDetailCustomerCompanyInputNewCustomer = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label13 = New System.Windows.Forms.Label()
        Dim Textbox_CustomerDetailCustomerCompanyInputNewCustomer = New Guna.UI2.WinForms.Guna2TextBox()
        Dim FlowLayoutPanel_CustomerDetailCustomerEmailInputNewCustomer = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label14 = New System.Windows.Forms.Label()
        Dim Textbox_CustomerDetailCustomerEmailInputNewCustomer = New Guna.UI2.WinForms.Guna2TextBox()
        Dim FlowLayoutPanel_CustomerDetailCustomerPhoneInputNewCustomer = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label15 = New System.Windows.Forms.Label()
        Dim Textbox_CustomerDetailCustomerPhoneInputNewCustomer = New Guna.UI2.WinForms.Guna2TextBox()
        Dim s = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label10 = New System.Windows.Forms.Label()
        Dim Textbox_CustomerDetailCustomerAddressLineInputNewCustomer = New Guna.UI2.WinForms.Guna2TextBox()
        Dim FlowLayoutPanel_CustomerDetailCustomerCityInputNewCustomer = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label11 = New System.Windows.Forms.Label()
        Dim Textbox_CustomerDetailCustomerCityInputNewCustomer = New Guna.UI2.WinForms.Guna2TextBox()
        Dim FlowLayoutPanel_CustomerDetailCustomerZipCodeInputNewCustomer = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label17 = New System.Windows.Forms.Label()
        Dim Textbox_CustomerDetailCustomerZipCodeInputNewCustomer = New Guna.UI2.WinForms.Guna2TextBox()
        Dim FlowLayoutPanel_CustomerDetailCustomerCountryInputNewCustomer = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label18 = New System.Windows.Forms.Label()
        Dim Textbox_CustomerDetailCustomerCountryInputNewCustomer = New Guna.UI2.WinForms.Guna2TextBox()
        Dim FlowLayoutPanel_PageHeaderNewCustomer = New System.Windows.Forms.FlowLayoutPanel()
        Dim Panel_PageHeaderIconNewCustomer = New System.Windows.Forms.Panel()
        Dim Label_PageHeaderNewCustomer = New System.Windows.Forms.Label()
        Dim Button_MakeCustomerNewSale = New Guna.UI2.WinForms.Guna2Button()
        Dim Button_ResetNewCustomer = New Guna.UI2.WinForms.Guna2Button()
        
        FlowLayoutPanel_NewCustomer.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        FlowLayoutPanel_NewCustomer.Controls.Add(FlowLayoutPanel_PageHeaderNewCustomer)
        FlowLayoutPanel_NewCustomer.Controls.Add(Label_CustomerDetailsNewCustomer)
        FlowLayoutPanel_NewCustomer.Controls.Add(FlowLayoutPanel_CustomerDetailCustomerNameInputNewCustomer)
        FlowLayoutPanel_NewCustomer.Controls.Add(FlowLayoutPanel_CustomerDetailCustomerCompanyInputNewCustomer)
        FlowLayoutPanel_NewCustomer.Controls.Add(FlowLayoutPanel_CustomerDetailCustomerEmailInputNewCustomer)
        FlowLayoutPanel_NewCustomer.Controls.Add(FlowLayoutPanel_CustomerDetailCustomerPhoneInputNewCustomer)
        FlowLayoutPanel_NewCustomer.Controls.Add(s)
        FlowLayoutPanel_NewCustomer.Controls.Add(FlowLayoutPanel_CustomerDetailCustomerCityInputNewCustomer)
        FlowLayoutPanel_NewCustomer.Controls.Add(FlowLayoutPanel_CustomerDetailCustomerZipCodeInputNewCustomer)
        FlowLayoutPanel_NewCustomer.Controls.Add(FlowLayoutPanel_CustomerDetailCustomerCountryInputNewCustomer)
        FlowLayoutPanel_NewCustomer.Controls.Add(Button_ResetNewCustomer)
        FlowLayoutPanel_NewCustomer.Controls.Add(Button_MakeCustomerNewSale)
        FlowLayoutPanel_NewCustomer.Location = New System.Drawing.Point(0, 0)
        FlowLayoutPanel_NewCustomer.Margin = New System.Windows.Forms.Padding(0)
        FlowLayoutPanel_NewCustomer.Name = "FlowLayoutPanel_NewCustomer"
        FlowLayoutPanel_NewCustomer.Size = New System.Drawing.Size(720, 720)
        FlowLayoutPanel_NewCustomer.TabIndex = 33
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
        Label_CustomerDetailCustomerNameInputNewCustomer.Size = New System.Drawing.Size(42, 16)
        Label_CustomerDetailCustomerNameInputNewCustomer.TabIndex = 2
        Label_CustomerDetailCustomerNameInputNewCustomer.Text = "Name"
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
        Textbox_CustomerDetailCustomerNameInputNewCustomer.PlaceholderText = "Select a customer. Type ""("" to show all"
        Textbox_CustomerDetailCustomerNameInputNewCustomer.SelectedText = ""
        Textbox_CustomerDetailCustomerNameInputNewCustomer.Size = New System.Drawing.Size(342, 40)
        Textbox_CustomerDetailCustomerNameInputNewCustomer.TabIndex = 39
        '
        'FlowLayoutPanel_CustomerDetailCustomerCompanyInputNewCustomer
        '
        FlowLayoutPanel_CustomerDetailCustomerCompanyInputNewCustomer.AutoSize = true
        FlowLayoutPanel_CustomerDetailCustomerCompanyInputNewCustomer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_CustomerDetailCustomerCompanyInputNewCustomer.Controls.Add(Label13)
        FlowLayoutPanel_CustomerDetailCustomerCompanyInputNewCustomer.Controls.Add(Textbox_CustomerDetailCustomerCompanyInputNewCustomer)
        FlowLayoutPanel_CustomerDetailCustomerCompanyInputNewCustomer.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_CustomerDetailCustomerCompanyInputNewCustomer.Location = New System.Drawing.Point(366, 132)
        FlowLayoutPanel_CustomerDetailCustomerCompanyInputNewCustomer.Margin = New System.Windows.Forms.Padding(6, 0, 12, 12)
        FlowLayoutPanel_CustomerDetailCustomerCompanyInputNewCustomer.Name = "FlowLayoutPanel_CustomerDetailCustomerCompanyInputNewCustomer"
        FlowLayoutPanel_CustomerDetailCustomerCompanyInputNewCustomer.Size = New System.Drawing.Size(342, 56)
        FlowLayoutPanel_CustomerDetailCustomerCompanyInputNewCustomer.TabIndex = 20
        '
        'Label13
        '
        Label13.AutoSize = true
        Label13.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label13.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label13.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label13.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label13.Location = New System.Drawing.Point(0, 0)
        Label13.Margin = New System.Windows.Forms.Padding(0)
        Label13.Name = "Label13"
        Label13.Size = New System.Drawing.Size(62, 16)
        Label13.TabIndex = 2
        Label13.Text = "Company"
        Label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Textbox_CustomerDetailCustomerCompanyInputNewCustomer
        '
        Textbox_CustomerDetailCustomerCompanyInputNewCustomer.Anchor = System.Windows.Forms.AnchorStyles.Left
        Textbox_CustomerDetailCustomerCompanyInputNewCustomer.AutoCompleteCustomSource.AddRange(New String() {"(001) Item 1", "(002) Item 2", "(003) Item 3", "(004) Item 4", "(005) Item 5", "(006) Item 6", "(007) Item 7", "(008) Item 8", "(009) Item 9", "(010) Item 10"})
        Textbox_CustomerDetailCustomerCompanyInputNewCustomer.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Textbox_CustomerDetailCustomerCompanyInputNewCustomer.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Textbox_CustomerDetailCustomerCompanyInputNewCustomer.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailCustomerCompanyInputNewCustomer.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_CustomerDetailCustomerCompanyInputNewCustomer.Cursor = System.Windows.Forms.Cursors.IBeam
        Textbox_CustomerDetailCustomerCompanyInputNewCustomer.DefaultText = ""
        Textbox_CustomerDetailCustomerCompanyInputNewCustomer.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer))
        Textbox_CustomerDetailCustomerCompanyInputNewCustomer.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer))
        Textbox_CustomerDetailCustomerCompanyInputNewCustomer.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_CustomerDetailCustomerCompanyInputNewCustomer.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_CustomerDetailCustomerCompanyInputNewCustomer.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailCustomerCompanyInputNewCustomer.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailCustomerCompanyInputNewCustomer.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
        Textbox_CustomerDetailCustomerCompanyInputNewCustomer.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_CustomerDetailCustomerCompanyInputNewCustomer.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailCustomerCompanyInputNewCustomer.Location = New System.Drawing.Point(0, 16)
        Textbox_CustomerDetailCustomerCompanyInputNewCustomer.Margin = New System.Windows.Forms.Padding(0)
        Textbox_CustomerDetailCustomerCompanyInputNewCustomer.Name = "Textbox_CustomerDetailCustomerCompanyInputNewCustomer"
        Textbox_CustomerDetailCustomerCompanyInputNewCustomer.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Textbox_CustomerDetailCustomerCompanyInputNewCustomer.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer))
        Textbox_CustomerDetailCustomerCompanyInputNewCustomer.PlaceholderText = "Select a customer. Type ""("" to show all"
        Textbox_CustomerDetailCustomerCompanyInputNewCustomer.SelectedText = ""
        Textbox_CustomerDetailCustomerCompanyInputNewCustomer.Size = New System.Drawing.Size(342, 40)
        Textbox_CustomerDetailCustomerCompanyInputNewCustomer.TabIndex = 39

        FlowLayoutPanel_CustomerDetailCustomerEmailInputNewCustomer.AutoSize = true
        FlowLayoutPanel_CustomerDetailCustomerEmailInputNewCustomer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_CustomerDetailCustomerEmailInputNewCustomer.Controls.Add(Label14)
        FlowLayoutPanel_CustomerDetailCustomerEmailInputNewCustomer.Controls.Add(Textbox_CustomerDetailCustomerEmailInputNewCustomer)
        FlowLayoutPanel_CustomerDetailCustomerEmailInputNewCustomer.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_CustomerDetailCustomerEmailInputNewCustomer.Location = New System.Drawing.Point(12, 200)
        FlowLayoutPanel_CustomerDetailCustomerEmailInputNewCustomer.Margin = New System.Windows.Forms.Padding(12, 0, 6, 12)
        FlowLayoutPanel_CustomerDetailCustomerEmailInputNewCustomer.Name = "FlowLayoutPanel_CustomerDetailCustomerEmailInputNewCustomer"
        FlowLayoutPanel_CustomerDetailCustomerEmailInputNewCustomer.Size = New System.Drawing.Size(342, 56)
        FlowLayoutPanel_CustomerDetailCustomerEmailInputNewCustomer.TabIndex = 21
        '
        'Label14
        '
        Label14.AutoSize = true
        Label14.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label14.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label14.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label14.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label14.Location = New System.Drawing.Point(0, 0)
        Label14.Margin = New System.Windows.Forms.Padding(0)
        Label14.Name = "Label14"
        Label14.Size = New System.Drawing.Size(38, 16)
        Label14.TabIndex = 2
        Label14.Text = "Email"
        Label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Textbox_CustomerDetailCustomerEmailInputNewCustomer
        '
        Textbox_CustomerDetailCustomerEmailInputNewCustomer.Anchor = System.Windows.Forms.AnchorStyles.Left
        Textbox_CustomerDetailCustomerEmailInputNewCustomer.AutoCompleteCustomSource.AddRange(New String() {"(001) Item 1", "(002) Item 2", "(003) Item 3", "(004) Item 4", "(005) Item 5", "(006) Item 6", "(007) Item 7", "(008) Item 8", "(009) Item 9", "(010) Item 10"})
        Textbox_CustomerDetailCustomerEmailInputNewCustomer.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Textbox_CustomerDetailCustomerEmailInputNewCustomer.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Textbox_CustomerDetailCustomerEmailInputNewCustomer.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailCustomerEmailInputNewCustomer.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_CustomerDetailCustomerEmailInputNewCustomer.Cursor = System.Windows.Forms.Cursors.IBeam
        Textbox_CustomerDetailCustomerEmailInputNewCustomer.DefaultText = ""
        Textbox_CustomerDetailCustomerEmailInputNewCustomer.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer))
        Textbox_CustomerDetailCustomerEmailInputNewCustomer.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer))
        Textbox_CustomerDetailCustomerEmailInputNewCustomer.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_CustomerDetailCustomerEmailInputNewCustomer.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_CustomerDetailCustomerEmailInputNewCustomer.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailCustomerEmailInputNewCustomer.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailCustomerEmailInputNewCustomer.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
        Textbox_CustomerDetailCustomerEmailInputNewCustomer.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_CustomerDetailCustomerEmailInputNewCustomer.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailCustomerEmailInputNewCustomer.Location = New System.Drawing.Point(0, 16)
        Textbox_CustomerDetailCustomerEmailInputNewCustomer.Margin = New System.Windows.Forms.Padding(0)
        Textbox_CustomerDetailCustomerEmailInputNewCustomer.Name = "Textbox_CustomerDetailCustomerEmailInputNewCustomer"
        Textbox_CustomerDetailCustomerEmailInputNewCustomer.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Textbox_CustomerDetailCustomerEmailInputNewCustomer.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer))
        Textbox_CustomerDetailCustomerEmailInputNewCustomer.PlaceholderText = "Select a customer. Type ""("" to show all"
        Textbox_CustomerDetailCustomerEmailInputNewCustomer.SelectedText = ""
        Textbox_CustomerDetailCustomerEmailInputNewCustomer.Size = New System.Drawing.Size(342, 40)
        Textbox_CustomerDetailCustomerEmailInputNewCustomer.TabIndex = 39
        '
        'FlowLayoutPanel_CustomerDetailCustomerPhoneInputNewCustomer
        '
        FlowLayoutPanel_CustomerDetailCustomerPhoneInputNewCustomer.AutoSize = true
        FlowLayoutPanel_CustomerDetailCustomerPhoneInputNewCustomer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_CustomerDetailCustomerPhoneInputNewCustomer.Controls.Add(Label15)
        FlowLayoutPanel_CustomerDetailCustomerPhoneInputNewCustomer.Controls.Add(Textbox_CustomerDetailCustomerPhoneInputNewCustomer)
        FlowLayoutPanel_CustomerDetailCustomerPhoneInputNewCustomer.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_CustomerDetailCustomerPhoneInputNewCustomer.Location = New System.Drawing.Point(366, 200)
        FlowLayoutPanel_CustomerDetailCustomerPhoneInputNewCustomer.Margin = New System.Windows.Forms.Padding(6, 0, 12, 12)
        FlowLayoutPanel_CustomerDetailCustomerPhoneInputNewCustomer.Name = "FlowLayoutPanel_CustomerDetailCustomerPhoneInputNewCustomer"
        FlowLayoutPanel_CustomerDetailCustomerPhoneInputNewCustomer.Size = New System.Drawing.Size(342, 56)
        FlowLayoutPanel_CustomerDetailCustomerPhoneInputNewCustomer.TabIndex = 22
        '
        'Label15
        '
        Label15.AutoSize = true
        Label15.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label15.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label15.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label15.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label15.Location = New System.Drawing.Point(0, 0)
        Label15.Margin = New System.Windows.Forms.Padding(0)
        Label15.Name = "Label15"
        Label15.Size = New System.Drawing.Size(43, 16)
        Label15.TabIndex = 2
        Label15.Text = "Phone"
        Label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Textbox_CustomerDetailCustomerPhoneInputNewCustomer
        '
        Textbox_CustomerDetailCustomerPhoneInputNewCustomer.Anchor = System.Windows.Forms.AnchorStyles.Left
        Textbox_CustomerDetailCustomerPhoneInputNewCustomer.AutoCompleteCustomSource.AddRange(New String() {"(001) Item 1", "(002) Item 2", "(003) Item 3", "(004) Item 4", "(005) Item 5", "(006) Item 6", "(007) Item 7", "(008) Item 8", "(009) Item 9", "(010) Item 10"})
        Textbox_CustomerDetailCustomerPhoneInputNewCustomer.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Textbox_CustomerDetailCustomerPhoneInputNewCustomer.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Textbox_CustomerDetailCustomerPhoneInputNewCustomer.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailCustomerPhoneInputNewCustomer.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_CustomerDetailCustomerPhoneInputNewCustomer.Cursor = System.Windows.Forms.Cursors.IBeam
        Textbox_CustomerDetailCustomerPhoneInputNewCustomer.DefaultText = ""
        Textbox_CustomerDetailCustomerPhoneInputNewCustomer.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer))
        Textbox_CustomerDetailCustomerPhoneInputNewCustomer.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer))
        Textbox_CustomerDetailCustomerPhoneInputNewCustomer.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_CustomerDetailCustomerPhoneInputNewCustomer.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_CustomerDetailCustomerPhoneInputNewCustomer.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailCustomerPhoneInputNewCustomer.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailCustomerPhoneInputNewCustomer.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
        Textbox_CustomerDetailCustomerPhoneInputNewCustomer.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_CustomerDetailCustomerPhoneInputNewCustomer.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailCustomerPhoneInputNewCustomer.Location = New System.Drawing.Point(0, 16)
        Textbox_CustomerDetailCustomerPhoneInputNewCustomer.Margin = New System.Windows.Forms.Padding(0)
        Textbox_CustomerDetailCustomerPhoneInputNewCustomer.Name = "Textbox_CustomerDetailCustomerPhoneInputNewCustomer"
        Textbox_CustomerDetailCustomerPhoneInputNewCustomer.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Textbox_CustomerDetailCustomerPhoneInputNewCustomer.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer))
        Textbox_CustomerDetailCustomerPhoneInputNewCustomer.PlaceholderText = "Select a customer. Type ""("" to show all"
        Textbox_CustomerDetailCustomerPhoneInputNewCustomer.SelectedText = ""
        Textbox_CustomerDetailCustomerPhoneInputNewCustomer.Size = New System.Drawing.Size(342, 40)
        Textbox_CustomerDetailCustomerPhoneInputNewCustomer.TabIndex = 39
        '
        's
        '
        s.AutoSize = true
        s.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        s.Controls.Add(Label10)
        s.Controls.Add(Textbox_CustomerDetailCustomerAddressLineInputNewCustomer)
        s.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        s.Location = New System.Drawing.Point(12, 268)
        s.Margin = New System.Windows.Forms.Padding(12, 0, 6, 12)
        s.Name = "s"
        s.Size = New System.Drawing.Size(342, 56)
        s.TabIndex = 23
        '
        'Label10
        '
        Label10.AutoSize = true
        Label10.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label10.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label10.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label10.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label10.Location = New System.Drawing.Point(0, 0)
        Label10.Margin = New System.Windows.Forms.Padding(0)
        Label10.Name = "Label10"
        Label10.Size = New System.Drawing.Size(78, 16)
        Label10.TabIndex = 2
        Label10.Text = "Address Line"
        Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Textbox_CustomerDetailCustomerAddressLineInputNewCustomer
        '
        Textbox_CustomerDetailCustomerAddressLineInputNewCustomer.Anchor = System.Windows.Forms.AnchorStyles.Left
        Textbox_CustomerDetailCustomerAddressLineInputNewCustomer.AutoCompleteCustomSource.AddRange(New String() {"(001) Item 1", "(002) Item 2", "(003) Item 3", "(004) Item 4", "(005) Item 5", "(006) Item 6", "(007) Item 7", "(008) Item 8", "(009) Item 9", "(010) Item 10"})
        Textbox_CustomerDetailCustomerAddressLineInputNewCustomer.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Textbox_CustomerDetailCustomerAddressLineInputNewCustomer.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Textbox_CustomerDetailCustomerAddressLineInputNewCustomer.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailCustomerAddressLineInputNewCustomer.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_CustomerDetailCustomerAddressLineInputNewCustomer.Cursor = System.Windows.Forms.Cursors.IBeam
        Textbox_CustomerDetailCustomerAddressLineInputNewCustomer.DefaultText = ""
        Textbox_CustomerDetailCustomerAddressLineInputNewCustomer.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer))
        Textbox_CustomerDetailCustomerAddressLineInputNewCustomer.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer))
        Textbox_CustomerDetailCustomerAddressLineInputNewCustomer.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_CustomerDetailCustomerAddressLineInputNewCustomer.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_CustomerDetailCustomerAddressLineInputNewCustomer.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailCustomerAddressLineInputNewCustomer.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailCustomerAddressLineInputNewCustomer.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
        Textbox_CustomerDetailCustomerAddressLineInputNewCustomer.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_CustomerDetailCustomerAddressLineInputNewCustomer.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailCustomerAddressLineInputNewCustomer.Location = New System.Drawing.Point(0, 16)
        Textbox_CustomerDetailCustomerAddressLineInputNewCustomer.Margin = New System.Windows.Forms.Padding(0)
        Textbox_CustomerDetailCustomerAddressLineInputNewCustomer.Name = "Textbox_CustomerDetailCustomerAddressLineInputNewCustomer"
        Textbox_CustomerDetailCustomerAddressLineInputNewCustomer.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Textbox_CustomerDetailCustomerAddressLineInputNewCustomer.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer))
        Textbox_CustomerDetailCustomerAddressLineInputNewCustomer.PlaceholderText = "Select a customer. Type ""("" to show all"
        Textbox_CustomerDetailCustomerAddressLineInputNewCustomer.SelectedText = ""
        Textbox_CustomerDetailCustomerAddressLineInputNewCustomer.Size = New System.Drawing.Size(342, 40)
        Textbox_CustomerDetailCustomerAddressLineInputNewCustomer.TabIndex = 39
        '
        'FlowLayoutPanel_CustomerDetailCustomerCityInputNewCustomer
        '
        FlowLayoutPanel_CustomerDetailCustomerCityInputNewCustomer.AutoSize = true
        FlowLayoutPanel_CustomerDetailCustomerCityInputNewCustomer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_CustomerDetailCustomerCityInputNewCustomer.Controls.Add(Label11)
        FlowLayoutPanel_CustomerDetailCustomerCityInputNewCustomer.Controls.Add(Textbox_CustomerDetailCustomerCityInputNewCustomer)
        FlowLayoutPanel_CustomerDetailCustomerCityInputNewCustomer.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_CustomerDetailCustomerCityInputNewCustomer.Location = New System.Drawing.Point(366, 268)
        FlowLayoutPanel_CustomerDetailCustomerCityInputNewCustomer.Margin = New System.Windows.Forms.Padding(6, 0, 12, 12)
        FlowLayoutPanel_CustomerDetailCustomerCityInputNewCustomer.Name = "FlowLayoutPanel_CustomerDetailCustomerCityInputNewCustomer"
        FlowLayoutPanel_CustomerDetailCustomerCityInputNewCustomer.Size = New System.Drawing.Size(342, 56)
        FlowLayoutPanel_CustomerDetailCustomerCityInputNewCustomer.TabIndex = 24
        '
        'Label11
        '
        Label11.AutoSize = true
        Label11.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label11.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label11.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label11.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label11.Location = New System.Drawing.Point(0, 0)
        Label11.Margin = New System.Windows.Forms.Padding(0)
        Label11.Name = "Label11"
        Label11.Size = New System.Drawing.Size(28, 16)
        Label11.TabIndex = 2
        Label11.Text = "City"
        Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Textbox_CustomerDetailCustomerCityInputNewCustomer
        '
        Textbox_CustomerDetailCustomerCityInputNewCustomer.Anchor = System.Windows.Forms.AnchorStyles.Left
        Textbox_CustomerDetailCustomerCityInputNewCustomer.AutoCompleteCustomSource.AddRange(New String() {"(001) Item 1", "(002) Item 2", "(003) Item 3", "(004) Item 4", "(005) Item 5", "(006) Item 6", "(007) Item 7", "(008) Item 8", "(009) Item 9", "(010) Item 10"})
        Textbox_CustomerDetailCustomerCityInputNewCustomer.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Textbox_CustomerDetailCustomerCityInputNewCustomer.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Textbox_CustomerDetailCustomerCityInputNewCustomer.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailCustomerCityInputNewCustomer.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_CustomerDetailCustomerCityInputNewCustomer.Cursor = System.Windows.Forms.Cursors.IBeam
        Textbox_CustomerDetailCustomerCityInputNewCustomer.DefaultText = ""
        Textbox_CustomerDetailCustomerCityInputNewCustomer.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer))
        Textbox_CustomerDetailCustomerCityInputNewCustomer.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer))
        Textbox_CustomerDetailCustomerCityInputNewCustomer.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_CustomerDetailCustomerCityInputNewCustomer.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_CustomerDetailCustomerCityInputNewCustomer.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailCustomerCityInputNewCustomer.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailCustomerCityInputNewCustomer.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
        Textbox_CustomerDetailCustomerCityInputNewCustomer.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_CustomerDetailCustomerCityInputNewCustomer.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailCustomerCityInputNewCustomer.Location = New System.Drawing.Point(0, 16)
        Textbox_CustomerDetailCustomerCityInputNewCustomer.Margin = New System.Windows.Forms.Padding(0)
        Textbox_CustomerDetailCustomerCityInputNewCustomer.Name = "Textbox_CustomerDetailCustomerCityInputNewCustomer"
        Textbox_CustomerDetailCustomerCityInputNewCustomer.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Textbox_CustomerDetailCustomerCityInputNewCustomer.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer))
        Textbox_CustomerDetailCustomerCityInputNewCustomer.PlaceholderText = "Select a customer. Type ""("" to show all"
        Textbox_CustomerDetailCustomerCityInputNewCustomer.SelectedText = ""
        Textbox_CustomerDetailCustomerCityInputNewCustomer.Size = New System.Drawing.Size(342, 40)
        Textbox_CustomerDetailCustomerCityInputNewCustomer.TabIndex = 39
        '
        'FlowLayoutPanel_CustomerDetailCustomerZipCodeInputNewCustomer
        '
        FlowLayoutPanel_CustomerDetailCustomerZipCodeInputNewCustomer.AutoSize = true
        FlowLayoutPanel_CustomerDetailCustomerZipCodeInputNewCustomer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_CustomerDetailCustomerZipCodeInputNewCustomer.Controls.Add(Label17)
        FlowLayoutPanel_CustomerDetailCustomerZipCodeInputNewCustomer.Controls.Add(Textbox_CustomerDetailCustomerZipCodeInputNewCustomer)
        FlowLayoutPanel_CustomerDetailCustomerZipCodeInputNewCustomer.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_CustomerDetailCustomerZipCodeInputNewCustomer.Location = New System.Drawing.Point(12, 336)
        FlowLayoutPanel_CustomerDetailCustomerZipCodeInputNewCustomer.Margin = New System.Windows.Forms.Padding(12, 0, 6, 12)
        FlowLayoutPanel_CustomerDetailCustomerZipCodeInputNewCustomer.Name = "FlowLayoutPanel_CustomerDetailCustomerZipCodeInputNewCustomer"
        FlowLayoutPanel_CustomerDetailCustomerZipCodeInputNewCustomer.Size = New System.Drawing.Size(342, 56)
        FlowLayoutPanel_CustomerDetailCustomerZipCodeInputNewCustomer.TabIndex = 25
        '
        'Label17
        '
        Label17.AutoSize = true
        Label17.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label17.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label17.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label17.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label17.Location = New System.Drawing.Point(0, 0)
        Label17.Margin = New System.Windows.Forms.Padding(0)
        Label17.Name = "Label17"
        Label17.Size = New System.Drawing.Size(59, 16)
        Label17.TabIndex = 2
        Label17.Text = "Zip Code"
        Label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Textbox_CustomerDetailCustomerZipCodeInputNewCustomer
        '
        Textbox_CustomerDetailCustomerZipCodeInputNewCustomer.Anchor = System.Windows.Forms.AnchorStyles.Left
        Textbox_CustomerDetailCustomerZipCodeInputNewCustomer.AutoCompleteCustomSource.AddRange(New String() {"(001) Item 1", "(002) Item 2", "(003) Item 3", "(004) Item 4", "(005) Item 5", "(006) Item 6", "(007) Item 7", "(008) Item 8", "(009) Item 9", "(010) Item 10"})
        Textbox_CustomerDetailCustomerZipCodeInputNewCustomer.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Textbox_CustomerDetailCustomerZipCodeInputNewCustomer.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Textbox_CustomerDetailCustomerZipCodeInputNewCustomer.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailCustomerZipCodeInputNewCustomer.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_CustomerDetailCustomerZipCodeInputNewCustomer.Cursor = System.Windows.Forms.Cursors.IBeam
        Textbox_CustomerDetailCustomerZipCodeInputNewCustomer.DefaultText = ""
        Textbox_CustomerDetailCustomerZipCodeInputNewCustomer.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer))
        Textbox_CustomerDetailCustomerZipCodeInputNewCustomer.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer))
        Textbox_CustomerDetailCustomerZipCodeInputNewCustomer.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_CustomerDetailCustomerZipCodeInputNewCustomer.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_CustomerDetailCustomerZipCodeInputNewCustomer.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailCustomerZipCodeInputNewCustomer.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailCustomerZipCodeInputNewCustomer.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
        Textbox_CustomerDetailCustomerZipCodeInputNewCustomer.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_CustomerDetailCustomerZipCodeInputNewCustomer.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailCustomerZipCodeInputNewCustomer.Location = New System.Drawing.Point(0, 16)
        Textbox_CustomerDetailCustomerZipCodeInputNewCustomer.Margin = New System.Windows.Forms.Padding(0)
        Textbox_CustomerDetailCustomerZipCodeInputNewCustomer.Name = "Textbox_CustomerDetailCustomerZipCodeInputNewCustomer"
        Textbox_CustomerDetailCustomerZipCodeInputNewCustomer.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Textbox_CustomerDetailCustomerZipCodeInputNewCustomer.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer))
        Textbox_CustomerDetailCustomerZipCodeInputNewCustomer.PlaceholderText = "Select a customer. Type ""("" to show all"
        Textbox_CustomerDetailCustomerZipCodeInputNewCustomer.SelectedText = ""
        Textbox_CustomerDetailCustomerZipCodeInputNewCustomer.Size = New System.Drawing.Size(342, 40)
        Textbox_CustomerDetailCustomerZipCodeInputNewCustomer.TabIndex = 39
        '
        'FlowLayoutPanel_CustomerDetailCustomerCountryInputNewCustomer
        '
        FlowLayoutPanel_CustomerDetailCustomerCountryInputNewCustomer.AutoSize = true
        FlowLayoutPanel_CustomerDetailCustomerCountryInputNewCustomer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_CustomerDetailCustomerCountryInputNewCustomer.Controls.Add(Label18)
        FlowLayoutPanel_CustomerDetailCustomerCountryInputNewCustomer.Controls.Add(Textbox_CustomerDetailCustomerCountryInputNewCustomer)
        FlowLayoutPanel_CustomerDetailCustomerCountryInputNewCustomer.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_CustomerDetailCustomerCountryInputNewCustomer.Location = New System.Drawing.Point(366, 336)
        FlowLayoutPanel_CustomerDetailCustomerCountryInputNewCustomer.Margin = New System.Windows.Forms.Padding(6, 0, 12, 12)
        FlowLayoutPanel_CustomerDetailCustomerCountryInputNewCustomer.Name = "FlowLayoutPanel_CustomerDetailCustomerCountryInputNewCustomer"
        FlowLayoutPanel_CustomerDetailCustomerCountryInputNewCustomer.Size = New System.Drawing.Size(342, 56)
        FlowLayoutPanel_CustomerDetailCustomerCountryInputNewCustomer.TabIndex = 26
        '
        'Label18
        '
        Label18.AutoSize = true
        Label18.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label18.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label18.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label18.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label18.Location = New System.Drawing.Point(0, 0)
        Label18.Margin = New System.Windows.Forms.Padding(0)
        Label18.Name = "Label18"
        Label18.Size = New System.Drawing.Size(51, 16)
        Label18.TabIndex = 2
        Label18.Text = "Country"
        Label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Textbox_CustomerDetailCustomerCountryInputNewCustomer
        '
        Textbox_CustomerDetailCustomerCountryInputNewCustomer.Anchor = System.Windows.Forms.AnchorStyles.Left
        Textbox_CustomerDetailCustomerCountryInputNewCustomer.AutoCompleteCustomSource.AddRange(New String() {"(001) Item 1", "(002) Item 2", "(003) Item 3", "(004) Item 4", "(005) Item 5", "(006) Item 6", "(007) Item 7", "(008) Item 8", "(009) Item 9", "(010) Item 10"})
        Textbox_CustomerDetailCustomerCountryInputNewCustomer.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Textbox_CustomerDetailCustomerCountryInputNewCustomer.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Textbox_CustomerDetailCustomerCountryInputNewCustomer.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailCustomerCountryInputNewCustomer.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_CustomerDetailCustomerCountryInputNewCustomer.Cursor = System.Windows.Forms.Cursors.IBeam
        Textbox_CustomerDetailCustomerCountryInputNewCustomer.DefaultText = ""
        Textbox_CustomerDetailCustomerCountryInputNewCustomer.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer), CType(CType(208,Byte),Integer))
        Textbox_CustomerDetailCustomerCountryInputNewCustomer.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer), CType(CType(226,Byte),Integer))
        Textbox_CustomerDetailCustomerCountryInputNewCustomer.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_CustomerDetailCustomerCountryInputNewCustomer.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer), CType(CType(138,Byte),Integer))
        Textbox_CustomerDetailCustomerCountryInputNewCustomer.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailCustomerCountryInputNewCustomer.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailCustomerCountryInputNewCustomer.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
        Textbox_CustomerDetailCustomerCountryInputNewCustomer.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Textbox_CustomerDetailCustomerCountryInputNewCustomer.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94,Byte),Integer), CType(CType(148,Byte),Integer), CType(CType(255,Byte),Integer))
        Textbox_CustomerDetailCustomerCountryInputNewCustomer.Location = New System.Drawing.Point(0, 16)
        Textbox_CustomerDetailCustomerCountryInputNewCustomer.Margin = New System.Windows.Forms.Padding(0)
        Textbox_CustomerDetailCustomerCountryInputNewCustomer.Name = "Textbox_CustomerDetailCustomerCountryInputNewCustomer"
        Textbox_CustomerDetailCustomerCountryInputNewCustomer.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Textbox_CustomerDetailCustomerCountryInputNewCustomer.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer), CType(CType(153,Byte),Integer))
        Textbox_CustomerDetailCustomerCountryInputNewCustomer.PlaceholderText = "Select a customer. Type ""("" to show all"
        Textbox_CustomerDetailCustomerCountryInputNewCustomer.SelectedText = ""
        Textbox_CustomerDetailCustomerCountryInputNewCustomer.Size = New System.Drawing.Size(342, 40)
        Textbox_CustomerDetailCustomerCountryInputNewCustomer.TabIndex = 39
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
        FlowLayoutPanel_PageHeaderNewCustomer.Size = New System.Drawing.Size(233, 32)
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
        Label_PageHeaderNewCustomer.Size = New System.Drawing.Size(185, 31)
        Label_PageHeaderNewCustomer.TabIndex = 2
        Label_PageHeaderNewCustomer.Text = "New Customer"
        Label_PageHeaderNewCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Button_MakeCustomerNewSale
        '
        Button_MakeCustomerNewSale.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(44,Byte),Integer), CType(CType(62,Byte),Integer))
        Button_MakeCustomerNewSale.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Button_MakeCustomerNewSale.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Button_MakeCustomerNewSale.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer))
        Button_MakeCustomerNewSale.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer))
        Button_MakeCustomerNewSale.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Button_MakeCustomerNewSale.Font = New System.Drawing.Font("Segoe UI", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel)
        Button_MakeCustomerNewSale.ForeColor = System.Drawing.Color.White
        Button_MakeCustomerNewSale.Location = New System.Drawing.Point(366, 404)
        Button_MakeCustomerNewSale.Margin = New System.Windows.Forms.Padding(6, 0, 12, 12)
        Button_MakeCustomerNewSale.Name = "Button_MakeCustomerNewSale"
        Button_MakeCustomerNewSale.Size = New System.Drawing.Size(342, 40)
        Button_MakeCustomerNewSale.TabIndex = 33
        Button_MakeCustomerNewSale.Text = "Make Customer"
        Button_MakeCustomerNewSale.TextOffset = New System.Drawing.Point(0, -2)
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
        Button_ResetNewCustomer.Location = New System.Drawing.Point(12, 404)
        Button_ResetNewCustomer.Margin = New System.Windows.Forms.Padding(12, 0, 6, 12)
        Button_ResetNewCustomer.Name = "Button_ResetNewCustomer"
        Button_ResetNewCustomer.Size = New System.Drawing.Size(342, 40)
        Button_ResetNewCustomer.TabIndex = 34
        Button_ResetNewCustomer.Text = "Reset"
        Button_ResetNewCustomer.TextOffset = New System.Drawing.Point(0, -2)
        '
        'Prompt_NewCustomer
        '
        Prompt_NewCustomer.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Prompt_NewCustomer.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Prompt_NewCustomer.ClientSize = New System.Drawing.Size(720, 720)
        Prompt_NewCustomer.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Prompt_NewCustomer.Controls.Add(FlowLayoutPanel_NewCustomer)
        Prompt_NewCustomer.Name = "Prompt_NewCustomer"
        Prompt_NewCustomer.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Prompt_NewCustomer.Location = New Point(Me.Location.X + 24, Me.Location.Y + 24)

        FlowLayoutPanel_NewCustomer.ResumeLayout(false)
        FlowLayoutPanel_NewCustomer.PerformLayout
        FlowLayoutPanel_CustomerDetailCustomerNameInputNewCustomer.ResumeLayout(false)
        FlowLayoutPanel_CustomerDetailCustomerNameInputNewCustomer.PerformLayout
        FlowLayoutPanel_CustomerDetailCustomerCompanyInputNewCustomer.ResumeLayout(false)
        FlowLayoutPanel_CustomerDetailCustomerCompanyInputNewCustomer.PerformLayout
        FlowLayoutPanel_CustomerDetailCustomerEmailInputNewCustomer.ResumeLayout(false)
        FlowLayoutPanel_CustomerDetailCustomerEmailInputNewCustomer.PerformLayout
        FlowLayoutPanel_CustomerDetailCustomerPhoneInputNewCustomer.ResumeLayout(false)
        FlowLayoutPanel_CustomerDetailCustomerPhoneInputNewCustomer.PerformLayout
        s.ResumeLayout(false)
        s.PerformLayout
        FlowLayoutPanel_CustomerDetailCustomerCityInputNewCustomer.ResumeLayout(false)
        FlowLayoutPanel_CustomerDetailCustomerCityInputNewCustomer.PerformLayout
        FlowLayoutPanel_CustomerDetailCustomerZipCodeInputNewCustomer.ResumeLayout(false)
        FlowLayoutPanel_CustomerDetailCustomerZipCodeInputNewCustomer.PerformLayout
        FlowLayoutPanel_CustomerDetailCustomerCountryInputNewCustomer.ResumeLayout(false)
        FlowLayoutPanel_CustomerDetailCustomerCountryInputNewCustomer.PerformLayout
        FlowLayoutPanel_PageHeaderNewCustomer.ResumeLayout(false)
        FlowLayoutPanel_PageHeaderNewCustomer.PerformLayout
        ResumeLayout(false)

        Prompt_NewCustomer.ShowDialog(Me)
    End Sub


    '' Control Methods
    
    Private Sub SaleScreenDraft_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PopulateCategoriesSelection()
        PopulateItemsGrid(CurrentCategoryID)
    End Sub

    Private Sub Button_ToPayNewSale_Click(sender As Object, e As EventArgs) Handles Button_ToPayNewSale.Click
        ShowPlaceOrderPrompt()
    End Sub

    Private Sub Textbox_VATPercentageSaleScreen_TextChanged(sender As Object, e As EventArgs) Handles Textbox_VATPercentageSaleScreen.TextChanged
        Dim subtotal As Decimal = 0D

        For Each Item In CurrentTransactionItems
            subtotal += Item.transaction_item_amount
        Next

        CurrentTransactionEntry.subtotal = subtotal
        Label_SubtotalSaleScreen.Text = subtotal

        Dim vat_amount As Decimal = 0D
        Dim vat_percentage As Decimal = Convert.ToDecimal(Textbox_VATPercentageSaleScreen.Text) / 100D

        For Each Item In CurrentTransactionItems
            vat_amount += (Item.transaction_item_selling_price * Item.transaction_item_quantity) * vat_percentage
        Next

        CurrentTransactionEntry.net_total = subtotal + vat_amount
        CurrentTransactionEntry.tax_percentage = Textbox_VATPercentageSaleScreen.Text
        CurrentTransactionEntry.tax_amount = vat_amount

        Label_VATAmountSaleScreen.Text = vat_amount
        Label_NetTotalSaleScreen.Text = CurrentTransactionEntry.net_total
    End Sub

    Private Sub Textbox_SearchBarBrands_TextChanged(sender As Object, e As EventArgs) Handles Textbox_SearchBarBrands.TextChanged
        SearchAndPopulateItemsGrid(CurrentCategoryID, Textbox_SearchBarBrands.Text)
    End Sub

    Private Sub Button_ResetSaleScreeen_Click(sender As Object, e As EventArgs) Handles Button_ResetSaleScreeen.Click
        CurrentTransactionItems.Clear()
        CurrentSelectedTransactionItemIDs.Clear()
        CurrentTransactionEntry = New _TransactionEntry()

        CurrentPage = 1
        CurrentCategoryPage = 1
        CurrentCategoryID = -1
        TransactionItemsTableCurrentPage = 1
        TransactionItemsTableTotalPagess = 1

        Dim subtotal As Decimal = 0D

        For Each Item In CurrentTransactionItems
            subtotal += Item.transaction_item_amount
        Next

        CurrentTransactionEntry.subtotal = subtotal
        Label_SubtotalSaleScreen.Text = subtotal

        Dim vat_amount As Decimal = 0D
        Dim vat_percentage As Decimal = Convert.ToDecimal(Textbox_VATPercentageSaleScreen.Text) / 100D


        For Each Item In CurrentTransactionItems
            vat_amount += (Item.transaction_item_selling_price * Item.transaction_item_quantity) * vat_percentage
        Next

        CurrentTransactionEntry.net_total = subtotal + vat_amount
        CurrentTransactionEntry.tax_percentage = Textbox_VATPercentageSaleScreen.Text
        CurrentTransactionEntry.tax_amount = vat_amount

        Label_VATAmountSaleScreen.Text = vat_amount
        Label_NetTotalSaleScreen.Text = CurrentTransactionEntry.net_total

        PopulateItemsGrid(CurrentCategoryID)
        PopulateCategoriesSelection()
        UpdateTransactionItemsTable()
    End Sub


End Class