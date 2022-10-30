Imports System.Net

Module Main

    Sub Main()
        Console.WriteLine("Enter Wallet Address:")
        Dim walletAddress = Console.ReadLine()
        VerifyWallet(walletAddress)
    End Sub

    Sub VerifyWallet(walletAddress)
        Dim WebClient As New WebClient
        Dim result = WebClient.DownloadString("https://api.runonflux.io/daemon/validateaddress?zelcashaddress=" + walletAddress)
        Console.WriteLine(result)
        Console.ReadLine()
    End Sub

End Module
