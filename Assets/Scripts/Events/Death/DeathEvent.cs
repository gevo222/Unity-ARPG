using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Death {
    void die();
}

public class DeathEvent : MonoBehaviour
{
    public void OnUpdateHealth(Transform parent, int hp)
    {
        if (hp <= 0)
        {
            var death = parent.gameObject.GetComponent<Death>();
            death?.die();
        }
    }
}
