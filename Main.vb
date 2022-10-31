Imports System.Net
Imports Newtonsoft.Json

Module Main

    Sub Main()
        Console.WriteLine("Enter Wallet Address:")
        Dim walletAddress = Console.ReadLine()
        VerifyWallet(walletAddress)
    End Sub

    Sub VerifyWallet(walletAddress)
        Dim WebClient As New WebClient
        Dim result = WebClient.DownloadString("https://api.runonflux.io/daemon/validateaddress?zelcashaddress=" + walletAddress)
        Dim resultObject = JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(result)

        If resultObject.Item("data").Item("isvalid") Then
            GetNodeInfo(walletAddress)
        End If
    End Sub

    Sub GetNodeInfo(walletAddress)

    End Sub

End Module
