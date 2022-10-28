using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using _1TESTE.Models;

namespace _1TESTE.Context
{
    public class PottencialContext : DbContext
    {
        public PottencialContext(DbContextOptions<PottencialContext> options) : base(options)
        {

        }
        public DbSet<Pedido> Pedidos {get; set;}
        public DbSet<Vendedor> Vendedores {get; set;}
        public DbSet<Produto> Produtos {get; set; }
    }
    
}