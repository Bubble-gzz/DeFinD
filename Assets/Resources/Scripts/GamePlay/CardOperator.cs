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
    }
    override protected void Start()
    {
        base.Start();
        puzzleInfo = Global.puzzleInfo;
        CreateCards(puzzleInfo.Init);
        ParseRules();
    }

    override protected void Update()
    {
        base.Update();
    }
    public void Refresh()
    {
        DetectRulePatterns();
    }
    void CreateCards(string s)
    {
        foreach(char ch in s)
        {
            GameCard newCard = (GameCard)AddCard(cards.Count);
            newCard.cardOperator = this;
            newCard.ch = ch;
            newCard.cover.sprite = puzzleInfo.GetSprite(ch);
            cardS.Append(ch);
        }
    }
    void ParseRules()
    {
        foreach(string rule in puzzleInfo.rules)
        {
            bool onLeftSide = true;
            StringBuilder newS = new StringBuilder();
            Rule newRule = new Rule();
            rules.Add(newRule);
            for (int i = 0; i < rule.Length; i++)
            {
                char ch = rule[i];
                if (!isSpecialCharacter(ch))
                {
                    newS.Append(ch);
                }
                if (isSpecialCharacter(ch) || i == rule.Length - 1)
                {
                    if (onLeftSide) newRule.leftSide.Add(newS.ToString());
                    else newRule.rightSide.Add(newS.ToString());
                    newS.Clear();
                }
                if (ch == '=' || ch == '>')
                {
                    onLeftSide = false;
                    if (ch == '=') newRule.directed = false;
                    else newRule.directed = true;
                }
            }
        }
    }
    bool isSpecialCharacter(char ch)
    {
        return ch == '=' || ch == '>' || ch == '|';
    }
    int CountPattern(string pattern)
    {
        int L = pattern.Length, res = 0;
        for (int i = 0; i + L - 1 < cards.Count; i++)
        {
            if ( PatternMatch(cardS.ToString(i, L), pattern) ) res++;
        }
        return res;
    }
    bool PatternMatch(string s, string t)
    {
        if (s == null || t == null) return false;
        if (s.Length != t.Length) return false;
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] != t[i]) return false;
        }
        return true;
    }
    void DetectRulePatterns()
    {
        foreach(var rule in rules)
        {
            rule.leftMatch = rule.rightMatch = false;
            foreach(var pattern in rule.leftSide)
                rule.leftMatch |= PatternMatch(SelectedS(), pattern);

            if (rule.directed) continue;
            
            foreach(var pattern in rule.rightSide)
                rule.rightMatch |= PatternMatch(SelectedS(), pattern);
        }
    }
    string SelectedS()
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
    [Serializable]
    public class Rule{
        public List<string> leftSide, rightSide;
        public bool directed, leftMatch, rightMatch;
        public Rule()
        {
            leftSide = new List<string>();
            rightSide = new List<string>();
        }
    }
    public List<Rule> rules;
}
