﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace StructureHelper.ChestHelper
{
    class ChestEntity
    {
        public List<ChestRule> rules = new List<ChestRule>();

        public void SaveChestRulesFile()
        {
            string path = ModLoader.ModPath.Replace("Mods", "SavedStructures");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            string thisPath = Path.Combine(path, "SavedChest_" + DateTime.Now.ToString("d-M-y----H-m-s-f"));
            Main.NewText("Chest data saved as " + thisPath, Color.Yellow);
            FileStream stream = File.Create(thisPath);
            stream.Close();

            TagCompound tag = SaveChestRules();
            TagIO.ToFile(tag, thisPath);
        }

        public TagCompound SaveChestRules()
        {
            TagCompound tag = new TagCompound();

            tag.Add("Count", rules.Count);

            for (int k = 0; k < rules.Count; k++)
                tag.Add("Rule" + k, rules[k].Serizlize());

            return tag;
        }

        public static List<ChestRule> LoadChestRules(TagCompound tag)
        {
            var rules = new List<ChestRule>();
            int count = tag.GetInt("Count");

            for(int k = 0; k < count; k++)
                rules.Add(ChestRule.Deserialize(tag.GetCompound("Rule" + k)));

            return rules;
        }

        public static void SetChest(Chest chest, List<ChestRule> rules)
        {
            int index = 0;
            rules.ForEach(n => n.PlaceItems(chest, ref index));
        }
    }
}
