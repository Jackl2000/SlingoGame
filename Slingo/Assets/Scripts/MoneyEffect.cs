using System.Collections;
using UnityEngine;

public class MoneyEffect : MonoBehaviour
{
    public GameObject collectButton;
    [HideInInspector] public bool playAnimation = false;

    [SerializeField] private float speed = 15f;
    private ParticleSystem ps;
    private ParticleSystem.Particle[] particles = new ParticleSystem.Particle[500];

    private bool play = false;

    private void Start()
    {
        ps = transform.GetChild(4).GetComponentInChildren<ParticleSystem>();
    }

    private void LateUpdate()
    {
        if(playAnimation)
        {
            ps.transform.parent.transform.SetParent(transform.parent, true);
            ps.Stop();
            ps.Play();
            playAnimation = false;
            play = true;
        }

        if (play)
        {
            ps.gameObject.transform.position = Vector3.MoveTowards(ps.transform.position, collectButton.transform.position, speed * Time.deltaTime);
            if (Vector3.Distance(ps.transform.position, collectButton.transform.position) < 0.1f)
            {
                play = false;
                ps.Stop();
                ps.transform.parent.transform.SetParent(transform, true);
                StartCoroutine(Delay());
            }
        }
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.3f);
        ps.transform.localPosition = Vector3.zero;
    }
}