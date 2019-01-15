using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericSample
{
	public class CarAssembler : ICarAssembler
	{
		private readonly IEnumerable<ICar> _cars;

		public CarAssembler(IEnumerable<ICar> cars)
		{
			_cars = cars;
		}

		public void AssembleCars()
		{
			foreach (var car in _cars)
			{
				Console.WriteLine(nameof(car));
			}
		}
	}

	public interface ICarAssembler
	{
		void AssembleCars();
	}
}
