Public Class Item
    Public Property ItemId As Integer
    Public Property ItemName As String
    Public Property AvailableQuantity As Integer
    Public Property UnavailableQuantity As Integer
    Public Property SellingPrice As Decimal
    Public Property BrandId As Integer
    Public Property CategoryId As Integer

    Public Sub New(itemId As Integer, itemName As String, availableQuantity As Integer, unavailableQuantity As Integer, sellingPrice As Decimal, brandId As Integer, categoryId As Integer)
        Me.ItemId = itemId
        Me.ItemName = itemName
        Me.AvailableQuantity = availableQuantity
        Me.UnavailableQuantity = unavailableQuantity
        Me.SellingPrice = sellingPrice
        Me.BrandId = brandId
        Me.CategoryId = categoryId
    End Sub

    Public Sub New()
    End Sub

    Public Sub DisplayDetails()
        Console.WriteLine("Item ID: " & ItemId)
        Console.WriteLine("Item Name: " & ItemName)
        Console.WriteLine("Available Quantity: " & AvailableQuantity)
        Console.WriteLine("Unavailable Quantity: " & UnavailableQuantity)
        Console.WriteLine("Selling Price: " & SellingPrice.ToString("C"))
        Console.WriteLine("Brand ID: " & BrandId)
        Console.WriteLine("Category ID: " & CategoryId)
    End Sub
End Class
