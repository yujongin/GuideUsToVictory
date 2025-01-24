using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHoverEventHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public UnitSelectManager manager;
    public Sprite defaultImage;
    public Sprite selectImage;
    public Sprite descriptionImage;
    bool isSelected;
    public bool IsSelected
    {
        get { return isSelected; }
        set
        {
            if (value == false)
            {
                Deselected();
            }
            isSelected = value;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isSelected)
            transform.DOScale(Vector3.one * 1.1f, 0.5f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isSelected)
            transform.DOScale(Vector3.one, 0.5f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Selected();
    }

    public void Selected()
    {
        GetComponent<Image>().sprite = selectImage;
        IsSelected = true;
        manager.SelectChange(this);
        manager.ShowDescription(descriptionImage);
    }
    void Deselected()
    {
        GetComponent<Image>().sprite = defaultImage;
        transform.DOScale(Vector3.one, 0.5f);
    }
}
