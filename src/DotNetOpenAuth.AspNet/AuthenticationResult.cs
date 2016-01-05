//-----------------------------------------------------------------------
// <copyright file="AuthenticationResult.cs" company="Microsoft">
//     Copyright (c) Microsoft. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace DotNetOpenAuth.AspNet {
	using System;
	using System.Collections.Specialized;
	using System.Diagnostics.CodeAnalysis;

	/// <summary>
	/// Represents the result of OAuth or OpenID authentication.
	/// </summary>
	public sealed class AuthenticationResult {
		/// <summary>
		/// Returns an instance which indicates failed authentication.
		/// </summary>
		[SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes",
			Justification = "This type is immutable.")]
		public static readonly AuthenticationResult Failed = new AuthenticationResult(isSuccessful: false);

		/// <summary>
		/// Initializes a new instance of the <see cref="AuthenticationResult"/> class.
		/// </summary>
		/// <param name="isSuccessful">
		/// if set to <c>true</c> [is successful]. 
		/// </param>
		public AuthenticationResult(bool isSuccessful) : this(isSuccessful, null, null, null, null) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="AuthenticationResult"/> class.
		/// </summary>
		/// <param name="exception">
		/// The exception. 
		/// </param>
		public AuthenticationResult(Exception exception)
			: this(exception, provider: null) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AuthenticationResult"/> class.
		/// </summary>
		/// <param name="exception">The exception.</param>
		/// <param name="provider">The provider name.</param>
		public AuthenticationResult(Exception exception, string provider)
			: this(isSuccessful: false) {
			if (exception == null) {
				throw new ArgumentNullException(nameof(exception));
			}

			this.Error = exception;
			this.Provider = provider;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AuthenticationResult"/> class.
		/// </summary>
		/// <param name="isSuccessful">
		/// if set to <c>true</c> [is successful]. 
		/// </param>
		/// <param name="provider">
		/// The provider. 
		/// </param>
		/// <param name="providerUserId">
		/// The provider user id. 
		/// </param>
		/// <param name="email">
		/// User e-mail. 
		/// </param>
		/// <param name="extraData">
		/// The extra data. 
		/// </param>
		public AuthenticationResult(
			bool isSuccessful, string provider, string providerUserId, string email, NameValueCollection extraData) {
			this.IsSuccessful = isSuccessful;
			this.Provider = provider;
			this.ProviderUserId = providerUserId;
			this.Email = email;
			this.ExtraData = extraData ?? new NameValueCollection();
		}

		/// <summary>
		/// Gets the error that may have occured during the authentication process
		/// </summary>
		public Exception Error { get; }

		/// <summary>
		/// Gets the optional extra data that may be returned from the provider
		/// </summary>
		public NameValueCollection ExtraData { get; }

		/// <summary>
		/// Gets a value indicating whether the authentication step is successful.
		/// </summary>
		/// <value> <c>true</c> if authentication is successful; otherwise, <c>false</c> . </value>
		public bool IsSuccessful { get; }

		/// <summary>
		/// Gets the provider's name.
		/// </summary>
		public string Provider { get; }

		/// <summary>
		/// Gets the user id that is returned from the provider.  It is unique only within the Provider's namespace.
		/// </summary>
		public string ProviderUserId { get; }

		/// <summary>
		/// Gets the user e-mail that is returned from the provider.
		/// </summary>
		public string Email { get; }
	}
}
