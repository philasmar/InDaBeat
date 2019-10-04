using System;
using System.Collections;
using System.Collections.Generic;

public class WebUtils
{
    public String SendRequestToQRS(SenseEnv env, String method, String api, String jsonBody)
    {
        ServicePointManager.ServerCertificateValidationCallback +=
   (sender, cert, chain, sslPolicyErrors) => true;
        string url = getAPIUrl(env, api);
        //Create the HTTP Request and add required headers and content in Xrfkey
        string Xrfkey = "123456789ABCDEFG";
        HttpWebRequest request;
        if (url.Contains("?"))
        {
            request = (HttpWebRequest)WebRequest.Create(url + "&xrfkey=" + Xrfkey);
        }
        else
        {
            request = (HttpWebRequest)WebRequest.Create(url + "?xrfkey=" + Xrfkey);
        }
        // Add the method to authentication the user
        request.Method = method;
        request.Accept = "application/json";
        request.Headers.Add("X-Qlik-xrfkey", Xrfkey);
        request.Headers.Add("hdr-usr", getUserDirectory() + "\\" + getUserName());
        request.ContentLength = jsonBody.Length;
        request.Timeout = 14400000;
        request.KeepAlive = false;
        string body = jsonBody;

        byte[] bodyBytes = Encoding.UTF8.GetBytes(body);

        if (!string.IsNullOrEmpty(body))
        {
            request.ContentType = "application/json";
            request.ContentLength = bodyBytes.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bodyBytes, 0, bodyBytes.Length);
            requestStream.Close();
        }
        try
        {
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
}
