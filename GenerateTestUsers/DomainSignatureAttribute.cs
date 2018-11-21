using System;

namespace GenerateTestUsers
{
	[Serializable]
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class DomainSignatureAttribute : Attribute
	{
	}
}