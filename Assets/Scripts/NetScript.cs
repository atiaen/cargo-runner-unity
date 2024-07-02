using System;
using UnityEngine;

public class NetScript : MonoBehaviour
{
    public GameManager gameManager;
    void Start()
    {
        gameManager = GameManager.Instance;
        // Here you can add code that needs to be called when script is created, just before the first game update
    }

    void OnEnable()
    {
        // Register for event
        var collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }
    }

    void OnDisable()
    {
        // Unregister for event
    }

    void OnTriggerEnter(Collider other)
    {
        // Check for enemy
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            GameManager.OnEnemyDestroyed?.Invoke();
        }else if (other.CompareTag("Pickup"))
        {
            Destroy(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check for enemy
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            GameManager.OnEnemyDestroyed?.Invoke();
        }else if (other.CompareTag("Pickup"))
        {
            Destroy(other.gameObject);
        }
    }

    void Update()
    {
        // Here you can add code that needs to be called every frame
    }
}
