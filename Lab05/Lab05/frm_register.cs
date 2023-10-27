using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using lab05_BLL;
using Lab05_DAL.model; 

namespace Lab05
{
    public partial class frm_register : Form
    {
        private StudentDAO s_DAO = new StudentDAO();
        private FacultyDAO f_DAO = new FacultyDAO();
        private MajorDAO mj_DAO = new MajorDAO();
        public frm_register()
        {
            InitializeComponent();
        }

        private void cboRegisterKhoa_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

      

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            int majorID = int.Parse(cboRegisterNganh.SelectedValue.ToString());
            for(int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                if (dataGridView2.Rows[i].Cells["Chon"].Value != null&&
                    bool.Parse(dataGridView2.Rows[i].Cells["Chon"].Value.ToString()))
                {
                    string studentID = dataGridView2.Rows[i].Cells["MaSV"].Value.ToString();
                    Student s =s_DAO.GetStudent(studentID);
                    if (s != null)
                    {
                        s.MajorID = majorID;
                        int ketqua = s_DAO.Update(s);
                        if(ketqua == 0)
                        {
                            MessageBox.Show("Successful", "Ket qua");
                            Fill_dgv_DSSV();
                        }
                        else
                        {
                            MessageBox.Show("Successful", "Ket qua");
                        }
                    }
                }
            }
        }

        private void Fill_cboKhoa()
        {
            List<Faculty> f_List = f_DAO.GetAll();
            cboRegisterKhoa.DataSource = f_List;
            cboRegisterKhoa.ValueMember = "FacultyID";
            cboRegisterKhoa.DisplayMember = "FacultyName";
            this.cboRegisterKhoa.SelectedIndexChanged += new System.EventHandler(this.cboRegisterKhoa_SelectedIndexChanged_1);
        }
        private void Fill_cbo_Major()
        {
            int facultyID = int.Parse(cboRegisterKhoa.SelectedValue.ToString());
            List<Major> mj_List = mj_DAO.GetAll(facultyID);
            cboRegisterNganh.DataSource = mj_List;
            cboRegisterNganh.ValueMember = "MajorID";
            cboRegisterNganh.DisplayMember = "Name";
        } 
        private void Fomat_dgv_QLSV()
        {
            dataGridView2.Columns.Clear();
            DataGridViewCheckBoxColumn column = new DataGridViewCheckBoxColumn();
            column.Name = "Chon";
            column.HeaderText = "Chọn";
            dataGridView2.Columns.Add(column);
            dataGridView2.Columns.Add("MaSV", "Mã SV");
            dataGridView2.Columns.Add("Hoten", "Họ Tên SV");
            dataGridView2.Columns["Hoten"].Width = 160;
            dataGridView2.Columns.Add("Khoa", "Khoa");
            dataGridView2.Columns.Add("DiemTB", "AverageSCore");
        }

        private void Fill_dgv_DSSV()
        {
            dataGridView2.Rows.Clear();
            int facultyID = int.Parse(cboRegisterKhoa.SelectedValue.ToString());
            List<Student> s_List = s_DAO.GetAll_NoMajor(facultyID);
            foreach(Student s in s_List)
            {
                string facultyName = null;
                if(s.FacultyID != null)
                {
                    facultyName = f_DAO.GetFaculty((int)s.FacultyID).FacultyName;
                }
                int index = dataGridView2.Rows.Add();
                dataGridView2.Rows[index].Cells["Chon"].Value = false;
                dataGridView2.Rows[index].Cells["MaSV"].Value = s.StudentID;
                dataGridView2.Rows[index].Cells["HoTen"].Value = s.FullName;
                dataGridView2.Rows[index].Cells["Khoa"].Value = (facultyName != null) ? "" : facultyName ;
                dataGridView2.Rows[index].Cells["DiemTB"].Value = s.AverageScore;
            }
        }
        private void frm_register_Load(object sender, EventArgs e)
        {
            Fomat_dgv_QLSV();
            Fill_cboKhoa();
            Fill_cbo_Major();
            Fill_dgv_DSSV();
        }

        private void cboRegisterKhoa_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            Fill_cbo_Major();
            Fill_cboKhoa();
        }

        private void cboRegisterNganh_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
      
    }
