Imports System.Drawing
Imports System.IO
Imports Tributario.WebServicos.GeraString

Public Class frmRandomImage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim s As String
        s = GenerateRandomString(5)
        Dim hMACIString As String = Hash(s)

        'Guarda o resultado em um cookie.
        Dim c As HttpCookie = New HttpCookie("hMACIString")
        c.Value = hMACIString
        Dim dtNow As DateTime = DateTime.Now
        Dim tsYear As New TimeSpan(365, 0, 0, 0)
        c.Expires = dtNow.Add(tsYear)
        Response.Cookies.Add(c)

        Call DrawStringImage(s)
    End Sub

    Private Sub DrawStringImage(ByVal ds As String)
        Try

            Dim rRandom As New Random(System.DateTime.Now.Millisecond)
            Dim afBKGIMG As String()
            Dim fBKGIMG As String = String.Empty
            'Aqui eu configuro o fundo da imagem.
            afBKGIMG = Directory.GetFiles(Server.MapPath("BKGIMG"))
            Do While True
                fBKGIMG = afBKGIMG(rRandom.Next(0, afBKGIMG.GetLength(0)))
                If fBKGIMG.Substring(fBKGIMG.Length - 3, 3).ToUpper = "JPG" Then
                    Exit Do
                End If
            Loop

            Dim bmap As Bitmap = New Bitmap(fBKGIMG)
            Dim g As Graphics = Graphics.FromImage(bmap)
            Dim drawFont As New Font("Courier New", 9, FontStyle.Bold + FontStyle.Italic, GraphicsUnit.Millimeter)
            Dim drawBrush As New SolidBrush(Color.Red)
            Dim xPos As Integer = 0
            Dim yPos As Integer = 0
            g.DrawString(ds, drawFont, drawBrush, xPos, yPos)

            Dim pen As Pen = New Pen(Color.Blue, 1.5)
            Dim points As PointF() = {New PointF(rRandom.Next(0, bmap.Width), rRandom.Next(0, bmap.Height)), _
            New PointF(rRandom.Next(0, bmap.Width), rRandom.Next(0, bmap.Height)), _
            New PointF(rRandom.Next(0, bmap.Width), rRandom.Next(0, bmap.Height))}
            g.DrawLines(pen, points)

            Response.ContentType = "image/jpeg"
            bmap.Save(Response.OutputStream, Imaging.ImageFormat.Jpeg)
            g.Dispose()
            bmap.Dispose()
        Catch ex As Exception

        End Try

    End Sub
End Class