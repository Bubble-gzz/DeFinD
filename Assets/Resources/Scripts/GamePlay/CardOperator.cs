using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CardOperator : CardManager
{
    public string puzzleName;
    PuzzleInfo puzzleInfo;
    override protected void Awake()
    {
        base.Awake();
        puzzleInfo = Resources.Load<PuzzleInfo>("PuzzleData/" + puzzleName);
    }
    override protected void Start()
    {
        base.Start();
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
            Card newCard = AddCard(cards.Count);
            newCard.text.text = ch.ToString();
        }
    }
    void ParseRules()
    {
        
    }
}
