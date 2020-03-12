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
using VendaProduto.Classes;

namespace VendaProduto.Activities
{
    [Activity()]
    public class AddProdutoActivity : AppCompatActivity
    {
        EditText edtNomeProduto, edtPrecoUnit, edtQtdEstoque;
        Button btnCadastrarProduto;

        List<Produto> ListaProdutos = new List<Produto>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_addProduto);

            edtNomeProduto = FindViewById<EditText>(Resource.Id.edtNomeProduto);
            edtPrecoUnit = FindViewById<EditText>(Resource.Id.edtPrecoUnit);
            edtQtdEstoque = FindViewById<EditText>(Resource.Id.edtQtdEstoque);
            btnCadastrarProduto = FindViewById<Button>(Resource.Id.btnCadastrarProduto);

            //TODO - O botão cadastrar irá inserir um novo produto no banco.
            btnCadastrarProduto.Click += BtnCadastrarProduto_Click;

        }

        private void BtnCadastrarProduto_Click(object sender, EventArgs e)
        {
            Produto newproduto = new Produto(edtNomeProduto.Text, decimal.Parse(edtPrecoUnit.Text), int.Parse(edtQtdEstoque.Text), 1);
            newproduto.Id = newproduto.IncluirProduto();
            ListaProdutos.Add(newproduto);
            Toast.MakeText(this, "Item Adicionado", ToastLength.Long).Show();
        }
    }
}