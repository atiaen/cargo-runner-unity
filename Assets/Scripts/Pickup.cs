using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public int pickupType;
    public int lowerValue;
    public int upperValue;
    public float destroyDelay = 5f; // Time in seconds before destroying the object

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyAfterDelay());

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.PlaySoundEffect("Pickup");

            if(pickupType == 0)
            {
                var currentShields = GameManager.Instance.heldShields;
                var maxShields = GameManager.Instance.maxShields;

                if (currentShields < maxShields)
                {
                    GameManager.Instance.heldShields++;
                    GameManager.Instance.heldShields = Mathf.Clamp(GameManager.Instance.heldShields, 0, maxShields);
                    
                }
            }

            if(pickupType == 1)
            {
                GameManager.Instance.IncreaseScoreWithValues(lowerValue, upperValue);
            }

            Destroy(gameObject);
        }

        if (other.CompareTag("Net"))
        {
            Destroy(gameObject);
        }

    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }

}
