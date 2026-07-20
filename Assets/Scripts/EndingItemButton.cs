using UnityEngine;
using UnityEngine.EventSystems;

public class EndingItemButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public EndingScreenNavigator navigator;
    public int index;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnPointerEnter(PointerEventData eventData)
    {
        navigator.Hover(index);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        navigator.ClearHover(index);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        navigator.Activate(index);
    }
}
