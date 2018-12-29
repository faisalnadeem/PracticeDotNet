using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ObjectSerialiserTest
{
	public class PersonSerialTest
	{
		public Person SerializeAndDeserialize(Person person)
		{
			var indented = Formatting.Indented;
			var settings = new JsonSerializerSettings()
			{
				TypeNameHandling = TypeNameHandling.All
			};
			var serialized = JsonConvert.SerializeObject(person, indented, settings);
			var deserialized = JsonConvert.DeserializeObject(serialized, settings);
			return (Person) deserialized;
		}

	}

	public class Person
	{
		public IProfession Profession { get; set; }
	}

	public interface IProfession
	{
		string JobTitle { get; }
	}

	public class Programming : IProfession
	{
		public string JobTitle => "Software Developer";
		public string FavoriteLanguage { get; set; }
	}

	public class Writing : IProfession
	{
		public string JobTitle => "Copywriter";
		public string FavoriteWord { get; set; }
	}
}
