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
    public GameObject MoneyBag;
    public TextMeshProUGUI rewardText;
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
        //Gets random number between 0 and 1
        float chance = Random.Range(0f, 1f);
        Debug.Log(chance);
        totalReward = PlayerData.Instance.CombatBonusReward;
        Debug.Log("Player data: " + PlayerData.Instance.bet);
        //40 chance for silver Chest.
        //40% chance for silver Chest.
        if (PlayerStats.Instance.Level != 5)
        {
            if (chance <= 0.4f)
            {
                //Silver chest
                Reward = Random.Range(PlayerData.Instance.bet * 15, PlayerData.Instance.bet * 20 + 1);
                chest.GetComponent<Image>().sprite = SilverChestSprite;
                Debug.Log("Silver chest");
                ChestType = "Silver";

            }
            else
            {
                //Iron chest
                Reward = Random.Range(PlayerData.Instance.bet * 10, PlayerData.Instance.bet * 15 + 1);
                chest.GetComponent<Image>().sprite = IronChestSprite;
                Debug.Log("Iron chest");
                ChestType = "Iron";
            }
        }
        else
        {
            //Gold chest
            Reward = Random.Range(PlayerData.Instance.bet * 20, PlayerData.Instance.bet * 25 + 1);
            chest.GetComponent<Image>().sprite = GoldChestSprite;
            Debug.Log("Gold Chest");
            ChestType = "Gold";
        }
        //Change and adjust panel text to current reward and chest type.
        string price = $"Tillykke! \n Du fandt {UIManager.Instance.DisplayMoney(Reward)} i kisten";
        chest.GetComponentInChildren<TextMeshProUGUI>().text = price;
        rewardText.text = UIManager.Instance.DisplayMoney(totalReward);
    }
    //Method to drop chest
    public void DropChest()
    {
        if(ChestType == "Silver")
        {
            chestAni.SetBool("DropSilverChest", true);
        }
        else if (ChestType == "Iron")
        {
            chestAni.SetBool("DropIronChest", true);
        }
        else if (ChestType == "Gold")
        {
            chestAni.SetBool("DropGoldChest", true);
        }
        transform.SetAsLastSibling();
    }
    //Gets total amount of money throughout the bonusgame
    public void TotalRewards()
    {
        totalReward += Reward;
        PlayerData.Instance.CombatBonusReward += Reward;
        rewardText.text = UIManager.Instance.DisplayMoney(totalReward);
        PlayerData.Instance.CombatBonusIncrementReward += Reward;
        if(PlayerData.Instance.CombatBonusIncrementReward >= 20 * PlayerData.Instance.bet)
        {
            PlayerData.Instance.moneyBagWidth += 20f;
            MoneyBag.GetComponent<RectTransform>().sizeDelta = new Vector2(PlayerData.Instance.moneyBagWidth, 120);
            PlayerData.Instance.CombatBonusIncrementReward -= 20 * PlayerData.Instance.bet;
        }
    }
}
