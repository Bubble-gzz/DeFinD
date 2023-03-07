using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : SmoothObject
{
    bool selected;
    override protected void Awake()
    {
        base.Awake();
    }
    override protected void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    override protected void Update()
    {
        base.Update();    }
    override public void SelfDestroy()
    {
        Destroy(gameObject);
    }
    public void Select()
    {
        selected = true;
    }
    public void Deselect()
    {
        selected = false;
    }
}
