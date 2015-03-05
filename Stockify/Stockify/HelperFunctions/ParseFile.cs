using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Stockify.HelperFunctions
{
    public class ParseFile
    {
        /*
        public List<string[]> parseCSV(string path)
        {
            List<string[]> parsedData = new List<string[]>();

            try
            {
                using (StreamReader readFile = new StreamReader(path))
                {
                    string line;
                    string[] row;

                    while ((line = readFile.ReadLine()) != null)
                    {
                        row = line.Split(',');
                        parsedData.Add(row);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return parsedData;
        }
        */
        /*
        public IEnumerable<Company> parseCSV(string input)
        {
            
                // We change file extension here to make sure it's a .csv file.
                string[] lines = File.ReadAllLines(input);
                string removeHeader = "Date,Open,High,Low,Close,Volume,Adj Close";
                string removeWhiteSpace = "";
                lines = lines.Where(val => val != removeHeader).ToArray();
                lines = lines.Where(val => val != removeWhiteSpace).ToArray();
                // lines.Select allows me to project each line as a Person. 
                // This will give me an IEnumerable<Person> back.
                return lines.Select(line =>
                {
                    string[] data = line.Split(',');
                    // We return a person with the data in order.
                 
                    return new Company(data[0], Convert.ToDecimal(data[1]),
                        Convert.ToDecimal(data[2]), Convert.ToDecimal(data[3]),
                        Convert.ToDecimal(data[4]), Convert.ToDouble(data[5]),
                        Convert.ToDecimal(data[6]));
                });
         }
        */
    }
}
