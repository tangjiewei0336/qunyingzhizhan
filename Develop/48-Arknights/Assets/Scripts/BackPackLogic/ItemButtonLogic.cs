using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemButtonLogic : MonoBehaviour
{
    public int ItemSerial;
    public CS_LoadItem MainControlScript;

    public void Click()
    {
        Debug.Log("Clicked");
        MainControlScript = GameObject.Find("MainControl").GetComponent<CS_LoadItem>();
        MainControlScript.ButtonClicked(this.gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
