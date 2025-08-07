Imports MySql.Data.MySqlClient

Public Class Window_EditBrand
    Public ConnectionString = "server=localhost;user id=root;password=benjaminn1202;database=erp_sims_db"
    Public Dim SelectedBrandToEdit = 1

    Private Sub Window_EditBrand_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label_PageHeaderEditBrand.Text = "Editing Brand " & SelectedBrandToEdit
        PopulateInputsEditBrand(SelectedBrandToEdit)
    End Sub

    Private Sub Button_MakeBrandEditBrand_Click(sender As Object, e As EventArgs) Handles Button_MakeBrandEditBrand.Click
        UpdateBrandDetails(SelectedBrandToEdit)
    End Sub

    Private Sub PopulateInputsEditBrand(BrandID)
        Using conn As New MySqlConnection(ConnectionString)
            conn.Open()

            Dim query As String = "SELECT * FROM brands WHERE brand_id=" & BrandID
            Dim cmd As New MySqlCommand(query, conn)

            Using reader As MySqlDataReader = cmd.ExecuteReader()
                reader.Read()
                Dim BrandName = reader("brand_name")

                Textbox_BrandDetailBrandIDInputEditBrand.Text = BrandID
                Textbox_BrandDetailBrandNameInputEditBrand.Text = BrandName
            End Using
        End Using
    End Sub

    Private Sub UpdateBrandDetails(id As Integer)
        Dim isValid As Boolean = True

        If String.IsNullOrWhiteSpace(Textbox_BrandDetailBrandNameInputEditBrand.Text) Then
            Textbox_BrandDetailBrandNameInputEditBrand.FillColor = Color.FromArgb(255, 191, 189)
            isValid = False
        Else
            Textbox_BrandDetailBrandNameInputEditBrand.FillColor = Color.FromArgb(235, 225, 255)
        End If

        If Not isValid Then
            MessageBox.Show("Brand edit failed.")
            Return
        End If

        Dim brandName As String = Textbox_BrandDetailBrandNameInputEditBrand.Text

        Using conn As New MySqlConnection(ConnectionString)
            conn.Open()
            Dim updateCmd As New MySqlCommand("UPDATE brands SET brand_name = @brandName WHERE brand_id = @id", conn)
            updateCmd.Parameters.AddWithValue("@brandName", brandName)
            updateCmd.Parameters.AddWithValue("@id", id)
            updateCmd.ExecuteNonQuery()
            MessageBox.Show("Brand edit succeeded.")
        End Using
    End Sub
End Class