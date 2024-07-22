using UnityEngine;

public class GridEnemy : MonoBehaviour
{
    private Tile[,] grid;
    private float tileSize;
    private int gridWidth;
    private int gridHeight;
    private Transform gridTransform;

    public Vector3 boxSize = new Vector3(1, 1, 1); // Size of the box
    public Vector3 addedDirection = new Vector3(1,1,1); // Direction to cast the box
    public float distance = 5.0f; // Distance to cast the box
    public LayerMask layerMask; // Layer mask to filter the objects to collide with

    public float moveSpeed = 2.0f;

    public void Init(Tile[,] grid, float tileSize, int gridWidth, int gridHeight, Transform gridTransform)
    {
        this.grid = grid;
        this.tileSize = tileSize;
        this.gridWidth = gridWidth;
        this.gridHeight = gridHeight;
        this.gridTransform = gridTransform;
    }

    void Update()
    {

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
        }
        else
        {
            Vector3 newPosition = transform.position + Vector3.left * moveSpeed * Time.deltaTime; // Move along negative X-axis
            transform.position = newPosition;
        }

    

        //MoveAlongXAxis();
    }

    void MoveAlongXAxis()
    {
        Vector3 newPosition = transform.position + Vector3.left * moveSpeed * Time.deltaTime; // Move along negative X-axis
        Vector3 localPosition = gridTransform.InverseTransformPoint(newPosition);

        // Check if the new position is within the grid bounds
        int x = Mathf.FloorToInt(localPosition.x / tileSize);
        int y = Mathf.FloorToInt(localPosition.z / tileSize);

        if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
        {
            // Check for obstacles
            if (grid[x, y] != null && grid[x, y].isOccupied)
            {
                AttackObstacle(grid[x, y]);
            }
            else
            {
                transform.position = newPosition;
            }
        }
        else if (x < -0.5) // Check if the enemy has moved beyond the negative end of the grid
        {
            Destroy(gameObject); // Destroy the enemy when it reaches the end of the grid
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
