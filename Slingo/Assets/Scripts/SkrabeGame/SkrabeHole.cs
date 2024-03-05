using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkrabeHole : MonoBehaviour
{
    public GameObject holePrefab;
    public SkrabeSelector selector;

    public GameObject selectedCard;
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
                else if(CheckForIntergration())
                {
                    holes.Add(Instantiate(holePrefab, Input.mousePosition, Quaternion.identity, transform));
                }
            }
        }
    }

    private bool CheckForIntergration()
    {
        Vector3[] corners = new Vector3[4];
        selectedCard.GetComponent<RectTransform>().GetWorldCorners(corners);

        List<Vector3> screenSpaceCorners = new List<Vector3>();

        foreach(Vector3 corner in corners)
        {
            Vector3 screenSpace = Camera.main.ScreenToViewportPoint(corner);
            screenSpaceCorners.Add(screenSpace);
        }

        if(Input.mousePosition.y >= corners[0].y && Input.mousePosition.y <= corners[1].y && Input.mousePosition.x >= corners[0].x && Input.mousePosition.x <= corners[3].x)
        {
            return true;
        }
        return false;
    }
}
