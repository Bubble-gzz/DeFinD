using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RulePanel : MonoBehaviour
{
    // Start is called before the first frame update
    PuzzleInfo puzzleInfo;
    public GameObject RulePrefab;
    public Sprite toSymbol, equalSymbol, orSymbol;
    void Awake()
    {
    }
    void Start()
    {
        puzzleInfo = Global.puzzleInfo;
        ParseRules();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ParseRules()
    {
        foreach(var rule in puzzleInfo.rules)
        {
            GameObject newRule = Instantiate(RulePrefab, transform);
            foreach(char ch in rule)
            {
                GameObject newItem = new GameObject();
                newItem.AddComponent<Image>();
                newItem.GetComponent<Image>().sprite = GetSprite(ch);
                newItem.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);
                newItem.transform.SetParent(newRule.transform);
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
}
