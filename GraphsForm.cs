using System;
using System.Drawing;
using System.Windows.Forms;
using System.Net.Http;

namespace CoronaVirusStatistics
{
    public partial class GraphsForm : Form
    {
        public GraphsForm()
        {
            InitializeComponent();
        }

        private readonly HttpClient client = new HttpClient();

        private void GraphsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            MainDisplay.graphOpen = false;
        }

        private async void graphBttn_Click(object sender, EventArgs e)
        {
            MainDisplay main = new MainDisplay();
            Bitmap graph = new Bitmap(graphImage.Width, graphImage.Height);
            Graphics graphics = Graphics.FromImage(graph);
            int[] countryValues = new int[] { 0, 0, 0, 0 };
            if (comboBox1.Text != "")
                countryValues[0] = Convert.ToInt32(main.GetValue(await client.GetStringAsync($"https://coronavirus-19-api.herokuapp.com/countries/{comboBox1.Text}"), "active"));
            if (comboBox2.Text != "")
                countryValues[1] = Convert.ToInt32(main.GetValue(await client.GetStringAsync($"https://coronavirus-19-api.herokuapp.com/countries/{comboBox2.Text}"), "active"));
            if (comboBox3.Text != "")
                countryValues[2] = Convert.ToInt32(main.GetValue(await client.GetStringAsync($"https://coronavirus-19-api.herokuapp.com/countries/{comboBox3.Text}"), "active"));
            if (comboBox4.Text != "")
                countryValues[3] = Convert.ToInt32(main.GetValue(await client.GetStringAsync($"https://coronavirus-19-api.herokuapp.com/countries/{comboBox4.Text}"), "active"));

            int maxWidth = 375, maxValue = Math.Max(Math.Max(countryValues[0], countryValues[1]), Math.Max(countryValues[2], countryValues[3]));
            if (comboBox1.Text != "")
            {
                graphics.FillRectangle(Brushes.Red, 0, 20, ValueFromPercentage(Percentage(countryValues[0], maxValue), maxWidth), 50);
                label1.Text = $"Active cases for {comboBox1.Text}: {countryValues[0]}";
            }
            if (comboBox2.Text != "")
            {
                graphics.FillRectangle(Brushes.Red, 0, 100, ValueFromPercentage(Percentage(countryValues[1], maxValue), maxWidth), 50);
                label2.Text = $"Active cases for {comboBox2.Text}: {countryValues[1]}";
            }
            if (comboBox3.Text != "")
            {
                graphics.FillRectangle(Brushes.Red, 0, 180, ValueFromPercentage(Percentage(countryValues[2], maxValue), maxWidth), 50);
                label3.Text = $"Active cases for {comboBox3.Text}: {countryValues[2]}";
            }
            if (comboBox4.Text != "")
            {
                graphics.FillRectangle(Brushes.Red, 0, 260, ValueFromPercentage(Percentage(countryValues[3], maxValue), maxWidth), 50);
                label4.Text = $"Active cases for {comboBox4.Text}: {countryValues[3]}";
            }
            graphImage.Image = graph;
        }

        private int Percentage(int value, int oneHundredPercentValue) => value * 100 / oneHundredPercentValue;

        private int ValueFromPercentage(int percentage, int OneHundredPercentValue) => OneHundredPercentValue / 100 * percentage;
    }
}