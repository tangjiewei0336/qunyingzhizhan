using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CS_EnemyManager : MonoBehaviour
{

    private static CS_EnemyManager instance = null;
    public static CS_EnemyManager Instance { get { return instance; } }
    public int OperatingArray;

    [SerializeField] EnemyArray[] EnemyArraies;
    [SerializeField] float myPathRandomness = 0.2f;
    private int myEnemyCount = -1;
    private int TotalEnemyCount = 0;
    private List<CS_Enemy> myEnemyList = new List<CS_Enemy>();
    public CS_RouteSave MyRoute;

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

    private void Start()
    {

        for (int i = 0; i < EnemyArraies.Length; i++)
        {
            EnemyArraies[i].myEnemySpawnIndex = 0;
            EnemyArraies[i].LocalTimeScale = EnemyArraies[i].SingularEnemy[0].MyDeployInterval + EnemyArraies[i].TimeOffset;
            TotalEnemyCount += EnemyArraies[i].SingularEnemy.Count;
        }

        // init count
        myEnemyCount = 0;
        CS_UIManager.Instance.SetCount(myEnemyCount, TotalEnemyCount);
    }

    private void Update()
    {
        Update_Timer();
    }

    private void Update_Timer()
    {

        for (int i = 0; i < EnemyArraies.Length; i++)
        {

            if (EnemyArraies[i].LocalTimeScale < 0)
            {
                continue;
            }

            // update timer
            EnemyArraies[i].LocalTimeScale -= Time.deltaTime;

            if (EnemyArraies[i].LocalTimeScale > 0)
            {
                continue;
            }

            // generate enemy
            GameObject t_enemyObject = Instantiate(EnemyArraies[i].SingularEnemy[EnemyArraies[i].myEnemySpawnIndex].My_Enemy_Prefab);
            t_enemyObject.SetActive(false);
            // get enemy script
            CS_Enemy t_enemy = t_enemyObject.GetComponent<CS_Enemy>();
            t_enemy.MySummonInfo = EnemyArraies[i].SingularEnemy[EnemyArraies[i].myEnemySpawnIndex];
            // init enemy
            t_enemy.Init();
            // add enemy to list
            myEnemyList.Add(t_enemy);

            // stop spawn process if all listed are finished
            EnemyArraies[i].myEnemySpawnIndex++;
            if (EnemyArraies[i].myEnemySpawnIndex >= EnemyArraies[i].SingularEnemy.Count)
            {
                EnemyArraies[i].LocalTimeScale = -1;
                return;
            }
            // set timer
            EnemyArraies[i].LocalTimeScale = EnemyArraies[i].SingularEnemy[EnemyArraies[i].myEnemySpawnIndex].MyDeployInterval;
        }




    }

    public List<Vector3> GetPath(StageEnemies TempEnemy)
    {
        List<Vector3> t_pathList = new List<Vector3>();
        foreach (Transform t_transform in MyRoute.RouteCollection[TempEnemy.MyRouteIndex].myPathTransformArray)
        {
            // create some randomness for the path
            Vector3 f_position = t_transform.position;
            f_position.x = Random.Range(-myPathRandomness, myPathRandomness) + f_position.x;
            f_position.z = Random.Range(-myPathRandomness, myPathRandomness) + f_position.z;
            t_pathList.Add(f_position);
        }
        return t_pathList;
    }

    public void LoseEnemy(CS_Enemy g_enemy)
    {
        // update enemy count
        myEnemyCount++;
        CS_UIManager.Instance.SetCount(myEnemyCount, TotalEnemyCount);

        if (myEnemyCount == TotalEnemyCount)
        {
            CS_UIManager.Instance.ShowPageEnd();
            CS_GameManager.Instance.StopGettingCost();
        }

        // remove enemy from list
        myEnemyList.Remove(g_enemy);
    }

    public List<CS_Enemy> GetEnemyList()
    {
        return myEnemyList;
    }
    //public void InsertRoute()
    //{
    //    EnemyArraies[OperatingArray].SingularEnemy.Insert(EnemyArraies[OperatingArray].OperatingItem, new StageEnemies());
    //}

    //public void CloneRoute()
    //{
    //    EnemyArraies[OperatingArray].SingularEnemy.Add(EnemyArraies[OperatingArray].SingularEnemy[EnemyArraies[OperatingArray].OperatingItem]);
    //}

    //public void DeleteRoute()
    //{
    //    EnemyArraies[OperatingArray].SingularEnemy.RemoveAt(EnemyArraies[OperatingArray].OperatingItem);
    //}

}

[System.Serializable]
public struct StageEnemies
{
    public GameObject My_Enemy_Prefab;
    public float MyDeployInterval;
    public int MyRouteIndex;
}

[System.Serializable]
public struct EnemyArray
{
    public int myEnemySpawnIndex;
    public float TimeOffset;
    public float LocalTimeScale;
    public List<StageEnemies> SingularEnemy;
    public int OperatingItem;
}
