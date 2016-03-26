using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {
    Vector3 diff;

    public GameObject target;
    public float followSpeed;

	// Use this for initialization
	void Start () {
        diff = target.transform.position - transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position = Vector3.Lerp(transform.position, target.transform.position - diff, Time.deltaTime * followSpeed);
	}
}
