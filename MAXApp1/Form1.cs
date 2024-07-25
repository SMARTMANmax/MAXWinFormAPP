namespace MAXApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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
    }

}


