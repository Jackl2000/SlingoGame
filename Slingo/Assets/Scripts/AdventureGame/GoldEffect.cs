using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            if (Vector3.Distance(ps.transform.position, target.transform.position) < 0.1f)
            {
                ps.Stop();
                playing=false;
                target.GetComponent<Animator>().SetBool("MoneyInBag", true);
                StopAnimation();
                GetComponent<ChestChance>().TotalRewards();
            }
            else ps.gameObject.transform.position = Vector3.MoveTowards(ps.transform.position, target.transform.position, speed * Time.deltaTime);
        }
    }

    private async void StopAnimation()
    {
        await Task.Delay(1500);
        if(target != null) target.GetComponent<Animator>().SetBool("MoneyInBag", false);

    }
}