using UnityEngine;

public class GravityPuzzle : MonoBehaviour, IBoundary
{
    public GameObject puzzleLock;               //The piece of ground holding the puzzle in place at the end.
    private bool puzzleCompleted = false;       //A boolean keeping track of the puzzles' progress.

    public GameObject gemstoneDiamond;          //The reward for completing the puzzle.

    public GameObject puzzlePiece;              //The puzzle piece that needs to be solved.
    public Rigidbody2D puzzlePieceRigidBody;    //The rigidbody attached to the puzzle piece.
    public GameObject puzzlePieceShadow;        //The puzzle piece shadow that the puzzle piece needs to reach.
    public SpriteRenderer puzzleShadowSprite;   //The sprite attached to the shadow puzzle piece.

    private Vector3 puzzlePieceStartPosition;   //The starting position of the puzzle piece.

    public AudioSource puzzleCompleteSound;     //The soundfile to be played when completing a puzzle.

    private readonly string puzzleString = "PuzzlePiece"; //The string holding reference to the Puzzle game object.

    public ConstantForce2D puzzleForces;        //The constant force 2D object that simulates gravity.

    private Vector2[] gravityValues = {new Vector2(0, -25f), new Vector2(-25f, 0), new Vector2(0, 25f),
        new Vector2(25f, 0) };        //An array storing the various force values to apply to the puzzle on GravShift.

    public SetAnimationState[] platformAnimations;

    private void Start()
    {
        puzzlePieceStartPosition = puzzlePiece.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if(!puzzleCompleted && collision.CompareTag(puzzleString))
        {
            puzzleLock.SetActive(true);
            puzzleCompleted = true;
            puzzleCompleteSound.Play();
            gemstoneDiamond.SetActive(true);

            DisablePuzzlePiece();
            StopPlatformAnimations();
            ChangePuzzlePieceLocation(puzzlePieceShadow.transform.position);
        }        
    }

    private void StopPlatformAnimations()
    {
        if(platformAnimations.Length > 0)
        {
            foreach (SetAnimationState platformAnimationScript in platformAnimations)
            {
                platformAnimationScript.EndAnimation();
            }
        }
    }

    //Disables the sprite on the shadow puzzle piece and stops rigidbody simulations on the puzzle piece.
    private void DisablePuzzlePiece()
    {
        puzzleShadowSprite.enabled = false;
        UnsimulatePuzzlePiece();
    }

    //Changes the position of the puzzle piece to the new location.
    private void ChangePuzzlePieceLocation(Vector3 newPuzzlePieceLocation)
    {
        puzzlePiece.transform.position = newPuzzlePieceLocation;
    }

    public void SimulatePuzzlePiece()
    {
        //if (!puzzleCompleted)
        {
            puzzlePieceRigidBody.simulated = true;
        }
    }

    public void UnsimulatePuzzlePiece()
    {
        puzzlePieceRigidBody.simulated = false;
    }

    public bool PuzzleCompletionStatus()
    {
        return puzzleCompleted;
    }

    public void ResetPuzzle()
    {
        puzzlePiece.transform.position = puzzlePieceStartPosition;
    }

    public void AlterPuzzleGravity(int gravityIndex)
    {
        puzzleForces.force = gravityValues[gravityIndex];        
    }
}
