using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject tile;
    public GameObject winTile;
    [SerializeField] [Range(1,50)] private int levelHeight = 5;
    [SerializeField] [Range(1, 50)] private int levelWidth = 5;
    [SerializeField] [Range(0f, 1f)] private float levelPadding = 0.1f;
    [SerializeField] private int numWinTiles = 3;
    [SerializeField] private int minWinTileDistance = 5;
    [SerializeField] private float powerupSpawnInterval = 20f;

    private static GameObject[,] levelTiles;

    private IEnumerator powerupInterval;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    { 

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

    private void PlacePowerup()
    {
        GameObject powerTile = null;
        while (powerTile == null)
        {
            powerTile = levelTiles[Random.Range(0, levelTiles.GetLength(0) - 1), Random.Range(0, levelTiles.GetLength(1) - 1)];
        }
        powerTile.GetComponent<Tile>().SetPowerup();
    }

    IEnumerator PowerupInterval(float duration)
    {
        while(true)
        {
            yield return new WaitForSeconds(duration);
            PlacePowerup();
        }
    }

    /* --- Public Functions --- */

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

        levelTiles = new GameObject[numRows, numCols];

        Vector3 oldPosition = position;

        for (int i = 0; i < numRows; i++)
        {
            position.z = (i * (tileHeight + padding));

            for (int j = 0; j < numCols; j++)
            {
                position.x = (j * (tileWidth + padding));
                GameObject newTile = Instantiate(tile, position, Quaternion.identity);
                newTile.GetComponent<Tile>().SetPosition(i, j);
                levelTiles[i, j] = newTile;
            }
        }

        if (powerupInterval != null)
            StopCoroutine(powerupInterval);
        powerupInterval = PowerupInterval(powerupSpawnInterval);
        StartCoroutine(powerupInterval);
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
    
    public void RemoveTile(Vector2 pos)
    {
        levelTiles[(int)pos.x, (int)pos.y] = null;
    }
}
