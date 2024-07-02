using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using NToastNotify;
using System.Drawing.Drawing2D;
using WebKinkel.Data;
using WebKinkel.Models;

namespace WebKinkel.Pages
{
    public class CalcularIMCModel : PageModel
    {
        [BindProperty]
        public CalcularIMCModelView ModelView { get; set; }
        private BoletoPpHomologContext _context;
        //private IToastNotification _toastNotification;
        public CalcularIMCModel(BoletoPpHomologContext context)
        {
            _context = context;
            //_toastNotification = toastNotification;
        }
        public void OnGet()
        {
            ModelView = new CalcularIMCModelView();
            ModelView.Altura = 1.65;
            ModelView.Peso = 70;

        }
    
        public ActionResult Index(CalcularIMCModel model)
        {
            var selectedValue = model.ModelView.SelectTipoBebida;
            //ViewBag.TipoBebida = selectedValue.ToString();
            return NotFound();
        }
        public void OnPost(string command)
        {
            if (command == "add")
            {
                ModelView.Calc();
                //ModelView.TipoDeBebida = ModelView.SelectTipoBebida.ToString();
            }
            if (command == "sub")
            {

            }
            if (command == "mul")
            {
            }
            if (command == "div")
            {

            }
            if (command.IsNullOrEmpty())
            {
                //var selectedValue = ModelView.SelectTipoBebida;
                //ModelView.TipoDeBebida = selectedValue.ToString();
                //ModelView.TipoDeBebida = ModelView.SelectTipoBebida.ToString();
            }
        }


    }
    public class CalcularIMCModelView
    {
        public double Altura { get; set; }
        public double Peso { get; set; }= double.MaxValue;
        public double IMC { get; set; }=double.MaxValue;
        public string TipoDeBebida {  get; set; }

        public CalcularIMCModelView() { }

        [HttpPost]
        public void OnPost(string command)
        {
            if (command == "add")
            {
                Calc();
            }
        }
            public void Calc()
        {
            this.IMC = Altura * Peso;
        }
        private TipoBebida _tipoBebida { get; set;}
        public TipoBebida SelectTipoBebida 
        {
            get
            {  return _tipoBebida; }
            set
            {
                _tipoBebida = value;
                this.TipoDeBebida = value.ToString();
                this.IMC = 0;
            }

        }
        public enum TipoBebida
        {
            Tea, Coffee, GreenTea, BlackTea
        }


    }
}
