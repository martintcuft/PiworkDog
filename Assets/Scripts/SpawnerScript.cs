using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
	public GameObject prefab;
	public float spawnEvery = 2f;
	[SerializeField] float counter = 0f;
	[SerializeField] bool onSpawn_flip = false;
	public AudioSource audioPlayer;
	
    void Start() {
        
    }

    void Update() {
        counter += Time.deltaTime;
		if(counter >= spawnEvery) {
			Instantiate(prefab, transform.position, new Quaternion(0f, 0f, 0f, 0f)).GetComponent<DangerScript>().sprite.flipX = onSpawn_flip;
			audioPlayer.Play();
			counter -= spawnEvery;
		}
    }
}
