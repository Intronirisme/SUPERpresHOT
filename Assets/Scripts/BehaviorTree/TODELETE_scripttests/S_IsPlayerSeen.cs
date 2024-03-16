using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class S_IsPlayerSeen : MonoBehaviour
{
    GameObject player;
    RaycastHit hit;
    public NavMeshAgent navAgent;
    void Start()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, 5f);
        foreach(Collider collider in colliders)
        {
            if (collider.gameObject.tag == "Player")
            {
                Debug.Log("Player in range");
                player = collider.gameObject;
                IsLineOfSightClear(player);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IsLineOfSightClear(GameObject player)
    {
        Debug.DrawRay(transform.position, (player.transform.position - transform.position), Color.green, 9999999f);
        if(Physics.Raycast(transform.position, (player.transform.position - transform.position), out hit))
        {
            if(hit.collider.gameObject.tag == "Player")
            {
                Debug.Log("Player in Line Of Sight");
                MoveToPlayer(player);
            }
            else
            {
                Debug.Log("Player not in Line Of Sight");
            }
        }
    }

    public void MoveToPlayer(GameObject player)
    {
        navAgent.destination = player.transform.position;
        //transform.LookAt(player.transform);
    }
}
