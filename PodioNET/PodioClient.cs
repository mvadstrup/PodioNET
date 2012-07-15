using System;
using System.IO;
using System.Net;
using RestSharp;

namespace PodioNET
{
    public class PodioClient
    {
        public PodioClient()
        {
            _authkeys = new AuthKeys();
        }

        private DateTime ExpectedExpiry { get; set; }
        private double ExpiresIn { get; set; }
        private string RefreshToken { get; set; }
        private static string AccessToken { get; set; }


        private string BaseUrl = "https://api.podio.com";
        private AuthKeys _authkeys;


        public bool Authenticate(string username, string password)
        {
            var req = new RestRequest("oauth/token");
            req.AddParameter("username", username);
            req.AddParameter("password", password);
            req.AddParameter("grant_type", "password");
            req.Method = Method.POST;
            var authResponse = Execute<AuthResponse>(req);
            if (authResponse == null)
            {
                return false;
            }
            RefreshToken = authResponse.RefreshToken;
            AccessToken = authResponse.AccessToken;
            ExpiresIn = authResponse.ExpiresIn;
            ExpectedExpiry = DateTime.Now.AddSeconds(authResponse.ExpiresIn);
            return true;
        }

        private T Execute<T>(RestRequest request) where T : new()
        {
            var client = new RestClient { BaseUrl = BaseUrl };
            request.AddParameter("client_id", _authkeys.ClientId);
            request.AddParameter("client_secret", _authkeys.ClientSecret);
            if (!string.IsNullOrEmpty(AccessToken))
            {
                request.AddParameter("oauth_token", AccessToken);

            }
            var response = client.Execute<T>(request);

            return response.Data;
        }


        public T RestExecute<T>(RestRequest request) where T : new()
        {
            var client = new RestClient { BaseUrl = BaseUrl };
            if (!string.IsNullOrEmpty(AccessToken))
            {
                request.AddParameter("oauth_token", AccessToken);
            }

            var response = client.Execute<T>(request);

            return response.Data;
        }

        public void DownloadFile(Int64 fileId, string targetPath)
        {
            var webClient = new WebClient();
            webClient.DownloadFile("https://api.podio.com/file/" + fileId + "/raw?oauth_token=" + AccessToken,targetPath);
        }
    }


    public class AuthResponse
    {
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public AuthRef Ref { get; set; }
        public double ExpiresIn { get; set; }
        public string RefreshToken { get; set; }

        public class AuthRef
        {
            public string Type { get; set; }
            public string Id { get; set; }
        }
    }

    public class AuthKeys
    {
        public AuthKeys()
        {
            ClientId = "XXXXXXXXXXXXXXXXXX";
            ClientSecret = "XXXXXXXXXXXXXXXXXXXX";
        }

        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public override string ToString()
        {
            return string.Format("key {0}, secret {1}", ClientId, ClientSecret);
        }
    }

}
