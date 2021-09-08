using UnityEngine;

public class CollectibleCoin : MonoBehaviour
{
    public Colour CoinColour { get; set; } = Colour.None;   //Set the default to none.
    private CoinGroupColourizer parentCoinClass;            //Class that manages all the individual coins.
    private Animator coinAnimator;                          //Animator component for selecting coin visual.

    public void Awake()
    {
        parentCoinClass = GetComponentInParent<CoinGroupColourizer>();
        coinAnimator = GetComponent<Animator>();

        if(parentCoinClass != null)
        {
            parentCoinClass.AddCoinToList(this);
        }
    }

    public void SetColourOfCoin(Colour ColourLabel, Color colourReference)
    {
        CoinColour = ColourLabel;
        UpdateAnimator();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            DestroyCoin();
        }
    }

    private void DestroyCoin()
    {
        //Debug.Log("Colour of Coin: " + CoinColour);

        if (parentCoinClass != null)
        {
            parentCoinClass.RemoveCoinFromList(this, transform.position);
        }

        Destroy(gameObject);
    }

    public void UpdateAnimator()
    {
        coinAnimator.SetInteger("CoinColour", (int)CoinColour);
    }
}


