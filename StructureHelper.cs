
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System.Linq;
using StructureHelper.ChestHelper;

namespace StructureHelper
{
    public class StructureHelper : Mod
    {
        public StructureHelper() { Instance = this; }

        public int PreviewWidth;
        public int PreviewHeight;
        public static StructureHelper Instance { get; set; }

        public override void Load()
        {
            if (System.IO.File.Exists(ModLoader.ModPath.Replace("Mods", "SavedStructures") + "/TestWandCache"))
            {
                TagCompound tag = TagIO.FromFile(ModLoader.ModPath.Replace("Mods", "SavedStructures") + "/TestWandCache");
                PreviewWidth = tag.GetInt("Width");
                PreviewHeight = tag.GetInt("Height");
            }
        }

        public override void Unload()
        {
            Instance = null;
        }

        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            if (Main.LocalPlayer.HeldItem.modItem is TestWandPaste)
            {
                spriteBatch.End();
                spriteBatch.Begin(default, default, default, default, default, default, Main.GameViewMatrix.ZoomMatrix);

                Texture2D tex = ModContent.GetTexture("StructureHelper/box");

                if (PreviewWidth >= 0 && PreviewHeight >= 0)
                {
                    float tileScale = 16 * Main.GameViewMatrix.Zoom.Length() * 0.707106688737f;
                    Vector2 pos = (Main.MouseWorld / tileScale).ToPoint16().ToVector2() * tileScale - Main.screenPosition;
                    pos = Vector2.Transform(pos, Matrix.Invert(Main.GameViewMatrix.ZoomMatrix));
                    pos = Vector2.Transform(pos, Main.UIScaleMatrix);

                    spriteBatch.Draw(tex, new Rectangle((int)pos.X, (int)pos.Y, PreviewWidth * 16 + 16, PreviewHeight * 16 + 16), tex.Frame(), Color.White * 0.25f, 0, Vector2.Zero, 0, 0);
                }

                spriteBatch.End();
                spriteBatch.Begin(default, default, default, default, default, default, Main.UIScaleMatrix);
            }

            if (Main.LocalPlayer.HeldItem.modItem is CopyWand)
            {
                spriteBatch.End();
                spriteBatch.Begin(default, default, default, default, default, default, Main.GameViewMatrix.ZoomMatrix);

                Texture2D tex = ModContent.GetTexture("StructureHelper/corner");
                Texture2D tex2 = ModContent.GetTexture("StructureHelper/box");
                Point16 TopLeft = (Main.LocalPlayer.HeldItem.modItem as CopyWand).TopLeft;
                int Width = (Main.LocalPlayer.HeldItem.modItem as CopyWand).Width;
                int Height = (Main.LocalPlayer.HeldItem.modItem as CopyWand).Height;

                float tileScale = 16 * Main.GameViewMatrix.Zoom.Length() * 0.707106688737f;
                Vector2 pos = (Main.MouseWorld / tileScale).ToPoint16().ToVector2() * tileScale - Main.screenPosition;
                pos = Vector2.Transform(pos, Matrix.Invert(Main.GameViewMatrix.ZoomMatrix));
                pos = Vector2.Transform(pos, Main.UIScaleMatrix);

                spriteBatch.Draw(tex, pos, tex.Frame(), Color.White * 0.5f, 0, tex.Frame().Size() / 2, 1, 0, 0);

                if (Width != 0 && TopLeft != null)
                {
                    spriteBatch.Draw(tex2, new Rectangle((int)(TopLeft.X * 16 - Main.screenPosition.X), (int)(TopLeft.Y * 16 - Main.screenPosition.Y), Width * 16 + 16, Height * 16 + 16), tex2.Frame(), Color.White * 0.25f);
                    spriteBatch.Draw(tex, (TopLeft.ToVector2() + new Vector2(Width + 1, Height + 1)) * 16 - Main.screenPosition, tex.Frame(), Color.Red, 0, tex.Frame().Size() / 2, 1, 0, 0);
                }
                if (TopLeft != null) spriteBatch.Draw(tex, TopLeft.ToVector2() * 16 - Main.screenPosition, tex.Frame(), Color.Cyan, 0, tex.Frame().Size() / 2, 1, 0, 0);

                spriteBatch.End();
                spriteBatch.Begin(default, default, default, default, default, default, Main.UIScaleMatrix);
            }

            if (Main.LocalPlayer.HeldItem.modItem is MultiWand)
            {
                spriteBatch.End();
                spriteBatch.Begin(default, default, default, default, default, default, Main.GameViewMatrix.ZoomMatrix);

                Texture2D tex = ModContent.GetTexture("StructureHelper/corner");
                Texture2D tex2 = ModContent.GetTexture("StructureHelper/box");
                Point16 TopLeft = (Main.LocalPlayer.HeldItem.modItem as MultiWand).TopLeft;
                int Width = (Main.LocalPlayer.HeldItem.modItem as MultiWand).Width;
                int Height = (Main.LocalPlayer.HeldItem.modItem as MultiWand).Height;
                int count = (Main.LocalPlayer.HeldItem.modItem as MultiWand).StructureCache.Count;

                float tileScale = 16 * Main.GameViewMatrix.Zoom.Length() * 0.707106688737f;
                Vector2 pos = (Main.MouseWorld / tileScale).ToPoint16().ToVector2() * tileScale - Main.screenPosition;
                pos = Vector2.Transform(pos, Matrix.Invert(Main.GameViewMatrix.ZoomMatrix));
                pos = Vector2.Transform(pos, Main.UIScaleMatrix);

                spriteBatch.Draw(tex, pos, tex.Frame(), Color.White * 0.5f, 0, tex.Frame().Size() / 2, 1, 0, 0);

                if (Width != 0 && TopLeft != null)
                {
                    spriteBatch.Draw(tex2, new Rectangle((int)(TopLeft.X * 16 - Main.screenPosition.X), (int)(TopLeft.Y * 16 - Main.screenPosition.Y), Width * 16 + 16, Height * 16 + 16), tex2.Frame(), Color.White * 0.25f);
                    spriteBatch.Draw(tex, (TopLeft.ToVector2() + new Vector2(Width + 1, Height + 1)) * 16 - Main.screenPosition, tex.Frame(), Color.Yellow, 0, tex.Frame().Size() / 2, 1, 0, 0);
                }
                if (TopLeft != null) spriteBatch.Draw(tex, TopLeft.ToVector2() * 16 - Main.screenPosition, tex.Frame(), Color.LimeGreen, 0, tex.Frame().Size() / 2, 1, 0, 0);

                spriteBatch.End();
                spriteBatch.Begin();
                Utils.DrawBorderString(spriteBatch, "Structures to save: " + count, Main.MouseScreen + new Vector2(0, 30), Color.White);

                spriteBatch.End();
                spriteBatch.Begin(default, default, default, default, default, default, Main.UIScaleMatrix);
            }
        }

        /// <summary>
        /// This method generates a structure from a file within your mod.
        /// </summary>
        /// <param name="path">The path to your structure file within your mod - this should not include your mod's folder, only the path beyond it.</param>
        /// <param name="pos">The position in the world in which you want your structure to generate, in tile coordinates.</param>
        /// <param name="mod">The instance of your mod to grab the file from. This should almost always be YourModClass.Instance.</param>
        /// ///<param name="fullPath">Indicates if you want to use a fully qualified path to get the structure file instead of one from your mod - generally should only used for debug purposes.</param>
        public static void GenerateStructure(string path, Point16 pos, Mod mod, bool fullPath = false)
        {
            TagCompound tag;

            if (!fullPath) tag = TagIO.FromStream(mod.GetFileStream(path));
            else tag = TagIO.FromFile(path);

            if (tag == null) throw new Exception("Path to structure was unable to be found. Are you passing the correct path?");
            Generate(tag, pos);        
        }

        internal static void GenerateDebugStructure(string path, Point16 pos, Mod mod, bool fullPath = false)
        {
            TagCompound tag;

            if (!fullPath) tag = TagIO.FromStream(mod.GetFileStream(path));
            else tag = TagIO.FromFile(path);

            if (tag == null) throw new Exception("Path to structure was unable to be found. Are you passing the correct path?");
            Generate(tag, pos, true);
        }

        public static void GenerateMultistructureRandom(string path, Point16 pos, Mod mod, bool fullPath = false)
        {
            TagCompound tag;

            if (!fullPath) tag = TagIO.FromStream(mod.GetFileStream(path));
            else tag = TagIO.FromFile(path);

            if (tag == null) throw new Exception("Path to structure was unable to be found. Are you passing the correct path?");

            List<TagCompound> structures = (List<TagCompound>)tag.GetList<TagCompound>("Structures");
            int index = WorldGen.genRand.Next(structures.Count);
            TagCompound targetStructure = structures[index];
            Generate(targetStructure, pos);
        }

        public static void GenerateMultistructureSpecific(string path, Point16 pos, Mod mod, int index, bool fullPath = false)
        {
            TagCompound tag;

            if (!fullPath) tag = TagIO.FromStream(mod.GetFileStream(path));
            else tag = TagIO.FromFile(path);

            if (tag == null) throw new Exception("Path to structure was unable to be found. Are you passing the correct path?");

            TagCompound targetStructure = ((List<TagCompound>)tag.GetList<TagCompound>("Structures"))[index];
            Generate(targetStructure, pos);
        }

        public static void GenerateChest(string path, Point16 pos, Mod mod, int tileType)
        {
            int i = WorldGen.PlaceChest(pos.X, pos.Y, (ushort)tileType);
            if (i == -1) return;

            var tag = TagIO.FromStream(mod.GetFileStream(path));
            if (tag == null) throw new Exception("Path to chest was unable to be found. Are you passing the correct path?");

            Item item = new Item();
            item.SetDefaults(1);

            Chest chest = Main.chest[i];
            ChestEntity.SetChest(chest, ChestEntity.LoadChestRules(tag));
        }

        internal static void Generate(TagCompound tag, Point16 pos, bool ignoreNull = false)
        {
            List<TileSaveData> data = (List<TileSaveData>)tag.GetList<TileSaveData>("TileData");
            if(data == null) throw new Exception("Corrupt or Invalid structure data.");

            for (int x = 0; x <= tag.GetInt("Width"); x++)
            {
                for (int y = 0; y <= tag.GetInt("Height"); y++)
                {
                    bool isNullTile = false;
                    bool isNullWall = false;
                    int index = y + x * (tag.GetInt("Height") + 1);
                    TileSaveData d = data[index];
                    Tile tile = Framing.GetTileSafely(pos.X + x, pos.Y + y);

                    if (!int.TryParse(d.Tile, out int type))
                    {
                        string[] parts = d.Tile.Split();
                        if (parts[0] == "StructureHelper" && parts[1] == "NullBlock" && !ignoreNull) isNullTile = true;

                        else if (parts.Length > 1 && ModLoader.GetMod(parts[0]) != null && ModLoader.GetMod(parts[0]).TileType(parts[1]) != 0)
                            type = ModLoader.GetMod(parts[0]).TileType(parts[1]);

                        else try
                            {
                                Type tileType = Type.GetType(d.Tile);
                                var getType = typeof(ModContent).GetMethod("TileType", BindingFlags.Static | BindingFlags.Public);
                                type = (int)getType.MakeGenericMethod(tileType).Invoke(null, null);
                            }
                            catch { type = 0; }
                    }

                    if (!int.TryParse(d.Wall, out int wallType))
                    {
                        string[] parts = d.Wall.Split();
                        if (parts[0] == "StructureHelper" && parts[1] == "NullWall" && !ignoreNull) isNullWall = true;

                        else if (parts.Length > 1 && ModLoader.GetMod(parts[0]) != null && ModLoader.GetMod(parts[0]).WallType(parts[1]) != 0)
                            wallType = ModLoader.GetMod(parts[0]).WallType(parts[1]);
                            
                        else try
                            {
                                Type wallTypeType = Type.GetType(d.Wall); //I am so sorry for this name
                                var getWallType = typeof(ModContent).GetMethod("WallType", BindingFlags.Static | BindingFlags.Public);
                                wallType = (int)getWallType.MakeGenericMethod(wallTypeType).Invoke(null, null);
                            }
                            catch { wallType = 0; }
                    }


                    if (!d.Active) isNullTile = false;
                    if (!isNullTile || ignoreNull) //leave everything else about the tile alone if its a null block
                    {
                        tile.ClearEverything();
                        tile.type = (ushort)type;
                        tile.frameX = d.FrameX;
                        tile.frameY = d.FrameY;
                        tile.slope(d.Slope);
                        tile.halfBrick(d.HalfSlab);
                        tile.actuator(d.HasActuator);
                        tile.inActive(d.Actuated);
                        tile.liquid = d.Liquid;
                        tile.liquidType(d.LiquidType);
                        tile.color(d.Color);
                        tile.wallColor(d.WallColor);
                        tile.wire(d.Wire[0] > 0);
                        tile.wire2(d.Wire[1] > 0);
                        tile.wire3(d.Wire[2] > 0);
                        tile.wire4(d.Wire[3] > 0);
                        tile.active(d.Active);
                        if (!d.Active) tile.inActive(false);

                        if (d.TEType != "") //place and load a tile entity
                        {
                            int typ;

                            if (!int.TryParse(d.TEType, out typ))
                            {
                                string[] parts = d.TEType.Split();
                                typ = ModLoader.GetMod(parts[0]).TileEntityType(parts[1]);
                            }

                            if (d.TEType != "")
                            {
                                TileEntity.PlaceEntityNet(pos.X + x, pos.Y + y, typ);
                                if (d.TEData != null && typ > 2) (TileEntity.ByPosition[new Point16(pos.X + x, pos.Y + y)] as ModTileEntity).Load(d.TEData);
                            }
                        }
                    }

                    if (!isNullWall || ignoreNull) //leave the wall alone if its a null wall
                    {
                        tile.wall = (ushort)wallType;
                        tile.wallFrameX(d.WFrameX);
                        tile.wallFrameY(d.WFrameY);
                    }

                }
            }
        }
    }
}