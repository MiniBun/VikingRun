using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    public GameObject straightRoad;
    public GameObject rightRoad;
    public GameObject leftRoad;
    public GameObject holeRoad;
    public GameObject coin;
    public bool isGenRoad=false;
    public bool isRemove = false;
    public List<GameObject> roads;
    public List<GameObject> coins;
    public GameObject roadTrace;
    public GameObject coinTrace;
    private float angle=0;
    [SerializeField]
    Vector3 traceMove;
    [SerializeField]
    int times = 0;

    // Start is called before the first frame update
    void Start()
    {
        isGenRoad = false;
        traceMove = new Vector3(0, 0, 1);
        for (int i = 0; i < 15; i++)
        {
            var t = Instantiate(straightRoad);
            t.transform.position = roadTrace.transform.position;
            roads.Add(t);
            roadTrace.transform.position += traceMove * 6;
            coinTrace.transform.position += traceMove * 6;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isGenRoad)
        {
            times++;
            int i = Random.Range(0, 6); // 0~2 straight 3 hole 4 left 5 right
            
            if (times > 8 && i == 3) // hole
            {
                var t = Instantiate(holeRoad);
                t.transform.position = roadTrace.transform.position;
                t.transform.Rotate(0, angle, 0, Space.Self);
                roads.Add(t);
                roadTrace.transform.position += traceMove * 12;
                isGenRoad = false;
                isRemove = true;
                times = 0;
            }
            else if (times>8&&i==4) // left
            {
                angle = (angle - 90) % 360;
                Vector3 temp = traceMove;
                roadTrace.transform.position += traceMove * 3.85f;
                traceMove.x = Mathf.Sin(angle * Mathf.Deg2Rad);
                traceMove.z = Mathf.Cos(angle * Mathf.Deg2Rad);
                roadTrace.transform.position += traceMove * 0.7f;
                var t = Instantiate(leftRoad);
                t.transform.position = roadTrace.transform.position;
                t.transform.Rotate(0, angle + 90, 0, Space.Self);
                roads.Add(t);
                roadTrace.transform.position += temp * 0.65f;
                roadTrace.transform.position += traceMove * 9.75f;
                isGenRoad = false;
                isRemove = true;
                times = 0;
            }
            else if (times > 8 && i == 5) // right
            {
                angle = (angle + 90) % 360;
                Vector3 temp = traceMove;
                roadTrace.transform.position += traceMove * 3.85f;
                traceMove.x = Mathf.Sin(angle * Mathf.Deg2Rad);
                traceMove.z = Mathf.Cos(angle * Mathf.Deg2Rad);
                roadTrace.transform.position += traceMove * 0.7f;
                var t = Instantiate(rightRoad);
                t.transform.position = roadTrace.transform.position;
                t.transform.Rotate(0, angle - 90, 0, Space.Self);
                roads.Add(t);
                roadTrace.transform.position += temp * 0.65f;
                roadTrace.transform.position += traceMove * 9.85f;
                isGenRoad = false;
                isRemove = true;
                times = 0;
            }
            else
            {
                var t = Instantiate(straightRoad);
                t.transform.position = roadTrace.transform.position;
                t.transform.Rotate(0, angle, 0, Space.Self);
                roadTrace.transform.position += traceMove* 6;
                int k = Random.Range(0, 5); //0,1 no coin 2 left 3 middle 4 right
                if (k == 2 || k == 3 || k == 4) // create coin
                {
                    string coin1Pos=null, coin2Pos=null, coin3Pos=null;
                    if(k==2) //left
                    {
                        coin1Pos = "coinLeft1";
                        coin2Pos = "coinLeft2";
                        coin3Pos = "coinLeft3";
                    }
                    if (k == 3) // middle
                    {
                        coin1Pos = "coinMid1";
                        coin2Pos = "coinMid2";
                        coin3Pos = "coinMid3";
                    }
                    if (k == 4) // right
                    {
                        coin1Pos = "coinRight1";
                        coin2Pos = "coinRight2";
                        coin3Pos = "coinRight3";
                    }
                    var coin1 = Instantiate(coin);
                    coin1.transform.parent = t.transform.Find(coin1Pos);
                    coin1.transform.localPosition = Vector3.zero;
                    t.transform.Find(coin1Pos).transform.Rotate(0, angle, 0, Space.Self);
                    var coin2 = Instantiate(coin);
                    coin2.transform.parent = t.transform.Find(coin2Pos);
                    coin2.transform.localPosition = Vector3.zero;
                    t.transform.Find(coin2Pos).transform.Rotate(0, angle, 0, Space.Self);
                    var coin3 = Instantiate(coin);
                    coin3.transform.parent = t.transform.Find(coin3Pos);
                    coin3.transform.localPosition = Vector3.zero;
                    t.transform.Find(coin3Pos).transform.Rotate(0, angle, 0, Space.Self);
                }
                roads.Add(t);
                isGenRoad = false;
                isRemove = true;
            }
        }
        if (isRemove)
        {
            isRemove = false;
            Destroy(roads[0]);
            roads.RemoveAt(0);
        }
    }
    
}
