using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slutprojekt
{
    static class Assets
    {
        private static Texture2D bana1;
        private static Texture2D bana2;
        private static Texture2D t1U0;
        private static Texture2D t1U1;
        private static Texture2D t1U2;
        private static Texture2D t1U3;
        private static Texture2D t1Projectile;
        private static Texture2D t2U0;
        private static Texture2D t2U1;
        private static Texture2D t2U2;
        private static Texture2D t2U3;
        private static Texture2D t2Projectile;
        private static Texture2D frontMenuTex;
        private static Texture2D button;
        private static SpriteFont text;
        private static Texture2D exit;

        public static void LoadContent(ContentManager content)
        {
            bana1 = content.Load<Texture2D>("Bana1");
            bana2 = content.Load<Texture2D>("Bana2");
            t1U0 = content.Load<Texture2D>("T1U0");
            t1U1 = content.Load<Texture2D>("T1U1");
            t1U2 = content.Load<Texture2D>("T1U2");
            t1U3 = content.Load<Texture2D>("T1U3");
            t1Projectile = content.Load<Texture2D>("T1Projectile");
            t2U0 = content.Load<Texture2D>("T2U0");
            t2U1 = content.Load<Texture2D>("T2U1");
            t2U2 = content.Load<Texture2D>("T2U2");
            t2U3 = content.Load<Texture2D>("T2U3");
            t2Projectile = content.Load<Texture2D>("T2Projectile");
            frontMenuTex = content.Load<Texture2D>("Menu");
            button = content.Load<Texture2D>("Button");
            text = content.Load<SpriteFont>("File");
            exit = content.Load<Texture2D>("Exit");
        }

        public static Texture2D Button { get { return button; } }

        public static Texture2D Exit { get { return exit; } }

        public static SpriteFont Text
        {
            get { return text; }
        }

        public static Texture2D Bana1
        {      
            get { return bana1; }
        }      
               
        public static Texture2D Bana2
        {      
            get { return bana2; }
        }      
               
        public static Texture2D T1U0
        {      
            get { return t1U0; }
        }      
               
        public static Texture2D T1U1
        {      
            get { return t1U1; }
        }      
               
        public static Texture2D T1U2
        {      
            get { return t1U2; }
        }      
               
        public static Texture2D T1U3 //Måste fixa resten
        {      
            get { return t1U1; }
        }      
               
        public static Texture2D T1Projectile
        {      
            get { return t1U1; }
        }      
               
        public static Texture2D T2U0
        {      
            get { return t1U1; }
        }      
               
        public static Texture2D T2U1
        {
            get { return t1U1; }
        }

        public static Texture2D T2U2
        {
            get { return t1U1; }
        }

        public static Texture2D T2U3
        {
            get;
            private set;
        }

        public static Texture2D T2Projectile
        {
            get;
            private set;
        }

        public static Texture2D FrontMenuTex
        {
            get { return frontMenuTex; }
        }
    }
}
