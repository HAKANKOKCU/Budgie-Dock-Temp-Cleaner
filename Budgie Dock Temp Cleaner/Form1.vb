Public Class Form1

    Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        BackgroundWorker1.ReportProgress(0)
        Dim filestonotdelete As New ArrayList
        For Each ff As String In My.Computer.FileSystem.GetFiles(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock")
            Dim nm = My.Computer.FileSystem.GetName(ff)
            If nm.StartsWith("Icon") And nm.EndsWith(".data") Then
                For Each ic As String In My.Computer.FileSystem.ReadAllText(ff).Split("|")
                    If ic.Split(":")(0) = "icon" Then
                        filestonotdelete.Add(ic.Split("*")(1).Replace("{BD-TD-}", ":"))
                    End If
                Next
            End If
        Next
        Dim fis = My.Computer.FileSystem.GetFiles(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BudgieDock\Icons")
        Dim ci = 0
        Dim cleanedsize = 0
        For Each dd As String In fis
            ci += 1
            Try
                If Not filestonotdelete.Contains(dd) Then
                    Dim x As New IO.FileInfo(dd)
                    cleanedsize += x.Length
                    My.Computer.FileSystem.DeleteFile(dd)
                    BackgroundWorker1.ReportProgress((ci / fis.Count) * 100, cleanedsize.ToString)
                End If
            Catch
            End Try
        Next
        BackgroundWorker1.ReportProgress(100, cleanedsize)
    End Sub

    Private Sub BackgroundWorker1_ProgressChanged(ByVal sender As System.Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
        ProgressBar1.Value = e.ProgressPercentage
        ProgressBar1.Invalidate()
        Label2.Text = e.UserState & " Bytes Cleaned"
        Label2.Invalidate()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        BackgroundWorker1.RunWorkerAsync()
        Button1.Enabled = False
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As System.Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        Button1.Enabled = True
    End Sub
End Class
