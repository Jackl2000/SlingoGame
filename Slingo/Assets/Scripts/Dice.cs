using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class Dice : MonoBehaviour
{

    public Transform[] diceFaces;
    public Rigidbody rb;

    private int _diceIndex = -1;

    private bool hasStoppedRolling;
    private bool delayFinished;

    public static UnityAction<int, int> OnDiceResult;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    /**
     * Roll the dice, once it's speed is equals to 0, it has stopped rolling.
     * 
     * 
     */
    public void Update()
    {
        if (!delayFinished)
        {
            delayFinished = true;
                return;
        }

        if (!hasStoppedRolling && rb.velocity.sqrMagnitude == 0f)
        {
            hasStoppedRolling = true;
            GetNumberOnTopFace();
        }
        
    }

    [ContextMenu(itemName: "Get Top Face")]
    public int GetNumberOnTopFace()
    {
        if (diceFaces == null) return -1;

        var topFace = 0;
        var lastYPosition = diceFaces[0].position.y;


        for (int i = 0; i < diceFaces.Length; i++)
        {
            if (diceFaces[i].position.y > lastYPosition)
            {
                lastYPosition = diceFaces[i].position.y;
                topFace = i;
            }
        }

        Debug.Log($"Dice result {topFace + 1}");


        OnDiceResult?.Invoke(_diceIndex, topFace + 1);

        return topFace + 1;

    }

    public void RollDice(float throwForce, float rollForce, int i)
    {
        _diceIndex = i;
        var randomVariance = Random.Range(-1f, 1f);
        rb.AddForce(transform.forward * (throwForce + randomVariance), ForceMode.Impulse);


        var randX = Random.Range(0f, 2f);
        var randY = Random.Range(0f, 2f);
        var randZ = Random.Range(0f, 2f);


        rb.AddTorque(new Vector3(randX, randY, randZ) * (rollForce + randomVariance), ForceMode.Impulse);


        StartCoroutine(DelayResult());

    }

    private IEnumerator DelayResult()
    {
        const float velocityThreshold = 0.001f;
        const float delayTime = 1f;

        float timer = 0f;

        while(timer < delayTime)
        {
            if (rb.velocity.sqrMagnitude < velocityThreshold)
            {
                hasStoppedRolling = true;
                GetNumberOnTopFace();
                yield break;
            }

            timer += Time.deltaTime;
            yield return null;
        }
   
    }
}
