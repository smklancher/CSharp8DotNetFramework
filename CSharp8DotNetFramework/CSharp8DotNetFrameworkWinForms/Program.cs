using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharp8DotNetFramework
{
    internal static class Program
    {
        //references: https://stu.dev/csharp8-doing-unsupported-things/
        //https://stackoverflow.com/questions/56651472/does-c-sharp-8-support-the-net-framework/57020770#57020770

        // To create an SDK style project on .NET Framework, either:
        // 1. Create a normal project and then convert (https://github.com/hvanbakel/CsprojToVs2017)
        //    dotnet tool install --global Project2015To2017.Migrate2019.Tool
        //    dotnet migrate-2019 wizard .\MyProjectDirectory
        // 2. Create a .NET Core or .NET Standard project
        //    And then edit target to .NET Framework, such as
        //    <TargetFramework>net472</TargetFramework>
        //    https://stackoverflow.com/a/58548163/221018

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}