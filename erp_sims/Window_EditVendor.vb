Imports MySql.Data.MySqlClient

Public Class Window_EditVendor
    Public ConnectionString = "server=localhost;user id=root;password=benjaminn1202;database=erp_sims_db"
    Public Dim SelectedVendorToEdit = 1

    Private Sub Window_EditVendor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label_PageHeaderEditVendor.Text = "Editing Vendor " & SelectedVendorToEdit
        PopulateInputsEditVendor(SelectedVendorToEdit)
    End Sub

    Private Sub Button_SaveChangesEditVendor_Click(sender As Object, e As EventArgs) Handles Button_SaveChangesEditVendor.Click
        UpdateVendorDetails(SelectedVendorToEdit)
    End Sub

    Private Sub PopulateInputsEditVendor(VendorID)
        Using conn As New MySqlConnection(ConnectionString)
            conn.Open()

            Dim query As String = "SELECT * FROM Vendors WHERE vendor_id=" & VendorID
            Dim cmd As New MySqlCommand(query, conn)

            Using reader As MySqlDataReader = cmd.ExecuteReader()
                reader.Read()
                Dim FirstName = reader("first_name")
                Dim MiddleName = reader("middle_name")
                Dim LastName = reader("last_name")
                Dim VendorName = reader("vendor_name")
                Dim Email = reader("email")
                Dim Phone = reader("phone")
                Dim AddressLine = reader("address_line")
                Dim City = reader("city")
                Dim ZipCode = reader("zip_code")
                Dim Country = reader("country")

                Textbox_VendorDetailVendorIDInputEditVendor.Text = VendorID
                Textbox_VendorDetailVendorNamenputEditVendor.Text = VendorName
                Textbox_VendorDetailContactPersonFirstNameInputEditVendor.Text = FirstName
                Textbox_VendorDetailContactPersonMiddleNameInputEditVendor.Text = MiddleName
                Textbox_VendorDetailContactPersonLastNameInputEditVendor.Text = LastName
                Textbox_VendorDetailVendorEmailInputEditVendor.Text = Email
                Textbox_VendorDetailVendorPhoneInputEditVendor.Text = Phone
                Textbox_VendorDetailVendorAddressLineInputEditVendor.Text = AddressLine
                Textbox_VendorDetailVendorCityInputEditVendor.Text = City
                Textbox_VendorDetailVendorZipCodeInputEditVendor.Text = ZipCode
                Textbox_VendorDetailVendorCountryInputEditVendor.Text = Country
            End Using
        End Using
    End Sub
    
    Private Sub UpdateVendorDetails(id As Integer)
        Dim isValid As Boolean = True

        If String.IsNullOrWhiteSpace(Textbox_VendorDetailVendorNamenputEditVendor.Text) Then
            Textbox_VendorDetailVendorNamenputEditVendor.FillColor = Color.FromArgb(255, 191, 189)
            isValid = False
        Else
            Textbox_VendorDetailVendorNamenputEditVendor.FillColor = Color.FromArgb(235, 225, 255)
        End If

        If String.IsNullOrWhiteSpace(Textbox_VendorDetailContactPersonFirstNameInputEditVendor.Text) Then
            Textbox_VendorDetailContactPersonFirstNameInputEditVendor.FillColor = Color.FromArgb(255, 191, 189)
            isValid = False
        Else
            Textbox_VendorDetailContactPersonFirstNameInputEditVendor.FillColor = Color.FromArgb(235, 225, 255)
        End If

        If String.IsNullOrWhiteSpace(Textbox_VendorDetailContactPersonLastNameInputEditVendor.Text) Then
            Textbox_VendorDetailContactPersonLastNameInputEditVendor.FillColor = Color.FromArgb(255, 191, 189)
            isValid = False
        Else
            Textbox_VendorDetailContactPersonLastNameInputEditVendor.FillColor = Color.FromArgb(235, 225, 255)
        End If

        If String.IsNullOrWhiteSpace(Textbox_VendorDetailVendorEmailInputEditVendor.Text) Then
            Textbox_VendorDetailVendorEmailInputEditVendor.FillColor = Color.FromArgb(255, 191, 189)
            isValid = False
        Else
            Textbox_VendorDetailVendorEmailInputEditVendor.FillColor = Color.FromArgb(235, 225, 255)
        End If

        If String.IsNullOrWhiteSpace(Textbox_VendorDetailVendorPhoneInputEditVendor.Text) Then
            Textbox_VendorDetailVendorPhoneInputEditVendor.FillColor = Color.FromArgb(255, 191, 189)
            isValid = False
        Else
            Textbox_VendorDetailVendorPhoneInputEditVendor.FillColor = Color.FromArgb(235, 225, 255)
        End If

        If String.IsNullOrWhiteSpace(Textbox_VendorDetailVendorAddressLineInputEditVendor.Text) Then
            Textbox_VendorDetailVendorAddressLineInputEditVendor.FillColor = Color.FromArgb(255, 191, 189)
            isValid = False
        Else
            Textbox_VendorDetailVendorAddressLineInputEditVendor.FillColor = Color.FromArgb(235, 225, 255)
        End If

        If String.IsNullOrWhiteSpace(Textbox_VendorDetailVendorCityInputEditVendor.Text) Then
            Textbox_VendorDetailVendorCityInputEditVendor.FillColor = Color.FromArgb(255, 191, 189)
            isValid = False
        Else
            Textbox_VendorDetailVendorCityInputEditVendor.FillColor = Color.FromArgb(235, 225, 255)
        End If

        If String.IsNullOrWhiteSpace(Textbox_VendorDetailVendorZipCodeInputEditVendor.Text) Then
            Textbox_VendorDetailVendorZipCodeInputEditVendor.FillColor = Color.FromArgb(255, 191, 189)
            isValid = False
        Else
            Textbox_VendorDetailVendorZipCodeInputEditVendor.FillColor = Color.FromArgb(235, 225, 255)
        End If

        If String.IsNullOrWhiteSpace(Textbox_VendorDetailVendorCountryInputEditVendor.Text) Then
            Textbox_VendorDetailVendorCountryInputEditVendor.FillColor = Color.FromArgb(255, 191, 189)
            isValid = False
        Else
            Textbox_VendorDetailVendorCountryInputEditVendor.FillColor = Color.FromArgb(235, 225, 255)
        End If

        If Not isValid Then
            MessageBox.Show("Vendor edit failed.")
            Return
        End If

        Using conn As New MySqlConnection(ConnectionString)
            conn.Open()
            Dim updateCmd As New MySqlCommand("UPDATE vendors SET vendor_name = @vendorName, first_name = @firstName, middle_name = @middleName, last_name = @lastName, email = @email, phone = @phone, address_line = @addressLine, city = @city, zip_code = @zipCode, country = @country WHERE vendor_id = @id", conn)
            updateCmd.Parameters.AddWithValue("@vendorName", Textbox_VendorDetailVendorNamenputEditVendor.Text)
            updateCmd.Parameters.AddWithValue("@firstName", Textbox_VendorDetailContactPersonFirstNameInputEditVendor.Text)
            updateCmd.Parameters.AddWithValue("@middleName", Textbox_VendorDetailContactPersonMiddleNameInputEditVendor.Text)
            updateCmd.Parameters.AddWithValue("@lastName", Textbox_VendorDetailContactPersonLastNameInputEditVendor.Text)
            updateCmd.Parameters.AddWithValue("@email", Textbox_VendorDetailVendorEmailInputEditVendor.Text)
            updateCmd.Parameters.AddWithValue("@phone", Textbox_VendorDetailVendorPhoneInputEditVendor.Text)
            updateCmd.Parameters.AddWithValue("@addressLine", Textbox_VendorDetailVendorAddressLineInputEditVendor.Text)
            updateCmd.Parameters.AddWithValue("@city", Textbox_VendorDetailVendorCityInputEditVendor.Text)
            updateCmd.Parameters.AddWithValue("@zipCode", Textbox_VendorDetailVendorZipCodeInputEditVendor.Text)
            updateCmd.Parameters.AddWithValue("@country", Textbox_VendorDetailVendorCountryInputEditVendor.Text)
            updateCmd.Parameters.AddWithValue("@id", id)
            updateCmd.ExecuteNonQuery()
            MessageBox.Show("Vendor edit succeeded.")
        End Using
    End Sub
End Class