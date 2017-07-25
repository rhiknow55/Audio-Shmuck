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

	Color[] colours = new Color[]
	{
		Color.black,
		Color.blue,
		Color.cyan,
		Color.gray,
		Color.green,
		Color.magenta,
		Color.red,
		Color.white,
		Color.yellow,
		Color.grey
	};

	GameObject tile;
	// Floor. HAS to be a square.
	float floorTileScaleYFactor;
	Transform thisTransform;

	Vector3 rotateAxis;
	float rotateAngle;

    private GameObject[,] floorTiles;
	int maxNumOfCircles;

    void Start()
	{
		// By declaring the floor transform here once, we prevent the need to call this.transform (it uses a lot of performance)
		thisTransform = this.GetComponent<Transform>();

		InitFloorVisualizer();
		//CircularSetup();
		FullRandomSetup();

		RotateVisualizers();
	}

	public void Setup(Vector3 _axis, float _angle)
	{
		rotateAxis = _axis;
		rotateAngle = _angle;
	}

	public void RotateVisualizers()
	{
		// Columns
		for (int col = 0; col < floorTiles.GetLength(0); col++)
		{

			// Rows
			for (int row = 0; row < floorTiles.GetLength(1); row++)
			{
				floorTiles[col, row].transform.RotateAround(this.transform.localPosition, rotateAxis, rotateAngle);
			}

		}
	}

	public void InitFloorVisualizer()
	{
		tile = GlobalManager.instance.GetTileGO();

		floorTileScaleYFactor = 5f;

		// Set the number of compiler subbads to the max number of circles possible
		maxNumOfCircles = Mathf.FloorToInt(thisTransform.localScale.x / 2);
		//AbstractAudioCompiler.numberOfSubbands = maxNumOfCircles;

		InitFloorTilesArray();
	}

	// calls the Setup method for all the floor tiles in the multidimensional array
	// Center 4 floor tiles will be at freq band 0 and pulsate outwards.
	// So the number of subbands in the AudioCompiler will be dependant on how many square-circles can be formed on the floor.
	void CircularSetup()
	{
		// Sets up the center 4 floor tiles
		int[] centerIndexes = new int[]
		{
			// Top left. In (subbands - 1) column, and (subband - 1) row.
			(maxNumOfCircles - 1)*maxNumOfCircles*2 + (maxNumOfCircles - 1),
			// Bottom left.  In (subbands - 1) column, and (subband) row.
			(maxNumOfCircles - 1)*maxNumOfCircles*2 + (maxNumOfCircles),
			// Top right. In (subbands) column, and (subband - 1) row.
			(maxNumOfCircles)*maxNumOfCircles*2 + (maxNumOfCircles - 1),
			// Bottom right. In (subbands) column, and (subband) row.
			(maxNumOfCircles)*maxNumOfCircles*2 + (maxNumOfCircles)
		};

		// If the col index is 0, your freqband is 0. At 24, your freq band is 24. At 25, your freq band is 24 and at col index 49, your freqband is 0.

		// Top left only
		SetUpArrayForCircular(0, floorTiles.GetLength(0) / 2, 0, floorTiles.GetLength(1) / 2, 0, 0);
		// Top right only
		SetUpArrayForCircular(floorTiles.GetLength(0) / 2, floorTiles.GetLength(0), 0, floorTiles.GetLength(1) / 2, floorTiles.GetLength(0), 0);
		// Bottom left only
		SetUpArrayForCircular(0, floorTiles.GetLength(0) / 2, floorTiles.GetLength(1) / 2, floorTiles.GetLength(1), 0, floorTiles.GetLength(1));
		// Bottom right only
		SetUpArrayForCircular(floorTiles.GetLength(0) / 2, floorTiles.GetLength(0), floorTiles.GetLength(1) / 2, floorTiles.GetLength(1), floorTiles.GetLength(0), floorTiles.GetLength(1));
	}

	// Sets up each quadrant of the array floor tiles.
	void SetUpArrayForCircular(int _colStart, int _colEnd, int _rowStart, int _rowEnd, int _colSubtractor, int _rowSubtractor)
	{
		for (int col = _colStart; col < _colEnd; col++)
		{
			for (int row = _rowStart; row < _rowEnd; row++)
			{
				int x;
				if (col < maxNumOfCircles) x = Mathf.Abs(col - _colSubtractor);
				else x = Mathf.Abs(col - _colSubtractor + 1);

				int y;
				if (row < maxNumOfCircles) y = Mathf.Abs(row - _rowSubtractor);
				else y = Mathf.Abs(row - _rowSubtractor + 1);

				if (x < y)
				{
					floorTiles[col, row].GetComponent<FloorTile>().Setup(x, floorTileScaleYFactor);
					floorTiles[col, row].GetComponent<FloorTile>().SetColor(colours[x]);
				}
				else
				{
					floorTiles[col, row].GetComponent<FloorTile>().Setup(y, floorTileScaleYFactor);
					floorTiles[col, row].GetComponent<FloorTile>().SetColor(colours[y]);
				}
			}
		}
	}

	// Set up the tiles to have fully random frequency bands.
	public void FullRandomSetup()
	{
		// Columns
		for (int col = 0; col < floorTiles.GetLength(0); col++)
		{

			// Rows
			for (int row = 0; row < floorTiles.GetLength(1); row++)
			{
				int randFreqBand = Random.Range(0, AudioCompiler.freqSubbandsInstant.Length);

				floorTiles[col, row].GetComponent<FloorTile>().Setup(randFreqBand, floorTileScaleYFactor);
			}

		}
	}



	// Initialise the floorTiles multidimensional array.
	// Assume the scale of the floor is divisible integer
	void InitFloorTilesArray()
	{
		int z = Mathf.FloorToInt(thisTransform.localScale.z / tile.transform.localScale.z);
		int x = Mathf.FloorToInt(thisTransform.localScale.x / tile.transform.localScale.x);
        

        floorTiles = new GameObject[z, x];


        InitFloorTiles();
    }

    // Instantiate the actual tile GOs accordingly.
    void InitFloorTiles()
    {
        // Create a main container GO for holding ALL floor tile clones
        GameObject floorTilesContainer = new GameObject("FLOORTILES");
		floorTilesContainer.transform.parent = thisTransform;

        // The scales of the tile GO
        float tileZScale = tile.transform.localScale.z;
        float tileYScale = tile.transform.localScale.y;
        float tileXScale = tile.transform.localScale.x;

        // Getting the position vector of the the origin point on the floor GO, and therefore the 0,0 of the floorTiles array.
        Vector3 floorPos = thisTransform.localPosition;
		Quaternion floorRot = thisTransform.localRotation;
        float xTopLeft = floorPos.x - thisTransform.localScale.x / 2;
        float yTopLeft = floorPos.y + thisTransform.localScale.y / 2;
        float zTopLeft = floorPos.z + thisTransform.localScale.z / 2;
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
                GameObject obj = Instantiate(tile, tilePos, floorRot, colContainer.transform);
				// Set the GO name for differentiation
                obj.name = "FloorTile " + (col * floorTiles.GetLength(0) + row);
				// Add the FloorTile script to every floor tile
				obj.AddComponent<FloorTile>();

				floorTiles[col, row] = obj;
            }

        }

    }
}
