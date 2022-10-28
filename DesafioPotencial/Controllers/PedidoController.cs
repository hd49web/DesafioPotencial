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
    public class PedidoController : ControllerBase
    {
        public static List<Pedido> ListaPedidos;

        public PedidoController()
        {
            if (ListaPedidos == null)
            {
                //criando um pedido padrão (exemplo)
                ListaPedidos = new List<Pedido>();

                //chamando as controllers para criar o pedido padrão
                var PedidoController = new PedidoController();
                var vendedorController = new VendedorController();
                var produtoController = new ProdutoController();
                
                //var pedido = PedidoController.ListaPedidos.Find(x => x.Id == int id++);
                var vendedor = VendedorController.ListaVendedores.Find(x => x.CPF == "222.333.444-55");
                var produto = ProdutoController.ListaProduto.Find(x => x.Nome == "bola");
          
                var listaProd = new List<Produto>();
                listaProd.Add(produto);
                var pedidos = new Pedido(vendedor, listaProd);
                ListaPedidos.Add(pedidos);
            }
        }

        [HttpGet("Registro de todas as Vendas")]
        public IActionResult Index()
        {
            if (ListaPedidos.Count() == 0)
                return Ok("Não há pedidos realizados!");

            return Ok(ListaPedidos);
        }

        [HttpPost("Registrar Venda com CPF do Vendedor  e ID Produto")]
        public IActionResult Create(string cpfVendedor, int Idp )
        {
            //Verificando se o vendedor existe//
            var vendedor = VendedorController.ListaVendedores.Find(x => x.CPF == cpfVendedor);
            if (vendedor == null)
                return NotFound();
            // //verificando se existe pelo menos 1 produto na lista
            var listaProdutos = new List<Produto>();
            {
                var produto = ProdutoController.ListaProduto.Find(x => x.Idp == Idp);
                if (produto == null)
                //retornar um erro caso um produto não exista ou for inserido de maneira errada
                return NotFound($"O produto com id {Idp} não existe!");
                listaProdutos.Add(produto);
            }
           

            var pedido = new Pedido( vendedor, listaProdutos );
            string totalPedido = pedido.Produtos.Sum(x => x.Valor).ToString("C");
            
           

            ListaPedidos.Add(pedido);
               
            return Ok($"Pedido Criado com sucesso! O código dele é '{pedido.Id}' e o valor total dos itens é de {totalPedido}");
        }

        [HttpGet("Encontrar o Pedido de Venda por ID Pedido")]
        public IActionResult FindById(int pedido)
        {
            var ret = ListaPedidos.Find(x => x.Id == pedido);
            if (ret == null)
                return NotFound("Você não informou um pedido válido");

            return Ok(ret);
        }

        [HttpPost("Pagamento da Venda com ID Pedido e Cartão de Credito com 16 digitos")]
        public IActionResult MakePayment(int pedido, string creditCard)
        {
            //tentando encontrar o pedido informado
            var ret = ListaPedidos.Find(x => x.Id == pedido);
            if (ret == null)
                return NotFound("Você não informou um pedido válido");

            //verificando o pedido foi cancelado
            if (ret.Status == StatusPedido.PedidoCancelado)
                return BadRequest("Você não pode fazer um pagamento de um pedido que já foi cancelado!");

            //verificando se o pedido não foi pago, transportado ou entregue
            if (ret.Status != StatusPedido.AguardandoPagamento)
                return BadRequest("Não é possível efetuar o pagamento desse pedido, pois o mesmo encontra-se pago!");

            //validando o cartão de credito
            if (string.IsNullOrEmpty(creditCard))
                return BadRequest("Favor insira um número de cartão!");

            if (creditCard.Length != 16 || !creditCard.All(char.IsDigit))
                return BadRequest("Favor insira um cartão com pelo menos 16 digitos!");

            //se tudo ocorrer bem, a atualização do pedido é feito aqui
            ret.AtualizarPagamento(StatusPedido.PagamentoAprovado);
            return Ok("Pagamento realizado com sucesso!");
        }

        [HttpPatch("Cancelar Venda por numero do ID Pedido")]
        public IActionResult CancelOrder(int pedido)
        {
            var ret = ListaPedidos.Find(x => x.Id == pedido);
            if (ret == null)
                return NotFound("Você não informou um pedido válido");

            //verificando o pedido foi cancelado
            if (ret.Status == StatusPedido.PedidoCancelado)
                return BadRequest("Você não pode cancelar um pedido que já foi cancelado!");

            //verificando o pedido foi enviado para a transportadora
            if (ret.Status == StatusPedido.EnviadoTransportadora)
                return BadRequest("Você não pode cancelar esse pedido, pois ele ja foi enviado para a transportadora!");

            //verificando o pedido foi entregue
            if (ret.Status == StatusPedido.PedidoEntregue)
                return BadRequest("Você não pode cancelar esse pedido, pois ele ja foi entregue para o cliente!");

            //cancelando pedido
            ret.AtualizarPagamento(StatusPedido.PedidoCancelado);

            return Ok("Pedido cancelado com sucesso!");
        }

        [HttpPatch("Enviar ID Pedido para Tranportadora")]
        public IActionResult SendToTransport(int pedido)
        {
            var ret = ListaPedidos.Find(x => x.Id == pedido);
            if (ret == null)
                return NotFound("Você não informou um pedido válido");

            //verificando o pedido foi cancelado
            if (ret.Status == StatusPedido.PedidoCancelado)
                return BadRequest("Você não enviar para o transporte um pedido que já foi cancelado!");

            //verificando se o pedido não tem seu pagamento efetuado
            if (ret.Status == StatusPedido.AguardandoPagamento)
                return BadRequest("Você não pode enviar para o transporte um pedido que ainda não foi efetuado o pagamento!");

            //verificando se o pedido foi enviado para a transportadora
            if (ret.Status == StatusPedido.EnviadoTransportadora)
                return BadRequest("Você já enviou esse pedido para a transportadora!");

            //verificando se o pedido já foi entregue
            if (ret.Status == StatusPedido.PedidoEntregue)
                return BadRequest("Você não pode enviar para o transporte um pedido que já foi entregue");

            //cancelando pedido
            ret.AtualizarPagamento(StatusPedido.EnviadoTransportadora);

            return Ok("Pedido enviado para a transportadora com sucesso!");
        }

        [HttpPatch("ID Pedido Entregue")]
        public IActionResult DeliveryTracking(int pedido)
        {
            var ret = ListaPedidos.Find(x => x.Id == pedido);
            if (ret == null)
                return NotFound("Você não informou um pedido válido");

            //verificando o pedido foi cancelado
            if (ret.Status == StatusPedido.PedidoCancelado)
                return BadRequest("Você não pode informar a entrega de um pedido que foi cancelado!");

            //verificando o pedido já foi entregue
            if (ret.Status == StatusPedido.PedidoEntregue)
                return BadRequest("Esse pedido já foi entregue. Não é possivel atualizar seu status.");

            //verificando o pedido foi cancelado
            if (ret.Status != StatusPedido.EnviadoTransportadora)
                return BadRequest("Esse pedido pode ter sido pago, mas ainda não foi enviado para a transportadora. Portanto, não é possível atualizar o status do pedido para 'Entregue' ");

            //atualizando o pedido como Entregue
            ret.AtualizarPagamento(StatusPedido.PedidoEntregue);

            return Ok("Status do pedido atualizado para 'Entregue'.");
        }

    } 
}