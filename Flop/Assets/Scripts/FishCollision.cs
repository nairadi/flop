using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishCollision : MonoBehaviour
{
    private void OnCollisionStay2D(Collision2D collision)
    {
		if (collision.gameObject.CompareTag("ConvayerBelt")) {
			Launcher.convayerAllowLaunch = true;
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("ConvayerBelt")) {
			Launcher.convayerAllowLaunch = false;
		}
	}
}