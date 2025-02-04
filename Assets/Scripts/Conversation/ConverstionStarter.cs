using UnityEngine;
using DialogueEditor;

/// <summary>
/// Triggers an NPC conversation when the player is within range and presses the interaction key.
/// </summary>
public class ConverstionStarter : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Object NPC Conversation")]
    private NPCConversation Myconversation;

    /// <summary>
    /// Checks if the player is within range and starts the conversation when the interaction key is pressed.
    /// </summary>
    /// <param name="other">The collider of the object that stays within the trigger zone.</param>
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
        {
            ConversationManager.Instance.StartConversation(Myconversation);
        }
    }
}
