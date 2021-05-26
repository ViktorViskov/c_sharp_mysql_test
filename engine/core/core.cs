// libs
using ui;
using utils;
using connection;


namespace core
{
    // main class for app
    class Core
    {
        // variables
        private UI display = new UI();
        private Connection con;

        // pages
        pageData logIn = new pageData("LogIn page", "ESC > exit, Up Down > Navigation, Enter > input data, Space > continue", new string[] { "Address", "User name", "Password" });
        pageData mainMenu = new pageData("Main menu", "ESC > exit, Up Down > Navigation, Left Right > Pages, Space > select", new string[] { "Show all schools", "Show all classes", "Show students", "Add student", "Create school", "Create class", "Create table", "Delete school", "Delete class" });
        pageData createDatabase = new pageData("Create school", "ESC > back, Up Down > Navigation, Enter > input data, Space > continue", new string[] { "School name" });
        pageData createTable = new pageData("Create table", "ESC > back, Up Down > Navigation, Enter > input data, Space > continue", new string[] { "Table name", "Number of columns" });
        pageData createClass = new pageData("Create Class", "ESC > back, Up Down > Navigation, Enter > input data, Space > continue", new string[] { "Class name" });
        pageData connectionError = new pageData("Connection error", "Press ESC to exit...", new string[] { "Login or password not correct!" });
        pageData incorrectInput = new pageData("Input error", "Press ESC to continue...", new string[] { "Check inputed data and try again" });
        pageData successMessage = new pageData("Success", "Press ESC to continue...", new string[] { "Operation was successful" });


        // constructor
        public Core()
        {
            // app initialization


            // start application
            App();

        }

        // Application main loop
        public void App()
        {

            // variables
            bool exit = false;

            // 
            // start application
            // 

            // get data about connection
            object[] connectionProperties = display.Input(logIn);

            // try exception
            try
            {
                // open connection
                con = new Connection(connectionProperties[0].ToString(), connectionProperties[1].ToString(), connectionProperties[2].ToString());
            }
            catch
            {
                // connection error
                display.Message(connectionError);

                // stop application loop
                exit = true;
            }


            // 
            // main interface loop
            // 
            while (!exit)
            {
                // variables;
                string[] databases;
                string[] tables;
                string databaseName;
                string tableName;
                string[] tableInfo;
                string[] requestArray;
                object[] userInput;
                int columnAmount;
                string stringRequest;

                // show menu and get user action
                int userAction = display.Menu(mainMenu);

                // user action processing
                switch (userAction)
                {
                    // exit
                    case -1:
                        exit = true;
                        break;

                    // first menu action show all databases
                    case 0:
                        // load databases list
                        databases = con.IO("SHOW DATABASES").Split(";");

                        // show to screen
                        display.Message(new pageData("School list", "Left Right > Pages, Press ESC to back...", databases));
                        break;

                    // show all tables from some database
                    case 1:
                        // load databases list
                        databases = con.IO("SHOW DATABASES").Split(";");

                        // try exception
                        try
                        {
                            // show menu and get database name
                            databaseName = databases[display.Menu(new pageData("Select school to show classes", "ESC > exit, Up Down > Navigation, Left Right > Pages, Space > select", databases))];

                            // show all tables from this database
                            display.Message(new pageData($"Classes from {databaseName} school", "Press ESC to back to main menu...", con.IO($"SHOW TABLES FROM {databaseName}").Split(";")));
                        }

                        // if pressed esc, continue loop
                        catch
                        {
                            break;
                        }

                        break;

                    // show all data from table
                    case 2:
                        // load databases list
                        databases = con.IO("SHOW DATABASES").Split(";");

                        // try exception
                        try
                        {
                            // show menu and get database name
                            databaseName = databases[display.Menu(new pageData("Select school to show classes", "ESC > exit, Up Down > Navigation, Left Right > Pages, Space > select", databases))];

                            // load tables list
                            tables = con.IO($"SHOW TABLES FROM {databaseName}").Split(";");

                            // try exception
                            try
                            {
                                // show menu and get table name
                                tableName = tables[display.Menu(new pageData("Select class to show all students", "ESC > exit, Up Down > Navigation, Left Right > Pages, Space > select", tables))];

                                // show all data from this table
                                display.Message(new pageData($"Students from {databaseName}/{tableName}", "Left Right > Pages, Press ESC to back to main menu...", con.IO($"SELECT * FROM {databaseName}.{tableName}").Replace(",", "\t").Split(";")));
                            }

                            // if esc stop
                            catch
                            {
                                break;
                            }
                        }

                        // if pressed esc, continue loop
                        catch
                        {
                            break;
                        }

                        break;

                    // add data to database
                    case 3:
                        // load databases list
                        databases = con.IO("SHOW DATABASES").Split(";");

                        // try exception
                        try
                        {
                            // show menu and get database name
                            databaseName = databases[display.Menu(new pageData("Select school to show classes", "ESC > exit, Up Down > Navigation, Left Right > Pages, Space > select", databases))];

                            // load tables list
                            tables = con.IO($"SHOW TABLES FROM {databaseName}").Split(";");

                            // try exception
                            try
                            {
                                // show menu and get table name
                                tableName = tables[display.Menu(new pageData("Select class to show all students", "ESC > exit, Up Down > Navigation, Left Right > Pages, Space > select", tables))];

                                // try exception
                                try
                                {
                                    // load all information about table
                                    tableInfo = con.IO($"DESCRIBE {databaseName}.{tableName}").Split(";");

                                    // make request array
                                    requestArray = new string[tableInfo.Length];

                                    // add variables to request array
                                    for (int i = 0; i < requestArray.Length; i++)
                                    {
                                        requestArray[i] = tableInfo[i].Split(",")[0] + $" [{tableInfo[i].Split(",")[1]}]";
                                    }

                                    // getting data from user
                                    userInput = display.Input(new pageData($"Add student to {databaseName}/{tableName}", "ESC > back, Up Down > Navigation, Enter > input data, Space > continue", requestArray));

                                    if (userInput.Length != 0)
                                    {
                                        try
                                        {
                                            // make request
                                            con.I($"INSERT INTO {databaseName}.{tableName} VALUES ('{string.Join("','", userInput)}')");

                                            // print message
                                            display.Message(successMessage);
                                        }
                                        // print message error
                                        catch
                                        {
                                            display.Message(incorrectInput);
                                        }
                                    }

                                }
                                // if error or esc, stop
                                catch
                                {
                                    break;
                                }

                            }

                            // if esc stop
                            catch
                            {
                                break;
                            }
                        }

                        // if pressed esc, continue loop
                        catch
                        {
                            break;
                        }
                        break;

                    // create database
                    case 4:
                        // getting data from user
                        userInput = display.Input(createDatabase);

                        // if user input not empty
                        if (userInput.Length != 0)
                        {
                            try
                            {
                                // make request
                                con.I($"CREATE DATABASE {userInput[0]}");

                                // print message
                                display.Message(successMessage);
                            }
                            // print message error
                            catch
                            {
                                display.Message(incorrectInput);
                            }
                        }

                        break;

                    // create class
                    case 5:
                        // load databases list
                        databases = con.IO("SHOW DATABASES").Split(";");

                        // try exeption
                        try
                        {
                            // show menu and get database name
                            databaseName = databases[display.Menu(new pageData("Select database to create table", "ESC > exit, Up Down > Navigation, Space > select", databases))];

                            // try exception
                            try
                            {
                                // get information about table
                                userInput = display.Input(createClass);

                                // check for userInput is not empty
                                if (userInput.Length != 0)
                                {
                                    // try exception
                                    try
                                    {
                                        // make request
                                        con.I($"CREATE TABLE {databaseName}.{userInput[0]} (CPR INT, name varchar(255), age int, PRIMARY KEY (CPR))");

                                        // print message
                                        display.Message(successMessage);
                                    }

                                    // print message error
                                    catch
                                    {
                                        display.Message(incorrectInput);
                                    }
                                }
                            }
                            // incorect number for columns
                            catch
                            {
                                display.Message(incorrectInput);
                            }
                        }

                        // if cancel back
                        catch
                        {
                            break;
                        }
                        break;

                    // create table
                    case 6:
                        // load databases list
                        databases = con.IO("SHOW DATABASES").Split(";");

                        // try exception
                        try
                        {
                            // show menu and get database name
                            databaseName = databases[display.Menu(new pageData("Select database to create table", "ESC > exit, Up Down > Navigation, Space > select", databases))];

                            // try exception
                            try
                            {
                                // get information about table
                                userInput = display.Input(createTable);

                                // check for userInput is not empty
                                if (userInput.Length != 0)
                                {
                                    // proccess user inputed data
                                    tableName = $"{userInput[0]}";
                                    columnAmount = int.Parse($"{userInput[1]}");

                                    // make request array
                                    requestArray = new string[columnAmount * 2];

                                    // add variables to request array
                                    for (int i = 0, j = 1; i < requestArray.Length; i += 2, j++)
                                    {
                                        requestArray[i] = $"Column name {j}:";
                                        requestArray[i + 1] = $"Column type {j}:";
                                    }

                                    // getting data from user
                                    userInput = display.Input(new pageData($"Creating table {tableName} in {databaseName}", "ESC > back, Up Down > Navigation, Enter > input data, Space > continue", requestArray));

                                    // if user input not 0
                                    if (userInput.Length != 0)
                                    {
                                        // try exception
                                        try
                                        {
                                            // init string request
                                            stringRequest = "";
                                            // create string request
                                            for (int i = 0; i < userInput.Length; i += 2)
                                            {
                                                // add options
                                                stringRequest += $"{userInput[i]} {userInput[i + 1]}";

                                                // add separator if not last
                                                if (i + 2 < userInput.Length)
                                                {
                                                    stringRequest += ",";
                                                }
                                            }

                                            // make request
                                            con.I($"CREATE TABLE {databaseName}.{tableName} ({stringRequest})");

                                            // print message
                                            display.Message(successMessage);
                                        }
                                        // print message error
                                        catch
                                        {
                                            display.Message(incorrectInput);
                                        }
                                    }
                                }
                            }
                            // incorect number for columns
                            catch
                            {
                                display.Message(incorrectInput);
                            }
                        }

                        // if cancel back
                        catch
                        {
                            break;
                        }
                        break;

                    // delete database
                    case 7:
                        // load databases list
                        databases = con.IO("SHOW DATABASES").Split(";");

                        // try exception
                        try
                        {
                            // show menu and get database name
                            databaseName = databases[display.Menu(new pageData("Select school to show classes", "ESC > exit, Up Down > Navigation, Left Right > Pages, Space > select", databases))];

                            // ask user about action
                            if (display.Menu(new pageData($"Are you really want to delete {databaseName} school?", "ESC > exit, Up Down > Navigation, Space > select", new string[] { "No", "Yes" })) == 1)
                            {
                                // make request for deleting
                                con.I($"DROP DATABASE {databaseName}");

                                // show success message
                                display.Message(successMessage);
                            }

                        }

                        // if pressed esc, continue loop
                        catch
                        {
                            break;
                        }

                        break;

                    // dete table from database
                    case 8:
                        // load databases list
                        databases = con.IO("SHOW DATABASES").Split(";");

                        // try exception
                        try
                        {
                            // show menu and get database name
                            databaseName = databases[display.Menu(new pageData("Select school to show classes", "ESC > exit, Up Down > Navigation, Left Right > Pages, Space > select", databases))];

                            // load tables list
                            tables = con.IO($"SHOW TABLES FROM {databaseName}").Split(";");

                            // try exception
                            try
                            {
                                // show menu and get table name
                                tableName = tables[display.Menu(new pageData("Select class to delete", "ESC > exit, Up Down > Navigation, Left Right > Pages, Space > select", tables))];

                                // ask user about action
                                if (display.Menu(new pageData($"Are you really want to delete {databaseName}/{tableName} class?", "ESC > exit, Up Down > Navigation, Space > select", new string[] { "No", "Yes" })) == 1)
                                {
                                    // make request for deleting
                                    con.I($"DROP TABLE {databaseName}.{tableName}");

                                    // show success message
                                    display.Message(successMessage);
                                }
                            }

                            // if esc stop
                            catch
                            {
                                break;
                            }
                        }

                        // if pressed esc, continue loop
                        catch
                        {
                            break;
                        }
                        break;
                }
            }
        }
    }
}