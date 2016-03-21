/* 
 *   FILE           : Employee.cs
 *   PROJECT        : INFO2180-14F - Employee Management System
 *   PROGRAMMER     : Grigory Kozyrev, Ben Lorantfy, Kevin Li, Michael Da Silva
 *   FIRST VERSION  : 2014-10-10
 *   DESCRIPTION    : The functions in this file are used to 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Supporting;
namespace AllEmployees
{
    /*
     *  NAME    : Employee
     *  PURPOSE : The Employee class has been created to keep track of basic employee information.
     *  The Employee has members to track the First name, last name SIN and date of birth.
     *  The Employee also has the ability to check if the attributes are valid. 
     */

    /// 
    /// \class Employee
    ///
    /// \brief The Employee class has been created to keep track of basic employee information.<br>
    /// The Employee has members to track the First name, last name SIN and date of birth. <br>
    /// The Employee also has the ability to check if the attributes are valid. 
    ///
    /// \author <i>Dev Till Death</i>
    ///
    public abstract class Employee
    {
        /* -------------- ATTRIBUTES ------------ */
        protected string firstName;       ///< Name of the employee
        protected string lastName;        ///< last Name of the employee
        protected string sin;             ///< SIN of the employee
        protected DateTime dateOfBirth;   ///< Date of birth of the employee

        static bool nameTest = false;     ///< Used for unit testing
        
        /* -------------- CONSTRUCTORS ------------ */
        /// \brief Employee(Parent) Regular constructor
        /// \details <b>Details</b> 
        /// Initialize attributes in the class<br>
        ///
        public Employee()
        {

        }

        /// \brief Override Constructor
        /// \details <b>Details</b> 
        /// Assign firstName and last name
        /// <b>Input</b>
        ///			~ firstName (string)
        ///			~ lastName (string)
        ///  
        public Employee(string firstName, string lastName)
        {
            if (!SetFirstName(firstName))
            {
                throw new ArgumentException("Invalid first name");
            }
            if (!SetLastName(lastName))
            {
                throw new ArgumentException("Invalid last name");
            }
        }

        /// \brief Override Constructor
        /// \details <b>Details</b> 
        /// Assign firstName, last name, sin and data of birth
        /// <b>Input</b>
        ///			~ firstName (string)
        ///			~ lastName (string)
        ///         ~ sin (string)
        ///         ~ dateOfBirth (DateTime)
        public Employee(string firstName, string lastName, string sin, DateTime dateOfBirth)
        {
            if (!SetFirstName(firstName))
            {
                throw new ArgumentException("Invalid first name");
            }
            if (!SetLastName(lastName))
            {
                throw new ArgumentException("Invalid last name");
            }
            if (!SetSIN(sin))
            {
                throw new ArgumentException("Invalid sin");
            }
            if (!SetDateOfBirth(dateOfBirth.ToString()))
            {
                throw new ArgumentException("Invalid date of birth");
            }
        }

        /* -------------- GETTERS ------------ */
        public String GetFirstName()
        {
            return firstName;
        }

        public String GetLastName()
        {
            return lastName;
        }

        public String GetSIN()
        {
            return sin;
        }

        public DateTime GetDateOfBirth()
        {
            return dateOfBirth;
        }

        public static bool GetNameTest
        {
            get { return nameTest; }
        }

        

        /* -------------- MUTATORS ------------ */
        /// \brief Sets First Name field
        /// \details <b>Details</b> 
        /// Sets First Name if validation passes<br>
        ///	<b>Input</b> 
        ///			~ new First Name (string)
        /// \return
        ///			~ first name if success<br>
        ///			~ throw exception otherwise
        ///			
        public bool SetFirstName(string firstName)
        {
            bool result = nameValidation(firstName);
            if (result)
            {
                this.firstName = firstName;
            }
            else
            {
                Logging.Log(this.ToString(), "SetFirstName", "Invalid First Name Input");
            }
            return result;
        }

        /// \brief Sets Last Name field
        /// \details <b>Details</b> 
        /// Sets Last Name if validation passes<br>
        ///	<b>Input</b> 
        ///			~ new Last Name (string)
        /// \return
        ///			~ Last name if success<br>
        ///			~ throw exception otherwise
        ///			
        public bool SetLastName(string lastName)
        {
            bool result = nameValidation(lastName);
            if (result)
            {
                this.lastName = lastName;
            }
            else
            {
                Logging.Log(this.ToString(), "SetLastName", " Invalid Last Name Input");
            }
            return result;
        }

        /// \brief Sets SIN field
        /// \details <b>Details</b> 
        /// Sets SIN if validation passes<br>
        ///	<b>Input</b> 
        ///			~ new Last Name (string)
        /// \return
        ///			~ Last name if success<br>
        ///			~ throw exception otherwisess
        ///	
        public bool SetSIN(string sin)
        {
            bool result = sinValidation(ref sin);
            if (result)
            {
                this.sin = sin;
            }
            else
            {
                Logging.Log(this.ToString(), "SetSIN", "Invalid Social Insurance Number Input");
            }
            return result;
        }

        /// \brief Sets Date Of Birth field
        /// \details <b>Details</b> 
        /// Sets Date Of Birth if validation passes<br>
        ///	<b>Input</b> 
        ///			~ new Last Name (string)
        /// \return
        ///			~ Date of Birth if success<br>
        ///			~ throw exception otherwise
        ///
        public bool SetDateOfBirth(string dateOfBirth)
        {
            bool result = dateTimeValidation(ref dateOfBirth);
            if (result)
            {
                //how to output?  ->  Console.WriteLine(tempDateOfBirth.ToString("yyyy-MM-dd"));
                DateTime tempdateOfBirth = DateTime.Parse(dateOfBirth);
                this.dateOfBirth = tempdateOfBirth;
            }
            else
            {
                Logging.Log(this.ToString(), "SetDateOfBirth", "Invalid Date Of Birth Input");
            }
            return result;
        }





        /* -------------- METHODS ------------ */
        //abstract functions, will be implemented in child class
        public abstract bool Validate();
        public abstract string Details();

        public abstract Employee Copy();
        
        /// \brief nameValidation method for the Employee class
        /// \details <b>Details</b> 
        /// Checks the last name against a regular expression<br>Name can only contain letters, spaces, dashes and periods.
        /// <b>Input</b>
        ///			~ new name (string)
        /// \return
        ///			~ true if success<br>
        ///			~ false otherwise
        public bool nameValidation(string name)
        {
            bool valid = false;
            if (name != "")
            {
                if (Regex.Match(name, @"^[a-zA-Z-`]+$").Success)
                {
                    valid = true;
                    nameTest = valid;
                }
                else
                {
                    valid = false;
                    nameTest = valid;
                }
            }
            else
            {
                valid = true;
                nameTest = valid;
            }
            return valid;
        }

        public virtual List<string> Data()
        {
            List<string> data = new List<string>();

            data.Add(firstName);
            data.Add(lastName);
            data.Add(sin);
            data.Add(dateOfBirth.Date.ToString("yyyy-MM-dd"));

            return data;
        }

        /// \brief sinValidation method for the Employee class
        /// \details <b>Details</b> 
        /// Checks the Sin against a regular expression<br>Pass: nothing but a 9 digit number.
        /// <b>Input</b>
        ///			~ new sin (string)
        /// \return
        ///			~ true if success<br>
        ///			~ false otherwise

        //this function needs to be override in contract employee		
       public virtual bool sinValidation(ref string sin)
        {
            bool valid = false;
            int sum = 0;
            //trim spaces
            sin = sin.Trim();
            sin = sin.Replace(" ", "").Replace("-", "").Replace(".", "");

            //check for empty
            if (sin == "" || sin == "0")
            {
                valid = true;
            }
            else
            {
                if (Regex.Match(sin, @"^[0-9]{9}$").Success)
                {
                    //check for valid sin number
                    sum = sin.Where((e) => e >= '0' && e <= '9')
                    .Reverse()
                    .Select((e, i) => ((int)e - 48) * (i % 2 == 0 ? 1 : 2))
                    .Sum((e) => e / 10 + e % 10);

                    if (sum % 10 == 0)
                    {
                        valid = true;
                        sin = sin.Insert(3, " ");
                        sin = sin.Insert(7, " ");
                    }
                }
            }
            return valid;
        }

        /// \brief dateTimeValidation method for the Employee class
        /// \details <b>Details</b> 
        /// Checks the date whether it's right format or not<br>Pass: right format yyyy-mm-dd.
        /// <b>Input</b>
        ///			~ new sin (string)
        /// \return
        ///			~ true if success<br>
        protected bool dateTimeValidation(ref string date)
        {
            DateTime tempDateOf;
            bool valid = false;
            //check if it's empty
            if (date != "")
            {
                if (date == "N/A" || date == "n/a")
                {
                    //assign a value that indicates empty
                    date = "1.1.0001";
                    valid = true;
                }
                else if (DateTime.TryParse(date, out tempDateOf))
                {
                    valid = true;
                }
                else
                {
                    valid = false;
                }
            }
            else
            {
                date = "1.1.0001";
                valid = true;
            }
            return valid;
        }

        /// \brief Double check date time
        /// \details <b>Details</b> 
        ///  Check whether the hire date, termination date or some other date is before date of birth<br>
        ///	<b>Input</b> 
        ///			~ new date (DateTime)
        /// \return
        ///			~ Bool result, valid date reture true, vise versa<br>
        ///
        protected bool CrossFieldCheckDate(DateTime date)
        {
            bool result = false;

            if (date > dateOfBirth)
            {
                result = true;
            }
            return result;
        }
    }
}
