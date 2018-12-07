using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLCompressorSample
{
	class Program
	{
		static void Main(string[] args)
		{
			XmlDataStore.TestMain();
		}
	}

	public class XmlDataStore
	{
		private static string directoryPath = @"c:\sftptemp";
		public static void TestMain()
		{
			var directorySelected = new DirectoryInfo(directoryPath);
			Compress(directorySelected);

			foreach (var fileToDecompress in directorySelected.GetFiles("*.gz"))
			{
				Decompress(fileToDecompress);
			}
		}

		public static void CompressDirectoryWithSubs()
		{
			string basePath = @"e:\";

			Queue<string> subDirectories = new Queue<string>();
			subDirectories.Enqueue(basePath);

			string path = null;
			while (subDirectories.Count > 0)
			{
				path = subDirectories.Dequeue();

				foreach (var file in Directory.EnumerateFiles(basePath))
				{
					// Add file to Zip
					// If you need the relative path to the file or directory, use 
					// http://stackoverflow.com/questions/275689/how-to-get-relative-path-from-absolute-path
				}

				foreach (var subDirectory in Directory.EnumerateDirectories(basePath))
				{
					subDirectories.Enqueue(subDirectory);
				}
			}
		}

		public static void Compress(DirectoryInfo directorySelected)
		{
			foreach (FileInfo fileToCompress in directorySelected.GetFiles())
			{
				using (FileStream originalFileStream = fileToCompress.OpenRead())
				{
					if ((File.GetAttributes(fileToCompress.FullName) &
					   FileAttributes.Hidden) != FileAttributes.Hidden & fileToCompress.Extension != ".gz")
					{
						using (FileStream compressedFileStream = File.Create(fileToCompress.FullName + ".gz"))
						{
							using (GZipStream compressionStream = new GZipStream(compressedFileStream,
							   CompressionMode.Compress))
							{
								originalFileStream.CopyTo(compressionStream);

							}
						}
						FileInfo info = new FileInfo(directoryPath + "\\" + fileToCompress.Name + ".gz");
						Console.WriteLine("Compressed {0} from {1} to {2} bytes.",
						fileToCompress.Name, fileToCompress.Length.ToString(), info.Length.ToString());
					}

				}
			}
		}

		public static void Decompress(FileInfo fileToDecompress)
		{
			using (FileStream originalFileStream = fileToDecompress.OpenRead())
			{
				string currentFileName = fileToDecompress.FullName;
				string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

				using (FileStream decompressedFileStream = File.Create(newFileName))
				{
					using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
					{
						decompressionStream.CopyTo(decompressedFileStream);
						Console.WriteLine("Decompressed: {0}", fileToDecompress.Name);
					}
				}
			}
		}
	}
}
