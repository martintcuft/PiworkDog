using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScript : MonoBehaviour
{
	public byte piTotal = 5;
    void Start() {}
    public void UpdatePisDisplayed(byte pis) {
		if(pis == piTotal) {
			for(int i = 3; i < transform.childCount; i++) {
				if(transform.GetChild(i) == null)break;
				transform.GetChild(i).gameObject.SetActive(true);
			}
		}
		else {
			for(int i = 3; i < transform.childCount; i++) {
				if(transform.GetChild(i) == null)break;
				transform.GetChild(i).gameObject.SetActive(pis > 0);
				if(pis > 0)pis--;
			}
		}
	}
}
