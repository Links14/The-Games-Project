using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using static UnityEngine.RuleTile.TilingRuleOutput;
using System.Linq;
using System;
using UnityEditor;
using static UnityEditor.Experimental.GraphView.GraphView;
using Unity.Mathematics;


[System.Serializable]
public class SaveData
{
    public List<Tuple<Vector3, float, float, int>> plants; // Position, growth, water, stage

    public List<SerializableVector3> plantPos;
    public List<float> plantGrowth;
    public List<float> plantWater;
    public List<int> plantStage;
    public List<string> plantID;

}

public class SaveManager
{
    private string dataDirPath;
    private string dataFileName;
    private bool useEncryption;

    private readonly string encryptionKey = "StabMonter";

    public SaveManager(string _dataFileName, bool _useEncryption = true)
    {
        this.dataDirPath = Application.persistentDataPath;
        this.dataFileName = _dataFileName;
        this.useEncryption = _useEncryption;
    }

    // simple XOR encryption scheme
    private string EncryptDecrypt(string _data)
    {
        string modifiedData = "";
        for (int i = 0; i < _data.Length; i++)
        {
            modifiedData += (char)(_data[i] ^ encryptionKey[i % encryptionKey.Length]);
        }
        return modifiedData;
    }

    public SaveData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        SaveData loadedData = null;
        string dataToLoad = "";

        if (File.Exists(fullPath))
        {
            try
            {
                // load the serialized data
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                // optionally decrypt data
                if (useEncryption)
                    dataToLoad = EncryptDecrypt(dataToLoad);

                // deserialize data
                loadedData = JsonUtility.FromJson<SaveData>(dataToLoad);
            }
            catch
            {
                Debug.LogError("Error Loading data at filepath " + fullPath);
            }
        }
        return loadedData;
    }

    // SaveData Save and Load
    public void Save(SaveData _data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(_data, true);

            // optionally decrypt data
            if (useEncryption)
                dataToStore = EncryptDecrypt(dataToStore);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch
        {
            Debug.LogError("Could not access file " + dataFileName + " at location " + fullPath);
        }
    }
}
