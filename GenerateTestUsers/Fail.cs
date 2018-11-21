using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace GenerateTestUsers
{
	public static class Fail
	{
		private const string NOT_CASTABLE_MESSAGE = "Expected object of type '{0}' but was '{1}'";

		[DebuggerStepThrough]
		[PublicAPI]
		[ContractAnnotation("=> halt")]
		public static void Always(string message, params object[] args)
		{
			throw new DesignByContractViolationException(String.Format(CultureInfo.InvariantCulture, message, args));
		}

		[DebuggerStepThrough]
		[PublicAPI]
		public static DesignByContractViolationException Because(string message, params object[] args)
		{
			return new DesignByContractViolationException(String.Format(CultureInfo.InvariantCulture, message, args));
		}

		[DebuggerStepThrough]
		[PublicAPI]
		public static void IfEqual(object expected, object actual, string message, params object[] args)
		{
			if (expected.Equals(actual))
				Always(message, args);
		}

		[DebuggerStepThrough]
		[PublicAPI]
		[ContractAnnotation("value: null => halt")]
		public static void IfNull(object value, string message = "Expected not null but was null.", params object[] args)
		{
			if (value == null)
				Always(message, args);
		}

		[DebuggerStepThrough]
		[PublicAPI]
		[ContractAnnotation("value: null => halt")]
		public static void IfNullOrEmpty(string value, string message = "Expected not null and not empty", params object[] args)
		{
			IfNull(value, message, args);

			if (value.Length == 0)
				Always(message, args);
		}

		[DebuggerStepThrough]
		[PublicAPI]
		[ContractAnnotation("value: notnull => halt")]
		public static void IfNotNull(object value, string message = "Expected null but was not null.", params object[] args)
		{
			if (value != null)
				Always(message, args);
		}

		[DebuggerStepThrough]
		[PublicAPI]
		[ContractAnnotation("value: false => halt")]
		public static void IfFalse(bool value, string message = "", params object[] args)
		{
			if (value == false)
				Always(message, args);
		}

		[DebuggerStepThrough]
		[PublicAPI]
		[ContractAnnotation("value: true => halt")]
		public static void IfTrue(bool value, string message = "", params object[] args)
		{
			if (value)
				Always(message, args);
		}

		[DebuggerStepThrough]
		[PublicAPI]
		[ContractAnnotation("argumentValue: null => halt")]
		public static void IfArgumentNull(object argumentValue, string argumentName)
		{
			if (argumentValue == null)
				Always("Argument '{0}' was null.", argumentName);
		}

		[DebuggerStepThrough]
		[PublicAPI]
		[ContractAnnotation("argumentValue: null => halt")]
		public static void IfArgumentNullOrEmpty(string argumentValue, string argumentName)
		{
			IfArgumentNull(argumentValue, argumentName);

			if (argumentValue.Length == 0)
				Always("Argument '{0}' was empty.", argumentName);
		}

		[DebuggerStepThrough]
		[PublicAPI]
		public static void IfCollectionEmpty<T>(IEnumerable<T> collection) where T : class
		{
			IfArgumentNull(collection, "collection");

			if (collection.Any() == false)
				Always("Collection should not be empty but it is.");
		}

		[DebuggerStepThrough]
		[PublicAPI]
		[ContractAnnotation("collection: null => halt")]
		public static void IfCollectionNullOrEmpty<T>(IEnumerable<T> collection, string message, params object[] messageArgs)
			where T : class
		{
			if (collection == null || collection.Any() == false)
				Always(message, messageArgs);
		}

		[DebuggerStepThrough]
		[PublicAPI]
		[ContractAnnotation("collection: null => halt")]
		public static void IfCollectionNullOrEmpty<T>(IEnumerable<T> collection) where T : class
		{
			if (collection == null)
				Always("Collection should not be null but it is.");

			if (collection.Any() == false)
				Always("Collection should not be empty but it is.");
		}

		[DebuggerStepThrough]
		[PublicAPI]
		public static void IfCollectionContainsNull<T>(IEnumerable<T> collection) where T : class
		{
			IfArgumentNull(collection, "collection");

			IfTrue(collection.Contains(null), "Collection contains null object.");
		}

		[DebuggerStepThrough]
		[PublicAPI]
		public static void IfCollectionContains<T>(
			IEnumerable<T> collection, Func<T, bool> func, string message, params object[] messageArgs)
		{
			IfArgumentNull(collection, "collection");

			IfNotNull(collection.FirstOrDefault(func), message, messageArgs);
		}

		[DebuggerStepThrough]
		[PublicAPI]
		public static void IfEmpty(Guid toCheck, string message, params object[] messageArgs)
		{
			IfEqual(Guid.Empty, toCheck, message, messageArgs);
		}

		[DebuggerStepThrough]
		[PublicAPI]
		public static void IfArgumentEmpty(Guid toCheck, string argumentName)
		{
			IfEqual(Guid.Empty, toCheck, "Argument '{0}' was an empty Guid.", argumentName);
		}

		[DebuggerStepThrough]
		[PublicAPI]
		public static void IfNotCastable(object value, Type expectedType)
		{
			IfNotCastable(value, expectedType, NOT_CASTABLE_MESSAGE, expectedType.Name, value);
		}

		[DebuggerStepThrough]
		[PublicAPI]
		public static void IfNotCastable(object value, Type expectedType, string message, params object[] args)
		{
			if (value == null)
				return;

			if (expectedType.IsInstanceOfType(value) == false)
				Always(message, args);
		}

		[DebuggerStepThrough]
		[PublicAPI]
		public static void IfNotCastable<T>(object toCheck, string message, params object[] args)
		{
			IfNotCastable(toCheck, typeof(T), message, args);
		}

		[DebuggerStepThrough]
		public static void IfNotCastable<T>(object toCheck)
		{
			IfNotCastable(toCheck, typeof(T));
		}
	}

	[Serializable]
	public class DesignByContractViolationException : Exception
	{
		public DesignByContractViolationException(string message) : base(message)
		{
		}

		protected DesignByContractViolationException(SerializationInfo info, StreamingContext context) :
			base(info, context)
		{
		}
	}
}
