Imports System.Net
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

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
            Dim ipList = GetNodeInfo(walletAddress)

            If ipList.Count > 0 Then
                For Each ip In ipList
                    GetNodeBenchMarks(ip)
                Next
            End If
        End If

    End Sub

    Function GetNodeInfo(walletAddress)
        Dim WebClient As New WebClient
        Dim result = WebClient.DownloadString("https://api.runonflux.io/daemon/listzelnodes?filter=" + walletAddress)
        Dim resultObject = JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(result)
        Dim ipList = New ArrayList

        If resultObject.Item("data").Count > 0 Then
            For Each node In resultObject.Item("data")
                Dim ipAddress As String = node.Item("ip")

                If ipAddress.Contains(":") Then
                    ipList.Add(ipAddress)
                Else
                    ipAddress &= ":16127"
                    ipList.Add(ipAddress)
                End If
            Next
        End If
        GetNodeInfo = ipList
    End Function

    Sub GetNodeBenchMarks(ip)
        Dim WebClient As New WebClient
        Dim result = WebClient.DownloadString(String.Format("http://{0}/daemon/getbenchmarks", ip))
        Dim resultObject = JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(result)
        Dim benchmarksResult = JToken.Parse(resultObject.Item("data"))
        Dim status = benchmarksResult.Item("status")

        If status = "failed" Then
            Console.WriteLine("Failed")
        Else
            Console.WriteLine("Passed")
        End If
        Console.ReadKey()
    End Sub

End Module
