using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MeshFilter))]
public class WaveScript : MonoBehaviour
{
    public float amplitude = 1.0f;  // A: Amplitude of the wave
    public float wavelength = 2.0f; // λ: Wavelength of the wave
    public float speed = 5.0f;   // V: Velocity of the wave
    public float decaySpeed = 0.1f; // a: Speed of decay

    private Vector3[] originalVertices; // Store original mesh vertices
    private Vector3[] modifiedVertices; // Modified vertices during runtime
    private Mesh mesh;                  // Reference to the water mesh
    private bool isWaveActive = false;  // Track whether waves are active
    private float timeOfImpact;         // Time when the impact occurred
    private Vector3 entryPoint;         // P0(x0, z0): Center point of the wave
    
    public ParticleSystem splashEffect;

    public Text values;

    void Start()
    {
        // Get the mesh attached to this object
        mesh = GetComponent<MeshFilter>().mesh;

        // Cache the original vertex positions
        originalVertices = mesh.vertices;
        modifiedVertices = new Vector3[originalVertices.Length];
    }

    void Update()
    {
        
        // Keybinds to change values    
        if (Input.GetKeyDown(KeyCode.A)) amplitude += 0.1f;
        if (Input.GetKeyDown(KeyCode.B)) amplitude -= 0.1f;
        if (Input.GetKeyDown(KeyCode.K)) wavelength += 0.1f;
        if (Input.GetKeyDown(KeyCode.L)) wavelength -= 0.1f;
        if (Input.GetKeyDown(KeyCode.V)) speed += 0.1f;
        if (Input.GetKeyDown(KeyCode.N)) speed -= 0.1f;
        
        // Display wave values
        values.text = "Amplitude = " + amplitude + "\nWavelength = " + wavelength + "\nSpeed = " + speed + "\nDecay Speed = " + decaySpeed + "\nTime = " + Time.time;

        if (!isWaveActive) return;

        float t = Time.time - timeOfImpact;

        for (int i = 0; i < originalVertices.Length; i++)
        {
            Vector3 worldVertex = transform.TransformPoint(originalVertices[i]);

            // Compute radial distance from the entry point
            float r = Vector2.Distance(new Vector2(worldVertex.x, worldVertex.z), new Vector2(entryPoint.x, entryPoint.z));

            // Compute the wave displacement
            float wavePhase = (2 * Mathf.PI * (r - speed * t)) / wavelength;
            float displacement = amplitude * Mathf.Exp(-r * decaySpeed) * Mathf.Cos(wavePhase);

            // Apply only Y-axis displacement
            modifiedVertices[i] = originalVertices[i];
            modifiedVertices[i].y += displacement;
        }

        // Update the mesh
        mesh.vertices = modifiedVertices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    // Trigger waves when a diver enters the water
    private void OnTriggerEnter(Collider other)
    {
        // Set the entry point of the wave
        entryPoint = other.transform.position;

        // Activate the waves
        isWaveActive = true;

        // Record the time of impact
        timeOfImpact = Time.time;
        
        if (splashEffect != null)
        {
            Debug.Log("SPLASH");
            splashEffect.transform.position = new Vector3(-0.5f, 0, 5f);
            splashEffect.Play();
        }
    }
}
