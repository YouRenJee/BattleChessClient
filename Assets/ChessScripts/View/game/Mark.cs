using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mark : MonoBehaviour
{
    public GameObject pre;
    public void OnClick(GameObject gm)
    {
        if (tag == "Cls")
        {
            
            Transform ts = gm.transform.Find("BzPre(Clone)");
            if (ts != null)
            {
                SpriteRenderer SP = ts.GetComponent<SpriteRenderer>();
                SP.sprite = null;
            }
        }
        else
        {
            Transform ts = gm.transform.Find("BzPre(Clone)");
            if (ts != null)
            {
                SpriteRenderer SP = ts.GetComponent<SpriteRenderer>();
                SP.sprite = GetComponent<SpriteRenderer>().sprite;
            }
            else
            {
                GameObject obj = Instantiate(pre);
                obj.transform.position = new Vector3(gm.transform.position.x, gm.transform.position.y, gm.transform.position.z - 0.1f);
                obj.transform.SetParent(gm.transform);
                obj.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
            }

        }

    }
}
