using UnityEngine;
using DialogueEditor;
public class ConverstionStarter : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Object NPC Conversation")]
    private NPCConversation Myconversation;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                ConversationManager.Instance.StartConversation(Myconversation);
            }
        }
    }
}
