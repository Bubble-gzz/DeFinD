using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceOption : ItemLayout
{
    // Start is called before the first frame update
    public GameObject CardPrefab;
    PuzzleInfo puzzleInfo;
    public string cardS;
    protected override void Awake()
    {
        base.Awake();
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
            GameCard newCard = Instantiate(CardPrefab, transform).GetComponent<GameCard>();
            AppendItem(newCard.transform);
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
