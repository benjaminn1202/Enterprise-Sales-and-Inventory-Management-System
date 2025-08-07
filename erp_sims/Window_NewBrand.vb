Imports MySql.Data.MySqlClient

Public Class Window_NewBrand
    Public ConnectionString = "server=localhost;user id=root;password=benjaminn1202;database=erp_sims_db"

    Private Sub Button_MakeBrandNewBrand_Click(sender As Object, e As EventArgs) Handles Button_MakeBrandNewBrand.Click
        Dim brandName = Textbox_BrandDetailBrandNameInputNewBrand.Text
        Dim isValid As Boolean = True

        If String.IsNullOrEmpty(brandName) Then
            Textbox_BrandDetailBrandNameInputNewBrand.FillColor = Color.FromArgb(255, 191, 189)
            isValid = False
        Else
            Textbox_BrandDetailBrandNameInputNewBrand.FillColor = Color.FromArgb(235, 225, 255)
        End If

        If Not isValid Then
            MessageBox.Show("Brand entry failed. Please fill all required fields.", "Entry Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Using connection As New MySqlConnection(ConnectionString)
            Try
                connection.Open()

                Dim checkQuery As String = "SELECT COUNT(*) FROM brands WHERE brand_name = @BrandName"
                Dim checkCmd As New MySqlCommand(checkQuery, connection)
                checkCmd.Parameters.AddWithValue("@BrandName", brandName)

                Dim brandExists As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())

                If brandExists > 0 Then
                    MessageBox.Show("This brand already exists. Please enter a different brand name.", "Entry Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If

                Dim insertQuery As String = "INSERT INTO brands (brand_name, created_by, created_at) VALUES (@BrandName, @CreatedBy, @CreatedAt)"
                Dim cmd As New MySqlCommand(insertQuery, connection)
                cmd.Parameters.AddWithValue("@BrandName", brandName)
                cmd.Parameters.AddWithValue("@CreatedBy", 1) ' Change this shit to CurrentUserID variable.
                cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now)

                cmd.ExecuteNonQuery()

                MessageBox.Show("Brand entry successful!", "Entry Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)

                Textbox_BrandDetailBrandNameInputNewBrand.Text = ""

            Catch ex As MySqlException
                MessageBox.Show("Brand entry failed: " & ex.Message, "Entry Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub Button_BackNewBrand_Click(sender As Object, e As EventArgs) Handles Button_BackNewBrand.Click
        Dim Window_Brands As New Window_Brands()
        Window_Brands.Show()
        Me.Close()
    End Sub

    Private Sub Window_NewBrand_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class