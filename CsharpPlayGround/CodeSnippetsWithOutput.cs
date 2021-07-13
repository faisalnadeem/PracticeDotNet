using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpPlayGround
{
    public class CodeSnippetsWithOutput
    {
        public static void MainTest()
        {
            /*Compile time errors
            int a = 5;
            int b = a + 2; //OK

            bool test = true;

            // Error. Operator '+' cannot be applied to operands of type 'int' and 'bool'.
            int c = a + test;
            //Severity	Code	Description	Project	File	Line	Suppression State
            //Error	CS0019	Operator '+' cannot be applied to operands of type 'int' and 'bool'	CsharpPlayGround
            //C:\src\git-repos\PracticeDotNet\CsharpPlayGround\CodeSnippetsWithOutput.cs	19	Active
             */

            // Declaration only:
            float temperature;
            string name;
            CodeSnippetsWithOutput myClass;

            // Declaration with initializers (four examples):
            char firstLetter = 'C';
            var limit = 3;
            int[] source = { 0, 1, 2, 3, 4, 5 };
            var query = from item in source
                where item <= limit
                select item;


        }
    }
}
