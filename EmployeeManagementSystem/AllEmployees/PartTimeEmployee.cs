/* 
 *   FILE           : PartTimeEmployee.cs
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
     *  NAME    : PartTimeEmployee
     *  PURPOSE : The PartTimeEmployee class has been created to keep track of information 
     *  that applies to Part Time employee specifically.
     *  The PartTimeEmployee has members to track the date of hire, date of termination, and hourly rate.
     *  The PartTimeEmployee also has the ability to check if the attributes are valid. 
     */

    /// 
    /// \class PartTimeEmployee
    ///
    /// \brief The PartTimeEmployee class has been created to keep track of information <br>
    ///  that applies to Contract employee specifically.
    ///
    /// The PartTimeEmployee class has been created to keep track of information <br>
    /// that applies to Part Time employee specifically.<br>
    /// The PartTimeEmployee has members to track the date of hire, date of termination, and hourly rate.<br>
    /// The PartTimeEmployee also has the ability to check if the attributes are valid.
    ///
    /// \author <i>Dev Till Death</i>
    ///

    public class PartTimeEmployee : Employee
    {

        /* -------------- ATTRIBUTES ------------ */
        private DateTime dateOfHire;            ///< Part Time Employees Date of Hire
        private DateTime dateOfTermination;     ///< Part Time Employees Date of Termiination
        private double hourlyRate = -1;              ///< Part Time Employees Hourly wage

        /* -------------- CONSTRUCTORS ------------ */
        /// \brief PartTimeEmployee(Child) Regular constructor
        /// \details <b>Details</b> 
        /// Initialize attributes in the class<br>
        ///
        public PartTimeEmployee()
        {

        }

        /// \brief PartTimeEmployee Override(Child) Constructor
        /// \details <b>Details</b> 
        /// Assign firstName and last name
        /// <b>Input</b>
        ///			~ firstName (string)
        ///			~ lastName (string)
        public PartTimeEmployee(string firstName, string lastName)
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

        /// \brief PartTimeEmployee(Child) Override Constructor
        /// \details <b>Details</b> 
        /// Assign firstName and last name, data of birth, data of hire, data of termination and salary
        /// <b>Input</b>
        ///			~ firstName (string)
        ///			~ lastName (string)
        ///			~ sin (string)
        ///			~ dateOfBirth (DateTime)
        ///			~ dateOfHire (DateTime)
        ///			~ dateOfTermination (DateTime)
        ///			~ hourlyRate (double)
        ///
        public PartTimeEmployee(string firstName
                                , string lastName
                                , string sin
                                , DateTime dateOfBirth
                                , DateTime dateOfHire
                                , DateTime dateOfTermination
                                , double hourlyRate
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
            if (!SetHourlyRate(hourlyRate.ToString()))
            {
                throw new ArgumentException("Invalid hourly  rate");
            }
        }


        /* -------------- MUTATORS ------------ */
        /// \brief Set Date of Hire
        /// \details <b>Details</b> 
        /// Sets Date of Hire for Part time employees if Valid date is entered <br>
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
                Logging.Log(this.ToString(), "SetDateOfHire", "Invalid Date Of Hire Input");
            }
            return result;
        }

        /// \brief Set Date of Termination
        /// \details <b>Details</b> 
        /// Sets Date of Termination for Part time employees if Valid date is entered <br>
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

        /// \brief Set Hourly Rate
        /// \details <b>Details</b> 
        /// Sets Hourly Rate for part time employees if Valid double is entered <br>
        ///	<b>Input</b> 
        ///			~ new Hourly Rate (double)
        /// \return
        ///			~ Hourly Rate if success<br>
        ///			~ throw exception otherwise
        ///	
        public bool SetHourlyRate(string hourlyRate)
        {
            double result;
            bool valid = false;
            //try to convert string to double
            if (double.TryParse(hourlyRate, out result))
            {
                if (result >= 0.00)
                {
                    this.hourlyRate = result;
                    valid = true;
                }
                else
                {
                    valid = false;
                    Logging.Log(this.ToString(), "SetHourlyRate", "Invalid Hourly Rate Input");
                }
            }
            else
            {
                valid = false;
                Logging.Log(this.ToString(), "SetHourlyRate", "Invalid Hourly Rate Input");
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
                && dateOfHire != new DateTime() && hourlyRate != 0 && HireAndTerminationDateCompare())
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
            result = "Part Time Employee\n" +
                     "Last: " + lastName + " Fist: " + firstName + "\n" +
                     "Born: " + dateOfBirth.ToString("yyyy-MM-dd") + " SIN: " + sin + "\n" +
                     "Hired: " + dateOfHire.ToString("yyyy-MM-dd") + " Terminated: " + dateOfTermination.ToString("yyyy-MM-dd") + "\n" +
                     "Hourly Rate: " + hourlyRate.ToString();

            Logging.Log(this.ToString(), "Details", result);

            return result;
        }

        public override List<string> Data()
        {
            List<string> data = new List<string>(base.Data());

            data.Insert(0, "PT");
            data.Add(dateOfHire.Date.ToString("yyyy-MM-dd"));
            data.Add(dateOfTermination.Date.ToString("yyyy-MM-dd"));
            data.Add(hourlyRate.ToString());

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

            if ((dateOfTermination > dateOfHire)  || (dateOfTermination == new DateTime()))
            {
                result = true;
            }

            return result;
        }

        public override Employee Copy()
        {
            return (PartTimeEmployee)this.MemberwiseClone();
        }
    }
}
