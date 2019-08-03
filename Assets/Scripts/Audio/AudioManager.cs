using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public static AudioManager instance;

	[SerializeField] private AudioSource bigDeathSound;

    void Start() {
        if (instance == null) {
			instance = this;
		}
    }


    public void PlayBigDeathSound() {
		bigDeathSound.Play();
	}
}
