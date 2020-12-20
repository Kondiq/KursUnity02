using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 0.5f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Vector3 position = this.transform.position;
            position.x -= speed;
            this.transform.position = position;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Vector3 position = this.transform.position;
            position.x += speed;
            this.transform.position = position;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Vector3 position = this.transform.position;
            position.z += speed;
            this.transform.position = position;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Vector3 position = this.transform.position;
            position.z -= speed;
            this.transform.position = position;
        }
    }
}
