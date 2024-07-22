using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;


public class GameMaster : MonoBehaviour
{
    public float tileSize = 1.0f;
    public float placementHeight = 0.05f;

    public GameObject selectedUnit; 
    public GameObject highlightPrefab; 

    GameObject spawnHighlightPrefab;

    //private Tile[,] grid;

    void Start()
    {
        spawnHighlightPrefab = Instantiate(highlightPrefab, new Vector3(0,0,0), transform.rotation);
        spawnHighlightPrefab.transform.localScale = new Vector3(tileSize,2f,tileSize);
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject.CompareTag("Tile"))
            {

                Tile clickedTile = hit.transform.GetComponent<Tile>();
                if (clickedTile != null && !clickedTile.isOccupied)
                {
                    spawnHighlightPrefab.transform.position = hit.transform.position;
                    if (Input.GetButtonDown("Fire1")) // Left mouse button clicked
                    {
                        //Debug.Log("Clicked");
                        PlaceUnit(clickedTile);
                    }
                    
                }
               
            }
           
           
        }
      
    }

    void PlaceUnit(Tile tile)
    {
        Vector3 unitPosition = tile.transform.position + new Vector3(0, placementHeight, 0); // Adjust the height if needed
        var gb = Instantiate(selectedUnit, unitPosition, Quaternion.identity);
        gb.transform.rotation = Quaternion.Euler(0, 90, 0); // Assuming 90 degrees Y rotation to face the X-axis

        tile.isOccupied = true;
    }

 
}
