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
		
		GameObject dust_effect = GameObject.Find("Tear_Dust");
		if(dust_effect != null) {
			dust_effect.transform.position = transform.position;
			dust_effect.GetComponent<ParticleSystem>().Play();
			dust_effect.GetComponent<AudioSource>().PlayOneShot(dust_effect.GetComponent<AudioSource>().clip);
		}
        Destroy(gameObject);
    }
	/*private void OnTriggerEnter(Collider other) {
		Debug.Log("--- Whoa im tear");
        ObjectScript objectItself = other.gameObject.GetComponent<ObjectScript>();
        if (objectItself != null) objectItself.OnTearContact();
        Destroy(gameObject);
    }*/
}
