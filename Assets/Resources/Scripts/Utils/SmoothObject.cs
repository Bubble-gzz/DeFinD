using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothObject : MonoBehaviour
{
    public Vector2 targetPos;
    public float posFollowSpeed = 1f;
    public Vector2 targetSize;
    public float sizeFollowSpeed = 1f;
    public bool settledDown;
    virtual protected void Awake()
    {
        targetSize = transform.localScale;
        settledDown = false;
    }
    virtual protected void Start()
    {
        
    }

    // Update is called once per frame
    virtual protected void Update()
    {
        //targetPos = Global.mainCam.ScreenToWorldPoint(Input.mousePosition);
        CheckPos();
        CheckSize();
    }
    virtual public void SelfDestroy()
    {
        Destroy(gameObject);
    }
    void CheckPos()
    {
        Vector3 offset = (Vector3)targetPos - transform.localPosition;
        if (offset.magnitude < 0.001f) {
            transform.localPosition = targetPos;
            settledDown = true;
        }
        else {
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, posFollowSpeed * Time.deltaTime);
            settledDown = false;
        }
    }
    void CheckSize()
    {
        Vector3 offset = (Vector3)targetSize - transform.localScale;
        if (offset.magnitude < 0.01f) transform.localScale = targetSize;
        else transform.localScale = Vector3.Lerp(transform.localScale, targetSize, sizeFollowSpeed * Time.deltaTime);    
    }
}
