//-----------------------------------------------------------------------
// <copyright file="DictionaryExtensions.cs" company="Microsoft">
//     Copyright (c) Microsoft. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace DotNetOpenAuth.AspNet.Clients {
	using System;
	using System.Collections.Specialized;
	using System.Text;

	/// <summary>
	/// The dictionary extensions.
	/// </summary>
	internal static class DictionaryExtensions {
		/// <summary>
		/// Adds a key/value pair to the specified dictionary if the value is not null or empty.
		/// </summary>
		/// <param name="dictionary">
		/// The dictionary. 
		/// </param>
		/// <param name="key">
		/// The key. 
		/// </param>
		/// <param name="value">
		/// The value. 
		/// </param>
		internal static void AddItemIfNotEmpty(this NameValueCollection dictionary, string key, string value) {
			if (key == null) {
				throw new ArgumentNullException("key");
			}

			if (!string.IsNullOrEmpty(value)) {
				dictionary[key] = value;
			}
		}

		/// <summary>
		/// Concatenates a list of name-value pairs as key=value&amp;key=value,
		/// taking care to properly encode each key and value for URL
		/// transmission according to RFC 3986.  No ? is prefixed to the string.
		/// </summary>
		/// <param name="parameters">The dictionary of key/values to read from.</param>
		/// <returns>The formulated querystring style string.</returns>
		internal static string BuildQuery(this NameValueCollection parameters)
		{
			var sb = new StringBuilder();
			for (int i = 0; i < parameters.Count; i++)
			{
				if (i != 0)
				{
					sb.Append('&');
				}
				sb.Append(Uri.EscapeDataString(parameters.GetKey(i))).Append('=').Append(Uri.EscapeDataString(parameters[i]));
			}
			return sb.ToString();
		}
	}
}
