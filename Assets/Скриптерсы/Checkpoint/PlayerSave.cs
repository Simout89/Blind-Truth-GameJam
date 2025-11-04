using UnityEngine;

namespace Скриптерсы
{
    public class PlayerSave
    {
        public Vector3 position;
        public float panTilt;

        public int ammo;

        public bool haveWeapon;

        public PlayerSave(Vector3 position, float panTilt, int ammo,  bool haveWeapon)
        {
            this.position = position;
            this.panTilt = panTilt;

            this.ammo = ammo;
            this.haveWeapon = haveWeapon;
        }
    }
}