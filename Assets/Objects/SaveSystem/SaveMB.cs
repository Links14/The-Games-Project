using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using UnityEngine.SceneManagement;

public class SaveMB : MonoBehaviour
{
    private SaveData data;
    private List<IData> iDataObjs;
    private SaveManager saveManager;

    [Header("Configs")]
    [SerializeField] private bool useEncryption;
    [SerializeField] private Dictionary<string, PlantSO> plants = new Dictionary<string, PlantSO>();
    public static SaveMB Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
            Debug.LogError("More Than one SaveMB instance found...");
        }
        Instance = this;

        PlantSO[] plantArr = Resources.LoadAll<PlantSO>("Plants");

        foreach (PlantSO plant in plantArr)
        {
            Debug.Log($"Adding: {plant.ObjectName}");

            if (!plants.ContainsKey(plant.ObjectID))
            {
                plants.Add(plant.ObjectID, plant);
            } 
            else
            {
                Debug.LogWarning($"Duplicate Plant ID detected: {plant.ObjectID}");
            }
        }

    }

    private void Start()
    {
        // Debug.Log(Application.persistentDataPath);
        saveManager = new("Player/game.data", false);
        this.iDataObjs = FindAllIDataObjects();
        LoadGame();
    }

    public void NewGame()
    {
        data = new SaveData();
    }

    public void LoadGame()
    {
        this.data = saveManager.Load();
        if (this.data == null)
        {
            Debug.Log("No data was found, creating a new game");
            NewGame();
        }

        foreach (IData _info in iDataObjs)
        {
            _info.LoadData(this.data);
        }
    }

    public void SaveGame()
    {
        foreach (IData _info in iDataObjs)
        {
            _info.SaveData(ref this.data);
        }
        saveManager.Save(this.data);
    }

    private List<IData> FindAllIDataObjects()
    {
        IEnumerable<IData> list = FindObjectsOfType<MonoBehaviour>().OfType<IData>();

        return new List<IData>(list);
    }




    public PlantSO GetPlantByID(string _plantID)
    {
        if (plants.TryGetValue(_plantID, out PlantSO plant))
        {
            return plant;
        }
        Debug.LogWarning($"Plant with ID '{_plantID}' not found!");
        return null;
    }
}
