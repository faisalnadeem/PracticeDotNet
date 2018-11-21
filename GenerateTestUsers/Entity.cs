using System;

namespace GenerateTestUsers
{
	[Serializable]
	public abstract class Entity : EntityWithTypedId<int>
	{
	}
}