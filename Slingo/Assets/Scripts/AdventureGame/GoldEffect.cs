using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldEffect : MonoBehaviour
{
    public GameObject target;
    private ParticleSystem ps;
    private bool playing = false;
    private float speed = 1500f;
    private void Start()
    {
        ps = GetComponentInChildren<ParticleSystem>();
    }

    public void PlayParticleCoins()
    {
        ps.Play();
        playing = true;
    }

    private void FixedUpdate()
    {
        if(playing)
        {
            Debug.Log(Vector3.Distance(ps.transform.position, target.transform.position));
            if (Vector3.Distance(ps.transform.position, target.transform.position) < 0.1f)
            {
                ps.Stop();
                playing=false;
            }
            else ps.gameObject.transform.position = Vector3.MoveTowards(ps.transform.position, target.transform.position, speed * Time.deltaTime);
        }
    }
}
