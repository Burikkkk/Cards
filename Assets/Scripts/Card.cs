using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    private static Sprite back;
    private static bool checkMatch = true;
    [SerializeField]
    private Sprite front;

    private int id;
    private Sprite current;
    private Vector3 scale;

    private static Card firstPicked = null;

    private GameManager manager;

    void Start()
    {
        current = back;
        gameObject.GetComponent<Image>().sprite = back;
        //gameObject.GetComponent<Animator>().SetBool("LessThan3", true);
        scale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    public void SetCard(int id, Sprite front, GameManager manager)
    {
        this.id = id;
        this.front = front;
        this.manager = manager;
    }

    public void SetFront(Sprite front)
    {
        this.front = front;
    }

    public static void SetBack(Sprite backSprite)
    {
        back = backSprite;
    }

    public static void SetCheckMatch(bool value)
    {
        checkMatch = value;
    }

    public void CheckCardsMatch()
    {
        if (!checkMatch)
            return;

        if(firstPicked == null)
        {
            firstPicked = this;
            return;
        }
        
        if (firstPicked.id != id || (firstPicked.id == -1 && id == -1))
        {
            manager.ChangePlayer();
            firstPicked.RotateBack();
            RotateBack();
        }
        else 
        {
            manager.cardsAmount-=2;
            GameManager.currentPlayer.IncrementScore();

            firstPicked.gameObject.GetComponent<Animator>().SetBool("Paired", true);
            FlipGameObject(firstPicked.gameObject);
            gameObject.GetComponent<Animator>().SetBool("Paired", true);
            FlipGameObject(gameObject);
        }
        firstPicked = null;
    }

    public void DisableObject()
    {
        Destroy(gameObject);
    }

    public void Rotate()
    {
        gameObject.GetComponent<Animator>().SetBool("Rotate", true);
        gameObject.GetComponent<Animator>().SetBool("NotCompare", false);
    }

    private void FlipGameObject(GameObject obj)
    {
        Vector3 scale = obj.transform.localScale;
        scale.x *= -1;
        obj.transform.localScale = scale;
    }

    public void RotateBack()
    {
        gameObject.GetComponent<Animator>().SetBool("Rotate", false);
        gameObject.GetComponent<Animator>().SetBool("NotCompare", true);

    }

    public void ChangeSprite() 
    {
        // отражается картинка при повороте
        scale.x *= -1;
        transform.localScale = scale;
        if (current==back)
        {
            gameObject.GetComponent<Image>().sprite = front;
            current = front;
        }
        else
        {
            gameObject.GetComponent<Image>().sprite = back;
            current = back;
        }
    }
}
