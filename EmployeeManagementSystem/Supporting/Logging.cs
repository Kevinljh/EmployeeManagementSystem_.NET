/* 
 *   FILE           : Logging.cs
 *   PROJECT        : INFO2180-14F - Employee Management System
 *   PROGRAMMER     : Grigory Kozyrev, Ben Lorantfy, Kevin Li, Michael Da Silva
 *   FIRST VERSION  : 2014-11-14
 *   DESCRIPTION    : The functions in this file are used to log events for ease of maintence
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Supporting {
    /// 
    /// \class Logging
    ///
    /// \brief The purpose of this class is to log events to a file, to improve ease of maintence
    ///
    /// Class has no constants<br>
    /// It has 2 private data member:<br>
    /// -reader (StreamReader)
    /// -writer (StreamWriter)
    /// 
    /// \author <i>Ben Lorantfy</i>
    ///
    public static class Logging {
        private static FileStream writeStream;
        private static StreamWriter writer; ///< Used to write to log file
        private static string directory;
        private static string filename;
        private static string path;

        static Logging()
        {
            directory = "Logs";
            filename = "ems." + DateTime.Today.Year.ToString() + '-' + DateTime.Today.Month.ToString() + '-' + DateTime.Today.Day.ToString() + ".log";

            bool exists = System.IO.Directory.Exists(directory);

            //If not, create it
            if (!exists)
            {
                System.IO.Directory.CreateDirectory(directory);
            }

            path = directory + '\\' + filename;

            Start();
        }


        /// \brief Start logging
        /// \details <b>Details</b> 
        /// Opens/creates logging file
        /// <b>Input</b>
        ///		~Nothing
        /// \returns Nothing
        public static void Start() 
        {
            writeStream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read);
            writer = new StreamWriter(writeStream);

            if (!File.Exists(path))
            {
                // Create a file to write to. 
                writer.WriteLine(DateTime.Now.Date + DateTime.Now.TimeOfDay + " Logging file created");
                writer.Flush();

            }
        }

        /// \brief Starts logging
        /// \details <b>Details</b> 
        /// Opens/creates logging file
        /// <b>Input</b>
        ///		~ class name of method that logged (string)
        ///		~ name of method that logged (string)
        ///		~ details/message of log (string)
        /// \returns Nothing
        public static void Log(string className, string methodName, string details)
        {
            // this text is added 
            writer.WriteLine(DateTime.Now.Date + DateTime.Now.TimeOfDay + " [" + methodName + "] " + className + " - " + details);
            writer.Flush();
        }

    }
}
