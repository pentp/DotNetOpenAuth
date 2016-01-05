//-----------------------------------------------------------------------
// <copyright file="MicrosoftClientUserData.cs" company="Microsoft">
//     Copyright (c) Microsoft. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace DotNetOpenAuth.AspNet.Clients {
	using System.ComponentModel;
	using System.Runtime.Serialization;

#pragma warning disable CS1591
	[DataContract, EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class MicrosoftClientUserData {
		[DataMember(Name = "first_name")]
		public string FirstName;
		[DataMember(Name = "id")]
		public string Id;
		[DataMember(Name = "last_name")]
		public string LastName;
		[DataMember(Name = "name")]
		public string Name;
		[DataMember(Name = "emails")]
		public EmailTypes EMails;

		[DataContract]
		public sealed class EmailTypes
		{
			[DataMember(Name = "preferred")]
			public string Preferred;
		}
	}
}
