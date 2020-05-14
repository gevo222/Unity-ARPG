using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageNumbers : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int poolSize;

    private Queue<GameObject> pool;

    void Start()
    {
        pool = new Queue<GameObject>();
        for (int i = 0; i < poolSize; ++i)
        {
            var obj = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            obj.transform.parent = transform;
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public void OnUpdateHealth(Transform parent, int value)
    {
        var prev = parent.gameObject.GetComponent<CharacterStats>().currentHP;
        var damageTaken = prev - value;
		if (damageTaken <= 0) return;

        var textObj = pool.Dequeue();
        textObj.transform.position = parent.position + Vector3.up * 2;
        textObj.SetActive(true);
        pool.Enqueue(textObj);

        var textMesh = textObj.GetComponentInChildren<TextMeshPro>();
        textMesh.color = damageTaken > 0 ? Color.red : Color.green;
        textMesh.SetText($"<b>{prev - value}</b>");

        var rigidBody = textObj.GetComponent<Rigidbody2D>();
        if (rigidBody) rigidBody.velocity = new Vector2(Random.Range(-0.5f, 0.5f), 1f);

        StartCoroutine(DisablePooledObject(textObj));
    }

    private IEnumerator DisablePooledObject(GameObject obj)
    {
        yield return new WaitForSeconds(1.5f);
        obj.SetActive(false);
    }
}
