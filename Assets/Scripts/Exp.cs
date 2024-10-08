using UnityEngine;

public class Exp : MonoBehaviour
{
    private SpriteRenderer sr;
    private int transDir = 1;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        sr.color = new Color(sr.color.r, sr.color.g - Time.deltaTime / 2 * transDir, sr.color.b);
        if (sr.color.g < .3f || sr.color.g > .9f)
        {
            transDir *= -1;
        }

        if ((transform.position - GameController.instance.player.transform.position).magnitude > Camera.main.orthographicSize * 3)
        {
            Destroy(gameObject);
        }
        if ((transform.position - GameController.instance.player.transform.position).magnitude < GameController.instance.player.expGainRange)
        {
            transform.position = Vector3.Lerp(transform.position, GameController.instance.player.transform.position, Time.deltaTime * 4);
        }
        if ((transform.position - GameController.instance.player.transform.position).magnitude < 1)
        {
            GameController.instance.player.GainExp(1);
            Destroy(gameObject);
        }
    }
}
