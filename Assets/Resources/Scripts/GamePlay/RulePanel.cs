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
    public float maxWidth;
    PanelSlider slider;
    GameObject gameCardPrefab;
    List<ItemLayout> pages;
    void Awake()
    {
        Global.rulePanel = this;
        slider = GetComponentInChildren<PanelSlider>();
        if (slider == null) Debug.Log("Rule Panel Slider Missing!");
        slider.leftKey = KeyCode.Q;
        slider.rightKey = KeyCode.E;
        slider.pageWidth = maxWidth;
        pages = new List<ItemLayout>();
    }
    void Start()
    {
        puzzleInfo = Global.puzzleInfo;
        cardOperator = Global.cardOperator;
        ReplaceOptions = cardOperator.ReplaceOptions;
        gameCardPrefab = Global.gameCardPrefab;
        
        ParseRules();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ParseRules()
    {
        
        /* Create Rules */
        int pageCount = 1, pageID;
        foreach(string rule in puzzleInfo.rules)
        {
            bool onLeftSide = true;
            StringBuilder newS = new StringBuilder();
            Rule newRule = new Rule();
            rules.Add(newRule);
            pageID = 0;
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
                if (ch == '@')
                {
                    pageID = Utils.GetInt(rule, ref i);
                    if (pageID + 1 > pageCount) pageCount = pageID + 1;
                    continue;
                }
                if (ch == '=' || ch == '>')
                {
                    onLeftSide = false;
                    newRule.directed = (ch == '>');
                }
            }
        }
        pages.Clear();
        slider.Clear();
        for (int i = 0; i < pageCount; i++)
        {
            GameObject newPage = new GameObject();
            newPage.transform.localScale = new Vector3(1, 1, 1);
            ItemLayout layoutSettings = newPage.AddComponent<ItemLayout>();
            layoutSettings.maxWidth = maxWidth;
            layoutSettings.alignment = ItemLayout.Alignment.Center;
            layoutSettings.nested = true;
            layoutSettings.defaultInterval = 1.6f;
            layoutSettings.minInterval = 1.3f;
            slider.AppendPage(newPage.transform);
            pages.Add(layoutSettings);
        }
        /* Create UI */
        foreach(var rule in puzzleInfo.rules)
        {
            ItemLayout newRule = Instantiate(RulePrefab).GetComponent<ItemLayout>();
            pageID = 0;
            for (int i = 0; i < rule.Length; i++)
            {
                char ch = rule[i];
                if (ch == '@')
                {
                    pageID = Utils.GetInt(rule, ref i);
                    continue;
                }
                if (isSpecialCharacter(ch))
                {
                    GameObject newSymbol = new GameObject();
                    SpriteRenderer sr = newSymbol.AddComponent<SpriteRenderer>();
                    sr.sprite = GetSprite(ch);
                    sr.sortingLayerName = "UI";
                    sr.sortingOrder = 1;
                    sr.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                    newRule.AppendItem(newSymbol.transform);
                }
                else
                {
                    GameCard newCard = Instantiate(gameCardPrefab).GetComponent<GameCard>();
                    newCard.ch = ch;
                    newCard.cover.sprite = puzzleInfo.GetSprite(ch);
                    newRule.AppendItem(newCard.transform);
                    newCard.cover.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                    newCard.bg.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                }
            }
            pages[pageID].AppendItem(newRule.transform);
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
        return ch == '=' || ch == '>' || ch == '|' || ch == '@';
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
    List<string> commands = new List<string>{
        "I","II","III","IV","V","VI","VII","VIII","IX","X"
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
                    newOption.CreateCards(s);
                    optionLayout.AppendItem(newOption.rootObject.transform);
                }
            if (rule.rightMatch)
                foreach(var s in rule.leftSide)
                {
                    ReplaceOption newOption = Instantiate(replaceOptionPrefab).GetComponentInChildren<ReplaceOption>();
                    newOption.rootObject.transform.position = new Vector3(0, 10, 0);
                    ReplaceOptions.Add(newOption);
                    newOption.cardS = s;
                    newOption.CreateCards(s);
                    optionLayout.AppendItem(newOption.rootObject.transform);
                }
        }
        if (ReplaceOptions.Count > 0)
        {
            int i = 0;
            foreach(var option in ReplaceOptions)
            {
                //option.maxWidth = optionLayout.maxWidth / (ReplaceOptions.Count + 1) * 0.4f;
                if (ReplaceOptions.Count > 1) option.text.text = commands[i];
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
