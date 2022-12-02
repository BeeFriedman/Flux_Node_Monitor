Imports System.Configuration
Imports System.Net.Mail

Public Class EmailAlert
    Dim SmtpClient As New SmtpClient()
    Dim Message As New MailMessage()

    Sub SendEmail()
        Try
            Dim email = ConfigurationManager.AppSettings("email")
            Message.From = New MailAddress("fluxalert@flux.com", "FluxAlerts")
            Message.To.Add(New MailAddress(email, "test"))
            Message.Subject = "Flux Node Alert"
            Message.Body = "Your node is down!"
            SmtpClient.Send(Message)
        Catch e As Exception
            Console.WriteLine(e.Message)
        End Try
    End Sub
End Class
