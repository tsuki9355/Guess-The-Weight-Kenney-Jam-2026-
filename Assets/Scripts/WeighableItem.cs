using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WeighableItem : MonoBehaviour
{
    [Tooltip("Shown in the hover tooltip for shelf items.")]
    public string displayName = "Item";

    [Tooltip("Weight in grams. Shown to the player for shelf items; kept hidden for the mystery item.")]
    public float weightGrams = 100f;

    [Tooltip("Converts weightGrams into physical Rigidbody mass so the beam tilts realistically. Tune this if the scale feels too stiff or too twitchy.")]
    public float gramsToMass = 0.005f;

    // Which pan (if any) this item is currently reserved on. Null = not placed.
    [HideInInspector] public ScalePan currentPan;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        SyncMass();
    }

    public void SetWeight(float grams)
    {
        weightGrams = grams;
        SyncMass();
    }

    void SyncMass()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        rb.mass = Mathf.Max(0.05f, weightGrams * gramsToMass);
    }
}
