using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _1TESTE.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace _1TESTE.Controllers
{
    [ApiController]
    [Route("Controller")]
    public class VendedorController : ControllerBase
    {
      public static List<Vendedor> ListaVendedores;
      public VendedorController()
      {
        ListaVendedores = new List<Vendedor>();
        
        var vend1 = new Vendedor();
        vend1.Idv = 201;
        vend1.CPF = "222.333.444-55";
        vend1.Nome = "PAULO";
        vend1.Email ="PAULO@GMAIL.COM";
        vend1.Telefone = "11-975009566";
        ListaVendedores.Add(vend1);

        var vend2 = new Vendedor();
        vend2.Idv = 202;
        vend2.CPF = "222.222.222-22";
        vend2.Nome = "TAMIRIS";
        vend2.Email ="TAMIRIS@GMAIL.COM";
        vend2.Telefone = "11-975000000";
        ListaVendedores.Add(vend2);  

        var vend3 = new Vendedor();
        vend3.Idv= 203;
        vend3.CPF = "333.333.333-33";
        vend3.Nome = "GEOVANI";
        vend3.Email ="GEOVANI@GMAIL.COM";
        vend3.Telefone = "11-975000001";
        ListaVendedores.Add(vend3);  

      }  
        [HttpGet("Regsitro de todos os Vendedores")]
        public IActionResult Index()
        {
            return Ok(ListaVendedores);
        }

    }
}