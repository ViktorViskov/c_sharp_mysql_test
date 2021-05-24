// libs
using System;
using MySql.Data.MySqlClient;

// namespace
namespace connection
{

    // class for open connection
    class Connection
    {
        // variables
        string settingsSting;
        MySqlConnection connection;

        // constructor
        public Connection(string address, string userName, string password)
        {
            // make connection string
            this.settingsSting = $"server={address};userid={userName};password={password};";

            // open connection
            Open();
        }

        // method for open connection
        public void Open()
        {
            connection = new MySqlConnection(settingsSting);
            connection.Open();
        }

        // method for send sql requst
        public void I(string sqlRequest)
        {
            // make command
            MySqlCommand cmd = new MySqlCommand(sqlRequest, this.connection);

            //send command
            cmd.ExecuteNonQuery();
        }

        // method for send sql request and get response
        public string IO(string sqlRequest)
        {
            // make command
            MySqlCommand cmd = new MySqlCommand(sqlRequest, this.connection);

            // send command and get data
            MySqlDataReader response = cmd.ExecuteReader();

            // buffer string 
            string bufferString = "";

            // getting data
            while (response.Read())
            {

                // Get columns count
                int columns = response.FieldCount;

                // loop for get all data from all columns
                for (int i = 0; i < columns; i++)
                {
                    // add one column
                    bufferString += $"{response[i]}";

                    // add column separator
                    if (i + 1 != columns)
                    {
                        bufferString += ",";
                    }
                }
                // add row separator
                bufferString += ";";
            }

            // delete last separator
            if (bufferString.Length > 0)
            {
                bufferString = bufferString.Substring(0, bufferString.Length - 1);
            }

            // close connection
            response.Close();

            // result
            return bufferString;
        }
    }
}