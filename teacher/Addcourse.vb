Imports MySql.Data.MySqlClient
Public Class Addcourse
    Private Sub Addcourse_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.WindowState = FormWindowState.Maximized
        Me.WindowState = FormWindowState.Normal
        MaximizeBox = False
    End Sub
    Dim mysqlconn As MySqlConnection
    Dim command As MySqlCommand
    Dim previous As sessionexam

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        mysqlconn = New MySqlConnection
        mysqlconn.ConnectionString = "server=localhost;user=root;database=Teacherpanel;port=3306;password=;"
        Dim Reader As MySqlDataReader

        Try
            mysqlconn.Open()
            Dim Query As String = "insert into Teacherpanel.course(CourseName,NoOfSemester) values('" & Course.Text & "','" & Semester.Text & "')"
            command = New MySqlCommand(Query, mysqlconn)
            Reader = command.ExecuteReader
            MessageBox.Show("Course Updated!!")
            mysqlconn.Close()

        Catch ex As MySqlException
            MessageBox.Show(ex.Message)
        Finally
            mysqlconn.Dispose()
        End Try

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If previous Is Nothing Then
            previous = New sessionexam
        Else
            previous.Dispose()
            previous = New sessionexam
        End If

        previous.Show()
        Me.Close()
    End Sub
End Class