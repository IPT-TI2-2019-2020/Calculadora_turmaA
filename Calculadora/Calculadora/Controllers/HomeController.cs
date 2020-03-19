using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Calculadora.Models;

namespace Calculadora.Controllers {
   public class HomeController : Controller {
      private readonly ILogger<HomeController> _logger;

      public HomeController(ILogger<HomeController> logger) {
         _logger = logger;
      }



      [HttpGet]
      public IActionResult Index() {

         // inicializar o primeiro valor do 'visor'
         ViewBag.Visor = "0";

         return View();
      }


      [HttpPost]
      public IActionResult Index(string visor, string bt, string primeiroOperando, string operador) {

         // filtrar o conteúdo da variável 'bt'
         switch (bt) {
            case "1":
            case "2":
            case "3":
            case "4":
            case "5":
            case "6":
            case "7":
            case "8":
            case "9":
            case "0":
               // processar os algarismos
               if (visor == "0") visor = bt;
               else visor += bt; // visor = visor + bt;

               break;

            case "+/-":
               // processar a inversão do valor no 'visor'
               visor = Convert.ToDouble(visor) * -1 + "";
               // outra hipótese, seria o processamento de strings
               // dentro do valor do  'visor' -> .StartsWith() , .Substring() , .Length
               break;

            case ",":
               // processar o separador da parte inteira da decimal
               if (!visor.Contains(",")) visor += ",";

               break;

            case "+":
            case "-":
            case "x":
            case ":":
               // processar os 'operadores'

               if (operador == null) {
                  // garantir o efeito de 'memória' no HTTP
                  ViewBag.PrimeiroOperando = visor;
                  ViewBag.Operador = bt;
               }
               else {
                  ;
               }

               // falta dar ordem para limpar o ecrã (visor)




               break;


         } // switch(bt)

         // exportar os dados para a View
         ViewBag.Visor = visor;

         return View();
      }







      [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
      public IActionResult Error() {
         return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
      }
   }
}
