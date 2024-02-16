using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class MoneyEffect : MonoBehaviour
{
    public GameObject collectButton;
    [HideInInspector] public bool playAnimation = false;
    public GridCheck gridCheck;

    [SerializeField] private float speed = 15f;
    public ParticleSystem ps;
    public GameObject pathParent;
    public spin spin;

    private List<GameObject> path = new List<GameObject>();
    private int currentPathIndex = 0;

    private Vector3 startingPosition;
    private Quaternion startingRotation;
    private bool play = false;

    private void Start()
    {
        GameObject[] paths = { pathParent.transform.GetChild(0).gameObject, pathParent.transform.GetChild(1).gameObject, pathParent.transform.GetChild(2).gameObject, pathParent.transform.GetChild(3).gameObject, pathParent.transform.GetChild(1).gameObject };
        foreach (GameObject pathObject in paths)
        {
            path.Add(pathObject);
        }
        startingPosition = ps.gameObject.transform.position;
        startingRotation = ps.gameObject.transform.rotation;
    }

    private void LateUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Y) && !playAnimation)
        {
            playAnimation = true;
        }

        if(playAnimation && gridCheck.slingoAnimationFinished)
        {
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
                StartCoroutine(Delay());
            }
        }
    }

    private IEnumerator Delay()
    {
        GridCheck gridCheck = GetComponentInParent<GridCheck>();
        TextMeshProUGUI text = collectButton.GetComponentInChildren<TextMeshProUGUI>();

        if(gridCheck.slingoCount >= 3)
        {
            text.text = "Modtag " + UIManager.Instance.DisplayMoney(gridCheck.rewards[gridCheck.slingoCount]);
        }
        if(spin.spinLeft <= 0) text.color = Color.white;
        else text.color = Color.gray;
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(UIManager.Instance.TextColorAnimation(this, text, 0.03f, new Color[] { Color.yellow, Color.yellow, Color.white, Color.yellow }, text.color, 2));
        yield return new WaitForSeconds(1);
        ps.gameObject.transform.position = startingPosition;
        ps.gameObject.transform.rotation = startingRotation;
    }
}