using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlantSO : SO
{
    [SerializeField] private int stage;
    [SerializeField] private float growthRate;
    [SerializeField] private Sprite sprite;
    [SerializeField] private PlantSO nextPlant;

    public PlantSO GrowPlant()
    {
        return nextPlant;
    }

    public int Stage { get { return stage; } }
    public float GrowthRate { get { return growthRate; } }
    public Sprite Sprite { get { return sprite; } }
    public PlantSO NextPlant { get { return nextPlant; } }
}
