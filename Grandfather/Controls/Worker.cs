using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.IO;

namespace Grandfather.Controls
{
    class Worker
    {
        public string Destination = "";
        public Uri Uri;
        public string Output = "";
        public DocumentType Type = DocumentType.System;

        public event EventHandler OnNavigate;
        public event EventHandler OnSuccess;
        public event EventHandler OnError;

        public TcpClient Client;
        public Thread Thread;

        public void Navigate(string url)
        {
            Thread = new Thread(() =>
            {
                try
                {
                    Uri = new Uri(url);
                    Type = DocumentType.System;
                    Destination = url;
                    if (Client != null)
                    {
                        Client?.Close();
                    }

                    if (OnNavigate != null) OnNavigate(this, null);

                    if (Uri.Scheme == "gemini")
                    {
                        int port = 1965;
                        if (Uri.Port >= 0) port = Uri.Port;

                        Client = new TcpClient(Uri.Host, port);

                        using (SslStream stream = new SslStream(Client.GetStream(), false,
                            new RemoteCertificateValidationCallback(HandleServerCertificate), null))
                        {
                            stream.AuthenticateAsClient(Uri.Host);
                            stream.Write(Encoding.UTF8.GetBytes(Uri.AbsoluteUri + "\r\n"));

                            Output = ReadMessage(stream);
                        }

                        Client.Close();
                        Type = DocumentType.Gemini;

                        if (OnSuccess != null) OnSuccess(this, null);
                    }
                    else if (Uri.Scheme == "gopher")
                    {
                        int port = 70;
                        if (Uri.Port >= 0) port = Uri.Port;

                        Client = new TcpClient(Uri.Host, port);
                        NetworkStream stream = Client.GetStream();

                        Match type = new Regex(@"^\/?(.?)([^\?]*)\??(.*)").Match(Uri.LocalPath);

                        stream.Write(Encoding.UTF8.GetBytes(type.Groups[2].Value + (type.Groups[3].Value == "" ? "" : "\t" + type.Groups[3].Value) + "\r\n"));

                        Output = ReadMessage(stream);
                        Type = type.Groups[1].Value == "" || type.Groups[1].Value == "1" || type.Groups[1].Value == "7"
                            ? DocumentType.Gopher : DocumentType.Text;

                        Client.Close();

                        if (OnSuccess != null) OnSuccess(this, null);
                    }
                    else
                    {
                    }
                }
                catch (Exception e)
                {
                    if (Client == null) return;
                    if (OnError != null) OnError(this, new UnhandledExceptionEventArgs(e, false));
                }
            });
            Thread.Start();
        }

        string ReadMessage(Stream stream)
        {
            byte[] buffer = new byte[16384];
            StringBuilder message = new StringBuilder();
            int bytes = -1;
            while (bytes != 0)
            {
                bytes = stream.Read(buffer, 0, buffer.Length);
                Decoder decoder = Encoding.UTF8.GetDecoder();
                char[] chars = new char[decoder.GetCharCount(buffer, 0, bytes)];
                decoder.GetChars(buffer, 0, bytes, chars, 0);
                message.Append(chars);
                if (message.ToString().IndexOf("<EOF>") >= 0) break;
            }
            return message.ToString();
        }

        bool HandleServerCertificate(object sender, X509Certificate certificate,
            X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            X509Certificate2 cert2 = new X509Certificate2(certificate);
            return cert2.GetNameInfo(X509NameType.DnsName, false).ToLower() == Uri.Host;
        }
    }

    enum DocumentType
    {
        System,
        Gemini,
        Gopher,
        Text,
    }
}
