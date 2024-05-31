using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Pets.Scriptables;
using UnityEngine;

namespace _3._Scripts.Pets
{
    public class PetsHandler
    {
        public List<Pet> Pets { get; } = new();

        public void AddPet(Pet obj) => Pets.Add(obj);

        public void CreatePet(PetData data, Vector3 position)
        {
            var pet = Object.Instantiate(data.Prefab);
            pet.transform.position = position;
            pet.SetData(data);
            pet.Activate();
            AddPet(pet);
        }

        public void RemovePet(Pet pet) => Pets.Remove(pet);

        public void DestroyPet(string id)
        {
            var pet = Pets.FirstOrDefault(p => p.Data.ID == id);
            if (pet == null) return;

            pet.Deactivate();
            RemovePet(pet);
            Object.Destroy(pet.gameObject);
        }
    }
}