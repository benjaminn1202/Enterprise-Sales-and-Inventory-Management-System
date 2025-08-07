Imports MySql.Data.MySqlClient

Public Class Window_EditCategory
    Public ConnectionString = "server=localhost;user id=root;password=benjaminn1202;database=erp_sims_db"
    Public Dim SelectedCategoryToEdit = 1

    Private Sub Window_EditCategory_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label_PageHeaderEditCategory.Text = "Editing Category " & SelectedCategoryToEdit
        PopulateInputsEditCategory(SelectedCategoryToEdit)
    End Sub

    Private Sub Button_MakeCategoryEditCategory_Click(sender As Object, e As EventArgs) Handles Button_MakeCategoryEditCategory.Click
        UpdateCategoryDetails(SelectedCategoryToEdit)
    End Sub

    Private Sub PopulateInputsEditCategory(CategoryID)
        Using conn As New MySqlConnection(ConnectionString)
            conn.Open()

            Dim query As String = "SELECT * FROM categories WHERE category_id=" & CategoryID
            Dim cmd As New MySqlCommand(query, conn)

            Using reader As MySqlDataReader = cmd.ExecuteReader()
                reader.Read()
                Dim CategoryName = reader("category_name")

                Textbox_CategoryDetailCategoryIDInputEditCategory.Text = CategoryID
                Textbox_CategoryDetailCategoryNameInputEditCategory.Text = CategoryName
            End Using
        End Using
    End Sub

    Private Sub UpdateCategoryDetails(id As Integer)
        Dim isValid As Boolean = True

        If String.IsNullOrWhiteSpace(Textbox_CategoryDetailCategoryNameInputEditCategory.Text) Then
            Textbox_CategoryDetailCategoryNameInputEditCategory.FillColor = Color.FromArgb(255, 191, 189)
            isValid = False
        Else
            Textbox_CategoryDetailCategoryNameInputEditCategory.FillColor = Color.FromArgb(235, 225, 255)
        End If

        If Not isValid Then
            MessageBox.Show("Category edit failed.")
            Return
        End If

        Dim CategoryName As String = Textbox_CategoryDetailCategoryNameInputEditCategory.Text

        Using conn As New MySqlConnection(ConnectionString)
            conn.Open()
            Dim updateCmd As New MySqlCommand("UPDATE Categories SET category_name = @CategoryName WHERE category_id = @id", conn)
            updateCmd.Parameters.AddWithValue("@CategoryName", CategoryName)
            updateCmd.Parameters.AddWithValue("@id", id)
            updateCmd.ExecuteNonQuery()
            MessageBox.Show("Category edit succeeded.")
        End Using
    End Sub
End Class