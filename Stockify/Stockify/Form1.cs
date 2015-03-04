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
namespace Stockify
{
    public partial class Form1 : Form
    {
        SearchQuery Query = new SearchQuery();
        Queries Yahoo_Call = new Queries();
        CustomFileHander QueryFile = new CustomFileHander();
        QueryAssembly QueryAssembly = new QueryAssembly();
        ParseFile inputFile = new ParseFile();

        string fileLocation = "";
        string loadMethod = "";

        public Form1()
        {
            InitializeComponent();
            DateTime dtEndDate = DateTime.Today.AddDays(-1);
            dtpEndDate.Value = dtEndDate;
            //init the dates
            Query.startDay = dtpStartDate.Value.Day.ToString();
            Query.startMonth = dtpStartDate.Value.Day.ToString();
            Query.startYear = dtpStartDate.Value.Day.ToString();

            Query.endDay = dtpEndDate.Value.Day.ToString();
            Query.endMonth = dtpEndDate.Value.Month.ToString();
            Query.endYear = dtpEndDate.Value.Year.ToString();
        }

        /// <summary>
        /// Date Picker -> Get Starting Date
        /// </summary>
        private void dtpStartDate_ValueChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Date Picker -> Get Ending Date
        /// </summary>
        private void dtpEndDate_ValueChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Resolution -> User Picked Daily
        /// </summary>
        private void rbDaily_CheckedChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Resolution -> User Picked Weekly
        /// </summary>
        private void rbWeekly_CheckedChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Resolution -> User Picked Monthly
        /// </summary>
        private void rbMonthly_CheckedChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Load Method -> Download from Online
        /// </summary>
        private void rbOnline_CheckedChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Load Method -> Load from File
        /// </summary>
        private void rbDisplay_CheckedChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Clear input fields of the form
        /// </summary>
        private void btnClear_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Submit button to get query based on the user options provided in the form
        /// </summary>
        private void btnSubmit_Click(object sender, EventArgs e)
        {

        }
    }
}
