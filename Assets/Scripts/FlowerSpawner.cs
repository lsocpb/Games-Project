using UnityEngine;

public class FlowerSpawner : MonoBehaviour
{
    [Header("What")]
    [SerializeField] private GameObject flowerPrefab;
    [SerializeField] private int count = 30;

    [Header("Where (rectangle on XZ)")]
    [SerializeField] private Vector3 center = Vector3.zero;
    [SerializeField] private Vector2 size = new Vector2(30f, 30f);

    [Header("Ground sampling")]
    [SerializeField] private float rayStartHeight = 200f;   // jak wysoko startuje promień
    [SerializeField] private float rayLength = 500f;        // jak daleko w dół szuka
    [SerializeField] private LayerMask groundMask;          // warstwa terenu/ziemi
    [SerializeField] private float surfaceOffset = 0.02f;   // żeby nie wchodziło w ziemię

    [Header("No overlap (optional)")]
    [SerializeField] private bool avoidOverlap = true;
    [SerializeField] private float minDistance = 1.2f;
    [SerializeField] private LayerMask blockingMask;        // np. warstwa Flower + przeszkody
    [SerializeField] private int maxAttemptsPerFlower = 40;

    [Header("Rotation / Parent")]
    [SerializeField] private bool alignToNormal = true;     // obrót wg nachylenia terenu
    [SerializeField] private bool randomYRotation = true;
    [SerializeField] private Transform parentForSpawned;

    private void Start() => Spawn();

    public void Spawn()
    {
        if (flowerPrefab == null)
        {
            Debug.LogError("FlowerSpawner: flowerPrefab not assigned.");
            return;
        }

        int spawned = 0;
        int safety = count * maxAttemptsPerFlower;

        for (int i = 0; i < safety && spawned < count; i++)
        {
            if (TryGetPointOnGround(out Vector3 pos, out Vector3 normal))
            {
                if (avoidOverlap && Physics.CheckSphere(pos, minDistance, blockingMask, QueryTriggerInteraction.Ignore))
                    continue;

                Quaternion rot = Quaternion.identity;

                if (alignToNormal)
                    rot = Quaternion.FromToRotation(Vector3.up, normal);

                if (randomYRotation)
                    rot = rot * Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

                Instantiate(flowerPrefab, pos, rot, parentForSpawned);
                spawned++;
            }
        }

        if (spawned < count)
            Debug.LogWarning($"FlowerSpawner: spawned {spawned}/{count}. Increase area/attempts or loosen overlap rules.");
    }

    private bool TryGetPointOnGround(out Vector3 position, out Vector3 normal)
    {
        float x = Random.Range(center.x - size.x * 0.5f, center.x + size.x * 0.5f);
        float z = Random.Range(center.z - size.y * 0.5f, center.z + size.y * 0.5f);

        Vector3 origin = new Vector3(x, center.y + rayStartHeight, z);

        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, rayLength, groundMask, QueryTriggerInteraction.Ignore))
        {
            normal = hit.normal;
            position = hit.point + hit.normal * surfaceOffset; // lekko nad powierzchnię
            return true;
        }

        position = default;
        normal = Vector3.up;
        return false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector3(center.x, center.y, center.z), new Vector3(size.x, 0.1f, size.y));
    }
#endif
}
