using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application.Helper
{
    public class VNPAYLibrary
    {
        private readonly SortedList<string, string> _requestData = new SortedList<string, string>(new VNPAYCompare());
        private readonly SortedList<string, string> _responseData = new SortedList<string, string>(new VNPAYCompare());

        public void AddRequestData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _requestData.Add(key, value);
            }
        }

        public void AddResponseData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _responseData.Add(key, value);
            }
        }

        public string CreateRequestUrl(string baseUrl, string vnp_HashSecret)
        {
            StringBuilder data = new StringBuilder();
            foreach (var (key, value) in _requestData)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    data.Append(WebUtility.UrlEncode(key) + "=" + WebUtility.UrlEncode(value) + "&");
                }
            }

            string querystring = data.ToString();
            querystring = querystring.Remove(querystring.Length - 1, 1);

            string signData = querystring;
            string vnp_SecureHash = HmacSHA512(vnp_HashSecret, signData);
            querystring += "&vnp_SecureHash=" + vnp_SecureHash;

            return baseUrl + "?" + querystring;
        }

        public bool ValidateSignature(string vnp_HashSecret)
        {
            string responseRaw = BuildDataToHash();
            string vnp_SecureHash = _responseData["vnp_SecureHash"];
            return vnp_SecureHash == HmacSHA512(vnp_HashSecret, responseRaw);
        }

        private string BuildDataToHash()
        {
            StringBuilder data = new StringBuilder();
            foreach (var (key, value) in _responseData)
            {
                if (!string.IsNullOrEmpty(value) && key.StartsWith("vnp_") && key != "vnp_SecureHash")
                {
                    data.Append(key + "=" + value + "&");
                }
            }
            return data.ToString().TrimEnd('&');
        }

        private static string HmacSHA512(string key, string inputData)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);
            using (var hmac = new HMACSHA512(keyBytes))
            {
                byte[] hashValue = hmac.ComputeHash(inputBytes);
                StringBuilder hex = new StringBuilder(hashValue.Length * 2);
                foreach (byte b in hashValue)
                {
                    hex.AppendFormat("{0:x2}", b);
                }
                return hex.ToString();
            }
        }
    }


}
