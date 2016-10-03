using UnityEngine;
using System.Collections;

// モデルをつかんだり離したりする
public class HoldModel : MonoBehaviour {

	const float HoldRange = 1f;
	GameObject holdTargetObject;
	GameObject holdObject = null;
	GameObject model;

	void Start()
	{
		model = transform.FindChild ("Model").gameObject;
	}

	void Update()
	{
		UpdateTargetObject();
		UpdateHoldState();
		UpdateModelVisibility ();
	}

	void UpdateTargetObject()
	{
		if( holdTargetObject == null ) return;

		float distance = (transform.position - holdTargetObject.transform.position).magnitude;
		if( distance > HoldRange ){
			Debug.Log(holdTargetObject.name + "との距離が離れたので掴むターゲットから除外");
			holdTargetObject = null;
		}
	}


	/// <summary>
	/// 掴む候補オブジェクトがコライダに侵入したら、掴むターゲットを更新
	/// </summary>
	void OnTriggerEnter (Collider col)
	{
		if( !col.tag.Equals("Model") ){
			return;
		}
		Debug.Log (col.gameObject.name + "をつかむターゲットに指定");
		holdTargetObject = col.gameObject;
	}

	/// <summary>
	/// 掴み状態を更新
	/// </summary>
	void UpdateHoldState()
	{
		SteamVR_TrackedObject trackedObject = GetComponent<SteamVR_TrackedObject>();
		var device = SteamVR_Controller.Input((int) trackedObject.index);

		if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger)) {
			Hold ();
		}
		if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger)) {
			Release ();
		}
	}

	/// <summary>
	/// 範囲内に掴むものがあったら掴む
	/// </summary>
	void Hold()
	{
		Debug.Log("Hold");
		if( holdTargetObject == null ){
			Debug.Log("掴む候補なし");
			return;
		} 

		// 掴んでいるオブジェクトを子要素にする
		Debug.Log( holdTargetObject.name + "を掴んだ");
		holdObject = holdTargetObject;
		holdObject.transform.SetParent( transform );
		holdObject.transform.position = Vector3.zero;
		holdObject.transform.rotation = Quaternion.identity;
	}

	/// <summary>
	/// ものを掴んでいたら離す
	/// </summary>
	void Release()
	{
		Debug.Log("Release");
		if( holdObject == null ){
			Debug.Log("掴んでいるオブジェクトなし");
			return;
		}

		Debug.Log( holdObject.name + "を離した");
		holdObject.transform.SetParent( null );
		// holdObject.BackToOriginPosition();
		holdObject = null;
	}

	void UpdateModelVisibility()
	{
		model.SetActive (holdObject == null);
	}
}
