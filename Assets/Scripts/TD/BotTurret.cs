using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BotTurret : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Transform> projectileSpawnPoints;

    public GameObject projectile;
    public TMP_Text healthText;

    public BotType botType;

    public Vector3 raycastOriginOffset = Vector3.zero; // Offset for the Raycast origin
    public float rayDistance = 10.0f; // Distance to cast the rays
    public LayerMask layerMask; // Layer mask to filter the objects to collide with
    public float raycastAngle = 0f; // Angle at which the Raycast is performed
    public int numberOfRays = 3; // Number of rays to cast

    public float shootingInterval = 1.0f; // Time interval between each shot
    public float projectileSpeed = 10.0f; // Speed of the projectile

    private float nextShotTime;

    void Start()
    {
        nextShotTime = Time.time;

        string formattedValue = botType.health.ToString("F1");

        healthText.text = formattedValue;
    }


    private void Update()
    {
        string formattedValue = botType.health.ToString("F1");

        healthText.text = formattedValue;

        // The starting point of the Raycasts
        Vector3 origin = transform.position + raycastOriginOffset;
        
        // Perform the Raycasts
        for (int i = 0; i < numberOfRays; i++)
        {
            // Calculate the direction of each Raycast
            float angleStep = (numberOfRays > 1) ? (raycastAngle / (numberOfRays - 1)) : 0;
            float angle = -raycastAngle / 2 + i * angleStep;
            Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward;

            // Perform the Raycast
            RaycastHit hit;
            bool isHit = Physics.Raycast(origin, direction, out hit, rayDistance, layerMask);

            // Check if the Raycast hit anything
            if (isHit)
            {
                Debug.Log("Raycast hit: " + hit.collider.name);
                ShootBullet();
            }
        }
    }

    public void ShootBullet()
    {

        if (Time.time >= nextShotTime)
        {
            for (int j = 0; j < projectileSpawnPoints.Count; j++)
            {

                var pos = projectileSpawnPoints[j];
                var p = Instantiate(projectile, pos.position, Quaternion.identity);

                // Apply velocity to the projectile
                Rigidbody rb = p.GetComponent<Rigidbody>();
                var bulletSpeed = p.GetComponent<Bullet>().bulletSpeed;

                if (rb != null)
                {
                    rb.velocity = pos.TransformDirection(-pos.forward * bulletSpeed);
                }

            }

            nextShotTime = Time.time + shootingInterval;

        }
        

    }

    private void DrawRaycast(Vector3 origin, Vector3 direction, float distance, bool isHit, RaycastHit hit)
    {
        // Calculate the end position of the Raycast
        Vector3 endPos = origin + direction * distance;

        // Draw the Raycast line
        Gizmos.color = isHit ? Color.red : Color.green;
        Gizmos.DrawLine(origin, isHit ? hit.point : endPos);

        // Draw a sphere at the hit point if there is a hit
        if (isHit)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(hit.point, 0.1f);
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 origin = transform.position + raycastOriginOffset;

        for (int i = 0; i < numberOfRays; i++)
        {
            float angleStep = (numberOfRays > 1) ? (raycastAngle / (numberOfRays - 1)) : 0;
            float angle = -raycastAngle / 2 + i * angleStep;
            Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward;
            DrawRaycast(origin, direction, rayDistance, false, new RaycastHit());
        }
    }
}
