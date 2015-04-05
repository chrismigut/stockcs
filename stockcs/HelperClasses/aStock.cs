using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Windows.Forms;

namespace stockcs.HelperClasses
{
    public class aStock
    {
        private string _period;
        private string companyTicker;
        aPeriodType PeriodType;
        DateTime StartingDate = new DateTime();
        DateTime EndingDate = new DateTime();
        // DateTime StartingDate, EndingDate;
        public aStock() { }
        public aStock(string resolution, DateTime startTime, DateTime endTime, string ticker)
        {
            //aPeriodType PeriodType 
            if (resolution.ToUpper() == "DAILY")
            {
                PeriodType = aPeriodType.DAILY;
            }
            else if (resolution.ToUpper() == "WEEKLY")
            {
                PeriodType = aPeriodType.WEEKLY;
            }
            else if (resolution.ToUpper() == "MONTHLY")
            {
                PeriodType = aPeriodType.MONTHLY;
            }
            StartingDate = startTime;
            EndingDate = endTime;

            companyTicker = ticker;
        }
        enum aPeriodType
        {
            DAILY,
            WEEKLY,
            MONTHLY
        };




        /// <summary>
        /// Read from a csv file
        /// </summary>
        public List<aCandlestick> ReadFromFile(string filename)
        {
            List<aCandlestick> Candlestick = new List<aCandlestick>();
            string filePath = "";
            // try
            //{
            switch (PeriodType)
            {
                case aPeriodType.DAILY:
                    filePath = @"C:\DEV\STOCKDATA\Daily\" + filename + ".csv";
                    break;
                case aPeriodType.WEEKLY:
                    filePath = @"C:\DEV\STOCKDATA\Weekly\" + filename + ".csv";
                    break;
                case aPeriodType.MONTHLY:
                    filePath = @"C:\DEV\STOCKDATA\Monthly\" + filename + ".csv";
                    break;
            }
            int compareDates = EndingDate.CompareTo(StartingDate);
            if (compareDates > 0)
            {
                DateTime recordDate = new DateTime();
                int validLowerBound, validUpperBound;

                //***********************
                //This is a valid date to, trim with selected dates. 
                //***********************
                // read from saved file
                string[] lines = File.ReadAllLines(filePath);
                string removeHeader = "Date,Open,High,Low,Close,Volume,Adj Close";
                string removeWhiteSpace = "";
                lines = lines.Where(val => val != removeHeader).ToArray();
                lines = lines.Where(val => val != removeWhiteSpace).ToArray();
                foreach (var line in lines)
                {
                    string[] data = line.Split(',');
                    recordDate = DateTime.Parse(data[0]);
                    validLowerBound = recordDate.CompareTo(StartingDate);
                    validUpperBound = recordDate.CompareTo(EndingDate);

                    if ((validLowerBound >= 0) && (validUpperBound <= 0))
                    {
                        aCandlestick tickerRow = new aCandlestick(decimal.Parse(data[1]), decimal.Parse(data[2]), decimal.Parse(data[3]), decimal.Parse(data[4]), double.Parse(data[5]), DateTime.Parse(data[0]));
                        Candlestick.Add(tickerRow);
                    }
                }
            }
            //This will poopulate the candlestick this foreach was to test the reasults. 
            //foreach (var item in Candlestick)
            //{
            //    MessageBox.Show(item.Open.ToString() + item.High.ToString() + item.Low.ToString() + item.Close.ToString() + item.Volume.ToString() + item.StartingDate.ToShortDateString());
            //}
            return Candlestick;

            //}
            //catch
            //{
            //    MessageBox.Show("Error: readFromFile");
            //    throw;
            //}
        }

        /// <summary>
        /// Read from Yahoo
        /// </summary>
        public void ReadFromURL(string URL)
        {
            string returnQueryText = "";

            //Connection to yahoo server and download infomation
            var webClient = new WebClient();
            byte[] data = webClient.DownloadData(URL);
            int index = data.Count();
            returnQueryText = index.ToString();
            string str = System.Text.Encoding.Default.GetString(data);
            returnQueryText = str;

            SaveToFile(returnQueryText);
        }

        /// <summary>
        /// Save data to a file 
        /// </summary>
        public void SaveToFile(string SaveToFile)
        {
            try
            {
                //Queries sendQuery = new Queries();

                //Check to see if directories exist before they written to
                //veriflyDirectories();
                switch (PeriodType)
                {
                    case aPeriodType.DAILY:
                        using (StreamWriter w = File.CreateText("C:\\DEV\\STOCKDATA\\Daily\\" + companyTicker + ".csv"))
                        {
                            w.WriteLine(SaveToFile);
                        }
                        break;
                    case aPeriodType.WEEKLY:
                        using (StreamWriter w = File.CreateText(@"C:\DEV\STOCKDATA\Weekly\" + companyTicker + ".csv"))
                        {
                            w.WriteLine(SaveToFile);
                        }
                        break;
                    case aPeriodType.MONTHLY:
                        using (StreamWriter w = File.CreateText(@"C:\DEV\STOCKDATA\Monthly\" + companyTicker + ".csv"))
                        {
                            w.WriteLine(SaveToFile);
                        }
                        break;
                    default:
                        using (StreamWriter w = File.CreateText(@"C:\DEV\STOCKDATA\NASDAQ\companylist.csv"))
                        {
                            w.WriteLine(SaveToFile);
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: writeToFile | " + e.ToString());
            }
        }
    }
}
