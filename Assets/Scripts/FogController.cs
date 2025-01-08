using UnityEngine;

public class FogController : MonoBehaviour
{
    public Color lightRealmFogColor = Color.clear;
    public Color shadowRealmFogColor = Color.black;
    public float lightRealmFogDensity = 0f;
    public float shadowRealmFogDensity = 0.02f;
    public float transitionSpeed = 2f;

    public void ChangeFog(bool isLightRealm)
    {
        Color targetFogColor = isLightRealm ? lightRealmFogColor : shadowRealmFogColor;
        float targetFogDensity = isLightRealm ? lightRealmFogDensity : shadowRealmFogDensity;

        StartCoroutine(ChangeFogProperties(targetFogColor, targetFogDensity));
        Debug.Log("Fog changed to: " + (isLightRealm ? "Light Realm" : "Shadow Realm"));
    }

    private System.Collections.IEnumerator ChangeFogProperties(Color targetColor, float targetDensity)
    {
        float t = 0;
        Color startColor = RenderSettings.fogColor;
        float startDensity = RenderSettings.fogDensity;

        while (t < 1)
        {
            t += Time.deltaTime * transitionSpeed;
            RenderSettings.fogColor = Color.Lerp(startColor, targetColor, t);
            RenderSettings.fogDensity = Mathf.Lerp(startDensity, targetDensity, t);
            yield return null;
        }
    }
}
