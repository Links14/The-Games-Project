using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Plant", menuName = "Farming/Plants")]
public class PlantSO : SO
{
    // (1 is dead plant), (2 is sapling), (3 is young plant), (4 is mature plant)
    [SerializeField]
    [TextArea]
    string description;
    [Space]
    // length of these two should be the same
    [SerializeField] private float[] growthRates = { 0f, 1f, 1f, 0f };  // first and last index should be 0f
    [SerializeField] private Sprite[] sprites = new Sprite[4];          // index 0 should be dead plant

    public float[] GrowthRates { get { return growthRates; } }
    public Sprite[] Sprites { get { return sprites; } }
}