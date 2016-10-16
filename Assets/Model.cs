using UnityEngine;
using System.Collections;

public class Model : MonoBehaviour {

	/// <summary>
	/// 掴まれている時のスケール
	/// </summary>
	const float scaleInHeld = 0.1f;

	Vector3 originPos;
	Vector3 originScl;
	Quaternion originRot;

	Rigidbody rigidBody;
	Vector3 heldRotDiff = new Vector3(-90f, 30f, -215f);

	void Start ()
	{
		originPos = transform.position;
		originScl = transform.localScale;
		originRot = transform.rotation;

		rigidBody = GetComponent<Rigidbody> ();
	}
	
	void Update ()
	{
		UpdateTransform();
		UpdateRigidBoduEnable ();
	}

	void UpdateTransform()
	{
		if( transform.parent == null ){
			transform.position = originPos;
			transform.localScale = originScl;
			transform.rotation = originRot;
		}else{
			// transform.localScale = Vector3.one * scaleInHeld;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.Euler (heldRotDiff);
		}
	}

	void UpdateRigidBoduEnable ()
	{
		rigidBody.useGravity = transform.parent == null;
	}

}
