using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MovingObject {

    public int wallDamage = 1;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restartLevelDelay = 1f;
    public Text foodText;
    public enum Weapons {pistol, sniperRifle, knife}
    public Weapons weapon;
    public GameObject[] weaponPrefabs;

    private Vector2 direction = new Vector2 (1,0);
    private Animator animator;
    private Rigidbody2D rb2D;
    private int food;
    

	// Use this for initialization
	protected override void Start ()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();

        food = GameManager.instance.playerFoodPoints;

        foodText.text = "Food: " + food;

        weapon = Weapons.pistol;

        base.Start();
	}
	private void OnDisable()
    {
        GameManager.instance.playerFoodPoints = food;
    }

	// Update is called once per frame
	void Update ()
    {
        if (!GameManager.instance.playersTurns) return;

        int horizontal = 0;
        int vertical = 0;
        bool space = false;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");
        space = Input.GetKey(KeyCode.Space);

        if (horizontal != 0)
            vertical = 0;

        if (vertical != 0)
            horizontal = 0;

        if (horizontal != 0 || vertical != 0 || space)
        {
            if (space)
            {
                if (weapon == Weapons.pistol)
                    AttemptAttack<Pistol>();
            }
            else
            {
                AttemptMove<Wall>(horizontal, vertical);
                direction = new Vector2(horizontal, vertical);
            }
            
        }
            
	}

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        food --;
        foodText.text = "Food: " + food;

        base.AttemptMove<T>(xDir, yDir);

        RaycastHit2D hit;

        CheckIfGameOver();

        GameManager.instance.playersTurns = false;
    }

    protected void AttemptAttack<T>()
        where T: Component
    {
        food--;
        foodText.text = "Food: " + food;

        GameObject bullet = weaponPrefabs[(int)weapon];
        Instantiate(bullet, rb2D.position, Rotation(direction)); 

        CheckIfGameOver();

        GameManager.instance.playersTurns = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Exit")
        {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
        else if (other.tag == "Food")
        {
            food += pointsPerFood;
            foodText.text = "+" + pointsPerFood + " Food: " + food;
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Soda")
        {
            food += pointsPerSoda;
            foodText.text = "+" + pointsPerSoda + " Food: " + food;
            other.gameObject.SetActive(false);
        }
    }

    protected override void OnCantMove<T>(T component)
    {
        Wall hitWall = component as Wall;
        hitWall.DamageWall(wallDamage);
        animator.SetTrigger("playerChop");
    }

    private void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void LoseFood (int loss)
    {
        animator.SetTrigger("playerHit");
        food -= loss;
        foodText.text = "-" + loss + " Food: " + food;
        CheckIfGameOver();
    }

    private void CheckIfGameOver()
    {
        if (food <= 0)
            GameManager.instance.GameOver();
    }

    private Quaternion Rotation(Vector2 dir)
    {
        Quaternion rot = Quaternion.identity;
        if (dir == new Vector2(0, -1))
            rot.eulerAngles = new Vector3(0, 0, 90);
        else if (dir == new Vector2(1, 0))
            rot.eulerAngles = new Vector3(0, 0, 180);
        else if (dir == new Vector2(0, 1))
            rot.eulerAngles = new Vector3(0, 0, 270);
        return rot;
    }
}
