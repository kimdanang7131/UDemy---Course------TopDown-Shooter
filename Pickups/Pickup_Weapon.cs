using UnityEngine;

public class Pickup_Weapon : Interactable
{
    [SerializeField] private Weapon_Data weaponData;
    [SerializeField] private Weapon weapon;

    [SerializeField] private BackupWeaponModel[] models;

    private bool oldWeapon; // 이미 생성되어서 떨군 무기인지 아닌지 구분하기 위해서

    void Start()
    {
        if (oldWeapon == false)
            weapon = new Weapon(weaponData);

        SetupGameObject();
    }

    // 무기를 떨어트릴 때 현재 무기에 대한 정보를 넣어주기 위해서 ( 다시 주울때를 위해 )
    public void SetupPickupWeapon(Weapon weapon, Transform transform)
    {
        oldWeapon = true;

        this.weapon = weapon;
        weaponData = weapon.weaponData;

        this.transform.position = transform.position + new Vector3(0, .75f, 0);
    }

    private void SetupWeaponModel()
    {
        foreach (BackupWeaponModel model in models)
        {
            model.gameObject.SetActive(false);

            // 현재 weaponData와 일치하는 모델만 활성화
            if (model.weaponType == weaponData.weaponType)
            {
                model.gameObject.SetActive(true);
                UpdateMeshAndMaterial(model.GetComponent<MeshRenderer>());
            }
        }
    }

    public override void Interaction()
    {
        weaponController.PickupWeapon(weapon);
        ObjectPool.instance.ReturnObject(gameObject);
    }

    [ContextMenu("Update GameObject Name")]
    public void SetupGameObject()
    {
        gameObject.name = "Pickup_Weapon - " + weaponData.weaponType.ToString();

        SetupWeaponModel();
    }
}
