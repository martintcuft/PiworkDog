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
	public SpriteRenderer sprite;
	
	//Variables para la detección de piso
	public Vector2 boxSize;
	public float castDist;
	public LayerMask floorLayer;
	
	//Pis recolectados
	public byte collectedPis = 0;
	UIScript ui;
	
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
        }
        else if (moveLeft && !moveRight) {
            rb2D.velocity = new Vector2(-runSpeed, rb2D.velocity.y);
        }
        else {
            rb2D.velocity = new Vector2(0, rb2D.velocity.y);
        }
		//salto, con una cooldown
		if ((Input.GetKey("space") || Input.GetKey("up") || Input.GetKey("w")) && isGrounded() && jumpCounter == 0) {
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpSpeed);
			jumpCounter = jumpCD;
        }
		
		if(rb2D.velocity.x > 0) sprite.flipX = true;
		if(rb2D.velocity.x < 0) sprite.flipX = false;
    }
	void FixedUpdate() {
		if(jumpCounter > 0) jumpCounter--;
	}
	public bool isGrounded() {
		//función de revisión del toque de piso
		if(Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDist, floorLayer) && rb2D.velocity.y <= 0.25f) {
			return true;
		}
		else return false;
	}
	private void OnDrawGizmos() {
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
}
