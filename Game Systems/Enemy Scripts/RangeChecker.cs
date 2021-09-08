using UnityEngine;

public class RangeChecker : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float detectionRange = 5;

    private readonly string playerString = "Player";
    private bool targetWasInRange = false;

    private void Awake()
    {
        if(target == null)
        {
            FindNewTarget();
        }
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToTarget = (target.position - transform.position).magnitude;

        if (distanceToTarget <= detectionRange && targetWasInRange == false)
        {
            //Debug.LogFormat("{0} Entered range of {1}.", target.name, name);
            targetWasInRange = true;
        }
        else if (distanceToTarget > detectionRange && targetWasInRange == true)
        {
            //Debug.LogFormat("{0} Exited range of {1}.", target.name, name);
            targetWasInRange = false;
        }
    }

    void FindNewTarget()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag(playerString);

        if(playerObject != null)
        {
            target = playerObject.transform;
        }
    }
}
