/* 
 *   FILE           : UI.cs
 *   PROJECT        : INFO2180-14F - Employee Management System
 *   PROGRAMMER     : Grigory Kozyrev, Ben Lorantfy, Kevin Li, Michael Da Silva
 *   FIRST VERSION  : 2014-11-14
 *   DESCRIPTION    : The functions of this file is to provide user interface
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AllEmployees;
using TheCompany;
using Supporting;

namespace Presentation
{
    //
    // todo: Bugs to fix or close:
    // todo: Filter by type
    // Search parameters going off edge
    // Company name doesn't allow spaces (should it?)
    // Database lower case search
    //


    /// 
    /// \class UI
    ///
    /// \brief The purpose of this class is to prerform interaction between a user and the system
    ///
    /// Class has no constants<br>
    /// It has 2 private data member:<br>
    /// -tempEmployee (Employee)<br>
    /// -database (Database)
    ///
    /// \author <i>Greg Kozyrev and Ben Lorantfy</i>
    ///
    public class UI
    {
        #region PropertiesAndConstructor
        private Database database = new Database();     ///Database interface
        private int width = Console.WindowWidth;
        private int height = Console.WindowHeight;
        private int leftNavWidth = (int)(Console.WindowWidth * 0.4);

        string typeParam = Database.kAnyTag;
        string firstNameParam = Database.kAnyTag;
        string lastNameParam = Database.kAnyTag;
        string sinParam = Database.kAnyTag;
        string dateOfBirthParam = Database.kAnyTag;
        string companyParam = Database.kAnyTag;
        string businessNumParam = Database.kAnyTag;
        string dateOfIncParam = Database.kAnyTag;

        enum MainMenu
        {
            Search = 1
           ,
            Add = 2
                ,
            Quit = 3
                , Invalid
        };
        enum SearchMenu
        {
            AddParameter = 1
           ,
            Edit = 2
                ,
            Remove = 3
                ,
            Back = 4
                ,
            Up
                ,
            Down
                , Invalid
        };
        enum SearchParameterMenu
        {
            Type = 1
           ,
            First = 2
                ,
            Last = 3
                ,
            SIN = 4
                ,
            DateOfBirth = 5
                ,
            Back = 6
                , Invalid
        };
        enum AddMenu
        {
            FullTime = 1
           ,
            PartTime = 2
                ,
            Seasonal = 3
                ,
            Contract = 4
                ,
            Back = 5
                , Invalid
        };
        enum AddInfoMenu
        {
            Done = 1
           ,
            ClearAll = 2
                ,
            Cancel = 3
                ,
            EnterFields
                , Invalid
        };
        enum EmployeeFields
        {
            /* common to all employees except contract */
            FirstName = 0
           ,
            LastName = 1
                ,
            SIN = 2
                ,
            DateOfBirth = 3

                /* common to full and part employees */
                ,
            DateOfHire = 4
                ,
            DateOfTermination = 5

                /* full time employees */
                ,
            Salary = 6

                /* part time employees */
                ,
            HourlyRate = 6

                /* seasonal employees */
                ,
            Season = 4
                ,
            PiecePay = 5

                /* contract employees */
                ,
            CompanyName = 0
                ,
            BusinessNumber = 1
                ,
            DateOfIncorporation = 2
                ,
            StartDate = 3
                ,
            StopDate = 4
                , FixedAmount = 5
        };

        enum EmployeeType
        {
            ANY = 0,
            FullTime = 1,
            PartTime = 2,
            Seasonal = 3,
            Contract = 4
        }

        enum RemovePrompt
        {
             Yes=1
            ,No=2
            ,Invalid
        }

        public UI() { }
        public void Run()
        {
            try
            {
                database.ReadDatabase();
                Console.CursorVisible = false;
                HomePage();
                database.SaveDatabase();
            }
            catch(Exception e)
            {
                Logging.Log("UI", "Run", "Very unexpected error catched in Run function. Details: " + e.ToString());
            }
        }
        #endregion

        #region Pages
        private void HomePage()
        {
            //
            // Display the main menu and title
            //
            Clear();
            DisplayTitle();
            DisplayMainMenu();

            //
            // Get users' desired option, possibly navigating to another page
            //
            MainMenu option = MainMenu.Invalid;
            bool done = false;
            while (!done)
            {
                option = GetMainMenuOption();
                switch (option)
                {
                    case MainMenu.Search:
                        done = true;
                        SearchPage();
                        break;

                    case MainMenu.Add:
                        done = true;
                        AddPage();
                        break;

                    case MainMenu.Quit:
                        done = true;
                        break;
                }
            }
        }
        private void SearchPage()
        {
            List<Employee> results = GetSearchResults();
            int numResults = results.Count;
            int page = 1;
            int boxIndex = 1;

            //
            // Display search page menu
            //
            Clear();
            DisplaySearchMenu();

            //
            // Display search results
            //
            DisplayResults(results, page);

            WriteSeperator();

            if (results.Count > 0)
            {
                DrawBox(boxIndex);
            }


            bool done = false;
            SearchMenu option = SearchMenu.Invalid;
            while (!done)
            {
                option = GetSearchMenuOption();
                switch (option)
                {
                    case SearchMenu.AddParameter:
                        done = true;
                        SearchParameterPage();
                        break;
                    case SearchMenu.Edit:
                        if (results.Count > 0)
                        {
                            done = true;
                            InfoPage(results[boxIndex * page - 1], false);
                        }
                        break;
                    case SearchMenu.Remove:
                        if (results.Count > 0)
                        {
                            done = true;
                            Clear();
                            DisplayConfirmPrompt();
                            RemovePrompt removeOption = RemovePrompt.Invalid;
                            while (removeOption == RemovePrompt.Invalid && removeOption != RemovePrompt.No)
                            {
                                removeOption = GetRemovePromptOption();
                                switch (removeOption)
                                {
                                    case RemovePrompt.Yes:
                                        database.Remove(results[boxIndex * page - 1]);
                                        break;
                                }
                            }

                            SearchPage();
                        }
                        break;
                    case SearchMenu.Down:
                        if (results.Count > 0)
                        {
                            int potentialBoxIndex = (boxIndex < 4) ? boxIndex + 1 : 1;
                            if (potentialBoxIndex + (4*(page - 1)) <= numResults)
                            {
                                EraseBox(boxIndex);
                                boxIndex = potentialBoxIndex;
                                if (boxIndex == 1)
                                {
                                    page++;
                                    ClearResults();
                                    DisplayResults(results, page);
                                }
                                DrawBox(boxIndex);
                            }
                        }

                        break;
                    case SearchMenu.Up:
                        if (results.Count > 0)
                        {
                            if (boxIndex > 1)
                            {
                                EraseBox(boxIndex);
                                boxIndex--;
                                DrawBox(boxIndex);
                            }
                            else if (page > 1)
                            {
                                EraseBox(boxIndex);
                                boxIndex = 4;
                                page--;
                                ClearResults();
                                DisplayResults(results, page);
                                DrawBox(boxIndex);
                            }
                        }
                        break;
                    case SearchMenu.Back:
                        done = true;
                        HomePage();
                        break;
                }
            }
        }
        private List<Employee> GetSearchResults()
        {
            List<Employee> fullPartAndSeasonalEmployeesMatch = database.Search(typeParam, firstNameParam, lastNameParam, sinParam, dateOfBirthParam);
            List<Employee> contractEmployeesMatch = database.Search(typeParam, Database.kAnyTag, companyParam, businessNumParam, dateOfIncParam);
            List<Employee> searchResults = fullPartAndSeasonalEmployeesMatch.Union(contractEmployeesMatch).ToList();
            return searchResults;
        }
        private void SearchParameterPage()
        {
            Clear();
            DisplaySearchParameterMenu();

            bool done = false;
            while (!done)
            {
                TextBox txtParameterValue = null;
                ConsoleKey exitKey = ConsoleKey.A;
                SearchParameterMenu option = GetSearchParameterOption();
                if (option != SearchParameterMenu.Back && option != SearchParameterMenu.Invalid)
                {
                    Clear();

                    if (option == SearchParameterMenu.Type)
                    {
                        DisplayTypeParameterPrompt();

                        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                        ConsoleKey key = keyInfo.Key;
                        int numericOption = (int)Char.GetNumericValue(keyInfo.KeyChar);

                        switch((EmployeeType)numericOption)
                        {
                            case EmployeeType.ANY:
                                typeParam = Database.kAnyTag;
                                break;
                            case EmployeeType.FullTime:
                                typeParam = Database.kFullTimeEmp;
                                break;
                            case EmployeeType.PartTime:
                                typeParam = Database.kPartTimeEmp;
                                break;
                            case EmployeeType.Contract:
                                typeParam = Database.kContractEmp;
                                break;
                            case EmployeeType.Seasonal:
                                typeParam = Database.kSeasonalEmp;
                                break;
                        }
                    }
                    else
                    {
                        DisplayParameterPrompt();
                        txtParameterValue = new TextBox(width / 2 - 15, 6, 30);
                        exitKey = ConsoleKey.A;
                        switch (option)
                        {
                            case SearchParameterMenu.Type:
                                txtParameterValue.Text = (typeParam == "<ANY>") ? "" : typeParam;
                                break;
                            case SearchParameterMenu.First:
                                txtParameterValue.Text = (firstNameParam == "<ANY>") ? "" : firstNameParam;
                                break;
                            case SearchParameterMenu.Last:
                                txtParameterValue.Text = (lastNameParam == "<ANY>") ? "" : lastNameParam;
                                break;
                            case SearchParameterMenu.SIN:
                                txtParameterValue.Text = (sinParam == "<ANY>") ? "" : sinParam;
                                break;
                            case SearchParameterMenu.DateOfBirth:
                                txtParameterValue.Text = (dateOfBirthParam == "<ANY>") ? "" : dateOfBirthParam;
                                break;
                        }

                        while (exitKey != ConsoleKey.Enter && exitKey != ConsoleKey.Escape)
                        {
                            exitKey = txtParameterValue.Focus();
                        }
                    }

                }
                switch (option)
                {
                    case SearchParameterMenu.Type:
                        if (exitKey == ConsoleKey.Enter)
                        {
                            typeParam = (txtParameterValue.Text == "") ? Database.kAnyTag : txtParameterValue.Text;
                        }
                        break;
                    case SearchParameterMenu.First:
                        if (exitKey == ConsoleKey.Enter)
                        {
                            firstNameParam = (txtParameterValue.Text == "") ? Database.kAnyTag : txtParameterValue.Text;
                        }
                        break;
                    case SearchParameterMenu.Last:
                        if (exitKey == ConsoleKey.Enter)
                        {
                            lastNameParam = (txtParameterValue.Text == "") ? Database.kAnyTag : txtParameterValue.Text;
                        }
                        break;
                    case SearchParameterMenu.SIN:
                        if (exitKey == ConsoleKey.Enter)
                        {
                            sinParam = (txtParameterValue.Text == "") ? Database.kAnyTag : txtParameterValue.Text;
                        }
                        break;
                    case SearchParameterMenu.DateOfBirth:
                        if (exitKey == ConsoleKey.Enter)
                        {
                            dateOfBirthParam = (txtParameterValue.Text == "") ? Database.kAnyTag : txtParameterValue.Text;
                        }
                        break;
                    case SearchParameterMenu.Back:
                        done = true;
                        SearchPage();
                        break;
                }
                if (option != SearchParameterMenu.Back && option != SearchParameterMenu.Invalid)
                {
                    Clear();
                    DisplaySearchParameterMenu();
                }
            }
        }
        private void AddPage()
        {
            Clear();
            DisplayAddMenu();

            bool done = false;
            while (!done)
            {
                AddMenu option = GetAddMenuOption();
                switch (option)
                {
                    case AddMenu.FullTime:
                        done = true;
                        InfoPage(new FullTimeEmployee(), true);
                        break;

                    case AddMenu.PartTime:
                        done = true;
                        InfoPage(new PartTimeEmployee(), true);
                        break;

                    case AddMenu.Seasonal:
                        done = true;
                        InfoPage(new SeasonalEmployee(), true);
                        break;

                    case AddMenu.Contract:
                        done = true;
                        InfoPage(new ContractEmployee(), true);
                        break;

                    case AddMenu.Back:
                        done = true;
                        HomePage();
                        break;
                }
            }
        }
        private void InfoPage(Employee employeeBeforeChanges, bool add)
        {

            //
            // This employee is used to store a copy of employeeBeforeChanges
            // This is so that employee can be altered without changing employeeBeforeChanges
            // and user can cancel changes if they want
            //
            //todo, implement copy in employee classes (copy all fields not just create new employee)
            Employee employee = employeeBeforeChanges.Copy();
            List<string> employeeData = employee.Data();

            Clear();
            DisplayInfoMenu(add ? "Add" : "Edit", employee);
            WriteSeperator();
            bool done = false;
            int numTextBoxes = 0;
            List<TextBox> textboxes = new List<TextBox>();
            if (employee is FullTimeEmployee || employee is PartTimeEmployee) numTextBoxes = 7;
            if (employee is ContractEmployee || employee is SeasonalEmployee) numTextBoxes = 6;

            for (int row = 0; row < numTextBoxes; row++)
            {
                textboxes.Add(new TextBox(17 + leftNavWidth, 1 + 3 * row, 25));
            }

            //todo: set textbox text value to tempEmployee values( 0001-01-01 becomes "")
            //todo: create placeholder attribute for textbox and use it for input formats
            if (!(employee is ContractEmployee))
            {
                textboxes[(int)EmployeeFields.FirstName].Text = String.IsNullOrWhiteSpace(employeeData[(int)EmployeeFields.FirstName + 1]) ? "" : employeeData[(int)EmployeeFields.FirstName + 1];
                textboxes[(int)EmployeeFields.LastName].Text = String.IsNullOrWhiteSpace(employeeData[(int)EmployeeFields.LastName + 1]) ? "" : employeeData[(int)EmployeeFields.LastName + 1];
                textboxes[(int)EmployeeFields.DateOfBirth].Text = employeeData[(int)EmployeeFields.DateOfBirth + 1] == (new DateTime()).ToString("yyyy-MM-dd") ? "" : employeeData[(int)EmployeeFields.DateOfBirth + 1];
                textboxes[(int)EmployeeFields.SIN].Text = String.IsNullOrWhiteSpace(employeeData[(int)EmployeeFields.SIN + 1]) ? "" : employeeData[(int)EmployeeFields.SIN + 1];
            }
            else
            {
                textboxes[(int)EmployeeFields.CompanyName].Text = String.IsNullOrWhiteSpace(employeeData[(int)EmployeeFields.CompanyName + 2]) ? "" : employeeData[(int)EmployeeFields.CompanyName + 2];
                textboxes[(int)EmployeeFields.BusinessNumber].Text = String.IsNullOrWhiteSpace(employeeData[(int)EmployeeFields.BusinessNumber + 2]) ? "" : employeeData[(int)EmployeeFields.BusinessNumber + 2];
                textboxes[(int)EmployeeFields.DateOfIncorporation].Text = employeeData[(int)EmployeeFields.DateOfIncorporation + 2] == (new DateTime()).ToString("yyyy-MM-dd") ? "" : employeeData[(int)EmployeeFields.DateOfIncorporation + 2];
                textboxes[(int)EmployeeFields.StartDate].Text = employeeData[(int)EmployeeFields.StartDate + 2] == (new DateTime()).ToString("yyyy-MM-dd") ? "" : employeeData[(int)EmployeeFields.StartDate + 2];
                textboxes[(int)EmployeeFields.StopDate].Text = employeeData[(int)EmployeeFields.StopDate + 2] == (new DateTime()).ToString("yyyy-MM-dd") ? "" : employeeData[(int)EmployeeFields.StopDate + 2];
                textboxes[(int)EmployeeFields.FixedAmount].Text = String.IsNullOrWhiteSpace(employeeData[(int)EmployeeFields.FixedAmount + 2]) || employeeData[(int)EmployeeFields.FixedAmount + 2] == "-1" ? "" : employeeData[(int)EmployeeFields.FixedAmount + 2];
            }

            if (employee is FullTimeEmployee)
            {
                textboxes[(int)EmployeeFields.DateOfHire].Text = employeeData[(int)EmployeeFields.DateOfHire + 1] == (new DateTime()).ToString("yyyy-MM-dd") ? "" : employeeData[(int)EmployeeFields.DateOfHire + 1];
                textboxes[(int)EmployeeFields.DateOfTermination].Text = employeeData[(int)EmployeeFields.DateOfTermination + 1] == (new DateTime()).ToString("yyyy-MM-dd") ? "" : employeeData[(int)EmployeeFields.DateOfTermination + 1];
                textboxes[(int)EmployeeFields.Salary].Text = String.IsNullOrWhiteSpace(employeeData[(int)EmployeeFields.Salary + 1]) || employeeData[(int)EmployeeFields.Salary + 1] == "-1" ? "" : employeeData[(int)EmployeeFields.Salary + 1];
            }
            else if (employee is PartTimeEmployee)
            {
                textboxes[(int)EmployeeFields.DateOfHire].Text = employeeData[(int)EmployeeFields.DateOfHire + 1] == (new DateTime()).ToString("yyyy-MM-dd") ? "" : employeeData[(int)EmployeeFields.DateOfHire + 1];
                textboxes[(int)EmployeeFields.DateOfTermination].Text = employeeData[(int)EmployeeFields.DateOfTermination + 1] == (new DateTime()).ToString("yyyy-MM-dd") ? "" : employeeData[(int)EmployeeFields.DateOfTermination + 1];
                textboxes[(int)EmployeeFields.HourlyRate].Text = String.IsNullOrWhiteSpace(employeeData[(int)EmployeeFields.HourlyRate + 1]) || employeeData[(int)EmployeeFields.HourlyRate + 1] == "-1" ? "" : employeeData[(int)EmployeeFields.HourlyRate + 1];
            }
            else if (employee is SeasonalEmployee)
            {
                textboxes[(int)EmployeeFields.Season].Text = String.IsNullOrWhiteSpace(employeeData[(int)EmployeeFields.Season + 1]) || employeeData[(int)EmployeeFields.Season + 1] == "-1" ? "" : employeeData[(int)EmployeeFields.Season + 1];
                textboxes[(int)EmployeeFields.PiecePay].Text = String.IsNullOrWhiteSpace(employeeData[(int)EmployeeFields.PiecePay + 1]) || employeeData[(int)EmployeeFields.PiecePay + 1] == "-1" ? "" : employeeData[(int)EmployeeFields.PiecePay + 1];
            }

            int focusedField = 0;
            AddInfoMenu option = AddInfoMenu.EnterFields;
            while (!done)
            {
                if (option == AddInfoMenu.EnterFields)
                {
                    FormFocus(textboxes, ref focusedField, employee, DetectAddInfoErrors);
                }
                else if (option == AddInfoMenu.Done)
                {                   
                    bool someAreEmpty = false;
                    foreach(TextBox textbox in textboxes)
                    {
                        if(textbox.Empty()){
                            someAreEmpty = true;
                            break;
                        }
                    }
                    
                    if(!someAreEmpty){
                        // todo: call employee.validate() and only do this block if it returns true, otherwise print an error (use DisplayInfoError)
                        if (employee.Validate())
                        {
                            done = true;

                            //
                            // If making new employee, add it to the database class
                            //
                            if (add)
                            {
                                database.Add(employee);
                                AddPage();
                            }
                            else
                            {
                                database.Update(employeeBeforeChanges, employee);
                                SearchPage();
                            }
                        }
                        else
                        {
                            DisplayInfoError("Fields combined are invalid.");
                        }                 
                    }
                    else
                    {
                        DisplayInfoError("All fields required.");
                    }

                }
                else if (option == AddInfoMenu.ClearAll)
                {
                    foreach (TextBox textbox in textboxes)
                    {
                        textbox.Clear();
                    }
                }
                else if (option == AddInfoMenu.Cancel)
                {
                    done = true;
                    if (add)
                    {
                        AddPage();
                    }
                    else
                    {
                        SearchPage();
                    }
                }

                if (!done)
                {
                    option = GetAddInfoMenuOption();
                }
            }

        }
        #endregion

        #region GetMenuOptions
        private RemovePrompt GetRemovePromptOption()
        {
            RemovePrompt menuOption = RemovePrompt.Invalid;
            int option = (int)Char.GetNumericValue(Console.ReadKey(true).KeyChar);
            if (option != -1)
            {
                if (Enum.IsDefined(typeof(RemovePrompt), option))
                {
                    menuOption = (RemovePrompt)option;
                }
            }
            return menuOption;
        }

        private MainMenu GetMainMenuOption()
        {
            MainMenu menuOption = MainMenu.Invalid;
            int option = (int)Char.GetNumericValue(Console.ReadKey(true).KeyChar);
            if (option != -1)
            {
                if (Enum.IsDefined(typeof(MainMenu), option))
                {
                    menuOption = (MainMenu)option;
                }
            }
            return menuOption;
        }
        private SearchMenu GetSearchMenuOption()
        {
            //
            // Gets key and key info
            //
            SearchMenu menuOption = SearchMenu.Invalid;
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            ConsoleKey key = keyInfo.Key;
            int numericOption = (int)Char.GetNumericValue(keyInfo.KeyChar);

            // If key was not a digit
            if (numericOption != -1)
            {
                if (Enum.IsDefined(typeof(SearchMenu), numericOption))
                {
                    menuOption = (SearchMenu)numericOption;
                }
            }
            // Else check if key was up or down
            else
            {
                if (key == ConsoleKey.UpArrow)
                {
                    menuOption = SearchMenu.Up;
                }
                else if (key == ConsoleKey.DownArrow)
                {
                    menuOption = SearchMenu.Down;
                }
            }
            return menuOption;
        }
        private SearchParameterMenu GetSearchParameterOption()
        {
            SearchParameterMenu menuOption = SearchParameterMenu.Invalid;
            int option = (int)Char.GetNumericValue(Console.ReadKey(true).KeyChar);
            if (option != -1)
            {
                if (Enum.IsDefined(typeof(SearchParameterMenu), option))
                {
                    menuOption = (SearchParameterMenu)option;
                }
            }
            return menuOption;
        }
        private AddMenu GetAddMenuOption()
        {
            //
            // Gets key and key info
            //
            AddMenu menuOption = AddMenu.Invalid;
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            ConsoleKey key = keyInfo.Key;
            int numericOption = (int)Char.GetNumericValue(keyInfo.KeyChar);

            // If key was not a digit
            if (numericOption != -1)
            {
                if (Enum.IsDefined(typeof(SearchMenu), numericOption))
                {
                    menuOption = (AddMenu)numericOption;
                }
            }
            return menuOption;
        }
        private AddInfoMenu GetAddInfoMenuOption()
        {
            //
            // Gets key and key info
            //
            AddInfoMenu menuOption = AddInfoMenu.Invalid;
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            ConsoleKey key = keyInfo.Key;
            int numericOption = (int)Char.GetNumericValue(keyInfo.KeyChar);

            // If key was not a digit
            if (numericOption != -1)
            {
                if (Enum.IsDefined(typeof(AddInfoMenu), numericOption))
                {
                    menuOption = (AddInfoMenu)numericOption;
                }
            }
            // Else check if key was up or down
            else if (key == ConsoleKey.UpArrow || key == ConsoleKey.Tab || key == ConsoleKey.DownArrow)
            {
                menuOption = AddInfoMenu.EnterFields;
            }
            return menuOption;
        }
        #endregion

        #region DisplayMenus
        private void ClearResults()
        {
            //
            // Clears old employee data
            //
            Console.CursorVisible = true;
            for (int i = 0; i < height; i++)
            {
                Console.CursorTop = i;
                Console.CursorLeft = leftNavWidth;
                Console.Write(new string(' ', width - leftNavWidth));
            }
        }

        private void DisplayResults(List<Employee> results, int page)
        {
            int employeeCounter = page * 4 - 3;

            if (results.Count == 0)
            {
                string noResults = "No Results";
                Console.CursorLeft = ((width - leftNavWidth) / 2 + leftNavWidth) - noResults.Length / 2;
                Console.CursorTop = height / 2;
                Console.Write(noResults);
            }
            else
            {

                //
                // Writes employee data
                //
                Console.CursorTop = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (employeeCounter <= results.Count)
                    {
                        string[] resultLines = results[employeeCounter - 1].Details().Split('\n');
                        WriteRightBlank(1);
                        for (int j = 0; j < resultLines.Length; j++)
                        {
                            WriteRight(" " + resultLines[j]);
                        }
                        employeeCounter++;
                    }
                }
            }

        }
        private void DisplayInfoError(string message)
        {
            Console.CursorLeft = 0;
            Console.CursorTop = height - 3;
            Console.Write(" Error:");
            Console.CursorTop++;
            Console.CursorLeft = 0;
            Console.Write(" " + message + new string(' ', leftNavWidth - message.Length - 3));
        }

        private void ClearInfoError()
        {
            Console.CursorLeft = 0;
            Console.CursorTop = height - 3;
            Console.Write(new string(' ', leftNavWidth - 3));
            Console.CursorTop++;
            Console.CursorLeft = 0;
            Console.Write(new string(' ', leftNavWidth - 3));
        }
        private void DisplayConfirmPrompt()
        {
            WriteBlankLine(3);
            WriteCenter("==== Are you sure? ===============");
            WriteCenter("|                                |");
            WriteCenter("|      1.       Yes              |");
            WriteCenter("|      2.       No               |");
            WriteCenter("|                                |");
            WriteCenter("==================================");
        }

        private void DisplayMainMenu()
        {
            WriteBlankLine(2);
            WriteCenter("==== Main Menu ===================");
            WriteCenter("|                                |");
            WriteCenter("|                                |");
            WriteCenter("|      1.      Search            |");
            WriteCenter("|      2.       Add              |");
            WriteCenter("|      3.       Quit             |");
            WriteCenter("|                                |");
            WriteCenter("|                                |");
            WriteCenter("==================================");
        }
        private void DisplayTitle()
        {
            WriteBlankLine(2);
            WriteCenter(@" _____                    _   _       _ _____ __  __ ____    ");
            WriteCenter(@"| ____|___ ___  ___ _ __ | |_(_) __ _| | ____|  \/  / ___|   ");
            WriteCenter(@"|  _| / __/ __|/ _ \ '_ \| __| |/ _` | |  _| | |\/| \___ \   ");
            WriteCenter(@"| |___\__ \__ \  __/ | | | |_| | (_| | | |___| |  | |___) |  ");
            WriteCenter(@"|_____|___/___/\___|_| |_|\__|_|\__,_|_|_____|_|  |_|____/ TM");
        }
        private void DisplaySearchMenu()
        {
            WriteLeftBlank();
            WriteLeft("Search Employees");
            WriteLeft("================");
            WriteLeft();
            WriteLeft(" 1. Add Parameter");
            WriteLeft(" 2. Edit");
            WriteLeft(" 3. Remove");
            WriteLeft(" 4. Back");
            WriteLeftBlank();
            WriteLeftBlank(height - 24);
            WriteLeft("Current Parameters");
            WriteLeft("------------------");
            WriteLeft("Type          : " + typeParam);
            WriteLeft("First Name    : " + firstNameParam);
            WriteLeft("Last Name     : " + lastNameParam);
            WriteLeft("SIN           : " + sinParam);
            WriteLeft("Date of Birth : " + dateOfBirthParam);
            WriteLeftBlank();
            WriteLeft("Contractor Params");
            WriteLeft("Company Name  : " + companyParam);
            WriteLeft("Business Num  : " + businessNumParam);
            WriteLeft("Date Of Inc.  : " + dateOfIncParam);
        }
        private void DrawBox(int offset)
        {
            string horizontalEdge = new string('─', width - leftNavWidth - 3);

            Console.CursorTop = (offset - 1) * 6;
            Console.CursorLeft = leftNavWidth;
            Console.Write("┌" + horizontalEdge + "┐");

            for (int i = 0; i < 5; i++)
            {
                Console.CursorTop++;
                Console.CursorLeft = leftNavWidth;
                Console.Write("│");
                Console.CursorLeft = width - 2;
                Console.Write("│");
            }

            Console.CursorLeft = leftNavWidth;
            Console.CursorTop++;
            Console.Write("└" + horizontalEdge + "┘");
        }
        private void EraseBox(int offset)
        {
            string horizontalEdge = new string(' ', width - leftNavWidth - 1);

            Console.CursorTop = (offset - 1) * 6;
            Console.CursorLeft = leftNavWidth;
            Console.Write(horizontalEdge);

            for (int i = 0; i < 5; i++)
            {
                Console.CursorTop++;
                Console.CursorLeft = leftNavWidth;
                Console.Write(" ");
                Console.CursorLeft = width - 2;
                Console.Write(" ");
            }

            Console.CursorLeft = leftNavWidth;
            Console.CursorTop++;
            Console.Write(horizontalEdge);
        }
        private void DisplayAddMenu()
        {
            WriteBlankLine(2);
            WriteCenter("==== Add Employee ================");
            WriteCenter("|                                |");
            WriteCenter("|  Select what type of employee  |");
            WriteCenter("|  to add by choosing the        |");
            WriteCenter("|  corresponding number          |");
            WriteCenter("|                                |");
            WriteCenter("|      1.    Full Time           |");
            WriteCenter("|      2.    Part Time           |");
            WriteCenter("|      3.    Seasonal            |");
            WriteCenter("|      4.    Contract            |");
            WriteCenter("|      5.      Back              |");
            WriteCenter("|                                |");
            WriteCenter("==================================");
        }
        private void DisplayInfoMenu(string operation, Employee employee)
        {
            WriteBlankLine();
            string subtitle = operation + " Employee";
            WriteLeft(subtitle);
            WriteLeft(new string('=', subtitle.Length));
            WriteBlankLine();
            WriteLeft("Press tab, down arrow, and");
            WriteLeft("up arrow to switch between");
            WriteLeft("fields and enter to finish");
            WriteLeft("entering a field. Press");
            WriteLeft("Shift+Del to clear field");
            WriteLeftBlank();
            WriteLeft("1. Done");
            WriteLeft("2. Clear All");
            WriteLeft("3. Cancel");
            Console.CursorTop = 0;

            if (employee is ContractEmployee)
            {
                WriteRightBlank(2);
                WriteRight("Company Name  :");
                WriteRightBlank(2);
                WriteRight("Buisness Num. :");
                WriteRightBlank(2);
                WriteRight("Date of Inc.  :");
                WriteRightBlank(2);
                WriteRight("Start Date    :");
                WriteRightBlank(2);
                WriteRight("End Date      :");
                WriteRightBlank(2);
                WriteRight("Fixed Amount  :");
            }
            else
            {
                WriteRightBlank(2);
                WriteRight("First Name    :");
                WriteRightBlank(2);
                WriteRight("Last Name     :");
                WriteRightBlank(2);
                WriteRight("SIN           :");
                WriteRightBlank(2);
                WriteRight("Date of Birth :");
                if (employee is FullTimeEmployee || employee is PartTimeEmployee)
                {
                    WriteRightBlank(2);
                    WriteRight("Date of Hire  :");
                    WriteRightBlank(2);
                    WriteRight("Date of Term. :");
                    if (employee is FullTimeEmployee)
                    {
                        WriteRightBlank(2);
                        WriteRight("Salary        :");
                    }
                    else
                    {
                        WriteRightBlank(2);
                        WriteRight("Hourly Rate   :");
                    }
                }
                else if (employee is SeasonalEmployee)
                {
                    WriteRightBlank(2);
                    WriteRight("Season        :");
                    WriteRightBlank(2);
                    WriteRight("Piece Pay     :");
                }

            }
        }
        private void DisplaySearchParameterMenu()
        {
            WriteBlankLine(2);
            WriteCenter("==== Add Parameter ===============");
            WriteCenter("|                                |");
            WriteCenter("|  Select a search parameter     |");
            WriteCenter("|  to add by choosing the        |");
            WriteCenter("|  corresponding number          |");
            WriteCenter("|                                |");
            WriteCenter("|      1.       Type             |");
            WriteCenter("|      2.    First Name          |");
            WriteCenter("|      3.    Last Name           |");
            WriteCenter("|      4.       SIN              |");
            WriteCenter("|      5.   Date of Birth        |");
            WriteCenter("|      6.       Back             |");
            WriteCenter("|                                |");
            WriteCenter("==================================");
        }
        private void DisplayParameterPrompt()
        {
            WriteBlankLine(2);
            WriteCenter("==== Add Parameter ======================");
            WriteCenter("|                                        |");
            WriteCenter("|      Please enter a value for the      |");
            WriteCenter("| search parameter or nothing to remove  |");
            WriteCenter("|                                        |");
            WriteCenter("|                                        |");
            WriteCenter("|                                        |");
            WriteCenter("==========================================");
        }

        private void DisplayTypeParameterPrompt()
        {
            WriteBlankLine(2);
            WriteCenter("==== Add Parameter ======================");
            WriteCenter("|                                        |");
            WriteCenter("|      Please choose employee type       |");
            WriteCenter("|                                        |");
            WriteCenter("|          1. Full Time Employee         |");
            WriteCenter("|          2. Part Time Employee         |");
            WriteCenter("|          3. Seasonal  Employee         |");
            WriteCenter("|          4. Contract  Employee         |");
            WriteCenter("|          0. Any Employee Type          |");
            WriteCenter("|                                        |");
            WriteCenter("==========================================");
        }
        #endregion

        #region Misc
        private void DetectAddInfoErrors(string text, int field, Employee employee)
        {
            ClearInfoError();

            // todo: update employee fields, write error if necceasary
            if (text != "")
            {
                if (employee is ContractEmployee)
                {
                    DetectContractErrors(text, field, employee);
                }
                else
                {
                    DetectFullPartAndSeasonalErrors(text, field, employee);

                    if (employee is FullTimeEmployee || employee is PartTimeEmployee)
                    {
                        if (employee is FullTimeEmployee)
                        {
                            DetectFullErrors(text, field, employee);
                        }
                        else
                        {
                            DetectPartErrors(text, field, employee);
                        }
                    }
                    else if (employee is SeasonalEmployee)
                    {
                        DetectSeasonalErrors(text, field, employee);
                    }
                }
            }

        }
        private void DetectFullPartAndSeasonalErrors(string text, int field, Employee employee)
        {
            switch ((EmployeeFields)field)
            {
                case EmployeeFields.FirstName:
                    if (!employee.SetFirstName(text))
                    {
                        DisplayInfoError("First name invalid");
                    }
                    break;
                case EmployeeFields.LastName:
                    if (!employee.SetLastName(text))
                    {
                        DisplayInfoError("Last name invalid");
                    }
                    break;

                case EmployeeFields.SIN:
                    if (!employee.SetSIN(text))
                    {
                        DisplayInfoError("Sin invalid");
                    }
                    break;

                case EmployeeFields.DateOfBirth:
                    if (!employee.SetDateOfBirth(text))
                    {
                        DisplayInfoError("Date of birth invalid");
                    }
                    break;
            }
        }

        private void DetectFullErrors(string text, int field, Employee employee)
        {
            switch ((EmployeeFields)field)
            {
                case EmployeeFields.DateOfHire:
                    if (!(employee as FullTimeEmployee).SetDateOfHire(text))
                    {
                        DisplayInfoError("Date of hire invalid");
                    }
                    break;

                case EmployeeFields.DateOfTermination:
                    if (!(employee as FullTimeEmployee).SetDateOfTermination(text))
                    {
                        DisplayInfoError("Date of termination invalid");
                    }
                    break;

                case EmployeeFields.Salary:
                    if (!(employee as FullTimeEmployee).SetSalary(text))
                    {
                        DisplayInfoError("Salary invalid");
                    }
                    break;
            }

        }
        private void DetectPartErrors(string text, int field, Employee employee)
        {
            switch ((EmployeeFields)field)
            {
                case EmployeeFields.DateOfHire:
                    if (!(employee as PartTimeEmployee).SetDateOfHire(text))
                    {
                        DisplayInfoError("Date of hire invalid");
                    }
                    break;

                case EmployeeFields.DateOfTermination:
                    if (!(employee as PartTimeEmployee).SetDateOfTermination(text))
                    {
                        DisplayInfoError("Date of termination invalid");
                    }
                    break;
                case EmployeeFields.HourlyRate:
                    if (!(employee as PartTimeEmployee).SetHourlyRate(text))
                    {
                        DisplayInfoError("Hourly Rate invalid");
                    }
                    break;
            }
        }
        private void DetectSeasonalErrors(string text, int field, Employee employee)
        {
            switch ((EmployeeFields)field)
            {
                case EmployeeFields.Season:
                    if (!(employee as SeasonalEmployee).SetSeason(text))
                    {
                        DisplayInfoError("Season invalid");
                    }
                    break;

                case EmployeeFields.PiecePay:
                    if (!(employee as SeasonalEmployee).SetPiecePay(text))
                    {
                        DisplayInfoError("Piece Pay invalid");
                    }
                    break;

            }
        }
        private void DetectContractErrors(string text, int field, Employee employee)
        {
            switch ((EmployeeFields)field)
            {
                case EmployeeFields.CompanyName:
                    if (!(employee as ContractEmployee).SetLastName(text))
                    {
                        DisplayInfoError("Company name invalid");
                    }

                    break;
                case EmployeeFields.DateOfIncorporation:
                    if (!(employee as ContractEmployee).SetDateOfBirth(text))
                    {
                        DisplayInfoError("Date of Inc. invalid");
                    }

                    break;

                case EmployeeFields.BusinessNumber:
                    if (!(employee as ContractEmployee).SetSIN(text))
                    {
                        DisplayInfoError("Business Number invalid");
                    }

                    break;

                case EmployeeFields.StartDate:
                    if (!(employee as ContractEmployee).SetContractStartDate(text))
                    {
                        DisplayInfoError("Start date invalid");
                    }
                    break;

                case EmployeeFields.StopDate:
                    if (!(employee as ContractEmployee).SetContractEndDate(text))
                    {
                        DisplayInfoError("Stop date invalid");
                    }
                    break;

                case EmployeeFields.FixedAmount:
                    if (!(employee as ContractEmployee).SetFixedContractAmount(text))
                    {
                        DisplayInfoError("Fixed amount invalid");
                    }
                    break;
            }
        }
        private void FormFocus(List<TextBox> textboxes, ref int focusedField, Employee employee, Action<string, int, Employee> onChange)
        {
            bool cycle = true;
            while (cycle)
            {
                ConsoleKey key = textboxes[focusedField].Focus();

                // After user switches to another textbox, or escapes textbox, call provided function
                onChange(textboxes[focusedField].Text, focusedField, employee);

                if (key == ConsoleKey.DownArrow || key == ConsoleKey.Tab)
                {
                    if (focusedField < textboxes.Count - 1) focusedField++;
                }
                else if (key == ConsoleKey.UpArrow)
                {
                    if (focusedField > 0) focusedField--;
                }
                else
                {
                    cycle = false;
                }
            }
        }
        private void Clear()
        {
            Console.Clear();
        }
        private void WriteBlankLine(int howMany = 1)
        {
            for (int i = 0; i < howMany; i++)
            {
                Console.Write("\n");
            }
        }
        private void WriteLeftBlank(int howMany = 1)
        {
            Console.CursorTop += howMany;
        }
        private void WriteLeft(string text = "")
        {
            Console.CursorLeft = 0;

            //
            // Write text
            // Write a leading space for more padding between edge of command prompt
            //
            Console.Write(" " + text);

            // Move cursor to next line
            Console.CursorTop++;
        }
        private void WriteSeperator()
        {
            Console.CursorLeft = leftNavWidth - 2;
            Console.CursorTop = 0;
            while (Console.CursorTop < height)
            {
                Console.Write("│");
                Console.CursorTop++;
                Console.CursorLeft--;
            }
            Console.CursorTop = 0;
        }
        private void WriteRightBlank(int howMany = 1)
        {
            Console.CursorTop += howMany;

        }
        private void WriteRight(string text = "")
        {
            Console.CursorLeft = leftNavWidth;

            // Write text
            Console.Write(text);

            // Move cursor to next line
            Console.CursorTop++;
        }
        private void WriteCenter(string text)
        {
            string spaces = new string(' ', (int)(width / 2 - text.Length / 2));
            Console.WriteLine(spaces + text);
        }
        #endregion
    }
}
