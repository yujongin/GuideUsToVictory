using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;
using TMPro;
public class UIManager : MonoBehaviour
{
    Stack<GameObject> panels = new Stack<GameObject>();
    public Button closeBtn;
    public GameObject menuPanel;
    public GameObject unitControllPanel;
    public List<Button> unitBtns = new List<Button>();

    public UnitData[] units;
    TeamData myTeamData;
    public void Init()
    {
        panels.Push(menuPanel);
        closeBtn.onClick.AddListener(ClosePanel);
        myTeamData = Managers.Game.myTeam;
        SetUnitControllBtns();

        for (int i = 0; i < unitBtns.Count; i++)
        {
            unitBtns[i].GetComponent<ButtonEventHandler>().OnLeftClick -= OnUnitIncrease;
            unitBtns[i].GetComponent<ButtonEventHandler>().OnRightClick -= OnUnitDecrease;
            unitBtns[i].GetComponent<ButtonEventHandler>().OnLeftClick += OnUnitIncrease;
            unitBtns[i].GetComponent<ButtonEventHandler>().OnRightClick += OnUnitDecrease;
        }
    }
    private void SetUnitControllBtns()
    {
        ERace race = myTeamData.Race;
        ETeam team = myTeamData.Team;

        units = Managers.Resource.GetUnitsByRace(team, race);
        for(int i = 0; i< units.Length; i++)
        {
            if (units[i].UnitLockedIcon != null)
            {
                unitBtns[i].image.sprite = units[i].UnitLockedIcon;
                continue;
            }

            unitBtns[i].image.sprite = team == ETeam.Blue ? units[i].BlueUnitIcon : units[i].RedUnitIcon;
        }
    }
    public void OnUnitIncrease(string unitName)
    {
        for (int i = 0; i < units.Length; i++)
        {
            if (units[i].name == unitName)
            {
                Managers.Game.AddUnit(Managers.Game.myTeam.Team, units[i]);
                TMP_Text unitCountTmp = unitBtns[i].GetComponentInChildren<TMP_Text>();
                unitCountTmp.text = myTeamData.UnitCountDict[units[i].Name].ToString();
            }
        }
    }
    public void OnUnitDecrease(string unitName)
    {
        for (int i = 0; i < units.Length; i++)
        {
            if (units[i].name == unitName)
            {
                Managers.Game.RemoveUnit(Managers.Game.myTeam.Team, units[i]);
                TMP_Text unitCountTmp = unitBtns[i].GetComponentInChildren<TMP_Text>();
                unitCountTmp.text = myTeamData.UnitCountDict[units[i].Name].ToString();
            }
        }
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
