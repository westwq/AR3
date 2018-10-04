using UnityEngine;

public class AWE_Estimote_Beacon : MonoBehaviour {

	/// <summary>
	/// Name of the Beacon
	/// </summary>
	public string Name { get; set; }

	/// <summary>
	/// String MAC address of the Beacon
	/// </summary>
	public string MACAddress { get; set; }

	/// <summary>
	/// String Proximity UUID of the Beacon
	/// </summary>
	public string ProximityUUID { get; set; }

	/// <summary>
	/// Major ID of the Beacon
	/// </summary>
	public int Major { get; set; }

	/// <summary>
	/// Minor ID of the Beacon
	/// </summary>
	public int Minor { get; set; }

	/// <summary>
	/// Relative Signal Strength Indicator of the Beacon
	/// </summary>
	public int RSSI { get; set; }

	/// <summary>
	/// Measured power of the Beacon at 1 meter's distance. Only for Android, on iOS this will be 0
	/// </summary>
	public int MeasuredPower { get; set; }

	/// <summary>
	/// Calculated strength of the Beacon by subtracting RSSI from Measure Power. Only for Android, on iOS this will be 0
	/// </summary>
	public int CalculatedStrength { 
		get { return this.RSSI - this.MeasuredPower; }
	}

	/// <summary>
	/// Calculated distance to the beacon in meters, algorithm by Estimote
	/// </summary>
	public double Distance { get; set; }

	/// <summary>
	/// Color of beacon
	/// </summary>
	public string Color { get; set; }

	/// <summary>
	/// Battery life expectancy in days
	/// </summary>
	public double BatteryLifeInDays { get; set; }

	/// <summary>
	/// Current accelerometer output in a 3D vector
	/// </summary>
	public Vector3 Accelerometer { get; set; }

	/// <summary>
	/// Current air pressure in pascal, as of Estimote SDK 0.13.0 (Android) and 4.11.0 (iOS) air pressure output doesn't work
	/// </summary>
	public double Pressure { get; set; }

	/// <summary>
	/// Current ambient light level in lux
	/// </summary>
	public double Light { get; set; }

	/// <summary>
	/// Current temperature in Celcius
	/// </summary>
	public double Temperature { get; set; }
}
