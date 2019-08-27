using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.IO;

namespace consoleproject
{
    public class Program
    {

        private const string URL = "http://itis-shaca.com/webrms/api/v1/mvar";
        private const string username = "hexagon";
        private const string password = "hexagon2019";
        static void Main(string[] args)
        {
            Program.SubmitMVAR();
        }

        private static void SubmitMVAR()
        {
            Uri restURI = new Uri(URL);

            /*
             *    Establish Basic Authentication credentials
             *
             */
            NetworkCredential networkCredentials = new NetworkCredential(username, password);
            CredentialCache credentialCache = new CredentialCache();
            credentialCache.Add(restURI, "Basic", networkCredentials);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(restURI);
            request.PreAuthenticate = true;
            request.Credentials = credentialCache;

            request.Method = "POST";
            request.ContentType = "application/xml;charset=utf-8";

            /*
             *
             *    Submit an XML MVAR Document
             *
             */

            string XMLData;
            XMLData = @"<?xml version=""1.0"" encoding=""utf-8""?>
                            <MVARInformation>
                                <Number>TT103-10234</Number>
                                <Summary>Big bad accident in Maui</Summary>
                                <Notes>Notes about the accident</Notes>
                                <DateTimeReported>10/01/2019 5:50:15</DateTimeReported>
                            </MVARInformation>";

            request.ContentLength = XMLData.Length;
            using (Stream webStream = request.GetRequestStream())
            using (StreamWriter requestWriter = new StreamWriter(webStream, System.Text.Encoding.ASCII))
            {
                requestWriter.Write(XMLData);
            }
            try
            {
                WebResponse webResponse = request.GetResponse();
                using (Stream webStream = webResponse.GetResponseStream() ?? Stream.Null)
                using (StreamReader responseReader = new StreamReader(webStream))
                {
                    string response = responseReader.ReadToEnd();
                    Console.Out.WriteLine(response);
                }
            }
            catch (Exception e)
            {
                Console.Out.WriteLine("-----------------");
                Console.Out.WriteLine(e.Message);
            }
        }
    }
}