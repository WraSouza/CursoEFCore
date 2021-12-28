using System;
using System.Collections.Generic;
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

            RemoverRegistro();
            //AtualizarDados();
            //ConsultarPedidoCarregamentoAdiantado();
            //CadastrarPedido();
            //ConsultarDados();
            //InserirdadosEmMassa();
            //InserirDados();
        }

        private static void RemoverRegistro()
        {
            using var db = new Data.ApplicationContext();

            var cliente = db.Clientes.Find(2);

            //Opção 1 para remover
            //db.Clientes.Remove(cliente);

            //Opção 2 para remover
            db.Remove(cliente);

            //Opção 3 para remover
            //db.Entry(cliente).State =  EntityState.Deleted;

            db.SaveChanges();
        }

        private static void AtualizarDados()
        {
            using var db = new Data.ApplicationContext();
            var cliente = db.Clientes.Find(1);
            cliente.Nome = "Cliente Alteração Passo 2";

            //Com esse método ele irá "atualizar" todos os atributos porém não é necessário pois apenas alguns campos foram alterados.
            //O mais correto seria não utilizar o update pois ele rastreia quais campos foram alterados
            //db.Clientes.Update(cliente);
            db.SaveChanges();
        }

        private static void ConsultarPedidoCarregamentoAdiantado()
        {
            using var db = new Data.ApplicationContext();
            //Para Carregar os relacionamentos deve-se inserir o Include e a propriedade de navegação que se deseja
            var pedidos = db.Pedidos
            .Include(p => p.Itens)
            .ThenInclude(p => p.Produto)
            .ToList();

            Console.WriteLine(pedidos.Count);
        }

        private static void CadastrarPedido()
        {
            using var db = new Data.ApplicationContext();
            var cliente = db.Clientes.FirstOrDefault();
            var produto = db.Produtos.FirstOrDefault();

            var pedido = new Pedido
            {
                ClienteId = cliente.Id,
                IniciadoEm = DateTime.Now,
                FinalizadoEm = DateTime.Now,
                Observacao = "Pedido Teste",
                Status = StatusPedido.Analise,
                TipoFrete = TipoFrete.SemFrete,
                Itens = new List<PedidoItem>
                {
                    new PedidoItem
                    {
                        ProdutoId = produto.Id,
                        Desconto = 0,
                        Quantidade = 1,
                        Valor = 10,
                    }
                }
            };

            db.Pedidos.Add(pedido);
            db.SaveChanges();
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
