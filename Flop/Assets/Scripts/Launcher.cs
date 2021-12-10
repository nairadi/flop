using UnityEngine;

public class Launcher : MonoBehaviour
{
	public Rigidbody2D fishRigidbody;
	public float LAUNCH_FORCE_MULT = 0.02f;
	public float MAX_LAUNCH_MAGNITUDE = 28.0f;
	public float VELOCITY_ERROR_LIMIT = 0.5f;
	public float TIME_STABLE_DETERMINATION = 1f;
	public float AIMER_LENGTH_MULT = 0.00005f;
	public float timeElapsedStable;
	float startStable;


	LineRenderer aimer;
	public GameObject aimerGO;
	GameObject newAimerGO;
	bool drawAimer = false;
	bool mouseDownUpToggle = false;
	public static bool convayerAllowLaunch = false;

	Vector3 startPosition;
	Vector3 endPosition;

    public bool canLaunch = true;

	void OnMouseDown()
	{
        if (!canLaunch) return;

        if (!mouseDownUpToggle) {
			float startTime = Time.time;
			if (fishRigidbody != null && timeElapsedStable >= TIME_STABLE_DETERMINATION) {
				startPosition = Input.mousePosition;
				drawAimer = true;

				newAimerGO = Instantiate(aimerGO);
				aimer = newAimerGO.GetComponent<LineRenderer>();
				mouseDownUpToggle = true;
			}
		}
	}

	void OnMouseUp()
	{
        if (!canLaunch) return;

		if (mouseDownUpToggle) {
			drawAimer = false;
			Destroy(newAimerGO);
            if (fishRigidbody != null && (timeElapsedStable >= TIME_STABLE_DETERMINATION || convayerAllowLaunch)) {
				endPosition = Input.mousePosition;

				Vector3 direction = (startPosition - endPosition);
				float magnitude = direction.magnitude;

				direction = direction.normalized * magnitude * LAUNCH_FORCE_MULT;
				direction = Vector3.ClampMagnitude(direction, MAX_LAUNCH_MAGNITUDE);

				ForceMode2D force = ForceMode2D.Impulse;
				fishRigidbody.AddForce(direction, force);
				mouseDownUpToggle = false;
			}
		}
	}



	void Update()
	{
        if (!canLaunch) return;

		if(fishRigidbody != null && (fishRigidbody.velocity.magnitude <= VELOCITY_ERROR_LIMIT || convayerAllowLaunch)) {
			timeElapsedStable = Time.time - startStable;
		} else {
			timeElapsedStable = 0f;
			startStable = Time.time;
		}
		if(aimer != null) {
			if (drawAimer) {
                aimer.SetPosition(0, fishRigidbody.transform.position);
				Vector3 direction = (startPosition - Input.mousePosition);
				float magnitude = direction.magnitude;
				magnitude = Mathf.Min(MAX_LAUNCH_MAGNITUDE, magnitude);
				direction *= magnitude * AIMER_LENGTH_MULT;
				aimer.SetPosition(1, (fishRigidbody.transform.position + direction));

			} else {
				Destroy(newAimerGO);
			}
		}
	}
}
