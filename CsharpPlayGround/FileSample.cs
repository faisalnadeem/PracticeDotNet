#region Help:  Introduction to the script task
/* The Script Task allows you to perform virtually any operation that can be accomplished in
 * a .Net application within the context of an Integration Services control flow.
 *
 * Expand the other regions which have "Help" prefixes for examples of specific ways to use
 * Integration Services features within this script task. */
#endregion


#region Namespaces
using System;
using System.Data;
using Microsoft.SqlServer.Dts.Runtime;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Net;
#endregion

namespace ST_ba27ebfe2edb442389b897fa76553a9e
{
    /// <summary>
    /// ScriptMain is the entry point class of the script.  Do not change the name, attributes,
    /// or parent of this class.
    /// </summary>
[Microsoft.SqlServer.Dts.Tasks.ScriptTask.SSISScriptTaskEntryPointAttribute]
public partial class ScriptMain : Microsoft.SqlServer.Dts.Tasks.ScriptTask.VSTARTScriptObjectModelBase
{
        #region Help:  Using Integration Services variables and parameters in a script
        /* To use a variable in this script, first ensure that the variable has been added to
         * either the list contained in the ReadOnlyVariables property or the list contained in
         * the ReadWriteVariables property of this script task, according to whether or not your
         * code needs to write to the variable.  To add the variable, save this script, close this instance of
         * Visual Studio, and update the ReadOnlyVariables and
         * ReadWriteVariables properties in the Script Transformation Editor window.
         * To use a parameter in this script, follow the same steps. Parameters are always read-only.
         *
         * Example of reading from a variable:
         *  DateTime startTime = (DateTime) Dts.Variables["System::StartTime"].Value;
         *
         * Example of writing to a variable:
         *  Dts.Variables["User::myStringVariable"].Value = "new value";
         *
         * Example of reading from a package parameter:
         *  int batchId = (int) Dts.Variables["$Package::batchId"].Value;
         *  
         * Example of reading from a project parameter:
         *  int batchId = (int) Dts.Variables["$Project::batchId"].Value;
         *
         * Example of reading from a sensitive project parameter:
         *  int batchId = (int) Dts.Variables["$Project::batchId"].GetSensitiveValue();
         * */

        #endregion

        #region Help:  Firing Integration Services events from a script
        /* This script task can fire events for logging purposes.
         *
         * Example of firing an error event:
         *  Dts.Events.FireError(18, "Process Values", "Bad value", "", 0);
         *
         * Example of firing an information event:
         *  Dts.Events.FireInformation(3, "Process Values", "Processing has started", "", 0, ref fireAgain)
         *
         * Example of firing a warning event:
         *  Dts.Events.FireWarning(14, "Process Values", "No values received for input", "", 0);
         * */
        #endregion

        #region Help:  Using Integration Services connection managers in a script
        /* Some types of connection managers can be used in this script task.  See the topic
         * "Working with Connection Managers Programatically" for details.
         *
         * Example of using an ADO.Net connection manager:
         *  object rawConnection = Dts.Connections["Sales DB"].AcquireConnection(Dts.Transaction);
         *  SqlConnection myADONETConnection = (SqlConnection)rawConnection;
         *  //Use the connection in some code here, then release the connection
         *  Dts.Connections["Sales DB"].ReleaseConnection(rawConnection);
         *
         * Example of using a File connection manager
         *  object rawConnection = Dts.Connections["Prices.zip"].AcquireConnection(Dts.Transaction);
         *  string filePath = (string)rawConnection;
         *  //Use the connection in some code here, then release the connection
         *  Dts.Connections["Prices.zip"].ReleaseConnection(rawConnection);
         * */
        #endregion


/// <summary>
        /// This method is called when this script task executes in the control flow.
        /// Before returning from this method, set the value of Dts.TaskResult to indicate success or failure.
        /// To open Help, press F1.
        /// </summary>
public void Main()
{
            // TODO: Add your code here

            // TODO: Add your code here

            string datetime = DateTime.Now.ToString("yyyyMMddHHmmss");
            try
            {

                //Declare Variables
                //string SourceFolderPath = Dts.Variables["User::SourceFolder"].Value.ToString();
                string SourceFolderPath = Dts.Variables["https://drfoster.sharefile.com/share/view/s3a91a2b47974340a/"].Value.ToString();
                string FileExtension = Dts.Variables["User::FileExtension"].Value.ToString();
                string FileDelimiter = Dts.Variables["User::FileDelimiter"].Value.ToString();
                string TableName = Dts.Variables["User::DestinationTable"].Value.ToString();
                string ArchiveFolder = Dts.Variables["User::ArchiveFolder"].Value.ToString();
                //string ColumnList = "";

                //Reading file names one by one
                string SourceDirectory = SourceFolderPath;
                string[] fileEntries = Directory.GetFiles(SourceDirectory, "*" + FileExtension);
                foreach (string fileName in fileEntries)
                {

                    SqlConnection myADONETConnection = new SqlConnection();
                    myADONETConnection = (SqlConnection)  (Dts.Connections["DBConn"].AcquireConnection(Dts.Transaction) as SqlConnection);

                    //Writing Data of File Into Table
                    int counter = 0;
                    string line;
                    //MessageBox.Show(fileName);

                   // System.IO.StreamReader SourceFile =  new System.IO.StreamReader(fileName);

                    string myWebString = SourceFolderPath + fileName;
                    WebClient myWebClient = new WebClient();
                    //myWebClient.DownloadFile(myWebString, myLocalPath);
                    myWebClient.DownloadFile(myWebString, ArchiveFolder +"\\" + (fileName.Replace(SourceFolderPath, "")).Replace(FileExtension, "") + "_" + datetime + FileExtension);
                    /*while ((line = SourceFile.ReadLine()) != null)
                    {
                        if (counter > 0)
                        {

                            string query = "Insert into " + TableName + " Values ('" + line.Replace(FileDelimiter, "','") + "')";
                            //MessageBox.Show(query.ToString());
                            SqlCommand myCommand1 = new SqlCommand(query, myADONETConnection);
                            myCommand1.ExecuteNonQuery();
                        }

                        counter++;
                    }

                    SourceFile.Close();
                    */
                    //move the file to archive folder after adding datetime to it
                    // File.Copy(fileName, ArchiveFolder + "\\" + (fileName.Replace(SourceFolderPath, "")).Replace(FileExtension, "") + "_" + datetime + FileExtension);
                    Dts.TaskResult = (int)ScriptResults.Success;
                }
            }
            catch (Exception exception)
            {
                // Create Log File for Errors
                using (StreamWriter sw = File.CreateText(Dts.Variables["User::LogFolder"].Value.ToString()
                    + "\\" + "ErrorLog_" + datetime + ".log"))
                {
                    sw.WriteLine(exception.ToString());
                    Dts.TaskResult = (int)ScriptResults.Failure;
                }

            }


        }

        #region ScriptResults declaration
        /// <summary>
        /// This enum provides a convenient shorthand within the scope of this class for setting the
        /// result of the script.
        ///
        /// This code was generated automatically.
        /// </summary>
        enum ScriptResults
        {
            Success = Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Success,
            Failure = Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure
        };
        #endregion

}
}