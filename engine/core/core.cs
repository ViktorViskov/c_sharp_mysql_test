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
        pageData mainMenu = new pageData("Main menu", "ESC > exit, Up Down > Navigation, Space > select", new string[] { "Show all folders", "Show all tables", "Show table", "Insert data", "Create folder", "Create table", "Delete folder", "Delete table" });
        pageData createDatabase = new pageData("Create folder", "ESC > back, Up Down > Navigation, Enter > input data, Space > continue", new string[] { "Folder name" });
        pageData createTable = new pageData("Create table", "ESC > back, Up Down > Navigation, Enter > input data, Space > continue", new string[] { "Table name", "Number of columns" });
        pageData connectionError = new pageData("Connection error", "Press any key to exit...", new string[] { "Login or password not correct!" });
        pageData incorrectInput = new pageData("Input error", "Press any key to continue...", new string[] { "Check inputed data and try again" });
        pageData successMessage = new pageData("Success", "Press any key to continue...", new string[] { "Operation was successful" });


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
                        display.Message(new pageData("Folders list", "Press any key to back...", databases));
                        break;

                    // show all tables from some database
                    case 1:
                        // load databases list
                        databases = con.IO("SHOW DATABASES").Split(";");

                        // TODO fix bug if many values its give console error. Cursor position is - number

                        // try exception
                        try
                        {
                            // show menu and get database name
                            databaseName = databases[display.Menu(new pageData("Select folder to show tables", "ESC > exit, Up Down > Navigation, Space > select", databases))];

                            // show all tables from this database
                            display.Message(new pageData($"Tables list from {databaseName}", "Press any key to back to main menu...", con.IO($"SHOW TABLES FROM {databaseName}").Split(";")));
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

                        // TODO fix bug if many values its give console error. Cursor position is - number

                        // try exception
                        try
                        {
                            // show menu and get database name
                            databaseName = databases[display.Menu(new pageData("Select folder to show tables", "ESC > exit, Up Down > Navigation, Space > select", databases))];

                            // load tables list
                            tables = con.IO($"SHOW TABLES FROM {databaseName}").Split(";");

                            // try exception
                            try
                            {
                                // show menu and get table name
                                tableName = tables[display.Menu(new pageData("Select table to show all data", "ESC > exit, Up Down > Navigation, Space > select", tables))];

                                // show all data from this table
                                display.Message(new pageData($"Data from {databaseName}/{tableName}", "Press any key to back to main menu...", con.IO($"SELECT * FROM {databaseName}.{tableName}").Replace(",", "\t").Split(";")));
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

                        // TODO fix bug if many values its give console error. Cursor position is - number

                        // try exception
                        try
                        {
                            // show menu and get database name
                            databaseName = databases[display.Menu(new pageData("Select folder to show tables", "ESC > exit, Up Down > Navigation, Space > select", databases))];

                            // load tables list
                            tables = con.IO($"SHOW TABLES FROM {databaseName}").Split(";");

                            // try exception
                            try
                            {
                                // show menu and get table name
                                tableName = tables[display.Menu(new pageData("Select table to show all data", "ESC > exit, Up Down > Navigation, Space > select", tables))];

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
                                    userInput = display.Input(new pageData($"Add data to {databaseName}/{tableName}", "ESC > back, Up Down > Navigation, Enter > input data, Space > continue", requestArray));

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

                    // create table
                    case 5:
                        // load databases list
                        databases = con.IO("SHOW DATABASES").Split(";");

                        // TODO fix bug if many values its give console error. Cursor position is - number

                        // try exception
                        try
                        {
                            // show menu and get database name
                            databaseName = databases[display.Menu(new pageData("Select folder to create table", "ESC > exit, Up Down > Navigation, Space > select", databases))];

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
                    case 6:
                        // load databases list
                        databases = con.IO("SHOW DATABASES").Split(";");

                        // TODO fix bug if many values its give console error. Cursor position is - number

                        // try exception
                        try
                        {
                            // show menu and get database name
                            databaseName = databases[display.Menu(new pageData("Select folder to show tables", "ESC > exit, Up Down > Navigation, Space > select", databases))];

                            // ask user about action
                            if (display.Menu(new pageData($"Are you really want to delete {databaseName} folder?", "ESC > exit, Up Down > Navigation, Space > select", new string[] { "No", "Yes" })) == 1)
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
                    case 7:
                        // load databases list
                        databases = con.IO("SHOW DATABASES").Split(";");

                        // TODO fix bug if many values its give console error. Cursor position is - number

                        // try exception
                        try
                        {
                            // show menu and get database name
                            databaseName = databases[display.Menu(new pageData("Select folder to show tables", "ESC > exit, Up Down > Navigation, Space > select", databases))];

                            // load tables list
                            tables = con.IO($"SHOW TABLES FROM {databaseName}").Split(";");

                            // try exception
                            try
                            {
                                // show menu and get table name
                                tableName = tables[display.Menu(new pageData("Select table to show all data", "ESC > exit, Up Down > Navigation, Space > select", tables))];

                                // ask user about action
                                if (display.Menu(new pageData($"Are you really want to delete {databaseName}/{tableName} table?", "ESC > exit, Up Down > Navigation, Space > select", new string[] { "No", "Yes" })) == 1)
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