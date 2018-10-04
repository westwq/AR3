using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    public float heading;
    public Text txt;
    public static Compass instance;
    public enum Loc { Unknown, ICT, B3105, B2705 };
    private float b31Heading = 270;
    private float b27Heading = 90;
    private float bound = 60;

    public AWE_Estimote awe_estimote;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        heading = 0;
        txt.text = "starting";
        StartCoroutine(InitializeLocation());

        Debug.Log("<color=red>Estimote</color>");
        //Instantiate(awe_estimote);
        awe_estimote.beginScanAtStart = true;
        awe_estimote.Setup();
        //awe_estimote.StartScan();

    }
    public IEnumerator InitializeLocation()
    {
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("location disabled by user");
            yield break;
        }
        // enable compass
        Input.compass.enabled = true;
        // start the location service
        Debug.Log("start location service");
        Input.location.Start(10, 0.01f);
        // Wait until service initializes
        int maxSecondsToWaitForLocation = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxSecondsToWaitForLocation > 0)
        {
            yield return new WaitForSeconds(1);
            maxSecondsToWaitForLocation--;
        }

        // Service didn't initialize in 20 seconds
        if (maxSecondsToWaitForLocation < 1)
        {
            Debug.Log("location service timeout");
            yield break;
        }
        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("unable to determine device location");
            yield break;
        }
        Debug.Log("location service loaded");
        yield break;
    }
    // Update is called once per frame
    void Update()
    {
        heading = Input.compass.trueHeading;
        txt.text = heading.ToString();

        if (awe_estimote.beacons == null)
        {
            awe_estimote.StartScan();
            txt.text += "(X)";
        }
        else
        {
            txt.text += "(" + awe_estimote.beacons.Count + ")";
        }
        txt.text += ":" + GetLoc().ToString();
    }

    Loc GetLoc()
    {
        //check ICT block
        //check level 5

        if (b31Heading - bound <= heading && heading <= b31Heading + bound)
            return Loc.B3105;
        else if (b27Heading - bound <= heading && heading <= b27Heading + bound)
            return Loc.B2705;


        return Loc.Unknown;
    }
}
