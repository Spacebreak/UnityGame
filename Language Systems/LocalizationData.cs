//These fields need to be serialized so they can be sent and converted into bytes between different formats.

//Stores all of the localization items in an array of LocalizationItem classes.
[System.Serializable]
public class LocalizationData 
{
    public LocalizationItem[] items;
}

//Each LocalizationItem class in the LocalizationData class contains a key and a corresponding value.
[System.Serializable]
public class LocalizationItem
{
    public string key;
    public string value;
}