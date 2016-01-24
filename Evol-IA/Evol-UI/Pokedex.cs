﻿using PokeRules;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace Evol_UI
{
    public class Pokedex
    {
        private static Pokedex activePokedex;
        public static Pokedex ActivePokedex
        {
            get
            {
                if (activePokedex == null)
                    activePokedex = new Pokedex();
                return activePokedex;
            }
        }

        private Dictionary<int, PokeData> data = new Dictionary<int, PokeData>();
        private Dictionary<string, Bitmap> spritesFront = new Dictionary<string, Bitmap>();
        private Dictionary<string, Bitmap> spritesBack = new Dictionary<string, Bitmap>();

        public PokeData GetData(int i)
        {
            if (data.ContainsKey(i))
                return data[i];
            else
                return null;
        }
        public Bitmap GetFrontSprite(string name)
        {
            if (spritesFront.ContainsKey(name))
                return spritesFront[name];
            else
                return null;
        }
        public Bitmap GetBackSprite(string name)
        {
            if (spritesBack.ContainsKey(name))
                return spritesBack[name];
            else
                return null;
        }
        public BitmapSource GetFrontSpriteSource(string name)
        {
            return GetSourceFromBitmap(GetFrontSprite(name));
        }

        public BitmapSource GetBackSpriteSource(string name)
        {
            return GetSourceFromBitmap(GetBackSprite(name));
        }

        private static BitmapSource GetSourceFromBitmap(Bitmap bmp)
        {
            if (bmp == null)
                return null;
            //else
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bmp.GetHbitmap(),
                IntPtr.Zero,
                System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
        }

        public bool LoadPokemon(int i)
        {
            if (data.ContainsKey(i))
                return true;
            //else
            string number = "_" + i.ToString("D3");

            Stream file = GenerateStreamFromString(Properties.Resources.ResourceManager.GetString(number));
            if (file == null)
                return false;
            //else
            XmlSerializer serializer = new XmlSerializer(typeof(PokeData));
            data.Add(i, (PokeData)serializer.Deserialize(file));

            string name = GetData(i).Name;
            Bitmap front = Properties.SpritesFrontResources.ResourceManager.GetObject(number) as Bitmap;
            Bitmap back = Properties.SpritesBackResources.ResourceManager.GetObject(number) as Bitmap;
            spritesFront.Add(name, front);
            spritesBack.Add(name, back);

            return true;
        }


        private Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}