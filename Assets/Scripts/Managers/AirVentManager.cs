using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirVentManager
{
    private static readonly object airVentManagerLock = new object();
    private static AirVentManager airVentManager;
    //private static 
    /*public static AirVentManager GetAirVentManager {
        get {
            if (airVentManager == null) {
                lock (airVentManagerLock) {
                    if (airVentManager == null) {
                        airVentManager = new AirVentManager();
                    }
                }
            }
            return airVentManager;
        }
    }**/
}
