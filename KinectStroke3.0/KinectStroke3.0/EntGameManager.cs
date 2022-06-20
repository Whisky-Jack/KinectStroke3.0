﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MeepEngine
{
    public class EntGameManager
        : Entity
    {
        public EntGameManager(Game game, SpriteBatch spriteBatch)
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

        // Difficulty parameters
        //public float[] sunScale = new float[12] { 1f, 1f, 1f, 0.75f, 0.75f, 0.75f, 0.5f, 0.5f, 0.5f, 0.25f, 0.25f, 0.25f };
        //int[] scoreDecay = new int[12] { 0, 0, 0, 20, 20, 20, 10, 10, 10, 5, 5, 5 };
        //float[] delay = new float[12] { 3f, 2.5f, 2f, 1.5f, 1f, 0.75f, 0.5f, 0.5f, 0.5f, 0.25f, 0.25f, 0.25f };


        // Difficulty parameters
        public float[] sunScale = new float[12] { 1, 1, 1, 0.6f, 0.6f, 0.5f, 0.5f, 0.35f, 0.35f, 0.25f, 0.2f, 0.2f };
        // { 1f, 1f, 1f, 0.85f, 0.775f, 0.7f, 0.625f, 0.55f, 0.475f, 0.4f, 0.325f, 0.25f };
        int[] scoreDecay = new int[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        float[] delay = new float[12] { 3, 2, 1, 1, 1, 1, 0.5f, 0.1f, 0.1f, 0.1f, 0.1f, 0.08f };
        // { 3f, 2.5f, 2f, 1.5f, 0.775f, 0.7f, 0.625f, 0.55f, 0.475f, 0.4f, 0.325f, 0.25f };




        // Sun location
        public int sunLocation;
        int[] sunGradient = new int[4] { 0, -20, -30, -35 };
        public float sunShift = 0f;

        // Sun alphas
        float[] alpha = new float[4] { 0 , 0 , 0 , 0 };
        float[] alphaTarget = new float[4];
        float alphaGain = 0.05f;

        // Asteroid delay
        float delayTimer = 0f;

        // Score decay
        int decayCount = 0;

        // Score
        public static int score;

        // Output
        public StreamWriter dataOutput;
        public StreamWriter summaryOutput;

        // Asteroid counter
        public static int totalAsteroids = 100;
        public int currentAsteroid = 0;
        public int successfulAsteroids = 0;

        // Game start countdown
        bool gameStarted = (Main.activeRoom == Main.rmCalibrate);
        double startCountdown = 5f;

        public override void Update(GameTime gameTime)
        {
            // Starting countdown
            if (!gameStarted)
            {
                startCountdown -= gameTime.ElapsedGameTime.TotalSeconds;
                if (startCountdown <= 0f)
                {
                    gameStarted = true;
                }
                return;
            }

            // Keep asteroids coming, choose new sun location
            if (currentAsteroid < totalAsteroids || Main.activeRoom == Main.rmCalibrate)
            {
                if (!Main.InstanceExists(typeof(EntAsteroid)))
                {
                    if (delayTimer < delay[EntSetup.difficulty])
                    {
                        delayTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                    else
                    {
                        currentAsteroid++;
                        Main.InstanceCreate(new EntAsteroid(game, spriteBatch), Vector2.Zero);

                        sunLocation = Main.rand.Next(4);
                        sunShift = (1 - sunScale[EntSetup.difficulty]) * ((float)Main.rand.NextDouble() - 0.5f);
                        for (int i = 0; i < 4; i++)
                            alpha[i] = 0;

                        delayTimer = 0f;
                    }
                }
            }
            else
            {
                // Finished all asteroids
                if (!Main.InstanceExists(typeof(EntAsteroid)))
                {
                    WriteSummary();
                    FinishWritingData();
                    MediaPlayer.Stop();
                    Main.RoomGoto(Main.rmGameOver);
                }
            }

            // Score decay
            if (scoreDecay[EntSetup.difficulty] != 0)
            {
                if(decayCount != scoreDecay[EntSetup.difficulty])
                {
                    decayCount++;
                }
                else
                {
                    score--;
                    decayCount = 0;
                }
            }

            base.Update(gameTime);
        }

        string path;

        public override void Create()
        {
            score = 0;
            sunLocation = Main.rand.Next(4);

            if (!EntSetup.freePlay && Main.activeRoom != Main.rmCalibrate)
            {
                // Data output
                path = EntSetup.dataDirectory + EntSetup.SpecificSubjectPath();
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                path += EntSetup.SpecificSubjectFile();
                dataOutput = new StreamWriter(path);

                try { dataOutput.WriteLine("Asteroid,Catch Time,Throw Time,Caught,Thrown,Into Sun"); }
                catch { Main.RoomGoto(Main.rmMenu); }

                // Summary output
                path = EntSetup.dataDirectory + EntSetup.SummaryFilePath();
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                path += EntSetup.SummaryFile();

                if (!File.Exists(path))
                {
                    summaryOutput = new StreamWriter(path);
                    summaryOutput.WriteLine("Day,Block,Difficulty,Score,Successful Asteroids,Total Asteroids,Success Rate,Range of Motion,Range/Window");
                }
                else
                {
                    summaryOutput = File.AppendText(path);
                    try { summaryOutput.Write(""); }
                    catch { Main.RoomGoto(Main.rmMenu); }
                }
            }
        }

        public override void Destroy()
        {
            
        }

        public override void Draw(GameTime gameTime)
        {
            if (Main.activeRoom != Main.rmCalibrate && EntSetup.showScore)
                Main.DrawText("Score: " + score.ToString(), new Vector2(32, 32), Color.White, Assets.font);

            if (!gameStarted)
                Main.DrawText(((int)startCountdown).ToString(), new Vector2(Main.roomWidth / 2 - Assets.font.MeasureString(((int)startCountdown).ToString()).X / 2, Main.roomHeight / 2 - Assets.font.MeasureString(((int)startCountdown).ToString()).Y / 2), Color.White, Assets.font);
            
            DrawSun();

            base.Draw(gameTime);
        }

        void DrawSun()
        {
            alphaTarget[0] = 0.1f + 0.3f * (float)Main.rand.NextDouble();
            alphaTarget[1] = 0.3f + 0.25f * (float)Main.rand.NextDouble();
            alphaTarget[2] = 0.45f + 0.35f * (float)Main.rand.NextDouble();
            alphaTarget[3] = 0.8f;

            for (int i = 0; i < 4; i++)
                alpha[i] -= alphaGain * (alpha[i] - alphaTarget[i]);

            if (sunLocation == 0) // North
                for (int i = 0; i < 4; i++)
                    Main.DrawSprite(Assets.sprSun, new Vector2(Main.roomWidth / 2 + Main.roomWidth * sunShift, sunGradient[i] * sunScale[EntSetup.difficulty] + 8 + (1 - sunScale[EntSetup.difficulty]) * 8), (float)Math.PI, 1f * sunScale[EntSetup.difficulty], 0.3f, alpha[i]);
            else if (sunLocation == 1) // South
                for (int i = 0; i < 4; i++)
                    Main.DrawSprite(Assets.sprSun, new Vector2(Main.roomWidth / 2 + Main.roomWidth * sunShift, Main.roomHeight - sunGradient[i] * sunScale[EntSetup.difficulty] - 8 - (1 - sunScale[EntSetup.difficulty]) * 8), 0f, 1f * sunScale[EntSetup.difficulty], 0.3f, alpha[i]);
            else if (sunLocation == 2) // East
                for (int i = 0; i < 4; i++)
                    Main.DrawSprite(Assets.sprSun, new Vector2(Main.roomWidth - sunGradient[i] * sunScale[EntSetup.difficulty] - 12 - (1 - sunScale[EntSetup.difficulty]) * 12, Main.roomHeight / 2 + Main.roomHeight * sunShift), 1.5f * (float)Math.PI, 0.75f * sunScale[EntSetup.difficulty], 0.3f, alpha[i]);
            else if (sunLocation == 3) // West
                for (int i = 0; i < 4; i++)
                    Main.DrawSprite(Assets.sprSun, new Vector2(sunGradient[i] * sunScale[EntSetup.difficulty] + 12 + (1 - sunScale[EntSetup.difficulty]) * 12, Main.roomHeight / 2 + Main.roomHeight * sunShift), 0.5f * (float)Math.PI, 0.75f * sunScale[EntSetup.difficulty], 0.3f, alpha[i]);
        }

        public void WriteData(float timeCatch, float timeThrow, bool successCatch, bool successThrow, bool successSun)
        {
            if (!EntSetup.freePlay && Main.activeRoom != Main.rmCalibrate)
                dataOutput.WriteLine(currentAsteroid.ToString() + "," + timeCatch.ToString() + "," + timeThrow.ToString() + "," + (successCatch ? 1 : 0).ToString() + "," + (successThrow ? 1 : 0).ToString() + "," + (successSun ? 1 : 0).ToString());
        }

        public void FinishWritingData()
        {
            if (!EntSetup.freePlay && Main.activeRoom != Main.rmCalibrate)
                dataOutput.Close();
        }

        public void WriteSummary()
        {
            
            if (!EntSetup.freePlay && Main.activeRoom != Main.rmCalibrate)
            {
                write_coordinates();
                write_asteroid_coordinates();
                float successRate = (float)successfulAsteroids / (float)currentAsteroid;
                float rangePercent = EntScaledKinect.CalculateRangeArea() / ((float)Main.roomWidth * (float)Main.roomHeight);
                summaryOutput.WriteLine(EntSetup.day.ToString() + "," + EntSetup.block.ToString() + "," + EntSetup.diffString[EntSetup.difficulty] + "," + score.ToString() + "," + successfulAsteroids.ToString() + "," + currentAsteroid.ToString() + "," + successRate.ToString() + "," + EntScaledKinect.CalculateRangeArea().ToString() + "," + rangePercent.ToString());
                summaryOutput.Close();
                
            }
        }
        private void write_asteroid_coordinates()
        {
            List<Entity> list_cursor = Main.FindEntity(typeof(EntGameCursor));
            EntGameCursor cursor = (EntGameCursor)list_cursor[0];
            List<float> a_coordinates_x = cursor.get_a_x_list();
            List<float> a_coordinates_y = cursor.get_a_y_list();
            List<float> a_time_record = cursor.get_a_time_list();

            StreamWriter a_coord_file;

            path = EntSetup.dataDirectory + EntSetup.SummaryFilePath();
            path += EntSetup.aCoordFile();

            if (!File.Exists(path))
            {
                a_coord_file = new StreamWriter(path);
                try
                {
                    a_coord_file.WriteLine("Asteroids");
                    a_coord_file.WriteLine("x,y,t");

                }
                catch { Main.RoomGoto(Main.rmMenu); }
            }
            else
            {
                a_coord_file = File.AppendText(path);
                try
                {
                    a_coord_file.WriteLine("x,y,t");

                }
                catch { Main.RoomGoto(Main.rmMenu); }
            }
            int len_x = a_coordinates_x.Count;
            int len_y = a_coordinates_y.Count;
            int t_rec = a_time_record.Count;
            System.Diagnostics.Debug.WriteLine("X len: " + len_x.ToString() + "Y len: " + len_y.ToString() + "t len: " + t_rec.ToString());
            int indx = 0;
            while (len_x != 0 && len_y != 0 && t_rec != 0)
            {
                string line = a_coordinates_x[indx].ToString("R") + "," + a_coordinates_y[indx].ToString("R") + "," + a_time_record[indx].ToString("R");
                a_coord_file.WriteLine(line);

                ++indx;
                --len_x;
                --len_y;
                --t_rec;
            }
            a_coord_file.Close();




        }
        private void write_coordinates() {
            List<Entity> list_cursor = Main.FindEntity(typeof(EntGameCursor));
            EntGameCursor cursor = (EntGameCursor)list_cursor[0];
            List<float> user_coordinates_x = cursor.get_x_list();
            List<float> user_coordinates_y = cursor.get_y_list();
            List<float> time_record = cursor.get_time_list();

            StreamWriter coord_file;

            path = EntSetup.dataDirectory + EntSetup.SummaryFilePath();
            path += EntSetup.coordFile();
            
            if (!File.Exists(path))
            {
                coord_file = new StreamWriter(path);
                try
                {
                    coord_file.WriteLine("x,y,t");

                }
                catch { Main.RoomGoto(Main.rmMenu); }
            }
            else
            {
                coord_file = File.AppendText(path);
                try
                {
                    coord_file.WriteLine("x,y,t");
                    
                }
                catch { Main.RoomGoto(Main.rmMenu); }
            }
            int len_x = user_coordinates_x.Count;
            int len_y = user_coordinates_y.Count;
            int t_rec = time_record.Count;
            int indx = 0;
            while (len_x != 0 && len_y != 0 && t_rec != 0)
            {
                string line = user_coordinates_x[indx].ToString("R") + "," + user_coordinates_y[indx].ToString("R") + "," + time_record[indx].ToString("R");
                coord_file.WriteLine(line);
                
                ++indx;
                --len_x;
                --len_y;
                --t_rec;
            }
            coord_file.Close();
            
        }

    }
}