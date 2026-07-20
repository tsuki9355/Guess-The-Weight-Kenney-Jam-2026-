using UnityEngine;

public class ScalePan : MonoBehaviour
{
    public Transform[] itemSlots;
    public bool isLeftSide = false;

    WeighableItem[] occupants;

    public int MaxItems => itemSlots.Length;

    void Awake()
    {
        occupants = new WeighableItem[itemSlots.Length];
    }

    public Transform Reserve(WeighableItem item)
    {
        for (int i = 0; i < occupants.Length; i++)
        {
            if (occupants[i] == null)
            {
                occupants[i] = item;
                item.currentPan = this;
                return itemSlots[i];
            }
        }

        return null;
    }

    public void Release(WeighableItem item)
    {
        for (int i = 0; i < occupants.Length; i++)
        {
            if (occupants[i] == item)
            {
                occupants[i] = null;
                item.currentPan = null;
                return;
            }
        }
    }

    public void ClearPan()
    {
        for (int i = 0; i < occupants.Length; i++)
        {
            if (occupants[i] != null)
                Destroy(occupants[i].gameObject);
            occupants[i] = null;
        }
    }
}
