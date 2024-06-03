using System; //Librería para conectar con Files
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;//Librería añadida para trabajar con la DB
using System.IO;//Librería para poder abrir archivos
using Mono.Data.Sqlite;//Librería para trabajar con SQLite
using UnityEngine.UI; //Para usar el Canvas de Unity

public class ConexDB : MonoBehaviour
{
    //Variable donde guardar la dirección de la Base de Datos
    string rutaDB;
    string strConexion;
    //Nombre de la base de datos con la que vamos a trabajar
    string DBFileName = "ZapatosJuanAlbertoDB.db";
    //Variable texto UI
    public Text myText;

    //Referencia que necesitamos para poder crear una conexión 
    IDbConnection dbConnection;
    //Referencia que necesitamos para poder ejecutar comandos
    IDbCommand dbCommand;
    //Referencia que necesitamos para leer datos
    IDataReader reader;

    // Start is called before the first frame update
    void Start()
    {
        //Llamamos al método que abre las conexiones
        AbrirDB();
    }

    //Método que nos servirá para abrir la conexión con la Base de Datos
    void AbrirDB()
    {
        //Crear y abrir la conexión
        //Compuebo en que plataforma estamos
        //Si estamos en PC mantenemos la ruta
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //Ruta dentro del pc para buscar la base de datos
            rutaDB = Application.dataPath + "/StreamingAssets/" + DBFileName;
        }
        //Si estamos en Android
        else if (Application.platform == RuntimePlatform.Android)
        {
            //Ruta dentro del dispositivo Android para buscar la base de datos
            rutaDB = Application.persistentDataPath + "/" + DBFileName;
            //Comprobar si el archivo se encuentra almacenado en persistant data
            if (!File.Exists(rutaDB))
            {
                //Almaceno el archivo en load db
                WWW loadDB = new WWW("jar;file://" + Application.dataPath + DBFileName);
                while (!loadDB.isDone)
                {

                }
                //Copio el archivo a persistant data
                File.WriteAllBytes(rutaDB, loadDB.bytes);
            }
        }

        //Para leer esta misma base de datos desde un dispositivo móvil
        strConexion = "URI=file:" + rutaDB;
        //Creamos una nueva conexión usando esa ruta
        dbConnection = new SqliteConnection(strConexion);
        //Abrimos esa conexión
        dbConnection.Open();

        //Crear la consulta
        //Generamos un comando para la conexión que hemos abierto
        dbCommand = dbConnection.CreateCommand();
        //El query
        string sqlQuery = "SELECT * FROM zapatillas";
        //Le pasamos el query al comando que vamos a ejecutar
        dbCommand.CommandText = sqlQuery;

        //Leer la base de datos
        //Ejecutamos el comando que hemos creado en formato lectura de datos
        reader = dbCommand.ExecuteReader();
        //Haremos que lea datos hasta que no queden más por leer de ese query
        while (reader.Read())
        {
            //id -> recojo el dato de la primera columna
            int id = reader.GetInt32(0);
            //marca -> recojo el dato de la segunda columna
            string marca = reader.GetString(1);
            //color -> recojo el dato de la tercera columna
            string color = reader.GetString(2);
            //cantidad -> recojo el dato de la cuarta columna
            int cantidad = reader.GetInt32(3);
            //Mostramos en consola los datos obtenidos de cada fila
            Debug.Log(id + " - " + marca + " - " + color + " - " + cantidad);
            myText.text = id.ToString() + " - " + marca + " - " + color + " - " + cantidad.ToString();
        }

        //Cerrar las conexiones
        //Cerramos el lector de datos
        reader.Close();
        //Vaciamos por si acaso el lector de datos
        reader = null;
        //Dejamos de disponer del comando que habíamos creado
        dbCommand.Dispose();
        //Vaciamos por si acaso el comando que habíamos creado
        dbCommand = null;
        //Cerramos la conexión que hemos abierto
        dbConnection.Close();
        //Vaciamos por si acaso la conexión que hemos usado
        dbConnection = null;
    }

}
