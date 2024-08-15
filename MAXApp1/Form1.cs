using Dapper;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MAXApp1
{
    public partial class Form1 : Form
    {
        public string ConnString { get; set; }
        // �ŧi DatabaseManager ����
        public DatabaseManager dbManager;
        private BindingList<itemsview> itemsList = new BindingList<itemsview>();
        private List<itemsview> itemsListForSorting = new List<itemsview>();
        private System.Windows.Forms.SortOrder sortOrder = System.Windows.Forms.SortOrder.Ascending; // �Ϊ�l�� Descending
        public Form1()
        {
            InitializeComponent();
            //ConnString = "Server=localhost;Database=BL;User Id=SYSADM;Password=SYSADM;"; 
            //ConnString = "Server=192.168.1.9;Database=_SMARTMANTEST;User Id=SYSADM;Password=SYSADM;";
            // ��l�� DatabaseManager
            dbManager = new DatabaseManager("localhost", "BL", "SYSADM", "SYSADM");
            // ���U SelectedIndexChanged �ƥ�
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            this.Load += new EventHandler(Form1_Load);
            dataGridViewItems.ColumnHeaderMouseDoubleClick += dataGridViewItems_ColumnHeaderMouseDoubleClick;
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
            //dataGridView1.DataSource = items;
            // ��l�� DataGridView ����Ʒ�
            dataGridViewItems.DataSource = itemsList;
            // 3. �M�� dataGridView1 ������ơA���O�d�C�]�m
            dataGridViewItems.Rows.Clear();
            // ���]����k�q��Ʈw����� items �ê�l�� itemsList
            LoadItemsFromDatabase();
        }

        public void LoadItemsFromDatabase()
        {
            //using (SqlConnection conn = new SqlConnection(ConnString))
            using (SqlConnection conn = dbManager.OpenConnection())
            {

                //  dataGridView1 ������ơA���O�d�C�]�m
                dataGridViewItems.Rows.Clear();
                //conn.Open();
                var items = conn.Query<itemsview>("SELECT * FROM Items").ToList();
                //itemsListForSorting = items;
                // �Ыؤ@�ӷs���C��A�ñN items �����e�ƻs��o�ӷs�C��
                itemsListForSorting = items.ToList();
                foreach (var item in items)
                {
                    itemsList.Add(item);
                }
                //conn.Close();
                dataGridViewItems.Columns["Id"].HeaderText = "ID";
                dataGridViewItems.Columns["Name"].HeaderText = "�W��";
                dataGridViewItems.Columns["Description"].HeaderText = "�y�z";
                dataGridViewItems.Columns["MarketValue"].HeaderText = "��������";
                dataGridViewItems.Columns["Quantity"].HeaderText = "�ƶq";
                dataGridViewItems.Columns["Type"].HeaderText = "����";
                dataGridViewItems.Columns["LastUpdated"].HeaderText = "�̫��s";
            }
            dataGridViewItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewItems.AllowUserToAddRows = false;
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
                //conn.ConnectionString = "Server=192.168.1.9;Database=_SMARTMANTEST;User Id=SYSADM;Password=SYSADM;";
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
                //// ����s����Ʈw���{���X
                LoadItemsFromDatabase();
                // �۰ʽվ����e��
                dataGridViewItems.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            }
        }

        private void pbItemsSelect_Click(object sender, EventArgs e)
        {
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
                //FormUpdate formUpdate = new FormUpdate(item, ConnString)
                //{
                //    Owner = this
                //};
                FormUpdate formUpdate = new FormUpdate(this.dbManager, item, ConnString);
                formUpdate.Owner = this; // this �O��e�� Form1 ���
                formUpdate.ShowDialog();
            }
            else
            {
                MessageBox.Show("�п�ܤ@�Ӷ���");
            }
        }
        public void UpdateDataGridViewItems(int id, string name, string description, int marketValue, int quantity, string type, DateTime lastUpdated)
        {
            BindingList<itemsview> itemsList;

            // �ˬd DataSource �O�_�� BindingList<itemsview>
            if (dataGridViewItems.DataSource is BindingList<itemsview> bindingList)
            {
                itemsList = bindingList;
            }
            // �p�G DataSource �O List<itemsview>�A�h�ഫ�� BindingList<itemsview>
            else if (dataGridViewItems.DataSource is List<itemsview> list)
            {
                itemsList = new BindingList<itemsview>(list);
            }
            else
            {
                // �Y DataSource �O��L�����A�h��l�Ƥ@�ӷs�� BindingList<itemsview>
                itemsList = new BindingList<itemsview>();
            }

            // �M��O�_�w�s�b�ۦP�� Id
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
            // ���s�]�w�����D
            dataGridViewItems.Columns["Id"].HeaderText = "ID";
            dataGridViewItems.Columns["Name"].HeaderText = "�W��";
            dataGridViewItems.Columns["Description"].HeaderText = "�y�z";
            dataGridViewItems.Columns["MarketValue"].HeaderText = "��������";
            dataGridViewItems.Columns["Quantity"].HeaderText = "�ƶq";
            dataGridViewItems.Columns["Type"].HeaderText = "����";
            dataGridViewItems.Columns["LastUpdated"].HeaderText = "�̫��s";

            // �۰ʽվ����e��
            dataGridViewItems.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }
        private void pbQuit3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pbitemsNew_Click(object sender, EventArgs e)
        {
            //SqlConnection conn = new SqlConnection(ConnString);
            //conn.Open();
            SqlConnection conn = dbManager.OpenConnection();
            // �����e items ���̤j Id
            //string query = "SELECT ISNULL(MAX(Id), 0) + 1 FROM Items";
            //SqlCommand cmd = new SqlCommand(query, conn);
            //int newId = (int)cmd.ExecuteScalar();
            //// ����U�ӭn��ܪ� Id
            //string query = "SELECT IDENT_CURRENT('items') + IDENT_INCR('items')";
            //SqlCommand cmd = new SqlCommand(query, conn);
            //int newId = Convert.ToInt32(cmd.ExecuteScalar());
            // �ϥ� Dapper �Ӱ���d�ߨ���� newId
            string query = "SELECT IDENT_CURRENT('items') + IDENT_INCR('items')";
            int newId = conn.Query<int>(query).Single();
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
            FormUpdate formUpdate = new FormUpdate(this.dbManager, item, ConnString)
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
                // �ϥ� Dapper �R����Ʈw�������
                using (SqlConnection conn = dbManager.OpenConnection())
                {
                    string deleteQuery = "DELETE FROM Items WHERE Id = @Id";
                    conn.Execute(deleteQuery, new { Id = selectedId });
                }

                // �R�� BindingList �������
                var itemToRemove = itemsList.FirstOrDefault(item => item.Id == selectedId.ToString());
                if (itemToRemove != null)
                {
                    itemsList.Remove(itemToRemove);
                }

                MessageBox.Show("�R�����\�I");
            }
        }

        //private void dataGridViewItems_ColumnDividerDoubleClick(object sender, DataGridViewColumnDividerDoubleClickEventArgs e)
        //{

        //}

        private void dataGridViewItems_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // ����I�������W��
            //string sortColumn = dataGridViewItems.Columns[e.ColumnIndex].Name;

            //// �����e���ƧǤ�V
            //System.Windows.Forms.SortOrder sortOrder = dataGridViewItems.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection;
            //string sortDirection = sortOrder == System.Windows.Forms.SortOrder.Ascending ? "DESC" : "ASC";

            //// ��s�ƧǹϼФ�V
            //dataGridViewItems.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = sortOrder == System.Windows.Forms.SortOrder.Ascending ? System.Windows.Forms.SortOrder.Descending : System.Windows.Forms.SortOrder.Ascending;

            //// �ϥ� Dapper �q��Ʈw�����s�d�ߨñƧ�
            //using (SqlConnection conn = dbManager.OpenConnection())
            //{
            //    string query = $"SELECT * FROM Items ORDER BY {sortColumn} {sortDirection}";
            //    var items = conn.Query<itemsview>(query).ToList();

            //    // �N�d�ߵ��G�j�w�� dataGridViewItems
            //    dataGridViewItems.DataSource = new BindingList<itemsview>(items);
            //}
            string sortColumn = dataGridViewItems.Columns[e.ColumnIndex].Name;

            // �����ƧǶ���
            if (sortOrder == System.Windows.Forms.SortOrder.Ascending)
            {
                sortOrder = System.Windows.Forms.SortOrder.Descending;
                itemsListForSorting = itemsListForSorting.OrderByDescending(item => item.GetType().GetProperty(sortColumn).GetValue(item, null)).ToList();
            }
            else
            {
                sortOrder = System.Windows.Forms.SortOrder.Ascending;
                itemsListForSorting = itemsListForSorting.OrderBy(item => item.GetType().GetProperty(sortColumn).GetValue(item, null)).ToList();
            }

            // �N�Ƨǫ᪺�C���ഫ�� BindingList
            itemsList = new BindingList<itemsview>(itemsListForSorting);

            // ��s dataGridViewItems �����
            dataGridViewItems.DataSource = null; // ����ô��
            dataGridViewItems.DataSource = itemsList; // ���sô��
            //// �����ƧǤ�V
            //ListSortDirection direction = ListSortDirection.Ascending;

            //if (dataGridViewItems.SortOrder == System.Windows.Forms.SortOrder.Ascending || dataGridViewItems.SortOrder == System.Windows.Forms.SortOrder.None)
            //{
            //    direction = ListSortDirection.Descending;
            //}
            //else
            //{
            //    direction = ListSortDirection.Ascending;
            //}

            // ����w�C�i��Ƨ�
            //dataGridViewItems.Sort(dataGridViewItems.Columns[e.ColumnIndex], direction);
            dataGridViewItems.Columns["Id"].HeaderText = "ID";
            dataGridViewItems.Columns["Name"].HeaderText = "�W��";
            dataGridViewItems.Columns["Description"].HeaderText = "�y�z";
            dataGridViewItems.Columns["MarketValue"].HeaderText = "��������";
            dataGridViewItems.Columns["Quantity"].HeaderText = "�ƶq";
            dataGridViewItems.Columns["Type"].HeaderText = "����";
            dataGridViewItems.Columns["LastUpdated"].HeaderText = "�̫��s";
            dataGridViewItems.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

        }

        private void dfSerch_TextChanged(object sender, EventArgs e)
        {
            string searchText = dfSerch.Text.ToLower();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                // �p�G�j�M�ج��šA��_��l���
                dataGridViewItems.DataSource = itemsListForSorting;
            }
            else
            {
                // �i��z��
                var filteredList = itemsListForSorting
                    .Where(item => item.GetType().GetProperties()
                        .Any(prop => prop.GetValue(item, null) != null &&
                                     prop.GetValue(item, null).ToString().ToLower().Contains(searchText)))
                    .ToList();

                // ��s DataSource ���z��᪺�C��
                dataGridViewItems.DataSource = new BindingList<itemsview>(filteredList);
            }
        }
    }
}



