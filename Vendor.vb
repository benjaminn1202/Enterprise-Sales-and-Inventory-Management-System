Public Class Vendor
    Public Property VendorId As Integer
    Public Property VendorName As String
    Public Property ContactPerson As String
    Public Property Email As String
    Public Property Phone As String
    Public Property AddressLine As String
    Public Property City As String
    Public Property ZipCode As String
    Public Property Country As String
    Public Property CreatedBy As Integer
    Public Property CreatedAt As DateTime

    Public Sub New(vendorId As Integer, vendorName As String, contactPerson As String, email As String, phone As String, addressLine As String, city As String, zipCode As String, country As String, createdBy As Integer, createdAt As DateTime)
        Me.VendorId = vendorId
        Me.VendorName = vendorName
        Me.ContactPerson = contactPerson
        Me.Email = email
        Me.Phone = phone
        Me.AddressLine = addressLine
        Me.City = city
        Me.ZipCode = zipCode
        Me.Country = country
        Me.CreatedBy = createdBy
        Me.CreatedAt = createdAt
    End Sub

    Public Sub New()
    End Sub

    Public Sub DisplayDetails()
        Console.WriteLine("Vendor ID: " & VendorId)
        Console.WriteLine("Vendor Name: " & VendorName)
        Console.WriteLine("Contact Person: " & ContactPerson)
        Console.WriteLine("Email: " & Email)
        Console.WriteLine("Phone: " & Phone)
        Console.WriteLine("Address Line: " & AddressLine)
        Console.WriteLine("City: " & City)
        Console.WriteLine("Zip Code: " & ZipCode)
        Console.WriteLine("Country: " & Country)
        Console.WriteLine("Created By: " & CreatedBy)
        Console.WriteLine("Created At: " & CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"))
    End Sub
End Class
