using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSet : MonoBehaviour
{
    [SerializeField]
    public bool isCall = false;
    // Start is called before the first frame update
    void Start()
    {
        isCall = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static implicit operator TriggerSet(Collider v)
    {
        throw new NotImplementedException();
    }
}
