Imports System.Runtime.InteropServices.ComTypes
Imports iText.Kernel.Geom
Imports System.Drawing.Point
Imports MySql.Data.MySqlClient
Imports Guna.UI2.WinForms
Imports System.Text
Imports System.Text.RegularExpressions


' filter yung gagawin

Public Class Window_Sales
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
    Public PaymentTerms As PaymentTerm() = CreatePaymentTermArray()
    Public Taxes As Tax() = CreateTaxArray()
    Public Customers As Customer() = CreateCustomerArray()
    Public Brands As Brand() = CreateBrandArray()
    Public Categories As Category() = CreateCategoryArray()
    Public Items As Item() = CreateItemArray()
    Public Accounts As Account() = CreateAccountArray()
    Public TransactionEntries As TransactionEntry() = CreateTransactionEntryArray()
    Public TransactionItems As TransactionItem() = CreateTransactionItemArray()
    Public Employees As Employee() = CreateEmployeeArray()
    



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
    


    '' Filter Values
    Private FilterTransactionsListAndSummaryStartDate = DateTime.MinValue
    Private FilterTransactionsListAndSummaryEndDate = DateTime.MaxValue
    Private FilterTransactionsListAndSummaryStatus = "All Transactions"
    Dim FilterTextDate = "All Time"
    Private FilteredTransactions As List(Of TransactionEntry)

    Public Sub FilterTransactionsByDateAndStatus(startDate As DateTime, endDate As DateTime, status As String)
        FilteredTransactions = TransactionEntries.Where(Function(te) te.CreatedAt.Date >= startDate.Date AndAlso te.CreatedAt.Date <= endDate.Date AndAlso (status = "All Transactions" OrElse te.Status = status)).ToList()
        GenerateSalesSummary()
        UpdateSalesListTable()
    End Sub

    

    Private TransactionsListItemsPerPage As Integer = 10
    Private TransactionsListCurrentPage As Integer = 1
    Private TransactionsListTotalPages As Integer
    Public SalesListCurrentPage As Integer = 1
    Public SalesListItemsPerPage As Integer = 10
    Public SalesListTotalPages As Integer
    



    '' Update sale summary
    Public Sub GenerateSalesSummary()
        If FilteredTransactions Is Nothing OrElse TransactionItems Is Nothing OrElse Items Is Nothing Then
            MessageBox.Show("Data sources are not initialized.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim totalNetValue As Decimal = FilteredTransactions.Sum(Function(te) te.NetTotal)
        Dim totalTransactions As Integer = FilteredTransactions.Count
        Dim totalItemsSold As Integer = TransactionItems.Sum(Function(ti) ti.TransactionItemQuantity)
        Dim totalUnpaidAmount As Decimal = FilteredTransactions.
            Where(Function(te) te.Status = "Unpaid").
            Sum(Function(te) te.DueAmount)
        Dim averageTransactionValue As Decimal = If(totalTransactions > 0, totalNetValue / totalTransactions, 0)

        Dim filteredTransactionIds = FilteredTransactions.Select(Function(te) te.TransactionId).ToList()

        Dim filteredTransactionItems = TransactionItems.
            Where(Function(ti) filteredTransactionIds.Contains(ti.TransactionId)).ToList()

        Dim itemSales = Items.
            GroupJoin(
                filteredTransactionItems,
                Function(item) item.ItemId,
                Function(ti) ti.ItemId,
                Function(item, tiGroup) New With {
                    .ItemId = item.ItemId,
                    .ItemName = item.ItemName,
                    .TotalQuantity = tiGroup.Sum(Function(ti) ti.TransactionItemQuantity)
                }).ToList()

        Dim mostSoldItem = itemSales.
            OrderByDescending(Function(i) i.TotalQuantity).
            FirstOrDefault()
        Dim leastSoldItem = itemSales.
            OrderBy(Function(i) i.TotalQuantity).
            FirstOrDefault()

        Label_NetTotalValueSalesSummary.Text = String.Format("{0:C2}", totalNetValue)
        Label_TotalTransactionsValueSalesSummary.Text = totalTransactions.ToString()
        Label_TotalItemsSoldValueSalesSummary.Text = totalItemsSold.ToString()
        Label_TotalUnpaidAmountValueSalesSummary.Text = String.Format("{0:C2}", totalUnpaidAmount)
        Label_ATVValueSalesSummary.Text = String.Format("{0:C2}", averageTransactionValue)
        Label_MostSoldItemValueSalesSummary.Text = String.Format("{0} ({1} sold)",
                                                                 If(mostSoldItem IsNot Nothing, mostSoldItem.ItemName, "N/A"),
                                                                 If(mostSoldItem IsNot Nothing, mostSoldItem.TotalQuantity, 0))
        Label_LeastSoldItemValueSalesSummary.Text = String.Format("{0} ({1} sold)",
                                                                  If(leastSoldItem IsNot Nothing, leastSoldItem.ItemName, "N/A"),
                                                                  If(leastSoldItem IsNot Nothing, leastSoldItem.TotalQuantity, 0))
    End Sub



    '' Populate sales list table
    Public Sub UpdateSalesListTable()
        FlowLayoutPanel_SalesListTable.Controls.Clear()

        SalesListTotalPages = Math.Ceiling(FilteredTransactions.Count / SalesListItemsPerPage)
        Label_SalesListTablePageControlPageNumber.Text = $"{SalesListCurrentPage}/{SalesListTotalPages}"

        Dim TableLayoutPanel_SalesList As New TableLayoutPanel With {
            .AutoSize = True,
            .AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink,
            .BackColor = System.Drawing.Color.FromArgb(CType(255, Byte), CType(255, Byte), CType(255, Byte)),
            .CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single],
            .Margin = New System.Windows.Forms.Padding(0, 0, 0, 0),
            .MaximumSize = New System.Drawing.Size(1102, 0),
            .MinimumSize = New System.Drawing.Size(1102, 0)
        }

        TableLayoutPanel_SalesList.ColumnCount = 9
        For i = 1 To 8
            TableLayoutPanel_SalesList.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11!))
        Next

        Dim StartIndex As Integer = (SalesListCurrentPage - 1) * SalesListItemsPerPage
        Dim EndIndex As Integer = Math.Min(StartIndex + SalesListItemsPerPage, FilteredTransactions.Count)

        For i = StartIndex To EndIndex - 1
            Dim transaction As TransactionEntry = FilteredTransactions(i)
            TableLayoutPanel_SalesList.RowCount += 1

            Dim TransactionCustomer As New Customer
            Dim TransactionAccount As New Employee

            For Each customer In Customers
                If customer.CustomerId = transaction.CustomerId Then
                    TransactionCustomer = customer
                End If
            Next
        
            For Each account In Employees
                If account.EmployeeId = transaction.CreatedBy Then
                    TransactionAccount = account
                End If
            Next

            Dim labels As New List(Of Label)
            labels.Add(CreateLabel(transaction.TransactionId.ToString(), 2, True))
            labels.Add(CreateLabel(transaction.CustomerId & " - " & TransactionCustomer.Name, 2))
            labels.Add(CreateLabel(transaction.TaxAmount, 1))
            labels.Add(CreateLabel(transaction.NetTotal, 1))
            labels.Add(CreateLabel(transaction.DueAmount, 1))
            labels.Add(CreateLabel(transaction.DueDate, 1))
            labels.Add(CreateLabel(transaction.Status, 1))
            labels.Add(CreateLabel(transaction.CreatedAt, 1))
            labels.Add(CreateLabel(transaction.CreatedBy & " - " & TransactionAccount.FirstName & " " & TransactionAccount.MiddleName & " " & TransactionAccount.LastName, 1))

            AddHandler labels(0).Click,
            Sub()
                ShowOutvoiceReceiptPrompt(transaction.TransactionId)
            End Sub

            For col As Integer = 0 To labels.Count - 1
                TableLayoutPanel_SalesList.Controls.Add(labels(col), col, TableLayoutPanel_SalesList.RowCount - 1)
            Next

            Dim maxLabelHeight As Integer = labels.Max(Function(lbl) lbl.Height)

            TableLayoutPanel_SalesList.RowStyles.Add(New RowStyle(SizeType.Absolute, maxLabelHeight + 12))
        Next

        FlowLayoutPanel_SalesListTable.Controls.Add(TableLayoutPanel_SalesList)
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

    Public CommissionsListCurrentPage As Integer = 1
    Public CommissionsListItemsPerPage As Integer = 10
    Public CommissionsListTotalPages As Integer
    
    Public Sub UpdateCommissionsListTable()
        FlowLayoutPanel_CommissionsListTable.Controls.Clear()
        
        CommissionsListTotalPages = Math.Ceiling(Customers.Count / CommissionsListItemsPerPage)
        Label_CommissionsListTablePageControlPageNumber.Text = $"{CommissionsListCurrentPage}/{CommissionsListTotalPages}"

        Dim TableLayoutPanel_CustomersList As New TableLayoutPanel With {
            .AutoSize = True,
            .AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink,
            .BackColor = System.Drawing.Color.FromArgb(CType(255, Byte), CType(255, Byte), CType(255, Byte)),
            .CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single],
            .Margin = New System.Windows.Forms.Padding(0, 0, 0, 0),
            .MaximumSize = New System.Drawing.Size(1102, 0),
            .MinimumSize = New System.Drawing.Size(1102, 0)
        }

        TableLayoutPanel_CustomersList.ColumnCount = 3
        For i = 1 To 3
            TableLayoutPanel_CustomersList.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100 / 3!))
        Next

        Dim StartIndex As Integer = (CommissionsListCurrentPage - 1) * CommissionsListItemsPerPage
        Dim EndIndex As Integer = Math.Min(StartIndex + CommissionsListItemsPerPage, Employees.Count)

        For i = StartIndex To EndIndex - 1
            Dim employee As Employee = Employees(i)
            TableLayoutPanel_CustomersList.RowCount += 1

            Dim Transactionemployee As New employee
            Dim TransactionAccount As New Account
            Dim TransactionPaymentTerm As New PaymentTerm

            ' Directly get the transaction data for this employee
            Dim netTotal As Decimal = employee.Commissions * (1 + 0.005D)

            ' Create the labels for the employee row
            Dim labels As New List(Of Label)
            labels.Add(CreateLabel(employee.EmployeeId.ToString() & " - " & employee.FirstName & " " & employee.MiddleName & " " & employee.LastName, 2, True))
            labels.Add(CreateLabel("P" & netTotal, 2, True))
            labels.Add(CreateLabel(employee.Commissions.ToString(), 2, True))

            For col As Integer = 0 To labels.Count - 1
                TableLayoutPanel_CustomersList.Controls.Add(labels(col), col, TableLayoutPanel_CustomersList.RowCount - 1)
            Next

            Dim maxLabelHeight As Integer = labels.Max(Function(lbl) lbl.Height)

            TableLayoutPanel_CustomersList.RowStyles.Add(New RowStyle(SizeType.Absolute, maxLabelHeight + 12))
        Next

        FlowLayoutPanel_CommissionsListTable.Controls.Add(TableLayoutPanel_CustomersList)
    End Sub


    '' Methods to show windows
    Function ShowSalesListAndSummaryFilterPrompt()
        Dim Prompt_SalesListAndSummaryFilter As New Form()
        Dim FlowLayoutPanel_CustomerDetailsNewCustomer = New System.Windows.Forms.FlowLayoutPanel()
        Dim FlowLayoutPanel_PageHeaderNewCustomer = New System.Windows.Forms.FlowLayoutPanel()
        Dim Panel_PageHeaderIconNewCustomer = New System.Windows.Forms.Panel()
        Dim Label_PageHeaderNewCustomer = New System.Windows.Forms.Label()
        Dim Label_CustomerDetailsNewCustomer = New System.Windows.Forms.Label()
        Dim FlowLayoutPanel_StartDateInputSales = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label_StartDateInputSales = New System.Windows.Forms.Label()
        Dim DateTimePicker_StartDateSalesListInputAndSummaryFilter = New Guna.UI2.WinForms.Guna2DateTimePicker()
        Dim FlowLayoutPanel_EndDateInputSales = New System.Windows.Forms.FlowLayoutPanel()
        Dim Label_EndDateSales = New System.Windows.Forms.Label()
        Dim DateTimePicker_EndDateInputSalesListAndSummaryFilter = New Guna.UI2.WinForms.Guna2DateTimePicker()
        Dim Button_TodaySalesListAndSummaryFilter = New Guna.UI2.WinForms.Guna2Button()
        Dim Button_WeeklySalesListAndSummaryFilter = New Guna.UI2.WinForms.Guna2Button()
        Dim Button_MonthlySalesListAndSummaryFilter = New Guna.UI2.WinForms.Guna2Button()
        Dim Button_QuarterlySalesListAndSummaryFilter = New Guna.UI2.WinForms.Guna2Button()
        Dim Button_AnnuallySalesListAndSummaryFilter = New Guna.UI2.WinForms.Guna2Button()
        Dim Label1 = New System.Windows.Forms.Label()
        Dim Button_AllStatusSalesListAndSummaryFilter = New Guna.UI2.WinForms.Guna2Button()
        Dim Button_CompletedSalesListAndSummaryFilter = New Guna.UI2.WinForms.Guna2Button()
        Dim Button_PendingApprovalSalesListAndSummaryFilter = New Guna.UI2.WinForms.Guna2Button()
        Dim Button_OpenSalesListAndSummaryFilter = New Guna.UI2.WinForms.Guna2Button()
        Dim Button_VoidedSalesListAndSummaryFilter = New Guna.UI2.WinForms.Guna2Button()
        Dim Button_OverdueSalesListAndSummaryFilter = New Guna.UI2.WinForms.Guna2Button()
        Dim FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Dim Button_ResetSalesListAndSummaryFilter = New Guna.UI2.WinForms.Guna2Button()
        Dim Button_ApplyFiltersSalesListAndSummaryFilter = New Guna.UI2.WinForms.Guna2Button()
        Dim Button_AllTimeSalesListAndSummaryFilter = New Guna.UI2.WinForms.Guna2Button()
        FlowLayoutPanel_CustomerDetailsNewCustomer.SuspendLayout
        FlowLayoutPanel_PageHeaderNewCustomer.SuspendLayout
        FlowLayoutPanel_StartDateInputSales.SuspendLayout
        FlowLayoutPanel_EndDateInputSales.SuspendLayout
        SuspendLayout
        '
        'FlowLayoutPanel_CustomerDetailsNewCustomer
        '
        FlowLayoutPanel_CustomerDetailsNewCustomer.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        FlowLayoutPanel_CustomerDetailsNewCustomer.Controls.Add(FlowLayoutPanel_PageHeaderNewCustomer)
        FlowLayoutPanel_CustomerDetailsNewCustomer.Controls.Add(Label_CustomerDetailsNewCustomer)
        FlowLayoutPanel_CustomerDetailsNewCustomer.Controls.Add(FlowLayoutPanel_StartDateInputSales)
        FlowLayoutPanel_CustomerDetailsNewCustomer.Controls.Add(FlowLayoutPanel_EndDateInputSales)
        FlowLayoutPanel_CustomerDetailsNewCustomer.Controls.Add(Button_AllTimeSalesListAndSummaryFilter)
        FlowLayoutPanel_CustomerDetailsNewCustomer.Controls.Add(Button_TodaySalesListAndSummaryFilter)
        FlowLayoutPanel_CustomerDetailsNewCustomer.Controls.Add(Button_WeeklySalesListAndSummaryFilter)
        FlowLayoutPanel_CustomerDetailsNewCustomer.Controls.Add(Button_MonthlySalesListAndSummaryFilter)
        FlowLayoutPanel_CustomerDetailsNewCustomer.Controls.Add(Button_QuarterlySalesListAndSummaryFilter)
        FlowLayoutPanel_CustomerDetailsNewCustomer.Controls.Add(Button_AnnuallySalesListAndSummaryFilter)
        FlowLayoutPanel_CustomerDetailsNewCustomer.Controls.Add(Label1)
        FlowLayoutPanel_CustomerDetailsNewCustomer.Controls.Add(Button_AllStatusSalesListAndSummaryFilter)
        FlowLayoutPanel_CustomerDetailsNewCustomer.Controls.Add(Button_CompletedSalesListAndSummaryFilter)
        FlowLayoutPanel_CustomerDetailsNewCustomer.Controls.Add(Button_PendingApprovalSalesListAndSummaryFilter)
        FlowLayoutPanel_CustomerDetailsNewCustomer.Controls.Add(Button_OpenSalesListAndSummaryFilter)
        FlowLayoutPanel_CustomerDetailsNewCustomer.Controls.Add(Button_VoidedSalesListAndSummaryFilter)
        FlowLayoutPanel_CustomerDetailsNewCustomer.Controls.Add(Button_OverdueSalesListAndSummaryFilter)
        FlowLayoutPanel_CustomerDetailsNewCustomer.Controls.Add(FlowLayoutPanel1)
        FlowLayoutPanel_CustomerDetailsNewCustomer.Controls.Add(Button_ResetSalesListAndSummaryFilter)
        FlowLayoutPanel_CustomerDetailsNewCustomer.Controls.Add(Button_ApplyFiltersSalesListAndSummaryFilter)
        FlowLayoutPanel_CustomerDetailsNewCustomer.Location = New System.Drawing.Point(0, 0)
        FlowLayoutPanel_CustomerDetailsNewCustomer.Margin = New System.Windows.Forms.Padding(0)
        FlowLayoutPanel_CustomerDetailsNewCustomer.Name = "FlowLayoutPanel_CustomerDetailsNewCustomer"
        FlowLayoutPanel_CustomerDetailsNewCustomer.Size = New System.Drawing.Size(720, 720)
        FlowLayoutPanel_CustomerDetailsNewCustomer.TabIndex = 34
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
        FlowLayoutPanel_PageHeaderNewCustomer.Size = New System.Drawing.Size(409, 32)
        FlowLayoutPanel_PageHeaderNewCustomer.TabIndex = 0
        '
        'Panel_PageHeaderIconNewCustomer
        '
        Panel_PageHeaderIconNewCustomer.Anchor = System.Windows.Forms.AnchorStyles.Left
        Panel_PageHeaderIconNewCustomer.BackgroundImage = Global.ERP_SIMS.My.Resources.Resources.HeaderIcon_SalesReport
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
        Label_PageHeaderNewCustomer.Size = New System.Drawing.Size(361, 31)
        Label_PageHeaderNewCustomer.TabIndex = 2
        Label_PageHeaderNewCustomer.Text = "Sales List and Summary Filters"
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
        Label_CustomerDetailsNewCustomer.Text = "Date Range"
        Label_CustomerDetailsNewCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'FlowLayoutPanel_StartDateInputSales
        '
        FlowLayoutPanel_StartDateInputSales.AutoSize = true
        FlowLayoutPanel_StartDateInputSales.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_StartDateInputSales.Controls.Add(Label_StartDateInputSales)
        FlowLayoutPanel_StartDateInputSales.Controls.Add(DateTimePicker_StartDateSalesListInputAndSummaryFilter)
        FlowLayoutPanel_StartDateInputSales.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_StartDateInputSales.Location = New System.Drawing.Point(12, 132)
        FlowLayoutPanel_StartDateInputSales.Margin = New System.Windows.Forms.Padding(12, 0, 6, 12)
        FlowLayoutPanel_StartDateInputSales.Name = "FlowLayoutPanel_StartDateInputSales"
        FlowLayoutPanel_StartDateInputSales.Size = New System.Drawing.Size(342, 56)
        FlowLayoutPanel_StartDateInputSales.TabIndex = 6
        '
        'Label_StartDateInputSales
        '
        Label_StartDateInputSales.AutoSize = true
        Label_StartDateInputSales.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_StartDateInputSales.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_StartDateInputSales.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_StartDateInputSales.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_StartDateInputSales.Location = New System.Drawing.Point(0, 0)
        Label_StartDateInputSales.Margin = New System.Windows.Forms.Padding(0)
        Label_StartDateInputSales.Name = "Label_StartDateInputSales"
        Label_StartDateInputSales.Size = New System.Drawing.Size(63, 16)
        Label_StartDateInputSales.TabIndex = 2
        Label_StartDateInputSales.Text = "Start Date"
        Label_StartDateInputSales.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'DateTimePicker_StartDateSalesListInputAndSummaryFilter
        '
        DateTimePicker_StartDateSalesListInputAndSummaryFilter.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        DateTimePicker_StartDateSalesListInputAndSummaryFilter.BorderThickness = 1
        DateTimePicker_StartDateSalesListInputAndSummaryFilter.Checked = true
        DateTimePicker_StartDateSalesListInputAndSummaryFilter.CheckedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        DateTimePicker_StartDateSalesListInputAndSummaryFilter.CheckedState.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        DateTimePicker_StartDateSalesListInputAndSummaryFilter.CheckedState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        DateTimePicker_StartDateSalesListInputAndSummaryFilter.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        DateTimePicker_StartDateSalesListInputAndSummaryFilter.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
        DateTimePicker_StartDateSalesListInputAndSummaryFilter.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        DateTimePicker_StartDateSalesListInputAndSummaryFilter.Format = System.Windows.Forms.DateTimePickerFormat.[Long]
        DateTimePicker_StartDateSalesListInputAndSummaryFilter.Location = New System.Drawing.Point(0, 16)
        DateTimePicker_StartDateSalesListInputAndSummaryFilter.Margin = New System.Windows.Forms.Padding(0)
        DateTimePicker_StartDateSalesListInputAndSummaryFilter.MaxDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        DateTimePicker_StartDateSalesListInputAndSummaryFilter.MinDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        DateTimePicker_StartDateSalesListInputAndSummaryFilter.Name = "DateTimePicker_StartDateSalesListInputAndSummaryFilter"
        DateTimePicker_StartDateSalesListInputAndSummaryFilter.Size = New System.Drawing.Size(342, 40)
        DateTimePicker_StartDateSalesListInputAndSummaryFilter.TabIndex = 4
        DateTimePicker_StartDateSalesListInputAndSummaryFilter.Value = New Date(2024, 10, 27, 21, 34, 18, 656)
        '
        'FlowLayoutPanel_EndDateInputSales
        '
        FlowLayoutPanel_EndDateInputSales.AutoSize = true
        FlowLayoutPanel_EndDateInputSales.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel_EndDateInputSales.Controls.Add(Label_EndDateSales)
        FlowLayoutPanel_EndDateInputSales.Controls.Add(DateTimePicker_EndDateInputSalesListAndSummaryFilter)
        FlowLayoutPanel_EndDateInputSales.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel_EndDateInputSales.Location = New System.Drawing.Point(366, 132)
        FlowLayoutPanel_EndDateInputSales.Margin = New System.Windows.Forms.Padding(6, 0, 12, 12)
        FlowLayoutPanel_EndDateInputSales.Name = "FlowLayoutPanel_EndDateInputSales"
        FlowLayoutPanel_EndDateInputSales.Size = New System.Drawing.Size(342, 56)
        FlowLayoutPanel_EndDateInputSales.TabIndex = 35
        '
        'Label_EndDateSales
        '
        Label_EndDateSales.AutoSize = true
        Label_EndDateSales.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label_EndDateSales.Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label_EndDateSales.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer), CType(CType(102,Byte),Integer))
        Label_EndDateSales.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label_EndDateSales.Location = New System.Drawing.Point(0, 0)
        Label_EndDateSales.Margin = New System.Windows.Forms.Padding(0)
        Label_EndDateSales.Name = "Label_EndDateSales"
        Label_EndDateSales.Size = New System.Drawing.Size(59, 16)
        Label_EndDateSales.TabIndex = 2
        Label_EndDateSales.Text = "End Date"
        Label_EndDateSales.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'DateTimePicker_EndDateInputSalesListAndSummaryFilter
        '
        DateTimePicker_EndDateInputSalesListAndSummaryFilter.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        DateTimePicker_EndDateInputSalesListAndSummaryFilter.BorderThickness = 1
        DateTimePicker_EndDateInputSalesListAndSummaryFilter.Checked = true
        DateTimePicker_EndDateInputSalesListAndSummaryFilter.CheckedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        DateTimePicker_EndDateInputSalesListAndSummaryFilter.CheckedState.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        DateTimePicker_EndDateInputSalesListAndSummaryFilter.CheckedState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        DateTimePicker_EndDateInputSalesListAndSummaryFilter.FillColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        DateTimePicker_EndDateInputSalesListAndSummaryFilter.Font = New System.Drawing.Font("Microsoft JhengHei", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
        DateTimePicker_EndDateInputSalesListAndSummaryFilter.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        DateTimePicker_EndDateInputSalesListAndSummaryFilter.Format = System.Windows.Forms.DateTimePickerFormat.[Long]
        DateTimePicker_EndDateInputSalesListAndSummaryFilter.Location = New System.Drawing.Point(0, 16)
        DateTimePicker_EndDateInputSalesListAndSummaryFilter.Margin = New System.Windows.Forms.Padding(0)
        DateTimePicker_EndDateInputSalesListAndSummaryFilter.MaxDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        DateTimePicker_EndDateInputSalesListAndSummaryFilter.MinDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        DateTimePicker_EndDateInputSalesListAndSummaryFilter.Name = "DateTimePicker_EndDateInputSalesListAndSummaryFilter"
        DateTimePicker_EndDateInputSalesListAndSummaryFilter.Size = New System.Drawing.Size(342, 40)
        DateTimePicker_EndDateInputSalesListAndSummaryFilter.TabIndex = 5
        DateTimePicker_EndDateInputSalesListAndSummaryFilter.Value = New Date(2024, 10, 27, 21, 34, 18, 656)
        '
        'Button_TodaySalesListAndSummaryFilter
        '
        Button_TodaySalesListAndSummaryFilter.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Button_TodaySalesListAndSummaryFilter.BorderThickness = 1
        Button_TodaySalesListAndSummaryFilter.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Button_TodaySalesListAndSummaryFilter.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Button_TodaySalesListAndSummaryFilter.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer))
        Button_TodaySalesListAndSummaryFilter.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer))
        Button_TodaySalesListAndSummaryFilter.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Button_TodaySalesListAndSummaryFilter.Font = New System.Drawing.Font("Segoe UI", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel)
        Button_TodaySalesListAndSummaryFilter.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Button_TodaySalesListAndSummaryFilter.Location = New System.Drawing.Point(189, 200)
        Button_TodaySalesListAndSummaryFilter.Margin = New System.Windows.Forms.Padding(6, 0, 6, 12)
        Button_TodaySalesListAndSummaryFilter.Name = "Button_TodaySalesListAndSummaryFilter"
        Button_TodaySalesListAndSummaryFilter.Size = New System.Drawing.Size(165, 40)
        Button_TodaySalesListAndSummaryFilter.TabIndex = 48
        Button_TodaySalesListAndSummaryFilter.Text = "Today"
        Button_TodaySalesListAndSummaryFilter.TextOffset = New System.Drawing.Point(0, -2)
        '
        'Button_WeeklySalesListAndSummaryFilter
        '
        Button_WeeklySalesListAndSummaryFilter.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Button_WeeklySalesListAndSummaryFilter.BorderThickness = 1
        Button_WeeklySalesListAndSummaryFilter.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Button_WeeklySalesListAndSummaryFilter.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Button_WeeklySalesListAndSummaryFilter.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer))
        Button_WeeklySalesListAndSummaryFilter.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer))
        Button_WeeklySalesListAndSummaryFilter.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Button_WeeklySalesListAndSummaryFilter.Font = New System.Drawing.Font("Segoe UI", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel)
        Button_WeeklySalesListAndSummaryFilter.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Button_WeeklySalesListAndSummaryFilter.Location = New System.Drawing.Point(366, 200)
        Button_WeeklySalesListAndSummaryFilter.Margin = New System.Windows.Forms.Padding(6, 0, 6, 12)
        Button_WeeklySalesListAndSummaryFilter.Name = "Button_WeeklySalesListAndSummaryFilter"
        Button_WeeklySalesListAndSummaryFilter.Size = New System.Drawing.Size(165, 40)
        Button_WeeklySalesListAndSummaryFilter.TabIndex = 36
        Button_WeeklySalesListAndSummaryFilter.Text = "Weekly"
        Button_WeeklySalesListAndSummaryFilter.TextOffset = New System.Drawing.Point(0, -2)
        '
        'Button_MonthlySalesListAndSummaryFilter
        '
        Button_MonthlySalesListAndSummaryFilter.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Button_MonthlySalesListAndSummaryFilter.BorderThickness = 1
        Button_MonthlySalesListAndSummaryFilter.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Button_MonthlySalesListAndSummaryFilter.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Button_MonthlySalesListAndSummaryFilter.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer))
        Button_MonthlySalesListAndSummaryFilter.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer))
        Button_MonthlySalesListAndSummaryFilter.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Button_MonthlySalesListAndSummaryFilter.Font = New System.Drawing.Font("Segoe UI", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel)
        Button_MonthlySalesListAndSummaryFilter.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Button_MonthlySalesListAndSummaryFilter.Location = New System.Drawing.Point(543, 200)
        Button_MonthlySalesListAndSummaryFilter.Margin = New System.Windows.Forms.Padding(6, 0, 6, 12)
        Button_MonthlySalesListAndSummaryFilter.Name = "Button_MonthlySalesListAndSummaryFilter"
        Button_MonthlySalesListAndSummaryFilter.Size = New System.Drawing.Size(165, 40)
        Button_MonthlySalesListAndSummaryFilter.TabIndex = 37
        Button_MonthlySalesListAndSummaryFilter.Text = "Monthly"
        Button_MonthlySalesListAndSummaryFilter.TextOffset = New System.Drawing.Point(0, -2)
        '
        'Button_QuarterlySalesListAndSummaryFilter
        '
        Button_QuarterlySalesListAndSummaryFilter.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Button_QuarterlySalesListAndSummaryFilter.BorderThickness = 1
        Button_QuarterlySalesListAndSummaryFilter.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Button_QuarterlySalesListAndSummaryFilter.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Button_QuarterlySalesListAndSummaryFilter.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer))
        Button_QuarterlySalesListAndSummaryFilter.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer))
        Button_QuarterlySalesListAndSummaryFilter.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Button_QuarterlySalesListAndSummaryFilter.Font = New System.Drawing.Font("Segoe UI", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel)
        Button_QuarterlySalesListAndSummaryFilter.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Button_QuarterlySalesListAndSummaryFilter.Location = New System.Drawing.Point(12, 252)
        Button_QuarterlySalesListAndSummaryFilter.Margin = New System.Windows.Forms.Padding(12, 0, 6, 12)
        Button_QuarterlySalesListAndSummaryFilter.Name = "Button_QuarterlySalesListAndSummaryFilter"
        Button_QuarterlySalesListAndSummaryFilter.Size = New System.Drawing.Size(165, 40)
        Button_QuarterlySalesListAndSummaryFilter.TabIndex = 38
        Button_QuarterlySalesListAndSummaryFilter.Text = "Quarterly"
        Button_QuarterlySalesListAndSummaryFilter.TextOffset = New System.Drawing.Point(0, -2)
        '
        'Button_AnnuallySalesListAndSummaryFilter
        '
        Button_AnnuallySalesListAndSummaryFilter.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Button_AnnuallySalesListAndSummaryFilter.BorderThickness = 1
        Button_AnnuallySalesListAndSummaryFilter.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Button_AnnuallySalesListAndSummaryFilter.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Button_AnnuallySalesListAndSummaryFilter.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer))
        Button_AnnuallySalesListAndSummaryFilter.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer))
        Button_AnnuallySalesListAndSummaryFilter.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Button_AnnuallySalesListAndSummaryFilter.Font = New System.Drawing.Font("Segoe UI", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel)
        Button_AnnuallySalesListAndSummaryFilter.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Button_AnnuallySalesListAndSummaryFilter.Location = New System.Drawing.Point(189, 252)
        Button_AnnuallySalesListAndSummaryFilter.Margin = New System.Windows.Forms.Padding(6, 0, 6, 12)
        Button_AnnuallySalesListAndSummaryFilter.Name = "Button_AnnuallySalesListAndSummaryFilter"
        Button_AnnuallySalesListAndSummaryFilter.Size = New System.Drawing.Size(165, 40)
        Button_AnnuallySalesListAndSummaryFilter.TabIndex = 40
        Button_AnnuallySalesListAndSummaryFilter.Text = "Annually"
        Button_AnnuallySalesListAndSummaryFilter.TextOffset = New System.Drawing.Point(0, -2)
        '
        'Label1
        '
        Label1.Anchor = System.Windows.Forms.AnchorStyles.Left
        Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Label1.Font = New System.Drawing.Font("Microsoft JhengHei", 22!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, CType(0,Byte))
        Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Label1.Location = New System.Drawing.Point(12, 328)
        Label1.Margin = New System.Windows.Forms.Padding(12, 24, 12, 24)
        Label1.Name = "Label1"
        Label1.Size = New System.Drawing.Size(696, 28)
        Label1.TabIndex = 41
        Label1.Text = "Transaction Status"
        Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Button_AllStatusSalesListAndSummaryFilter
        '
        Button_AllStatusSalesListAndSummaryFilter.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Button_AllStatusSalesListAndSummaryFilter.BorderThickness = 1
        Button_AllStatusSalesListAndSummaryFilter.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Button_AllStatusSalesListAndSummaryFilter.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Button_AllStatusSalesListAndSummaryFilter.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer))
        Button_AllStatusSalesListAndSummaryFilter.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer))
        Button_AllStatusSalesListAndSummaryFilter.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Button_AllStatusSalesListAndSummaryFilter.Font = New System.Drawing.Font("Segoe UI", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel)
        Button_AllStatusSalesListAndSummaryFilter.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Button_AllStatusSalesListAndSummaryFilter.Location = New System.Drawing.Point(12, 380)
        Button_AllStatusSalesListAndSummaryFilter.Margin = New System.Windows.Forms.Padding(12, 0, 6, 12)
        Button_AllStatusSalesListAndSummaryFilter.Name = "Button_AllStatusSalesListAndSummaryFilter"
        Button_AllStatusSalesListAndSummaryFilter.Size = New System.Drawing.Size(165, 40)
        Button_AllStatusSalesListAndSummaryFilter.TabIndex = 49
        Button_AllStatusSalesListAndSummaryFilter.Text = "All Status"
        Button_AllStatusSalesListAndSummaryFilter.TextOffset = New System.Drawing.Point(0, -2)
        '
        'Button_CompletedSalesListAndSummaryFilter
        '
        Button_CompletedSalesListAndSummaryFilter.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Button_CompletedSalesListAndSummaryFilter.BorderThickness = 1
        Button_CompletedSalesListAndSummaryFilter.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Button_CompletedSalesListAndSummaryFilter.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Button_CompletedSalesListAndSummaryFilter.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer))
        Button_CompletedSalesListAndSummaryFilter.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer))
        Button_CompletedSalesListAndSummaryFilter.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Button_CompletedSalesListAndSummaryFilter.Font = New System.Drawing.Font("Segoe UI", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel)
        Button_CompletedSalesListAndSummaryFilter.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Button_CompletedSalesListAndSummaryFilter.Location = New System.Drawing.Point(189, 380)
        Button_CompletedSalesListAndSummaryFilter.Margin = New System.Windows.Forms.Padding(6, 0, 6, 12)
        Button_CompletedSalesListAndSummaryFilter.Name = "Button_CompletedSalesListAndSummaryFilter"
        Button_CompletedSalesListAndSummaryFilter.Size = New System.Drawing.Size(165, 40)
        Button_CompletedSalesListAndSummaryFilter.TabIndex = 42
        Button_CompletedSalesListAndSummaryFilter.Text = "Completed"
        Button_CompletedSalesListAndSummaryFilter.TextOffset = New System.Drawing.Point(0, -2)
        '
        'Button_PendingApprovalSalesListAndSummaryFilter
        '
        Button_PendingApprovalSalesListAndSummaryFilter.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Button_PendingApprovalSalesListAndSummaryFilter.BorderThickness = 1
        Button_PendingApprovalSalesListAndSummaryFilter.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Button_PendingApprovalSalesListAndSummaryFilter.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Button_PendingApprovalSalesListAndSummaryFilter.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer))
        Button_PendingApprovalSalesListAndSummaryFilter.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer))
        Button_PendingApprovalSalesListAndSummaryFilter.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Button_PendingApprovalSalesListAndSummaryFilter.Font = New System.Drawing.Font("Segoe UI", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel)
        Button_PendingApprovalSalesListAndSummaryFilter.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Button_PendingApprovalSalesListAndSummaryFilter.Location = New System.Drawing.Point(366, 380)
        Button_PendingApprovalSalesListAndSummaryFilter.Margin = New System.Windows.Forms.Padding(6, 0, 6, 12)
        Button_PendingApprovalSalesListAndSummaryFilter.Name = "Button_PendingApprovalSalesListAndSummaryFilter"
        Button_PendingApprovalSalesListAndSummaryFilter.Size = New System.Drawing.Size(165, 40)
        Button_PendingApprovalSalesListAndSummaryFilter.TabIndex = 43
        Button_PendingApprovalSalesListAndSummaryFilter.Text = "Pending Approval"
        Button_PendingApprovalSalesListAndSummaryFilter.TextOffset = New System.Drawing.Point(0, -2)
        '
        'Button_OpenSalesListAndSummaryFilter
        '
        Button_OpenSalesListAndSummaryFilter.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Button_OpenSalesListAndSummaryFilter.BorderThickness = 1
        Button_OpenSalesListAndSummaryFilter.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Button_OpenSalesListAndSummaryFilter.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Button_OpenSalesListAndSummaryFilter.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer))
        Button_OpenSalesListAndSummaryFilter.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer))
        Button_OpenSalesListAndSummaryFilter.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Button_OpenSalesListAndSummaryFilter.Font = New System.Drawing.Font("Segoe UI", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel)
        Button_OpenSalesListAndSummaryFilter.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Button_OpenSalesListAndSummaryFilter.Location = New System.Drawing.Point(543, 380)
        Button_OpenSalesListAndSummaryFilter.Margin = New System.Windows.Forms.Padding(6, 0, 6, 12)
        Button_OpenSalesListAndSummaryFilter.Name = "Button_OpenSalesListAndSummaryFilter"
        Button_OpenSalesListAndSummaryFilter.Size = New System.Drawing.Size(165, 40)
        Button_OpenSalesListAndSummaryFilter.TabIndex = 44
        Button_OpenSalesListAndSummaryFilter.Text = "Open"
        Button_OpenSalesListAndSummaryFilter.TextOffset = New System.Drawing.Point(0, -2)
        '
        'Button_VoidedSalesListAndSummaryFilter
        '
        Button_VoidedSalesListAndSummaryFilter.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Button_VoidedSalesListAndSummaryFilter.BorderThickness = 1
        Button_VoidedSalesListAndSummaryFilter.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Button_VoidedSalesListAndSummaryFilter.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Button_VoidedSalesListAndSummaryFilter.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer))
        Button_VoidedSalesListAndSummaryFilter.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer))
        Button_VoidedSalesListAndSummaryFilter.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Button_VoidedSalesListAndSummaryFilter.Font = New System.Drawing.Font("Segoe UI", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel)
        Button_VoidedSalesListAndSummaryFilter.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Button_VoidedSalesListAndSummaryFilter.Location = New System.Drawing.Point(12, 432)
        Button_VoidedSalesListAndSummaryFilter.Margin = New System.Windows.Forms.Padding(12, 0, 6, 12)
        Button_VoidedSalesListAndSummaryFilter.Name = "Button_VoidedSalesListAndSummaryFilter"
        Button_VoidedSalesListAndSummaryFilter.Size = New System.Drawing.Size(165, 40)
        Button_VoidedSalesListAndSummaryFilter.TabIndex = 45
        Button_VoidedSalesListAndSummaryFilter.Text = "Voided"
        Button_VoidedSalesListAndSummaryFilter.TextOffset = New System.Drawing.Point(0, -2)
        '
        'Button_OverdueSalesListAndSummaryFilter
        '
        Button_OverdueSalesListAndSummaryFilter.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Button_OverdueSalesListAndSummaryFilter.BorderThickness = 1
        Button_OverdueSalesListAndSummaryFilter.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Button_OverdueSalesListAndSummaryFilter.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Button_OverdueSalesListAndSummaryFilter.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer))
        Button_OverdueSalesListAndSummaryFilter.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer))
        Button_OverdueSalesListAndSummaryFilter.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Button_OverdueSalesListAndSummaryFilter.Font = New System.Drawing.Font("Segoe UI", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel)
        Button_OverdueSalesListAndSummaryFilter.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Button_OverdueSalesListAndSummaryFilter.Location = New System.Drawing.Point(189, 432)
        Button_OverdueSalesListAndSummaryFilter.Margin = New System.Windows.Forms.Padding(6, 0, 6, 12)
        Button_OverdueSalesListAndSummaryFilter.Name = "Button_OverdueSalesListAndSummaryFilter"
        Button_OverdueSalesListAndSummaryFilter.Size = New System.Drawing.Size(165, 40)
        Button_OverdueSalesListAndSummaryFilter.TabIndex = 46
        Button_OverdueSalesListAndSummaryFilter.Text = "Overdue"
        Button_OverdueSalesListAndSummaryFilter.TextOffset = New System.Drawing.Point(0, -2)
        '
        'FlowLayoutPanel1
        '
        FlowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        FlowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        FlowLayoutPanel1.Location = New System.Drawing.Point(366, 432)
        FlowLayoutPanel1.Margin = New System.Windows.Forms.Padding(6, 0, 6, 12)
        FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        FlowLayoutPanel1.Size = New System.Drawing.Size(342, 40)
        FlowLayoutPanel1.TabIndex = 47
        '
        'Button_ResetSalesListAndSummaryFilter
        '
        Button_ResetSalesListAndSummaryFilter.BackColor = System.Drawing.Color.FromArgb(CType(CType(220,Byte),Integer), CType(CType(76,Byte),Integer), CType(CType(100,Byte),Integer))
        Button_ResetSalesListAndSummaryFilter.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Button_ResetSalesListAndSummaryFilter.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Button_ResetSalesListAndSummaryFilter.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer))
        Button_ResetSalesListAndSummaryFilter.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer))
        Button_ResetSalesListAndSummaryFilter.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Button_ResetSalesListAndSummaryFilter.Font = New System.Drawing.Font("Segoe UI", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel)
        Button_ResetSalesListAndSummaryFilter.ForeColor = System.Drawing.Color.White
        Button_ResetSalesListAndSummaryFilter.Location = New System.Drawing.Point(12, 496)
        Button_ResetSalesListAndSummaryFilter.Margin = New System.Windows.Forms.Padding(12, 12, 6, 12)
        Button_ResetSalesListAndSummaryFilter.Name = "Button_ResetSalesListAndSummaryFilter"
        Button_ResetSalesListAndSummaryFilter.Size = New System.Drawing.Size(342, 40)
        Button_ResetSalesListAndSummaryFilter.TabIndex = 34
        Button_ResetSalesListAndSummaryFilter.Text = "Reset Filters"
        Button_ResetSalesListAndSummaryFilter.TextOffset = New System.Drawing.Point(0, -2)
        '
        'Button_ApplyFiltersSalesListAndSummaryFilter
        '
        Button_ApplyFiltersSalesListAndSummaryFilter.BackColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(44,Byte),Integer), CType(CType(62,Byte),Integer))
        Button_ApplyFiltersSalesListAndSummaryFilter.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Button_ApplyFiltersSalesListAndSummaryFilter.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Button_ApplyFiltersSalesListAndSummaryFilter.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer))
        Button_ApplyFiltersSalesListAndSummaryFilter.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer))
        Button_ApplyFiltersSalesListAndSummaryFilter.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Button_ApplyFiltersSalesListAndSummaryFilter.Font = New System.Drawing.Font("Segoe UI", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel)
        Button_ApplyFiltersSalesListAndSummaryFilter.ForeColor = System.Drawing.Color.White
        Button_ApplyFiltersSalesListAndSummaryFilter.Location = New System.Drawing.Point(366, 496)
        Button_ApplyFiltersSalesListAndSummaryFilter.Margin = New System.Windows.Forms.Padding(6, 12, 12, 12)
        Button_ApplyFiltersSalesListAndSummaryFilter.Name = "Button_ApplyFiltersSalesListAndSummaryFilter"
        Button_ApplyFiltersSalesListAndSummaryFilter.Size = New System.Drawing.Size(342, 40)
        Button_ApplyFiltersSalesListAndSummaryFilter.TabIndex = 33
        Button_ApplyFiltersSalesListAndSummaryFilter.Text = "Apply Filters"
        Button_ApplyFiltersSalesListAndSummaryFilter.TextOffset = New System.Drawing.Point(0, -2)
        '
        'Button_AllTimeSalesListAndSummaryFilter
        '
        Button_AllTimeSalesListAndSummaryFilter.BackColor = System.Drawing.Color.FromArgb(CType(CType(235,Byte),Integer), CType(CType(225,Byte),Integer), CType(CType(255,Byte),Integer))
        Button_AllTimeSalesListAndSummaryFilter.BorderThickness = 1
        Button_AllTimeSalesListAndSummaryFilter.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Button_AllTimeSalesListAndSummaryFilter.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Button_AllTimeSalesListAndSummaryFilter.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer), CType(CType(169,Byte),Integer))
        Button_AllTimeSalesListAndSummaryFilter.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer), CType(CType(141,Byte),Integer))
        Button_AllTimeSalesListAndSummaryFilter.FillColor = System.Drawing.Color.FromArgb(CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer), CType(CType(0,Byte),Integer))
        Button_AllTimeSalesListAndSummaryFilter.Font = New System.Drawing.Font("Segoe UI", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel)
        Button_AllTimeSalesListAndSummaryFilter.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer), CType(CType(34,Byte),Integer))
        Button_AllTimeSalesListAndSummaryFilter.Location = New System.Drawing.Point(12, 200)
        Button_AllTimeSalesListAndSummaryFilter.Margin = New System.Windows.Forms.Padding(12, 0, 6, 12)
        Button_AllTimeSalesListAndSummaryFilter.Name = "Button_AllTimeSalesListAndSummaryFilter"
        Button_AllTimeSalesListAndSummaryFilter.Size = New System.Drawing.Size(165, 40)
        Button_AllTimeSalesListAndSummaryFilter.TabIndex = 50
        Button_AllTimeSalesListAndSummaryFilter.Text = "All Time"
        Button_AllTimeSalesListAndSummaryFilter.TextOffset = New System.Drawing.Point(0, -2)
        '
        '_Temporary_SalesListAndSummaryFilter
        '
        Prompt_SalesListAndSummaryFilter.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Prompt_SalesListAndSummaryFilter.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Prompt_SalesListAndSummaryFilter.ClientSize = New System.Drawing.Size(720, 720)
        Prompt_SalesListAndSummaryFilter.Controls.Add(FlowLayoutPanel_CustomerDetailsNewCustomer)
        Prompt_SalesListAndSummaryFilter.Name = "_Temporary_SalesListAndSummaryFilter"
        FlowLayoutPanel_CustomerDetailsNewCustomer.ResumeLayout(false)
        FlowLayoutPanel_CustomerDetailsNewCustomer.PerformLayout
        FlowLayoutPanel_PageHeaderNewCustomer.ResumeLayout(false)
        FlowLayoutPanel_PageHeaderNewCustomer.PerformLayout
        FlowLayoutPanel_StartDateInputSales.ResumeLayout(false)
        FlowLayoutPanel_StartDateInputSales.PerformLayout
        FlowLayoutPanel_EndDateInputSales.ResumeLayout(false)
        FlowLayoutPanel_EndDateInputSales.PerformLayout
        ResumeLayout(false)

        Dim StartDate = FilterTransactionsListAndSummaryStartDate
        Dim EndDate = FilterTransactionsListAndSummaryEndDate
        Dim Status = FilterTransactionsListAndSummaryStatus
        FilterTextDate = FilterTextDate

        Dim defaultBackColor As Color = Color.FromArgb(235, 225, 255)
        Dim defaultForeColor As Color = Color.FromArgb(34, 34, 34)
        Dim activeBackColor As Color = Color.FromArgb(0, 44, 62)
        Dim activeForeColor As Color = Color.FromArgb(255, 255, 255)

        Dim today As DateTime = DateTime.Now.Date
        Dim currentQuarter As Integer = (DateTime.Now.Month - 1) \ 3 + 1

        If StartDate = DateTime.MinValue AndAlso EndDate = DateTime.MaxValue Then
            ResetButtonStyles(Button_TodaySalesListAndSummaryFilter, Button_WeeklySalesListAndSummaryFilter, Button_MonthlySalesListAndSummaryFilter,
                              Button_QuarterlySalesListAndSummaryFilter, Button_AnnuallySalesListAndSummaryFilter)

            Button_AllTimeSalesListAndSummaryFilter.BackColor = activeBackColor
            Button_AllTimeSalesListAndSummaryFilter.ForeColor = activeForeColor

        ElseIf StartDate = DateTime.Now.Date AndAlso EndDate = DateTime.Now.Date Then
            ResetButtonStyles(Button_AllTimeSalesListAndSummaryFilter, Button_WeeklySalesListAndSummaryFilter, Button_MonthlySalesListAndSummaryFilter,
                              Button_QuarterlySalesListAndSummaryFilter, Button_AnnuallySalesListAndSummaryFilter)

            Button_TodaySalesListAndSummaryFilter.BackColor = activeBackColor
            Button_TodaySalesListAndSummaryFilter.ForeColor = activeForeColor

        ElseIf StartDate = today.AddDays(-CInt(today.DayOfWeek)) AndAlso EndDate = StartDate.AddDays(6) Then
            today = DateTime.Now.Date
            StartDate = today.AddDays(-CInt(today.DayOfWeek))
            EndDate = StartDate.AddDays(6)

            ResetButtonStyles(Button_AllTimeSalesListAndSummaryFilter, Button_TodaySalesListAndSummaryFilter, Button_MonthlySalesListAndSummaryFilter,
                              Button_QuarterlySalesListAndSummaryFilter, Button_AnnuallySalesListAndSummaryFilter)

            Button_WeeklySalesListAndSummaryFilter.BackColor = activeBackColor
            Button_WeeklySalesListAndSummaryFilter.ForeColor = activeForeColor

            FilterTextDate = $"{StartDate:MM/dd/yyyy} to {EndDate:MM/dd/yyyy} (This Week)"

        ElseIf StartDate = New DateTime(DateTime.Now.Year, DateTime.Now.Month, 1) AndAlso EndDate = StartDate.AddMonths(1).AddDays(-1) Then
            StartDate = New DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)
            EndDate = StartDate.AddMonths(1).AddDays(-1)

            ResetButtonStyles(Button_AllTimeSalesListAndSummaryFilter, Button_TodaySalesListAndSummaryFilter, Button_WeeklySalesListAndSummaryFilter,
                              Button_QuarterlySalesListAndSummaryFilter, Button_AnnuallySalesListAndSummaryFilter)

            Button_MonthlySalesListAndSummaryFilter.BackColor = activeBackColor
            Button_MonthlySalesListAndSummaryFilter.ForeColor = activeForeColor

        ElseIf StartDate =  New DateTime(DateTime.Now.Year, DateTime.Now.Month, 1) AndAlso EndDate = StartDate.AddMonths(3).AddDays(-1)
            currentQuarter = (DateTime.Now.Month - 1) \ 3 + 1
            StartDate = New DateTime(DateTime.Now.Year, (currentQuarter - 1) * 3 + 1, 1)
            EndDate = StartDate.AddMonths(3).AddDays(-1)

            ResetButtonStyles(Button_AllTimeSalesListAndSummaryFilter, Button_TodaySalesListAndSummaryFilter, Button_WeeklySalesListAndSummaryFilter,
                              Button_MonthlySalesListAndSummaryFilter, Button_AnnuallySalesListAndSummaryFilter)

            Button_QuarterlySalesListAndSummaryFilter.BackColor = activeBackColor
            Button_QuarterlySalesListAndSummaryFilter.ForeColor = activeForeColor

        ElseIf StartDate =  New DateTime(DateTime.Now.Year, 1, 1) AndAlso EndDate = New DateTime(DateTime.Now.Year, 12, 31)
            StartDate = New DateTime(DateTime.Now.Year, 1, 1)
            EndDate = New DateTime(DateTime.Now.Year, 12, 31)

            ResetButtonStyles(Button_AllTimeSalesListAndSummaryFilter, Button_TodaySalesListAndSummaryFilter, Button_WeeklySalesListAndSummaryFilter,
                              Button_MonthlySalesListAndSummaryFilter, Button_QuarterlySalesListAndSummaryFilter)

            Button_AnnuallySalesListAndSummaryFilter.BackColor = activeBackColor
            Button_AnnuallySalesListAndSummaryFilter.ForeColor = activeForeColor

        End If

        If Status = "All Transactions" Then
            Button_AllStatusSalesListAndSummaryFilter.BackColor = Color.FromArgb(0, 44, 62)
            Button_AllStatusSalesListAndSummaryFilter.ForeColor = Color.FromArgb(255, 255, 255)

            Button_CompletedSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_CompletedSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_PendingApprovalSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_PendingApprovalSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_VoidedSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_VoidedSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_OpenSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_OpenSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_OverdueSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_OverdueSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)
        ElseIf Status = "Completed" Then
            Button_AllStatusSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_AllStatusSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_CompletedSalesListAndSummaryFilter.BackColor = Color.FromArgb(0, 44, 62)
            Button_CompletedSalesListAndSummaryFilter.ForeColor = Color.FromArgb(255, 255, 255)

            Button_PendingApprovalSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_PendingApprovalSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_VoidedSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_VoidedSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_OpenSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_OpenSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_OverdueSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_OverdueSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)
        ElseIf Status = "Pending Approval" Then
            Button_AllStatusSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_AllStatusSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_CompletedSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_CompletedSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_PendingApprovalSalesListAndSummaryFilter.BackColor = Color.FromArgb(0, 44, 62)
            Button_PendingApprovalSalesListAndSummaryFilter.ForeColor = Color.FromArgb(255, 255, 255)

            Button_VoidedSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_VoidedSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_OpenSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_OpenSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_OverdueSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_OverdueSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)
        ElseIf Status = "Voided" Then
            Button_AllStatusSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_AllStatusSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_CompletedSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_CompletedSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_PendingApprovalSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_PendingApprovalSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_VoidedSalesListAndSummaryFilter.BackColor = Color.FromArgb(0, 44, 62)
            Button_VoidedSalesListAndSummaryFilter.ForeColor = Color.FromArgb(255, 255, 255)

            Button_OpenSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_OpenSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_OverdueSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_OverdueSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)
        ElseIf Status = "Open" Then
            Button_AllStatusSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_AllStatusSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_CompletedSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_CompletedSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_PendingApprovalSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_PendingApprovalSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_VoidedSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_VoidedSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_OpenSalesListAndSummaryFilter.BackColor = Color.FromArgb(0, 44, 62)
            Button_OpenSalesListAndSummaryFilter.ForeColor = Color.FromArgb(255, 255, 255)

            Button_OverdueSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_OverdueSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)
        ElseIf Status = "Overdue" Then
            Button_AllStatusSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_AllStatusSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_CompletedSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_CompletedSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_PendingApprovalSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_PendingApprovalSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_VoidedSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_VoidedSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_OpenSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_OpenSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_OverdueSalesListAndSummaryFilter.BackColor = Color.FromArgb(0, 44, 62)
            Button_OverdueSalesListAndSummaryFilter.ForeColor = Color.FromArgb(255, 255, 255)
        End If

        AddHandler Button_AllTimeSalesListAndSummaryFilter.Click, Sub(sender, e)
            StartDate = DateTime.MinValue
            EndDate = DateTime.MaxValue

            ResetButtonStyles(Button_TodaySalesListAndSummaryFilter, Button_WeeklySalesListAndSummaryFilter, Button_MonthlySalesListAndSummaryFilter,
                              Button_QuarterlySalesListAndSummaryFilter, Button_AnnuallySalesListAndSummaryFilter)

            Button_AllTimeSalesListAndSummaryFilter.BackColor = activeBackColor
            Button_AllTimeSalesListAndSummaryFilter.ForeColor = activeForeColor

            FilterTextDate = "All Time"
        End Sub

        ' Today button handler
        AddHandler Button_TodaySalesListAndSummaryFilter.Click, Sub(sender, e)
            StartDate = DateTime.Now.Date
            EndDate = DateTime.Now.Date

            ResetButtonStyles(Button_AllTimeSalesListAndSummaryFilter, Button_WeeklySalesListAndSummaryFilter, Button_MonthlySalesListAndSummaryFilter,
                              Button_QuarterlySalesListAndSummaryFilter, Button_AnnuallySalesListAndSummaryFilter)

            Button_TodaySalesListAndSummaryFilter.BackColor = activeBackColor
            Button_TodaySalesListAndSummaryFilter.ForeColor = activeForeColor

            FilterTextDate = $"{StartDate:MM/dd/yyyy} (Today)"
        End Sub

        ' Weekly button handler
        AddHandler Button_WeeklySalesListAndSummaryFilter.Click, Sub(sender, e)
            today = DateTime.Now.Date
            StartDate = today.AddDays(-CInt(today.DayOfWeek))
            EndDate = StartDate.AddDays(6)

            ResetButtonStyles(Button_AllTimeSalesListAndSummaryFilter, Button_TodaySalesListAndSummaryFilter, Button_MonthlySalesListAndSummaryFilter,
                              Button_QuarterlySalesListAndSummaryFilter, Button_AnnuallySalesListAndSummaryFilter)

            Button_WeeklySalesListAndSummaryFilter.BackColor = activeBackColor
            Button_WeeklySalesListAndSummaryFilter.ForeColor = activeForeColor

            FilterTextDate = $"{StartDate:MM/dd/yyyy} to {EndDate:MM/dd/yyyy} (This Week)"
        End Sub

        ' Monthly button handler
        AddHandler Button_MonthlySalesListAndSummaryFilter.Click, Sub(sender, e)
            StartDate = New DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)
            EndDate = StartDate.AddMonths(1).AddDays(-1)

            ResetButtonStyles(Button_AllTimeSalesListAndSummaryFilter, Button_TodaySalesListAndSummaryFilter, Button_WeeklySalesListAndSummaryFilter,
                              Button_QuarterlySalesListAndSummaryFilter, Button_AnnuallySalesListAndSummaryFilter)

            Button_MonthlySalesListAndSummaryFilter.BackColor = activeBackColor
            Button_MonthlySalesListAndSummaryFilter.ForeColor = activeForeColor

            FilterTextDate = $"{StartDate:MM/dd/yyyy} to {EndDate:MM/dd/yyyy} (This Month)"
        End Sub

        ' Quarterly button handler
        AddHandler Button_QuarterlySalesListAndSummaryFilter.Click, Sub(sender, e)
            currentQuarter = (DateTime.Now.Month - 1) \ 3 + 1
            StartDate = New DateTime(DateTime.Now.Year, (currentQuarter - 1) * 3 + 1, 1)
            EndDate = StartDate.AddMonths(3).AddDays(-1)

            ResetButtonStyles(Button_AllTimeSalesListAndSummaryFilter, Button_TodaySalesListAndSummaryFilter, Button_WeeklySalesListAndSummaryFilter,
                              Button_MonthlySalesListAndSummaryFilter, Button_AnnuallySalesListAndSummaryFilter)

            Button_QuarterlySalesListAndSummaryFilter.BackColor = activeBackColor
            Button_QuarterlySalesListAndSummaryFilter.ForeColor = activeForeColor

            FilterTextDate = $"{StartDate:MM/dd/yyyy} to {EndDate:MM/dd/yyyy} (This Quarter)"
        End Sub

        ' Annually button handler
        AddHandler Button_AnnuallySalesListAndSummaryFilter.Click, Sub(sender, e)
            StartDate = New DateTime(DateTime.Now.Year, 1, 1)
            EndDate = New DateTime(DateTime.Now.Year, 12, 31)

            ResetButtonStyles(Button_AllTimeSalesListAndSummaryFilter, Button_TodaySalesListAndSummaryFilter, Button_WeeklySalesListAndSummaryFilter,
                              Button_MonthlySalesListAndSummaryFilter, Button_QuarterlySalesListAndSummaryFilter)

            Button_AnnuallySalesListAndSummaryFilter.BackColor = activeBackColor
            Button_AnnuallySalesListAndSummaryFilter.ForeColor = activeForeColor

            FilterTextDate = $"{StartDate:MM/dd/yyyy} to {EndDate:MM/dd/yyyy} (This Year)"
        End Sub

        AddHandler Button_AllStatusSalesListAndSummaryFilter.Click, Sub(sender, e)
            Status = "All Transactions"
    
            Button_AllStatusSalesListAndSummaryFilter.BackColor = Color.FromArgb(0, 44, 62)
            Button_AllStatusSalesListAndSummaryFilter.ForeColor = Color.FromArgb(255, 255, 255)

            Button_CompletedSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_CompletedSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_PendingApprovalSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_PendingApprovalSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_VoidedSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_VoidedSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_OpenSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_OpenSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_OverdueSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_OverdueSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)
        End Sub

        AddHandler Button_CompletedSalesListAndSummaryFilter.Click, Sub(sender, e)
            Status = "Completed"
    
            Button_AllStatusSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_AllStatusSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_CompletedSalesListAndSummaryFilter.BackColor = Color.FromArgb(0, 44, 62)
            Button_CompletedSalesListAndSummaryFilter.ForeColor = Color.FromArgb(255, 255, 255)

            Button_PendingApprovalSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_PendingApprovalSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_VoidedSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_VoidedSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_OpenSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_OpenSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_OverdueSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_OverdueSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)
        End Sub

        AddHandler Button_PendingApprovalSalesListAndSummaryFilter.Click, Sub(sender, e)
            Status = "Pending Approval"
    
            Button_AllStatusSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_AllStatusSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_CompletedSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_CompletedSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_PendingApprovalSalesListAndSummaryFilter.BackColor = Color.FromArgb(0, 44, 62)
            Button_PendingApprovalSalesListAndSummaryFilter.ForeColor = Color.FromArgb(255, 255, 255)

            Button_VoidedSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_VoidedSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_OpenSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_OpenSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_OverdueSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_OverdueSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)
        End Sub

        AddHandler Button_VoidedSalesListAndSummaryFilter.Click, Sub(sender, e)
            Status = "Voided"
    
            Button_AllStatusSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_AllStatusSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_CompletedSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_CompletedSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_PendingApprovalSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_PendingApprovalSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_VoidedSalesListAndSummaryFilter.BackColor = Color.FromArgb(0, 44, 62)
            Button_VoidedSalesListAndSummaryFilter.ForeColor = Color.FromArgb(255, 255, 255)

            Button_OpenSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_OpenSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_OverdueSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_OverdueSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)
        End Sub

        AddHandler Button_OpenSalesListAndSummaryFilter.Click, Sub(sender, e)
            Status = "Open"
    
            Button_AllStatusSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_AllStatusSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_CompletedSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_CompletedSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_PendingApprovalSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_PendingApprovalSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_VoidedSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_VoidedSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_OpenSalesListAndSummaryFilter.BackColor = Color.FromArgb(0, 44, 62)
            Button_OpenSalesListAndSummaryFilter.ForeColor = Color.FromArgb(255, 255, 255)

            Button_OverdueSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_OverdueSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)
        End Sub

        AddHandler Button_OverdueSalesListAndSummaryFilter.Click, Sub(sender, e)
            Status = "Overdue"
    
            Button_AllStatusSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_AllStatusSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_CompletedSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_CompletedSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_PendingApprovalSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_PendingApprovalSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_VoidedSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_VoidedSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_OpenSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_OpenSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_OverdueSalesListAndSummaryFilter.BackColor = Color.FromArgb(0, 44, 62)
            Button_OverdueSalesListAndSummaryFilter.ForeColor = Color.FromArgb(255, 255, 255)
        End Sub

        AddHandler Button_ResetSalesListAndSummaryFilter.Click,
        Sub(sender, e)
            StartDate = DateTime.MinValue
            EndDate = DateTime.MaxValue
            FilterTextDate = "All Time"

            ResetButtonStyles(Button_TodaySalesListAndSummaryFilter, Button_WeeklySalesListAndSummaryFilter, Button_MonthlySalesListAndSummaryFilter,
                              Button_QuarterlySalesListAndSummaryFilter, Button_AnnuallySalesListAndSummaryFilter)

            Button_AllTimeSalesListAndSummaryFilter.BackColor = activeBackColor
            Button_AllTimeSalesListAndSummaryFilter.ForeColor = activeForeColor

            Status = "All Transactions"
    
            Button_AllStatusSalesListAndSummaryFilter.BackColor = Color.FromArgb(0, 44, 62)
            Button_AllStatusSalesListAndSummaryFilter.ForeColor = Color.FromArgb(255, 255, 255)

            Button_CompletedSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_CompletedSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_PendingApprovalSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_PendingApprovalSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_VoidedSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_VoidedSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_OpenSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_OpenSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)

            Button_OverdueSalesListAndSummaryFilter.BackColor = Color.FromArgb(235, 225, 255)
            Button_OverdueSalesListAndSummaryFilter.ForeColor = Color.FromArgb(34, 34, 34)
        End Sub

        AddHandler Button_ApplyFiltersSalesListAndSummaryFilter.Click, 
        Sub(sender, e)
            FilterTransactionsListAndSummaryStartDate = StartDate
            FilterTransactionsListAndSummaryEndDate = EndDate
            FilterTransactionsListAndSummaryStatus = Status
            SalesListCurrentPage = 1
            FilterTransactionsByDateAndStatus(FilterTransactionsListAndSummaryStartDate, FilterTransactionsListAndSummaryEndDate, FilterTransactionsListAndSummaryStatus)
            Panel_SaleDetailsCustomerDetailsNewSale.Text = "Date: " & FilterTextDate & Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Transaction Status: " & Status
            Prompt_SalesListAndSummaryFilter.Close()
        End Sub

        Prompt_SalesListAndSummaryFilter.ShowDialog(Me)
    End Function

    Sub ResetButtonStyles(ParamArray buttons() As Guna2Button)
        Dim defaultBackColor As Color = Color.FromArgb(235, 225, 255)
        Dim defaultForeColor As Color = Color.FromArgb(34, 34, 34)
        Dim activeBackColor As Color = Color.FromArgb(0, 44, 62)
        Dim activeForeColor As Color = Color.FromArgb(255, 255, 255)

        For Each btn As Guna2Button In buttons
            btn.BackColor = defaultBackColor
            btn.ForeColor = defaultForeColor
        Next
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
                            Dim paymentTermId As String = reader.GetInt32("payment_term_id")
                            Dim company As String = reader.GetString("company")
                            Dim email As String = reader.GetString("email")
                            Dim phone As String = reader.GetString("phone")
                            Dim addressLine As String = reader.GetString("address_line")
                            Dim city As String = reader.GetString("city")
                            Dim zipCode As String = reader.GetString("zip_code")
                            Dim country As String = reader.GetString("country")
                            Dim createdBy As Integer = reader.GetInt32("created_by")
                            Dim createdAt As DateTime = reader.GetDateTime("created_at")

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

    Public Function CreateTransactionItemArray() As TransactionItem()
        Dim transactionItems As New List(Of TransactionItem)

        Try
            Using connection As New MySqlConnection(ConnectionString)
                connection.Open()

                Dim query As String = "SELECT transaction_item_id, transaction_id, item_id, transaction_item_quantity, transaction_item_selling_price, transaction_item_amount, stock_id FROM transaction_items"
                Using command As New MySqlCommand(query, connection)
                    Using reader As MySqlDataReader = command.ExecuteReader()
                        While reader.Read()
                            Dim item As New TransactionItem(
                                reader.GetInt32("transaction_item_id"),
                                If(reader.IsDBNull(reader.GetOrdinal("transaction_id")), 0, reader.GetInt32("transaction_id")),
                                If(reader.IsDBNull(reader.GetOrdinal("item_id")), 0, reader.GetInt32("item_id")),
                                reader.GetInt32("transaction_item_quantity"),
                                reader.GetDecimal("transaction_item_selling_price"),
                                reader.GetDecimal("transaction_item_amount"),
                                If(reader.IsDBNull(reader.GetOrdinal("stock_id")), 0, reader.GetInt32("stock_id"))
                            )
                            transactionItems.Add(item)
                        End While
                    End Using
                End Using
            End Using

        Catch ex As MySqlException
            Console.WriteLine($"Database error: {ex.Message}")
        Catch ex As Exception
            Console.WriteLine($"Unexpected error: {ex.Message}")
        End Try

        Return transactionItems.ToArray()
    End Function

    Public Function CreateTransactionEntryArray() As TransactionEntry()
        Dim transactionEntries As New List(Of TransactionEntry)()

        Dim connectionString As String = "server=localhost;user id=root;password=benjaminn1202;database=sims_db"

        Dim query As String = "SELECT * FROM transaction_entries"

        Using connection As New MySqlConnection(ConnectionString)
            Try
                connection.Open()

                Using command As New MySqlCommand(query, connection)
                    Using reader As MySqlDataReader = command.ExecuteReader()
                        While reader.Read()
                            Dim entry As New TransactionEntry With {
                                .TransactionId = reader.GetInt32("transaction_id"),
                                .CustomerId = If(reader.IsDBNull(reader.GetOrdinal("customer_id")), 0, reader.GetInt32("customer_id")),
                                .Subtotal = reader.GetDecimal("subtotal"),
                                .NetTotal = reader.GetDecimal("net_total"),
                                .ReceivedAmount = reader.GetDecimal("received_amount"),
                                .ChangeAmount = reader.GetDecimal("change_amount"),
                                .TaxId = If(reader.IsDBNull(reader.GetOrdinal("tax_id")), 0, reader.GetInt32("tax_id")),
                                .TaxAmount = If(reader.IsDBNull(reader.GetOrdinal("tax_amount")), 0, reader.GetDecimal("tax_amount")),
                                .PaymentTermId = If(reader.IsDBNull(reader.GetOrdinal("payment_term_id")), 0, reader.GetInt32("payment_term_id")),
                                .DueAmount = If(reader.IsDBNull(reader.GetOrdinal("due_amount")), 0, reader.GetDecimal("due_amount")),
                                .DueDate = If(reader.IsDBNull(reader.GetOrdinal("due_date")), Date.MinValue, reader.GetDateTime("due_date")),
                                .Status = reader.GetString("status"),
                                .CreatedAt = If(reader.IsDBNull(reader.GetOrdinal("created_at")), DateTime.MinValue, reader.GetDateTime("created_at")),
                                .CreatedBy = If(reader.IsDBNull(reader.GetOrdinal("created_by")), 0, reader.GetInt32("created_by"))
                            }

                            ' Add the entry to the list
                            transactionEntries.Add(entry)
                        End While
                    End Using
                End Using
            Catch ex As Exception
                Console.WriteLine($"Error retrieving transaction entries: {ex.Message}")
            Finally
                connection.Close()
            End Try
        End Using

        Return transactionEntries.ToArray()
    End Function



    '' Control methods
    Private Sub Window_Sales_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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
        UpdateCommissionsListTable()
        FilterTransactionsByDateAndStatus(FilterTransactionsListAndSummaryStartDate, FilterTransactionsListAndSummaryEndDate, FilterTransactionsListAndSummaryStatus)
    End Sub

    Private Sub Button_NewSale_Click_1(sender As Object, e As EventArgs) Handles Button_NewSale.Click
        ShowSalesListAndSummaryFilterPrompt()
    End Sub

    Private Sub Button_Logout_Click(sender As Object, e As EventArgs) Handles Button_Logout.Click
        Dim result As DialogResult = MessageBox.Show("Are you sure you want to log out?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            Dim newForm As New Window_Login()
            newForm.Show()
            Me.Close()
        End If
    End Sub

    Private Sub Button_SalesListTablePageControlNext_Click_1(sender As Object, e As EventArgs) Handles Button_SalesListTablePageControlNext.Click
        If SalesListCurrentPage < SalesListTotalPages Then
            SalesListCurrentPage += 1
            UpdateSalesListTable()
        End If
    End Sub

    Private Sub Button_SalesListTablePageControlPrevious_Click_1(sender As Object, e As EventArgs) Handles Button_SalesListTablePageControlPrevious.Click
        If SalesListCurrentPage > 1 Then
            SalesListCurrentPage -= 1
            UpdateSalesListTable()
        End If
    End Sub

    Private Sub Button_SearchVendors_Click(sender As Object, e As EventArgs) Handles Button_SearchVendors.Click
        Dim searchText As String = Textbox_SearchBarSalesList.Text.Trim().ToLower()
    Dim filteredTransactions As List(Of TransactionEntry)

    ' Filter transactions based on the search text
    filteredTransactions = TransactionEntries.Where(Function(transaction)
        Dim customer = Customers.FirstOrDefault(Function(c) c.CustomerId = transaction.CustomerId)
        Dim account = Employees.FirstOrDefault(Function(a) a.EmployeeId = transaction.CreatedBy)

        Dim customerName = If(customer?.Name, "").ToLower()
        Dim createdByName = If(account.FirstName & " " & account.MiddleName & " " & account.LastName, "").ToLower()

        Return customerName.Contains(searchText) OrElse createdByName.Contains(searchText)
    End Function).ToList()

    FlowLayoutPanel_SalesListTable.Controls.Clear()

    SalesListTotalPages = Math.Ceiling(filteredTransactions.Count / SalesListItemsPerPage)
    Label_SalesListTablePageControlPageNumber.Text = $"{SalesListCurrentPage}/{SalesListTotalPages}"

    Dim TableLayoutPanel_SalesList As New TableLayoutPanel With {
        .AutoSize = True,
        .AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink,
        .BackColor = System.Drawing.Color.FromArgb(CType(255, Byte), CType(255, Byte), CType(255, Byte)),
        .CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single],
        .Margin = New System.Windows.Forms.Padding(0, 0, 0, 0),
        .MaximumSize = New System.Drawing.Size(1102, 0),
        .MinimumSize = New System.Drawing.Size(1102, 0)
    }

    TableLayoutPanel_SalesList.ColumnCount = 9
    For i = 1 To 8
        TableLayoutPanel_SalesList.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11!))
    Next

    Dim StartIndex As Integer = (SalesListCurrentPage - 1) * SalesListItemsPerPage
    Dim EndIndex As Integer = Math.Min(StartIndex + SalesListItemsPerPage, filteredTransactions.Count)

    For i = StartIndex To EndIndex - 1
        Dim transaction As TransactionEntry = filteredTransactions(i)
        TableLayoutPanel_SalesList.RowCount += 1

        Dim TransactionCustomer = Customers.FirstOrDefault(Function(c) c.CustomerId = transaction.CustomerId)
        Dim TransactionAccount = Employees.FirstOrDefault(Function(a) a.EmployeeId = transaction.CreatedBy)

        Dim labels As New List(Of Label)
        labels.Add(CreateLabel(transaction.TransactionId.ToString(), 2, True))
        labels.Add(CreateLabel(transaction.CustomerId & " - " & If(TransactionCustomer?.Name, "N/A"), 2))
        labels.Add(CreateLabel(transaction.TaxAmount.ToString(), 1))
        labels.Add(CreateLabel(transaction.NetTotal.ToString(), 1))
        labels.Add(CreateLabel(transaction.DueAmount.ToString(), 1))
        labels.Add(CreateLabel(transaction.DueDate.ToString(), 1))
        labels.Add(CreateLabel(transaction.Status, 1))
        labels.Add(CreateLabel(transaction.CreatedAt.ToString(), 1))
        labels.Add(CreateLabel(transaction.CreatedBy & " - " & If(TransactionAccount.FirstName & " " & TransactionAccount.MiddleName & " " & TransactionAccount.LastName, "N/A"), 1))

        For col As Integer = 0 To labels.Count - 1
            TableLayoutPanel_SalesList.Controls.Add(labels(col), col, TableLayoutPanel_SalesList.RowCount - 1)
        Next

        Dim maxLabelHeight As Integer = labels.Max(Function(lbl) lbl.Height)

        TableLayoutPanel_SalesList.RowStyles.Add(New RowStyle(SizeType.Absolute, maxLabelHeight + 12))
    Next

    FlowLayoutPanel_SalesListTable.Controls.Add(TableLayoutPanel_SalesList)
    End Sub



    Private Sub ShowOutvoiceReceiptPrompt(transactionId As Integer)
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

        Dim Transaction_Entry As TransactionEntry = Nothing

        Dim query As String = "SELECT * FROM transaction_entries WHERE transaction_id = @TransactionId"

        Using connection As New MySqlConnection(ConnectionString)
            Using command As New MySqlCommand(query, connection)
                command.Parameters.AddWithValue("@TransactionId", transactionId)

                Try
                    connection.Open()
                    Using reader As MySqlDataReader = command.ExecuteReader()
                        If reader.HasRows Then
                            While reader.Read()
                                Transaction_Entry = New TransactionEntry(
                                    reader.GetInt32("transaction_id"),
                                    If(IsDBNull(reader("customer_id")), 0, reader.GetInt32("customer_id")),
                                    reader.GetDecimal("subtotal"),
                                    reader.GetDecimal("net_total"),
                                    reader.GetDecimal("received_amount"),
                                    reader.GetDecimal("change_amount"),
                                    If(IsDBNull(reader("tax_id")), 0, reader.GetInt32("tax_id")),
                                    If(IsDBNull(reader("tax_amount")), 0D, reader.GetDecimal("tax_amount")),
                                    If(IsDBNull(reader("payment_term_id")), 0, reader.GetInt32("payment_term_id")),
                                    If(IsDBNull(reader("due_amount")), 0D, reader.GetDecimal("due_amount")),
                                    If(IsDBNull(reader("due_date")), Date.MinValue, reader.GetDateTime("due_date")),
                                    reader.GetString("status"),
                                    reader.GetDateTime("created_at"),
                                    If(IsDBNull(reader("created_by")), 0, reader.GetInt32("created_by"))
                                )
                            End While
                        End If
                    End Using
                Catch ex As Exception
                    MessageBox.Show("An error occurred while fetching the transaction entry: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End Using
        End Using


        Dim CurrentTransactionItems As New List(Of TransactionItem)()
        query = "SELECT * FROM transaction_items WHERE transaction_id = @TransactionId"

        Using connection As New MySqlConnection(ConnectionString)
            Using command As New MySqlCommand(query, connection)
                command.Parameters.AddWithValue("@TransactionId", transactionId)

                Try
                    connection.Open()
                    Using reader As MySqlDataReader = command.ExecuteReader()
                        While reader.Read()
                            Dim transactionItem As New TransactionItem(
                                reader.GetInt32("transaction_item_id"),
                                If(IsDBNull(reader("transaction_id")), 0, reader.GetInt32("transaction_id")),
                                If(IsDBNull(reader("item_id")), 0, reader.GetInt32("item_id")),
                                reader.GetInt32("transaction_item_quantity"),
                                reader.GetDecimal("transaction_item_selling_price"),
                                reader.GetDecimal("transaction_item_amount"),
                                If(IsDBNull(reader("stock_id")), 0, reader.GetInt32("stock_id"))
                            )
                            CurrentTransactionItems.Add(transactionItem)
                        End While
                    End Using
                Catch ex As Exception
                    MessageBox.Show("An error occurred while fetching transaction items: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End Using
        End Using

        Dim TransactionCustomer As New Customer
        Dim TransactionAccount As New Account

        For Each customer In Customers
            If customer.CustomerId = Transaction_Entry.CustomerId Then
                TransactionCustomer = customer
            End If
        Next

        For Each stock In CurrentTransactionItems
            Dim item = Items.FirstOrDefault(Function(i) i.ItemId = stock.ItemId)
            Dim itemName As String = If(item IsNot Nothing, item.ItemName, "Unknown Item")

            Dim qty As String = stock.TransactionItemQuantity.ToString().PadRight(20)
            Dim name As String = itemName.PadRight(30)
            Dim amount As String = stock.TransactionItemAmount.ToString("C").PadLeft(10)

            output.AppendLine($"{qty}{name}{amount}")
        Next
        
        For Each account In Accounts
            If account.AccountId = TransactionCustomer.CreatedBy Then
                TransactionAccount = account
            End If
        Next

        Transaction_Entry.CustomerId = TransactionCustomer.CustomerId

        Label3.Text = "Invoice ID: " & Transaction_Entry.TransactionId & Environment.NewLine & _
        "Customer: " & Transaction_Entry.CustomerId & " - "  & TransactionCustomer.Name &  Environment.NewLine & _
        "Due Date: " & Transaction_Entry.DueDate &  Environment.NewLine & _
        "Created By: " & Transaction_Entry.CreatedBy  & " - "  & TransactionAccount.Name & Environment.NewLine & _
        "Created At: " & DateTime.Now & Environment.NewLine
        Label5.Text = output.ToString()
        Label8.Text = "Net Total: " & Transaction_Entry.NetTotal & Environment.NewLine & _
        "Tax Amount: " & Transaction_Entry.TaxAmount & Environment.NewLine & _
        "Grand Total: " & Transaction_Entry.NetTotal & Environment.NewLine & _
        "Due Amount: " & Transaction_Entry.DueAmount & Environment.NewLine & _
        "Change Amount: " & Transaction_Entry.ChangeAmount & Environment.NewLine

        Prompt_OutvoiceReceipt.ShowDialog(Me)
    End Sub
End Class