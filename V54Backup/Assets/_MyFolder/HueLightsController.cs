using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HueLightsController : MonoBehaviour
{
    [SerializeField] private string bridgeIP = "10.0.0.138";
    [SerializeField] private string username = "q1vBE3yboLWBg8Rl5LBXx43qCf5V2bnC43c0w7wA";

    private const int minBri = 1;
    private const int maxBri = 254;

    // Start is called before the first frame update
    void Start()
    {
        // Example: Turn on a specific light
        SetLightState(1, true);
    }

    public void SetLightState(int lightID, bool isOn)
    {
        string jsonBody = $"{{\"on\": {isOn.ToString().ToLower()}}}";
        StartCoroutine(SendRequestToHue(lightID, jsonBody, "state change"));
    }

    public void SetLightBrightness(int lightID, int brightness)
    {
        brightness = Mathf.Clamp(brightness, minBri, maxBri);
        string jsonBody = $"{{\"bri\": {brightness}}}";
        StartCoroutine(SendRequestToHue(lightID, jsonBody, "brightness change"));
    }

    private IEnumerator SendRequestToHue(int lightID, string jsonBody, string actionDescription)
    {
        string url = $"http://{bridgeIP}/api/{username}/lights/{lightID}/state";
        UnityWebRequest www = UnityWebRequest.Put(url, jsonBody);
        www.method = "PUT";
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Failed to {actionDescription} for Light {lightID}: {www.error}");
        }
        else
        {
            Debug.Log($"Successfully sent {actionDescription} for Light {lightID}");
        }
    }
}