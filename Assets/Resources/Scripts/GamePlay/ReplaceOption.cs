using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReplaceOption : ItemLayout, ISelectable
{
    // Start is called before the first frame update
    public GameObject CardPrefab;
    PuzzleInfo puzzleInfo;
    public string cardS;
    public TMP_Text text;
    public GameObject rootObject;
    protected override void Awake()
    {
        base.Awake();
        puzzleInfo = Global.puzzleInfo;
    }
    override protected void Start()
    {
        base.Start();
        puzzleInfo = Global.puzzleInfo;
        //CreateCards(cardS);
        Arrive();
    }

    override protected void Update()
    {
        base.Update();
    }
    public void CreateCards(string s)
    {
        //Debug.Log("CreateCards:" + s);
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
        motion.targetSize = new Vector2(1, 0);
        StartCoroutine(Leave_C());
    }
    IEnumerator Leave_C()
    {
        text.enabled = false;
        yield return new WaitForSeconds(0.5f);
        Destroy(rootObject);
    }
    public void Use()
    {
        
    }

    public void SetHighlight(bool flag)
    {
        if (flag)
        {
            motion.targetSize = new Vector2(1.2f, 1.2f);
        }
        else
        {
            motion.targetSize = new Vector2(1, 1);
        }
    }

    public void Hit()
    {
        Global.cardOperator.PickOption(this);
    }
}
