using UnityEngine;
using System;
using AWSIM.TrafficSimulation;

namespace AWSIM
{
    /// <summary>
    /// Script for Scene. By Scirpt-Execution-Order, This class is called earlier than other MonoBehaviour.
    /// Enables configuration at scene startup.
    /// </summary>
    public class AutowareSimulation : MonoBehaviour
    {
        [SerializeField] TrafficManager trafficManager;

        [Header("Player Config")]
        [SerializeField] string commandLineConfigParam = "--json_path";

        [Header("Editor Debug")]
        [SerializeField] bool useJsonConfig;

        [Tooltip("Specify this Path if you want to debug Json loading at Unity Editor runtime.")]
        [SerializeField] string jsonPath;

        [Serializable]
        public class Configuration
        {
            public float TimeScale;                 // Reflected in Time.timeScale
            public int RandomTrafficSeed;           // Reflected in TrafficManager.seed
            public int MaxVehicleCount;             // Reflected in TrafficManager.maxVehicleCount
        }

        void Awake()
        {

#if !UNITY_EDITOR
            // initialize
            useJsonConfig = false;
            jsonPath = "";

            // For player, if path is not specified as a command line argument, json is not read.
            useJsonConfig = CommandLineUtility.GetCommandLineArg(out jsonPath, commandLineConfigParam);
#endif

            if (useJsonConfig)
            {
                var config = CommandLineUtility.LoadJsonFromPath<Configuration>(jsonPath);
                Time.timeScale = config.TimeScale;
                trafficManager.seed = config.RandomTrafficSeed;
                trafficManager.maxVehicleCount = config.MaxVehicleCount;
            }
        }
    }
}