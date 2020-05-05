Imports MySql.Data.MySqlClient

Module Connection

    Dim Mysqlconn As New MySqlConnection
    Dim Stconn As String
    Dim Result As String

    Function OpenDB() As Boolean
        Try
            If Mysqlconn.State = ConnectionState.Closed Then
                Stconn = "server=localhost;user=root;database=Teacherpanel;port=3306;password=;"
                Mysqlconn.ConnectionString = Stconn
                Mysqlconn.Open()
                Result = True
            End If
        Catch ex As MySqlException
            MessageBox.Show(ex.Message)
            Result = False
        End Try

        Return Result

    End Function

    Public Function IsTableExists(ByVal tableName As String)
        OpenDB()
        Dim restrictions(4) As String
        restrictions(2) = tableName
        Dim dbTbl As DataTable = Mysqlconn.GetSchema("Tables", restrictions)
        If dbTbl.Rows.Count = 0 Then


            Return False
        Else
            MsgBox("Table already exists", MsgBoxStyle.Critical)

            Return True
        End If
    End Function





End Module
