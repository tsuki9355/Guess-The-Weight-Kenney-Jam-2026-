using UnityEngine;
using UnityEngine.InputSystem;

public class SidePanelToggle : MonoBehaviour
{
    public CanvasGroup panel;
    public RectTransform panelRect;

    [Range(0f, 1f)]
    public float baseAlpha = 0.35f;

    public float fadeSpeed = 8f;

    void Update()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        bool overPanel = RectTransformUtility.RectangleContainsScreenPoint(panelRect, mousePos, null);

        float targetAlpha = overPanel ? 1f : baseAlpha;

        panel.alpha = Mathf.MoveTowards(panel.alpha, targetAlpha, fadeSpeed * Time.deltaTime);
        panel.interactable = overPanel;
        panel.blocksRaycasts = overPanel;
    }
}
