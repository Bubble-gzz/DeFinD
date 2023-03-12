using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;

public class TargetPanel : MonoBehaviour
{
    // Start is called before the first frame update
    PuzzleInfo puzzleInfo;
    CardOperator cardOperator;
    public float maxWidth;
    PanelSlider slider;
    GameObject gameCardPrefab;
    public GameObject TargetPrefab;
    List<ItemLayout> pages;
    void Awake()
    {
        Global.targetPanel = this;
        slider = GetComponentInChildren<PanelSlider>();
        if (slider == null) Debug.Log("Rule Panel Slider Missing!");
        slider.leftKey = KeyCode.A;
        slider.rightKey = KeyCode.D;
        slider.pageWidth = maxWidth;
        pages = new List<ItemLayout>();
        targets = new List<TargetItem>();
    }
    void Start()
    {
        puzzleInfo = Global.puzzleInfo;
        cardOperator = Global.cardOperator;
        gameCardPrefab = Global.gameCardPrefab;

        ParseTargets();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ParseTargets()
    {
        
        /* Create Pages */
        int pageCount = 1, pageID;
        foreach(var target in puzzleInfo.Targets)
        {
            string s = target.s;
            pageID = 0;
            for (int i = 0; i < s.Length; i++)
            {
                char ch = s[i];
                if (ch == '@')
                {
                    pageID = Utils.GetInt(s, ref i);
                    if (pageID + 1 > pageCount) pageCount = pageID + 1;
                    continue;
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
            layoutSettings.defaultInterval = 1.2f;
            layoutSettings.minInterval = 0.7f;
            slider.AppendPage(newPage.transform);
            pages.Add(layoutSettings);
        }

        /* Create UI */
        foreach(var target in puzzleInfo.Targets)
        {
            var s = target.s;
            TargetItem newTarget = Instantiate(TargetPrefab).GetComponent<TargetItem>();
            ItemLayout newTargetLayout = newTarget.GetComponent<ItemLayout>();

            var newS = new StringBuilder();
            pageID = 0;
            for (int i = 0; i < s.Length; i++)
            {
                char ch = s[i];
                if (!isSpecialCharacter(ch))
                {
                    GameCard newCard = Instantiate(gameCardPrefab).GetComponent<GameCard>();
                    newCard.ch = ch;
                    newCard.cover.sprite = GetSprite(ch);
                    newTargetLayout.AppendItem(newCard.transform);
                    newCard.cover.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                    newCard.bg.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;

                    newS.Append(ch);
                }
                if (isSpecialCharacter(ch) || i + 1 == s.Length)
                {
                    newTarget.s = newS.ToString();
                }
                if (ch == '@')
                {
                    pageID = Utils.GetInt(s, ref i);
                    continue;
                }

            }
            pages[pageID].AppendItem(newTarget.transform);
            newTarget.UpdateRequireCount(target.count);
            newTarget.UpdateExistCount(0);
            targets.Add(newTarget);
        }
        DetectTargets(puzzleInfo.Init);
    }
    Sprite GetSprite(char ch) {
        return puzzleInfo.GetSprite(ch);
    }
    bool isSpecialCharacter(char ch)
    {
        return ch == '=' || ch == '>' || ch == '|' || ch == '@';
    }
    public void DetectTargets(string t)
    {
        int acceptCount = 0;
        foreach(var target in targets)
        {
            DetectTarget(t, target);
            if (target.accepted) acceptCount++;
        }
        if (acceptCount == targets.Count)
        {
            Global.puzzleComplete = true;
            Debug.Log("Puzzle Complete");
        }
        else Global.puzzleComplete = false;
    }
    void DetectTarget(string t, TargetItem target)
    {
        string s = target.s;
        int l = s.Length, count = 0;
        for (int i = 0; i + l - 1 < t.Length; i++)
        {
            bool match = true;
            for (int j = 0; j < l; j++)
                if (s[j] != t[i + j]) {
                    match = false;
                    break;
                }
            if (match) count++;
        }
        target.UpdateExistCount(count);
    }
    List<TargetItem> targets;
}
