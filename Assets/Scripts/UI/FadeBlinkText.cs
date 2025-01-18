using UnityEngine;
using TMPro;

public class FadeBlinkText : MonoBehaviour
{
	[Header("References")]
	public TextMeshProUGUI textToBlink;

	[Header("Settings")]
	public float speed = 1.0f; // Controls how fast the text fades in/out

	private void Update()
	{
		// The value will oscillate between 0 and 1
		float alpha = (Mathf.Sin(Time.time * speed) + 1.0f) * 0.5f;

		// Get current color
		Color currentColor = textToBlink.color;

		// Set the alpha
		currentColor.a = alpha;

		// Assign it back to the text
		textToBlink.color = currentColor;
	}
}
