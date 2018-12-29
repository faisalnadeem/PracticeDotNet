using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitOfWorkDemo
{
	public interface IStudentRepository : IDisposable
	{
		IEnumerable<Student> GetStudents();
		Student GetStudentByID(int studentId);
		void InsertStudent(Student student);
		void DeleteStudent(int studentID);
		void UpdateStudent(Student student);
		void Save();
	}

	public class Student
	{
	}
}
