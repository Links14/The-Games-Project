using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Plant", menuName = "Farming/Plants")]
public class PlantSO : SO
{
    // (0 is dead plant), (1 is sapling), (2 is young plant), (3 is mature plant)
    [SerializeField]
    [TextArea]
    string description;
    [Space]
    // growth rates is 2 shorter than sprites
    [SerializeField] private float[] growthRates = {1f, 1f};
    [SerializeField] private Sprite[] sprites = new Sprite[4];          // index 0 should be dead plant

    public float[] GrowthRates { get { return growthRates; } }
    public Sprite[] Sprites { get { return sprites; } }
}