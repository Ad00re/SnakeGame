using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class body : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 currentPosition;
    private Vector3 nextPosition;
    
    private Vector3 direction;
    void Start()
    {
        currentPosition = transform.position;
        nextPosition = currentPosition + 0.5f * direction;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
