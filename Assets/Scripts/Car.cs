using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DevDuck;
using UnityEngine.Serialization;

public enum Directions
{
    UP,
    DOWN,
    RIGHT,
    LEFT
}

public enum CarType
{
    NORMAL,
    TANKER_TRUCK
}

public class Car : MonoBehaviour
{
    public CarType carType;
    [SerializeField] CarColor carColor;
    public List<Directions> ListDirection = new List<Directions>();
    [SerializeField] MapPoint from;
    public Vector3 startPos;
    public Transform dummyTarget;
    public List<Vector3> ListPosition = new List<Vector3>();
    [SerializeField] float delayTime;
    public LayerMask carLayer;
    public LayerMask pointLayer;
    public Sequence sequence;
    public Sequence sequenceDummy;
    int t = 0;
    bool canInsCoin;
    public BoxCollider boxCollider;
    RaycastHit _hitInfo;
    public bool isOnRunning;
    private CarDirectionSprite carDirectionSprite;

    public bool isCheckRedLight;
    
    private void Start()
    {
        startPos = transform.position;
        boxCollider = GetComponent<BoxCollider>();
        carColor = GetComponent<CarColor>();
        carDirectionSprite = GetComponent<CarDirectionSprite>();
        // collider = gameObject.GetComponent<BoxCollider>();
        GetDirRayCast();
        StartCoroutine(AddPoint());
    }

    public void CarMovement()
    {
        isOnRunning = true;
        isCheckRedLight = true;
        GetListPosition();
        CheckCanClick();
        AudioManager.instance.PlaySound("CarEngine");
        if (CheckCanClick() /* && carType == CarType.NORMAL*/)
        {
            LogicGame.instance.carAmount--;
            if (carDirectionSprite!= null)
            {
                carDirectionSprite.sprDirection.sprite = null;
            }
            gameObject.layer = LayerMask.NameToLayer("CarDisable");
            LogicGame.instance.CheckWin();
            isOnRunning = false;

        }

        t = 0;
        dummyTarget.transform.localPosition = new Vector3(0, 0.5f, 0);
        sequence = DOTween.Sequence();
        sequence.PrependInterval(delayTime);
        sequenceDummy = DOTween.Sequence();
        for (int i = 1; i < ListPosition.Count; ++i)
        {
            Vector3 temp = ListPosition[i] - ListPosition[i - 1];
            //  sequence.PrependCallback(() => { canInsCoin = true; });
            sequence.Append(transform.DOMove(ListPosition[i], (ListPosition[i] - ListPosition[i - 1]).magnitude / 25f)
                .OnUpdate(() =>
                {
                    if (ListDirection.Count > 1)
                    {
                        Vector3 angle = (dummyTarget.transform.position - transform.position).normalized;
                        float v = Mathf.Atan2(angle.z, -angle.x) * Mathf.Rad2Deg;
                        transform.rotation =
                            Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, v - 90, 0), 20f);
                    }

                    PosToInstantiateCoin();
                })
                .OnComplete(() => { ++t; }
                ).SetEase(Ease.Linear)).SetAutoKill(true);
            sequenceDummy
                .Append(dummyTarget.DOMove(ListPosition[i], (ListPosition[i] - ListPosition[i - 1]).magnitude / 25f)
                    .SetEase(Ease.Linear)).SetAutoKill(true);
        }
    }

    private bool CheckCanClick()
    {
        for (int i = 1; i < ListPosition.Count; ++i)
        {
            Vector3 temp = ListPosition[i] - ListPosition[i - 1];

            if (Physics.Raycast(ListPosition[i - 1] + new Vector3(0, 0.5f, 0), temp, out _hitInfo, temp.magnitude,
                    carLayer))
            {
                Observer.Notify(EventAction.EVENT_CAR_DONE_ACTION, false);
                return false;
            }
        }

        Observer.Notify(EventAction.EVENT_CAR_DONE_ACTION, true);
        return true;
    }

    public bool CheckCanMove()
    {
        GetListPosition();
        for (int i = 1; i < ListPosition.Count; ++i)
        {
            Vector3 temp = ListPosition[i] - ListPosition[i - 1];

            if (Physics.Raycast(ListPosition[i - 1] + new Vector3(0, 0.5f, 0), temp, out _hitInfo, temp.magnitude,
                    carLayer))
            {
                return false;
            }
        }

        return true;
    }

    private void GetListPosition()
    {
        ListPosition.Add(startPos);
        MapPoint current = from;
        MapPoint last = null;
        bool aChance = true;
        for (int i = 0; i >= 0 && i < ListDirection.Count; ++i)
        {
            last = current;
            switch (ListDirection[i])
            {
                case Directions.UP:
                    current = current.up;
                    break;
                case Directions.DOWN:
                    current = current.down;
                    break;
                case Directions.LEFT:
                    current = current.left;
                    break;
                case Directions.RIGHT:
                    current = current.right;
                    break;
            }

            // 
            if (current != null)
            {
                aChance = true;

                ListPosition.Add(current.transform.position);
                if (i == ListDirection.Count - 1)
                {
                    ListPosition.Add(current.transform.position +
                                     (current.transform.position - last.transform.position).normalized * 100);
                }
            }
            //        
            else if (aChance)
            {
                i -= 2;
                current = last;
                aChance = false;
            }
            else break;
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Car") && isOnRunning)
        {
            isCheckRedLight = false;

            if (carType == CarType.NORMAL)
            {
                Car carTrigger = col.transform.GetComponent<Car>();
                carTrigger.ShakeCar(transform.forward, carTrigger.startPos);
                Observer.Notify(EventAction.EVENT_CAR_TRIGGER, carTrigger.transform.position);
                sequence.Pause();
                sequenceDummy.Pause();
                dummyTarget.localPosition = Vector3.zero ;

                sequence = DOTween.Sequence();
                sequence.PrependInterval(delayTime);
                sequenceDummy = DOTween.Sequence();
                ListPosition[t + 1] = col.transform.position;
                for (int i = t; i >= 0; --i)
                {
                    sequence.Append(transform
                        .DOMove(ListPosition[i], (ListPosition[i + 1] - ListPosition[i]).magnitude / 25f).OnUpdate(() =>
                        {
                            if (ListDirection.Count > 1)
                            {
                                Vector3 angle = (dummyTarget.transform.position - transform.position).normalized;
                                float v = Mathf.Atan2(angle.z, -angle.x) * Mathf.Rad2Deg;
                                transform.rotation = Quaternion.RotateTowards(transform.rotation,
                                    Quaternion.Euler(0, v + 90, 0), 20f);
                            }
                        }).SetEase(Ease.Linear).OnComplete(() =>
                        {
                            ++t;
                            if (this.transform.position == startPos)
                            {
                                Observer.Notify(EventAction.EVENT_CAR_DONE_ACTION, true);
                                ListPosition.Clear();
                                isOnRunning = false;
                            }
                        })).SetAutoKill(true);

                    sequenceDummy
                        .Append(dummyTarget
                            .DOMove(ListPosition[i], (ListPosition[i + 1] - ListPosition[i]).magnitude / 25f)
                            .SetEase(Ease.Linear)).SetAutoKill(true);
                }
                
                AudioManager.instance.PlaySound("CarCrash");
                AudioManager.instance.PlaySound("CarEngine");
            }

            if (carType == CarType.TANKER_TRUCK)
            {
                sequenceDummy.Pause();
                sequence.Pause();
                Debug.Log("Lose because hit tanker truck");
                GlobalData.isInGame = false;
                this.GetComponent<TankerTruck>().PlayFireExplosion();
                this.GetComponent<Animator>().Play("CarBreak");
                DOVirtual.DelayedCall(1, (() => LogicUI.ins.ShowLosePopup(LOSETYPE.TRUNK_TANKER_HITTED)));
                AudioManager.instance.PlaySound("CarCrash");

                Debug.Log("?????");
            }
        }
        Pedestrians pedestrians = col.gameObject.GetComponent<Pedestrians>();
        if (pedestrians != null)
        {
            sequenceDummy.Pause();
            sequence.Pause();
            this.transform.DOKill();
           // pedestrians.transform.DOKill();
           pedestrians.Animator.enabled = false;
            Vector3 dir = (pedestrians.transform.position - transform.position).normalized;
            pedestrians.GetComponent<Rigidbody>().AddForce(dir * 2, ForceMode.Impulse);
           // this.GetComponent<Rigidbody>().AddForce(dir * -3, ForceMode.Impulse);
            Debug.Log("hit pedestrian then show lose panel");
            DOVirtual.DelayedCall(1, (() => LogicUI.ins.ShowLosePopup(LOSETYPE.HUMAND_HITTED)));
        }
        TankerTruck tankerTruck = col.gameObject.GetComponent<TankerTruck>();
        if (tankerTruck != null)
        {
            sequenceDummy.Pause();
            sequence.Pause();
            this.GetComponent<Animator>().Play("CarBreak");
            tankerTruck.PlayFireExplosion();
            this.gameObject.layer = LayerMask.NameToLayer("CarDisable");
            DOVirtual.DelayedCall(1, (() => LogicUI.ins.ShowLosePopup(LOSETYPE.TRUNK_TANKER_HITTED)));
            AudioManager.instance.PlaySound("CarCrash");

        }
    }


    private void ShakeCar(Vector3 dir, Vector3 startPosition)
    {
        Vector3 des = startPosition + dir;
        transform.DOMove(des, /*(des - startPosition).magnitude /5f*/ 0.2f).OnComplete(() =>
        {
            transform.DOMove(startPosition, /*(des - startPosition).magnitude / 5*/ 0.2f);
        });
    }

    public void PosToInstantiateCoin()
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(boxCollider.transform.position);
        if ((viewportPos.x < 0 || viewportPos.x > 1 || viewportPos.y < 0 || viewportPos.y > 1) && !canInsCoin)
        {
            canInsCoin = true;
            Observer.Notify(EventAction.EVENT_GET_COIN_CAR, boxCollider.transform.position);
            Observer.Notify(EventAction.EVENT_CAR_DISABLE, this);
            // LogicGame.instance.CheckLose();
            sequenceDummy.Complete();
            sequence.Complete();
            AudioManager.instance.PlaySound("Coin");
            LogicGame.instance.CheckWin();
            Destroy(boxCollider.gameObject, 0.5f);
        }
        /*var bounds = meshCol.bounds;
        cameraFrustum = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        if (GeometryUtility.TestPlanesAABB(cameraFrustum, bounds))
        {
        }
        else
        {
            if (!canInsCoin)
            {
                Vector3 v = meshCol.transform.position - 2.5f * transform.forward + new Vector3(0, 2f, 0);
                Observer.Notify(EventAction.EVENT_CAR_DONE_ACTION, true);
                //  Time.timeScale = 0 ;
                canInsCoin = true;

                Debug.Log("out off scene  : " + v);
            }
        }*/
    }

    IEnumerator AddPoint()
    {
        yield return new WaitForSeconds(0.2f);
        RaycastHit hitPoint;
        if (Physics.Raycast(transform.position /*+ new Vector3(0, 0.5f, 0)*/, -GetDirRayCast(), out hitPoint,
                Mathf.Infinity, pointLayer))
        {
            from = hitPoint.transform.gameObject.GetComponent<MapPoint>();
        }
    }

    public Vector3 GetDirRayCast()
    {
        if (ListDirection[0] == Directions.UP)
        {
            return Vector3.forward;
        }

        if (ListDirection[0] == Directions.DOWN)
        {
            return -Vector3.forward;
        }

        if (ListDirection[0] == Directions.LEFT)
        {
            return Vector3.left;
        }
        else
        {
            return Vector3.right;
        }
    }

    public Vector3 GetLastDirection()
    {
        if (ListDirection[ListDirection.Count - 1] == Directions.UP)
        {
            return Vector3.forward;
        }

        if (ListDirection[ListDirection.Count - 1] == Directions.DOWN)
        {
            return -Vector3.forward;
        }

        if (ListDirection[ListDirection.Count - 1] == Directions.LEFT)
        {
            return Vector3.left;
        }
        else
        {
            return Vector3.right;
        }
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), -transform.forward * 50f, Color.red);
        Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), transform.forward * 50f, Color.yellow);
    }
}