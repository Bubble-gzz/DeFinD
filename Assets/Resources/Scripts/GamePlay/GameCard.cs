using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCard : Card
{
    // Start is called before the first frame update
    public Image cover;
    public char ch;
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
    override public void Unpick()
    {
        base.Unpick();
        Global.rulePanel.DetectRulePatterns();
    }
    public void Leave()
    {
        targetSize = new Vector2(1, 0);
        StartCoroutine(Leave_C());
    }
    IEnumerator Leave_C()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
