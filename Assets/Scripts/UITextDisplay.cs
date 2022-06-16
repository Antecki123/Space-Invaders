using TMPro;
using UnityEngine;

public class UITextDisplay : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private StringVariable variable;

    public void UpdateValue() => text.text = variable.text.ToString();
}