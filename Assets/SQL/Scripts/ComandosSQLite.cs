using Mono.Data.Sqlite;
using System.Data;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class ComandosSQLite : MonoBehaviour
{
    // Variable donde guardar la dirección de la Base de Datos
    private string rutaDB;
    private string strConexion;
    // Nombre de la base de datos con la que vamos a trabajar
    [SerializeField] private string DBFileName = "SweetSorceryDB.db";
    [SerializeField] int _playerID;

    // Referencia que necesitamos para poder crear una conexión 
    private IDbConnection dbConnection;
    // Referencia que necesitamos para poder ejecutar comandos
    private IDbCommand dbCommand;
    // Referencia que necesitamos para leer datos
    private IDataReader reader;

    // Singleton instance
    private static ComandosSQLite instance;

    public static ComandosSQLite Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Configurar la ruta de la base de datos
        ConfigurarRutaDB();
        ComandoSelect("*", "PlayerAccountData");
    }

    void ConfigurarRutaDB()
    {
        // Comprobamos en qué plataforma estamos
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            rutaDB = Application.dataPath + "/StreamingAssets/" + DBFileName;
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            rutaDB = Application.persistentDataPath + "/" + DBFileName;
            // Comprobar si el archivo se encuentra almacenado en persistent data
            if (!File.Exists(rutaDB))
            {
                // Almaceno el archivo en load db
                // Copio el archivo a persistent data
                WWW loadDB = new WWW("jar:file://" + Application.dataPath + DBFileName);
                while (!loadDB.isDone)
                {
                    // Wait until the file is loaded
                }
                File.WriteAllBytes(rutaDB, loadDB.bytes);
            }
        }

        strConexion = "URI=file:" + rutaDB;
    }

    void AbrirDB()
    {
        dbConnection = new SqliteConnection(strConexion);
        dbConnection.Open();
    }

    // Cerrar la conexión
    void CerrarDB()
    {
        if (reader != null)
        {
            reader.Close();
            reader = null;
        }
        if (dbCommand != null)
        {
            dbCommand.Dispose();
            dbCommand = null;
        }
        if (dbConnection != null)
        {
            dbConnection.Close();
            dbConnection = null;
        }
    }

    public void RegisterNewPlayer(string name, string pass)
    {
        if (IsNameTaken(name))
        {
            Debug.Log("El nombre de usuario ya está tomado.");
            return;
        }

        AbrirDB();
        using (var command = dbConnection.CreateCommand())
        {
            // Insert new player into PlayerAccountData
            command.CommandText = "INSERT INTO PlayerAccountData (Name, Pass) VALUES (@Name, @Pass)";

            var paramName = command.CreateParameter();
            paramName.ParameterName = "@Name";
            paramName.Value = name;
            command.Parameters.Add(paramName);

            var paramPass = command.CreateParameter();
            paramPass.ParameterName = "@Pass";
            paramPass.Value = pass;
            command.Parameters.Add(paramPass);

            command.ExecuteNonQuery();
            Debug.Log("Usuario: " +  name + "- Con contraseña: " + pass + " CREADO CON EXITO");
        }

        // Get the PlayerID of the new player
        long playerID = GetPlayerID(name);

        if (playerID != -1)
        {
            // Insert new entry into GameData
            using (var command = dbConnection.CreateCommand())
            {
                command.CommandText = "INSERT INTO GameData (PlayerID, Name) VALUES (@PlayerID, @Name)";

                var paramPlayerID = command.CreateParameter();
                paramPlayerID.ParameterName = "@PlayerID";
                paramPlayerID.Value = playerID;
                command.Parameters.Add(paramPlayerID);

                var paramName = command.CreateParameter();
                paramName.ParameterName = "@Name";
                paramName.Value = name;
                command.Parameters.Add(paramName);

                command.ExecuteNonQuery();
            }
        }

        CerrarDB();
    }

    public bool IsNameTaken(string name)
    {
        bool isTaken = false;

        AbrirDB();
        using (var command = dbConnection.CreateCommand())
        {
            command.CommandText = "SELECT COUNT(*) FROM PlayerAccountData WHERE Name = @Name";

            var paramName = command.CreateParameter();
            paramName.ParameterName = "@Name";
            paramName.Value = name;
            command.Parameters.Add(paramName);

            long count = (long)command.ExecuteScalar();
            isTaken = count > 0;
        }
        CerrarDB();

        return isTaken;
    }

    public void UpdateGameData(string columna, int? newValue = null)
    {
        AbrirDB();

        // Sentencia SQL para actualizar el valor de la columna en GameData
        string sqlQuery = $"UPDATE GameData SET {columna} = ";

        if (newValue.HasValue)
        {
            sqlQuery += "@NewValue";
        }
        else
        {
            sqlQuery += $"{columna} + 1";
        }

        sqlQuery += " WHERE PlayerID = @PlayerID";

        using (var command = dbConnection.CreateCommand())
        {
            command.CommandText = sqlQuery;

            if (newValue.HasValue)
            {
                var paramNewValue = command.CreateParameter();
                paramNewValue.ParameterName = "@NewValue";
                paramNewValue.Value = newValue.Value;
                command.Parameters.Add(paramNewValue);
            }

            var paramPlayerID = command.CreateParameter();
            paramPlayerID.ParameterName = "@PlayerID";
            paramPlayerID.Value = _playerID;
            command.Parameters.Add(paramPlayerID);

            // Ejecutar la consulta
            command.ExecuteNonQuery();
        }

        CerrarDB();
    }

    private long GetPlayerID(string name)
    {
        long playerID = -1;

        using (var command = dbConnection.CreateCommand())
        {
            command.CommandText = "SELECT PlayerID FROM PlayerAccountData WHERE Name = @Name";

            var paramName = command.CreateParameter();
            paramName.ParameterName = "@Name";
            paramName.Value = name;
            command.Parameters.Add(paramName);

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    playerID = (long)reader["PlayerID"];
                }
            }
        }

        return playerID;
    }

    void ComandoSelect(string select, string table)
    {
        AbrirDB();
        dbCommand = dbConnection.CreateCommand();
        string sqlQuery = "SELECT " + select + " FROM " + table;
        dbCommand.CommandText = sqlQuery;
        reader = dbCommand.ExecuteReader();
        while (reader.Read())
        {
            // Aquí procesaríamos los datos obtenidos de la consulta
            Debug.Log("Name: " + reader["Name"] + " - Pass: " + reader["Pass"]);
        }
        CerrarDB();
    }

    public void TestGameData(string columna)
    {
        UpdateGameData(columna);
    }
}
