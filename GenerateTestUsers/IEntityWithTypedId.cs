using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateTestUsers
{
	public interface IEntityWithTypedId<out TId>
	{
		TId Id { get; }
		bool IsTransient();
	}
}
