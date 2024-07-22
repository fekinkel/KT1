using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WebBoletoPP.Models;

namespace WebBoletoPP.Pages.Carne
{
    public class CreateModel : PageModel
    {

        [BindProperty]
        public CreateModelView ModelView { get; set; } = default!;
        public PpCarne PpCarne { get; set; } = default!;
        public int IndexListaPP { get; set; }

        private readonly BoletoPpHomologContext _context;

        //private readonly IProductService _productService;
        private readonly IWebHostEnvironment _webHostEnv;

        public CreateModel(IWebHostEnvironment webHostEnv, BoletoPpHomologContext context)
        {
            _context = context;
            _webHostEnv = webHostEnv;
            ModelView = new CreateModelView();
        }

        public async Task<IActionResult> OnGet(CreateModelView modview, string TipoGet)
        {

            ModelView.Codigo = "0";
            ModelView.Observacao = "0";
            ModelView.Quantidade = 1;
            ModelView.DataVencimento = DateOnly.FromDateTime(DateTime.Now);
            CreateModelView.ListaPrecoPublicos = CarregarListaPP(0);
            return Page();

            //if (jsonString.IsNullOrEmpty())
            //{
            //    ModelView = new CreateModelView(_context);
            //    ModelView.Codigo = "0";
            //    ModelView.Quantidade = 1;
            //    ModelView.DataVencimento = DateOnly.FromDateTime(DateTime.Now);
            //    return Page();
            //}
            //else
            //{
            //    Valor_PP val;
            //    try
            //    {
            //        val = JsonSerializer.Deserialize<Valor_PP>(jsonString);
            //    }
            //    catch (Exception ex)
            //    {
            //        throw;
            //        //return RedirectToPage("./Error");
            //    }
            //    if (val != null)
            //    {
            //        var valor1 = BuscarValorPP(val);
            //        return new JsonResult(new { valorufm = valor1, valorufir = 10 });
            //    }
            //    else
            //    {
            //        return new JsonResult(new { erro = "sem valor" });
            //    }
            //}
        }
        public async Task<IActionResult> OnPostGerarPdfAsync(CreateModelView modview, int PpId)
        {
            var caminhoReport = Path.Combine(_webHostEnv.WebRootPath, @"rpt\BoletoPP.frx");
            var reportFile = caminhoReport;

            var freport = new FastReport.Report();

            //var listaDados = _datasourse

            //freport.Dictionary.RegisterBusinessObject(listaDados, "listaDados", 10, true);
            //freport.Report.Save(reportFile);

            /*freport.Report.Load(reportFile);
            freport.Prepare();
            var pdfExport = new PDFSimpleExport();

            using MemoryStream ms = new MemoryStream();
            pdfExport.Export(freport, ms);

            ms.Flush();

            return File(ms.ToArray(), "application/pdf");*/

            var response = Path.Combine(_webHostEnv.WebRootPath, @"rpt\BoletoPP.pdf");
            string linha;
            List<byte[]> arrayList = new List<byte[]>(response.Length);

            /*using (StreamReader sr = new StreamReader(response))
            {
                int i = 0;
                while ((linha = sr.ReadLine()) != null)
                {
                    i += linha.Length;
                }
                
                byte[] bytes = new byte[i];
                bytes = Encoding.ASCII.GetBytes(response);

                return File(bytes.ToArray(), "application/pdf");
            }*/
            /*
            var document = new Document(response);
            var memoryStream = new System.IO.MemoryStream();
            document.Save(memoryStream);
            */
            // create ByteArray with PDF content


            // Load input PDF file
            string inputFile = response;

            // Initialize a byte array
            byte[] buff = null;

            // Initialize FileStream object
            FileStream fs = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            long numBytes = new FileInfo(inputFile).Length;

            // Load the file contents in the byte array
            buff = br.ReadBytes((int)numBytes);
            fs.Close();
            string valor1 = Encoding.UTF8.GetString(buff);
            string valor2 = Convert.ToBase64String(buff); // Encoding.UTF8.GetString(buff);

            return File(buff.ToArray(), "application/pdf");

        }
        public async Task<IActionResult> OnPostGerarPdfSqlAsync()
        {
            string s1 = BuscarValorSQL(1);

            byte[] buff = Convert.FromBase64String(s1); // Encoding.UTF8.GetBytes(s1);

            return File(buff.ToArray(), "application/pdf");
        }
        public async Task<IActionResult> OnPostGerarPdfRdlcAsync()
        {
            var caminhoReport = Path.Combine(_webHostEnv.WebRootPath, @"rpt\BoletoPP.rdlc");
            var reportFile = caminhoReport;

            string deviceInfo = "";
            string[] streamIds;
            //Warning[] warnings;
            string mineTypes = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            var viewer = new Microsoft.Reporting.WebForms.ReportViewer();
            //viewer.ProcessingMode = ProcessingMode.Local;
            //viewer.LocalReport.ReportPath = "rptBoletoPP.rdlc";

            //viewer.LocalReport.DataSources.Add(item: new ReportDataSource("dsBoletoPP", GetBoletoPPData()));

            //viewer.LocalReport.Refresh();

            //var bytes = viewer.LocalReport.Render("PDF", deviceInfo, out mineTypes, out encoding,
            //    out extension, out streamIds, out warnings);

            //return File(bytes.ToArray(), "application/pdf");

            return Page();
        }
        private List<Models.DadosBoletoPP> GetBoletoPPData()
        {
            return new List<DadosBoletoPP>
            {
                new DadosBoletoPP{Nome="Fernando",Sobrenome="Kinkel Serejo", Endereço="Jacinto Pagliato", Valor=1254.32},
            };
        }
        public async Task<IActionResult> OnPostSalvarAsync(CreateModelView modview, int PpId)
        {
            Valor_PP retorno = BuscarValorUfir("02/05/2024", modview.PpId);
            modview.ValorUFIR = retorno.valorufirdia;
            modview.ValorUfm = retorno.valorufmpp;
            return Page();
        }

        public async Task<IActionResult> OnPostValoresAsync(CreateModelView modview)
        {
            Valor_PP retorno = BuscarValorUfir("02/05/2024", modview.PpId);
            modview.ValorUFIR = retorno.valorufirdia;
            modview.ValorUfm = retorno.valorufmpp;
            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(CreateModelView modelView, string Action)
        {
            if (!ModelState.IsValid || _context.PpCarnes == null || PpCarne == null)
            {                
                ModelView.Codigo = "55";// ModelView.ListaPrecoPublicos.Count.ToString();
                CreateModelView.ListaPrecoPublicos = CarregarListaPP(0);
                return Page();
            }

            _context.PpCarnes.Add(PpCarne);
            _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
        public double BuscarValorPP(Valor_PP valIdPP)
        {
            /*if (valIdPP.vid > 0)
            {
                var PrecoPublicoQuery = from d in _context.PpPrecoPublicos
                                        where d.Id == valIdPP.id // Sort by name.
                                        select d.ValorUfm;

                return PrecoPublicoQuery.FirstOrDefault();

            }
            else
            {
                return 0;
            }
            */
            return 0;
        }
        public string BuscarValorSQL(int id_pp)
        {
            string sSql = $"Select text_PP_pdf as value From PP_PDF Where id_PP_Pdf = {id_pp}";

            var retorno = _context.Database.SqlQueryRaw<string>(sSql).FirstOrDefault();

            //return retorno.Substring(1, retorno.Length - 1);
            return retorno;


        }
        public Valor_PP BuscarValorUfir(string DataVencimento, int valIdPP)
        {
            if (valIdPP > 0)
            {
                string sSql = $"Select (Select convert(decimal(12,6), dbo.Valor_Ufir_dia(\'{DataVencimento}\')) as valorufirdia, convert(decimal(12,6), Valor_UFM) as valorufmpp From PP_PRECO_PUBLICO Where ID = {valIdPP} For Json Path) as value";

                var retorno =  _context.Database.SqlQueryRaw<string>(sSql).FirstOrDefault();

                if (retorno != null)
                {
                    retorno = retorno.Trim(new char[] { ']', '[' });

                    Valor_PP valPP = JsonConvert.DeserializeObject<Valor_PP>(retorno);

                    return valPP;
                }
                else
                {
                    return new Valor_PP();
                }
            }
            else
            {
                return new Valor_PP();
            }
        }
        public List<SelectListItem> CarregarListaPP(int IdPP)
        {
            var lista = new List<SelectListItem>();

            var PrecoPublicoQuery = from d in _context.PpPrecoPublicos
                                    orderby d.Id // Sort by name.
                                    select d;
            var precopublico = PrecoPublicoQuery.ToList();


            try
            {
                foreach (var item in precopublico)
                {
                    var option = new SelectListItem()
                    {
                        Text = item.Descricao,
                        Value = item.Id.ToString(),
                        Selected = (item.Id == IdPP)
                    };
                    lista.Add(option);
                }

            }
            catch (Exception ex)
            {
                throw;
            }

            return lista;
        }

    }

    public class Valor_PP
    {
        public decimal valorufirdia { get; set; }
        public decimal valorufmpp { get; set; }
    }
    public class CreateModelView
    {
        [Display(Name = "Código")]
        public string Codigo { get; set; } = null!;
        [Display(Name = "Parcela")]
        public int Parcela { get; set; }
        [Display(Name = "Valor do Serviço em UFM")]
        public decimal ValorUfm { get; set; }
        [Display(Name = "Valor da UFIR")]
        [DisplayFormat(DataFormatString="{0:C2}",ApplyFormatInEditMode = true)]
        public decimal ValorUFIR { get; set; }
        public double Quantidade { get; set; }
        public double ValorReal { get; set; }
        public string Observacao { get; set; } = null!;
        public string Login { get; set; } = null!;
        [Display(Name = "Selecione o serviço:")]
        public int PpId { get; set; }
        public IEnumerable<SelectListItem> ePPid {  get; set; }

        [Display(Name = "Data de Vencimento")]
        public DateOnly DataVencimento { get; set; }
        public DateOnly? DataPagamento { get; set; } = null;
        public static List<SelectListItem> ListaPrecoPublicos { get; set; }
        public CreateModelView() { }

        #region Métodos


        #endregion
    }
}
