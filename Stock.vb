Public Class Stock
    Public Property StockId As Integer
    Public Property StockEntryId As Integer
    Public Property StockQuantity As Integer
    Public Property ItemBuyingPrice As Decimal
    Public Property TotalBuyingPrice As Decimal
    Public Property ItemId As Integer
    Public Property Expiry As DateTime?
    Public Property Status As String ' 'Available' or 'Unavailable'

    Public Sub New(stockId As Integer, stockEntryId As Integer, stockQuantity As Integer, itemBuyingPrice As Decimal, totalBuyingPrice As Decimal, itemId As Integer, expiry As DateTime?, status As String)
        Me.StockId = stockId
        Me.StockEntryId = stockEntryId
        Me.StockQuantity = stockQuantity
        Me.ItemBuyingPrice = itemBuyingPrice
        Me.TotalBuyingPrice = totalBuyingPrice
        Me.ItemId = itemId
        Me.Expiry = expiry
        Me.Status = status
    End Sub

    Public Sub New()
    End Sub

    Public Sub DisplayDetails()
        Console.WriteLine("Stock ID: " & StockId)
        Console.WriteLine("Stock Entry ID: " & StockEntryId)
        Console.WriteLine("Stock Quantity: " & StockQuantity)
        Console.WriteLine("Item Buying Price: " & ItemBuyingPrice.ToString("C"))
        Console.WriteLine("Total Buying Price: " & TotalBuyingPrice.ToString("C"))
        Console.WriteLine("Item ID: " & ItemId)
        If Expiry.HasValue Then
            Console.WriteLine("Expiry: " & Expiry.Value.ToString("yyyy-MM-dd"))
        Else
            Console.WriteLine("Expiry: N/A")
        End If
        Console.WriteLine("Status: " & Status)
    End Sub
End Class
