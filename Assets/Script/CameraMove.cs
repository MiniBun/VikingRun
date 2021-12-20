using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    //���H���ؼ�
    public Transform target;

    //��V�V�q
    private Vector3 dir;
    private void Start()
    {
        //�p���ṳ�����V���a����V�����q
        dir = target.position - transform.position;
    }
    private void Update()
    {
        //�ɮɨ��p���ṳ�������H��m
        Vector3 bastPos = target.position - dir;
        transform.position = bastPos;
    }
}