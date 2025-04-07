using UnityEngine;

public class DiverMovement : MonoBehaviour
{
    public float speed;
    private Vector3 startPosition = new Vector3(-22f, -0.5f, 5f);
    private Vector3 topOfStairsPosition = new Vector3(-6f, 9f, 5f);
    private Vector3 endOfDivingBoardPosition = new Vector3(-0.5f, 9f, 5f);

    private bool isMoving = false;
    private bool isAtTopOfStairs = false;
    private bool isAtEndOfBoard = false;

    public int diveCount = 0;
    
    // Handle physics
    private Rigidbody rb;
    
    // Reference to the wave script (assign in Inspector)
    public WaveScript wave;
    
    void Start()
    {
        // Diver starts at the initial position
        transform.position = startPosition;
        
        // Get or add Rigidbody component
        if (!gameObject.TryGetComponent<Rigidbody>(out rb))
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.useGravity = false; // Disable gravity at the start
        rb.isKinematic = true; // Prevent Rigidbody from interfering with movement initially
    }

    void Update()
    {
        // Move up the stairs when "C" is pressed
        if (Input.GetKeyDown(KeyCode.C) && !isMoving && !isAtTopOfStairs)
        {
            isMoving = true;
        }

        if (isMoving && !isAtTopOfStairs)
        {
            MoveDiverUpStairs();
        }

        // Run to end of the diving board when "R" is pressed
        else if (Input.GetKeyDown(KeyCode.R) && isAtTopOfStairs && !isMoving && !isAtEndOfBoard)
        {
            isMoving = true;
        }

        if (isMoving && isAtTopOfStairs && !isAtEndOfBoard)
        {
            MoveDiverToEndOfBoard();
        }

        if (transform.position.y < -10f)
        {
            ResetDiver();
        }
    }

    // Move the diver up the stairs
    void MoveDiverUpStairs()
    {
        // Smoothly move the diver from start to target position
        transform.position = Vector3.MoveTowards(transform.position, topOfStairsPosition, speed * Time.deltaTime);

        // If the diver has reached the target position, stop moving
        if (transform.position == topOfStairsPosition)
        {
            isMoving = false;
            isAtTopOfStairs = true;
            Debug.Log("Diver reached the top of the stairs");
        }
    }
    
    // Move the diver to the end of the diving board
    void MoveDiverToEndOfBoard()
    {
        // Smoothly move the diver from start to target position
        transform.position = Vector3.MoveTowards(transform.position, endOfDivingBoardPosition, speed * Time.deltaTime);

        // If the diver has reached the target position, stop moving
        if (transform.position == endOfDivingBoardPosition)
        {
            // Enable gravity to fall off the board
            rb.useGravity = true; 
            rb.isKinematic = false;
            
            isMoving = false;
            isAtEndOfBoard = true;
            
            Debug.Log("Diver reached the target position!");
        }
        
    }
    
    void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to the water
        if (other.CompareTag("Water"))
        {
            diveCount++; // Increment dive count
            Debug.Log($"Diver landed in the water! Total dives: {diveCount}");
            
            // Trigger wave effect  
            // if (wave != null)
            // {
            //     wave.StartWave(other.transform.position);
            // }

            // Optional: Reset the diver to the starting position
            // ResetDiver();
        }
    }
        
    void ResetDiver()
    {
        Debug.Log("Resetting diver to the start position");
        isMoving = false;
        isAtTopOfStairs = false;
        isAtEndOfBoard = false;

        rb.isKinematic = true;  // Disable physics again
        rb.useGravity = false;  // Turn off gravity
        transform.position = startPosition;
    }
}
