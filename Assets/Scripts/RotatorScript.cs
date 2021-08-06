using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RotatorScript : NetworkBehaviour
{
    [SerializeField] float speed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, speed, 0);
    }
}
