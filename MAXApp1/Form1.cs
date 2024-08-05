using Dapper;
using Microsoft.Win32.SafeHandles;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;

namespace MAXApp1
{
    public partial class Form1 : Form
    {
        public string ConnString { get; set; }
        private BindingList<itemsview> itemsList = new BindingList<itemsview>();
        public Form1()
        {
            InitializeComponent();
            ConnString = "Server=localhost;Database=BL;User Id=SYSADM;Password=SYSADM;";
            // ���U SelectedIndexChanged �ƥ�
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            this.Load += new EventHandler(Form1_Load);
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            // �]�m���Y
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));

            tableLayoutPanel1.Controls.Add(new Label() { Text = "���u�s��", Font = new Font("Arial", 10, FontStyle.Bold) }, 0, 0);
            tableLayoutPanel1.Controls.Add(new Label() { Text = "����m�W", Font = new Font("Arial", 10, FontStyle.Bold) }, 1, 0);
            tableLayoutPanel1.Controls.Add(new Label() { Text = "�ͤ�", Font = new Font("Arial", 10, FontStyle.Bold) }, 2, 0);
            tableLayoutPanel1.Controls.Add(new Label() { Text = "�����Ҹ�", Font = new Font("Arial", 10, FontStyle.Bold) }, 3, 0);

            // �]�m��ؼ˦�
            tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            // �ھڻݭn�]�m TableLayoutPanel ����L�ݩ�
            tableLayoutPanel1.AutoSize = true;
            //tableLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowOnly;
            // �����ƾڥ[��
            //List<itemsview> items = new List<itemsview>
        //{
        //    new itemsview { Id = "1", Name = "Item1", Description = "Description1", MarketValue = 100, Quantity = 10, Type = "Type1", LastUpdated = DateTime.Now },
        //   new itemsview { Id = "2", Name = "Item2", Description = "Description2", MarketValue = 200, Quantity = 20, Type = "Type2", LastUpdated = DateTime.Now }
        //};

            //dataGridView1.DataSource = items;
            // ��l�� DataGridView ����Ʒ�
            dataGridViewItems.DataSource = itemsList;
            // 3. �M�� dataGridView1 ������ơA���O�d�C�]�m
            dataGridViewItems.Rows.Clear();
            // ���]����k�q��Ʈw����� items �ê�l�� itemsList
            LoadItemsFromDatabase();
        }

        private void LoadItemsFromDatabase()
        {
            using (SqlConnection conn = new SqlConnection(ConnString))
            {

                //  dataGridView1 ������ơA���O�d�C�]�m
                dataGridViewItems.Rows.Clear();
                conn.Open();
                var items = conn.Query<itemsview>("SELECT * FROM Items").ToList();

                foreach (var item in items)
                {
                    itemsList.Add(item);
                }
                conn.Close();
                dataGridViewItems.Columns["Id"].HeaderText = "ID";
                dataGridViewItems.Columns["Name"].HeaderText = "�W��";
                dataGridViewItems.Columns["Description"].HeaderText = "�y�z";
                dataGridViewItems.Columns["MarketValue"].HeaderText = "��������";
                dataGridViewItems.Columns["Quantity"].HeaderText = "�ƶq";
                dataGridViewItems.Columns["Type"].HeaderText = "����";
                dataGridViewItems.Columns["LastUpdated"].HeaderText = "�̫��s";
            }
            dataGridViewItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }


        private void btQuit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }


        private void btSave_Click(object sender, EventArgs e)
        {

            // �����J���
            string employeCd = dfEmployeCd.Text;
            string chineseName = dfChineseName.Text;
            string birthday = dfBirthday.Text;
            string idNumber = dfIDNumber.Text;

            // �p��s�������
            int newRowIndex = tableLayoutPanel1.RowCount;

            // �W�[�s����
            //tableLayoutPanel1.RowCount += 1;
            tableLayoutPanel1.RowCount = tableLayoutPanel1.RowCount + 1;
            // �վ�氪�˦��T�O�C�氪�פ@�P
            tableLayoutPanel1.RowStyles.Clear();
            for (int i = 0; i < tableLayoutPanel1.RowCount; i++)
            {
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / tableLayoutPanel1.RowCount));
            }

            // �K�[��ƨ�s����
            tableLayoutPanel1.Controls.Add(new Label() { Text = employeCd }, 0, newRowIndex);
            tableLayoutPanel1.Controls.Add(new Label() { Text = chineseName }, 1, newRowIndex);
            tableLayoutPanel1.Controls.Add(new Label() { Text = birthday }, 2, newRowIndex);
            tableLayoutPanel1.Controls.Add(new Label() { Text = idNumber }, 3, newRowIndex);

            // ��ܦ��\�T��
            MessageBox.Show("�s�ɦ��\�I");

            // �M�Ť奻��
            dfEmployeCd.Text = string.Empty;
            dfChineseName.Text = string.Empty;
            dfBirthday.Text = string.Empty;
            dfIDNumber.Text = string.Empty;
        }

        private void pbImport_Click(object sender, EventArgs e)
        {
            // �Ȱ� dataGridView1 ���G��
            dataGridView1.SuspendLayout();

            try
            {
                // 1. �q tableLayoutPanel1 ��Ū�����
                List<string[]> data = new List<string[]>();

                for (int row = 1; row < tableLayoutPanel1.RowCount; row++)
                {
                    List<string> rowData = new List<string>();
                    bool isEmptyRow = true;

                    for (int col = 0; col < tableLayoutPanel1.ColumnCount; col++)
                    {
                        Control control = tableLayoutPanel1.GetControlFromPosition(col, row);
                        if (control != null)
                        {
                            rowData.Add(control.Text);
                            isEmptyRow = false;
                        }
                        else
                        {
                            rowData.Add(string.Empty); // �p�G���󬰪šA�K�[�Ŧr��
                        }
                    }

                    if (!isEmptyRow)
                    {
                        data.Add(rowData.ToArray());
                    }
                }

                // 2. �ھڲĤ@�����Ƨ�
                var sortedData = data.OrderBy(row => row[0]).ToArray();

                // 3. �M�� dataGridView1 ������ơA���O�d�C�]�m
                dataGridView1.Rows.Clear();

                // 4. �K�[�Ƨǫ᪺��ƨ� dataGridView1
                foreach (var row in sortedData)
                {
                    dataGridView1.Rows.Add(row);
                }
            }
            finally
            {
                // ��_ dataGridView1 ���G��
                dataGridView1.ResumeLayout();
            }
        }
        private void btQuit2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pbConnect_Click(object sender, EventArgs e)
        {
            try
            {
                // ���լO�_�i�H�s���Ʈw
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = "Server=localhost;Database=BL;User Id=SYSADM;Password=SYSADM;";
                this.ConnString = conn.ConnectionString;
                EmployeeService.ConnString = conn.ConnectionString;
                // �i�H�Υ��ѳ����X�T��
                conn.Open();
                MessageBox.Show("�s�u���\!");
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("�o�Ϳ��~: " + ex.Message + "���̿�?" + ex.StackTrace);
            }
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // �ˬd�O�_�襤�F tabPage3
            if (tabControl1.SelectedTab == tabPage3)
            {
                // ����s����Ʈw���{���X
                ConnectToDatabase();
            }
        }
        private void ConnectToDatabase()
        {
            try
            {
                //SqlConnection conn = new SqlConnection();
                //conn.ConnectionString = "Server=localhost;Database=BL;User Id=SYSADM;Password=SYSADM";
                //this.ConnString = conn.ConnectionString;
                //EmployeeService.ConnString = conn.ConnectionString;
                //conn.Open(); // ���ե��}�s��
                //////MessageBox.Show("�s�����\");
                //conn.Close(); // �����s��
                //LoadItemsFromDatabase();
                // �۰ʽվ����e��
                dataGridViewItems.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"�s������: {ex.Message}");
            }
        }

        private void pbItemsSelect_Click(object sender, EventArgs e)
        {
            //SqlConnection conn = new SqlConnection();
            //conn.ConnectionString = ConnString;
            //conn.Open();
            //List<itemsview> items = new List<itemsview>();
            //items = conn.Query<itemsview>("Select * From items").ToList();
            //conn.Close();
            //dataGridViewItems.DataSource = items;
            //// �]�w�����Y
            //if (dataGridViewItems.Columns.Count > 0)
            //{
            //    dataGridViewItems.Columns["Id"].HeaderText = "ID";
            //    dataGridViewItems.Columns["Name"].HeaderText = "�W��";
            //    dataGridViewItems.Columns["Description"].HeaderText = "�y�z";
            //    dataGridViewItems.Columns["MarketValue"].HeaderText = "��������";
            //    dataGridViewItems.Columns["Quantity"].HeaderText = "�ƶq";
            //    dataGridViewItems.Columns["Type"].HeaderText = "����";
            //    dataGridViewItems.Columns["LastUpdated"].HeaderText = "�̫��s";
            //    // �۰ʽվ����e��
            //    dataGridViewItems.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            //}
            LoadItemsFromDatabase();
            //// set so whole row is selected �����Q���
            //dataGridViewItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }


        private void pbitemsUpdate_Click_1(object sender, EventArgs e)
        {
            if (dataGridViewItems.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewItems.SelectedRows[0];

                itemsview item = new itemsview
                {
                    Id = selectedRow.Cells["Id"].Value.ToString(),
                    Name = selectedRow.Cells["Name"].Value.ToString(),
                    Description = selectedRow.Cells["Description"].Value.ToString(),
                    MarketValue = Convert.ToInt32(selectedRow.Cells["MarketValue"].Value),
                    Quantity = Convert.ToInt32(selectedRow.Cells["Quantity"].Value),
                    Type = selectedRow.Cells["Type"].Value.ToString(),
                    LastUpdated = Convert.ToDateTime(selectedRow.Cells["LastUpdated"].Value)
                };
                FormUpdate formUpdate = new FormUpdate(item, ConnString)
                {
                    Owner = this
                };
                formUpdate.ShowDialog();
            }
            else
            {
                MessageBox.Show("�п�ܤ@�Ӷ���");
            }
        }
        public void UpdateDataGridViewItems(int id, string name, string description, int marketValue, int quantity, string type, DateTime lastUpdated)
        {
            BindingList<itemsview> itemsList = (BindingList<itemsview>)dataGridViewItems.DataSource;
            var existingItem = itemsList.FirstOrDefault(item => item.Id == id.ToString());

            if (existingItem != null)
            {
                // ��s�{����
                existingItem.Name = name;
                existingItem.Description = description;
                existingItem.MarketValue = marketValue;
                existingItem.Quantity = quantity;
                existingItem.Type = type;
                existingItem.LastUpdated = lastUpdated;
            }
            else
            {
                // �s�W��
                itemsList.Add(new itemsview
                {
                    Id = id.ToString(),
                    Name = name,
                    Description = description,
                    MarketValue = marketValue,
                    Quantity = quantity,
                    Type = type,
                    LastUpdated = lastUpdated
                });
             }

            // ���s�j�w DataGridView �H��̷ܳs���
            dataGridViewItems.DataSource = null;
            dataGridViewItems.DataSource = itemsList;
            // �۰ʽվ����e��
            dataGridViewItems.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

        }
        private void pbQuit3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pbitemsNew_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(ConnString);
            conn.Open();

            // �����e items ���̤j Id
            //string query = "SELECT ISNULL(MAX(Id), 0) + 1 FROM Items";
            //SqlCommand cmd = new SqlCommand(query, conn);
            //int newId = (int)cmd.ExecuteScalar();
            // ����U�ӭn��ܪ� Id
            string query = "SELECT IDENT_CURRENT('items') + IDENT_INCR('items')";
            SqlCommand cmd = new SqlCommand(query, conn);
            int newId = Convert.ToInt32(cmd.ExecuteScalar());
            conn.Close();

            // �Ыطs�� itemsview ��ҡA�ó]�m�s�� Id
            itemsview item = new itemsview
            {
                Id = newId.ToString(),
                Name = string.Empty,
                Description = string.Empty,
                MarketValue = 0,
                Quantity = 0,
                Type = string.Empty,
                LastUpdated = DateTime.Now
            };

            // ���} FormUpdate �öǻ� newItem �M ConnString
            FormUpdate formUpdate = new FormUpdate(item, ConnString)
            {
                Owner = this
            };
            formUpdate.ShowDialog();
        }

        private void pbitemsDel_Click(object sender, EventArgs e)
        {
            // �ˬd�O�_��������
            if (dataGridViewItems.SelectedRows.Count == 0)
            {
                MessageBox.Show("�п�ܤ@�Ӷ���");
                return;
            }

            // ����襤����
            DataGridViewRow selectedRow = dataGridViewItems.SelectedRows[0];

            // ���o�襤���檺 Id ��
            int selectedId;
            if (!int.TryParse(selectedRow.Cells["Id"].Value.ToString(), out selectedId))
            {
                MessageBox.Show("��������صL��");
                return;
            }

            // �T�{�R��
            DialogResult result = MessageBox.Show($"�T�w�n�R�� Id �� {selectedId} ����ƶܡH", "�T�{�R��", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                // �R����Ʈw�������
                using (SqlConnection conn = new SqlConnection(ConnString))
                {
                    conn.Open();
                    string deleteQuery = "DELETE FROM Items WHERE Id = @Id";
                    SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn);
                    deleteCmd.Parameters.AddWithValue("@Id", selectedId);
                    deleteCmd.ExecuteNonQuery();
                    conn.Close();
                }

                // �R�� DataGridView �������
                //dataGridViewItems.Rows.Remove(selectedRow);
                // �R�� BindingList �������
                var itemToRemove = itemsList.FirstOrDefault(item => item.Id == selectedId.ToString());
                if (itemToRemove != null)
                {
                    itemsList.Remove(itemToRemove);
                }
                MessageBox.Show("�R�����\�I");
            }
        }
    }

}



