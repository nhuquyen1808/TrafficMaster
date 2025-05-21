using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager 
{
   public const string Coin = "Coin";
   public const string LevelUnlock = "LevelUnlock";
   public const string hint  =  "Hint";
   public const string FIRST_TIME_DOWNLOAD  = "FirstTimeDownload";
   public const string LevelReal = "LevelReal";
   public const string UI_HOME = "UIHome";
   
   public const string hintAmount =  "HintAmount";
   public const string helicopterAmount =  "HelicopterAmount";
   public const string land =  "land";
}

public static class GlobalData
{
   public static bool isInGame;
   
}
