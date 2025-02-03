using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonEventHandler : MonoBehaviour, IPointerClickHandler
{
    public Action<string> OnLeftClick;
    public Action<string> OnRightClick;
    int myIndex;
    void Start()
    {
        myIndex = Managers.UI.unitBtns.IndexOf(GetComponent<Button>());
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!GetComponent<Button>().interactable) return;
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick.Invoke(Managers.UI.units[myIndex].Name);
        }
        else if(eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick.Invoke(Managers.UI.units[myIndex].Name);
        }
    }
}
