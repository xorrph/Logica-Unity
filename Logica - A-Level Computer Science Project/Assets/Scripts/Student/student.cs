using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using TMPro;
using System.Linq;
using System.Drawing;
using System;
using UnityEngine.SceneManagement;

public class student : MonoBehaviour
{
    // connects to a file containing all the tables
    private string dbName = "URI=file:Databases.db";
    // input boxes used in the sign up 
    public TMP_InputField SusernameINP;
    public TMP_InputField SpasswordINP;
    public TMP_InputField SconfirmpasswordINP;
    public TMP_InputField ScodeINP;
    public TMP_InputField LusernameINP;
    public TMP_InputField LpasswordINP;
    public static string username;
    public static string password;
    public static int readID;
    public string readPassword;
    public string confirmpassword;
    public string code;
    public float readSens;
    public int readGFX;
    public string readScore;
    public TextMeshProUGUI submitStext;
    bool proceed;
    int na;
    bool tableExists;


    //Creates the Student database 
    public void Createdb()
    {
        //Creates a new Sqlite database
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                //Creates the table with the fields Username and Password which both have a max input length of 20 characters 
                command.CommandText = "CREATE TABLE IF NOT EXISTS Student(StudentID INTEGER, ClassID VARCHAR(4), Username VARCHAR(20), Password VARCHAR(20),Sensitivity FLOAT, Graphics INTEGER, PRIMARY KEY (StudentID AUTOINCREMENT));";
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
    }

    public void MainMenu()
    {
        if (proceed)
        {
            SceneManager.LoadScene(22);
            gameObject.GetComponent<ScreenDirecter>().SetIn();
        }
    }

    public void Createdb2()
    {
        //Creates a new Sqlite database
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                //Creates the table with the fields Username and Password which both have a max input length of 20 characters 
                command.CommandText = "CREATE TABLE IF NOT EXISTS LevelProgress(StudentID INTEGER, LevelNum INTEGER, Score VARCHAR(20), PRIMARY KEY (StudentID, LevelNum));";
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
    }

    public void checkDbStudent()
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name= 'Student'";
                using (IDataReader reader = command.ExecuteReader())
                {
                    tableExists = reader.Read();
                }
            }
        }
        if (!tableExists)
        {
            submitStext.text = "Sign up first";
            proceed = false;
        }
    }


    //Retrieves the entered textfrom the sign up page
    public void RetrieveFromSignUp()
    {
        username = SusernameINP.text;
        password = SpasswordINP.text;
        confirmpassword = SconfirmpasswordINP.text;
        code = ScodeINP.text;
        if (username == "" || password == "" || confirmpassword == "" || code == "")
        {
            submitStext.text = "Fill in all inputs";
        }
        else if (password.Length < 8)
        {
            submitStext.text = "Passwords is too short";
        }
        else if (HasSpecialChars(password) == false)
        {
            submitStext.text = "Password does not have a special character";
        }
        else if (password != confirmpassword)
        {
            submitStext.text = "Passwords did not match";
        }
        else if(!int.TryParse(code, out na))
        {
            submitStext.text = "Code should be a number";
        }
        else if (code.Length > 4 || code.Length < 4)
        {
            submitStext.text = "Code is not 4 digits";
        }
        else
        {
            proceed = true;
        }
    }

    public void RetrieveFromLogIn()
    {
        username = LusernameINP.text;
        password = LpasswordINP.text;

        if (username == "" || password == "")
        {
            submitStext.text = "Fill in all inputs";
        }
        else
        {
            proceed = true;
        }
    }

    //Adds a new player into the Student table
    public void AddStudent()
    {
        if (proceed)
        {
            using (var connection = new SqliteConnection(dbName))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    //Inserts the new values into the database
                    command.CommandText = "INSERT INTO Student(ClassID,Username, Password, Sensitivity, Graphics) " +
                        "VALUES ('" + code + "','" + username + "','" + password + "','" + 2.5 + "','" + 0 + "');";
                    command.ExecuteNonQuery();
                    command.CommandText = command.CommandText = "SELECT * FROM Student WHERE Username = '" + username + "'AND Password ='" + password + "';";
                    command.ExecuteNonQuery();
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        //Every line the SQL code outputs is read 
                        while (reader.Read())
                        {
                            object x = reader["StudentID"];
                            readID = Convert.ToInt32(x);
                        }
                        connection.Close();

                    }
                }
                connection.Close();
            }
        }
    }

    public void CheckStudent()
    {
        if (proceed)
        {
            using (var connection = new SqliteConnection(dbName))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    //Inserts the new values into the database
                    command.CommandText = "SELECT * FROM Student WHERE Username = '" + username + "'AND Password ='" + password + "';";
                    command.ExecuteNonQuery();
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        //Every line the SQL code outputs is read 
                        while (reader.Read())
                        {
                            //Stores the settings stored in the databases
                            object v = reader["Sensitivity"];
                            readSens = float.Parse(v.ToString());
                            object c = reader["Graphics"];
                            readGFX = Convert.ToInt32(c);
                            object x = reader["StudentID"];
                            readID = Convert.ToInt32(x);
                        }
                        connection.Close();
                    }
                }
                if (readID == 0)
                {
                    submitStext.text = "Incorrect Password";
                    proceed = false;
                }
            }
        }
    }

    public void UpdateSens(float sens)
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                //Updates the new values
                command.CommandText = "UPDATE Student SET Sensitivity = '" + sens + "'  WHERE Username = '" + username + "'AND Password ='" + password + "';";
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
    }

    public void UpdateGraphics(int gfx)
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                //Updates the new values
                command.CommandText = "UPDATE Student SET Graphics = '" + gfx + "'  WHERE Username = '" + username + "'AND Password ='" + password + "';";
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
    }

    public void intialiseSettings()
    {
        gameObject.GetComponent<PlayerInfo>().setGraphics(readGFX);
        gameObject.GetComponent<PlayerInfo>().setSens(readSens);
    }

    public void studentScore(int score, int level)
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                //executes sql query
                command.CommandText = "SELECT * FROM LevelProgress WHERE StudentID = '" + readID + "'AND LevelNum ='" + level + "';";
                command.ExecuteNonQuery();
                using (IDataReader reader = command.ExecuteReader())
                {
                    //Every line the SQL code outputs is read 
                    while (reader.Read())
                    {
                        // Reads the current score stored
                        object v = reader["Score"];
                        readScore = v.ToString();
                    }
                }
                // if it is empty it inserts a new entry to the database
                if (readScore.ToString() == "")
                {
                    command.CommandText = "INSERT INTO LevelProgress(StudentID,LevelNum, Score) " +
                       "VALUES ('" + readID + "','" + level + "','" + score.ToString() + "');";
                    command.ExecuteNonQuery();

                }
                // if new score is larger than the stored database it updates it
                else if (score > Convert.ToInt32(readScore))
                {
                    //Updates the new values
                    command.CommandText = "UPDATE LevelProgress SET Score = '" + score.ToString() + "'  WHERE StudentID = '" + readID + "'AND LevelNum ='" + level + "';";
                    command.ExecuteNonQuery();
                }
            }
            connection.Close();
        }
    }

    bool HasSpecialChars(string password)
    {
        return password.Any(ch => !char.IsLetterOrDigit(ch));
    }
}
