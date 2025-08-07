Imports MySql.Data.MySqlClient

Public Class Window_ViewCustomer
    Public ConnectionString = "server=localhost;user id=root;password=benjaminn1202;database=erp_sims_db"

    Private Sub ViewCustomer(CustomerID)
        Using conn As New MySqlConnection(ConnectionString)
            conn.Open()

            Dim query As String = "SELECT * FROM customers WHERE customer_id=" & CustomerID
            Dim cmd As New MySqlCommand(query, conn)
            
            Dim CreatedByID = ""
            Dim CreatedBy = ""

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
                CreatedByID = reader("created_by")
                Dim CreatedAt = reader("created_at")

                Textbox_CustomerDetailCustomerIDInputViewCustomer.Text = CustomerID
                Textbox_CustomerDetailCustomerFirstNameInputViewCustomer.Text = FirstName
                Textbox_CustomerDetailCustomerMiddleNameInputViewCustomer.Text = MiddleName
                Textbox_CustomerDetailCustomerLastNameInputViewCustomer.Text = LastName
                Textbox_CustomerDetailCustomerCompanyInputViewCustomer.Text = Company
                Textbox_CustomerDetailCustomerEmailInputViewCustomer.Text = Email
                Textbox_CustomerDetailCustomerPhoneInputViewCustomer.Text = Phone
                Textbox_CustomerDetailCustomerAddressLineInputViewCustomer.Text = AddressLine
                Textbox_CustomerDetailCustomerCityInputViewCustomer.Text = City
                Textbox_CustomerDetailCustomerZipCodeInputViewCustomer.Text = ZipCode
                Textbox_CustomerDetailCustomerCountryInputViewCustomer.Text = Country
                Textbox_CreationDetailCustomerCreatedAtInputViewCustomer.Text = CreatedAt
            End Using

            query = "SELECT email FROM accounts WHERE account_id=" & CreatedByID
            cmd = New  MySqlCommand(query, conn)

            Using reader As MySqlDataReader = cmd.ExecuteReader()
                reader.Read()
                CreatedBy = reader("email")
                Textbox_CreationDetailCustomerCreatedByInputViewCustomer.Text = CreatedBy
            End Using
        End Using
    End Sub

    Private Sub Window_ViewCustomer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ViewCustomer(1)
    End Sub
End Class