using UnityEngine;
using TMPro;
using System.Collections;
public class GPSLocation : MonoBehaviour
{
    [SerializeField] TMP_Text locationText;
    double latitud, longitud, altitud, precision;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        if (Input.location.isEnabledByUser)
        {
            Input.location.Start(10f, 5f);
            StartCoroutine(ServiceStatus());
            while (true)
            {
                locationText.text = "Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " +
                    Input.location.lastData.altitude + " " + Input.location.lastData.timestamp;
                yield return new WaitForSeconds(1);
            }
        }
    }
    IEnumerator ServiceStatus()
    {
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }
        if (maxWait < 1)
        {
            Debug.Log("Timed out");
            yield return false;
        }
        else if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.LogError("Unable to determine device location");
            yield return false;
        }
        else
        {
            Debug.Log("Succesfull service initialized");
            yield return true;
        }
    }
    private void OnApplicationQuit()
    {
        Input.location.Stop();
        StopCoroutine(Start());
    }
}
