using UnityEngine;
using TMPro;

public class EditTextOnHover : MonoBehaviour
{
    TextMeshProUGUI textToEnlargen;
    private float originalTextSize;

    //Start is called before the first frame update
    void Start()
    {
        textToEnlargen = GetComponent<TextMeshProUGUI>();
        originalTextSize = textToEnlargen.fontSize;
    }

    public void SetFontSizeActive()
    {
        textToEnlargen.enableAutoSizing = false;

        textToEnlargen.fontSize = originalTextSize + 4;
    }

    public void SetFontSizeNotActive()
    {
        textToEnlargen.enableAutoSizing = true;

        textToEnlargen.fontSize = originalTextSize;
    }


}
