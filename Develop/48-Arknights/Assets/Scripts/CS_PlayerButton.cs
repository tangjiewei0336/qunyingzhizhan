using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CS_PlayerButton : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler {

    [SerializeField] int myIndex;

    public Text DeployCostDisplayComp;
    public string CodeName;
    public Canvas Parent;
    public void setCost(int cost)
    {
        DeployCostDisplayComp.text = cost.ToString();
    }

    public void OnBeginDrag (PointerEventData eventData) {
        CS_GameManager.Instance.BeginDragPlayer ();
        Debug.Log("OnBeginDrag");
    }

    public void OnDrag (PointerEventData eventData) {
        CS_GameManager.Instance.DragPlayer ();
    }

    public void OnEndDrag (PointerEventData eventData) {
        CS_GameManager.Instance.EndDragPlayer ();
    }

    public void OnPointerDown (PointerEventData eventData) {
        CS_GameManager.Instance.SetMyCurrentPlayer (myIndex);
        Debug.Log("OnPointerDown");
    }

}
