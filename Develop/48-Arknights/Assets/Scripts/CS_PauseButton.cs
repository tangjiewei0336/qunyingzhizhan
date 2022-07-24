using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CS_PauseButton : MonoBehaviour
{
    public SpeedButton SpeedButtonScript;
    public Text StopButtonText;
    // Start is called before the first frame update
    public CS_GameManager control;
    public GameObject PauseImage;
    public void Click()
    {
        if (StopButtonText.text == "| |")
        {
            SpeedButtonScript.Paused = true;
            StopButtonText.text = "▶";
        }
        else
        {
            SpeedButtonScript.Paused = false;
            StopButtonText.text = "| |";
        }
        if (!SpeedButtonScript.Paused)
        {
            if (control.FriendSelecting)
            {
                control.SetSpeed(SpeedScale.Slow, SpeedLockBehavior.Unlock);
                PauseImage.SetActive(false);
            }
            else
            {
                control.SetSpeed(SpeedScale.Normal, SpeedLockBehavior.Unlock);
                PauseImage.SetActive(false);
            }
        }
        else
        {
            control.SetSpeed(SpeedScale.Paused, SpeedLockBehavior.Lock);
            PauseImage.SetActive(true);
        }
    }
}
