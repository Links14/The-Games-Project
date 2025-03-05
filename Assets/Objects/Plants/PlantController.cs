using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlantController : MonoBehaviour, Interactable, IData
{
    // (-1 is nothing), (0 is dead plant), (1 is sapling), (2 is young plant), (3 is mature plant)
    [SerializeField] PlantSO plant;
    [SerializeField] private Sprite[] soils = new Sprite[2]; // 0 is under 50% water, 1 is over 50% water
    [SerializeField] private GameObject plantSprite;
    [SerializeField] private GameObject soilSprite;
    [SerializeField] private float interval = 5f;
    [SerializeField] private float growth = 0f;
    [SerializeField] private float water = 60f;
    [SerializeField] private int stage = -1;

    private void Start()
    {
        if (plant != null)
        {
            StartGrowth();
            stage = 1;
            plantSprite.GetComponent<SpriteRenderer>().sprite = plant.Sprites[stage];
            soilSprite.GetComponent<SpriteRenderer>().sprite = soils[1];
        }
    }

    private void CheckGrowth()
    {
        water -= 0.5f;
        Debug.Log($"Growth: {growth}\tWater: {water}");

        if (plant != null)
        {
            if (stage != 0)
                stage = Mathf.Min((int)(growth / 20f) + 1, plant.Sprites.Length - 1);

            if (stage == -1)
            {
                plant = null;
                plantSprite.GetComponent<SpriteRenderer>().sprite = null; // remove plant sprite
            }
            else
            {
                plantSprite.GetComponent<SpriteRenderer>().sprite = plant.Sprites[stage];
                if (stage > 0 && stage < (plant.Sprites.Length - 1))
                {
                    if (growth < 20f * (plant.Sprites.Length - 1))
                        growth += plant.GrowthRates[stage - 1];
                    else
                        growth = 20f * (plant.Sprites.Length - 1);
                }
            }
        }

        if (water <= 0)
            if (stage > 0)
                PlantInit(0, 0f); // kill plant
        else if (water > 50)
            soilSprite.GetComponent<SpriteRenderer>().sprite = soils[1];
        else
            soilSprite.GetComponent<SpriteRenderer>().sprite = soils[0];
    }

    private void PlantInit(int _stage)
    {
        PlantInit(_stage, water);
    }

    private void PlantInit(int _stage, float _water)
    {
        PlantInit(_stage, _water, plant);
    }

    private void PlantInit(int _stage, float _water, PlantSO _plant)
    {
        PlantInit(_stage, _water, _plant, growth);
    }

    private void PlantInit(int _stage, float _water, PlantSO _plant, float _growth)
    {
        // StopGrowth();
        stage = _stage;
        growth = _growth;
        water = _water;
        plant = _plant;

        if (water > 50)
            soilSprite.GetComponent<SpriteRenderer>().sprite = soils[1];
        else
            soilSprite.GetComponent<SpriteRenderer>().sprite = soils[0];

        if (stage == -1)
        {
            plant = null;
            plantSprite.GetComponent<SpriteRenderer>().sprite = null; // remove plant sprite
        }
        else
        {
            plantSprite.GetComponent<SpriteRenderer>().sprite = plant.Sprites[stage];
            if (stage > 0 && stage < plant.Sprites.Length - 1)
            {
                // StartGrowth();
            }
        }
    }

    private void StartGrowth()
    {
        InvokeRepeating(nameof(CheckGrowth), 5f, interval);
    }

    private void StopGrowth()
    {
        CancelInvoke(nameof(CheckGrowth));
    }

    public void Interact()
    {
        switch (stage)
        {
            default:
                Debug.Log("Oops! Something seems to have gone wrong...");
                break;
            case -1:
                // prompt the player to plant one of the available seed options
                Debug.Log("Plant a Seed!");
                break;
            case 0:
                // clears dead plant
                Debug.Log("Cleared Dead Plant");
                break;
            case int n when (n < plant.Sprites.Length - 1):
                Debug.Log("This plant still needs to grow!");
                break;
            case int n when (n == plant.Sprites.Length - 1):
                // Harvest plant, add to inventory
                Debug.Log("Harvested Plant!");
                PlantInit(-1, water, null, 0f); // reset the plot
                break;
        }

        // if holding watering can
        // add to water pool, play animation and sound
        if (water + 20f >= 100f)
            water = 100f;
        else
            water += 20f;
    }

    public void LoadData(SaveData _data)
    {
        try {
            var pos = _data.plantPos[0];
            this.transform.position = pos.ToVector3();
            _data.plantPos.Remove(pos);

            var a = _data.plantStage[0];
            var b = _data.plantWater[0];
            var c = _data.plantID[0];
            var d = _data.plantGrowth[0];

            PlantInit(a, b, SaveMB.Instance.GetPlantByID(c), d);
            _data.plantStage.Remove(a);
            _data.plantWater.Remove(b);
            _data.plantID.Remove(c);
            _data.plantGrowth.Remove(d); 
        }
        catch
        {
            Debug.LogWarning("Caught error loading data in PlantController Object. " + this.gameObject);
        }
    }

    public void SaveData(ref SaveData _data)
    {
        _data.plantStage.Add(stage);
        _data.plantWater.Add(water);
        _data.plantID.Add(plant.ObjectID);
        _data.plantGrowth.Add(growth);
    }
}
