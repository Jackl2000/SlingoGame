using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkrabeHole : MonoBehaviour
{
    public GameObject holePrefab;
    public SkrabeSelector selector;

    public GameObject selectedCard;
    private List<GameObject> holes = new List<GameObject>();
    [SerializeField] private List<Vector2> positionPoints = new List<Vector2>();
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (CheckForVisibility() && selectedCard != null)
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
                    if (selectedCard != null)
                    {
                        SetPositionPointOnSelectedCard();
                        holes.Add(hole);
                    } 
                    else Destroy(hole);
                    
                }
                else if(CheckForIntergration())
                {
                    holes.Add(Instantiate(holePrefab, Input.mousePosition, Quaternion.identity, transform));
                }
            }
        }
    }

    private void SetPositionPointOnSelectedCard()
    {
        positionPoints.Clear();
        TextMeshProUGUI rewardText = selectedCard.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>(true);
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                float xCount = rewardText.transform.position.x + 40 + (25 * i);
                float yCount = rewardText.transform.position.y + 40 + (25 * j);
                positionPoints.Add(new Vector2(xCount, yCount));
            }
        }
    }

    private bool CheckForVisibility()
    {
        int holeHit = 0;
        for (int i = 0; i < positionPoints.Count; i++)
        {
            Vector3 direction = positionPoints[i] - new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.transform.position, direction);
            Debug.DrawRay(Camera.main.transform.position, direction);
            if (hit.collider != null && hit.collider.gameObject.name == "Hole(Clone)")
            {
                holeHit++;
            }
        }
        Debug.Log("Hit raycasts: " + holeHit + " : hole count: " + holes.Count);
        if ((holeHit >= 25 && holes.Count > 100) || (holeHit >= 20 && holes.Count > 250)) return true;
        else
        {
            return false;
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
