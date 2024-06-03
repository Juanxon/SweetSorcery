using System;
using UnityEngine;


    public class FunctionTimer
    {
        //Creamos un método del tipo FunctionTimer en el que creamos un GameObject al que le añadimos un nuevo FunctionTimer
        //Al que le pasamos por parámetro el método y el tiempo

            /// <summary>
            /// Llama a un método pasado un tiempo
            /// </summary>
            /// <param name="action"> Método o action que quieres llamar.</param>
            /// <param name="timer"> Tiempo en el que quieres que ocurrael método.</param>
        public static FunctionTimer Create(Action action, float timer){
            GameObject gameObject = new GameObject("Function Timer", typeof(TinyMonoBehabiour));
            FunctionTimer functionTimer = new FunctionTimer(action, timer, gameObject);

            //Guardamos en el delegado OnUpdate el método Update de FunctionTimer y empieza a ejecutarse en el Update del TinyMonoBehabiour
            gameObject.GetComponent<TinyMonoBehabiour>().OnUpdate = functionTimer.Update;
            return functionTimer;
        }
        //Clase dummy para tener acceso a las funciones de MonoBehaviour
        public class TinyMonoBehabiour : MonoBehaviour
        {
            public Action OnUpdate;
            private void Update() {
                if(OnUpdate != null)
                OnUpdate();
            }
        }
        private Action action;
        private float timer;
        private GameObject gameObject;
        private bool isDestroyed;
        //Constructor para el temporizador
        private FunctionTimer(Action action, float timer, GameObject gameObject)
        {
            this.action = action;
            this.timer = timer;
            this.gameObject = gameObject;
            isDestroyed = false;
        }
        //Funcion llamada update pero como no hereda de monobehabiour hay que ponerlo en el update de otro script
        public void Update()
        {
            if(!isDestroyed)
            {
                timer -= Time.deltaTime;
                if(timer < 0)
                {
                    action();
                    DestroySelf();
                }
            }
        }

        //Realmente no lo destruye pero sí hace que solo se haga una vez
        private void DestroySelf()
        {
            isDestroyed = true;
            UnityEngine.Object.Destroy(gameObject);
        }
    }

