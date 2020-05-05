Imports System.Data
Imports System.IO
Imports System.Data.OleDb
Imports MySql.Data.MySqlClient
Public Class uploadQuestion

    'connection of string to the excel 
    Private Excel03ConString As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'"
    Private Excel07ConString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'"
    Dim Query As String
    Dim mysqlconn As MySqlConnection
    Dim command As MySqlCommand
    Dim send As Boolean
    Dim Filepath As String
    Dim CurrentYear As Integer = System.DateTime.Now.Year
    Dim Combination As String
    Dim Scode As String
    Private Sub UploadQ_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Year.Items.Add(CurrentYear)
        Me.WindowState = FormWindowState.Maximized
        Me.WindowState = FormWindowState.Normal
        MaximizeBox = False

        mysqlconn = New MySqlConnection
        mysqlconn.ConnectionString = "server=localhost;user=root;database=Teacherpanel;port=3306;password=;"
        Dim Reader As MySqlDataReader
        Dim Data As String
        Try
            mysqlconn.Open()
            Dim Query As String = "Select * from  Teacherpanel.Course"
            command = New MySqlCommand(Query, mysqlconn)
            Reader = command.ExecuteReader
            While Reader.Read
                Data = Reader.GetString("Coursename")
                Course.Items.Add(Data)
            End While

            'Semester Detali

        Catch ex As MySqlException
            MessageBox.Show(ex.Message)
        Finally
            mysqlconn.Close()
            mysqlconn.Dispose()
        End Try

    End Sub
    Private Sub Course_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Course.SelectedIndexChanged
        Semester.Items.Clear()
        mysqlconn = New MySqlConnection
        mysqlconn.ConnectionString = "server=localhost;user=root;database=Teacherpanel;port=3306;password=;"
        Dim Reader As MySqlDataReader
        Dim Data As Integer
        Dim temp As Integer = 1
        Try
            mysqlconn.Open()
            Dim Query As String = "Select * from  Teacherpanel.course where CourseName='" & Course.SelectedItem & "'"
            command = New MySqlCommand(Query, mysqlconn)
            Reader = command.ExecuteReader
            While Reader.Read
                Data = Reader.GetDecimal("NoOfSemester")
            End While
        Catch ex As MySqlException
            MessageBox.Show(ex.Message)
        Finally
            mysqlconn.Close()
            mysqlconn.Dispose()
        End Try

        While temp <= Data
            Semester.Items.Add(temp)
            temp = temp + 1
        End While
    End Sub
    Private Sub Semeter_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Semester.SelectedIndexChanged
        mysqlconn = New MySqlConnection
        mysqlconn.ConnectionString = "server=localhost;user=root;database=Teacherpanel;port=3306;password=;"
        Dim Reader As MySqlDataReader
        Dim Data As String

        Try
            mysqlconn.Open()
            Dim Query As String = "Select * from  Teacherpanel.Subject where Course='" & Course.SelectedItem & "' AND Semester=" & Semester.SelectedItem & ";"
            command = New MySqlCommand(Query, mysqlconn)
            Reader = command.ExecuteReader
            While Reader.Read
                Data = Reader.GetString("SubjectName")
                Scode = Reader.GetString("SubjectCode")
                Subject.Items.Add(Data)
            End While
        Catch ex As MySqlException
            MessageBox.Show(ex.Message)
        Finally
            mysqlconn.Close()
            mysqlconn.Dispose()
        End Try
    End Sub

    Private Sub Submit_Click(sender As Object, e As EventArgs) Handles Submit.Click


        'Connection for QuestionTableBank
        Dim mysqlconnforqtb As New MySqlConnection
        Dim mysqlcommandforqtb As MySqlCommand
        Dim Queryforqtb As String
        mysqlconnforqtb.ConnectionString = "server=localhost;user=root;database=Teacherpanel;port=3306;password="
        'end

        'Combination of all feilds in upload question panel
        Combination = Course.SelectedItem + Semester.SelectedItem.ToString + Scode + Year.SelectedItem.ToString
        Combination = Combination.Replace(" ", "")


        'Creating table from Combination

        MessageBox.Show(Combination)
        If OpenDB() Then
            If Not IsTableExists(Combination) Then

                Dim command As MySqlCommand
                mysqlconn = New MySqlConnection
                mysqlconn.ConnectionString = "server=localhost;user=root;database=Teacherpanel;port=3306;password=;"
                Try
                    mysqlconn.Open()
                    Dim Query As String = "CREATE TABLE IF NOT EXISTS `" & Combination & "` (
    QuestionNumber  integer,
    Question varchar(255) NOT NULL,	Op1 varchar(255),Op2 varchar(255),Op3 varchar(255),Op4 varchar(255),CorrentAnswer  varchar(255)
    ,PRIMARY KEY (QuestionNumber)
);"
                    MessageBox.Show("created")
                    command = New MySqlCommand(Query, mysqlconn)
                    command.ExecuteNonQuery()
                Catch ex As MySqlException
                    MessageBox.Show(ex.Message)
                Finally
                    mysqlconn.Close()
                    mysqlconn.Dispose()
                End Try
            End If
        End If


        'Inserting values into QuestionTableBank
        Try
            mysqlconnforqtb.Open()
            Queryforqtb = "insert into Teacherpanel.questiontablebank(Tname,Course,Semester,SubjectCode) values('" & Combination & "','" & Course.SelectedItem & "','" & Semester.SelectedItem & "','" & Scode & "' ) "
            mysqlcommandforqtb = New MySqlCommand(Queryforqtb, mysqlconnforqtb)
            mysqlcommandforqtb.ExecuteNonQuery()
        Catch ex As MySqlException
            MessageBox.Show(ex.Message)

        Finally
            mysqlconnforqtb.Close()
            mysqlconnforqtb.Dispose()
        End Try



    End Sub
    'File Path
    Private Sub Select_file_btn_1(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.InitialDirectory = ""
        OpenFileDialog1.Title = "select file"
        OpenFileDialog1.ShowDialog()
        Filepath = OpenFileDialog1.FileName
        MessageBox.Show(Filepath)
    End Sub
End Class