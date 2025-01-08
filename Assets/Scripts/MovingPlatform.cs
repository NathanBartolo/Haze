using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform[] points; // Points to move between
    public float speed = 2f;   // Speed of the platform
    public int startPointIndex = 0; // Index of the starting point

    private int currentPointIndex; // Current point the platform is moving towards

    void Start()
    {
        currentPointIndex = startPointIndex;
        transform.position = points[currentPointIndex].position; // Start at the initial point
    }

    void Update()
    {
        MovePlatform();
    }

    void MovePlatform()
    {
        if (points.Length == 0) return; // No points to move between

        // Move the platform towards the target point
        transform.position = Vector2.MoveTowards(transform.position, points[currentPointIndex].position, speed * Time.deltaTime);

        // Check if the platform reached the target point
        if (Vector2.Distance(transform.position, points[currentPointIndex].position) < 0.1f)
        {
            // Switch to the next point
            currentPointIndex = (currentPointIndex + 1) % points.Length;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform); // Parent the player to the platform
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null); // Unparent the player from the platform
        }
    }

}
