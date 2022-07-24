using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CS_HPBar : MonoBehaviour
{
    public CS_Player CurrentPlayer;
    public CS_GameManager control;
    public GameObject HPBar;
    public GameObject NameBar;
    TextMeshProUGUI Digits;
    TextMeshProUGUI CodeName;

    // Start is called before the first frame update
    void Awake()
    {
        Digits = HPBar.GetComponent<TextMeshProUGUI>();
        CodeName = NameBar.GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        CurrentPlayer = control.myCurrentStagePlayer;
    }

    void Update()
    {
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(520 * CurrentPlayer.myCurrentHealth / CurrentPlayer.myStatus_MaxHealth, 10);
        Digits.text = CurrentPlayer.myCurrentHealth.ToString() + "/" + CurrentPlayer.myStatus_MaxHealth.ToString();
        CodeName.text = CurrentPlayer.CodeName;
    }
}
