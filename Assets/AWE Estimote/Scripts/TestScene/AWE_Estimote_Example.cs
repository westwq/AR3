using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

public class AWE_Estimote_Example : MonoBehaviour {

	public GameObject loaderIndicator;
	public Text debugEstimoteInfoText;
	public AWE_Estimote awe_estimote;
	public Button scanButton;
	public Text scanButtonText;

	// Use this for initialization
	void Start () {
		//As beacons only update once every 1 second by default, there's no need to update app too fast anyway
		Application.targetFrameRate = 20;

		//Make UI more of an app than game and show navigation buttons
		Screen.fullScreen = false;

		//Setup the AWE Estimote Asset
		awe_estimote.Setup();

		//Change text on the scan button as we begin scanning immediately
		if (awe_estimote.beginScanAtStart){
			scanButtonText.text = "STOP SCANNING";
		}
	}

	/// <summary>
	/// Method for starting and stopping scanning. Updates text on scan button as well
	/// </summary>
	public void ToggleScanning(){
		if (scanButtonText.text == "START SCANNING"){
			awe_estimote.StartScan();
			scanButtonText.text = "STOP SCANNING";
		}
		else {
			awe_estimote.StopScan();
			scanButtonText.text = "START SCANNING";
		}
	}


	/// <summary>
	/// Clears the list of beacons in the UI
	/// </summary>
	public void ClearBeaconsList(){
		if (awe_estimote.searching){
			awe_estimote.StopScan();
			awe_estimote.beacons.Clear();
			for (int i = 0;i < awe_estimote.beaconsScrollContent.transform.childCount; i++){
				Destroy(awe_estimote.beaconsScrollContent.transform.GetChild(i).gameObject);
			}
			awe_estimote.StartScan();
		}
		else {
			awe_estimote.beacons.Clear();
			for (int i = 0;i < awe_estimote.beaconsScrollContent.transform.childCount; i++){
				Destroy(awe_estimote.beaconsScrollContent.transform.GetChild(i).gameObject);
			}
		}

	}

	// Update is called once per frame
	void FixedUpdate () {

		//When the app the is ready to scan after initialization, make the scan button interactable and change color
		if (awe_estimote.initialized && !scanButton.interactable){
			scanButton.interactable = true;
			scanButtonText.color = new Color(0.996f,0.313f,0f,1);
		}	


		//When actively searcing, rotate the AWE logo to reflect we are scanning
		if (awe_estimote.searching){
			loaderIndicator.transform.Rotate(Vector3.forward,-1f);

			if (loaderIndicator.GetComponent<Image>().fillClockwise){
				if(loaderIndicator.GetComponent<Image>().fillAmount < 1f) {
					loaderIndicator.GetComponent<Image>().fillAmount = loaderIndicator.GetComponent<Image>().fillAmount + 0.025f;
				}
				else {
					loaderIndicator.GetComponent<Image>().fillClockwise = false;
				}	
			}
			else {
				if(loaderIndicator.GetComponent<Image>().fillAmount > 0f) {
					loaderIndicator.GetComponent<Image>().fillAmount = loaderIndicator.GetComponent<Image>().fillAmount - 0.025f;
				}
				else {
					loaderIndicator.GetComponent<Image>().fillClockwise = true;
				}
			}
		}

		//Update the debug info ui text
		debugEstimoteInfoText.text = awe_estimote.debugEstimoteInfo;
	}
}