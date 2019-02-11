using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NullablePropJsonRequiredAlwaysDemo
{
	class Program
	{
		static void Main(string[] args)
		{

			string json = @"{
							'Name': 'Starcraft III',
							'gameId': 0,
							'ReleaseDate': null
							}";

			var starcraft = JsonConvert.DeserializeObject<Videogame>(json);

			Console.WriteLine(starcraft.ToString());

			Console.WriteLine("Press any key to exit...");
			Console.ReadKey();
		}
	}

	public class Videogame
	{
		[JsonProperty(Required = Required.Always)]
		public string Name { get; set; }

		[JsonProperty(Required = Required.Always)]
		public int? GameId { get; set; }

		[JsonProperty(Required = Required.AllowNull)]
		public DateTime? ReleaseDate { get; set; }

		public override string ToString()
		{
			return $"Game Id:{GameId}, Name:{Name}, ReleaseDate:{ReleaseDate}";
		}
	}
}