using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 4)
        {
            this.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            if (gameObject.transform.localPosition.x > 1f)
            {
                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@"D:\VR Lab\Experiment_data.txt", true))
                {
                    file.WriteLine(gameObject.transform.localPosition.x + " " + gameObject.transform.localPosition.y + " " + gameObject.transform.localPosition.z);
                }
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }
}
