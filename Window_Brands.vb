Imports MySql.Data.MySqlClient

Public Class Window_Brands
    Public ConnectionString = "server=localhost;user id=root;password=benjaminn1202;database=erp_sims_db"
    
    Private Sub Window_Brands_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DisplayBrandRows(1)
    End Sub

    Private Sub DisplayBrandRows(pageNumber As Integer)
    FlowLayoutPanel_BrandsListTable.Controls.Clear()

    Dim headerPanel As New TableLayoutPanel With {
        .AutoSize = True,
        .AutoSizeMode = AutoSizeMode.GrowAndShrink,
        .BackColor = Color.FromArgb(241, 245, 249),
        .CellBorderStyle = TableLayoutPanelCellBorderStyle.[Single],
        .ColumnCount = 5,
        .Margin = New Padding(0),
        .MinimumSize = New Size(1102, 0),
        .RowCount = 1,
        .Size = New Size(1102, 58)
    }
    headerPanel.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 100!))
    headerPanel.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100!))
    headerPanel.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 224!))
    headerPanel.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 224!))
    headerPanel.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 70!))
    headerPanel.RowStyles.Add(New RowStyle(SizeType.Percent, 100!))

    Dim headers = {"Brand ID", "Brand Name", "Created By", "Created At", "More"}
    For i As Integer = 0 To headers.Length - 1
        headerPanel.Controls.Add(New Label With {
            .Anchor = AnchorStyles.Left,
            .AutoSize = True,
            .Font = New Font("Microsoft JhengHei", 12!, FontStyle.Bold, GraphicsUnit.Pixel),
            .ForeColor = Color.FromArgb(2, 34, 34, 34),
            .Margin = New Padding(12),
            .TextAlign = ContentAlignment.MiddleLeft,
            .Text = headers(i)
        }, i, 0)
    Next
    FlowLayoutPanel_BrandsListTable.Controls.Add(headerPanel)

    Dim pageSize As Integer = 5
    Dim offset As Integer = (pageNumber - 1) * pageSize

    Using conn As New MySqlConnection(ConnectionString)
        Dim cmd As New MySqlCommand("SELECT brand_id, brand_name, created_by, created_at FROM brands ORDER BY brand_id LIMIT @Offset, @PageSize", conn)
        cmd.Parameters.AddWithValue("@Offset", offset)
        cmd.Parameters.AddWithValue("@PageSize", pageSize)
        conn.Open()

        Using reader As MySqlDataReader = cmd.ExecuteReader()
            While reader.Read()
                Dim rowPanel As New TableLayoutPanel With {
                    .AutoSize = True,
                    .AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    .BackColor = Color.White,
                    .CellBorderStyle = TableLayoutPanelCellBorderStyle.[Single],
                    .ColumnCount = 5,
                    .Margin = New Padding(0),
                    .MinimumSize = New Size(1102, 0),
                    .RowCount = 1,
                    .Size = New Size(1102, 58)
                }
                rowPanel.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 100!))
                rowPanel.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100!))
                rowPanel.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 224!))
                rowPanel.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 224!))
                rowPanel.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 70!))
                rowPanel.RowStyles.Add(New RowStyle(SizeType.Percent, 100!))

                Dim labels = {reader("brand_id").ToString(), reader("brand_name").ToString(), reader("created_by").ToString(), reader("created_at").ToString()}
                Dim maxSizes = {New Size(76, 0), New Size(878, 0), New Size(200, 0), New Size(200, 0)}
                Dim brand_id = reader("brand_id")

                For i As Integer = 0 To 3
                    rowPanel.Controls.Add(New Label With {
                        .Anchor = AnchorStyles.Left,
                        .AutoSize = True,
                        .BackColor = Color.Transparent,
                        .Font = New Font("Microsoft JhengHei", 12!, FontStyle.Bold, GraphicsUnit.Pixel),
                        .ForeColor = Color.FromArgb(2, 34, 34, 34),
                        .Margin = New Padding(12),
                        .Size = New Size(110, 32),
                        .TabIndex = 17,
                        .TextAlign = ContentAlignment.MiddleLeft,
                        .MaximumSize = maxSizes(i),
                        .Text = labels(i)
                    }, i, 0)
                Next

                Dim moreControlsContainer As New Panel With {
                    .Anchor = AnchorStyles.Left,
                    .AutoSize = True,
                    .AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    .Margin = New Padding(12, 0, 0, 0),
                    .Size = New Size(34, 32)
                }
                Dim editButton As New Guna.UI2.WinForms.Guna2Button With {
                    .BackgroundImage = Global.ERP_SIMS.My.Resources.Resources.Button_TableEdit,
                    .FillColor = Color.Transparent,
                    .Font = New Font("Segoe UI", 9!),
                    .ForeColor = Color.White,
                    .Margin = New Padding(0),
                    .Size = New Size(34, 32)
                }
                AddHandler editButton.Click, Sub() ShowEditBrandPanel(brand_id)
                moreControlsContainer.Controls.Add(editButton)
                rowPanel.Controls.Add(moreControlsContainer, 4, 0)
                FlowLayoutPanel_BrandsListTable.Controls.Add(rowPanel)
            End While
        End Using
    End Using

    Label_BrandsListTablePageControlPageNumber.Text = pageNumber.ToString()
End Sub

Private Sub Button_BrandsListTablePageControlPrevious_Click(sender As Object, e As EventArgs) Handles Button_BrandsListTablePageControlPrevious.Click
    Dim pageTextParts As String() = Label_BrandsListTablePageControlPageNumber.Text.Split(" "c)
    Dim currentPage As Integer

    If pageTextParts.Length > 1 AndAlso Integer.TryParse(pageTextParts(1), currentPage) Then
        If currentPage > 1 Then DisplayBrandRows(currentPage - 1)
    Else
        MessageBox.Show("Page number could not be determined.")
    End If
End Sub

Private Sub Button_BrandsListTablePageControlNext_Click(sender As Object, e As EventArgs) Handles Button_BrandsListTablePageControlNext.Click
    Dim pageTextParts As String() = Label_BrandsListTablePageControlPageNumber.Text.Split(" "c)
    Dim currentPage As Integer

    If pageTextParts.Length > 1 AndAlso Integer.TryParse(pageTextParts(1), currentPage) Then
        DisplayBrandRows(currentPage + 1)
    Else
        MessageBox.Show("Page number could not be determined.")
    End If
End Sub



    Private Sub ShowEditBrandPanel(TargetBrandID)

    End Sub

    Private Sub Button_NewBrand_Click(sender As Object, e As EventArgs) Handles Button_NewBrand.Click
        Dim Window_NewBrand As New Window_NewBrand()
        Window_NewBrand.Show()
        Me.Close()
    End Sub

    Private Sub Button_NavigationItemCustomers_Click(sender As Object, e As EventArgs) Handles Button_NavigationItemCustomers.Click
        Dim Customers As New Window_Customers()
        Window_Customers.Show()
        Me.Close()
    End Sub

    Private Sub Button_NavigationItemSales_Click(sender As Object, e As EventArgs) Handles Button_NavigationItemSales.Click
        Dim Window_Sales As New Window_Sales()
        Window_Sales.Show()
        Me.Close()
    End Sub

    Private Sub Button_NavigationItemStockEntries_Click(sender As Object, e As EventArgs) Handles Button_NavigationItemStockEntries.Click
        Dim Window_StockEntries As New Window_StockEntries()
        Window_StockEntries.Show()
        Me.Close()
    End Sub

    Private Sub Button_NavigationItemStocks_Click(sender As Object, e As EventArgs) Handles Button_NavigationItemStocks.Click
        Dim Window_Stocks As New Window_Stocks()
        Window_Stocks.Show()
        Me.Close()
    End Sub

    Private Sub Button_NavigationItemCategories_Click(sender As Object, e As EventArgs) Handles Button_NavigationItemCategories.Click
        Dim Categories As New Window_Categories()
        Window_Categories.Show()
        Me.Close()
    End Sub

    Private Sub Button_NavigationItemVendors_Click(sender As Object, e As EventArgs) Handles Button_NavigationItemVendors.Click
        Dim Window_Vendors As New Window_Vendors()
        Window_Vendors.Show()
        Me.Close()
    End Sub
End Class