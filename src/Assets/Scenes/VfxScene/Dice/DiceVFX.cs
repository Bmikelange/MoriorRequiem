using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceVFX : MonoBehaviour {

    public Vector3 direction;
    public float force = 400.0f;

    public void Init(Vector3 _direction, Vector2Int power, Vector2Int result, Sprite icon = null)
    {
        direction = _direction;
        float[] dirAngles = new float[] { -10f, 10f };
        for (int i = 0; i < transform.childCount; ++i)
        {
            Transform diceParent = transform.GetChild(i);
            float percentageOfMaxRange = (float)(result[i] - power.x) / (float)(power.y - power.x);
            string diceType = (percentageOfMaxRange > 0.75f ? "P" : percentageOfMaxRange > 0.25f ? "" : "T");
            int diceToKeep = (percentageOfMaxRange > 0.75f ? 2 : percentageOfMaxRange > 0.25f ? 1 : 0);
            int j = 0;
            while(diceParent.childCount > 1)
            {
                if(diceToKeep == 0)
                {
                    j = 1;
                }
                GameObject toDestroy = diceParent.GetChild(j).gameObject;
                toDestroy.transform.SetParent(null);
                Destroy(toDestroy);
                diceToKeep--;
            }

            Transform dice = diceParent.GetChild(0);
            Rigidbody rigidbody = dice.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                Vector3 ndir = Quaternion.AngleAxis(dirAngles[i], Vector3.up) * direction;
                Vector3 nforce = ndir * force;
                Vector3 pos = dice.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                rigidbody.AddForceAtPosition(nforce, pos, ForceMode.Force);
            }

            DiceScript ds = dice.GetComponent<DiceScript>();
            if (ds != null) ds.Init(result[i], diceType);

            MeshRenderer mr = dice.GetComponent<MeshRenderer>();
            Texture texture = (diceType == "" ? icon.texture : Resources.Load<Texture2D>("Background" + diceType));
            mr.materials[1].mainTexture = texture;
        }

    }

        /*public void Init(Vector3 _direction, Vector2Int result, Sprite icon = null) {
            direction = _direction;

            float[] dirAngles = new float[] { -10f, 10f };

            int[] nbde = new int[]{0,0};
            for(int j = 0; j < 2;j++)
            {
                if(result[j] <= 4)
                    nbde[j] = 4;
                else if(result[j] <= 6)
                    nbde[j] = 3;
                else if(result[j] <= 8)
                    nbde[j] = 2;
                else if(result[j] <= 12)
                    nbde[j] = 1;
                else
                    nbde[j] = 0;
            }

            int[] v = new int[] {Random.Range(4-nbde[0],4),Random.Range(9-nbde[1],9)};
            for(int i = 0;i < transform.childCount;i++)
            {
                if(i != v[0] && i != v[1])
                    GameObject.Destroy(transform.GetChild(i).gameObject);
            }

            string[] type = new string[] {"",""};
            for(int i=0;i<2;i++)
            {
                if(v[i]==3 || v[i]==8)
                    type[i] = "P";
                if(v[i]==1 || v[i]==6) 
                    type[i] = "";
                else
                    type[i] = "T"; 

            }


            for (int i = 0; i < 2; i++) {
                Transform child = transform.GetChild(v[i]);
                Rigidbody rigidbody = child.gameObject.GetComponent<Rigidbody>();
                if (rigidbody != null) {
                    Vector3 ndir = Quaternion.AngleAxis(dirAngles[i], Vector3.up) * direction;
                    Vector3 nforce = ndir * force;
                    Vector3 pos = child.transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));

                    rigidbody.AddForceAtPosition(nforce, pos, ForceMode.Force);
                }

                DiceScript ds = child.GetComponent<DiceScript>();
                if (ds != null) {
                    ds.Init(result[i],type[i]);
                }

                MeshRenderer mr = child.GetComponent<MeshRenderer>();
                if (mr != null && icon != null && type[i]=="") mr.materials[1].mainTexture = icon.texture;
                else mr.materials[1].mainTexture = Resources.Load<Texture2D>("Background" + type[i]);
            }
        }*/
    }
