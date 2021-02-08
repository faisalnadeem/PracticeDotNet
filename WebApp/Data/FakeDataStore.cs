using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Data
{
	public class FakeDataStore
	{
		private const int START_INDEX = 1;
		private const int STOP_INDEX = 5;

		public IEnumerable<CompanyInformation> GetCompanyInformations()
		{
			var abos = CreateInstance<CompanyInformation>();

			var list = new List<CompanyInformation>();
			for (int i = START_INDEX; i < STOP_INDEX; i++)
			{
				list.Add(new CompanyInformation{Id = i, Name = $"Company-{i}", AccountsOfficeRef = "Accou"});
			}
			return list;
		}

		public T CreateInstance<T>() where T : class
		{
			var obj = Activator.CreateInstance<T>();

			var propertyInfo = GetAllPropertiesFor(obj);

			return obj;
		}

		public static string[] GetAllPropertiesFor(object obj)
		{
			if (obj == null) return new string[] { };
			var type = obj.GetType();
			var props = type.GetProperties();
			var propNames = new List<string>();
			foreach (var prp in props)
			{
				propNames.Add(prp.Name);
			}
			return propNames.ToArray();
		}

		private void SetValue(PropertyInfo prop, Type type)
		{
			var propertyType = prop.PropertyType;
			try
			{
				// Get the type code so we can switch
				var typeCode = Type.GetTypeCode(propertyType);
                string value = "";
                switch (typeCode)
				{
					case TypeCode.Int32:
						prop.SetValue(type, Convert.ToInt32(value), null);
						break;
					case TypeCode.Int64:
						prop.SetValue(type, Convert.ToInt64(value), null);
						break;
					case TypeCode.String:
						prop.SetValue(type, value, null);
						break;
					case TypeCode.Object:
						if (propertyType == typeof(Guid) || propertyType == typeof(Guid?))
						{
                            object obj = null;
                            prop.SetValue(obj, Guid.Parse(value), null);
							return;
						}
						break;
					default:
						prop.SetValue(type, value, null);
						break;
				}

				return;
			}
			catch (Exception ex)
			{
				throw new Exception("Failed to set property value for our Foreign Key");
			}
		} 
	}
}
