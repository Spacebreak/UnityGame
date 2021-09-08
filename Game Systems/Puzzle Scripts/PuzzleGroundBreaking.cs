using UnityEngine;

public class PuzzleGroundBreaking : MonoBehaviour, IBoundary
{
    public Rigidbody2D seesawPuzzleCubeRigidBody;   //The rigidbody attached to the puzzle cube on the SeeSaw.
    public GameObject seesawPuzzleCube;             //The gameobject of the puzzle cube.
    public GameObject breakableGround;              //The ground that is to be broken by the puzzle cube.
    public GameObject breakableGroundGravity;       //The gravity of the breakable ground. Disabled once ground breaks.

    private bool puzzleCompleted;                   //Tracks puzzle completion status.
    private Vector3 puzzlePieceStartPosition;       //The starting position of the puzzle piece.
    private Quaternion puzzlePieceStartRotation;    //The starting rotation of the puzzle piece.
    public Rigidbody2D seesawRigidbody;             //The rigidbody attached to the seesaw.
    private Quaternion seesawStartRotation;         //The starting rotation of the seesaw.

    private readonly string groundBreakString = "GroundBreak";

    private void Start()
    {
        puzzlePieceStartPosition = seesawPuzzleCube.transform.position;
        puzzlePieceStartRotation = seesawPuzzleCube.transform.rotation;
        seesawStartRotation = seesawRigidbody.transform.rotation;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.relativeVelocity.y > 12 && collision.gameObject.CompareTag(groundBreakString))
        {
            breakableGround.SetActive(false);
            breakableGroundGravity.SetActive(false);
            seesawPuzzleCube.SetActive(false);
            puzzleCompleted = true;
            //Debug.Log("Relative Y: " + collision.relativeVelocity.y);            
        }
    }

    public void SimulatePuzzlePiece()
    {
        seesawPuzzleCubeRigidBody.simulated = true;
    }

    public void UnsimulatePuzzlePiece()
    {
        seesawPuzzleCubeRigidBody.simulated = false;
    }

    //Revert the puzzle piece back to its' original position.
    public void ResetPuzzle()
    {
        seesawPuzzleCubeRigidBody.velocity = Vector2.zero;
        seesawPuzzleCube.transform.position = puzzlePieceStartPosition;
        seesawPuzzleCube.transform.rotation = puzzlePieceStartRotation;
        seesawRigidbody.transform.rotation = seesawStartRotation;
    }

    public bool PuzzleCompletionStatus()
    {
        return puzzleCompleted;
    }
}
