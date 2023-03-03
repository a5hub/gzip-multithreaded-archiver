using System;
using System.Diagnostics;

namespace GZip.Common
{
    [DebuggerStepThrough]
	public static class Guard
	{
		/// <summary> Expected instance of type is not null </summary>
		/// <typeparam name="T"> Instance type </typeparam>
		/// <param name="value"> Instance value </param>
		/// <param name="expectationMessage"> Expected exception message </param>
		public static void NotNull<T>(T value, string expectationMessage = null) where T : class
		{
			if (value == null)
			{
				var message = $"Value of type '{typeof(T)}' is unexpectedly null. {expectationMessage}";

				throw new ArgumentNullException(message);
			}
		}

        /// <summary> Given expression is true </summary>
		/// <param name="value"> Expression result </param>
        /// <param name="expectationMessage"> Expected exception message </param>
		public static void True(bool value, string expectationMessage)
		{
			if (!value)
			{
				throw new ArgumentException(expectationMessage.ToString());
			}
		}

		/// <summary> Chosen type is enum </summary>
		/// <typeparam name="TEnum"> Enum type </typeparam>
		public static void TypeIsEnum<TEnum>()
		{
			var type = typeof(TEnum);
			if (!type.IsEnum)
			{
				var msg = $"Type '{type.AssemblyQualifiedName}' must be enum.";
				throw new ArgumentException(msg);
			}

		}

		/// <summary> Instance of structure type has one of it's value </summary>
		/// <typeparam name="T"> Type </typeparam>
		/// <param name="value"> Value </param>
		public static void DefinedEnumValue<T>(T value) where T : struct
		{
			TypeIsEnum<T>();
			var type = typeof(T);
			if (!Enum.IsDefined(type, value))
			{
				var msg = $"Value '{value}' must be in enum.";
				throw new ArgumentException(msg);
			}
		}
    }
}
