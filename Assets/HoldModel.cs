using UnityEngine;
using System.Collections;

// モデルをつかんだり離したりする
public class HoldModel : MonoBehaviour {

	/// <summary>
	/// 何かをつかんでいるときのオブジェクトのスケール
	/// </summary>
	const float scaleInHeld = 0.1f;

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

		// タッチパッドが押されたかどうか
		if (device.GetPressDown (SteamVR_Controller.ButtonMask.Touchpad)) {
			Vector2 pos = device.GetAxis ();
			if (pos.x > 0.5f) {
				ChangeFObjectScale (true);
			} else if (pos.x < 0.5f) {
				ChangeFObjectScale (false);
			}
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
		holdObject.transform.localScale = Vector3.one * scaleInHeld;
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

	/// <summary>
	/// コントローラのビジュアルの表示 / 非表示を切り替える
	/// </summary>
	void UpdateModelVisibility()
	{
		model.SetActive (holdObject == null);
	}

	/// <summary>
	/// つかんでいるオブジェクトの大きさを変更する
	/// </summary>
	/// <param name="largen">大きくするか？</param>
	void ChangeFObjectScale(bool largen){
		if (holdObject == null) {
			Debug.LogWarning ("つかんでいるオブジェクトなし");
			return;
		}

		// 一回で変更されるスケール値
		float unitScale = 0.01f;

		if (!largen) {
			unitScale *= -1f;
		} 

		float currentScale = holdObject.transform.localScale.x;
		holdObject.transform.localScale = Vector3.one * ( currentScale + unitScale );

	}
}
