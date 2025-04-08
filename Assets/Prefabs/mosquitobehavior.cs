// mosquitobehavior.cs

using UnityEngine;

public class MosquitoBehavior : MonoBehaviour
{
    // NOT MosquitoManager!
    public Transform diver;
    public float followSpeed = 2f;
    public float buzzingRange = 1f;
    public float buzzingSpeed = 2f;

    private Vector3 offset;

    void Start()
    {
        offset = Random.insideUnitSphere * buzzingRange;
        offset.y = Mathf.Abs(offset.y);
    }

    void Update()
    {
        if (diver == null)
            return;

        Vector3 targetPosition = diver.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        offset += Random.insideUnitSphere * buzzingSpeed * Time.deltaTime;

        if (offset.magnitude > buzzingRange)
        {
            offset = offset.normalized * buzzingRange;
        }

        transform.rotation = Random.rotation;
    }
}
