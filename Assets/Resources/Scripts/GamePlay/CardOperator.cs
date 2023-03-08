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
    void UserInputs()
    {

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
        items[pos].GetComponent<GameCard>().SelfDestroy();
        DelItem(pos);
        cardS.Remove(pos, 1);
    }
    public string SelectedS()
    {
        int seg = 0;
        bool flag = false;
        StringBuilder res = new StringBuilder();
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].GetComponent<GameCard>().picked)
            {
                if (!flag) seg++;
                flag = true;
                res.Append(cardS[i]);
            }
            else flag = false;
        }
        //Debug.Log("SelectedS:" + res.ToString());
        if (seg == 1) return res.ToString();
        return null; 
    }
    public List<ReplaceOption> ReplaceOptions;
}