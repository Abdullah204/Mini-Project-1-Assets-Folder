using UnityEngine;

public class SwipeMovement : MonoBehaviour
{
    private Vector2 touchStartPos;
    private bool d = false;
    private readonly float left = -9f;
    private readonly float right = 9f;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);

            if (t.phase == TouchPhase.Began)
            {
                touchStartPos = t.position;
                d = true;
            }
            else if (t.phase == TouchPhase.Moved)
            {
                if (d)
                {
                    float sd = t.deltaPosition.x * Time.deltaTime;
                    float s = 2.5f;
                    float newX = Mathf.Clamp(transform.position.x + sd * s, left, right);
                    transform.position = new Vector3(newX, transform.position.y, transform.position.z);
                }

            }
            else if(t.phase == TouchPhase.Ended)
            {
                d = false;
            }
              
            
        }
    }
}
