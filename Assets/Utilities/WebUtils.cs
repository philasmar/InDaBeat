using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

public class WebUtils
{
    public String SendRequestToQRS(String method, String url, String jsonBody)
    {
        HttpWebRequest request;
        request = (HttpWebRequest)WebRequest.Create(url);
        // Add the method to authentication the user
        request.Method = method;
        request.Accept = "application/json";
        request.Headers.Add("Authorization", "Bearer " + Properties.access_token);
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
