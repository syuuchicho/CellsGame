using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtensions
{
    /// <summary>
    /// �������g���܂ނ��ׂĂ̎q�I�u�W�F�N�g�̃��C���[��ݒ肵�܂�
    /// </summary>
    public static void SetLayerRecursively(this GameObject self,int layer)
    {
        self.layer = layer;

        foreach (Transform n in self.transform)
        {
            SetLayerRecursively(n.gameObject, layer);
        }
    }
}
