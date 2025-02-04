using UnityEngine;
using UnityEngine.UI;

public class Prison : MonoBehaviour
{
    private bool IsHavekey;
    public GameObject PrisonDoor;
    private GameManager Manager;
    void Start()
    {
        Manager=FindAnyObjectByType<GameManager>();
        IsHavekey = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setIsHaveKey()
    {
        IsHavekey = true;
        Debug.Log("the player Have key: " + IsHavekey);
    }
    private void OpenPrisonDoor() 
    {
        Vector3 currentPosition = PrisonDoor.transform.position;
        PrisonDoor.transform.position = new Vector3(currentPosition.x, currentPosition.y + 3f, currentPosition.z);
        Manager.OpenGate();

    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (IsHavekey)
            {
                Debug.Log("the door is open");
                OpenPrisonDoor();
            }
        }
    }
}
