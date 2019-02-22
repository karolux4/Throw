using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollision : MonoBehaviour
{
    public Vector3 Velocity { get; set; }
    public Vector3 Angular_Velocity { get; set; }
    private void Start()
    {
        Velocity = new Vector3();
        Angular_Velocity = new Vector3();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 4)
        {
            this.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            if (gameObject.transform.localPosition.z < 1.2f && Velocity.magnitude>0.5f)
            {
                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@"D:\VR Lab\Experiment_data.txt", true))
                {
                    file.WriteLine("Collision place(x,y,z):"+(float)((int)(gameObject.transform.localPosition.x*100))/(float)100 + " " + (float)((int)(gameObject.transform.localPosition.y * 100)) /(float) 100 + " " +(float)((int)(gameObject.transform.localPosition.z * 100)) / (float)100);
                    file.WriteLine("Velocity, Angular velocity:" + Velocity.ToString() + " " + Angular_Velocity.ToString());
                }
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }
    private void Update()
    {
        /*if(this.gameObject.transform.localPosition.y<-1f)
        {
            Destroy(this.gameObject);
        }*/
    }
}
