using System;
using System.Net.Http;
using System.Windows.Forms;

namespace CoronaVirusStatistics
{
    public partial class MainDisplay : Form
    {
        public static bool graphOpen = false;
        private string country = "";

        public MainDisplay()
        {
            InitializeComponent();
        }

        private readonly HttpClient client = new HttpClient();

        private async void Country_SelectedIndexChanged(object sender, EventArgs e)
        {
            country = Country.SelectedItem.ToString();

            try
            {
                string response = await client.GetStringAsync($"https://coronavirus-19-api.herokuapp.com/countries/{country}");
                totalInfections.Text = $"Confirmed Cases: {GetValue(response, "cases")}";
                actualInfetions.Text = $"Confirmed Active Cases: {GetValue(response, "active")}";
                todayCases.Text = $"Today Cases: {GetValue(response, "todayCases")}";
                casesPM.Text = $"Cases Per Million: {GetValue(response, "casesPerOneMillion")}";
                critical.Text = $"Critical Cases: {GetValue(response, "critical")}";
                deaths.Text = $"Deaths: {GetValue(response, "deaths")}";
                todayDeaths.Text = $"Today Deaths: {GetValue(response, "todayDeaths")}";
                recovered.Text = $"Recovered: {GetValue(response, "recovered")}";
                tests.Text = $"Tests Made: {GetValue(response, "totalTests")}";
                deathsPM.Text = $"Deaths Per Million: {GetValue(response, "deathsPerOneMillion")}";
                testsPM.Text = $"Tests Per Million: {GetValue(response, "testsPerOneMillion")}";
            }
            catch (Exception)
            {
                MessageBox.Show("There was an error trying to retrieve data from the server.\nPlease retry later.", "Error");
            }
        }

        private void Country_TextUpdate(object sender, EventArgs e)
        {
            Country.Text = "Countries";
        }

        public string GetValue(string stringOfValue, string valueName)
        {
            stringOfValue = stringOfValue.Replace("\"", "").Replace("{", "").Replace("}", "").Replace($"country:{country},", "") + ",";
            stringOfValue = stringOfValue.Remove(0, stringOfValue.IndexOf(valueName));
            stringOfValue = stringOfValue.Remove(stringOfValue.IndexOf(","));
            stringOfValue = stringOfValue.Remove(0, stringOfValue.IndexOf(':') + 1);
            return stringOfValue;
        }

        private void InfoBttn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("If a country has values equal to zero it can be that the value is zero or they are not being reported.\nSource: worldometers.info\nSupport for 213 countries.\n\n\nCreated by Germán Gasset Martí\t\t" +
                "          Copyright ©  2020", "About");
        }

        private void graphBttn_Click(object sender, EventArgs e)
        {
            if (!graphOpen)
            {
                graphOpen = true;
                GraphsForm graphsForm = new GraphsForm();
                graphsForm.Show();
            }
        }
    }
}