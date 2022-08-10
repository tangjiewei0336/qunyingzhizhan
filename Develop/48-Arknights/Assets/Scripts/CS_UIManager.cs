using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CS_UIManager : MonoBehaviour {

    private static CS_UIManager instance = null;

    public GameObject DigitPrefab;
    public static CS_UIManager Instance { get { return instance; } }
    // Start is called before the first frame update

    [SerializeField] Text myText_Life;
    [SerializeField] Text myText_Count;
    [SerializeField] Text myText_CostBalance;

    [SerializeField] Image myImage_CostProgressBar;

    [SerializeField] GameObject myPage_End;
    [SerializeField] GameObject myPage_Fail;

    [SerializeField] Button[] BackupPlayer;


    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
    }

    private void Start() {
        myPage_End.SetActive(false);
        myPage_Fail.SetActive(false);
    }

    public void SetLife(int g_life) {
        myText_Life.text = g_life.ToString();
    }

    public void SetCount(int g_current, int g_total) {
        myText_Count.text = g_current.ToString("0") + "/" + g_total.ToString("0");
    }

    /// <summary>
    /// 设置获取费用进度条
    /// </summary>
    /// <param name="Percentage"></param>
    public void SetProgressbarValue(float Percentage)
    {
        myImage_CostProgressBar.rectTransform.sizeDelta = new Vector2(500f * Percentage, 18);
        myImage_CostProgressBar.rectTransform.anchoredPosition = new Vector3(-500f + 250f * Percentage, 307, 0);
    }

    public void SetCost(int cost)
    {
        myText_CostBalance.text = cost.ToString();
    }

    //public void OnButtonPlayer (int g_index) {
    //    CS_GameManager.Instance.SetMyCurrentPlayer (g_index);
    //}

    public void OnButtonTitle() {
        Time.timeScale = 1;
        SceneManager.LoadScene("Title");
    }

    public void OnButtonRestart() {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ShowPageEnd() {
        Time.timeScale = 1f;
        myPage_End.SetActive(true);
        Invoke(nameof(Summarize), 1.67f);
    }

    public void Summarize()
    {
        SceneManager.LoadScene("Title");

    }

    public void ShowPageFail() {
        myPage_Fail.SetActive(true);
    }

    GameObject TempObject;
    public void DisplayDigits(long value, Vector3 position,GameObject Target)
    {
        TempObject = Instantiate(DigitPrefab);
        TempObject.transform.SetParent(GameObject.Find("NumericDisplay").transform);
        CS_DamageDisplayDigit TempScript = TempObject.GetComponent<CS_DamageDisplayDigit>();
        TempObject.GetComponent<RectTransform>().position = position;
        TempScript.SetDamageValue(value,Target,new Vector3(Random.Range(-0.5f, 0.5f) , Random.Range(-0.5f, 0.5f), Random.Range(-0.1f, 0.1f)));
        TempObject.SetActive(true);
    }

}
