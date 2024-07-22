using TMPro;
using UnityEngine;

public class GridEnemy : MonoBehaviour
{

    public Vector3 boxSize = new Vector3(1, 1, 1); // Size of the box
    public Vector3 addedDirection = new Vector3(1,1,1); // Direction to cast the box
    public float distance = 5.0f; // Distance to cast the box
    public LayerMask layerMask; // Layer mask to filter the objects to collide with

    public float moveSpeed = 2.0f;
    public float attackDamage = 0.2f;

    public float health = 2.0f;

    public float attackInterval = 1.0f; // Time interval between each shot

    private float nextAttackTime;

    public TMP_Text healthText;

    private void Start()
    {
        nextAttackTime = Time.time;
        string formattedValue = health.ToString("F1");

        healthText.text = formattedValue;

    }

    void Update()
    {
        string formattedValue = health.ToString("F1");

        healthText.text = formattedValue;

        if (health <= 0)
        {
            Destroy(gameObject);
        }

        // The starting point of the BoxCast
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward + addedDirection;

        // Perform the BoxCast
        RaycastHit hit;
        bool isHit = Physics.BoxCast(origin, boxSize / 2, direction, out hit, transform.rotation, distance, layerMask);


        // Check if the BoxCast hit anything
        if (isHit)
        {
            Debug.Log("BoxCast hit: " + hit.collider.name);
            if (Time.time >= nextAttackTime)
            {
                var other = hit.collider.gameObject.GetComponent<BotTurret>();
                other.botType.health -= attackDamage;

                nextAttackTime = Time.time + attackInterval;
            }
        }
        else
        {
            Vector3 newPosition = transform.position + Vector3.left * moveSpeed * Time.deltaTime; // Move along negative X-axis
            transform.position = newPosition;
        }

    

    }

    void AttackObstacle(Tile tile)
    {
        // Implement your attack logic here
        Debug.Log("Attacking obstacle at " + tile.transform.position);
        //Destroy(gameObject); // Destroy the enemy after attacking
    }


    void OnDrawGizmos()
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward + addedDirection;

        // Calculate the end position of the BoxCast
        Vector3 endPos = origin + direction * distance;

        // Draw the box at the origin position
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(origin, boxSize);

        // Draw the box at the end position
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(endPos, boxSize);

        // Draw a line between the origin and end position
        Gizmos.color = Color.green;
        Gizmos.DrawLine(origin, endPos);


    }
}
