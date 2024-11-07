using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using TMPro;
using System.Linq;
using System.Drawing;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class teacher : MonoBehaviour
{
    // connects to a file containing all the tables
    private string dbName = "URI=file:Databases.db";
    // input boxes used in the sign up 
    public TMP_InputField SusernameINP;
    public TMP_InputField SpasswordINP;
    public TMP_InputField SconfirmpasswordINP;
    public TMP_InputField LusernameINP;
    public TMP_InputField LpasswordINP;
    public static string username;
    public static string password;
    public static int readID;
    public static string code;
    public string confirmpassword;
    public TextMeshProUGUI submitStext;
    public TextMeshProUGUI codeText;
    public TMP_InputField classNameINP;
    public TMP_Dropdown selectClass;
    public TMP_Dropdown selectLevel;
    public List<string> classOptions;
    public List<string> details;
    public Component[] texts;
    public bool proceed;
    public GameObject studentDetails;
    public Transform parentObj;
    public RectTransform content;
    public GameObject StudentObj;
    public List<GameObject> StudentObjs;
    public Scrollbar scroll;
    bool first;
    bool tableExists;
    int a;

    public void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 36)
        {
            DropDownSetUp();
        }
        if (SceneManager.GetActiveScene().buildIndex == 37)
        {
            DropDownSetUp();
        }
        StudentObjs = new List<GameObject>();
    }
    public void clicked()
    {
        first = true; //to make sure its the first time it is being clicked
    }

    public void backToTM()
    {
        SceneManager.LoadScene(34);
    }

    public void TM()
    {
        if (proceed)
        {
            SceneManager.LoadScene(34);
        }
    }

    //Creates the Player database 
    public void Createdb()
    {
        //Creates a new Sqlite database
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                //Creates the table with the fields Username and Password which both have a max input length of 20 characters 
                command.CommandText = "CREATE TABLE IF NOT EXISTS Teacher(TeacherID INTEGER, Username VARCHAR(20), Password VARCHAR(20),PRIMARY KEY (TeacherID AUTOINCREMENT));";
                command.ExecuteNonQuery();
            }
            connection.Close();
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
                command.CommandText = "CREATE TABLE IF NOT EXISTS Class(ClassID VARCHAR(4), TeacherID INTEGER, className VARCHAR(20),PRIMARY KEY (ClassID));";
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
    }

    //Retrieves the entered textfrom the sign up page
    public void RetrieveFromSignUp()
    {
        username = SusernameINP.text;
        password = SpasswordINP.text;
        confirmpassword = SconfirmpasswordINP.text;


        if (username == "" || password == "" || confirmpassword == "")
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

    //Adds a new player into the PlayerInfo table
    public void AddTeacher()
    {
        if (proceed)
        {
            using (var connection = new SqliteConnection(dbName))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    //Inserts the new values into the database
                    command.CommandText = "INSERT INTO Teacher(Username, Password) " +
                        "VALUES ('" + username + "','" + password + "');";
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
    }

    public void CheckTeacher()
    {
        if (proceed)
        {
            using (var connection = new SqliteConnection(dbName))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    //Inserts the new values into the database
                    command.CommandText = "SELECT * FROM Teacher WHERE Username = '" + username + "'AND Password ='" + password + "';";
                    command.ExecuteNonQuery();
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        //Every line the SQL code outputs is read 
                        while (reader.Read())
                        {
                            //Stores password stored in the databases
                            object v = reader["TeacherID"];
                            readID = Convert.ToInt32(v);
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

    public void checkDbTeacher()
    {
        tableExists = false;
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name= 'Teacher'";
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

    public void checkDbClass()
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT COUNT(*) FROM Class WHERE TeacherID = @TeacherID;";
                command.Parameters.AddWithValue("@TeacherID", readID);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int count = reader.GetInt32(0);
                        if (count != 0)
                        {
                            tableExists = true;
                        }
                    }
                }
            }
        }
        if (!tableExists)
        {
            submitStext.text = "Add a class first";
        }
    }

    public void checkDbStudent()
    {
        tableExists = false;
        proceed = true;
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
            submitStext.text = "Get students to join your class";
            proceed = false;
        }
        else
        {
            checkDbLP();
        }
    }

    public void checkDbLP()
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name= 'LevelProgress'";
                using (IDataReader reader = command.ExecuteReader())
                {
                    tableExists = reader.Read();
                }
            }
        }
        if (!tableExists)
        {
            submitStext.text = "None of your students have started to complete levels";
            proceed = false;
        }
    }

    public void AddClass()
    {
        if (classNameINP.text == "" || code == null)
        {
            submitStext.text = "Fill in all inputs";
            proceed = false;
        }
        else
        {
            submitStext.text = "Submitted";
            proceed = true;
            using (var connection = new SqliteConnection(dbName))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    //Inserts the new values into the database
                    command.CommandText = "INSERT INTO Class(ClassID,TeacherID, className) " +
                        "VALUES ('" + code + "','" + readID + "','" + classNameINP.text + "');";
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

    }

    public void DropDownSetUp()
    {
        selectClass.ClearOptions();
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                //Inserts the new values into the database
                command.CommandText = "SELECT className FROM Class WHERE TeacherID = '" + readID + "';";
                command.ExecuteNonQuery();
                using (IDataReader reader = command.ExecuteReader())
                {
                    //Every line the SQL code outputs is read 
                    while (reader.Read())
                    {
                        //Stores password stored in the databases
                        object v = reader["className"];
                        classOptions.Add(v.ToString());
                    }
                    connection.Close();
                }
            }
        }
        selectClass.AddOptions(classOptions);
    }

    public void RemoveClass()
    {
        if (classOptions.Count != 0)
        {
            string selected = selectClass.options[selectClass.value].text;
            using (var connection = new SqliteConnection(dbName))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    if (tableExists)
                    {
                        //Inserts the new values into the database
                        command.CommandText = "DELETE FROM Class WHERE className = '" + selected + "'AND TeacherID ='" + readID + "';";
                        command.ExecuteNonQuery();
                    }
                }
                connection.Close();
            }
            classOptions.Clear();
            DropDownSetUp();
            checkDbClass();
        }
    }

    public void Generate()
    {
        System.Random rng = new System.Random();
        int number = rng.Next(0,10000);
        string digits = number.ToString("0000");
        code = digits;
        codeText.text = digits;
    }
    public void checkGen()
    {
        bool same = false;
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                //Inserts the new values into the database
                command.CommandText = "SELECT ClassID FROM Class;";
                command.ExecuteNonQuery();
                using (IDataReader reader = command.ExecuteReader())
                {
                    //Every line the SQL code outputs is read 
                    while (reader.Read())
                    {
                        //Stores password stored in the databases
                        object v = reader["ClassID"];
                        if (v.ToString() == code)
                        {
                            same = true;
                        }
                    }
                    connection.Close();
                }
            }
        }
        if (same)
        {
            Generate();
        }
    }

    public void DatabaseLeaderBoard()
    {
        if (proceed)
        {
            using (var connection = new SqliteConnection(dbName))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    //Inserts the new values into the database
                    command.CommandText = "SELECT Student.Username , LevelProgress.Score FROM Student JOIN Class ON Student.ClassID = Class.ClassID JOIN LevelProgress ON Student.StudentID = LevelProgress.StudentID WHERE LevelProgress.LevelNum = '" + selectLevel.value + "'AND Class.className = '" + selectClass.options[selectClass.value].text + "'AND  Class.TeacherID = '" + readID + "'ORDER BY Score DESC;";
                    command.ExecuteNonQuery();
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        //Every line the SQL code outputs is read 
                        while (reader.Read())
                        {
                            //Stores password stored in the databases
                            object v = reader["Username"];
                            details.Add(v.ToString());
                            object x = reader["Score"];
                            details.Add(x.ToString());
                        }
                        connection.Close();
                    }
                }
            }
        }

    }

    public void leadSetUp()
    {
        Vector3Int pos = new Vector3Int(0, 0, 0);
        /*float height = ((details.Count) / 2) * 106.25f;
        content.sizeDelta = new Vector2(800, height);*/
        scroll.value = 1;
        if (first)
        {
            for (; StudentObjs.Count > 0;)
            {
                Destroy(StudentObjs[0]);
                StudentObjs.RemoveAt(0);
            }
        }
        for (int i = 0; i < (details.Count) ; i = i +2)
        {
            StudentObj = Instantiate(studentDetails, pos, Quaternion.identity);
            StudentObjs.Add(StudentObj); // add to list so you can clear the leaderboard when changing classes or levels
            StudentObj.name = "studentDetails";
            StudentObj.transform.SetParent(parentObj);
            StudentObj.transform.SetAsLastSibling();
            texts = StudentObj.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (TextMeshProUGUI t in texts)
            {
                if(t.name == "user")
                {
                    t.text = details[i];
                }
                if(t.name == "score")
                {
                    t.text = details[i + 1];
                }
            }
        }
        details.Clear(); // clear details list
    }

    bool HasSpecialChars(string password)
    {
        return password.Any(ch => !char.IsLetterOrDigit(ch));
    }
}
