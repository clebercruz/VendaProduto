using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using VendaProduto.Adapters;
using VendaProduto.Classes;
using ToolbarV7 = Android.Support.V7.Widget.Toolbar;

namespace VendaProduto.Activities
{    
    /// <summary>
    /// Activity que irá gerenciar produtos. (CRUD)
    /// Uma lista de produtos é carregada numa list (que é adaptada e exibida numa ListView).
    /// </summary>
    [Activity()]
    public class ProdutoActivity : AppCompatActivity
    {
        ToolbarV7 tlbProduto;

        //TODO - Criar o método ItemClick e ItemLongClick do lstProdutos.
        //ItemClick: irá abrir uma NOVA ACTIVITY com os dados do produto tocado para EDIÇÃO.
        //ItemLongClick: irá perguntar ao usuário se o produto selecionado será DESATIVADO (Ativo = 0)
        ListView lstProdutos;
        List<Produto> infoProduto;
        List<Produto> produtos = new Produto().BuscarTodosProdutos();
        List<Produto> produtosFiltro;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_produto);

            tlbProduto = FindViewById<ToolbarV7>(Resource.Id.tlbProduto);
            lstProdutos = FindViewById<ListView>(Resource.Id.lstProdutos);

            SetSupportActionBar(tlbProduto);
            SupportActionBar.Title = "Gerenciar Produtos";

            //Cópia de produtos em produtosFiltro
            produtosFiltro = produtos;

            AdaptadorProdutos adp = new AdaptadorProdutos(this, produtos);
            lstProdutos.Adapter = adp;

            lstProdutos.ItemLongClick += LstProdutos_ItemLongClick;
            lstProdutos.ItemClick += LstProdutos_ItemClick;
        }

        private void LstProdutos_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //Abrir a activity que vai atualizar os dados do produto tocado
            Intent telaUpdate = new Intent(this, typeof(UpdateProdutoActivity));

            if (produtosFiltro != null)
            {
                telaUpdate.PutExtra("att_produto", JsonConvert.SerializeObject(produtosFiltro));
                telaUpdate.PutExtra("att_produto", JsonConvert.SerializeObject(produtosFiltro[e.Position]));

            }
            else
            {
                telaUpdate.PutExtra("att_produto", JsonConvert.SerializeObject(produtosFiltro));
                telaUpdate.PutExtra("att_produto", JsonConvert.SerializeObject(produtos[e.Position]));
            }

            StartActivityForResult(telaUpdate, 13);
        }

        private void LstProdutos_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            //Criar um alerta perguntando ao usuário se realmente deseja excluir
            Android.Support.V7.App.AlertDialog.Builder builder;
            builder = new Android.Support.V7.App.AlertDialog.Builder(this);

            builder.SetTitle("Atenção!");
            builder.SetMessage("Deseja realmente excluir " + produtosFiltro[e.Position].NomeProduto + "?");
            builder.SetIconAttribute(Android.Resource.Attribute.AlertDialogIcon);
            builder.SetNegativeButton("Não", delegate { });
            builder.SetPositiveButton("Sim", delegate
            {
                var match = produtos.Select((Value, Index) => new { Value, Index }).Single(p => p.Value.Id == produtosFiltro[e.Position].Id);
                string mensagem = match.Value.DesativarProduto();
                produtosFiltro.RemoveAt(e.Position);

                AdaptadorProdutos adaptador = new AdaptadorProdutos(this, produtosFiltro);
                lstProdutos.Adapter = adaptador;
                Toast.MakeText(this, mensagem, ToastLength.Short).Show();
            });
            builder.Show();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.toolbar_produto, menu);

            //Cria e configura o SearchView
            IMenuItem item = menu.FindItem(Resource.Id.tlbItem_pesquisarProduto);
            View searchView = item.ActionView as Android.Support.V7.Widget.SearchView;
            Android.Support.V7.Widget.SearchView itemPesquisar;
            itemPesquisar = searchView.JavaCast<Android.Support.V7.Widget.SearchView>();

            //Método de evento que é executado a cada letra que digitamos ou apagamos
            itemPesquisar.QueryTextChange += (s, e) =>
            {
                //Aqui iremos fazer a nossa busca!
                List<Produto> filtroDeProdutos = Produto.BuscarProduto(e.NewText, infoProduto);
                AdaptadorProdutos adaptador = new AdaptadorProdutos(this, filtroDeProdutos);
                lstProdutos.Adapter = adaptador;
            };

            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.tlbItem_addProduto:
                    Intent tlAddProd = new Intent(this, typeof(AddProdutoActivity));
                    StartActivity(tlAddProd);
                    break;
                default:
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}