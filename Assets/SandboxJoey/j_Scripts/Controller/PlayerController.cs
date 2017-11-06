using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float xSpeed;
	public float zSpeed;
	public float ySpeed;
	
	void FixedUpdate()
	{
		var x = Input.GetAxis("Horizontal") * Time.deltaTime * xSpeed;
		var z = Input.GetAxis("Vertical") * Time.deltaTime * zSpeed;
		var y = Input.GetAxis ("Jump") * Time.deltaTime * ySpeed;

		transform.Rotate (x, 0, 0);
		transform.Translate (0, z, 0);
		transform.Translate (0, 0, y);

	}
}