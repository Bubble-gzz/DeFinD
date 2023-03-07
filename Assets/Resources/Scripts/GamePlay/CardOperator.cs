using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CardOperator : CardManager
{
    PuzzleInfo puzzleInfo;
    override protected void Awake()
    {
        base.Awake();
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
            Card newCard = AddCard(cards.Count);
            newCard.text.text = ch.ToString();
        }
    }

}
