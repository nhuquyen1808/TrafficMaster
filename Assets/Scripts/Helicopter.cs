using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace DevDuck
{
    public class Helicopter : MonoBehaviour
    {
        public Transform containCar;
        public Tween CurrentTween;
        Vector3 startHelicopterPosition;

        private void Start()
        {
            startHelicopterPosition = transform.position;
        }

        public void MoveToCar(Car car)
        {
            CurrentTween.Complete();
            transform.position = startHelicopterPosition;
            Vector3 midPoint = (transform.position + car.transform.position) / 2f + Vector3.up * 5;
            Vector3[] path = new Vector3[] { transform.position, midPoint, car.transform.position };
            CurrentTween = transform.DOPath(path, 1, PathType.CatmullRom)
                .SetEase(Ease.Linear).OnComplete(() =>
                {
                    DoSomething(car);
                    CurrentTween = transform.DOMove(transform.position + new Vector3(0, 2, 0), 0.5f)
                        .SetEase(Ease.Linear).OnComplete(
                            () =>
                            {
                                CurrentTween = transform.DOMove(car.transform.position + new Vector3(5, 10, 45), 2f)
                                    .SetDelay(.2f).OnComplete(
                                        () =>
                                        {
                                            Destroy(car.gameObject);
                                            transform.position = startHelicopterPosition;
                                        });
                            });
                });
        }

        public void DoSomething(Car car)
        {
            Destroy(car.GetComponent<Rigidbody>());
            GlobalData.isInGame = true;
            car.boxCollider.enabled = false;
            car.transform.SetParent(containCar);
            car.transform.SetParent(containCar);
            car.transform.localPosition = Vector3.zero;
        }
    }
}