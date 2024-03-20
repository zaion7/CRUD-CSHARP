using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WFDotNetCoreGravarDadosMySQL
{
    public partial class Form1 : Form
    {

        // aqui vamos criat as informações para conectar nosso banco
        private MySqlConnection Conexao;
        private string data_source = "datasource=localhost;username=root;password=;database=db_agenda";


        private int? id_contato_selecionado = null;


        public Form1()
        {
            InitializeComponent();

            //criando as linhas e as colunas
            lst_contatos.View = View.Details;
            lst_contatos.AllowColumnReorder = true;
            lst_contatos.FullRowSelect = true;
            lst_contatos.GridLines = true;

            //colocando as informações na linha e coluna
            lst_contatos.Columns.Add("ID", 30, HorizontalAlignment.Left);
            lst_contatos.Columns.Add("Nome", 150, HorizontalAlignment.Left);
            lst_contatos.Columns.Add("Email", 150, HorizontalAlignment.Left);
            lst_contatos.Columns.Add("Telefone", 150, HorizontalAlignment.Left);

            //chamando a função carregar contatos
            CarregarContatos();


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                //criar a conexão com mysql
                Conexao = new MySqlConnection(data_source);

                Conexao.Open();

                MySqlCommand cmd = new MySqlCommand();

                cmd.Connection = Conexao;



                if (id_contato_selecionado == null)
                {
                    cmd.CommandText = "INSERT INTO contato (nome,email,telefone) VALUES (@nome,@email,@telefone)";


                    //definir os parametros que vao pro banco
                    cmd.Parameters.AddWithValue("@nome", txtNome.Text);
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@telefone", txtTelefone.Text);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();

                    //exibindo mensagem ao usuario
                    MessageBox.Show("Contato inserido com sucesso", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    cmd.CommandText = "UPDATE contato SET nome=@nome, email=@email,telefone=@telefone WHERE id=@id";
                    cmd.Parameters.AddWithValue("@nome", txtNome.Text);
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@telefone", txtTelefone.Text);
                    cmd.Parameters.AddWithValue("@id", id_contato_selecionado);

                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Contato atualizado com sucesso", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }



                txtNome.Text = string.Empty;
                txtEmail.Text = "";
                txtTelefone.Text = "";


                CarregarContatos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro Ocorreu: " + ex.Message, " Erro: ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Conexao.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {

                Conexao = new MySqlConnection(data_source);
                Conexao.Open();

                MySqlCommand cmd = new MySqlCommand();

                cmd.Connection = Conexao;

                cmd.CommandText = "SELECT * FROM contato WHERE nome LIKE @q OR email LIKE @q";

                cmd.Parameters.AddWithValue("@q", "%" + txt_buscar.Text + "%");

                cmd.Prepare();

                MySqlDataReader reader = cmd.ExecuteReader();


                lst_contatos.Items.Clear();

                while (reader.Read())
                {
                    string[] row =
                    {
                        reader.GetInt16(0).ToString(),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3),
                    };

                    lst_contatos.Items.Add(new ListViewItem(row));
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro Ocorreu: " + ex.Message, " Erro: ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Conexao.Close();
            }
        }

        private void CarregarContatos()
        {
            Conexao = new MySqlConnection(data_source);
            Conexao.Open();

            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = Conexao;

            cmd.CommandText = "SELECT * FROM contato ORDER BY id desc";

            cmd.Prepare();

            MySqlDataReader reader = cmd.ExecuteReader();


            lst_contatos.Items.Clear();

            while (reader.Read())
            {
                string[] row =
                {
                        reader.GetInt16(0).ToString(),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3),
                    };

                lst_contatos.Items.Add(new ListViewItem(row));
            }
        }

        private void lst_contatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection itens_selecionados = lst_contatos.SelectedItems;

            foreach (ListViewItem item in itens_selecionados)
            {
                id_contato_selecionado = Convert.ToInt32(item.SubItems[0].Text);

                txtNome.Text = item.SubItems[1].Text;
                txtEmail.Text = item.SubItems[2].Text;
                txtTelefone.Text = item.SubItems[3].Text;

                MessageBox.Show("ID Selecionado = " + id_contato_selecionado);

            }
        }

        private void lst_contatos_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult conf = MessageBox.Show("Tem certeza que deseja excluir ?",
                   "Excluir Registro", MessageBoxButtons.YesNo);

                if ( conf == DialogResult.Yes)
                {
                    // Vamos colocar lógica do código aqui dentro do IF 
                    Conexao = new MySqlConnection(data_source);
                    Conexao.Open();

                    MySqlCommand cmd = new MySqlCommand();

                    cmd.Connection = Conexao;

                    cmd.CommandText = "DELETE FROM contato WHERE id=@id";
                    cmd.Parameters.AddWithValue("@id", id_contato_selecionado);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Contato excluido com sucesso!",
                        "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    CarregarContatos();
                }

                else
                {

                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            id_contato_selecionado = null;

            txtNome.Text = String.Empty;
            txtEmail.Text = "";
            txtTelefone.Text = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
