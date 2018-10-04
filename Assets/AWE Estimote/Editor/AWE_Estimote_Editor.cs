using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.UI;

[CustomEditor( typeof( AWE_Estimote) )]
public class AWE_Estimote_Editor : Editor {

	private Texture aweLogo;
	private bool showingHelp = false;

	public override void OnInspectorGUI(){
		AWE_Estimote script = (AWE_Estimote) target;

		//Default inspector
		//base.OnInspectorGUI();
		//EditorGUILayout.Space();


		GUIStyle logoStyle = new GUIStyle();
		logoStyle.alignment = TextAnchor.MiddleCenter;
		aweLogo = Resources.Load("awe-logo") as Texture;
		GUILayout.Label(aweLogo,logoStyle);

		GUIStyle headLineStyle = new GUIStyle();
		headLineStyle.fontStyle = FontStyle.Bold;
		headLineStyle.fontSize = 20;
		headLineStyle.alignment = TextAnchor.MiddleCenter;
		EditorGUILayout.LabelField( "AWE Estimote Unity Asset",headLineStyle);

		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();

		script.appId = EditorGUILayout.TextField("Estimote App ID",script.appId);
		if(showingHelp){
			EditorGUILayout.HelpBox("Paste the created App ID from the Estimote Cloud here",MessageType.None,true);
			EditorGUILayout.Space();
			EditorGUILayout.Space();
		}

		script.appToken = EditorGUILayout.TextField("Estimote App Token",script.appToken);
		if(showingHelp){
			EditorGUILayout.HelpBox("Paste the created App Token from the Estimote Cloud here",MessageType.None,true);
			EditorGUILayout.Space();
			EditorGUILayout.Space();
		}

		/*
		SerializedObject serializedObject = new SerializedObject(target);
		SerializedProperty property = serializedObject.FindProperty("uuids");
		serializedObject.Update();
		EditorGUILayout.PropertyField(property,new GUIContent("Proximity UUIDs"),true);
		serializedObject.ApplyModifiedProperties();
		*/
		script.uuid = EditorGUILayout.TextField("Proximity UUID (Not Identifier!)",script.uuid);
		if(showingHelp){
			EditorGUILayout.HelpBox("Paste the Proximity UUIDs for the selected Estimote Beacons, found on Estimote Cloud",MessageType.None,true);
			EditorGUILayout.Space();
			EditorGUILayout.Space();
		}

        EditorGUILayout.Space();

        script.scanPeriodMillis = EditorGUILayout.IntField("Scan Period (Android)",script.scanPeriodMillis);
		if(showingHelp){
			EditorGUILayout.HelpBox("The period to scan for before waiting on Android. Default 6000ms, Minimum 200ms. iOS scans autonomously. Please get your Scan Period + Wait Period above 6000ms if you target Android", MessageType.None,true);
			EditorGUILayout.Space();
			EditorGUILayout.Space();
		}

		script.waitTimeMillis = EditorGUILayout.IntField("Wait Period (Android)",script.waitTimeMillis);
		if(showingHelp){
			EditorGUILayout.HelpBox("The period to wait between scans on Android. Increase this to save battery. Default 0ms. iOS scans autonomously. Please get your Scan Period + Wait Period above 6000ms if you target Android", MessageType.None,true);
			EditorGUILayout.Space();
			EditorGUILayout.Space();
		}

        if (script.scanPeriodMillis + script.waitTimeMillis < 6000)
        {
            EditorGUILayout.HelpBox("Warning! Android Nougat only allows for 5 scans per 30 seconds. Please get your Scan Period + Wait Period above 6000ms if you target Android. Current combined Scan and Wait Period: " + (script.scanPeriodMillis + script.waitTimeMillis), MessageType.None);
        }
        

        EditorGUILayout.Space();
		EditorGUILayout.Space();


		script.debugEstimoteCloud = EditorGUILayout.Toggle("Debug Estimote Connection",script.debugEstimoteCloud);
		if(showingHelp){
			EditorGUILayout.HelpBox("Shows info about success of connection to Estimote Cloud and connection to beacons",MessageType.None,true);
			EditorGUILayout.Space();
			EditorGUILayout.Space();
		}

		script.beginScanAtStart = EditorGUILayout.Toggle("Auto-start ranging",script.beginScanAtStart);
		if(showingHelp){
			EditorGUILayout.HelpBox("Whether or not to begin scan for ranging beacons immediately after initialization is complete",MessageType.None,true);
			EditorGUILayout.Space();
			EditorGUILayout.Space();
		}
			
		script.promptForBT = EditorGUILayout.Toggle("Promt BlueTooth enable",script.promptForBT);
		if(showingHelp){
			EditorGUILayout.HelpBox("Whether or not to prompt the user to enable Bluetooth or not. iOS can only advice to turn BT on.",MessageType.None,true);
			EditorGUILayout.Space();
			EditorGUILayout.Space();
		}


		EditorGUILayout.Space();
		EditorGUILayout.Space();

        if (script.beaconPrefab == null)
        {
            EditorGUILayout.HelpBox("To display beacons in UI list: Attach the 'Beacon' prefab from AWE Estimote/Prefabs/", MessageType.Info, true);
        }
		script.beaconPrefab = (GameObject)EditorGUILayout.ObjectField("Beacon Prefab",script.beaconPrefab,typeof(GameObject),true);
		if(showingHelp){
			EditorGUILayout.HelpBox("UI representation of a beacon. Leave this blank if you just want the beacons to be added to the 'beacons' list.",MessageType.None,true);
			EditorGUILayout.Space();
			EditorGUILayout.Space();
		}


		script.beaconsScrollContent = (GameObject)EditorGUILayout.ObjectField("Scroll View Content",script.beaconsScrollContent,typeof(GameObject),true);
		if(showingHelp){
			EditorGUILayout.HelpBox("UI list representation of a beacon. This is where the UI beacons will be shown. Leave this blank if you just want the beacons to be added to the 'beacons' list.",MessageType.None,true);
			EditorGUILayout.Space();
			EditorGUILayout.Space();
		}

		if(showingHelp){
			
			if (GUILayout.Button("Visit Estimote SDK Doc")){
				Application.OpenURL("http://developer.estimote.com/");
			}

			EditorGUILayout.Space();

			if (GUILayout.Button("Visit Estimote Cloud")){
				Application.OpenURL("https://cloud.estimote.com/");
			}


			EditorGUILayout.Space();
			if (GUILayout.Button("Visit AWE website")){
				Application.OpenURL("https://www.theawe.dk");
			}

			EditorGUILayout.Space();
			EditorGUILayout.Space();
		}


		EditorGUILayout.Space();

		if (GUILayout.Button("Toggle Help")){
			if (showingHelp){
				showingHelp = false;
			}
			else {
				showingHelp = true;
			}
		}
	}
}
