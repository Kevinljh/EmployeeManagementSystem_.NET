/* 
 *   FILE           : SeasonalEmployee.cs
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
    *  NAME    : SeasonalEmployee
    *  PURPOSE : The SeasonalEmployee class has been created to keep track of information 
     *  that applies to Seasonal Employees specifically.
     *  The SeasonalEmployee has members to track the Season and Wages.
     *  The SeasonalEmployee also has the ability to check if the attributes are valid. 
     *  */

    /// 
    /// \class SeasonalEmployee
    ///
    /// \brief The SeasonalEmployee class has been created to keep track of information  <br>
    ///  that applies to Seasonal Employees specifically.<br>
    /// The SeasonalEmployee has members to track the Season and Wages. <br>
    /// The SeasonalEmployee also has the ability to check if the attributes are valid. <br>
    ///
    /// \author <i>Dev Till Death</i>
    ///
    public class SeasonalEmployee : Employee
    {
        /* -------------- ATTRIBUTES ------------ */
        private string season;          ///<    Seasonal Employee season
        private double piecePay = -1;        ///<    Payment given to an employee on a per unit of work basis

        static bool seasonTest = false;                 ///< used for unit testing
        static bool piecePayTest = false;               ///< used for unit testing
        
        /* -------------- CONSTRUCTORS ------------ */
        /// \brief SeasonalEmployee(Child) Regular constructor
        /// \details <b>Details</b> 
        /// Initialize attributes in the class<br>
        ///
        public SeasonalEmployee()
        {

        }

        /// \brief SeasonalEmployee(Child) Override Constructor
        /// \details <b>Details</b> 
        /// Assign firstName and last name
        /// <b>Input</b>
        ///			~ firstName (string)
        ///			~ lastName (string)
        public SeasonalEmployee(string firstName, string lastName)
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
        ///			~ season (string)
        ///			~ piecePay (double)
        public SeasonalEmployee(string firstName
                                , string lastName
                                , string sin
                                , DateTime dateOfBirth
                                , string season
                                , double piecePay
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
            if (!SetSeason(season))
            {
                throw new ArgumentException("Invalid seaon");
            }
            if (!SetPiecePay(piecePay.ToString()))
            {
                throw new ArgumentException("Invalid piece pay");
            }
        }
        /* -------------- GETTERS ------------ */
        public static bool GetSeasonTest
        {
            get { return seasonTest; }
        }
        public static bool GetPiecePayTest
        {
            get { return piecePayTest; }
        }
        
        /* -------------- MUTATORS ------------ */

        /// \brief Set Season
        /// \details <b>Details</b> 
        /// Sets Season for Seasonal time employees if Valid date is entered <br>
        ///	<b>Input</b> 
        ///			~ new Season (string)
        /// \return
        ///			~ Season if success<br>
        ///			~ throw exception otherwise
        ///	
        public bool SetSeason(string season)
        {
            bool result = false;
            string newSeason = season.ToUpper();

            if (newSeason.Equals(""))
            {
                this.season = newSeason;
                result = true;
            }
            else
            {
                if (newSeason.Equals("WINTER") || newSeason.Equals("SPRING") || newSeason.Equals("SUMMER") || newSeason.Equals("FALL"))
                {
                    this.season = newSeason;
                    result = true;
                }
                else
                {
                    Logging.Log(this.ToString(), "SetSeason", "Invalid Season Input");
                }
            }
            
            return result;
        }

        /// \brief Set PiecePay
        /// \details <b>Details</b> 
        /// Sets PiecePay for Seasonal time employees if Valid date is entered <br>
        ///	<b>Input</b> 
        ///			~ new piecePay (double)
        /// \return
        ///			~ Season if success<br>
        ///			~ throw exception otherwise
        ///	
        public bool SetPiecePay(string piecePay)
        {
            double result;
            bool valid = false;
            piecePayTest = valid;
            //try to convert string to double
            if (double.TryParse(piecePay, out result))
            {
                if (result >= 0.00)
                {
                    this.piecePay = result;
                    
                    valid = true;
                    piecePayTest = valid;
                }
                else
                {

                    valid = false;
                    piecePayTest = valid;
                    Logging.Log(this.ToString(), "SetPiecePay", "Invalid Piece Pay Input");
                }
            }
            else
            {
                valid = false;
                piecePayTest = valid;
                Logging.Log(this.ToString(), "SetPiecePay", "Invalid Piece Pay Input");
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
            if (lastName != "" && dateOfBirth != new DateTime() && sin != "" && sin != "0"
                && season != "" && piecePay != 0)
            {
                result = true;
                Logging.Log(this.ToString(), "Validate", "Last Name: " + lastName + "First Name: " + firstName + "SIN: " + sin + "Valid");
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
            result = "Seasonal Employee\n" +
                     "Last: " + lastName + " Fist: " + firstName + "\n" +
                     "Born: " + dateOfBirth.ToString("yyyy-MM-dd") + " SIN: " + sin + "\n" +
                     "Season: " + season + "\n" +
                     "Piece Pay: " + piecePay.ToString();

            Logging.Log(this.ToString(), "Details", result);

            return result;
        }

        public override List<string> Data()
        {
            List<string> data = new List<string>(base.Data());

            data.Insert(0, "SN");
            data.Add(season);
            data.Add(piecePay.ToString());

            return data;
        }

        public override Employee Copy()
        {
            return (SeasonalEmployee)this.MemberwiseClone();
        }
    }
}
