using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    private void OnCollisionEnter(Collision other) 
    {
        if (other.gameObject.layer ==  3)
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
