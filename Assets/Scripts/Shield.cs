using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public PlayerScript player;
    public Transform playerTrans;
    Transform _cachedTrans;
    public Vector3 offset = new Vector3(0, 0, -2);

    void Start()
    {
        _cachedTrans = transform;
    }

    // Update is called once per frame
    void Update()
    {

    }
    void LateUpdate()
    {
        _cachedTrans.position = new Vector3
            (playerTrans.position.x + offset.x, playerTrans.position.y + offset.y, playerTrans.position.z + offset.z);

    }

  
    void OnTriggerEnter(Collider other)
    {
        // Check for enemy
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Hit enemy");
            AudioManager.Instance.PlaySoundEffect("Impact");
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
