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
    public GameObject ATKBar;
    public GameObject DEFBar;
    public GameObject SPDBar;
    public GameObject BLKBar;
    TextMeshProUGUI Digits;
    TextMeshProUGUI CodeName;
    TextMeshProUGUI ATK;
    TextMeshProUGUI DEF;
    TextMeshProUGUI SPD;
    TextMeshProUGUI BLK;
    public bool ChangePerson;

    // Start is called before the first frame update
    void Awake()
    {
        Digits = HPBar.GetComponent<TextMeshProUGUI>();
        CodeName = NameBar.GetComponent<TextMeshProUGUI>();
        ATK = ATKBar.GetComponent<TextMeshProUGUI>();
        DEF = DEFBar.GetComponent<TextMeshProUGUI>();
        SPD = SPDBar.GetComponent<TextMeshProUGUI>();
        BLK = BLKBar.GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        CurrentPlayer = control.myCurrentStagePlayer;
    }

    void Update()
    {
        if (ChangePerson)
        {
            CurrentPlayer = control.myCurrentStagePlayer;
            ChangePerson = false;
        }
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(520 * CurrentPlayer.GetHealthPercent(), 10);
        Digits.text = CurrentPlayer.boardProperty.myStatus_Health.ToString() + "/" + CurrentPlayer.boardProperty.initial_myStatus_Health.ToString();
        CodeName.text = CurrentPlayer.CodeName;
        ATK.text = CurrentPlayer.boardProperty.myStatus_Attack.ToString();
        DEF.text = CurrentPlayer.boardProperty.myStatus_Defense.ToString();
        SPD.text = CurrentPlayer.boardProperty.myStatus_SpellResistance.ToString();
        BLK.text = CurrentPlayer.boardProperty.myStatus_Blocking.ToString() + "/" + CurrentPlayer.boardProperty.initial_myStatus_Blocking.ToString();
    }
}
