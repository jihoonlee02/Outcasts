using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class AirVentManager : MonoBehaviour
{
    # region Singleton
    private static AirVentManager instance;
    public static AirVentManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AirVentManager>();
            }
            return instance;
        }
    }
    #endregion
    [SerializeField] private AirVentGroupStruct[] totalAirVentPower;
    private AirVent[] airVents;
    private static Hashtable airVentsHashtable;

    public AirVentGroupStruct[] TotalAirVentPower => totalAirVentPower;

    private void Start() {
        InitializeAirVents();
    }

    private void Update() {
        
    }

    private void OnDestroy() {
        
    }

    private void InitializeAirVents() {
        //totalAirVentPower = new Tuple<int, float>[numberOfAirVentGroups];
        
        airVents = GameObject.FindObjectsOfType<AirVent>();
        airVentsHashtable = new Hashtable();
        foreach (AirVent airVent in airVents) {
            GameObject airVentGO = airVent.gameObject;
            if (!airVentsHashtable.ContainsKey(airVent.AirVentGroup)) {
                List<AirVent>[] tempAL = new List<AirVent>[2];
                tempAL[0] = new List<AirVent>();
                tempAL[1] = new List<AirVent>();
                airVentsHashtable.Add(airVent.AirVentGroup, tempAL);
            }
            List<AirVent>[] airVentAL = (List<AirVent>[])(airVentsHashtable[airVent.AirVentGroup]);
            if (airVent.Activated) {
                airVentAL[0].Add(airVent);
            } else {
                airVentAL[1].Add(airVent);
            }
        }
        foreach (AirVentGroupStruct airVentGroup in totalAirVentPower) {
            int airVentGroupNum = airVentGroup.airVentGroupNum;
            List<AirVent>[] tempAL = (List<AirVent>[])(airVentsHashtable[airVentGroupNum]);
            float partialPower = airVentGroup.airVentPower / tempAL[0].Count;
            foreach (AirVent airVent in tempAL[0]) {
                airVent.AirVentGroupStruct = airVentGroup;
                Transform airPivot = airVent.gameObject.transform.parent;
                airPivot.localScale = new Vector3(airPivot.localScale.x, partialPower, airPivot.localScale.z);
                airVent.VentSource.volume = (partialPower / airVentGroup.airVentPower) * 0.8f;
                //airVent.gameObject.transform.parent.localScale.y = partialPower;
            }
            foreach (AirVent airVent in tempAL[1]) {
                airVent.AirVentGroupStruct = airVentGroup;
            }
        }
    }

    public static void ActivateVent(AirVent airVent) {
        List<AirVent>[] airVentAL = (List<AirVent>[])(airVentsHashtable[airVent.AirVentGroup]);
        airVentAL[1].Remove(airVent);
        airVentAL[0].Add(airVent);
        Transform airPivot = airVent.gameObject.transform.parent;
        airPivot.localScale = new Vector3(airPivot.localScale.x, 0, airPivot.localScale.z);
        foreach (AirVent air in airVentAL[0]) {
            air.ChangePower(air.AirVentGroupStruct.airVentPower/airVentAL[0].Count);
        }
    }

    public static void DeactivateVent(AirVent airVent) {
        List<AirVent>[] airVentAL = (List<AirVent>[])(airVentsHashtable[airVent.AirVentGroup]);
        airVentAL[0].Remove(airVent);
        airVentAL[1].Add(airVent);
        Transform airPivot = airVent.gameObject.transform.parent;
        airPivot.localScale = new Vector3(airPivot.localScale.x, 0, airPivot.localScale.z);
        airVent.ChangePower(0);
        foreach (AirVent air in airVentAL[0]) {
            air.ChangePower(air.AirVentGroupStruct.airVentPower/airVentAL[0].Count);
        }
    }
}
