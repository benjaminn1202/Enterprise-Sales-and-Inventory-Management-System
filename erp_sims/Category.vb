Public Class Category
    Public Property CategoryId As Integer
    Public Property CategoryName As String
    Public Property CreatedAt As DateTime
    Public Property CreatedBy As Integer

    Public Sub New()
    End Sub

    Public Sub New(categoryId As Integer, categoryName As String, createdAt As DateTime, createdBy As Integer)
        Me.CategoryId = categoryId
        Me.CategoryName = categoryName
        Me.CreatedAt = createdAt
        Me.CreatedBy = createdBy
    End Sub

    Public Sub DisplayDetails()
        Console.WriteLine("Category ID: " & CategoryId)
        Console.WriteLine("Category Name: " & CategoryName)
        Console.WriteLine("Created At: " & CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"))
        Console.WriteLine("Created By: " & CreatedBy)
    End Sub
End Class