using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class WeaponSwitching : MonoBehaviour
{
    InputAction switching;
    public int selectedWeapon = 0;
    public TextMeshProUGUI ammoInfo;
    void Start()
    {
        switching = new InputAction("Scrool", binding: "<mouse>/scroll");
        switching.Enable();

        SelectWeapon();
    }

    void Update()
    {
        Gun currentGun = FindObjectOfType<Gun>();

        ammoInfo.text = $"{currentGun.currentAmmo}/{currentGun.magazineSize}";

        float scrollValue = switching.ReadValue<Vector2>().y;

        int previousSelected = selectedWeapon;
        if (scrollValue > 0)
        {
            selectedWeapon++;
            if (selectedWeapon > 2)
                selectedWeapon = 0;
        }
        else if (scrollValue < 0)
        {
            selectedWeapon--;
            if (selectedWeapon < 0)
                selectedWeapon = 2;
        }

        if (previousSelected != selectedWeapon)
            SelectWeapon();

    }

    private void SelectWeapon()
    {
        foreach (Transform weapon in transform)
            weapon.gameObject.SetActive(false);

        transform.GetChild(selectedWeapon).gameObject.SetActive(true);
    }
}
