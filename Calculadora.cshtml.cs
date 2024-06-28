using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebKinkel.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebKinkel.Pages
{
    public class CalculadoraModel : PageModel
    {
        public void OnGet()
        {
        }
        [BindProperty]
        public ModeloCalc ModeloCalc { get; set; } = default!;
        public CalculadoraModel() { 
            ModeloCalc = new ModeloCalc();
        }   
        [HttpGet]
        public ActionResult Index()
        {
            return Page();
        }
        [HttpPost]
        public ActionResult Index(ModeloCalc model, string command)
        {
            if (command == "add")
            {
                model.Resultado = model.A + model.B;
            }
            if (command == "sub")
            {
                model.Resultado = model.A - model.B;
            }
            if (command == "mul")
            {
                model.Resultado = model.A * model.B;
            }
            if (command == "div")
            {
                model.Resultado = model.A / model.B;
            }
            return Page();
        }

        public void OnPost(string command)
        {
            var model = ModeloCalc;

            if (command == "add")
            {
                model.Resultado = model.A + model.B;
            }
            if (command == "sub")
            {
                model.Resultado = model.A - model.B;
            }
            if (command == "mul")
            {
                model.Resultado = model.A * model.B;
            }
            if (command == "div")
            {
                model.Resultado = model.A / model.B;
            }
            //return model;
        }
    }
}
