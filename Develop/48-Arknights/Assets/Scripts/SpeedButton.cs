using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedButton : MonoBehaviour
{
    public Text text1;
    // Start is called before the first frame update
    public CS_GameManager control;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Click()
    {
        Debug.Log("SpeedChange");
        if (text1.text == "1x")
        {
            Debug.Log("SpeedChange");
            control.SetSpeed(2);
            text1.text = "2x";
        }
        else
        {
            control.SetSpeed(1);
            text1.text = "1x";
        }
    }
}
