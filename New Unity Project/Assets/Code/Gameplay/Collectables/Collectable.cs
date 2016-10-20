using UnityEngine;
using System.Collections;

public class Collectable : MonoBehaviour {

	public CollectableData data;

	[SerializeField]
	private Transform rendererTransform;
	private Transform trsf;
	private Rigidbody rgdBody;

	void Awake () {
		trsf = transform;
		rgdBody = GetComponent<Rigidbody>();
	}

	void Update()
	{
		rendererTransform.Rotate(Vector3.up*data.rotationSpeed*Time.deltaTime, Space.World);
	}

	void OnEnable()
	{
		StartCoroutine(collectableCoroutine());
	}

	public void grab()
	{
		StopAllCoroutines();
		gameObject.SetActive(false);
	}

	IEnumerator collectableCoroutine()
	{
		trsf.rotation = Quaternion.Euler(new Vector3(0.0f,Random.Range(-0.0f, 360.0f),0.0f));
		Vector3 rs = Random.insideUnitSphere;
		rgdBody.AddForce(new Vector3(rs.x, Mathf.Abs(rs.y), rs.z) * data.initialSpeed, ForceMode.VelocityChange);
		WaitForEndOfFrame wait = new WaitForEndOfFrame();

		float value = 0.0f;
		while(value < 1.0f)
		{
			value += Time.deltaTime*data.animSpeed;
			float ev = data.animationrCurve.Evaluate(value);

			trsf.localScale = new Vector3(ev, ev, ev);

			yield return wait;
		}

		yield return new WaitForSeconds(data.duration);

		value = 1.0f;
		while(value > 0.0f)
		{
			value -= Time.deltaTime*data.animSpeed;
			float ev = data.animationrCurve.Evaluate(value);

			trsf.localScale = new Vector3(ev, ev, ev);

			yield return wait;
		}

		gameObject.SetActive(false);
	}
}
