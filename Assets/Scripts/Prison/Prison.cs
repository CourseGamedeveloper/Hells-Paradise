using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the prison mechanics, including detecting key possession and opening the prison door.
/// </summary>
public class Prison : MonoBehaviour
{
    private bool IsHaveKey;
    public GameObject PrisonDoor;
    private GameManager Manager;

    private void Start()
    {
        Manager = FindAnyObjectByType<GameManager>();
        IsHaveKey = false;
    }

    /// <summary>
    /// Sets the key possession status to true.
    /// </summary>
    public void SetIsHaveKey()
    {
        IsHaveKey = true;
        Debug.Log("The player has the key: " + IsHaveKey);
    }

    /// <summary>
    /// Opens the prison door if the player has the key.
    /// </summary>
    private void OpenPrisonDoor()
    {
        Vector3 currentPosition = PrisonDoor.transform.position;
        PrisonDoor.transform.position = new Vector3(currentPosition.x, currentPosition.y + 3f, currentPosition.z);
        Manager.OpenGate();
    }

    /// <summary>
    /// Detects if the player enters the trigger area and opens the door if they have the key.
    /// </summary>
    /// <param name="other">The collider that enters the trigger.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && IsHaveKey)
        {
            Debug.Log("The door is open");
            OpenPrisonDoor();
        }
    }
}
