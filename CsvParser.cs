
namespace CsvParserApp
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Parses csv file
    /// </summary>
    public class CsvParser : IEnumerable<DataRow>
    {
        /// <summary>
        /// Contains columns names
        /// </summary>
        public List<string> Headers { get; private set; }

        /// <summary>
        /// Contains all rows without header
        /// </summary>
        private List<DataRow> Content { get; set; }

        /// <summary>
        /// Parses csv located in specified path separated by commas by default
        /// </summary>
        /// <param name="path">path including file name</param>
        /// <param name="columnDelimiter">column separator</param>
        public CsvParser Parse(string path, char columnDelimiter = ',')
        {
            var separator = new char[] { columnDelimiter };

            var regexp = new Regex($"{columnDelimiter}(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }

            var rows = File.ReadAllText(path)
                .Split(new[] { Environment.NewLine }, StringSplitOptions.None)
                .ToList();

            this.Headers = rows.First().Split(separator).Select(h => h.Trim()).ToList();
            this.Content = rows.Skip(1)
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(s => new DataRow { Columns = this.Headers, Row = regexp.Split(s, Headers.Count).ToList() })
                .ToList();

            // clean up the fields (remove quotes " " and leading spaces)
            this.Content.ForEach(dr => dr.Row = dr.Row.Select(s => s = s.TrimStart(' ', '"').TrimEnd('"', ' ')).ToList());

            return this;
        }

        public IEnumerator<DataRow> GetEnumerator()
        {
            return this.Content.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    /// <summary>
    /// Represents csv record 
    /// </summary>
    public class DataRow
    {
        public List<string> Row { get; set; }

        public List<string> Columns { get; set; }

        /// <summary>
        /// Extracts particular <see cref="DataRow"/> by column name 
        /// </summary>
        /// <param name="columnName">columnName</param>
        /// <returns></returns>
        public string this[string columnName] => this.Row[this.Columns.IndexOf(columnName)];

        /// <summary>
        /// Extracts particular <see cref="DataRow"/> by index
        /// </summary>
        /// <param name="index">index</param>
        /// <returns></returns>
        public string this[int index] => this.Row[index];
    }
}
