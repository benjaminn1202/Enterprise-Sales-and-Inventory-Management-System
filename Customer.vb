Public Class Customer
    Public Property CustomerId As Integer
    Public Property Name As String
    Public Property PaymentTermId As Integer
    Public Property Company As String
    Public Property Email As String
    Public Property Phone As String
    Public Property AddressLine As String
    Public Property City As String
    Public Property ZipCode As String
    Public Property Country As String
    Public Property CreatedBy As Integer
    Public Property CreatedAt As DateTime

    Public Sub New()
    End Sub

    Public Sub New(customerId As Integer, name As String, paymentTermId As Integer, company As String, email As String, phone As String, addressLine As String, city As String, zipCode As String, country As String, createdBy As Integer, createdAt As DateTime)
        Me.CustomerId = customerId
        Me.Name = name
        Me.PaymentTermId = paymentTermId
        Me.Company = company
        Me.Email = email
        Me.Phone = phone
        Me.AddressLine = addressLine
        Me.City = city
        Me.ZipCode = zipCode
        Me.Country = country
        Me.CreatedBy = createdBy
        Me.CreatedAt = createdAt
    End Sub

End Class
