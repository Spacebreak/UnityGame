using UnityEngine;

public class ItemDetection : MonoBehaviour
{
    private static readonly string objectString = "Object";     //The string holding the GameObject tag reference to the object.

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(objectString))
        {
            collision.GetComponent<InteractableItem>().SetItemInteractable(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(objectString))
        {
            collision.GetComponent<InteractableItem>().SetItemInteractable(false);
        }
    }
}
