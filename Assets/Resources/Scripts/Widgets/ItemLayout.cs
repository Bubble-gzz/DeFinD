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
    public Alignment alignment;
    public float maxWidth;
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
    }
    virtual protected void Start()
    {
        items = new List<SmoothObject>();
        if (minInterval < 0.001f) minInterval = 1.2f;
    }

    // Update is called once per frame
    virtual protected void Update()
    {
    }

    bool inBoundary(int pos)
    {
        return (pos >= 0 && pos < items.Count);
    }
    virtual public Transform AddItem(int pos, Transform newItem)
    {
        SmoothObject itemMotion = newItem.GetComponent<SmoothObject>();
        if (itemMotion == null) itemMotion = newItem.gameObject.AddComponent<SmoothObject>();
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
        float pos, interval;
        
        if ((items.Count - 1) * defaultInterval > maxWidth) interval = maxWidth / (items.Count - 1);
        else interval = defaultInterval;
        if (interval < minInterval && alignment != Alignment.Spread)
        {
            motion.targetSize = new Vector2(interval / minInterval, interval / minInterval);
            interval = minInterval;
        }
        else motion.targetSize = new Vector2(1, 1);

        if (alignment == Alignment.Center)
        {
            if (items.Count % 2 == 0) pos = - (items.Count / 2 - 0.5f) * interval;
            else pos = - (items.Count / 2) * interval;
        }
        else if (alignment == Alignment.Right) pos = - items.Count * interval;
        else if (alignment == Alignment.Left) pos = 0;
        else {
            interval = maxWidth / (items.Count + 1);
            pos = - maxWidth / 2 + interval;
        }
        for (int i = 0; i < items.Count; i++)
        {
            items[i].targetPos = new Vector2(pos, 0);
            pos += interval;
        }

    }
}
