Public Class TransactionEntry
    Public Property TransactionId As Integer
    Public Property CustomerId As Integer
    Public Property Subtotal As Decimal
    Public Property NetTotal As Decimal
    Public Property ReceivedAmount As Decimal
    Public Property ChangeAmount As Decimal
    Public Property TaxId As Integer
    Public Property TaxAmount As Decimal
    Public Property PaymentTermId As Integer
    Public Property DueAmount As Decimal
    Public Property DueDate As Date
    Public Property Status As String
    Public Property CreatedAt As DateTime
    Public Property CreatedBy As Integer

    Public Sub New()
    End Sub

    Public Sub New(transactionId As Integer, customerId As Integer, subtotal As Decimal, netTotal As Decimal, receivedAmount As Decimal, changeAmount As Decimal, taxId As Integer, taxAmount As Decimal, paymentTermId As Integer, dueAmount As Decimal, dueDate As Date, status As String, createdAt As DateTime, createdBy As Integer)
        Me.TransactionId = transactionId
        Me.CustomerId = customerId
        Me.Subtotal = subtotal
        Me.NetTotal = netTotal
        Me.ReceivedAmount = receivedAmount
        Me.ChangeAmount = changeAmount
        Me.TaxId = taxId
        Me.TaxAmount = taxAmount
        Me.PaymentTermId = paymentTermId
        Me.DueAmount = dueAmount
        Me.DueDate = dueDate
        Me.Status = status
        Me.CreatedAt = createdAt
        Me.CreatedBy = createdBy
    End Sub

    Public Sub DisplayDetails()
        Console.WriteLine($"Transaction ID: {TransactionId}")
        Console.WriteLine($"Customer ID: {CustomerId}")
        Console.WriteLine($"Subtotal: {Subtotal}")
        Console.WriteLine($"Net Total: {NetTotal}")
        Console.WriteLine($"Received Amount: {ReceivedAmount}")
        Console.WriteLine($"Change Amount: {ChangeAmount}")
        Console.WriteLine($"Tax ID: {TaxId}")
        Console.WriteLine($"Tax Amount: {TaxAmount}")
        Console.WriteLine($"Payment Term ID: {PaymentTermId}")
        Console.WriteLine($"Due Amount: {DueAmount}")
        Console.WriteLine($"Due Date: {DueDate}")
        Console.WriteLine($"Status: {Status}")
        Console.WriteLine($"Created At: {CreatedAt}")
        Console.WriteLine($"Created By: {CreatedBy}")
    End Sub

    Public Function CheckNullProperties() As List(Of String)
        Dim nullProperties As New List(Of String)()

        ' Check if any property is null or invalid
        If CustomerId = Nothing Then nullProperties.Add("CustomerId")
        If ReceivedAmount = Nothing Then nullProperties.Add("ReceivedAmount")
        If TaxId  = Nothing Then nullProperties.Add("TaxId")
        If PaymentTermId = Nothing Then nullProperties.Add("PaymentTermId")

        Return nullProperties
    End Function
End Class
