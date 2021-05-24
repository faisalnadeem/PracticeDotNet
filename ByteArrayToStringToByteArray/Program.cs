using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ByteArrayToStringToByteArray
{
    class Program
    {
        static async Task Main(string[] args)
        {

            await new NewlineAppenderTest().Test();

            Console.ReadLine();
            return;

            //Encoding.UTF8.GetString();// 
            //var utf8Encoding = new UTF8Encoding(!BitConverter.IsLittleEndian, true);
            //var stringContent = "It was the best of times, it was the worst of times...";
            //// We need to dimension the array, since we'll populate it with 2 method calls.
            //var bytes = new byte[utf8Encoding.GetByteCount(stringContent) + utf8Encoding.GetPreamble().Length];
            //// Encode the string.
            //Array.Copy(utf8Encoding.GetPreamble(), bytes, utf8Encoding.GetPreamble().Length);

            //utf8Encoding.GetBytes(stringContent, 0, stringContent.Length, bytes, utf8Encoding.GetPreamble().Length);

            //// Decode the byte array.
            //var s2 = utf8Encoding.GetString(bytes, 0, bytes.Length);
            //Console.WriteLine(s2);

            //var s3 = $"[{s2}]";
            //var bytesWithAddedBrackets = new byte[utf8Encoding.GetByteCount(s3) + utf8Encoding.GetPreamble().Length];
            //Array.Copy(utf8Encoding.GetPreamble(), bytesWithAddedBrackets, utf8Encoding.GetPreamble().Length);
            //utf8Encoding.GetBytes(s3, 0, stringContent.Length, bytesWithAddedBrackets, utf8Encoding.GetPreamble().Length);

            //var s4 = utf8Encoding.GetString(bytesWithAddedBrackets, 0, bytesWithAddedBrackets.Length);

            var filePath = "C:\\temp\\cs_1826495586715566080_2021-04-07.json";

            var fileContent = await File.ReadAllTextAsync(filePath);


            var response = new HttpResponseMessage
            {
                Content = new StringContent(fileContent)
            };

            var byteArrayString = await response.Content.ReadAsByteArrayAsync();

            var fileContentString = Encoding.UTF8.GetString(byteArrayString); // .GetString(byteArrayString, fileContent.Length);

            //var wrappedFileContent = $"[{Environment.NewLine}{fileContentString.TrimEnd('\r', '\n', ',')}{Environment.NewLine}]";
            var wrappedFileContent = $"[{fileContentString.TrimEnd('\r', '\n', ',')}]";


            var newByteArray = Encoding.UTF8.GetBytes(wrappedFileContent);

            var backToBytes = Encoding.UTF8.GetString(newByteArray, 0, newByteArray.Length);

            Console.WriteLine(backToBytes);

            Console.ReadLine();
        }
    }
}
