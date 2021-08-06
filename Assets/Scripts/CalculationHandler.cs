using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CalculationHandler : NetworkBehaviour  
{
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (!isLocalPlayer) {
            rb.isKinematic = true;  // Deactivated
            
        }
    }
}
