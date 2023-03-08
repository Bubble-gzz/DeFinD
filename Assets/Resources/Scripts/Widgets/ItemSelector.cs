using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelector : ItemLayout
{
    ISelectable selectedItem;
    [SerializeField]
    SmoothObject selectIcon;

    // Update is called once per frame
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
        int selectDelta = 0;
        if (Input.GetKeyDown(KeyCode.LeftArrow)) selectDelta--;
        if (Input.GetKeyDown(KeyCode.RightArrow)) selectDelta++;
        
        int select = GetSelectedIndex();
        if (selectDelta != 0 && inBoundary(select + selectDelta))
        {
            select += selectDelta;
            ReSelect(items[select].GetComponent<ISelectable>());
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            items[select].GetComponent<ISelectable>().Hit();
        }
        /* Debug
            if (Input.GetKeyDown(KeyCode.A))
            {
                AddCard(Random.Range(0, cards.Count));
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                DelCard(Random.Range(0, cards.Count));
            }
        */
    }
    void UpdateSelectIcon()
    {
        if (items.Count == 0)
        {
            selectIcon.gameObject.SetActive(false);
        }
        else
        {
            if (selectedItem == null) selectedItem = items[0].GetComponent<ISelectable>();
            selectIcon.gameObject.SetActive(true);
            selectIcon.targetPos = selectedItem.transform.localPosition;  
        }
    }
    int GetSelectedIndex()
    {
        if (selectedItem == null) return -1;
        return items.IndexOf((SmoothObject)selectedItem);
    }
    bool inBoundary(int pos)
    {
        return (pos >= 0 && pos < items.Count);
    }
    void ReSelect(ISelectable newItem)
    {
        if (selectedItem != null) selectedItem.SetHighlight(false);
        selectedItem = newItem;
        if (selectedItem != null) selectedItem.SetHighlight(true);
    }
}
