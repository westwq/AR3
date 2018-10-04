using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AWE_Estimote_Beacon_Handler : MonoBehaviour {
	public Text beaconName;
	public Text proximityUUID;
	public Text major;
	public Text minor;
	public Text rssi;
	public Text distance;
	public Text color;
	public Text batLife;
	public Text accelerometerX;
	public Text accelerometerY;
	public Text accelerometerZ;
	public Text pressure;
	public Text lightText;
	public Text tempText;
	
	// Update is called once per frame
	public void UpdateUI () {
		beaconName.text = GetComponent<AWE_Estimote_Beacon>().Name;
		proximityUUID.text = GetComponent<AWE_Estimote_Beacon>().ProximityUUID;
		major.text = GetComponent<AWE_Estimote_Beacon>().Major.ToString();
		minor.text = GetComponent<AWE_Estimote_Beacon>().Minor.ToString();
		rssi.text = GetComponent<AWE_Estimote_Beacon>().RSSI.ToString();
		distance.text = GetComponent<AWE_Estimote_Beacon>().Distance.ToString("F2");
		color.text = GetComponent<AWE_Estimote_Beacon>().Color;
		batLife.text = GetComponent<AWE_Estimote_Beacon>().BatteryLifeInDays.ToString("F0");
		accelerometerX.text = GetComponent<AWE_Estimote_Beacon>().Accelerometer.x.ToString("F2");
		accelerometerY.text = GetComponent<AWE_Estimote_Beacon>().Accelerometer.y.ToString("F2");
		accelerometerZ.text = GetComponent<AWE_Estimote_Beacon>().Accelerometer.z.ToString("F2");
		pressure.text = GetComponent<AWE_Estimote_Beacon>().Pressure.ToString("F2");
		lightText.text = GetComponent<AWE_Estimote_Beacon>().Light.ToString("F2");
		tempText.text = GetComponent<AWE_Estimote_Beacon>().Temperature.ToString("F2");
	}
}
