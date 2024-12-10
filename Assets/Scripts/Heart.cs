using UnityEngine;

public class Heart : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Speed of rotation in degrees per second.")]
    public float rotationSpeed = 100f;
    
    [Tooltip("Axis of rotation.")]
    public Vector3 rotationAxis = Vector3.up;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotationAxis,rotationSpeed*Time.deltaTime);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && enabled)
        {
    
            Destroy(this.gameObject); // Destroy the other object (e.g., bullet)
         
        }
    }
}
