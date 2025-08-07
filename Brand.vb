Public Class Brand
    Public Property BrandId As Integer
    Public Property BrandName As String
    Public Property CreatedAt As DateTime
    Public Property CreatedBy As Integer

    Public Sub New()
    End Sub

    Public Sub New(brandId As Integer, brandName As String, createdAt As DateTime, createdBy As Integer)
        Me.BrandId = brandId
        Me.BrandName = brandName
        Me.CreatedAt = createdAt
        Me.CreatedBy = createdBy
    End Sub

    Public Sub DisplayDetails()
        Console.WriteLine("Brand ID: " & BrandId)
        Console.WriteLine("Brand Name: " & BrandName)
        Console.WriteLine("Created At: " & CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"))
        Console.WriteLine("Created By: " & CreatedBy)
    End Sub
End Class