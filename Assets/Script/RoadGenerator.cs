using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    public GameObject straightRoad;
    public GameObject rightRoad;
    public GameObject leftRoad;
    public GameObject target;
    public bool isGenRoad=false;
    public bool isRemove = false;
    public List<GameObject> roads;
    public GameObject roadTrace;
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
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isGenRoad)
        {
            times++;
            if (times % 10 != 0)
            {
                var t = Instantiate(straightRoad);
                t.transform.position = roadTrace.transform.position;
                t.transform.Rotate(0, angle, 0, Space.Self);
                roads.Add(t);
                roadTrace.transform.position += traceMove* 6;
                isGenRoad = false;
                isRemove = true;
            }
            else // right
            {
                angle = (angle + 90) % 360;
                Vector3 temp = traceMove;
                roadTrace.transform.position += traceMove * 3.85f;
                traceMove.x = Mathf.Sin(angle * Mathf.Deg2Rad);
                traceMove.z = Mathf.Cos(angle * Mathf.Deg2Rad);
                roadTrace.transform.position += traceMove * 0.7f;
                var t = Instantiate(rightRoad);
                t.transform.position = roadTrace.transform.position;
                t.transform.Rotate(0, angle-90, 0, Space.Self);
                roads.Add(t);
                roadTrace.transform.position += temp * 0.65f;
                roadTrace.transform.position += traceMove * 9.85f;
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
