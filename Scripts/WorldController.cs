using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldController : MonoBehaviour
{
    [Header("Chunk Settings")]
    [SerializeField] private Vector2Int chunkSize = new(16, 16);
    [SerializeField] private int initialRadius = 1; // how many chunks to generate in each direction from (0,0)
    [SerializeField] private Vector2Int trunksPerChunk = new(3, 7);

    [Header("Tile References")]
    [SerializeField] private TileBase floorTile;
    [SerializeField] private TileBase trunkTile;
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tilemap obstacleTilemap;
    [SerializeField] private Transform player;
    private Vector2Int currentPlayerChunk;
    [SerializeField] private Vector2Int chunkCoordinates;

    private HashSet<Vector2Int> loadedChunks = new();

    void Start()
    {
        GenerateInitialChunks();
    }

    void GenerateInitialChunks()
    {
        for (int x = -initialRadius; x <= initialRadius; x++)
        {
            for (int y = -initialRadius; y <= initialRadius; y++)
            {
                Vector2Int chunkCoord = new(x, y);
                LoadChunk(chunkCoord);
            }
        }
    }

    void LoadChunk(Vector2Int chunkCoord)
    {
        if (loadedChunks.Contains(chunkCoord)) return;
        loadedChunks.Add(chunkCoord);

        Vector3Int chunkOffset = new(chunkCoord.x * chunkSize.x, chunkCoord.y * chunkSize.y, 0);

        // Generate floor tiles
        List<Vector3Int> floorPositions = new(chunkSize.x * chunkSize.y);
        List<TileBase> floorTiles = new(chunkSize.x * chunkSize.y);

        for (int x = 0; x < chunkSize.x; x++)
        {
            for (int y = 0; y < chunkSize.y; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0) + chunkOffset;
                floorPositions.Add(pos);
                floorTiles.Add(floorTile);
            }
        }
        groundTilemap.SetTiles(floorPositions.ToArray(), floorTiles.ToArray());

        // Randomly place obstacles
        int trunkCount = Random.Range(trunksPerChunk.x, trunksPerChunk.y + 1);
        HashSet<Vector3Int> usedPositions = new();

        for (int i = 0; i < trunkCount; i++)
        {
            Vector3Int pos;
            int failsafe = 100;
            do
            {
                int rx = Random.Range(0, chunkSize.x);
                int ry = Random.Range(0, chunkSize.y);
                pos = new Vector3Int(rx, ry, 0) + chunkOffset;
                failsafe--;
            }
            while (usedPositions.Contains(pos) && failsafe > 0);

            if (failsafe <= 0) break;
            usedPositions.Add(pos);
            obstacleTilemap.SetTile(pos, trunkTile);
        }
    }
    void Update()
    {
        Vector2Int playerChunk = GetChunkCoord(player.position);

        if (playerChunk != currentPlayerChunk)
        {
            currentPlayerChunk = playerChunk;
            LoadSurroundingChunks(playerChunk);
        }
    }
    Vector2Int GetChunkCoord(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x / chunkSize.x);
        int y = Mathf.FloorToInt(position.y / chunkSize.y);
        return new Vector2Int(x, y);
    }
    void LoadSurroundingChunks(Vector2Int center)
    {
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                Vector2Int chunk = new(center.x + dx, center.y + dy);
                LoadChunk(chunk);
            }
        }
    }
}
