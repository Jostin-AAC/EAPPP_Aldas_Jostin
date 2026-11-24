using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Gestor_Rápido_de_Pedidos_con_Catálogo
{
    public partial class Form1 : Form
    {
        private const decimal RECARGO_ENTREGA = 0.50m;
        private const decimal EXTRA_POSTRE = 1.00m;

        private decimal precioSeleccionado = 0.00m;

        private bool reiniciando = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            reiniciando = true;

            lvCatalogo.View = View.Details;
            lvCatalogo.FullRowSelect = true;
            lvCatalogo.Columns.Clear();
            lvCatalogo.Columns.Add("Producto", 185);
            lvCatalogo.Columns.Add("Precio", 75);

            lvCatalogo.Items.Clear();
            lvCatalogo.Items.Add("Empanada de queso", "1.25m");
            lvCatalogo.Items.Add("Sánduche de pollo", "2.50m");
            lvCatalogo.Items.Add("Bolón de verde mixto", "2.25m");
            lvCatalogo.Items.Add("Humita tradicional", "1.50m");
            lvCatalogo.Items.Add("Tostada con queso", "1.00m");
            lvCatalogo.Items.Add("Café pasado", "0.85m");
            lvCatalogo.Items.Add("Chocolate caliente", "1.35m");
            lvCatalogo.Items.Add("Jugo natural (naranja/mora)", "1.50m");
            lvCatalogo.Items.Add("Batido de fruta", "2.00m");
            lvCatalogo.Items.Add("Botella de agua", "0.75m");

            lvPedido.View = View.Details;
            lvPedido.FullRowSelect = true;
            lvPedido.Columns.Clear();
            lvPedido.Columns.Add("Producto", 150);
            lvPedido.Columns.Add("Cant.", 50);
            lvPedido.Columns.Add("Subtotal", 76);

            txtProducto.ReadOnly = true;
            txtProducto.Clear();
            txtCantidad.Text = "1";
            rbtnMostrador.Checked = true;
            rbtnEntrega.Checked = false;
            chkPostre.Checked = false;
            chkServilletas.Checked = false;
            btnAgregar.Enabled = false;
            btnQuitar.Enabled = false;

            lblTotal.Text = "Total: $0.00";
            lblEstado.Text = "Estado: Seleccione un producto.";

            reiniciando = false;

        }

        private void lvCatalogo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvCatalogo.SelectedItems.Count > 0)
            {
                ListViewItem fila = lvCatalogo.SelectedItems[0];
                string nombreProducto = fila.Text;
                precioSeleccionado = (decimal)fila.Tag;

                txtProducto.Text = nombreProducto;

                lblEstado.Text = "Estado: Producto seleccionado - " + nombreProducto +
                    " ($" + precioSeleccionado.ToString("0.00", CultureInfo.InvariantCulture) + ").";
            }
            else
            {
                precioSeleccionado = 0.00m;
                txtProducto.Clear();
                btnAgregar.Enabled = false;
            }
        }

        private void txtCliente_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtCantidad_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void rbtnMostrador_CheckedChanged(object sender, EventArgs e)
        {
            if (reiniciando || !rbtnMostrador.Checked) return;

            lblEstado.Text = "Estado: Opción aplicada: Retiro en mostrador ($0.00).";
        }

        private void rbtnEntrega_CheckedChanged(object sender, EventArgs e)
        {
            if (reiniciando || !rbtnEntrega.Checked) return;

            lblEstado.Text = "Estado: Opción aplicada: Entrega en aula (+$0.50).";
        }

        private void chkPostre_CheckedChanged(object sender, EventArgs e)
        {
            if (reiniciando) return;

            lblEstado.Text = chkPostre.Checked
                ? "Estado: Opción aplicada: Postre del día (+$1.00)."
                : "Estado: Opción retirada: Postre del día.";

        }

        private void chkServilletas_CheckedChanged(object sender, EventArgs e)
        {
            if (reiniciando) return;

            lblEstado.Text = chkServilletas.Checked
                ? "Estado: Opción aplicada: Servilletas / Cubiertos ($0.00)."
                : "Estado: Opción retirada: Servilletas / Cubiertos.";
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCliente.Text))
            {
                lblEstado.Text = "Error: Ingrese el nombre del cliente.";
                txtCliente.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtProducto.Text) || precioSeleccionado <= 0)
            {
                lblEstado.Text = "Error: Seleccione un producto del catálogo.";
                return;
            }

            if (!int.TryParse(txtCantidad.Text.Trim(), out int cantidad) || cantidad < 1)
            {
                lblEstado.Text = "Error: La cantidad debe ser un entero mayor o igual a 1.";
                txtCantidad.Focus();
                txtCantidad.SelectAll();
                return;
            }

            decimal subtotal = cantidad * precioSeleccionado;
            string producto = txtProducto.Text;
            string subtotalTexto = "$" + subtotal.ToString("0.00", CultureInfo.InvariantCulture);

            ListViewItem itemPedido = new ListViewItem(producto);
            itemPedido.SubItems.Add(cantidad.ToString());

            lvPedido.Items.Add(itemPedido);

            txtCantidad.Text = "1";
            lblEstado.Text = $"Estado: '{producto}' agregado (Cant: {cantidad}";
        }

        private void btnQuitar_Click(object sender, EventArgs e)
        {
            if (lvPedido.SelectedItems.Count > 0)
            {
                string productoEliminado = lvPedido.SelectedItems[0].Text;
                lvPedido.Items.Remove(lvPedido.SelectedItems[0]);

                btnQuitar.Enabled = false;
                lblEstado.Text = $"Estado: '{productoEliminado}' eliminado del pedido.";
            }

        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            reiniciando = true;

            lvPedido.Items.Clear();
            txtCliente.Clear();
            txtProducto.Clear();
            lvCatalogo.SelectedItems.Clear();

            precioSeleccionado = 0.00m;
            txtCantidad.Text = "1";
            rbtnMostrador.Checked = true;
            rbtnEntrega.Checked = false;
            chkPostre.Checked = false;
            chkServilletas.Checked = false;
            btnAgregar.Enabled = false;
            btnQuitar.Enabled = false;

            lblEstado.Text = "Estado: Pedido reiniciado.";
            txtCliente.Focus();

            reiniciando = false;

        }
        private void lvPedido_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnQuitar.Enabled = lvPedido.SelectedItems.Count > 0;
        }
    }
}
