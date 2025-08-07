Public Class Tax
    Public Property TaxId As Integer
    Public Property TaxName As String
    Public Property TaxPercentage As Decimal
    Public Property CreatedBy As Integer
    Public Property CreatedAt As DateTime

    Public Sub New()
    End Sub

    Public Sub New(taxId As Integer, taxName As String, taxPercentage As Decimal, createdBy As Integer, createdAt As DateTime)
        Me.TaxId = taxId
        Me.TaxName = taxName
        Me.TaxPercentage = taxPercentage
        Me.CreatedBy = createdBy
        Me.CreatedAt = createdAt
    End Sub
End Class
