//-----------------------------------------------------------------------
// <copyright file="FacebookGraphData.cs" company="Microsoft">
//     Copyright (c) Microsoft. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace DotNetOpenAuth.AspNet.Clients {
	using System.ComponentModel;
	using System.Runtime.Serialization;

#pragma warning disable CS1591
	[DataContract, EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class FacebookGraphData {
		[DataMember(Name = "email")]
		public string Email;
		[DataMember(Name = "id")]
		public string Id;
		[DataMember(Name = "name")]
		public string Name;
		[DataMember(Name = "first_name")]
		public string FirstName;
		[DataMember(Name = "last_name")]
		public string LastName;
	}
}
