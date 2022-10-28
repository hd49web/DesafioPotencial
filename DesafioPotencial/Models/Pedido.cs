using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1TESTE.Models
{
    public class Pedido
    {
      
        public int Id { get; set; }
        public DateTime DataPedido { get; set; }
        public Vendedor _vendedor { get; set; }
        public List<Produto> Produtos  { get; set; }
        public StatusPedido Status { get; set; }
     

        public Pedido ( Vendedor vendedor , List<Produto> listaProdutos)
        {
            //this.Id = Guid.NewGuid();
            this.Id = Id;
            this.DataPedido = DateTime.Now;
            this._vendedor = vendedor;
            this.Produtos = listaProdutos;
            this.Status = StatusPedido.AguardandoPagamento;

        }
         public bool AtualizarPagamento(StatusPedido status)
        {
            this.Status = status;
            return true;
        }

    }
}

