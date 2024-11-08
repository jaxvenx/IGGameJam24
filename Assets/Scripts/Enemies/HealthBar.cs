using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private SpriteRenderer bar;
    private float maxBarWidth = 20.5f;

    //Update Rotation to face the Main Camera
    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
    }

    public void SetHealth(float health, float maxHealth)
    {
        float healthPercentage = health / maxHealth;
        bar.size = new Vector2(maxBarWidth * healthPercentage, bar.size.y);
    }
}
