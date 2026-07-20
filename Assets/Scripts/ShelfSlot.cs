using UnityEngine;

public class ShelfSlot : MonoBehaviour
{
    ShelfItemOption option;
    GameObject current;

    public void Initialize(ShelfItemOption item)
    {
        Clear();
        option = item;
        SpawnItem();
    }

    public void Clear()
    {
        if (current != null)
            Destroy(current);

        current = null;
        option = null;
    }

    void SpawnItem()
    {
        current = Instantiate(option.prefab, transform.position, option.prefab.transform.rotation);

        var item = current.GetComponent<WeighableItem>();
        item.displayName = option.displayName;
        item.SetWeight(option.weightGrams);

        var drag = current.GetComponent<DraggableItem>();
        if (drag != null)
            drag.OnPickedUp += SpawnItem;
    }
}
