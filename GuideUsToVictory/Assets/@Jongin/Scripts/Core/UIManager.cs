using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    Stack<GameObject> panels;
    public Button closeBtn;
    public List<Button> unitAddBtns = new List<Button>();
    private void Start()
    {
        
    }
    public void OnUnitAdd(string unitName)
    {


    }
    private void Update()
    {
        if(panels.Count == 1)
        {
            closeBtn.gameObject.SetActive(false);
        }
        else
        {
            closeBtn.gameObject.SetActive(true);
        }
    }
    public void OpenPanel(GameObject panel)
    {
        panels.Peek().SetActive(false);
        panel.SetActive(true);
        panels.Push(panel);
    }
    public void ClosePanel()
    {
        panels.Pop().SetActive(false);
        panels.Peek().SetActive(true);
    }
}
