using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothObject : MonoBehaviour
{
    public Vector2 targetPos;
    public float followSpeed = 1f;
    virtual protected void Awake()
    {

    }
    virtual protected void Start()
    {
        
    }

    // Update is called once per frame
    virtual protected void Update()
    {
        //targetPos = Global.mainCam.ScreenToWorldPoint(Input.mousePosition);
        CheckPos();
    }
    virtual public void SelfDestroy()
    {
        Destroy(gameObject);
    }
    void CheckPos()
    {
        Vector3 offset = (Vector3)targetPos - transform.localPosition;
        if (offset.magnitude < 0.01f) transform.localPosition = targetPos;
        else transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, followSpeed * Time.deltaTime);
    }
}
