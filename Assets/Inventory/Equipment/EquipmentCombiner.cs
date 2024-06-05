using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Arena.InvenSystem.Equipment
{
    public class EquipmentCombiner : MonoBehaviour
    {
        private readonly Dictionary<int, Transform> rootBoneDictionery = new Dictionary<int, Transform>();
        private readonly Transform transform;
        public EquipmentCombiner(GameObject rootGo)
        {
            transform = rootGo.transform;
            TraverseHierachy(transform);
        }
        public Transform AddLimb(GameObject itemGo, List<string> boneNames)
        {
            Transform limb = ProcessBoneObject(itemGo.GetComponentInChildren<SkinnedMeshRenderer>(), boneNames);
            limb.SetParent(transform);

            return limb;
        }
        private Transform ProcessBoneObject(SkinnedMeshRenderer renderer, List<string> boneNames)
        {
            Transform itemTransform = new GameObject().transform;
            SkinnedMeshRenderer meshRenderer = itemTransform.gameObject.AddComponent<SkinnedMeshRenderer>();
            Transform[] boneTransforms = new Transform[boneNames.Count];
            for (int i = 0; i < boneNames.Count; i++)
            {
                boneTransforms[i] = rootBoneDictionery[boneNames[i].GetHashCode()];
            }
            meshRenderer.bones = boneTransforms;
            meshRenderer.sharedMesh = renderer.sharedMesh;
            meshRenderer.materials = renderer.sharedMaterials;

            return itemTransform;

        }
        private Transform[] ProcessMeshObject(MeshRenderer[] meshRenderers)
        {
            List<Transform> itemTransforms = new List<Transform>();
            foreach (MeshRenderer meshRenderer in meshRenderers)
            {
                if (meshRenderer.transform.parent != null)
                {
                    Transform parent = rootBoneDictionery[meshRenderer.transform.parent.name.GetHashCode()];
                    GameObject itemGo = GameObject.Instantiate(meshRenderer.gameObject, parent);
                    itemTransforms.Add(itemGo.transform);
                }
            }
            return itemTransforms.ToArray();
        }
        public Transform[] AddMesh(GameObject itemGo)
        {
            Transform[] itemTransforms = ProcessMeshObject(itemGo.GetComponentsInChildren<MeshRenderer>());
            return itemTransforms;
        }

        private void TraverseHierachy(Transform root)
        {
            foreach (Transform child in root)
            {
                rootBoneDictionery.Add(child.GetHashCode(), child);
                TraverseHierachy(child);
            }
        }
    }
}