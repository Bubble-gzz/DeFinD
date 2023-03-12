using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TargetItem : MonoBehaviour
{
    // Start is called before the first frame update
    SmoothObject textMotion;
    TMP_Text text;
    int exist, require;
    public string s;
    public Color normalColor, accpetedColor;
    public bool accepted;
    void Awake()
    {
        exist = 0; require = 0;
        text = GetComponentInChildren<TMP_Text>();
        textMotion = transform.Find("Count").GetComponent<SmoothObject>();
       //textMotion = 
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateRequireCount(int newCount)
    {
        if (require == newCount) return;
        require = newCount;
        UpdateText();
    }
    public void UpdateExistCount(int newCount)
    {
        if (exist == newCount) return;
        exist = newCount;
        UpdateText();
    }
    void UpdateText()
    {
        textMotion.transform.localScale = new Vector2(1.2f, 1.2f);
        text.text = exist + "/" + require;
        accepted = (exist == require);
        if (accepted) text.color = accpetedColor;
        else          text.color = normalColor;
    }
}
