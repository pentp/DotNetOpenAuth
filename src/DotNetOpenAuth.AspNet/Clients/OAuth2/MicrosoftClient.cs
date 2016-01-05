//-----------------------------------------------------------------------
// <copyright file="MicrosoftClient.cs" company="Microsoft">
//     Copyright (c) Microsoft. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace DotNetOpenAuth.AspNet.Clients {
	using System;
	using System.Collections.Specialized;
	using System.Net;
	using System.Text;
	using Validation;

	/// <summary>
	/// The Microsoft account client.
	/// </summary>
	public sealed class MicrosoftClient : OAuth2Client {
		#region Constants and Fields

		/// <summary>
		/// The authorization endpoint.
		/// </summary>
		private const string AuthorizationEndpoint = "https://login.live.com/oauth20_authorize.srf";

		/// <summary>
		/// The token endpoint.
		/// </summary>
		private const string TokenEndpoint = "https://login.live.com/oauth20_token.srf";

		/// <summary>
		/// The _app id.
		/// </summary>
		private readonly string appId;

		/// <summary>
		/// The _app secret.
		/// </summary>
		private readonly string appSecret;

		/// <summary>
		/// The requested scopes.
		/// </summary>
		private readonly string[] requestedScopes;

		#endregion

		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="MicrosoftClient"/> class.
		/// Requests a scope of "wl.basic" by default, but "wl.signin" is a good minimal alternative.
		/// </summary>
		/// <param name="appId">The app id.</param>
		/// <param name="appSecret">The app secret.</param>
		public MicrosoftClient(string appId, string appSecret)
			: this(appId, appSecret, "wl.emails")
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MicrosoftClient"/> class.
		/// </summary>
		/// <param name="appId">The app id.</param>
		/// <param name="appSecret">The app secret.</param>
		/// <param name="requestedScopes">One or more requested scopes.</param>
		public MicrosoftClient(string appId, string appSecret, params string[] requestedScopes)
			: base("microsoft") {
			Requires.NotNullOrEmpty(appId, nameof(appId));
			Requires.NotNullOrEmpty(appSecret, nameof(appSecret));

			this.appId = appId;
			this.appSecret = appSecret;
			this.requestedScopes = requestedScopes;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Gets the full url pointing to the login page for this client. The url should include the specified return url so that when the login completes, user is redirected back to that url.
		/// </summary>
		/// <param name="returnUrl">The return URL.</param>
		/// <returns>
		/// An absolute URL.
		/// </returns>
		protected override Uri GetServiceLoginUrl(Uri returnUrl) {
			return new UriBuilder(AuthorizationEndpoint)
			{
				Query = new NameValueCollection {
					{ "client_id", this.appId },
					{ "scope", string.Join(" ", this.requestedScopes) },
					{ "response_type", "code" },
					{ "redirect_uri", returnUrl.AbsoluteUri },
				}.BuildQuery()
			}.Uri;
		}

		/// <summary>
		/// Given the access token, gets the logged-in user's data. The returned dictionary must include two keys 'id', and 'username'.
		/// </summary>
		/// <param name="accessToken">
		/// The access token of the current user. 
		/// </param>
		/// <returns>
		/// A dictionary contains key-value pairs of user data 
		/// </returns>
		protected override NameValueCollection GetUserData(string accessToken) {
			MicrosoftClientUserData graph;
			using (var response = WebRequest.CreateHttp("https://apis.live.net/v5.0/me?access_token=" + Uri.EscapeDataString(accessToken)).GetResponse()) {
				graph = JsonHelper.Deserialize<MicrosoftClientUserData>(response.GetResponseStream());
			}

			var userData = new NameValueCollection();
			userData.AddItemIfNotEmpty("id", graph.Id);
			userData.AddItemIfNotEmpty("name", graph.Name);
			userData.AddItemIfNotEmpty("first_name", graph.FirstName);
			userData.AddItemIfNotEmpty("last_name", graph.LastName);
			userData.AddItemIfNotEmpty("email", graph.EMails?.Preferred);
			return userData;
		}

		/// <summary>
		/// Queries the access token from the specified authorization code.
		/// </summary>
		/// <param name="returnUrl">
		/// The return URL. 
		/// </param>
		/// <param name="authorizationCode">
		/// The authorization code. 
		/// </param>
		/// <returns>
		/// The query access token.
		/// </returns>
		protected override string QueryAccessToken(Uri returnUrl, string authorizationCode) {
			var entity = Encoding.ASCII.GetBytes(new NameValueCollection {
				{ "client_id", this.appId },
				{ "redirect_uri", returnUrl.AbsoluteUri },
				{ "client_secret", this.appSecret },
				{ "code", authorizationCode },
				{ "grant_type", "authorization_code" },
			}.BuildQuery());

			var tokenRequest = WebRequest.CreateHttp(TokenEndpoint);
			tokenRequest.ContentType = "application/x-www-form-urlencoded";
			tokenRequest.ContentLength = entity.Length;
			tokenRequest.Method = "POST";

			using (var requestStream = tokenRequest.GetRequestStream()) {
				requestStream.Write(entity, 0, entity.Length);
			}

			using (var tokenResponse = (HttpWebResponse)tokenRequest.GetResponse()) {
				if (tokenResponse.StatusCode == HttpStatusCode.OK) {
					var tokenData = JsonHelper.Deserialize<OAuth2AccessTokenData>(tokenResponse.GetResponseStream());
					if (tokenData != null) {
						return tokenData.AccessToken;
					}
				}
			}

			return null;
		}

		#endregion
	}
}