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
            //dbManager = new DatabaseManager("192.168.1.9", "_SMARTMANTEST", "SYSADM", "SYSADM");
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

                // �q��Ʈw���d�߸��
                var items = conn.Query<itemsview>("SELECT * FROM Items").ToList();

                // �M�� itemsListForSorting �í��s�[�����
                itemsListForSorting.Clear();
                itemsListForSorting.AddRange(items);

                // �M�� BindingList �í��s�[�����
                itemsList.Clear();
                foreach (var item in items)
                {
                    itemsList.Add(item);
                }

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
            dataGridViewItems.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
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
            // �N e �ഫ�� DataGridViewCellEventArgs
            DataGridViewCellEventArgs cellEventArgs = e as DataGridViewCellEventArgs;

            // �ˬd�ഫ�O�_���\
            if (cellEventArgs != null)
            {
                // �ˬd�O�_�O���D��
                if (cellEventArgs.RowIndex == -1)
                {
                    // �p�G�O���D��A�h����������޿�
                    return;
                }
            }
            ItemUpdater updater = new ItemUpdater(this.dbManager);
            updater.UpdateSelectedItem(dataGridViewItems, this);
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
            SqlConnection conn = dbManager.OpenConnection();
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
            FormUpdate formUpdate = new FormUpdate(this.dbManager, item)
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

        private void dataGridViewItems_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // �ˬd�O�_�O���D��
            if (e.RowIndex == -1)
            {
                // �p�G�O���D��A�h����������޿�
                return;
            }
            ItemUpdater updater = new ItemUpdater(this.dbManager);
            updater.UpdateSelectedItem(dataGridViewItems, this);
        }
        public void ShowTabPage(int index)
        {
            tabControl1.SelectedIndex = index;
        }

        private void pbitemsDel2_Click(object sender, EventArgs e)
        {
            // �ˬd�O�_��������
            if (dataGridViewItems.SelectedRows.Count == 0)
            {
                MessageBox.Show("�п�ܦܤ֤@�Ӷ���");
                return;
            }

            // �T�{�R��
            DialogResult result = MessageBox.Show($"�T�w�n�R���襤�� {dataGridViewItems.SelectedRows.Count} ����ƶܡH", "�T�{�R��", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                using (SqlConnection conn = dbManager.OpenConnection())
                {
                    // �}�l���
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // �M���襤���C�@��
                            foreach (DataGridViewRow selectedRow in dataGridViewItems.SelectedRows)
                            {
                                // ���o�襤���檺 Id ��
                                int selectedId;
                                if (!int.TryParse(selectedRow.Cells["Id"].Value.ToString(), out selectedId))
                                {
                                    MessageBox.Show("��������صL��");
                                    return;
                                }

                                // �ϥ� Dapper �R����Ʈw�������
                                string deleteQuery = "DELETE FROM Items WHERE Id = @Id";
                                conn.Execute(deleteQuery, new { Id = selectedId }, transaction);

                                //// �R�� itemsListForSorting �������
                                var itemToRemove = itemsListForSorting.FirstOrDefault(item => item.Id == selectedId.ToString());
                                if (itemToRemove != null)
                                {
                                    itemsListForSorting.Remove(itemToRemove);
                                }
                            }

                            // ������
                            transaction.Commit();
                            // ��s DataGridView
                            dataGridViewItems.DataSource = null;
                            dataGridViewItems.DataSource = itemsListForSorting;
                            dataGridViewItems.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                            MessageBox.Show("�R�����\�I");
                            
                        }
                        catch (Exception ex)
                        {
                            // �p�G�����~�A�^�u���
                            transaction.Rollback();
                            MessageBox.Show($"�R������: {ex.Message}");
                        }
                    }
                }
            }
        }

        private void pbImport1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files (*.csv)|*.csv";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string[] lines = File.ReadAllLines(openFileDialog.FileName);
                List<itemsview> items = new List<itemsview>();

                using (SqlConnection conn = dbManager.OpenConnection())
                {
                   
                    for (var i = 1; i < lines.Length; i++)
                    {
                        var splitData = lines[i].Split(',');
                        // ���L���Y�� "�W��" �}�Y����
                        if (splitData[0] == "�W��")
                        {
                            continue;
                        }

                        int nextId = conn.QuerySingle<int>("SELECT IDENT_CURRENT('items') + IDENT_INCR('items')");

                        itemsview item = new itemsview
                        {
                            Id = nextId++.ToString(),
                            Name = splitData[0],
                            Type = splitData[1],    
                            Description = splitData[4],
                            MarketValue = Convert.ToInt32(splitData[2]),
                            Quantity = Convert.ToInt32(splitData[3]),
                            LastUpdated = DateTime.Now
                        };


                        conn.Execute(@"INSERT INTO Items (Name, Type, Description, MarketValue, Quantity, LastUpdated) 
                               VALUES (@Name, @Type, @Description, @MarketValue, @Quantity, @LastUpdated)",
                            new
                                {
                                Name = item.Name,
                                Type = item.Type,
                                Description = item.Description,
                                MarketValue = item.MarketValue,
                                Quantity = item.Quantity,
                                LastUpdated = DateTime.Now
                                });

                        items.Add(item);
                    }
                }

                // ��s DataGridView
                itemsListForSorting.AddRange(items);
                dataGridViewItems.DataSource = null;
                dataGridViewItems.DataSource = itemsListForSorting;
                dataGridViewItems.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
        }
    }
}



