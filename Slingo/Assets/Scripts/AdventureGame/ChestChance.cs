
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ChestChance : MonoBehaviour
{
    public GameObject chest;
    public Sprite SilverChestSprite;
    public Sprite IronChestSprite;
    public Sprite GoldChestSprite;
    public string ChestType;
    [SerializeField] private Animator chestAni;
    public float Reward;
    public GameObject MoneyBag;
    public float totalReward;

    // Start is called before the first frame update
    void Start()
    {
        MoneyBag.GetComponent<RectTransform>().sizeDelta = new Vector2(PlayerData.Instance.moneyBagWidth, 120);
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
        totalReward = PlayerData.Instance.CombatBonusReward;
        Debug.Log("Player data: " + PlayerData.Instance.bet);
        //40 chance for silver Chest.
        //40% chance for silver Chest.
        if (lastBoss != true)
        {
            if (chance <= 0.4f)
            {
                //Silver chest
                Reward = Random.Range(PlayerData.Instance.bet * 15, PlayerData.Instance.bet * 20 + 1);
                chest.GetComponent<Image>().sprite = SilverChestSprite;
                Debug.Log("Silver chest");
                ChestType = "Silver";
                Debug.Log(Reward);

            }
            else
            {
                //Iron chest
                Reward = Random.Range(PlayerData.Instance.bet * 10, PlayerData.Instance.bet * 15 + 1);
                chest.GetComponent<Image>().sprite = IronChestSprite;
                Debug.Log("Iron chest");
                ChestType = "Iron";
                Debug.Log(Reward);
            }
        }
        else
        {
            //Gold chest
            Reward = Random.Range(PlayerData.Instance.bet * 20, PlayerData.Instance.bet * 25 + 1);
            chest.GetComponent<Image>().sprite = GoldChestSprite;
            Debug.Log("Gold Chest");
            ChestType = "Gold";
            Debug.Log(Reward);
        }
        //Change and adjust panel text to current reward and chest type.
        string price = $"Tillykke! \n Du fandt {UIManager.Instance.DisplayMoney(Reward)} kr i kisten";
        chest.GetComponentInChildren<TextMeshProUGUI>().text = price;
        MoneyBag.GetComponentInChildren<TextMeshProUGUI>().text = UIManager.Instance.DisplayMoney(totalReward);
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
    //Gets total amount of money throughout the bonusgame
    public void TotalRewards()
    {
        totalReward += Reward;
        PlayerData.Instance.CombatBonusReward += Reward;
        MoneyBag.GetComponentInChildren<TextMeshProUGUI>().text = UIManager.Instance.DisplayMoney(totalReward);
        PlayerData.Instance.CombatBonusIncrementReward += Reward;
        if(PlayerData.Instance.CombatBonusIncrementReward >= 20 * PlayerData.Instance.bet)
        {
            PlayerData.Instance.moneyBagWidth += 20f;
            MoneyBag.GetComponent<RectTransform>().sizeDelta = new Vector2(PlayerData.Instance.moneyBagWidth, 120);
            PlayerData.Instance.CombatBonusIncrementReward -= 20 * PlayerData.Instance.bet;
        }
    }
}
