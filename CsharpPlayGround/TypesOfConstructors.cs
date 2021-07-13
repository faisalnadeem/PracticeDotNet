using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DateTime = System.DateTime;

namespace CsharpPlayGround
{
    public class TypesOfConstructors
    {
        private string InitialValue;
        private static readonly long instanceCreated;
 
        //default constructor
        //public TypesOfConstructors()
        //{
        //    InitialValue = "Initial value from default constructor";
        //}
        //private constructor
        private TypesOfConstructors()
        {
            InitialValue = "initial value from private default constructor";
        }

        //parameterized constructor
        public TypesOfConstructors(string initialValue)
        {
            InitialValue = initialValue;
        }

        //copy constructor
        public TypesOfConstructors(TypesOfConstructors objecTypesOfConstructors)
        {
            this.InitialValue = objecTypesOfConstructors.InitialValue;
        }

        //static constructor
        static TypesOfConstructors()
        {
            try
            {
                instanceCreated = DateTime.Now.Day/0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }



        public void GetInitialValue()
        {
            Console.WriteLine(InitialValue);
        }

        public void GetInstanceCreatedTime()
        {
            Console.WriteLine(instanceCreated);
        }

        public static void MainTest()
        {
            TypesOfConstructors typeOfCtor = null;

            try
            {
                typeOfCtor = new TypesOfConstructors("Initial value from parameterized constructor coz of private constructor inaccessible");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
                
            typeOfCtor.GetInitialValue();
            typeOfCtor.GetInstanceCreatedTime();
            
            typeOfCtor = new TypesOfConstructors("Initial value from parameterized constructor");
            typeOfCtor.GetInitialValue();
            typeOfCtor.GetInstanceCreatedTime();

            var newTypeOfCtor = new TypesOfConstructors("This value for object copy constructor");
            newTypeOfCtor.GetInitialValue();
            newTypeOfCtor.GetInstanceCreatedTime();

            var copyTypeOfCtor = new TypesOfConstructors(newTypeOfCtor);
            copyTypeOfCtor.GetInitialValue();
            copyTypeOfCtor.GetInstanceCreatedTime();

        }
    }
}
