using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ObjectScript : MonoBehaviour, IPointerClickHandler
{
	public byte objID = 0;//0 = square, 1 = book, 2 = pencilcase, 3 = pibook
    void Start() {
        
    }
	public void OnPointerClick(PointerEventData eventData) {//Ocurre cuando el objecto recibe un click
		Debug.Log("Clicked object type: " + objID);//Envía un mensaje en la consola de testeo
	}
	public void OnTearContact() {//Se debe ejecutar cuando una lágrima choca con un objecto que tenga la tag "Object" y/o el script "ObjectScript"
		
	}
}
