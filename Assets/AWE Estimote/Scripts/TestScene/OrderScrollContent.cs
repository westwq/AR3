using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Linq;

public class OrderScrollContent : MonoBehaviour {
	
	// Update is called once per frame
	public void UpdateOrder () {
		int count = transform.childCount-1;
		for (int i = 0; i < count; i++){
			int j = i+1;
			if (transform.GetChild(i).GetComponent<AWE_Estimote_Beacon>().CalculatedStrength < transform.GetChild(j).GetComponent<AWE_Estimote_Beacon>().CalculatedStrength){
				transform.GetChild(i).SetSiblingIndex(j);
			}
		}
	}
}
