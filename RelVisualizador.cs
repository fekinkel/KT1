using Microsoft.Reporting.WinForms;
using PMV.Tributario.BL.Shared.Tabela;
using PMV.Tributario.BL.Shared.Utilitario;
using PMV.Tributario.DL.Fiscalizacao.DS;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PMV.Tributario.Imobiliario.Relatorio
{
    public partial class RelVisualizador : Form
    {
        private DSNotificacaoLancamento.MO_LANCAMENTO_DADOS_IMPRESSAODataTable _dtDados = new DSNotificacaoLancamento.MO_LANCAMENTO_DADOS_IMPRESSAODataTable();
        private DSNotificacaoLancamento.MO_NOTIFICACAO_LANCAMENTO_HISTORICODataTable _dtNotificacaoHistorico = new DSNotificacaoLancamento.MO_NOTIFICACAO_LANCAMENTO_HISTORICODataTable();
        private DSNotificacaoLancamento.MO_NOTIFICACAO_LANCAMENTO_HISTORICODataTable _dtNotificacaoHistoricoMerge = new DSNotificacaoLancamento.MO_NOTIFICACAO_LANCAMENTO_HISTORICODataTable();
        private DSNotificacaoLancamento.MO_NOTIFICACAO_LANCAMENTO_PARCELAMENTO_ATUALIZADODataTable _dtNotificacaoParcelamentoAtualizado = new DSNotificacaoLancamento.MO_NOTIFICACAO_LANCAMENTO_PARCELAMENTO_ATUALIZADODataTable();
        private DSNotificacaoLancamento.MO_NOTIFICACAO_LANCAMENTO_TERMOREMESSADataTable _dtTermoRemessa = new DSNotificacaoLancamento.MO_NOTIFICACAO_LANCAMENTO_TERMOREMESSADataTable();
        private DSNotificacaoLancamento.MO_NOTIFICACAO_PARAMETRODataTable _dtParametro = new DSNotificacaoLancamento.MO_NOTIFICACAO_PARAMETRODataTable();
        private BL.Fiscalizacao.Notificacao.NotificacaoLancamento blNotificacao = new BL.Fiscalizacao.Notificacao.NotificacaoLancamento();
        private DSNotificacaoLancamento.MO_LANCAMENTO_IMPRESSAODataTable _dtNotificacao = new DSNotificacaoLancamento.MO_LANCAMENTO_IMPRESSAODataTable();
        private DSNotificacaoLancamento.MO_NOTIFICACAO_LANCAMENTO_LANCAMENTODataTable _dtLancamento = new DSNotificacaoLancamento.MO_NOTIFICACAO_LANCAMENTO_LANCAMENTODataTable();
        private DSNotificacaoLancamento.MO_LANCAMENTO_LANCAMENTO_IMPRESSAODataTable _dtLancamentoImpressao = new DSNotificacaoLancamento.MO_LANCAMENTO_LANCAMENTO_IMPRESSAODataTable();
        private DSNotificacaoLancamento.MO_LANCAMENTO_DADOS_IMPRESSAODataTable _dtDadosImpressao = new DSNotificacaoLancamento.MO_LANCAMENTO_DADOS_IMPRESSAODataTable();
        private DSNotificacaoLancamento.MO_NOTIFICACAO_LANCAMENTO_CONTRIBUINTEDataTable _dtNotificacaoContribuinte = new DSNotificacaoLancamento.MO_NOTIFICACAO_LANCAMENTO_CONTRIBUINTEDataTable();
        private DSNotificacaoLancamento.MO_NOTIFICACAO_LANCAMENTO_UNIFICACAODataTable _dtNotificacaoUnificacao = new DSNotificacaoLancamento.MO_NOTIFICACAO_LANCAMENTO_UNIFICACAODataTable();
        private DSNotificacaoLancamento.MO_NOTIFICACAO_LANCAMENTO_PARCELAMENTO_COMPLEMENTODataTable _dtParcelamentoComplemento = new DSNotificacaoLancamento.MO_NOTIFICACAO_LANCAMENTO_PARCELAMENTO_COMPLEMENTODataTable();
        private DSNotificacaoLancamento.MO_NOTIFICACAO_LANCAMENTO_PARCELAMENTODataTable _dtParcelamento = new DSNotificacaoLancamento.MO_NOTIFICACAO_LANCAMENTO_PARCELAMENTODataTable();
        private DSNotificacaoLancamento.MO_NOTIFICACAO_LANCAMENTO_PARCELAMENTO_PARCELADataTable _dtParcela = new DSNotificacaoLancamento.MO_NOTIFICACAO_LANCAMENTO_PARCELAMENTO_PARCELADataTable();
        private DSNotificacaoLancamento.PARCELAS_IMPRESSAODataTable _dtParcelaImpressao = new DSNotificacaoLancamento.PARCELAS_IMPRESSAODataTable();
        public RelVisualizador()
        {
            InitializeComponent();
        }

        public void ShowDialog(int idNotificacao, string relatorio, string codParcelamento = "")
        {
            //C  = NOTIFICAÇÃO
            //D  = DEMONSTRATIVO
            //DP = DEMONSTRATIVO DE PARCELAMENTO
            //G  = GUIA DO CARNE
            //TC = TERMO DE CONFISSAO
            //TR = TERMO REMESSA D.A
            //ID = ISS Declarado
            if (relatorio == "C" || relatorio == "D")
            {
                _dtParametro = blNotificacao.ObterParametro();
                _dtNotificacao = blNotificacao.BuscarNotificacaoIdImpressao(idNotificacao);
                _dtNotificacaoContribuinte = blNotificacao.ListarContribuinteIdNotificacao(idNotificacao);
                _dtNotificacaoUnificacao = blNotificacao.ObterUnificacao(idNotificacao);
                _dtLancamento = blNotificacao.ListarLancamentoNotificacaoIdNotificacao(idNotificacao);


                foreach (DataRow drQualificacao in _dtNotificacaoContribuinte.Rows)
                {
                    drQualificacao["LOGRADOURO"] = (drQualificacao["LOGRADOURO"].ToString() + ", " + drQualificacao["NUMERO"].ToString() + ", " + drQualificacao["BAIRRO"].ToString() + ", " + drQualificacao["COMPLEMENTO"].ToString() + ", " + drQualificacao["CIDADE"].ToString() + "/" + drQualificacao["UF"].ToString() + ", " + drQualificacao["CEP"].ToString()).ToUpper();
                    drQualificacao["QUALIFICACAO_DESCRICAO"] = drQualificacao["QUALIFICACAO_DESCRICAO"].ToString().Substring(0, drQualificacao["QUALIFICACAO_DESCRICAO"].ToString().Length - 2);
                }

                string inscricaoUnificada = string.Empty;
                decimal totalTerreno = 0;
                decimal totalTestado = 0;

                DataRow dtrUnificacao = _dtNotificacaoUnificacao.NewRow();

                dtrUnificacao["ID_NOTIFICACAO_UNIFICACAO"] = 1;
                dtrUnificacao["INSCRICAO"] = _dtNotificacao.Rows[0]["INSCRICAO"].ToString();
                dtrUnificacao["AREA_TERRENO_TESTADO"] = _dtNotificacao.Rows[0]["AREA_TESTADO"].ToString();
                dtrUnificacao["AREA_TERRENO"] = _dtNotificacao.Rows[0]["AREA_TERRENO"].ToString();
                dtrUnificacao["QUADRA"] = _dtNotificacao.Rows[0]["QUADRA"].ToString();
                dtrUnificacao["LOTE"] = _dtNotificacao.Rows[0]["LOTE"].ToString();

                _dtNotificacaoUnificacao.Rows.InsertAt(dtrUnificacao, 0);

                foreach (DataRow drUnificacao in _dtNotificacaoUnificacao.Rows)
                {
                    totalTerreno += Convert.ToDecimal(drUnificacao["AREA_TERRENO"]);
                    totalTestado += Convert.ToDecimal(drUnificacao["AREA_TERRENO_TESTADO"]);
                    inscricaoUnificada += drUnificacao["INSCRICAO"].ToString() + ", ";
                }
                                
                int sequencialDados = 0;
                decimal issConstrucao = 0;
                decimal issDemolicao = 0;
                decimal issReforma = 0;
                decimal issEngenharia = 0;
                decimal licencaConstrucao = 0;
                decimal licencaReformaDemo = 0;
                decimal alinhamento = 0;
                decimal alvara = 0;
                decimal penalidade = 0;
                decimal valorTotal = 0;
                bool construcaoCobrado = false;
                bool implantacaoCobrado = false;
                bool desmenbramentoCobrado = false;
                bool unificacaoCobrado = false;
                //alinhamento é cobrado uma unica vez
                //alvara cobrado uma vez por tipo de lancamento

                int i = 0;
                foreach (DataRow drLancamento in _dtLancamento.Rows)
                {
                    int sequencial = 0;
                    i++;
                    #region Taxas

                    alvara = 0;

                    if (alinhamento == 0)
                    {
                        alinhamento = Convert.ToDecimal(drLancamento["TX_ALINHAMENTO"]);
                    }
                    else
                    {
                        alinhamento = 0;
                    }

                    if (drLancamento["TIPO"].ToString() == "CONSTRUÇÃO/REFORMA/DEMOLIÇÃO" && construcaoCobrado == false)
                    {
                        alvara += Convert.ToDecimal(drLancamento["TX_ALVARA"]);

                        construcaoCobrado = true;
                    }
                    else if (drLancamento["TIPO"].ToString() == "IMPLANTAÇÃO" && implantacaoCobrado == false)
                    {
                        alvara += Convert.ToDecimal(drLancamento["TX_ALVARA"]);

                        implantacaoCobrado = true;
                    }
                    else if (drLancamento["TIPO"].ToString() == "DESMEMBRAMENTO" && desmenbramentoCobrado == false)
                    {
                        alvara += Convert.ToDecimal(drLancamento["TX_ALVARA"]);

                        desmenbramentoCobrado = true;
                    }
                    else if (drLancamento["TIPO"].ToString() == "UNIFICAÇÃO" && unificacaoCobrado == false)
                    {
                        alvara += Convert.ToDecimal(drLancamento["TX_ALVARA"]);

                        unificacaoCobrado = true;
                    }

                    issConstrucao = Convert.ToDecimal(drLancamento["IMPOSTO_ISS_CONSTRUCAO"]);
                    issDemolicao = Convert.ToDecimal(drLancamento["IMPOSTO_ISS_DEMOLICAO"]);
                    issReforma = Convert.ToDecimal(drLancamento["IMPOSTO_ISS_REFORMA"]);
                    issEngenharia = Convert.ToDecimal(drLancamento["IMPOSTO_ISS_ENGENHARIA"]);
                    licencaConstrucao = Convert.ToDecimal(drLancamento["TX_LICENCA_CONSTRUCAO"]);
                    licencaReformaDemo = Convert.ToDecimal(drLancamento["TX_LICENCA_REFORMA_DEMOLICAO"]);
                    penalidade = Convert.ToDecimal(drLancamento["TX_PENALIDADES"]);

                    decimal areaBaseIssConstrucao = Convert.ToDecimal(drLancamento["AREA_BASE_ISS_CONSTRUCAO"].ToString());
                    decimal areaBaseIssDemolicao = Convert.ToDecimal(drLancamento["AREA_BASE_ISS_DEMOLICAO"].ToString());
                    decimal areaBaseIssReforma = Convert.ToDecimal(drLancamento["AREA_BASE_ISS_REFORMA"].ToString());
                    decimal areaBaseIssEngenharia = Convert.ToDecimal(drLancamento["BASE_ISS_ENGENHARIA"].ToString());
                    decimal areaBaseIssPenalidade = Convert.ToDecimal(drLancamento["AREA_PENALIDADE_CONSTRUCAO"].ToString());

                    //if (areaBaseIssConstrucao > 0)
                    //{
                    //    DataRow dtrDados = _dtDadosImpressao.NewRow();

                    //    dtrDados["SEQUENCIAL"] = sequencialDados;
                    //    dtrDados["TIPO"] = "5.1 " + drLancamento["SUBTIPO_IMOVEL"].ToString();
                    //    dtrDados["DESCRICAO"] = "ÁREA BASE ISS CONSTRUÇÃO";
                    //    dtrDados["VALOR"] = areaBaseIssConstrucao;

                    //    _dtDadosImpressao.Rows.Add(dtrDados);
                    //}
                    //if (areaBaseIssDemolicao > 0)
                    //{
                    //    sequencialDados += 1;

                    //    DataRow dtrDados = _dtDadosImpressao.NewRow();

                    //    dtrDados["SEQUENCIAL"] = sequencialDados;
                    //    dtrDados["TIPO"] = "5.1 " + drLancamento["SUBTIPO_IMOVEL"].ToString();
                    //    dtrDados["DESCRICAO"] = "ÁREA BASE ISS DEMOLIÇÃO";
                    //    dtrDados["VALOR"] = areaBaseIssDemolicao;

                    //    _dtDadosImpressao.Rows.Add(dtrDados);
                    //}
                    //if (areaBaseIssReforma > 0)
                    //{
                    //    sequencialDados += 1;

                    //    DataRow dtrDados = _dtDadosImpressao.NewRow();

                    //    dtrDados["SEQUENCIAL"] = sequencialDados;
                    //    dtrDados["TIPO"] = "5.1 " + drLancamento["SUBTIPO_IMOVEL"].ToString();
                    //    dtrDados["DESCRICAO"] = "ÁREA BASE ISS REFORMA";
                    //    dtrDados["VALOR"] = areaBaseIssReforma;

                    //    _dtDadosImpressao.Rows.Add(dtrDados);
                    //}
                    //if (areaBaseIssEngenharia > 0)
                    //{
                    //    sequencialDados += 1;

                    //    DataRow dtrDados = _dtDadosImpressao.NewRow();

                    //    dtrDados["SEQUENCIAL"] = sequencialDados;
                    //    dtrDados["TIPO"] = "5.1 " + drLancamento["SUBTIPO_IMOVEL"].ToString();
                    //    dtrDados["DESCRICAO"] = "ÁREA BASE ISS ENGENHARIA";
                    //    dtrDados["VALOR"] = areaBaseIssEngenharia;

                    //    _dtDadosImpressao.Rows.Add(dtrDados);
                    //}
                    //if (areaBaseIssPenalidade > 0)
                    //{
                    //    sequencialDados += 1;

                    //    DataRow dtrDados = _dtDadosImpressao.NewRow();

                    //    dtrDados["SEQUENCIAL"] = sequencialDados;
                    //    dtrDados["TIPO"] = "5.1 " + drLancamento["SUBTIPO_IMOVEL"].ToString();
                    //    dtrDados["DESCRICAO"] = "PENALIDADE";
                    //    dtrDados["VALOR"] = areaBaseIssPenalidade;

                    //    _dtDadosImpressao.Rows.Add(dtrDados);
                    //}

                    if (issConstrucao > 0)
                    {
                        DataRow dtrImpressao = _dtLancamentoImpressao.NewRow();

                        sequencial += 1;

                        dtrImpressao["TIPO"] = (5 + i) + " - " + drLancamento["SUBTIPO_IMOVEL"].ToString();
                        dtrImpressao["SEQUENCIAL"] = (5 + i) + "." + sequencial;
                        dtrImpressao["TITULO"] = "ISS Construção";
                        dtrImpressao["AREA"] = areaBaseIssConstrucao;
                        dtrImpressao["BASE"] = Convert.ToDecimal(drLancamento["BASE_ISS_CONSTRUCAO"]).ToString("N2");
                        dtrImpressao["ALIQUOTA"] = (Convert.ToDecimal(drLancamento["CONSTRUCAO_ALIQUOTA"]) * 100).ToString("N2");
                        dtrImpressao["FUNDAMENTO"] = drLancamento["ISS_CONSTRUCAO_UFM_DESCRICAO"];
                        dtrImpressao["VALOR"] = issConstrucao.ToString("N2");

                        _dtLancamentoImpressao.Rows.Add(dtrImpressao);
                    }
                    if (issDemolicao > 0)
                    {
                        DataRow dtrImpressao = _dtLancamentoImpressao.NewRow();

                        sequencial += 1;

                        dtrImpressao["TIPO"] = (5 + i) + " - " + drLancamento["SUBTIPO_IMOVEL"].ToString();
                        dtrImpressao["SEQUENCIAL"] = (5 + i) + "." + sequencial;
                        dtrImpressao["TITULO"] = "ISS Demolição";
                        dtrImpressao["AREA"] = areaBaseIssDemolicao;
                        dtrImpressao["BASE"] = Convert.ToDecimal(drLancamento["BASE_ISS_DEMOLICAO"]).ToString("N2");
                        dtrImpressao["ALIQUOTA"] = (Convert.ToDecimal(drLancamento["CONSTRUCAO_ALIQUOTA"]) * 100).ToString("N2");
                        dtrImpressao["FUNDAMENTO"] = drLancamento["ISS_DEMOLICAO_UFM_DESCRICAO"];
                        dtrImpressao["VALOR"] = issDemolicao.ToString("N2");

                        _dtLancamentoImpressao.Rows.Add(dtrImpressao);
                    }
                    if (issReforma > 0)
                    {
                        DataRow dtrImpressao = _dtLancamentoImpressao.NewRow();

                        sequencial += 1;

                        dtrImpressao["TIPO"] = (5 + i) + " - " + drLancamento["SUBTIPO_IMOVEL"].ToString();
                        dtrImpressao["SEQUENCIAL"] = (5 + i) + "." + sequencial;
                        dtrImpressao["TITULO"] = "ISS Reforma";
                        dtrImpressao["AREA"] = areaBaseIssReforma;
                        dtrImpressao["BASE"] = Convert.ToDecimal(drLancamento["BASE_ISS_REFORMA"]).ToString("N2");
                        dtrImpressao["ALIQUOTA"] = (Convert.ToDecimal(drLancamento["CONSTRUCAO_ALIQUOTA"]) * 100).ToString("N2");
                        dtrImpressao["FUNDAMENTO"] = drLancamento["ISS_REFORMA_UFM_DESCRICAO"];
                        dtrImpressao["VALOR"] = issReforma.ToString("N2");

                        _dtLancamentoImpressao.Rows.Add(dtrImpressao);
                    }
                    if (issEngenharia > 0)
                    {
                        DataRow dtrImpressao = _dtLancamentoImpressao.NewRow();

                        sequencial += 1;

                        dtrImpressao["TIPO"] = (5 + i) + " - " + drLancamento["SUBTIPO_IMOVEL"].ToString();
                        dtrImpressao["SEQUENCIAL"] = (5 + i) + "." + sequencial;
                        dtrImpressao["TITULO"] = "ISS Engenharia";
                        dtrImpressao["AREA"] = areaBaseIssEngenharia;
                        dtrImpressao["BASE"] = Convert.ToDecimal(drLancamento["BASE_ISS_ENGENHARIA"]).ToString("N2");
                        dtrImpressao["ALIQUOTA"] = (Convert.ToDecimal(drLancamento["ENGENHARIA_ALIQUOTA"]) * 100).ToString("N2");
                        dtrImpressao["FUNDAMENTO"] = drLancamento["ISS_ENGENHARIA_UFM_DESCRICAO"];
                        dtrImpressao["VALOR"] = issEngenharia.ToString("N2");

                        _dtLancamentoImpressao.Rows.Add(dtrImpressao);
                    }
                    if (licencaConstrucao > 0)
                    {
                        DataRow dtrImpressao = _dtLancamentoImpressao.NewRow();

                        sequencial += 1;

                        dtrImpressao["TIPO"] = (5 + i) + " - " + drLancamento["SUBTIPO_IMOVEL"].ToString();
                        dtrImpressao["SEQUENCIAL"] = (5 + i) + "." + sequencial;
                        dtrImpressao["TITULO"] = "Taxa de Licença/Fiscalização Construção";
                        dtrImpressao["AREA"] = "";
                        dtrImpressao["BASE"] = "0,00";
                        dtrImpressao["ALIQUOTA"] = "0,00";
                        dtrImpressao["FUNDAMENTO"] = drLancamento["TX_CONSTRUCAO_UFM_DESCRICAO"];
                        dtrImpressao["VALOR"] = licencaConstrucao.ToString("N2");

                        _dtLancamentoImpressao.Rows.Add(dtrImpressao);
                    }
                    if (licencaReformaDemo > 0)
                    {
                        DataRow dtrImpressao = _dtLancamentoImpressao.NewRow();

                        sequencial += 1;

                        dtrImpressao["TIPO"] = (5 + i) + " - " + drLancamento["SUBTIPO_IMOVEL"].ToString();
                        dtrImpressao["SEQUENCIAL"] = (5 + i) + "." + sequencial;
                        dtrImpressao["TITULO"] = "Taxa de Licença/Fiscalização Reforma/Demolição";
                        dtrImpressao["AREA"] = "";
                        dtrImpressao["BASE"] = "0,00";
                        dtrImpressao["ALIQUOTA"] = "0,00";
                        dtrImpressao["FUNDAMENTO"] = drLancamento["TX_LICENCA_UFM_DESCRICAO"];
                        dtrImpressao["VALOR"] = licencaReformaDemo.ToString("N2");

                        _dtLancamentoImpressao.Rows.Add(dtrImpressao);
                    }
                    if (alinhamento > 0)
                    {
                        DataRow dtrImpressao = _dtLancamentoImpressao.NewRow();

                        sequencial += 1;

                        dtrImpressao["TIPO"] = (5 + i) + " - " + drLancamento["SUBTIPO_IMOVEL"].ToString();
                        dtrImpressao["SEQUENCIAL"] = (5 + i) + "." + sequencial;
                        dtrImpressao["TITULO"] = "Alinhamento - Preço Público";
                        dtrImpressao["AREA"] = "";
                        dtrImpressao["BASE"] = "0,00";
                        dtrImpressao["ALIQUOTA"] = "0,00";
                        dtrImpressao["FUNDAMENTO"] = drLancamento["TX_ALINHAMENTO_UFM_DESCRICAO"];
                        dtrImpressao["VALOR"] = alinhamento.ToString("N2");

                        _dtLancamentoImpressao.Rows.Add(dtrImpressao);
                    }
                    if (alvara > 0)
                    {
                        DataRow dtrImpressao = _dtLancamentoImpressao.NewRow();

                        sequencial += 1;
                        dtrImpressao["TIPO"] = (5 + i) + " - " + drLancamento["SUBTIPO_IMOVEL"].ToString();
                        dtrImpressao["SEQUENCIAL"] = (5 + i) + "." + sequencial;
                        dtrImpressao["TITULO"] = "Alvará Preço - Público";
                        dtrImpressao["AREA"] = "";
                        dtrImpressao["BASE"] = "0,00";
                        dtrImpressao["ALIQUOTA"] = "0,00";
                        dtrImpressao["FUNDAMENTO"] = drLancamento["TX_ALVARA_UFM_DESCRICAO"];
                        dtrImpressao["VALOR"] = alvara.ToString("N2");

                        _dtLancamentoImpressao.Rows.Add(dtrImpressao);
                    }
                    if (penalidade > 0)
                    {
                        DataRow dtrImpressao = _dtLancamentoImpressao.NewRow();

                        sequencial += 1;

                        dtrImpressao["TIPO"] = (5 + i) + " - " + drLancamento["SUBTIPO_IMOVEL"].ToString();
                        dtrImpressao["SEQUENCIAL"] = (5 + i) + "." + sequencial;
                        dtrImpressao["TITULO"] = "Penalidade";
                        dtrImpressao["AREA"] = areaBaseIssPenalidade;
                        dtrImpressao["BASE"] = "0,00";
                        dtrImpressao["ALIQUOTA"] = "0,00";
                        dtrImpressao["FUNDAMENTO"] = drLancamento["TX_PENALIDADES_UFM_DESCRICAO"];
                        dtrImpressao["VALOR"] = penalidade.ToString("N2");

                        _dtLancamentoImpressao.Rows.Add(dtrImpressao);
                    }

                    #endregion

                    #region Dados do Lancamento

                    decimal areaConstruir = Convert.ToDecimal(drLancamento["AREA_CONSTRUIR"].ToString());
                    decimal areaLegalConstr = Convert.ToDecimal(drLancamento["AREA_LEGAL_CONSTRUCAO"].ToString());
                    decimal areaExistenteLegalConst = Convert.ToDecimal(drLancamento["AREA_LEGAL_EXISTENTE_CONSTRUCAO"].ToString());
                    decimal areaDemolir = Convert.ToDecimal(drLancamento["AREA_DEMOLIR"].ToString());
                    decimal areaLegalDemolir = Convert.ToDecimal(drLancamento["AREA_DEMOLIR_LEGAL"].ToString());
                    decimal areaReformar = Convert.ToDecimal(drLancamento["AREA_REFORMAR"].ToString());
                    decimal areaLegalReformar = Convert.ToDecimal(drLancamento["AREA_LEGAL_REFORMA"].ToString());
                    decimal areaDecadConstr = Convert.ToDecimal(drLancamento["AREA_DECADENCIA_CONSTRUCAO"].ToString());
                    decimal areaDecadDemolir = Convert.ToDecimal(drLancamento["AREA_DECADENCIA_DEMOLICAO"].ToString());
                    decimal numeroUnidade = Convert.ToDecimal(drLancamento["NUMERO_UNIDADES"].ToString());
                    decimal numeroUnidadeLegal = Convert.ToDecimal(drLancamento["NUMERO_ABRIGO_DESMONTAVEL"].ToString());
                    decimal areaEdicula = Convert.ToDecimal(drLancamento["AREA_EDICULA"].ToString());
                    decimal abrigoLegal = Convert.ToDecimal(drLancamento["ABRIGO_DESMONTAVEL_LEGALIZAR"].ToString());
                    decimal abrigoLegalizado = Convert.ToDecimal(drLancamento["ABRIGO_DESMONTAVEL_EXISTENTE"].ToString());
                    decimal areaTerrenoDeduzir = Convert.ToDecimal(drLancamento["AREA_DEDUZIR"].ToString());
                    decimal areaTerrenoAumentar = Convert.ToDecimal(drLancamento["AREA_AUMENTAR"].ToString());
                    decimal areaTestadaDeduzir = Convert.ToDecimal(drLancamento["AREA_TESTADA_DEDUZIR"].ToString());
                    decimal areaTestadaAumentar = Convert.ToDecimal(drLancamento["AREA_TESTADA_AUMENTAR"].ToString());                   
                    bool elevador = Convert.ToBoolean(drLancamento["ELEVADOR"].ToString());
                    decimal enquadramentoDemolicao = Convert.ToDecimal(drLancamento["AREA_ENQUADRAMENTO_DEMOLICAO"].ToString());
                    decimal enquadramentoConstrucao = Convert.ToDecimal(drLancamento["AREA_ENQUADRAMENTO"].ToString());
                    decimal enquadramentoReforma = Convert.ToDecimal(drLancamento["AREA_ENQUADRAMENTO_REFORMA"].ToString());
                    decimal enquadramentoPenalidadedReforma = Convert.ToDecimal(drLancamento["AREA_ENQUADRAMENTO_PENALIDADE"].ToString());

                    if (areaConstruir > 0)
                    {
                        DataRow dtrDados = _dtDadosImpressao.NewRow();

                        dtrDados["SEQUENCIAL"] = "5." + i + ".1 DADOS INFORMADOS";
                        dtrDados["TIPO"] = "5." + i + " TIPO DE CONSTRUÇÃO / PROJETO: " + drLancamento["SUBTIPO_IMOVEL"].ToString();
                        dtrDados["DESCRICAO"] = "ÁREA A CONSTRUIR";
                        dtrDados["VALOR"] = areaConstruir;

                        _dtDadosImpressao.Rows.Add(dtrDados);
                    }
                    if (areaLegalConstr > 0)
                    {
                        DataRow dtrDados = _dtDadosImpressao.NewRow();

                        dtrDados["SEQUENCIAL"] = "5." + i + ".1 DADOS INFORMADOS"; ;
                        dtrDados["TIPO"] = "5." + i + " TIPO DE CONSTRUÇÃO / PROJETO: " + drLancamento["SUBTIPO_IMOVEL"].ToString();
                        dtrDados["DESCRICAO"] = "ÁREA A LEGALIZAR/REGULARIZAR DE CONSTRUÇÃO";
                        dtrDados["VALOR"] = areaLegalConstr;

                        _dtDadosImpressao.Rows.Add(dtrDados);
                    }
                    if (areaExistenteLegalConst > 0)
                    {
                        DataRow dtrDados = _dtDadosImpressao.NewRow();

                        dtrDados["SEQUENCIAL"] = "5." + i + ".1 DADOS INFORMADOS"; ;
                        dtrDados["TIPO"] = "5." + i + " TIPO DE CONSTRUÇÃO / PROJETO: " + drLancamento["SUBTIPO_IMOVEL"].ToString();
                        dtrDados["DESCRICAO"] = "ÁREA EXISTENTE LEGALIZADA DE CONSTRUÇÃO";
                        dtrDados["VALOR"] = areaExistenteLegalConst;

                        _dtDadosImpressao.Rows.Add(dtrDados);
                    }
                    if (areaDemolir > 0)
                    {
                        DataRow dtrDados = _dtDadosImpressao.NewRow();

                        dtrDados["SEQUENCIAL"] = "5." + i + ".1 DADOS INFORMADOS"; ;
                        dtrDados["TIPO"] = "5." + i + " TIPO DE CONSTRUÇÃO / PROJETO: " + drLancamento["SUBTIPO_IMOVEL"].ToString();
                        dtrDados["DESCRICAO"] = "ÁREA A DEMOLIR";
                        dtrDados["VALOR"] = areaDemolir;

                        _dtDadosImpressao.Rows.Add(dtrDados);
                    }
                    if (areaLegalDemolir > 0)
                    {
                        DataRow dtrDados = _dtDadosImpressao.NewRow();

                        dtrDados["SEQUENCIAL"] = "5." + i + ".1 DADOS INFORMADOS"; ;
                        dtrDados["TIPO"] = "5." + i + " TIPO DE CONSTRUÇÃO / PROJETO: " + drLancamento["SUBTIPO_IMOVEL"].ToString();
                        dtrDados["DESCRICAO"] = "ÁREA A LEGALIZAR/REGULARIZAR DE DEMOLIÇÃO";
                        dtrDados["VALOR"] = areaLegalDemolir;

                        _dtDadosImpressao.Rows.Add(dtrDados);
                    }
                    if (areaReformar > 0)
                    {
                        DataRow dtrDados = _dtDadosImpressao.NewRow();

                        dtrDados["SEQUENCIAL"] = "5." + i + ".1 DADOS INFORMADOS"; ;
                        dtrDados["TIPO"] = "5." + i + " TIPO DE CONSTRUÇÃO / PROJETO: " + drLancamento["SUBTIPO_IMOVEL"].ToString();
                        dtrDados["DESCRICAO"] = "ÁREA A REFORMAR";
                        dtrDados["VALOR"] = areaReformar;

                        _dtDadosImpressao.Rows.Add(dtrDados);
                    }
                    if (areaLegalReformar > 0)
                    {
                        DataRow dtrDados = _dtDadosImpressao.NewRow();

                        dtrDados["SEQUENCIAL"] = "5." + i + ".1 DADOS INFORMADOS"; ;
                        dtrDados["TIPO"] = "5." + i + " TIPO DE CONSTRUÇÃO / PROJETO: " + drLancamento["SUBTIPO_IMOVEL"].ToString();
                        dtrDados["DESCRICAO"] = "ÁREA A LEGALIZAR/REGULARIZAR DE REFORMA";
                        dtrDados["VALOR"] = areaLegalReformar;

                        _dtDadosImpressao.Rows.Add(dtrDados);
                    }
                    if (areaDecadConstr > 0)
                    {                        
                        DataRow dtrDados = _dtDadosImpressao.NewRow();

                        dtrDados["SEQUENCIAL"] = "5." + i + ".1 DADOS INFORMADOS"; ;
                        dtrDados["TIPO"] = "5." + i + " TIPO DE CONSTRUÇÃO / PROJETO: " + drLancamento["SUBTIPO_IMOVEL"].ToString();
                        dtrDados["DESCRICAO"] = "ÁREA DECADÊNCIA DE CONSTRUÇÃO";
                        dtrDados["VALOR"] = areaDecadConstr;

                        _dtDadosImpressao.Rows.Add(dtrDados);
                    }
                    if (areaDecadDemolir > 0)
                    {
                        DataRow dtrDados = _dtDadosImpressao.NewRow();

                        dtrDados["SEQUENCIAL"] = "5." + i + ".1 DADOS INFORMADOS"; ;
                        dtrDados["TIPO"] = "5." + i + " TIPO DE CONSTRUÇÃO / PROJETO: " + drLancamento["SUBTIPO_IMOVEL"].ToString();
                        dtrDados["DESCRICAO"] = "ÁREA DECADÊNCIA DE DEMOLIÇÃO";
                        dtrDados["VALOR"] = areaDecadDemolir;

                        _dtDadosImpressao.Rows.Add(dtrDados);
                    }
                    if (numeroUnidade > 0)
                    {
                        DataRow dtrDados = _dtDadosImpressao.NewRow();

                        dtrDados["SEQUENCIAL"] = "5." + i + ".1 DADOS INFORMADOS"; ;
                        dtrDados["TIPO"] = "5." + i + " TIPO DE CONSTRUÇÃO / PROJETO: " + drLancamento["SUBTIPO_IMOVEL"].ToString();
                        dtrDados["DESCRICAO"] = "NÚMEROS DE UNIDADES";
                        dtrDados["VALOR"] = numeroUnidade;

                        _dtDadosImpressao.Rows.Add(dtrDados);
                    }
                    if (numeroUnidadeLegal > 0)
                    {
                        DataRow dtrDados = _dtDadosImpressao.NewRow();

                        dtrDados["SEQUENCIAL"] = "5." + i + ".1 DADOS INFORMADOS"; ;
                        dtrDados["TIPO"] = "5." + i + " TIPO DE CONSTRUÇÃO / PROJETO: " + drLancamento["SUBTIPO_IMOVEL"].ToString();
                        dtrDados["DESCRICAO"] = "NÚMEROS DE UNIDADES AUTÔNOMAS A LEGALIZAR";
                        dtrDados["VALOR"] = numeroUnidadeLegal;

                        _dtDadosImpressao.Rows.Add(dtrDados);
                    }
                    if (areaEdicula > 0)
                    {
                        DataRow dtrDados = _dtDadosImpressao.NewRow();

                        dtrDados["SEQUENCIAL"] = "5." + i + ".1 DADOS INFORMADOS"; ;
                        dtrDados["TIPO"] = "5." + i + " TIPO DE CONSTRUÇÃO / PROJETO: " + drLancamento["SUBTIPO_IMOVEL"].ToString();
                        dtrDados["DESCRICAO"] = "ÁREA DE EDÍCULA";
                        dtrDados["VALOR"] = areaEdicula;

                        _dtDadosImpressao.Rows.Add(dtrDados);
                    }
                    if (abrigoLegal > 0)
                    {
                        DataRow dtrDados = _dtDadosImpressao.NewRow();

                        dtrDados["SEQUENCIAL"] = "5." + i + ".1 DADOS INFORMADOS"; ;
                        dtrDados["TIPO"] = "5." + i + " TIPO DE CONSTRUÇÃO / PROJETO: " + drLancamento["SUBTIPO_IMOVEL"].ToString();
                        dtrDados["DESCRICAO"] = "ABRIGO DESMONTÁVEL A LEGALIZAR/REGULARIZAR";
                        dtrDados["VALOR"] = abrigoLegal;

                        _dtDadosImpressao.Rows.Add(dtrDados);
                    }
                    if (abrigoLegalizado > 0)
                    {
                        DataRow dtrDados = _dtDadosImpressao.NewRow();

                        dtrDados["SEQUENCIAL"] = "5." + i + ".1 DADOS INFORMADOS"; ;
                        dtrDados["TIPO"] = "5." + i + " TIPO DE CONSTRUÇÃO / PROJETO: " + drLancamento["SUBTIPO_IMOVEL"].ToString();
                        dtrDados["DESCRICAO"] = "ABRIGO DESMONTÁVEL EXISTENTE LEGALIZADO";
                        dtrDados["VALOR"] = abrigoLegalizado;

                        _dtDadosImpressao.Rows.Add(dtrDados);
                    }
                    if (areaTerrenoDeduzir > 0)
                    {
                        DataRow dtrDados = _dtDadosImpressao.NewRow();

                        dtrDados["SEQUENCIAL"] = "5." + i + ".1 DADOS INFORMADOS"; ;
                        dtrDados["TIPO"] = "5." + i + " TIPO DE CONSTRUÇÃO / PROJETO: " + drLancamento["SUBTIPO_IMOVEL"].ToString();
                        dtrDados["DESCRICAO"] = "ÁREA DE TERRENO A DEDUZIR - IMPLANTAÇÃO";
                        dtrDados["VALOR"] = areaTerrenoDeduzir;

                        _dtDadosImpressao.Rows.Add(dtrDados);
                    }
                    if (areaTerrenoAumentar > 0)
                    {
                        DataRow dtrDados = _dtDadosImpressao.NewRow();

                        dtrDados["SEQUENCIAL"] = "5." + i + ".1 DADOS INFORMADOS"; ;
                        dtrDados["TIPO"] = "5." + i + " TIPO DE CONSTRUÇÃO / PROJETO: " + drLancamento["SUBTIPO_IMOVEL"].ToString();
                        dtrDados["DESCRICAO"] = "ÁREA DE TERRENO A AUMENTAR";
                        dtrDados["VALOR"] = areaTerrenoAumentar;

                        _dtDadosImpressao.Rows.Add(dtrDados);
                    }
                    if (areaTestadaDeduzir > 0)
                    {
                        DataRow dtrDados = _dtDadosImpressao.NewRow();

                        dtrDados["SEQUENCIAL"] = "5." + i + ".1 DADOS INFORMADOS"; ;
                        dtrDados["TIPO"] = "5." + i + " TIPO DE CONSTRUÇÃO / PROJETO: " + drLancamento["SUBTIPO_IMOVEL"].ToString();
                        dtrDados["DESCRICAO"] = "TESTADA A DEDUZIR - DESMENBRAMENTO";
                        dtrDados["VALOR"] = areaTestadaDeduzir;

                        _dtDadosImpressao.Rows.Add(dtrDados);
                    }
                    if (areaTestadaAumentar > 0)
                    {
                        DataRow dtrDados = _dtDadosImpressao.NewRow();

                        dtrDados["SEQUENCIAL"] = "5." + i + ".1 DADOS INFORMADOS"; ;
                        dtrDados["TIPO"] = "5." + i + " TIPO DE CONSTRUÇÃO / PROJETO: " + drLancamento["SUBTIPO_IMOVEL"].ToString();
                        dtrDados["DESCRICAO"] = "TESTADA A AUMENTAR";
                        dtrDados["VALOR"] = areaTestadaAumentar;

                        _dtDadosImpressao.Rows.Add(dtrDados);
                    }
                    if (enquadramentoConstrucao > 0)
                    {
                        DataRow dtrDados = _dtDadosImpressao.NewRow();

                        dtrDados["SEQUENCIAL"] = "5." + i + ".2 ÁREAS UTILIZADAS PARA ENQUADRAMENTO NO PREÇO DO M² ESTIMADO EM LEI";
                        dtrDados["TIPO"] = "5." + i + " TIPO DE CONSTRUÇÃO / PROJETO: " + drLancamento["SUBTIPO_IMOVEL"].ToString();
                        dtrDados["DESCRICAO"] = "CONSTRUÇÃO (m²)";
                        dtrDados["VALOR"] = enquadramentoConstrucao;

                        _dtDadosImpressao.Rows.Add(dtrDados);
                    }
                    if (enquadramentoDemolicao > 0)
                    {
                        DataRow dtrDados = _dtDadosImpressao.NewRow();

                        dtrDados["SEQUENCIAL"] = "5." + i + ".2 ÁREAS UTILIZADAS PARA ENQUADRAMENTO NO PREÇO DO M² ESTIMADO EM LEI";
                        dtrDados["TIPO"] = "5." + i + " TIPO DE CONSTRUÇÃO / PROJETO: " + drLancamento["SUBTIPO_IMOVEL"].ToString();
                        dtrDados["DESCRICAO"] = "DEMOLIÇÃO (m²)";
                        dtrDados["VALOR"] = enquadramentoDemolicao;

                        _dtDadosImpressao.Rows.Add(dtrDados);
                    }
                    if (enquadramentoReforma > 0)
                    {
                        DataRow dtrDados = _dtDadosImpressao.NewRow();

                        dtrDados["SEQUENCIAL"] = "5." + i + ".2 ÁREAS UTILIZADAS PARA ENQUADRAMENTO NO PREÇO DO M² ESTIMADO EM LEI";
                        dtrDados["TIPO"] = "5." + i + " TIPO DE CONSTRUÇÃO / PROJETO: " + drLancamento["SUBTIPO_IMOVEL"].ToString();
                        dtrDados["DESCRICAO"] = "REFORMA (m²)";
                        dtrDados["VALOR"] = enquadramentoReforma;

                        _dtDadosImpressao.Rows.Add(dtrDados);
                    }
                    if (enquadramentoPenalidadedReforma > 0)
                    {
                        DataRow dtrDados = _dtDadosImpressao.NewRow();

                        dtrDados["SEQUENCIAL"] = "5." + i + ".2 ÁREAS UTILIZADAS PARA ENQUADRAMENTO NO PREÇO DO M² ESTIMADO EM LEI";
                        dtrDados["TIPO"] = "5." + i + " TIPO DE CONSTRUÇÃO / PROJETO: " + drLancamento["SUBTIPO_IMOVEL"].ToString();
                        dtrDados["DESCRICAO"] = "PENALIDADE (m²)";
                        dtrDados["VALOR"] = enquadramentoPenalidadedReforma;

                        _dtDadosImpressao.Rows.Add(dtrDados);
                    }
                    #endregion

                    valorTotal+= issConstrucao + issDemolicao + issReforma + issEngenharia + licencaConstrucao + licencaReformaDemo + alinhamento + alvara + penalidade;
                }

                _dtNotificacao.Rows[0]["NUMERO_NOTIFICACAO"] = _dtNotificacao.Rows[0]["NUMERO_NOTIFICACAO"].ToString().PadLeft(5, '0');
                _dtNotificacao.Rows[0]["OBS_SUJEICAO"] = _dtParametro.Select("CHAVE = 'OBS_SUJEICAO'")[0]["VALOR"].ToString();
                _dtNotificacao.Rows[0]["INSCRICAO"] = (inscricaoUnificada).Substring(0, (inscricaoUnificada).Length - 2);
                _dtNotificacao.Rows[0]["LOCAL_OBRA"] = _dtNotificacao.Rows[0]["LOGRADOURO"].ToString() + ", " + _dtNotificacao.Rows[0]["NUMERO"].ToString() + ", QUADRA " + _dtNotificacao.Rows[0]["QUADRA"].ToString() + ", LOTE " + _dtNotificacao.Rows[0]["LOTE"].ToString() + ", " + _dtNotificacao.Rows[0]["BAIRRO"].ToString() + ", " + _dtNotificacao.Rows[0]["CIDADE"].ToString() + "/" + _dtNotificacao.Rows[0]["UF"].ToString();
                _dtNotificacao.Rows[0]["AREA_TERRENO"] = totalTerreno;
                _dtNotificacao.Rows[0]["AREA_TESTADO"] = totalTestado;
                _dtNotificacao.Rows[0]["VALOR_EXTENSO"] = "R$ " + valorTotal.ToString("N2") + " (" + EscreverExtenso(valorTotal) + ")";
                _dtNotificacao.Rows[0]["MATRICULA"] = UsuarioLogado.drUsuarioLogado.MATRICULA.ToString();
                _dtNotificacao.Rows[0]["NOME"] = UsuarioLogado.drUsuarioLogado.NOME;

                string marcaAgua = "vazio";

                if (_dtNotificacao.Rows[0]["STATUS"].ToString() == "C")
                {
                    marcaAgua = "cancelada";
                }
                else if (_dtNotificacao.Rows[0]["STATUS"].ToString() == "F" || _dtNotificacao.Rows[0]["STATUS"].ToString() == "A")
                {
                    if (Convert.ToBoolean(_dtNotificacao.Rows[0]["SUSPENSO"].ToString()))
                    {
                        marcaAgua = "suspensa";
                    }

                    if (Convert.ToBoolean(_dtNotificacao.Rows[0]["RETIFICADA"].ToString()))
                    {
                        marcaAgua = "retificada";
                    }
                }

                DataView dtvLancamento = _dtLancamentoImpressao.DefaultView;

                dtvLancamento.Sort = "SEQUENCIAL ASC";

                if (relatorio == "C")
                {
                    rptVisualizador.LocalReport.ReportEmbeddedResource = "PMV.Tributario.Fiscalizacao.Relatorio.rptNotificacao.rdlc";

                    ReportParameter p = new ReportParameter("image", marcaAgua);
                    this.rptVisualizador.LocalReport.SetParameters(new ReportParameter[] { p });

                    rptVisualizador.LocalReport.DataSources.Add(new ReportDataSource("dtsCabecalho", (DataTable)_dtNotificacao));
                    rptVisualizador.LocalReport.DataSources.Add(new ReportDataSource("dtsContribuinte", (DataTable)_dtNotificacaoContribuinte));
                    rptVisualizador.LocalReport.DataSources.Add(new ReportDataSource("dtsUnificacao", (DataTable)_dtNotificacaoUnificacao));
                    rptVisualizador.LocalReport.DataSources.Add(new ReportDataSource("dtsLancamento", (DataTable)dtvLancamento.Table));
                    rptVisualizador.LocalReport.DataSources.Add(new ReportDataSource("dtsDados", (DataTable)_dtDadosImpressao));
                }
                else if (relatorio == "D")
                {
                    _dtParcelamento = blNotificacao.ObterParcelamentoAtivo(idNotificacao);

                    rptVisualizador.LocalReport.ReportEmbeddedResource = "PMV.Tributario.Fiscalizacao.Relatorio.rptDemonstrativo.rdlc";
                    rptVisualizador.LocalReport.DataSources.Add(new ReportDataSource("dtsCabecalho", (DataTable)_dtNotificacao));
                    rptVisualizador.LocalReport.DataSources.Add(new ReportDataSource("dtsContribuinte", (DataTable)_dtNotificacaoContribuinte));
                    rptVisualizador.LocalReport.DataSources.Add(new ReportDataSource("dtsLancamento", (DataTable)dtvLancamento.Table));
                    rptVisualizador.LocalReport.DataSources.Add(new ReportDataSource("dtsParcelamento", (DataTable)_dtParcelamento));
                }
            }
            else if (relatorio == "DP")
            {
                _dtNotificacao = blNotificacao.BuscarNotificacaoIdImpressao(idNotificacao);
                
               DSNotificacaoLancamento.MO_NOTIFICACAO_LANCAMENTO_PARCELAMENTORow dtr = null;
                DSNotificacaoLancamento.MO_NOTIFICACAO_LANCAMENTO_PARCELAMENTODataTable _dtParcelamentoProcessado = new DSNotificacaoLancamento.MO_NOTIFICACAO_LANCAMENTO_PARCELAMENTODataTable();

                if (codParcelamento == string.Empty)
                {
                    _dtParcelamento = blNotificacao.ObterParcelamentoAtivo(idNotificacao);

                    dtr = _dtParcelamento.Where(x => x.ATIVO == true).FirstOrDefault();

                    if (dtr == null)
                    {
                        _dtParcelamento = blNotificacao.ObterParcelamento(idNotificacao);

                        dtr = _dtParcelamento.OrderByDescending(x => x.ID_NOTIFICACAO_PARCELAMENTO).First();
                    }

                    _dtParcelamentoProcessado.ImportRow(dtr);
                }
                else
                {
                    _dtParcelamento = blNotificacao.Obter(Convert.ToInt32(codParcelamento));

                    dtr = _dtParcelamento.FirstOrDefault();

                    _dtParcelamentoProcessado.ImportRow(dtr);
                }
                
                _dtParcelamentoComplemento = blNotificacao.ObterParcelamentoComplemento(dtr.ID_NOTIFICACAO_PARCELAMENTO);

                UFV objUFV = new UFV();

                DL.Shared.DS.DSShared.SH_UFVRow drUFV = objUFV.BuscarData(Convert.ToDateTime(dtr.DATA_VENCIMENTO));

                decimal valorTabela = drUFV.VALOR;

                _dtParcelamentoComplemento.ToList().ForEach((x) =>
                {
                    x.VALOR_ABATIMENTO = Math.Round(x.VALOR_ABATIMENTO * valorTabela, 2);
                   // x.TRIBUTO = x.
                });

                _dtParcela = blNotificacao.ObterParcela(dtr.ID_NOTIFICACAO_PARCELAMENTO);

                string textoCancelado = string.Empty;

                if (dtr == null)
                {
                    dtr = _dtParcelamento.OrderByDescending(x => x.ID_NOTIFICACAO_PARCELAMENTO).First();
                }

                if (dtr.ATIVO == false)
                {
                    textoCancelado = "CANCELADO" + Environment.NewLine + "MOTIVO: " + dtr.CANCELAMENTO_MOTIVO.ToUpper() + Environment.NewLine + "PROCESSO: " + dtr.CANCELAMENTO_PROCESSO.ToUpper() + " / " + dtr.CANCELAMENTO_PROCESSO_ANO + Environment.NewLine + "DATA: " + dtr.CANCELAMENTO_DATA.ToShortDateString();
                }
                else if (dtr.SUSPENSO)
                {
                    textoCancelado = "SUSPENDIDO" + Environment.NewLine + "MOTIVO: " + dtr.SUSPENSO_MOTIVO.ToUpper() + Environment.NewLine + "PROCESSO: " + dtr.SUSPENSO_PROCESSO.ToUpper() + Environment.NewLine + "DATA: " + dtr.SUSPENSO_DATA.ToShortDateString();
                }

                rptVisualizador.LocalReport.ReportEmbeddedResource = "PMV.Tributario.Fiscalizacao.Relatorio.rptParcelamento.rdlc";

                ReportParameter p = new ReportParameter("textoCancelado", textoCancelado);
                this.rptVisualizador.LocalReport.SetParameters(new ReportParameter[] { p });

                rptVisualizador.LocalReport.DataSources.Add(new ReportDataSource("dtsCabecalho", (DataTable)_dtNotificacao));
                rptVisualizador.LocalReport.DataSources.Add(new ReportDataSource("dtsParcelamento", (DataTable)_dtParcelamentoProcessado));
                rptVisualizador.LocalReport.DataSources.Add(new ReportDataSource("dtsComplemento", (DataTable)_dtParcelamentoComplemento));
                rptVisualizador.LocalReport.DataSources.Add(new ReportDataSource("dtsParcelas", (DataTable)_dtParcela));
            }
            else if (relatorio == "G")
            {
                DataTable dtbValorAgrupado = new DataTable();

                _dtNotificacao = blNotificacao.BuscarNotificacaoIdImpressao(idNotificacao);
                _dtNotificacaoContribuinte = blNotificacao.ListarContribuinteIdNotificacao(idNotificacao);
                _dtParcelamento = blNotificacao.ObterParcelamentoAtivo(idNotificacao);

                _dtParcelamentoComplemento = blNotificacao.ObterParcelamentoComplemento(Convert.ToInt32(_dtParcelamento.Rows[0]["ID_NOTIFICACAO_PARCELAMENTO"]));

                _dtParcelaImpressao = blNotificacao.ObterParcelaImpresao(Convert.ToInt32(_dtParcelamento.Rows[0]["ID_NOTIFICACAO_PARCELAMENTO"]));

                dtbValorAgrupado = blNotificacao.ObterTotalAgrupado(Convert.ToInt32(_dtParcelamento.Rows[0]["ID_NOTIFICACAO_PARCELAMENTO"]));

                CodigoBarras blCodBar = new CodigoBarras();

                foreach (DSNotificacaoLancamento.PARCELAS_IMPRESSAORow item in _dtParcelaImpressao)
                {
                    string codigoBarras = blCodBar.CodBarrasArrecadacaoV4(item.TOTAL, item.DATA_VENCIMENTO, item.AVISO.PadLeft(10, '0') + item.PARCELA.ToString().PadLeft(2, '0'), (item.PARCELA == 99));

                    var representacaoNumerica = blCodBar.RepresentacaoArrecadacaoV4(codigoBarras);

                    for (int j = 0; j < representacaoNumerica.Count; j++)
                    {
                        item.LINHA_DIGITAVEL += representacaoNumerica[j] + "  ";
                    }

                    item.CODIGO_BARRAS = REGGerarCodigoBarra(codigoBarras);
                }

                rptVisualizador.LocalReport.ReportEmbeddedResource = "PMV.Tributario.Fiscalizacao.Relatorio.rptGuiaArrecadacao.rdlc";
                rptVisualizador.LocalReport.DataSources.Add(new ReportDataSource("dtsCabecalho", (DataTable)_dtNotificacao));
                rptVisualizador.LocalReport.DataSources.Add(new ReportDataSource("dtsParcelamento", (DataTable)_dtParcelamento));
                rptVisualizador.LocalReport.DataSources.Add(new ReportDataSource("dtsContribuinte", (DataTable)_dtNotificacaoContribuinte.Where(x => x.PRINCIPAL == true).CopyToDataTable()));
                rptVisualizador.LocalReport.DataSources.Add(new ReportDataSource("dtsComplemento", (DataTable)_dtParcelamentoComplemento));
                rptVisualizador.LocalReport.DataSources.Add(new ReportDataSource("dtsAgrupado", (DataTable)dtbValorAgrupado));
                rptVisualizador.LocalReport.DataSources.Add(new ReportDataSource("dtsParcela", (DataTable)_dtParcelaImpressao));
            }
            else if (relatorio == "TC")
            {
                DataTable _dtbValorAgrupado = new DataTable();

                _dtNotificacao = blNotificacao.BuscarNotificacaoIdImpressao(idNotificacao);
                _dtParcelamento = blNotificacao.ObterParcelamentoAtivo(idNotificacao);
                _dtParcela = blNotificacao.ObterParcela(Convert.ToInt32(_dtParcelamento.Rows[0]["ID_NOTIFICACAO_PARCELAMENTO"]));
                _dtbValorAgrupado = blNotificacao.ObterTotalAgrupado(Convert.ToInt32(_dtParcelamento.Rows[0]["ID_NOTIFICACAO_PARCELAMENTO"]));

                rptVisualizador.LocalReport.ReportEmbeddedResource = "PMV.Tributario.Fiscalizacao.Relatorio.rptTermoConfissao.rdlc";
                rptVisualizador.LocalReport.DataSources.Add(new ReportDataSource("dtsCabecalho", (DataTable)_dtNotificacao));
                rptVisualizador.LocalReport.DataSources.Add(new ReportDataSource("dtsParcelamento", (DataTable)_dtParcelamento));
                rptVisualizador.LocalReport.DataSources.Add(new ReportDataSource("dtsAgrupado", (DataTable)_dtbValorAgrupado));
                rptVisualizador.LocalReport.DataSources.Add(new ReportDataSource("dtsParcelas", (DataTable)_dtParcela));
            }
            else if (relatorio == "TR")
            {
                _dtNotificacao = blNotificacao.BuscarNotificacaoIdImpressao(idNotificacao);
                _dtNotificacaoContribuinte = blNotificacao.ListarContribuinteIdNotificacao(idNotificacao);
                _dtTermoRemessa = blNotificacao.ObterTermoRemessa(idNotificacao);
                _dtLancamento = blNotificacao.ListarLancamentoNotificacaoIdNotificacao(idNotificacao);
                _dtNotificacaoParcelamentoAtualizado = blNotificacao.ObterParcelamentoAtualizado(idNotificacao);
                _dtParcelamento = blNotificacao.ObterParcelamento(idNotificacao);
                _dtNotificacaoUnificacao = blNotificacao.ObterUnificacao(idNotificacao);

                decimal issConstrucao = 0;
                decimal issDemolicao = 0;
                decimal issReforma = 0;
                decimal issEngenharia = 0;
                decimal licencaConstrucao = 0;
                decimal licencaReformaDemo = 0;
                decimal alinhamento = 0;
                decimal alvara = 0;
                decimal penalidade = 0;
                decimal valorTotal = 0;
                bool construcaoCobrado = false;
                bool implantacaoCobrado = false;
                bool desmenbramentoCobrado = false;
                bool unificacaoCobrado = false;
                string baselegal = string.Empty;
                string taxas = "Taxas do Poder de Polícia + Preço Público + ";
                string causaSuspensao = " ";
                string periodoInterrupcao = " ";
                string inscricaoUnificada = string.Empty;
                foreach (DataRow drLancamento in _dtLancamento.Rows)
                {
                    #region Taxas

                    alvara = 0;

                    if (alinhamento == 0)
                    {
                        alinhamento = Convert.ToDecimal(drLancamento["TX_ALINHAMENTO"]);
                    }
                    else
                    {
                        alinhamento = 0;
                    }

                    if (drLancamento["TIPO"].ToString() == "CONSTRUÇÃO/REFORMA/DEMOLIÇÃO" && construcaoCobrado == false)
                    {
                        alvara += Convert.ToDecimal(drLancamento["TX_ALVARA"]);

                        construcaoCobrado = true;
                    }
                    else if (drLancamento["TIPO"].ToString() == "IMPLANTAÇÃO" && implantacaoCobrado == false)
                    {
                        alvara += Convert.ToDecimal(drLancamento["TX_ALVARA"]);

                        implantacaoCobrado = true;
                    }
                    else if (drLancamento["TIPO"].ToString() == "DESMEMBRAMENTO" && desmenbramentoCobrado == false)
                    {
                        alvara += Convert.ToDecimal(drLancamento["TX_ALVARA"]);

                        desmenbramentoCobrado = true;
                    }
                    else if (drLancamento["TIPO"].ToString() == "UNIFICAÇÃO" && unificacaoCobrado == false)
                    {
                        alvara += Convert.ToDecimal(drLancamento["TX_ALVARA"]);

                        unificacaoCobrado = true;
                    }

                    issConstrucao = Convert.ToDecimal(drLancamento["IMPOSTO_ISS_CONSTRUCAO"]);
                    issDemolicao = Convert.ToDecimal(drLancamento["IMPOSTO_ISS_DEMOLICAO"]);
                    issReforma = Convert.ToDecimal(drLancamento["IMPOSTO_ISS_REFORMA"]);
                    issEngenharia = Convert.ToDecimal(drLancamento["IMPOSTO_ISS_ENGENHARIA"]);
                    licencaConstrucao = Convert.ToDecimal(drLancamento["TX_LICENCA_CONSTRUCAO"]);
                    licencaReformaDemo = Convert.ToDecimal(drLancamento["TX_LICENCA_REFORMA_DEMOLICAO"]);
                    penalidade = Convert.ToDecimal(drLancamento["TX_PENALIDADES"]);

                    if (issConstrucao > 0 || issDemolicao > 0 || issReforma > 0 || issEngenharia > 0)
                    {
                        taxas += "ISS + ";
                    }

                    if (issConstrucao > 0)
                    {
                        baselegal += drLancamento["ISS_CONSTRUCAO_UFM_DESCRICAO"] + " - ";
                    }
                    if (issDemolicao > 0)
                    {
                        baselegal += drLancamento["ISS_DEMOLICAO_UFM_DESCRICAO"] + " - ";
                    }
                    if (issReforma > 0)
                    {
                        baselegal += drLancamento["ISS_REFORMA_UFM_DESCRICAO"] + " - ";
                    }
                    if (issEngenharia > 0)
                    {
                        baselegal += drLancamento["ISS_ENGENHARIA_UFM_DESCRICAO"] + " - ";
                    }
                    if (licencaConstrucao > 0)
                    {
                        baselegal += drLancamento["TX_CONSTRUCAO_UFM_DESCRICAO"] + " - ";
                    }
                    if (licencaReformaDemo > 0)
                    {
                        baselegal += drLancamento["TX_LICENCA_UFM_DESCRICAO"] + " - ";
                    }
                    if (alinhamento > 0)
                    {
                        baselegal += drLancamento["TX_ALINHAMENTO_UFM_DESCRICAO"] + " - ";
                    }
                    if (alvara > 0)
                    {
                        baselegal += drLancamento["TX_ALVARA_UFM_DESCRICAO"] + " - ";
                    }
                    if (penalidade > 0)
                    {
                        baselegal += drLancamento["TX_PENALIDADES_UFM_DESCRICAO"] + " - ";

                        taxas += "Penalidade";
                    }

                    #endregion
                }

                foreach (DataRow drQualificacao in _dtNotificacaoContribuinte.Rows)
                {
                    drQualificacao["LOGRADOURO"] = (drQualificacao["LOGRADOURO"].ToString() + ", " + drQualificacao["NUMERO"].ToString() + ", " + drQualificacao["BAIRRO"].ToString() + ", " + drQualificacao["COMPLEMENTO"].ToString() + ", " + drQualificacao["CIDADE"].ToString() + "/" + drQualificacao["UF"].ToString() + ", " + drQualificacao["CEP"].ToString()).ToUpper();
                }

                foreach (DSNotificacaoLancamento.MO_NOTIFICACAO_LANCAMENTO_PARCELAMENTO_ATUALIZADORow dtrAtualizado in _dtNotificacaoParcelamentoAtualizado)
                {
                    valorTotal += dtrAtualizado.VALOR;
                }

                foreach (DataRow item in _dtParcelamento)
                {
                    DataRow dtrdados = _dtDados.NewRow();

                    dtrdados["DESCRICAO"] = "Processo :" + item["CANCELAMENTO_PROCESSO"].ToString() + "/" + item["CANCELAMENTO_PROCESSO_ANO"].ToString() + " Parcelamento : " + item["AVISO"].ToString();
                    dtrdados["VALOR"] = "De " + Convert.ToDateTime(item["DATA_CRIACAO"].ToString()).ToShortDateString() + " Até " + Convert.ToDateTime(item["CANCELAMENTO_DATA"].ToString()).ToShortDateString() + " considerando o disposto no art. 8º, caput, da lei 1719/03";

                    _dtDados.Rows.Add(dtrdados);
                }

                _dtNotificacaoHistorico = blNotificacao.ObterHistorico(idNotificacao, "SUSPENSÃO");
                _dtNotificacaoHistoricoMerge = blNotificacao.ObterHistorico(idNotificacao, "ATIVAÇÃO");

                _dtNotificacaoHistoricoMerge.Merge(_dtNotificacaoHistorico);

                if (_dtNotificacaoHistoricoMerge.Rows.Count > 0)
                {
                    foreach (DataRow item in _dtNotificacaoHistoricoMerge.OrderBy(x => x.ID_HISTORICO).ToList())
                    {
                        if (item["TIPO"].ToString() == "SUSPENSÃO")
                        {
                            causaSuspensao = "Processo : " + item["PROCESSO"].ToString() + "/" + item["PROCESSO_ANO"].ToString() + " Motivo : " + item["MOTIVO"];

                            periodoInterrupcao += "De " + Convert.ToDateTime(item["DATA"]).ToShortDateString();
                        }
                        else
                        {
                            periodoInterrupcao += " Até " + Convert.ToDateTime(item["DATA"]).ToShortDateString();

                            DataRow dtrdados = _dtDados.NewRow();

                            dtrdados["DESCRICAO"] = causaSuspensao;
                            dtrdados["VALOR"] = periodoInterrupcao;

                            _dtDados.Rows.Add(dtrdados);
                            periodoInterrupcao = "";
                        }
                    }
                }

                foreach (DataRow drUnificacao in _dtNotificacaoUnificacao.Rows)
                {
                    inscricaoUnificada += drUnificacao["INSCRICAO"].ToString() + ", ";
                }

                inscricaoUnificada = (inscricaoUnificada.Length > 0 ? (inscricaoUnificada).Substring(0, (inscricaoUnificada).Length - 2) : "");

                _dtNotificacao.Rows[0]["INSCRICAO"] = _dtNotificacao[0]["INSCRICAO"].ToString() + ", " + inscricaoUnificada;

                rptVisualizador.LocalReport.ReportEmbeddedResource = "PMV.Tributario.Fiscalizacao.Relatorio.rptTermoRemessa.rdlc";
                rptVisualizador.LocalReport.DataSources.Add(new ReportDataSource("dtsCabecalho", (DataTable)_dtNotificacao));
                rptVisualizador.LocalReport.DataSources.Add(new ReportDataSource("dtsContribuinte", (DataTable)_dtNotificacaoContribuinte));
                rptVisualizador.LocalReport.DataSources.Add(new ReportDataSource("dtsRemessa", (DataTable)_dtTermoRemessa));
                rptVisualizador.LocalReport.DataSources.Add(new ReportDataSource("dtsDados", (DataTable)_dtDados));
                rptVisualizador.LocalReport.DataSources.Add(new ReportDataSource("dtsUnificacao", (DataTable)_dtNotificacaoUnificacao));

                ReportParameter p = new ReportParameter("naturezaDebito", taxas);
                this.rptVisualizador.LocalReport.SetParameters(new ReportParameter[] { p });

                p = new ReportParameter("baseLegal", baselegal);
                this.rptVisualizador.LocalReport.SetParameters(new ReportParameter[] { p });

                p = new ReportParameter("valor", valorTotal.ToString());
                this.rptVisualizador.LocalReport.SetParameters(new ReportParameter[] { p });
            }
            else if (relatorio == "SD")
            {
                DSNotificacaoLancamento.MO_NOTIFICACAO_LANCAMENTO_PARCELAMENTO_ATUALIZADODataTable _dtNotificacaoParcelamentoAtualizado;

                _dtNotificacaoParcelamentoAtualizado = blNotificacao.ObterParcelamentoAtualizado(idNotificacao);

                _dtNotificacao = blNotificacao.BuscarNotificacaoIdImpressao(idNotificacao);

                DSNotificacaoLancamento.MO_NOTIFICACAO_LANCAMENTO_PARCELAMENTORow dtr = null;
                DSNotificacaoLancamento.MO_NOTIFICACAO_LANCAMENTO_PARCELAMENTODataTable _dtParcelamentoProcessado = new DSNotificacaoLancamento.MO_NOTIFICACAO_LANCAMENTO_PARCELAMENTODataTable();

                if (codParcelamento == string.Empty)
                {
                    _dtParcelamento = blNotificacao.ObterParcelamentoAtivo(idNotificacao);

                    dtr = _dtParcelamento.Where(x => x.ATIVO == true).FirstOrDefault();

                    if (dtr == null)
                    {
                        _dtParcelamento = blNotificacao.ObterParcelamento(idNotificacao);

                        dtr = _dtParcelamento.OrderByDescending(x => x.ID_NOTIFICACAO_PARCELAMENTO).First();
                    }

                    _dtParcelamentoProcessado.ImportRow(dtr);
                }
                else
                {
                    _dtParcelamento = blNotificacao.Obter(Convert.ToInt32(codParcelamento));

                    dtr = _dtParcelamento.FirstOrDefault();

                    _dtParcelamentoProcessado.ImportRow(dtr);
                }

                for (int i = 0; i < _dtNotificacaoParcelamentoAtualizado.Count; i++)
                {
                    _dtNotificacaoParcelamentoAtualizado.Rows[i]["TRIBUTO"] = _dtNotificacaoParcelamentoAtualizado.Rows[i]["TRIBUTODESC"].ToString();
                }

                UFV objUFV = new UFV();

                DL.Shared.DS.DSShared.SH_UFVRow drUFV = objUFV.BuscarData(Convert.ToDateTime(dtr.DATA_VENCIMENTO));

                decimal valorTabela = drUFV.VALOR;

                rptVisualizador.LocalReport.ReportEmbeddedResource = "PMV.Tributario.Fiscalizacao.Relatorio.rptSaldoDevedor.rdlc";
                rptVisualizador.LocalReport.DataSources.Add(new ReportDataSource("dtsCabecalho", (DataTable)_dtNotificacao));
                rptVisualizador.LocalReport.DataSources.Add(new ReportDataSource("dtsParcelamento", (DataTable)_dtParcelamentoProcessado));
                rptVisualizador.LocalReport.DataSources.Add(new ReportDataSource("dtsComplemento", (DataTable)_dtNotificacaoParcelamentoAtualizado)); 
            }
            else if (relatorio == "ID")
            {
                var blISSDeclarado = new BL.Fiscalizacao.ISS.ISSDeclarado();
                var blISSDeclaracao = new BL.Fiscalizacao.ISS.ISSDeclaradoDeclaracao();

                var dtDeclarado = blISSDeclarado.BuscarISSDeclarado(idNotificacao).Table;

                var dtDeclaracao = blISSDeclaracao.BuscarPorIdISSDeclarado(idNotificacao);


                rptVisualizador.LocalReport.ReportEmbeddedResource = "PMV.Tributario.Fiscalizacao.Relatorio.rptISSDeclarado.rdlc";
                rptVisualizador.LocalReport.DataSources.Add(new ReportDataSource("dsISSDeclarado", (DataTable)dtDeclarado));
                rptVisualizador.LocalReport.DataSources.Add(new ReportDataSource("dsISSDeclaradoDeclaracao", (DataTable)dtDeclaracao));
            }
            else if (relatorio == "XX")
            {
                var blNotificacaoISSDeclarado = new BL.Fiscalizacao.ISS.NotificacaoISSDeclarado();
                //var blISSDeclaracao = new BL.Fiscalizacao.ISS.ISSDeclaradoDeclaracao();

                var dtDeclarado = blNotificacaoISSDeclarado.BuscarNotificacaoISSDeclarado(29463, 29643, 2024, 'T');
                var dtDeclaradoDeclaracao = blNotificacaoISSDeclarado.BuscarNotificacaoISSDeclaradoDeclaracao(1766);

                //var dtDeclaracao = blISSDeclaracao.BuscarPorIdISSDeclarado(idNotificacao);

                rptVisualizador.LocalReport.ReportEmbeddedResource = "PMV.Tributario.Fiscalizacao.Relatorio.rptNotificacaoISSDeclarado.rdlc";
                rptVisualizador.LocalReport.DataSources.Add(new ReportDataSource("dsNotificacaoISSDeclarado", (DataTable)dtDeclarado));
                rptVisualizador.LocalReport.DataSources.Add(new ReportDataSource("dsNotificacaoISSDeclaradoDeclaracao", (DataTable)dtDeclaradoDeclaracao));
                //rptVisualizador.LocalReport.DataSources.Add(new ReportDataSource("dsDeclaradoDeclaracao", (DataTable)dtDeclaracao));
            }


            this.rptVisualizador.RefreshReport();

            this.ShowDialog();
        }

        public void ShowDialog(DataTable dt,string relatorio)
        {
            if (relatorio == "EDITALIPTU")
            {
                var drAssinatura = new BL.Shared.Cadastro.AssinaturaRelatorio().BuscarAssinatura("IM", "RelEditalIPTUSeloVerde");
                rptVisualizador.LocalReport.ReportEmbeddedResource = "PMV.Tributario.Fiscalizacao.Relatorio.rptEditaIPTUSeloVerde.rdlc";

                Shared.Form.RelatorioDadosAdicionais frmDados = new Shared.Form.RelatorioDadosAdicionais();

                if (frmDados.ShowDialog() == DialogResult.OK)
                {
                    var assinatura = $"{drAssinatura.NomeAssinatura}{Environment.NewLine}{drAssinatura.CargoAssinatura}{Environment.NewLine}{Environment.NewLine}{drAssinatura.DepartamentoAssinatura}{Environment.NewLine}{drAssinatura.SecaoAssinatura}";

                    rptVisualizador.LocalReport.SetParameters(new ReportParameter("TITULO", frmDados.Titulo));
                    rptVisualizador.LocalReport.SetParameters(new ReportParameter("Header", frmDados.Cabecalho));
                    rptVisualizador.LocalReport.SetParameters(new ReportParameter("Footer", frmDados.Rodape));
                    rptVisualizador.LocalReport.SetParameters(new ReportParameter("Data", $"Votorantim, {DateTime.Now:M} de {DateTime.Now:yyyy}."));
                    rptVisualizador.LocalReport.SetParameters(new ReportParameter("Assinatura", assinatura));
                    rptVisualizador.LocalReport.DataSources.Add(new ReportDataSource("dsRelatorio", dt));
                    rptVisualizador.RefreshReport();
                }
                else
                {
                    return;
                }
            }
            else if (relatorio == "IMPPUBLISTAMB")
            {
                rptVisualizador.LocalReport.ReportEmbeddedResource = "PMV.Tributario.Fiscalizacao.Relatorio.rptDeclaracaoISSDeclaradoPublicacao.rdlc";
                rptVisualizador.LocalReport.DataSources.Add(new ReportDataSource("dsRelatorio", dt));
                rptVisualizador.RefreshReport();
            }
            else if (relatorio == "IMPPUBNOTTOM")
            {
                rptVisualizador.LocalReport.ReportEmbeddedResource = "PMV.Tributario.Fiscalizacao.Relatorio.rptDeclaracaoISSDeclaradoPublicacaoNotificacao.rdlc";
                rptVisualizador.LocalReport.DataSources.Add(new ReportDataSource("dsRelatorio", dt));
                rptVisualizador.RefreshReport();
            }
            else if (relatorio == "IMPPUBNOTPRE")
            {
                rptVisualizador.LocalReport.ReportEmbeddedResource = "PMV.Tributario.Fiscalizacao.Relatorio.rptDeclaracaoISSDeclaradoPublicacaoNotificacaoPrest.rdlc";
                rptVisualizador.LocalReport.DataSources.Add(new ReportDataSource("dsRelatorio", dt));
                rptVisualizador.RefreshReport();
            }
            else if (relatorio == "IMPPUBINSCRTOM")
            {
                rptVisualizador.LocalReport.ReportEmbeddedResource = "PMV.Tributario.Fiscalizacao.Relatorio.rptDeclaracaoIssDeclaradoPublicacaoPorEdital.rdlc";
                rptVisualizador.LocalReport.DataSources.Add(new ReportDataSource("dsRelatorio", dt));
                rptVisualizador.RefreshReport();
            }
            else if (relatorio == "IMPPUBINSCRPRE")
            {
                rptVisualizador.LocalReport.ReportEmbeddedResource = "PMV.Tributario.Fiscalizacao.Relatorio.rptDeclaracaoIssDeclaradoPublicacaoPorEditalPrest.rdlc";
                rptVisualizador.LocalReport.DataSources.Add(new ReportDataSource("dsRelatorio", dt));
                rptVisualizador.RefreshReport();
            }

            this.ShowDialog();
        }
        // O método EscreverExtenso recebe um valor do tipo decimal
        public static string EscreverExtenso(decimal valor)
        {
            if (valor <= 0 | valor >= 1000000000000000)
                return "Valor não suportado pelo sistema.";
            else
            {
                string strValor = valor.ToString("000000000000000.00");
                string valor_por_extenso = string.Empty;

                for (int i = 0; i <= 15; i += 3)
                {
                    valor_por_extenso += Escrever_Valor_Extenso(Convert.ToDecimal(strValor.Substring(i, 3)));

                    if (i == 0 & valor_por_extenso != string.Empty)
                    {
                        if (Convert.ToInt32(strValor.Substring(0, 3)) == 1)
                            valor_por_extenso += " TRILHÃO" + ((Convert.ToDecimal(strValor.Substring(3, 12)) > 0) ? " E " : string.Empty);
                        else if (Convert.ToInt32(strValor.Substring(0, 3)) > 1)
                            valor_por_extenso += " TRILHÕES" + ((Convert.ToDecimal(strValor.Substring(3, 12)) > 0) ? " E " : string.Empty);
                    }
                    else if (i == 3 & valor_por_extenso != string.Empty)
                    {
                        if (Convert.ToInt32(strValor.Substring(3, 3)) == 1)
                            valor_por_extenso += " BILHÃO" + ((Convert.ToDecimal(strValor.Substring(6, 9)) > 0) ? " E " : string.Empty);
                        else if (Convert.ToInt32(strValor.Substring(3, 3)) > 1)
                            valor_por_extenso += " BILHÕES" + ((Convert.ToDecimal(strValor.Substring(6, 9)) > 0) ? " E " : string.Empty);
                    }
                    else if (i == 6 & valor_por_extenso != string.Empty)
                    {
                        if (Convert.ToInt32(strValor.Substring(6, 3)) == 1)
                            valor_por_extenso += " MILHÃO" + ((Convert.ToDecimal(strValor.Substring(9, 6)) > 0) ? " E " : string.Empty);
                        else if (Convert.ToInt32(strValor.Substring(6, 3)) > 1)
                            valor_por_extenso += " MILHÕES" + ((Convert.ToDecimal(strValor.Substring(9, 6)) > 0) ? " E " : string.Empty);
                    }
                    else if (i == 9 & valor_por_extenso != string.Empty)
                        if (Convert.ToInt32(strValor.Substring(9, 3)) > 0)
                            valor_por_extenso += " MIL" + ((Convert.ToDecimal(strValor.Substring(12, 3)) > 0) ? " E " : string.Empty);

                    if (i == 12)
                    {
                        if (valor_por_extenso.Length > 8)
                            if (valor_por_extenso.Substring(valor_por_extenso.Length - 6, 6) == "BILHÃO" | valor_por_extenso.Substring(valor_por_extenso.Length - 6, 6) == "MILHÃO")
                                valor_por_extenso += " DE";
                            else
                                if (valor_por_extenso.Substring(valor_por_extenso.Length - 7, 7) == "BILHÕES" | valor_por_extenso.Substring(valor_por_extenso.Length - 7, 7) == "MILHÕES" | valor_por_extenso.Substring(valor_por_extenso.Length - 8, 7) == "TRILHÕES")
                                valor_por_extenso += " DE";
                            else
                                    if (valor_por_extenso.Substring(valor_por_extenso.Length - 8, 8) == "TRILHÕES")
                                valor_por_extenso += " DE";

                        if (Convert.ToInt64(strValor.Substring(0, 15)) == 1)
                            valor_por_extenso += " REAL";
                        else if (Convert.ToInt64(strValor.Substring(0, 15)) > 1)
                            valor_por_extenso += " REAIS";

                        if (Convert.ToInt32(strValor.Substring(16, 2)) > 0 && valor_por_extenso != string.Empty)
                            valor_por_extenso += " E ";
                    }

                    if (i == 15)
                        if (Convert.ToInt32(strValor.Substring(16, 2)) == 1)
                            valor_por_extenso += " CENTAVO";
                        else if (Convert.ToInt32(strValor.Substring(16, 2)) > 1)
                            valor_por_extenso += " CENTAVOS";
                }
                return valor_por_extenso;
            }
        }

        static string Escrever_Valor_Extenso(decimal valor)
        {
            if (valor <= 0)
                return string.Empty;
            else
            {
                string montagem = string.Empty;
                if (valor > 0 & valor < 1)
                {
                    valor *= 100;
                }
                string strValor = valor.ToString("000");
                int a = Convert.ToInt32(strValor.Substring(0, 1));
                int b = Convert.ToInt32(strValor.Substring(1, 1));
                int c = Convert.ToInt32(strValor.Substring(2, 1));

                if (a == 1) montagem += (b + c == 0) ? "CEM" : "CENTO";
                else if (a == 2) montagem += "DUZENTOS";
                else if (a == 3) montagem += "TREZENTOS";
                else if (a == 4) montagem += "QUATROCENTOS";
                else if (a == 5) montagem += "QUINHENTOS";
                else if (a == 6) montagem += "SEISCENTOS";
                else if (a == 7) montagem += "SETECENTOS";
                else if (a == 8) montagem += "OITOCENTOS";
                else if (a == 9) montagem += "NOVECENTOS";

                if (b == 1)
                {
                    if (c == 0) montagem += ((a > 0) ? " E " : string.Empty) + "DEZ";
                    else if (c == 1) montagem += ((a > 0) ? " E " : string.Empty) + "ONZE";
                    else if (c == 2) montagem += ((a > 0) ? " E " : string.Empty) + "DOZE";
                    else if (c == 3) montagem += ((a > 0) ? " E " : string.Empty) + "TREZE";
                    else if (c == 4) montagem += ((a > 0) ? " E " : string.Empty) + "QUATORZE";
                    else if (c == 5) montagem += ((a > 0) ? " E " : string.Empty) + "QUINZE";
                    else if (c == 6) montagem += ((a > 0) ? " E " : string.Empty) + "DEZESSEIS";
                    else if (c == 7) montagem += ((a > 0) ? " E " : string.Empty) + "DEZESSETE";
                    else if (c == 8) montagem += ((a > 0) ? " E " : string.Empty) + "DEZOITO";
                    else if (c == 9) montagem += ((a > 0) ? " E " : string.Empty) + "DEZENOVE";
                }
                else if (b == 2) montagem += ((a > 0) ? " E " : string.Empty) + "VINTE";
                else if (b == 3) montagem += ((a > 0) ? " E " : string.Empty) + "TRINTA";
                else if (b == 4) montagem += ((a > 0) ? " E " : string.Empty) + "QUARENTA";
                else if (b == 5) montagem += ((a > 0) ? " E " : string.Empty) + "CINQUENTA";
                else if (b == 6) montagem += ((a > 0) ? " E " : string.Empty) + "SESSENTA";
                else if (b == 7) montagem += ((a > 0) ? " E " : string.Empty) + "SETENTA";
                else if (b == 8) montagem += ((a > 0) ? " E " : string.Empty) + "OITENTA";
                else if (b == 9) montagem += ((a > 0) ? " E " : string.Empty) + "NOVENTA";

                if (strValor.Substring(1, 1) != "1" & c != 0 & montagem != string.Empty) montagem += " E ";

                if (strValor.Substring(1, 1) != "1")
                    if (c == 1) montagem += "UM";
                    else if (c == 2) montagem += "DOIS";
                    else if (c == 3) montagem += "TRÊS";
                    else if (c == 4) montagem += "QUATRO";
                    else if (c == 5) montagem += "CINCO";
                    else if (c == 6) montagem += "SEIS";
                    else if (c == 7) montagem += "SETE";
                    else if (c == 8) montagem += "OITO";
                    else if (c == 9) montagem += "NOVE";

                return montagem;
            }
        }
        private void RelVisualizador_Load(object sender, EventArgs e)
        {

        }
        public byte[] REGGerarCodigoBarra(string strCodigo)
        {
            const int intFino = 1;
            const int intLargo = 3;
            const int intAltura = 50;
            int intLarguraFinal = 450;
            int intAlturaFinal = 50;
            int intAtual = 0;
            int intI;
            int intF;
            int intF1;
            int intF2;
            string strF;
            string strTexto;

            string[] strCodigoBarra = new string[200];

            Size dimensaoFinal = new Size(intLarguraFinal, intAlturaFinal);

            Image oIMGFinal = new Bitmap(intLarguraFinal, intAlturaFinal, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            SolidBrush oBPreto = new SolidBrush(Color.Black);
            SolidBrush oBBranco = new SolidBrush(Color.White);
            Graphics oGrap = Graphics.FromImage(oIMGFinal);

            oGrap.FillRectangle(oBBranco, new Rectangle(0, 0, intLarguraFinal, intAlturaFinal));

            strCodigoBarra[0] = "00110";
            strCodigoBarra[1] = "10001";
            strCodigoBarra[2] = "01001";
            strCodigoBarra[3] = "11000";
            strCodigoBarra[4] = "00101";
            strCodigoBarra[5] = "10100";
            strCodigoBarra[6] = "01100";
            strCodigoBarra[7] = "00011";
            strCodigoBarra[8] = "10010";
            strCodigoBarra[9] = "01010";

            for (intF1 = 9; intF1 >= 0; intF1--)
            {
                for (intF2 = 9; intF2 >= 0; intF2--)
                {
                    intF = intF1 * 10 + intF2;

                    strTexto = "";

                    for (int intCont = 0; intCont <= 4; intCont++)
                    {
                        strTexto += strCodigoBarra[intF1].Substring(intCont, 1) + strCodigoBarra[intF2].Substring(intCont, 1);
                    }

                    strCodigoBarra[intF] = strTexto;
                }
            }

            oGrap.FillRectangle(oBPreto, new Rectangle(0, 0, intFino, intAltura));

            intAtual += intFino;

            oGrap.FillRectangle(oBBranco, new Rectangle(intAtual, 0, intFino, intAltura));

            intAtual += intFino;

            oGrap.FillRectangle(oBPreto, new Rectangle(intAtual, 0, intFino, intAltura));

            intAtual += intFino;

            oGrap.FillRectangle(oBBranco, new Rectangle(intAtual, 0, intFino, intAltura));

            intAtual += intFino;

            strTexto = strCodigo;

            if ((strTexto.Length % 2) != 0)
            {
                strTexto = "0" + strTexto;
            }

            while (strTexto.Length > 0)
            {
                intI = Convert.ToInt32(strTexto.Substring(0, 2));

                strTexto = InverteString(InverteString(strTexto).Substring(0, strTexto.Length - 2));

                strF = strCodigoBarra[intI];

                for (int intCont = 0; intCont <= 9; intCont = intCont + 2)
                {

                    if (strF.ToString().Substring(intCont, 1) == "0")
                        intF1 = intFino;
                    else
                        intF1 = intLargo;

                    oGrap.FillRectangle(oBPreto, new Rectangle(intAtual, 0, intF1, intAltura));

                    intAtual += intF1;

                    if (strF.ToString().Substring(intCont + 1, 1) == "0")
                        intF2 = intFino;
                    else
                        intF2 = intLargo;

                    oGrap.FillRectangle(oBBranco, new Rectangle(intF2, 0, intFino, intAltura));

                    intAtual += intF2;
                }
            }

            oGrap.FillRectangle(oBPreto, new Rectangle(intAtual, 0, intLargo, intAltura));

            intAtual += intLargo;

            oGrap.FillRectangle(oBBranco, new Rectangle(intAtual, 0, intFino, intAltura));

            intAtual += intFino;

            oGrap.FillRectangle(oBPreto, new Rectangle(intAtual, 0, 1, intAltura));

            intAtual += 1;

            //oIMGFinal.Save("c:\\temp\\foto.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);

            byte[] result;
            using (MemoryStream ms = new MemoryStream())
            {
                oIMGFinal.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                result = ms.ToArray();
            }

            oIMGFinal.Dispose();

            return result;

        }

        public static string InverteString(string strString)
        {

            string strResult = "";

            for (int intCont = strString.Length - 1; intCont >= 0; intCont--)
                strResult += strString.Substring(intCont, 1);

            return strResult;
        }

        private void rptVisualizador_Load(object sender, EventArgs e)
        {

        }
    }
}
