using UnityEngine;
using UnityEngine.EventSystems;

public class MenuItemButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public MenuNavigator navigator;
    public int index;

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
