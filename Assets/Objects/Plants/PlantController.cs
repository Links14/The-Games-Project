using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantController : MonoBehaviour, Interactable
{
    [SerializeField] PlantSO plant;
    private float growth = 0;
    private float water;

    private void FixedUpdate()
    {
        growth += Time.fixedDeltaTime * plant.GrowthRate;
        Debug.Log($"Growth: {growth:.2%}");

        if (growth >= 100)
        {
            plant = plant.GrowPlant();
        }
    }

    public void Interact()
    {
        if (plant.Stage == 4)
        {
            // add to player inventory
        }
        else
        {
            Debug.Log("This plant still needs to grow!");
        }

            throw new System.NotImplementedException();
    }
}
