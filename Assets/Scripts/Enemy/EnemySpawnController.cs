using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using Random = UnityEngine.Random;

/// <summary>
/// Responsible for spawning and destroying of enemies
/// </summary>
public class EnemySpawnController : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private NavMeshSurface[] navMeshSurfaces;
    [SerializeField] private NavMeshSurface currentSurface;

    [SerializeField] private List<BotBehaviour> enemies;


    private void Awake()
    {
        EventsBus.Subscribe<OnRemoveAllEnemies>(OnRemoveAllEnemies);
        EventsBus.Subscribe<OnEnemySpawn>(OnEnemySpawn);
        EventsBus.Subscribe<OnEnemyDeath>(OnEnemyDeath);

        if (currentSurface == null)
        {
            currentSurface = navMeshSurfaces[0];
        }

    }

    private void OnDestroy()
    {
        EventsBus.Unsubscribe<OnRemoveAllEnemies>(OnRemoveAllEnemies);
        EventsBus.Unsubscribe<OnEnemySpawn>(OnEnemySpawn);
        EventsBus.Unsubscribe<OnEnemyDeath>(OnEnemyDeath);
    }

    /// <summary>
    /// ¬озвращает случайную точку на NavMesh поверхности
    /// </summary>
    /// <returns></returns>
    private Vector3 GetRandomSpawnablePoint(NavMeshSurface surface)
    {
        NavMeshHit hit;
        Vector3 randomPoint;

        do
        {
            randomPoint = surface.transform.position + Random.insideUnitSphere * surface.size.magnitude * 0.5f;
        }
        while (!NavMesh.SamplePosition(randomPoint, out hit, surface.size.magnitude * 0.1f, NavMesh.AllAreas));

        return hit.position;
    }

    private void CheckEnemyNumber()
    {
        if (enemies.Count % 10 == 0)
        {
            string newText = $"{enemies.Count} enemies here";

            EventsBus.Publish(new OnRefreshUIText() { className = "EnemyUIWindow", text = newText });
        }
    }

    #region SUBSCRIPTIONS
    private void OnRemoveAllEnemies(OnRemoveAllEnemies eventData)
    {
        if (enemies.Count > 0)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                Destroy(enemies[i].gameObject);
            }
            enemies.Clear();
        }
        else
        {
            Debug.Log("No enemies to remove!");
        }
    }

    [ContextMenu("SpawnEnemy")]
    private void OnEnemySpawn(OnEnemySpawn eventData)
    {
        Vector3 position = GetRandomSpawnablePoint(currentSurface);

        GameObject prefab = gameData.GetPrefabByName(eventData.prefabName);

        GameObject spawnedObject = Instantiate(prefab, position, Quaternion.identity);

        BotBehaviour behaviour = spawnedObject.GetComponent<BotBehaviour>();
        if (behaviour != null)
            enemies.Add(behaviour);

        CheckEnemyNumber();
    }

    private void OnEnemyDeath(OnEnemyDeath eventData)
    {
        if (enemies.Count > 0)
            enemies.Remove(eventData.enemy);
    }
    #endregion
}
