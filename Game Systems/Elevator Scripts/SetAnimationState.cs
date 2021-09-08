using UnityEngine;

public class SetAnimationState : MonoBehaviour
{
    public Animator animationController;
    public int platformNumber;

    private void Awake()
    {
        animationController.SetInteger("Platform", platformNumber);
    }

    public void EndAnimation()
    {
        animationController.SetBool("PuzzleComplete", true);
    }
}
