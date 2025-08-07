Imports MySql.Data.MySqlClient
Imports Guna.UI2.WinForms

Public Class Window_Login
    Public ConnectionString = "server=localhost;user id=root;password=benjaminn1202;database=hrm_db"

    Private Sub Button_Login_Click(sender As Object, e As EventArgs) Handles Button_Login.Click
        ValidateLogin()
    End Sub

    Private Sub Checkbox_LoginShowPassword_CheckedChanged(sender As Object, e As EventArgs) Handles Checkbox_LoginShowPassword.CheckedChanged
        If Checkbox_LoginShowPassword.Checked Then
            Textbox_LoginInputPassword.PasswordChar = ""
        Else
            Textbox_LoginInputPassword.PasswordChar = "*"
        End If
    End Sub

    Public Function ValidateLogin()
        If String.IsNullOrWhiteSpace(Textbox_LoginInputEmail.Text) Then
            MessageBox.Show("Email cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return Nothing
        End If

        If String.IsNullOrWhiteSpace(Textbox_LoginInputPassword.Text) Then
            MessageBox.Show("Password cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return Nothing
        End If

        Dim currentAccount As Account = Nothing

        Using connection As New MySqlConnection(ConnectionString)
            Try
                connection.Open()

                Dim query As String = "
                    SELECT e.employee_id, e.email, e.password, e.position_id, e.first_name, 
                           e.middle_name, e.last_name, e.employee_id, p.position_name
                    FROM employee_table
                    INNER JOIN position_tbl p ON e.position_id = p.position_id
                    WHERE e.email = @Email AND e.password = @Password
                    LIMIT 1"

                Using command As New MySqlCommand(query, connection)
                    command.Parameters.AddWithValue("@Email", Textbox_LoginInputEmail.Text)
                    command.Parameters.AddWithValue("@Password", Textbox_LoginInputPassword.Text)

                    Using reader As MySqlDataReader = command.ExecuteReader()
                        If reader.Read() Then
                            currentAccount = New Account() With {
                                .AccountId = reader.GetInt32("employee_id"),
                                .Email = reader.GetString("email"),
                                .Password = reader.GetString("password"),
                                .Type = If(reader.IsDBNull(reader.GetOrdinal("position_name")), "N/A", reader.GetString("position_name")), ' If NULL, store "N/A"
                                .Name = $"{If(reader.IsDBNull(reader.GetOrdinal("first_name")), "N/A", reader.GetString("first_name"))} " & _
                                        $"{If(reader.IsDBNull(reader.GetOrdinal("middle_name")), "N/A", reader.GetString("middle_name"))} " & _
                                        $"{If(reader.IsDBNull(reader.GetOrdinal("last_name")), "N/A", reader.GetString("last_name"))}",
                                .EmployeeId = reader.GetInt32("employee_id")
                            }

                            GlobalShared.CurrentUser = currentAccount
                            MessageBox.Show($"Welcome, {currentAccount.Name}!", "Login Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)

                            Dim newTransactionForm As New Window_NewTransaction()
                            newTransactionForm.Show()

                            Application.OpenForms(0).Close()
                        Else
                            MessageBox.Show("Invalid email or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        End If
                    End Using
                End Using
            Catch ex As Exception
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Function

    Private Sub Window_Login_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class