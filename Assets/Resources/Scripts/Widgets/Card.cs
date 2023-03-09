using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Card : SmoothObject, ISelectable
{
    public Color PickedColor;
    Color originalColor;
    bool selected;
    public bool picked;
    protected bool destroyed;
    Vector3 normalSize = new Vector3(1, 1, 1);
    Vector3 pickedSize = new Vector3(1.1f, 1.1f, 1);
    Vector3 selectedSize = new Vector3(1.25f, 1.25f, 1);
    public TMP_Text text;
    public Renderer bg;
    override protected void Awake()
    {
        base.Awake();
        originalColor = bg.material.color;
        picked = false;
        destroyed = false;
        text = GetComponentInChildren<TMP_Text>();
    }
    override protected void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    override protected void Update()
    {
        base.Update();
        SizeCheck();
    }
    void SizeCheck()
    {
        if (destroyed) return;
        if (!picked & !selected) targetSize = normalSize;
        else if (selected) targetSize = selectedSize;
        else targetSize = pickedSize;
    }
    override public void SelfDestroy()
    {
        Destroy(gameObject);
    }
    public void SetHighlight(bool flag)
    {
        selected = flag;
    }
    public void Hit()
    {
        if (picked) Unpick();
        else Pick();
    }
    virtual public void Pick()
    {
        picked = true;
        bg.material.color = PickedColor;
        transform.localScale = new Vector3(0.9f, 0.9f, 1);
    }
    virtual public void Unpick()
    {
        picked = false;
        bg.material.color = originalColor;
        transform.localScale = new Vector3(0.9f, 0.9f, 1);
    }
}
