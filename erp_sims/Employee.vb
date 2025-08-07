Public Class Employee
    ' Properties matching the table columns
    Public Property EmployeeId As Integer
    Public Property LastName As String
    Public Property FirstName As String
    Public Property MiddleName As String
    Public Property Suffix As String
    Public Property Gender As String
    Public Property DateOfBirth As Date
    Public Property PhoneNumber As String
    Public Property Email As String
    Public Property Password As String
    Public Property EmployeeAddress As String
    Public Property DepartmentId As Integer?
    Public Property PositionId As Integer?
    Public Property HireDate As Date
    Public Property ExperienceLevel As String
    Public Property SalaryAdvance As Decimal
    Public Property Deductions As Decimal
    Public Property Commissions As Decimal
    Public Property Status As String
    Public Property CreatedAt As DateTime
    Public Property UpdatedAt As DateTime

    ' Default constructor
    Public Sub New()
    End Sub

    ' Constructor with all parameters
    Public Sub New(employeeId As Integer, lastName As String, firstName As String, middleName As String, suffix As String, 
                   gender As String, dateOfBirth As Date, phoneNumber As String, email As String, password As String, 
                   employeeAddress As String, departmentId As Integer?, positionId As Integer?, hireDate As Date, 
                   experienceLevel As String, salaryAdvance As Decimal, deductions As Decimal, commissions As Decimal, 
                   status As String, createdAt As DateTime, updatedAt As DateTime)

        Me.EmployeeId = employeeId
        Me.LastName = lastName
        Me.FirstName = firstName
        Me.MiddleName = middleName
        Me.Suffix = suffix
        Me.Gender = gender
        Me.DateOfBirth = dateOfBirth
        Me.PhoneNumber = phoneNumber
        Me.Email = email
        Me.Password = password
        Me.EmployeeAddress = employeeAddress
        Me.DepartmentId = departmentId
        Me.PositionId = positionId
        Me.HireDate = hireDate
        Me.ExperienceLevel = experienceLevel
        Me.SalaryAdvance = salaryAdvance
        Me.Deductions = deductions
        Me.Commissions = commissions
        Me.Status = status
        Me.CreatedAt = createdAt
        Me.UpdatedAt = updatedAt
    End Sub

    ' Constructor with minimal required fields
    Public Sub New(employeeId As Integer, lastName As String, firstName As String, gender As String, dateOfBirth As Date, 
                   phoneNumber As String, email As String, hireDate As Date, status As String)

        Me.EmployeeId = employeeId
        Me.LastName = lastName
        Me.FirstName = firstName
        Me.Gender = gender
        Me.DateOfBirth = dateOfBirth
        Me.PhoneNumber = phoneNumber
        Me.Email = email
        Me.HireDate = hireDate
        Me.Status = status
        Me.CreatedAt = DateTime.Now
        Me.UpdatedAt = DateTime.Now
    End Sub
End Class
