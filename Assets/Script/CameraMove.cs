using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    //跟隨的目標
    public Transform target;

    //方向向量
    private Vector3 dir;
    private void Start()
    {
        //計算攝像機指向玩家的方向偏移量
        dir = target.position - transform.position;
    }
    private void Update()
    {
        //時時刻刻計算攝像機的跟隨位置
        Vector3 bastPos = target.position - dir;
        transform.position = bastPos;
    }
}