using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public int pickupType;
    public int lowerValue;
    public int upperValue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {

            if(pickupType == 0)
            {
                Debug.Log("I've hit the player and I'm a shield");
               bool autoShieldsEnabled = GameManager.Instance.autoShieldsEnabled;
                if (autoShieldsEnabled)
                {

                }

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
}
