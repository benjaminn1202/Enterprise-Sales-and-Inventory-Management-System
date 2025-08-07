Imports MySql.Data.MySqlClient

Public Class Window_NewVendor
    Public ConnectionString = "server=localhost;user id=root;password=benjaminn1202;database=erp_sims_db"

    Private Sub Button_MakeVendorNewVendor_Click(sender As Object, e As EventArgs) Handles Button_MakeVendorNewVendor.Click
        Dim vendorName = Textbox_VendorDetailVendorNamenputNewVendor.Text
        Dim firstName = Textbox_VendorDetailContactPersonFirstNameInputNewVendor.Text
        Dim middleName = Textbox_VendorDetailContactPersonMiddleNameInputNewVendor.Text
        Dim lastName = Textbox_VendorDetailContactPersonLastNameInputNewVendor.Text
        Dim email = Textbox_VendorDetailVendorEmailInputNewVendor.Text
        Dim phone = Textbox_VendorDetailVendorPhoneInputNewVendor.Text
        Dim addressLine = Textbox_VendorDetailVendorAddressLineInputNewVendor.Text
        Dim city = Textbox_VendorDetailVendorCityInputNewVendor.Text
        Dim zipCode = Textbox_VendorDetailVendorZipCodeInputNewVendor.Text
        Dim country = Textbox_VendorDetailVendorCountryInputNewVendor.Text

        Dim isValid As Boolean = True

        If String.IsNullOrEmpty(vendorName) Then
            Textbox_VendorDetailVendorNamenputNewVendor.FillColor = Color.FromArgb(255, 191, 189)
            isValid = False
        Else
            Textbox_VendorDetailVendorNamenputNewVendor.FillColor = Color.FromArgb(235, 225, 255)
        End If

        If String.IsNullOrEmpty(firstName) Then
            Textbox_VendorDetailContactPersonFirstNameInputNewVendor.FillColor = Color.FromArgb(255, 191, 189)
            isValid = False
        Else
            Textbox_VendorDetailContactPersonFirstNameInputNewVendor.FillColor = Color.FromArgb(235, 225, 255)
        End If

        If String.IsNullOrEmpty(lastName) Then
            Textbox_VendorDetailContactPersonLastNameInputNewVendor.FillColor = Color.FromArgb(255, 191, 189)
            isValid = False
        Else
            Textbox_VendorDetailContactPersonLastNameInputNewVendor.FillColor = Color.FromArgb(235, 225, 255)
        End If

        If String.IsNullOrEmpty(email) Then
            Textbox_VendorDetailVendorEmailInputNewVendor.FillColor = Color.FromArgb(255, 191, 189)
            isValid = False
        Else
            Textbox_VendorDetailVendorEmailInputNewVendor.FillColor = Color.FromArgb(235, 225, 255)
        End If

        If String.IsNullOrEmpty(phone) Then
            Textbox_VendorDetailVendorPhoneInputNewVendor.FillColor = Color.FromArgb(255, 191, 189)
            isValid = False
        Else
            Textbox_VendorDetailVendorPhoneInputNewVendor.FillColor = Color.FromArgb(235, 225, 255)
        End If

        If String.IsNullOrEmpty(addressLine) Then
            Textbox_VendorDetailVendorAddressLineInputNewVendor.FillColor = Color.FromArgb(255, 191, 189)
            isValid = False
        Else
            Textbox_VendorDetailVendorAddressLineInputNewVendor.FillColor = Color.FromArgb(235, 225, 255)
        End If

        If String.IsNullOrEmpty(city) Then
            Textbox_VendorDetailVendorCityInputNewVendor.FillColor = Color.FromArgb(255, 191, 189)
            isValid = False
        Else
            Textbox_VendorDetailVendorCityInputNewVendor.FillColor = Color.FromArgb(235, 225, 255)
        End If

        If String.IsNullOrEmpty(zipCode) Then
            Textbox_VendorDetailVendorZipCodeInputNewVendor.FillColor = Color.FromArgb(255, 191, 189)
            isValid = False
        Else
            Textbox_VendorDetailVendorZipCodeInputNewVendor.FillColor = Color.FromArgb(235, 225, 255)
        End If

        If String.IsNullOrEmpty(country) Then
            Textbox_VendorDetailVendorCountryInputNewVendor.FillColor = Color.FromArgb(255, 191, 189)
            isValid = False
        Else
            Textbox_VendorDetailVendorCountryInputNewVendor.FillColor = Color.FromArgb(235, 225, 255)
        End If

        If Not isValid Then
            MessageBox.Show("Vendor entry failed. Please check the required fields.", "Entry Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Using connection As New MySqlConnection(ConnectionString)
            Try
                connection.Open()
                Dim cmd As New MySqlCommand("INSERT INTO vendors (vendor_name, first_name, middle_name, last_name, email, phone, address_line, city, zip_code, country, created_by, created_at) VALUES (@VendorName, @FirstName, @MiddleName, @LastName, @Email, @Phone, @AddressLine, @City, @ZipCode, @Country, @CreatedBy, @CreatedAt)", connection)

                cmd.Parameters.AddWithValue("@VendorName", vendorName)
                cmd.Parameters.AddWithValue("@FirstName", firstName)
                cmd.Parameters.AddWithValue("@MiddleName", middleName)
                cmd.Parameters.AddWithValue("@LastName", lastName)
                cmd.Parameters.AddWithValue("@Email", email)
                cmd.Parameters.AddWithValue("@Phone", phone)
                cmd.Parameters.AddWithValue("@AddressLine", addressLine)
                cmd.Parameters.AddWithValue("@City", city)
                cmd.Parameters.AddWithValue("@ZipCode", zipCode)
                cmd.Parameters.AddWithValue("@Country", country)
                cmd.Parameters.AddWithValue("@CreatedBy", 1) ' Replace with actual created_by ID
                cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now)

                cmd.ExecuteNonQuery()
                MessageBox.Show("Vendor entry successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

                Textbox_VendorDetailVendorNamenputNewVendor.Text = ""
                Textbox_VendorDetailContactPersonFirstNameInputNewVendor.Text = ""
                Textbox_VendorDetailContactPersonMiddleNameInputNewVendor.Text = ""
                Textbox_VendorDetailContactPersonLastNameInputNewVendor.Text = ""
                Textbox_VendorDetailVendorEmailInputNewVendor.Text = ""
                Textbox_VendorDetailVendorPhoneInputNewVendor.Text = ""
                Textbox_VendorDetailVendorAddressLineInputNewVendor.Text = ""
                Textbox_VendorDetailVendorCityInputNewVendor.Text = ""
                Textbox_VendorDetailVendorZipCodeInputNewVendor.Text = ""
                Textbox_VendorDetailVendorCountryInputNewVendor.Text = ""
            Catch ex As MySqlException
                MessageBox.Show("Vendor entry failed: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub Button_BackNewVendor_Click(sender As Object, e As EventArgs) Handles Button_BackNewVendor.Click
        Dim Window_Vendors As New Window_Vendors()
        Window_Vendors.Show()
        Me.Close()
    End Sub
End Class