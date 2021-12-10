using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour {
    public Vector2 conveyorBeltForce;

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
			collision.rigidbody.AddForce(conveyorBeltForce);
			}
    }
}
