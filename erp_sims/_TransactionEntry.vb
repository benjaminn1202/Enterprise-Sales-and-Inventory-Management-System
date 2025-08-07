Public Class _TransactionEntry
    Public Property transaction_id As Integer
    Public Property customer_id As Integer
    Public Property subtotal As Decimal
    Public Property net_total As Decimal
    Public Property received_amount As Decimal
    Public Property change As Decimal
    Public Property tax_percentage As Decimal
    Public Property tax_amount As Decimal
    Public Property due_amount As Decimal
    Public Property due_date As DateTime
    Public Property status As String
    Public Property created_by As Integer
    Public Property created_at As DateTime
End Class
