﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadDemo
{
    public class TaskExampleOnlyRanToCompletion
    {
        public static void TestTask()
        {
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            var files = new List<Tuple<string, string, long, DateTime>>();

            var t = new Task(() =>
            {
                string dir = "C:\\Windows\\System32\\";
                object obj = new Object();
                if (Directory.Exists(dir))
                {
                    Parallel.ForEach(Directory.GetFiles(dir),
                        f =>
                        {
                            if (token.IsCancellationRequested)
                                token.ThrowIfCancellationRequested();
                            var fi = new FileInfo(f);
                            lock (obj)
                            {
                                files.Add(Tuple.Create(fi.Name, fi.DirectoryName, fi.Length, fi.LastWriteTimeUtc));
                            }
                        });
                }
            }).ContinueWith((x) => { Console.WriteLine("hiil"); }, CancellationToken.None,
                TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.Current);
            //t.Start();
            tokenSource.Cancel();
            try
            {
                t.Start();
                t.Wait();
                Console.WriteLine("Retrieved information for {0} files.", files.Count);
            }
            catch (AggregateException e)
            {
                Console.WriteLine("Exception messages:");
                foreach (var ie in e.InnerExceptions)
                    Console.WriteLine("   {0}: {1}", ie.GetType().Name, ie.Message);

                Console.WriteLine("\nTask status: {0}", t.Status);
            }
            finally
            {
                tokenSource.Dispose();
            }
        }
    }
}