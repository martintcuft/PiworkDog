using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ObjectScript : MonoBehaviour//, IPointerClickHandler
{
	public byte objID = 0;//0 = n/a, 1 = book, 2 = pencilcase, 3 = pi book, 4 = door
	public Collider2D collider;
	
	//Animación de objeto
	public SpriteRenderer sprite;
	public byte spriteFrame = 0;
	[SerializeField] float animCounter = 0f;
	public Sprite[] objSprites;
	
	bool shouldBeSolid = false;//usado para que epi no atraviese el piso si pone de pie un libro mientras este parado en él
	public bool interacted = false;//si el objeto fué interacuado o no (ej: de pie || botado)
	public GameObject piPrefab;
	
	//Máscaras de capas cuando el objeto es tangible y cuando no
	[SerializeField] private LayerMask excludeSolidLayers;
	[SerializeField] private LayerMask excludePassLayers;
	
	//Particle System
	private byte dustType = 254;
	[SerializeField] ParticleSystem book_dust;
	[SerializeField] ParticleSystem pencil_dust;
	[SerializeField] ParticleSystem pibook_dust;
	
	//Sounds
	public AudioClip[] obj_sfx;
	public AudioSource audioPlayer;
	
    void Start() {
        //set corresponding sprite
		switch(objID) {
			case 0: sprite.sprite = objSprites[0]; break;
			case 1: sprite.sprite = objSprites[!interacted ? 0 : 3]; break;
			case 2: sprite.sprite = objSprites[!interacted ? 8 : 12]; break;
			case 3: sprite.sprite = objSprites[16]; break;
			case 4: sprite.sprite = objSprites[20]; break;
		}
    }
	void Update() {
		if(shouldBeSolid && !Physics2D.BoxCast(transform.position, new Vector2(1f, 1f), 0, Vector3.zero, 0f, excludePassLayers)) {
			collider.excludeLayers = excludeSolidLayers;
			shouldBeSolid = false;
		}
		AnimateObject();
	}
	/*public void OnPointerClick(PointerEventData eventData) {//Ocurre cuando el objecto recibe un click
		Debug.Log("Clicked object type: " + objID);//Envía un mensaje en la consola de testeo
	}*/
	private void PlayDustEffect() {
		switch(dustType) {
			case 0: book_dust.Play(); break;
			case 1: pencil_dust.Play(); break;
			case 2: pibook_dust.Play(); break;
			default: break;
		}
	}
	public void OnTearContact() {//Se debe ejecutar cuando una lágrima choca con un objecto que tenga la tag "Object" y/o el script "ObjectScript"
		Debug.Log("Tear hit object type: " + objID);
		animCounter = 0f;
		if(!interacted)spriteFrame = 0;
		switch(objID) {
			case 0: break;
			case 1: //book
				if(!interacted) {//collider.excludeLayers == excludeSolidLayers
					transform.GetChild(0).gameObject.layer = 0;
					shouldBeSolid = false;
					collider.excludeLayers = excludePassLayers;
					interacted = true;
				}
				else {
					transform.GetChild(0).gameObject.layer = 7;
					shouldBeSolid = true;
					//collider.excludeLayers = excludeSolidLayers;//epi will clip outbounds
					interacted = false;
				}
				dustType = 0; Invoke("PlayDustEffect", 0.3f);
				audioPlayer.PlayOneShot(obj_sfx[0]);
			break;
			case 2: //pencilcase
				dustType = 1;
				if(!interacted) {
					Invoke("ActivateTempChildren", 0.5f);
					//*Play pencil effect*
					interacted = true; Invoke("PlayDustEffect", 0.2f);
				}
				else {
					for(int i = 0; i < transform.childCount; i++) {
						if(transform.GetChild(i).tag == "TempChild") {
							transform.GetChild(i).gameObject.SetActive(false);
						}
					}
					//*Play pencil effect*
					interacted = false; PlayDustEffect();
				}
				audioPlayer.PlayOneShot(obj_sfx[1]);
			break;
			case 3: //pi book
				if(!interacted) {
					transform.GetChild(0).gameObject.layer = 0;
					collider.excludeLayers = excludePassLayers;
					Instantiate(piPrefab, new Vector3(transform.position.x, transform.position.y + 0.5f, 0), new Quaternion(0f, 0f, 0f, 0f));
					interacted = true;
					Destroy(gameObject, 0.5f);
				}
				dustType = 2; PlayDustEffect();
			break;
			case 4: //door
			break;
		}
	}
	private void ActivateTempChildren() {
		for(int i = 0; i < transform.childCount; i++) {
			if(transform.GetChild(i).tag == "TempChild") {
				transform.GetChild(i).gameObject.SetActive(true);
			}
		}
	}
	
	public void AnimateObject() {
		float adv = Time.deltaTime*10f;
		switch(objID) {
			case 0: break;
			case 1: //book
				if(!interacted && spriteFrame != 0) {
					SetObjFrameTo((byte)(3+animCounter), 7, 0);
					animCounter += adv*1.1f;
				}
				else if(interacted) {
					if(spriteFrame < 3) {
						SetObjFrameTo((byte)(animCounter), 7);
						animCounter += adv;
					}
					else spriteFrame = 3;
				}
			break;
			case 2: //pencilcase
				if(!interacted && spriteFrame != 8) {
					SetObjFrameTo((byte)(12+animCounter), 15, 8);
					animCounter += adv*0.9f;
				}
				else if(interacted) {
					if(spriteFrame < 12) {
						SetObjFrameTo((byte)(8+animCounter), 15);
						animCounter += adv*0.9f;
					}
					else spriteFrame = 12;
				}
			break;
			case 3: //pi book
				if(!interacted && spriteFrame != 16) {
					SetObjFrameTo(16);
				}
				else if(interacted) {
					if(spriteFrame < 19) {
						SetObjFrameTo((byte)(16+animCounter));
						animCounter += adv;
					}
					else spriteFrame = 19;
				}
			break;
			case 4: //door
				SetObjFrameTo(20);
			break;
		}
		if(spriteFrame > objSprites.Length-1) spriteFrame = (byte)(objSprites.Length-1);
		sprite.sprite = objSprites[spriteFrame];
	}
	private void SetObjFrameTo(byte toFrame, byte loopWhen = 20, byte loopTo = 0) {
		if(toFrame > spriteFrame) spriteFrame = toFrame;
		if(spriteFrame > loopWhen) spriteFrame = loopTo;
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