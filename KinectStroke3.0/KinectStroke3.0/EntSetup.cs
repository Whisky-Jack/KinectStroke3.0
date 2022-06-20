﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using System.Text;
using System.IO;

namespace MeepEngine
{
    public class EntSetup
        : Entity
    {
        public EntSetup(Game game, SpriteBatch spriteBatch)
            : base(game, spriteBatch)
        {
            Enabled = true;
            Visible = true;
            
            // Set sprite parameters
            sprite = Assets.nosprite;
            imageAngle = 0f;
            imageScale = 1f;
            layer = 0f;
        }

        // Form parameters
        public static string subject = "Practice";
        public static int day = 1;
        public static int block = 1;
        public static int handType = 0;
        public static int difficulty = 0;
        public static bool showScore = true;
        public static string dataDirectory = @"J:\\Experiments_Funded\\WMI_Learning_2014\\Stroke_Data\\Behavioural Data";
        public static bool freePlay = false;

        // Difficulty strings
        public static string[] diffString = new string[12] { "P1", "P2", "P3", "L1", "L2", "L3", "L4", "L5", "L6", "L7", "L8", "L9" };

        // Form
        public static FrmSetup form;
        public static bool formIsOpen = false;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Create()
        {

        }

        public override void Destroy()
        {

        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public static void ShowForm()
        {
            if (!formIsOpen)
            {
                form = new FrmSetup();
                form.Activate();
                form.Show();

                formIsOpen = true;
            }
        }

        public static void UpdateKinect()
        {
            if (handType == 0)
                EntKinect.handType = Microsoft.Kinect.JointType.HandRight;
            else
                EntKinect.handType = Microsoft.Kinect.JointType.HandLeft;
        }

        public static string GetDirectoryAboveProject(int levels)
        {
            string currentDirPath = Directory.GetCurrentDirectory();
            string projectDirPath = GetDirectoryAbove(currentDirPath, levels + 5);
            return projectDirPath;
        }

        public static string CreateDataDirectory(string path)
        {
            string dataDirPath = path + "\\Data";
            if (!Directory.Exists(dataDirPath))
            {
                Directory.CreateDirectory(dataDirPath);
                Console.WriteLine("Data directory was not found, therefore was created at: " + dataDirPath);
            }
            return dataDirPath;
        }

        public static string GetDirectoryAbove(string path, int levels)
        {
            if (levels < 1)
                return path;
            else
                return GetDirectoryAbove(Directory.GetParent(path).FullName, levels - 1);
        }

        public static string SpecificSubjectPath()
        {
            return @"\" + subject;
            //return @"\" + subject + @"\" + "Day " + day.ToString() + @"\" + "Block " + block.ToString();
        }

        public static string SummaryFilePath()
        {
            return @"\" + subject;
        }

        public static string SpecificSubjectFile()
        {
            return @"\" + subject + "_" + "D" + day.ToString() + "_" + "B" + block.ToString() + "_" + diffString[difficulty] + ".csv";
        }

        public static string SummaryFile()
        {
            return @"\" + subject + "_" + "Summary" + ".csv";
        }
        public static string coordFile()
        {
            return @"\" + subject + "_" + "D" + day.ToString() + "_" + "B" + block.ToString() + "_" + diffString[difficulty] + "_"+ "Coordinates" + ".csv";
        }

        public static string aCoordFile()
        {
            return @"\" + subject + "_" + "D" + day.ToString() + "_" + "B" + block.ToString() + "_" + diffString[difficulty] + "_" + "Asteroid_Coordinates" + ".csv";
        }
    }
}