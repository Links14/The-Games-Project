using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlantController : MonoBehaviour, Interactable
{
    // (0 is nothing), (1 is dead plant), (2 is sapling), (3 is young plant), (4 is mature plant)
    [SerializeField] PlantSO plant;
    [SerializeField] private Sprite[] soils = new Sprite[2]; // 0 is under 50% water, 1 is over 50% water
    [SerializeField] private Sprite activeSoil;
    [SerializeField] private GameObject plantSprite;
    [SerializeField] private float interval = 5f;
    [SerializeField] private float growth = 0f;
    [SerializeField] private float water = 60f;
    [SerializeField] private int stage = 0;

    private void Start()
    {
        if (plant != null)
        {
            StartGrowth();
            stage = 2;
            plantSprite.GetComponent<SpriteRenderer>().sprite = plant.Sprites[stage - 1];
        }
    }

    private void CheckGrowth()
    {
        growth += plant.GrowthRates[stage - 1];
        water -= 0.5f;
        Debug.Log($"Growth: {growth}\tWater: {water}");

        if (stage > 1)
        {
            if (growth >= 20) { 
                PlantInit(stage + 1);
            }
        }
        if (water <= 0)
            PlantInit(1, 0f); // kill plant
        else if (water > 50) 
            activeSoil = soils[1];
        else 
            activeSoil = soils[0];
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
        StopGrowth();
        stage = _stage;
        growth = 0f;
        water = _water;
        plant = _plant;

        switch (_stage)
        {
            case 0:
                plant = null;
                plantSprite = null; // remove plant sprite
                if (water > 50) activeSoil = soils[1];
                else activeSoil = soils[0];

                break;
            case 1:
                plantSprite.GetComponent<SpriteRenderer>().sprite = plant.Sprites[0]; // set sprite to dead plant
                break;
            case int n when (n < plant.Sprites.Length):
                StartGrowth();
                plantSprite.GetComponent<SpriteRenderer>().sprite = plant.Sprites[stage - 1];
                break;
            case int n when (n == plant.Sprites.Length):
                plantSprite.GetComponent<SpriteRenderer>().sprite = plant.Sprites[stage - 1];
                break;
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
            case 0:
                // prompt the player to plant one of the available seed options
                Debug.Log("Plant a Seed!");
                break;
            case 1:
                // clears dead plant
                Debug.Log("Cleared Dead Plant");
                break;
            case int n when (n < plant.Sprites.Length):
                Debug.Log("This plant still needs to grow!");
                break;
            case int n when (n == plant.Sprites.Length):
                // Harvest plant, add to inventory
                Debug.Log("Harvested Plant!");
                PlantInit(0, water); // reset the plot
                break;
        }

        // if holding watering can
        // add to water pool, play animation and sound
        if (water + 20f >= 100f)
            water = 100f;
        else
            water += 20f;
    }
}
