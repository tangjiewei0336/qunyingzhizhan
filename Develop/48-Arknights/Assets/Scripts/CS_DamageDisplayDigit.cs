using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CS_DamageDisplayDigit : MonoBehaviour
{
    TextMeshPro DisPlayBar;
    GameObject TargetObj;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Destroy",1);
    }

    Vector3 Offset;
    public void SetDamageValue(long value, GameObject Stick,Vector3 Deviation)
    {
        TargetObj = Stick;
        DisPlayBar = this.gameObject.GetComponent<TextMeshPro>();
        DisPlayBar.text = value.ToString();
        Offset = Deviation;
    }

    private void Destroy()
    {
        Destroy(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        this.transform.position = TargetObj.transform.position;
        this.transform.Translate(new Vector3(Offset.x, Offset.y, Offset.z));
    }
}
