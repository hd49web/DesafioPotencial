using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using _1TESTE.Models;
using Microsoft.EntityFrameworkCore;


namespace _1TESTE.Controllers
{
    [ApiController]
    [Route("Controller")]
    public class ProdutoController : ControllerBase
    {

    public static List<Produto> ListaProduto;
    public ProdutoController()
    {
        ListaProduto = new List<Produto>();

        var prod1 = new Produto();
        prod1.Idp = 001;
        prod1.Nome = "bola";
        prod1.Valor = 98.00M;
        ListaProduto.Add(prod1);

        var prod2 = new Produto();
        prod2.Idp = 002;
        prod2.Nome = "Camisa Seleção Brasileira";
        prod2.Valor = 108.00M;
        ListaProduto.Add(prod2);
        
        var prod3 = new Produto();
        prod3.Idp = 003;
        prod3.Nome = "Albun de Figurinha da Compa do Mundo";
        prod3.Valor = 20.00M;
        ListaProduto.Add(prod3);
    }    
        [HttpGet("Registro de todos os Produtos")]
        public IActionResult Index()
        {
            return Ok(ListaProduto);
        }
    }
}