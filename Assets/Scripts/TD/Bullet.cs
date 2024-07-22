using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed;
    public float bulletDamage = 0.5f;

    public float destroyDelay = 4f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyAfterDelay());

    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 newPosition = -transform.position + Vector3.left * bulletSpeed * Time.deltaTime;
        //transform.position = newPosition;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Enemy"))
        {
            //AudioManager.Instance.PlaySoundEffect("Pickup");
            other.gameObject.GetComponent<GridEnemy>().health -= bulletDamage;
            Destroy(gameObject);
            Debug.Log("Hit enemy");

        }

    }


    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}
