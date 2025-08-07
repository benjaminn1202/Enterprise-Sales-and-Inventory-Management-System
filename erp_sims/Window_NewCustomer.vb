Imports MySql.Data.MySqlClient

Public Class Window_NewCustomer
    Public ConnectionString = "server=localhost;user id=root;password=benjaminn1202;database=erp_sims_db"

    Private Sub Button_MakeCustomerNewCustomer_Click(sender As Object, e As EventArgs) Handles Button_MakeCustomerNewCustomer.Click
        Dim firstName = Textbox_CustomerDetailCustomerFirstNameInputNewCustomer.Text
        Dim middleName = Textbox_CustomerDetailCustomerMiddleNameInputNewCustomer.Text
        Dim lastName = Textbox_CustomerDetailCustomerLastNameInputNewCustomer.Text
        Dim company = Textbox_CustomerDetailCustomerCompanyInputNewCustomer.Text
        Dim email = Textbox_CustomerDetailCustomerEmailInputNewCustomer.Text
        Dim phone = Textbox_CustomerDetailCustomerPhoneInputNewCustomer.Text
        Dim addressLine = Textbox_CustomerDetailCustomerAddressLineInputNewCustomer.Text
        Dim city = Textbox_CustomerDetailCustomerCityInputNewCustomer.Text
        Dim zipCode = Textbox_CustomerDetailCustomerZipCodeInputNewCustomer.Text
        Dim country = Textbox_CustomerDetailCustomerCountryInputNewCustomer.Text

        Dim isValid As Boolean = True

        If String.IsNullOrEmpty(firstName) Then
            Textbox_CustomerDetailCustomerFirstNameInputNewCustomer.FillColor = Color.FromArgb(255, 191, 189)
            isValid = False
        Else
            Textbox_CustomerDetailCustomerFirstNameInputNewCustomer.FillColor = Color.FromArgb(235, 225, 255)
        End If

        If String.IsNullOrEmpty(lastName) Then
            Textbox_CustomerDetailCustomerLastNameInputNewCustomer.FillColor = Color.FromArgb(255, 191, 189)
            isValid = False
        Else
            Textbox_CustomerDetailCustomerLastNameInputNewCustomer.FillColor = Color.FromArgb(235, 225, 255)
        End If

        If String.IsNullOrEmpty(email) Then
            Textbox_CustomerDetailCustomerEmailInputNewCustomer.FillColor = Color.FromArgb(255, 191, 189)
            isValid = False
        Else
            Textbox_CustomerDetailCustomerEmailInputNewCustomer.FillColor = Color.FromArgb(235, 225, 255)
        End If

        If String.IsNullOrEmpty(phone) Then
            Textbox_CustomerDetailCustomerPhoneInputNewCustomer.FillColor = Color.FromArgb(255, 191, 189)
            isValid = False
        Else
            Textbox_CustomerDetailCustomerPhoneInputNewCustomer.FillColor = Color.FromArgb(235, 225, 255)
        End If

        If String.IsNullOrEmpty(addressLine) Then
            Textbox_CustomerDetailCustomerAddressLineInputNewCustomer.FillColor = Color.FromArgb(255, 191, 189)
            isValid = False
        Else
            Textbox_CustomerDetailCustomerAddressLineInputNewCustomer.FillColor = Color.FromArgb(235, 225, 255)
        End If

        If String.IsNullOrEmpty(city) Then
            Textbox_CustomerDetailCustomerCityInputNewCustomer.FillColor = Color.FromArgb(255, 191, 189)
            isValid = False
        Else
            Textbox_CustomerDetailCustomerCityInputNewCustomer.FillColor = Color.FromArgb(235, 225, 255)
        End If

        If String.IsNullOrEmpty(zipCode) Then
            Textbox_CustomerDetailCustomerZipCodeInputNewCustomer.FillColor = Color.FromArgb(255, 191, 189)
            isValid = False
        Else
            Textbox_CustomerDetailCustomerZipCodeInputNewCustomer.FillColor = Color.FromArgb(235, 225, 255)
        End If

        If String.IsNullOrEmpty(country) Then
            Textbox_CustomerDetailCustomerCountryInputNewCustomer.FillColor = Color.FromArgb(255, 191, 189)
            isValid = False
        Else
            Textbox_CustomerDetailCustomerCountryInputNewCustomer.FillColor = Color.FromArgb(235, 225, 255)
        End If

        If Not isValid Then
            MessageBox.Show("Customer entry failed. Please fill in all required fields.", "Entry Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Using connection As New MySqlConnection(ConnectionString)
            Try
                connection.Open()
                Dim checkCmd As New MySqlCommand("SELECT * FROM customers WHERE first_name = @FirstName AND middle_name = @MiddleName AND last_name = @LastName", connection)
                checkCmd.Parameters.AddWithValue("@FirstName", firstName)
                checkCmd.Parameters.AddWithValue("@MiddleName", middleName)
                checkCmd.Parameters.AddWithValue("@LastName", lastName)

                Dim reader As MySqlDataReader = checkCmd.ExecuteReader()
                If reader.HasRows Then
                    MessageBox.Show("Customer with the same name already exists.", "Entry Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If
                reader.Close()

                Dim insertCmd As New MySqlCommand("INSERT INTO customers (first_name, middle_name, last_name, company, email, phone, address_line, city, zip_code, country, created_by, created_at) VALUES (@FirstName, @MiddleName, @LastName, @Company, @Email, @Phone, @AddressLine, @City, @ZipCode, @Country, @CreatedBy, @CreatedAt)", connection)
                insertCmd.Parameters.AddWithValue("@FirstName", firstName)
                insertCmd.Parameters.AddWithValue("@MiddleName", middleName)
                insertCmd.Parameters.AddWithValue("@LastName", lastName)
                insertCmd.Parameters.AddWithValue("@Company", company)
                insertCmd.Parameters.AddWithValue("@Email", email)
                insertCmd.Parameters.AddWithValue("@Phone", phone)
                insertCmd.Parameters.AddWithValue("@AddressLine", addressLine)
                insertCmd.Parameters.AddWithValue("@City", city)
                insertCmd.Parameters.AddWithValue("@ZipCode", zipCode)
                insertCmd.Parameters.AddWithValue("@Country", country)
                insertCmd.Parameters.AddWithValue("@CreatedBy", 1) ' Change this shit (1) to "CurrentUserID" variable
                insertCmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now)

                insertCmd.ExecuteNonQuery()
                MessageBox.Show("Customer entry successful!", "Entry Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

                Textbox_CustomerDetailCustomerFirstNameInputNewCustomer.Text = ""
                Textbox_CustomerDetailCustomerMiddleNameInputNewCustomer.Text = ""
                Textbox_CustomerDetailCustomerLastNameInputNewCustomer.Text = ""
                Textbox_CustomerDetailCustomerCompanyInputNewCustomer.Text = ""
                Textbox_CustomerDetailCustomerEmailInputNewCustomer.Text = ""
                Textbox_CustomerDetailCustomerPhoneInputNewCustomer.Text = ""
                Textbox_CustomerDetailCustomerAddressLineInputNewCustomer.Text = ""
                Textbox_CustomerDetailCustomerCityInputNewCustomer.Text = ""
                Textbox_CustomerDetailCustomerZipCodeInputNewCustomer.Text = ""
                Textbox_CustomerDetailCustomerCountryInputNewCustomer.Text = ""
            Catch ex As MySqlException
                MessageBox.Show("Database connection failed: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub Button_BackNewCustomer_Click(sender As Object, e As EventArgs) Handles Button_BackNewCustomer.Click
        Dim Customers As New Window_Customers()
        Window_Customers.Show()
        Me.Close()
    End Sub
End Class