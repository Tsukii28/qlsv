using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab05_DAL.model;

namespace lab05_BLL
{
    public class StudentDAO
    {
        public List<Student> GetAll()
        {
            using (Model_QLSV db_Context = new Model_QLSV())
            {
                return db_Context.Students.ToList();
            }
        }
         
        public List<Student> GetAll_NoMajor()
        {
            using (Model_QLSV db_Context = new Model_QLSV())
            {
                return db_Context.Students.Where(n => n.MajorID == null).ToList();
            }
        }
        public List<Student> GetAll(int facultyID)
        {
            using (Model_QLSV db_Context = new Model_QLSV())
            {
                return db_Context.Students.Where(n => n.FacultyID == facultyID).ToList();
            }
        }

        public  List<Student> GetAll_NoMajor(int facultyID)
        {
            using (Model_QLSV db_Context = new Model_QLSV())
            {
                return db_Context.Students.Where(n => n.FacultyID == facultyID && n.MajorID == null).ToList();
            }
        }

        public Student GetStudent(string studentID)
        {
            using (Model_QLSV db_Context = new Model_QLSV())
            {
                return db_Context.Students.FirstOrDefault(n => n.StudentID == studentID);
            }
        }

        public int  Update(Student s) 
        {
            using(Model_QLSV db_Context = new Model_QLSV())
            {
                if (db_Context.Students.Any(n => n.StudentID == s.StudentID))
                {
                    db_Context.Entry(s).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    db_Context.Students.Add(s);
                }
                try
                {
                    db_Context.SaveChanges();
                    return 0;
                }
                catch(Exception ) 
                {
                    return -1 ;
                }
            }
        }
        public int Delete (Student s)
        {
            using(Model_QLSV db_Context = new Model_QLSV())
            {
                if(db_Context.Students.Any(n=> n.StudentID == s.StudentID))
                {
                    db_Context.Students.Remove(s);
                    try
                    {
                        db_Context.SaveChanges();
                        return 0;
                    }
                    catch(Exception)
                    {
                        return -1 ;
                    }
                }
                else
                {
                    return 0 ;
                }
            }
        }
        public int Delete (string studentID)
        {
            using(Model_QLSV db_Context = new Model_QLSV())
            {
                Student s = db_Context.Students.FirstOrDefault(n=> n.StudentID == studentID);
                if(s != null)
                {
                    return 0;
                }
                else
                {
                    db_Context.Students.Remove(s);
                    try
                    {
                        db_Context.SaveChanges();
                        return 0;
                    }
                    catch (Exception)
                    {
                        return -1 ;
                    }
                }
            }
        }
    }
}
