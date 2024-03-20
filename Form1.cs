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
        // Vamos criar as informações para conectar nosso banco
        private MySqlConnection Conexao;
        private string data_source = "datasource=localhost;username=root;password=;database=db_agenda";
        public Form1()
        {
            InitializeComponent();

            //criando as linhas e colunas
            lst_contatos.View = View.Details;
            lst_contatos.AllowColumnReorder = true;
            lst_contatos.FullRowSelect = true;
            lst_contatos.GridLines = true;

            //colocando as informações na linha e coluna
            lst_contatos.Columns.Add("ID", 30, HorizontalAlignment.Left);
            lst_contatos.Columns.Add("Nome", 150, HorizontalAlignment.Left);
            lst_contatos.Columns.Add("Email", 150, HorizontalAlignment.Left);
            lst_contatos.Columns.Add("Telefone", 150, HorizontalAlignment.Left);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtNome_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {


                //Criar a conexão com mysql
                Conexao = new MySqlConnection(data_source);

                //Abrir a conexão
                Conexao.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = Conexao;

                cmd.CommandText = "INSERT INTO contato(nome,email,telefone) VALUES (@nome,@email,@telefone)";

                cmd.Prepare();

                //definir os parametros que vao pro banco 
                cmd.Parameters.AddWithValue("@nome", txtNome.Text);
                cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@telefone", txtTelefone.Text);

                cmd.ExecuteNonQuery();
                //Exibindo a mensagem ao usuario
                MessageBox.Show("Contato inserido com sucesso", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Executar o comando insert
                /*
                string sql = $"INSERT INTO contato (nome,email,telefone)" +
                             "VALUES" +
                             "('" + txtNome.Text + "','" + txtTelefone.Text + "','" + txtEmail.Text + "')";
                MySqlCommand comando = new MySqlCommand(sql, Conexao);
                Conexao.Open();
                //Executar comando sql
                comando.ExecuteReader();
                //Informar mensagem caso tenha dado certo
                MessageBox.Show("Cadastro inserido com sucesso!");
            }
                 */
            catch (Exception ex)
            {
                MessageBox.Show("Erro Ocorreu " + ex.Message , " Erro " , ex.Message,
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
               
            }
            finally
            {
                Conexao.Close();
            }

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string q = " '%" + txt_buscar.Text + "%' ";
                Conexao = new MySqlConnection(data_source);

                string sql = "SELECT * FROM contato WHERE nome LIKE" + q + "OR email LIKE" + q;
                //SELECT * FROM CONTATO WHERE NOME LIKE %JOSE%
                Conexao.Open();

                //executar comando
                MySqlCommand comando = new MySqlCommand(sql, Conexao);

                //usando o mysqldatareader para executar
                MySqlDataReader reader = comando.ExecuteReader();

                lst_contatos.Clear();
                while (reader.Read())
                {
                    string[] row =
                    {
                        reader.GetInt16(0).ToString(),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3),
                    };

                    /*vamos montar um elemento windowsform que será nossa linha */
                    var linhaListView = new ListViewItem(row);
                    /*em seguida pegamos o ListView e acrescentamos essa
                     linha que acabamos de criar*/
                    lst_contatos.Items.Add(linhaListView);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Conexao.Close();
            }
        }
    }
}