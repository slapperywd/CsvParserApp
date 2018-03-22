namespace CsvParserApp
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Parses csv file
    /// </summary>
    public class CsvParser
    {
        /// <summary>
        /// Contains columns names
        /// </summary>
        public List<string> Headers { get; private set; }

        /// <summary>
        /// Contains all rows without header
        /// </summary>
        public List<List<string>> Content { get; private set; }

        /// <summary>
        /// Parses csv located in specified path separated by commas by default
        /// </summary>
        /// <param name="path">file path including name</param>
        /// <param name="delemiters">csv separators</param>
        public void Parse(string path, char[] delemiters = null)
        {
            char[] separators = delemiters ?? new[] { ',' };

            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }

            List<string> rows = File.ReadAllText(path).Split('\n').ToList();

            this.Headers = rows.First().Split(separators).ToList();
            this.Content = rows.Skip(1)
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(s => s.Split(separators).ToList()).ToList();
        }
    }
}
