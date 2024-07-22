using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BotTurret : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Transform> projectileSpawnPoints;

    public GameObject projectile;

    public BotType botType;

    public Vector3 raycastOriginOffset = Vector3.zero; // Offset for the Raycast origin
    public float rayDistance = 10.0f; // Distance to cast the rays
    public LayerMask layerMask; // Layer mask to filter the objects to collide with
    public float raycastAngle = 0f; // Angle at which the Raycast is performed
    public int numberOfRays = 3; // Number of rays to cast
    void Start()
    {
        
    }


    private void Update()
    {
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

            // Visualize the Raycast in the Scene view
            //DrawRaycast(origin, direction, rayDistance, isHit, hit);

            // Check if the Raycast hit anything
            if (isHit)
            {
                Debug.Log("Raycast hit: " + hit.collider.name);
                for(int j =0; j < projectileSpawnPoints.Count; j++)
                {
                    var pos = projectileSpawnPoints[j];
                    var gb = Instantiate(projectile, pos.position, Quaternion.identity);
                }
            }
        }
    }

    public void ShootBullet()
    {

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
        if (!Application.isPlaying)
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
}
