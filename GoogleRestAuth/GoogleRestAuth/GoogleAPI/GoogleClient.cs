// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

// The code is basing on samples created by Google Inc.
// https://github.com/googlesamples/oauth-apps-for-windows

namespace GoogleRestAuth.GoogleAPI
{
    using Helpers;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Windows.Data.Json;
    using Windows.Security.Authentication.Web;
    using Windows.Security.Credentials;
    using Windows.Security.Cryptography;
    using Windows.Security.Cryptography.Core;
    using Windows.Storage.Streams;

    public class GoogleClient
    {
        private enum TokenTypes { AccessToken, RefreshToken }

        private const string GoogleTokenTime = "GoogleTokenTime";

        private const string ClientID = "putYourClientHere.apps.googleusercontent.com";
        private const string ClientSecret = "clientsSecretHere";

        private const string RedirectURI = "urn:ietf:wg:oauth:2.0:oob";
        private const string AuthorizationEndpoint = "https://accounts.google.com/o/oauth2/auth";
        private const string ApprovalEndpoint = "https://accounts.google.com/o/oauth2/approval";
        private const string TokenEndpoint = "https://www.googleapis.com/oauth2/v4/token";

        private static string accessToken = string.Empty;

        private bool isAuthorized = false;
        public bool IsAuthorized
        {
            get { return isAuthorized; }
            set { isAuthorized = value; }
        }


        private Lazy<DateTimeOffset> tokenLastAccess = new Lazy<DateTimeOffset>(() =>
        {
            return DateTimeOffset.ParseExact(Settings.ReadOrDefault(GoogleTokenTime, DateTimeOffset.MinValue.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture)),
                                                                       "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
        });

        private DateTimeOffset TokenLastAccess
        {
            get { return tokenLastAccess.Value; }
            set
            {
                tokenLastAccess = new Lazy<DateTimeOffset>(() => value);
                Settings.Save(GoogleTokenTime, value.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture));
            }
        }

        private PasswordVault vault = new PasswordVault();
        private HttpClient httpClient = new HttpClient(new HttpClientHandler { AllowAutoRedirect = true });

        private static GoogleClient client = null;
        public static GoogleClient Client
        {
            get
            {
                if (client == null) client = new GoogleClient();
                return client;
            }
        }


        private string GetTokenFromVault(TokenTypes tokenType)
        {
            var token = vault.RetrieveAll().FirstOrDefault((x) => x.Resource == tokenType.ToString());
            if (token != null)
            {
                token.RetrievePassword();
                return token.Password;
            }

            return string.Empty;
        }

        public async Task<bool> SignInWithGoogleAsync()
        {

            if (DateTimeOffset.UtcNow < TokenLastAccess.AddSeconds(3600))
            {
                accessToken = GetTokenFromVault(TokenTypes.AccessToken);
                IsAuthorized = true;
                return true;
            }
            else
            {
                string token = GetTokenFromVault(TokenTypes.RefreshToken);
                if (!string.IsNullOrWhiteSpace(token))
                {
                    StringContent content = new StringContent($"client_secret={ClientSecret}&refresh_token={token}&client_id={ClientID}&grant_type=refresh_token",
                                                              Encoding.UTF8, "application/x-www-form-urlencoded");

                    HttpResponseMessage response = await httpClient.PostAsync(TokenEndpoint, content);
                    string responseString = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        JsonObject tokens = JsonObject.Parse(responseString);

                        accessToken = tokens.GetNamedString("access_token");

                        foreach (var item in vault.RetrieveAll().Where((x) => x.Resource == TokenTypes.AccessToken.ToString())) vault.Remove(item);

                        vault.Add(new PasswordCredential(TokenTypes.AccessToken.ToString(), "MyApp", accessToken));
                        TokenLastAccess = DateTimeOffset.UtcNow;
                        IsAuthorized = true;
                        return true;
                    }
                }
            }

            string state = RandomDataBase64(32);
            string code_verifier = RandomDataBase64(32);
            string code_challenge = Base64UrlEncodeNoPadding(Sha256(code_verifier));
            const string code_challenge_method = "S256";

            string authString = "https://accounts.google.com/o/oauth2/auth?client_id=" + ClientID;
            authString += "&scope=profile";
            authString += $"&redirect_uri={RedirectURI}";
            authString += $"&state={state}";
            authString += $"&code_challenge={code_challenge}";
            authString += $"&code_challenge_method={code_challenge_method}";
            authString += "&response_type=code";

            var receivedData = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.UseTitle, new Uri(authString), new Uri(ApprovalEndpoint));

            switch (receivedData.ResponseStatus)
            {
                case WebAuthenticationStatus.Success:
                    await GetAccessToken(receivedData.ResponseData.Substring(receivedData.ResponseData.IndexOf(' ') + 1), state, code_verifier);
                    return true;
                case WebAuthenticationStatus.ErrorHttp:
                    Debug.WriteLine($"HTTP error: {receivedData.ResponseErrorDetail}");
                    return false;

                case WebAuthenticationStatus.UserCancel:
                default:
                    return false;
            }
        }

        private async Task GetAccessToken(string data, string expectedState, string codeVerifier)
        {
            // Parses URI params into a dictionary - ref: http://stackoverflow.com/a/11957114/72176 
            Dictionary<string, string> queryStringParams = data.Split('&').ToDictionary(c => c.Split('=')[0], c => Uri.UnescapeDataString(c.Split('=')[1]));

            if (queryStringParams.ContainsKey("error"))
            {
                Debug.WriteLine($"OAuth error: {queryStringParams["error"]}.");
                return;
            }

            if (!queryStringParams.ContainsKey("code") || !queryStringParams.ContainsKey("state"))
            {
                Debug.WriteLine($"Wrong response {data}");
                return;
            }

            if (queryStringParams["state"] != expectedState)
            {
                Debug.WriteLine($"Invalid state {queryStringParams["state"]}");
                return;
            }

            StringContent content = new StringContent($"code={queryStringParams["code"]}&client_secret={ClientSecret}&redirect_uri={Uri.EscapeDataString(RedirectURI)}&client_id={ClientID}&code_verifier={codeVerifier}&grant_type=authorization_code",
                                                      Encoding.UTF8, "application/x-www-form-urlencoded");

            HttpResponseMessage response = await httpClient.PostAsync(TokenEndpoint, content);
            string responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Debug.WriteLine("Authorization code exchange failed.");
                return;
            }

            JsonObject tokens = JsonObject.Parse(responseString);
            accessToken = tokens.GetNamedString("access_token");

            foreach (var item in vault.RetrieveAll().Where((x) => x.Resource == TokenTypes.AccessToken.ToString() || x.Resource == TokenTypes.RefreshToken.ToString())) vault.Remove(item);

            vault.Add(new PasswordCredential(TokenTypes.AccessToken.ToString(), "MyApp", accessToken));
            vault.Add(new PasswordCredential(TokenTypes.RefreshToken.ToString(), "MyApp", tokens.GetNamedString("refresh_token")));
            TokenLastAccess = DateTimeOffset.UtcNow;
            IsAuthorized = true;
        }

        private string RandomDataBase64(uint length)
        {
            IBuffer buffer = CryptographicBuffer.GenerateRandom(length);
            return Base64UrlEncodeNoPadding(buffer);
        }

        private IBuffer Sha256(string inputStirng)
        {
            HashAlgorithmProvider sha = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha256);
            IBuffer buff = CryptographicBuffer.ConvertStringToBinary(inputStirng, BinaryStringEncoding.Utf8);
            return sha.HashData(buff);
        }

        private string Base64UrlEncodeNoPadding(IBuffer buffer)
        {
            string base64 = CryptographicBuffer.EncodeToBase64String(buffer);

            base64 = base64.Replace("+", "-");
            base64 = base64.Replace("/", "_");
            base64 = base64.Replace("=", string.Empty);

            return base64;
        }
    }
}
