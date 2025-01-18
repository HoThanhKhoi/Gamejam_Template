using System.Linq;
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
            var keyList = FindObjectsByType<Key>(FindObjectsSortMode.None);
            
            var key = keyList.FirstOrDefault(n => n.keyColor == requiredKeyColor);
            if (key != null)
            {
                Unlock();
                key.UseKey();
            }
        }
    }
}
