
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TestProject.Dialogs;
using TestProject.Models;
using TestProject.Services;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TestProject.Forms
{
    public partial class supplierForm : Form
    {
        private SupplierService _supplierService;

        public supplierForm()
        {
            InitializeComponent();
            _supplierService = new SupplierService();
            SetupSupplierGrid();
            LoadSuppliers();
            textBox1.TextChanged += TextBox1_TextChanged;
            BtnAddSupplier.Click += BtnAddSupplier_Click;
        }

        private void SetupSupplierGrid()
        {
            if (dataGridView1 == null)
            {
                dataGridView1 = new DataGridView();
                dataGridView1.Dock = DockStyle.Fill;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.MultiSelect = false;
                dataGridView1.BackgroundColor = System.Drawing.Color.White;
                dataGridView1.BorderStyle = BorderStyle.None;
                panel2.Controls.Add(dataGridView1);
            }

            if (dataGridView1.Columns.Count == 0)
            {
                dataGridView1.Columns.Add("id", "ID");
                dataGridView1.Columns.Add("name", "Supplier Name");
                dataGridView1.Columns.Add("contactPerson", "Contact Person");
                dataGridView1.Columns.Add("email", "Email");
                dataGridView1.Columns.Add("phone", "Phone");
                dataGridView1.Columns.Add("address", "Address");
                dataGridView1.Columns.Add("action", "Actions");

                dataGridView1.Columns["id"].Width = 50;
                dataGridView1.Columns["name"].Width = 180;
                dataGridView1.Columns["contactPerson"].Width = 120;
                dataGridView1.Columns["email"].Width = 150;
                dataGridView1.Columns["phone"].Width = 100;
                dataGridView1.Columns["address"].Width = 180;
                dataGridView1.Columns["action"].Width = 80;
            }

            dataGridView1.CellClick += DataGridView1_CellClick;
        }

        private void LoadSuppliers()
        {
            dataGridView1.Rows.Clear();
            var suppliers = _supplierService.GetAllSuppliers();

            foreach (var supplier in suppliers)
            {
                if (supplier.IsActive)
                {
                    int rowIndex = dataGridView1.Rows.Add(
                        supplier.Id,
                        supplier.Name,
                        supplier.ContactPerson,
                        supplier.Email,
                        supplier.Phone,
                        supplier.Address,
                        "Edit"
                    );
                    dataGridView1.Rows[rowIndex].Tag = supplier.Id;
                }
            }
        }

        private void BtnAddSupplier_Click(object sender, EventArgs e)
        {
            
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView1.Columns["action"].Index)
            {
                var supplierId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Tag);
                var supplier = _supplierService.GetSupplierById(supplierId);

                if (supplier != null)
                {
                    ShowSupplierManagementMenu(supplier, e);
                }
            }
        }

        private void ShowSupplierManagementMenu(Supplier supplier, DataGridViewCellEventArgs e)
        {
            var contextMenu = new ContextMenuStrip();

            var editItem = new ToolStripMenuItem("Edit Supplier");
            var deleteItem = new ToolStripMenuItem("Delete Supplier");
            var viewItem = new ToolStripMenuItem("View Details");

            editItem.Click += (s, ev) => EditSupplier(supplier);
            deleteItem.Click += (s, ev) => DeleteSupplier(supplier);
            viewItem.Click += (s, ev) => ViewSupplierDetails(supplier);

            contextMenu.Items.Add(viewItem);
            contextMenu.Items.Add(editItem);
            contextMenu.Items.Add(deleteItem);

            var cellRect = dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            contextMenu.Show(dataGridView1, cellRect.Location);
        }

        private void EditSupplier(Supplier supplier)
        {
            var dialog = new SupplierDialog(supplier);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var updatedSupplier = dialog.GetSupplier();
                updatedSupplier.Id = supplier.Id;
                _supplierService.UpdateSupplier(updatedSupplier);
                LoadSuppliers();
                MessageBox.Show("Supplier updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void DeleteSupplier(Supplier supplier)
        {
            var result = MessageBox.Show($"Are you sure you want to delete supplier '{supplier.Name}'?\nThis will mark the supplier as inactive.",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                _supplierService.DeleteSupplier(supplier.Id);
                LoadSuppliers();
                MessageBox.Show("Supplier deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ViewSupplierDetails(Supplier supplier)
        {
            MessageBox.Show($"Supplier Details:\n\nName: {supplier.Name}\nContact: {supplier.ContactPerson}\nEmail: {supplier.Email}\nPhone: {supplier.Phone}\nMobile: {supplier.Mobile}\nAddress: {supplier.Address}\nTax ID: {supplier.TaxID}",
                "Supplier Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            string searchText = textBox1.Text;
            dataGridView1.Rows.Clear();

            var suppliers = _supplierService.SearchSuppliers(searchText);

            foreach (var supplier in suppliers)
            {
                if (supplier.IsActive)
                {
                    int rowIndex = dataGridView1.Rows.Add(
                        supplier.Id,
                        supplier.Name,
                        supplier.ContactPerson,
                        supplier.Email,
                        supplier.Phone,
                        supplier.Address,
                        "Edit"
                    );
                    dataGridView1.Rows[rowIndex].Tag = supplier.Id;
                }
            }
        }

        private void BtnAddSupplier_Click_1(object sender, EventArgs e)
        {
            var dialog = new SupplierDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                _supplierService.AddSupplier(dialog.GetSupplier());
                LoadSuppliers();
                MessageBox.Show("Supplier added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}