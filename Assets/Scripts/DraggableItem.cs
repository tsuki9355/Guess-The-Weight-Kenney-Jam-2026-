using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class DraggableItem : MonoBehaviour
{
    public event System.Action OnPickedUp;
    bool pickedUpFired;

    public void NotifyPickedUp()
    {
        if (pickedUpFired)
            return;

        pickedUpFired = true;
        OnPickedUp?.Invoke();
        OnPickedUp = null;
    }
}
