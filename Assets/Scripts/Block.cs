using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private string blockName;
    [SerializeField] private float durability;
    [SerializeField] private int stack;
    [SerializeField] private float walkSpeed;
    [SerializeField] private bool isDangerous;
    [SerializeField] private int damage;

    // Proprietà in sola lettura per l'accesso da altri script
    public string BlockName => blockName;
    public float Durability => durability;
    public int Stack => stack;
    public float WalkSpeed => walkSpeed;
    public bool IsDangerous => isDangerous;
    public int Damage => damage;

    public void InizializzaBlocco(string blockName, float durability, int stack, float walkSpeed, bool isDangerous, int damage)
    {
        this.blockName = blockName;
        this.durability = durability;
        this.stack = stack;
        this.walkSpeed = walkSpeed;
        this.isDangerous = isDangerous;
        this.damage = damage;
    }
}
