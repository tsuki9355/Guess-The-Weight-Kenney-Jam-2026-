using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Guess The Weight/Level")]
public class LevelData : ScriptableObject
{
    [Header("Every item shown on the shelf for this level")]
    public List<ShelfItemOption> shelfItems;

    [Header("This level's mystery item — its exact weight, no randomization")]
    public ShelfItemOption mysteryItem;

    [Header("How many copies of the mystery item are on the left pan (player still guesses the weight of ONE)")]
    [Min(1)]
    public int mysteryCount = 1;

    [Header("Optional instruction popup shown when this level starts")]
    public bool showInstruction = false;

    [TextArea(3, 8)]
    public string instructionText = "";
}
