using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyEffect : MonoBehaviour
{
    public GameObject collectButton;
    [HideInInspector] public bool playAnimation = false;

    [SerializeField] private float speed = 15f;
    private ParticleSystem ps;
    private GameObject psParent;
    private ParticleSystem.Particle[] particles = new ParticleSystem.Particle[500];

    private List<GameObject> path = new List<GameObject>();
    private int currentPathIndex = 0;

    private bool play = false;

    private void Start()
    {
        ps = transform.GetChild(4).GetComponentInChildren<ParticleSystem>();
        psParent = ps.transform.parent.gameObject;
        GameObject[] paths = { ps.transform.parent.GetChild(1).gameObject, ps.transform.parent.GetChild(2).gameObject, ps.transform.parent.GetChild(3).gameObject, ps.transform.parent.GetChild(4).gameObject };
        foreach (GameObject pathObject in paths)
        {
            path.Add(pathObject);
        }
    }

    private void LateUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Y) && !playAnimation)
        {
            playAnimation = true;
        }

        if(playAnimation)
        {
            ps.transform.transform.parent.transform.SetParent(transform.parent, true);
            ps.Play();
            playAnimation = false;
            play = true;
        }

        if (play)
        {
            ParticlePath(GetTargetOnPath());
        }
    }

    private GameObject GetTargetOnPath()
    {
        GameObject currentTarget = null;
        if (currentPathIndex >= path.Count)
        {
            currentTarget = collectButton;
        }
        else
        {
            currentTarget = path[currentPathIndex];
        }
        
        return currentTarget;
    }

    private void ParticlePath(GameObject target)
    {
        ps.gameObject.transform.LookAt(target.transform.position);
        ps.gameObject.transform.position = Vector3.MoveTowards(ps.transform.position, target.transform.position, speed * Time.deltaTime);
        if (Vector3.Distance(ps.transform.position, target.transform.position) < 0.1f)
        {
            if (target != collectButton)
            {
                currentPathIndex++;
            }
            else
            {
                play = false;
                ps.Stop();
                currentPathIndex = 0;
                ps.transform.parent.transform.SetParent(transform);
                StartCoroutine(Delay());
            }
        }
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(1);
        ps.transform.localPosition = Vector3.zero;
    }
}