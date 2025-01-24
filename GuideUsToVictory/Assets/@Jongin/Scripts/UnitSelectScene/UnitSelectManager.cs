using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitSelectManager : MonoBehaviour
{
    public GameObject descriptionObject;
    public ButtonHoverEventHandler[] buttons;

    public Vector3 descriptionStartPos;
    private void Start()
    {
        descriptionStartPos = descriptionObject.transform.position;
        buttons[0].Selected();
    }

    public void SelectChange(ButtonHoverEventHandler handler)
    {
        foreach (var button in buttons)
        {
            if(button != handler)
            {
                button.IsSelected = false;
            }
        }
    }

    public void ShowDescription(Sprite description)
    {
        descriptionObject.transform.position = descriptionStartPos;
        descriptionObject.transform.DOMoveX(descriptionObject.transform.position.x + 310, 1f);
        Image image = descriptionObject.GetComponent<Image>();
        image.sprite = description;
        Color color = image.color;
        color.a = 0;
        image.color = color;

        image.DOFade(1, 1);
    }

}
