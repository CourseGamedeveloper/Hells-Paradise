using UnityEngine;

public class CursorManager : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; // מסתיר את הסמן
    }

    void Update()
    {
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false; // מסתיר את הסמן
        }
        

        // שחרור הסמן בעת לחיצה על ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None; // שחרור הסמן
            Cursor.visible = true; // מציג את הסמן
        }
    }
}
