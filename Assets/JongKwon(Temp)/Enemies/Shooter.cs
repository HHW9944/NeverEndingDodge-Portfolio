using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField]
    GameObject missilePrefab;

    void Start()
    {
        Instantiate(missilePrefab, transform.position, transform.rotation);
    }

    void Update()
    {
        
    }
}
