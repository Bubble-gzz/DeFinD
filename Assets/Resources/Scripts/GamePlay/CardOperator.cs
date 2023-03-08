using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
public class CardOperator : CardManager
{
    PuzzleInfo puzzleInfo;
    StringBuilder cardS;
    override protected void Awake()
    {
        base.Awake();
        cardS = new StringBuilder();
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
    }
    void CreateCards(string s)
    {
        foreach(char ch in s)
        {
            GameCard newCard = (GameCard)AddCard(cards.Count);
            newCard.ch = ch;
            newCard.cover.sprite = puzzleInfo.GetSprite(ch);
            cardS.Append(ch);
        }
    }

    /*
    int CountPattern(string pattern)
    {
        int L = pattern.Length, res = 0;
        for (int i = 0; i + L - 1 < cards.Count; i++)
        {
            if ( PatternMatch(cardS.ToString(i, L), pattern) ) res++;
        }
        return res;
    }
    */

    public string SelectedS()
    {
        int seg = 0;
        bool flag = false;
        StringBuilder res = new StringBuilder();
        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i].picked)
            {
                if (!flag) seg++;
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