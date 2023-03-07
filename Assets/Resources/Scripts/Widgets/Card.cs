using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Card : SmoothObject
{
    public bool interactable;
    public Color PickedColor;
    Color originalColor;
    bool selected;
    public bool picked;
    Vector3 normalSize = new Vector3(1, 1, 1);
    Vector3 pickedSize = new Vector3(1.1f, 1.1f, 1);
    Vector3 selectedSize = new Vector3(1.25f, 1.25f, 1);
    public TMP_Text text;
    override protected void Awake()
    {
        base.Awake();
        originalColor = GetComponent<Renderer>().material.color;
        picked = false;
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
        UserInput();
        SizeCheck();
    }
    void SizeCheck()
    {
        if (!picked & !selected) targetSize = normalSize;
        else if (selected) targetSize = selectedSize;
        else targetSize = pickedSize;
    }
    void UserInput()
    {
        if (!interactable) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (selected)
            {
                if (picked) Unpick();
                else Pick();
            }
        }
    }
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
    virtual public void Pick()
    {
        picked = true;
        GetComponent<Renderer>().material.color = PickedColor;
        transform.localScale = new Vector3(0.9f, 0.9f, 1);
    }
    virtual public void Unpick()
    {
        picked = false;
        GetComponent<Renderer>().material.color = originalColor;
        transform.localScale = new Vector3(0.9f, 0.9f, 1);
    }
}
