using EPSI_Bluetooth.Exceptions;
using EPSI_Bluetooth.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EPSI_Bluetooth.Services
{
    public class AuthToken
    {
        public string Token { get; set; }
    }

    public class APIService
    {
        private const string _serverUrl = "http://vps475171.ovh.net/private/bluetooth/api/";
        private string _username;
        private string _password;
        private string _token;
        public string Token { get { return _token; } }

        public APIService() { }

        public APIService(string username, string password)
        {
            this._username = username;
            this._password = password;
        }

        private HttpClient GetHttpClientBasicAuth(string username, string password)
        {
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("Accept", "application/json");
            var byteArray = Encoding.ASCII.GetBytes(username + ":" + password);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            return client;
        }

        private HttpClient GetHttpClientToken(string token)
        {
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("Authorization", "Token " + token);

            return client;
        }

        public async Task<Boolean> LoginAsync(string username, string password)
        {
            string token = string.Empty;

            HttpClient client = this.GetHttpClientBasicAuth(username, password);

            HttpResponseMessage response = await client.GetAsync(_serverUrl + "token/");

            var success = false;

            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    var content = await response.Content.ReadAsStringAsync();
                    var authToken = JsonConvert.DeserializeObject<AuthToken>(content);
                    _token = authToken.Token;
                    success = true;
                    break;

                case System.Net.HttpStatusCode.Unauthorized:
                    throw new AuthorizationException("Unknown user");
            }

            return success;
        }

        private async Task RenewAuthToken()
        {
            HttpClient client = this.GetHttpClientBasicAuth(_username, _password);

            HttpResponseMessage response = await client.GetAsync(_serverUrl + "token/");

            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    var content = await response.Content.ReadAsStringAsync();
                    var authToken = JsonConvert.DeserializeObject<AuthToken>(content);
                    _token = authToken.Token;
                    break;

                case System.Net.HttpStatusCode.Unauthorized:
                    throw new AuthorizationException("Unknown user");

                default:
                    throw new Exception(String.Format("Unable to get token : {0}", response.StatusCode));

            }
        }

        public async Task<CustomerContainer> GetCustomerContainerAsync(string token)
        {
            CustomerContainer container = null;

            HttpClient client = this.GetHttpClientToken(token);

            HttpResponseMessage response = await client.GetAsync(_serverUrl + "customers/");

            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    var content = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine(content);
                    container = JsonConvert.DeserializeObject<CustomerContainer>(content);
                    break;

                case System.Net.HttpStatusCode.Unauthorized:
                    throw new AuthorizationException("Unknown token");
            }
            return container;
        }

        public async Task<CustomerContainer> GetCustomerContainerWithRetryAsync(bool retry = true)
        {
            CustomerContainer container = null;
            try
            {
                container = await GetCustomerContainerAsync(_token);
            }
            catch (AuthorizationException)
            {
                if (retry)
                {
                    await RenewAuthToken();
                    container = await GetCustomerContainerAsync(_token);
                }
                else
                {
                    throw;
                }
            }
            return container;
        }

        public async Task<CustomerModel> GetCustomerFromIdWithRetryAsync(string customer_id, bool retry = true)
        {
            CustomerModel need = null;
            try
            {
                need = await GetCustomerFromIdAsync(_token, customer_id);
            }
            catch (AuthorizationException)
            {
                if (retry)
                {
                    await RenewAuthToken();
                    need = await GetCustomerFromIdAsync(_token, customer_id);
                }
                else
                {
                    throw;
                }
            }
            return need;
        }

        public async Task<CustomerModel> GetCustomerFromIdAsync(string token, string customer_id)
        {
            CustomerModel need = null;

            HttpClient client = this.GetHttpClientToken(token);

            HttpResponseMessage response = await client.GetAsync(_serverUrl + "customers/" + customer_id);

            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    var content = await response.Content.ReadAsStringAsync();
                    need = JsonConvert.DeserializeObject<CustomerModel>(content);
                    break;

                case System.Net.HttpStatusCode.Unauthorized:
                    throw new AuthorizationException("Unknown token");
            }
            return need;
        }
    }
}
