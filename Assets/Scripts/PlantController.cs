using UnityEngine;

public class PlantController : MonoBehaviour
{
    private SkinnedMeshRenderer skinnedMesh;

    // Prêdkoœæ pulsowania
    public float speed = 50.0f;

    void Start()
    {
        // Pobieramy komponent renderera z obiektu
        skinnedMesh = GetComponent<SkinnedMeshRenderer>();
    }

    void Update()
    {
        if (skinnedMesh != null)
        {
            // Mathf.PingPong sprawia, ¿e wartoœæ idzie od 0 do 100 i z powrotem
            float weight = Mathf.PingPong(Time.time * speed, 100.0f);

            // Ustawiamy wagê BlendShape. 
            skinnedMesh.SetBlendShapeWeight(0, weight);
        }
    }
}