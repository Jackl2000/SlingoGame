
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestChance : MonoBehaviour
{
    public GameObject chest;
    public Sprite SilverChestSprite;
    public Sprite IronChestSprite;
    public Sprite GoldChestSprite;
    public string ChestType;
    [SerializeField] private Animator chestAni;
    public float Reward;

    // Start is called before the first frame update
    void Start()
    {
        ChangeChestRandomly();
        chestAni = chest.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void ChangeChestRandomly()
    {
        bool lastBoss = false;
        //Gets random number between 0 and 1
        float chance = Random.Range(0f, 1f);
        Debug.Log(chance);
        //40% chance for silver Chest.
        if (lastBoss != true)
        {
            if (chance <= 0.4f)
            {
                //Silver chest
                Reward = Random.Range(150, 250 + 1);
                chest.GetComponent<Image>().sprite = SilverChestSprite;
                Debug.Log("Silver chest");
                ChestType = "Silver";
                Debug.Log(Reward);

            }
            else
            {
                //Iron chest
                Reward = Random.Range(50, 150 + 1);
                chest.GetComponent<Image>().sprite = IronChestSprite;
                Debug.Log("Iron chest");
                ChestType = "Iron";
                Debug.Log(Reward);
            }
        }
        else
        {
            //Gold chest
            Reward = Random.Range(250, 500 + 1);
            chest.GetComponent<Image>().sprite = GoldChestSprite;
            Debug.Log("Gold Chest");
            ChestType = "Gold";
            Debug.Log(Reward);
        }
        //Change and adjust panel text to current reward and chest type.
        string price = $"Tillykke! \n Du fandt {Reward} kr i kisten";
        chest.GetComponentInChildren<TextMeshProUGUI>().text = price;
        Debug.Log("Text is component" + chest.GetComponentInChildren<TextMeshProUGUI>().text);
    }
    //Method to drop chest
    public void DropChest()
    {
        if(ChestType == "Silver")
        {
            chestAni.SetBool("DropSilverChest", true);
        }
        if (ChestType == "Iron")
        {
            chestAni.SetBool("DropIronChest", true);
        }
        if (ChestType == "Gold")
        {
            chestAni.SetBool("DropGoldChest", true);
        }
    }
}
