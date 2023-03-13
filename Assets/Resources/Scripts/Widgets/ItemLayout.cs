using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLayout : MonoBehaviour
{
    // Start is called before the first frame update
    public enum Alignment{
        Left,
        Center,
        Right,
        Spread
    }
    public bool autoAddChildren;
    public Alignment alignment;
    public bool nested;
    public float maxWidth;
    public float width;
    public float defaultInterval;
    public float minInterval;

    public float posFollowSpeed;
    public float sizeFollowSpeed;

    public List<SmoothObject> items;
    protected SmoothObject motion;
    virtual protected void Awake()
    {
        if (sizeFollowSpeed < 0.001f) sizeFollowSpeed = 40f;
        if (posFollowSpeed < 0.001f) posFollowSpeed = 55f;
        
        motion = GetComponent<SmoothObject>();
        if (motion == null) {
            motion = gameObject.AddComponent<SmoothObject>();
        }
        motion.posFollowSpeed = posFollowSpeed;
        motion.sizeFollowSpeed = sizeFollowSpeed;
        items = new List<SmoothObject>();
        if (minInterval < 0.001f) minInterval = 1f;
        if (autoAddChildren)
            AddChildren();
    }
    virtual protected void Start()
    {

    }

    // Update is called once per frame
    virtual protected void Update()
    {
    }
    void AddChildren()
    {
        Transform items = transform.Find("items");
        List<Transform> children = new List<Transform>();
        foreach(Transform child in items) children.Add(child);
        foreach(Transform child in children)
        {
            AppendItem(child);
        }
    }
    bool inBoundary(int pos)
    {
        return (pos >= 0 && pos < items.Count);
    }
    virtual public Transform AddItem(int pos, Transform newItem)
    {
        SmoothObject itemMotion = newItem.GetComponent<SmoothObject>();
        if (itemMotion == null) {
            itemMotion = newItem.gameObject.AddComponent<SmoothObject>();
            itemMotion.posFollowSpeed = 40f;
            itemMotion.sizeFollowSpeed = 40f;
        }
        items.Insert(pos, itemMotion);
        newItem.SetParent(transform);
        UpdatePos();
        return newItem;
    }
    virtual public Transform AppendItem(Transform newItem)
    {
        return AddItem(items.Count, newItem);
    }
    virtual public void DelItem(int pos)
    {
        items.RemoveAt(pos);
        UpdatePos();
    }
    virtual public void ClearItems()
    {
        items.Clear();
    }
    public void UpdatePos()
    {
        if (nested) UpdatePos_Uneven();
        else UpdatePos_Even();

    }
    void UpdatePos_Uneven()
    {
        float pos, interval, totalWidth = 0;
        int n = items.Count;
        for (int i = 0; i < n; i++)
        {
            ItemLayout childLayout = items[i].GetComponentInChildren<ItemLayout>();
            childLayout.UpdatePos();
            totalWidth += childLayout.width;
        }
        //Debug.Log("n:" + n + "  totalWidth:" + totalWidth);
        if (totalWidth + defaultInterval * (n - 1) <= maxWidth) interval = defaultInterval;
        else interval = minInterval;
        totalWidth += interval * (n - 1);
        if (totalWidth > maxWidth)
        {
            motion.targetSize = new Vector2(maxWidth / totalWidth, maxWidth / totalWidth);
        }
        else motion.targetSize = new Vector2(1, 1);

        pos = - totalWidth / 2;
        for (int i = 0; i < n; i++)
        {
            float width = items[i].GetComponentInChildren<ItemLayout>().width;
            //Debug.Log("width[" + i + "]:" + width);
            pos +=  width / 2;
            items[i].targetPos = new Vector2(pos, 0);
            pos += interval + width / 2;            
        }
        width = Mathf.Min(totalWidth, maxWidth);
        //Debug.Log("interval:" + interval + "  totalWidth:" + totalWidth);
    }
    void UpdatePos_Even()
    {
        float pos, interval;
        int n = items.Count;
        
        if ((n + 1) * defaultInterval > maxWidth) interval = maxWidth / (n + 1);
        else interval = defaultInterval;

        if (interval < minInterval && alignment != Alignment.Spread)
        {
            motion.targetSize = new Vector2(interval / minInterval, interval / minInterval);
            interval = minInterval;
        }
        else motion.targetSize = new Vector2(1, 1);

        if (alignment == Alignment.Center)
        {
            if (n % 2 == 0) pos = - (n / 2 - 0.5f) * interval;
            else pos = - (n / 2) * interval;
        }
        else if (alignment == Alignment.Right) pos = - n * interval;
        else if (alignment == Alignment.Left) pos = 0;
        else {
            interval = maxWidth / (n + 1);
            pos = - maxWidth / 2 + interval;
        }
        for (int i = 0; i < n; i++)
        {
            items[i].targetPos = new Vector2(pos, 0);
            pos += interval;
        }
        width = Mathf.Min(maxWidth, interval * (n - 1 + 0.6f * 2));
        //Debug.Log("child width after updatePos:" + width);
    }
}