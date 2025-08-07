Imports MySql.Data.MySqlClient

Public Class Window_NewSale
    Private Sub Button_BackNewSale_Click(sender As Object, e As EventArgs) Handles Button_BackNewSale.Click
        Dim Window_Sales As New Window_Sales()
        Window_Sales.Show()
        Me.Close()
    End Sub
End Class