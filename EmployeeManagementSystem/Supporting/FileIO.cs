/* 
 *   FILE           : FileIO.cs
 *   PROJECT        : INFO2180-14F - Employee Management System
 *   PROGRAMMER     : Grigory Kozyrev, Ben Lorantfy, Kevin Li, Michael Da Silva
 *   FIRST VERSION  : 2014-11-14
 *   DESCRIPTION    : The functions in this file are used to read and write records to files
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Supporting
{
    /// 
    /// \class FileIO
    ///
    /// \brief The purpose of this class is to abstract reading and writing records to a file
    ///
    /// Class has no constants<br>
    /// It has 2 private data member:<br>
    /// -reader (StreamReader)
    /// -writer (StreamWriter)
    /// 
    /// \author <i>Ben Lorantfy</i>
    ///
    public class FileIO
    {
        string directory;
        string filename;
        string path;
        StreamReader reader;  ///< Used to read from db file
        StreamWriter writter; ///< Used to read from db file
        FileStream readStream;
        FileStream writeStream;

        public FileIO(string directory="\\DBase", string filename="Database.txt")
        {
            this.directory = directory;
            this.filename = filename;

            bool exists = System.IO.Directory.Exists(directory);

            //If not, create it
            if (!exists)
            {
                System.IO.Directory.CreateDirectory(directory);
            }

            path = directory + '\\' + filename;

            if (path != "")
            {
                Open(path);
            }
        }

        /// \brief Opens database file
        /// \details <b>Details</b> 
        /// Opens database file to read and write to
        /// <b>Input</b>
        ///		~ path to database file (string)
        /// \returns Nothing        
        public void Open(string path) {
            try
            {
                this.path = path;
                writeStream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read);
                readStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                reader = new StreamReader(readStream);
                writter = new StreamWriter(writeStream);
            }
            catch(Exception)
            { }
        }

        /// \brief Writes record to database file
        /// \details <b>Details</b> 
        /// Takes a list of fields and appends them into a pipe-delimited string and writes it as a line to a file
        /// <b>Input</b>
        ///		~ list of field values in record (List&lt;string&gt;)
        /// \returns Nothing 
        public void AppendRecord(List<string> fields) {
            try
            {
                string record = string.Join("|", fields.ToArray());
                record += '|';
                writter.WriteLine(record);
                writter.Flush();
            }
            catch(Exception)
            { }
        }

        public void Empty()
        {
            try
            {
                Close();

                writeStream = new FileStream(path, FileMode.Truncate, FileAccess.Write, FileShare.Read);
                writeStream.Close();

                Open(path);
            }
            catch(Exception)
            { }
        }

        /// \brief Reads record from database
        /// \details <b>Details</b> 
        /// Reads a line from the database file and returns a list of field values
        /// <b>Input</b>
        ///		~Nothing
        /// \returns list of field values in record (List&lt;string&gt;)
        public List<string> ReadRecord() {
            List<string> list = null;
            string line = reader.ReadLine();
            if (line != null)
            {
                list = new List<string>(line.Split('|'));
                list.RemoveAt(list.Count - 1);
            }
            else
            {
                // Reset reader to beginning for next read
                Reset();
            }
            return list;
        }

        public void Reset()
        {
            reader.BaseStream.Position = 0;
            reader.DiscardBufferedData();
        }

        /// \brief Converts a pipe-delimited string to a list of values
        /// \details <b>Details</b> 
        /// Reads a line from the database file and returns a list of field values
        /// <b>Input</b>
        ///		~record as pipe-delimited string (string)
        /// \returns list of values (List&lt;string&gt;)
        private List<string> ParseRecord(string record) {
            throw new NotImplementedException();
        }

        /// \brief Closes the database files
        /// \details <b>Details</b> 
        /// Closes the database writer and reader
        /// <b>Input</b>
        ///		~Nothing
        /// \returns Nothing
        public void Close() {
            readStream.Close();
            writeStream.Close();
        }

        ~FileIO()
        {
            Close();
        }
    }
}
