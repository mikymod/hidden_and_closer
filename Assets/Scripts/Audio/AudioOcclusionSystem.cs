using System.Collections.Generic;
using UnityEngine;

public class AudioOcclusionSystem : MonoBehaviour
{
    public LayerMask obstacleMask;
 
    private void Update()
    {
        float distanceToTarget = Vector3.Distance(transform.position, GameObject.Find("Player(Clone)").transform.position);
        if (distanceToTarget <= 16)
        {
            Vector3 dirToTarget = (GameObject.Find("Player(Clone)").transform.position - transform.position).normalized;
            Debug.DrawRay(transform.position, dirToTarget * 16);
            if (!Physics.Raycast(transform.position, dirToTarget, distanceToTarget, obstacleMask))
            {
                //Debug.Log("playerHit");
            }
        }
    }
}
