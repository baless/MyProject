using UnityEngine;
using System.Collections;

public class DestroyParticle : MonoBehaviour {

    ParticleSystem GetParticle;

	// Use this for initialization
	void Start () {
        GetParticle = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
        if (GetParticle.isPlaying == false)
            Destroy(gameObject);
	}
}
