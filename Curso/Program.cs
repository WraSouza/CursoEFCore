using System;
using System.Linq;
using Curso.Domain;
using Curso.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CursoEFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            // using var db = new Data.ApplicationContext();

            // var existe = db.Database.GetPendingMigrations().Any();

            // if(existe)
            // {
                
            // }

            ConsultarDados();
            //InserirdadosEmMassa();
            //InserirDados();
        }

        private static void ConsultarDados()
        {
            
            using var db = new Data.ApplicationContext();

            //Consulta por Sintaxe. Pode ser feita assim também.
            //var consultaPorSintaxe = (from c in db.Clientes where c.Id>0 select c).ToList();

            //Consulta Por Método. Se colocar o método AsNoTracking ele não busca na memória e sim direto no banco.
            //var consultaPorMetodo = db.Clientes.AsNoTracking().Where(p => p.Id > 0).ToList();
            var consultaPorMetodo = db.Clientes.Where(p => p.Id > 0).ToList();

            foreach(var clientes in consultaPorMetodo)
            {
                Console.WriteLine($"Consultando Cliente: {clientes.Id}");
                //Busca primeiro na memória e caso não encontre, vai buscar no banco de dados
                //Somente o método FIND busca em memória. Os outros buscam no banco de dados.
                //db.Clientes.Find(clientes.Id);
                db.Clientes.FirstOrDefault(p => p.Id == clientes.Id);
            }
        }

        private static void InserirdadosEmMassa()
        {
            var produto = new Produto
            {
              Descricao = "Produto Teste",
              CodigoBarras = "1234567891231",
              Valor = 10,
              TipoProduto = TipoProduto.MercadoriaParaRevenda,
              Ativo = true  
            };

            var cliente = new Cliente
            {
                Nome = "Wladimir",
                CEP = "99999999",
                Cidade = "Santo Ângelo",
                Estado = "RS",
                Telefone = "999999999",
                
            };

            var listaClientes = new[]
            {
                new Cliente
                {
                    Nome = "Fabiana",
                CEP = "99999999",
                Cidade = "Santo Ângelo",
                Estado = "RS",
                Telefone = "999999999",
                },

                new Cliente
                {
                    Nome = "Maria Luiza",
                CEP = "99999999",
                Cidade = "Santo Ângelo",
                Estado = "RS",
                Telefone = "999999999",
                },
            };


            using var db = new Data.ApplicationContext();
            //db.AddRange(produto, cliente);

            db.Clientes.AddRange(listaClientes);

            var registros = db.SaveChanges();
            Console.WriteLine($"Total Registro(s): {registros}");
        }

        private static void InserirDados()
        {
            var produto = new Produto
            {
              Descricao = "Produto Teste",
              CodigoBarras = "1234567891231",
              Valor = 10,
              TipoProduto = TipoProduto.MercadoriaParaRevenda,
              Ativo = true  
            };

            using var db = new Data.ApplicationContext();
            //db.Produtos.Add(produto);
            //db.Set<Produto>().Add(produto);
            //db.Entry(produto).State = EntityState.Added;
            db.Add(produto);

            var registros = db.SaveChanges();
            Console.WriteLine($"Total Registro(s): {registros}");
        }
    }
}
