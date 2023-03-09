using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
public class CardOperator : ItemSelector
{
    public GameObject cardPrefab;
    PuzzleInfo puzzleInfo;
    StringBuilder cardS;
    StringBuilder sb;
    override protected void Awake()
    {
        base.Awake();
        cardS = new StringBuilder();
        sb = new StringBuilder();
        Global.cardOperator = this;
        ReplaceOptions = new List<ReplaceOption>();
    }
    override protected void Start()
    {
        base.Start();
        puzzleInfo = Global.puzzleInfo;
        CreateCards(puzzleInfo.Init);
    }

    override protected void Update()
    {
        base.Update();
        UserInputs();
    }
    List<KeyCode> commands = new List<KeyCode>(){
        KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D,
        KeyCode.E, KeyCode.F, KeyCode.G, KeyCode.H,
    };
    void UserInputs()
    {
        for (int i = 0; i < ReplaceOptions.Count; i++)
            if (Input.GetKeyDown(commands[i]) || (i == 0 && Input.GetKeyDown(KeyCode.Return)))
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