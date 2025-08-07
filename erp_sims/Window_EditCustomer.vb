Imports MySql.Data.MySqlClient

Public Class Window_EditCustomer
    Public ConnectionString = "server=localhost;user id=root;password=benjaminn1202;database=erp_sims_db"
    Public Dim SelectedCustomerToEdit = 1

    Private Sub Window_EditCustomer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PopulateInputsEditCustomer(SelectedCustomerToEdit)
    End Sub

    Private Sub Button_SaveChangesEditCustomer_Click(sender As Object, e As EventArgs) Handles Button_SaveChangesEditCustomer.Click
        UpdateCustomerDetails(SelectedCustomerToEdit)
    End Sub

    Private Sub PopulateInputsEditCustomer(CustomerID)
        Using conn As New MySqlConnection(ConnectionString)
            conn.Open()

            Dim query As String = "SELECT * FROM customers WHERE customer_id=" & CustomerID
            Dim cmd As New MySqlCommand(query, conn)

            Using reader As MySqlDataReader = cmd.ExecuteReader()
                reader.Read()
                Dim FirstName = reader("first_name")
                Dim MiddleName = reader("middle_name")
                Dim LastName = reader("last_name")
                Dim Company = reader("company")
                Dim Email = reader("email")
                Dim Phone = reader("phone")
                Dim AddressLine = reader("address_line")
                Dim City = reader("city")
                Dim ZipCode = reader("zip_code")
                Dim Country = reader("country")

                Textbox__CustomerDetailCustomerIDInputEditCustomer.Text = CustomerID
                Textbox_CustomerDetailCustomerFirstNameInputEditCustomer.Text = FirstName
                Textbox_CustomerDetailCustomerMiddleNameInputEditCustomer.Text = MiddleName
                Textbox_CustomerDetailCustomerLastNameInputEditCustomer.Text = LastName
                Textbox_CustomerDetailCustomerCompanyInputEditCustomer.Text = Company
                Textbox_CustomerDetailCustomerEmailInputEditCustomer.Text = Email
                Textbox_CustomerDetailCustomerPhoneInputEditCustomer.Text = Phone
                Textbox_CustomerDetailCustomerAddressLineInputEditCustomer.Text = AddressLine
                Textbox_CustomerDetailCustomerCityInputEditCustomer.Text = City
                Textbox_CustomerDetailCustomerZipCodeInputEditCustomer.Text = ZipCode
                Textbox_CustomerDetailCustomerCountryInputEditCustomer.Text = Country
            End Using
        End Using
    End Sub
    
    Private Sub UpdateCustomerDetails(id As Integer)
        Dim isValid As Boolean = True

        If String.IsNullOrWhiteSpace(Textbox_CustomerDetailCustomerFirstNameInputEditCustomer.Text) Then
            Textbox_CustomerDetailCustomerFirstNameInputEditCustomer.FillColor = Color.FromArgb(255, 191, 189)
            isValid = False
        Else
            Textbox_CustomerDetailCustomerFirstNameInputEditCustomer.FillColor = Color.FromArgb(235, 225, 255)
        End If

        If String.IsNullOrWhiteSpace(Textbox_CustomerDetailCustomerLastNameInputEditCustomer.Text) Then
            Textbox_CustomerDetailCustomerLastNameInputEditCustomer.FillColor = Color.FromArgb(255, 191, 189)
            isValid = False
        Else
            Textbox_CustomerDetailCustomerLastNameInputEditCustomer.FillColor = Color.FromArgb(235, 225, 255)
        End If

        If String.IsNullOrWhiteSpace(Textbox_CustomerDetailCustomerCompanyInputEditCustomer.Text) Then
            Textbox_CustomerDetailCustomerCompanyInputEditCustomer.FillColor = Color.FromArgb(255, 191, 189)
            isValid = False
        Else
            Textbox_CustomerDetailCustomerCompanyInputEditCustomer.FillColor = Color.FromArgb(235, 225, 255)
        End If

        If String.IsNullOrWhiteSpace(Textbox_CustomerDetailCustomerEmailInputEditCustomer.Text) Then
            Textbox_CustomerDetailCustomerEmailInputEditCustomer.FillColor = Color.FromArgb(255, 191, 189)
            isValid = False
        Else
            Textbox_CustomerDetailCustomerEmailInputEditCustomer.FillColor = Color.FromArgb(235, 225, 255)
        End If

        If String.IsNullOrWhiteSpace(Textbox_CustomerDetailCustomerPhoneInputEditCustomer.Text) Then
            Textbox_CustomerDetailCustomerPhoneInputEditCustomer.FillColor = Color.FromArgb(255, 191, 189)
            isValid = False
        Else
            Textbox_CustomerDetailCustomerPhoneInputEditCustomer.FillColor = Color.FromArgb(235, 225, 255)
        End If

        If String.IsNullOrWhiteSpace(Textbox_CustomerDetailCustomerAddressLineInputEditCustomer.Text) Then
            Textbox_CustomerDetailCustomerAddressLineInputEditCustomer.FillColor = Color.FromArgb(255, 191, 189)
            isValid = False
        Else
            Textbox_CustomerDetailCustomerAddressLineInputEditCustomer.FillColor = Color.FromArgb(235, 225, 255)
        End If

        If String.IsNullOrWhiteSpace(Textbox_CustomerDetailCustomerCityInputEditCustomer.Text) Then
            Textbox_CustomerDetailCustomerCityInputEditCustomer.FillColor = Color.FromArgb(255, 191, 189)
            isValid = False
        Else
            Textbox_CustomerDetailCustomerCityInputEditCustomer.FillColor = Color.FromArgb(235, 225, 255)
        End If

        If String.IsNullOrWhiteSpace(Textbox_CustomerDetailCustomerZipCodeInputEditCustomer.Text) Then
            Textbox_CustomerDetailCustomerZipCodeInputEditCustomer.FillColor = Color.FromArgb(255, 191, 189)
            isValid = False
        Else
            Textbox_CustomerDetailCustomerZipCodeInputEditCustomer.FillColor = Color.FromArgb(235, 225, 255)
        End If

        If String.IsNullOrWhiteSpace(Textbox_CustomerDetailCustomerCountryInputEditCustomer.Text) Then
            Textbox_CustomerDetailCustomerCountryInputEditCustomer.FillColor = Color.FromArgb(255, 191, 189)
            isValid = False
        Else
            Textbox_CustomerDetailCustomerCountryInputEditCustomer.FillColor = Color.FromArgb(235, 225, 255)
        End If

        If Not isValid Then
            MessageBox.Show("Customer edit failed.")
            Return
        End If

        Dim firstName As String = Textbox_CustomerDetailCustomerFirstNameInputEditCustomer.Text
        Dim middleName As String = Textbox_CustomerDetailCustomerMiddleNameInputEditCustomer.Text
        Dim lastName As String = Textbox_CustomerDetailCustomerLastNameInputEditCustomer.Text
        Dim company As String = Textbox_CustomerDetailCustomerCompanyInputEditCustomer.Text
        Dim email As String = Textbox_CustomerDetailCustomerEmailInputEditCustomer.Text
        Dim phone As String = Textbox_CustomerDetailCustomerPhoneInputEditCustomer.Text
        Dim addressLine As String = Textbox_CustomerDetailCustomerAddressLineInputEditCustomer.Text
        Dim city As String = Textbox_CustomerDetailCustomerCityInputEditCustomer.Text
        Dim zipCode As String = Textbox_CustomerDetailCustomerZipCodeInputEditCustomer.Text
        Dim country As String = Textbox_CustomerDetailCustomerCountryInputEditCustomer.Text

        Using conn As New MySqlConnection(ConnectionString)
            conn.Open()
            Dim updateCmd As New MySqlCommand("UPDATE customers SET first_name = @firstName, middle_name = @middleName, last_name = @lastName, company = @company, email = @email, phone = @phone, address_line = @addressLine, city = @city, zip_code = @zipCode, country = @country WHERE customer_id = @id", conn)
            updateCmd.Parameters.AddWithValue("@firstName", firstName)
            updateCmd.Parameters.AddWithValue("@middleName", middleName)
            updateCmd.Parameters.AddWithValue("@lastName", lastName)
            updateCmd.Parameters.AddWithValue("@company", company)
            updateCmd.Parameters.AddWithValue("@email", email)
            updateCmd.Parameters.AddWithValue("@phone", phone)
            updateCmd.Parameters.AddWithValue("@addressLine", addressLine)
            updateCmd.Parameters.AddWithValue("@city", city)
            updateCmd.Parameters.AddWithValue("@zipCode", zipCode)
            updateCmd.Parameters.AddWithValue("@country", country)
            updateCmd.Parameters.AddWithValue("@id", id)
            updateCmd.ExecuteNonQuery()
            MessageBox.Show("Customer edit succeeded.")
        End Using
    End Sub

End Class