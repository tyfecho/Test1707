using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using FcmSharp;
using FcmSharp.Requests;
using FcmSharp.Settings;
using System.Web;
using System.Runtime.Serialization.Json;
using FcmSharp.Serializer;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Test1707
{
    class Program
    {
        static void Main(string[] args)
        {
            // Read the Service Account Key from a File, which is not under Version Control:
            var settings = FileBasedFcmClientSettings.CreateFromFile("nlbfffdev", @"C:\Users\Tyrone\source\repos\Test1707\Test1707\nlbfffdev-c66734f845a4.json");

            // Construct the Client:
            using (var client = new FcmClient(settings))
            {
                // Construct the Data Payload to send:
                var data = new Dictionary<string, string>()
                {
                    {"A", "B"},
                    {"C", "D"}
                };

                // The Message should be sent to the News Topic:
                var message = new FcmMessage()
                {
                    ValidateOnly = false,
                    Message = new Message
                    {
                        Topic = "news",
                        Data = data
                    }
                };

                // Finally send the Message and wait for the Result:
                CancellationTokenSource cts = new CancellationTokenSource();

                // Send the Message and wait synchronously:
                var result = client.SendAsync(message, cts.Token).GetAwaiter().GetResult();

                // Print the Result to the Console:
                System.Console.WriteLine("Message ID = {0}", result.Name);

                string SFNresult = SendFCMNotification("AIzaSyCcxDhm3-hRNpb1CG4CLS0cZ_VaPUFk2u8", "455828917963", "null");

                System.Console.WriteLine("SendFCMNotification result:", SFNresult);

                System.Console.ReadLine();
            }
        }

        private static string SendFCMNotification(string apiKey, string senderId, string postData)
        {
            var result = "-1";
            //  CREATE REQUEST
            //  https://android.googleapis.com/gcm/send
            //  https://fcm.googleapis.com/fcm/send
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            Request.Method = "POST";
            Request.ContentType = "application/json";
            Request.Headers.Add(string.Format("Authorization: key={0}", apiKey));
            Request.Headers.Add(string.Format("Sender: id={0}", senderId));

            string datanew = "{\"notification\":{\"title':\"Title\",\"body\":\"Body\"},\"data\":{\"payload\":\"1\"}}";
            
            var data = new
            {
                to = "etmiQh_ePfU:APA91bFfP_lDb_L0cuOagSe77sAe7WBgwa8WkysoIuiTLvj3vb4MVBuL7mZu0ksIrJ80gxE-vxhlhcK_RORq1B1hVqJdle2-MJ9fXi2fNKkgZhX27pb7lE_MbreQr3mP_lVC8EcrZp9aD2DZ9Jypq4CZR5OaRs6KvA", // Uncoment this if you want to test for single device
                //  to = "/topics/" + "Test", // this is for topic 
                notification = new
                {
                    title = "Title",
                    body = "message",
                    //icon="myicon"
                },
                data = new
                {
                    payload = "1"
                }
            };

            using ( var streamWriter = new StreamWriter (Request.GetRequestStream()))
            {
                String postJsonData = "{\"to\": \"etmiQh_ePfU:APA91bFfP_lDb_L0cuOagSe77sAe7WBgwa8WkysoIuiTLvj3vb4MVBuL7mZu0ksIrJ80gxE-vxhlhcK_RORq1B1hVqJdle2-MJ9fXi2fNKkgZhX27pb7lE_MbreQr3mP_lVC8EcrZp9aD2DZ9Jypq4CZR5OaRs6KvA\",\"data\": {\"message\": \"This is a Firebase Cloud Messaging Topic Message!\",}}";
                streamWriter.Write(postJsonData);
                streamWriter.Flush();
            }

            var httpResponse = (HttpWebResponse)Request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }



                /*
                JObject jsonObj = JObject.Parse(datanew);
                byte[] byteArray = Encoding.UTF8.GetBytes(postJsonData);
                Request.ContentLength = byteArray.Length;

                using (Stream dataStream = Request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);

                    using (WebResponse webResponse = Request.GetResponse())
                    {
                        using (Stream dataStreamResponse = webResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                return sResponseFromServer;
                            }
                        }
                    }
                };*/



                //  SEND MESSAGE  
                /*try
                {
                    WebResponse Response = Request.GetResponse();

                    HttpStatusCode ResponseCode = ((HttpWebResponse)Response).StatusCode;
                    if (ResponseCode.Equals(HttpStatusCode.Unauthorized) || ResponseCode.Equals(HttpStatusCode.Forbidden))
                    {
                        var text = "Unauthorized - need new token";
                    }
                    else if (!ResponseCode.Equals(HttpStatusCode.OK))
                    {
                        var text = "Response from web service isn't OK";
                    }

                    StreamReader Reader = new StreamReader(Response.GetResponseStream());
                    string responseLine = Reader.ReadToEnd();
                    Reader.Close();

                    return responseLine;
                }
                catch (Exception e)
                {

                }*/
                return result;
        }
    }
}
