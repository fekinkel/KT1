using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WebForms;
using System.IO;
using System.Diagnostics;
using PrimeiroAppSharp.Formularios;
using System.Net.Http;
using System.Net.Http.Headers;

namespace PrimeiroAppSharp
{
    public partial class Form1 : Form
    {
        BLL.DS.Megasena taMegaSenaAdapter;
        public Form1(string valor)
        {
            taMegaSenaAdapter = new BLL.DS.Megasena();
            InitializeComponent();
            Text = valor;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnImprimirRelatorio_Click(object sender, EventArgs e)
        {
            //ReportViewer reportViewer = new ReportViewer();

            //reportViewer.ProcessingMode = ProcessingMode.Local;

            //reportViewer.LocalReport.ReportEmbeddedResource = "WindowsFormsApp1.rptMegaSena.rdlc";

            //List<ReportParameter> listParameter = new List<ReportParameter>();
            //listParameter.Add(new ReportParameter("NumeroSorteio", txtNumeroSorteio.Text));
            //reportViewer.LocalReport.SetParameters(listParameter);

            var bt = taMegaSenaAdapter.ObterMegaCab(2720, 2722);

            Relatorio.frmMegaSena form = new Relatorio.frmMegaSena();
          
            form.ShowDialog(10);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: esta linha de código carrega dados na tabela 'cFGDataSet1.MEGA'. Você pode movê-la ou removê-la conforme necessário.
            //this.mEGATableAdapter.Fill(this.cFGDataSet1.MEGA);

            var bt = taMegaSenaAdapter.ObterMegaCab(2720, 2723);

            dataGridView1.DataSource = bt;

            cbxBox.Items.Add(new KeyValuePair<string, string>("T", "Teswte"));
            cbxBox.Items.Add(new KeyValuePair<string, string>("N", "Novoe"));
            cbxBox.ValueMember = "Key";
            cbxBox.DisplayMember = "Value";

            //this.mEGATableAdapter.Fill(this.cFGDataSet1.MEGA);
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "B")
            { 
                dataGridView1.Columns[e.ColumnIndex].Selected = true;
            }
        }

        private void mEGABindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ExportToPdf();
        }
        private void ExportToPdf()
        {
            string deviceInfo = "";
            string[] streamIds;
            Warning[] warnings;
            string mineTypes = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;
            viewer.LocalReport.ReportPath = "rptBoletoPP.rdlc";
            viewer.LocalReport.DataSources.Add(new ReportDataSource("dsBoletoPP", GetBoletoPPData()));

            viewer.LocalReport.Refresh(); 

            var bytes = viewer.LocalReport.Render("PDF", deviceInfo, out mineTypes, out encoding, 
                out extension, out streamIds, out warnings);


            string filename = @"C:\Temp\BoletoPPdata.pdf";

            File.WriteAllBytes(filename, bytes);

            System.Diagnostics.Process.Start(filename);
        }
        private List<Formularios.DadosBoletoPP> GetBoletoPPData() 
        {
            return new List<DadosBoletoPP>
            {
                new DadosBoletoPP{Nome="Fernando",Sobrenome="Kinkel Serejo", Endereço="Jacinto Pagliato", Valor=1254.32},
            };
        }

        private async Task btnReceita_ClickAsync(object sender, EventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://www.receitaws.com.br/v1/cnpj/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response =  await client.GetAsync("43299791000377");
                var jsonResponse = await response.Content.ReadAsStringAsync();
                richTextBox1.Clear();
                richTextBox1.Text = jsonResponse.ToString();
            }
        }

        private void btnReceita_Click(object sender, EventArgs e)
        {
            btnReceita_ClickAsync(sender,e);
        }
    }
}
