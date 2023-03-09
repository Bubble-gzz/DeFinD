using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;

public class RulePanel : MonoBehaviour
{
    // Start is called before the first frame update
    PuzzleInfo puzzleInfo;
    public GameObject RulePrefab;
    public Sprite toSymbol, equalSymbol, orSymbol;
    CardOperator cardOperator;
    public GameObject replaceOptionPrefab;
    public ItemLayout optionLayout;
    void Awake()
    {
        Global.rulePanel = this;
    }
    void Start()
    {
        puzzleInfo = Global.puzzleInfo;
        ParseRules();
        cardOperator = Global.cardOperator;
        ReplaceOptions = cardOperator.ReplaceOptions;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ParseRules()
    {
        /* Create UI */
        foreach(var rule in puzzleInfo.rules)
        {
            GameObject newRule = Instantiate(RulePrefab, transform);
            foreach(char ch in rule)
            {
                GameObject newItem = new GameObject();
                newItem.AddComponent<Image>();
                newItem.GetComponent<Image>().sprite = GetSprite(ch);
                newItem.GetComponent<RectTransform>().sizeDelta = new Vector2(30, 30);
                newItem.transform.SetParent(newRule.transform);
            }
        }

        /* Create Rules */
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
                    newRule.directed = (ch == '>');
                }
            }
        }
    }
    Sprite GetSprite(char ch)
    {
        if (ch == '>') return toSymbol;
        if (ch == '=') return equalSymbol;
        if (ch == '|') return orSymbol;
        return puzzleInfo.GetSprite(ch);
    }
    bool isSpecialCharacter(char ch)
    {
        return ch == '=' || ch == '>' || ch == '|';
    }

    public void DetectRulePatterns()
    {
        foreach(var rule in rules)
        {
            rule.leftMatch = rule.rightMatch = false;
            foreach(var pattern in rule.leftSide)
                rule.leftMatch |= PatternMatch(cardOperator.SelectedS(), pattern);

            if (rule.directed) continue;
            
            foreach(var pattern in rule.rightSide)
                rule.rightMatch |= PatternMatch(cardOperator.SelectedS(), pattern);
        }
        UpdateReplaceOptions();
    }
    List<char> commands = new List<char>{
        'A','B','C','D','E','F','G','H','I','J','K'
    };
    void UpdateReplaceOptions()
    {   
        optionLayout.ClearItems();
        foreach(var option in ReplaceOptions) option.Leave();
        ReplaceOptions.Clear();
        foreach(var rule in rules)
        {
            if (rule.leftMatch)
                foreach(var s in rule.rightSide)
                {
                    ReplaceOption newOption = Instantiate(replaceOptionPrefab).GetComponentInChildren<ReplaceOption>();
                    newOption.rootObject.transform.position = new Vector3(0, 15, 0);
                    ReplaceOptions.Add(newOption);
                    newOption.cardS = s;
                    optionLayout.AppendItem(newOption.rootObject.transform);
                }
            if (rule.rightMatch)
                foreach(var s in rule.leftSide)
                {
                    ReplaceOption newOption = Instantiate(replaceOptionPrefab).GetComponentInChildren<ReplaceOption>();
                    newOption.rootObject.transform.position = new Vector3(0, 10, 0);
                    ReplaceOptions.Add(newOption);
                    newOption.cardS = s;
                    optionLayout.AppendItem(newOption.rootObject.transform);
                }
        }
        if (ReplaceOptions.Count > 0)
        {
            int i = 0;
            foreach(var option in ReplaceOptions)
            {
                option.maxWidth = optionLayout.maxWidth / (ReplaceOptions.Count + 1) * 0.4f;
                if (ReplaceOptions.Count > 1) option.text.text = commands[i].ToString();
                else option.text.text = "Enter";
                i++;
            }
        }
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
    List<ReplaceOption> ReplaceOptions;
}
