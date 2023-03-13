using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
public class CardOperator : ItemSelector
{
    public GameObject cardPrefab;
    public GameObject selectIconPrefab;
    public ItemSelector optionMenu;

    PuzzleInfo puzzleInfo;
    StringBuilder cardS;
    StringBuilder sb;
    SmoothObject lastSelectIcon;
    enum OpMode{
        Option,
        Card
    }
    OpMode opMode;
    override protected void Awake()
    {
        base.Awake();
        cardS = new StringBuilder();
        sb = new StringBuilder();
        Global.cardOperator = this;
        ReplaceOptions = new List<ReplaceOption>();
        lastSelectIcon = null;
        selectIcon = Instantiate(selectIconPrefab, transform).GetComponent<SmoothObject>();
        SetActive(true);
        opMode = OpMode.Card;
    }
    override protected void Start()
    {
        base.Start();
        puzzleInfo = Global.puzzleInfo;
        CreateCards(puzzleInfo.Init);
        SelectWithIndex((items.Count - 1)/ 2);
    }

    override protected void Update()
    {
        base.Update();
        UserInputs();
    }
    List<KeyCode> commands = new List<KeyCode>(){
        KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3, KeyCode.Keypad4,
        KeyCode.Keypad5, KeyCode.Keypad6, KeyCode.Keypad7, KeyCode.Keypad8
    };
    List<KeyCode> commands2 = new List<KeyCode>(){
        KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4,
        KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8
    };
    void UserInputs()
    {
        if (opMode == OpMode.Option && ReplaceOptions.Count == 0)
            SwitchMode(OpMode.Card);
        if (Utils.GetKeyDown(KeyCode.UpArrow))
        {
            if (opMode == OpMode.Option) SwitchMode(OpMode.Card);
            else SwitchMode(OpMode.Option);
        }
        for (int i = 0; i < ReplaceOptions.Count; i++)
            if (Utils.GetKeyDown(commands[i]) || Utils.GetKeyDown(commands2[i])
            || (i == 0 && ReplaceOptions.Count == 1 && Utils.GetKeyDown(KeyCode.Return)))
            {
                PickOption(i);
                break;
            }
    }
    void SwitchMode(OpMode newMode)
    {
        if (opMode == newMode) return;
        if (newMode == OpMode.Option && ReplaceOptions.Count < 1) return;
        opMode = newMode;
        if (opMode == OpMode.Option)
        {
            optionMenu.selectIcon = selectIcon;
            selectIcon.transform.SetParent(optionMenu.transform);
            lastSelectIcon = selectIcon;
            selectIcon = null;
            SetActive(false);
            optionMenu.SetActive(true);
        }
        else
        {
            StartCoroutine(SelectIconFlyAway(optionMenu.selectIcon));
            optionMenu.SetActive(false);
            optionMenu.selectIcon = null;
            selectIcon = Instantiate(selectIconPrefab, transform).GetComponent<SmoothObject>();
            selectIcon.transform.localPosition = new Vector3(0, -7, 0);
            SetActive(true);
        }
    }
    IEnumerator SelectIconFlyAway(SmoothObject icon)
    {
        icon.targetPos = new Vector2(0, 7);
        yield return new WaitForSeconds(0.7f);
        Destroy(icon.gameObject);
    }
    public void PickOption(ReplaceOption option)
    {
        for (int i = 0; i < ReplaceOptions.Count; i++)
            if (ReplaceOptions[i] == option)
            {   
                PickOption(i);
                break;
            }
    }
    void PickOption(int index)
    {
        while ((items[firstSelectedPos] as Card).picked)
        {
            DelCard(firstSelectedPos);
            if (firstSelectedPos >= items.Count) break;
        }
        ReplaceOption option = ReplaceOptions[index];
        for (int i = 0; i < ReplaceOptions.Count; i++)
            if (i != index) ReplaceOptions[i].Leave();
            else {
                for (int j = option.items.Count - 1; j >= 0; j--)
                {
                    AddCard(firstSelectedPos, option.items[j] as GameCard);
                }
                ReplaceOptions[i].Leave();
            }
        ReplaceOptions.Clear();
        Global.targetPanel.DetectTargets(cardS.ToString());
    }
    void CreateCards(string s)
    {
        foreach(char ch in s)
        {
            GameCard newCard = CreateNewCard(items.Count, ch);
            newCard.cover.sprite = puzzleInfo.GetSprite(ch);
        }
    }
    GameCard CreateNewCard(int pos, char ch)
    {
        GameCard newCard = Instantiate(cardPrefab, this.transform).GetComponent<GameCard>();
        newCard.ch = ch;
        AddCard(pos, newCard);
        return newCard;
    }
    GameCard AddCard(int pos, GameCard newCard)
    {
        AddItem(pos, newCard.transform);
        cardS.Insert(pos, newCard.ch);
        return newCard;
    }
    void DelCard(int pos)
    {
        items[pos].GetComponent<GameCard>().Leave();
        DelItem(pos);
        cardS.Remove(pos, 1);
    }
    int firstSelectedPos;
    public string SelectedS()
    {
        int seg = 0;
        bool flag = false;
        StringBuilder res = new StringBuilder();
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].GetComponent<GameCard>().picked)
            {
                if (!flag) {
                    seg++;
                    if (seg == 1) firstSelectedPos = i;
                }
                flag = true;
                res.Append(cardS[i]);
            }
            else flag = false;
        }
        if (seg == 1) return res.ToString();
        return null; 
    }
    public List<ReplaceOption> ReplaceOptions;
}