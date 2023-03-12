using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelector : ItemLayout
{
    SmoothObject selectedItem;
    public SmoothObject selectIcon;
    bool active;
    protected int defaultSelectPos;

    // Update is called once per frame
    protected override void Awake()
    {
        base.Awake();
        active = true;
    }
    override protected void Start()
    {
        base.Start();
        selectedItem = null;
    }
    override protected void Update()
    {
        base.Update();
        UserInputs();
        UpdateSelectIcon();
    }
    void UserInputs()
    {
        if (!active || selectIcon == null) return;
        int selectDelta = 0;
        if (Input.GetKeyDown(KeyCode.LeftArrow)) selectDelta--;
        if (Input.GetKeyDown(KeyCode.RightArrow)) selectDelta++;
        
        int select = GetSelectedIndex();
        if (selectDelta != 0 && inBoundary(select + selectDelta))
        {
            select += selectDelta;
            ReSelect(items[select]);
        }
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            items[select].GetComponentInChildren<ISelectable>().Hit();
        }
    }
    void UpdateSelectIcon()
    {
        if (!active) return;
        if (selectIcon == null) return;
        if (items.Count == 0)
        {
            selectIcon.gameObject.SetActive(false);
        }
        else
        {
            if (selectedItem == null) {
                ReSelect(items[0]);
            }
            selectIcon.gameObject.SetActive(true);
            selectIcon.targetPos = selectedItem.transform.localPosition;  
        }
    }
    public override void DelItem(int pos)
    {
        base.DelItem(pos);
        if (items.Count == 0) selectedItem = null;
        else if (pos >= items.Count) ReSelect(items[items.Count - 1]);
        else ReSelect(items[pos]);
    }
    int GetSelectedIndex()
    {
        if (selectedItem == null) return -1;
        return items.IndexOf((SmoothObject)selectedItem);
    }
    protected void SelectWithIndex(int index)
    {
        if (index >= items.Count)
        {
            Debug.Log("[ItemSelector.SelectWithIndex]: Index out of range!");
            return;
        }
        ReSelect(items[index]);
    }
    bool inBoundary(int pos)
    {
        return (pos >= 0 && pos < items.Count);
    }
    void ReSelect(SmoothObject newItem)
    {
        if (selectedItem != null) selectedItem.GetComponentInChildren<ISelectable>().SetHighlight(false);
        selectedItem = newItem;
        if (selectedItem != null) selectedItem.GetComponentInChildren<ISelectable>().SetHighlight(true);
    }
    public void SetActive(bool flag)
    {
        if (flag == active) return;
        active = flag;
        if (!active)
        {
            defaultSelectPos = GetSelectedIndex();
            ReSelect(null);
        }
        else
        {
            defaultSelectPos = Mathf.Min(defaultSelectPos, items.Count - 1);
            ReSelect(items[defaultSelectPos]);
        }
    }
}
