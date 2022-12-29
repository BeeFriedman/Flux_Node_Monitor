Imports System.Net
Imports System.Timers
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Module Main
    Private ipList As New List(Of String)
    Private ipFailList As New List(Of String)

    Sub Main()
        Console.WriteLine("Enter Wallet Address:")
        Dim walletAddress = Console.ReadLine()
        VerifyWallet(walletAddress)
        Dim Timer As New Timer
        Timer.AutoReset = True
        Timer.Interval = 60000
        AddHandler Timer.Elapsed, AddressOf tick
        Timer.Start()
        Console.ReadKey()
    End Sub

    Sub tick(ByVal sender As Object, ByVal e As ElapsedEventArgs)
        Console.WriteLine("Timer Elapsed")
        Monitor()
    End Sub

    Sub VerifyWallet(walletAddress)
        Dim WebClient As New WebClient
        Dim result = WebClient.DownloadString("https://api.runonflux.io/daemon/validateaddress?zelcashaddress=" + walletAddress)
        Dim resultObject = JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(result)

        If resultObject.Item("data").Item("isvalid") Then
            Console.WriteLine("Wallet address confirmed monitoring started!")
            GetNodeInfo(walletAddress)
        Else
            Console.WriteLine("Wrong wallet address program will shut down!")
        End If
    End Sub

    Sub GetNodeInfo(walletAddress)
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
    End Sub

    Sub GetNodeBenchMarks(ip)
        Try
            Dim WebClient As New WebClient
            Dim result = WebClient.DownloadString(String.Format("http://{0}/daemon/getbenchmarks", ip))
            Dim resultObject = JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(result)
            Dim benchmarksResult = JToken.Parse(resultObject.Item("data"))
            Dim status = benchmarksResult.Item("status")
            If status = "failed" Then
                ipFailList.Add(ip)
            End If
        Catch
            ipFailList.Add(ip)
        End Try
    End Sub

    Sub Monitor()
        If ipList.Count > 0 Then
            For Each ip In ipList
                GetNodeBenchMarks(ip)
            Next
        End If
        If ipFailList.Count > 0 Then
            SendEmail(ipFailList)
        End If
    End Sub

End Module
