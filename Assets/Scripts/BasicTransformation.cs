using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTransformation : MonoBehaviour
{
    Matrix4x4 matrix;
    bool shift, translate, scale, rotate = false;
    bool x, y, z, mode = true;
    float eulX;
    float eulY;
    float eulZ;

    // Start is called before the first frame update
    void Start()
    {
        // Get matrix of transform
        matrix = transform.localToWorldMatrix;

        // Get rotation of transform
        eulX = Mathf.Abs(transform.rotation.eulerAngles.x);
        eulY = Mathf.Abs(transform.rotation.eulerAngles.y);
        eulZ = Mathf.Abs(transform.rotation.eulerAngles.z);
    }

    // Update is called once per frame
    void Update()
    {
        // M toggles mode
        if (Input.GetKeyDown(KeyCode.M))
        {
            mode = !mode;

            // Get matrix of transform
            matrix = transform.localToWorldMatrix;

            // Get rotation of transform
            eulX = Mathf.Abs(transform.rotation.eulerAngles.x);
            eulY = Mathf.Abs(transform.rotation.eulerAngles.y);
            eulZ = Mathf.Abs(transform.rotation.eulerAngles.z);
        }

        // Reset cube when switching modes in scale mode
        if (Input.GetKeyDown(KeyCode.M) && scale)
        {
            //gameObject.transform.rotation = Quaternion.identity;
        }

        // Check key presses
        // Check if shift is pressed
        if (Input.GetKey(KeyCode.LeftShift))
        {
            shift = true;
        }

        // Check which Transformation is toggled
        // Translate
        if (Input.GetKeyDown(KeyCode.T))
        {
            translate = !translate;
            scale = false;
            rotate = false;
        }
        // Scale
        if (Input.GetKeyDown(KeyCode.S) && !mode)
        {
            //gameObject.transform.rotation = Quaternion.identity;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {

            translate = false;
            scale = !scale;
            rotate = false;
        }

        //Rotate
        if (Input.GetKeyDown(KeyCode.R))
        {
            translate = false;
            scale = false;
            rotate = !rotate;
        }

        // Check which axis is pressed
        if (Input.GetKey(KeyCode.X))
        {
            x = true;
        }
        if (Input.GetKey(KeyCode.Y))
        {
            y = true;
        }
        if (Input.GetKey(KeyCode.Z))
        {
            z = true;
        }


        // Perform the appropriate transform

        // Internal Functions

        // Translate
        if (translate && !shift && x && mode)
        {
            transform.Translate(new Vector3(1, 0, 0) * Time.deltaTime);
        }

        if (translate && !shift && y && mode)
        {
            transform.Translate(new Vector3(0, 1, 0) * Time.deltaTime);
        }

        if (translate && !shift && z && mode)
        {
            transform.Translate(new Vector3(0, 0, 1) * Time.deltaTime);
        }

        if (translate && shift && x && mode)
        {
            transform.Translate(new Vector3(-1, 0, 0) * Time.deltaTime);
        }

        if (translate && shift && y && mode)
        {
            transform.Translate(new Vector3(0, -1, 0) * Time.deltaTime);
        }

        if (translate && shift && z && mode)
        {
            transform.Translate(new Vector3(0, 0, -1) * Time.deltaTime);
        }

        // Scale
        if (scale && !shift && x && mode)
        {
            transform.localScale += new Vector3(2 * Time.deltaTime, 0, 0);
        }
        if (scale && !shift && y && mode)
        {
            transform.localScale += new Vector3(0, 2 * Time.deltaTime, 0);
        }
        if (scale && !shift && z && mode)
        {
            transform.localScale += new Vector3(0, 0, 2 * Time.deltaTime);
        }

        if (scale && shift && x && mode)
        {
            transform.localScale += new Vector3(-2 * Time.deltaTime, 0, 0);
        }
        if (scale && shift && y && mode)
        {
            transform.localScale += new Vector3(0, -2 * Time.deltaTime, 0);
        }
        if (scale && shift && z && mode)
        {
            transform.localScale += new Vector3(0, 0, -2 * Time.deltaTime);
        }

        // Rotate
        if (rotate && !shift && x && mode)
        {
            transform.Rotate(new Vector3(1, 0, 0));
        }
        if (rotate && !shift && y && mode)
        {
            transform.Rotate(new Vector3(0, 1, 0));
        }
        if (rotate && !shift && z && mode)
        {
            transform.Rotate(new Vector3(0, 0, 1));
        }
        if (rotate && shift && x && mode)
        {
            transform.Rotate(new Vector3(-1, 0, 0));
        }
        if (rotate && shift && y && mode)
        {
            transform.Rotate(new Vector3(0, -1, 0));
        }
        if (rotate && shift && z && mode)
        {
            transform.Rotate(new Vector3(0, 0, -1));
        }

        // Custom Functions

        // Translate
        if (translate && !shift && x && !mode)
        {
            matrix[0, 3] += (1 * Time.deltaTime);
        }

        if (translate && !shift && y && !mode)
        {
            matrix[1, 3] += (1 * Time.deltaTime);
        }

        if (translate && !shift && z && !mode)
        {
            matrix[2, 3] += (1 * Time.deltaTime);
        }

        if (translate && shift && x && !mode)
        {
            matrix[0, 3] += (-1 * Time.deltaTime);
        }

        if (translate && shift && y && !mode)
        {
            matrix[1, 3] += (-1 * Time.deltaTime);
        }

        if (translate && shift && z && !mode)
        {
            matrix[2, 3] += (-1 * Time.deltaTime);
        }

        // Apply translate
        if (translate && !mode)
        {
            transform.position = new Vector3(matrix[0, 3], matrix[1, 3], matrix[2, 3]);
            transform.rotation = Quaternion.Euler(eulX, eulY, eulZ);
        }


        // Scale
        if (scale && !shift && x && !mode)
        {
            matrix[0, 0] += (2 * Time.deltaTime);
        }
        if (scale && !shift && y && !mode)
        {
            matrix[1, 1] += (2 * Time.deltaTime);
        }
        if (scale && !shift && z && !mode)
        {
            matrix[2, 2] += (2 * Time.deltaTime);
        }

        if (scale && shift && x && !mode)
        {
            matrix[0, 0] += (-2 * Time.deltaTime); ;
        }
        if (scale && shift && y && !mode)
        {
            matrix[1, 1] += (-2 * Time.deltaTime);
        }
        if (scale && shift && z && !mode)
        {
            matrix[2, 2] += (-2 * Time.deltaTime);
        }

        // Apply scale
        if (scale && !mode)
        {
            gameObject.transform.localScale = new Vector3(matrix[0, 0], matrix[1, 1], matrix[2, 2]);
        }




        // Rotate
        if (rotate && !shift && x && !mode)
        {
            ++eulX;
        }
        if (rotate && !shift && y && !mode)
        {
            ++eulY;
        }
        if (rotate && !shift && z && !mode)
        {
            ++eulZ;
        }

        if (rotate && shift && x && !mode)
        {
            --eulX;
        }
        if (rotate && shift && y && !mode)
        {
            --eulY;
        }
        if (rotate && shift && z && !mode)
        {
            --eulZ;
        }

        // Apply rotate
        if (rotate && !mode)
        {
            transform.rotation = Quaternion.Euler(eulX, eulY, eulZ);
        }


        // Reset values
        shift = false;
        x = false;
        y = false;
        z = false;

    }
}