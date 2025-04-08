// mosquitoscript.cs

using UnityEngine;

public class MosquitoManager : MonoBehaviour
{
    public GameObject mosquitoPrefab;
    public Transform diver;
    public int numberOfMosquitoes = 100;
    public float spawnRadius = 5f;

    void Start()
    {
        for (int i = 0; i < numberOfMosquitoes; i++)
        {
            Vector3 spawnPosition = diver.position + Random.insideUnitSphere * spawnRadius;
            spawnPosition.y = Mathf.Max(spawnPosition.y, 1f);

            // Instantiate mosquito
            GameObject mosquito = Instantiate(mosquitoPrefab, spawnPosition, Quaternion.identity, transform);

            // 👉 Assign the diver to the mosquito's MosquitoBehavior script
            mosquito.GetComponent<MosquitoBehavior>().diver = diver;
        }
    }
}

