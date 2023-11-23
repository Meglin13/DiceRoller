using TMPro;
using UnityEngine;

public class RollerScript : MonoBehaviour
{
    [SerializeField]
    private float MaxForce = 5;

    [SerializeField]
    private DiceScript Dice;

    [SerializeField]
    private TextMeshProUGUI diceResult;

    private void Start()
    {
        Dice.OnDiceStop += SetText;
    }

    private void SetText()
    {
        //TODO: Добавить моды

        diceResult.text = Dice.GetUpperSide().ToString();
    }

    public void RollDice()
    {
        if (Dice.rb.velocity == Vector3.zero)
        {
            Dice.transform.position = transform.position;

            float x = Random.Range(-MaxForce, MaxForce);
            float y = Random.Range(-MaxForce, MaxForce);
            float z = Random.Range(-MaxForce, MaxForce);

            Dice.rb.AddForce(x, y, z, ForceMode.Impulse);
            Dice.rb.AddTorque(x, y, z, ForceMode.Impulse);

            StartCoroutine(Dice.WaitTillStop());
        }
    }
}
