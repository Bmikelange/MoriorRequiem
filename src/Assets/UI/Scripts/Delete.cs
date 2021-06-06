using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delete : MonoBehaviour
{
    // Start is called before the first frame update
    public void DeleteGameObject()
    {
        GameObject.Destroy(gameObject);
    }
}
