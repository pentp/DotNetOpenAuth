//-----------------------------------------------------------------------
// <copyright file="OAuth2AccessTokenData.cs" company="Microsoft">
//     Copyright (c) Microsoft. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace DotNetOpenAuth.AspNet.Clients {
	using System.ComponentModel;
	using System.Runtime.Serialization;

#pragma warning disable CS1591
	[DataContract, EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class OAuth2AccessTokenData {
		[DataMember(Name = "access_token")]
		public string AccessToken;
	}
}
