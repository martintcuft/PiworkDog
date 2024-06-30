using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
	public GameObject prefab;
	public float spawnEvery = 2f;
	public bool silentSpawns = false;
	[SerializeField] float counter = 0f;
	[SerializeField] bool onSpawn_flip = false;
	public AudioSource audioPlayer;
	
    void Start() {
        
    }

    void Update() {
        counter += Time.deltaTime;
		if(counter >= spawnEvery) {
			GameObject theThing = Instantiate(prefab, transform.position, new Quaternion(0f, 0f, 0f, 0f));
			if(theThing.GetComponent<DangerScript>() != null) theThing.GetComponent<DangerScript>().sprite.flipX = onSpawn_flip;
			if(!silentSpawns) audioPlayer.Play();
			counter -= spawnEvery;
		}
    }
}
