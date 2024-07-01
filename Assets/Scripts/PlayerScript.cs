using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
	//Variables de movimiento, salto y el cuerpo del jugador
    public float runSpeed = 2;
    public float jumpSpeed = 3;
	byte jumpCounter = 0;//jump cooldown, para prevenir saltos instantáneos por debajo de plataformas
	public byte jumpCD = 10;
	Rigidbody2D rb2D;
	BoxCollider2D epi_hitbox;
	public Transform RespawnPoint;
	
	//Sprite y animación por frames
	public SpriteRenderer sprite;
	public byte spriteFrame = 0;
	public byte animType = 0;
	float animCounter = 0f;
	public Sprite[] epiSprites;
	
	//Variables para la detección de piso
	public Vector2 boxSize;
	public float castDist;
	public LayerMask floorLayer;
	
	//Pis recolectados
	public byte collectedPis = 0;
	UIScript ui;
	
	//Disparo de lágrima
	public GameObject tearPrefab;
	public float tearSpeed = 1f;
	byte tearCounter = 0;//jump cooldown, para prevenir saltos instantáneos por debajo de plataformas
	public byte tearCD = 15;
	
	//Particle System
	[SerializeField] Transform particle_trans;
	[SerializeField] ParticleSystem walk_dust;
	[SerializeField] ParticleSystem jump_dust;
	[SerializeField] ParticleSystem land_dust;
	[SerializeField] ParticleSystem pi_dust2;
	[SerializeField] ParticleSystem epi_blood;
	[SerializeField] ParticleSystem epi_death;
	
	//Sounds
	public AudioClip[] epi_sfx;
	public AudioSource audioPlayer;
	public AudioSource piAudioPlayer;
	
    void Start() {
        rb2D = GetComponent<Rigidbody2D>();
        epi_hitbox = GetComponent<BoxCollider2D>();
		ui = GameObject.Find("UI").GetComponent<UIScript>();
    }
    void Update() {
		if(!rb2D.simulated) return;
		//moverse a la derecha o izquierda
		bool moveRight = Input.GetKey("d") || Input.GetKey("right");
		bool moveLeft = Input.GetKey("a") || Input.GetKey("left");
        if (moveRight && !moveLeft) {
            rb2D.velocity = new Vector2(runSpeed, rb2D.velocity.y);
			AnimateEpi(1);
        }
        else if (moveLeft && !moveRight) {
            rb2D.velocity = new Vector2(-runSpeed, rb2D.velocity.y);
			AnimateEpi(1);
        }
        else {
            rb2D.velocity = new Vector2(0, rb2D.velocity.y);
			AnimateEpi(0);
        }
		//salto, con una cooldown
		if ((Input.GetKey("space") || Input.GetKey("up") || Input.GetKey("w")) && jumpCounter == 0 && isGrounded() && rb2D.velocity.y <= 0.25f) {
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpSpeed);
			jump_dust.Play();
			jumpCounter = jumpCD;
			audioPlayer.PlayOneShot(epi_sfx[2]);
			AnimateEpi(2);
        }
		if(rb2D.velocity.y < 0.25f && !isGrounded()) {
			AnimateEpi(3);
		}
		
		//disparo de lágrima
		if (Input.GetMouseButtonDown(0) && tearCounter == 0) {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            if (hit.collider != null) {// && hit.collider.CompareTag("Object")
				Vector2 moddedPosition = new Vector2((int)hit.point.x+(hit.point.x < 0 ? -0.5f : 0.5f), (int)hit.point.y+(hit.point.y < 0 ? -0.5f : 0.5f));
				Vector2 toShootTo = moddedPosition - new Vector2(transform.position.x, transform.position.y);
				toShootTo.Normalize();
                InstantiateTear(toShootTo * tearSpeed);
				audioPlayer.PlayOneShot(epi_sfx[5]);
				
				if(toShootTo.x > 0) sprite.flipX = true;
				if(toShootTo.x < 0) sprite.flipX = false;
				//*Efecto de disparar lágrima*
				tearCounter = tearCD;
				AnimateEpi(4);
				Debug.DrawRay(moddedPosition, toShootTo*3, Color.cyan, 2.5f);
            }
        }
		
		if(transform.position.y < -10f) transform.position = RespawnPoint.position;
		//girar el sprite según dirección
		if(rb2D.velocity.x > 0) sprite.flipX = true;
		if(rb2D.velocity.x < 0) sprite.flipX = false;
		UpdateAnimateEpi();
    }
	
	void FixedUpdate() {
		if(jumpCounter > 0) jumpCounter--;
		if(tearCounter > 0) tearCounter--;
	}
	public bool isGrounded() {
		//función de revisión del toque de piso
		if(Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDist, floorLayer)) {
			return true;
		}
		else return false;
	}
	private void OnDrawGizmos() {//ver la detección en el editor
		Gizmos.DrawWireCube(transform.position-transform.up*castDist, boxSize);
	}
	
	private void OnTriggerEnter2D(Collider2D collision) {
		//recolectar PI
		if(collision.gameObject.CompareTag("Collect")) {
			piAudioPlayer.pitch = 1f + collectedPis/8f;
			collectedPis++;
			pi_dust2.Play();
			//audioPlayer.PlayOneShot(epi_sfx[4]);//was pi collect sfx, now is bark
			piAudioPlayer.Play();
			Destroy(collision.gameObject, 0f);
			ui.UpdatePisDisplayed(collectedPis);
		}
		//being hit
		if (collision.gameObject.layer == 8) {
			collision.transform.parent.gameObject.GetComponent<DangerScript>().JustHitPlayer();
            HitAndRestartSequence();
        }
	}
	
	//función de creación y disparo de lágrima
    void InstantiateTear(Vector2 velocity) {
		Instantiate(tearPrefab, transform.position, new Quaternion(0f, 0f, 0f, 0f)).GetComponent<Rigidbody2D>().velocity = velocity;
        //GameObject tear = Instantiate(tearPrefab, position, Quaternion.identity);
        //tear.AddComponent<Tear>();
    }
	
	public void AnimateEpi(byte tryAnimID) {//decide qué ID de animación colocar
		if(	animType == 2 && tryAnimID == 3 || 
			animType == 3 || tryAnimID == 4 || 
			animType == 1 && tryAnimID == 0 || 
			tryAnimID == 2 || 
			animType < tryAnimID) 
		{
			animCounter = 0f;
			animType = tryAnimID;
		}
	}
	public void UpdateAnimateEpi() {//ejecuta la elección del frame según la ID de animación
		float adv = Time.deltaTime*6f;
		byte oldFrame = spriteFrame;
		switch(animType) {
			case 0: //idle 0
				spriteFrame = 0; 
				animCounter = 0f;
			break;
			case 1: //caminar 0, 1, 2, 3
				if(animCounter < 4f) {
					spriteFrame = (byte)animCounter;
					animCounter += adv*1.5f;
				}
				else {
					animCounter = 0f;
					animType = 0;
				}
			break;
			case 2: //salto 4, 5
				if(animCounter < 4f) {
					spriteFrame = (byte)(animCounter/2f+4);
					animCounter += adv*1.5f;
				}
				else {
					animCounter = 0f;
					animType = 0;
				}
				
			break;
			case 3: //caída 6, 7
				if(animCounter < 4f) {
					spriteFrame = (byte)(animCounter/2f+6);
					animCounter += adv;
				}
				else {
					animCounter = 0f;
					animType = 0;
				}
			break;
			case 4: //disparo de lágrima 8, 9, 10, 11
				if(animCounter < 4f) {
					spriteFrame = (byte)(animCounter+8);
					animCounter += adv*1.25f;
				}
				else {
					animCounter = 0f;
					animType = 0;
				}
			break;
		}
		if(spriteFrame > epiSprites.Length-1) spriteFrame = (byte)(epiSprites.Length-1);
		sprite.sprite = epiSprites[spriteFrame];
		
		if(oldFrame != spriteFrame) {
			UpdateParticles((oldFrame == 6 || oldFrame == 7) && (spriteFrame != 6 && spriteFrame != 7));
			if(animType == 1 && spriteFrame == 1) { audioPlayer.PlayOneShot(epi_sfx[0]); }//spriteFrame % 3 == 0 && spriteFrame != 3
		}
	}
	
	public void HitAndRestartSequence() {//esto debe ejecutarse cuando el jugador es golpeado por el gato
		Debug.Log("Owie");
		epi_blood.Play();
		epi_hitbox.enabled = rb2D.simulated = false;
		sprite.sprite = epiSprites[6];
		audioPlayer.PlayOneShot(epi_sfx[6]);
		Time.timeScale = (Time.timeScale + 0.5f)/2f;
		Invoke("DeathAnim", 0.5f);
	}
	private void DeathAnim() {
		epi_death.Play();
		audioPlayer.PlayOneShot(epi_sfx[7]);
		sprite.enabled = false;
		Time.timeScale = 1f;
		Invoke("Respawn", 2f);
	}
	public void Respawn() {
		audioPlayer.PlayOneShot(epi_sfx[4]);
		epi_hitbox.enabled = rb2D.simulated = sprite.enabled = true;
		transform.position = RespawnPoint.position;
	}
	
	public void UpdateParticles(bool fell) {
		if(fell) {
			land_dust.Play();
			audioPlayer.PlayOneShot(epi_sfx[3]);
		}
		else if(animType == 1) {
			walk_dust.Play();
		}
		if(sprite.flipX) particle_trans.rotation = new Quaternion(0f, 90f, 0f, 0f);
		else particle_trans.rotation = new Quaternion(0f, 0f, 0f, 0f);
	}
}
