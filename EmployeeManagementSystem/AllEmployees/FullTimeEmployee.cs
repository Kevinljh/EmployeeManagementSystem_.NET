/* 
 *   FILE           : FullTimeEmployee.cs
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
     *  NAME    : FullTimeEmployee
     *  PURPOSE : The FullTimeEmployee class has been created to keep track of information 
     *  that applies to Full Time employee specifically.
     *  The FullTimeEmployee has members to track the date of hire, date of termination, and employees salary.
     *  The FullTimeEmployee also has the ability to check if the attributes are valid. 
     */

    /// 
    /// \class FullTimeEmployee
    ///
    /// \brief The FullTimeEmployee class has been created to keep track of information <br>
    ///  that applies to Full Time employee specifically.
    ///
    /// The FullTimeEmployee class has been created to keep track of information <br>
    /// that applies to Full Time employee specifically.<br>
    /// The FullTimeEmployee has members to track the date of hire, date of termination, and employees salary.<br>
    /// The FullTimeEmployee also has the ability to check if the attributes are valid. 
    ///
    /// \author <i>Dev Till Death</i>
    ///
    public class FullTimeEmployee : Employee
    {

        /* -------------- ATTRIBUTES ------------ */
        private DateTime dateOfHire;            ///< Full Time Employees Date of Hire
        private DateTime dateOfTermination;     ///< Full Time Employees Date of Termination
        private double salary = -1;                  ///< Full Time Employee Salary

        static bool salaryTest = false;      ///< Used for unit testing
        static bool dateOfTerm = false;     ///< Used for unit testing
        /* -------------- CONSTRUCTORS ------------ */
        /// \brief FullTimeEmployee(Child) regular constructor
        /// \details <b>Details</b> 
        /// Initialize attributes in the class<br>
        ///
        public FullTimeEmployee()
        {

        }

        /// \brief FullTimeEmployee Override(Child) Constructor
        /// \details <b>Details</b> 
        /// Assign firstName and last name
        /// <b>Input</b>
        ///			~ firstName (string)
        ///			~ lastName (string)
        ///         ~ dateOfBirth (DateTime)
        public FullTimeEmployee(string firstName, string lastName)
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

        /// \brief FullTimeEmployee(Child) Override Constructor
        /// \details <b>Details</b> 
        /// Assign firstName and last name, data of birth, data of hire, data of termination and salary
        /// <b>Input</b>
        ///			~ firstName (string)
        ///			~ lastName (string)
        ///			~ sin (string)
        ///			~ dateOfBirth (DateTime)
        ///			~ dateOfHire (DateTime)
        ///			~ dateOfTermination (DateTime)
        ///			~ salary (double)
        ///
        public FullTimeEmployee(string firstName
                                , string lastName
                                , string sin
                                , DateTime dateOfBirth
                                , DateTime dateOfHire
                                , DateTime dateOfTermination
                                , double salary
                               )
            : base(
              firstName
             , lastName
             , sin
             , dateOfBirth)
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
            if (!SetDateOfHire(dateOfHire.ToString()))
            {
                throw new ArgumentException("Invalid date of hire");
            }
            if (!SetDateOfTermination(dateOfTermination.ToString()))
            {
                throw new ArgumentException("Invalid date of termination");
            }
            if (!SetSalary(salary.ToString()))
            {
                throw new ArgumentException("Invalid salary");
            }
        }
        /* -------------- GETTERS ------------ */
        public static bool GetSalaryTest
        {
            get { return salaryTest; }
        }
        public static bool GetDateOfTermTest
        {
            get { return dateOfTerm; }
        }
        /* -------------- PUBLIC ACCESSORS / MUTATORS ------------ */
        /// \brief Set Date of Hire
        /// \details <b>Details</b> 
        /// Sets Date of Hire for Full time employees if Valid date is entered <br>
        ///	<b>Input</b> 
        ///			~ new Date of Hire (string)
        /// \return
        ///			~ Date of Hire if success<br>
        ///			~ throw exception otherwise
        ///	
        public bool SetDateOfHire(string dateOfHire)
        {
            bool result = false;

            if (dateTimeValidation(ref dateOfHire))
            {
                //how to output?  ->  Console.WriteLine(tempDateOfBirth.ToString("yyyy-MM-dd"));
                DateTime tempDate = DateTime.Parse(dateOfHire);
                this.dateOfHire = tempDate;
                result = true;                
            }
            else
            {
                Logging.Log(this.ToString(), "SetDateOfHire", "Invalid Date of hire Input");
            }
            return result;
        }

        /// \brief Set Date of Termination
        /// \details <b>Details</b> 
        /// Sets Date of Termination for Full time employees if Valid date is entered <br>
        ///	<b>Input</b> 
        ///			~ new Date of Termination (string)
        /// \return
        ///			~ Date of Termination if success<br>
        ///			~ throw exception otherwise
        ///	
        public bool SetDateOfTermination(string dateOfTermination)
        {
            bool result = false;

            if (dateTimeValidation(ref dateOfTermination))
            {
                //how to output?  ->  Console.WriteLine(tempDateOfBirth.ToString("yyyy-MM-dd"));
                DateTime tempDate = DateTime.Parse(dateOfTermination);
                this.dateOfTermination = tempDate;
                result = true;
            }
            else
            {
                Logging.Log(this.ToString(), "SetDateOfTermination", "Invalid Date of Termination Input");
            }
            return result;
        }

        /// \brief Set Salary
        /// \details <b>Details</b> 
        /// Sets Salary for Full time employees if Valid double is entered <br>
        ///	<b>Input</b> 
        ///			~ new Salary (string)
        /// \return
        ///			~ Salary if success<br>
        ///			~ throw exception otherwise
        ///	
        public bool SetSalary(string salary)
        {
            double result;
            bool valid = false;
            if (double.TryParse(salary, out result))
            {
                if (result >= 0.00)
                {
                    this.salary = result;
                    valid = true;
                    salaryTest = valid;
                }
                else
                {
                    valid = false;
                    salaryTest = valid;
                    Logging.Log(this.ToString(), "SetSalary", "Invalid Salary Input");
                }
            }
            else
            {
                valid = false;
                salaryTest = valid;

                Logging.Log(this.ToString(), "SetSalary", "Invalid Salary Input");
            
            }
            return valid;
        }


        /* -------------- METHODS ------------ */

        /// \brief Validate all attrubutes
        /// \details <b>Details</b> 
        /// Validates all the attrubutes before go to container class <br>
        ///	<b>Input</b> 
        ///			~ none
        /// \return
        ///			~ Boolean result indicates whether success of not<br>
        ///
        public override bool Validate()
        {
            bool result = false;
            //check every attribute making sure they are not empty and valid
            if (lastName != "" && firstName != "" && dateOfBirth != new DateTime() && sin != "" && sin != "0"
                && dateOfHire != new DateTime() && salary != 0 && HireAndTerminationDateCompare())
            {
                DateTime maxDate = DateTime.Now;
                DateTime minDate = new DateTime(1900, 1, 1);

                //date of terminatino might be null
                if (dateOfTermination == new DateTime())
                {
                    //not before birthday
                    if (CrossFieldCheckDate(dateOfHire))
                    {
                        if ((dateOfHire < maxDate) && (dateOfHire > minDate) && (dateOfBirth < maxDate) && (dateOfBirth > minDate))
                        {
                            result = true;
                            Logging.Log(this.ToString(), "Validate", "Last Name: " + lastName + "First Name: " + firstName + "SIN: " + sin + "Valid");
                        }
                    }
                }
                else
                {
                    //not before birthday
                    if (CrossFieldCheckDate(dateOfHire) && CrossFieldCheckDate(dateOfTermination))
                    {
                        if ((dateOfHire < maxDate) && (dateOfHire > minDate) && (dateOfBirth < maxDate) && (dateOfBirth > minDate) && (dateOfTermination < maxDate) && (dateOfTermination > minDate))
                        {
                            result = true;
                            Logging.Log(this.ToString(), "Validate", "Last Name: " + lastName + "First Name: " + firstName + "SIN: " + sin + "Valid");
                        }
                    }
                }
            }
            else
            {
                Logging.Log(this.ToString(), "Validate", "Last Name: " + lastName + "First Name: " + firstName + "SIN: " + sin + "Invalid");
            }
            return result;
        }

        /// \brief Format attributes for output 
        /// \details <b>Details</b> 
        ///  Append all the attribute into string, so that ui can print it out <br>
        ///	<b>Input</b> 
        ///			~ None
        /// \return
        ///			~ String result contained all the attributes for output<br>
        ///
        public override string Details()
        {
            string result;

            //append string for output
            result = "Full Time Employee\n" +
                     "Last: " + lastName + " Fist: " + firstName + "\n" +
                     "Born: " + dateOfBirth.ToString("yyyy-MM-dd") + " SIN: " + sin + "\n" +
                     "Hire: " + dateOfHire.ToString("yyyy-MM-dd") + " Terminated: " + dateOfTermination.ToString("yyyy-MM-dd") + "\n" +
                     "Salary: " + salary.ToString();

            Logging.Log(this.ToString(), "Details", result);

            return result;
        }

        public override List<string> Data()
        {
            List<string> data = new List<string>(base.Data());

            data.Insert(0, "FT");
            data.Add(dateOfHire.Date.ToString("yyyy-MM-dd"));
            data.Add(dateOfTermination.Date.ToString("yyyy-MM-dd"));
            data.Add(salary.ToString());

            return data;
        }
        /// \brief Compare hire date with termination date
        /// \details <b>Details</b> 
        ///  Compare if contract start date is before end date. <br>
        ///	<b>Input</b> 
        ///			~ new endDate (DateTime)
        /// \return
        ///			~ Boolean valid indicates whether the input is valid or not<br>
        ///
        private bool HireAndTerminationDateCompare()
        {
            bool result = false;

            if ((dateOfTermination > dateOfHire) || (dateOfTermination.Equals(new DateTime())))
            {
                result = true;
            }

            return result;
        }

        public override Employee Copy()
        {
            return (FullTimeEmployee)this.MemberwiseClone();
        }
    }
}
