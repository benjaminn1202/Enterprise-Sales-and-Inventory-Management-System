Public Class PaymentTerm
    Public Property PaymentTermId As Integer
    Public Property TermName As String
    Public Property TermDays As Integer
    Public Property CreatedBy As Integer
    Public Property CreatedAt As DateTime

    Public Sub New()
    End Sub

    Public Sub New(paymentTermId As Integer, termName As String, termDays As Integer, createdBy As Integer, createdAt As DateTime)
        Me.PaymentTermId = paymentTermId
        Me.TermName = termName
        Me.TermDays = termDays
        Me.CreatedBy = createdBy
        Me.CreatedAt = createdAt
    End Sub
End Class