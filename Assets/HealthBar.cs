using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private float maxHealth = 12f;
    [SerializeField] private float minXPosition = -530f;
    [SerializeField] private float maxXPosition = 0f;
    [SerializeField] public int testDamage = 1;
    [SerializeField] public Color highCol = Color.green;
    [SerializeField] public Color lowCol = Color.red;

    private float currentHealth;
    private float displayedHealth;
    private RectTransform rectTransform;
    private Image image;
    private float transitionSpeed = 0.75f;

    void Start()
    {
        currentHealth = maxHealth;
        displayedHealth = maxHealth;
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        UpdateHealthBarPosition();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            DidTakeDamage(testDamage);
        }

        if (displayedHealth != currentHealth)
        {
            displayedHealth = Mathf.MoveTowards(displayedHealth, currentHealth, Time.deltaTime * (maxHealth / transitionSpeed));
            UpdateHealthBarPosition();
        }
    }

    public void DidTakeDamage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0f, maxHealth);
    }

    private void UpdateHealthBarPosition()
    {
        float healthPercentage = displayedHealth / maxHealth;
        float newX = Mathf.Lerp(minXPosition, maxXPosition, healthPercentage);
        Vector3 newPosition = new Vector3(newX, rectTransform.localPosition.y, rectTransform.localPosition.z);
        rectTransform.localPosition = newPosition;

        if (image != null)
        {
            image.color = Color.Lerp(lowCol, highCol, healthPercentage);
        }
    }
}