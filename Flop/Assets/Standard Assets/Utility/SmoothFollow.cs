using UnityEngine;

namespace UnityStandardAssets.Utility
{
	public class SmoothFollow : MonoBehaviour {
        // The target we are following
        public Transform target;

        public Bounds bounds;

		// The distance in the x-z plane to the target
		[SerializeField]
		private float distance = 10.0f;
		// the height we want the camera to be above the target
		[SerializeField]
		private float offsetYDefault;

        [SerializeField]
        private float offsetXDefault;

		[SerializeField]
		private float dampingY;

        [SerializeField]
        private float dampingX;

		private float offsetY;
		private float offsetX;
		private float targetOrthographicSize;
		private float initialOrthographicCameraSize;
		private Camera gameCamera;

		void Start()
		{
			gameCamera = gameObject.GetComponent<Camera>();
			initialOrthographicCameraSize = gameCamera.orthographicSize;
			ResetCamera();
		}

		public void CenterCamera()
		{
			offsetY = 0;
			offsetX = 0;
			targetOrthographicSize = 1;
		}

		public void ResetCamera()
		{
			offsetY = offsetYDefault;
			offsetX = offsetXDefault;
			targetOrthographicSize = initialOrthographicCameraSize;
		}

		// Update is called once per frame
		void FixedUpdate()
		{
			// Early out if we don't have a target
			if (!target)
				return;
                
			var wantedY = target.position.y + offsetY;
            var wantedX = target.position.x + offsetX;

			var currentY = transform.position.y;
            var currentX = transform.position.x;

			// Damp the height
			currentY = Mathf.Lerp(currentY, wantedY, dampingY * Time.deltaTime);
            currentX = Mathf.Lerp(currentX, wantedX, dampingX * Time.deltaTime);

			// Set the height of the camera
			transform.position = new Vector3(currentX ,currentY , transform.position.z);

			// Set the size (zoom)
			gameCamera.orthographicSize = Mathf.Lerp(gameCamera.orthographicSize, targetOrthographicSize, Time.deltaTime);
            ClampToBounds();
		}

        public void ClampToBounds() {
            if (bounds == null) return;

            var currentY = transform.position.y;
            var currentX = transform.position.x;

            var halfHeight = Camera.main.orthographicSize;
            var halfWidth = halfHeight * Camera.main.aspect;

            if (currentX < bounds.min.x + halfWidth) {
                transform.position = new Vector3(bounds.min.x + halfWidth, transform.position.y, transform.position.z);
            }
            else if (currentX > bounds.max.x - halfWidth) {
                transform.position = new Vector3(bounds.max.x - halfWidth, transform.position.y, transform.position.z);
            }

            if (currentY < bounds.min.y + halfHeight) {
                transform.position = new Vector3(transform.position.x, bounds.min.y + halfHeight, transform.position.z);
            }
            else if (currentY > bounds.max.y - halfHeight) {
                transform.position = new Vector3(transform.position.x, bounds.max.y - halfHeight, transform.position.z);
            }
        }
    }
}