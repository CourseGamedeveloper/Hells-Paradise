using UnityEngine;

public class Heart : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Speed of rotation in degrees per second.")]
    private float rotationSpeed = 100f;

    [SerializeField]
    [Tooltip("Axis of rotation.")]
    private Vector3 rotationAxis = Vector3.up;

    // Update is called once per frame
    void Update()
    {
        // Rotate the heart around the specified axis
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && enabled)
        {
            // Destroy this object when the player collides with it
            Destroy(gameObject);
        }
    }
}
