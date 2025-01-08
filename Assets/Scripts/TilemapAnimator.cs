using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapSwitcher : MonoBehaviour
{
    public Tilemap tilemap1;           // Reference to the first Tilemap
    public Tilemap tilemap2;           // Reference to the second Tilemap
    public float switchInterval = 1f; // Time interval for switching

    private bool isTilemap1Active = true; // Tracks which tilemap is active

    void Start()
    {
        // Ensure only one tilemap is active at the start
        tilemap1.gameObject.SetActive(isTilemap1Active);
        tilemap2.gameObject.SetActive(!isTilemap1Active);

        // Start the switching coroutine
        StartCoroutine(SwitchTilemaps());
    }

    IEnumerator SwitchTilemaps()
    {
        while (true)
        {
            // Wait for the specified interval
            yield return new WaitForSeconds(switchInterval);

            // Toggle the active state of the tilemaps
            isTilemap1Active = !isTilemap1Active;
            tilemap1.gameObject.SetActive(isTilemap1Active);
            tilemap2.gameObject.SetActive(!isTilemap1Active);
        }
    }
}
