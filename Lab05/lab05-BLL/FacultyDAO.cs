using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab05_DAL.model;

namespace lab05_BLL
{
    public class FacultyDAO
    {
        public List<Faculty> GetAll()
        {
            using (Model_QLSV db_Context =  new Model_QLSV())
            {
                return db_Context.Faculties.ToList();
            }
        }

        public Faculty GetFaculty(int facultyID) 
        {
            using (Model_QLSV db_Context = new Model_QLSV())
            {
                return db_Context.Faculties.FirstOrDefault(n => n.FacultyID == facultyID);
            }
        }

    }
}
