using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    public InputActionReference shootAction;
    [SerializeField] private GameObject bulletOrigin;
    [SerializeField] private int sinType = 0;
    [SerializeField] private Bullet[] bulletTypes; // 8 different bullet types one normal and 7 different sin Types

    private void OnEnable()
    {
        shootAction.action.Enable();
        shootAction.action.performed += _ => Shoot();
    }

    private void Shoot()
    {
        //Middle of the screen
        Vector3 direction = Camera.main.transform.forward;
        bulletTypes[sinType].Shoot(bulletOrigin.transform, direction);
    }
}
