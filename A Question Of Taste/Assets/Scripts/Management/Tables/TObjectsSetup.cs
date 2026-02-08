using System;
using UnityEngine;

[Serializable]
public class TObjectsSetup
{
    public GameObject objPrefab;
    public Vector2 objTransform;
}

[Serializable]
public class TLevelEvents
{
    public bool firstOnAwake;
    public TCandleSetup[] candleEventSetup;
}

[Serializable]
public class TCandleSetup
{
    public Vector2[] candlesPosition;
}

[Serializable]
public class TCandleEvent
{
    
    public GameObject CallerPrefab;

    public TCandleSetup[] candlesSetup;
}


