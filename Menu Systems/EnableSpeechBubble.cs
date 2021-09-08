using UnityEngine;

public class EnableSpeechBubble : MonoBehaviour
{
    [SerializeField] private AutomaticTextFeeder textFeeder;

    private bool inRangeOfCharacter;
    private bool speechBubbleActive;

    // Update is called once per frame
    void Update()
    {
        if (inRangeOfCharacter)
        {
            if (InputManager.GetConfirm() && !speechBubbleActive)
            {
                textFeeder.toggleSpeechBubble(true);
                speechBubbleActive = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        inRangeOfCharacter = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        inRangeOfCharacter = false;
        textFeeder.toggleSpeechBubble(false);
        speechBubbleActive = false;
    }
}
