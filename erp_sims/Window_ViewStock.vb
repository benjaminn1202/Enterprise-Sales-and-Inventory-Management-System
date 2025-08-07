Imports MySql.Data.MySqlClient


Public Class Window_ViewStock
    Public ConnectionString = "server=localhost;user id=root;password=benjaminn1202;database=erp_sims_db"
    Public Dim SelectedStockToView = 1
    Private currentPage As Integer = 1
    Private rowsPerPage As Integer = 5
    Private totalRows As Integer = 0

    Private Sub Window_ViewStock_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label_PageHeaderViewStock.Text = "Viewing Stock " & SelectedStockToView
        ViewStock(SelectedStockToView)
        LoadStockEntriesViewStock(SelectedStockToView)
    End Sub

    Private Sub ViewStock(StockID)
        Using conn As New MySqlConnection(ConnectionString)
            conn.Open()

            Dim query As String = "SELECT * FROM stocks WHERE stock_id=" & StockID
            Dim cmd As New MySqlCommand(query, conn)

            Dim BrandID = ""
            Dim brand = ""
            Dim CategoryID = ""
            Dim Category = ""
            Dim CreatedByID = ""
            Dim CreatedBy = ""

            Using reader As MySqlDataReader = cmd.ExecuteReader()
                reader.Read()
                Dim StockName = reader("stock_name")
                Dim StockQuantity = reader("stock_quantity")
                BrandID = reader("brand_id")
                CategoryID = reader("category_id")
                CreatedByID = reader("created_by")
                Dim CreatedAt = reader("created_at")
                Dim StockSellingPrice = reader("stock_selling_price")

                Textbox_StockDetailStockIDInputViewStock.Text = StockID
                Textbox_StockDetailsStockNameInputViewStock.Text = StockName
                Textbox_StockDetailStockQuantityInputViewStock.Text = StockQuantity
                Textbox_StockDetailsSellingPriceInputViewStock.Text = StockSellingPrice
                Textbox_CreationDetailCustomerCreatedAtInputViewStock.Text = CreatedAt
            End Using
                
            query = "SELECT brand_name FROM brands WHERE brand_id=" & BrandID
            cmd = New  MySqlCommand(query, conn)

            Using reader As MySqlDataReader = cmd.ExecuteReader()
                reader.Read()
                Brand = reader("brand_name")
                Textbox_StockDetailsBrandInputViewStock.Text = Brand
            End Using

            query = "SELECT category_name FROM categories WHERE category_id=" & CategoryID
            cmd = New  MySqlCommand(query, conn)

            Using reader As MySqlDataReader = cmd.ExecuteReader()
                reader.Read()
                Category = reader("category_name")
                Textbox_StockDetailsCategoryInputViewStock.Text = Category
            End Using

            query = "SELECT email FROM accounts WHERE account_id=" & CreatedByID
            cmd = New  MySqlCommand(query, conn)

            Using reader As MySqlDataReader = cmd.ExecuteReader()
                reader.Read()
                CreatedBy = reader("email")
                Textbox_CreationDetailCustomerCreatedByInputViewStock.Text = CreatedBy
            End Using
        End Using
    End Sub

    Private Sub LoadStockEntriesViewStock(stock_id As Integer)
        Dim query As String = "SELECT COUNT(*) FROM stock_entries WHERE stock_id = @StockID"
        Dim conn As New MySqlConnection(ConnectionString)
        Dim cmd As New MySqlCommand(query, conn)
        cmd.Parameters.AddWithValue("@StockID", stock_id)
    
        conn.Open()
        totalRows = Convert.ToInt32(cmd.ExecuteScalar())
        conn.Close()

        Dim totalPages As Integer = Math.Ceiling(totalRows / Convert.ToDouble(rowsPerPage))

        Label_StockEntriesListTableViewStockPageControlPageNumber.Text = $"{totalPages} / {currentPage}"
        Label_StockEntriesListTableViewStockPageControlItemNumber.Text = $"{totalRows - ((currentPage - 1) * rowsPerPage)} rows remaining"

        Dim startRow As Integer = (currentPage - 1) * rowsPerPage
        query = "SELECT stock_entry_id, stock_entry_type, vendor_id, item_buying_price, quantity, total_buying_price, created_by, created_at FROM stock_entries WHERE stock_id = @StockID LIMIT @StartRow, @RowsPerPage"

        cmd = New MySqlCommand(query, conn)
        cmd.Parameters.AddWithValue("@StockID", stock_id)
        cmd.Parameters.AddWithValue("@StartRow", startRow)
        cmd.Parameters.AddWithValue("@RowsPerPage", rowsPerPage)

        conn.Open()
        Dim reader As MySqlDataReader = cmd.ExecuteReader()

        FlowLayoutPanel_StockEntriesListTableViewStock.SuspendLayout()
        FlowLayoutPanel_StockEntriesListTableViewStock.Controls.Clear()

        Dim headerRow As New TableLayoutPanel With {
            .AutoSize = True,
            .AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink,
            .BackColor = System.Drawing.Color.FromArgb(CType(CType(241, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(249, Byte), Integer)),
            .CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single,
            .ColumnCount = 8,
            .Margin = New Padding(0, 0, 0, 0),
            .MaximumSize = New Drawing.Size(1083, 0),
            .MinimumSize = New Drawing.Size(1083, 0),
            .RowCount = 1,
            .Size = New Drawing.Size(1083, 58)
        }
        For i As Integer = 0 To 7
            headerRow.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5!))
        Next
        FlowLayoutPanel_StockEntriesListTableViewStock.Controls.Add(headerRow)

        Dim headerLabels As String() = {"Stock Entry ID", "Stock Entry Type", "Vendor", "Buying Price", "Quantity", "Total Price", "Created By", "Created At"}
        For Each labelText As String In headerLabels
            Dim headerLabel As New Label With {
                .Anchor = System.Windows.Forms.AnchorStyles.Left,
                .AutoSize = True,
                .BackColor = System.Drawing.Color.FromArgb(0, 0, 0, 0),
                .Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, 0),
                .ForeColor = System.Drawing.Color.FromArgb(2, 34, 34, 34),
                .ImageAlign = System.Drawing.ContentAlignment.MiddleLeft,
                .Margin = New Padding(12),
                .MaximumSize = New Drawing.Size(111, 0),
                .Size = New Drawing.Size(110, 32),
                .TabIndex = 17,
                .TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                .Text = labelText
            }
            headerRow.Controls.Add(headerLabel)
        Next

        While reader.Read()
            Dim row As New TableLayoutPanel With {
                .AutoSize = True,
                .AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink,
                .BackColor = System.Drawing.Color.White,
                .CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single,
                .ColumnCount = 8,
                .Margin = New Padding(0, 0, 0, 0),
                .MaximumSize = New Drawing.Size(1083, 0),
                .MinimumSize = New Drawing.Size(1083, 0),
                .RowCount = 1,
                .Size = New Drawing.Size(1083, 58)
            }
            For i As Integer = 0 To 7
                row.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5!))
            Next

            FlowLayoutPanel_StockEntriesListTableViewStock.Controls.Add(row)

            Dim stockEntryValues As String() = {
                reader("stock_entry_id").ToString(),
                reader("stock_entry_type").ToString(),
                reader("vendor_id").ToString(),
                reader("item_buying_price").ToString(),
                reader("quantity").ToString(),
                reader("total_buying_price").ToString(),
                reader("created_by").ToString(),
                reader("created_at").ToString()
            }

            For Each value As String In stockEntryValues
                Dim rowLabel As New Label With {
                    .Anchor = System.Windows.Forms.AnchorStyles.Left,
                    .AutoSize = True,
                    .BackColor = System.Drawing.Color.FromArgb(0, 0, 0, 0),
                    .Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, 0),
                    .ForeColor = System.Drawing.Color.FromArgb(2, 34, 34, 34),
                    .ImageAlign = System.Drawing.ContentAlignment.MiddleLeft,
                    .Margin = New Padding(12),
                    .MaximumSize = New Drawing.Size(111, 0),
                    .Size = New Drawing.Size(110, 32),
                    .TabIndex = 17,
                    .TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                    .Text = value
                }
                row.Controls.Add(rowLabel)
            Next
        End While

        FlowLayoutPanel_StockEntriesListTableViewStock.ResumeLayout()

        reader.Close()
        conn.Close()
    End Sub

    Private Sub Button_StockEntriesListTableViewStockPageControlPrevious_Click(sender As Object, e As EventArgs) Handles Button_StockEntriesListTableViewStockPageControlPrevious.Click
        If currentPage > 1 Then
            currentPage -= 1
            LoadStockEntriesViewStock(SelectedStockToView)
        End If
    End Sub

    Private Sub Button_StockEntriesListTableViewStockPageControlNext_Click(sender As Object, e As EventArgs) Handles Button_StockEntriesListTableViewStockPageControlNext.Click
        Dim totalPages As Integer = Math.Ceiling(totalRows / Convert.ToDouble(rowsPerPage))
        If currentPage < totalPages Then
            currentPage += 1
            LoadStockEntriesViewStock(SelectedStockToView)
        End If
    End Sub

End Class