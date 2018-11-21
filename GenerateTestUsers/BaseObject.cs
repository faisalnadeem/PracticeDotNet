using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GenerateTestUsers
{
	[Serializable]
	public abstract class BaseObject
	{
		/// <summary>
		///     To help ensure hashcode uniqueness, a carefully selected random number multiplier 
		///     is used within the calculation.  Goodrich and Tamassia's Data Structures and
		///     Algorithms in Java asserts that 31, 33, 37, 39 and 41 will produce the fewest number
		///     of collissions.  See http://computinglife.wordpress.com/2008/11/20/why-do-hash-functions-use-prime-numbers/
		///     for more information.
		/// </summary>
		private const int HashMultiplier = 31;

		/// <summary>
		///     This static member caches the domain signature properties to avoid looking them up for 
		///     each instance of the same type.
		/// 
		///     A description of the very slick ThreadStatic attribute may be found at 
		///     http://www.dotnetjunkies.com/WebLog/chris.taylor/archive/2005/08/18/132026.aspx
		/// </summary>
		[ThreadStatic] private static Dictionary<Type, IEnumerable<PropertyInfo>> _signaturePropertiesDictionary;

		public override bool Equals(object obj)
		{
			var compareTo = obj as BaseObject;
			if (ReferenceEquals(this, compareTo))
				return true;

			// ReSharper disable CheckForReferenceEqualityInstead.2
			return compareTo != null && Equals(GetType(), compareTo.GetTypeUnproxied()) &&
				   // ReSharper restore CheckForReferenceEqualityInstead.2
				   HasSameObjectSignatureAs(compareTo);
		}

		/// <summary>
		///     This is used to provide the hashcode identifier of an object using the signature 
		///     properties of the object; although it's necessary for NHibernate's use, this can 
		///     also be useful for business logic purposes and has been included in this base 
		///     class, accordingly.  Since it is recommended that GetHashCode change infrequently, 
		///     if at all, in an object's lifetime, it's important that properties are carefully
		///     selected which truly represent the signature of an object.
		/// </summary>
		public override int GetHashCode()
		{
			unchecked
			{
				var signatureProperties = GetSignatureProperties().ToList();

				// It's possible for two objects to return the same hash code based on 
				// identically valued properties, even if they're of two different types, 
				// so we include the object's type in the hash calculation
				var hashCode = GetType().GetHashCode();

				hashCode = signatureProperties.Select(property => property.GetValue(this, null))
					.Where(value => value != null)
					.Aggregate(hashCode, (current, value) => (current * HashMultiplier) ^ value.GetHashCode());

				if (signatureProperties.Any())
					return hashCode;

				// If no properties were flagged as being part of the signature of the object,
				// then simply return the hashcode of the base object as the hashcode.
				// ReSharper disable BaseObjectGetHashCodeCallInGetHashCode
				return base.GetHashCode();
				// ReSharper restore BaseObjectGetHashCodeCallInGetHashCode
			}
		}

		/// <summary>
		/// </summary>
		private IEnumerable<PropertyInfo> GetSignatureProperties()
		{
			IEnumerable<PropertyInfo> properties;

			// Init the signaturePropertiesDictionary here due to reasons described at 
			// http://blogs.msdn.com/jfoscoding/archive/2006/07/18/670497.aspx
			if (_signaturePropertiesDictionary == null)
			{
				_signaturePropertiesDictionary = new Dictionary<Type, IEnumerable<PropertyInfo>>();
			}

			if (_signaturePropertiesDictionary.TryGetValue(GetType(), out properties))
			{
				return properties;
			}

			return _signaturePropertiesDictionary[GetType()] = GetTypeSpecificSignatureProperties();
		}

		/// <summary>
		///     You may override this method to provide your own comparison routine.
		/// </summary>
		public virtual bool HasSameObjectSignatureAs(BaseObject compareTo)
		{
			var signatureProperties = GetSignatureProperties().ToList();

			if ((from property in signatureProperties
				 let valueOfThisObject = property.GetValue(this, null)
				 let valueToCompareTo = property.GetValue(compareTo, null)
				 where valueOfThisObject != null || valueToCompareTo != null
				 where (valueOfThisObject == null ^ valueToCompareTo == null) || (!valueOfThisObject.Equals(valueToCompareTo))
				 select valueOfThisObject).Any())
			{
				return false;
			}

			// If we've gotten this far and signature properties were found, then we can
			// assume that everything matched; otherwise, if there were no signature 
			// properties, then simply return the default bahavior of Equals
			// ReSharper disable BaseObjectEqualsIsObjectEquals
			return signatureProperties.Any() || base.Equals(compareTo);
			// ReSharper restore BaseObjectEqualsIsObjectEquals
		}

		/// <summary>
		///     Enforces the template method pattern to have child objects determine which specific 
		///     properties should and should not be included in the object signature comparison. Note
		///     that the the BaseObject already takes care of performance caching, so this method 
		///     shouldn't worry about caching...just return the goods man!
		/// </summary>
		protected abstract IEnumerable<PropertyInfo> GetTypeSpecificSignatureProperties();

		/// <summary>
		///     When NHibernate proxies objects, it masks the type of the actual entity object.
		///     This wrapper burrows into the proxied object to get its actual type.
		/// 
		///     Although this assumes NHibernate is being used, it doesn't require any NHibernate
		///     related dependencies and has no bad side effects if NHibernate isn't being used.
		/// 
		///     Related discussion is at http://groups.google.com/group/sharp-architecture/browse_thread/thread/ddd05f9baede023a ...thanks Jay Oliver!
		/// </summary>
		protected virtual Type GetTypeUnproxied()
		{
			return GetType();
		}
	}
}
