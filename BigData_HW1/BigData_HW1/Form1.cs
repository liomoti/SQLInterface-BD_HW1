using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

//This program made by: moti and reut shaul 
namespace BigData_HW1
{
    public partial class Form1 : Form
    {
        bool second_datagridview = false;
        Random rnd = new Random();
        string conStr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename="+ System.Environment.CurrentDirectory+"\\DB\\BigData_HW1_DB.mdf;Integrated Security=True;Connect Timeout=30";
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_show_Click(object sender, EventArgs e)
        {
            //design settings
            panel_color.Visible = true;
            panel_color.Height = btn_show.Height;
            panel_color.Top = btn_show.Top;

            lbl_between.Visible = false;
            txtbox_max.Visible = false;
            txtbox_min.Visible = false;
            lbl_and.Visible = false;
            btn_search.Visible = false;

            lbl_header.Text = "Data:";
            //flag for another datagridview
            second_datagridview = false;

            dataGridView2.DataSource = null;
            dataGridView2.Rows.Clear();
            dataGridView2.Refresh();

            //sql settings
            SqlConnection CON = new SqlConnection(conStr);
            CON.Open();
            SqlCommand com = new SqlCommand("select tblProducts.Name,tblCategories.Name as Category, tblProducts.Stock " +
                "from tblCategories inner join tblProducts " +
                "on tblProducts.category = tblCategories.Id " +
                "order by tblProducts.Name asc;", CON);
            try
            {

                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = com;
                DataTable dbset = new DataTable();
                sda.Fill(dbset);
                BindingSource bsource = new BindingSource();

                bsource.DataSource = dbset;
                dataGridView.DataSource = bsource;
                sda.Update(dbset);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            CON.Close();
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            //design settings
            panel_color.Visible = true;
            panel_color.Height = (btn_open_search.Height) * 2;
            panel_color.Top = btn_open_search.Top;

            lbl_between.Visible = true;
            txtbox_max.Visible = true;
            txtbox_min.Visible = true;
            lbl_and.Visible = true;
            btn_search.Visible = true;

            lbl_header.Text = "Press on the table name to show its content";

            //flag for another datagridview
            second_datagridview = true;

            dataGridView2.DataSource = null;
            dataGridView2.Rows.Clear();
            dataGridView2.Refresh();

            //sql settings
            SqlConnection CON3 = new SqlConnection(conStr);
            CON3.Open();
            SqlCommand com3 = new SqlCommand("SELECT * FROM Sys.Tables", CON3);
            try
            {

                SqlDataAdapter sda3 = new SqlDataAdapter();
                sda3.SelectCommand = com3;
                DataTable dbset3 = new DataTable();
                sda3.Fill(dbset3);
                BindingSource bsource3 = new BindingSource();

                bsource3.DataSource = dbset3;
                dataGridView.DataSource = bsource3;
                for (int i = 1; i < dataGridView.Columns.Count; i++)
                {
                    dataGridView.Columns[i].Visible = false;
                }
                //dataGridView.Columns[0].Visible = true;
                sda3.Update(dbset3);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            CON3.Close();
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            

            dataGridView2.DataSource = null;
            dataGridView2.Rows.Clear();
            dataGridView2.Refresh();

            int min_stock = 0;
            int max_stock = 0;
            //checking if the user input is only numbers
            if (int.TryParse(txtbox_min.Text.ToString(), out min_stock)
                && int.TryParse(txtbox_max.Text.ToString(), out max_stock))
            {
                //checking if the user input is reasonable
                if (min_stock >= max_stock)
                {
                    MessageBox.Show("Please enter reasonable input");
                }
                else
                {
                    lbl_header.Text = "Press again on SEARCH PRODUCTS button to show the updates";
                    int tbl_number = rnd.Next(1, 9999); // creates a number between 1 and 9999
                    //sql settings
                    SqlConnection CON2 = new SqlConnection(conStr);
                    CON2.Open();
                    SqlCommand com2 = new SqlCommand("select tblProducts.Name as Product_Name, tblCategories.Name as Category " +
                        "into tbl_tmp" + tbl_number + " " +
                        "from tblProducts inner join tblCategories " +
                        "on tblProducts.category = tblCategories.Id " +
                        "where tblProducts.Stock between " + min_stock + " and " + max_stock + " ; ", CON2);
                    try
                    {

                        SqlDataAdapter sda2 = new SqlDataAdapter();
                        sda2.SelectCommand = com2;
                        DataTable dbset2 = new DataTable();
                        sda2.Fill(dbset2);
                        BindingSource bsource2 = new BindingSource();

                        bsource2.DataSource = dbset2;
                        dataGridView.DataSource = bsource2;
                        sda2.Update(dbset2);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    CON2.Close();
                    foreach (DataGridViewColumn column in dataGridView.Columns)
                    {
                        column.SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                }
            }
            else
            {
                //error message
                MessageBox.Show("Please enter only numbers");
                txtbox_min.Text = "";
                txtbox_max.Text = "";
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {


            //sql settings
            SqlConnection CON3 = new SqlConnection(conStr);
            CON3.Open();
            SqlCommand com3 = new SqlCommand("SELECT * FROM Sys.Tables", CON3);
            try
            {

                SqlDataAdapter sda3 = new SqlDataAdapter();
                sda3.SelectCommand = com3;
                DataTable dbset3 = new DataTable();
                sda3.Fill(dbset3);
                BindingSource bsource3 = new BindingSource();

                bsource3.DataSource = dbset3;
                dataGridView.DataSource = bsource3;
                for (int i = 1; i < dataGridView.Columns.Count; i++)
                {
                    dataGridView.Columns[i].Visible = false;
                }
                //dataGridView.Columns[0].Visible = true;
                sda3.Update(dbset3);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            CON3.Close();
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (second_datagridview)
            {
                if (e.RowIndex >= 0)
                {
                    string tbl_name = dataGridView.Rows[e.RowIndex].Cells[0].Value.ToString();
                    //MessageBox.Show(dataGridView.Rows[e.RowIndex].Cells[0].Value.ToString());
                    SqlConnection CON4 = new SqlConnection(conStr);
                    CON4.Open();
                    SqlCommand com4 = new SqlCommand("SELECT * from " + tbl_name + " ;", CON4);
                    try
                    {
                        SqlDataAdapter sda4 = new SqlDataAdapter();
                        sda4.SelectCommand = com4;
                        DataTable dbset4 = new DataTable();
                        sda4.Fill(dbset4);
                        BindingSource bsource4 = new BindingSource();
                        bsource4.DataSource = dbset4;
                        dataGridView2.DataSource = bsource4;
                        sda4.Update(dbset4);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    CON4.Close();
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
    }
}
