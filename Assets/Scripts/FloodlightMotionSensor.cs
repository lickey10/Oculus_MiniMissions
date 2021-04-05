using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloodlightMotionSensor : MonoBehaviour
{
    public Light light;
    public int LightDelay = 1;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag(DoneTags.player);
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the triggering gameobject is the player...
        if (other.transform.root.gameObject == player || other.transform.root.gameObject.name.ToLower().Contains("guard"))
        {
            CancelInvoke("turnLightOff");

            light.gameObject.SetActive(true);

            Invoke("turnLightOff", LightDelay);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // If the triggering gameobject is the player...
        if (other.transform.root.gameObject == player || other.transform.root.gameObject.name.ToLower().Contains("guard"))
        {
            CancelInvoke("turnLightOff");

            light.gameObject.SetActive(true);

            Invoke("turnLightOff", LightDelay);
        }
    }

    private void turnLightOff()
    {
        light.gameObject.SetActive(false);
    }
}
