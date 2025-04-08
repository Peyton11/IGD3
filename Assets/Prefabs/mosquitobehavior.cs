using UnityEngine;
using UnityEngine.UI;

public class MosquitoBehavior : MonoBehaviour
{
    public Transform diver;  // The diver the mosquito will follow
    public float followSpeed = 2f;
    public float buzzingRange = 3f;  // Make mosquitoes spread out more
    public float buzzingSpeed = 2f;  // Slower, more natural movement
    public float separationDistance = 1f;  // Minimum distance between mosquitoes
    public float separationStrength = 2f;  // How strongly they avoid each other

    private Vector3 offset;

    public Text mosquitovalues;

    void Start()
    {
        // Create an initial offset for each mosquito's position within the buzzing range
        offset = Random.insideUnitSphere * buzzingRange;
        offset.y = Mathf.Abs(offset.y); // Ensure mosquitoes stay above water
    }

    void Update()
    {
        if (diver == null)
            return;
        
        // Keybinds to change values    
        if (Input.GetKeyDown(KeyCode.Alpha1)) followSpeed += 0.1f;
        if (Input.GetKeyDown(KeyCode.Alpha2)) followSpeed -= 0.1f;
        if (Input.GetKeyDown(KeyCode.Alpha3)) buzzingRange += 0.1f;
        if (Input.GetKeyDown(KeyCode.Alpha4)) buzzingRange -= 0.1f;
        if (Input.GetKeyDown(KeyCode.Alpha5)) buzzingSpeed += 0.1f;
        if (Input.GetKeyDown(KeyCode.Alpha6)) buzzingSpeed -= 0.1f;
        if (Input.GetKeyDown(KeyCode.Alpha7)) separationDistance += 0.1f;
        if (Input.GetKeyDown(KeyCode.Alpha8)) separationDistance -= 0.1f;
        if (Input.GetKeyDown(KeyCode.Alpha9)) separationStrength += 0.1f;
        if (Input.GetKeyDown(KeyCode.Alpha0)) separationStrength -= 0.1f;
        
        // Display mosquito values
        mosquitovalues.text = "Mosquito values:\n" +
                              "Follow speed = " + followSpeed.ToString("F1") + "\n" +
                              "Buzzing range = " + buzzingRange.ToString("F1") + "\n" +
                              "Buzzing speed = " + buzzingSpeed.ToString("F1") + "\n" +
                              "Separation distance = " + separationDistance.ToString("F1") + "\n" +
                              "Separation strength = " + separationStrength.ToString("F1");

        // Calculate target position around the diver with the offset
        Vector3 targetPosition = diver.position + offset;

        // Adjust the height to be 1 unit higher
        targetPosition.y += 3f;

        // Prevent the mosquito from flying below water level (y = 0)
        targetPosition.y = Mathf.Max(targetPosition.y, 1f); // Stay above y = 1

        // Smoothly move toward the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        // Add more sporadic randomness to the mosquito's movement
        offset += new Vector3(
            Mathf.PerlinNoise(Time.time * buzzingSpeed, 0f) - 0.5f,
            Mathf.PerlinNoise(0f, Time.time * buzzingSpeed) - 0.5f,
            Mathf.PerlinNoise(Time.time * buzzingSpeed, Time.time * buzzingSpeed) - 0.5f
        ) * buzzingSpeed * Time.deltaTime;

        // Prevent the offset from growing too large
        if (offset.magnitude > buzzingRange)
        {
            offset = offset.normalized * buzzingRange;
        }

        // Add collision avoidance
        AvoidOtherMosquitoes();

        // Add more exaggerated randomness to the rotation for a buzzing effect
        transform.rotation = Quaternion.Euler(
            Random.Range(-30f, 30f),
            Random.Range(0f, 360f),
            Random.Range(-30f, 30f)
        );
    }

    void AvoidOtherMosquitoes()
    {
        Collider[] nearbyMosquitoes = Physics.OverlapSphere(transform.position, separationDistance);
        foreach (Collider mosquito in nearbyMosquitoes)
        {
            if (mosquito.gameObject != gameObject) // Avoid self
            {
                Vector3 awayFromMosquito = transform.position - mosquito.transform.position;
                transform.position += awayFromMosquito.normalized * separationStrength * Time.deltaTime;
            }
        }
    }
}
