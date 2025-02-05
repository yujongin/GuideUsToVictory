using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Define;
public class UIManager : MonoBehaviour
{
    Stack<GameObject> panels = new Stack<GameObject>();
    public Button closeBtn;
    public GameObject menuPanel;
    public GameObject unitControllPanel;
    public List<Button> unitBtns = new List<Button>();

    public GameObject btnDescription;
    TMP_Text name_Text;
    TMP_Text description_Text;
    GameObject statPanel;
    TMP_Text ad_Text;
    TMP_Text ap_Text;
    TMP_Text ar_Text;
    TMP_Text mr_Text;
    TMP_Text sp_Text;
    TMP_Text hp_Text;
    TMP_Text as_Text;
    TMP_Text ca_Text;
    TMP_Text fa_Text;

    public UnitData[] units;
    TeamData myTeamData;
    public void Init()
    {
        panels.Push(menuPanel);
        closeBtn.onClick.AddListener(ClosePanel);
        myTeamData = Managers.Game.myTeamData;
        SetUnitControllBtns();

        for (int i = 0; i < unitBtns.Count; i++)
        {
            unitBtns[i].GetComponent<ButtonEventHandler>().OnLeftClick -= OnUnitIncrease;
            unitBtns[i].GetComponent<ButtonEventHandler>().OnRightClick -= OnUnitDecrease;
            unitBtns[i].GetComponent<ButtonEventHandler>().OnLeftClick += OnUnitIncrease;
            unitBtns[i].GetComponent<ButtonEventHandler>().OnRightClick += OnUnitDecrease;
        }

        name_Text = btnDescription.transform.Find("name").GetComponent<TMP_Text>();
        description_Text = btnDescription.transform.Find("description").GetComponent<TMP_Text>();
        statPanel = btnDescription.transform.Find("StatPanel").gameObject;
        ad_Text = statPanel.transform.Find("AD").GetComponentInChildren<TMP_Text>();
        ap_Text = statPanel.transform.Find("AP").GetComponentInChildren<TMP_Text>();
        ar_Text = statPanel.transform.Find("AR").GetComponentInChildren<TMP_Text>();
        mr_Text = statPanel.transform.Find("MR").GetComponentInChildren<TMP_Text>();
        hp_Text = statPanel.transform.Find("HP").GetComponentInChildren<TMP_Text>();
        sp_Text = statPanel.transform.Find("SP").GetComponentInChildren<TMP_Text>();
        as_Text = statPanel.transform.Find("AS").GetComponentInChildren<TMP_Text>();
        ca_Text = statPanel.transform.Find("Capacity").GetComponentInChildren<TMP_Text>();
        fa_Text = statPanel.transform.Find("Faith").GetComponentInChildren<TMP_Text>();
    }
    private void SetUnitControllBtns()
    {
        ERace race = myTeamData.Race;
        ETeam team = myTeamData.Team;

        units = Managers.Resource.GetUnitsByRace(team, race);
        for (int i = 0; i < units.Length; i++)
        {
            unitBtns[i].GetComponent<ButtonDescription>().btnName = units[i].Name;
            unitBtns[i].GetComponent<ButtonDescription>().description = units[i].Description;
            unitBtns[i].GetComponent<ButtonDescription>().unitData = units[i];

            if (units[i].UnitLockedIcon != null)
            {
                unitBtns[i].image.sprite = units[i].UnitLockedIcon;
                continue;
            }

            unitBtns[i].image.sprite = team == ETeam.Blue ? units[i].BlueUnitIcon : units[i].RedUnitIcon;
        }
    }

    public void SetUnitUnlock()
    {
        for (int i = 0; i < myTeamData.UnitUnlock.Length; i++)
        {
            if (myTeamData.UnitUnlock[i])
            {
                unitBtns[i].image.sprite = myTeamData.Team == ETeam.Blue ? units[i].BlueUnitIcon : units[i].RedUnitIcon;
                unitBtns[i].interactable = true;
            }
        }
    }
    public void SetBtnDescription(string btnName, string description, UnitData unitData = null)
    {
        if (unitData != null)
        {
            ad_Text.text = unitData.AttackDamage.ToString();
            ap_Text.text = unitData.AbilityPower.ToString();
            ar_Text.text = unitData.Armor.ToString();
            mr_Text.text = unitData.MagicRegistance.ToString();
            hp_Text.text = unitData.Hp.ToString();
            sp_Text.text = unitData.Speed.ToString();
            as_Text.text = unitData.AttackSpeed.ToString();
            ca_Text.text = unitData.Capacity.ToString();
            fa_Text.text = unitData.PriceFaith.ToString();
            statPanel.SetActive(true);
        }
        name_Text.text = btnName;
        description_Text.text = description;
        btnDescription.SetActive(true);
    }
    public void CloseBtnDescription()
    {
        statPanel.SetActive(false);
        btnDescription.SetActive(false);
    }
    public void OnUnitIncrease(string unitName)
    {
        for (int i = 0; i < units.Length; i++)
        {
            if (units[i].name == unitName)
            {
                Managers.Game.AddUnit(Managers.Game.myTeamData.Team, units[i]);
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
                Managers.Game.RemoveUnit(Managers.Game.myTeamData.Team, units[i]);
                TMP_Text unitCountTmp = unitBtns[i].GetComponentInChildren<TMP_Text>();
                unitCountTmp.text = myTeamData.UnitCountDict[units[i].Name].ToString();
            }
        }
    }
    private void Update()
    {
        if (panels.Count == 1)
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
