using UnityEngine;

public class WallStatus : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private float returnCD;

    Collider playerCollider;
    private int currentHealth;
    private Material material;
    private Color color;
    float timer;
    bool returning;
    void Start()
    {
        playerCollider = PlayerManager.instance.player.transform.Find("CatPlayerOBJ(Rotation here)/Cat").gameObject.GetComponent<Collider>();
        material = GetComponent<Renderer>().material;
        currentHealth = maxHealth;
        timer = 0;

        returning = false;


        color = material.color;
        color.a = 0.1f;
        material.color = color;
        gameObject.layer = LayerMask.NameToLayer("Default");
        Physics.IgnoreCollision(GetComponent<Collider>(), playerCollider, true);
    }

    void Update()
    {
        if(currentHealth <= 0)
        {
            //Physics.IgnoreCollision(waterBossAttackCollider, GetComponent<Collider>(), false);
            gameObject.layer = LayerMask.NameToLayer("Ground");
            Physics.IgnoreCollision(GetComponent<Collider>(), playerCollider, false);
            timer += Time.deltaTime;
            color.a = 1f-(timer / returnCD);
            material.color = color;
            if (!returning)
            {
                returning = true;
                Invoke(nameof(CollisionDeactivate), returnCD);
                //InvokeRepeating
            }
        }
    }
    public void CollisionDeactivate()
    {
        CancelInvoke(nameof(CollisionDeactivate));
        gameObject.layer = LayerMask.NameToLayer("Default");
        Physics.IgnoreCollision(GetComponent<Collider>(), playerCollider, true);
        currentHealth = maxHealth;
        color.a = 0.1f;
        material.color = color;
        timer = 0;
        returning = false;
    }
    public void TakeDamage(int _damage)
    {
        if(currentHealth > 0)
        {
            currentHealth -= _damage;
            //Debug.Log(_damage);

            color.a = (0.1f + (float)(maxHealth - currentHealth) / (float)maxHealth * 0.9f);
            material.color = color;
        }

    }
}
