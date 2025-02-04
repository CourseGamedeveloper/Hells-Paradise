using UnityEngine;

/// <summary>
/// Manages the cursor visibility and locking state during gameplay.
/// </summary>
public class CursorManager : MonoBehaviour
{
    private void Start()
    {
        LockCursor();
    }

    private void Update()
    {
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            LockCursor();
        }

        // Unlock the cursor when pressing ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnlockCursor();
        }
    }

    /// <summary>
    /// Locks the cursor and makes it invisible.
    /// </summary>
    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// Unlocks the cursor and makes it visible.
    /// </summary>
    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
