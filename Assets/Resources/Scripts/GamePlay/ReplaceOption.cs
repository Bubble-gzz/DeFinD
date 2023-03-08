using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceOption : CardManager
{
    // Start is called before the first frame update
    PuzzleInfo puzzleInfo;
    public string cardS;
    public SmoothObject motion;
    protected override void Awake()
    {
        base.Awake();
        motion = GetComponent<SmoothObject>();
    }
    override protected void Start()
    {
        base.Start();
        puzzleInfo = Global.puzzleInfo;
        CreateCards(cardS);
        Arrive();
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
        }
    }
    public void Arrive()
    {
        
    }
    public void Leave()
    {
        
    }
    public void Use()
    {
        
    }
}
