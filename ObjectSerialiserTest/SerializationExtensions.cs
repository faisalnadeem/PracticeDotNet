using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ObjectSerialiserTest
{
	public static class SerializationExtensions
	{
		[PublicAPI]
		public static string SerializeToXml(this object value)
		{
			var serializer = new DataContractSerializer(value.GetType());
			using (var sw = new StringWriter())
			{
				var settings = new XmlWriterSettings { OmitXmlDeclaration = true, Indent = false };
				using (XmlWriter writer = XmlWriter.Create(sw, settings))
				{
					serializer.WriteObject(writer, value);
				}

				return sw.ToString();
			}
		}

		[PublicAPI]
		public static T DeserializeFromXml<T>(this string serialized)
		{
			var serializer = new DataContractSerializer(typeof(T));
			using (var sr = new StringReader(serialized))
			{
				using (XmlReader reader = XmlReader.Create(sr))
				{
					object value = serializer.ReadObject(reader);
					//Fail.IfNotCastable<T>(value);
					return (T)value;
				}
			}
		}

		public static string SerializeToSmartXml(this object obj, IEnumerable<Type> knownTypes = null)
		{
			Type objType = obj.GetType();
			var serializer = new DataContractSerializer(objType, knownTypes);
			using (var sw = new StringWriter())
			{
				var settings = new XmlWriterSettings { OmitXmlDeclaration = true, Indent = false };

				using (XmlWriter xml = XmlWriter.Create(sw, settings))
				{
					serializer.WriteStartObject(xml, obj);
					xml.WriteAttributeString("serialized-type", objType.FullName + ", " + objType.Assembly.GetName().Name);
					serializer.WriteObjectContent(xml, obj);
					serializer.WriteEndObject(xml);
				}

				return sw.ToString();
			}
		}

		public static object DeserializeFromSmartXml(this string data, IEnumerable<Type> knownTypes = null)
		{
			using (var sr = new StringReader(data))
			{
				var settings = new XmlReaderSettings();
				using (XmlReader xml = XmlReader.Create(sr, settings))
				{
					xml.Read();
					string serializedObjectType = xml.GetAttribute("serialized-type");
					if (String.IsNullOrEmpty(serializedObjectType))
					{
						throw new SerializationException(
							"Missing the 'type' argument to enable automatic deserialization");
					}

					Type type = Type.GetType(serializedObjectType, true);
					var serializer = new DataContractSerializer(type, knownTypes);

					return serializer.ReadObject(xml);
				}
			}
		}
	}
}
