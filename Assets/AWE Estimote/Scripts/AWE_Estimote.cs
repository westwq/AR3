using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

public class AWE_Estimote : MonoBehaviour
{
    public string appId; //App ID. Create an app id on Estimote Cloud for this Unity App. Add the id in the editor
    public string appToken; //App Token. Create an app token on Estimote Cloud for this Unity App. Add the token in the editor
    public bool debugEstimoteCloud = false; //Show debugging info about the setup and data retrieval of Estimote Beacons
    public string debugEstimoteInfo; //Public string containing the debug info
    public string uuid; //Proximity UUID. Change the Proximity UUID of the beacons you want to access in this Unity app. Change the selected beacons to have the same UUID, and add the UUID in the editor.

    public List<AWE_Estimote_Beacon> beacons; //List of beacons. You may access this list to use the beacons for any purpose
    public GameObject beaconPrefab; //Prefab for displaying beacons in UI
    public GameObject beaconsScrollContent; //List scroll view to display beacons in a list in UI
    public bool initialized = false; //public bool to show if the asset including Estimote Cloud has been initialized
    public bool searching = false; //public bool to show if the asset is scanning for beacons currently
    public bool beginScanAtStart = false; //Public bool whether or not to begin scanning immediately after setup and initialization has completed.
    public bool promptForBT = false; //Public bool whether or not to check and prompt for enabling BlueTooth on the device.

    public int scanPeriodMillis = 6000; // How long to perform Bluetooth Low Energy scanning, in milliseconds
    public int waitTimeMillis = 0; //How long to wait until performing next scanning, in milliseconds

    private AndroidJavaClass _class; //Android class for communicating with Android native code
    private AndroidJavaObject activity; //The Android activity
    private AndroidJavaObject context; //The Android context
    private string[] beaconString; //Strings with beacons data. Parsed in ScanListenerCallBack
    private bool btEnabled = false; //Whether or not BlueTooth is enabled



    /// <summary>
    /// Setup the AWE Estimote asset, ready to scan for beacons
    /// </summary>
    public void Setup()
    {
        if (beaconPrefab == null)
        {
            DebugEstimote("'beaconPrefab' has not been assigned in AWE_Estimote script inspector");
        }
        if (appId == "" || appToken == "" || uuid == "")
        {
            DebugEstimote("Info needed in AWE_Estimote.cs editor!");
        }
        else {
            if (uuid.Length != 36)
            {
                DebugEstimote("Are you sure you entered the Proximity UUID and not the Identifier to the Proximity UUID field in AWE_Estimote inspector? Current UUID is " + uuid.Length + " characters long but usually UUIDs are 32 characters long");
            }

            //Android
            if (Application.platform == RuntimePlatform.Android)
            {
                //Checking to see if the asset can be reached
                AndroidJavaClass pluginClass = new AndroidJavaClass("dk.theawe.estimotewrapper.wrapper");
                DebugEstimote(pluginClass.CallStatic<string>("getMessage"));

                //Get the activity info for this Unity app
                AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity");
                context = activity.Call<AndroidJavaObject>("getApplicationContext");

                _class = new AndroidJavaClass("dk.theawe.estimotewrapper.wrapper_handler");

                //Intialize the asset. Send the Unity activity back to Java so we know which context we're working in
                _class.CallStatic("initialize", context, activity, gameObject.name, promptForBT, scanPeriodMillis, waitTimeMillis);
            }
            //iOS
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
#if UNITY_IOS
				//Initialize the asset.
				_Initialize (gameObject.name, promptForBT);
#endif
            }
            else {
                //When using the editor or something else, we cannot access Estimote Beacons
                DebugEstimote("Not using iOS or Android");
            }
        }
    }


    /// <summary>
    /// Wait for BT to be reported as enabled before starting up the initialization
    /// </summary>
    private IEnumerator DelayedCloudInitialization()
    {
        //Android
        if (Application.platform == RuntimePlatform.Android)
        {
            //Do nothing until btEnabled is true. Changed by BTCallback from native code
            while (!btEnabled)
            {
                yield return null;
            }

            DebugEstimote("Unity: Initialize Cloud App");
            _class.CallStatic("initializeCloudApp", appId, appToken, debugEstimoteCloud, uuid);
        }
        //iOS
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            //Do nothing until btEnabled is true. Changed by BTCallback from native code
            while (!btEnabled)
            {
                yield return null;
            }
#if UNITY_IOS
			DebugEstimote ("Unity: Initialize Cloud App");
			_InitializeCloudApp(appId,appToken,debugEstimoteCloud,uuid);
#endif
        }
    }


    /// <summary>
    /// Begin scanning after initialization has completed. Can be invoked automatically by changed beginScanAtStart (bool) to true
    /// </summary>
    public void StartScan()
    {
        if (initialized && !searching)
        {
            //Android
            if (Application.platform == RuntimePlatform.Android)
            {
                DebugEstimote("Scanning...");
                _class.CallStatic("startScanning");
                searching = true;
            }
            //iOS
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
#if UNITY_IOS
				DebugEstimote("Beginning scan");
				_StartScanning();
				searching = true;
#endif
            }
        }
        else {
            DebugEstimote("Estimote not initialied");
        }

    }


    /// <summary>
    /// Stop scanning for beacons.
    /// </summary>
    public void StopScan()
    {
        if (initialized && searching)
        {
            //Android
            if (Application.platform == RuntimePlatform.Android)
            {
                DebugEstimote("Stopping scan");
                _class.CallStatic("stopScanning");
                searching = false;
            }
            //iOS
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
#if UNITY_IOS
				DebugEstimote("Stopping scan");
				_StopScanning();
				searching = false;
#endif
            }
        }
    }





    //
    //CALLBACKS. Called from native code
    //




    /// <summary>
    /// Is called with a response to whether or not the bluetooth is enabled on the device. Bluetooth must be enabled before this asset can retrieve data from the beacons.
    /// </summary>
    public void BTCallback(string message)
    {
        if (message == "true")
        {
            btEnabled = true;
        }
        else if (message == "BT disabled")
        {
            DebugEstimote("BlueTooth disabled");
        }
        else if (message == "BT Not Authorized")
        {
            DebugEstimote("BlueTooth disabled");
        }
        else {
            DebugEstimote("Unknown BlueTooth Error. Is BT on and did you authorize BT for this app?");
        }
    }

    /// <summary>
    /// Is called when initialization to AWE Estimote asset is succesful. This is needed to get data from the beacons.
    /// </summary>
    public void InitializationCallback(string message)
    {
        //After initialization, start scanning for proximity beacons
        if (message == "Initialized")
        {
            if (appId == "" || appToken == "" || uuid == "")
            {
                DebugEstimote("Estimote App Info needed in AWE_Estimote.cs");
            }
            else {
                StartCoroutine(DelayedCloudInitialization());
            }
        }
        else {
            DebugEstimote("Initialization: " + message);
        }
    }

    /// <summary>
    /// Is called when initialization to Estimote Cloud is succesful. This is needed for retriving the name, color and battery life of the beacon
    /// </summary>
    private void CloudInitializationCallback(string message)
    {
        DebugEstimote(message);

        //After initialization, start scanning for proximity beacons
        if (message == "Initialized")
        {
            initialized = true;

            if (beginScanAtStart)
            {
                StartScan();
            }
        }
    }

    /// <summary>
    /// Retrieved beacons data and adds each beacon with data to the list, beacons. Edit this if you do not want to populate a ui scroll list with the beacon data
    /// </summary>
    private void ScanListenerCallback(string message)
    {
        if (message != "")
        {
            beaconString = message.Split(';');

            for (int i = 0; i < beaconString.Length; i++)
            {
                if (beaconString[i] != "" && GetDataValue(beaconString[i], "UUID:") != "")
                {
                    //Create new beacon if it doesn't exist already
                    AWE_Estimote_Beacon tempBeacon = beacons.FirstOrDefault(b => b.ProximityUUID == GetDataValue(beaconString[i], "UUID:") && b.Major == int.Parse(GetDataValue(beaconString[i], "Major:")) && b.Minor == int.Parse(GetDataValue(beaconString[i], "Minor:")));

                    if (tempBeacon == null)
                    {
                        if (beaconPrefab != null)
                        {
                            //When we want to show beacons in the UI on a UI scroll view
                            if (beaconsScrollContent != null)
                            {
                                GameObject go = Instantiate(beaconPrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0)) as GameObject;
                                go.transform.SetParent(beaconsScrollContent.transform, false);
                                tempBeacon = go.GetComponent<AWE_Estimote_Beacon>();
                            }
                            //There's no UI scroll view to show the beacons in, so just add the beacons to the list instead
                            else {
                                tempBeacon = new AWE_Estimote_Beacon();
                            }
                            beacons.Add(tempBeacon);
                        }
                        //When we just want the beacons in the "beacons" list
                        else {
                            tempBeacon = new AWE_Estimote_Beacon();
                            beacons.Add(tempBeacon);
                        }

                        DebugEstimote("Found new beacon. " + beacons.Count.ToString() + " in total.");
                    }

                    //Update values for this beacon
                    if (GetDataValue(beaconString[i], "Name:") == "")
                    {
                        tempBeacon.Name = "Minor: " + GetDataValue(beaconString[i], "Minor:");
                    }
                    else {
                        tempBeacon.Name = GetDataValue(beaconString[i], "Name:");
                    }

                    tempBeacon.MACAddress = GetDataValue(beaconString[i], "MAC:");
                    tempBeacon.ProximityUUID = GetDataValue(beaconString[i], "UUID:");
                    tempBeacon.Major = int.Parse(GetDataValue(beaconString[i], "Major:"));
                    tempBeacon.Minor = int.Parse(GetDataValue(beaconString[i], "Minor:"));
                    if (int.Parse(GetDataValue(beaconString[i], "RSSI:")) != 0)
                    {
                        tempBeacon.RSSI = int.Parse(GetDataValue(beaconString[i], "RSSI:"));
                    }
                    tempBeacon.MeasuredPower = int.Parse(GetDataValue(beaconString[i], "MP:"));

                    double tempDist;
                    if (double.TryParse(GetDataValue(beaconString[i], "Dist:"), out tempDist))
                    {
                        tempBeacon.Distance = tempDist;
                    }
                    else {
                        tempBeacon.Distance = 0;
                    }

                    tempBeacon.Color = GetDataValue(beaconString[i], "Color:");

                    if (GetDataValue(beaconString[i], "BatLife:") != "null" && GetDataValue(beaconString[i], "BatLife:") != "")
                    {
                        double tempbat;
                        if (double.TryParse(GetDataValue(beaconString[i], "BatLife:"), out tempbat))
                        {
                            tempBeacon.BatteryLifeInDays = tempbat;
                        }
                        else {
                            tempBeacon.BatteryLifeInDays = 0;
                        }
                    }

                    string tempAcc = GetDataValue(beaconString[i], "Acc:");
                    if (tempAcc != "null")
                    {
                        if (tempAcc != "")
                        {
                            tempAcc = tempAcc.Replace("(", "");
                            tempAcc = tempAcc.Replace(")", "");

                            //Some phones' decimals are commas, and have to be periods to be able to parse them. This makes sure of this
                            if (tempAcc.Count(x => x == ',') == 5)
                            {
                                tempAcc = ReplaceCommas(tempAcc);
                            }

                            int middleLength = tempAcc.LastIndexOf(',') - (tempAcc.IndexOf(','));

                            float tempAccX = float.Parse(tempAcc.Substring(0, tempAcc.IndexOf(',')));
                            float tempAccY = float.Parse(tempAcc.Substring(tempAcc.IndexOf(',') + 1, middleLength - 1));
                            float tempAccZ = float.Parse(tempAcc.Substring(tempAcc.LastIndexOf(',') + 1));
                            tempBeacon.Accelerometer = new Vector3(tempAccX, tempAccY, tempAccZ);
                        }
                        else {
                            tempBeacon.Accelerometer = new Vector3(0, 0, 0);
                        }
                    }
                    else {
                        tempBeacon.Accelerometer = new Vector3(0, 0, 0);
                    }

                    if (GetDataValue(beaconString[i], "Pressure:") != "null" && GetDataValue(beaconString[i], "Pressure:") != "" && !GetDataValue(beaconString[i], "Pressure:").Contains("0.00"))
                    {
                        //DebugEstimote("Pres: " + GetDataValue(beaconString[i],"Pressure:"));
                        double tempPres;
                        if (double.TryParse(GetDataValue(beaconString[i], "Pressure:"), out tempPres))
                        {
                            tempBeacon.Pressure = tempPres;
                        }
                        else {
                            tempBeacon.Pressure = 0;
                        }
                    }

                    if (GetDataValue(beaconString[i], "Light:") != "null" && GetDataValue(beaconString[i], "Light:") != "" && !GetDataValue(beaconString[i], "Light:").Contains("0.00"))
                    {
                        //DebugEstimote("Light: " + GetDataValue(beaconString[i],"Light:"));
                        double tempLight;
                        if (double.TryParse(GetDataValue(beaconString[i], "Light:"), out tempLight))
                        {
                            tempBeacon.Light = tempLight;
                        }
                        else {
                            tempBeacon.Light = 0;
                        }
                    }

                    if (GetDataValue(beaconString[i], "Temp:") != "null" && GetDataValue(beaconString[i], "Temp:") != "" && !GetDataValue(beaconString[i], "Temp:").Contains("0.00"))
                    {
                        //DebugEstimote("Temp: " + GetDataValue(beaconString[i],"Temp:"));
                        double tempTemperature;
                        if (double.TryParse(GetDataValue(beaconString[i], "Temp:"), out tempTemperature))
                        {
                            tempBeacon.Temperature = tempTemperature;
                        }
                        else {
                            tempBeacon.Temperature = 0;
                        }
                    }

                    if (beaconsScrollContent != null && beaconPrefab != null)
                    {
                        tempBeacon.GetComponent<AWE_Estimote_Beacon_Handler>().UpdateUI();
                    }
                }
            }
            //Sort order of the beacons in ui by the one's closest, and update the UI of all beacons in UI
            if (beaconsScrollContent != null && beaconPrefab != null)
            {
                beaconsScrollContent.GetComponent<OrderScrollContent>().UpdateOrder();
            }
        }
    }




    //
    // iOS specific methods, called from other methods
    //


#if UNITY_IOS
	//iOS Methods
	[DllImport ("__Internal")]
	static extern void _Initialize(string go, bool promptForBT);

	[DllImport ("__Internal")]
	static extern void _InitializeCloudApp(string appID, string appToken, bool debugEstimoteCloud, string uuid);

	[DllImport ("__Internal")]
	static extern void _StartScanning();

	[DllImport ("__Internal")]
	static extern void _StopScanning();

#endif




    //
    //Helper functions, called from other methods
    //


    /// <summary>
    /// Converts retrieved beacon string into beacons data
    /// </summary>
    private string GetDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|"))
        {
            value = value.Remove(value.IndexOf("|"));
        }

        return value;
    }

    /// <summary>
    /// Debugger which outputs to both UI text and Debug.Log
    /// </summary>
    private void DebugEstimote(string debug)
    {
        if (debugEstimoteCloud && debugEstimoteInfo != debug)
        {
            debugEstimoteInfo = debug;
            Debug.Log(debug);
        }
    }

    /// <summary>
    /// Some phones' decimals are commas, and have to be periods to be able to parse them. This makes sure of this
    /// </summary>
    private static string ReplaceCommas(string str)
    {
        int first = IndexOfNth(str, ',', 1);
        int second = IndexOfNth(str, ',', 3);
        int third = IndexOfNth(str, ',', 5);
        str = str.Remove(first, 1).Insert(first, ".");
        str = str.Remove(second, 1).Insert(second, ".");
        str = str.Remove(third, 1).Insert(third, ".");
        return str;
    }

    /// <summary>
	/// Finds the index of a Nth occurence of a char in a string
	/// </summary>
	private static int IndexOfNth(string str, char c, int nth, int startPosition = 0)
    {
        int index = str.IndexOf(c, startPosition);
        if (index >= 0 && nth > 1)
        {
            return IndexOfNth(str, c, nth - 1, index + 1);
        }

        return index;
    }
}
