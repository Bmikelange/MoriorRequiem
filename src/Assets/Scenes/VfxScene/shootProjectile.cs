using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootProjectile : MonoBehaviour {
    public GameObject ball;
    public GameObject fire;
    public GameObject heal;
    public GameObject dHeal;
    public GameObject drainTarget;
    public GameObject drainProjectile;
    public GameObject thunder;
    public GameObject stun;
    public GameObject dice;
    public GameObject rock;
    public GameObject jaw;
    public GameObject shield;
    public GameObject projectileVFX;
    public Transform enemyTransform;
    public float force = 10000.0f;
    public float forceDice = 10.0f;
    public Vector3 offset = new Vector3(0, 1, 0);

    List<GameObject> instancedVFX = new List<GameObject>();
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.A)) {
            GameObject projectile = Instantiate(rock, transform.position, transform.rotation);
            instancedVFX.Add(projectile);
        }

        if (Input.GetKeyDown(KeyCode.B)) {
            GameObject projectile = Instantiate(thunder, transform.position, transform.rotation);
            instancedVFX.Add(projectile);
        }

        if (Input.GetKeyDown(KeyCode.C)) {
            GameObject projectile = Instantiate(fire, transform.position, transform.rotation);
            instancedVFX.Add(projectile);
        }

        if (Input.GetKeyDown(KeyCode.D)) {
            GameObject projectile = Instantiate(drainTarget, transform.position, transform.rotation);
            instancedVFX.Add(projectile);

            // TODO : projectile instantiation
            projectile = Instantiate(projectileVFX, transform.position + offset, transform.rotation);
            ProjectileVFX projVFX = projectile.GetComponent<ProjectileVFX>();
            if (projVFX) {
                projVFX.Init(transform.position + Vector3.up * 2f, enemyTransform.position + Vector3.up, drainProjectile, 0.75f);
            }
            instancedVFX.Add(projectile);
        }

        if (Input.GetKeyDown(KeyCode.G)) {
            GameObject projectile = Instantiate(stun, transform.position, transform.rotation);
            instancedVFX.Add(projectile);
        }

        if (Input.GetKeyDown(KeyCode.H)) {
            GameObject projectile = Instantiate(heal, transform.position, transform.rotation);
            instancedVFX.Add(projectile);
        }

        if (Input.GetKeyDown(KeyCode.P)) {
            GameObject projectile = Instantiate(shield, transform.position, transform.rotation);
            instancedVFX.Add(projectile);
        }

        if (Input.GetKeyDown(KeyCode.Q)) {
            GameObject projectile = Instantiate(jaw, transform.position, transform.rotation);
            instancedVFX.Add(projectile);
        }

        if (Input.GetKeyDown(KeyCode.R)) {

            foreach (var o in instancedVFX) {
                GameObject.Destroy(o);
            }
            instancedVFX.Clear();
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            GameObject projectile = Instantiate(projectileVFX, transform.position, transform.rotation);
            ProjectileVFX projVFX = projectile.GetComponent<ProjectileVFX>();
            if (projVFX) {
                projVFX.Init(transform.position + Vector3.up * 2f, enemyTransform.position + Vector3.up, ball, 0.75f);
            }
            instancedVFX.Add(projectile);

        }

        if (Input.GetKeyDown(KeyCode.V)) {
            GameObject projectile = Instantiate(dHeal, transform.position, transform.rotation);
            instancedVFX.Add(projectile);
        }

        if (Input.GetKeyDown(KeyCode.X)) {
            GameObject projectile = Instantiate(dice, transform.position, transform.rotation);
            projectile.GetComponent<DiceVFX>().Init(transform.forward, new Vector2Int(Random.Range(0, 99), Random.Range(0, 99)), new Vector2Int(Random.Range(0, 99), Random.Range(0, 99)), null);
            instancedVFX.Add(projectile);
        }

    }
}
