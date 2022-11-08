using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Shared
{
    public class Achievement
    {
        public Achievement(int id, string name, string description, string location, bool achieved, string imageName)
        {
            Id = id;
            Name = name;
            Description = description;
            Location = location;
            Achieved = achieved;
            ImageName= imageName;
        }
        public int Id;
        public string Name;
        public string Description;
        public string Location;
        public bool Achieved;
        public string ImageName;
    }
}