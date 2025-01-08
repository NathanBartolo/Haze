using UnityEngine;

public class RealmEnvironment : MonoBehaviour
{
    public Light mainLight; // Reference to your main directional light
    public Color lightRealmColor = Color.white; // Light realm color
    public Color shadowRealmColor = Color.gray; // Shadow realm color
    public float transitionSpeed = 2f; // Speed of light transition

    public void ChangeLighting(bool isLightRealm)
    {
        Color targetColor = isLightRealm ? lightRealmColor : shadowRealmColor;
        StartCoroutine(ChangeLightColor(targetColor));

        Debug.Log("Lighting changed to: " + (isLightRealm ? "Light Realm" : "Shadow Realm"));
    }

    private System.Collections.IEnumerator ChangeLightColor(Color targetColor)
    {
        float t = 0;
        Color startColor = mainLight.color;

        while (t < 1)
        {
            t += Time.deltaTime * transitionSpeed;
            mainLight.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }
    }
}
