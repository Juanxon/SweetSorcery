using UnityEngine;


    public static class ToolBox
    {
            /// <summary>
            /// Convierte un valor float en minutos y segundos.
            /// </summary>
            /// <param name="timeToDisplay">El valor de tiempo a mostrar.</param>
            /// <returns>La cadena de tiempo formateada en minutos y segundos.</returns>
        public static string FloatToTime(float timeToDisplay)
        {
            float minutes = Mathf.FloorToInt(timeToDisplay / 60);
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);

            return string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        public static float ApplyPercentage(float number, float percentage)
        {
            if(percentage == 0)
            {
                return 0;
            }
            float result = number * (percentage / 100);
            return result;
        }
            /// <summary>
            /// Devuelve number mas el porcentaje aplicado.
            /// </summary>
        public static float AddPercentage(float number, float percentage)
        {
            if(percentage == 0)
            {
                return number;
            }
            float result = number + (number * (percentage / 100));
            return result;
        }
        public static Vector3 Direction(Vector3 me, Vector3 other)
        {
            Vector3 direction = other - me;
            direction = direction.normalized;
            return direction;
        }

        public static Vector3 ObjectForward(Transform objectTransform)
        {
            Vector3 forward = objectTransform.TransformDirection(Vector3.forward);
            return forward;
        }

        /// <summary>
        /// Carga una escena en Unity por su nombre o índice.
        /// </summary>
        /// <param name="scene">El nombre o índice de la escena a cargar.</param>
        public static void LoadScene(string scene)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
        }

        /// <summary>
        /// Carga una escena en Unity por su nombre o índice.
        /// </summary>
        /// <param name="scene">El nombre o índice de la escena a cargar.</param>
        public static void LoadScene(int scene)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
        }
    }

