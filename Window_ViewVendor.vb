Imports MySql.Data.MySqlClient

Public Class Window_ViewVendor
    Public ConnectionString = "server=localhost;user id=root;password=benjaminn1202;database=erp_sims_db"
    Public Dim SelectedVendorToView = 1

    Private Sub Window_ViewVendor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label_PageHeaderViewVendor.Text = "Viewing Vendor " & SelectedVendorToView
        ViewVendor(SelectedVendorToView)
    End Sub

    Private Sub ViewVendor(VendorID)
        Using conn As New MySqlConnection(ConnectionString)
            conn.Open()

            Dim query As String = "SELECT * FROM vendors WHERE vendor_id=" & VendorID
            Dim cmd As New MySqlCommand(query, conn)

            Dim CreatedByID = ""
            Dim CreatedBy = ""

            Using reader As MySqlDataReader = cmd.ExecuteReader()
                    reader.Read()
                    Dim VendorName = reader("vendor_name")
                    Dim FirstName = reader("first_name")
                    Dim MiddleName = reader("middle_name")
                    Dim LastName = reader("last_name")
                    Dim Email = reader("email")
                    Dim Phone = reader("phone")
                    Dim AddressLine = reader("address_line")
                    Dim City = reader("city")
                    Dim ZipCode = reader("zip_code")
                    Dim Country = reader("country")
                    CreatedByID = reader("created_by")
                    Dim CreatedAt = reader("created_at")

                    Textbox_VendorDetailVendorIDInputViewVendor.Text = VendorID
                    Textbox_VendorDetailVendorNamenputViewVendor.Text = VendorName
                    Textbox_VendorDetailContactPersonFirstNameInputViewVendor.Text = FirstName
                    Textbox_VendorDetailContactPersonMiddleNameInputViewVendor.Text = MiddleName
                    Textbox_VendorDetailContactPersonLastNameInputViewVendor.Text = LastName
                    Textbox_VendorDetailVendorEmailInputViewVendor.Text = Email
                    Textbox_VendorDetailVendorPhoneInputViewVendor.Text = Phone
                    Textbox_VendorDetailVendorAddressLineInputViewVendor.Text = AddressLine
                    Textbox_VendorDetailVendorCityInputViewVendor.Text = City
                    Textbox_VendorDetailVendorZipCodeInputViewVendor.Text = ZipCode
                    Textbox_VendorDetailVendorCountryInputViewVendor.Text = Country
                    Textbox_CreationDetailCustomerCreatedAtInputViewVendor.Text = CreatedAt
                End Using
                
                query = "SELECT email FROM accounts WHERE account_id=" & CreatedByID
                cmd = New  MySqlCommand(query, conn)

                Using reader As MySqlDataReader = cmd.ExecuteReader()
                    reader.Read()
                    CreatedBy = reader("email")
                    Textbox_CreationDetailCustomerCreatedByInputViewVendor.Text = CreatedBy
                End Using
            End Using
    End Sub
End Class