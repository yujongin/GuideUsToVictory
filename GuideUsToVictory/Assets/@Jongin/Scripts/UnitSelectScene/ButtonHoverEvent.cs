using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverEvent : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    public GameObject hoverEffect;

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverEffect.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverEffect.SetActive(false);
    }


}
