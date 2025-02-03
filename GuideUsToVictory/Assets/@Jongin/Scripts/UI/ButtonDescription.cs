using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string btnName;
    public string description;
    public UnitData unitData;


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!GetComponent<Button>().interactable) return;
        Managers.UI.SetBtnDescription(btnName, description, unitData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Managers.UI.CloseBtnDescription();
    }
}
