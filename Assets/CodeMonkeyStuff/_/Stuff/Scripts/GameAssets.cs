/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */
 
using UnityEngine;
using System.Reflection;
//using V_AnimationSystem;

public class GameAssets : MonoBehaviour {
    
    private static GameAssets _i;

    public static GameAssets i {
        get {
            if (_i == null) _i = Instantiate(Resources.Load<GameAssets>("GameAssets"));
            return _i;
        }
    }

    private void Awake() {
        _i = this;
    }

    [System.Serializable]
    public class PlacedObjectTypeSO_Refs {

        public PlacedObjectTypeSO conveyorBelt;
        public PlacedObjectTypeSO miningMachine;
        public PlacedObjectTypeSO smelter;
        public PlacedObjectTypeSO grabber;
        public PlacedObjectTypeSO assembler;
        public PlacedObjectTypeSO storage;
        /* Xiaohan */
        public PlacedObjectTypeSO electricalLine;
        public PlacedObjectTypeSO pipe;
        /* Xiaohan */

    }

    /*JDM*/
    public int[] buildingCosts;
    /*JDM*/

    public PlacedObjectTypeSO_Refs placedObjectTypeSO_Refs;



    [System.Serializable]
    public class ItemSO_Refs {

        public ItemSO ironOre;
        public ItemSO goldOre;
        public ItemSO ironIngot;
        public ItemSO goldIngot;
        public ItemSO computer;
        public ItemSO copperOre;
        public ItemSO copperIngot;
        public ItemSO microchip;

        public ItemSO any;
        public ItemSO none;
    }


    public ItemSO_Refs itemSO_Refs;



    [System.Serializable]
    public class ItemRecipeSO_Refs {

        public ItemRecipeSO ironIngot;
        public ItemRecipeSO goldIngot;
        public ItemRecipeSO computer;
        public ItemRecipeSO microchip;
        public ItemRecipeSO copperIngot;
    }


    public ItemRecipeSO_Refs itemRecipeSO_Refs;


    public Transform pfWorldItem;
    public Transform pfBeltDebugVisualNode;
    public Transform pfBeltDebugVisualLine;

    public Transform fxBuildingPlaced;

    public Transform sndBuilding;





    //public Sprite s_ShootFlash;
    
    //public Transform pfSwordSlash;
    //public Transform pfEnemy;
    //public Transform pfEnemyFlyingBody;
    //public Transform pfImpactEffect;
    //public Transform pfDamagePopup;
    //public Transform pfDashEffect;
    //public Transform pfProjectileArrow;
    //public Transform pfBolt;
    //public Transform pfSmoke;
    //public Transform pfChatBubble;

    //public Material m_WeaponTracer;
    //public Material m_MarineSpriteSheet;

    //public Material m_DoorRed;
    //public Material m_DoorGreen;
    //public Material m_DoorBlue;

    //public Material m_DoorKeyHoleRed;
    //public Material m_DoorKeyHoleGreen;
    //public Material m_DoorKeyHoleBlue;





    
    //public static class UnitAnimTypeEnum {

    //    static UnitAnimTypeEnum() {
    //        V_Animation.Init();
    //        FieldInfo[] fieldInfoArr = typeof(UnitAnimTypeEnum).GetFields(BindingFlags.Static | BindingFlags.Public);
    //        foreach (FieldInfo fieldInfo in fieldInfoArr) {
    //            if (fieldInfo != null) {
    //                fieldInfo.SetValue(null, UnitAnimType.GetUnitAnimType(fieldInfo.Name));
    //            }
    //        }
    //    }

    //    public static UnitAnimType dSwordTwoHandedBack_Idle;
    //    public static UnitAnimType dSwordTwoHandedBack_Walk;
    //    public static UnitAnimType dSwordTwoHandedBack_Sword;
    //    public static UnitAnimType dSwordTwoHandedBack_Sword2;

    //    public static UnitAnimType dMinion_Idle;
    //    public static UnitAnimType dMinion_Walk;
    //    public static UnitAnimType dMinion_Attack;

    //    public static UnitAnimType dShielder_Idle;
    //    public static UnitAnimType dShielder_Walk;

    //    public static UnitAnimType dSwordShield_Idle;
    //    public static UnitAnimType dSwordShield_Walk;

    //    public static UnitAnimType dMarine_Idle;
    //    public static UnitAnimType dMarine_Walk;
    //    public static UnitAnimType dMarine_Attack;

    //    public static UnitAnimType dBareHands_Idle;
    //    public static UnitAnimType dBareHands_Walk;
        

    //}




    //public static class UnitAnimEnum {

    //    static UnitAnimEnum() {
    //        V_Animation.Init();
    //        FieldInfo[] fieldInfoArr = typeof(UnitAnimEnum).GetFields(BindingFlags.Static | BindingFlags.Public);
    //        foreach (FieldInfo fieldInfo in fieldInfoArr) {
    //            if (fieldInfo != null) {
    //                fieldInfo.SetValue(null, UnitAnim.GetUnitAnim(fieldInfo.Name));
    //            }
    //        }
    //    }
        
    //    public static UnitAnim dMarine_AimWeaponRight;
    //    public static UnitAnim dMarine_AimWeaponRightInvertV;
    //    public static UnitAnim dMarine_ShootWeaponRight;
    //    public static UnitAnim dMarine_ShootWeaponRightInvertV;
        
    //}

}
