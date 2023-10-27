using Lab05_DAL.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab05_BLL
{
    public class MajorDAO
    {
        public List<Major> GetAll()
        {
            using (Model_QLSV db_Context = new Model_QLSV())
            {
                return db_Context.Majors.ToList();
            }
        }
        public List<Major> GetAll(int facultyID)
        {
            using(Model_QLSV db_Context = new Model_QLSV())
            {
                return db_Context.Majors.Where(n => n.FacultyID == facultyID).ToList();
            }
        }
        public Major GetMajor(int facultyID, int majorID)
        {
            using(Model_QLSV db_Context = new Model_QLSV())
            {
                return db_Context.Majors.FirstOrDefault(n => n.MajorID == facultyID && n.MajorID == majorID);
            }
        }
    }
}
