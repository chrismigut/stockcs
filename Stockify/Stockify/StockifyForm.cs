using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Stockify.HelperClasses;
using Stockify.HelperFunctions;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;
namespace Stockify
{
    public partial class StockifyForm : Form
    {
        SearchQuery Query = new SearchQuery();
        Queries Yahoo_Call = new Queries();
        CustomFileHander QueryFile = new CustomFileHander();
        QueryAssembly QueryAssembly = new QueryAssembly();

        string fileLocation = "";
        string loadMethod = "";

        public StockifyForm()
        {
            InitializeComponent();
            DateTime dtEndDate = DateTime.Today;//.AddDays(-1);
            //init the dates
            Query.startDay = dtpStartDate.Value.Day.ToString();
            // yahoo's finance api requires months to start at zero instead of 1
            // example (look at &a=00, &d=11): http://finance.yahoo.com/q/hp?s=AAPL&a=00&b=24&c=1990&d=11&e=16&f=2014&g=d
            Query.startMonth = (dtpStartDate.Value.Month - 1).ToString();
            Query.startYear = dtpStartDate.Value.Year.ToString();

            Query.endDay = dtpEndDate.Value.Day.ToString();
            // yahoo's finance api requires months to start at zero instead of 1
            // example (look at &a=00, &d=11): http://finance.yahoo.com/q/hp?s=AAPL&a=00&b=24&c=1990&d=11&e=16&f=2014&g=d
            Query.endMonth = (dtpStartDate.Value.Month - 1).ToString();
            Query.endYear = dtpEndDate.Value.Year.ToString();

            // build grid
            lsvStock.View = View.Details;
            lsvStock.GridLines = true;
            lsvStock.Sorting = SortOrder.Descending;
            lsvStock.Columns.Add("Date");
            lsvStock.Columns.Add("Open");
            lsvStock.Columns.Add("High");
            lsvStock.Columns.Add("Low");
            lsvStock.Columns.Add("Close");
            lsvStock.Columns.Add("Volume");
            lsvStock.Columns.Add("Adj Close");
        }

        /// <summary>
        /// Date Picker -> Get Starting Date
        /// </summary>
        private void dtpStartDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                //Start Date
                //User has selected a starting date
                Query.startDay = dtpStartDate.Value.Day.ToString();
                // yahoo's finance api requires months to start at zero instead of 1
                // example (look at &a=00, &d=11): http://finance.yahoo.com/q/hp?s=AAPL&a=00&b=24&c=1990&d=11&e=16&f=2014&g=d
                Query.startMonth = (dtpStartDate.Value.Month - 1).ToString();
                Query.startYear = dtpStartDate.Value.Year.ToString();
            }
            catch
            {
                MessageBox.Show("Error: dtpStartDateFS_ValueChanged method");
            }
        }

        /// <summary>
        /// Date Picker -> Get Ending Date
        /// </summary>
        private void dtpEndDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                //Ending Date
                //User has selected a Ending date
                Query.endDay = dtpEndDate.Value.Day.ToString();
                // yahoo's finance api requires months to start at zero instead of 1
                // example (look at &a=00, &d=11): http://finance.yahoo.com/q/hp?s=AAPL&a=00&b=24&c=1990&d=11&e=16&f=2014&g=d
                Query.endMonth = (dtpEndDate.Value.Month - 1).ToString();
                Query.endYear = dtpEndDate.Value.Year.ToString();
            }
            catch
            {
                MessageBox.Show("Error: dtpEndDate_ValueChanged method");
            }
        }

        /// <summary>
        /// Resolution -> User Picked Daily
        /// </summary>
        private void rbDaily_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbDaily.Checked == true)
                {
                    Query.isDaily = true;
                    Query.isWeekly = false;
                    Query.isMonthly = false;
                    fileLocation = "Daily";
                }
            }
            catch
            {
                MessageBox.Show("Error: rbDaily_CheckedChanged");
            }
        }

        /// <summary>
        /// Resolution -> User Picked Weekly
        /// </summary>
        private void rbWeekly_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbWeekly.Checked == true)
                {
                    Query.isDaily = false;
                    Query.isWeekly = true;
                    Query.isMonthly = false;
                    fileLocation = "Weekly";
                }
            }
            catch
            {
                MessageBox.Show("Error: rbWeekly_CheckedChanged");
            }
        }

        /// <summary>
        /// Resolution -> User Picked Monthly
        /// </summary>
        private void rbMonthly_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbMonthly.Checked == true)
                {
                    Query.isDaily = false;
                    Query.isWeekly = false;
                    Query.isMonthly = true;
                    fileLocation = "Monthly";
                }
            }
            catch
            {
                MessageBox.Show("Error: rbMonthly_CheckedChanged");
            }
        }

        /// <summary>
        /// Load Method -> Download from Online
        /// </summary>
        private void rbOnline_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbOnline.Checked == true)
                {
                    loadMethod = "Online";
                    btnSubmit.Visible = true;
                    btnSubmit.Text = "Online";
                    rbDisplay.Checked = false;
                }
            }
            catch
            {
                MessageBox.Show("Error: rbOnline_CheckedChanged");
            }
        }

        /// <summary>
        /// Load Method -> Load from File
        /// </summary>
        private void rbDisplay_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbDisplay.Checked == true)
                {
                    loadMethod = "Offline";
                    btnSubmit.Visible = true;
                    btnSubmit.Text = "Display";
                    rbOnline.Checked = false;
                }
            }
            catch
            {
                MessageBox.Show("Error: rbDisplay_CheckedChanged");
            }
        }

        /// <summary>
        /// Clear input fields of the form
        /// </summary>
        private void btnClear_Click(object sender, EventArgs e)
        {
            //Clear the fields of the form
            txtCompanyTicker.Clear();
            rbDaily.Checked = false;
            rbWeekly.Checked = false;
            rbMonthly.Checked = false;

            rbOnline.Checked = false;
            rbDisplay.Checked = false;

            //Clear the query
            Query.startDay = "";
            Query.startMonth = "";
            Query.startYear = "";

            Query.endDay = "";
            Query.endMonth = "";
            Query.endYear = "";

            Query.isDaily = false;
            Query.isMonthly = false;
            Query.isWeekly = false;

            Query.companyName = "";

            //Hide submit button
            btnSubmit.Visible = false;

            // clear list view
            lsvStock.Items.Clear();

            // clear chart
            chCandleStick.Series["Candlestick"].Points.Clear();
        }

        /// <summary>
        /// Submit button to get query based on the user options provided in the form
        /// </summary>
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                // errors with dates are coming from here (i believe) dates acting buggy
                // check for end date is not less than start date
                int checkDay = int.Parse(Query.endDay) - int.Parse(Query.startDay);
                int checkMonth = int.Parse(Query.endMonth) - int.Parse(Query.startMonth);
                int checkYear = int.Parse(Query.endYear) - int.Parse(Query.startYear);

                if (/*(checkDay >= 0) && (checkMonth >= 0) &&*/ (checkYear >= 0))
                {
                    //If date is valid, then continue
                    string filePath = "";
                    Query.companyName = txtCompanyTicker.Text.Trim();

                    if (rbOnline.Checked == true)
                    {

                        //Assemble the search query - Online
                        QueryAssembly.buildStockQuery(Query);

                        //Read saved file
                        filePath = QueryFile.readFromFile(fileLocation, Query.companyName);

                        // load data to list view
                        loadDataToListView(filePath);

                        // do stuff with chart
                        loadDataToChart(filePath);
                    }
                    else if (rbDisplay.Checked == true)
                    {
                        // not sure what display does!
                    }
                    else
                    {
                        MessageBox.Show("Error: nothing was selected! -- should never reach here");
                    }
                }
                else
                {
                    MessageBox.Show("Error: Sorry the end date select is before the selected start date");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: btnSubmit_Click: " + ex.Message);
            }
        }
        /// <summary>
        /// Load data from csv file path to list view (performs reading from csv file here).
        /// </summary>
        public void loadDataToListView(string filePath)
        {
            // read from saved file
            string[] lines = File.ReadAllLines(filePath);
            string removeHeader = "Date,Open,High,Low,Close,Volume,Adj Close";
            string removeWhiteSpace = "";
            lines = lines.Where(val => val != removeHeader).ToArray();
            lines = lines.Where(val => val != removeWhiteSpace).ToArray();

            // add elements to list view items
            List<ListViewItem> items = new List<ListViewItem>();

            // read somewhere that this helps with speed
            lsvStock.BeginUpdate();

            // read each line in from filePath
            foreach (string line in lines)
            {
                string[] data = line.Split(',');
                // add each element as a list view item (subitem)
                ListViewItem element = new ListViewItem(data[0]);  //date
                element.SubItems.Add(data[1]); //open
                element.SubItems.Add(data[2]); //high
                element.SubItems.Add(data[3]); //low
                element.SubItems.Add(data[4]); //close
                element.SubItems.Add(data[5]); //volume
                element.SubItems.Add(data[6]); //adj close

                // add list view items to list view (lsvStock)
                lsvStock.Items.Add(element);
            }
            // stop updating list view
            lsvStock.EndUpdate();
        }
        /// <summary>
        /// Load data from csv file path to Candlestick Chart (performs reading from csv file here).
        /// </summary>
        public void loadDataToChart(string filePath)
        {
            // read from saved file
            string[] lines = File.ReadAllLines(filePath);
            string removeHeader = "Date,Open,High,Low,Close,Volume,Adj Close";
            string removeWhiteSpace = "";
            lines = lines.Where(val => val != removeHeader).ToArray();
            lines = lines.Where(val => val != removeWhiteSpace).ToArray();

            List<string> stockData = new List<string>();

            int cols = 0;

            // read each line in from filePath
            foreach (string line in lines)
            {
                string[] data = line.Split(',');
                stockData.Add(data[0]); //date
                stockData.Add(data[1]); //open
                stockData.Add(data[2]); //high
                stockData.Add(data[3]); //low
                stockData.Add(data[4]); //close
                stockData.Add(data[5]); //volume
                stockData.Add(data[6]); //adj close
                cols += 1;
            }

            // initialize series for Candlestick
            Series candle = new Series("Candlestick");
            chCandleStick.Series.Add(candle);
            chCandleStick.Name = "Candlestick Chart";

            // Set series chart type
            chCandleStick.Series["Candlestick"].ChartType = SeriesChartType.Candlestick;

            // Set the style of the open-close marks
            chCandleStick.Series["Candlestick"]["OpenCloseStyle"] = "Triangle";

            // Show both open and close marks
            chCandleStick.Series["Candlestick"]["ShowOpenClose"] = "Both";

            // Set point width
            chCandleStick.Series["Candlestick"]["PointWidth"] = "1.0";

            // Set colors bars
            chCandleStick.Series["Candlestick"]["PriceUpColor"] = "Green";
            chCandleStick.Series["Candlestick"]["PriceDownColor"] = "Red";

            int rows = 7;

            // do something with data (need to figure out data structure)
            /*
            for (int i = 0; i < cols; i++ )
                for (int j = 0; j < rows; j++)
                {
                    // adding date and high
                    chCandleStick.Series["price"].Points.AddXY(DateTime.Parse(stockData[i][j]), stockData[i][j]);
                    // adding low
                    chCandleStick.Series["price"].Points[i].YValues[1] = stockData[i][j];
                    //adding open
                    chCandleStick.Series["price"].Points[i].YValues[2] = stockData[i][j];
                    // adding close
                    chCandleStick.Series["price"].Points[i].YValues[3] = stockData[i][j];
                }
             */
        }

        /// <summary>
        /// Button to save chart based on time of button press
        /// </summary>
        private void btSaveImageOfChart_Click(object sender, EventArgs e)
        {
            // save png file to DEV folder
            // weird error with .net jit ??
            //string date = DateTime.Now.ToString("HH:mm:ss");
            chCandleStick.SaveImage("C:\\DEV\\chart.png", ChartImageFormat.Png);
        }
    }
}