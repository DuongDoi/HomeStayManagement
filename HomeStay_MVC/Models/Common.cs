using Microsoft.Extensions.Configuration;
using System.IO;
using System.Net.Http;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System;

namespace HomeStay_MVC.ModelCommon
{
    public class Common
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(Common));
        public static string SocketUnSyn(string msg, string host, int port, int timeout)
        {
            //log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            //log4net.Config.XmlConfigurator.Configure();

            string result = "-1000";
            TcpClient client = null;
            NetworkStream stream = null;
            try
            {



                byte[] data = System.Text.Encoding.ASCII.GetBytes(msg);

                client = new TcpClient(host, port);

                client.SendTimeout = timeout;
                stream = client.GetStream();
                BinaryWriter writer = new BinaryWriter(stream);
                writer.Write(data);




                String _readFromStream = String.Empty;
                byte[] data_read = new byte[1024];
                using (MemoryStream ms = new MemoryStream())
                {

                    int numBytesRead;
                    while ((numBytesRead = stream.Read(data_read, 0, data_read.Length)) > 0)
                    {
                        ms.Write(data_read, 0, numBytesRead);


                    }
                    _readFromStream = System.Text.Encoding.ASCII.GetString(ms.ToArray(), 0, (int)ms.Length);
                }





                //string _readFromStream = GetResponse(stream);
                /*
                data = new Byte[1024];
                String responseData = String.Empty;
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                */
                logger.Info("Response from:" + host + ":" + port + " is:" + _readFromStream);
                // Close everything.
                stream.Close();
                client.Close();
                //Phan tich data tra ve:
                result = _readFromStream;




            }
            catch (ArgumentNullException e)
            {
                logger.Info("ArgumentNullException and close connection:" + e.ToString());
                stream.Close();
                client.Close();
                return "-1000";
            }
            catch (SocketException e)
            {


                return "-1000";
            }
            return result;
        }


        public static string GetResponse(NetworkStream stream)
        {
            byte[] data = new byte[1024];
            using (MemoryStream memoryStream = new MemoryStream())
            {
                do
                {
                    stream.Read(data, 0, data.Length);
                    memoryStream.Write(data, 0, data.Length);
                } while (stream.DataAvailable);

                return System.Text.Encoding.ASCII.GetString(memoryStream.ToArray(), 0, (int)memoryStream.Length);
            }
        }










        public static String SendPost(string url, string str)
        {


            var data = new StringContent(str, Encoding.UTF8, "application/json");
            //StringBuilder sb = new StringBuilder();
            //sb.Append("Request: " + data);


            using (var client = new HttpClient())
            {
                var response = client.PostAsync(url, data).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;
                    //sb.Append("\r\nResponse: " + responseContent);
                    //log.Debug(sb.ToString());

                    // by calling .Result you are synchronously reading the result
                    string responseString = responseContent.ReadAsStringAsync().Result;

                    return responseString;
                }
                //sb.Append("\r\nResponse: " + response);
                //log.Debug(sb.ToString());
                return response.StatusCode.ToString();
            }
        }



        public static bool CheckPortV2(string ip, string port)
        {
            bool result = false;
            try
            {
                TcpClient c = new TcpClient(ip, Convert.ToInt32(port));
                result = true;
                c.Close();
            }
            catch (System.Net.Sockets.SocketException ex)
            {


            }
            return result;
        }


        public static bool CheckPort(string ip, string port)
        {
            bool result = false;
            string hostname = ip;
            int portno = Convert.ToInt32(port);
            IPAddress ipa = (IPAddress)Dns.GetHostAddresses(hostname)[0];
            try
            {
                System.Net.Sockets.Socket sock =
                        new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork,
                                                      System.Net.Sockets.SocketType.Stream,
                                                      System.Net.Sockets.ProtocolType.Tcp);
                sock.Connect(ipa, portno);
                if (sock.Connected == true) // Port is in use and connection is successful
                    result = true;
                sock.Close();
            }
            catch (System.Net.Sockets.SocketException ex)
            {


            }
            return result;
        }












        public static string GetValuesAppSetting(string key, string subkey)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));

            var root = builder.Build();
            var sampleConnectionString = root.GetSection(key)[subkey];
            return sampleConnectionString;
        }















    }
}
