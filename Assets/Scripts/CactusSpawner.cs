using UnityEngine;

public class CactusSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Terrain terrain;
    [SerializeField] private GameObject cactusPrefab;

    [Header("Settings")]
    [SerializeField] private int cactusCount = 200;
    [SerializeField] private float minScale = 8f;
    [SerializeField] private float maxScale = 12f;
    [SerializeField] private float maxSlope = 30f;

    [Header("Organization")]
    [SerializeField] private Transform parentForSpawned;

    private void Start()
    {
        SpawnCacti();
    }

    private void SpawnCacti()
    {
        if (terrain == null || cactusPrefab == null)
        {
            Debug.LogError("CactusSpawner: Missing terrain or cactus prefab.");
            return;
        }

        TerrainData data = terrain.terrainData;
        Vector3 terrainPos = terrain.transform.position;

        for (int i = 0; i < cactusCount; i++)
        {
            float x = Random.Range(0f, data.size.x);
            float z = Random.Range(0f, data.size.z);

            float y = data.GetInterpolatedHeight(
                x / data.size.x,
                z / data.size.z
            );

            Vector3 worldPos = new Vector3(
                x + terrainPos.x,
                y + terrainPos.y,
                z + terrainPos.z
            );

            Vector3 normal = data.GetInterpolatedNormal(
                x / data.size.x,
                z / data.size.z
            );

            float slope = Vector3.Angle(normal, Vector3.up);
            if (slope > maxSlope)
                continue;

            Quaternion rot = Quaternion.FromToRotation(Vector3.up, normal);
            GameObject cactus = Instantiate(
                cactusPrefab,
                worldPos,
                rot,
                parentForSpawned   // <<< TO JEST TEN PARENT
            );

            float scale = Random.Range(minScale, maxScale);
            cactus.transform.localScale = Vector3.one * scale;
        }
    }
}
