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
        private static Texture2D settings;
        private static Texture2D blank; //Ifall jag bara vill ha en knapp
        private static Texture2D enemy1;
        private static Texture2D enemy2;
        private static Texture2D enemy3;
        private static Texture2D partialMenu;
        private static Texture2D circle;
        private static Texture2D arrowRight;
        private static Texture2D arrowLeft;

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
            settings = content.Load<Texture2D>("Settings");
            blank = content.Load<Texture2D>("Blank");
            enemy1 = content.Load<Texture2D>("Enemy1");
            enemy2 = content.Load<Texture2D>("Enemy2");
            enemy3 = content.Load<Texture2D>("Enemy3");
            partialMenu = content.Load<Texture2D>("PartialMenu");
            circle = content.Load<Texture2D>("RangeCircle");
            arrowRight = content.Load<Texture2D>("ArrowRight");
            arrowLeft = content.Load<Texture2D>("ArrowLeft");
        }

        public static Texture2D PartialMenu { get { return partialMenu; } }

        public static Texture2D Button { get { return button; } }

        public static Texture2D Exit { get { return exit; } }

        public static Texture2D Settings { get { return settings; } }
        
        public static Texture2D Blank { get { return blank; } }

        public static Texture2D Enemy1 { get { return enemy1; } }

        public static Texture2D Enemy2 { get { return enemy2; } }

        public static Texture2D Enemy3 { get { return enemy3; } }

        public static SpriteFont Text { get { return text; } }

        public static Texture2D Circle { get { return circle; } }

        public static Texture2D ArrowRight { get { return arrowRight; } }

        public static Texture2D ArrowLeft { get { return arrowLeft; } }

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
               
        public static Texture2D T1U3
        {      
            get { return t1U3; }
        }      
               
        public static Texture2D T1Projectile
        {      
            get { return t1Projectile; }
        }      
               
        public static Texture2D T2U0
        {      
            get { return t2U0; }
        }      
               
        public static Texture2D T2U1
        {
            get { return t2U1; }
        }

        public static Texture2D T2U2
        {
            get { return t2U2; }
        }

        public static Texture2D T2U3
        {
            get { return t2U3; }
        }

        public static Texture2D T2Projectile
        {
            get { return t2Projectile; }
        }

        public static Texture2D FrontMenuTex
        {
            get { return frontMenuTex; }
        }
    }
}
