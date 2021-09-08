using UnityEngine;

public interface IBoundary
{
    void SimulatePuzzlePiece();
    void UnsimulatePuzzlePiece();
    void ResetPuzzle();
    bool PuzzleCompletionStatus();
}

public class PuzzleBoundary : MonoBehaviour
{
    private readonly string playerString = "Player";

    public GameObject puzzleObject;     //The object that holds the class that contains the boundary Interface.
    private IBoundary puzzleBoundary;   //The interface utilizing the desired methods.

    private void Start()
    {
        puzzleBoundary = puzzleObject.GetComponent<IBoundary>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!puzzleBoundary.PuzzleCompletionStatus() && collision.CompareTag(playerString))
        {
            puzzleBoundary.SimulatePuzzlePiece();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(playerString))
        {
            puzzleBoundary.UnsimulatePuzzlePiece();

            //If player leaves area before completing the puzzle, will reset the puzzle.
            if (!puzzleBoundary.PuzzleCompletionStatus())
            {
                puzzleBoundary.ResetPuzzle();
            }
        }
    }
}
