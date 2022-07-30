using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//NGUI TQL!!!
public class CS_3DUI_Camera : MonoBehaviour
{
	public Vector2 degrees = new Vector2(5f, 3f);
	public float range = 1f;

	Transform mTrans;
	Quaternion mStart;
	Vector2 mRot = Vector2.zero;

	void Start ()
	{
		mTrans = transform;
		mStart = mTrans.localRotation;
	}

	void Update ()
	{
		float delta = RealTime.deltaTime;
		Vector3 pos = UICamera.lastEventPosition;

		float halfWidth = Screen.width * 0.5f;
		float halfHeight = Screen.height * 0.5f;
		if (range < 0.1f) range = 0.1f;
		float x = Mathf.Clamp((pos.x - halfWidth) / halfWidth / range, -1f, 1f);
		float y = Mathf.Clamp((pos.y - halfHeight) / halfHeight / range, -1f, 1f);
		mRot = Vector2.Lerp(mRot, new Vector2(x, y), delta * 5f);

		mTrans.localRotation = mStart * Quaternion.Euler(-mRot.y * degrees.y, mRot.x * degrees.x, 0f);
	}
}
