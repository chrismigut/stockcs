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

        string searchStartDate;
        string searchEndDate;

        public StockifyForm()
        {
            InitializeComponent();
            this.dtpEndDate.MaxDate = DateTime.Today.AddDays(-1);
            this.dtpEndDate.Value = DateTime.Today.AddDays(-1);
            //init the dates
            Query.startDay = dtpStartDate.Value.Day.ToString();
            // yahoo's finance api requires months to start at zero instead of 1
            // example (look at &a=00, &d=11): http://finance.yahoo.com/q/hp?s=AAPL&a=00&b=24&c=1990&d=11&e=16&f=2014&g=d
            Query.startMonth = (dtpStartDate.Value.Month - 1).ToString();
            Query.startYear = dtpStartDate.Value.Year.ToString();

            Query.endDay = dtpEndDate.Value.Day.ToString();
            // yahoo's finance api requires months to start at zero instead of 1
            // example (look at &a=00, &d=11): http://finance.yahoo.com/q/hp?s=AAPL&a=00&b=24&c=1990&d=11&e=16&f=2014&g=d
            Query.endMonth = (dtpEndDate.Value.Month - 1).ToString();
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

            // completely clear candlestick chart
            chCandleStick.Series.Clear();
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
                    btnSubmit.Text = "Go";
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
            //Query.startDay = "";
            //Query.startMonth = "";
            //Query.startYear = "";

            //Query.endDay = "";
            //Query.endMonth = "";
            //Query.endYear = "";

            //init the dates
            Query.startDay = dtpStartDate.Value.Day.ToString();
            // yahoo's finance api requires months to start at zero instead of 1
            // example (look at &a=00, &d=11): http://finance.yahoo.com/q/hp?s=AAPL&a=00&b=24&c=1990&d=11&e=16&f=2014&g=d
            Query.startMonth = (dtpStartDate.Value.Month - 1).ToString();
            Query.startYear = dtpStartDate.Value.Year.ToString();

            Query.endDay = dtpEndDate.Value.Day.ToString();
            // yahoo's finance api requires months to start at zero instead of 1
            // example (look at &a=00, &d=11): http://finance.yahoo.com/q/hp?s=AAPL&a=00&b=24&c=1990&d=11&e=16&f=2014&g=d
            Query.endMonth = (dtpEndDate.Value.Month - 1).ToString();
            Query.endYear = dtpEndDate.Value.Year.ToString();

            Query.isDaily = false;
            Query.isMonthly = false;
            Query.isWeekly = false;

            Query.companyName = "";

            //Hide submit button
            btnSubmit.Visible = false;

            // clear list view
            lsvStock.Items.Clear();

            // completely clear candlestick chart
            chCandleStick.Series.Clear();
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

                        //Assemble the search query and download query - Online
                        QueryAssembly.buildStockQuery(Query);

                        //Read saved file
                        filePath = QueryFile.readFromFile(fileLocation, Query.companyName);

                        // load data to list view
                        searchStartDate = Query.startYear + "-" + Query.startMonth + "-" + Query.startDay;
                        searchEndDate = Query.endYear + "-" + Query.endMonth + "-" + Query.endDay;
                        loadDataToListView(filePath, searchStartDate, searchEndDate);
                    }
                    else if (rbDisplay.Checked == true)
                    {
                        //Read saved file
                        filePath = QueryFile.readFromFile(fileLocation, Query.companyName);

                        // load data to list view
                        searchStartDate = Query.startYear + "-" + Query.startMonth + "-" + Query.startDay;
                        searchEndDate = Query.endYear + "-" + Query.endMonth + "-" + Query.endDay;
                        loadDataToListView(filePath, searchStartDate, searchEndDate);

                        // do stuff with chart
                        loadDataToChart(filePath, searchStartDate, searchEndDate);
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
        public void loadDataToListView(string filePath, string searchStartDate, string searchEndDate)
        {
            // clear list view
            lsvStock.Items.Clear();

            // read from saved file
            string[] lines = File.ReadAllLines(filePath);
            string removeHeader = "Date,Open,High,Low,Close,Volume,Adj Close";
            string removeWhiteSpace = "";
            lines = lines.Where(val => val != removeHeader).ToArray();
            lines = lines.Where(val => val != removeWhiteSpace).ToArray();

            // read somewhere that this helps with speed
            // http://stackoverflow.com/questions/9008310/how-to-speed-adding-items-to-a-listview
            lsvStock.BeginUpdate();

            //Search Bounds timeframe
            char[] delimiterChars = { '-' };
            //Stat date
            string[] startDate = searchStartDate.Split(delimiterChars);
            int startYear = int.Parse(startDate[0]);
            int startMonth = int.Parse(startDate[1]);
            int startDay = int.Parse(startDate[2]);
            //End date
            string[] endDate = searchEndDate.Split(delimiterChars);
            int endYear = int.Parse(endDate[0]);
            int endMonth = int.Parse(endDate[1]);
            int endDay = int.Parse(endDate[2]);
            //Date from file
            int fileYear = 0;
            int fileMonth = 0;
            int fileDay = 0;
            //Get Between start-end date
            int betweenYearS = Math.Abs(endYear - startYear);
            int betweenMonthS = Math.Abs(endMonth - startMonth);
            int betweenDayS = Math.Abs(endDay - startDay);
            //Get Between end-start date
            int betweenYearE = Math.Abs(startYear - endYear);
            int betweenMonthE = Math.Abs(startMonth - endMonth);
            int betweenDayE = Math.Abs(startDay - endDay);
            //Valid file date
            int selectFileYearS = 0;
            int selectFileMonthS = 0;
            int selectFileDayS = 0;
            //Valid file date
            int selectFileYearE = 0;
            int selectFileMonthE = 0;
            int selectFileDayE = 0;

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

                //Get date from file
                string[] fileDate = data[0].Split(delimiterChars);
                fileYear = int.Parse(fileDate[0]);
                fileMonth = int.Parse(fileDate[1]);
                fileDay = int.Parse(fileDate[2]);

                //file date - start date = upper bounds on time frame
                selectFileYearS = Math.Abs(fileYear - startYear);
                selectFileMonthS = Math.Abs((fileMonth - 1) - startMonth);
                selectFileDayS = Math.Abs(fileDay - startDay);

                //file date - end date = lower bounds on time frame
                selectFileYearE = Math.Abs(fileYear - endYear);
                selectFileMonthE = Math.Abs((fileMonth - 1) - endMonth);
                selectFileDayE = Math.Abs(fileDay - endDay);

                //Check if record within given time frame
                //If file year is less than between year amount then is valid if not, check month next
                //If file month is less than between month amount is valid if not, check the day next
                //If file day is less than or equal then between day amount is vaild if not, not valid time frame
                //Check the file if the date is between the upper and lower date provided
                if ((selectFileYearS < betweenYearS))// && (fileYear > startYear)) /*|| (selectFileDayE < betweenYearE)*/
                {
                    // add list view items to list view (lsvStock)
                    lsvStock.Items.Add(element);
                }
                //Check the file date is the same as the upper bound date
                else if (selectFileYearS == betweenYearS)
                {
                    if (selectFileMonthS < betweenMonthS)
                    {
                        // add list view items to list view (lsvStock)
                        lsvStock.Items.Add(element);
                    }
                    else if (selectFileMonthS == betweenMonthS)
                    {
                        if (selectFileDayS <= betweenDayS)
                        {
                            // add list view items to list view (lsvStock)
                            lsvStock.Items.Add(element);
                        }
                    }
                }
                /*
                //Check if the file date is the same as the lower bound date
                else if (fileYear == startYear)
                {
                    if (fileMonth < startMonth)
                    {
                        // add list view items to list view (lsvStock)
                        lsvStock.Items.Add(element);
                    }
                    else if (fileMonth == startMonth)
                    {
                        if (fileDay <= startDay)
                        {
                            // add list view items to list view (lsvStock)
                            lsvStock.Items.Add(element);
                        }
                    }
                }
                */
            }
            // stop updating list view
            lsvStock.EndUpdate();

            // auto resize columns
            for (int i = 0; i < lsvStock.Columns.Count - 1; i++)
            {
                lsvStock.AutoResizeColumn(i, ColumnHeaderAutoResizeStyle.ColumnContent);
            }
        }

        /// <summary>
        /// Load data from csv file path to Candlestick Chart (performs reading from csv file here).
        /// </summary>
        public void loadDataToChart(string filePath, string searchStartDate, string searchEndDate)
        {
            // completely clear candlestick chart
            chCandleStick.Series.Clear();

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

            // initialize parameters
            //Search Bounds timeframe
            char[] delimiterChars = { '-' };
            //Stat date
            string[] startDate = searchStartDate.Split(delimiterChars);
            int startYear = int.Parse(startDate[0]);
            int startMonth = int.Parse(startDate[1]);
            int startDay = int.Parse(startDate[2]);
            //End date
            string[] endDate = searchEndDate.Split(delimiterChars);
            int endYear = int.Parse(endDate[0]);
            int endMonth = int.Parse(endDate[1]);
            int endDay = int.Parse(endDate[2]);
            //Date from file
            int fileYear = 0;
            int fileMonth = 0;
            int fileDay = 0;
            //Get Between start-end date
            int betweenYearS = Math.Abs(endYear - startYear);
            int betweenMonthS = Math.Abs(endMonth - startMonth);
            int betweenDayS = Math.Abs(endDay - startDay);
            //Get Between end-start date
            int betweenYearE = Math.Abs(startYear - endYear);
            int betweenMonthE = Math.Abs(startMonth - endMonth);
            int betweenDayE = Math.Abs(startDay - endDay);
            //Valid file date
            int selectFileYearS = 0;
            int selectFileMonthS = 0;
            int selectFileDayS = 0;
            //Valid file date
            int selectFileYearE = 0;
            int selectFileMonthE = 0;
            int selectFileDayE = 0;

            // read from saved file
            string[] lines = File.ReadAllLines(filePath);
            string removeHeader = "Date,Open,High,Low,Close,Volume,Adj Close";
            string removeWhiteSpace = "";
            lines = lines.Where(val => val != removeHeader).ToArray();
            lines = lines.Where(val => val != removeWhiteSpace).ToArray();

            // increment by i
            int i = 0;

            // read each line in from filePath
            foreach (string line in lines)
            {
                // load elements from each line in csv file into string array
                string[] data = line.Split(',');
                
                // set up date and time
                DateTime date = DateTime.Parse(data[0]);
                float open = float.Parse(data[1]);
                float high = float.Parse(data[2]);
                float low = float.Parse(data[3]);
                float close = float.Parse(data[4]);
                Double volume = Convert.ToDouble(data[5]);
                float adjVol = float.Parse(data[6]);

                //Get date from file
                string[] fileDate = data[0].Split(delimiterChars);
                fileYear = int.Parse(fileDate[0]);
                fileMonth = int.Parse(fileDate[1]);
                fileDay = int.Parse(fileDate[2]);

                //file date - start date = upper bounds on time frame
                selectFileYearS = Math.Abs(fileYear - startYear);
                selectFileMonthS = Math.Abs((fileMonth - 1) - startMonth);
                selectFileDayS = Math.Abs(fileDay - startDay);

                //file date - end date = lower bounds on time frame
                selectFileYearE = Math.Abs(fileYear - endYear);
                selectFileMonthE = Math.Abs((fileMonth - 1) - endMonth);
                selectFileDayE = Math.Abs(fileDay - endDay);

                //Check if record within given time frame
                //If file year is less than between year amount then is valid if not, check month next
                //If file month is less than between month amount is valid if not, check the day next
                //If file day is less than or equal then between day amount is vaild if not, not valid time frame
                if ((selectFileYearS < betweenYearS))
                {
                    // adding date and high
                    chCandleStick.Series["Candlestick"].Points.AddXY(date, high);
                    // adding low
                    chCandleStick.Series["Candlestick"].Points[i].YValues[1] = low;
                    //adding open
                    chCandleStick.Series["Candlestick"].Points[i].YValues[2] = open;
                    // adding close
                    chCandleStick.Series["Candlestick"].Points[i].YValues[3] = close;
                    i++;
                }
                else if (selectFileYearS == betweenYearS)
                {
                    if ((selectFileMonthS < betweenMonthS))
                    {
                        // adding date and high
                        chCandleStick.Series["Candlestick"].Points.AddXY(date, high);
                        // adding low
                        chCandleStick.Series["Candlestick"].Points[i].YValues[1] = low;
                        //adding open
                        chCandleStick.Series["Candlestick"].Points[i].YValues[2] = open;
                        // adding close
                        chCandleStick.Series["Candlestick"].Points[i].YValues[3] = close;
                        // increment
                        i++;
                    }
                    else if (selectFileMonthS == betweenMonthS)
                    {
                        if ((selectFileDayS <= betweenDayS))
                        {
                            // adding date and high
                            chCandleStick.Series["Candlestick"].Points.AddXY(date, high);
                            // adding low
                            chCandleStick.Series["Candlestick"].Points[i].YValues[1] = low;
                            //adding open
                            chCandleStick.Series["Candlestick"].Points[i].YValues[2] = open;
                            // adding close
                            chCandleStick.Series["Candlestick"].Points[i].YValues[3] = close;
                            // increment
                            i++;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Button to save the current state of chart in the form of a PNG file.
        /// </summary>
        private void btSaveImageOfChart_Click(object sender, EventArgs e)
        {
            // save png file to DEV folder
            // weird error with .net jit ??
            // string date = DateTime.Now.ToString("HH:mm:ss");
            chCandleStick.SaveImage("C:\\DEV\\chart.png", ChartImageFormat.Png);
        }
    }
}