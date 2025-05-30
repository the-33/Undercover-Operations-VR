using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Para usar Image

public class PlayerStats : MonoBehaviour
{
    public TextMeshPro UIText;
    public Image DamageVignette; // Asigna desde el inspector

    public int HP;
    public int MaxHP = 100;
    public int KillCount = 0;

    private float vignetteDuration = 0.5f;
    private float vignetteTimer = 0f;
    private Color vignetteColor;

    void Start()
    {
        HP = MaxHP;
        vignetteColor = DamageVignette.color;
        vignetteColor.a = 0;
        DamageVignette.color = vignetteColor;
        UpdateUI();
    }

    void Update()
    {
        // Fade out effect
        if (vignetteTimer > 0)
        {
            vignetteTimer -= Time.deltaTime;
            vignetteColor.a = Mathf.Lerp(0f, 0.5f, vignetteTimer / vignetteDuration);
            DamageVignette.color = vignetteColor;
        }
    }

    public void UpdateUI()
    {
        string color = "green";
        string barraHP = "";

        float hpPercent = (float)HP / MaxHP;

        if (hpPercent <= 0.2f)
            color = "red";
        else if (hpPercent <= 0.5f)
            color = "yellow";

        for (int i = 0; i < HP / 10; i++)
        {
            barraHP += "\udb81\udf64";
        }

        string text = $"<color=\"black\">\uee15 {KillCount}</color>\n" +
                      $"\uf004 <color=\"{color}\">{barraHP}</color>";

        UIText.text = text;
    }

    public void AddKill()
    {
        KillCount++;
        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        HP -= damage;

        vignetteTimer = vignetteDuration; // Dispara efecto viñeta
        vignetteColor.a = 0.7f;           // Opacidad visible
        DamageVignette.color = vignetteColor;

        UpdateUI();

        if (HP <= 0)
            SceneManager.LoadScene(0);
    }
}
