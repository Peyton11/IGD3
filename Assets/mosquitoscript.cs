using UnityEngine;
using UnityEngine.UI;

public class MosquitoManager : MonoBehaviour
{
    public GameObject mosquitoPrefab;
    public Transform diver;
    public int numberOfMosquitoes = 100;
    public float spawnRadius = 5f;

    public Text mosquitoUIText; // Reference to the UI Text element

    void Start()
    {
        for (int i = 0; i < numberOfMosquitoes; i++)
        {
            Vector3 spawnPosition = diver.position + Random.insideUnitSphere * spawnRadius;
            spawnPosition.y = Mathf.Max(spawnPosition.y, 1f);

            GameObject mosquito = Instantiate(mosquitoPrefab, spawnPosition, Quaternion.identity, transform);
            MosquitoBehavior behavior = mosquito.GetComponent<MosquitoBehavior>();
            behavior.diver = diver;

            // Assign the UI Text element to the mosquito
            behavior.mosquitovalues = mosquitoUIText;
        }
    }
}