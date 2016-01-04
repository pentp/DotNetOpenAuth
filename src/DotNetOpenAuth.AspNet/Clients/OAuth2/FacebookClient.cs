//-----------------------------------------------------------------------
// <copyright file="FacebookClient.cs" company="Microsoft">
//     Copyright (c) Microsoft. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace DotNetOpenAuth.AspNet.Clients {
	using System;
	using System.Collections.Specialized;
	using System.Diagnostics.CodeAnalysis;
	using System.Net;
	using System.Web;
	using Validation;

	/// <summary>
	/// The facebook client.
	/// </summary>
	[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Facebook", Justification = "Brand name")]
	public sealed class FacebookClient : OAuth2Client {
		#region Constants and Fields

		/// <summary>
		/// The authorization endpoint.
		/// </summary>
		private const string AuthorizationEndpoint = "https://www.facebook.com/dialog/oauth";

		/// <summary>
		/// The token endpoint.
		/// </summary>
		private const string TokenEndpoint = "https://graph.facebook.com/oauth/access_token";

		/// <summary>
		/// The _app id.
		/// </summary>
		private readonly string appId;

		/// <summary>
		/// The _app secret.
		/// </summary>
		private readonly string appSecret;

		/// <summary>
		/// The scope.
		/// </summary>
		private readonly string[] scope;

		#endregion

		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="FacebookClient"/> class
		/// with "email" as the scope.
		/// </summary>
		/// <param name="appId">
		/// The app id.
		/// </param>
		/// <param name="appSecret">
		/// The app secret.
		/// </param>
		public FacebookClient(string appId, string appSecret)
			: this(appId, appSecret, "email") {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FacebookClient"/> class.
		/// </summary>
		/// <param name="appId">
		/// The app id.
		/// </param>
		/// <param name="appSecret">
		/// The app secret.
		/// </param>
		/// <param name="scope">
		/// The scope of authorization to request when authenticating with Facebook. The default is "email".
		/// </param>
		public FacebookClient(string appId, string appSecret, params string[] scope)
			: base("facebook") {
			Requires.NotNullOrEmpty(appId, nameof(appId));
			Requires.NotNullOrEmpty(appSecret, nameof(appSecret));
			Requires.NotNullOrEmpty(scope, nameof(scope));

			this.appId = appId;
			this.appSecret = appSecret;
			this.scope = scope;
		}

		#endregion

		#region Methods

		/// <summary>
		/// The get service login url.
		/// </summary>
		/// <param name="returnUrl">
		/// The return url.
		/// </param>
		/// <returns>An absolute URI.</returns>
		protected override Uri GetServiceLoginUrl(Uri returnUrl) {
			return new UriBuilder(AuthorizationEndpoint)
			{
				Query = new NameValueCollection {
					{ "client_id", this.appId },
					{ "redirect_uri", returnUrl.AbsoluteUri },
					{ "scope", string.Join(" ", this.scope) },
				}.BuildQuery()
			}.Uri;
		}

		/// <summary>
		/// The get user data.
		/// </summary>
		/// <param name="accessToken">
		/// The access token.
		/// </param>
		/// <returns>A dictionary of profile data.</returns>
		protected override NameValueCollection GetUserData(string accessToken) {
			FacebookGraphData graphData;
			var request =
				WebRequest.Create(
					"https://graph.facebook.com/me?access_token=" + Uri.EscapeDataString(accessToken));
			using (var response = request.GetResponse()) {
				using (var responseStream = response.GetResponseStream()) {
					graphData = JsonHelper.Deserialize<FacebookGraphData>(responseStream);
				}
			}

			// this dictionary must contains 
			var userData = new NameValueCollection();
			userData.AddItemIfNotEmpty("id", graphData.Id);
			userData.AddItemIfNotEmpty("username", graphData.Email);
			userData.AddItemIfNotEmpty("name", graphData.Name);
			userData.AddItemIfNotEmpty("link", graphData.Link?.AbsoluteUri);
			userData.AddItemIfNotEmpty("gender", graphData.Gender);
			userData.AddItemIfNotEmpty("birthday", graphData.Birthday);
			userData.AddItemIfNotEmpty("email", graphData.Email);
			return userData;
		}

		/// <summary>
		/// Obtains an access token given an authorization code and callback URL.
		/// </summary>
		/// <param name="returnUrl">
		/// The return url.
		/// </param>
		/// <param name="authorizationCode">
		/// The authorization code.
		/// </param>
		/// <returns>
		/// The access token.
		/// </returns>
		protected override string QueryAccessToken(Uri returnUrl, string authorizationCode) {
			// Note: Facebook doesn't like us to url-encode the redirect_uri value
			var builder = new UriBuilder(TokenEndpoint);
			builder.Query = new NameValueCollection {
				{ "client_id", this.appId },
				{ "redirect_uri", returnUrl.AbsoluteUri },
				{ "client_secret", this.appSecret },
				{ "code", authorizationCode },
				{ "scope", "email" },
			}.BuildQuery();

			using (WebClient client = new WebClient()) {
				string data = client.DownloadString(builder.Uri);
				if (string.IsNullOrEmpty(data)) {
					return null;
				}

				var parsedQueryString = HttpUtility.ParseQueryString(data);
				return parsedQueryString["access_token"];
			}
		}

		#endregion
	}
}
