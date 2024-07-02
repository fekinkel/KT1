using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebKinkel.Data;
using WebKinkel.Models;

namespace WebKinkel.Pages.Carne
{
    public class CreateModel : PageModel
    {
        private readonly WebKinkel.Data.BoletoPpHomologContext _context;

        public CreateModelView ModelView { get; set; } = default!;

        public CreateModel(WebKinkel.Data.BoletoPpHomologContext context)
        {
            _context = context;

            ModelView = new CreateModelView(_context);
        }
        public ActionResult Index(CalcularIMCModel model)
        {
            var selectedValue = model.ModelView.SelectTipoBebida;
            //ViewBag.TipoBebida = selectedValue.ToString();
            return NotFound();
        }
        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public PpCarne PpCarne { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.PpCarnes == null || PpCarne == null)
            {
                ModelView.Codigo = ModelView.ListaPrecoPublicos.Count.ToString();
                return Page();
            }

            _context.PpCarnes.Add(PpCarne);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
    public class CreateModelView
    {
        [ValidateNever]
        public string Codigo { get; set; } = null!;
        public int Parcela { get; set; }
        public double ValorUfm { get; set; }
        public double ValorReal { get; set; }
        [ValidateNever]
        public string Observacao { get; set; } = null!;
        public string Login { get; set; } = null!;
        [ValidateNever]
        public int PpId { get; set; }
        public DateTime DataVencimento { get; set; }
        public DateTime? DataPagamento { get; set; } = null;
        public double Quantidade { get; set; }

        private readonly BoletoPpHomologContext _contextview;
        public List<SelectListItem> ListaPrecoPublico { get; set; }
        public List<SelectListItem> ListaPrecoPublicos { get; set; }

        //public ListaPrecoPublicos ItemLista {  get; set; }
        public CreateModelView(BoletoPpHomologContext _context, object SelectPrecoPublico = null)
        {
/*            var PrecoPublicoQuery = from d in _context.PpPrecoPublicos
                                    orderby d.Id // Sort by name.
                                    select d;
            ListaPrecoPublico = PrecoPublicoQuery.ToList();
*/            
            _contextview = _context;

            ListaPrecoPublicos = CarregarListaPP(0);
        }

        #region Métodos

        public List<SelectListItem> CarregarListaPP(int IdPP)
        {
            var lista = new List<SelectListItem>();

            var PrecoPublicoQuery = from d in _contextview.PpPrecoPublicos
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
            catch(Exception ex)
            {
                throw;
            }

            return lista;
        }

        #endregion
    }
}
