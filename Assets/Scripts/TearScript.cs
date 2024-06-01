using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TearScript : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision) {
		Debug.Log("--- Whoa im tear");
        //ObjectScript objectParent = collision.transform.parent.GetComponent<ObjectScript>();
        ObjectScript objectItself = collision.gameObject.GetComponent<ObjectScript>();
        //if (objectParent != null) objectParent.OnTearContact();
        if (objectItself != null) objectItself.OnTearContact();
        Destroy(gameObject);
    }
	/*private void OnTriggerEnter(Collider other) {
		Debug.Log("--- Whoa im tear");
        ObjectScript objectItself = other.gameObject.GetComponent<ObjectScript>();
        if (objectItself != null) objectItself.OnTearContact();
        Destroy(gameObject);
    }*/
}
