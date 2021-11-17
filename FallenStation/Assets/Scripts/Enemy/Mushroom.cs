using System.Collections;
using UnityEngine;


    public class Mushroom : MonoBehaviour
    {
        [SerializeField]  private GameObject spore;
        private GameObject player;

        private void Start()
        {
            player = GameManager.Instance.GetPlayer();
            spore.SetActive(false);
        }


        // Update is called once per frame
        void Update()
        {
            StartCoroutine(Spore());

         }
        IEnumerator Spore()
        {
            spore.SetActive(true);
            yield return new WaitForSeconds(3f);
            spore.SetActive(false);
    }
    }
