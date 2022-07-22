using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;

public class CS_PlayerButton : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{

    [SerializeField] int myIndex;

    public Text DeployCostDisplayComp;
    public string CodeName;
    public int DeployCost;
    public Canvas Parent;
    public Image Filters;
    public float RedeployTime = 0f;
    public GameObject Timer;

    void Start()
    {
    }

    void Update()
    {

    }

    public void UpdateButton(bool WithinMeans)
    {
        if (RedeployTime > 0f)
        {
            Filters.color = new Color(255 / 255f, 0, 0, 78 / 255f);
            Filters.gameObject.SetActive(true);
            RedeployTime -= Time.deltaTime;
            Timer.SetActive(true);
            Timer.GetComponent<MeshFilter>().mesh = CreateMesh(30);
        }
        else
        {
            RedeployTime = 0f;
            if (WithinMeans)
            {
                Filters.color = new Color(0, 0, 0, 78/255f);
                Filters.gameObject.SetActive(true);
                Timer.SetActive(false);
            }
            else
            {
                Filters.color = new Color(0, 0, 0, 78 / 255f);
                Filters.gameObject.SetActive(false);
                Timer.SetActive(false);

            }

        }
    }

    public Material TimerMaterial;
    //分区
    private int segments = 50;
    //半径
    private int radius = 5;
    //内半径
    private int interRadius = 3;

    private Mesh CreateMesh(float degree)
    {
        Mesh mesh = new Mesh();
        //因为要形成闭环，多加两个顶点，方便计算三角形，跟索引0,1的顶点是重合的
        int vertexLen = segments * 2 + 2;
        //把这个度数改成其他度数，就可以绘制成环形扇面了
        float angle = degree * Mathf.Deg2Rad;
        float curAngle = angle / 2;
        float deltaAngle = angle / segments;

        //形成圆环，可以绘制一圈等腰梯形。等腰梯形，可以由两个三角形组成。
        Vector3[] vertex = new Vector3[vertexLen];
        for (int i = 0; i < vertexLen; i += 2)
        {
            float cos = Mathf.Cos(curAngle);
            float sin = Mathf.Sin(curAngle);
            vertex[i] = new Vector3(cos * interRadius, 0, sin * interRadius);
            vertex[i + 1] = new Vector3(cos * radius, 0, sin * radius);
            //为了绘制圆形，当前的角度不断递减
            curAngle -= deltaAngle;
        }
        //填充三角形
        //三角形的顶点索引总数，三角形数目 * 3，下面相当于segments * 2 * 3，因为每个segment有两个三角形
        int tri_VertexIndexCount = segments * 6;
        int[] tri = new int[tri_VertexIndexCount];
        for (int i = 0, j = 0; i < tri_VertexIndexCount; i += 6, j += 2)
        {
            tri[i] = j;
            tri[i + 1] = j + 1;
            tri[i + 2] = j + 3;
            tri[i + 3] = j + 3;
            tri[i + 4] = j + 2;
            tri[i + 5] = j;
        }

        //根据顶点位置，设置UV纹理坐标
        Vector2[] uv = new Vector2[vertexLen];
        for (int i = 0; i < vertexLen; i++)
        {
            uv[i] = new Vector2(vertex[i].x / radius / 2 + 0.5f, vertex[i].z / radius / 2 + 0.5f);
        }

        mesh.vertices = vertex;
        mesh.triangles = tri;
        mesh.uv = uv;
        mesh.name = "Sphere";
        Debug.Log("Created Polygon Mesh.");
        return mesh;

    }




    public void setCost(int cost)
    {
        DeployCostDisplayComp.text = cost.ToString();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        CS_GameManager.Instance.BeginDragPlayer();
        Debug.Log("OnBeginDrag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        CS_GameManager.Instance.DragPlayer();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        CS_GameManager.Instance.EndDragPlayer();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        CS_GameManager.Instance.SetMyCurrentPlayer(CodeName);
        Debug.Log("OnPointerDown");
    }

}
