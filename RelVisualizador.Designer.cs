namespace PMV.Tributario.Imobiliario.Relatorio
{
    partial class RelVisualizador
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.rptVisualizador = new Microsoft.Reporting.WinForms.ReportViewer();
            this.DSNotificacaoLancamentoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.DSNotificacaoLancamentoBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // rptVisualizador
            // 
            this.rptVisualizador.AutoScroll = true;
            this.rptVisualizador.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "dtsRequerimento";
            reportDataSource1.Value = null;
            this.rptVisualizador.LocalReport.DataSources.Add(reportDataSource1);
            this.rptVisualizador.Location = new System.Drawing.Point(0, 0);
            this.rptVisualizador.Margin = new System.Windows.Forms.Padding(1);
            this.rptVisualizador.Name = "rptVisualizador";
            this.rptVisualizador.ServerReport.BearerToken = null;
            this.rptVisualizador.Size = new System.Drawing.Size(800, 695);
            this.rptVisualizador.TabIndex = 1;
            this.rptVisualizador.Load += new System.EventHandler(this.rptVisualizador_Load);
            // 
            // DSNotificacaoLancamentoBindingSource
            // 
            this.DSNotificacaoLancamentoBindingSource.DataMember = "MO_LANCAMENTO_IMPRESSAO";
            this.DSNotificacaoLancamentoBindingSource.DataSource = typeof(PMV.Tributario.DL.Fiscalizacao.DS.DSNotificacaoLancamento);
            // 
            // RelVisualizador
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 695);
            this.Controls.Add(this.rptVisualizador);
            this.MaximizeBox = false;
            this.Name = "RelVisualizador";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RelVisualizador";
            this.Load += new System.EventHandler(this.RelVisualizador_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DSNotificacaoLancamentoBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer rptVisualizador;
        private System.Windows.Forms.BindingSource DSNotificacaoLancamentoBindingSource;
    }
}