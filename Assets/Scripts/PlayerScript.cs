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
	
    void Start() {
        rb2D = GetComponent<Rigidbody2D>();
		ui = GameObject.Find("UI").GetComponent<UIScript>();
    }
    void Update() {
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
			jumpCounter = jumpCD;
			AnimateEpi(2);
        }
		if(rb2D.velocity.y < 0.25f && !isGrounded()) {
			AnimateEpi(3);
		}
		
		//disparo de lágrima
		if (Input.GetMouseButtonDown(0) && tearCounter == 0) {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("Object")) {
				Vector2 moddedPosition = new Vector2((int)hit.point.x+(hit.point.x < 0 ? -0.5f : 0.5f), (int)hit.point.y+(hit.point.y < 0 ? -0.5f : 0.5f));
				Vector2 toShootTo = moddedPosition - new Vector2(transform.position.x, transform.position.y);
				toShootTo.Normalize();
                InstantiateTear(toShootTo * tearSpeed);
				
				if(toShootTo.x > 0) sprite.flipX = true;
				if(toShootTo.x < 0) sprite.flipX = false;
				//*Efecto de disparar lágrima*
				tearCounter = tearCD;
				AnimateEpi(4);
				Debug.DrawRay(moddedPosition, toShootTo*3, Color.cyan, 2.5f);
            }
        }
		
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
		if(collision.gameObject.CompareTag("Collect")) {
			collectedPis++;
			//*Efecto de recolectar pi*
			Destroy(collision.gameObject, 0f);
			ui.UpdatePisDisplayed(collectedPis);
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
		switch(animType) {
			case 0: //idle
				animCounter = 0f;
				spriteFrame = 0; 
			break;
			case 1: //caminar
				if(animCounter < 4f) {
					animCounter += adv*1.5f;
					spriteFrame = (byte)animCounter;
				}
				else {
					animCounter = 0f;
					animType = 0;
				}
			break;
			case 2: //salto
				if(animCounter < 4f) {
					animCounter += adv*1.5f;
					spriteFrame = (byte)(animCounter/2f+4); 
				}
				else {
					animCounter = 0f;
					animType = 0;
				}
				
			break;
			case 3: //caída
				if(animCounter < 4f) {
					animCounter += adv;
					spriteFrame = (byte)(animCounter/2f+6); 
				}
				else {
					animCounter = 0f;
					animType = 0;
				}
			break;
			case 4: //disparo de lágrima
				if(animCounter < 4f) {
					animCounter += adv;
					spriteFrame = (byte)(animCounter+8); 
				}
				else {
					animCounter = 0f;
					animType = 0;
				}
			break;
		}
		if(spriteFrame > epiSprites.Length-1) spriteFrame = (byte)(epiSprites.Length-1);
		sprite.sprite = epiSprites[spriteFrame];
	}
}
