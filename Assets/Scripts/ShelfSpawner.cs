using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class ShelfItemOption
{
    public string displayName = "Item";
    public GameObject prefab;

    [Tooltip("Used as-is when this item appears as a known shelf item (the 'average' weight players see on the legend).")]
    public float weightGrams = 100f;

    [Tooltip("When this item is used as the MYSTERY, its actual weight is randomized to an integer within +/- this many grams of Weight Grams. 0 = no randomization, always exact.")]
    public float mysteryVariance = 0f;
}

public class ShelfSpawner : MonoBehaviour
{
    [Header("Physical shelf positions — needs at least as many as your biggest level's shelf item count")]
    public ShelfSlot[] shelfSlots;

    [Header("Where the first/hidden mystery item copy spawns (on the left pan)")]
    public Transform mysterySpawnPoint;

    [Header("Left pan — extra mystery copies (beyond the first) use its known-item slots")]
    public ScalePan leftPan;

    [Header("Optional: lists every known item + weight for the player")]
    public TMP_Text legendText;

    GameObject currentMystery;

    public float LoadLevel(LevelData level)
    {
        if (level.shelfItems.Count > shelfSlots.Length)
            Debug.LogWarning($"[ShelfSpawner] Level '{level.name}' needs {level.shelfItems.Count} shelf slots but only {shelfSlots.Length} exist. Add more Shelf Slots.");

        for (int i = 0; i < shelfSlots.Length; i++)
        {
            if (i < level.shelfItems.Count)
                shelfSlots[i].Initialize(level.shelfItems[i]);
            else
                shelfSlots[i].Clear();
        }

        BuildLegend(level.shelfItems);

        int maxCopies = 1 + leftPan.MaxItems;
        int count = Mathf.Clamp(level.mysteryCount, 1, maxCopies);
        if (level.mysteryCount > maxCopies)
            Debug.LogWarning($"[ShelfSpawner] Level '{level.name}' asks for {level.mysteryCount} mystery copies but the left pan only has room for {maxCopies}. Clamped to {maxCopies}.");

        return SpawnMysteryItems(level.mysteryItem, count);
    }

    void BuildLegend(List<ShelfItemOption> shelfItems)
    {
        if (legendText == null)
            return;

        var lines = new List<string>();
        foreach (var opt in shelfItems)
            lines.Add($"{opt.displayName}: {opt.weightGrams:F0}g");
        legendText.text = string.Join("\n", lines);
    }

    float RandomizeMysteryWeight(ShelfItemOption option)
    {
        if (option.mysteryVariance <= 0f)
            return option.weightGrams;

        int baseWeight = Mathf.RoundToInt(option.weightGrams);
        int variance = Mathf.RoundToInt(option.mysteryVariance);
        return Random.Range(baseWeight - variance, baseWeight + variance + 1);
    }

    float SpawnMysteryItems(ShelfItemOption mysteryOption, int count)
    {
        if (currentMystery != null)
            Destroy(currentMystery);

        float actualWeight = RandomizeMysteryWeight(mysteryOption);
        currentMystery = SpawnOneMysteryItem(mysteryOption, actualWeight, mysterySpawnPoint.position);

        int extras = count - 1;
        for (int i = 0; i < extras; i++)
        {
            var go = SpawnOneMysteryItem(mysteryOption, actualWeight, mysterySpawnPoint.position);
            var item = go.GetComponent<WeighableItem>();
            var slot = leftPan.Reserve(item);

            if (slot == null)
            {
                Debug.LogWarning($"[ShelfSpawner] No free left-pan slot for extra mystery copy #{i + 2}.");
                Destroy(go);
                continue;
            }

            go.transform.position = slot.position;
        }

        return actualWeight;
    }

    GameObject SpawnOneMysteryItem(ShelfItemOption option, float actualWeight, Vector3 position)
    {
        var go = Instantiate(option.prefab, position, option.prefab.transform.rotation);

        var item = go.GetComponent<WeighableItem>();
        item.displayName = option.displayName;
        item.SetWeight(actualWeight);

        var drag = go.GetComponent<DraggableItem>();
        if (drag != null)
            Destroy(drag);

        return go;
    }
}
