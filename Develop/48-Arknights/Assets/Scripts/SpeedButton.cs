using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedButton : MonoBehaviour
{
    public Text SpeedButtonText;
    // Start is called before the first frame update
    public CS_GameManager control;

    public bool Paused;
    public int SpeedTo = 1;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Click()
    {
        if (SpeedButtonText.text == "1x")
        {
            SpeedTo = 2;
            SpeedButtonText.text = "2x";
        }
        else
        {
            SpeedTo = 1;
            SpeedButtonText.text = "1x";
        }

        if (!Paused)
        {
            if (control.FriendSelecting)
            {
                control.SetSpeed(SpeedScale.Slow, SpeedLockBehavior.Unlock, SpeedTo);
            }
            else
            {
                control.SetSpeed(SpeedScale.Normal, SpeedLockBehavior.Unlock, SpeedTo);
            }

        }
        else
        {
            
                control.SetSpeed(SpeedScale.Paused, SpeedLockBehavior.Lock, SpeedTo);
        }

    }
}
