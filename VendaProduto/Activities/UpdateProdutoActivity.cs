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
using VendaProduto.Classes;

namespace VendaProduto.Activities
{
    [Activity(Label = "UpdateProdutoActivity")]
    public class UpdateProdutoActivity : AppCompatActivity
    {
        EditText edtNomeProdutoUp, edtPrecoUnitUp, edtQtdEstoqueUp;
        Button btnUpdate_up;

        List<Produto> produtos = new Produto().BuscarTodosProdutos();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_updateProduto);
            // Create your application here

            FindViewById<EditText>(Resource.Id.edtNomeProdutoUp);
            FindViewById<EditText>(Resource.Id.edtPrecoUnitUp);
            FindViewById<EditText>(Resource.Id.edtQtdEstoqueUp);
            FindViewById<Button>(Resource.Id.btnUpdate_up);

            //Recebe o objeto
            Produto upProduto = JsonConvert.DeserializeObject<Produto>(Intent.GetStringExtra("att_produto"));
            //Exibe os dados nos campos
            edtNomeProdutoUp.Text = upProduto.NomeProduto;
            edtPrecoUnitUp.Text = upProduto.PrecoUnit.ToString();
            edtQtdEstoqueUp.Text = upProduto.QtdEstocada.ToString();

            //Evento Click do botão
            //Vai pegar o conteúdo dos EditText e armazenar novamente
            //nas propriedades do objeto
            btnUpdate_up.Click += (s, e) =>
            {
                upProduto.NomeProduto = edtNomeProdutoUp.Text;
                upProduto.PrecoUnit = decimal.Parse(edtPrecoUnitUp.Text);
                upProduto.QtdEstocada = int.Parse(edtQtdEstoqueUp.Text);

                //Devolve o objeto e atualiza
                Intent voltar = new Intent();
                voltar.PutExtra("att_produto", JsonConvert.SerializeObject(upProduto));
                SetResult(Result.Ok, voltar);
                Finish();
            };
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (requestCode == 13 && resultCode == Result.Ok && data != null)
            {
                produtos = JsonConvert.DeserializeObject<List<Produto>>(data.GetStringExtra("att_produto"));
            }
            base.OnActivityResult(requestCode, resultCode, data);
        }
    }
}
