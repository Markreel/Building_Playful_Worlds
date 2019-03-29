using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    void Update()
    {
        transform.rotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, 1f, 0));
    }
}
