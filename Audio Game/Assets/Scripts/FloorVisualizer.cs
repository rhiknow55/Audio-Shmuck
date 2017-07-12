using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// @author Ryan Oh
/// 
/// Instantiates floor tiles.
/// Floor tiles move up and down to the music
/// Floor tiles differ in assigned frequency according to how close to center it is (in a circle)
/// </summary>
public class FloorVisualizer : MonoBehaviour {

    GameObject tile;
    GameObject floor;

    private GameObject[,] floorTiles;

    void Start()
	{
        floor = GlobalManager.instance.GetFloorGO();
        tile = GlobalManager.instance.GetTileGO();

        InitFloorTilesArray();
    }

	// Initialise the floorTiles multidimensional array.
	// Assume the scale of the floor is divisible integer
	void InitFloorTilesArray()
	{
		int z = Mathf.FloorToInt(floor.transform.localScale.z / tile.transform.localScale.z);
		int x = Mathf.FloorToInt(floor.transform.localScale.x / tile.transform.localScale.x);
        

        floorTiles = new GameObject[z, x];


        InitFloorTiles();
    }

	// Instantiate the actual tile GOs accordingly.
    void InitFloorTiles()
    {
        // Create a main container GO for holding ALL floor tile clones
        GameObject floorTilesContainer = new GameObject("FLOORTILES");

        // The scales of the tile GO
        float tileZScale = tile.transform.localScale.z;
        float tileYScale = tile.transform.localScale.y;
        float tileXScale = tile.transform.localScale.x;

        // Getting the position vector of the the origin point on the floor GO, and therefore the 0,0 of the floorTiles array.
        Vector3 floorPos = floor.transform.position;
        float xTopLeft = floorPos.x - floor.transform.localScale.x / 2;
        float yTopLeft = floorPos.y + floor.transform.localScale.y / 2;
        float zTopLeft = floorPos.z + floor.transform.localScale.z / 2;
        Vector3 floorTopLeft = new Vector3(xTopLeft, yTopLeft, zTopLeft);

        // the y pos of the tiles is constant, so declare it outside the loops (more efficient!)
        float tileY = floorTopLeft.y + tile.transform.localScale.y / 2;

        // Columns
        for (int col = 0; col < floorTiles.GetLength(0); col++)
        {
            // Creates a separate container GO for each column
            GameObject colContainer = new GameObject("Column " + col);
            // Sets this column container's parent to be the main floorTilesContainer GO
            colContainer.transform.parent = floorTilesContainer.transform;

			// Rows
            for (int row = 0; row < floorTiles.GetLength(1); row++)
            {
                // The center of the first tile on the top left is a little bit right and below the actual top left pos.
                float tileX = floorTopLeft.x + (tileXScale / 2) + (col * tileXScale);
                float tileZ = floorTopLeft.z - (tileZScale / 2) - (row * tileZScale);

                // Declare the changing position vector for every tile
                Vector3 tilePos = new Vector3(tileX, tileY, tileZ);
                floorTiles[col, row] = Instantiate(tile, tilePos, Quaternion.identity, colContainer.transform);
                floorTiles[col, row].name = "FloorTile " + (col * floorTiles.GetLength(0) + row);
			}

        }

    }
}
