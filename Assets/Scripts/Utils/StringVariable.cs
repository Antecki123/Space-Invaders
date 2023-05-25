using UnityEngine;

[CreateAssetMenu(fileName = "New String", menuName = "Scriptable Objects/String Variable")]
public class StringVariable : ScriptableObject
{
    public string text;
    [TextArea] public string discription;
}