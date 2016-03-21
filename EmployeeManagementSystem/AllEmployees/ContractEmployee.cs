/* 
 *   FILE           : ContractEmployee.cs
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
     *  NAME    : ContractEmployee
     *  PURPOSE : The ContractEmployee class has been created to keep track of information 
     *  that applies to Contract employee specifically.
     *  The ContractEmployee has members to track the contract start date, contract end date, and contract amount.
     *  The ContractEmployee also has the ability to check if the attributes are valid. 
     */

    /// 
    /// \class ContractEmployee
    ///
    /// \brief The ContractEmployee class has been created to keep track of information <br>
    ///  that applies to Contract employee specifically.<br>
    /// The ContractEmployee class has been created to keep track of information <br>
    ///  that applies to Contract employee specifically.<br>
    /// The ContractEmployee has members to track the contract start date, contract end date, and contract amount.<br>
    /// The ContractEmployee also has the ability to check if the attributes are valid.  
    ///
    /// \author <i>Dev Till Death</i>
    ///

    public class ContractEmployee : Employee
    {
        /* -------------- ATTRIBUTES ------------ */
        private DateTime contractStartDate;     ///< Contract Start Date of the employee
        private DateTime contractEndDate;       ///< Contract End Date of the employee
        private double fixedContractAmount = -1;     ///< Fixed Contract Amount

        static bool sinTest = false;      ///< Used for unit testing
        static bool fixedContractAmountTest = false;    ///< used for unit testing
        /* -------------- CONSTRUCTORS ------------ */
        /// \brief ContractEmployee(Child) Regular constructor
        /// \details <b>Details</b> 
        /// Initialize attributes in the class<br>
        ///
        public ContractEmployee()
        {

        }

        /// \brief ContractEmployee(Child) Override Constructor
        /// \details <b>Details</b> 
        /// Assign firstName and last name
        /// <b>Input</b>
        ///			~ firstName (string)
        ///			~ lastName (string)
        public ContractEmployee(string firstName, string lastName)
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

        /// \brief SeasonalEmployee(Child) Override Constructor
        /// \details <b>Details</b> 
        /// Assign firstName and last name, data of birth, data of hire, data of termination and salary
        /// <b>Input</b>
        ///			~ firstName (string)
        ///			~ lastName (string)
        ///			~ sin (string)
        ///			~ dateOfBirth (DateTime)
        ///			~ dateOfHire (DateTime)
        ///			~ dateOfTermination (DateTime)
        ///	
        public ContractEmployee(string firstName
                                , string lastName
                                , string sin
                                , DateTime dateOfBirth
                                , DateTime contractStartDate
                                , DateTime contractEndDate
                                , double fixedContractAmount)
            : base(
             firstName
            , lastName
            , sin
            , dateOfBirth
            )
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
            if (!SetContractStartDate(contractStartDate.ToString()))
            {
                throw new ArgumentException("Invalid contract start date");
            }
            if (!SetContractEndDate(contractEndDate.ToString()))
            {
                throw new ArgumentException("Invalid contract end date");
            }
            if (!SetFixedContractAmount(fixedContractAmount.ToString()))
            {
                throw new ArgumentException("Invalid fixed contract amount");
            }
        }
        /* -------------- GETTERS ------------ */
        public static bool GetSINTest
        {
            get { return sinTest; }
        }
        public static bool GetFixedContractAmount
        {
            get { return fixedContractAmountTest; }
        }
        /* -------------- MUTATORS ------------ */
        /// \brief Set contract Start Date
        /// \details <b>Details</b> 
        /// Sets Contract Start Date if Valid date is entered <br>
        ///	<b>Input</b> 
        ///			~ new Contract Start Date (DateTime)
        /// \return
        ///			~ Contract Start Date if success<br>
        ///			~ throw exception otherwise
        ///	
        public bool SetContractStartDate(string contractStartDate)
        {
            bool result = false;

            if (dateTimeValidation(ref contractStartDate))
            {
                //how to output?  ->  Console.WriteLine(tempDateOfBirth.ToString("yyyy-MM-dd"));
                DateTime tempDate = DateTime.Parse(contractStartDate);
                this.contractStartDate = tempDate;
                result = true;
            }
            else
            {
                Logging.Log(this.ToString(), "SetContractStartDate", "Invalid Contract Start Date Input");
            }
            return result;
        }

        /// \brief Set Contract End Date
        /// \details <b>Details</b> 
        /// Sets Contract End Date if Valid date is entered <br>
        ///	<b>Input</b> 
        ///			~ new Contract End Date (DateTime)
        /// \return
        ///			~ Contract End tDate if success<br>
        ///			~ throw exception otherwise
        ///
        public bool SetContractEndDate(string contractEndDate)
        {
            bool result = false;

            if (dateTimeValidation(ref contractEndDate))
            {
                //how to output?  ->  Console.WriteLine(tempDateOfBirth.ToString("yyyy-MM-dd"));
                DateTime tempDate = DateTime.Parse(contractEndDate);

                this.contractEndDate = tempDate;
                result = true;
            }
            else
            {
                Logging.Log(this.ToString(), "SetContractEndDate", "Invalid Contract End Date Input");
            }
            return result;
        }

        /// \brief Set Contract Amount
        /// \details <b>Details</b> 
        /// Sets Contract Amount if Valid date is entered <br>
        ///	<b>Input</b> 
        ///			~ new Contract Amount (double)
        /// \return
        ///			~ Contract Amount tDate if success<br>
        ///			~ throw exception otherwise
        ///
        public bool SetFixedContractAmount(string fixedContractAmount)
        {
            double result;
            bool valid = false;
            //try to convert string to double
            if (double.TryParse(fixedContractAmount, out result))
            {
                if (result >= 0.00)
                {
                    this.fixedContractAmount = result;
                    valid = true;
                }
                else
                {
                    valid = false;
                    Logging.Log(this.ToString(), "SetFixedContractAmount", "Invalid Fixed Contract Amount Input");
                }
            }
            else
            {
                valid = false;
                Logging.Log(this.ToString(), "SetFixedContractAmount", "Invalid Fixed Contract Amount Input");
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
            if (lastName != "" && dateOfBirth != new DateTime() && sin != null && sin != "" && sin != "0"
                && contractStartDate != new DateTime() && contractEndDate != new DateTime() && fixedContractAmount != 0
                && StartAndEndDateCompare(contractEndDate))
            {
                string test1 = dateOfBirth.ToString("yy");
                string test2 = dateOfBirth.ToString("yy");

                //check whether the first two number of sin is the date of birth
                if (sin[0] == dateOfBirth.ToString("yy")[0] && sin[1] == dateOfBirth.ToString("yy")[1])
                {
                    DateTime maxDate = DateTime.Now;
                    DateTime minDate = new DateTime(1900, 1, 1);
                    if (CrossFieldCheckDate(contractStartDate) && CrossFieldCheckDate(contractEndDate))
                    {
                        if ((contractStartDate < maxDate) && (contractStartDate > minDate) && (contractEndDate < maxDate)
                            && (contractEndDate > minDate) && (dateOfBirth < maxDate) && (dateOfBirth > minDate))
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
            result = "Contract Employee\n" +
                     "Company Name: " + lastName + "\n" +
                     "Born: " + dateOfBirth.ToString("yyyy-MM-dd") + " SIN: " + sin + "\n" +
                     "Start: " + contractStartDate.ToString("yyyy-MM-dd") + " End: " + contractEndDate.ToString("yyyy-MM-dd") + "\n" +
                     "Fixed Contract Amount: " + fixedContractAmount.ToString();

            Logging.Log(this.ToString(), "Details", result);
            
            return result;
        }

        /// \brief Override sin validation
        /// \details <b>Details</b> 
        ///  Check for valid attribute when user input <br>
        ///	<b>Input</b> 
        ///			~ new sin (string)
        /// \return
        ///			~ Boolean valid indicates whether the input is valid or not<br>
        ///
        public override bool sinValidation(ref string sin)
        {
            bool valid = false;
            //trim spaces
            sin = sin.Trim();
            sin = sin.Replace(" ", "").Replace("-", "").Replace(".", "");
            //check for empty
            if (sin == "" || sin == "0")
            {
                valid = true;
                sinTest = valid;
            }
            else
            {
                if (Regex.Match(sin, @"^[0-9]{9}$").Success)
                {
                    valid = true;
                    sinTest = valid;
                    //format xxxxx xxxx
                    sin = sin.Insert(5, " ");
                }
            }
            return valid;
        }

        public override List<string> Data()
        {
            List<string> data = new List<string>(base.Data());

            data.Insert(0, "CT");
            data.Add(contractStartDate.Date.ToString("yyyy-MM-dd"));
            data.Add(contractEndDate.Date.ToString("yyyy-MM-dd"));
            data.Add(fixedContractAmount.ToString());

            return data;
        }
        /// \brief Compare contract start and end date
        /// \details <b>Details</b> 
        ///  Compare if contract start date is before end date. <br>
        ///	<b>Input</b> 
        ///			~ new endDate (DateTime)
        /// \return
        ///			~ Boolean valid indicates whether the input is valid or not<br>
        ///
        private bool StartAndEndDateCompare(DateTime contractEndDate)
        {
            bool result = false;

            if (contractEndDate > contractStartDate)
            {
                result = true;
            }

            return result;
        }

        public override Employee Copy()
        {
            return (ContractEmployee)this.MemberwiseClone();
        }
    }
}
