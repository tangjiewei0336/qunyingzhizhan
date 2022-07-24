using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq.Expressions;
using System;

public class CS_GameManager : MonoBehaviour
{
    public GridLayout FlowLayout;

    public GameObject HpBar = null;
    public CS_HPBar HpBarScript = null;

    private static CS_GameManager instance = null;
    public static CS_GameManager Instance { get { return instance; } }

    [SerializeField] int myMaxLife = 10;
    private int myCurrentLife;

    [SerializeField] GameObject[] myPlayerPrefabs = null;

    [SerializeField] GameObject[] myButtonPrefabs = null;

    private List<CS_Player> myPlayerList = new List<CS_Player>();

    [SerializeField] GameObject myDirectionObject = null;

    private CS_Player myCurrentPlayer;
    public CS_Player myCurrentStagePlayer;


    int SpeedMultiplier = 1;

    [Obsolete("请不要直接设定速度，改用SetSpeed(SpeedScale speedScale, SpeedLockBehavior speedLockBehavior = SpeedLockBehavior.DoNothing,int SpeedTo = 0)")]
    public void SetSpeed(int speed)
    {
        Time.timeScale = speed;
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    List<FriendData> FriendAssembly;
    List<PersonalElementsCollection> StageElements = new List<PersonalElementsCollection>();

    public bool DoRayCast = true;

    private void Start()
    {
        // init total lives
        myCurrentLife = myMaxLife;
        CS_UIManager.Instance.SetLife(myCurrentLife);

        FriendAssembly = GameObject.Find("DataStructure").GetComponent<DataStructure>().TransportMessage.StandbyArmour;
        Debug.Log("Selected Friend Unit Detected:" + FriendAssembly.Count);
        int currentSerial = 0;
        foreach (FriendData TempFriendData in FriendAssembly)
        {
            PersonalElementsCollection TempElementInfo = new PersonalElementsCollection();
            int TempDeployCost = 0;
            // init all players
            Debug.Log("Call Player Init. Iteration:" + (currentSerial + 1).ToString());
            foreach (GameObject f_prefab in myPlayerPrefabs)
            {
                if (f_prefab.GetComponent<CS_Player>().CodeName == TempFriendData.Name)
                {
                    GameObject f_object = Instantiate(f_prefab, this.transform);
                    TempDeployCost = f_object.GetComponent<CS_Player>().GetDeployCost();
                    TempElementInfo.Model = f_object;
                    f_object.SetActive(false);
                    // get player script
                    CS_Player f_player = f_object.GetComponent<CS_Player>();
                    // add script to list
                    myPlayerList.Add(f_player);
                }
            }
            Debug.Log("Call Button Init. Iteration:" + (currentSerial + 1).ToString());
            foreach (GameObject f_prefab in myButtonPrefabs)
            {
                if (f_prefab.GetComponent<CS_PlayerButton>().CodeName == TempFriendData.Name)
                {
                    GameObject f_object = Instantiate(f_prefab, this.transform);
                    f_object.transform.SetParent(GameObject.Find("GameCanvas/FriendPanel").transform);
                    f_object.SetActive(true);
                    TempElementInfo.Button = f_object;
                    RectTransform TempPositionInfo = f_object.GetComponent<RectTransform>();
                    TempPositionInfo.gameObject.GetComponent<CS_PlayerButton>().DeployCost = TempDeployCost;
                    TempPositionInfo.localScale = new Vector3(1, 1, 1);
                    TempPositionInfo.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 300 * currentSerial, 300);
                    TempPositionInfo.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, 300);
                }
            }
            StageElements.Add(TempElementInfo);
            Debug.Log("FriendAssembly.Count:" + FriendAssembly.Count);
            currentSerial += 1;
        }


        // init direction object
        myDirectionObject.SetActive(false);
        CostBalance = InitialCostBalance;
        BeginGainCost = true;
        Debug.Log("Game Start");
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

    private bool friendselecting;
    public bool FriendSelecting {get{
            return friendselecting;
        }
        set
        {
            friendselecting = value;
            if (value)
            {
                HpBar.SetActive(true);
                Debug.Log("Transmitting Player:" + myCurrentStagePlayer.CodeName);
            }
            else
            {
                HpBar.SetActive(false);
            }

        }
    }

    void Update()
    {
        foreach (PersonalElementsCollection player in StageElements)
        {
            if (player.Button.GetComponent<CS_PlayerButton>().DeployCost <= CostBalance)
            {
                player.Button.GetComponent<CS_PlayerButton>().UpdateButton(false);
            }
            else
            {
                player.Button.GetComponent<CS_PlayerButton>().UpdateButton(true);
            }
            if (player.Model.activeSelf && player.Model.GetComponent<CS_Player>().GetState() != CS_Player.State.Arrange)
            {
                player.Button.SetActive(false);
            }
            else
            {
                player.Button.SetActive(true);

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

        if(DoRayCast)
        {
            RaycastHit t_hit;
            Ray t_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Input.GetMouseButtonUp(0))
            {
                if (Physics.Raycast(t_ray, out t_hit))
                {
                    Debug.Log("Raycasting...");
                    CS_Player t_player = t_hit.collider.gameObject.transform.parent.GetComponent<CS_Player>();
                    if (t_player != null)
                    {
                        Debug.Log("Object Detected: " + t_player.CodeName);
                        if (t_player.gameObject.activeSelf)
                        {
                            Debug.Log("Reflecting: " + t_player.CodeName);
                            if (!FriendSelecting)
                            {
                                Debug.Log("Selected New Friend Unit: " + t_player.CodeName);
                                myCurrentStagePlayer = t_player;
                                myCurrentStagePlayer.ShowHighlight();
                                SetSpeed(SpeedScale.Slow,SpeedLockBehavior.Lock,0);
                                FriendSelecting = true;

                            }
                            if (FriendSelecting && (myCurrentStagePlayer != t_player))
                            {
                                Debug.Log("Selected Another Friend Unit: " + t_player.CodeName);
                                myCurrentStagePlayer.HideHighlight();
                                myCurrentStagePlayer = t_player;
                                myCurrentStagePlayer.ShowHighlight();
                                SetSpeed(SpeedScale.Slow, SpeedLockBehavior.Lock, 0);
                                FriendSelecting = true;
                            }
                        }
                    }
                    else 
                    {
                        Vector3 MousePos = Input.mousePosition;
                        Debug.Log("X:" + MousePos.x + "  Y:" + MousePos.y);
                        if ((MousePos.x < Screen.width - 270) ||  (MousePos.y <Screen.height -  90))
                        {
                            Debug.Log(t_hit.collider.gameObject.transform.parent.name);
                            UndoFriendSelection();
                        }
                    }
                }
            }
        }
        
    }

    public void UndoFriendSelection()
    {
        if (myCurrentStagePlayer != null)
        {
            myCurrentStagePlayer.HideHighlight();
            FriendSelecting = false;
            SetSpeed(SpeedScale.Normal, SpeedLockBehavior.Unlock, 0);
        }
    }


    bool SpeedLocked = false;

    public void SetSpeed(SpeedScale speedScale, SpeedLockBehavior speedLockBehavior = SpeedLockBehavior.DoNothing,int SpeedTo = 0)
    {
        if (SpeedTo == 1)
        {
            SpeedMultiplier = 1;
        }
        else if (SpeedTo == 2)
        {
            SpeedMultiplier = 2;
        }
        if ((speedLockBehavior == SpeedLockBehavior.Unlock) || (speedLockBehavior == SpeedLockBehavior.Lock) ||
            ((speedLockBehavior == SpeedLockBehavior.DoNothing) && !SpeedLocked))
        {
            switch (speedScale)
            {
                case SpeedScale.Normal:
                    {
                        Time.timeScale = 1 * SpeedMultiplier;
                        break;
                    }
                case SpeedScale.Slow:
                    {
                        Time.timeScale = 0.1f;
                        break;
                    }
                case SpeedScale.Paused:
                    {
                        Time.timeScale = 0f;
                        break;
                    }
            }
        }
        if (speedLockBehavior == SpeedLockBehavior.Unlock)
        {
            SpeedLocked = false;
        }
        if (speedLockBehavior == SpeedLockBehavior.Lock)
        {
            SpeedLocked = true;
        }
    }

    public void SetDeadTimer(string CodeName, float RedeployTime)
    {
        Debug.Log("CS_GameManager : Death Signal Transmitted. Codename: " + CodeName);
        foreach (PersonalElementsCollection personalElement in StageElements)
        {
            Debug.Log("Comparing: " + personalElement.CodeName);
            if (personalElement.CodeName == CodeName)
            {
                personalElement.SetDead(RedeployTime);
            }
        }
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

    public void SetMyCurrentPlayer(string CodeName)
    {
        // dont do anything if its setting direction
        if (myDirectionObject.activeSelf == true)
        {
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


    public void BeginDragPlayer()
    {
        // dont do anything if its setting direction
        if (myDirectionObject.activeSelf == true)
        {
            Debug.Log("myDirectionObject.activeSelf = true");
            return;
        }
        UndoFriendSelection();
        Debug.Log("CostBalance: " + CostBalance);
        Debug.Log("myCurrentPlayer.GetDeployCost: " + myCurrentPlayer.GetDeployCost());

        if (myCurrentPlayer.GetDeployCost() <= CostBalance)
        {
            myCurrentPlayer.gameObject.SetActive(true);
            myCurrentPlayer.Arrange();
            myCurrentPlayer.ShowHighlight();
            DoRayCast = false;
            // set slow mode
            SetSpeed(SpeedScale.Slow, SpeedLockBehavior.Lock, 0);
        }
    }

    public void DragPlayer()
    {
        // dont do anything if its setting direction
        if (myDirectionObject.activeSelf == true)
        {
            return;
        }

        // do raycast
        RaycastHit t_hit;
        Ray t_ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(t_ray, out t_hit))
        {
            CS_Tile t_tile = t_hit.collider.gameObject.GetComponentInParent<CS_Tile>();
            if (t_tile != null)
            {
                if (t_tile.GetType() == myCurrentPlayer.GetTileType())
                {
                    Vector3 t_position = t_tile.transform.position;
                    t_position.y = t_hit.point.y;
                    myCurrentPlayer.transform.position = t_position;
                    return;
                }
            }

            myCurrentPlayer.transform.position = t_hit.point;
        }
    }

    public void EndDragPlayer()
    {
        // dont do anything if its setting direction
        if (myDirectionObject.activeSelf == true)
        {
            return;
        }

        // hide highlight
        myCurrentPlayer.HideHighlight();

        // do raycast
        RaycastHit t_hit;
        Ray t_ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(t_ray, out t_hit))
        {
            CS_Tile t_tile = t_hit.collider.gameObject.GetComponentInParent<CS_Tile>();
            if (t_tile != null)
            {
                if (t_tile.GetType() == myCurrentPlayer.GetTileType())
                {
                    // show direction object
                    myDirectionObject.transform.position = myCurrentPlayer.transform.position;
                    myDirectionObject.SetActive(true);
                    return;
                }
            }
        }

        // reset current player
        myCurrentPlayer.gameObject.SetActive(false);
        myCurrentPlayer = null;
        SetSpeed(SpeedScale.Normal, SpeedLockBehavior.Unlock, 0);
        DoRayCast = true;
    }

    public void BeginDragDirection()
    {
    }

    public void DragDirection()
    {

        // do raycast
        RaycastHit t_hit;
        Ray t_ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(t_ray, out t_hit))
        {
            Vector3 t_v2HitPos = new Vector3(t_hit.point.x, 0, t_hit.point.z);
            Vector3 t_v2PlayerPos = new Vector3(myCurrentPlayer.transform.position.x, 0, myCurrentPlayer.transform.position.z);
            if (Vector3.Distance(t_v2HitPos, t_v2PlayerPos) > 1)
            {
                // calculate forward
                Vector3 t_forward = t_v2HitPos - t_v2PlayerPos;
                if (Mathf.Abs(t_forward.x) > Mathf.Abs(t_forward.z))
                {
                    t_forward.z = 0;
                }
                else
                {
                    t_forward.x = 0;
                }

                // rotate
                myCurrentPlayer.transform.forward = t_forward;
                // show highlight
                myCurrentPlayer.ShowHighlight();
                return;
            }
        }
        // hide highlight
        myCurrentPlayer.HideHighlight();
    }

    public void EndDragDirection()
    {
        // do raycast
        RaycastHit t_hit;
        Ray t_ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(t_ray, out t_hit))
        {
            Vector3 t_v2HitPos = new Vector3(t_hit.point.x, 0, t_hit.point.z);
            Vector3 t_v2PlayerPos = new Vector3(myCurrentPlayer.transform.position.x, 0, myCurrentPlayer.transform.position.z);
            if (Vector3.Distance(t_v2HitPos, t_v2PlayerPos) > 1)
            {
                // hide highlight
                myCurrentPlayer.HideHighlight();
                // hide direction 
                myDirectionObject.SetActive(false);
                // pay cost
                reduceCost(myCurrentPlayer.GetDeployCost());
                // init player
                myCurrentPlayer.Init();
                myCurrentPlayer = null;
                // set slow mode back
                SetSpeed(SpeedScale.Normal, SpeedLockBehavior.Unlock, 0);
                DoRayCast = true;
                return;
            }
        }
    }

    public void OnClickTile(CS_Tile g_tile)
    {
        if (myCurrentPlayer != null)
        {
            myCurrentPlayer.transform.position = g_tile.transform.position;
            myCurrentPlayer.gameObject.SetActive(true);
            g_tile.Occupy(myCurrentPlayer);
        }
    }

    public void StopGettingCost()
    {
        BeginGainCost = false;
    }
    public void LoseLife()
    {
        myCurrentLife--;
        CS_UIManager.Instance.SetLife(myCurrentLife);

        if (myCurrentLife <= 0)
        {
            CS_UIManager.Instance.ShowPageFail();
            SetSpeed(SpeedScale.Paused , SpeedLockBehavior.Lock, 0);
        }
    }

    public List<CS_Player> GetPlayerList()
    {
        return myPlayerList;
    }
}

public class PersonalElementsCollection
{
    public SoldierType soldiertype;
    public GameObject Model
    {
        get
        {
            return model;
        }
        set
        {
            model = value;
            Debug.Log("Processing: " + model.name);
            playerScript = model.GetComponent<CS_Player>();
            soldiertype = playerScript.GetSoldierType();
            codename = playerScript.CodeName;
        }
    }
    public GameObject Button
    {
        get
        {
            return button;
        }
        set
        {
            if ((codename == "") || (value.GetComponent<CS_PlayerButton>().CodeName == codename))
            {
                button = value;
                buttonScript = button.GetComponent<CS_PlayerButton>();
            }
            else
            {
                throw new System.Exception("按钮所指向的干员与模型不一致");
            }
        }
    }
    private GameObject button;
    private GameObject model;
    private CS_PlayerButton buttonScript;
    private CS_Player playerScript;
    private string codename;
    private bool isDead;
    private float CostMultiplier;

    public void SetDead(float RedeployTime)
    {
        buttonScript.RedeployTime = RedeployTime;
    }


    /// <summary>
    /// 此项不能被设置，由模型信息决定。
    /// </summary>
    public string CodeName { get { return codename; } set { } }
}
public enum SoldierType
{
    Attacker = 1,
    Healer = 2
}


public enum SpeedScale
{
    Normal = 1,
    Slow = 3,
    Paused = 4

}

public enum SpeedLockBehavior
{
    DoNothing = 1,
    Lock = 2,
    Unlock = 3,
    Pending = 4

}