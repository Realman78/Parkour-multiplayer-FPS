using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    const float tau = Mathf.PI * 2f;
    [SerializeField] float period = 2f;
    [SerializeField] Vector3 movementVector;
    [Range(0, 1)] [SerializeField] float movementFactor;
    Vector3 startingPos;
    // Start is called before the first frame update
    void Start() {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update() {
        if (period >= Mathf.Epsilon) {
            float cycles = Time.time / period;
            float rawSinWave = Mathf.Sin(cycles * tau);
            movementFactor = rawSinWave / 2f + 0.5f;
            Vector3 offset = movementVector * movementFactor;
            transform.position = startingPos + offset;
        }

    }
}
