using UnityEngine;

public class MyCamera : MonoBehaviour
{
    private readonly Vector3 positionModiflier = new Vector3(0, 0, -10);

    // Update is called once per frame
    private void LateUpdate()
    {
        transform.position = GameController.instance.player.transform.position + positionModiflier;
    }
}
