Imports MySql.Data.MySqlClient

Public Class Window_NewStock
    Public ConnectionString = "server=localhost;user id=root;password=benjaminn1202;database=erp_sims_db"

    Private Sub Button_MakeStockNewStock_Click(sender As Object, e As EventArgs) Handles Button_MakeStockNewStock.Click
        Dim isValid As Boolean = True

        If String.IsNullOrWhiteSpace(Textbox_StockDetailsStockNameInputNewStock.Text) Then
            Textbox_StockDetailsStockNameInputNewStock.FillColor = Color.FromArgb(255, 191, 189)
            isValid = False
        Else
            Textbox_StockDetailsStockNameInputNewStock.FillColor = Color.FromArgb(235, 225, 255)
        End If

        If Not Integer.TryParse(Textbox_StockDetailStockQuantityInputNewStock.Text, Nothing) Then
            Textbox_StockDetailStockQuantityInputNewStock.FillColor = Color.FromArgb(255, 191, 189)
            isValid = False
        Else
            Textbox_StockDetailStockQuantityInputNewStock.FillColor = Color.FromArgb(235, 225, 255)
        End If

        If Not Decimal.TryParse(Textbox_StockDetailStockBuyingPriceInputNewStock.Text, Nothing) Then
            Textbox_StockDetailStockBuyingPriceInputNewStock.FillColor = Color.FromArgb(255, 191, 189)
            isValid = False
        Else
            Textbox_StockDetailStockBuyingPriceInputNewStock.FillColor = Color.FromArgb(235, 225, 255)
        End If

        If Not Decimal.TryParse(Textbox_StockDetailsSellingPriceInputNewStock.Text, Nothing) Then
            Textbox_StockDetailsSellingPriceInputNewStock.FillColor = Color.FromArgb(255, 191, 189)
            isValid = False
        Else
            Textbox_StockDetailsSellingPriceInputNewStock.FillColor = Color.FromArgb(235, 225, 255)
        End If

        If GetIdFromAutocomplete(Textbox_StockDetailsBrandInputNewStock.Text, "brands") = 0 Then
            Textbox_StockDetailsBrandInputNewStock.FillColor = Color.FromArgb(255, 191, 189)
            isValid = False
        Else
            Textbox_StockDetailsBrandInputNewStock.FillColor = Color.FromArgb(235, 225, 255)
        End If

        If GetIdFromAutocomplete(Textbox_StockDetailsCategoryInputNewStock.Text, "categories") = 0 Then
            Textbox_StockDetailsCategoryInputNewStock.FillColor = Color.FromArgb(255, 191, 189)
            isValid = False
        Else
            Textbox_StockDetailsCategoryInputNewStock.FillColor = Color.FromArgb(235, 225, 255)
        End If

        If GetIdFromAutocomplete(Textbox_StockDetailsVendorInputNewStock.Text, "vendors") = 0 Then
            Textbox_StockDetailsVendorInputNewStock.FillColor = Color.FromArgb(255, 191, 189)
            isValid = False
        Else
            Textbox_StockDetailsVendorInputNewStock.FillColor = Color.FromArgb(235, 225, 255)
        End If

        If Not isValid Then
            MessageBox.Show("Stock entry failed.")
            Return
        End If

        Dim stockName As String = Textbox_StockDetailsStockNameInputNewStock.Text
        Dim stockQuantity As Integer = Integer.Parse(Textbox_StockDetailStockQuantityInputNewStock.Text)
        Dim buyingPrice As Decimal = Decimal.Parse(Textbox_StockDetailStockBuyingPriceInputNewStock.Text)
        Dim sellingPrice As Decimal = Decimal.Parse(Textbox_StockDetailsSellingPriceInputNewStock.Text)
        Dim totalBuyingPrice As Decimal = stockQuantity * buyingPrice
        Dim createdBy As Integer = 1
        Dim createdAt As DateTime = DateTime.Now
        Dim brandId As Integer = GetIdFromAutocomplete(Textbox_StockDetailsBrandInputNewStock.Text, "brands")
        Dim categoryId As Integer = GetIdFromAutocomplete(Textbox_StockDetailsCategoryInputNewStock.Text, "categories")
        Dim vendorId As Integer = GetIdFromAutocomplete(Textbox_StockDetailsVendorInputNewStock.Text, "vendors")

        Using conn As New MySqlConnection(ConnectionString)
            conn.Open()
            Dim stockId As Integer

            Dim insertStockCmd As New MySqlCommand("INSERT INTO stocks (stock_name, stock_quantity, stock_selling_price, brand_id, category_id, created_by, created_at) VALUES (@stockName, @stockQuantity, @sellingPrice, @brandId, @categoryId, @createdBy, @createdAt)", conn)
            insertStockCmd.Parameters.AddWithValue("@stockName", stockName)
            insertStockCmd.Parameters.AddWithValue("@stockQuantity", stockQuantity)
            insertStockCmd.Parameters.AddWithValue("@sellingPrice", sellingPrice)
            insertStockCmd.Parameters.AddWithValue("@brandId", brandId)
            insertStockCmd.Parameters.AddWithValue("@categoryId", categoryId)
            insertStockCmd.Parameters.AddWithValue("@createdBy", createdBy)
            insertStockCmd.Parameters.AddWithValue("@createdAt", createdAt)
            insertStockCmd.ExecuteNonQuery()
            stockId = CInt(insertStockCmd.LastInsertedId)

            Dim insertEntryCmd As New MySqlCommand("INSERT INTO stock_entries (stock_id, quantity, item_buying_price, total_buying_price, vendor_id, created_by, created_at, stock_entry_type) VALUES (@stockId, @quantity, @buyingPrice, @totalBuyingPrice, @vendorId, @createdBy, @createdAt, @stockEntryType)", conn)
            insertEntryCmd.Parameters.AddWithValue("@stockId", stockId)
            insertEntryCmd.Parameters.AddWithValue("@quantity", stockQuantity)
            insertEntryCmd.Parameters.AddWithValue("@buyingPrice", buyingPrice)
            insertEntryCmd.Parameters.AddWithValue("@totalBuyingPrice", totalBuyingPrice)
            insertEntryCmd.Parameters.AddWithValue("@vendorId", vendorId)
            insertEntryCmd.Parameters.AddWithValue("@createdBy", createdBy)
            insertEntryCmd.Parameters.AddWithValue("@createdAt", createdAt)
            insertEntryCmd.Parameters.AddWithValue("@stockEntryType", "New Stock")
            insertEntryCmd.ExecuteNonQuery()

            MessageBox.Show("Stock entry succeeded.")
        End Using
    End Sub

    Private Function GetIdFromAutocomplete(value As String, tableName As String) As Integer
        Dim id As Integer = 0
        Using conn As New MySqlConnection(ConnectionString)
            conn.Open()

            Dim idColumnName As String
            Dim nameColumn As String

            Select Case tableName
                Case "stocks"
                    idColumnName = "stock_id"
                    nameColumn = "stock_name"
                Case "brands"
                    idColumnName = "brand_id"
                    nameColumn = "brand_name"
                Case "categories"
                    idColumnName = "category_id"
                    nameColumn = "category_name"
                Case "vendors"
                    idColumnName = "vendor_id"
                    nameColumn = "vendor_name"
                Case Else
                    MessageBox.Show("Invalid table name specified.")
                    Return 0
            End Select

            Dim query As String = $"SELECT {idColumnName} FROM {tableName} WHERE CONCAT('(', {idColumnName}, ') ', {nameColumn}) = @value"
            Dim cmd As New MySqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@value", value)

            Dim result = cmd.ExecuteScalar()

            If result IsNot Nothing Then
                id = CInt(result)
            Else
                MessageBox.Show("The entered value does not match any existing records. Please select or enter a valid value.")
            End If
        End Using
        Return id
    End Function

    Private Sub PopulateBrandAutoComplete()
        Dim brandAutoCompleteCollection As New AutoCompleteStringCollection()

        Using conn As New MySqlConnection(ConnectionString)
            conn.Open()

            Dim query As String = "SELECT brand_id, brand_name FROM brands"
            Dim cmd As New MySqlCommand(query, conn)

            Using reader As MySqlDataReader = cmd.ExecuteReader()
                While reader.Read()
                    Dim formattedValue As String = String.Format("({0}) {1}", reader("brand_id"), reader("brand_name"))

                    brandAutoCompleteCollection.Add(formattedValue)
                End While
            End Using
        End Using

        Textbox_StockDetailsBrandInputNewStock.AutoCompleteCustomSource = brandAutoCompleteCollection
    End Sub

    Private Sub PopulateCategoryAutoComplete()
        Dim categoryAutoCompleteCollection As New AutoCompleteStringCollection()

        Using conn As New MySqlConnection(ConnectionString)
            conn.Open()

            Dim query As String = "SELECT category_id, category_name FROM categories"
            Dim cmd As New MySqlCommand(query, conn)

            Using reader As MySqlDataReader = cmd.ExecuteReader()
                While reader.Read()
                    Dim formattedValue As String = String.Format("({0}) {1}", reader("category_id"), reader("category_name"))

                    categoryAutoCompleteCollection.Add(formattedValue)
                End While
            End Using
        End Using

        Textbox_StockDetailsCategoryInputNewStock.AutoCompleteCustomSource = categoryAutoCompleteCollection
    End Sub

    Private Sub PopulateVendorAutoComplete()
        Dim vendorAutoCompleteCollection As New AutoCompleteStringCollection()

        Using conn As New MySqlConnection(ConnectionString)
            conn.Open()

            Dim query As String = "SELECT vendor_id, vendor_name FROM vendors"
            Dim cmd As New MySqlCommand(query, conn)

            Using reader As MySqlDataReader = cmd.ExecuteReader()
                While reader.Read()
                    Dim formattedValue As String = String.Format("({0}) {1}", reader("vendor_id"), reader("vendor_name"))

                    vendorAutoCompleteCollection.Add(formattedValue)
                End While
            End Using
        End Using

        Textbox_StockDetailsVendorInputNewStock.AutoCompleteCustomSource = vendorAutoCompleteCollection
    End Sub

    Private Sub Window_NewStock_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PopulateVendorAutoComplete()
        PopulateCategoryAutoComplete()
        PopulateBrandAutoComplete()
    End Sub

    Private Sub Textbox_StockDetailStockQuantityInputNewStock_TextChanged(sender As Object, e As EventArgs) Handles Textbox_StockDetailStockQuantityInputNewStock.TextChanged
        UpdateStockBuyingPriceSummary()
    End Sub

    Private Sub Textbox_StockDetailStockBuyingPriceInputNewStock_TextChanged(sender As Object, e As EventArgs) Handles Textbox_StockDetailStockBuyingPriceInputNewStock.TextChanged
        UpdateStockBuyingPriceSummary()
    End Sub

    Private Sub UpdateStockBuyingPriceSummary()
        Dim quantity As Integer
        Dim buyingPrice As Decimal

        If Integer.TryParse(Textbox_StockDetailStockQuantityInputNewStock.Text, quantity) Then
            Label_StockDetailStockBuyingPriceSummaryTextNewStock.Text = "Quantity: " & quantity.ToString()
        Else
            Label_StockDetailStockBuyingPriceSummaryTextNewStock.Text = "Quantity: No value"
        End If

        Label_StockDetailStockBuyingPriceSummaryTextNewStock.Text &= Environment.NewLine

        If Decimal.TryParse(Textbox_StockDetailStockBuyingPriceInputNewStock.Text, buyingPrice) Then
            Label_StockDetailStockBuyingPriceSummaryTextNewStock.Text &= "Stock Buying Price: " & buyingPrice.ToString("C")
        Else
            Label_StockDetailStockBuyingPriceSummaryTextNewStock.Text &= "Stock Buying Price: No value"
        End If

        Label_StockDetailStockBuyingPriceSummaryTextNewStock.Text &= Environment.NewLine

        If quantity > 0 AndAlso buyingPrice > 0 Then
            Dim totalBuyingPrice As Decimal = quantity * buyingPrice
            Label_StockDetailStockBuyingPriceSummaryTextNewStock.Text &= "Total Buying Price: " & totalBuyingPrice.ToString("C")
        Else
            Label_StockDetailStockBuyingPriceSummaryTextNewStock.Text &= "Total Buying Price: No value"
        End If
    End Sub

    Private Sub Textbox_StockDetailsVendorInputNewStock_TextChanged(sender As Object, e As EventArgs) Handles Textbox_StockDetailsVendorInputNewStock.TextChanged
        UpdateVendorDetailsSummary()
    End Sub

    Private Sub UpdateVendorDetailsSummary()
        Dim vendorDetails As String = Textbox_StockDetailsVendorInputNewStock.Text
        Dim vendorId As Integer = GetVendorIdFromAutocomplete(vendorDetails)

        If vendorId = 0 Then
            Label_StockDetailsVendorDetailsSummaryTextNewStock.Text = "Vendor ID: No value" & vbCrLf &
                                                                  "Contact Person: No value" & vbCrLf &
                                                                  "Email: No value" & vbCrLf &
                                                                  "Phone: No value" & vbCrLf &
                                                                  "Address: No value"
            Return
        End If

        Using conn As New MySqlConnection(ConnectionString)
            conn.Open()

            Dim query As String = "SELECT vendor_id, first_name, middle_name, last_name, email, phone, address_line, city, zip_code, country FROM vendors WHERE vendor_id = @vendorId"
            Dim cmd As New MySqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@vendorId", vendorId)

            Using reader As MySqlDataReader = cmd.ExecuteReader()
                If reader.Read() Then
                    Dim contactPerson As String = reader("first_name") & " " & reader("last_name") & " " & If(IsDBNull(reader("middle_name")), "", reader("middle_name"))
                    Dim address As String = reader("address_line") & ", " & reader("city") & ", " & reader("country") & ", " & reader("zip_code")

                    Label_StockDetailsVendorDetailsSummaryTextNewStock.Text = "Vendor ID: " & reader("vendor_id") & vbCrLf &
                                                                          "Contact Person: " & contactPerson & vbCrLf &
                                                                          "Email: " & If(IsDBNull(reader("email")), "No value", reader("email")) & vbCrLf &
                                                                          "Phone: " & If(IsDBNull(reader("phone")), "No value", reader("phone")) & vbCrLf &
                                                                          "Address: " & address
                End If
            End Using
        End Using
    End Sub

    Private Function GetVendorIdFromAutocomplete(value As String) As Integer
        Dim vendorId As Integer = 0
        If value.StartsWith("(") AndAlso value.Contains(")") Then
            Dim idPart As String = value.Substring(1, value.IndexOf(")") - 1)
            Integer.TryParse(idPart, vendorId)
        End If
        Return vendorId
    End Function

    Private Sub Button_BackNewStock_Click(sender As Object, e As EventArgs) Handles Button_BackNewStock.Click
        Dim Window_Stocks As New Window_Stocks()
        Window_Stocks.Show()
        Me.Close()
    End Sub

    Private Sub Checkbox_StockDetailsNoExpiryQuestionNewStock_Click(sender As Object, e As EventArgs) Handles Checkbox_StockDetailsNoExpiryQuestionNewStock.Click
        If Checkbox_StockDetailsNoExpiryQuestionNewStock.Checked Then
            DateTimePicker_ExpiryDateInputNewStock.Visible = True
        Else
            DateTimePicker_ExpiryDateInputNewStock.Visible = False
        End If
    End Sub
End Class