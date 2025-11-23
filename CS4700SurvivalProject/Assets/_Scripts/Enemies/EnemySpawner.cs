using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EnemySpawner : NetworkBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnRadius = 1f, spawnTime = 1f;
    private float timer;
    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            enabled = false;
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnTime)
        {
            timer = 0;
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position + new Vector3(Random.Range(0f, spawnRadius), Random.Range(0f, spawnRadius)), Quaternion.identity);
        enemy.GetComponent<NetworkObject>().Spawn(true);
    }
}
