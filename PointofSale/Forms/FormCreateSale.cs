﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PointofSale.Models;
using PointofSale.Util;

namespace PointofSale.Forms
{
    public partial class FormCreateSale : Form
    {
        private Inventory selected_inventory;
        private List<SalesDetail> salesDetails = new List<SalesDetail>();

        public FormCreateSale()
        {
            InitializeComponent();

            readCustomers();
            readInventory();

            initializeUI();
            initializeDataGrid();
        }

        private void initializeUI()
        {
            txQuantity.Maximum = Decimal.MaxValue;
            txPrice.Maximum = Decimal.MaxValue;
            txDiscount.Maximum = Decimal.MaxValue;
        }

        private void readCustomers()
        {
            bsCustomers.DataSource = Customer.read(null, null);
            cbCustomer.DataSource = bsCustomers;
            cbCustomer.DisplayMember = "name";
            cbCustomer.ValueMember = "id";
            cbCustomer.ResetText();
            cbCustomer.SelectedIndex = -1;
            cbCustomer.DropDownStyle = ComboBoxStyle.DropDown;
            cbCustomer.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbCustomer.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        private void readInventory()
        {
            bsInventories.DataSource = Inventory.read(null, null);
            cbInventory.DataSource = bsInventories;
            cbInventory.DisplayMember = "name";
            cbInventory.ValueMember = "id";
            cbInventory.ResetText();
            cbInventory.SelectedIndex = -1;
            cbInventory.DropDownStyle = ComboBoxStyle.DropDown;
            cbInventory.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbInventory.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        private void initializeDataGrid()
        {
            dgvItems.Columns.Clear();
            dgvItems.Refresh();
            dgvItems.AutoGenerateColumns = false;

            dgvItems.DataSource = bsSelectedItems;

            dgvItems.Columns.Add(DGVHandler.CreateTextBox("inventoryCode", "Code", "inventoryCode", false));
            dgvItems.Columns.Add(DGVHandler.CreateTextBox("inventoryName", "Name", "inventoryName", false));
            dgvItems.Columns.Add(DGVHandler.CreateTextBox("price", "Price", "price", false));
            dgvItems.Columns.Add(DGVHandler.CreateTextBox("quantity", "Quantity", "quantity", false));

            dgvItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvItems.RowHeadersVisible = false;
            dgvItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            getCurrentSelectedInventory();

            SalesDetail salesDetail = new SalesDetail();

            salesDetail.inventoryCode = selected_inventory.code;
            salesDetail.inventoryName = selected_inventory.name;
            salesDetail.inventoryId = selected_inventory.id;
            salesDetail.price = selected_inventory.price;

            validateSelection(ref salesDetail);

            salesDetails.Add(salesDetail);

            bsSelectedItems.DataSource = salesDetails;

            selected_inventory = null;
        }

        private void cbInventory_SelectionChangeCommitted(object sender, EventArgs e)
        {
            selected_inventory = (cbInventory.SelectedItem as Inventory);
            txPrice.Value = selected_inventory.price;
        }

        // Form Controls

        private void getCurrentSelectedInventory()
        {
            selected_inventory = (cbInventory.SelectedItem as Inventory);
        }

        private void validateSelection(ref SalesDetail salesDetail)
        {
            try
            {
                if (txQuantity.Value > 0)
                {
                    salesDetail.quantity = txQuantity.Value;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
