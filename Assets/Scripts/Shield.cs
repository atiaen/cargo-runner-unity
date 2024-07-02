using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        // Check for enemy
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Hit enemy");
            Destroy(other.gameObject);
            GameManager.OnEnemyDestroyed?.Invoke();
            GameManager.OnShieldHit?.Invoke();
        }
        //else if (other.CompareTag("Pickup"))
        //{
        //    Destroy(other.gameObject);
        //}
    }
}
