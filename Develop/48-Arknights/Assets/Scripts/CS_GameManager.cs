using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_GameManager : MonoBehaviour {

    private static CS_GameManager instance = null;
    public static CS_GameManager Instance { get { return instance; } }

    [SerializeField] int myMaxLife = 10;
    private int myCurrentLife;

    [SerializeField] GameObject[] myPlayerPrefabs = null;

    [SerializeField] GameObject[] myButtonPrefabs = null;

    private List<CS_Player> myPlayerList = new List<CS_Player> ();

    [SerializeField] GameObject myDirectionObject = null;

    private CS_Player myCurrentPlayer;

    private void Awake () {
        if (instance != null && instance != this) {
            Destroy (this.gameObject);
        } else {
            instance = this;
        }
    }

    List<FriendData> FriendAssembly;

    private void Start () {
        // init total lives
        myCurrentLife = myMaxLife;
        CS_UIManager.Instance.SetLife (myCurrentLife);

        FriendAssembly =  GameObject.Find("DataStructure").GetComponent<DataStructure>().TransportMessage.StandbyArmour;
        Debug.Log(FriendAssembly);
        int currentSerial = 0;
        foreach(FriendData TempFriendData in FriendAssembly)
        {
            int TempDeployCost = 0;
            // init all players
            foreach (GameObject f_prefab in myPlayerPrefabs)
            {
                if (f_prefab.GetComponent<CS_Player>().CodeName == TempFriendData.Name)
                {
                    GameObject f_object = Instantiate(f_prefab, this.transform);
                    TempDeployCost = f_object.GetComponent<CS_Player>().GetDeployCost();
                    f_object.SetActive(false);
                    // get player script
                    CS_Player f_player = f_object.GetComponent<CS_Player>();
                    // add script to list
                    myPlayerList.Add(f_player);

                }
            }
            foreach (GameObject f_prefab in myButtonPrefabs)
            {
                if (f_prefab.GetComponent<CS_PlayerButton>().CodeName == TempFriendData.Name)
                {
                    GameObject f_object = Instantiate(f_prefab, this.transform);
                    f_object.transform.SetParent(GameObject.Find("GameCanvas").transform);
                    f_object.SetActive(true);
                    RectTransform TempPositionInfo = f_object.GetComponent<RectTransform>();
                    TempPositionInfo.gameObject.GetComponent<CS_PlayerButton>().DeployCost = TempDeployCost;
                    TempPositionInfo.localScale = new Vector3(1, 1, 1);
                    TempPositionInfo.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 300 * currentSerial, 300);
                    TempPositionInfo.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, 300);
                }
            }
            currentSerial += 1;
        }
        

        // init direction object
        myDirectionObject.SetActive (false);
        CostBalance = InitialCostBalance;
        BeginGainCost = true;
    }

    /// <summary>
    /// 是否让玩家获得费用
    /// </summary>
    bool BeginGainCost;
    float TimePast;
    [Tooltip("指定每多少秒获得一点部署费用，设定为0则不自动获得费用。")]
    public float CostGainInterval = 3;
    [Tooltip("指定关卡开始时的部署费用")]
    public int InitialCostBalance = 9;
    public int MaximumCost = 99;
    public int CostBalance = 0;


    void Update()
    {
        for (int i = 0; i < GameObject.Find("GameCanvas").transform.childCount; i++) {
           if (GameObject.Find("GameCanvas").transform.GetChild(i).GetComponent<CS_PlayerButton>() != null)
            {
                if (GameObject.Find("GameCanvas").transform.GetChild(i).GetComponent<CS_PlayerButton>().DeployCost <= CostBalance)
                {
                    GameObject.Find("GameCanvas").transform.GetChild(i).GetComponent<CS_PlayerButton>().Filters.gameObject.SetActive(false);
                }
                else
                {
                    GameObject.Find("GameCanvas").transform.GetChild(i).GetComponent<CS_PlayerButton>().Filters.gameObject.SetActive(true);
                }
            }
                      
        }
        if (BeginGainCost && (CostGainInterval != 0f) && (CostBalance < MaximumCost))
        {
            TimePast += Time.deltaTime;
            if (TimePast > CostGainInterval)
            {
                TimePast = 0;
                CostBalance += 1;
            }
            CS_UIManager.Instance.SetProgressbarValue(TimePast / CostGainInterval);
        }
        CS_UIManager.Instance.SetCost(CostBalance);
    }

    /// <summary>
    /// 扣除部署费用。
    /// </summary>
    /// <param name="expenditure">支出</param>
    /// <returns>是否扣除成功</returns>
    public bool reduceCost(int expenditure)
    {
        if (CostBalance >= expenditure)
        {
            CostBalance -= expenditure;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetMyCurrentPlayer (string CodeName) {
        // dont do anything if its setting direction
        if (myDirectionObject.activeSelf == true) {
            return;
        }

        foreach (CS_Player player in myPlayerList)
        {
            if (player.CodeName == CodeName)
            {
                myCurrentPlayer = player;
            }
        }
    }

    public void BeginDragPlayer () {
        // dont do anything if its setting direction
        if (myDirectionObject.activeSelf == true) {
            Debug.Log("myDirectionObject.activeSelf = true");
            return;
        }
        Debug.Log("CostBalance: " + CostBalance);
        Debug.Log("myCurrentPlayer.GetDeployCost: " + myCurrentPlayer.GetDeployCost());

        if (myCurrentPlayer.GetDeployCost() <= CostBalance)
        {
            myCurrentPlayer.gameObject.SetActive(true);
            myCurrentPlayer.Arrange();
            myCurrentPlayer.ShowHighlight();

            // set slow mode
            Time.timeScale = 0.1f;
                    }
    }

    public void DragPlayer () {
        // dont do anything if its setting direction
        if (myDirectionObject.activeSelf == true) {
            return;
        }

        // do raycast
        RaycastHit t_hit;
        Ray t_ray = Camera.main.ScreenPointToRay (Input.mousePosition);

        if (Physics.Raycast(t_ray, out t_hit)) {
            CS_Tile t_tile = t_hit.collider.gameObject.GetComponentInParent<CS_Tile> ();
            if (t_tile != null) {
                if (t_tile.GetType () == myCurrentPlayer.GetTileType ()) {
                    Vector3 t_position = t_tile.transform.position;
                    t_position.y = t_hit.point.y;
                    myCurrentPlayer.transform.position = t_position;
                    return;
                }
            }

            myCurrentPlayer.transform.position = t_hit.point;
        }
    }

    public void EndDragPlayer () {
        // dont do anything if its setting direction
        if (myDirectionObject.activeSelf == true) {
            return;
        }

        // hide highlight
        myCurrentPlayer.HideHighlight ();

        // do raycast
        RaycastHit t_hit;
        Ray t_ray = Camera.main.ScreenPointToRay (Input.mousePosition);

        if (Physics.Raycast (t_ray, out t_hit)) {
            CS_Tile t_tile = t_hit.collider.gameObject.GetComponentInParent<CS_Tile> ();
            if (t_tile != null) {
                if (t_tile.GetType () == myCurrentPlayer.GetTileType ()) {
                    // show direction object
                    myDirectionObject.transform.position = myCurrentPlayer.transform.position;
                    myDirectionObject.SetActive (true);
                    return;
                }
            }
        }

        // reset current player
        myCurrentPlayer.gameObject.SetActive (false);
        myCurrentPlayer = null;
    }

    public void BeginDragDirection () {
    }

    public void DragDirection () {

        // do raycast
        RaycastHit t_hit;
        Ray t_ray = Camera.main.ScreenPointToRay (Input.mousePosition);

        if (Physics.Raycast (t_ray, out t_hit)) {
            Vector3 t_v2HitPos = new Vector3 (t_hit.point.x, 0, t_hit.point.z);
            Vector3 t_v2PlayerPos = new Vector3 (myCurrentPlayer.transform.position.x, 0, myCurrentPlayer.transform.position.z);
            if (Vector3.Distance (t_v2HitPos, t_v2PlayerPos) > 1) {
                // calculate forward
                Vector3 t_forward = t_v2HitPos - t_v2PlayerPos;
                if (Mathf.Abs (t_forward.x) > Mathf.Abs (t_forward.z)) {
                    t_forward.z = 0;
                } else {
                    t_forward.x = 0;
                }

                // rotate
                myCurrentPlayer.transform.forward = t_forward;
                // show highlight
                myCurrentPlayer.ShowHighlight ();
                return;
            }
        }
        // hide highlight
        myCurrentPlayer.HideHighlight ();
    }

    public void EndDragDirection () {
        // do raycast
        RaycastHit t_hit;
        Ray t_ray = Camera.main.ScreenPointToRay (Input.mousePosition);

        if (Physics.Raycast (t_ray, out t_hit)) {
            Vector3 t_v2HitPos = new Vector3 (t_hit.point.x, 0, t_hit.point.z);
            Vector3 t_v2PlayerPos = new Vector3 (myCurrentPlayer.transform.position.x, 0, myCurrentPlayer.transform.position.z);
            if (Vector3.Distance (t_v2HitPos, t_v2PlayerPos) > 1) {
                // hide highlight
                myCurrentPlayer.HideHighlight ();
                // hide direction 
                myDirectionObject.SetActive (false);
                // pay cost
                reduceCost(myCurrentPlayer.GetDeployCost());
                // init player
                myCurrentPlayer.Init ();
                myCurrentPlayer = null;
                // set slow mode back
                Time.timeScale = 1f;
                return;
            }
        }
    }

    public void OnClickTile (CS_Tile g_tile) {
        if (myCurrentPlayer != null) {
            myCurrentPlayer.transform.position = g_tile.transform.position;
            myCurrentPlayer.gameObject.SetActive (true);
            g_tile.Occupy (myCurrentPlayer);
        }
    }

    public void StopGettingCost()
    {
        BeginGainCost = false;
    }
    public void LoseLife () {
        myCurrentLife--;
        CS_UIManager.Instance.SetLife (myCurrentLife);

        if (myCurrentLife <= 0) {
            CS_UIManager.Instance.ShowPageFail ();
            Time.timeScale = 0;
        }
    }

    public List<CS_Player> GetPlayerList () {
        return myPlayerList;
    }
}
