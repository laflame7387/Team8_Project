using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Transform player;
    public List<GameObject> mapChunks;
    public float chunkLength = 192f;
    public int preloadChunks = 2;

    private Queue<GameObject> activeChunks = new Queue<GameObject>();
    private Vector3 spawnPosition = Vector3.zero;

    void Start()
    {
        for(int i = 0; i<preloadChunks; i++)
        {
            SpawnChunk();
        }
    }

    void Update()
    {
        if(player.position.x > spawnPosition.x - chunkLength * preloadChunks)
        {
            SpawnChunk();
            DeleteOldChunk();
        }
    }

    void SpawnChunk()
    {
        int index = Random.Range(0, mapChunks.Count);
        GameObject chunk = Instantiate(mapChunks[index], spawnPosition, Quaternion.identity);
        activeChunks.Enqueue(chunk);
        spawnPosition.x += chunkLength;
    }

    void DeleteOldChunk()
    {
        if (activeChunks.Count > preloadChunks + 1)
        {
            GameObject oldChunk = activeChunks.Dequeue();
            Destroy(oldChunk);
        }
    }
}
