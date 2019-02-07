using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace ObjectSerialiserTest
{
	public class AbstractClassDeserializer
	{
		public static void Test()
		{
			var jset = new JsonSerializerSettings() {TypeNameHandling = TypeNameHandling.All};
			Base b = new Der();
			string json = JsonConvert.SerializeObject(b, jset);
			Base c = (Base) JsonConvert.DeserializeObject(json, jset);

			var model = new NoddleNoAlertNotificationEmailModel("test@testfn.co",
				null,
				"FirstName",
				Guid.NewGuid());


			string json1 = JsonConvert.SerializeObject(model, jset);


				var templateName = model.GetType().Name;
			switch (templateName)
			{
					case nameof(NoddleNoAlertNotificationEmailModel):
						Console.WriteLine($"{templateName} = {nameof(NoddleNoAlertNotificationEmailModel)}");
						return;
					default:
						Console.WriteLine($"{templateName} not found");
						return;
			}
			//try
			//{
			//	var typeFullName = model.GetType().FullName;
			//	var json2 = JsonConvert.SerializeObject(model);
			//	var deserializedModel = JsonConvert.DeserializeObject<EmailModel>(json2, new ParametersContructorConverter(typeFullName));
			//}
			//catch (Exception e)
			//{
			//	Console.WriteLine(e);
			//}

			//return;


			//try
			//{
			//	EmailModel emodel = (EmailModel) JsonConvert.DeserializeObject(json1, jset); //Collection was of a fixed size
			//}
			//catch (Exception e)
			//{
			//	Console.WriteLine(e);
			//}
			//EmailModel emodel = (EmailModel) JsonConvert.DeserializeObject(json1, jset);//Collection was of a fixed size


			//try
			//{
			//	var typeFullName = model.GetType().FullName;
			//	EmailModel dsModel = (EmailModel) JsonConvert.DeserializeObject(json1, new JsonSerializerSettings{
			//		ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
			//	});


			//}
			//catch (Exception e)
			//{
			//	Console.WriteLine(e);
			//}

			//try
			//{
			//	var json2 = JsonConvert.SerializeObject(model, jset);
			//	var deserializedModel = (EmailModel) JsonConvert.DeserializeObject(json2, jset);
			//}
			//catch (Exception e)
			//{
			//	Console.WriteLine(e);
			//}

		}

		//private static void CtorFallback(object sender, ConstructorHandlingFallbackEventArgs e)
		//{

		//	List<object> args = new List<object>();
		//	foreach (var p in e.ObjectContract.Properties)
		//	{
		//		args.Add(p.PropertyType.IsValueType ? Activator.CreateInstance(p.PropertyType) : null);
		//	}

		//	e.Object = Activator.CreateInstance(e.ObjectContract.UnderlyingType, args.ToArray());

		//	e.Handled = true;
		//}

	}

	public class ParametersContructorConverter : JsonConverter
	{

		public ParametersContructorConverter(string typeName)
		{
			ChildObjectType = Type.GetType(typeName);

		}

		public Type ChildObjectType { get; set; }

		public override bool CanConvert(Type objectType)
		{
			return objectType.IsAssignableFrom(ChildObjectType); //ChildObjectType.BaseType.IsAssignableFrom(objectType);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var jObject = JObject.Load(reader);
			var contructor = ChildObjectType.GetConstructors().FirstOrDefault();

			if (contructor == null)
			{
				return serializer.Deserialize(reader);
			}

			var parameters = contructor.GetParameters();			
			var values = parameters.Select(p => jObject.GetValue(p.Name, StringComparison.InvariantCultureIgnoreCase)?.ToObject(p.ParameterType)).ToArray();

			return contructor.Invoke(values);
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			serializer.Serialize(writer, value);
		}
	}

	public abstract class Base
	{
		protected Base()
		{
		}

		protected Base(int x, int y)
		{
			X = x;
			Y = y;
		}
		protected Base(int x, int y, int z)
		{
			X = x;
			Y = y;
			Z = z;

		}

		public int Y { get; set; }

		public int X { get; set; }


		public int Z { get; set; }

		public abstract int GetInt();
	}

	public class AddTwoNumbers : Base
	{
		public AddTwoNumbers(int x, int y) : base(x, y)
		{
		}
		public override int GetInt()
		{
			throw new NotImplementedException();
		}
	}

	public class Der : Base
	{
		public Der(){ }
		int g = 5;
		public override int GetInt()
		{
			return g + 2;
		}
	}

	public class Der2 : Base
	{
		int i = 10;
		public override int GetInt()
		{
			return i + 17;
		}
	}
}
