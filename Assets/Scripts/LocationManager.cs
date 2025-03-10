using UnityEngine;
using UnityEngine.Android;
using System.Collections;



public class LocationManager : MonoBehaviour
{
    private float lat;
    private float lon;
    private const int serviceStartWait = 20; // Time to wait for location services to start on phone
    private const float StudentLat = 53.7628944f;
    private const float StudentLon = -2.7073222f;
    private const float StudentRadius = 40.0f; // In meters
    //53.7628944,-2.7073222
    private const float LibraryLat = 53.763814f;
    private const float LibraryLon = -2.7073843f;
    private const float LibraryRadius = 40.0f; // In meters

    public LocationManager()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            Permission.RequestUserPermission(Permission.CoarseLocation);
        }
        else
        {
            Debug.Log("Not android I guess");
        }
    }

    public float GetLatitude()
    {
        return lat;
    }

    public float GetLongitude()
    {
        return lon;
    }

    public bool HasLocationPermission()
    {
        return Input.location.isEnabledByUser;
    }

    public IEnumerator UpdateLocation()
    {
        if (!HasLocationPermission())
        {
            Debug.Log("Location permission not enabled by user");
            yield break;
        }

        Input.location.Start();
        int waitTime = 0;
        while (Input.location.status == LocationServiceStatus.Initializing && waitTime < serviceStartWait)
        {
            yield return new WaitForSeconds(1);
            waitTime++;
        }

        if (waitTime >= serviceStartWait)
        {
            Debug.Log("GPS did not start in time");
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.LogError("Unable to determine device location");
            yield break;
        }
        else
        {

            this.lat = Input.location.lastData.latitude;
            this.lon = Input.location.lastData.longitude;
            Debug.Log(lat.ToString() + " " + lon.ToString());
        }

        Input.location.Stop();
    }

    public float GetDistanceToStudentCenter()
    {
        StartCoroutine(UpdateLocation());
        // Distance to UClan student centre 
        // Calculated assuming flat earth (which we all know is true)
        float metersPerDegreeLat = 111320.0f;
        float metersPerDegreeLon = metersPerDegreeLat * Mathf.Cos(Mathf.Deg2Rad * this.lat);

        float diffLat = (StudentLat - this.lat) * metersPerDegreeLat;
        float diffLon = (StudentLon - this.lon) * metersPerDegreeLon;

        float distanceToUni = Mathf.Sqrt(Mathf.Pow(diffLat, 2) + Mathf.Pow(diffLon, 2));
        Debug.Log("distance from uni:" + distanceToUni.ToString());
        return distanceToUni;
    }
    
    public float GetDistanceToLibrary()
    {
        StartCoroutine(UpdateLocation());

        // Calculated assuming flat earth (which we all know is true)
        float metersPerDegreeLat = 111320.0f;
        float metersPerDegreeLon = metersPerDegreeLat * Mathf.Cos(Mathf.Deg2Rad * this.lat);

        float diffLat = (LibraryLat - this.lat) * metersPerDegreeLat;
        float diffLon = (LibraryLon - this.lon) * metersPerDegreeLon;

        float distanceToUni = Mathf.Sqrt(Mathf.Pow(diffLat, 2) + Mathf.Pow(diffLon, 2));
        Debug.Log("distance from student center:" + distanceToUni.ToString());
        return distanceToUni;
    }

}