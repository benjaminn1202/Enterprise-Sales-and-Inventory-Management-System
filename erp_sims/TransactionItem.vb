Public Class TransactionItem
    Public Property TransactionItemId As Integer
    Public Property TransactionId As Integer
    Public Property ItemId As Integer
    Public Property TransactionItemQuantity As Integer
    Public Property TransactionItemSellingPrice As Decimal
    Public Property TransactionItemAmount As Decimal
    Public Property StockId As Integer

    Public Sub New()
    End Sub

    Public Sub New(transactionItemId As Integer, transactionId As Integer, itemId As Integer, transactionItemQuantity As Integer, transactionItemSellingPrice As Decimal, transactionItemAmount As Decimal, stockId As Integer)
        Me.TransactionItemId = transactionItemId
        Me.TransactionId = transactionId
        Me.ItemId = itemId
        Me.TransactionItemQuantity = transactionItemQuantity
        Me.TransactionItemSellingPrice = transactionItemSellingPrice
        Me.TransactionItemAmount = transactionItemAmount
        Me.StockId = stockId
    End Sub
End Class
