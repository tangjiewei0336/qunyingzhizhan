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
    public Texture[] RawUndertoneImages;

    public GameObject ItemPrefab;

    public string[] inventorydetail;
    public string[] itemdescription;


    public enum ItemRarity
    {
        Normal = 1,
        Rare = 2,
        Epic = 3,
        Legendary = 4,
        Special = 5,
        None = 99
    }

    public struct ItemDetail
    {
        public int ItemSerial;
        public ItemRarity Rarity;
        public int Stock;
        public string CodeName;
    }

    List<ItemDetail> StockOverview;

    // Start is called before the first frame update
    void Start()
    {
        StockOverview = new List<ItemDetail>();
        inventorydetail = ReadProf(InventoryDetail);
        itemdescription = ReadProf(ItemDescription);

        for (int i = 0; i < inventorydetail.Length / 2; i++)
        {
            ItemDetail TempItem = new ItemDetail
            {
                ItemSerial = int.Parse(inventorydetail[2 * i]),
                Stock = int.Parse(inventorydetail[2 * i + 1])
            };

            switch (Mid(TempItem.ItemSerial.ToString(), 1, 1))
            {
                case "1":
                    {
                        TempItem.Rarity = ItemRarity.Legendary;
                        break;
                    }
                case "2":
                    {
                        TempItem.Rarity = ItemRarity.Epic;
                        break;
                    }
                case "3":
                    {
                        TempItem.Rarity = ItemRarity.Rare;
                        break;
                    }
                case "4":
                    {
                        TempItem.Rarity = ItemRarity.Normal;
                        break;
                    }

            }

            StockOverview.Add(TempItem);

            Debug.Log(TempItem.ItemSerial);
            GameObject TempObject = Instantiate(ItemPrefab);

            foreach (Texture ImageResource in RawImages)
            {
                Debug.Log(Mid(ImageResource.name, 1, 4));
                if (Mid(ImageResource.name, 1, 4) == TempItem.ItemSerial.ToString())
                {
                    TempObject.transform.SetParent(GameObject.Find("Bag").transform);
                    RawImage TempImage = TempObject.transform.Find("ItemFabric").GetComponent<RawImage>();
                    TempImage.texture = ImageResource;
                    Text Digits = TempObject.transform.Find("Digits").GetComponent<Text>();
                    if (TempItem.Stock < 10000)
                    {
                        Digits.text = TempItem.Stock.ToString();
                    }
                    else
                    {
                        Digits.text = (TempItem.Stock / 1000f).ToString("f1") + "万";
                    }             
                }

                RawImage TempUnderToneImage = TempObject.transform.GetComponent<RawImage>();
                switch (TempItem.Rarity)
                {
                    case ItemRarity.Normal:
                        {
                            foreach (Texture UndertoneResource in RawUndertoneImages)
                            {
                                if (UndertoneResource.name == "普通稀有度")
                                {
                                    TempUnderToneImage.texture = UndertoneResource;
                                }
                            }
                            break;
                        }
                    case ItemRarity.Rare:
                        {
                            foreach (Texture UndertoneResource in RawUndertoneImages)
                            {
                                if (UndertoneResource.name == "稀有稀有度")
                                {
                                    TempUnderToneImage.texture = UndertoneResource;
                                }
                            }
                            break;
                        }
                    case ItemRarity.Epic:
                        {
                            foreach (Texture UndertoneResource in RawUndertoneImages)
                            {
                                if (UndertoneResource.name == "史诗稀有度")
                                {
                                    TempUnderToneImage.texture = UndertoneResource;
                                }
                            }
                            break;
                        }
                    case ItemRarity.Legendary:
                        {
                            foreach (Texture UndertoneResource in RawUndertoneImages)
                            {
                                if (UndertoneResource.name == "传奇稀有度")
                                {
                                    TempUnderToneImage.texture = UndertoneResource;
                                }
                            }
                            break;
                        }
                }

            }
        }
    }

    /// <summary>
    /// 截取中间字符
    /// </summary>
    /// <param name="sSource"></param>
    /// <param name="iStart"></param>
    /// <param name="iLength"></param>
    /// <returns></returns>
    public static string Mid(string sSource, int iStart, int iLength)
    {
        int iStartPoint = iStart > sSource.Length ? sSource.Length : iStart;
        return sSource.Substring(iStartPoint - 1, iStartPoint + iLength > sSource.Length ? sSource.Length - iStartPoint : iLength);
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
