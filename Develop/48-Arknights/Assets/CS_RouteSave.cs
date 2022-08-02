using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class CS_RouteSave : MonoBehaviour
{
    public List<NavigationGuide> RouteCollection;

    public int OperatingRoute;
   
    // Start is called before the first frame update
    void Start()
    {
   

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InsertRoute()
    {
        RouteCollection.Insert(OperatingRoute, new NavigationGuide());
    }

    public void CloneRoute() {
        RouteCollection.Add(RouteCollection[OperatingRoute]);
    }

    public void DeleteRoute()
    {
        RouteCollection.RemoveAt(OperatingRoute);
    }

}


[System.Serializable]
public struct NavigationGuide
{
    public List<Transform> myPathTransformArray;
    public int OperatingItem;
}
