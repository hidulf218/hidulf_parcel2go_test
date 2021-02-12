using System;
using System.Text.Json.Serialization;

namespace Core.Model
{
    public class Token
    {
        private int _ExpiryIn;

        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
        [JsonPropertyName("expires_in")]
        public int ExpiryIn
        {
            get => _ExpiryIn;
            set
            {
                _ExpiryIn = value;
                ExpiryTime = DateTime.Now.AddSeconds(value);
            }
        }
        public DateTime ExpiryTime { get; private set; }
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }
        [JsonPropertyName("scope")]
        public string Scope { get; set; }
        [JsonIgnore]
        public bool IsExpired { get => DateTime.Now >= ExpiryTime; }
    }
}
