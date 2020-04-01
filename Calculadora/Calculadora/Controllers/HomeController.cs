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


      /// <summary>
      /// invocação da View inicial do nosso projeto
      /// </summary>
      /// <returns></returns>
      [HttpGet]
      public IActionResult Index() {

         // inicializar o primeiro valor do 'visor'
         ViewBag.Visor = "0";

         return View();
      }


      /// <summary>
      /// Apresentação da calculadora
      /// </summary>
      /// <param name="visor">apresenta os números escritos na calculadora e o resultado das operações realizadas</param>
      /// <param name="bt">recolhe a escolha do utilizador perante os diversos botões da calculadora</param>
      /// <param name="primeiroOperando">assegura o efeito de 'memória' do HTTP: guarda o 'primeiro operando' necessário para as operações</param>
      /// <param name="operador">assegura o efeito de 'memória' do HTTP: guarda o 'operador' necessário para a operação aritmética</param>
      /// <param name="limpaVisor">especifica se o Visor deve ser limpo, ou não</param>
      /// <returns></returns>
      [HttpPost]
      public IActionResult Index(string visor, string bt, string primeiroOperando, string operador, string limpaVisor) {

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
               if (visor == "0" || limpaVisor == "sim") visor = bt;
               else visor += bt; // visor = visor + bt;

               // ativar o 'serviço' de 'memória' do HTTP
               ViewBag.PrimeiroOperando = primeiroOperando;
               ViewBag.Operador = operador;
               // assinalar q o visor não deve ser limpo
               ViewBag.LimpaVisor = "nao";
               break;

            case "+/-":
               // processar a inversão do valor no 'visor'
               visor = Convert.ToDouble(visor) * -1 + "";
               // outra hipótese, seria o processamento de strings
               // dentro do valor do  'visor' -> .StartsWith() , .Substring() , .Length

               // ativar o 'serviço' de 'memória' do HTTP
               ViewBag.PrimeiroOperando = primeiroOperando;
               ViewBag.Operador = operador;
               // assinalar q o visor não deve ser limpo
               ViewBag.LimpaVisor = "nao";
               break;

            case ",":
               // processar o separador da parte inteira da decimal
               if (!visor.Contains(",")) visor += ",";
               // ativar o 'serviço' de 'memória' do HTTP
               ViewBag.PrimeiroOperando = primeiroOperando;
               ViewBag.Operador = operador;
               // assinalar q o visor não deve ser limpo
               ViewBag.LimpaVisor = "nao";
               break;

            case "+":
            case "-":
            case "x":
            case ":":
            case "=":
               // processar os 'operadores'
               // 
               // -123,8 + 5 x

               if (operador == null) {
                  // é a primeira vez que se seleciona um 'operador'
                  // ativar o 'serviço' de 'memória' do HTTP
                  ViewBag.PrimeiroOperando = visor;
                  ViewBag.Operador = bt;
                  // dar ordem de 'limpar' o visor
                  ViewBag.LimpaVisor = "sim";
               }
               else {
                  // já carreguei pelo menos uma segunda vez no sinal de operador
                  double auxPrimeiroOperando = Convert.ToDouble(primeiroOperando);
                  double auxSegundoOperando = Convert.ToDouble(visor);

                  // excutar a operação
                  switch (operador) {
                     case "+":
                        visor = auxPrimeiroOperando + auxSegundoOperando + "";
                        break;
                     case "-":
                        visor = auxPrimeiroOperando - auxSegundoOperando + "";
                        break;
                     case "x":
                        visor = auxPrimeiroOperando * auxSegundoOperando + "";
                        break;
                     case ":":
                        visor = auxPrimeiroOperando / auxSegundoOperando + "";
                        break;
                  }
                  // ativar o 'serviço' de 'memória' do HTTP
                  ViewBag.PrimeiroOperando = visor;
                  ViewBag.Operador = bt;
                  // limpar o ecrã
                  ViewBag.LimpaVisor = "sim";
               }

               // trata o caso particular do sinal de '='
               if (bt == "=") {
                  // anular o efeito dos operadores
                  ViewBag.Operador = null;
               }
               break;

            case "C":
               // desativar o 'serviço' de 'memória' do HTTP
               ViewBag.PrimeiroOperando = null;
               ViewBag.Operador = null;
               // limpar o ecrã
               ViewBag.LimpaVisor = "sim";
               visor = "0";
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
