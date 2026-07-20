using UnityEngine;
using UnityEngine.InputSystem;

public class HoverRevealTarget : MonoBehaviour
{
    public RectTransform hoverZone;
    public CanvasGroup target;
    public float fadeSpeed = 8f;

    void Reset()
    {
        hoverZone = GetComponent<RectTransform>();
    }

    void Update()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        bool isOver = RectTransformUtility.RectangleContainsScreenPoint(hoverZone, mousePos, null);

        float targetAlpha = isOver ? 1f : 0f;
        target.alpha = Mathf.MoveTowards(target.alpha, targetAlpha, fadeSpeed * Time.deltaTime);
        target.interactable = isOver;
        target.blocksRaycasts = isOver;
    }
}
