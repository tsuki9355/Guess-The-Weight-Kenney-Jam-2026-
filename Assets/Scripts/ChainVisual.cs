using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ChainVisual : MonoBehaviour
{
    public Transform anchorPoint;
    public Transform pan;

    LineRenderer line;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
    }

    void LateUpdate()
    {
        if (anchorPoint == null || pan == null)
            return;

        line.SetPosition(0, anchorPoint.position);
        line.SetPosition(1, pan.position);
    }
}
