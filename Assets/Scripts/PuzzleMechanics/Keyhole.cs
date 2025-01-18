using UnityEngine;
using Utils;

public class Keyhole : MonoBehaviour
{
    public string requiredKeyColor; // Assign "Red", "Blue", etc. in Inspector

    public void Unlock()
    {
        // Damage the boss
        GameObject.FindGameObjectWithTag("Boss")?.GetComponent<IDamageable>().TakeDamage(100);

        // Destroy the keyhole
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var keyCollector = collision.collider.GetComponent<IKeyCollector>();
            Debug.Log(keyCollector == null);
            if (keyCollector != null && keyCollector.HasKey(requiredKeyColor))
            {
                Unlock();
                keyCollector.UseKey(requiredKeyColor);
            }
        }
    }
}
