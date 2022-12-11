Imports System.Configuration
Imports System.Net.Mail

Public Module EmailAlert
    Dim SmtpClient As New SmtpClient()
    Dim Message As New MailMessage()

    Sub SendEmail(ipList As List(Of String))
        For Each ip In ipList
            Message.Body += " Your node " & ip & " has failed benchmarks!" & vbNewLine
        Next

        Try
            Dim email = ConfigurationManager.AppSettings("email")
            Message.From = New MailAddress("fluxalert@flux.com", "FluxAlert")
            Message.To.Add(New MailAddress(email))
            Message.Subject = "Flux Node Alert"
            SmtpClient.Send(Message)
        Catch e As Exception
            Console.WriteLine(e.Message)
        End Try
    End Sub
End Module
