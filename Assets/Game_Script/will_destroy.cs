using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class will_destroy : MonoBehaviour
{
    public float sec;
    void Start()
    {
        Destroy(gameObject, sec);
    }

}
