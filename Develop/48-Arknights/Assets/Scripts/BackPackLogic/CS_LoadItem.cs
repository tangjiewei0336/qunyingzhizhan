using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEditor;

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

    public class ItemDetail
    {
        public int ItemSerial;
        public ItemRarity Rarity;
        private int stock;
        public int Stock
        {
            get
            {
                return stock;
            }
            set
            {
                stock = value;
            }
        }
        public string CodeName;

    }

    List<ItemDetail> StockOverview;

    /// <summary>
    /// 获取玩家是否拥有足够的指定物品
    /// </summary>
    /// <param name="ItemSerial">物品序号</param>
    /// <param name="Consumption">物品的所需数量</param>
    /// <returns></returns>
        public bool StockInquiry(int ItemSerial,int Consumption)
    {
        foreach(ItemDetail tempItem in StockOverview)
        {
            if ((tempItem.ItemSerial == ItemSerial) &&(tempItem.Stock >= Consumption))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 获取玩家拥有的某一物品的数量。
    /// </summary>
    /// <param name="ItemSerial">物品序号</param>
    /// <returns></returns>
    public int StockInspect(int ItemSerial)
    {
        foreach (ItemDetail tempItem in StockOverview)
        {
            if (tempItem.ItemSerial == ItemSerial)
            {
                return tempItem.Stock;
            }
        }
        return 0;
    }

    public bool UseItem(int ItemSerial, int Consumption, bool AllowDebt = false)
    {
        foreach (ItemDetail tempItem in StockOverview)
        {
            if ((tempItem.ItemSerial == ItemSerial) && ((tempItem.Stock >= Consumption) || AllowDebt))
            {
                tempItem.Stock -=  Consumption;
                if (tempItem.Stock == 0)
                {
                    StockOverview.Remove(tempItem);
                }
                return true;
            }
        }
        return false;
    }

    private enum RefreshMode
    {
        Addition = 1,
        Modification = 2
    }

    /// <summary>
    /// 刷新局部背包物品，仅可在背包场景下使用。
    /// </summary>
    /// <param name="refreshMode"></param>
    private void PartialRefresh(RefreshMode refreshMode) { 


    }


    public bool GainItem(int ItemSerial, int Addition)
    {
        foreach (ItemDetail tempItem in StockOverview)
        {
            if (tempItem.ItemSerial == ItemSerial)
            {
                tempItem.Stock += Addition ;
                return true;
            }
        }
        ItemDetail TempItem = new ItemDetail();
        TempItem.ItemSerial = ItemSerial;
        TempItem.Stock = Addition;
        StockOverview.Add(TempItem);
        return true;
    }

    public Texture ReturnTextureByRarity(int ItemSerial)
    {
        switch (Mid(ItemSerial.ToString(), 1, 1))
        {
            case "4":
                {
                    foreach (Texture UndertoneResource in RawUndertoneImages)
                    {
                        if (UndertoneResource.name == "普通稀有度")
                        {
                            return UndertoneResource;
                        }
                    }

                    return null;
                }
            case "3":
                {
                    foreach (Texture UndertoneResource in RawUndertoneImages)
                    {
                        if (UndertoneResource.name == "稀有稀有度")
                        {
                            return UndertoneResource;
                        }
                    }
                    return null;
                }
            case "2":
                {
                    foreach (Texture UndertoneResource in RawUndertoneImages)
                    {
                        if (UndertoneResource.name == "史诗稀有度")
                        {
                            return UndertoneResource;
                        }
                    }
                    return null;
                }
            case "1":
                {
                    foreach (Texture UndertoneResource in RawUndertoneImages)
                    {
                        if (UndertoneResource.name == "传奇稀有度")
                        {
                            return UndertoneResource;
                        }
                    }
                    return null;
                }
            default:
                {
                    throw new System.Exception("找不到指定的物品底纹。");
                }
        }
    }


    /// <summary>
    /// 可以重新写入文件到一个已存在的txt文本中去
    /// </summary>
    void ReWriteMyTxtByFileStreamTxt()
    {
        string path = AssetDatabase.GetAssetPath(InventoryDetail) + "/背包道具.txt";
        string[] strs = new string[StockOverview.Count * 2-1];
        for(int i = 0; i < StockOverview.Count; i++)
        {
            strs[2 * i] = StockOverview[i].ItemSerial.ToString();
            strs[2 * i + 1] = StockOverview[i].Stock.ToString();
        }
        File.WriteAllLines(path, strs);
    }



    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
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
                    TextMeshProUGUI Digits = TempObject.transform.Find("Digits").GetComponent<TextMeshProUGUI>();
                    Digits.enableAutoSizing = true;
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

                TempUnderToneImage.texture = ReturnTextureByRarity(TempItem.ItemSerial);
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
