using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaBall : MonoBehaviour
{
    Transform particleTarget;
    particleAttractorLinear particleSource;

    // Start is called before the first frame update
    void Start()
    {
        particleSource = GetComponentInChildren<particleAttractorLinear>();
        particleTarget = particleSource.target;

        particleSource.target = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        particleSource.target = particleTarget;
        particleTarget.position = other.transform.position;
    }

    private void OnTriggerStay(Collider other)
    {
        particleTarget.position = other.transform.position;
    }

    private void OnTriggerExit(Collider other)
    {
        particleSource.target = null;
    }
}
