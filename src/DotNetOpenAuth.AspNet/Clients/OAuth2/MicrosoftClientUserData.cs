//-----------------------------------------------------------------------
// <copyright file="MicrosoftClientUserData.cs" company="Microsoft">
//     Copyright (c) Microsoft. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace DotNetOpenAuth.AspNet.Clients {
	using System;
	using System.Collections.Specialized;
	using System.ComponentModel;
	using System.Runtime.Serialization;

	/// <summary>
	/// Contains data of a Windows Live user.
	/// </summary>
	/// <remarks>
	/// Technically, this class doesn't need to be public, but because we want to make it serializable in medium trust, it has to be public.
	/// </remarks>
	[DataContract]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class MicrosoftClientUserData {
		/// <summary>
		/// Gets or sets the first name.
		/// </summary>
		/// <value> The first name. </value>
		[DataMember(Name = "first_name")]
		public string FirstName;

		/// <summary>
		/// Gets or sets the gender.
		/// </summary>
		/// <value> The gender. </value>
		[DataMember(Name = "gender")]
		public string Gender;

		/// <summary>
		/// Gets or sets the id.
		/// </summary>
		/// <value> The id. </value>
		[DataMember(Name = "id")]
		public string Id;

		/// <summary>
		/// Gets or sets the last name.
		/// </summary>
		/// <value> The last name. </value>
		[DataMember(Name = "last_name")]
		public string LastName;

		/// <summary>
		/// Gets or sets the link.
		/// </summary>
		/// <value> The link. </value>
		[DataMember(Name = "link")]
		public Uri Link;

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value> The name. </value>
		[DataMember(Name = "name")]
		public string Name;

		[DataMember(Name = "emails")]
		public NameValueCollection EMails;
	}
}
