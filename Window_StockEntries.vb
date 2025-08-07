Imports Guna.UI2.WinForms
Imports MySql.Data.MySqlClient

Public Class Window_StockEntries
    Public ConnectionString = "server=localhost;user id=root;password=benjaminn1202;database=erp_sims_db"


    Private Sub Window_StockEntries_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        '' PopulateCustomerList()
    End Sub

    Private Sub PopulateCustomerList()
        Using connection As New MySqlConnection(ConnectionString)
            Dim query As String = "SELECT stock_entry_id, stock_entry_type, stock_id, stock_id, vendor_id, quantity, buying_price, total_buying_price, created_at, created_by FROM stocks"
            Dim command As New MySqlCommand(query, connection)

            Try
                connection.Open()
                Using reader As MySqlDataReader = command.ExecuteReader()
                    While reader.Read()
                        Dim rowPanel As New TableLayoutPanel With {
                            .AutoSize = True,
                            .AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink,
                            .BackColor = System.Drawing.Color.White,
                            .CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single],
                            .ColumnCount = 11,
                            .Margin = New System.Windows.Forms.Padding(0),
                            .MinimumSize = New System.Drawing.Size(1102, 0),
                            .RowCount = 1,
                            .Size = New System.Drawing.Size(1102, 58)
                        }
                        rowPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 108!))
                        rowPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 102!))
                        rowPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 102!))
                        rowPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 102!))
                        rowPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 102!))
                        rowPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 102!))
                        rowPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 102!))
                        rowPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 102!))
                        rowPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 102!))
                        rowPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 102!))
                        rowPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85!))

                        Dim stockEntryIDLabel = CreateLabel(reader("stock_entry_id").ToString(), New System.Drawing.Size(84, 0))
                        Dim stockEntryType = CreateLabel(reader("stock_entry_type"), New System.Drawing.Size(156, 0))
                        Dim stockID = CreateLabel(reader("stock_id").ToString(), New System.Drawing.Size(156, 0))
                        Dim StockName = CreateLabel(reader("stock_id").ToString(), New System.Drawing.Size(156, 0))
                        Dim Vendor = CreateLabel(reader("vendor_id").ToString(), New System.Drawing.Size(142, 0))
                        Dim EntryQuantity = CreateLabel(reader("quantity"), New System.Drawing.Size(156, 0))
                        Dim StockBuyingPrice = CreateLabel(reader("buying_price"), New System.Drawing.Size(156, 0))
                        Dim TotalBuyingPrice = CreateLabel(reader("total_buying_price"), New System.Drawing.Size(156, 0))
                        Dim DateCreated = CreateLabel(reader("created_at"), New System.Drawing.Size(156, 0))
                        Dim CreatedBy = CreateLabel(reader("created_by"), New System.Drawing.Size(156, 0))

                        rowPanel.Controls.Add(stockEntryIDLabel, 0, 0)
                        rowPanel.Controls.Add(stockEntryType, 1, 0)
                        rowPanel.Controls.Add(stockID, 2, 0)
                        rowPanel.Controls.Add(StockName, 3, 0)
                        rowPanel.Controls.Add(Vendor, 4, 0)
                        rowPanel.Controls.Add(EntryQuantity, 5, 0)
                        rowPanel.Controls.Add(StockBuyingPrice, 6, 0)
                        rowPanel.Controls.Add(TotalBuyingPrice, 7, 0)
                        rowPanel.Controls.Add(DateCreated, 8, 0)
                        rowPanel.Controls.Add(CreatedBy, 9, 0)

                        Dim moreControlsContainer As New FlowLayoutPanel With {
                            .AutoSize = True,
                            .AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink,
                            .Margin = New System.Windows.Forms.Padding(12, 12, 0, 0),
                            .Size = New System.Drawing.Size(34, 32)
                        }

                        Dim viewButton As New Guna2Button With {
                            .BackgroundImage = Global.ERP_SIMS.My.Resources.Resources.Button_TableView,
                            .FillColor = System.Drawing.Color.FromArgb(0, 0, 0, 0),
                            .Font = New System.Drawing.Font("Segoe UI", 9!),
                            .ForeColor = System.Drawing.Color.White,
                            .Margin = New System.Windows.Forms.Padding(0, 0, 0, 0),
                            .Size = New System.Drawing.Size(34, 32)
                        }

                        moreControlsContainer.Controls.Add(viewButton)
                        rowPanel.Controls.Add(moreControlsContainer, 6, 0)

                        FlowLayoutPanel_StockEntriesListTable.Controls.Add(rowPanel)
                    End While
                End Using
            Catch ex As MySqlException
                MessageBox.Show($"Error loading stock entry data: {ex.Message}")
            End Try
        End Using
    End Sub

    Private Function CreateLabel(text As String, maxSize As System.Drawing.Size) As Label
        Return New Label With {
            .Anchor = System.Windows.Forms.AnchorStyles.Left,
            .AutoSize = True,
            .BackColor = System.Drawing.Color.FromArgb(0, 0, 0, 0),
            .Font = New System.Drawing.Font("Microsoft JhengHei", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, 0),
            .ForeColor = System.Drawing.Color.FromArgb(2, 34, 34, 34),
            .ImageAlign = System.Drawing.ContentAlignment.MiddleLeft,
            .Margin = New System.Windows.Forms.Padding(12),
            .Size = New System.Drawing.Size(110, 32),
            .TabIndex = 17,
            .TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
            .MaximumSize = maxSize,
            .Text = text
        }
    End Function

Private Sub NavigateToForm(targetForm As Form, currentForm As Form)
targetForm.Show()
currentForm.Close()
End Sub

Private Sub Button_NavigationItemSaleScreen_Click(sender As Object, e As EventArgs) Handles Button_NavigationItemSaleScreen.Click
NavigateToForm(New SaleScreenDraft(), Me)
End Sub

Private Sub Button_NavigationItemSales_Click(sender As Object, e As EventArgs) Handles Button_NavigationItemSales.Click
NavigateToForm(New Window_Sales(), Me)
End Sub

Private Sub Button_NavigationItemCustomers_Click(sender As Object, e As EventArgs) Handles Button_NavigationItemCustomers.Click
NavigateToForm(New Window_Customers(), Me)
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

End Class