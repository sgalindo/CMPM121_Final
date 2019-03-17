using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LevelGenerator : MonoBehaviour
{
    public GameObject tile;
    public GameObject winTile;
    [SerializeField] [Range(1,50)] int levelHeight = 5;
    [SerializeField] [Range(1, 50)] int levelWidth = 5;
    [SerializeField] [Range(0f, 1f)] float levelPadding = 0.1f;
    [SerializeField] private int numWinTiles = 3;
    [SerializeField] private int minWinTileDistance = 5;

    public bool generate = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (generate)
        {
            DestroyTiles();
            GenerateLevel(Vector3.zero, levelHeight, levelWidth, levelPadding);
            generate = false;
        }
    }

    // Destroy all tiles in level (regular tile and win tile)
    private void DestroyTiles()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        for (int i = 0; i < tiles.Length; i++)
        {
            DestroyImmediate(tiles[i]);
        }

        GameObject[] winTiles = GameObject.FindGameObjectsWithTag("WinTile");
        for (int i = 0; i < winTiles.Length; i++)
        {
            DestroyImmediate(winTiles[i]);
        }
    }

    // Instantiates a grid of tiles.
    // position: Starting position of the first tile in the grid.
    // numRows: Number of rows in the grid.
    // numCols: Number of columns in the grid.
    // padding: Space between each tile.
    /// <summary>
    /// Instantiates a grid of tiles, along with its win tiles.
    /// </summary>
    /// <param name="position">Starting position of the first tile in the grid.</param>
    /// <param name="numRows">Number of rows in the grid.</param>
    /// <param name="numCols">Number of columns in the grid.</param>
    /// <param name="padding">Space between each tile.</param></param>
    public void GenerateLevel(Vector3 position, int numRows, int numCols, float padding)
    {
        // Dimensions of the tile prefab to space them correctly.
        float tileHeight = tile.GetComponent<Renderer>().bounds.size.z;
        float tileWidth = tile.GetComponent<Renderer>().bounds.size.x;

        Vector3 oldPosition = position;

        for (int i = 0; i < numRows; i++)
        {
            position.z = (i * (tileHeight + padding));

            for (int j = 0; j < numCols; j++)
            {
                position.x = (j * (tileWidth + padding));
                Instantiate(tile, position, Quaternion.identity);
            }
        }
    }

    // Instantiates a grid of tiles with randomly scattered win tiles.
    // position: Starting position of the first tile in the grid.
    // numRows: Number of rows in the grid.
    // numCols: Number of columns in the grid.
    // padding: Space between each tile.
    // numWinTiles: Number of win tiles to place on grid.
    // minWinTileDistance: Minimum distance between win tiles (in grid coordinates)
    /// <summary>
    /// Instantiates a grid of tiles with randomly scattered win tiles.
    /// </summary>
    /// <param name="position">Starting position of the first tile in the grid.</param>
    /// <param name="numRows">Number of rows in the grid.</param>
    /// <param name="numCols">Number of columns in the grid.</param>
    /// <param name="padding">Space between each tile.</param>
    /// <param name="numWinTiles">Number of win tiles to place on grid.</param>
    /// <param name="minWinTileDistance">Minimum distance between win tiles (in grid coordinates)</param>
    public void GenerateLevelWithWinTiles(Vector3 position, int numRows, int numCols, float padding, int numWinTiles, int minWinTileDistance)
    {
        // Dimensions of the tile prefab to space them correctly.
        float tileHeight = tile.GetComponent<Renderer>().bounds.size.z;
        float tileWidth = tile.GetComponent<Renderer>().bounds.size.x;

        int winTileCount = 0;

        List<Vector2> winTilePos =  new List<Vector2>();

        Vector3 oldPosition = position;

        for (int i = 0; i < numRows; i++)
        {
            position.z = (i * (tileHeight + padding));

            for (int j = 0; j < numCols; j++)
            {
                position.x = (j * (tileWidth + padding));
                if (Random.Range(0f, 1f) < 0.2f && CheckDistance(winTilePos, new Vector2(j, i), minWinTileDistance) && winTileCount < numWinTiles)
                {
                    Instantiate(winTile, position, Quaternion.identity);
                    winTilePos.Add(new Vector2(j, i));
                    winTileCount++;
                }
                else
                {
                    Instantiate(tile, position, Quaternion.identity);
                }
            }
        }
    }

    // Checks the distance between current tile and all other win tiles
    private bool CheckDistance(List<Vector2> winTilePos, Vector2 curr, int minDistance)
    {
        for (int i = 0; i < winTilePos.Count; i++)
        {
            if (Mathf.Abs(curr.x - winTilePos[i].x) < minDistance || Mathf.Abs(curr.y - winTilePos[i].y) < minDistance)
            {
                return false;
            }
        }
        return true;
    }
}
