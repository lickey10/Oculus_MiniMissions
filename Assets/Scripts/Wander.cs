using UnityEngine;
using System.Collections;
 
public class Wander : MonoBehaviour {
 
    public float wanderRadius;
    public float wanderTimer;
    public float pauseBeforeMoveMin = 0;
    public float pauseBeforeMoveMax = 0;

    private Transform target;
    private UnityEngine.AI.NavMeshAgent agent;
    private float timer;
    private float pauseTime = 0;
 
    // Use this for initialization
    void OnEnable () {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent> ();
        timer = wanderTimer;
    }
 
    // Update is called once per frame
    void Update () {
        timer += Time.deltaTime;
 
        if (timer >= wanderTimer) {

            if (pauseTime <= 0)
            {
                Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                agent.SetDestination(newPos);
                timer = 0;
                pauseTime = Random.Range(pauseBeforeMoveMin, pauseBeforeMoveMax);
            }
            else
                pauseTime -= Time.deltaTime;
        }
    }
 
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask) {
        Vector3 randDirection = Random.insideUnitSphere * dist;
 
        randDirection += origin;
 
        UnityEngine.AI.NavMeshHit navHit;
 
        UnityEngine.AI.NavMesh.SamplePosition (randDirection, out navHit, dist, layermask);
 
        return navHit.position;
    }
}