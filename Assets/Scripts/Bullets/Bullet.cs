using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public float speed;
    public Vector3 dir;

    /// <summary>
    /// 子弹特殊效果
    /// </summary>
    public void ExEffect(Enemy enemy)
    {

    }

    private void Update()
    {
        transform.position += speed * Time.deltaTime * dir;

        if ((transform.position - GameController.instance.player.transform.position).magnitude > Camera.main.orthographicSize*2)
        {
            gameObject.SetActive(false);
            transform.position = GameController.instance.player.transform.position;
        }
    }
}
