using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFileSystemDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var designType = DesignType.Desktop;
            var imgDir = string.Format("/content/{0}/images/", designType.ToString("G"));
            var imgDirD = string.Format("/content/{0}/images/", designType.ToString("D"));
            var imgDirX = string.Format("/content/{0}/images/", designType.ToString("X"));
            var imgDirF = string.Format("/content/{0}/images/", designType.ToString("F"));
            //var imgDirT = string.Format("/content/{0}/images/", designType.ToString("T")); //expect exception

        }
    }

    public enum DesignType
    {
        Desktop,
        Mobile,
        Redesign,
        //RoyalMail
    }

    public class FileParser
    {
        //public void Parse(IFile cvOutputFile)
        //{

        //}
    }
}
