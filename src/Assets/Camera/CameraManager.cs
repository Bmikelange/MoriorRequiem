using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StoreTransform
{
    public Vector3 position;
    public Quaternion rotation;
}

public class CameraManager : MonoBehaviour, ICombatObserver
{
    [SerializeField]
    CombatManager combatManager;                // pour appliquer le pattern observer

    private bool disable = true;                // bloc ou non l'animation
    public bool play = false;                   // détermine si on joue l'animation

    public float durationBeforeStart = 5.0f;    // temps d'inactivité avant de pouvoir lancer l'animation (en seconde)
    public float resolution = 0.05f;            // précision de la courbe
    public int indexAnim = 0;                   // indice de l'animation joué
    public List<float> animationDuration;       // temps total (en seconde) pour jouer toute l'animation
    public List<Transform> pathParent;          // le parcours

    private float inactiveTime = 0.0f;          // temps courant d'inactivité

    private StoreTransform original;            // position de départ de la caméra
    private StoreTransform targetPoint;         // prochain point de la courbe
    private int indexPoint = 0;                 // indice du point courant

    private List<StoreTransform> spline = new List<StoreTransform>();   // la spline générée
    private List<float> coeffSpline = new List<float>();                // les coeff [0,1] pour chaque partie de la courbe
    private int nbEnemies = 1;

    void OnDrawGizmos()
    {
        updateSpline();

        Gizmos.color = Color.green;

        for (int i = 0; i < spline.Count - 2; i++)
        {
            Gizmos.DrawLine(spline[i].position, spline[i + 1].position);
        }
    }

    private void Awake()
    {
        combatManager.addObserver(this);
    }

    void Start()
    {
        indexPoint = 0;
        original = new StoreTransform();
        original.position = transform.position;
        original.rotation = transform.rotation;
    }


    void computeCatmullRomSpline(int pos)
    {
        //on récupère les 4 points nécessaire (calcul de la courbe entre p1 et p2)
        Vector3 p0 = pathParent[indexAnim].GetChild(clampListPos(pos - 1)).transform.position;
        Vector3 p1 = pathParent[indexAnim].GetChild(pos).transform.position;
        Vector3 p2 = pathParent[indexAnim].GetChild(clampListPos(pos + 1)).transform.position;
        Vector3 p3 = pathParent[indexAnim].GetChild(clampListPos(pos + 2)).transform.position;

        // prise en compte de notre résolution
        int loops = Mathf.FloorToInt(1f / resolution);

        for (int i = 1; i <= loops; i++)
        {
            float t = i * resolution;

            //récupération de la coordonnée du point avec l'algo de Catmull-Rom spline
            Vector3 newPos = getCatmullRomPosition(t, p0, p1, p2, p3);
            Quaternion q = Quaternion.Slerp(pathParent[indexAnim].GetChild(pos).transform.rotation,
                                            pathParent[indexAnim].GetChild(clampListPos(pos + 1)).transform.rotation,
                                            t);
            // on stocke la transform
            StoreTransform st = new StoreTransform();
            st.position = newPos;
            st.rotation = q;
            spline.Add(st);
        }
    }

    // clamp pour l'algo de spline 
    int clampListPos(int pos)
    {
        if (pos < 0)
        {
            pos = pathParent[indexAnim].childCount - 1;
        }

        if (pos > pathParent[indexAnim].childCount)
        {
            pos = 1;
        }
        else if (pos > pathParent[indexAnim].childCount - 1)
        {
            pos = 0;
        }

        return pos;
    }

    // fonction repris de : http://www.iquilezles.org/www/articles/minispline/minispline.htm
    Vector3 getCatmullRomPosition(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        // les differents coeff du polymone cubique
        Vector3 a = 2f * p1;
        Vector3 b = p2 - p0;
        Vector3 c = 2f * p0 - 5f * p1 + 4f * p2 - p3;
        Vector3 d = -p0 + 3f * p1 - 3f * p2 + p3;

        // polynome cubique : a + b * t + c * t^2 + d * t^3
        return 0.5f * (a + (b * t) + (c * t * t) + (d * t * t * t));
    }

    void updateSpline()
    {
        spline.Clear();
        for (int i = 0; i < pathParent[indexAnim].childCount - 1; i++)
        {
            computeCatmullRomSpline(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!disable)
        {
            play = inactiveTime >= durationBeforeStart;
            inactiveTime += Time.deltaTime;
        }
        if (play)
        {
            movePosition();
            updateTarget();
        }
    }

    private void movePosition()
    {
        if (indexPoint == 0) // met directement la caméra au premier keypoint
        {
            transform.position = targetPoint.position;
            transform.rotation = targetPoint.rotation;
        }
        else
        {
            float time = coeffSpline[indexPoint - 1] * animationDuration[indexAnim];
            transform.DOMove(targetPoint.position, time);
            transform.DORotateQuaternion(targetPoint.rotation, time);
        }
    }

    private void updateTarget()
    {
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            indexPoint++;
            indexPoint %= spline.Count;
            targetPoint = spline[indexPoint];

            // l'animation est fini, on reset l'état l'animation
            if (indexPoint == 0)
            {
                reset();
            }
        }
    }

    // calcul la vitesse en fonction de la longueur de la courbe et du temps imparti
    void computeSpeed()
    {
        coeffSpline.Clear();
        float distance = 0.0f;

        for (int a = 0; a < spline.Count - 1; a++)
        {
            Vector3 from = spline[a].position;
            Vector3 to = spline[(a + 1) % spline.Count].position;
            distance += Vector3.Distance(from, to);
        }

        for (int a = 0; a < spline.Count - 1; a++)
        {
            Vector3 from = spline[a].position;
            Vector3 to = spline[(a + 1) % spline.Count].position;
            float dist = Vector3.Distance(from, to);

            coeffSpline.Add(dist / distance);
        }
    }

    void reset()
    {
        DOTween.Clear(); // clear les animations en cours (règles les pb d'asynch)

        play = false;
        disable = true;
        inactiveTime = 0.0f;

        transform.position = original.position;
        transform.rotation = original.rotation;

        indexPoint = 0;
        targetPoint = spline[indexPoint];
    }

    void ICombatObserver.notifyNewTurn(int turnEntityId)
    {
        if (turnEntityId == 0)
        {
            disable = false;
            indexPoint = 0;
            inactiveTime = 0.0f;
            // choose new spline
            indexAnim = Random.Range(nbEnemies - 1, nbEnemies + 1);
            updateSpline();
            targetPoint = spline[indexPoint];
            computeSpeed();
        }
        else disable = true;
    }

    void ICombatObserver.notifyAction(int entityId, ActionTurn action)
    {
        reset();
    }

    void ICombatObserver.notifyStateChanged(CombatState newState)
    {
        if (newState == CombatState.Win || newState == CombatState.GameOver)
        {
            disable = true;
            reset();
        }
    }

    void ICombatObserver.notifyCombatStart(List<int> enemiesId, bool boss)
    {
        disable = false;
        if (boss)
        {
            indexAnim = Random.Range(0, 2);
            nbEnemies = 1;
        }
        else
        {
            nbEnemies = enemiesId.Count;
            indexAnim = Random.Range(nbEnemies - 1, nbEnemies + 1);
        }

        updateSpline();
        targetPoint = spline[indexPoint];
        computeSpeed();
    }

    void ICombatObserver.notifyLifeChanged(int entityId, int life, int lifeMax) { }

    void ICombatObserver.notifyDied(int entityId) { }

    void ICombatObserver.notifyStuned(int entityId, int remainingTurn) { }

    void ICombatObserver.notifyAttackBlocked(int entityId, int remainingTurn) { }

    void ICombatObserver.notifyTakeDamage(int entityId, int value) { }

    void ICombatObserver.notifyHealed(int entityId, int value) { }

    void ICombatObserver.notifyEffectsChanged(int entityId, List<EntityEffect> effects) { }

    void ICombatObserver.notifySlotChanged(int entityId, List<DiceSlot> slots) { }

    void ICombatObserver.notifyEffectRoll(DiceEffect effect, Vector2Int result) { }

}
