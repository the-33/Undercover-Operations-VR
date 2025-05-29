using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public TextMeshPro UIText;

    public int HP;
    public int MaxHP = 100;

    public int KillCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HP = MaxHP;
    }

    public void UpdateUI()
    {
        string color = "green";
        string barraHP = "";

        float hpPercent = (float)HP / MaxHP;

        // Asignar color según el porcentaje
        if (hpPercent <= 0.2f)
            color = "red";
        else if (hpPercent <= 0.5f)
            color = "yellow";
        else
            color = "green";

        for (int i = 0; i < HP/10; i++)
        {
            barraHP += "\udb81\udf64";
        }

        string text = $"<color=\"black\">\uee15 {KillCount}</color>\n" +
            $"\uf004 <color=\"{color}\">{barraHP}</color>";

        UIText.text = text ;
    }

    public void AddKill()
    {
        KillCount++;
        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        HP -= damage;
        UpdateUI();
        if (HP <= 0) SceneManager.LoadScene(0);
    }
}
