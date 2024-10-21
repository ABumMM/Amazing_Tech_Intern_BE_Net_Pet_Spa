using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace PetSpa.Core.Utils
{
    public class VnPayLibrary
    {
        private readonly SortedDictionary<string, string> _requestData = new SortedDictionary<string, string>(new VnPayCompare());
        private readonly SortedDictionary<string, string> _responseData = new SortedDictionary<string, string>(new VnPayCompare());
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _vnpTmnCode;
        private readonly string _vnpHashSecret;

        public VnPayLibrary(IHttpContextAccessor httpContextAccessor, IOptions<PaymentSettings> paymentSettings)
        {
            _httpContextAccessor = httpContextAccessor;
            _vnpTmnCode = paymentSettings.Value.VnPayTmnCode;
            _vnpHashSecret = paymentSettings.Value.VnPayHashSecret;
        }

        public void AddRequestData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _requestData[key] = value;
            }
        }

        public void AddResponseData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _responseData[key] = value;
            }
        }

        public string GetResponseData(string key)
        {
            return _responseData.TryGetValue(key, out var retValue) ? retValue : string.Empty;
        }

        #region Request
        public string CreateRequestUrl(string baseUrl, string vnpHashSecret)
        {
            var data = new StringBuilder();

            foreach (var (key, value) in _requestData.Where(kv => !string.IsNullOrEmpty(kv.Value)))
            {
                data.Append(WebUtility.UrlEncode(key) + "=" + WebUtility.UrlEncode(value) + "&");
            }

            var querystring = data.ToString();

            // Remove last '&'
            if (querystring.Length > 0)
            {
                querystring = querystring.Remove(querystring.Length - 1, 1);
            }

            baseUrl += (baseUrl.Contains("?") ? "&" : "?") + querystring;

            var vnpSecureHash = Utils.HmacSHA512(vnpHashSecret, querystring);
            baseUrl += "&vnp_SecureHash=" + vnpSecureHash;

            return baseUrl;
        }
    
        #endregion

        #region Response process
        public bool ValidateSignature(string inputHash, string secretKey)
        {
            var rspRaw = GetResponseData();
            var myChecksum = Utils.HmacSHA512(secretKey, rspRaw);
            return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
        }

        private string GetResponseData()
        {
            var data = new StringBuilder();
            if (_responseData.ContainsKey("vnp_SecureHashType"))
            {
                _responseData.Remove("vnp_SecureHashType");
            }

            if (_responseData.ContainsKey("vnp_SecureHash"))
            {
                _responseData.Remove("vnp_SecureHash");
            }

            foreach (var (key, value) in _responseData.Where(kv => !string.IsNullOrEmpty(kv.Value)))
            {
                data.Append(WebUtility.UrlEncode(key) + "=" + WebUtility.UrlEncode(value) + "&");
            }

            // Remove last '&'
            if (data.Length > 0)
            {
                data.Remove(data.Length - 1, 1);
            }

            return data.ToString();
        }
        #endregion
    }

    public static class Utils
    {
        public static string HmacSHA512(string key, string inputData)
        {
            var hash = new StringBuilder();
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var inputBytes = Encoding.UTF8.GetBytes(inputData);
            using (var hmac = new HMACSHA512(keyBytes))
            {
                var hashValue = hmac.ComputeHash(inputBytes);
                foreach (var theByte in hashValue)
                {
                    hash.Append(theByte.ToString("x2"));
                }
            }

            return hash.ToString();
        }

        public static string GetIpAddress(IHttpContextAccessor httpContextAccessor)
        {
            var ipAddress = "127.0.0.1"; // Default IP if not found
            try
            {
                var context = httpContextAccessor.HttpContext;
                if (context != null)
                {
                    var remoteIpAddress = context.Connection.RemoteIpAddress;
                    if (remoteIpAddress != null)
                    {
                        if (remoteIpAddress.AddressFamily == AddressFamily.InterNetworkV6)
                        {
                            remoteIpAddress = Dns.GetHostEntry(remoteIpAddress).AddressList
                                .FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);
                        }

                        if (remoteIpAddress != null)
                            ipAddress = remoteIpAddress.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ipAddress = "Invalid IP: " + ex.Message;
            }

            return ipAddress;
        }
    }

    public class VnPayCompare : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x == y) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            var vnpCompare = CompareInfo.GetCompareInfo("en-US");
            return vnpCompare.Compare(x, y, CompareOptions.Ordinal);
        }
    }

}
