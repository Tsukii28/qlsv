using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using lab05_BLL;
using Lab05_DAL.model;

namespace Lab05
{
    public partial class frmQLSV : Form
    {
        private StudentDAO s_DAO = new StudentDAO();
        private FacultyDAO f_DAO = new FacultyDAO();
        private MajorDAO mj_DAO = new MajorDAO();

        public frmQLSV()
        {
            InitializeComponent();
        }

        
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Fill_dgv_QLSV();    
        }
        private void cboKhoa_SelectedIndexChanged_1(object sender, EventArgs e)
        {
           //Fill_dgv_QLSV();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dataGridView1.SelectedCells[0].RowIndex;
            if (dataGridView1.Rows[index].Cells["MaSV"].Value != null)
            {
                string studentID = dataGridView1.Rows[index].Cells["MaSV"].Value.ToString();
                Student s = s_DAO.GetStudent(studentID);
                txtMaSV.Text = s.StudentID;
                txtHoTen.Text = s.FullName;
                cboKhoa.SelectedValue = s.FacultyID;
                txtDiemTB.Text = s.AverageScore.ToString();
                if (s.Avatar == null)
                {
                    pictureBox1.Image = null;
                }
                else
                {
                    Fill_Image(s.Avatar);
                }
            }
        }
                private void Fill_Image(string filename)
                {
                    try {
                        string parentDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)
                        .Parent.Parent.FullName;
                        string imagePath = Path.Combine(parentDirectory, "Images",filename);
                        pictureBox1.Image = Image.FromFile(imagePath);
                        pictureBox1.Refresh();
                    }
                    catch (Exception) 
                    {
                        pictureBox1.Image = null;
                    }

                }
                   
        

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtMaSV.Text.Trim() == "")
            {
                MessageBox.Show("Ma SV khong duoc de trong", " Error ");
                return;
            }
            if (txtHoTen.Text.Trim() == "")
            {
                MessageBox.Show("Ho ten sinh vien khong duoc de trong", " Error ");
                return;
            }
            if (txtDiemTB.Text.Trim() == "")
            {
                MessageBox.Show("Diem TB duoc de trong", " Error ");
                return;
            }
            float diemTB = 0;
            if (!float.TryParse(txtDiemTB.Text, out diemTB))
            {
                MessageBox.Show("Diem TB khong dung dinh dang", "Error");
                return;
            }
            if (diemTB < 0 || diemTB > 10)
            {
                MessageBox.Show("Diem trong quy dinh  tu 0 - 10 ", "Error");
                return;
            }
            string fileName = null;
            if (pictureBox1.Image != null)
            {
                string parentDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                parentDirectory = Path.Combine(parentDirectory, "Images");
                DirectoryInfo dir = new DirectoryInfo(parentDirectory);
                if (!dir.Exists)
                {
                    dir.Create();
                }
                fileName = txtMaSV.Text + ".jpg";
                string imagePath = Path.Combine(parentDirectory, fileName);
                pictureBox1.Image.Save(imagePath);
            }



            Student s = new Student()
            {
                StudentID = txtMaSV.Text,
                FullName = txtHoTen.Text,
                AverageScore = diemTB,
                FacultyID = int.Parse(cboKhoa.SelectedValue.ToString()),
                Avatar = (fileName == null) ? "" : fileName };
                int ketqua = s_DAO.Update(s); 
                if(ketqua == 0)
            {
                MessageBox.Show("Successfully", "Ket qua");
                Fill_dgv_QLSV();
            }
            else
            {
                MessageBox.Show("Unsuccessfully", "Ket qua");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtMaSV.Text.Trim() == "")
            {
                MessageBox.Show("Ma sinh vien k duoc de trong", "Error");
                return;
            }
            int ketqua = s_DAO.Delete(txtMaSV.Text);
            if (ketqua == 0)
            {
                MessageBox.Show("Successfully", "Ket qua");
                Fill_dgv_QLSV();
            }
            else
            {
                MessageBox.Show("Unsuccessfully", "Ket qua");
            }

        }

        private void frmQLSV_Load(object sender, EventArgs e)
        {   
            Format_dgv_QLSV();
            Fill_cboKhoa();
            Fill_dgv_QLSV();
        }
        private void Fill_cboKhoa()
        {
            List<Faculty> f_List = f_DAO.GetAll();
            cboKhoa.DataSource = f_List;
            cboKhoa.ValueMember = "FacultyID";
            cboKhoa.DisplayMember = "FacultyName";
            this.cboKhoa.SelectedIndexChanged += new System.EventHandler(this.cboKhoa_SelectedIndexChanged_1);
        }

        private void Format_dgv_QLSV()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("MaSV", "Mã SV");
            dataGridView1.Columns.Add("Hoten", "Họ Tên SV");
            dataGridView1.Columns["Hoten"].Width = 160;
            dataGridView1.Columns.Add("Khoa", "Khoa");
            dataGridView1.Columns.Add("CNganh", "Chuyên Ngành");
            dataGridView1.Columns.Add("DiemTB", "AverageSCore");
        }

        private void Fill_dgv_QLSV()
        {   
            dataGridView1.Rows.Clear();
            int facultyID = int.Parse(cboKhoa.SelectedValue.ToString());
            List<Student> s_List;
            if (chk_und.Checked)
            {
                s_List = s_DAO.GetAll_NoMajor(facultyID);
            }
            else 
            {
                s_List = s_DAO.GetAll(facultyID);
            }
            
            
            
            foreach(Student s in s_List)
            {
                string facultyName = null;
                if(s.FacultyID != null)
                {
                    facultyName = f_DAO.GetFaculty((int)s.FacultyID).FacultyName;
                }

                string majorName = null;
                if(s.FacultyID != null && s.MajorID != null)
                {
                    majorName = mj_DAO.GetMajor((int)s.FacultyID, (int)s.MajorID).Name;
                }

                int index = dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells["MaSV"].Value = s.StudentID;
                dataGridView1.Rows[index].Cells["Hoten"].Value = s.FullName;
                dataGridView1.Rows[index].Cells["Khoa"].Value = (facultyName == null) ? "" : facultyName;
                dataGridView1.Rows[index].Cells["CNganh"].Value = (majorName == null) ? "" : majorName;
                dataGridView1.Rows[index].Cells["DiemTB"].Value = s.AverageScore;

            }
        }

        private void uploadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog f_Dialog = new OpenFileDialog();
            f_Dialog.Filter = "All Files |*.*|JPEG Files|*.jpg";
            if (f_Dialog.ShowDialog()== DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(f_Dialog.FileName);
            }
        }

        private void txtMaSV_TextChanged(object sender, EventArgs e)
        {
            timer1.Interval = 2000;
            timer1.Enabled = true;
            timer1.Enabled = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            if (txtMaSV.Text.Trim() == "")
            {
                pictureBox1.Image = null;
            }
            else
            {
                Student s = s_DAO.GetStudent(txtMaSV.Text);
                if (s == null || s.Avatar == null)
                {
                    pictureBox1.Image = null; 
                }
                else
                {
                    Fill_Image(s.Avatar);
                }
            }
        }

        private void dKMajorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();   
            frm_register form2 = new frm_register();
            form2.ShowDialog();
            this.Show();
            this.Fill_dgv_QLSV();
        }
    }
}
