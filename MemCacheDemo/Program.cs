using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;

namespace MemCacheDemo
{
    class Program
    {
        private const string FILE_CONTENT_CACHE_KEY = "filecontents";
        private const string fileName = @".\example.txt";
        
        static void Main(string[] args)
        {
            Console.WriteLine(Path.GetDirectoryName(Path.GetFullPath(fileName))); 
            bool readingFromCaching;
            var fileContent = GetFileContent(out  readingFromCaching);
            Console.Write(readingFromCaching ? "From CACHE " : "From File" + Environment.NewLine + fileContent + Environment.NewLine);
            fileContent = GetFileContent(out readingFromCaching);
            Console.Write(readingFromCaching ? "From CACHE " : "From File" + Environment.NewLine + fileContent + Environment.NewLine);
            fileContent = GetFileContent(out readingFromCaching);
            Console.Write(readingFromCaching ? "From CACHE " : "From File" + Environment.NewLine + fileContent + Environment.NewLine);
            fileContent = GetFileContent(out readingFromCaching);
            Console.Write(readingFromCaching ? "From CACHE " : "From File" + Environment.NewLine + fileContent + Environment.NewLine);
            fileContent = GetFileContent(out readingFromCaching);
            Console.Write(readingFromCaching ? "From CACHE " : "From File" + Environment.NewLine + fileContent + Environment.NewLine);

            Console.ReadLine();
        }

        private static string GetFileContent_MemCache(out bool readingFromCaching)
        {
            readingFromCaching = true;
            ObjectCache cache = MemoryCache.Default;  
            var fileContents = cache[FILE_CONTENT_CACHE_KEY] as string;

            if (fileContents != null) return fileContents;

            readingFromCaching = false;
            var policy = new CacheItemPolicy();  
  
            var filePaths = new List<string>();  
            filePaths.Add(Path.GetFullPath(fileName));  
  
            policy.ChangeMonitors.Add(new   
                HostFileChangeMonitor(filePaths));  
  
            // Fetch the file contents.  
            fileContents =   
                File.ReadAllText(Path.GetFullPath(fileName));  
  
            cache.Set(FILE_CONTENT_CACHE_KEY, fileContents, policy);

            return fileContents;  
        }

        protected static string GetFileContent(out bool readingFromCaching)
        {
            readingFromCaching = true;
            ObjectCache cache = MemoryCache.Default;
            string fileContents = cache[FILE_CONTENT_CACHE_KEY] as string;

            if (fileContents != null) return fileContents;

            var policy = new CacheItemPolicy();
            policy.AbsoluteExpiration =
                DateTimeOffset.Now.AddSeconds(10.0);

            List<string> filePaths = new List<string>();
            var directoryPath = Path.GetDirectoryName(Path.GetFullPath(fileName))??"";
            string cachedFilePath = Path.Combine( directoryPath , @"cacheText.txt");
            
            if (!File.Exists(cachedFilePath))
            {
                File.Create(cachedFilePath);
            }

            filePaths.Add(cachedFilePath);

            policy.ChangeMonitors.Add(new
                HostFileChangeMonitor(filePaths));

            // Fetch the file contents.
            fileContents = File.ReadAllText(cachedFilePath) + "\n"
                                                            + DateTime.Now.ToString();

            cache.Set(FILE_CONTENT_CACHE_KEY, fileContents, policy);

            return fileContents;
        }
    }
}
