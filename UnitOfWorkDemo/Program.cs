using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UnitOfWorkDemo
{
	class Program
	{
		static void Main(string[] args)
		{
			var callingMethod = GetCallingMethod();
			Console.WriteLine();
			//var students = new StudentController().GetStudents();
			//var unitOfWork = new UnitOfWork();
			//var students = unitOfWork.StudentRepository.Get();
			Console.ReadLine();
		}

		public static string GetCallingMethod()
		{
			string str = "";
			try
			{
				var st = new StackTrace();
				var frames = st.GetFrames();
				str = frames[0].GetMethod().Name;
			}
			catch (Exception)
			{
				;
			}

			return str;
		}

		public static string GetCallingMethod(string MethodAfter)
		{
			string str = "";
			try
			{
				StackTrace st = new StackTrace();
				StackFrame[] frames = st.GetFrames();
				for (int i = 0; i < st.FrameCount - 1; i++)
				{
					if (frames[i].GetMethod().Name.Equals(MethodAfter))
					{
						if (!frames[i + 1].GetMethod().Name.Equals(MethodAfter)) // ignores overloaded methods.
						{
							str = frames[i + 1].GetMethod().ReflectedType.FullName + "." + frames[i + 1].GetMethod().Name;
							break;
						}
					}
				}
			}
			catch (Exception)
			{
				;
			}

			return str;
		}

		public class StudentController
		{
			private IStudentRepository studentRepository;

			public StudentController()
			{
				this.studentRepository = new StudentRepository(new SchoolContext());
			}

			public StudentController(IStudentRepository studentRepository)
			{
				this.studentRepository = studentRepository;
			}

			public IEnumerable<Student> GetStudents()
			{
				return studentRepository.GetStudents();
			}
		}
	}
}