using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CancellationTokenDemo
{
    public class ExportFileTest
    {
        private readonly TimeSpan TIMEOUT_SECONDS = TimeSpan.FromSeconds(3);

        public async Task MainTest()
        {
            var cancellationTokenSource = new CancellationTokenSource();

            cancellationTokenSource.CancelAfter(TIMEOUT_SECONDS);

            var cancellationToken = cancellationTokenSource.Token;
            var utcNow = DateTime.UtcNow;
            var schedules = await ExportConfigurationTableStorageClientGetSchedules(utcNow);

            foreach (var schedule in schedules)
            {
                var exportFiles = (await ExportFileTableStorageClientGetAll(schedule.CustomerId, schedule.Identifier))
                    .ToList();

                await HandleClosedFiles(exportFiles);

                await HandleClosingFiles(exportFiles);
            }
        }

        private async Task HandleClosingFiles(List<ExportFile> exportFiles)
        {
            
        }

        private async Task HandleClosedFiles(List<ExportFile> exportFiles)
        {
            

        }

        private async Task<IEnumerable<ExportFile>> ExportFileTableStorageClientGetAll(string scheduleCustomerId, string scheduleIdentifier)
        {
            return new List<ExportFile>()
            {
                new ExportFile()
            };
        }


        private async Task<IEnumerable<Schedule>> ExportConfigurationTableStorageClientGetSchedules(DateTime utcNow)
        {
            var list = new List<Schedule>();
            list.Add(new Schedule()
            {
                CustomerId = "cs_1",
                Identifier = Guid.NewGuid().ToString()
            });

            return list;
        }
    }

    internal class ExportFile
    {
    }

    internal class Schedule
    {
        public string CustomerId { get; set; }
        public string Identifier { get; set; }
    }
}
