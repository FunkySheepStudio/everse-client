using UnityEngine;

public class UziBullets : MonoBehaviour
{
    float lifeTime = 0;
    public float velocity = 50;
    public float dispertion = 0.2f;
    public Vector3 direction;

    private void Start()
    {
        direction += Vector3.up * Random.Range(-dispertion, dispertion) + Vector3.forward * Random.Range(-dispertion, dispertion);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition += direction * Time.deltaTime * velocity;
        lifeTime += Time.deltaTime;
        if (lifeTime >= 2)
        {
            Destroy(gameObject);
        }
    }
}
