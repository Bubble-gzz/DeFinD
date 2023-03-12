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
    public GameObject smoothCirclePrefab;
    ItemLayout indexLayout;
    List<SmoothObject> indexes;
    public GameObject slider;
    public Vector3 indexSize0, indexSize1;
    public Color indexColor0, indexColor1;
    SmoothObject lastEnlightedIndex;
    SmoothObject leftArrow, rightArrow;
    void Awake()
    {
        leftArrow = transform.Find("leftArrow").GetComponent<SmoothObject>();
        rightArrow = transform.Find("rightArrow").GetComponent<SmoothObject>();

        motion = slider.AddComponent<SmoothObject>();
        motion.posFollowSpeed = 10f;
        motion.sizeFollowSpeed = 40f;

        pages = new List<Transform>();
        indexLayout = transform.Find("IndexLayout").GetComponent<ItemLayout>();
        indexes = new List<SmoothObject>();
        lastEnlightedIndex = null;
        curPage = 0;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UserInput();
        if (pageCount < 1) {
            leftArrow.targetSize = new Vector2(0, 0.3f);
            rightArrow.targetSize = new Vector2(0, 0.3f);
        }
        else
        {
            if (curPage > 0) leftArrow.targetSize = new Vector2(1, 1);
            else leftArrow.targetSize = new Vector2(0, 0.3f);

            if (curPage < pageCount - 1) rightArrow.targetSize = new Vector2(1, 1);
            else rightArrow.targetSize = new Vector2(0, 0.3f);
        }
    }
    void UserInput()
    {
        if (Input.GetKeyDown(leftKey))
        {
            if (curPage > 0) {
                curPage--;
                leftArrow.transform.localPosition = new Vector3(-6.2f, 0, 0);
                UpdatePos();
            }
        }
        if (Input.GetKeyDown(rightKey))
        {
            if (curPage + 1 < pageCount)
            {
                curPage++;
                rightArrow.transform.localPosition = new Vector3(6.2f, 0, 0);
                UpdatePos();
            }
        }
    }
    public void AppendPage(Transform newPage)
    {
        newPage.transform.SetParent(slider.transform);
        pages.Add(newPage);
        pageCount++;
        GameObject newCircle = Instantiate(smoothCirclePrefab);
        indexes.Add(newCircle.GetComponent<SmoothObject>());
        indexLayout.AppendItem(newCircle.transform);
        newCircle.GetComponent<SmoothObject>().targetColor = indexColor0;
        newCircle.GetComponent<SmoothObject>().targetSize = indexSize0;
        if (pageCount == 1) EnlightNewIndex();
        UpdatePos();
    }
    public void EnlightNewIndex()
    {
        //Debug.Log("curPage: " + curPage + "  indexCount:" + indexes.Count);
        if (lastEnlightedIndex == indexes[curPage]) return;
        if (lastEnlightedIndex != null && indexes[curPage] != lastEnlightedIndex)
        {
            lastEnlightedIndex.targetSize = indexSize0;
            lastEnlightedIndex.targetColor = indexColor0;
        }
        lastEnlightedIndex = indexes[curPage];
        indexes[curPage].targetSize = indexSize1;
        indexes[curPage].targetColor = indexColor1;
    }
    public void Clear()
    {
        foreach(var child in pages) Destroy(child);
        pages.Clear();
        pageCount = curPage = 0;
    }
    void UpdatePos()
    {
        EnlightNewIndex();
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
