/* 
 *   FILE           : Database.cs
 *   PROJECT        : INFO2180-14F - Employee Management System
 *   PROGRAMMER     : Grigory Kozyrev, Ben Lorantfy, Kevin Li, Michael Da Silva
 *   FIRST VERSION  : 2014-11-14
 *   DESCRIPTION    : The functions in this file are used to save different types of employees in database,
 *                  : and manage them (Remover, Update, Find)
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AllEmployees;
using Supporting;


namespace TheCompany
{
    /// 
    /// \class Database
    ///
    /// \brief The purpose of this class is to handle Employee data and manage it
    ///
    /// Class has no constants<br>
    /// It has 1 private data member:<br>
    /// -database (List<Employee>)
    ///
    /// \author <i>Greg Kozyrev</i>
    ///
    public class Database
    {
        public const String kAnyTag = "<ANY>";
        public const String kFullTimeEmp = "FT";
        public const String kPartTimeEmp = "PT";
        public const String kContractEmp = "CT";
        public const String kSeasonalEmp = "SN";

        FileIO fileManager = new FileIO("DBase", "Database.txt");
        List<Employee> database = new List<Employee>();     ///< Actuall database

        /// \brief Method to add new Employee
        /// \details <b>Details</b> 
        /// Tryes to add new employee into database. Uses deault Add() method in List container<br>
        /// <b>Input</b>
        ///		~newMember (Employee)
        /// \return
        ///			~ true if success<br>
        ///			~ false otherwise
        public Boolean Add(Employee newMember)
        {
            Boolean status = false;

            try
            {
                if (newMember.Validate() == true)
                {
                    database.Add(newMember);
                    status = true;
                    Logging.Log("Database", "Add", newMember.Details() + "\n WAS ADDED");
                }    
            }
            catch (Exception e)
            {
                Logging.Log("Database", "Add", newMember.Details() + "\nCANNOT BE ADDED" + e.ToString());
            }

            return status;
        }

        /// \brief Method to remove an Employee
        /// \details <b>Details</b> 
        /// Tryes to delete entered employee from the database. Uses deault Remove() method in List container<br>
        /// <b>Input</b>
        ///		~member (Employee)
        /// \return
        ///			~ true if success<br>
        ///			~ false otherwise
        public Boolean Remove(Employee member)
        {
            Boolean status = false;

            try
            {
                database.Remove(member);
                status = true;
                Logging.Log("Database", "Add", member.Details() + "\n WAS REMOVED");
            }
            catch (Exception e)
            {
                Logging.Log("Database", "Remove", member.Details() + "\nCANNOT BE REMOVED" + e.ToString());
            }

            return status;
        }

        /// \brief Method to update an Employee
        /// \details <b>Details</b> 
        /// Tryes to change data of specified employee to a new one. Copies data from entered one, into required employee<br>
        /// <b>Input</b>
        ///		~member (Employee)<br>
        ///		~newData (Employee)
        /// \return
        ///			~ true if success<br>
        ///			~ false otherwise
        public Boolean Update(Employee member, Employee newData)
        {
            Boolean status = false;

            try
            {
                int index = database.FindIndex(
                delegate (Employee obj)
                {
                    return obj.Equals(member);
                });

                if (index == -1)
                {
                    throw new Exception(String.Format(
                    "Employee not found"));
                }

                database[index] = newData;

                status = true;
                Logging.Log("Database", "Add", member.Details() + "\n WAS UPDATED");
            }
            catch (Exception e)
            {
                Logging.Log("Database", "Update", member.Details() + "\nCANNOT BE UPDATED TO\n" + newData.Details() + "\n" + e.ToString());
            }

            return status;
        }

        /// \brief Searches for Employees in the database
        /// \details <b>Details</b> 
        /// Performs search by 4 parameters. Returns List of all employees with the same parameters<br>
        /// <b>Input</b>
        ///		~firstName (String)<br>
        ///		~lastName (String)<br>
        ///		~SIN (String)<br>
        ///		~dateOfBirth (DateTime)
        /// \return
        ///			~ All founded employees (List<Employee>)
        public List<Employee> Search(String employeeType, String firstName, String lastName, String SIN, String dateOfBirth)
        {
            List<Employee> founded = new List<Employee>();

            try
            {
                for (int i = 0; i < database.Count; i++)
                {
                    founded = database.FindAll(
                    delegate(Employee obj)
                    {
                        bool theSame = ((obj.GetType() == typeof(FullTimeEmployee)) && (employeeType.Equals(kFullTimeEmp))) ||
                        ((obj.GetType() == typeof(PartTimeEmployee)) && (employeeType.Equals(kPartTimeEmp))) ||
                        ((obj.GetType() == typeof(ContractEmployee)) && (employeeType.Equals(kContractEmp))) ||
                        ((obj.GetType() == typeof(SeasonalEmployee)) && (employeeType.Equals(kSeasonalEmp))) ||
                        (employeeType.Equals(kAnyTag));

                        theSame = theSame && ((obj.GetFirstName().Equals(firstName)) || (firstName.Equals(kAnyTag)));
                        theSame = theSame && ((obj.GetLastName().Equals(lastName)) || (lastName.Equals(kAnyTag)));
                        theSame = theSame && ((obj.GetSIN().Equals(SIN)) || (SIN.Equals(kAnyTag)));
                        theSame = theSame && ((obj.GetDateOfBirth().ToString().Equals(dateOfBirth)) || (dateOfBirth.Equals(kAnyTag)));

                        return theSame;
                    });
                }
            }
            catch (Exception e)
            {
                Logging.Log("Database", "Add", "Cannot search with type = " + employeeType +
                " firstName = " + firstName + " lastName = " + lastName + " SIN = " + SIN + " dateOfBirth " + dateOfBirth + " | " + e.ToString());
            }

            return founded;
        }

        public void ReadDatabase()
        {
            List<string> data = null;
            bool status = false;

            do
            {
                data = null;
                data = fileManager.ReadRecord();

                if (data != null)
                {
                    status = ParseParameters(data);

                    if (status == false)
                    {
                        string returnData = "";

                        foreach (string str in data)
                        {
                            returnData += str + "|";
                        }
                        Logging.Log("Database", "ReadDatabase", returnData + " has wrong data");
                    }
                }
            }
            while (data != null);
        }

        public void SaveDatabase()
        {
            fileManager.Empty();

            foreach(Employee employee in database)
            {     
                fileManager.AppendRecord(employee.Data());
            }
        }

        private bool ParseParameters(List<string> parameters)
        {
            bool status = false;

            if (parameters.Count > 1)
            {
                string type = parameters[0];
                parameters.RemoveAt(0);

                switch(type)
                {
                    case kFullTimeEmp:
                        status = GenerateFullTimeEmp(parameters);
                        break;
                    case kPartTimeEmp:
                        status = GeneratePartTimeEmp(parameters);
                        break;
                    case kContractEmp:
                        status = GenerateContractEmp(parameters);
                        break;
                    case kSeasonalEmp:
                        status = GenerateSeasonalEmp(parameters);
                        break;
                    default:
                        status = false;
                        break;
                }
            }

            return status;
        }

        private bool GenerateFullTimeEmp(List<string> parameters)
        {
            bool status = false;

            if (parameters.Count == 7)
            {
                try
                {
                    DateTime dateOfBirth = Convert.ToDateTime(parameters[3]);
                    DateTime dateOfHire = Convert.ToDateTime(parameters[4]);
                    DateTime dateOfTermination;
                    if ((parameters[5].Equals("N/A")) || (parameters[5].Equals("n/a")))
                    {
                        dateOfTermination = new DateTime();
                    }
                    else
                    {
                        dateOfTermination = Convert.ToDateTime(parameters[5]);
                    }
                    Double salary = Convert.ToDouble(parameters[6]);

                    FullTimeEmployee tempEmployee = new FullTimeEmployee(parameters[0], parameters[1], parameters[2], dateOfBirth, dateOfHire, dateOfTermination, salary);

                    status = Add(tempEmployee);
                }
                catch(Exception e)
                {
                    status = false;
                }
            }

            return status;
        }

        private bool GeneratePartTimeEmp(List<string> parameters)
        {
            bool status = false;

            if (parameters.Count == 7)
            {
                try
                {
                    DateTime dateOfBirth = Convert.ToDateTime(parameters[3]);
                    DateTime dateOfHire = Convert.ToDateTime(parameters[4]);
                    DateTime dateOfTermination;
                    if ((parameters[5].Equals("N/A")) || (parameters[5].Equals("n/a")))
                    {
                        dateOfTermination = new DateTime();
                    }
                    else
                    {
                        dateOfTermination = Convert.ToDateTime(parameters[5]);
                    }
                    Double hourlyRate = Convert.ToDouble(parameters[6]);

                    PartTimeEmployee tempEmployee = new PartTimeEmployee(parameters[0], parameters[1], parameters[2], dateOfBirth, dateOfHire, dateOfTermination, hourlyRate);

                    status = Add(tempEmployee);
                }
                catch (Exception e)
                {
                    status = false;
                }
            }

            return status;
        }

        private bool GenerateSeasonalEmp(List<string> parameters)
        {
            bool status = false;

            if (parameters.Count == 6)
            {
                try
                {
                    DateTime dateOfBirth = Convert.ToDateTime(parameters[3]);
                    Double piecePay = Convert.ToDouble(parameters[5]);

                    SeasonalEmployee tempEmployee = new SeasonalEmployee(parameters[0], parameters[1], parameters[2], dateOfBirth, parameters[4], piecePay);

                    status = Add(tempEmployee);
                }
                catch (Exception e)
                {
                    status = false;
                }
            }

            return status;
        }

        private bool GenerateContractEmp(List<string> parameters)
        {
            bool status = false;

            if (parameters.Count == 7)
            {
                try
                {
                    DateTime dateOfBirth = Convert.ToDateTime(parameters[3]);
                    DateTime contractStartDate = Convert.ToDateTime(parameters[4]);
                    DateTime contractEndDate = Convert.ToDateTime(parameters[5]);
                    Double fixedContractAmount = Convert.ToDouble(parameters[6]);

                    ContractEmployee tempEmployee = new ContractEmployee(parameters[0], parameters[1], parameters[2], dateOfBirth, contractStartDate, contractEndDate, fixedContractAmount);

                    status = Add(tempEmployee);
                }
                catch (Exception e)
                {
                    status = false;
                }
            }

            return status;
        }
    }
}
