using System.Collections;
using UnityEngine;
using TMPro;

public class BlinkingText : MonoBehaviour
{
    public TextMeshProUGUI textToBlink; // Reference to the TextMeshPro object
    public float blinkInterval = 0.5f;  // Time interval for blinking

    private bool isBlinking = true;     // Control blinking state

    void Start()
    {
        if (textToBlink != null)
        {
            StartCoroutine(BlinkText());
        }
    }

    IEnumerator BlinkText()
    {
        while (isBlinking)
        {
            textToBlink.enabled = !textToBlink.enabled; // Toggle visibility
            yield return new WaitForSeconds(blinkInterval);
        }
    }

    public void StopBlinking()
    {
        isBlinking = false;
        textToBlink.enabled = true; // Ensure text is visible when blinking stops
    }
}
