using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGen : MonoBehaviour
{
    public int gridWidth = 10;
    public int gridHeight = 10;
    public float tileSize = 1.0f;
    public GameObject tilePrefab;
    public GameObject enemyPrefab;

    private Tile[,] grid;
    private bool[] unlockedLanes; // Array to track unlocked lanes


    public float spawnInterval = 1.0f; // Time interval between each shot
    public float projectileSpeed = 10.0f; // Speed of the projectile

    private float nextAttackTime;

    void Start()
    {
        CreateGrid();
        InitializeLanes();
        nextAttackTime = Time.time;


    }

    private void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            SpawnEnemy();
            nextAttackTime = Time.time + spawnInterval;
        }
    }

    void CreateGrid()
    {
        grid = new Tile[gridWidth, gridHeight];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                CreateTile(x, y);
            }
        }
    }

    void CreateTile(int x, int y)
    {
        Vector3 localTilePosition = new Vector3(x * tileSize, 0, y * tileSize);
        Vector3 worldTilePosition = transform.TransformPoint(localTilePosition);
        GameObject tileObject = Instantiate(tilePrefab, worldTilePosition, transform.rotation, transform);
        tileObject.name = $"Tile_{x}_{y}";

        tileObject.transform.localScale = new Vector3(tileSize, tileObject.transform.localScale.y, tileSize);
        Tile tile = tileObject.GetComponent<Tile>();
        grid[x, y] = tile;
    }

    void InitializeLanes()
    {
        unlockedLanes = new bool[gridHeight];
        // Initially unlock the first lane for demonstration purposes
        unlockedLanes[0] = true;
        unlockedLanes[1] = true;
        unlockedLanes[4] = true;

        unlockedLanes[2] = false;
        unlockedLanes[3] = false;
        unlockedLanes[5] = false;

        //// Set other lanes to locked (false)
        //for (int i = 1; i < unlockedLanes.Length; i++)
        //{
        //    unlockedLanes[i] = false;
        //}
    }

    public void UnlockLane(int lane)
    {
        if (lane >= 0 && lane < gridHeight)
        {
            unlockedLanes[lane] = true;
        }
    }

    void SpawnEnemy()
    {
        List<int> availableLanes = new List<int>();

        for (int i = 0; i < unlockedLanes.Length; i++)
        {
            if (unlockedLanes[i])
            {
                availableLanes.Add(i);
            }
        }

        if (availableLanes.Count > 0)
        {
            int randomIndex = Random.Range(0, availableLanes.Count);
            int randomY = availableLanes[randomIndex];
            Vector3 spawnPosition = new Vector3(gridWidth * tileSize, 0, randomY * tileSize); // Spawn outside the grid on the negative X-axis
            Vector3 worldSpawnPosition = transform.TransformPoint(spawnPosition);

            GameObject enemy = Instantiate(enemyPrefab, worldSpawnPosition, Quaternion.identity);
            enemy.transform.rotation = Quaternion.Euler(0, -90, 0); // Assuming 90 degrees Y rotation to face the X-axis

            //enemy.GetComponent<GridEnemy>().Init(grid, tileSize, gridWidth, gridHeight, transform); // Pass the parent transform
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 offset = transform.position;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 tilePosition = offset + new Vector3(x * tileSize, 0, y * tileSize);
                Gizmos.DrawWireCube(tilePosition, new Vector3(tileSize, 0.1f, tileSize));
            }
        }
    }
}
