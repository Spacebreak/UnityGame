using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gemstones : MonoBehaviour
{
    public List<GameObject> luperGemstones;     //All the gems in the level.
    private static TokenWhite whiteToken;       //The script responsible for shifting gravity.
    public AudioSource gemCollectedSound;       //The sound that will play when a gem is collected.

    // Start is called before the first frame update    
    void Start()
    {
        whiteToken = FindObjectOfType<TokenWhite>();
    }

    //When gravity is shifted, all the gems are rotated in order to match the correct orientation.
    public void UpdateGemstoneOrientation()
    {
        int gemstoneRotation;

        switch (whiteToken.GravityIndex())
        {
            case 0:
                gemstoneRotation = 0;
                break;

            case 1:
                gemstoneRotation = 270;
                break;

            case 2:
                gemstoneRotation = 180;
                break;

            case 3:
                gemstoneRotation = 90;
                break;

            default:
                gemstoneRotation = 0;
                break;
        }

        foreach(GameObject gem in luperGemstones)
        {
            if(gem != null)
            {
                gem.transform.eulerAngles = new Vector3(gem.transform.rotation.x, gem.transform.rotation.y, gemstoneRotation);
            }
        }
    }

    //Removes a gem from the collection, occurs when the gem is collected.
    public void RemoveGem(GameObject gemToRemove)
    {
        luperGemstones.Remove(gemToRemove);
    }

    //Plays a sound when gem is collected. Called from the Gem class.
    public void PlayGemCollectSound()
    {
        gemCollectedSound.Play();
    }
}
