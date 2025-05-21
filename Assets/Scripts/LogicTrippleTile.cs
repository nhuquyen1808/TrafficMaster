using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevDuck
{
    public class LogicTrippleTile : MonoBehaviour
    {
        public List<Sprite> animalSprites = new List<Sprite>();
        void Update()
        {
            if (Input.GetMouseButtonDown(0)) // Nhấn chuột trái
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit)) 
                {
                    
                    Debug.Log("Chọn đối tượng: " + hit.collider.gameObject.name);
                    SelectObject(hit.collider.gameObject);
                }
            }
        }

        void SelectObject(GameObject obj)
        {
            // Thay đổi màu object để dễ nhận biết
            obj.GetComponent<Renderer>().material.color = Color.red;
        }
    }
}
