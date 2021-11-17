using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class Spore : MonoBehaviour
    {



        public float centerx;
        public float centery;
        public float centerz;
        public float growSpeed;

        private float time;
        private int height;



        // Update is called once per frame
        void Update()
        {
            time += Time.deltaTime;
            transform.localScale = new Vector3(2 + time * growSpeed, 2 + time * growSpeed, 2 + time * (growSpeed/2));

        }

        private void OnEnable()
        {
            transform.localScale = new Vector3(2, 2, 2);
            time = 0;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                GameObject player = other.gameObject;
                CharacterCombat enemyCombat = GetComponent<CharacterCombat>();
                if (enemyCombat != null)
                {
                    enemyCombat.Attack(player.GetComponent<CharacterStats>());
                }
            }
        }
    }
