using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CS_LoadItem : MonoBehaviour
{
    public TextAsset InventoryDetail;
    public TextAsset ItemDescription;

    public Texture[] RawImages;

    public GameObject ItemPrefab;

    public string[] inventorydetail;
    public string[] itemdescription;

    // Start is called before the first frame update
    void Start()
    {
        inventorydetail = ReadProf(InventoryDetail);
        itemdescription = ReadProf(ItemDescription);
        for(int i = 0; i < inventorydetail.Length / 3; i++){
            foreach(string ItemCode in inventorydetail)
            {
                if (ItemCode == itemdescription[3*i])
                {
                    foreach(Texture ImageResource in RawImages)
                    {
                        if(ImageResource.name == )
                    }
                }
            }
            GameObject TempItem = Instantiate(ItemPrefab);
            TempItem.transform.Find("ItemFabric")


        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private string[] ReadProf(TextAsset target)
    {
        string text = target.text;
        string[] textArray = text.Split('\n');
        return textArray;
    }

}
