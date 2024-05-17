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
	
	//Variables para la detección de piso
	public Vector2 boxSize;
	public float castDist;
	public LayerMask floorLayer;
	
    void Start() {
        rb2D = GetComponent<Rigidbody2D>();
    }
    void Update() {
        if (Input.GetKey("d") || Input.GetKey("right")) {
            rb2D.velocity = new Vector2(runSpeed, rb2D.velocity.y);
        }
        else if (Input.GetKey("a") || Input.GetKey("left")) {
            rb2D.velocity = new Vector2(-runSpeed, rb2D.velocity.y);
        }
        else {
            rb2D.velocity = new Vector2(0, rb2D.velocity.y);
        }
		if ((Input.GetKey("space") || Input.GetKey("up")) && isGrounded() && jumpCounter == 0) {
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpSpeed);
			jumpCounter = jumpCD;
        }
    }
	void FixedUpdate() {
		if(jumpCounter > 0) jumpCounter--;
	}
	public bool isGrounded() {
		if(Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDist, floorLayer)) {
			return true;
		}
		else return false;
	}
	private void OnDrawGizmos() {
		Gizmos.DrawWireCube(transform.position-transform.up*castDist, boxSize);
	}
}
