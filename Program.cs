using System;

namespace CsvParserApp
{
    class Program
    {
        static void Main(string[] args)
        {
            CsvParser csvParser = new CsvParser();
            csvParser.Parse("EightKSearch2.csv");


            foreach (var header in csvParser.Headers)
            {
                Console.WriteLine(header);
            }

            foreach (var row in csvParser.Content)
            {
                foreach (var cell in row)
                {
                    Console.WriteLine(cell);
                }
            }
        }
    }
}
