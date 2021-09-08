using UnityEngine;

public class TokenBlack : MonoBehaviour
{
    private readonly string playerString = "Player";
    private readonly string lightString = "WorldLight";

    public GameObject areaLight; //The Light that lightens the area around Luper.
    private GameObject worldLighting; //The Light that lightens the entire world.

    // Start is called before the first frame update
    void Awake()
    {
        Camera.main.backgroundColor = Color.black;

        AttachLocalLightToLuper();
        ToggleWorldLighting(false);
    }

    public void ToggleWorldLighting(bool switchLighting)
    {
        worldLighting = GameObject.FindGameObjectWithTag(lightString);

        if (worldLighting != null)
        { 
            worldLighting.SetActive(switchLighting);
        }
    }

    private void AttachLocalLightToLuper()
    {
        GameObject mainCharacter = GameObject.FindGameObjectWithTag(playerString);

        if (mainCharacter != null)
        {
            areaLight.transform.position = new Vector3(mainCharacter.transform.position.x,
                mainCharacter.transform.position.y, -25);
            areaLight.transform.SetParent(mainCharacter.transform);
        }
    }
}
