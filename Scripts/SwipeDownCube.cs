using System.Collections;
using UnityEngine;

public class SwipeDownCube : MonoBehaviour
{
    private void OnEnable()
    {
        Swipeing.SwipeDown += MoveCube;
    }
    private void OnDisable()
    {
        Swipeing.SwipeDown -= MoveCube;
    }

    public void MoveCube()
    {
        transform.position += new Vector3(0, 0, 1.1f);
        Debug.Log("fejdfew");
        StartCoroutine(MoveingCube());
    }
    private IEnumerator MoveingCube()
    {
        yield return new WaitForSeconds(1);
        transform.position -= new Vector3(0, 0, 1.1f);
    }
}
