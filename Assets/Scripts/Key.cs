using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public enum KeyType
    {
        Yellow, Blue, Red,
    }
    public KeyType keyType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Player.Instance.PlayerData.UpdateKeys((int)keyType, 1);
        Destroy(gameObject);
    }
}
