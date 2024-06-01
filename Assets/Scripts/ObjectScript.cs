using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ObjectScript : MonoBehaviour, IPointerClickHandler
{
	public byte objID = 0;//0 = n/a, 1 = book, 2 = pencilcase, 3 = pibook, 4 = door
    void Start() {
        //set corresponding sprite
    }
	public void OnPointerClick(PointerEventData eventData) {//Ocurre cuando el objecto recibe un click
		Debug.Log("Clicked object type: " + objID);//Envía un mensaje en la consola de testeo
	}
	public void OnTearContact() {//Se debe ejecutar cuando una lágrima choca con un objecto que tenga la tag "Object" y/o el script "ObjectScript"
		Debug.Log("Tear hit object type: " + objID);
	}
}

/*public class ObjectTearScript : MonoBehaviour
{
	private void OnTriggerEnter(Collider other) {
		Debug.Log("--- Whoa im OBJECT");
		if (other.tag == "Tear") GetComponent<ObjectScript>().OnTearContact();
        Destroy(other.gameObject);
	}
}*/