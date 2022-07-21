using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class DataStructure : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public FriendGroup TransportMessage;

} 

/// <summary>
/// 存储上阵友方信息
/// </summary>
public struct FriendGroup
{
    public List<FriendData> StandbyArmour; 
}

/// <summary>
/// 单条友方信息
/// </summary>
public struct FriendData
{
    public string Name;
    public int Level;
}