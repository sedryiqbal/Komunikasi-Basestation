Imports System.Net
Imports System.IO
Imports System.Text.Encoding
Imports System.Net.Sockets

Public Class Form1
    Dim publisher As New Sockets.UdpClient(0)
    Dim subscriber As New Sockets.UdpClient(28097)
    Dim TCPClientz As Sockets.TcpClient
    Dim TCPClientStream As Sockets.NetworkStream

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, pictureBox1.Click
        subscriber.Client.ReceiveTimeout = 100
        subscriber.Client.Blocking = False
    End Sub

    'Koneksi Referee Box'

    Private Sub Connect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TBConnect.Click
        TCPClientz = New Sockets.TcpClient(TBipC.Text, TBportC.Text)
        Timer4.Enabled = True
        TCPClientStream = TCPClientz.GetStream()

    End Sub

    'Data yang di terima dari Refeere Box d teruskan ke Robot'

    Private Sub Timer4_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer4.Tick

        If TCPClientStream.DataAvailable = True Then
            Dim rcvbytes(TCPClientz.ReceiveBufferSize) As Byte
            TCPClientStream.Read(rcvbytes, 0, CInt(TCPClientz.ReceiveBufferSize))
            TBrb.Text = System.Text.Encoding.ASCII.GetString(rcvbytes)
            TBRobotP.Text = System.Text.Encoding.ASCII.GetString(rcvbytes)
            TBRobotD.Text = System.Text.Encoding.ASCII.GetString(rcvbytes)
            TBRobotK.Text = System.Text.Encoding.ASCII.GetString(rcvbytes)
        End If
    End Sub

    'Basestation mengirim data ke Robot Penyerang'

    Private Sub TBRobotP_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TBRobotP.TextChanged
        publisher.Connect(TBipP.Text, TBportP.Text)
        Dim sendbytes() As Byte = ASCII.GetBytes(TBRobotP.Text)
        publisher.Send(sendbytes, sendbytes.Length)
        Timer1.Start()
    End Sub

    'Connect Robot Penyerang'

    '  Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged

    'Try
    '   If (CheckBox1.Checked = True) Then


    '            Label39.Text = "Connected"

    '       Else

    '            Label39.Text = "Disconnected"
    '       End If
    '  Catch

    '    End Try


    'End Sub

    'Basestation Menerima Data dari Robot Penyerang ' 

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Try
            Dim ep As IPEndPoint = New IPEndPoint(IPAddress.Any, 0)
            Dim rcvbytes() As Byte = subscriber.Receive(ep)
            TBDSP.Text = ASCII.GetString(rcvbytes)
            If TBDSP.Text.StartsWith(">") Then
                TBDRP.Text = ASCII.GetString(rcvbytes)
            End If
            If TBDSP.Text.StartsWith("<") Then
                TBDCP.Text = ASCII.GetString(rcvbytes)
            End If
            If TBDSP.Text.StartsWith("!") Then
                TBDBD.Text = ASCII.GetString(rcvbytes)
            End If
            If TBDSP.Text.StartsWith("&") Then
                TBDBA.Text = ASCII.GetString(rcvbytes)
            End If
        Catch ex As Exception

        End Try

    End Sub

    'Data yang di pisah untuk di kirim ke R'

    Private Sub TBDCP_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TBDCP.TextChanged
        Dim text() As String
        text = Split(TBDCP.Text, ":")
        TBrcvP.Text = text(1)
        TBRobotD.Text = text(1)
    End Sub

    ' Data yang di terima dari Robot P di pisah untuk di kirimkan ke X dan Y

    Private Sub TBDRP_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TBDRP.TextChanged
        Dim text() As String
        text = Split(TBDRP.Text, ":")
        TBXP.Text = text(1)
        TBYP.Text = text(2)
    End Sub

    ' Data yang di terima dari Robot P di pisah untuk di kirimkan ke x dan y

    Private Sub TBDBD_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TBDBD.TextChanged
        Dim text() As String
        text = Split(TBDBD.Text, ":")
        TBxbp.Text = text(1)
        TBybp.Text = text(2)
    End Sub


    ' Data yang di terima dari Robot P di pisah untuk di kirimkan ke v dan w

    Private Sub TBDBA_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TBDBA.TextChanged
        Dim text() As String
        text = Split(TBDBA.Text, ":")
        TBvbp.Text = text(1)
        TBwbp.Text = text(2)
    End Sub

    'Data dari in X in Y di kirim ke TBTujuanP

    Private Sub TBsendP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TBsendP.Click
        'TBTujuanP.Text = String.Concat(TBINXP.Text, TBINYP.Text)'
        TBTujuanP.Text = ("*") + TBINXP.Text + (":") + TBINYP.Text + ("#")
    End Sub

    'Data dari TBTujuanP  di kirim ke Robot P'

    Private Sub TBTujuanP_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TBTujuanP.TextChanged
        publisher.Connect(TBipP.Text, TBportP.Text)
        Dim sendbytes() As Byte = ASCII.GetBytes(TBTujuanP.Text)
        publisher.Send(sendbytes, sendbytes.Length)
    End Sub

    'Menghapus data IN X IN Y TBTujuanP

    Private Sub Bclear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Bclear.Click
        TBINXP.Text = " "
        TBINYP.Text = " "
        TBTujuanP.Text = " "
    End Sub


    'Basestation Mengirim Data ke Robot Defender'

    Private Sub TBRobotD_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TBRobotD.TextChanged
        publisher.Connect(TBipD.Text, TBportD.Text)
        Dim sendbytes() As Byte = ASCII.GetBytes(TBRobotD.Text)
        publisher.Send(sendbytes, sendbytes.Length)
        Timer2.Start()
    End Sub

    'Basestation Menerima Data dari Robot Defender'

    Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick
        Try

            Dim ep As IPEndPoint = New IPEndPoint(IPAddress.Any, 0)
            Dim rcvbytes() As Byte = subscriber.Receive(ep)
            TBDSD.Text = ASCII.GetString(rcvbytes)
            If TBDSD.Text.StartsWith(">") Then
                TBDRD.Text = ASCII.GetString(rcvbytes)
            ElseIf TBDSD.Text.StartsWith("<") Then
                TBDCD.Text = ASCII.GetString(rcvbytes)
            End If
        Catch ex As Exception

        End Try

    End Sub

    'Data yang di pisah untuk di kirim ke R

    Private Sub TBDCD_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TBDCD.TextChanged
        Dim text() As String
        text = Split(TBDCD.Text, ":")
        TBrcvD.Text = text(1)
    End Sub

    ' Data yang di terima dari Robot D di pisah untuk di kirimkan ke X dan Y

    Private Sub TBDRD_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TBDRD.TextChanged
        Dim text() As String
        text = Split(TBDRD.Text, ":")
        TBXD.Text = text(1)
        TBYD.Text = text(2)
    End Sub

    'Data dari in X in Y di kirim ke TBTujuanD

    Private Sub TBsendD_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TBsendD.Click
        TBTujuanD.Text = ("*") + TBINXD.Text + (":") + TBINYD.Text + ("#")
    End Sub

    'Data dari TBTujuanD di kirim ke Robot D

    Private Sub TBTujuanD_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TBTujuanD.TextChanged
        publisher.Connect(TBipD.Text, TBportD.Text)
        Dim sendbytes() As Byte = ASCII.GetBytes(TBTujuanD.Text)
        publisher.Send(sendbytes, sendbytes.Length)
    End Sub

    'Menghapus data di IN X IN Y TBTujuanD 

    Private Sub ClearD_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClearD.Click
        TBINXD.Text = " "
        TBINYD.Text = " "
        TBTujuanD.Text = " "
    End Sub


    'Basestation Mengirim Data ke Robot Kiper'

    Private Sub TBRobotK_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TBRobotK.TextChanged
        publisher.Connect(TBipK.Text, TBportK.Text)
        Dim sendbytes() As Byte = ASCII.GetBytes(TBRobotK.Text)
        publisher.Send(sendbytes, sendbytes.Length)
        Timer3.Start()
    End Sub

    'Basestation Menerima data dari Robot Kiper'

    Private Sub Timer3_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer3.Tick
        Try
            Dim ep As IPEndPoint = New IPEndPoint(IPAddress.Any, 0)
            Dim rcvbytes() As Byte = subscriber.Receive(ep)
            TBrcvK.Text = ASCII.GetString(rcvbytes)
        Catch ex As Exception

        End Try

    End Sub

    Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button10.Click
        TBRobotP.Text = TBkoC.Text
        TBRobotD.Text = TBkoC.Text

    End Sub

    Private Sub Button15_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button15.Click
        TBRobotP.Text = TBfkC.Text
        TBRobotD.Text = TBfkC.Text
    End Sub

    Private Sub Button14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button14.Click
        TBRobotP.Text = TBgkC.Text
        TBRobotD.Text = TBgkC.Text
    End Sub

    Private Sub Button13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button13.Click
        TBRobotP.Text = TBtiC.Text
        TBRobotD.Text = TBtiC.Text
    End Sub

    Private Sub Button11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button11.Click
        TBRobotP.Text = TBcrC.Text
        TBRobotD.Text = TBcrC.Text
    End Sub

    Private Sub Button12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button12.Click
        TBRobotP.Text = TBpC.Text
        TBRobotD.Text = TBpC.Text
    End Sub

    Private Sub Button16_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button16.Click
        TBRobotP.Text = TBs.Text
        TBRobotD.Text = TBs.Text
        TBRobotK.Text = TBs.Text
    End Sub

    Private Sub Button17_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button17.Click
        TBRobotP.Text = TBSt.Text
        TBRobotD.Text = TBSt.Text
        TBRobotK.Text = TBSt.Text
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        TBRobotP.Text = TBdb.Text
        TBRobotD.Text = TBdb.Text
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        TBRobotP.Text = TBpa.Text
        TBRobotD.Text = TBpa.Text
    End Sub

    Private Sub Button18_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button18.Click
        TBRobotP.Text = TBe.Text
        TBRobotD.Text = TBe.Text
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        TBRobotP.Text = TBr.Text
        TBRobotD.Text = TBr.Text
        TBRobotK.Text = TBr.Text
    End Sub

    Private Sub Button27_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button27.Click
        TBRobotP.Text = TBkoM.Text
        TBRobotD.Text = TBkoM.Text
    End Sub

    Private Sub Button22_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button22.Click
        TBRobotP.Text = TBfkM.Text
        TBRobotD.Text = TBfkM.Text
    End Sub

    Private Sub Button23_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button23.Click
        TBRobotP.Text = TBgkM.Text
        TBRobotD.Text = TBgkM.Text
    End Sub

    Private Sub Button24_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button24.Click
        TBRobotP.Text = TBtiM.Text
        TBRobotD.Text = TBtiM.Text
    End Sub

    Private Sub Button26_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button26.Click
        TBRobotP.Text = TBcrM.Text
        TBRobotD.Text = TBcrM.Text
    End Sub

    Private Sub Button25_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button25.Click
        TBRobotP.Text = TBpM.Text
        TBRobotD.Text = TBpM.Text
    End Sub

    Private Sub Timer5_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer5.Tick
        RobotP.Location = New Point(TBYP.Text, TBXP.Text)
        RobotD.Location = New Point(TBYD.Text, TBXD.Text)
        Bola.Location = New Point(TBwbp.Text, TBvbp.Text)

    End Sub

    'Mengaktifkan Pergerakan Robot pada Lapangan di basestation'

    Private Sub CheckBox2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox2.CheckedChanged
        Try
            If (CheckBox2.Checked = True) Then

                Timer5.Enabled = True
                Label42.Text = "Aktif"

            Else

                Timer5.Enabled = False
                Label42.Text = "Nonaktif"
            End If
        Catch

        End Try
    End Sub

    Private Sub pictureBox1_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pictureBox1.MouseMove
        Label47.Text = "X = " & e.X & " ; Y = " & e.Y
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        TBRobotP.Text = " "
        TBRobotD.Text = " "
        TBrcvD.Text = " "
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        TBRobotD.Text = " "
    End Sub

End Class


