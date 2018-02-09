using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[Serializable]
public class GameData
{
    public float testData;
}

public class GameController : MonoBehaviour {

    public GameData gameData;

    public static GameController instance;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnGUI()
    {
        if(GUI.Button(new Rect(10,100,100,30), "Test"))
        {

        }
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gameInfo.dat");

        bf.Serialize(file, gameData);
        file.Close();
    }

    public void Load()
    {
        
        string filePath = Application.persistentDataPath + "/gameInfo.dat";

        if (File.Exists(filePath))
        {

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(filePath, FileMode.Open);

            gameData = (GameData)bf.Deserialize(file);

            file.Close();
        }
        
    }
}

