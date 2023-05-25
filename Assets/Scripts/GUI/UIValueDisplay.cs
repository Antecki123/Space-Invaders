using TMPro;
using UnityEngine;

public class UIValueDisplay : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private FloatVariable variable;

    public void UpdateValue() => text.text = variable.value.ToString();
}