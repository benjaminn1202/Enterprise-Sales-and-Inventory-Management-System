Public Class StockEntry
    Public Property StockEntryId As Integer
    Public Property VendorId As Integer
    Public Property Status As String ' 'Approved' or 'Disapproved'
    Public Property CreatedAt As DateTime
    Public Property NetTotal As Double
    Public Property CreatedBy As Integer

    Public Sub New(stockEntryId As Integer, vendorId As Integer, status As String, netTotal As Double, createdAt As DateTime, createdBy As Integer)
        Me.StockEntryId = stockEntryId
        Me.VendorId = vendorId
        Me.Status = status
        Me.NetTotal = netTotal
        Me.CreatedAt = createdAt
        Me.CreatedBy = createdBy
    End Sub

    Public Sub New()
    End Sub

    Public Sub DisplayDetails()
        Console.WriteLine("Stock Entry ID: " & StockEntryId)
        Console.WriteLine("Vendor ID: " & VendorId)
        Console.WriteLine("Status: " & Status)
        Console.WriteLine("Created At: " & CreatedAt)
        Console.WriteLine("Created By: " & CreatedBy)
    End Sub
End Class
