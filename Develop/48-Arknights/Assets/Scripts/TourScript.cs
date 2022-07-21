using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TourScript : MonoBehaviour
{
    Transform StandingPoint;

    public int ForthMoveSpeed = 8;
    public int HorizontalMoveSpeed = 80;

    // Start is called before the first frame update
    void Start()
    {
        StandingPoint = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            StandingPoint.Translate(0, 0, ForthMoveSpeed * Time.deltaTime);
            if ((StandingPoint.position.z > 20) || (StandingPoint.position.z < -27) || (StandingPoint.position.x < -33) || (StandingPoint.position.x > -11))
            {
                StandingPoint.Translate(0, 0, -ForthMoveSpeed * Time.deltaTime);
            }
        }
        if (Input.GetKey(KeyCode.S))
        {
            StandingPoint.Translate(0, 0, -ForthMoveSpeed * Time.deltaTime);

            if ((StandingPoint.position.z > 20) || (StandingPoint.position.z < -27) || (StandingPoint.position.x < -33) || (StandingPoint.position.x > -11))
            {
                StandingPoint.Translate(0, 0, ForthMoveSpeed * Time.deltaTime);
            }
        }
        if (Input.GetKey(KeyCode.A))
        {
            StandingPoint.eulerAngles = new Vector3(0, StandingPoint.eulerAngles.y - HorizontalMoveSpeed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            StandingPoint.eulerAngles = new Vector3(0, StandingPoint.eulerAngles.y + HorizontalMoveSpeed * Time.deltaTime, 0);
        }
    }
}
