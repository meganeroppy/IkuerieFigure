using UnityEngine;
using System.Collections;


// 床の高さなどを調節する
// プレイスペースのキャリブレーションが正しく設定できていれば、床の高さも正しいはず？
public class Floor : MonoBehaviour {

	// float floorPos = -1.48f;
	float floorPos = -1.05f; // 20161003

	// Use this for initialization
	void Start () {
		transform.position = Vector3.up * floorPos;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
