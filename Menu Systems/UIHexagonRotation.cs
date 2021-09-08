using UnityEngine;

public class UIHexagonRotation : MonoBehaviour
{
    public GameObject mobileAxisHexagon;

    // Update is called once per frame
    void Update()
    {
        mobileAxisHexagon.transform.Rotate(0, 0, 0.3f, Space.Self);
    }

}
