using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCard : Card
{
    // Start is called before the first frame update
    public Image cover;
    public char ch;
    public CardOperator cardOperator;
    override protected void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    override protected void Update()
    {
        base.Update();
    }
    override public void Pick()
    {
        base.Pick();
        Global.rulePanel.DetectRulePatterns();
    }
    public override void Unpick()
    {
        base.Unpick();
        Global.rulePanel.DetectRulePatterns();
    }
}
