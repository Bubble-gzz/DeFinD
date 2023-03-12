using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSlider : MonoBehaviour
{
    // Start is called before the first frame update
    public int pageCount, curPage;
    public float pageWidth, pageInterval;
    SmoothObject motion;
    List<Transform> pages;
    public KeyCode leftKey, rightKey;
    Vector3 originalSize;
    void Awake()
    {
        motion = gameObject.AddComponent<SmoothObject>();
        motion.posFollowSpeed = 30f;
        motion.sizeFollowSpeed = 40f;

        pages = new List<Transform>();
        originalSize = transform.localScale;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UserInput();
    }
    void UserInput()
    {
        if (Input.GetKeyDown(leftKey))
        {
            if (curPage > 0) {
                curPage--;
                UpdatePos();
            }
        }
        if (Input.GetKeyDown(rightKey))
        {
            if (curPage + 1 < pageCount)
            {
                curPage++;
                UpdatePos();
            }
        }
    }
    public void AppendPage(Transform newPage)
    {
        newPage.transform.SetParent(transform);
        pages.Add(newPage);
        pageCount++;
        UpdatePos();
    }
    public void Clear()
    {
        foreach(var child in pages) Destroy(child);
        pages.Clear();
        pageCount = curPage = 0;
    }
    void UpdatePos()
    {
        float pos = 0;
        for (int i = 0; i < pages.Count; i++)
        {
            SmoothObject motion = pages[i].GetComponent<SmoothObject>();
            Vector2 targetPos = new Vector2(pos, 0);
            if (motion == null) pages[i].position = targetPos;
            else motion.targetPos = targetPos;
            pos += pageWidth + pageInterval;
        }
        motion.targetPos = new Vector2(-(pageWidth + pageInterval) * curPage, 0);
    }
}
