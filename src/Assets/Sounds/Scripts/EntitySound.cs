using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySound : MonoBehaviour {
    // Start is called before the first frame update
    AudioSource source;
    [SerializeField]
    AudioClip attackClip;
    [SerializeField]
    AudioClip damageClip;
    [SerializeField]
    AudioClip deathClip;

    void Start() {
        source = GetComponent<AudioSource>();
    }

    public void PlayAttack() {
        if (source == null) {
            return;
        }
        if (source.isPlaying) {
            source.Stop();
        }
        source.clip = attackClip;
        source.Play();
    }

    public void PlayDamage() {
        if (source == null) {
            return;
        }
        if (source.isPlaying) {
            source.Stop();
        }
        source.clip = damageClip;
        source.Play();
    }

    public void PlayDeath() {
        if (source == null) {
            return;
        }
        if (source.isPlaying) {
            source.Stop();
        }
        source.clip = deathClip;
        source.Play();
    }
}
