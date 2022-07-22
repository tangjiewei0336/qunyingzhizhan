using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGameButton : MonoBehaviour
{
    // Start is called before the first frame update
    public Toggle[] Selections;
    public DataStructure TransmitMessage;
    public FriendGroup FriendLoadData;


    public void Click()
    {
        FriendLoadData = new FriendGroup();
        FriendLoadData.StandbyArmour = new List<FriendData>();
        foreach (Toggle TempToggle in Selections)
        {
            if (TempToggle.isOn)
            {
                FriendData TempFriendData = new FriendData();
                TempFriendData.Level = 1;
                TempFriendData.Name = TempToggle.GetComponentInChildren<Text>().text;
                FriendLoadData.StandbyArmour.Add(TempFriendData);
                TransmitMessage.TransportMessage = FriendLoadData;
            }
        }
        Debug.Log("123");
        Debug.Log(Selections.Length);
        Debug.Log(FriendLoadData.StandbyArmour.Count);
        Invoke("Next", 1);
    }

    void Next()
    {
         SceneManager.LoadScene("Game 1");
    }
    // Update is called once per frame
    void Update()
    {

    }
}
