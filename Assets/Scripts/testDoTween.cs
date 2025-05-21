using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

namespace DevDuck
{
    public class testDoTween : MonoBehaviour
    {
        public GameObject redObj, yellowObject;
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
               transform.DOMove(new Vector3(10, 0, 0), 1);
                //  redObj.GetComponent<Rigidbody>().AddForce(Vector3.forward * 10, ForceMode.Impulse);

            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Scene currentScene = SceneManager.GetActiveScene();
                if (currentScene != null)
                {
                    SceneManager.LoadSceneAsync(currentScene.name);
                }
            }
        }
        private void OnCollisionEnter(Collision collision)
        {
            Pedestrians pedestrians = collision.gameObject.GetComponent<Pedestrians>();
            if (pedestrians != null)
            {
                transform.DOKill();
                pedestrians.transform.DOKill(); 
                Vector3 dir = (pedestrians.transform.position - transform.position).normalized;
                pedestrians.GetComponent<Rigidbody>().AddForce(dir * 10, ForceMode.Impulse);
            }
        }
    }
}
