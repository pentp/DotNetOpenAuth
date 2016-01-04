using System;

namespace Validation
{
	static class Requires
	{
		public static void NotNull(object value, string paramName)
		{
			if(value == null) throw new ArgumentNullException(paramName);
		}

		public static void NotNullOrEmpty(string value, string paramName)
		{
			if(value == null) throw new ArgumentNullException(paramName);
			if(value.Length == 0) throw new ArgumentException($"{paramName} must not be empty", paramName);
		}

		public static void NotNullOrEmpty(string[] value, string paramName)
		{
			if(value == null) throw new ArgumentNullException(paramName);
			if(value.Length == 0) throw new ArgumentException($"{paramName} must not be empty", paramName);
		}
	}
}
