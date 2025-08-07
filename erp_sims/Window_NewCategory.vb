Imports MySql.Data.MySqlClient

Public Class Window_NewCategory
    Public ConnectionString = "server=localhost;user id=root;password=benjaminn1202;database=erp_sims_db"

    Private Sub Button_MakeCategoryNewCategory_Click(sender As Object, e As EventArgs) Handles Button_MakeCategoryNewCategory.Click
        Dim CategoryNameInput = Textbox_CategoryDetailCategoryNameInputNewCategory.Text

        Dim isValid As Boolean = True

        If String.IsNullOrEmpty(CategoryNameInput) Then
            Textbox_CategoryDetailCategoryNameInputNewCategory.FillColor = Color.FromArgb(255, 191, 189)
            isValid = False
        Else
            Textbox_CategoryDetailCategoryNameInputNewCategory.FillColor = Color.FromArgb(235, 225, 255)
        End If

        If Not isValid Then
            MessageBox.Show("Category entry failed. Please fill in the required fields.", "Entry Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Using connection As New MySqlConnection(ConnectionString)
            Try
                connection.Open()
                Dim checkCmd As New MySqlCommand("SELECT COUNT(*) FROM categories WHERE category_name = @CategoryName", connection)
                checkCmd.Parameters.AddWithValue("@CategoryName", CategoryNameInput)

                Dim categoryExists As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())
                If categoryExists > 0 Then
                    MessageBox.Show("Category already exists.", "Entry Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If

                Dim cmd As New MySqlCommand("INSERT INTO categories (category_name, created_by, created_at) VALUES (@CategoryName, @CreatedBy, @CreatedAt)", connection)
                cmd.Parameters.AddWithValue("@CategoryName", CategoryNameInput)
                cmd.Parameters.AddWithValue("@CreatedBy", 1)
                cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now)

                Dim rowsAffected As Integer = cmd.ExecuteNonQuery()
                If rowsAffected > 0 Then
                    MessageBox.Show("Category entry successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Textbox_CategoryDetailCategoryNameInputNewCategory.Text = ""
                Else
                    MessageBox.Show("Category entry failed.", "Entry Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            Catch ex As MySqlException
                MessageBox.Show("Database connection failed: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub Button_BackNewCategory_Click(sender As Object, e As EventArgs) Handles Button_BackNewCategory.Click
        Dim Categories As New Window_Categories()
        Window_Categories.Show()
        Me.Close()
    End Sub
End Class