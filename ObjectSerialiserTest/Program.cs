using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ObjectSerialiserTest
{
	class Program
	{
		static void Main(string[] args)
		{
			AbstractClassDeserializer.Test();
			return;

			new PersonSerialTest().SerializeAndDeserialize(GetProgrammer());
			var userData = new UserData { UserId = 0 };
			var userDataString = JsonConvert.SerializeObject(userData);

			var simpleUser = new SimpleUser(){UserId = 1, Name = "Simple"};
			var simpleUserDataString = JsonConvert.SerializeObject(simpleUser);

			var complexUser = new SimpleUser(){UserId = 2, Name = "Complex"};
			var complexUserDataString = JsonConvert.SerializeObject(simpleUser);

			var typeOfObject = typeof(UserData).Name;
			var typeOfSimpleObject = typeof(SimpleUser).Name;

			Type t = Type.GetType("UserData");
			Type t1 = Type.GetType("ComplexUser");

			var usr = JsonConvert.DeserializeObject(simpleUserDataString, t);

			

			//var userDataDeserialized = (( Type.GetType(typeOfSimpleObject).Assembly).GetType()) JsonConvert.DeserializeObject(simpleUserDataString);

		}

		private static void DowWork(TUser user) 
		{

		}

		public static Person GetProgrammer()
		{
			return new Person()
			{
				Profession = new Programming()
				{
					FavoriteLanguage = "C#"
				}
			};
		}

	}

	public abstract class TUser
	{
	}

	public class UserData
	{
		public int UserId { get; set; }
		public string Email { get; set; }
		public BrandedAccount? BrandedAccount { get; set; }
		public string FirstName { get; set; }
		public Guid CustomerId { get; set; }
	}

	public class SimpleUser : UserData
	{
		public string Name { get;set; }
	}
	public class ComplexUser : UserData
	{
		public string FullName { get;set; }
		public string Address { get; set; }
	}


}
