using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkrabeHole : MonoBehaviour
{
    public GameObject holePrefab;
    public SkrabeSelector selector;

    private GameObject selectedCard;
    private List<GameObject> holes = new List<GameObject>();

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (holes.Count > 100)
            {
                selector.ResetSelctedCard();
                selectedCard = null;
                foreach (GameObject go in holes)
                {
                    Destroy(go);
                }
                holes.Clear();
            }
            else
            {
                if (selectedCard == null)
                {
                    GameObject hole = Instantiate(holePrefab, Input.mousePosition, Quaternion.identity, transform);
                    selectedCard = selector.SelectCard(hole);
                    if (selectedCard != null) holes.Add(hole);
                    else Destroy(hole);
                }
                else
                {
                    holes.Add(Instantiate(holePrefab, Input.mousePosition, Quaternion.identity, transform));
                }

            }
        }
    }
}
