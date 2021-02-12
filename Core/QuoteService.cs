using Core.Model;
using Core.Model.Parameter;
using Core.Model.Result;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class QuoteService : IDisposable
    {
        private readonly IConfiguration _config;

        public QuoteService(IConfiguration config)
        {
            _config = config;
        }

        public bool IsTokenValid(Token token)
        {
            return token == null ? false : !token.IsExpired && !string.IsNullOrWhiteSpace(token.AccessToken); // valid token with expiry time and the existence of the access token
        }

        private async Task<HttpResponseMessage> MakeRequest(Token token, string host, string method, HttpContent requestParams)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(host);

                // set access token in the header is given
                if (token != null)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token.TokenType, token.AccessToken);
                }

                response = await client.PostAsync(method, requestParams).ConfigureAwait(false);
            }

            return response;
        }

        public async Task<Token> GetAuthorizeToken()
        {
            // capture settings from appsettings file
            var section = _config.GetSection("Api");
            var host = section["Host"];
            var method = section["AuthMethod"];
            var grantType = section["GrantType"];
            var clientId = section["ClientId"];
            var secret = section["Secret"];
            var scope = section["Scope"];

            // prepare the post data
            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("grant_type", grantType);
            postData.Add("scope", scope);
            postData.Add("client_id", clientId);
            postData.Add("client_secret", secret);
            HttpContent requestParams = new FormUrlEncodedContent(postData.ToArray());

            Token result = null;

            var response = await MakeRequest(null, host, method, requestParams);
            if (response.IsSuccessStatusCode) // prepare the response data on success
            {
                var responseData = await response.Content.ReadAsStringAsync();
                result = Serializer.Deserialize<Token>(responseData);
            }

            return result;
        }

        public async Task<QuoteResult> GetQuote(Token token, QuoteParameter parameter)
        {
            // capture settings from appsettings file
            var section = _config.GetSection("Api");
            var host = section["Host"];
            var method = section["QuoteMethod"];

            // prepare the post data
            var jsonContent = Serializer.Serialize<QuoteParameter>(parameter);
            HttpContent requestParams = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            QuoteResult result = null;

            if (IsTokenValid(token)) // submit only if token is valid
            {
                var response = await MakeRequest(token, host, method, requestParams);
                if (response.IsSuccessStatusCode) // prepare the response data on success
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    result = Serializer.Deserialize<QuoteResult>(responseData);
                }
            }

            return result;
        }

        public void Dispose()
        {
        }
    }
}
