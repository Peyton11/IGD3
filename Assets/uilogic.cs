using UnityEngine;
using UnityEngine.UI;

public class UILogic : MonoBehaviour
{
    public Text diveCountText; // Reference to the UI Text that displays the dive count
    private DiverMovement movementScript; // Reference to the Movement script

    void Start()
    {
        // Find the Movement script in the scene
        movementScript = FindObjectOfType<DiverMovement>();

        // Check if the Movement script was found
        if (movementScript == null)
        {
            Debug.LogError("Movement script not found in the scene! Make sure a GameObject has the Movement script attached.");
        }

        // Check if the diveCountText UI element is assigned in the Inspector
        if (diveCountText == null)
        {
            Debug.LogError("Dive Count Text is not assigned! Drag and drop the Text element into the Dive Count Text field in the Inspector.");
        }
    }

    void Update()
    {
        // Update the Text UI if references are valid
        if (movementScript != null && diveCountText != null)
        {
            diveCountText.text = "Dives: " + movementScript.diveCount.ToString();
        }
    }
}