using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] itemPrefabs; 
    [SerializeField] private float spawnInterval = 5f; 
    public Terrain terrain; 
    private bool itemSpawnActive; 

    private void Start()
    {
        StartCoroutine(SpawnItems());
    }

    private IEnumerator SpawnItems()
    {
        while (true)
        {
            SpawnItem();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnItem()
    {
 
        GameObject itemPrefab = itemPrefabs[Random.Range(0, itemPrefabs.Length)];
        Vector3 spawnPosition = GetRandomPosition();

        if (!IsPositionOccupied(spawnPosition))
        {
            Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
        }
    }

    private Vector3 GetRandomPosition()
    {
        float terrainWidth = terrain.terrainData.size.x;
        float terrainHeight = terrain.terrainData.size.z;
        float spawnX = Random.Range(0, terrainWidth);
        float spawnZ = Random.Range(0, terrainHeight);
        float spawnY = terrain.SampleHeight(new Vector3(spawnX, 0, spawnZ)) + 1.0f;

        return new Vector3(spawnX, spawnY, spawnZ);
    }

    private bool IsPositionOccupied(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, 1f);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("Wall")) 
            {
                return true;
            }
        }
        return false;
    }
    public void SetItemSpawnActive(bool value)
    {
        itemSpawnActive = value;
    }
}
