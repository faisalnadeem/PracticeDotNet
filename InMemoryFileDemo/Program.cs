using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InMemoryFileDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = "C:\\sftptemp\\Hali2.res";
            var content = File.ReadAllBytes(path);
            var inMemoryFile = new InMemoryFile(path, content);
            new MyFileParser().Parse(inMemoryFile);
        }
    }

    public class MyFileParser{
        public List<ParsedCallValidateOutputRow> Parse(InMemoryFile cvOutputFile)
        {
            var parsedRows = new List<ParsedCallValidateOutputRow>();
            var fileRows = Encoding.UTF8.GetString(cvOutputFile.Content)
                .Split(new[] { "\r", "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            var fileContainsRN = Encoding.UTF8.GetString(cvOutputFile.Content).Contains("\r\n");

            var rowNumber = 0;
            foreach (var row in fileRows)
            {
                if (RowWithoutAnyData(row))
                {
                    continue;
                }

                var parsedCVOutputRow = ParseRow(row, rowNumber);

                parsedRows.Add(parsedCVOutputRow);
                rowNumber++;
            }

            return parsedRows;
        }
        string[] _columns;
        public ParsedCallValidateOutputRow ParseRow(string rowAsString, int rowIndex)
        {
            var thinFileOutputColumnsCount = 350;
            var thinFileOutputColumnSeparator = '^';
            _columns = rowAsString.Split(thinFileOutputColumnSeparator);

            if (                _columns.Length != thinFileOutputColumnsCount)
                throw new Exception(
                $"Invalid columns number at line: {rowIndex + 1}. Was: {_columns.Length} columns, expected: {thinFileOutputColumnsCount}");

            return new ParsedCallValidateOutputRow(
                GetColumnAsInt(CallValidateOutputColumns.RID),
                TryGetColumnAsInt(CallValidateOutputColumns.autoML_numprimarychecks),
                GetColumn(CallValidateOutputColumns.autoML_matchlevel)
            );
        }

        private bool RowWithoutAnyData(string row)
        {
            var allColumnsEmpty = row.Replace("^".ToString(), string.Empty).IsNullOrWhitespace();

            return row.IsNullOrWhitespace() || allColumnsEmpty;
        }
        private string GetColumn(CallValidateOutputColumns columnName)
        {
            return _columns[(int)columnName];
        }

        private int GetColumnAsInt(CallValidateOutputColumns columnName)
        {
            return int.Parse(GetColumn(columnName));
        }

        private int? TryGetColumnAsInt(CallValidateOutputColumns columnName)
        {
            var column = GetColumn(columnName);
            return !column.IsNullOrWhitespace() ? int.Parse(column) : (int?)null;
        }

        internal void Parse(object inMemoryFile)
        {
            throw new NotImplementedException();
        }
    }

    public enum CallValidateOutputColumns
    {
        RID = 0,
        autoML_matchlevel = 83,
        autoML_numprimarychecks = 94
    }

    public class ParsedCallValidateOutputRow
    {
        public ParsedCallValidateOutputRow(int remoteId, int? numberOfPrimaryChecks, string reportType)
        {
            ReportType = reportType;
            NumberOfPrimaryChecks = numberOfPrimaryChecks;
            RemoteId = remoteId;
        }

        public int RemoteId { get; private set; }
        public int? NumberOfPrimaryChecks { get; private set; }
        public string ReportType { get; private set; }
    }

    public class InMemoryFile
    {
        public string Name { get; }
        public byte[] Content { get; }

        public InMemoryFile(string name, byte[] content)
        {
            Name = name;
            Content = content;
        }
    }

}
