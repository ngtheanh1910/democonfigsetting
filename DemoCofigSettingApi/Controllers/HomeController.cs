using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using DemoCofigSettingApi.Models;
using DemoCofigSettingApi.Repository;

namespace DemoCofigSettingApi.Controllers
{
    public class HomeController : Controller
    {
        // GET: Employee/EditEmpDetails/5    
        public ActionResult EditSetting(int id = 1)
        {
            //int id = 1;
            SettingRepository Repo = new SettingRepository();

            return View(Repo.GetAllSetting().Find(Emp => Emp.Id == id));

        }

        // POST: Employee/EditEmpDetails/5    
        [HttpPost]
        public ActionResult EditSetting(int id, Setting obj)
        {
            try
            {
                SettingRepository Repo = new SettingRepository();

                Repo.UpdateSetting(obj);
                //return RedirectToAction("GetAllEmpDetails");

                Stream responseMime = SendRequest(obj.Url, obj.Token, obj.NameUnit);

                var soapResponse = MimeToXml(responseMime);
                var responseXml = soapResponse.GetElementsByTagName("GetOrganizationsResponse", "http://www.e-doc.vn/Schema/");

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(responseXml[0].OuterXml);
                return View();
            }
            catch
            {
                return View();
            }
        }

        public Stream SendRequest(string url, string token, string nameunit)
        {
            string mimeMessage = "--Boundary\r\nContent-Type: text/xml; charset=utf-8\r\nContent-Transfer-Encoding: 8bit\r\nContent-ID:<Start>\r\n\r\n<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\"><SOAP-ENV:Header><Token xmlns=\"http://e-doc.vn/xsd\">" + token + "</Token></SOAP-ENV:Header><SOAP-ENV:Body><GetOrganizations xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><Token>" + token + "</Token><OrganId>" + nameunit + "</OrganId></GetOrganizations></SOAP-ENV:Body></SOAP-ENV:Envelope>\r\n\r\n--Boundary--\r\n";
            //string mimeMessage = "--Boundary\r\nContent-Type: text/xml; charset=utf-8\r\nContent-Transfer-Encoding: 8bit\r\nContent-ID: <Start>\r\n\r\n<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\"><SOAP-ENV:Header><Token xmlns=\"http://e-doc.vn/xsd\">bcnLuU6YT8eaRCc8/9KAp3ebeq0=</Token></SOAP-ENV:Header><SOAP-ENV:Body><GetOrganizations xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><Token>bcnLuU6YT8eaRCc8/9KAp3ebeq0=</Token><OrganId>000.03.99.H36</OrganId></GetOrganizations></SOAP-ENV:Body></SOAP-ENV:Envelope>\r\n\r\n--Boundary--\r\n";

            var _url = url;
            var _action = "GetOrganizations";

            HttpWebRequest request = CreateWebRequest(_url, _action);

            var buffer = Encoding.UTF8.GetBytes(mimeMessage);
            using (var stream = request.GetRequestStream())
            {
                var tmpStream = new MemoryStream(buffer);
                var tmpbuffer = new byte[3 * 1024];
                int bytesRead;
                do
                {
                    bytesRead = tmpStream.Read(tmpbuffer, 0, tmpbuffer.Length);
                    stream.Write(tmpbuffer, 0, bytesRead);
                } while (bytesRead > 0);
                stream.Close();
            }
            return request.GetResponse().GetResponseStream();
        }

        public static HttpWebRequest CreateWebRequest(string url, string action)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Headers.Add("SOAPAction", action);
            webRequest.ContentType = "multipart/related;type=\"text/xml\";boundary=\"" + "Boundary" + "\";start=\"" + "Start" + "\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }

        public static XmlDocument MimeToXml(string mime)
        {
            var doc = new XmlDocument();
            var strSoapEnvelope = Regex.Match(mime, @"<([^>]*)Envelope([^<]*)>[\S\s]*?</([^<]*):Envelope>").Value;
            doc.LoadXml(strSoapEnvelope);
            return doc;
        }

        public static XmlDocument MimeToXml(Stream stream)
        {
            var rdStream = new StreamReader(stream);
            var mimeContent = rdStream.ReadToEnd();
            return MimeToXml(mimeContent);
        }

    }
}