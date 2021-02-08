using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ReadConfigFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            var directoryInfo = new DirectoryInfo(@"C:\dev\yellow\git"); 
            var files = directoryInfo.GetFiles("*.sln"); //Getting solution files
            var solutions = new List<string>();
            var sln = "";
            var str = "";
            
            foreach (FileInfo file in files)
            {
                str = str + (str == "" ? "" : ", ") + file.Name;
                solutions.Add(file.Name);
                sln = Path.Combine(@"C:\dev\yellow\git\", file.Name);
                Console.WriteLine(str);
            }
            var solutionParser = Type.GetType("Microsoft.Build.Construction.SolutionParser, Microsoft.Build, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false, false);
            if (solutionParser != null)
            {
                var solutionParser_solutionReader = solutionParser.GetProperty("SolutionReader", BindingFlags.NonPublic | BindingFlags.Instance);
                var solutionParser_projects = solutionParser.GetProperty("Projects", BindingFlags.NonPublic | BindingFlags.Instance);
                var solutionParser_parseSolution = solutionParser.GetMethod("ParseSolution", BindingFlags.NonPublic | BindingFlags.Instance);

            }

            //var workspace = MSBuildWorkspace.Create();
            //var solution = workspace.OpenSolutionAsync(sln).Result;
            //var projects = solution.Projects;
            //foreach (var project in projects)
            //{
            //    //TODO              
            //}

            //var projects = new List<SolutionProject>();
            //var array = (Array)s_SolutionParser_projects.GetValue(solutionParser, null);
            //for (int i = 0; i < array.Length; i++)
            //{
            //    projects.Add(new SolutionProject(array.GetValue(i)));
            //}

            //SolutionProjects = projects;


            Console.ReadKey();
        }

        public static List<SolutionProject> SolutionProjects { get; private set; }
    }

    public class SolutionProject
    {
        static readonly Type s_ProjectInSolution;
        static readonly PropertyInfo s_ProjectInSolution_ProjectName;
        static readonly PropertyInfo s_ProjectInSolution_RelativePath;
        static readonly PropertyInfo s_ProjectInSolution_ProjectGuid;

        static SolutionProject()
        {
            s_ProjectInSolution =
                Type.GetType(
                    "Microsoft.Build.Construction.ProjectInSolution, Microsoft.Build, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
                    false, false);
            if (s_ProjectInSolution != null)
            {
                s_ProjectInSolution_ProjectName =
                    s_ProjectInSolution.GetProperty("ProjectName", BindingFlags.NonPublic | BindingFlags.Instance);
                s_ProjectInSolution_RelativePath = s_ProjectInSolution.GetProperty("RelativePath",
                    BindingFlags.NonPublic | BindingFlags.Instance);
                s_ProjectInSolution_ProjectGuid =
                    s_ProjectInSolution.GetProperty("ProjectGuid", BindingFlags.NonPublic | BindingFlags.Instance);
            }
        }

        public string ProjectName { get; private set; }
        public string RelativePath { get; private set; }
        public string ProjectGuid { get; private set; }

        public SolutionProject(object solutionProject)
        {
            this.ProjectName = s_ProjectInSolution_ProjectName.GetValue(solutionProject, null) as string;
            this.RelativePath = s_ProjectInSolution_RelativePath.GetValue(solutionProject, null) as string;
            this.ProjectGuid = s_ProjectInSolution_ProjectGuid.GetValue(solutionProject, null) as string;
        }
    }
}
