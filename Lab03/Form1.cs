using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Lab03
{
    public partial class Form1 : Form
    {
        SqlConnection conn;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnConectar_Click(object sender, EventArgs e)
        {
            String servidor = txtServidor.Text;
            String bd = txtBaseDatos.Text;
            String user = txtUsuario.Text;
            String pwd = txtPassword.Text;

            String str = $"Server={servidor};Database={bd};";

            if (chkAutenticacion.Checked)
                str += "Integrated Security=true";
            else
                str += $"User Id={user};Password={pwd};";
            try
            {
                conn = new SqlConnection(str);
                conn.Open();
                MessageBox.Show("Conectado Satisfacctoriamente");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar el servidor: \n"+ex.ToString());
            }

        }

        private void btnEstado_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == ConnectionState.Open)
                    MessageBox.Show($"Estado del servidor:{conn.State}\nVersión del Servidor:{conn.ServerVersion}\nBase de datos:{conn.Database}");
                else
                    MessageBox.Show($"Estado del servidor:{conn.State}");
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Imposible Determinar el estado del servidor: \n {ex.ToString()}");
            }
        }

        private void btnDesconectar_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                    MessageBox.Show("Conexión cerrada Satisfactoriamente");
                }
                else
                    MessageBox.Show("La Conexión ya está cerrada");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al cerrar la conexión: \n{ex.ToString()}");
            }
        }

        private void chkAutenticacion_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAutenticacion.Checked)
            {
                txtUsuario.Enabled = false;
                txtPassword.Enabled = false;
            }
            else
            {
                txtUsuario.Enabled = true;
                txtPassword.Enabled = true;
            }
        }

        private void btnPersona_Click(object sender, EventArgs e)
        {
            Persona persona = new Persona(conn);
            persona.Show();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            String usuario = txtUsuario.Text;
            String password = txtPassword.Text;

            String sql = "Select * from tbl_usuario where usuario_nombre = '" + usuario + "' and usuario_password='" + password + "'";
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(reader);
            if (dt.Rows[0][0].ToString() == "1")
            {
                MessageBox.Show("Inicio de Sesión Exitosa");
            }
            else
            {
                MessageBox.Show("Usuario y/o contraseña incorrecta");
            }
        }
    }
}
