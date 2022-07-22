using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_BackPackUI : MonoBehaviour
{
    public Vector2 range = new Vector2(5f, 3f);
    Transform mTrans;
    //Quaternion(四元数)表示旋转
    Quaternion mStart;
    Vector2 mRot = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        mTrans = transform; 
        // 获得界面的自身旋转角度
        mStart = mTrans.localRotation; 
    }

    // Update is called once per frame
    void Update()
    {
        //好耶！感谢SkyeBeFreeman的算法
        Vector3 pos = Input.mousePosition;
        float halfWidth = Screen.width * 0.5f;
        float halfHeight = Screen.height * 0.5f;
        float x = Mathf.Clamp((pos.x - halfWidth) / halfWidth, -1f, 1f);
        float y = Mathf.Clamp((pos.y - halfHeight) / halfHeight, -1f, 1f);
        mRot = Vector2.Lerp(mRot, new Vector2(x, y), Time.deltaTime * 5f);
        mTrans.localRotation = mStart * Quaternion.Euler(-mRot.y * range.y, mRot.x * range.x, 0f);
    }
}
