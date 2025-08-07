Public Class Account
    Public Property AccountId As Integer
    Public Property Email As String
    Public Property Password As String
    Public Property Type As String ' "sales_representative" or "admin"
    Public Property Name As String
    Public Property EmployeeId As String

    Public Sub New()
    End Sub

    Public Sub New(accountId As Integer, email As String, password As String, type As String, name As String, employeeId As String)
        Me.AccountId = accountId
        Me.Email = email
        Me.Password = password
        Me.Type = type
        Me.Name = name
        Me.EmployeeId = employeeId
    End Sub

    Public Sub DisplayDetails()
        Console.WriteLine("Account ID: " & AccountId)
        Console.WriteLine("Email: " & Email)
        Console.WriteLine("Password: " & Password)
        Console.WriteLine("Type: " & Type)
        Console.WriteLine("Name: " & Name)
        Console.WriteLine("Employee ID: " & EmployeeId)
    End Sub
End Class
